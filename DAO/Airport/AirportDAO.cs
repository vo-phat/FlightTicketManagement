using System;
using System.Collections.Generic;
using MySqlConnector;
using DTO.Airport;
using DAO.Database;

namespace DAO.Airport
{
    public class AirportDAO
    {
        #region Lấy danh sách tất cả sân bay
        public List<AirportDTO> GetAllAirports()
        {
            List<AirportDTO> airports = new List<AirportDTO>();

            string query = "SELECT airport_id, airport_code, airport_name, city, country FROM airports";

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
                            var airport = new AirportDTO(
                                reader.GetInt32("airport_id"),
                                reader.GetString("airport_code"),
                                reader.GetString("airport_name"),
                                reader["city"] == DBNull.Value ? null : reader.GetString("city"),
                                reader["country"] == DBNull.Value ? null : reader.GetString("country")
                            );
                            airports.Add(airport);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sân bay: " + ex.Message, ex);
            }

            return airports;
        }
        #endregion

        #region Thêm sân bay mới
        public bool InsertAirport(AirportDTO airport)
        {
            string query = @"INSERT INTO airports (airport_code, airport_name, city, country)
                             VALUES (@code, @name, @city, @country)";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@code", airport.AirportCode);
                        command.Parameters.AddWithValue("@name", airport.AirportName);
                        command.Parameters.AddWithValue("@city", (object)airport.City ?? DBNull.Value);
                        command.Parameters.AddWithValue("@country", (object)airport.Country ?? DBNull.Value);

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sân bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Cập nhật thông tin sân bay
        public bool UpdateAirport(AirportDTO airport)
        {
            string query = @"UPDATE airports
                             SET airport_code = @code,
                                 airport_name = @name,
                                 city = @city,
                                 country = @country
                             WHERE airport_id = @id";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@code", airport.AirportCode);
                        command.Parameters.AddWithValue("@name", airport.AirportName);
                        command.Parameters.AddWithValue("@city", (object)airport.City ?? DBNull.Value);
                        command.Parameters.AddWithValue("@country", (object)airport.Country ?? DBNull.Value);
                        command.Parameters.AddWithValue("@id", airport.AirportId);

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sân bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Xóa sân bay theo ID
        public bool DeleteAirport(int airportId)
        {
            string query = "DELETE FROM airports WHERE airport_id = @id";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", airportId);

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa sân bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Tìm kiếm sân bay (theo tên hoặc mã)
        public List<AirportDTO> SearchAirports(string keyword)
        {
            List<AirportDTO> results = new List<AirportDTO>();

            string query = @"SELECT airport_id, airport_code, airport_name, city, country 
                             FROM airports
                             WHERE airport_code LIKE @kw OR airport_name LIKE @kw OR city LIKE @kw";

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
                                var airport = new AirportDTO(
                                    reader.GetInt32("airport_id"),
                                    reader.GetString("airport_code"),
                                    reader.GetString("airport_name"),
                                    reader["city"] == DBNull.Value ? null : reader.GetString("city"),
                                    reader["country"] == DBNull.Value ? null : reader.GetString("country")
                                );
                                results.Add(airport);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sân bay: " + ex.Message, ex);
            }

            return results;
        }
        #endregion
    }
}
