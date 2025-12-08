using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Seat;
namespace DAO.Seat
{
    public class OpenSeatSelectorDAO
    {
        public List<SeatSelectDTO> GetOpenSeats(int flightId, int classId)
        {
            var list = new List<SeatSelectDTO>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                SELECT 
                    fs.flight_seat_id,
                    s.seat_number,
                    fs.base_price,
                    s.class_id,
                    fs.seat_status
                FROM flight_seats fs
                JOIN seats s ON fs.seat_id = s.seat_id
                WHERE fs.flight_id = @flightId;
            ";



                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@flightId", flightId);
                    cmd.Parameters.AddWithValue("@classId", classId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new SeatSelectDTO
                            {
                                FlightSeatId = reader.GetInt32("flight_seat_id"),
                                SeatNumber = reader.GetString("seat_number"),
                                Price = reader.GetDecimal("base_price"),
                                ClassId = reader.GetInt32("class_id"),
                                SeatStatus = reader.GetString("seat_status")   // thêm
                            });

                        }
                    }
                }
            }

            return list;
        }
    }
}
