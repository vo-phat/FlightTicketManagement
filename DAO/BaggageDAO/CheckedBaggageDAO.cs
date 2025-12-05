using DTO.Baggage;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.BaggageDAO
{
    public class CheckedBaggageDAO
    {
        public List<CheckedBaggageDTO> GetAll()
        {
            List<CheckedBaggageDTO> list = new List<CheckedBaggageDTO>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM checked_baggage";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new CheckedBaggageDTO
                    {
                        CheckedId = reader.GetInt32("checked_id"),
                        WeightKg = reader.GetInt32("weight_kg"),
                        Price = reader.GetDecimal("price"),
                        Description = reader.GetString("description")
                    });
                }
            }

            // If no data, return standard options
            if (list.Count == 0)
            {
                list.Add(new CheckedBaggageDTO { CheckedId = 1, WeightKg = 15, Price = 0, Description = "Hành lý ký gửi 15 kg (miễn phí)" });
                list.Add(new CheckedBaggageDTO { CheckedId = 2, WeightKg = 20, Price = 200000, Description = "Hành lý ký gửi 20 kg" });
                list.Add(new CheckedBaggageDTO { CheckedId = 3, WeightKg = 23, Price = 300000, Description = "Hành lý ký gửi 23 kg" });
                list.Add(new CheckedBaggageDTO { CheckedId = 4, WeightKg = 30, Price = 500000, Description = "Hành lý ký gửi 30 kg" });
            }

            return list;
        }

        public bool Insert(CheckedBaggageDTO dto)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"INSERT INTO checked_baggage 
                             (weight_kg, price, description)
                             VALUES (@w, @p, @d)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@w", dto.WeightKg);
                cmd.Parameters.AddWithValue("@p", dto.Price);
                cmd.Parameters.AddWithValue("@d", dto.Description);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(CheckedBaggageDTO dto)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"UPDATE checked_baggage
                             SET weight_kg=@w, price=@p, description=@d
                             WHERE checked_id=@id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@w", dto.WeightKg);
                cmd.Parameters.AddWithValue("@p", dto.Price);
                cmd.Parameters.AddWithValue("@d", dto.Description);
                cmd.Parameters.AddWithValue("@id", dto.CheckedId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = "DELETE FROM checked_baggage WHERE checked_id=@id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
