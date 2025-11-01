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
            // Chúng ta tạo một cột 'DisplayName' để hiển thị (Model + Manufacturer)
            string query = @"
                SELECT 
                    aircraft_id, 
                    CONCAT(model, ' (', manufacturer, ')') AS DisplayName
                FROM 
                    Aircrafts 
                ORDER BY 
                    model";
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