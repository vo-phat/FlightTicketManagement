using DAO.Database;
using DTO.Flight;
using MySqlConnector;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
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
                status: FlightStatusExtensions.Parse(GetString(reader, "status"))
                );
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
                    status
                FROM Flights
                WHERE flight_id = @flightId";

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
                    status
                ) VALUES (
                    @flightNumber,
                    @aircraftId,
                    @routeId,
                    @departureTime,
                    @arrivalTime,
                    @status
                )";

            var parameters = new Dictionary<string, object>
            {
                { "@flightNumber", flight.FlightNumber },
                { "@aircraftId", flight.AircraftId },
                { "@routeId", flight.RouteId },
                { "@departureTime", flight.DepartureTime },
                { "@arrivalTime", flight.ArrivalTime },
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
                { "@status", flight.Status.ToString() }
            };
            try
            {
                int affectedRows = ExecuteNonQuery(query, parameters);
                return affectedRows >= 0;
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
            string query = @"
                DELETE FROM Flights 
                Where flight_id = @flightId";
            var parameter = new Dictionary<string, object>
            {
                {"@flightId", flightId }
            };
            try
            {
                int affectedRows = ExecuteNonQuery(query, parameter);
                return affectedRows > 0;
            }
            catch (MySqlException ex)
            {
                // Re-throw MySqlException để BUS có thể catch với error code
                throw;
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
                    status
                FROM Flights
                WHERE flight_number LIKE @flightNumber
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
                    status
                FROM Flights
                WHERE status = @status
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
        public List<FlightDTO> GetByAircartId(int aircraftId)
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
<<<<<<< Updated upstream
                WHERE route_id = @route_id
=======
                WHERE route_id = @routeId
>>>>>>> Stashed changes
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
        #endregion
        #region Bussiness Logic Methods
        public bool IsFlightNumberExists(string flightNumber, DateTime departureTime, int excluderFlightId = 0)
        {
            string query = @"
                SELECT COUNT(*)
                FROM Flights
                WHERE flight_number = @flightNumber
                AND DATE(departure_time) = DATE(@departureTime)
                AND flight_id != @excluderFlightId";
            var parameters = new Dictionary<string, object>
            {
                {"@flightNumber", flightNumber},
                {"@departureTime", departureTime },
                {"@excluderFlightId", excluderFlightId }
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
        public int GetLastFlightNumberNumeric(string prefix)
        {
            string query = @"
                SELECT MAX(CAST(SUBSTRING(flight_number, LENGTH(@prefix) + 1) AS UNSIGNED))
                FROM Flights
                WHERE flight_number LIKE @prefixPattern";

            var parameters = new Dictionary<string, object>
                            {
                                { "@prefix", prefix },
                                { "@prefixPattern", $"{prefix}%" }
                            };
            try
            {
                object result = ExecuteScalar(query, parameters);

                if (result == DBNull.Value || result == null)
                    return 0;

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy số hiệu chuyến bay cuối: {ex.Message}");
                return 0;
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

        public DataTable GetFlightDetailsForDisplay(
            string? flightNumber = null,
            int? departureAirportId = null,
            int? arrivalAirportId = null,
            DateTime? departureDate = null)
        {
            var queryBuilder = new StringBuilder(@"
                SELECT 
                    f.flight_number AS 'FlightNumber',
                    dep.airport_name AS 'DepartureAirportName',
                    arr.airport_name AS 'ArrivalAirportName',
                    f.departure_time AS 'DepartureTime',
                    f.arrival_time AS 'ArrivalTime',
                    f.status AS 'Status'
                FROM 
                    Flights f
                LEFT JOIN 
                    Routes r ON f.route_id = r.route_id
                LEFT JOIN 
                    Airports dep ON r.departure_place_id = dep.airport_id
                LEFT JOIN 
                    Airports arr ON r.arrival_place_id = arr.airport_id
            ");

            var parameters = new Dictionary<string, object>();
            var whereClauses = new List<string>();

            // 1. Filter Mã chuyến bay (LIKE)
            if (!string.IsNullOrWhiteSpace(flightNumber))
            {
                whereClauses.Add("f.flight_number LIKE @flightNumber");
                parameters["@flightNumber"] = $"%{flightNumber}%";
            }

            // 2. Filter Sân bay đi
            if (departureAirportId.HasValue && departureAirportId > 0)
            {
                whereClauses.Add("r.departure_place_id = @departureAirportId");
                parameters["@departureAirportId"] = departureAirportId.Value;
            }

            // 3. Filter Sân bay đến
            if (arrivalAirportId.HasValue && arrivalAirportId > 0)
            {
                whereClauses.Add("r.arrival_place_id = @arrivalAirportId");
                parameters["@arrivalAirportId"] = arrivalAirportId.Value;
            }

            // 4. Filter Ngày đi (chỉ lọc ngày, bỏ qua giờ)
            if (departureDate.HasValue)
            {
                whereClauses.Add("DATE(f.departure_time) = DATE(@departureDate)");
                parameters["@departureDate"] = departureDate.Value;
            }

            // Nối các điều kiện WHERE
            if (whereClauses.Count > 0)
            {
                queryBuilder.Append(" WHERE " + string.Join(" AND ", whereClauses));
            }

            queryBuilder.Append(" ORDER BY f.departure_time DESC");

            try
            {
                return ExecuteQuery(queryBuilder.ToString(), parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu chuyến bay (JOINED): {ex.Message}", ex);
            }
        }
        public DataTable SearchFlightsForDisplay(
    string? flightNumber,
    int? departureAirportId,
    int? arrivalAirportId,
    DateTime? departureDate,
    int? cabinClassId,
    string? status = null)
        {
            var queryBuilder = new StringBuilder(@"
        SELECT 
            f.flight_id,
            f.flight_number AS 'FlightNumber',
            dep.airport_name AS 'DepartureAirportName',
            arr.airport_name AS 'ArrivalAirportName',
            f.departure_time AS 'DepartureTime',
            f.arrival_time AS 'ArrivalTime',
            f.status AS 'Status'
        FROM 
            Flights f
        LEFT JOIN 
            Routes r ON f.route_id = r.route_id
        LEFT JOIN 
            Airports dep ON r.departure_place_id = dep.airport_id
        LEFT JOIN 
            Airports arr ON r.arrival_place_id = arr.airport_id
    ");

            var parameters = new Dictionary<string, object>();
            var whereClauses = new List<string>();

            // Filter theo Status (nếu có)
            if (!string.IsNullOrWhiteSpace(status))
            {
                whereClauses.Add("f.status = @status");
                parameters["@status"] = status;
            }
            else
            {
                // Mặc định chỉ lấy chuyến bay có thể đặt (nếu không filter status)
                whereClauses.Add("f.status IN ('SCHEDULED', 'DELAYED')");
            }

            // 1. Lọc theo Ngày đi (NẾU CÓ)
            if (departureDate.HasValue)
            {
                // Logic: Lấy từ ngày đã chọn TRỞ ĐI
                whereClauses.Add("DATE(f.departure_time) >= DATE(@departureDate)");
                parameters.Add("@departureDate", departureDate.Value);
            }

            // 2. Lọc theo Nơi cất cánh (NẾU CÓ)
            if (departureAirportId.HasValue && departureAirportId.Value > 0)
            {
                whereClauses.Add("r.departure_place_id = @departureAirportId");
                parameters.Add("@departureAirportId", departureAirportId.Value);
            }

            // 3. Lọc theo Nơi hạ cánh (NẾU CÓ)
            if (arrivalAirportId.HasValue && arrivalAirportId.Value > 0)
            {
                whereClauses.Add("r.arrival_place_id = @arrivalAirportId");
                parameters.Add("@arrivalAirportId", arrivalAirportId.Value);
            }

            // 4. Lọc theo Hạng vé (NẾU CÓ)
            if (cabinClassId.HasValue && cabinClassId.Value > 0)
            {
                whereClauses.Add(@"
            EXISTS (
                SELECT 1
                FROM Flight_Seats fs
                JOIN Seats s ON fs.seat_id = s.seat_id
                WHERE fs.flight_id = f.flight_id
                  AND s.class_id = @cabinClassId
                  AND fs.seat_status = 'AVAILABLE'
            )
        ");
                parameters.Add("@cabinClassId", cabinClassId.Value);
            }

            // 5. Lọc theo Mã chuyến bay (THÊM MỚI)
            if (!string.IsNullOrWhiteSpace(flightNumber))
            {
                whereClauses.Add("f.flight_number LIKE @flightNumber");
                parameters.Add("@flightNumber", $"%{flightNumber}%");
            }

            // Nối các điều kiện WHERE
            if (whereClauses.Count > 0)
            {
                queryBuilder.Append(" WHERE " + string.Join(" AND ", whereClauses));
            }

            // Sắp xếp theo ngày đi sớm nhất
            queryBuilder.Append(" ORDER BY f.departure_time ASC");

            try
            {
                return ExecuteQuery(queryBuilder.ToString(), parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm chuyến bay: {ex.Message}", ex);
            }
        }
        #endregion

    }
}