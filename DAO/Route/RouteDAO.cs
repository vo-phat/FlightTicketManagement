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
            // JOIN với bảng Airports để lấy mã Sân bay đi và đến
            string query = @"
                SELECT 
                    r.route_id, 
                    CONCAT(dep.airport_code, ' → ', arr.airport_code, 
                           ' (', r.distance_km, 'km)') AS DisplayName
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
    }
}