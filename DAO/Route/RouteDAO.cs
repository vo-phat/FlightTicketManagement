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

        // Add missing methods for RouteBUS
        public System.Collections.Generic.List<DTO.Route.RouteDTO> GetAllRoutes()
        {
            var dt = GetAllRoutesForComboBox();
            var list = new System.Collections.Generic.List<DTO.Route.RouteDTO>();
            foreach (System.Data.DataRow row in dt.Rows)
            {
                list.Add(new DTO.Route.RouteDTO
                {
                    RouteId = Convert.ToInt32(row["route_id"]),
                    DurationMinutes = row["duration_minutes"] != DBNull.Value ? Convert.ToInt32(row["duration_minutes"]) : (int?)null
                });
            }
            return list;
        }

        public bool InsertRoute(DTO.Route.RouteDTO route)
        {
            // Stub implementation
            return false;
        }

        public bool UpdateRoute(DTO.Route.RouteDTO route)
        {
            // Stub implementation
            return false;
        }

        public bool DeleteRoute(int routeId)
        {
            // Stub implementation
            return false;
        }

        public System.Collections.Generic.List<DTO.Route.RouteDTO> SearchRoutes(string keyword)
        {
            return GetAllRoutes();
        }

        public System.Collections.Generic.Dictionary<int, string> GetRouteDisplayList()
        {
            var dict = new System.Collections.Generic.Dictionary<int, string>();
            var dt = GetAllRoutesForComboBox();
            foreach (System.Data.DataRow row in dt.Rows)
            {
                dict.Add(Convert.ToInt32(row["route_id"]), row["DisplayName"].ToString());
            }
            return dict;
        }

        public DTO.Route.RouteDTO GetRouteById(int routeId)
        {
            // Stub implementation
            return new DTO.Route.RouteDTO { RouteId = routeId };
        }
    }
}
