using DTO.Ticket;
using DTO.Ticket;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAO.TicketDAO
{
    public class NationalDAO
    {
        public List<NationalDTO> GetAll()
        {
            var list = new List<NationalDTO>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT national_id, country_name, country_code, phone_code FROM national";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new NationalDTO
                        {
                            NationalId = reader.GetInt32("national_id"),
                            CountryName = reader.GetString("country_name"),
                            CountryCode = reader.GetString("country_code"),
                            PhoneCode = reader.IsDBNull("phone_code") ? "" : reader.GetString("phone_code")
                        });
                    }
                }
            }

            return list;
        }
    }
}
