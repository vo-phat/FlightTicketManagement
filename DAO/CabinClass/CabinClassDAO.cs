using DAO.Database;
using System;
using System.Data;

namespace DAO.CabinClass
{
    public class CabinClassDAO : BaseDAO
    {
        #region Singleton Pattern
        private static CabinClassDAO _instance;
        private static readonly object _lock = new object();
        private CabinClassDAO() { }
        public static CabinClassDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CabinClassDAO();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        // Phương thức mới để lấy tất cả các hạng vé
        public DataTable GetAllCabinClasses()
        {
            string query = @"
                SELECT 
                    class_id, 
                    class_name 
                FROM 
                    Cabin_Classes 
                ORDER BY 
                    class_id";
            try
            {
                return ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hạng vé: " + ex.Message, ex);
            }
        }
    }
}