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
                string query = "SELECT checked_id, weight_kg, price, description FROM checked_baggage";

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

            return list;
        }
    }
}
