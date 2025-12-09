using DAO.Database;
using DTO.Flight;
using MySqlConnector;
using System;
using System.Data;
using System.Runtime.InteropServices;
namespace DAO.Flight
{
    public class FlightDAO: BaseDAO
    {
        #region Singleton Pattern
        private static FlightDAO _instance;
        private static readonly object _lock = new object();
        private FlightDAO() { }
        public static FlightDAO Instance
        {
            get
            {
                if (_instance == null) {
                    lock (_lock)
                    {
                        if(_instance == null)
                        {
                            _instance = new FlightDAO();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region Private Helpers Methods
        private FlightDTO MapReaderToDTO(MySqlDataReader reader)
        {
            return new FlightDTO(
                flightId: GetInt32(reader, "flight_id"),
                flightNumber: GetString(reader, "flight_number"),
                aircraftId: GetInt32(reader, "aircraft_id"),
                routeId: GetInt32(reader, "route_id"),
                departureTime: GetDateTime(reader, "departure_time"),
                arrivalTime: GetDateTime(reader, "arrival_time"),
                basePrice: GetDecimal(reader, "base_price") ?? 0m,
                note: GetString(reader, "note"),
                status: FlightStatusExtensions.Parse(GetString(reader, "status"))
                );
        }

        private FlightWithDetailsDTO MapReaderToDetailsDTO(MySqlDataReader reader)
        {
            return new FlightWithDetailsDTO
            {
                FlightId = GetInt32(reader, "flight_id"),
                FlightNumber = GetString(reader, "flight_number"),
                AircraftId = GetInt32(reader, "aircraft_id"),
                RouteId = GetInt32(reader, "route_id"),
                DepartureTime = GetDateTime(reader, "departure_time"),
                ArrivalTime = GetDateTime(reader, "arrival_time"),
                BasePrice = GetDecimal(reader, "base_price") ?? 0m,
                Note = GetString(reader, "note"),
                Status = FlightStatusExtensions.Parse(GetString(reader, "status")),
                
                // Airport information
                DepartureAirportId = GetInt32(reader, "departure_airport_id"),
                DepartureAirportCode = GetString(reader, "departure_airport_code"),
                DepartureAirportName = GetString(reader, "departure_airport_name"),
                DepartureCity = GetString(reader, "departure_city"),
                
                ArrivalAirportId = GetInt32(reader, "arrival_airport_id"),
                ArrivalAirportCode = GetString(reader, "arrival_airport_code"),
                ArrivalAirportName = GetString(reader, "arrival_airport_name"),
                ArrivalCity = GetString(reader, "arrival_city"),
                
                // Aircraft information
                AircraftModel = GetString(reader, "aircraft_model"),
                AircraftManufacturer = GetString(reader, "aircraft_manufacturer"),
                
                // Available seats
                AvailableSeats = GetInt32(reader, "available_seats")
            };
        }
        #endregion
        #region CRUD Operations
        public List<FlightDTO> GetAll()
        {
            List<FlightDTO> flights = new List<FlightDTO>();

            string query = @"
                SELECT 
                    flight_id,
                    flight_number,
                    aircraft_id,
                    route_id,
                    departure_time,
                    arrival_time,
                    base_price,
                    note,
                    status
                FROM Flights
                ORDER BY departure_time DESC";

            try
            {
                ExecuteReader(query, reader =>
                {
                    flights.Add(MapReaderToDTO(reader));
                });

                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách chuyến bay: {ex.Message}", ex);
            }
        }

        public List<FlightWithDetailsDTO> GetAllWithDetails()
        {
            List<FlightWithDetailsDTO> flights = new List<FlightWithDetailsDTO>();

            string query = @"
                SELECT 
                    f.flight_id,
                    f.flight_number,
                    f.aircraft_id,
                    f.route_id,
                    f.departure_time,
                    f.arrival_time,
                    f.base_price,
                    f.note,
                    f.status,
                    
                    -- Departure Airport
                    dep_airport.airport_id AS departure_airport_id,
                    dep_airport.airport_code AS departure_airport_code,
                    dep_airport.airport_name AS departure_airport_name,
                    dep_airport.city AS departure_city,
                    
                    -- Arrival Airport
                    arr_airport.airport_id AS arrival_airport_id,
                    arr_airport.airport_code AS arrival_airport_code,
                    arr_airport.airport_name AS arrival_airport_name,
                    arr_airport.city AS arrival_city,
                    
                    -- Aircraft information
                    ac.model AS aircraft_model,
                    ac.manufacturer AS aircraft_manufacturer,
                    
                    -- Available seats (tổng ghế trống)
                    COALESCE(SUM(CASE WHEN fs.seat_status = 'AVAILABLE' THEN 1 ELSE 0 END), 0) AS available_seats
                    
                FROM Flights f
                INNER JOIN Routes r ON f.route_id = r.route_id
                INNER JOIN Airports dep_airport ON r.departure_place_id = dep_airport.airport_id
                INNER JOIN Airports arr_airport ON r.arrival_place_id = arr_airport.airport_id
                LEFT JOIN Aircrafts ac ON f.aircraft_id = ac.aircraft_id
                LEFT JOIN Flight_Seats fs ON f.flight_id = fs.flight_id
                WHERE f.is_deleted = FALSE
                GROUP BY f.flight_id, f.flight_number, f.aircraft_id, f.route_id, 
                         f.departure_time, f.arrival_time, f.base_price, f.note, f.status,
                         dep_airport.airport_id, dep_airport.airport_code, 
                         dep_airport.airport_name, dep_airport.city,
                         arr_airport.airport_id, arr_airport.airport_code, 
                         arr_airport.airport_name, arr_airport.city,
                         ac.model, ac.manufacturer
                ORDER BY f.departure_time DESC";

            try
            {
                ExecuteReader(query, reader =>
                {
                    flights.Add(MapReaderToDetailsDTO(reader));
                });

                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách chuyến bay chi tiết: {ex.Message}", ex);
            }
        }

        public FlightDTO GetById(int flightId)
        {
            string query = @"
                SELECT 
                    flight_id,
                    flight_number,
                    aircraft_id,
                    route_id,
                    departure_time,
                    arrival_time,
                    base_price,
                    note,
                    status
                FROM Flights
                WHERE flight_id = @flightId AND is_deleted = FALSE";

            var parameters = new Dictionary<string, object>
            {
                { "@flightId", flightId }
            };

            FlightDTO flight = null;

            try
            {
                ExecuteReader(query, reader =>
                {
                    flight = MapReaderToDTO(reader);
                }, parameters);

                return flight;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chuyến bay ID {flightId}: {ex.Message}", ex);
            }
        }

        public long Insert(FlightDTO flight)
        {
            // Validate trước khi insert
            if (!flight.IsValid(out string errorMessage))
            {
                throw new ArgumentException($"Dữ liệu không hợp lệ: {errorMessage}");
            }

            string query = @"
                INSERT INTO Flights (
                    flight_number,
                    aircraft_id,
                    route_id,
                    departure_time,
                    arrival_time,
                    base_price,
                    note,
                    status
                ) VALUES (
                    @flightNumber,
                    @aircraftId,
                    @routeId,
                    @departureTime,
                    @arrivalTime,
                    @basePrice,
                    @note,
                    @status
                )";

            var parameters = new Dictionary<string, object>
            {
                { "@flightNumber", flight.FlightNumber },
                { "@aircraftId", flight.AircraftId },
                { "@routeId", flight.RouteId },
                { "@departureTime", flight.DepartureTime },
                { "@arrivalTime", flight.ArrivalTime },
                { "@basePrice", flight.BasePrice },
                { "@note", (object?)flight.Note ?? DBNull.Value },
                { "@status", flight.Status.ToString() }
            };

            try
            {
                long newId = ExecuteInsertAndGetId(query, parameters);
                return newId;
            }
            catch (MySqlException ex)
            {
                // Check for duplicate flight_number
                if (ex.Number == 1062) // MySQL duplicate entry error code
                {
                    throw new Exception($"Số hiệu chuyến bay '{flight.FlightNumber}' đã tồn tại vào thời điểm này!", ex);
                }
                throw new Exception($"Lỗi khi thêm chuyến bay: {ex.Message}", ex);
            }
        }
        public bool Update(FlightDTO flight)
        {
            if(!flight.IsValid(out string erroMessage))
            {
                throw new ArgumentException($"Dữ liệu chuyến bay không hợp lệ: {erroMessage}");
            }
            string query = @"
                UPDATE Flights 
                SET
                    flight_number = @flightNumber,
                    aircraft_id = @aircraftId,
                    route_id = @routeId,
                    departure_time = @departureTime,
                    arrival_time = @arrivalTime,
                    base_price = @basePrice,
                    note = @note,
                    status = @status
                WHERE flight_id = @flightId";
            var parameters = new Dictionary<string, object>
            {
                { "@flightId", flight.FlightId },
                { "@flightNumber", flight.FlightNumber },
                { "@aircraftId", flight.AircraftId },
                { "@routeId", flight.RouteId },
                { "@departureTime", flight.DepartureTime },
                { "@arrivalTime", flight.ArrivalTime },
                { "@basePrice", flight.BasePrice },
                { "@note", (object?)flight.Note ?? DBNull.Value },
                { "@status", flight.Status.ToString() }
            };
            try
            {
                int affectedRows = ExecuteNonQuery(query, parameters);
                return affectedRows > 0;
            }
            catch(MySqlException ex)
            {
                if(ex.Number == 1062) // Duplicate entry
                {
                    throw new Exception($"Số hiệu chuyến bay '{flight.FlightNumber}' đã tồn tại.", ex);
                }
                throw new Exception($"Lỗi khi cập nhật thông tin chuyến bay với ID {flight.FlightId}: {ex.Message}", ex);
            }
        }
        public bool Delete (int flightId)
        {
            var parameter = new Dictionary<string, object> { {"@flightId", flightId } };
            
            // Step 1: Check if there are any tickets related to this flight's seats using JOIN
            try
            {
                string checkTicketsQuery = @"
                    SELECT COUNT(*) 
                    FROM Tickets t
                    JOIN Flight_Seats fs ON t.flight_seat_id = fs.flight_seat_id
                    WHERE fs.flight_id = @flightId";
                    
                var ticketCount = ExecuteScalar(checkTicketsQuery, parameter);
                
                if (ticketCount != null && Convert.ToInt32(ticketCount) > 0)
                {
                    throw new Exception($"Không thể xóa chuyến bay vì đã có {ticketCount} vé được đặt. Vui lòng hủy các vé trước khi xóa chuyến bay.");
                }
            }
            catch (Exception ex) when (ex.Message.Contains("Không thể xóa chuyến bay vì đã có"))
            {
                // Re-throw our custom message
                throw;
            }
            catch (Exception ex)
            {
                // If check fails, still try to delete (will fail at FK constraint if needed)
                System.Diagnostics.Debug.WriteLine($"Warning: Could not check tickets: {ex.Message}");
            }
            
            // Step 2: Delete Flight_Seats (no tickets exist at this point)
            try
            {
                string deleteSeatsQuery = "DELETE FROM Flight_Seats WHERE flight_id = @flightId";
                ExecuteNonQuery(deleteSeatsQuery, parameter);
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Lỗi khi xóa ghế ngồi: {ex.Message}", ex);
            }
            
            // Step 3: Delete the flight
            try
            {
                string deleteFlightQuery = "DELETE FROM Flights WHERE flight_id = @flightId";
                int affectedRows = ExecuteNonQuery(deleteFlightQuery, parameter);
                return affectedRows > 0;
            }
            catch (MySqlException ex)
            {
                if(ex.Number == 1451) // Foreign key constraint fails
                {
                    throw new Exception($"Không thể xóa chuyến bay vì có dữ liệu liên quan trong cơ sở dữ liệu.", ex);
                }
                throw new Exception($"Lỗi khi xóa chuyến bay: {ex.Message}", ex);
            }
        }
        #endregion
        
        #region Business Validation Methods

        /// <summary>
        /// Kiểm tra xem flight_number đã tồn tại chưa (dùng cho validation khi tạo mới)
        /// </summary>
        public bool IsFlightNumberExists(string flightNumber, int? excludeFlightId = null)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM Flights 
                WHERE flight_number = @flightNumber";

            var parameters = new Dictionary<string, object>
            {
                { "@flightNumber", flightNumber.Trim().ToUpper() }
            };

            // Nếu đang edit, loại trừ chính bản ghi đó
            if (excludeFlightId.HasValue)
            {
                query += " AND flight_id != @excludeFlightId";
                parameters.Add("@excludeFlightId", excludeFlightId.Value);
            }

            try
            {
                object result = ExecuteScalar(query, parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra mã chuyến bay: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra xung đột thời gian máy bay (aircraft không được phục vụ 2 chuyến cùng lúc)
        /// </summary>
        public bool HasAircraftTimeConflict(int aircraftId, DateTime departureTime, DateTime arrivalTime, int? excludeFlightId = null)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM Flights 
                WHERE aircraft_id = @aircraftId
                  AND status != 'CANCELLED'
                  AND (
                      -- Departure time của chuyến mới nằm trong khoảng chuyến cũ
                      (@departureTime >= departure_time AND @departureTime < arrival_time)
                      OR
                      -- Arrival time của chuyến mới nằm trong khoảng chuyến cũ
                      (@arrivalTime > departure_time AND @arrivalTime <= arrival_time)
                      OR
                      -- Chuyến mới bao trùm chuyến cũ
                      (@departureTime <= departure_time AND @arrivalTime >= arrival_time)
                  )";

            var parameters = new Dictionary<string, object>
            {
                { "@aircraftId", aircraftId },
                { "@departureTime", departureTime },
                { "@arrivalTime", arrivalTime }
            };

            // Nếu đang edit, loại trừ chính bản ghi đó
            if (excludeFlightId.HasValue)
            {
                query += " AND flight_id != @excludeFlightId";
                parameters.Add("@excludeFlightId", excludeFlightId.Value);
            }

            try
            {
                object result = ExecuteScalar(query, parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra xung đột thời gian máy bay: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách chuyến bay xung đột với máy bay trong khoảng thời gian
        /// </summary>
        public List<FlightDTO> GetConflictingFlights(int aircraftId, DateTime departureTime, DateTime arrivalTime, int? excludeFlightId = null)
        {
            List<FlightDTO> flights = new List<FlightDTO>();

            string query = @"
                SELECT 
                    flight_id,
                    flight_number,
                    aircraft_id,
                    route_id,
                    departure_time,
                    arrival_time,
                    base_price,
                    note,
                    status
                FROM Flights 
                WHERE aircraft_id = @aircraftId
                  AND status != 'CANCELLED'
                  AND (
                      (@departureTime >= departure_time AND @departureTime < arrival_time)
                      OR
                      (@arrivalTime > departure_time AND @arrivalTime <= arrival_time)
                      OR
                      (@departureTime <= departure_time AND @arrivalTime >= arrival_time)
                  )";

            var parameters = new Dictionary<string, object>
            {
                { "@aircraftId", aircraftId },
                { "@departureTime", departureTime },
                { "@arrivalTime", arrivalTime }
            };

            if (excludeFlightId.HasValue)
            {
                query += " AND flight_id != @excludeFlightId";
                parameters.Add("@excludeFlightId", excludeFlightId.Value);
            }

            query += " ORDER BY departure_time";

            try
            {
                ExecuteReader(query, reader =>
                {
                    flights.Add(MapReaderToDTO(reader));
                }, parameters);

                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm chuyến bay xung đột: {ex.Message}", ex);
            }
        }

        #endregion

        #region Soft Delete
        public bool SoftDelete(int flightId)
        {
            string query = @"UPDATE Flights SET is_deleted = TRUE WHERE flight_id = @flightId";
            
            var parameters = new Dictionary<string, object>
            {
                { "@flightId", flightId }
            };

            try
            {
                int rowsAffected = ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Đã xảy ra lỗi khi xóa chuyến bay: {ex.Message}", ex);
            }
        }
        #endregion
        
        #region Search & Filter Methods
        public List<FlightDTO> SearchByFlightNumber(string flightNumber)
        {
            List<FlightDTO> flights = new List<FlightDTO>();

            string query = @"
                SELECT 
                    flight_id,
                    flight_number,
                    aircraft_id,
                    route_id,
                    departure_time,
                    arrival_time,
                    base_price,
                    note,
                    status
                FROM Flights
                WHERE flight_number LIKE @flightNumber AND is_deleted = FALSE
                ORDER BY departure_time DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@flightNumber", $"%{flightNumber}%" }
            };

            try
            {
                ExecuteReader(query, reader =>
                {
                    flights.Add(MapReaderToDTO(reader));
                }, parameters);

                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm chuyến bay: {ex.Message}", ex);
            }
        }
        public List<FlightDTO> GetByStatus(FlightStatus status)
        {
            List<FlightDTO> flights = new List<FlightDTO>();

            string query = @"
                SELECT 
                    flight_id,
                    flight_number,
                    aircraft_id,
                    route_id,
                    departure_time,
                    arrival_time,
                    base_price,
                    note,
                    status
                FROM Flights
                WHERE status = @status AND is_deleted = FALSE
                ORDER BY departure_time DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@status", status.ToString() }
            };

            try
            {
                ExecuteReader(query, reader =>
                {
                    flights.Add(MapReaderToDTO(reader));
                }, parameters);

                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chuyến bay theo trạng thái: {ex.Message}", ex);
            }
        }
        public List<FlightDTO> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            List<FlightDTO> flights = new List<FlightDTO>();

            string query = @"
                SELECT 
                    flight_id,
                    flight_number,
                    aircraft_id,
                    route_id,
                    departure_time,
                    arrival_time,
                    base_price,
                    note,
                    status
                FROM Flights
                WHERE departure_time >= @fromDate AND departure_time <= @toDate
                ORDER BY departure_time ASC";

            var parameters = new Dictionary<string, object>
            {
                { "@fromDate", fromDate },
                { "@toDate", toDate }
            };

            try
            {
                ExecuteReader(query, reader =>
                {
                    flights.Add(MapReaderToDTO(reader));
                }, parameters);

                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chuyến bay theo ngày: {ex.Message}", ex);
            }
        }
        public List<FlightDTO> GetByAircraftId(int aircraftId)
        {
            List<FlightDTO> flights = new List<FlightDTO>();
            string query = @"
                SELECT
                    flight_id,
                    flight_number,
                    aircraft_id,
                    route_id,
                    departure_time,
                    arrival_time,
                    status
                FROM Flights
                WHERE aircraft_id = @aircraftId
                ORDER BY departure_time DESC";
            var parameters = new Dictionary<string, object>
            {
                {"@aircraftId", aircraftId }
            };
            try
            {
                ExecuteReader(query, reader =>
                {
                    flights.Add(MapReaderToDTO(reader));
                }, parameters);
                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lọc chuyến bay với máy bay ID {aircraftId}: {ex.Message}", ex);
            }
        }
        public List<FlightDTO> GetByRouteId(int routeId)
        {
            List<FlightDTO> flights = new List<FlightDTO>();
            string query = @"
                SELECT
                    flight_id,
                    flight_number,
                    aircraft_id,
                    route_id,
                    departure_time,
                    arrival_time,
                    status
                FROM Flights
                WHERE route_id = @routeId
                ORDER BY departure_time DESC";
            var parameters = new Dictionary<string, object>
            {
                {"@routeId", routeId }
            };
            try
            {
                ExecuteReader(query, reader =>
                {
                    flights.Add(MapReaderToDTO(reader));
                }, parameters);
                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lọc chuyến bay với tuyến bay ID {routeId}: {ex.Message}", ex);
            }
        }

        public List<FlightWithDetailsDTO> GlobalSearch(string searchTerm)
        {
            var flights = new List<FlightWithDetailsDTO>();
            string query = @"
                SELECT 
                    f.flight_id, f.flight_number, f.aircraft_id, f.route_id, f.departure_time, f.arrival_time, f.base_price, f.note, f.status,
                    dep.airport_id AS departure_airport_id, dep.airport_code AS departure_airport_code, dep.airport_name AS departure_airport_name, dep.city AS departure_city,
                    arr.airport_id AS arrival_airport_id, arr.airport_code AS arrival_airport_code, arr.airport_name AS arrival_airport_name, arr.city AS arrival_city,
                    ac.model AS aircraft_model, ac.manufacturer AS aircraft_manufacturer,
                    COALESCE(SUM(CASE WHEN fs.seat_status = 'AVAILABLE' THEN 1 ELSE 0 END), 0) AS available_seats
                FROM Flights f
                INNER JOIN Routes r ON f.route_id = r.route_id
                INNER JOIN Airports dep ON r.departure_place_id = dep.airport_id
                INNER JOIN Airports arr ON r.arrival_place_id = arr.airport_id
                LEFT JOIN Aircrafts ac ON f.aircraft_id = ac.aircraft_id
                LEFT JOIN Flight_Seats fs ON f.flight_id = fs.flight_id
                WHERE f.is_deleted = FALSE AND (
                    f.flight_number LIKE @term OR
                    dep.airport_name LIKE @term OR
                    dep.city LIKE @term OR
                    arr.airport_name LIKE @term OR
                    arr.city LIKE @term
                )
                GROUP BY f.flight_id, f.flight_number, f.aircraft_id, f.route_id, 
                         f.departure_time, f.arrival_time, f.base_price, f.note, f.status,
                         dep.airport_id, dep.airport_code, dep.airport_name, dep.city,
                         arr.airport_id, arr.airport_code, arr.airport_name, arr.city,
                         ac.model, ac.manufacturer
                ORDER BY f.departure_time DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@term", $"%{searchTerm}%" }
            };

            try
            {
                ExecuteReader(query, reader =>
                {
                    flights.Add(MapReaderToDetailsDTO(reader));
                }, parameters);
                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thực hiện tìm kiếm toàn cục: {ex.Message}", ex);
            }
        }
        #endregion
        #region Bussiness Logic Methods
        public bool IsFlightNumberExists(string flightNumber, DateTime departureTime, int excludeFlightId = 0)
        {
            string query = @"
                SELECT COUNT(*)
                FROM Flights
                WHERE flight_number = @flightNumber
                AND DATE(departure_time) = DATE(@departureTime)
                AND flight_id != @excludeFlightId";
            var parameters = new Dictionary<string, object>
            {
                {"@flightNumber", flightNumber},
                {"@departureTime", departureTime },
                {"@excludeFlightId", excludeFlightId }
            };
            try
            {
                object result = ExecuteScalar(query, parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra tồn tại số hiệu chuyến bay '{flightNumber}': {ex.Message}", ex);
            }
        }
        public int CountAll()
        {
            string query = "SELECT COUNT(*) FROM Flights";
            try
            {
                object result = ExecuteScalar(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đếm tổng số chuyến bay: {ex.Message}", ex);
            }
        }
        public int CountByStatus(FlightStatus status)
        {
            string query = "SELECT COUNT(*) FROM Flights WHERE status = @status";
            var parameters = new Dictionary<string, object>
            {
                {"@status", status.ToString() }
            };
            try
            {
                object result = ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đếm số chuyến bay với trạng thái '{status}': {ex.Message}", ex);
            }
        }
        public bool UpdateStatus(int flightId, FlightStatus newStatus)
        {
            // Lấy flight hiện tại để kiểm tra transition
            var currentFlight = GetById(flightId);
            if (currentFlight == null)
            {
                throw new Exception($"Không tìm thấy chuyến bay ID {flightId}");
            }

            // Kiểm tra có thể chuyển trạng thái không
            if (!currentFlight.Status.CanTransitionTo(newStatus))
            {
                throw new Exception($"Không thể chuyển từ trạng thái {currentFlight.Status.GetDescription()} sang {newStatus.GetDescription()}");
            }

            string query = "UPDATE Flights SET status = @status WHERE flight_id = @flightId";

            var parameters = new Dictionary<string, object>
            {
                { "@flightId", flightId },
                { "@status", newStatus.ToString() }
            };

            try
            {
                int affectedRows = ExecuteNonQuery(query, parameters);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật trạng thái chuyến bay: {ex.Message}", ex);
            }
        }
        #endregion
        #region For DataGridView Binding
        public DataTable GetAllAsDataTable()
        {
            string query = @"
                SELECT 
                    f.flight_id AS 'ID',
                    f.flight_number AS 'Số hiệu',
                    f.departure_time AS 'Khởi hành',
                    f.arrival_time AS 'Đến',
                    f.status AS 'Trạng thái',
                    f.aircraft_id AS 'Máy bay ID',
                    f.route_id AS 'Tuyến bay ID'
                FROM Flights f
                ORDER BY f.departure_time DESC";

            try
            {
                return ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu chuyến bay: {ex.Message}", ex);
            }
        }
        #endregion

        #region Advanced Statistics
        /// <summary>
        /// Lấy thống kê chuyến bay theo tuyến bay phổ biến nhất
        /// </summary>
        public DataTable GetTopRoutesByFlightCount(int topN = 10)
        {
            string query = @"
                SELECT 
                    r.route_id,
                    CONCAT(dep.airport_name, ' → ', arr.airport_name) AS route_name,
                    COUNT(f.flight_id) AS flight_count,
                    r.distance_km,
                    r.duration_minutes
                FROM Flights f
                INNER JOIN Routes r ON f.route_id = r.route_id
                INNER JOIN Airports dep ON r.departure_place_id = dep.airport_id
                INNER JOIN Airports arr ON r.arrival_place_id = arr.airport_id
                GROUP BY r.route_id, route_name, r.distance_km, r.duration_minutes
                ORDER BY flight_count DESC
                LIMIT @topN";

            var parameters = new Dictionary<string, object>
            {
                { "@topN", topN }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thống kê tuyến bay phổ biến: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thống kê chuyến bay theo máy bay được sử dụng nhiều nhất
        /// </summary>
        public DataTable GetTopAircraftsByFlightCount(int topN = 10)
        {
            string query = @"
                SELECT 
                    a.aircraft_id,
                    a.model,
                    a.manufacturer,
                    'Vietnam Airlines' AS airline_name,
                    COUNT(f.flight_id) AS flight_count
                FROM Flights f
                INNER JOIN Aircrafts a ON f.aircraft_id = a.aircraft_id
                GROUP BY a.aircraft_id, a.model, a.manufacturer
                ORDER BY flight_count DESC
                LIMIT @topN";

            var parameters = new Dictionary<string, object>
            {
                { "@topN", topN }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thống kê máy bay: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tỷ lệ hoàn thành chuyến bay theo tháng
        /// </summary>
        public DataTable GetFlightCompletionRateByMonth(DateTime fromDate, DateTime toDate)
        {
            string query = @"
                SELECT 
                    DATE_FORMAT(departure_time, '%Y-%m') AS month_year,
                    COUNT(*) AS total_flights,
                    SUM(CASE WHEN status = 'COMPLETED' THEN 1 ELSE 0 END) AS completed_flights,
                    SUM(CASE WHEN status = 'CANCELLED' THEN 1 ELSE 0 END) AS cancelled_flights,
                    SUM(CASE WHEN status = 'DELAYED' THEN 1 ELSE 0 END) AS delayed_flights,
                    ROUND(SUM(CASE WHEN status = 'COMPLETED' THEN 1 ELSE 0 END) * 100.0 / COUNT(*), 2) AS completion_rate
                FROM Flights
                WHERE departure_time >= @fromDate AND departure_time <= @toDate
                GROUP BY month_year
                ORDER BY month_year";

            var parameters = new Dictionary<string, object>
            {
                { "@fromDate", fromDate },
                { "@toDate", toDate }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy tỷ lệ hoàn thành chuyến bay: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách chuyến bay sắp khởi hành (trong vòng N giờ tới)
        /// </summary>
        public List<FlightDTO> GetUpcomingFlights(int hoursAhead = 24)
        {
            List<FlightDTO> flights = new List<FlightDTO>();

            string query = @"
                SELECT 
                    flight_id,
                    flight_number,
                    aircraft_id,
                    route_id,
                    departure_time,
                    arrival_time,
                    status
                FROM Flights
                WHERE departure_time >= NOW() 
                AND departure_time <= DATE_ADD(NOW(), INTERVAL @hoursAhead HOUR)
                AND status IN ('SCHEDULED', 'DELAYED')
                ORDER BY departure_time ASC";

            var parameters = new Dictionary<string, object>
            {
                { "@hoursAhead", hoursAhead }
            };

            try
            {
                ExecuteReader(query, reader =>
                {
                    flights.Add(MapReaderToDTO(reader));
                }, parameters);

                return flights;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chuyến bay sắp khởi hành: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra xung đột thời gian sử dụng máy bay
        /// </summary>
        public bool CheckAircraftAvailability(int aircraftId, DateTime departureTime, DateTime arrivalTime, int excludeFlightId = 0)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM Flights
                WHERE aircraft_id = @aircraftId
                AND flight_id != @excludeFlightId
                AND status NOT IN ('CANCELLED')
                AND (
                    (departure_time <= @departureTime AND arrival_time > @departureTime)
                    OR (departure_time < @arrivalTime AND arrival_time >= @arrivalTime)
                    OR (departure_time >= @departureTime AND arrival_time <= @arrivalTime)
                )";

            var parameters = new Dictionary<string, object>
            {
                { "@aircraftId", aircraftId },
                { "@departureTime", departureTime },
                { "@arrivalTime", arrivalTime },
                { "@excludeFlightId", excludeFlightId }
            };

            try
            {
                object result = ExecuteScalar(query, parameters);
                return Convert.ToInt32(result) == 0; // True nếu không có xung đột
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra khả dụng của máy bay: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thống kê doanh thu theo chuyến bay
        /// </summary>
        public DataTable GetFlightRevenueStatistics(DateTime fromDate, DateTime toDate)
        {
            string query = @"
                SELECT 
                    f.flight_id,
                    f.flight_number,
                    f.departure_time,
                    f.status,
                    CONCAT(dep.airport_name, ' → ', arr.airport_name) AS route,
                    COUNT(DISTINCT t.ticket_id) AS total_tickets,
                    COUNT(DISTINCT CASE WHEN t.status = 'CONFIRMED' THEN t.ticket_id END) AS confirmed_tickets,
                    COALESCE(SUM(CASE WHEN p.status = 'SUCCESS' THEN p.amount ELSE 0 END), 0) AS total_revenue
                FROM Flights f
                INNER JOIN Routes r ON f.route_id = r.route_id
                INNER JOIN Airports dep ON r.departure_place_id = dep.airport_id
                INNER JOIN Airports arr ON r.arrival_place_id = arr.airport_id
                LEFT JOIN Flight_Seats fs ON f.flight_id = fs.flight_id
                LEFT JOIN Tickets t ON fs.flight_seat_id = t.flight_seat_id
                LEFT JOIN Booking_Passengers bp ON t.ticket_passenger_id = bp.booking_passenger_id
                LEFT JOIN Bookings b ON bp.booking_id = b.booking_id
                LEFT JOIN Payments p ON b.booking_id = p.booking_id
                WHERE f.departure_time >= @fromDate 
                AND f.departure_time <= @toDate
                GROUP BY f.flight_id, f.flight_number, f.departure_time, f.status, route
                ORDER BY f.departure_time DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@fromDate", fromDate },
                { "@toDate", toDate }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thống kê doanh thu chuyến bay: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tổng doanh thu theo tháng
        /// </summary>
        public DataTable GetMonthlyRevenueReport(DateTime fromDate, DateTime toDate)
        {
            string query = @"
                SELECT 
                    DATE_FORMAT(f.departure_time, '%Y-%m') AS month_year,
                    COUNT(DISTINCT f.flight_id) AS total_flights,
                    COUNT(DISTINCT CASE WHEN f.status = 'COMPLETED' THEN f.flight_id END) AS completed_flights,
                    COUNT(DISTINCT t.ticket_id) AS total_tickets,
                    COUNT(DISTINCT CASE WHEN t.status = 'CONFIRMED' THEN t.ticket_id END) AS confirmed_tickets,
                    COALESCE(SUM(CASE WHEN p.status = 'SUCCESS' THEN p.amount ELSE 0 END), 0) AS total_revenue,
                    COUNT(DISTINCT CASE WHEN p.status = 'SUCCESS' THEN p.payment_id END) AS successful_payments
                FROM Flights f
                LEFT JOIN Flight_Seats fs ON f.flight_id = fs.flight_id
                LEFT JOIN Tickets t ON fs.flight_seat_id = t.flight_seat_id
                LEFT JOIN Booking_Passengers bp ON t.ticket_passenger_id = bp.booking_passenger_id
                LEFT JOIN Bookings b ON bp.booking_id = b.booking_id
                LEFT JOIN Payments p ON b.booking_id = p.booking_id
                WHERE f.departure_time >= @fromDate 
                AND f.departure_time <= @toDate
                GROUP BY month_year
                ORDER BY month_year DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@fromDate", fromDate },
                { "@toDate", toDate }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy báo cáo doanh thu tháng: {ex.Message}", ex);
            }
        }
        #endregion

        #region Advanced Search - Tìm kiếm chuyến bay nâng cao
        /// <summary>
        /// Tìm kiếm chuyến bay với nhiều tiêu chí (Advanced Search)
        /// NOTE: This is a simplified version using LINQ filtering on GetAllWithDetails
        /// </summary>
        public List<FlightWithDetailsDTO> SearchFlightsAdvanced(FlightSearchCriteriaDTO criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria không được null");
            }

            try
            {
                // Get all flights then filter with LINQ
                var allFlights = GetAllWithDetails();
                var query = allFlights.AsQueryable();

                // Apply filters
                if (criteria.DepartureAirportId.HasValue)
                    query = query.Where(f => f.DepartureAirportId == criteria.DepartureAirportId.Value);

                if (criteria.ArrivalAirportId.HasValue)
                    query = query.Where(f => f.ArrivalAirportId == criteria.ArrivalAirportId.Value);

                if (!string.IsNullOrWhiteSpace(criteria.FlightNumber))
                    query = query.Where(f => f.FlightNumber != null && f.FlightNumber.Contains(criteria.FlightNumber));

                if (criteria.Status.HasValue)
                    query = query.Where(f => f.Status == criteria.Status.Value);

                if (criteria.DepartureDate.HasValue)
                    query = query.Where(f => f.DepartureTime.HasValue && f.DepartureTime.Value.Date == criteria.DepartureDate.Value.Date);

                if (criteria.DepartureDateFrom.HasValue)
                    query = query.Where(f => f.DepartureTime.HasValue && f.DepartureTime.Value >= criteria.DepartureDateFrom.Value);

                if (criteria.DepartureDateTo.HasValue)
                    query = query.Where(f => f.DepartureTime.HasValue && f.DepartureTime.Value <= criteria.DepartureDateTo.Value);

                if (criteria.MinAvailableSeats.HasValue)
                    query = query.Where(f => f.AvailableSeats >= criteria.MinAvailableSeats.Value);

                // Apply sorting
                string sortBy = criteria.SortBy ?? "DepartureTime";
                bool isAscending = string.IsNullOrWhiteSpace(criteria.SortOrder) || criteria.SortOrder.ToUpper() == "ASC";

                query = sortBy switch
                {
                    "AvailableSeats" => isAscending 
                        ? query.OrderBy(f => f.AvailableSeats)
                        : query.OrderByDescending(f => f.AvailableSeats),
                    _ => isAscending
                        ? query.OrderBy(f => f.DepartureTime)
                        : query.OrderByDescending(f => f.DepartureTime)
                };

                // Apply pagination
                if (criteria.PageNumber.HasValue && criteria.PageSize.HasValue)
                {
                    int skip = (criteria.PageNumber.Value - 1) * criteria.PageSize.Value;
                    query = query.Skip(skip).Take(criteria.PageSize.Value).AsQueryable();
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm chuyến bay nâng cao: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Đếm tổng số kết quả tìm kiếm (để phân trang)
        /// </summary>
        public int CountSearchResults(FlightSearchCriteriaDTO criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria không được null");
            }

            var parameters = new Dictionary<string, object>();

            string query = @"
                SELECT COUNT(DISTINCT f.flight_id) as total
                FROM Flights f
                INNER JOIN Routes r ON f.route_id = r.route_id";

            if (criteria.ClassId.HasValue)
            {
                query += @"
                LEFT JOIN Flight_Seats fs ON f.flight_id = fs.flight_id
                LEFT JOIN Seats s ON fs.seat_id = s.seat_id";
            }

            query += " WHERE 1=1";

            // Same filters as SearchFlightsAdvanced
            if (criteria.DepartureAirportId.HasValue)
            {
                query += " AND r.departure_place_id = @departureAirportId";
                parameters.Add("@departureAirportId", criteria.DepartureAirportId.Value);
            }

            if (criteria.ArrivalAirportId.HasValue)
            {
                query += " AND r.arrival_place_id = @arrivalAirportId";
                parameters.Add("@arrivalAirportId", criteria.ArrivalAirportId.Value);
            }

            if (criteria.DepartureDate.HasValue)
            {
                query += " AND DATE(f.departure_time) = DATE(@departureDate)";
                parameters.Add("@departureDate", criteria.DepartureDate.Value.Date);
            }
            else
            {
                if (criteria.DepartureDateFrom.HasValue)
                {
                    query += " AND f.departure_time >= @departureDateFrom";
                    parameters.Add("@departureDateFrom", criteria.DepartureDateFrom.Value);
                }

                if (criteria.DepartureDateTo.HasValue)
                {
                    query += " AND f.departure_time <= @departureDateTo";
                    parameters.Add("@departureDateTo", criteria.DepartureDateTo.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(criteria.FlightNumber))
            {
                query += " AND f.flight_number LIKE @flightNumber";
                parameters.Add("@flightNumber", $"%{criteria.FlightNumber}%");
            }

            if (criteria.Status.HasValue)
            {
                query += " AND f.status = @status";
                parameters.Add("@status", criteria.Status.Value.ToString());
            }

            if (criteria.AircraftId.HasValue)
            {
                query += " AND f.aircraft_id = @aircraftId";
                parameters.Add("@aircraftId", criteria.AircraftId.Value);
            }

            if (criteria.RouteId.HasValue)
            {
                query += " AND f.route_id = @routeId";
                parameters.Add("@routeId", criteria.RouteId.Value);
            }

            if (criteria.ClassId.HasValue)
            {
                query += " AND s.class_id = @classId";
                parameters.Add("@classId", criteria.ClassId.Value);
            }

            try
            {
                var result = ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đếm kết quả tìm kiếm: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm nhanh chuyến bay (cho người dùng đặt vé)
        /// </summary>
        public List<FlightWithDetailsDTO> QuickSearchFlights(
            int departureAirportId,
            int arrivalAirportId,
            DateTime departureDate,
            int? classId = null,
            int? minSeats = 1)
        {
            var criteria = new FlightSearchCriteriaDTO
            {
                DepartureAirportId = departureAirportId,
                ArrivalAirportId = arrivalAirportId,
                DepartureDate = departureDate,
                ClassId = classId,
                MinAvailableSeats = minSeats,
                Status = FlightStatus.SCHEDULED, // Chỉ tìm chuyến bay đã lên lịch
                SortBy = "DepartureTime",
                SortOrder = "ASC"
            };

            // Get all flights then filter with LINQ
            var allFlights = GetAllWithDetails();
            var filtered = allFlights.Where(f =>
            {
                if (f.Status != FlightStatus.SCHEDULED) return false;
                if (f.DepartureAirportId != departureAirportId) return false;
                if (f.ArrivalAirportId != arrivalAirportId) return false;
                if (f.DepartureTime?.Date != departureDate.Date) return false;
                if (minSeats.HasValue && f.AvailableSeats < minSeats.Value) return false;
                return true;
            }).OrderBy(f => f.DepartureTime).ToList();

            return filtered;
        }
        #endregion

        #region Cabin Class Statistics
        /// <summary>
        /// Lấy thống kê theo hạng vé trong khoảng thời gian
        /// </summary>
        public DataTable GetCabinClassStatistics(DateTime fromDate, DateTime toDate)
        {
            string query = @"
                SELECT 
                    cc.class_name AS cabin_class_name,
                    COUNT(DISTINCT t.ticket_id) AS total_tickets,
                    COALESCE(SUM(CASE WHEN p.status = 'SUCCESS' THEN p.amount ELSE 0 END), 0) AS total_revenue,
                    ROUND(
                        (COUNT(DISTINCT CASE WHEN fs.seat_status = 'BOOKED' THEN fs.flight_seat_id END) * 100.0) / 
                        NULLIF(COUNT(DISTINCT fs.flight_seat_id), 0),
                        1
                    ) AS booking_rate
                FROM Cabin_Classes cc
                LEFT JOIN Seats s ON cc.class_id = s.class_id
                LEFT JOIN Flight_Seats fs ON s.seat_id = fs.seat_id
                LEFT JOIN Flights f ON fs.flight_id = f.flight_id
                LEFT JOIN Tickets t ON fs.flight_seat_id = t.flight_seat_id
                LEFT JOIN Booking_Passengers bp ON t.ticket_passenger_id = bp.booking_passenger_id
                LEFT JOIN Bookings b ON bp.booking_id = b.booking_id
                LEFT JOIN Payments p ON b.booking_id = p.booking_id
                WHERE f.departure_time >= @fromDate 
                AND f.departure_time <= @toDate
                AND f.is_deleted = 0
                GROUP BY cc.class_id, cc.class_name
                ORDER BY total_revenue DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@fromDate", fromDate },
                { "@toDate", toDate }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thống kê hạng vé: {ex.Message}", ex);
            }
        }
        #endregion
    }
}