
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO.Database {
    /// <summary>
    /// Test class để kiểm tra BaseDAO hoạt động
    /// </summary>
    public class BaseDAOTest : BaseDAO {
        /// <summary>
        /// Test kết nối và đếm số bảng
        /// </summary>
        public string TestConnection() {
            try {
                // Test ExecuteScalar - Đếm số bảng trong database
                string query = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'flightticketmanagement'";
                object result = ExecuteScalar(query);

                return $"Kết nối thành công!\nSố bảng trong database: {result}";
            } catch (Exception ex) {
                return $"Lỗi kết nối: {ex.Message}";
            }
        }

        /// <summary>
        /// Test ExecuteQuery với bảng Airlines
        /// </summary>
        public string TestExecuteQuery() {
            try {
                // Test ExecuteQuery - Lấy danh sách Airlines
                string query = "SELECT * FROM Airlines LIMIT 5";
                var dt = ExecuteQuery(query);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Lấy được {dt.Rows.Count} airlines:");
                sb.AppendLine();

                // Hiển thị chi tiết
                foreach (System.Data.DataRow row in dt.Rows) {
                    sb.AppendLine($"- ID: {row["airline_id"]}, Code: {row["airline_code"]}, Name: {row["airline_name"]}");
                }

                return sb.ToString();
            } catch (Exception ex) {
                return $"Lỗi query: {ex.Message}";
            }
        }

        /// <summary>
        /// Test ExecuteReader
        /// </summary>
        public string TestExecuteReader() {
            try {
                string query = "SELECT * FROM Airports LIMIT 5";
                var results = new StringBuilder();
                results.AppendLine("Test ExecuteReader - Airports:");
                results.AppendLine();

                int count = 0;
                ExecuteReader(query, reader => {
                    count++;
                    string code = GetString(reader, "airport_code");
                    string name = GetString(reader, "airport_name");
                    string city = GetString(reader, "city");

                    results.AppendLine($"{count}. [{code}] {name} - {city}");
                });

                results.AppendLine();
                results.AppendLine($"Tổng cộng: {count} airports");

                return results.ToString();
            } catch (Exception ex) {
                return $"Lỗi ExecuteReader: {ex.Message}";
            }
        }

        /// <summary>
        /// Test tất cả các methods
        /// </summary>
        public string TestAll() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== TEST BASE DAO ===");
            sb.AppendLine();

            // Test 1: Connection
            sb.AppendLine("TEST 1: Connection & Scalar");
            sb.AppendLine(TestConnection());
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // Test 2: ExecuteQuery
            sb.AppendLine("TEST 2: ExecuteQuery (DataTable)");
            sb.AppendLine(TestExecuteQuery());
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // Test 3: ExecuteReader
            sb.AppendLine("TEST 3: ExecuteReader (Callback)");
            sb.AppendLine(TestExecuteReader());

            return sb.ToString();
        }
    }
}
