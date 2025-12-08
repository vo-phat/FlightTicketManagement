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

        /// <summary>
        /// Lấy thống kê theo chuyến bay
        /// </summary>
        public DataTable GetFlightStats(int year, int month)
        {
            string query = @"
                SELECT 
                    f.flight_id,
                    f.flight_code,
                    CONCAT(dep.airport_code, ' → ', arr.airport_code) AS Route,
                    DATE_FORMAT(f.departure_time, '%Y-%m-%d %H:%i') AS DepartureTime,
                    DATE_FORMAT(f.arrival_time, '%Y-%m-%d %H:%i') AS ArrivalTime,
                    ac.total_seats AS TotalSeats,
                    COALESCE(COUNT(DISTINCT t.ticket_id), 0) AS BookedSeats,
                    COALESCE(COUNT(DISTINCT t.passenger_id), 0) AS TotalPassengers,
                    COALESCE(SUM(CASE WHEN p.status = 'SUCCESS' THEN p.amount ELSE 0 END), 0) AS Revenue
                FROM Flights f
                JOIN Routes r ON f.route_id = r.route_id
                JOIN Airports dep ON r.departure_place_id = dep.airport_id
                JOIN Airports arr ON r.arrival_place_id = arr.airport_id
                JOIN Aircraft ac ON f.aircraft_id = ac.aircraft_id
                LEFT JOIN Tickets t ON f.flight_id = t.flight_id
                LEFT JOIN Bookings b ON t.booking_id = b.booking_id
                LEFT JOIN Payments p ON b.booking_id = p.booking_id
                WHERE 
                    YEAR(f.departure_time) = @year
                    AND MONTH(f.departure_time) = @month
                    AND f.flight_status NOT IN ('CANCELLED')
                GROUP BY 
                    f.flight_id, f.flight_code, f.departure_time, f.arrival_time, 
                    ac.total_seats, dep.airport_code, arr.airport_code
                ORDER BY 
                    f.departure_time DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@year", year },
                { "@month", month }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi GetFlightStats: {ex.Message}");
                // Return empty table with correct structure
                var emptyResult = new DataTable();
                emptyResult.Columns.Add("flight_id", typeof(int));
                emptyResult.Columns.Add("flight_code", typeof(string));
                emptyResult.Columns.Add("Route", typeof(string));
                emptyResult.Columns.Add("DepartureTime", typeof(string));
                emptyResult.Columns.Add("ArrivalTime", typeof(string));
                emptyResult.Columns.Add("TotalSeats", typeof(int));
                emptyResult.Columns.Add("BookedSeats", typeof(int));
                emptyResult.Columns.Add("TotalPassengers", typeof(int));
                emptyResult.Columns.Add("Revenue", typeof(decimal));
                return emptyResult;
            }
        }

        /// <summary>
        /// Lấy thống kê thanh toán theo phương thức
        /// </summary>
        public DataTable GetPaymentStats(int year, int month)
        {
            string query = @"
                SELECT 
                    p.payment_method AS PaymentMethod,
                    COUNT(*) AS TotalTransactions,
                    SUM(p.amount) AS TotalAmount,
                    SUM(CASE WHEN p.status = 'SUCCESS' THEN 1 ELSE 0 END) AS SuccessCount,
                    SUM(CASE WHEN p.status = 'FAILED' THEN 1 ELSE 0 END) AS FailedCount
                FROM Payments p
                WHERE 
                    YEAR(p.payment_date) = @year
                    AND MONTH(p.payment_date) = @month
                GROUP BY 
                    p.payment_method
                ORDER BY 
                    TotalAmount DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@year", year },
                { "@month", month }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi GetPaymentStats: {ex.Message}");
                var emptyResult = new DataTable();
                emptyResult.Columns.Add("PaymentMethod", typeof(string));
                emptyResult.Columns.Add("TotalTransactions", typeof(int));
                emptyResult.Columns.Add("TotalAmount", typeof(decimal));
                emptyResult.Columns.Add("SuccessCount", typeof(int));
                emptyResult.Columns.Add("FailedCount", typeof(int));
                return emptyResult;
            }
        }

        /// <summary>
        /// Lấy tóm tắt thống kê thanh toán
        /// </summary>
        public void GetPaymentSummary(int year, int month, out decimal totalRevenue, out int totalTransactions, 
            out int successCount, out int failedCount)
        {
            decimal localRevenue = 0;
            int localTransactions = 0;
            int localSuccess = 0;
            int localFailed = 0;

            string query = @"
                SELECT 
                    SUM(CASE WHEN p.status = 'SUCCESS' THEN p.amount ELSE 0 END) AS TotalRevenue,
                    COUNT(*) AS TotalTransactions,
                    SUM(CASE WHEN p.status = 'SUCCESS' THEN 1 ELSE 0 END) AS SuccessCount,
                    SUM(CASE WHEN p.status = 'FAILED' THEN 1 ELSE 0 END) AS FailedCount
                FROM Payments p
                WHERE 
                    YEAR(p.payment_date) = @year
                    AND MONTH(p.payment_date) = @month";

            var parameters = new Dictionary<string, object>
            {
                { "@year", year },
                { "@month", month }
            };

            try
            {
                ExecuteReader(query, reader =>
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("TotalRevenue")))
                            localRevenue = reader.GetDecimal("TotalRevenue");
                        if (!reader.IsDBNull(reader.GetOrdinal("TotalTransactions")))
                            localTransactions = reader.GetInt32("TotalTransactions");
                        if (!reader.IsDBNull(reader.GetOrdinal("SuccessCount")))
                            localSuccess = reader.GetInt32("SuccessCount");
                        if (!reader.IsDBNull(reader.GetOrdinal("FailedCount")))
                            localFailed = reader.GetInt32("FailedCount");
                    }
                }, parameters);

                totalRevenue = localRevenue;
                totalTransactions = localTransactions;
                successCount = localSuccess;
                failedCount = localFailed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi GetPaymentSummary: {ex.Message}");
                totalRevenue = 0;
                totalTransactions = 0;
                successCount = 0;
                failedCount = 0;
            }
        }
    }
}