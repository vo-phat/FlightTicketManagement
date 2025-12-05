using DTO.Baggage;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.BaggageDAO
{
    public class CarryOnBaggageDAO
    {
        public List<CarryOnBaggageDTO> GetAll()
        {
            List<CarryOnBaggageDTO> list = new List<CarryOnBaggageDTO>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                SELECT c.carryon_id, c.weight_kg, c.class_id, c.size_limit,
                       c.description, c.is_default,
                       cl.class_name
                FROM carryon_baggage c
                JOIN cabin_classes cl ON c.class_id = cl.class_id
                ORDER BY c.class_id, c.carryon_id;
            ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new CarryOnBaggageDTO
                    {
                        CarryOnId = reader.GetInt32("carryon_id"),
                        WeightKg = reader.GetInt32("weight_kg"),
                        ClassId = reader.GetInt32("class_id"),
                        ClassName = reader.GetString("class_name"),
                        SizeLimit = reader.GetString("size_limit"),
                        Description = reader.GetString("description"),
                        IsDefault = reader.GetBoolean("is_default")
                    });
                }
            }

            return list;
        }

        public bool Insert(CarryOnBaggageDTO dto)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                INSERT INTO carryon_baggage 
                (weight_kg, class_id, size_limit, description, is_default)
                VALUES (@w, @c, @s, @d, @df)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@w", dto.WeightKg);
                cmd.Parameters.AddWithValue("@c", dto.ClassId);
                cmd.Parameters.AddWithValue("@s", dto.SizeLimit);
                cmd.Parameters.AddWithValue("@d", dto.Description);
                cmd.Parameters.AddWithValue("@df", dto.IsDefault);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(CarryOnBaggageDTO dto)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                UPDATE carryon_baggage
                SET weight_kg=@w,
                    class_id=@c,
                    size_limit=@s,
                    description=@d,
                    is_default=@df
                WHERE carryon_id=@id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@w", dto.WeightKg);
                cmd.Parameters.AddWithValue("@c", dto.ClassId);
                cmd.Parameters.AddWithValue("@s", dto.SizeLimit);
                cmd.Parameters.AddWithValue("@d", dto.Description);
                cmd.Parameters.AddWithValue("@df", dto.IsDefault);
                cmd.Parameters.AddWithValue("@id", dto.CarryOnId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = "DELETE FROM carryon_baggage WHERE carryon_id=@id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public void RemoveDefaultForClass(int classId)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "UPDATE carryon_baggage SET is_default = 0 WHERE class_id = @cid";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@cid", classId);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
