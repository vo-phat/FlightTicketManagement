using MySqlConnector;
using System;

namespace DAO.Database {
    /// <summary>
    /// Class quản lý kết nối đến MySQL Database
    /// Sử dụng Singleton pattern để đảm bảo chỉ có một connection string
    /// </summary>
    public static class DatabaseConnection {
        // Connection string - chứa thông tin kết nối database
        private static readonly string connectionString =
            "server=localhost;port=3306;user=root;password=;database=flightticketmanagement;SslMode=None;";

        /// <summary>
        /// Lấy Connection String
        /// </summary>
        public static string ConnectionString => connectionString;

        /// <summary>
        /// Tạo và trả về một MySqlConnection mới (chưa mở)
        /// Người gọi chịu trách nhiệm mở và đóng connection
        /// </summary>
        /// <returns>MySqlConnection object</returns>
        public static MySqlConnection GetConnection() {
            try {
                return new MySqlConnection(connectionString);
            } catch (Exception ex) {
                throw new Exception($"Lỗi khi tạo kết nối database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra xem có thể kết nối đến database không
        /// </summary>
        /// <returns>True nếu kết nối thành công, False nếu thất bại</returns>
        public static bool TestConnection() {
            try {
                using (var connection = GetConnection()) {
                    connection.Open();
                    return connection.State == System.Data.ConnectionState.Open;
                }
            } catch {
                return false;
            }
        }

        /// <summary>
        /// Lấy thông tin phiên bản MySQL Server
        /// </summary>
        /// <returns>Version string hoặc thông báo lỗi</returns>
        public static string GetServerVersion() {
            try {
                using (var connection = GetConnection()) {
                    connection.Open();
                    return connection.ServerVersion;
                }
            } catch (Exception ex) {
                return $"Không thể kết nối: {ex.Message}";
            }
        }
    }
}