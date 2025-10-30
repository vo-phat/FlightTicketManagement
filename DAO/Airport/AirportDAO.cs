using DAO.Database;
using System;
using System.Data;

namespace DAO.Airport
{
    public class AirportDAO : BaseDAO
    {
        #region Singleton Pattern
        private static AirportDAO _instance;
        private static readonly object _lock = new object();
        private AirportDAO() { }
        public static AirportDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new AirportDAO();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Lấy danh sách sân bay (ID và Tên hiển thị) để dùng cho ComboBox.
        /// </summary>
        public DataTable GetAllAirportsForComboBox()
        {
            // Chúng ta tạo một cột 'DisplayName' để hiển thị (Tên + Code) cho dễ nhìn
            string query = @"
                SELECT 
                    airport_id, 
                    CONCAT(airport_name, ' (', airport_code, ')') AS DisplayName
                FROM 
                    Airports 
                ORDER BY 
                    airport_name";
            try
            {
                // Dùng phương thức ExecuteQuery từ BaseDAO
                return ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sân bay: " + ex.Message, ex);
            }
        }
    }
}