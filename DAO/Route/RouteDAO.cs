using DAO.Database;
using System;
using System.Data;

namespace DAO.Route
{
    public class RouteDAO : BaseDAO
    {
        #region Singleton Pattern
        private static RouteDAO _instance;
        private static readonly object _lock = new object();
        private RouteDAO() { }
        public static RouteDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RouteDAO();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Lấy danh sách tuyến bay (ID và Tên tuyến) để dùng cho ComboBox.
        /// </summary>
        public DataTable GetAllRoutesForComboBox()
        {
            string query = @"
                    SELECT 
                        r.route_id, 
                        CONCAT(dep.airport_name, ' (', dep.airport_code, ')',
                               ' → ', 
                               arr.airport_name, ' (', arr.airport_code, ')',
                               ' (', r.distance_km, 'km)') AS DisplayName,
                        r.duration_minutes
                    FROM 
                        Routes r
                    INNER JOIN 
                        Airports dep ON r.departure_place_id = dep.airport_id
                    INNER JOIN 
                        Airports arr ON r.arrival_place_id = arr.airport_id
                    ORDER BY 
                        dep.airport_code, arr.airport_code";
            try
            {
                return ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tuyến bay: " + ex.Message, ex);
            }
        }
<<<<<<< Updated upstream
=======

        public RouteDTO GetRouteById(int routeId)
        {
            string query = "SELECT route_id, departure_place_id, arrival_place_id, distance_km, duration_minutes FROM routes WHERE route_id = @id";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", routeId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new RouteDTO(
                                    reader.GetInt32("route_id"),
                                    reader.GetInt32("departure_place_id"),
                                    reader.GetInt32("arrival_place_id"),
                                    reader["distance_km"] == DBNull.Value ? (int?)null : reader.GetInt32("distance_km"),
                                    reader["duration_minutes"] == DBNull.Value ? (int?)null : reader.GetInt32("duration_minutes")
                                );
                            }
                            else
                            {
                                throw new Exception($"Không tìm thấy tuyến bay với ID {routeId}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy tuyến bay ID {routeId}: {ex.Message}", ex);
            }
        }
        #endregion
>>>>>>> Stashed changes
    }
}