using System;
using System.Collections.Generic;
using MySqlConnector;
using DTO.FlightSeat;
using DAO.Database;

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
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@flightId", flightId);

                        using (var reader = cmd.ExecuteReader())
                        {
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
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách ghế theo chuyến bay: " + ex.Message, ex);
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
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@flightId", flightId);
                        if (!string.IsNullOrWhiteSpace(status))
                            cmd.Parameters.AddWithValue("@status", status);
                        if (minPrice.HasValue)
                            cmd.Parameters.AddWithValue("@min", minPrice.Value);
                        if (maxPrice.HasValue)
                            cmd.Parameters.AddWithValue("@max", maxPrice.Value);

                        using (var reader = cmd.ExecuteReader())
                        {
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
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lọc ghế theo chuyến bay: " + ex.Message, ex);
            }

            return results;
        }
        #endregion

        #region Cập nhật / Chặn ghế
        public bool UpdateSeatStatus(int flightSeatId, string newStatus)
        {
            string query = @"UPDATE flight_seats 
                             SET seat_status = @status
                             WHERE flight_seat_id = @id";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", newStatus);
                        cmd.Parameters.AddWithValue("@id", flightSeatId);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật trạng thái ghế: " + ex.Message, ex);
            }
        }
        #endregion

        #region Xem sơ đồ ghế (lọc theo chuyến, máy bay, hạng)
        public List<FlightSeatDTO> GetSeatMap(int? flightId, int? aircraftId, int? classId)
        {
            var results = new List<FlightSeatDTO>();
            string query = @"SELECT fs.flight_seat_id, fs.flight_id, fs.seat_id, fs.base_price, fs.seat_status
                             FROM flight_seats fs
                             JOIN seats s ON fs.seat_id = s.seat_id
                             WHERE 1=1";

            if (flightId.HasValue)
                query += " AND fs.flight_id = @flightId";
            if (aircraftId.HasValue)
                query += " AND s.aircraft_id = @aircraftId";
            if (classId.HasValue)
                query += " AND s.class_id = @classId";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        if (flightId.HasValue)
                            cmd.Parameters.AddWithValue("@flightId", flightId.Value);
                        if (aircraftId.HasValue)
                            cmd.Parameters.AddWithValue("@aircraftId", aircraftId.Value);
                        if (classId.HasValue)
                            cmd.Parameters.AddWithValue("@classId", classId.Value);

                        using (var reader = cmd.ExecuteReader())
                        {
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
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xem sơ đồ ghế: " + ex.Message, ex);
            }

            return results;
        }
        #endregion
    }
}
