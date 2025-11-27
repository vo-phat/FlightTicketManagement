using System;
using System.Collections.Generic;
using MySqlConnector;
using DTO.CabinClass;
using DAO.Database;

namespace DAO.CabinClass
{
    public class CabinClassDAO
    {
        #region Lấy danh sách tất cả hạng ghế
        public List<CabinClassDTO> GetAllCabinClasses()
        {
            List<CabinClassDTO> classes = new List<CabinClassDTO>();
            string query = "SELECT class_id, class_name, description FROM cabin_classes";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cabinClass = new CabinClassDTO(
                                reader.GetInt32("class_id"),
                                reader.GetString("class_name"),
                                reader["description"] == DBNull.Value ? null : reader.GetString("description")
                            );
                            classes.Add(cabinClass);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hạng ghế: " + ex.Message, ex);
            }

            return classes;
        }
        #endregion

        #region Thêm hạng ghế mới
        public bool InsertCabinClass(CabinClassDTO cabinClass)
        {
            string query = @"INSERT INTO cabin_classes (class_name, description)
                             VALUES (@name, @desc)";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", cabinClass.ClassName);
                        command.Parameters.AddWithValue("@desc", (object)cabinClass.Description ?? DBNull.Value);

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm hạng ghế: " + ex.Message, ex);
            }
        }
        #endregion

        #region Cập nhật thông tin hạng ghế
        public bool UpdateCabinClass(CabinClassDTO cabinClass)
        {
            string query = @"UPDATE cabin_classes
                             SET class_name = @name,
                                 description = @desc
                             WHERE class_id = @id";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", cabinClass.ClassName);
                        command.Parameters.AddWithValue("@desc", (object)cabinClass.Description ?? DBNull.Value);
                        command.Parameters.AddWithValue("@id", cabinClass.ClassId);

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật hạng ghế: " + ex.Message, ex);
            }
        }
        #endregion

        #region Xóa hạng ghế theo ID
        public bool DeleteCabinClass(int classId)
        {
            string query = "DELETE FROM cabin_classes WHERE class_id = @id";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", classId);

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa hạng ghế: " + ex.Message, ex);
            }
        }
        #endregion

        #region Tìm kiếm hạng ghế theo tên
        public List<CabinClassDTO> SearchCabinClasses(string keyword)
        {
            List<CabinClassDTO> results = new List<CabinClassDTO>();
            string query = @"SELECT class_id, class_name, description 
                             FROM cabin_classes
                             WHERE class_name LIKE @kw OR description LIKE @kw";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var cabinClass = new CabinClassDTO(
                                    reader.GetInt32("class_id"),
                                    reader.GetString("class_name"),
                                    reader["description"] == DBNull.Value ? null : reader.GetString("description")
                                );
                                results.Add(cabinClass);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm hạng ghế: " + ex.Message, ex);
            }

            return results;
        }
        #endregion
    }
}
