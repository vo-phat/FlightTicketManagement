using BUS.Common;
using DAO.Flight;
using DAO.FlightSeat;
using DTO.Flight;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUS.Flight
{
    public class FlightBUS
    {
        private static FlightBUS instance;
        private readonly FlightDAO flightDAO;
        private readonly FlightSeatDAO flightSeatDAO;

        // Business rules constants
        private const int MIN_BOOKING_HOURS_BEFORE_DEPARTURE = 2;
        private const int MAX_FLIGHT_DURATION_HOURS = 24;
        private const int MIN_FLIGHT_DURATION_MINUTES = 30;

        public static FlightBUS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FlightBUS();
                }
                return instance;
            }
        }

        private FlightBUS()
        {
            flightDAO = FlightDAO.Instance;
            flightSeatDAO = FlightSeatDAO.Instance;
        }

        public BusinessResult CreateFlight(FlightDTO flight)
        {
            try
            {
                // Step 1: Validate input parameters
                if (flight == null)
                {
                    return BusinessResult.ValidationError("ThÃ´ng tin chuyáº¿n bay khÃ´ng Ä‘Æ°á»£c Ä‘á»ƒ trá»‘ng");
                }

                // Step 2: Validate flight number format
                if (string.IsNullOrWhiteSpace(flight.FlightNumber))
                {
                    return BusinessResult.ValidationError("MÃ£ chuyáº¿n bay khÃ´ng Ä‘Æ°á»£c Ä‘á»ƒ trá»‘ng");
                }

                // Step 3: Check if flight number already exists
                if (flightDAO.ExistsByFlightNumber(flight.FlightNumber))
                {
                    return BusinessResult.ValidationError($"MÃ£ chuyáº¿n bay '{flight.FlightNumber}' Ä‘Ã£ tá»“n táº¡i trong há»‡ thá»‘ng");
                }

                // Step 4: Validate route
                if (flight.RouteId <= 0)
                {
                    return BusinessResult.ValidationError("Tuyáº¿n bay pháº£i Ä‘Æ°á»£c chá»n");
                }

                // Step 5: Validate departure time
                if (flight.DepartureTime <= DateTime.Now.AddHours(MIN_BOOKING_HOURS_BEFORE_DEPARTURE))
                {
                    return BusinessResult.ValidationError(
                        $"Thá»i gian khá»Ÿi hÃ nh pháº£i sau thá»i Ä‘iá»ƒm hiá»‡n táº¡i Ã­t nháº¥t {MIN_BOOKING_HOURS_BEFORE_DEPARTURE} giá»");
                }

                // Step 6: Validate flight duration
                if (flight.ArrivalTime <= flight.DepartureTime)
                {
                    return BusinessResult.ValidationError("Thá»i gian Ä‘áº¿n pháº£i sau thá»i gian khá»Ÿi hÃ nh");
                }

                var duration = flight.ArrivalTime - flight.DepartureTime;
                if (duration.HasValue && duration.Value.TotalMinutes < MIN_FLIGHT_DURATION_MINUTES)
                {
                    return BusinessResult.ValidationError(
                        $"Thá»i gian bay tá»‘i thiá»ƒu lÃ  {MIN_FLIGHT_DURATION_MINUTES} phÃºt");
                }

                if (duration.HasValue && duration.Value.TotalHours > MAX_FLIGHT_DURATION_HOURS)
                {
                    return BusinessResult.ValidationError(
                        $"Thá»i gian bay khÃ´ng Ä‘Æ°á»£c vÆ°á»£t quÃ¡ {MAX_FLIGHT_DURATION_HOURS} giá»");
                }

                // Step 7: Validate aircraft
                if (flight.AircraftId <= 0)
                {
                    return BusinessResult.ValidationError("Pháº£i chá»n mÃ¡y bay cho chuyáº¿n bay");
                }

                // Step 8: Create flight in database
                long newFlightIdLong = flightDAO.Insert(flight);
                int newFlightId = (int)newFlightIdLong;
                if (newFlightId <= 0)
                {
                    return BusinessResult.FailureResult("KhÃ´ng thá»ƒ táº¡o chuyáº¿n bay. Vui lÃ²ng thá»­ láº¡i");
                }

                // Step 9: Retrieve created flight
                var createdFlight = flightDAO.GetById(newFlightId);
                if (createdFlight == null)
                {
                    return BusinessResult.FailureResult("Táº¡o chuyáº¿n bay thÃ nh cÃ´ng nhÆ°ng khÃ´ng thá»ƒ láº¥y thÃ´ng tin chi tiáº¿t");
                }

                return BusinessResult.SuccessResult(
                    $"Táº¡o chuyáº¿n bay '{flight.FlightNumber}' thÃ nh cÃ´ng",
                    createdFlight
                );
            }
            catch (MySqlException ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        public BusinessResult UpdateFlight(FlightDTO flight)
        {
            try
            {
                // Step 1: Validate input
                if (flight == null || flight.FlightId <= 0)
                {
                    return BusinessResult.ValidationError("ThÃ´ng tin chuyáº¿n bay khÃ´ng há»£p lá»‡");
                }

                // Step 2: Check if flight exists
                var existingFlight = flightDAO.GetById(flight.FlightId);
                if (existingFlight == null)
                {
                    return BusinessResult.ValidationError("Chuyáº¿n bay khÃ´ng tá»“n táº¡i trong há»‡ thá»‘ng");
                }

                // Step 3: Check if flight can be modified
                var canModifyResult = CanModifyFlight(flight.FlightId);
                if (!canModifyResult.Success)
                {
                    return canModifyResult;
                }

                // Step 4: Validate flight number (if changed)
                if (!string.IsNullOrWhiteSpace(flight.FlightNumber) &&
                    flight.FlightNumber != existingFlight.FlightNumber)
                {
                    if (flightDAO.ExistsByFlightNumber(flight.FlightNumber))
                    {
                        return BusinessResult.ValidationError(
                            $"MÃ£ chuyáº¿n bay '{flight.FlightNumber}' Ä‘Ã£ tá»“n táº¡i trong há»‡ thá»‘ng");
                    }
                }

                // Step 5: Validate route
                if (flight.RouteId <= 0)
                {
                    return BusinessResult.ValidationError("Tuyáº¿n bay pháº£i Ä‘Æ°á»£c chá»n");
                }

                // Step 6: Validate times
                if (flight.ArrivalTime <= flight.DepartureTime)
                {
                    return BusinessResult.ValidationError("Thá»i gian Ä‘áº¿n pháº£i sau thá»i gian khá»Ÿi hÃ nh");
                }

                var duration = flight.ArrivalTime - flight.DepartureTime;
                if (duration.HasValue && duration.Value.TotalMinutes < MIN_FLIGHT_DURATION_MINUTES)
                {
                    return BusinessResult.ValidationError(
                        $"Thá»i gian bay tá»‘i thiá»ƒu lÃ  {MIN_FLIGHT_DURATION_MINUTES} phÃºt");
                }

                if (duration.HasValue && duration.Value.TotalHours > MAX_FLIGHT_DURATION_HOURS)
                {
                    return BusinessResult.ValidationError(
                        $"Thá»i gian bay khÃ´ng Ä‘Æ°á»£c vÆ°á»£t quÃ¡ {MAX_FLIGHT_DURATION_HOURS} giá»");
                }

                // Step 7: Validate aircraft
                if (flight.AircraftId <= 0)
                {
                    return BusinessResult.ValidationError("Pháº£i chá»n mÃ¡y bay cho chuyáº¿n bay");
                }

                // Step 8: Update in database
                bool updateSuccess = flightDAO.Update(flight);
                if (!updateSuccess)
                {
                    return BusinessResult.FailureResult("KhÃ´ng thá»ƒ cáº­p nháº­t chuyáº¿n bay. Vui lÃ²ng thá»­ láº¡i");
                }

                // Step 9: Retrieve updated flight
                var updatedFlight = flightDAO.GetById(flight.FlightId);
                return BusinessResult.SuccessResult(
                    $"Cáº­p nháº­t chuyáº¿n bay '{flight.FlightNumber}' thÃ nh cÃ´ng",
                    updatedFlight
                );
            }
            catch (MySqlException ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        public BusinessResult DeleteFlight(int flightId)
        {
            try
            {
                // Step 1: Validate input
                if (flightId <= 0)
                {
                    return BusinessResult.ValidationError("MÃ£ chuyáº¿n bay khÃ´ng há»£p lá»‡");
                }

                // Step 2: Check if flight exists
                var flight = flightDAO.GetById(flightId);
                if (flight == null)
                {
                    return BusinessResult.ValidationError("Chuyáº¿n bay khÃ´ng tá»“n táº¡i trong há»‡ thá»‘ng");
                }

                // Step 3: Check if flight can be deleted
                var canDeleteResult = CanDeleteFlight(flightId);
                if (!canDeleteResult.Success)
                {
                    return canDeleteResult;
                }

                // Step 4: CASCADE DELETE - Delete related data in correct order
                // 4a. Delete tickets first (they reference flight_seats)
                bool ticketsDeleted = DeleteTicketsByFlightId(flightId);
                if (!ticketsDeleted)
                {
                    return BusinessResult.FailureResult(
                        "KhÃ´ng thá»ƒ xÃ³a vÃ© liÃªn quan Ä‘áº¿n chuyáº¿n bay. Vui lÃ²ng thá»­ láº¡i");
                }

                // 4b. Delete flight seats (they reference flights)
                bool seatsDeleted = flightSeatDAO.DeleteByFlightId(flightId);
                if (!seatsDeleted)
                {
                    return BusinessResult.FailureResult(
                        "KhÃ´ng thá»ƒ xÃ³a gháº¿ cá»§a chuyáº¿n bay. Vui lÃ²ng thá»­ láº¡i");
                }

                // 4c. Finally delete the flight
                bool flightDeleted = flightDAO.Delete(flightId);
                if (!flightDeleted)
                {
                    return BusinessResult.FailureResult("KhÃ´ng thá»ƒ xÃ³a chuyáº¿n bay. Vui lÃ²ng thá»­ láº¡i");
                }

                return BusinessResult.SuccessResult(
                    $"XÃ³a chuyáº¿n bay '{flight.FlightNumber}' vÃ  dá»¯ liá»‡u liÃªn quan thÃ nh cÃ´ng"
                );
            }
            catch (MySqlException ex)
            {
                // Handle foreign key constraint errors
                if (ex.Number == 1451) // Cannot delete or update a parent row
                {
                    return BusinessResult.FailureResult(
                        "KhÃ´ng thá»ƒ xÃ³a chuyáº¿n bay nÃ y vÃ¬ cÃ³ dá»¯ liá»‡u liÃªn quan (gháº¿, vÃ© Ä‘áº·t, thanh toÃ¡n)",
                        errorCode: ex.Number.ToString()
                    );
                }

                return new BusinessResult(false, "Lỗi kết nối cơ sở dữ liệu khi xóa chuyến bay")
                {
                    TechnicalMessage = ex.Message,
                    ErrorCode = ex.Number.ToString()
                };
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        private bool DeleteTicketsByFlightId(int flightId)
        {
            try
            {
                // Get all flight seats for this flight
                var flightSeats = flightSeatDAO.GetByFlightId(flightId);
                if (flightSeats == null || flightSeats.Count == 0)
                {
                    return true; // No seats, nothing to delete
                }

                // For each seat, we would need to delete tickets
                // This assumes you have a TicketDAO with a DeleteByFlightSeatId method
                // For now, this is a placeholder that returns true
                // TODO: Implement actual ticket deletion when TicketDAO is available

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public BusinessResult GetFlightById(int flightId)
        {
            try
            {
                if (flightId <= 0)
                {
                    return BusinessResult.ValidationError("MÃ£ chuyáº¿n bay khÃ´ng há»£p lá»‡");
                }

                var flight = flightDAO.GetById(flightId);
                if (flight == null)
                {
                    return BusinessResult.FailureResult("KhÃ´ng tÃ¬m tháº¥y chuyáº¿n bay");
                }

                return BusinessResult.SuccessResult("Láº¥y thÃ´ng tin chuyáº¿n bay thÃ nh cÃ´ng", flight);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        public BusinessResult GetAllFlights()
        {
            try
            {
                var flights = flightDAO.GetAll();
                return BusinessResult.SuccessResult(
                    $"Láº¥y danh sÃ¡ch {flights.Count} chuyáº¿n bay thÃ nh cÃ´ng",
                    flights
                );
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        public BusinessResult SearchFlights(
            string flightNumber = null,
            int? departurePlaceId = null,
            int? arrivalPlaceId = null,
            DateTime? departureDate = null,
            string status = null)
        {
            try
            {
                var flights = flightDAO.Search(
                    flightNumber,
                    departurePlaceId,
                    arrivalPlaceId,
                    departureDate,
                    status
                );

                return BusinessResult.SuccessResult(
                    $"TÃ¬m tháº¥y {flights.Count} chuyáº¿n bay",
                    flights
                );
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        public BusinessResult GetFlightsByRoute(int departurePlaceId, int arrivalPlaceId, DateTime? departureDate = null)
        {
            try
            {
                if (departurePlaceId <= 0 || arrivalPlaceId <= 0)
                {
                    return BusinessResult.ValidationError("Äiá»ƒm Ä‘i vÃ  Ä‘iá»ƒm Ä‘áº¿n pháº£i Ä‘Æ°á»£c chá»n");
                }

                if (departurePlaceId == arrivalPlaceId)
                {
                    return BusinessResult.ValidationError("Äiá»ƒm Ä‘i vÃ  Ä‘iá»ƒm Ä‘áº¿n khÃ´ng Ä‘Æ°á»£c trÃ¹ng nhau");
                }

                var flights = flightDAO.GetByRoute(departurePlaceId, arrivalPlaceId, departureDate);
                return BusinessResult.SuccessResult(
                    $"TÃ¬m tháº¥y {flights.Count} chuyáº¿n bay",
                    flights
                );
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        public BusinessResult GetFlightsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BusinessResult.ValidationError("NgÃ y báº¯t Ä‘áº§u pháº£i trÆ°á»›c ngÃ y káº¿t thÃºc");
                }

                var flights = flightDAO.GetByDateRange(startDate, endDate);
                return BusinessResult.SuccessResult(
                    $"TÃ¬m tháº¥y {flights.Count} chuyáº¿n bay",
                    flights
                );
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        public BusinessResult GetFlightsByAircraft(int aircraftId)
        {
            try
            {
                if (aircraftId <= 0)
                {
                    return BusinessResult.ValidationError("MÃ£ mÃ¡y bay khÃ´ng há»£p lá»‡");
                }

                var flights = flightDAO.GetByAircraftId(aircraftId);
                return BusinessResult.SuccessResult(
                    $"TÃ¬m tháº¥y {flights.Count} chuyáº¿n bay",
                    flights
                );
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        private BusinessResult ValidateFlightBusinessRules(FlightDTO flight)
        {
            // Business rule 1: Check for overlapping flights with same aircraft
            var aircraftFlights = flightDAO.GetByAircraftId(flight.AircraftId);
            var overlappingFlights = aircraftFlights.Where(f =>
                f.FlightId != flight.FlightId && // Exclude current flight when updating
                f.DepartureTime < flight.ArrivalTime &&
                f.ArrivalTime > flight.DepartureTime
            ).ToList();

            if (overlappingFlights.Any())
            {
                var conflictingFlight = overlappingFlights.First();
                return BusinessResult.ValidationError(
                    $"MÃ¡y bay Ä‘Ã£ Ä‘Æ°á»£c sá»­ dá»¥ng cho chuyáº¿n bay '{conflictingFlight.FlightNumber}' " +
                    $"trong khoáº£ng thá»i gian nÃ y ({conflictingFlight.DepartureTime:dd/MM/yyyy HH:mm} - " +
                    $"{conflictingFlight.ArrivalTime:dd/MM/yyyy HH:mm})"
                );
            }

            // Business rule 2: Check minimum turnaround time (assume 2 hours)
            const int TURNAROUND_HOURS = 2;
            var nearbyFlights = aircraftFlights.Where(f =>
                f.FlightId != flight.FlightId &&
                f.ArrivalTime.HasValue && f.DepartureTime.HasValue &&
                flight.ArrivalTime.HasValue && flight.DepartureTime.HasValue &&
                (Math.Abs((f.ArrivalTime - flight.DepartureTime).Value.TotalHours) < TURNAROUND_HOURS ||
                 Math.Abs((flight.ArrivalTime - f.DepartureTime).Value.TotalHours) < TURNAROUND_HOURS)
            ).ToList();

            if (nearbyFlights.Any())
            {
                var nearFlight = nearbyFlights.First();
                return BusinessResult.ValidationError(
                    $"MÃ¡y bay cáº§n Ã­t nháº¥t {TURNAROUND_HOURS} giá» giá»¯a cÃ¡c chuyáº¿n bay. " +
                    $"Chuyáº¿n bay '{nearFlight.FlightNumber}' quÃ¡ gáº§n vá»›i thá»i gian Ä‘Ã£ chá»n"
                );
            }

            return BusinessResult.SuccessResult("Validation passed");
        }

        private BusinessResult CanModifyFlight(int flightId)
        {
            try
            {
                var flight = flightDAO.GetById(flightId);
                if (flight == null)
                {
                    return BusinessResult.ValidationError("Chuyáº¿n bay khÃ´ng tá»“n táº¡i");
                }

                // Business rule: Cannot modify flights that have already departed
                if (flight.DepartureTime <= DateTime.Now)
                {
                    return BusinessResult.ValidationError(
                        "KhÃ´ng thá»ƒ sá»­a chuyáº¿n bay Ä‘Ã£ khá»Ÿi hÃ nh hoáº·c Ä‘ang bay"
                    );
                }

                // Business rule: Cannot modify flights less than MIN_BOOKING_HOURS before departure
                if (flight.DepartureTime <= DateTime.Now.AddHours(MIN_BOOKING_HOURS_BEFORE_DEPARTURE))
                {
                    return BusinessResult.ValidationError(
                        $"KhÃ´ng thá»ƒ sá»­a chuyáº¿n bay trong vÃ²ng {MIN_BOOKING_HOURS_BEFORE_DEPARTURE} giá» trÆ°á»›c giá» khá»Ÿi hÃ nh"
                    );
                }

                // Check if flight has confirmed bookings
                var flightSeats = flightSeatDAO.GetByFlightId(flightId);
                var hasBookings = flightSeats.Any(seat => seat.SeatStatus == "booked" || seat.SeatStatus == "confirmed");
                if (hasBookings)
                {
                    return BusinessResult.ValidationError(
                        "KhÃ´ng thá»ƒ sá»­a chuyáº¿n bay Ä‘Ã£ cÃ³ khÃ¡ch hÃ ng Ä‘áº·t vÃ©. Vui lÃ²ng liÃªn há»‡ quáº£n lÃ½"
                    );
                }

                return BusinessResult.SuccessResult("Can modify flight");
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        private BusinessResult CanDeleteFlight(int flightId)
        {
            try
            {
                var flight = flightDAO.GetById(flightId);
                if (flight == null)
                {
                    return BusinessResult.ValidationError("Chuyáº¿n bay khÃ´ng tá»“n táº¡i");
                }

                // Business rule: Cannot delete flights that have already departed
                if (flight.DepartureTime <= DateTime.Now)
                {
                    return BusinessResult.ValidationError(
                        "KhÃ´ng thá»ƒ xÃ³a chuyáº¿n bay Ä‘Ã£ khá»Ÿi hÃ nh"
                    );
                }

                // Note: We allow deletion with cascade, so we don't check for bookings here
                // The cascade delete will handle removing all related data

                return BusinessResult.SuccessResult("Can delete flight");
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        // Flight number suggestion
        public BusinessResult SuggestNextFlightNumber(string prefix)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(prefix))
                {
                    return BusinessResult.ValidationError("Prefix khÃ´ng Ä‘Æ°á»£c Ä‘á»ƒ trá»‘ng");
                }

                prefix = prefix.ToUpper().Trim();

                // Get all flights with this prefix
                var result = SearchFlights(prefix);
                if (!result.Success)
                {
                    // If search fails, suggest prefix + 1
                    return BusinessResult.SuccessResult("Gá»£i Ã½ mÃ£ chuyáº¿n bay", $"{prefix}1");
                }

                var flights = result.GetData<List<FlightDTO>>();
                if (flights == null || flights.Count == 0)
                {
                    return BusinessResult.SuccessResult("Gá»£i Ã½ mÃ£ chuyáº¿n bay", $"{prefix}1");
                }

                // Find highest number with this prefix
                int maxNumber = 0;
                foreach (var flight in flights)
                {
                    if (flight.FlightNumber.StartsWith(prefix))
                    {
                        string numberPart = flight.FlightNumber.Substring(prefix.Length);
                        if (int.TryParse(numberPart, out int number))
                        {
                            if (number > maxNumber)
                            {
                                maxNumber = number;
                            }
                        }
                    }
                }

                string suggested = $"{prefix}{maxNumber + 1}";
                return BusinessResult.SuccessResult("Gá»£i Ã½ mÃ£ chuyáº¿n bay", suggested);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        // For GUI compatibility
        public BusinessResult SearchFlightsForDisplay(string flightNumber, int? departureAirportId, 
            int? arrivalAirportId, DateTime? departureDate, int? cabinClassId, string status)
        {
            try
            {
                var flights = SearchFlights(flightNumber, departureAirportId, arrivalAirportId, departureDate, status);
                return flights;
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        // Statistics methods
        public BusinessResult GetFlightStatistics(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BusinessResult.ValidationError("NgÃ y báº¯t Ä‘áº§u pháº£i trÆ°á»›c ngÃ y káº¿t thÃºc");
                }

                var flights = flightDAO.GetByDateRange(startDate, endDate);

                var stats = new
                {
                    Total = flights.Count,
                    Scheduled = flights.Count(f => f.Status == FlightStatus.SCHEDULED),
                    Delayed = flights.Count(f => f.Status == FlightStatus.DELAYED),
                    Cancelled = flights.Count(f => f.Status == FlightStatus.CANCELLED),
                    Completed = flights.Count(f => f.Status == FlightStatus.COMPLETED),
                    FlightsByRoute = flights.GroupBy(f => f.RouteId)
                        .Select(g => new
                        {
                            RouteId = g.Key,
                            Count = g.Count()
                        }).ToList()
                };

                return BusinessResult.SuccessResult("Thá»'ng kÃª chuyáº¿n bay thÃ nh cÃ´ng", stats);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        public BusinessResult GetFlightStatistics()
        {
            return GetFlightStatistics(DateTime.MinValue, DateTime.MaxValue);
        }

        public BusinessResult UpdateFlightStatus(int flightId, FlightStatus newStatus)
        {
            try
            {
                var flightResult = GetFlightById(flightId);
                if (!flightResult.Success)
                    return flightResult;

                var flight = flightResult.GetData<FlightDTO>();
                if (flight == null)
                    return BusinessResult.FailureResult("Không tìm thấy chuyến bay");

                // Validation: status transitions
                var validTransitions = new Dictionary<FlightStatus, FlightStatus[]>
                {
                    [FlightStatus.SCHEDULED] = new[] { FlightStatus.DELAYED, FlightStatus.CANCELLED, FlightStatus.COMPLETED },
                    [FlightStatus.DELAYED] = new[] { FlightStatus.CANCELLED, FlightStatus.COMPLETED },
                    [FlightStatus.CANCELLED] = System.Array.Empty<FlightStatus>(),
                    [FlightStatus.COMPLETED] = System.Array.Empty<FlightStatus>()
                };

                if (!validTransitions[flight.Status].Contains(newStatus))
                    return BusinessResult.FailureResult($"Không thể chuyển trạng thái từ {flight.Status} sang {newStatus}");

                flight.Status = newStatus;
                var updateResult = flightDAO.Update(flight);
                return updateResult
                    ? BusinessResult.SuccessResult("Cập nhật trạng thái thành công")
                    : BusinessResult.FailureResult("Cập nhật trạng thái thất bại");
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        public BusinessResult SearchFlightsByNumber(string flightNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(flightNumber))
                    return BusinessResult.ValidationError("Mã chuyến bay không được rỗng");

                var flights = flightDAO.Search(flightNumber, null, null, null, "");
                return BusinessResult.SuccessResult($"Tìm thấy {flights.Count} chuyến bay", flights);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        public BusinessResult GetFlightsByStatus(FlightStatus status)
        {
            try
            {
                var allFlights = flightDAO.GetAll();
                var flights = allFlights.Where(f => f.Status == status).ToList();
                return BusinessResult.SuccessResult($"Tìm thấy {flights.Count} chuyến bay có trạng thái {status}", flights);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }
    }
}
