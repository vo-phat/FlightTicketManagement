using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using DTO.Baggage;
using MySqlConnector;
using System.Collections.Generic;
namespace DAO.BaggageDAO
{
    public class CabinClassDAO
    {
        public List<CabinClassDTO> GetAll()
        {
            List<CabinClassDTO> list = new List<CabinClassDTO>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = "SELECT class_id, class_name, description FROM cabin_classes";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new CabinClassDTO
                    {
                        ClassId = reader.GetInt32("class_id"),
                        ClassName = reader.GetString("class_name"),
                        Description = reader.GetString("description")
                    });
                }
            }

            return list;
        }
    }

}
