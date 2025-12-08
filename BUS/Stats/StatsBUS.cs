using BUS.Common;
using DAO.Stats;
using DTO.Stats;
using System;
using System.Data;

namespace BUS.Stats
{
    public class StatsBUS
    {
        #region Singleton Pattern
        private static StatsBUS _instance;
        private static readonly object _lock = new object();
        private StatsBUS() { }
        public static StatsBUS Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock) { if (_instance == null) _instance = new StatsBUS(); }
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Lấy báo cáo doanh thu chi tiết theo Năm
        /// </summary>
        public BusinessResult GetRevenueReport(int year)
        {
            try
            {
                var report = new RevenueReportViewModel();

                // 1. Lấy dữ liệu tóm tắt
                decimal localTotalRevenue;
                int localTotalTransactions;
                StatsDAO.Instance.GetRevenueSummary(year, out localTotalRevenue, out localTotalTransactions);
                report.TotalRevenue = localTotalRevenue;
                report.TotalTransactions = localTotalTransactions;

                // 2. Lấy breakdown theo tháng
                report.MonthlyBreakdown = StatsDAO.Instance.GetMonthlyRevenue(year);

                // 3. Lấy breakdown theo chuyến bay (top 5)
                report.RouteBreakdown = StatsDAO.Instance.GetRevenueByRoute(year, 5);

                return BusinessResult.SuccessResult("Tải báo cáo thành công.", report);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Lấy thống kê theo chuyến bay đi
        /// </summary>
        public BusinessResult GetFlightStatsReport(int year, int month)
        {
            try
            {
                var dataTable = StatsDAO.Instance.GetFlightStats(year, month);
                var report = new FlightStatsReportViewModel();

                // Nếu không có dữ liệu trong tháng, trả về báo cáo rỗng thay vì lỗi
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    return BusinessResult.SuccessResult("Không có dữ liệu chuyến bay.", report);
                }

                decimal totalRevenue = 0;
                int totalPassengers = 0;
                List<decimal> occupancyRates = new List<decimal>();

                foreach (DataRow row in dataTable.Rows)
                {
                    try
                    {
                        int totalSeats = Convert.ToInt32(row["TotalSeats"]);
                        int bookedSeats = Convert.ToInt32(row["BookedSeats"]);
                        int totalPassengers_row = Convert.ToInt32(row["TotalPassengers"]);
                        decimal revenue = Convert.ToDecimal(row["Revenue"]);
                        
                        decimal occupancyRate = totalSeats > 0 ? (decimal)bookedSeats / totalSeats * 100 : 0;

                        var flightStat = new FlightStatsViewModel
                        {
                            FlightId = Convert.ToInt32(row["flight_id"]),
                            FlightCode = row["flight_code"].ToString() ?? "",
                            Route = row["Route"].ToString() ?? "",
                            DepartureTime = row["DepartureTime"].ToString() ?? "",
                            ArrivalTime = row["ArrivalTime"].ToString() ?? "",
                            TotalSeats = totalSeats,
                            BookedSeats = bookedSeats,
                            OccupancyRate = Math.Round(occupancyRate, 2),
                            TotalPassengers = totalPassengers_row,
                            Revenue = revenue
                        };

                        report.FlightDetails.Add(flightStat);
                        totalRevenue += revenue;
                        totalPassengers += totalPassengers_row;
                        occupancyRates.Add(occupancyRate);
                    }
                    catch (Exception rowEx)
                    {
                        // Skip rows with errors
                        Console.WriteLine($"Lỗi xử lý dòng: {rowEx.Message}");
                        continue;
                    }
                }

                report.TotalFlights = report.FlightDetails.Count;
                report.TotalRevenue = Math.Round(totalRevenue, 2);
                report.TotalPassengers = totalPassengers;
                report.AverageOccupancyRate = occupancyRates.Count > 0 ? Math.Round((decimal)occupancyRates.Average(), 2) : 0;

                return BusinessResult.SuccessResult("Tải báo cáo chuyến bay thành công.", report);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Lấy thống kê thanh toán
        /// </summary>
        public BusinessResult GetPaymentStatsReport(int year, int month)
        {
            try
            {
                var report = new PaymentStatsReportViewModel();

                // Lấy tóm tắt
                decimal totalRevenue;
                int totalTransactions;
                int successCount;
                int failedCount;

                StatsDAO.Instance.GetPaymentSummary(year, month, out totalRevenue, out totalTransactions, 
                    out successCount, out failedCount);

                report.TotalRevenue = totalRevenue;
                report.TotalTransactions = totalTransactions;
                report.SuccessfulTransactions = successCount;
                report.FailedTransactions = failedCount;
                report.SuccessRate = totalTransactions > 0 ? Math.Round((decimal)successCount / totalTransactions * 100, 2) : 0;

                // Lấy chi tiết theo phương thức thanh toán
                var dataTable = StatsDAO.Instance.GetPaymentStats(year, month);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        try
                        {
                            int total = Convert.ToInt32(row["TotalTransactions"]);
                            int success = Convert.ToInt32(row["SuccessCount"]);
                            decimal rate = total > 0 ? (decimal)success / total * 100 : 0;

                            var paymentMethod = new PaymentMethodStatsViewModel
                            {
                                PaymentMethod = row["PaymentMethod"].ToString() ?? "Unknown",
                                TotalTransactions = total,
                                TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                                SuccessCount = success,
                                FailedCount = Convert.ToInt32(row["FailedCount"]),
                                SuccessRate = Math.Round(rate, 2)
                            };

                            report.PaymentMethods.Add(paymentMethod);
                        }
                        catch (Exception rowEx)
                        {
                            Console.WriteLine($"Lỗi xử lý dòng payment: {rowEx.Message}");
                            continue;
                        }
                    }
                }

                return BusinessResult.SuccessResult("Tải báo cáo thanh toán thành công.", report);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Lấy báo cáo doanh thu theo tháng
        /// </summary>
        public BusinessResult GetMonthlyRevenueReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = DAO.Flight.FlightDAO.Instance.GetMonthlyRevenueReport(fromDate, toDate);
                
                if (data == null || data.Rows.Count == 0)
                {
                    return BusinessResult.SuccessResult("Không có dữ liệu báo cáo tháng.", null);
                }
                
                return BusinessResult.SuccessResult("Tải báo cáo tháng thành công.", data);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Lấy thống kê theo hạng vé
        /// </summary>
        public BusinessResult GetCabinClassStatistics(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = DAO.Flight.FlightDAO.Instance.GetCabinClassStatistics(fromDate, toDate);
                
                if (data == null || data.Rows.Count == 0)
                {
                    return BusinessResult.SuccessResult("Không có dữ liệu thống kê hạng vé.", null);
                }
                
                return BusinessResult.SuccessResult("Tải thống kê hạng vé thành công.", data);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }
    }
}