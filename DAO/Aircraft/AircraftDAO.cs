using DAO.Database;
using System;
using System.Data;

namespace DAO.Aircraft
{
    public class AircraftDAO : BaseDAO
    {
        #region Singleton Pattern
        private static AircraftDAO _instance;
        private static readonly object _lock = new object();
        private AircraftDAO() { }
        public static AircraftDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new AircraftDAO();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
        public DataTable GetAllAircraftForComboBox()
        {
            string query = @"
                SELECT 
                    a.aircraft_id, 
                    CONCAT(a.model, ' (', a.manufacturer, ')') AS DisplayName,
                    al.airline_code
                FROM 
                    Aircrafts a
                INNER JOIN
                    Airlines al ON a.airline_id = al.airline_id
                ORDER BY 
                    al.airline_code, a.model";
            try
            {
                return ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách máy bay: " + ex.Message, ex);
            }
        }
    }
}
