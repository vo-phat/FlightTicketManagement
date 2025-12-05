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

        #region Cập nhật toàn bộ thông tin ghế trong bảng flight_seats
        public bool UpdateFlightSeat(FlightSeatDTO dto)
        {
            const string query = @"
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

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@flightId", dto.FlightId);
                cmd.Parameters.AddWithValue("@seatId", dto.SeatId);
                cmd.Parameters.AddWithValue("@price", dto.BasePrice);
                cmd.Parameters.AddWithValue("@status", dto.SeatStatus);
                cmd.Parameters.AddWithValue("@id", dto.FlightSeatId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật thông tin ghế flight_seats: {ex.Message}", ex);
            }
        }
        #endregion

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
        ORDER BY f.flight_number, s.seat_number";

            try
            {
                using var conn = DatabaseConnection.GetConnection();
                conn.Open();

                using var cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new FlightSeatDTO(
                        reader.GetInt32("flight_seat_id"),
                        reader.GetInt32("flight_id"),
                        reader.GetInt32("aircraft_id"),
                        reader.GetInt32("seat_id"),
                        reader.GetInt32("class_id"),
                        reader.GetDecimal("BasePrice"),
                        reader.GetString("SeatStatus"),
                        reader.GetString("FlightName"),
                        reader.GetString("AircraftName"),
                        reader.GetInt32("AircraftCapacity"),  // ✅ THÊM capacity
                        reader.GetString("SeatNumber"),
                        reader.GetString("ClassName")
                    ));
                }
            }
            catch (Exception ex)
            {
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
    }
}