using System;
using System.Collections.Generic;
using MySqlConnector;
using DTO.Aircraft;
using DAO.Database;

namespace DAO.Aircraft
{
    public class AircraftDAO
    {
        #region Lấy danh sách tất cả máy bay Vietnam Airlines
        public List<AircraftDTO> GetAllAircrafts()
        {
            List<AircraftDTO> aircrafts = new List<AircraftDTO>();

            string query = @"SELECT aircraft_id, registration_number, model, manufacturer, capacity, 
                                   manufacture_year, status 
                            FROM aircrafts 
                            ORDER BY registration_number";

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
                            var aircraft = new AircraftDTO(
                                reader.GetInt32("aircraft_id"),
                                reader.GetString("registration_number"),
                                reader["model"] == DBNull.Value ? null : reader.GetString("model"),
                                reader["manufacturer"] == DBNull.Value ? null : reader.GetString("manufacturer"),
                                reader["capacity"] == DBNull.Value ? (int?)null : reader.GetInt32("capacity"),
                                reader["manufacture_year"] == DBNull.Value ? (int?)null : reader.GetInt32("manufacture_year"),
                                reader["status"] == DBNull.Value ? "ACTIVE" : reader.GetString("status")
                            );
                            aircrafts.Add(aircraft);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách máy bay Vietnam Airlines: " + ex.Message, ex);
            }

            return aircrafts;
        }
        #endregion

        #region Thêm máy bay mới Vietnam Airlines
        public bool InsertAircraft(AircraftDTO aircraft)
        {
            string query = @"INSERT INTO aircrafts (registration_number, model, manufacturer, capacity, manufacture_year, status)
                             VALUES (@registration_number, @model, @manufacturer, @capacity, @manufacture_year, @status)";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@registration_number", aircraft.RegistrationNumber);
                        command.Parameters.AddWithValue("@model", (object)aircraft.Model ?? DBNull.Value);
                        command.Parameters.AddWithValue("@manufacturer", (object)aircraft.Manufacturer ?? DBNull.Value);
                        command.Parameters.AddWithValue("@capacity", (object)aircraft.Capacity ?? DBNull.Value);
                        command.Parameters.AddWithValue("@manufacture_year", (object)aircraft.ManufactureYear ?? DBNull.Value);
                        command.Parameters.AddWithValue("@status", aircraft.Status);

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm máy bay Vietnam Airlines: " + ex.Message, ex);
            }
        }
        #endregion

        #region Cập nhật thông tin máy bay Vietnam Airlines
        public bool UpdateAircraft(AircraftDTO aircraft)
        {
            string query = @"UPDATE aircrafts
                             SET registration_number = @registration_number,
                                 model = @model,
                                 manufacturer = @manufacturer,
                                 capacity = @capacity,
                                 manufacture_year = @manufacture_year,
                                 status = @status
                             WHERE aircraft_id = @id";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@registration_number", aircraft.RegistrationNumber);
                        command.Parameters.AddWithValue("@model", (object)aircraft.Model ?? DBNull.Value);
                        command.Parameters.AddWithValue("@manufacturer", (object)aircraft.Manufacturer ?? DBNull.Value);
                        command.Parameters.AddWithValue("@capacity", (object)aircraft.Capacity ?? DBNull.Value);
                        command.Parameters.AddWithValue("@manufacture_year", (object)aircraft.ManufactureYear ?? DBNull.Value);
                        command.Parameters.AddWithValue("@status", aircraft.Status);
                        command.Parameters.AddWithValue("@id", aircraft.AircraftId);

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật máy bay Vietnam Airlines: " + ex.Message, ex);
            }
        }
        #endregion

        #region Xóa máy bay theo ID
        public bool DeleteAircraft(int aircraftId)
        {
            string query = "DELETE FROM aircrafts WHERE aircraft_id = @id";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", aircraftId);

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa máy bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Tìm kiếm máy bay Vietnam Airlines (theo registration, model, manufacturer)
        public List<AircraftDTO> SearchAircrafts(string keyword)
        {
            List<AircraftDTO> results = new List<AircraftDTO>();

            string query = @"SELECT aircraft_id, registration_number, model, manufacturer, capacity, manufacture_year, status
                             FROM aircrafts
                             WHERE registration_number LIKE @kw OR model LIKE @kw OR manufacturer LIKE @kw
                             ORDER BY registration_number";

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
                                var aircraft = new AircraftDTO(
                                    reader.GetInt32("aircraft_id"),
                                    reader.GetString("registration_number"),
                                    reader["model"] == DBNull.Value ? null : reader.GetString("model"),
                                    reader["manufacturer"] == DBNull.Value ? null : reader.GetString("manufacturer"),
                                    reader["capacity"] == DBNull.Value ? (int?)null : reader.GetInt32("capacity"),
                                    reader["manufacture_year"] == DBNull.Value ? (int?)null : reader.GetInt32("manufacture_year"),
                                    reader["status"] == DBNull.Value ? "ACTIVE" : reader.GetString("status")
                                );
                                results.Add(aircraft);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm máy bay: " + ex.Message, ex);
            }

            return results;
        }
        #endregion
    }
}
