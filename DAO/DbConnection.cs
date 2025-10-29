using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace DAO
//{
//    public static class DbConnection
//    {
//        private static readonly string connStr =
//            "Server=localhost;Database=flightticketmanagement;User ID=root;Password=;";

//        public static MySqlConnection GetConnection()
//        {
//            MySqlConnection conn = new MySqlConnection(connStr);
//            conn.Open();
//            return conn;
//        }
//    }

//}
namespace DAO
{
    public static class DbConnection
    {
        private static readonly string connStr =
            "Server=localhost;Database=flightticketmanagement;User ID=root;Password=;";

        public static MySqlConnection GetConnection()
        {
            // Chỉ tạo và trả về đối tượng kết nối.
            // KHÔNG gọi conn.Open() ở đây.
            return new MySqlConnection(connStr);
        }
    }
}
