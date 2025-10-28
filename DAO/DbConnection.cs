using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public static class DbConnection
    {
        private static readonly string connStr =
            "Server=localhost;Database=flight_ticket_db;User ID=root;Password=;";

        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(connStr);
            conn.Open();
            return conn;
        }
    }

}
