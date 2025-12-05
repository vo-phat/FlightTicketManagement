using System;
using System.Collections.Generic;
using MySqlConnector;
using DTO.Aircraft;
using DAO.Database;

namespace DAO.Aircraft
{
    public class AircraftDAO
    {
        #region Lấy danh sách tất cả máy bay
        public List<AircraftDTO> GetAllAircrafts()
        {
            List<AircraftDTO> aircrafts = new List<AircraftDTO>();

            string query = @"SELECT aircraft_id, airline_id, model, manufacturer, capacity 
                            FROM aircrafts 
                            ORDER BY aircraft_id";

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
                                reader["airline_id"] == DBNull.Value ? (int?)null : reader.GetInt32("airline_id"),
                                reader["model"] == DBNull.Value ? null : reader.GetString("model"),
                                reader["manufacturer"] == DBNull.Value ? null : reader.GetString("manufacturer"),
                                reader["capacity"] == DBNull.Value ? (int?)null : reader.GetInt32("capacity")
                            );
                            aircrafts.Add(aircraft);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách máy bay: " + ex.Message, ex);
            }

            return aircrafts;
        }
        #endregion

        #region Thêm máy bay mới
        public bool InsertAircraft(AircraftDTO aircraft)
        {
            string query = @"INSERT INTO aircrafts (airline_id, model, manufacturer, capacity)
                             VALUES (@airline_id, @model, @manufacturer, @capacity)";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@airline_id", (object)aircraft.AirlineId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@model", (object)aircraft.Model ?? DBNull.Value);
                        command.Parameters.AddWithValue("@manufacturer", (object)aircraft.Manufacturer ?? DBNull.Value);
                        command.Parameters.AddWithValue("@capacity", (object)aircraft.Capacity ?? DBNull.Value);

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm máy bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Cập nhật thông tin máy bay
        public bool UpdateAircraft(AircraftDTO aircraft)
        {
            string query = @"UPDATE aircrafts
                             SET airline_id = @airline_id,
                                 model = @model,
                                 manufacturer = @manufacturer,
                                 capacity = @capacity
                             WHERE aircraft_id = @id";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@airline_id", (object)aircraft.AirlineId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@model", (object)aircraft.Model ?? DBNull.Value);
                        command.Parameters.AddWithValue("@manufacturer", (object)aircraft.Manufacturer ?? DBNull.Value);
                        command.Parameters.AddWithValue("@capacity", (object)aircraft.Capacity ?? DBNull.Value);
                        command.Parameters.AddWithValue("@id", aircraft.AircraftId);

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật máy bay: " + ex.Message, ex);
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

            string query = @"SELECT aircraft_id, airline_id, model, manufacturer, capacity
                             FROM aircrafts
                             WHERE model LIKE @kw OR manufacturer LIKE @kw
                             ORDER BY aircraft_id";

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
                                    reader["airline_id"] == DBNull.Value ? (int?)null : reader.GetInt32("airline_id"),
                                    reader["model"] == DBNull.Value ? null : reader.GetString("model"),
                                    reader["manufacturer"] == DBNull.Value ? null : reader.GetString("manufacturer"),
                                    reader["capacity"] == DBNull.Value ? (int?)null : reader.GetInt32("capacity")
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
