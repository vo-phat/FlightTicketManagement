using System;
using System.Collections.Generic;
using MySqlConnector;
using DTO.FlightSeat;
using DAO.Database;
using System.Linq;

namespace DAO.FlightSeat
{
    public class FlightSeatDAO
    {
        #region Lấy danh sách ghế theo chuyến bay
        public List<FlightSeatDTO> GetSeatsByFlight(int flightId)
        {
            var seats = new List<FlightSeatDTO>();
            string query = @"SELECT flight_seat_id, flight_id, seat_id, base_price, seat_status
                             FROM flight_seats WHERE flight_id = @flightId";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@flightId", flightId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    seats.Add(new FlightSeatDTO(
                        reader.GetInt32("flight_seat_id"),
                        reader.GetInt32("flight_id"),
                        reader.GetInt32("seat_id"),
                        reader.GetDecimal("base_price"),
                        reader.GetString("seat_status")
                    ));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách ghế theo chuyến bay: {ex.Message}", ex);
            }

            return seats;
        }
        #endregion

        #region Lọc ghế theo trạng thái hoặc giá
        public List<FlightSeatDTO> FilterSeats(int flightId, string? status, decimal? minPrice, decimal? maxPrice)
        {
            var results = new List<FlightSeatDTO>();
            string query = @"SELECT flight_seat_id, flight_id, seat_id, base_price, seat_status
                             FROM flight_seats WHERE flight_id = @flightId";

            if (!string.IsNullOrWhiteSpace(status))
                query += " AND seat_status = @status";
            if (minPrice.HasValue)
                query += " AND base_price >= @min";
            if (maxPrice.HasValue)
                query += " AND base_price <= @max";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@flightId", flightId);
                if (!string.IsNullOrWhiteSpace(status))
                    cmd.Parameters.AddWithValue("@status", status);
                if (minPrice.HasValue)
                    cmd.Parameters.AddWithValue("@min", minPrice.Value);
                if (maxPrice.HasValue)
                    cmd.Parameters.AddWithValue("@max", maxPrice.Value);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new FlightSeatDTO(
                        reader.GetInt32("flight_seat_id"),
                        reader.GetInt32("flight_id"),
                        reader.GetInt32("seat_id"),
                        reader.GetDecimal("base_price"),
                        reader.GetString("seat_status")
                    ));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lọc ghế theo chuyến bay: {ex.Message}", ex);
            }

            return results;
        }
        #endregion

        #region Cập nhật trạng thái ghế
        public bool UpdateSeatStatus(int flightSeatId, string newStatus)
        {
            const string query = @"UPDATE flight_seats
                                   SET seat_status = @status
                                   WHERE flight_seat_id = @id";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@id", flightSeatId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật trạng thái ghế: {ex.Message}", ex);
            }
        }
        #endregion

        #region Cập nhật hạng ghế trong bảng Seats
        public bool UpdateSeatClass(int seatId, int newClassId)
        {
            const string query = @"UPDATE seats SET class_id = @classId WHERE seat_id = @seatId";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@classId", newClassId);
                cmd.Parameters.AddWithValue("@seatId", seatId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật hạng ghế trong bảng Seats: {ex.Message}", ex);
            }
        }
        #endregion

        #region Cập nhật giá trong Flight_Seats
        public bool UpdateFlightSeatPrice(int flightSeatId, decimal newPrice)
        {
            const string query = @"UPDATE flight_seats SET base_price = @price WHERE flight_seat_id = @id";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@price", newPrice);
                cmd.Parameters.AddWithValue("@id", flightSeatId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật giá ghế: {ex.Message}", ex);
            }
        }
        #endregion

        public bool UpdateFlightSeat(FlightSeatDTO dto)
        {
            const string checkTicketsQuery = @"
        SELECT COUNT(*) 
        FROM tickets t
        JOIN booking_passengers bp ON t.ticket_passenger_id = bp.booking_passenger_id
        WHERE t.flight_seat_id = @flightSeatId";

            const string updateQuery = @"
        UPDATE flight_seats
        SET 
            flight_id = @flightId,
            seat_id = @seatId,
            base_price = @price,
            seat_status = @status
        WHERE flight_seat_id = @id";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                // ✅ KIỂM TRA: Nếu ghế đã có vé đặt, KHÔNG CHO PHÉP thay đổi seat_id
                using (var checkCmd = new MySqlCommand(checkTicketsQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@flightSeatId", dto.FlightSeatId);
                    int ticketCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (ticketCount > 0)
                    {
                        // Lấy seat_id hiện tại để so sánh
                        const string getCurrentSeatQuery = "SELECT seat_id FROM flight_seats WHERE flight_seat_id = @id";
                        using var getCurrentCmd = new MySqlCommand(getCurrentSeatQuery, conn);
                        getCurrentCmd.Parameters.AddWithValue("@id", dto.FlightSeatId);
                        int currentSeatId = Convert.ToInt32(getCurrentCmd.ExecuteScalar());

                        // Nếu đang cố thay đổi seat_id (tức là đổi sang ghế khác/hạng khác)
                        if (currentSeatId != dto.SeatId)
                        {
                            throw new Exception(
                                $"❌ Không thể thay đổi hạng ghế vì đã có {ticketCount} vé được đặt cho ghế này!\n\n" +
                                "Vui lòng chỉ cập nhật giá hoặc hủy các vé trước khi thay đổi hạng ghế.");
                        }
                    }
                }

                // [DEBUG] In ra giá trị trước khi UPDATE
                System.Diagnostics.Debug.WriteLine($"[BEFORE UPDATE] FlightSeatId: {dto.FlightSeatId}, SeatId: {dto.SeatId}, ClassId: {dto.ClassId}");

                using var cmd = new MySqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@flightId", dto.FlightId);
                cmd.Parameters.AddWithValue("@seatId", dto.SeatId);
                cmd.Parameters.AddWithValue("@price", dto.BasePrice);
                cmd.Parameters.AddWithValue("@status", dto.SeatStatus);
                cmd.Parameters.AddWithValue("@id", dto.FlightSeatId);

                int rowsAffected = cmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"[UPDATE] Rows affected: {rowsAffected}");

                // Nếu cập nhật thành công, lấy lại class_id từ seat_id mới
                if (rowsAffected > 0)
                {
                    const string getClassQuery = @"
                SELECT s.class_id, cc.class_name, s.seat_number
                FROM seats s
                JOIN cabin_classes cc ON s.class_id = cc.class_id
                WHERE s.seat_id = @seatId";

                    using var classCmd = new MySqlCommand(getClassQuery, conn);
                    classCmd.Parameters.AddWithValue("@seatId", dto.SeatId);

                    System.Diagnostics.Debug.WriteLine($"[SELECT] Querying for seat_id: {dto.SeatId}");

                    using var reader = classCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int oldClassId = dto.ClassId;
                        int newClassId = reader.GetInt32("class_id");
                        string newClassName = reader.GetString("class_name");
                        string newSeatNumber = reader.GetString("seat_number");

                        System.Diagnostics.Debug.WriteLine($"[FOUND] Old ClassId: {oldClassId} → New ClassId: {newClassId}");
                        System.Diagnostics.Debug.WriteLine($"[FOUND] New ClassName: {newClassName}, SeatNumber: {newSeatNumber}");

                        // Cập nhật lại class_id trong DTO
                        dto.ClassId = newClassId;
                        dto.ClassName = newClassName;
                        dto.SeatNumber = newSeatNumber;

                        System.Diagnostics.Debug.WriteLine($"[AFTER UPDATE] ClassId in DTO: {dto.ClassId}, ClassName: {dto.ClassName}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[ERROR] Không tìm thấy seat với seat_id = {dto.SeatId}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] UPDATE không ảnh hưởng dòng nào!");
                }

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EXCEPTION] {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[EXCEPTION] StackTrace: {ex.StackTrace}");
                throw new Exception($"Lỗi khi cập nhật thông tin ghế flight_seats: {ex.Message}", ex);
            }
        }

        public List<FlightSeatDTO> GetAllFlightSeats()
        {
            var list = new List<FlightSeatDTO>();
            string query = @"
        SELECT 
            fs.flight_seat_id,
            fs.flight_id,
            f.aircraft_id,
            fs.seat_id,
            s.class_id,
            f.flight_number AS FlightName,
            CONCAT(a.manufacturer, ' ', a.model) AS AircraftName,
            a.capacity AS AircraftCapacity,
            s.seat_number AS SeatNumber,
            c.class_name AS ClassName,
            fs.base_price AS BasePrice,
            fs.seat_status AS SeatStatus
        FROM flight_seats fs
        JOIN flights f ON fs.flight_id = f.flight_id
        JOIN aircrafts a ON f.aircraft_id = a.aircraft_id
        JOIN seats s ON fs.seat_id = s.seat_id
        JOIN cabin_classes c ON s.class_id = c.class_id
        WHERE f.is_deleted = FALSE
        ORDER BY f.flight_number, s.seat_number";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                int rowCount = 0;
                while (reader.Read())
                {
                    rowCount++;
                    var dto = new FlightSeatDTO(
                        reader.GetInt32("flight_seat_id"),
                        reader.GetInt32("flight_id"),
                        reader.GetInt32("aircraft_id"),
                        reader.GetInt32("seat_id"),
                        reader.GetInt32("class_id"),
                        reader.GetDecimal("BasePrice"),
                        reader.GetString("SeatStatus"),
                        reader.GetString("FlightName"),
                        reader.GetString("AircraftName"),
                        reader.GetInt32("AircraftCapacity"),
                        reader.GetString("SeatNumber"),
                        reader.GetString("ClassName")
                    );

                    list.Add(dto);

                    // [DEBUG] Log 5 dòng đầu
                    if (rowCount <= 5)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"[GetAllFlightSeats] Row {rowCount}: " +
                            $"FlightSeatId={dto.FlightSeatId}, " +
                            $"SeatId={dto.SeatId}, " +
                            $"ClassId={dto.ClassId}, " +
                            $"SeatNumber={dto.SeatNumber}, " +
                            $"ClassName={dto.ClassName}"
                        );
                    }
                }

                System.Diagnostics.Debug.WriteLine($"[GetAllFlightSeats] Total rows: {rowCount}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetAllFlightSeats] Exception: {ex.Message}");
                throw new Exception($"Lỗi khi tải danh sách ghế máy bay: {ex.Message}", ex);
            }

            return list;
        }

        #region Xem sơ đồ ghế (lọc theo chuyến, máy bay, hạng)
        public List<FlightSeatDTO> GetSeatMap(int? flightId, int? aircraftId, int? classId)
        {
            var results = new List<FlightSeatDTO>();
            string query = @"
                SELECT fs.flight_seat_id, fs.flight_id, fs.seat_id, s.class_id,
                       fs.base_price, fs.seat_status,
                       s.seat_number, cc.class_name
                FROM flight_seats fs
                JOIN seats s ON fs.seat_id = s.seat_id
                JOIN cabin_classes cc ON s.class_id = cc.class_id
                WHERE 1=1";

            if (flightId.HasValue)
                query += " AND fs.flight_id = @flightId";
            if (aircraftId.HasValue)
                query += " AND s.aircraft_id = @aircraftId";
            if (classId.HasValue)
                query += " AND s.class_id = @classId";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                if (flightId.HasValue)
                    cmd.Parameters.AddWithValue("@flightId", flightId.Value);
                if (aircraftId.HasValue)
                    cmd.Parameters.AddWithValue("@aircraftId", aircraftId.Value);
                if (classId.HasValue)
                    cmd.Parameters.AddWithValue("@classId", classId.Value);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new FlightSeatDTO(
                        reader.GetInt32("flight_seat_id"),
                        reader.GetInt32("flight_id"),
                        reader.GetInt32("seat_id"),
                        reader.GetInt32("class_id"),
                        reader.GetDecimal("base_price"),
                        reader.GetString("seat_status"),
                        string.Empty,
                        reader.GetString("seat_number"),
                        reader.GetString("class_name")
                    ));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xem sơ đồ ghế: {ex.Message}", ex);
            }

            return results;
        }
        #endregion

        #region Kiểm tra ghế còn trống
        /// <summary>
        /// Kiểm tra ghế có còn trống (AVAILABLE) hay không
        /// </summary>
        /// <param name="flightId">Mã chuyến bay</param>
        /// <param name="seatId">Mã ghế</param>
        /// <returns>True nếu ghế còn trống (AVAILABLE), False nếu đã được đặt hoặc bị chặn</returns>
        public bool IsSeatAvailable(int flightId, int seatId)
        {
            const string query = @"
                SELECT seat_status 
                FROM flight_seats 
                WHERE flight_id = @flightId AND seat_id = @seatId";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@flightId", flightId);
                cmd.Parameters.AddWithValue("@seatId", seatId);

                var status = cmd.ExecuteScalar()?.ToString();

                // Ghế chỉ available nếu status = 'AVAILABLE'
                return status == "AVAILABLE";
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra trạng thái ghế: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra ghế theo số ghế (seat_number) có còn trống không
        /// </summary>
        /// <param name="flightId">Mã chuyến bay</param>
        /// <param name="seatNumber">Số ghế (VD: "12A", "5F")</param>
        /// <returns>True nếu ghế còn trống, False nếu đã được đặt</returns>
        public bool IsSeatAvailableBySeatNumber(int flightId, string seatNumber)
        {
            const string query = @"
                SELECT fs.seat_status 
                FROM flight_seats fs
                JOIN seats s ON fs.seat_id = s.seat_id
                WHERE fs.flight_id = @flightId AND s.seat_number = @seatNumber";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@flightId", flightId);
                cmd.Parameters.AddWithValue("@seatNumber", seatNumber.Trim().ToUpper());

                var status = cmd.ExecuteScalar()?.ToString();

                return status == "AVAILABLE";
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra trạng thái ghế theo seat_number: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết ghế để kiểm tra
        /// </summary>
        public FlightSeatDTO? GetFlightSeatInfo(int flightId, int seatId)
        {
            const string query = @"
                SELECT fs.flight_seat_id, fs.flight_id, fs.seat_id, 
                       fs.base_price, fs.seat_status,
                       s.seat_number, s.class_id, cc.class_name
                FROM flight_seats fs
                JOIN seats s ON fs.seat_id = s.seat_id
                JOIN cabin_classes cc ON s.class_id = cc.class_id
                WHERE fs.flight_id = @flightId AND fs.seat_id = @seatId";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@flightId", flightId);
                cmd.Parameters.AddWithValue("@seatId", seatId);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new FlightSeatDTO(
                        reader.GetInt32("flight_seat_id"),
                        reader.GetInt32("flight_id"),
                        reader.GetInt32("seat_id"),
                        reader.GetInt32("class_id"),
                        reader.GetDecimal("base_price"),
                        reader.GetString("seat_status"),
                        string.Empty,
                        reader.GetString("seat_number"),
                        reader.GetString("class_name")
                    );
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin ghế: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy số ghế trống (AVAILABLE) cho chuyến bay theo từng hạng vé
        /// </summary>
        /// <param name="flightId">Mã chuyến bay</param>
        /// <returns>Dictionary với key = class_id, value = số ghế AVAILABLE</returns>
        public Dictionary<int, int> GetAvailableSeatsByClass(int flightId)
        {
            var result = new Dictionary<int, int>();
            
            const string query = @"
                SELECT s.class_id, 
                       COUNT(fs.flight_seat_id) AS available_count
                FROM flight_seats fs
                JOIN seats s ON fs.seat_id = s.seat_id
                WHERE fs.flight_id = @flightId 
                  AND fs.seat_status = 'AVAILABLE'
                GROUP BY s.class_id";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@flightId", flightId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int classId = reader.GetInt32("class_id");
                    int count = reader.GetInt32("available_count");
                    result[classId] = count;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy số ghế trống theo hạng vé: {ex.Message}", ex);
            }

            return result;
        }
        #endregion

        public void ReleaseSeatByTicketId(
           int ticketId,
           MySqlTransaction tran)
        {
            string sql = @"
                UPDATE flight_seats
                SET seat_status = 'AVAILABLE'
                WHERE flight_seat_id = (
                    SELECT flight_seat_id
                    FROM tickets
                    WHERE ticket_id = @tid
                )";

            using var cmd = new MySqlCommand(sql, tran.Connection, tran);
            cmd.Parameters.AddWithValue("@tid", ticketId);
            cmd.ExecuteNonQuery();
        }
    }
}