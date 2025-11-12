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
                    SUM(amount) AS TotalRevenue,
                    COUNT(payment_id) AS TotalTransactions
                FROM 
                    Payments
                WHERE 
                    status = 'SUCCESS' 
                    AND YEAR(payment_date) = @year";

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
                    MONTH(payment_date) AS 'Thang',
                    SUM(amount) AS 'DoanhThu'
                FROM 
                    Payments
                WHERE 
                    status = 'SUCCESS' 
                    AND YEAR(payment_date) = @year
                GROUP BY 
                    MONTH(payment_date)
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
        /// Lấy Top 5 tuyến bay có doanh thu cao nhất theo Năm
        /// </summary>
        public DataTable GetRevenueByRoute(int year, int topN = 5)
        {
            string query = @"
                SELECT 
                    CONCAT(dep.airport_code, ' → ', arr.airport_code) AS TuyenBay,
                    SUM(p.amount) AS DoanhThu
                FROM Payments p
                JOIN Bookings b ON p.booking_id = b.booking_id
                JOIN Booking_Passengers bp ON b.booking_id = bp.booking_id
                JOIN Tickets t ON bp.booking_passenger_id = t.ticket_passenger_id
                JOIN Flight_Seats fs ON t.flight_seat_id = fs.flight_seat_id
                JOIN Flights f ON fs.flight_id = f.flight_id
                JOIN Routes r ON f.route_id = r.route_id
                JOIN Airports dep ON r.departure_place_id = dep.airport_id
                JOIN Airports arr ON r.arrival_place_id = arr.airport_id
                WHERE 
                    p.status = 'SUCCESS'
                    AND YEAR(p.payment_date) = @year
                GROUP BY 
                    TuyenBay
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
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy doanh thu theo tuyến bay (DAO): {ex.Message}", ex);
            }
        }
    }
}