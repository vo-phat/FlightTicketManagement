using System;
using System.Collections.Generic;
using MySqlConnector;
using DTO.Seat;
using DAO.Database;

namespace DAO.Seat
{
    public class SeatDAO
    {
        #region Lấy danh sách tất cả ghế
        public List<SeatDTO> GetAllSeats()
        {
            var seats = new List<SeatDTO>();
            string query = "SELECT seat_id, aircraft_id, seat_number, class_id FROM seats";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            seats.Add(new SeatDTO(
                                reader.GetInt32("seat_id"),
                                reader.GetInt32("aircraft_id"),
                                reader.GetString("seat_number"),
                                reader.GetInt32("class_id")
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách ghế: " + ex.Message, ex);
            }

            return seats;
        }
        #endregion

        #region Lọc danh sách ghế (theo máy bay, hạng ghế)
        public List<SeatDTO> FilterSeats(int? aircraftId, int? classId)
        {
            var seats = new List<SeatDTO>();
            string query = "SELECT seat_id, aircraft_id, seat_number, class_id FROM seats WHERE 1=1";

            if (aircraftId.HasValue)
                query += " AND aircraft_id = @aircraftId";
            if (classId.HasValue)
                query += " AND class_id = @classId";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        if (aircraftId.HasValue)
                            cmd.Parameters.AddWithValue("@aircraftId", aircraftId.Value);
                        if (classId.HasValue)
                            cmd.Parameters.AddWithValue("@classId", classId.Value);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                seats.Add(new SeatDTO(
                                    reader.GetInt32("seat_id"),
                                    reader.GetInt32("aircraft_id"),
                                    reader.GetString("seat_number"),
                                    reader.GetInt32("class_id")
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lọc danh sách ghế: " + ex.Message, ex);
            }

            return seats;
        }
        #endregion

        #region Tìm kiếm ghế theo số ghế
        public List<SeatDTO> SearchSeats(string keyword)
        {
            var results = new List<SeatDTO>();
            string query = @"SELECT seat_id, aircraft_id, seat_number, class_id 
                             FROM seats 
                             WHERE seat_number LIKE @kw";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new SeatDTO(
                                    reader.GetInt32("seat_id"),
                                    reader.GetInt32("aircraft_id"),
                                    reader.GetString("seat_number"),
                                    reader.GetInt32("class_id")
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm ghế: " + ex.Message, ex);
            }

            return results;
        }
        #endregion

        #region Xem thông tin ghế
        public SeatDTO? GetSeatById(int seatId)
        {
            string query = "SELECT seat_id, aircraft_id, seat_number, class_id FROM seats WHERE seat_id = @id";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", seatId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new SeatDTO(
                                    reader.GetInt32("seat_id"),
                                    reader.GetInt32("aircraft_id"),
                                    reader.GetString("seat_number"),
                                    reader.GetInt32("class_id")
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin ghế: " + ex.Message, ex);
            }

            return null;
        }
        #endregion

        #region Tạo ghế mới
        public bool InsertSeat(SeatDTO seat)
        {
            string query = @"INSERT INTO seats (aircraft_id, seat_number, class_id)
                             VALUES (@aircraft, @number, @class)";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@aircraft", seat.AircraftId);
                        cmd.Parameters.AddWithValue("@number", seat.SeatNumber);
                        cmd.Parameters.AddWithValue("@class", seat.ClassId);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm ghế mới: " + ex.Message, ex);
            }
        }
        #endregion

        #region Sửa thông tin ghế
        public bool UpdateSeat(SeatDTO seat)
        {
            string query = @"UPDATE seats 
                             SET aircraft_id = @aircraft,
                                 seat_number = @number,
                                 class_id = @class
                             WHERE seat_id = @id";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@aircraft", seat.AircraftId);
                        cmd.Parameters.AddWithValue("@number", seat.SeatNumber);
                        cmd.Parameters.AddWithValue("@class", seat.ClassId);
                        cmd.Parameters.AddWithValue("@id", seat.SeatId);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật ghế: " + ex.Message, ex);
            }
        }
        #endregion

        #region Xóa ghế
        public bool DeleteSeat(int seatId)
        {
            string query = "DELETE FROM seats WHERE seat_id = @id";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", seatId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa ghế: " + ex.Message, ex);
            }
        }
        #endregion



        #region Lấy danh sách tất cả ghế kèm chi tiết (hạng ghế, máy bay)
        public List<SeatDTO> GetAllSeatsWithDetails()
        {
            var seats = new List<SeatDTO>();
            string query = @"
                SELECT
                    s.seat_id,
                    s.aircraft_id,
                    s.seat_number,
                    s.class_id,
                    cc.class_name,
                    a.model AS aircraft_model,
                    a.manufacturer AS aircraft_manufacturer
                FROM
                    seats s
                JOIN
                    cabin_classes cc ON s.class_id = cc.class_id
                JOIN
                    aircrafts a ON s.aircraft_id = a.aircraft_id";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Sử dụng constructor 7 tham số mới trong SeatDTO
                            seats.Add(new SeatDTO(
                                reader.GetInt32("seat_id"),
                                reader.GetInt32("aircraft_id"),
                                reader.GetString("seat_number"),
                                reader.GetInt32("class_id"),
                                reader.GetString("class_name"),
                                reader.GetString("aircraft_model"),
                                reader.GetString("aircraft_manufacturer")
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Bạn nên sử dụng một logger thay vì chỉ throw Exception.
                throw new Exception("Lỗi khi lấy danh sách ghế kèm chi tiết: " + ex.Message, ex);
            }

            return seats;
        }
        #endregion
    }
}