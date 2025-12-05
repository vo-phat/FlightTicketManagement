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

        /// <summary>
        /// Lấy báo cáo tháng với doanh thu
        /// </summary>
        public System.Data.DataTable GetMonthlyRevenueReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return _flightDAO.GetMonthlyRevenueReport(fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lấy báo cáo doanh thu tháng: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy top tuyến bay phổ biến nhất
        /// </summary>
        public System.Data.DataTable GetTopRoutes(int topN = 10)
        {
            try
            {
                return _flightDAO.GetTopRoutesByFlightCount(topN);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lấy top tuyến bay: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy top máy bay được sử dụng nhiều nhất
        /// </summary>
        public System.Data.DataTable GetTopAircrafts(int topN = 10)
        {
            try
            {
                return _flightDAO.GetTopAircraftsByFlightCount(topN);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lấy top máy bay: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tỷ lệ hoàn thành chuyến bay theo tháng
        /// </summary>
        public System.Data.DataTable GetFlightCompletionRate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return _flightDAO.GetFlightCompletionRateByMonth(fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lấy tỷ lệ hoàn thành: {ex.Message}", ex);
            }
        }
        #endregion

        #region Advanced Search - Tìm kiếm nâng cao
        /// <summary>
        /// Tìm kiếm chuyến bay nâng cao với nhiều tiêu chí
        /// </summary>
        public List<FlightWithDetailsDTO> SearchFlightsAdvanced(FlightSearchCriteriaDTO criteria, out string message)
        {
            message = string.Empty;
            try
            {
                if (criteria == null)
                {
                    message = "Tiêu chí tìm kiếm không được rỗng.";
                    return new List<FlightWithDetailsDTO>();
                }

                if (!criteria.IsValid(out string validationError))
                {
                    message = validationError;
                    return new List<FlightWithDetailsDTO>();
                }

                var results = _flightDAO.SearchFlightsAdvanced(criteria);
                
                if (results.Count == 0)
                {
                    message = "Không tìm thấy chuyến bay phù hợp với tiêu chí tìm kiếm.";
                }
                else
                {
                    message = $"Tìm thấy {results.Count} chuyến bay.";
                }

                return results;
            }
            catch (Exception ex)
            {
                message = $"BUS: Lỗi khi tìm kiếm chuyến bay: {ex.Message}";
                return new List<FlightWithDetailsDTO>();
            }
        }

        /// <summary>
        /// Đếm tổng số kết quả tìm kiếm (để phân trang)
        /// </summary>
        public int CountSearchResults(FlightSearchCriteriaDTO criteria)
        {
            try
            {
                if (criteria == null)
                    return 0;

                return _flightDAO.CountSearchResults(criteria);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi đếm kết quả tìm kiếm: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm nhanh chuyến bay (cho form đặt vé)
        /// Tìm chuyến bay từ A -> B vào ngày cụ thể
        /// </summary>
        public List<FlightWithDetailsDTO> QuickSearchFlights(
            int departureAirportId,
            int arrivalAirportId,
            DateTime departureDate,
            out string message,
            int? classId = null,
            int? minSeats = 1)
        {
            message = string.Empty;
            try
            {
                // Validate input
                if (departureAirportId <= 0)
                {
                    message = "Vui lòng chọn sân bay đi.";
                    return new List<FlightWithDetailsDTO>();
                }

                if (arrivalAirportId <= 0)
                {
                    message = "Vui lòng chọn sân bay đến.";
                    return new List<FlightWithDetailsDTO>();
                }

                if (departureAirportId == arrivalAirportId)
                {
                    message = "Sân bay đi và sân bay đến không được giống nhau.";
                    return new List<FlightWithDetailsDTO>();
                }

                if (departureDate.Date < DateTime.Today)
                {
                    message = "Ngày khởi hành không được trong quá khứ.";
                    return new List<FlightWithDetailsDTO>();
                }

                var results = _flightDAO.QuickSearchFlights(
                    departureAirportId,
                    arrivalAirportId,
                    departureDate,
                    classId,
                    minSeats
                );

                if (results.Count == 0)
                {
                    message = $"Không tìm thấy chuyến bay phù hợp vào ngày {departureDate:dd/MM/yyyy}.";
                }
                else
                {
                    message = $"Tìm thấy {results.Count} chuyến bay.";
                }

                return results;
            }
            catch (Exception ex)
            {
                message = $"BUS: Lỗi khi tìm kiếm chuyến bay: {ex.Message}";
                return new List<FlightWithDetailsDTO>();
            }
        }

        /// <summary>
        /// Tìm chuyến bay khứ hồi
        /// </summary>
        public (List<FlightWithDetailsDTO> OutboundFlights, List<FlightWithDetailsDTO> ReturnFlights) 
            SearchRoundTripFlights(
                int departureAirportId,
                int arrivalAirportId,
                DateTime outboundDate,
                DateTime returnDate,
                out string message,
                int? classId = null,
                int? minSeats = 1)
        {
            message = string.Empty;
            try
            {
                // Validate
                if (outboundDate.Date >= returnDate.Date)
                {
                    message = "Ngày về phải sau ngày đi.";
                    return (new List<FlightWithDetailsDTO>(), new List<FlightWithDetailsDTO>());
                }

                // Tìm chuyến đi
                var outboundFlights = QuickSearchFlights(
                    departureAirportId,
                    arrivalAirportId,
                    outboundDate,
                    out string outMsg,
                    classId,
                    minSeats
                );

                // Tìm chuyến về (đảo chiều)
                var returnFlights = QuickSearchFlights(
                    arrivalAirportId,      // Đảo chiều
                    departureAirportId,    // Đảo chiều
                    returnDate,
                    out string retMsg,
                    classId,
                    minSeats
                );

                if (outboundFlights.Count == 0 && returnFlights.Count == 0)
                {
                    message = "Không tìm thấy chuyến bay phù hợp cho cả chuyến đi và về.";
                }
                else if (outboundFlights.Count == 0)
                {
                    message = $"Không tìm thấy chuyến đi. Tìm thấy {returnFlights.Count} chuyến về.";
                }
                else if (returnFlights.Count == 0)
                {
                    message = $"Tìm thấy {outboundFlights.Count} chuyến đi. Không tìm thấy chuyến về.";
                }
                else
                {
                    message = $"Tìm thấy {outboundFlights.Count} chuyến đi và {returnFlights.Count} chuyến về.";
                }

                return (outboundFlights, returnFlights);
            }
            catch (Exception ex)
            {
                message = $"BUS: Lỗi khi tìm chuyến bay khứ hồi: {ex.Message}";
                return (new List<FlightWithDetailsDTO>(), new List<FlightWithDetailsDTO>());
            }
        }

        /// <summary>
        /// Lấy danh sách chuyến bay khả dụng (còn ghế trống, chưa khởi hành)
        /// </summary>
        public List<FlightWithDetailsDTO> GetAvailableFlights(int? minSeats = 1)
        {
            try
            {
                var criteria = new FlightSearchCriteriaDTO
                {
                    DepartureDateFrom = DateTime.Now,
                    Status = FlightStatus.SCHEDULED,
                    MinAvailableSeats = minSeats,
                    SortBy = "DepartureTime",
                    SortOrder = "ASC"
                };

                return _flightDAO.SearchFlightsAdvanced(criteria);
            }
            catch (Exception ex)
            {
                throw new Exception($"BUS: Lỗi khi lấy danh sách chuyến bay khả dụng: {ex.Message}", ex);
            }
        }
        #endregion
    }
}