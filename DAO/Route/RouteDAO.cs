using System;
using System.Collections.Generic;
using MySqlConnector;
using DTO.Route;
using DAO.Database;

namespace DAO.Route
{
    public class RouteDAO
    {
        #region Lấy danh sách tất cả tuyến bay
        public List<RouteDTO> GetAllRoutes()
        {
            List<RouteDTO> routes = new List<RouteDTO>();
            string query = "SELECT route_id, departure_place_id, arrival_place_id, distance_km, duration_minutes FROM routes";

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
                            var route = new RouteDTO(
                                reader.GetInt32("route_id"),
                                reader.GetInt32("departure_place_id"),
                                reader.GetInt32("arrival_place_id"),
                                reader["distance_km"] == DBNull.Value ? (int?)null : reader.GetInt32("distance_km"),
                                reader["duration_minutes"] == DBNull.Value ? (int?)null : reader.GetInt32("duration_minutes")
                            );
                            routes.Add(route);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tuyến bay: " + ex.Message, ex);
            }

            return routes;
        }
        #endregion

        #region Thêm tuyến bay mới
        public bool InsertRoute(RouteDTO route)
        {
            string query = @"INSERT INTO routes (departure_place_id, arrival_place_id, distance_km, duration_minutes)
                             VALUES (@dep, @arr, @dist, @dur)";
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@dep", route.DeparturePlaceId);
                        command.Parameters.AddWithValue("@arr", route.ArrivalPlaceId);
                        command.Parameters.AddWithValue("@dist", (object)route.DistanceKm ?? DBNull.Value);
                        command.Parameters.AddWithValue("@dur", (object)route.DurationMinutes ?? DBNull.Value);
                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm tuyến bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Cập nhật thông tin tuyến bay
        public bool UpdateRoute(RouteDTO route)
        {
            string query = @"UPDATE routes
                             SET departure_place_id = @dep,
                                 arrival_place_id = @arr,
                                 distance_km = @dist,
                                 duration_minutes = @dur
                             WHERE route_id = @id";
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@dep", route.DeparturePlaceId);
                        command.Parameters.AddWithValue("@arr", route.ArrivalPlaceId);
                        command.Parameters.AddWithValue("@dist", (object)route.DistanceKm ?? DBNull.Value);
                        command.Parameters.AddWithValue("@dur", (object)route.DurationMinutes ?? DBNull.Value);
                        command.Parameters.AddWithValue("@id", route.RouteId);
                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật tuyến bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Xóa tuyến bay theo ID
        public bool DeleteRoute(int routeId)
        {
            string query = "DELETE FROM routes WHERE route_id = @id";
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", routeId);
                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa tuyến bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Tìm kiếm tuyến bay
        public List<RouteDTO> SearchRoutes(string keyword)
        {
            List<RouteDTO> results = new List<RouteDTO>();
            string query = @"SELECT route_id, departure_place_id, arrival_place_id, distance_km, duration_minutes
                             FROM routes
                             WHERE route_id LIKE @kw
                                OR departure_place_id LIKE @kw
                                OR arrival_place_id LIKE @kw
                                OR distance_km LIKE @kw
                                OR duration_minutes LIKE @kw";
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
                                var route = new RouteDTO(
                                    reader.GetInt32("route_id"),
                                    reader.GetInt32("departure_place_id"),
                                    reader.GetInt32("arrival_place_id"),
                                    reader["distance_km"] == DBNull.Value ? (int?)null : reader.GetInt32("distance_km"),
                                    reader["duration_minutes"] == DBNull.Value ? (int?)null : reader.GetInt32("duration_minutes")
                                );
                                results.Add(route);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm tuyến bay: " + ex.Message, ex);
            }

            return results;
        }
        public Dictionary<int, string> GetRouteDisplayList()
        {
            var routes = new Dictionary<int, string>();

            string query = @"
                SELECT 
                    r.route_id,
                    CONCAT(dep.airport_code, ' → ', arr.airport_code) AS route_name
                FROM routes r
                JOIN airports dep ON r.departure_place_id = dep.airport_id
                JOIN airports arr ON r.arrival_place_id = arr.airport_id
                ORDER BY dep.airport_code, arr.airport_code;
            ";

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("route_id");
                            string name = reader.GetString("route_name");
                            routes[id] = name;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tuyến bay: " + ex.Message, ex);
            }

            return routes;
        }
        #endregion
    }


}
