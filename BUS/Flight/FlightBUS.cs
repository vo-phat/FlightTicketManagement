using DTO.Flight;
using DAO.Flight;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUS.Flight
{
    public class FlightBUS
    {
        #region Singleton Pattern
        private static FlightBUS _instance;
        private static readonly object _lock = new object();
        private readonly FlightDAO _flightDAO;

        private FlightBUS()
        {
            _flightDAO = FlightDAO.Instance;
        }

        public static FlightBUS Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FlightBUS();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region CRUD Operations
        public List<FlightDTO> GetAllFlights()
        {
            try
            {
                return _flightDAO.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lấy danh sách chuyến bay: {ex.Message}", ex);
            }
        }

        public List<FlightWithDetailsDTO> GetAllFlightsWithDetails()
        {
            try
            {
                return _flightDAO.GetAllWithDetails();
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lấy danh sách chuyến bay chi tiết: {ex.Message}", ex);
            }
        }

        public FlightDTO GetFlightById(int flightId)
        {
            try
            {
                if (flightId <= 0)
                    throw new ArgumentException("Flight ID không hợp lệ");

                return _flightDAO.GetById(flightId);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lấy thông tin chuyến bay: {ex.Message}", ex);
            }
        }

        public bool CreateFlight(FlightDTO flight, out string message)
        {
            return CreateFlight(flight, out message, out _);
        }

        public bool CreateFlight(FlightDTO flight, out string message, out List<string> warnings)
        {
            message = string.Empty;
            warnings = new List<string>();

            try
            {
                // 1. Basic validation (DTO level)
                if (!flight.IsValid(out string validationError))
                {
                    message = validationError;
                    return false;
                }

                // 2. Business validation using FlightValidationHelper
                // Note: Airport validation is handled by Route entity
                if (!flight.DepartureTime.HasValue || !flight.ArrivalTime.HasValue)
                {
                    message = "Thời gian khởi hành và đến không được để trống";
                    return false;
                }

                // Validate flight code format
                if (!FlightValidationHelper.IsValidFlightCode(flight.FlightNumber, out string codeError))
                {
                    message = codeError;
                    return false;
                }

                // Validate departure time
                if (!FlightValidationHelper.IsValidDepartureTime(flight.DepartureTime.Value, out string depError))
                {
                    message = depError;
                    return false;
                }

                // Validate arrival time
                if (!FlightValidationHelper.IsValidArrivalTime(flight.DepartureTime.Value, flight.ArrivalTime.Value, out string arrError))
                {
                    message = arrError;
                    return false;
                }

                // Validate base price
                if (!FlightValidationHelper.IsValidBasePrice(flight.BasePrice, out string priceError, out string? priceWarning))
                {
                    message = priceError;
                    return false;
                }
                
                if (!string.IsNullOrEmpty(priceWarning))
                {
                    warnings.Add(priceWarning);
                }



                // 3. Check duplicate flight number
                if (_flightDAO.IsFlightNumberExists(flight.FlightNumber))
                {
                    message = $"Mã chuyến bay '{flight.FlightNumber}' đã tồn tại trong hệ thống";
                    return false;
                }

                // 4. Check aircraft time conflict
                if (!flight.DepartureTime.HasValue || !flight.ArrivalTime.HasValue)
                {
                    message = "Thời gian khởi hành và đến không được để trống";
                    return false;
                }

                if (_flightDAO.HasAircraftTimeConflict(flight.AircraftId, flight.DepartureTime.Value, flight.ArrivalTime.Value))
                {
                    var conflictFlights = _flightDAO.GetConflictingFlights(
                        flight.AircraftId, 
                        flight.DepartureTime.Value, 
                        flight.ArrivalTime.Value
                    );

                    if (conflictFlights.Any())
                    {
                        var conflictInfo = string.Join(", ", conflictFlights.Select(f => 
                            $"{f.FlightNumber} ({f.DepartureTime:HH:mm}-{f.ArrivalTime:HH:mm})"
                        ));
                        message = $"Máy bay đã được sử dụng cho các chuyến bay khác: {conflictInfo}";
                        return false;
                    }
                }

                // 5. Insert to database
                long newId = _flightDAO.Insert(flight);
                if (newId > 0)
                {
                    // ✅ Auto-generate seats for new flight
                    try
                    {
                        GenerateSeatsForNewFlight((int)newId, flight.AircraftId, flight.BasePrice);
                    }
                    catch (Exception seatEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"[CreateFlight] Warning: Could not auto-generate seats: {seatEx.Message}");
                        warnings.Add($"Chuyến bay đã tạo nhưng không thể tự động tạo ghế: {seatEx.Message}");
                    }

                    message = "Tạo chuyến bay thành công!";
                    return true;
                }

                message = "Không thể tạo chuyến bay";
                return false;
            }
            catch (Exception ex)
            {
                message = $"BUS: Lỗi khi tạo chuyến bay: {ex.Message}";
                return false;
            }
        }

        public bool UpdateFlight(FlightDTO flight, out string message)
        {
            return UpdateFlight(flight, out message, out _);
        }

        public bool UpdateFlight(FlightDTO flight, out string message, out List<string> warnings)
        {
            message = string.Empty;
            warnings = new List<string>();

            try
            {
                // 1. Basic validation
                if (!flight.IsValid(out string validationError))
                {
                    message = validationError;
                    return false;
                }

                // 2. Business validation
                if (!flight.DepartureTime.HasValue || !flight.ArrivalTime.HasValue)
                {
                    message = "Thời gian khởi hành và đến không được để trống";
                    return false;
                }

                // Validate flight code
                if (!FlightValidationHelper.IsValidFlightCode(flight.FlightNumber, out string codeError))
                {
                    message = codeError;
                    return false;
                }

                // Validate times
                if (!FlightValidationHelper.IsValidDepartureTime(flight.DepartureTime.Value, out string depError))
                {
                    message = depError;
                    return false;
                }

                if (!FlightValidationHelper.IsValidArrivalTime(flight.DepartureTime.Value, flight.ArrivalTime.Value, out string arrError))
                {
                    message = arrError;
                    return false;
                }

                // Validate price
                if (!FlightValidationHelper.IsValidBasePrice(flight.BasePrice, out string priceError, out string? priceWarning))
                {
                    message = priceError;
                    return false;
                }
                
                if (!string.IsNullOrEmpty(priceWarning))
                {
                    warnings.Add(priceWarning);
                }

                // 3. Check duplicate (exclude current flight)
                if (_flightDAO.IsFlightNumberExists(flight.FlightNumber, flight.FlightId))
                {
                    message = $"Mã chuyến bay '{flight.FlightNumber}' đã tồn tại trong hệ thống";
                    return false;
                }

                // 4. Check aircraft conflict (exclude current flight)
                if (!flight.DepartureTime.HasValue || !flight.ArrivalTime.HasValue)
                {
                    message = "Thời gian khởi hành và đến không được để trống";
                    return false;
                }

                if (_flightDAO.HasAircraftTimeConflict(
                    flight.AircraftId, 
                    flight.DepartureTime.Value, 
                    flight.ArrivalTime.Value, 
                    flight.FlightId))
                {
                    var conflictFlights = _flightDAO.GetConflictingFlights(
                        flight.AircraftId,
                        flight.DepartureTime.Value,
                        flight.ArrivalTime.Value,
                        flight.FlightId
                    );

                    if (conflictFlights.Any())
                    {
                        var conflictInfo = string.Join(", ", conflictFlights.Select(f =>
                            $"{f.FlightNumber} ({f.DepartureTime:HH:mm}-{f.ArrivalTime:HH:mm})"
                        ));
                        message = $"Máy bay đã được sử dụng cho các chuyến bay khác: {conflictInfo}";
                        return false;
                    }
                }

                // 5. Update database
                bool result = _flightDAO.Update(flight);
                if (result)
                {
                    message = "Cập nhật chuyến bay thành công!";
                    return true;
                }

                message = "Không thể cập nhật chuyến bay";
                return false;
            }
            catch (Exception ex)
            {
                message = $"BUS: Lỗi khi cập nhật chuyến bay: {ex.Message}";
                return false;
            }
        }

        public bool DeleteFlight(int flightId, out string message)
        {
            message = string.Empty;
            try
            {
                if (flightId <= 0)
                {
                    message = "Flight ID không hợp lệ";
                    return false;
                }

                // Soft delete
                bool result = _flightDAO.SoftDelete(flightId);
                if (result)
                {
                    message = "Xóa chuyến bay thành công!";
                    return true;
                }

                message = "Không thể xóa chuyến bay";
                return false;
            }
            catch (Exception ex)
            {
                // Pass through the original error message from DAO
                message = ex.Message;
                return false;
            }
        }
        #endregion

        #region Search & Filter
        public List<FlightDTO> SearchFlights(string flightNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(flightNumber))
                    return GetAllFlights();

                return _flightDAO.SearchByFlightNumber(flightNumber);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi tìm kiếm chuyến bay: {ex.Message}", ex);
            }
        }

        public List<FlightDTO> GetFlightsByStatus(FlightStatus status)
        {
            try
            {
                return _flightDAO.GetByStatus(status);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lọc chuyến bay theo trạng thái: {ex.Message}", ex);
            }
        }

        public List<FlightDTO> GetFlightsByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                if (fromDate > toDate)
                    throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");

                return _flightDAO.GetByDateRange(fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lọc chuyến bay theo ngày: {ex.Message}", ex);
            }
        }

        public List<FlightDTO> GetFlightsByAircraft(int aircraftId)
        {
            try
            {
                if (aircraftId <= 0)
                    throw new ArgumentException("Aircraft ID không hợp lệ");

                return _flightDAO.GetByAircraftId(aircraftId);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lọc chuyến bay theo máy bay: {ex.Message}", ex);
            }
        }

        public List<FlightDTO> GetFlightsByRoute(int routeId)
        {
            try
            {
                if (routeId <= 0)
                    throw new ArgumentException("Route ID không hợp lệ");

                return _flightDAO.GetByRouteId(routeId);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lọc chuyến bay theo tuyến bay: {ex.Message}", ex);
            }
        }

        public List<FlightWithDetailsDTO> GlobalSearchFlights(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return new List<FlightWithDetailsDTO>();

                return _flightDAO.GlobalSearch(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi tìm kiếm toàn cục: {ex.Message}", ex);
            }
        }
        #endregion

        #region Status Management
        public bool UpdateFlightStatus(int flightId, FlightStatus newStatus, out string message)
        {
            message = string.Empty;
            try
            {
                bool result = _flightDAO.UpdateStatus(flightId, newStatus);
                if (result)
                {
                    message = $"Cập nhật trạng thái thành {newStatus.GetDescription()} thành công!";
                    return true;
                }

                message = "Không thể cập nhật trạng thái";
                return false;
            }
            catch (Exception ex)
            {
                message = $"BUS: {ex.Message}";
                return false;
            }
        }
        #endregion

        #region Statistics
        public int GetTotalFlightCount()
        {
            try
            {
                return _flightDAO.CountAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi đếm tổng số chuyến bay: {ex.Message}", ex);
            }
        }

        public Dictionary<FlightStatus, int> GetFlightCountByStatus()
        {
            try
            {
                var result = new Dictionary<FlightStatus, int>();
                foreach (FlightStatus status in Enum.GetValues(typeof(FlightStatus)))
                {
                    result[status] = _flightDAO.CountByStatus(status);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi thống kê chuyến bay theo trạng thái: {ex.Message}", ex);
            }
        }

        public Dictionary<string, int> GetFlightStatsByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var flights = GetFlightsByDateRange(fromDate, toDate);
                return new Dictionary<string, int>
                {
                    { "Tổng số chuyến bay", flights.Count },
                    { "Đã lên lịch", flights.Count(f => f.Status == FlightStatus.SCHEDULED) },
                    { "Bị hoãn", flights.Count(f => f.Status == FlightStatus.DELAYED) },
                    { "Đã hủy", flights.Count(f => f.Status == FlightStatus.CANCELLED) },
                    { "Hoàn thành", flights.Count(f => f.Status == FlightStatus.COMPLETED) }
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi thống kê chuyến bay: {ex.Message}", ex);
            }
        }

        public List<FlightDTO> GetUpcomingFlights(int hoursAhead = 24)
        {
            try
            {
                return _flightDAO.GetUpcomingFlights(hoursAhead);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lấy chuyến bay sắp khởi hành: {ex.Message}", ex);
            }
        }
        #endregion

        #region Advanced Validation
        /// <summary>
        /// Kiểm tra máy bay có sẵn sàng cho chuyến bay không (không bị xung đột thời gian)
        /// </summary>
        public bool ValidateAircraftAvailability(int aircraftId, DateTime departureTime, DateTime arrivalTime, int excludeFlightId, out string message)
        {
            message = string.Empty;
            try
            {
                bool isAvailable = _flightDAO.CheckAircraftAvailability(aircraftId, departureTime, arrivalTime, excludeFlightId);
                
                if (!isAvailable)
                {
                    message = "Máy bay đã được lên lịch cho chuyến bay khác trong khoảng thời gian này.";
                    return false;
                }

                message = "Máy bay khả dụng";
                return true;
            }
            catch (Exception ex)
            {
                message = $"BUS: Lỗi khi kiểm tra khả dụng máy bay: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Validate toàn bộ business logic trước khi tạo/cập nhật chuyến bay
        /// </summary>
        public bool ValidateFlightBusinessRules(FlightDTO flight, out string message)
        {
            message = string.Empty;

            // 1. Validate basic data
            if (!flight.IsValid(out string validationError))
            {
                message = validationError;
                return false;
            }

            // 2. Check duplicate flight number on same date
            if (_flightDAO.IsFlightNumberExists(flight.FlightNumber, flight.DepartureTime.Value, flight.FlightId))
            {
                message = $"Số hiệu chuyến bay '{flight.FlightNumber}' đã tồn tại vào ngày {flight.DepartureTime.Value:dd/MM/yyyy}.";
                return false;
            }

            // 3. Check aircraft availability
            if (!_flightDAO.CheckAircraftAvailability(flight.AircraftId, flight.DepartureTime.Value, flight.ArrivalTime.Value, flight.FlightId))
            {
                message = "Máy bay đã được lên lịch cho chuyến bay khác trong khoảng thời gian này.";
                return false;
            }

            // 4. Check reasonable flight duration (not too short or too long)
            var duration = flight.GetFlightDuration();
            if (duration.HasValue)
            {
                if (duration.Value.TotalMinutes < 30)
                {
                    message = "Thời gian bay quá ngắn (tối thiểu 30 phút).";
                    return false;
                }
                
                if (duration.Value.TotalHours > 24)
                {
                    message = "Thời gian bay quá dài (tối đa 24 giờ).";
                    return false;
                }
            }

            // 5. Check departure time is in the future (for new flights)
            if (flight.FlightId == 0 && flight.DepartureTime.Value < DateTime.Now.AddHours(-1))
            {
                message = "Không thể tạo chuyến bay với thời gian khởi hành trong quá khứ.";
                return false;
            }

            message = "Validation thành công";
            return true;
        }

        /// <summary>
        /// Tự động cập nhật trạng thái chuyến bay dựa trên thời gian hiện tại
        /// </summary>
        public int AutoUpdateFlightStatuses()
        {
            try
            {
                int updatedCount = 0;
                var now = DateTime.Now;

                // Get all scheduled or delayed flights
                var scheduledFlights = _flightDAO.GetByStatus(FlightStatus.SCHEDULED);
                var delayedFlights = _flightDAO.GetByStatus(FlightStatus.DELAYED);
                var allActiveFlights = scheduledFlights.Concat(delayedFlights).ToList();

                foreach (var flight in allActiveFlights)
                {
                    if (!flight.ArrivalTime.HasValue || !flight.DepartureTime.HasValue)
                        continue;

                    // If arrival time has passed, mark as completed
                    if (flight.ArrivalTime.Value < now)
                    {
                        if (_flightDAO.UpdateStatus(flight.FlightId, FlightStatus.COMPLETED))
                        {
                            updatedCount++;
                        }
                    }
                }

                return updatedCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi tự động cập nhật trạng thái chuyến bay: {ex.Message}", ex);
            }
        }
        #endregion

        #region Reports & Analytics
        /// <summary>
        /// Lấy báo cáo doanh thu theo chuyến bay
        /// </summary>
        public System.Data.DataTable GetFlightRevenueReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return _flightDAO.GetFlightRevenueStatistics(fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lấy báo cáo doanh thu chuyến bay: {ex.Message}", ex);
            }
        }
        #endregion

        #region Seat Generation
        /// <summary>
        /// Auto-generate Seats and FlightSeats for a new flight
        /// </summary>
        private void GenerateSeatsForNewFlight(int flightId, int aircraftId, decimal basePrice)
        {
            try
            {
                // Import necessary namespaces at method level
                var seatBUS = new BUS.Seat.SeatBUS();
                var aircraftBUS = new BUS.Aircraft.AircraftBUS();
                var cabinClassBUS = new BUS.CabinClass.CabinClassBUS();

                // 1. Get aircraft capacity
                var aircraft = aircraftBUS.GetAllAircrafts().FirstOrDefault(a => a.AircraftId == aircraftId);
                if (aircraft == null || !aircraft.Capacity.HasValue)
                {
                    throw new Exception($"Không tìm thấy máy bay ID {aircraftId} hoặc không có thông tin capacity");
                }

                int capacity = aircraft.Capacity.Value;

                // 2. Check if aircraft already has seats
                var existingSeats = seatBUS.GetAllSeats().Where(s => s.AircraftId == aircraftId).ToList();
                
                if (existingSeats.Count == 0)
                {
                    // 3. Create Seats for aircraft (similar to GenerateSeatsForNewAircraft)
                    var allClasses = cabinClassBUS.GetAllCabinClasses();
                    var businessClass = allClasses.FirstOrDefault(c => c.ClassName == "Business");
                    var economyClass = allClasses.FirstOrDefault(c => c.ClassName == "Economy");

                    if (economyClass == null)
                    {
                        throw new Exception("Không tìm thấy hạng ghế Economy");
                    }

                    int businessRows = 0, economyRows = 0;
                    int businessCols = 4; // A B C D
                    int economyCols = 6; // A B C D E F

                    // Determine seat configuration based on capacity
                    if (capacity < 100)
                    {
                        // Small aircraft: Economy only
                        economyRows = (int)Math.Ceiling(capacity / (double)economyCols);
                    }
                    else
                    {
                        // Medium/Large: 15% Business, 85% Economy
                        int businessSeats = (int)(capacity * 0.15);
                        businessRows = (int)Math.Ceiling(businessSeats / (double)businessCols);
                        int remainingSeats = capacity - (businessRows * businessCols);
                        economyRows = (int)Math.Ceiling(remainingSeats / (double)economyCols);
                    }

                    int currentRow = 1;
                    var createdSeatIds = new List<int>();

                    // Generate Business class seats
                    if (businessRows > 0 && businessClass != null)
                    {
                        for (int row = 0; row < businessRows; row++)
                        {
                            for (char col = 'A'; col <= 'D'; col++)
                            {
                                string seatNumber = $"{currentRow}{col}";
                                var seat = new DTO.Seat.SeatDTO(0, aircraftId, seatNumber, businessClass.ClassId);
                                seatBUS.AddSeat(seat, out _); // ✅ AddSeat returns bool
                            }
                            currentRow++;
                        }
                    }

                    // Generate Economy class seats
                    for (int row = 0; row < economyRows; row++)
                    {
                        for (char col = 'A'; col <= 'F'; col++)
                        {
                            string seatNumber = $"{currentRow}{col}";
                            var seat = new DTO.Seat.SeatDTO(0, aircraftId, seatNumber, economyClass.ClassId);
                            seatBUS.AddSeat(seat, out _); // ✅ AddSeat returns bool
                        }
                        currentRow++;
                    }

                    System.Diagnostics.Debug.WriteLine($"[GenerateSeatsForNewFlight] Created Seats for aircraft {aircraftId}");
                    
                    // Refresh seat list
                    existingSeats = seatBUS.GetAllSeats().Where(s => s.AircraftId == aircraftId).ToList();
                }

                // 4. Create FlightSeats from Aircraft Seats
                using (var conn = DAO.Database.DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    foreach (var seat in existingSeats)
                    {
                        string insertQuery = @"
                            INSERT INTO flight_seats (flight_id, seat_id, base_price, seat_status)
                            VALUES (@flightId, @seatId, @basePrice, 'AVAILABLE')";

                        using (var cmd = new MySqlConnector.MySqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@flightId", flightId);
                            cmd.Parameters.AddWithValue("@seatId", seat.SeatId);
                            cmd.Parameters.AddWithValue("@basePrice", basePrice);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"[GenerateSeatsForNewFlight] Created {existingSeats.Count} FlightSeats for flight {flightId}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tự động tạo ghế cho chuyến bay: {ex.Message}", ex);
            }
        }
        #endregion
    }
}
