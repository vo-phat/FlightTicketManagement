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
                    UPPER(p.status) = 'SUCCESS' 
                    AND UPPER(b.status) IN ('CONFIRMED')
                    AND YEAR(p.payment_date) = @year";

            var parameters = new Dictionary<string, object> { { "@year", year } };

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
                    }
                }, parameters);

                totalRevenue = localRevenue;
                totalTransactions = localTransactions;
                Console.WriteLine($"GetRevenueSummary: Revenue={localRevenue}, Transactions={localTransactions}");
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
                    UPPER(p.status) = 'SUCCESS' 
                    AND UPPER(b.status) IN ('CONFIRMED')
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
                FROM Routes r
                JOIN Airports dep ON r.departure_place_id = dep.airport_id
                JOIN Airports arr ON r.arrival_place_id = arr.airport_id
                LEFT JOIN Flights f ON r.route_id = f.route_id AND YEAR(f.departure_time) = @year
                LEFT JOIN Flight_Seats fs ON f.flight_id = fs.flight_id
                LEFT JOIN Tickets t ON fs.flight_seat_id = t.flight_seat_id
                LEFT JOIN Booking_Passengers bp ON t.ticket_passenger_id = bp.booking_passenger_id
                LEFT JOIN Bookings b ON bp.booking_id = b.booking_id
                LEFT JOIN Payments p ON b.booking_id = p.booking_id 
                    AND UPPER(p.status) = 'SUCCESS'
                    AND UPPER(b.status) = 'CONFIRMED'
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
                Console.WriteLine($"GetRevenueByRoute: Found {result.Rows.Count} rows.");
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
                    f.flight_number AS flight_code,
                    CONCAT(dep.airport_code, ' → ', arr.airport_code) AS Route,
                    DATE_FORMAT(f.departure_time, '%Y-%m-%d %H:%i') AS DepartureTime,
                    DATE_FORMAT(f.arrival_time, '%Y-%m-%d %H:%i') AS ArrivalTime,
                    ac.capacity AS TotalSeats,
                    COALESCE(COUNT(DISTINCT CASE WHEN fs.seat_status = 'BOOKED' THEN fs.flight_seat_id END), 0) AS BookedSeats,
                    COALESCE(COUNT(DISTINCT t.ticket_id), 0) AS TotalPassengers,
                    COALESCE(SUM(CASE WHEN UPPER(p.status) = 'SUCCESS' THEN p.amount ELSE 0 END), 0) AS Revenue
                FROM Flights f
                JOIN Routes r ON f.route_id = r.route_id
                JOIN Airports dep ON r.departure_place_id = dep.airport_id
                JOIN Airports arr ON r.arrival_place_id = arr.airport_id
                JOIN Aircrafts ac ON f.aircraft_id = ac.aircraft_id
                LEFT JOIN Flight_Seats fs ON f.flight_id = fs.flight_id
                LEFT JOIN Tickets t ON fs.flight_seat_id = t.flight_seat_id
                LEFT JOIN Booking_Passengers bp ON t.ticket_passenger_id = bp.booking_passenger_id
                LEFT JOIN Bookings b ON bp.booking_id = b.booking_id
                LEFT JOIN Payments p ON b.booking_id = p.booking_id
                WHERE 
                    YEAR(f.departure_time) = @year
                    AND MONTH(f.departure_time) = @month
                    AND UPPER(f.status) NOT IN ('CANCELLED')
                GROUP BY 
                    f.flight_id, f.flight_number, f.departure_time, f.arrival_time, 
                    ac.capacity, dep.airport_code, arr.airport_code
                ORDER BY 
                    f.departure_time DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@year", year },
                { "@month", month }
            };

            try
            {
                var dt = ExecuteQuery(query, parameters);
                Console.WriteLine($"GetFlightStats: Found {dt.Rows.Count} rows (Year={year}, Month={month}).");
                return dt;
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
                    SUM(CASE WHEN UPPER(p.status) = 'SUCCESS' THEN 1 ELSE 0 END) AS SuccessCount,
                    SUM(CASE WHEN UPPER(p.status) = 'FAILED' THEN 1 ELSE 0 END) AS FailedCount
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
                var dt = ExecuteQuery(query, parameters);
                Console.WriteLine($"GetPaymentStats: Found {dt.Rows.Count} rows for {year}-{month:D2}.");
                return dt;
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
                    SUM(CASE WHEN UPPER(p.status) = 'SUCCESS' THEN p.amount ELSE 0 END) AS TotalRevenue,
                    COUNT(*) AS TotalTransactions,
                    SUM(CASE WHEN UPPER(p.status) = 'SUCCESS' THEN 1 ELSE 0 END) AS SuccessCount,
                    SUM(CASE WHEN UPPER(p.status) = 'FAILED' THEN 1 ELSE 0 END) AS FailedCount
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
        
        public DataTable GetMonthlyRevenueReport(DateTime fromDate, DateTime toDate)
        {
            string query = @"
                SELECT 
                    DATE_FORMAT(f.departure_time, '%Y-%m') AS month_year,
                    COUNT(DISTINCT f.flight_id) AS total_flights,
                    COUNT(DISTINCT CASE WHEN f.status = 'COMPLETED' THEN f.flight_id END) AS completed_flights,
                    COUNT(DISTINCT t.ticket_id) AS total_tickets,
                    COUNT(DISTINCT CASE WHEN t.status = 'CONFIRMED' THEN t.ticket_id END) AS confirmed_tickets,
                    COALESCE(SUM(CASE WHEN p.status = 'SUCCESS' THEN p.amount ELSE 0 END), 0) AS total_revenue,
                    COUNT(DISTINCT CASE WHEN p.status = 'SUCCESS' THEN p.payment_id END) AS successful_payments
                FROM Flights f
                LEFT JOIN Flight_Seats fs ON f.flight_id = fs.flight_id
                LEFT JOIN Tickets t ON fs.flight_seat_id = t.flight_seat_id
                LEFT JOIN Booking_Passengers bp ON t.ticket_passenger_id = bp.booking_passenger_id
                LEFT JOIN Bookings b ON bp.booking_id = b.booking_id
                LEFT JOIN Payments p ON b.booking_id = p.booking_id
                WHERE f.departure_time >= @fromDate 
                AND f.departure_time <= @toDate
                GROUP BY month_year
                ORDER BY month_year DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@fromDate", fromDate },
                { "@toDate", toDate }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy báo cáo doanh thu tháng: {ex.Message}", ex);
            }
        }

        public DataTable GetCabinClassStatistics(DateTime fromDate, DateTime toDate)
        {
            string query = @"
                SELECT 
                    cc.class_name AS cabin_class_name,
                    COUNT(DISTINCT t.ticket_id) AS total_tickets,
                    COALESCE(SUM(CASE WHEN p.status = 'SUCCESS' THEN p.amount ELSE 0 END), 0) AS total_revenue,
                    ROUND(
                        (COUNT(DISTINCT CASE WHEN fs.seat_status = 'BOOKED' THEN fs.flight_seat_id END) * 100.0) / 
                        NULLIF(COUNT(DISTINCT fs.flight_seat_id), 0),
                        1
                    ) AS booking_rate
                FROM Cabin_Classes cc
                LEFT JOIN Seats s ON cc.class_id = s.class_id
                LEFT JOIN Flight_Seats fs ON s.seat_id = fs.seat_id
                LEFT JOIN Flights f ON fs.flight_id = f.flight_id
                LEFT JOIN Tickets t ON fs.flight_seat_id = t.flight_seat_id
                LEFT JOIN Booking_Passengers bp ON t.ticket_passenger_id = bp.booking_passenger_id
                LEFT JOIN Bookings b ON bp.booking_id = b.booking_id
                LEFT JOIN Payments p ON b.booking_id = p.booking_id
                WHERE f.departure_time >= @fromDate 
                AND f.departure_time <= @toDate
                AND f.is_deleted = 0
                GROUP BY cc.class_id, cc.class_name
                ORDER BY total_revenue DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@fromDate", fromDate },
                { "@toDate", toDate }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thống kê hạng vé: {ex.Message}", ex);
            }
        }
        
        public DataTable GetTopRoutesByFlightCount(int year, int topN = 10)
        {
            string query = @"
                SELECT 
                    r.route_id,
                    CONCAT(dep.airport_name, ' → ', arr.airport_name) AS route_name,
                    COUNT(f.flight_id) AS flight_count,
                    r.distance_km,
                    r.duration_minutes
                FROM Flights f
                INNER JOIN Routes r ON f.route_id = r.route_id
                INNER JOIN Airports dep ON r.departure_place_id = dep.airport_id
                INNER JOIN Airports arr ON r.arrival_place_id = arr.airport_id
                WHERE YEAR(f.departure_time) = @year
                GROUP BY r.route_id, route_name, r.distance_km, r.duration_minutes
                ORDER BY flight_count DESC
                LIMIT @topN";

            var parameters = new Dictionary<string, object>
            {
                { "@year", year },
                { "@topN", topN }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thống kê tuyến bay phổ biến: {ex.Message}", ex);
            }
        }
        
        public DataTable GetTopAircraftsByFlightCount(int year, int topN = 10)
        {
            string query = @"
                SELECT 
                    a.aircraft_id,
                    a.model,
                    a.manufacturer,
                    COALESCE(al.airline_name, 'N/A') AS airline_name,
                    COUNT(f.flight_id) AS flight_count
                FROM Flights f
                INNER JOIN Aircrafts a ON f.aircraft_id = a.aircraft_id
                LEFT JOIN Airlines al ON a.airline_id = al.airline_id
                WHERE YEAR(f.departure_time) = @year
                GROUP BY a.aircraft_id, a.model, a.manufacturer, al.airline_name
                ORDER BY flight_count DESC
                LIMIT @topN";

            var parameters = new Dictionary<string, object>
            {
                { "@year", year },
                { "@topN", topN }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thống kê máy bay: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả thống kê thanh toán (không lọc theo tháng) - Alternative approach
        /// </summary>
        public DataTable GetAllPaymentStats()
        {
            string query = @"
                SELECT 
                    p.payment_method AS PaymentMethod,
                    COUNT(*) AS TotalTransactions,
                    SUM(p.amount) AS TotalAmount,
                    SUM(CASE WHEN UPPER(p.status) = 'SUCCESS' THEN 1 ELSE 0 END) AS SuccessCount,
                    SUM(CASE WHEN UPPER(p.status) = 'FAILED' THEN 1 ELSE 0 END) AS FailedCount,
                    YEAR(p.payment_date) AS Year,
                    MONTH(p.payment_date) AS Month
                FROM Payments p
                GROUP BY 
                    p.payment_method,
                    YEAR(p.payment_date),
                    MONTH(p.payment_date)
                ORDER BY 
                    Year DESC, Month DESC, TotalAmount DESC";

            try
            {
                var dt = ExecuteQuery(query);
                Console.WriteLine($"GetAllPaymentStats: Found {dt.Rows.Count} total rows across all periods.");
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi GetAllPaymentStats: {ex.Message}");
                var emptyResult = new DataTable();
                emptyResult.Columns.Add("PaymentMethod", typeof(string));
                emptyResult.Columns.Add("TotalTransactions", typeof(int));
                emptyResult.Columns.Add("TotalAmount", typeof(decimal));
                emptyResult.Columns.Add("SuccessCount", typeof(int));
                emptyResult.Columns.Add("FailedCount", typeof(int));
                emptyResult.Columns.Add("Year", typeof(int));
                emptyResult.Columns.Add("Month", typeof(int));
                return emptyResult;
            }
        }

        /// <summary>
        /// Lấy tất cả tóm tắt thống kê thanh toán (không lọc theo tháng) - Alternative approach
        /// </summary>
        public DataTable GetAllPaymentSummary()
        {
            string query = @"
                SELECT 
                    YEAR(p.payment_date) AS Year,
                    MONTH(p.payment_date) AS Month,
                    SUM(CASE WHEN UPPER(p.status) = 'SUCCESS' THEN p.amount ELSE 0 END) AS TotalRevenue,
                    COUNT(*) AS TotalTransactions,
                    SUM(CASE WHEN UPPER(p.status) = 'SUCCESS' THEN 1 ELSE 0 END) AS SuccessCount,
                    SUM(CASE WHEN UPPER(p.status) = 'FAILED' THEN 1 ELSE 0 END) AS FailedCount
                FROM Payments p
                GROUP BY 
                    YEAR(p.payment_date),
                    MONTH(p.payment_date)
                ORDER BY 
                    Year DESC, Month DESC";

            try
            {
                var dt = ExecuteQuery(query);
                Console.WriteLine($"GetAllPaymentSummary: Found {dt.Rows.Count} total periods.");
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi GetAllPaymentSummary: {ex.Message}");
                var emptyResult = new DataTable();
                emptyResult.Columns.Add("Year", typeof(int));
                emptyResult.Columns.Add("Month", typeof(int));
                emptyResult.Columns.Add("TotalRevenue", typeof(decimal));
                emptyResult.Columns.Add("TotalTransactions", typeof(int));
                emptyResult.Columns.Add("SuccessCount", typeof(int));
                emptyResult.Columns.Add("FailedCount", typeof(int));
                return emptyResult;
            }
        }
    }
}
