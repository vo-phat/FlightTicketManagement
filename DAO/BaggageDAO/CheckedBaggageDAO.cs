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

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                // Query from Baggage table with standard checked baggage weights
                string query = @"
                    SELECT DISTINCT 
                        ROW_NUMBER() OVER (ORDER BY allowed_weight_kg) as checked_id,
                        allowed_weight_kg as weight_kg,
                        fee as price,
                        CONCAT('Hành lý ký gửi ', allowed_weight_kg, ' kg') as description
                    FROM Baggage 
                    WHERE baggage_type = 'CHECKED'
                    ORDER BY allowed_weight_kg";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
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
    }
}
