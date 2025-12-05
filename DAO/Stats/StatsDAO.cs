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
        /// Lấy Top N tuyến bay có doanh thu cao nhất
        /// </summary>
        public DataTable GetRevenueByRoute(int year, int topN = 5)
        {
            string query = @"
                SELECT 
                    CONCAT(dep.airport_code, ' → ', arr.airport_code, ' (', dep.city, ' - ', arr.city, ')') AS TuyenBay,
                    SUM(p.amount) AS DoanhThu,
                    COUNT(DISTINCT f.flight_id) AS SoChuyenBay
                FROM Payments p
                JOIN Bookings b ON p.booking_id = b.booking_id
                JOIN Tickets t ON b.booking_id = t.booking_id
                JOIN Flights f ON t.flight_id = f.flight_id
                JOIN Routes r ON f.route_id = r.route_id
                JOIN Airports dep ON r.departure_place_id = dep.airport_id
                JOIN Airports arr ON r.arrival_place_id = arr.airport_id
                WHERE 
                    p.status = 'SUCCESS'
                    AND b.status = 'CONFIRMED'
                    AND YEAR(p.payment_date) = @year
                GROUP BY 
                    r.route_id, dep.airport_code, arr.airport_code, dep.city, arr.city
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