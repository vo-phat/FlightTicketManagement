using DAO.Database;
using System;
using System.Data;
using System.Collections.Generic;
using MySqlConnector;

namespace DAO.Stats
{
    public class StatsDAO : BaseDAO
    {
        #region Singleton Pattern
        private static StatsDAO _instance;
        private static readonly object _lock = new object();
        private StatsDAO() { }
        public static StatsDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock) { if (_instance == null) _instance = new StatsDAO(); }
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Lấy dữ liệu tóm tắt (Tổng doanh thu, Tổng giao dịch) theo Năm
        /// </summary>
        public void GetRevenueSummary(int year, out decimal totalRevenue, out int totalTransactions)
        {
            decimal localRevenue = 0;
            int localTransactions = 0;

            string query = @"
                SELECT 
                    SUM(p.amount) AS TotalRevenue,
                    COUNT(p.payment_id) AS TotalTransactions
                FROM 
                    Payments p
                JOIN Bookings b ON p.booking_id = b.booking_id
                WHERE 
                    p.status = 'SUCCESS' 
                    AND b.status IN ('CONFIRMED')
                    AND YEAR(p.payment_date) = @year";

            var parameters = new Dictionary<string, object> { { "@year", year } };

            try
            {
                ExecuteReader(query, reader =>
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("TotalRevenue")))
                        localRevenue = reader.GetDecimal("TotalRevenue");
                    if (!reader.IsDBNull(reader.GetOrdinal("TotalTransactions")))
                        localTransactions = reader.GetInt32("TotalTransactions");
                }, parameters);

                totalRevenue = localRevenue;
                totalTransactions = localTransactions;
            }
            catch (Exception ex)
            {
                totalRevenue = 0;
                totalTransactions = 0;
                throw new Exception($"Lỗi khi lấy tóm tắt doanh thu (DAO): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy doanh thu (chỉ 'SUCCESS') nhóm theo Tháng của 1 Năm
        /// </summary>
        public DataTable GetMonthlyRevenue(int year)
        {
            string query = @"
                SELECT 
                    MONTH(p.payment_date) AS 'Thang',
                    SUM(p.amount) AS 'DoanhThu'
                FROM 
                    Payments p
                JOIN Bookings b ON p.booking_id = b.booking_id
                WHERE 
                    p.status = 'SUCCESS' 
                    AND b.status IN ('CONFIRMED')
                    AND YEAR(p.payment_date) = @year
                GROUP BY 
                    MONTH(p.payment_date)
                ORDER BY 
                    Thang ASC";

            var parameters = new Dictionary<string, object> { { "@year", year } };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy doanh thu hàng tháng (DAO): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy Top N theo account/khách hàng có doanh thu cao nhất (vì chưa có dữ liệu tickets đầy đủ)
        /// </summary>
        public DataTable GetRevenueByRoute(int year, int topN = 5)
        {
            // Query theo account_id vì dữ liệu payments chưa liên kết đến flights qua tickets
            string query = @"
                SELECT 
                    CONCAT('Khách hàng #', a.account_id, ' (', a.email, ')') AS TuyenBay,
                    SUM(p.amount) AS DoanhThu,
                    COUNT(p.payment_id) AS SoChuyenBay
                FROM Payments p
                JOIN Bookings b ON p.booking_id = b.booking_id
                JOIN Accounts a ON b.account_id = a.account_id
                WHERE 
                    p.status = 'SUCCESS'
                    AND b.status = 'CONFIRMED'
                    AND YEAR(p.payment_date) = @year
                GROUP BY 
                    a.account_id, a.email
                ORDER BY 
                    DoanhThu DESC
                LIMIT @limit";

            var parameters = new Dictionary<string, object>
            {
                { "@year", year },
                { "@limit", topN }
            };

            try
            {
                DataTable result = ExecuteQuery(query, parameters);
                
                // Nếu không có dữ liệu, trả về bảng rỗng với cấu trúc đúng
                if (result.Rows.Count == 0)
                {
                    result = new DataTable();
                    result.Columns.Add("TuyenBay", typeof(string));
                    result.Columns.Add("DoanhThu", typeof(decimal));
                    result.Columns.Add("SoChuyenBay", typeof(int));
                }
                
                return result;
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về bảng rỗng
                Console.WriteLine($"Lỗi GetRevenueByRoute: {ex.Message}");
                var emptyResult = new DataTable();
                emptyResult.Columns.Add("TuyenBay", typeof(string));
                emptyResult.Columns.Add("DoanhThu", typeof(decimal));
                emptyResult.Columns.Add("SoChuyenBay", typeof(int));
                return emptyResult;
            }
        }
    }
}