using DAO;
using DTO.Baggage;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAO.BaggageDAO
{
    public class CarryBaggageDAO
    {
        public List<CarryOnBaggageDTO> GetAllCarryOnBaggage()
        {
            List<CarryOnBaggageDTO> list = new List<CarryOnBaggageDTO>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT carryon_id, weight_kg, size_limit, description, is_default FROM carryon_baggage";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CarryOnBaggageDTO
                        {
                            CarryOnId = reader.GetInt32("carryon_id"),
                            WeightKg = reader.GetInt32("weight_kg"),
                            SizeLimit = reader.GetString("size_limit"),
                            Description = reader.GetString("description"),
                            IsDefault = reader.GetBoolean("is_default")
                        });
                    }
                }
            }

            return list;
        }
    }
}
