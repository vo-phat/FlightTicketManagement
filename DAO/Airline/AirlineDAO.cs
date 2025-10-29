using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using DTO.Airline;
using DAO.Database;

namespace DAO.Airline
{
    public class AirlineDAO
    {
        #region Lấy danh sách tất cả hãng hàng không
        public List<AirlineDTO> GetAllAirlines()
        {
            List<AirlineDTO> airlines = new List<AirlineDTO>();
            string query = "SELECT airline_id, airline_code, airline_name, country FROM airlines";
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
                            var airline = new AirlineDTO
                            {
                                AirlineId = reader.GetInt32("airline_id"),
                                AirlineCode = reader.GetString("airline_code"),
                                AirlineName = reader.GetString("airline_name"),
                                Country = reader["country"] == DBNull.Value ? null : reader.GetString("country")
                            };
                            airlines.Add(airline);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hãng hàng không: " + ex.Message, ex);
            }
            return airlines;
        }
        #endregion
        #region Thêm hãng hàng không mới
        public bool InsertAirline(AirlineDTO airline)
        {
            string query = @"INSERT INTO airlines (airline_code, airline_name, country)
                             VALUES (@code, @name, @country)";
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@code", airline.AirlineCode);
                        command.Parameters.AddWithValue("@name", airline.AirlineName);
                        command.Parameters.AddWithValue("@country", (object)airline.Country ?? DBNull.Value);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm hãng hàng không: " + ex.Message, ex);
            }
        }
        #endregion

        #region Cập nhật thông tin hãng hàng không
        public bool UpdateAirline(AirlineDTO airline)
        {
            string query = @"UPDATE airlines 
                             SET airline_code = @code, airline_name = @name, country = @country
                             WHERE airline_id = @id";
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@code", airline.AirlineCode);
                        command.Parameters.AddWithValue("@name", airline.AirlineName);
                        command.Parameters.AddWithValue("@country", (object)airline.Country ?? DBNull.Value);
                        command.Parameters.AddWithValue("@id", airline.AirlineId);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật hãng hàng không: " + ex.Message, ex);
            }
        }
        #endregion

        #region Xóa hãng hàng không theo ID
        public bool DeleteAirline(int airlineId)
        {
            string query = " DELETE FROM airlines WHERE airline.id = @id";
            try
            {
                using ( var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();  
                    using ( var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", airlineId);
                        int row = command.ExecuteNonQuery();
                        return row > 0;
                    }
                }
            }catch(Exception ex)
            {
                throw new Exception("Lỗi khi xóa hãng hàng không :" + ex.Message, ex);

            }
        }
        #endregion

        #region Tìm kiếm hãng hàng không theo tên hoặc mã
        public List<AirlineDTO> SearchAirlines(string keyword)
        {
            List<AirlineDTO> airlines = new List<AirlineDTO>();
            string query = @"SELECT airline_id, airline_code, airline_name, country 
                             FROM airlines 
                             WHERE airline_name LIKE @keyword OR airline_code LIKE @keyword";
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var airline = new AirlineDTO
                                {
                                    AirlineId = reader.GetInt32("airline_id"),
                                    AirlineCode = reader.GetString("airline_code"),
                                    AirlineName = reader.GetString("airline_name"),
                                    Country = reader["country"] == DBNull.Value ? null : reader.GetString("country")
                                };
                                airlines.Add(airline);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm hãng hàng không: " + ex.Message, ex);
            }
            return airlines;
        }
        #endregion
    }
}