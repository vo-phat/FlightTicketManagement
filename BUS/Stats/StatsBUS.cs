using BUS.Common;
using DAO.Stats;
using DTO.Stats;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
                var occupancyRates = new List<decimal>();

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
        /// Lấy thống kê thanh toán - Using alternative unfiltered approach
        /// </summary>
        public BusinessResult GetPaymentStatsReport(int year, int month)
        {
            try
            {
                var report = new PaymentStatsReportViewModel();
                
                Console.WriteLine($"=== GetPaymentStatsReport: year={year}, month={month} ===");

                // Try alternative approach: Get all data and filter in-memory
                var allStatsData = StatsDAO.Instance.GetAllPaymentStats();
                var allSummaryData = StatsDAO.Instance.GetAllPaymentSummary();
                
                Console.WriteLine($"Retrieved {allStatsData.Rows.Count} payment stats rows and {allSummaryData.Rows.Count} summary rows from DB.");

                // Filter summary data for the requested year/month
                decimal totalRevenue = 0;
                int totalTransactions = 0;
                int successCount = 0;
                int failedCount = 0;

                foreach (DataRow row in allSummaryData.Rows)
                {
                    try
                    {
                        int rowYear = Convert.ToInt32(row["Year"]);
                        int rowMonth = Convert.ToInt32(row["Month"]);
                        
                        if (rowYear == year && rowMonth == month)
                        {
                            totalRevenue = Convert.ToDecimal(row["TotalRevenue"]);
                            totalTransactions = Convert.ToInt32(row["TotalTransactions"]);
                            successCount = Convert.ToInt32(row["SuccessCount"]);
                            failedCount = Convert.ToInt32(row["FailedCount"]);
                            Console.WriteLine($"Found matching summary: Revenue={totalRevenue}, Transactions={totalTransactions}");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing summary row: {ex.Message}");
                    }
                }

                report.TotalRevenue = totalRevenue;
                report.TotalTransactions = totalTransactions;
                report.SuccessfulTransactions = successCount;
                report.FailedTransactions = failedCount;
                report.SuccessRate = totalTransactions > 0 ? Math.Round((decimal)successCount / totalTransactions * 100, 2) : 0;

                Console.WriteLine($"Summary stats calculated: Total={totalTransactions}, Success={successCount}, Failed={failedCount}, Rate={report.SuccessRate}%");

                // Filter payment method stats for the requested year/month using foreach instead of LINQ
                foreach (DataRow row in allStatsData.Rows)
                {
                    try
                    {
                        int rowYear = Convert.ToInt32(row["Year"]);
                        int rowMonth = Convert.ToInt32(row["Month"]);
                        
                        // Skip rows that don't match our year/month
                        if (rowYear != year || rowMonth != month)
                            continue;

                        int total = Convert.ToInt32(row["TotalTransactions"]);
                        int success = Convert.ToInt32(row["SuccessCount"]);
                        decimal rate = total > 0 ? (decimal)success / total * 100 : 0;

                        var paymentMethod = new PaymentMethodStatsViewModel
                        {
                            PaymentMethod = row["PaymentMethod"]?.ToString() ?? "Unknown",
                            TotalTransactions = total,
                            TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                            SuccessCount = success,
                            FailedCount = Convert.ToInt32(row["FailedCount"]),
                            SuccessRate = Math.Round(rate, 2)
                        };

                        report.PaymentMethods.Add(paymentMethod);
                        Console.WriteLine($"Added payment method: {paymentMethod.PaymentMethod}, Amount={paymentMethod.TotalAmount:N0}");
                    }
                    catch (Exception rowEx)
                    {
                        Console.WriteLine($"Lỗi xử lý dòng payment method: {rowEx.Message}");
                        continue;
                    }
                }

                if (report.PaymentMethods.Count == 0)
                {
                    string message = $"Không có dữ liệu thanh toán cho tháng {month:D2}/{year}. Vui lòng chọn tháng khác (có dữ liệu: 1-6/2024).";
                    Console.WriteLine(message);
                    return BusinessResult.SuccessResult(message, report);  // Return as success with empty data
                }

                Console.WriteLine($"=== Payment Stats Report Complete: {report.PaymentMethods.Count} payment methods ===");
                return BusinessResult.SuccessResult("Tải báo cáo thanh toán thành công.", report);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== EXCEPTION in GetPaymentStatsReport: {ex.Message} ===");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Lấy thống kê thanh toán cho toàn bộ năm - Yearly view
        /// </summary>
        public BusinessResult GetPaymentStatsReportYearly(int year)
        {
            try
            {
                var report = new PaymentStatsReportViewModel();
                
                Console.WriteLine($"=== GetPaymentStatsReportYearly: year={year} ===");

                // Get all payment data
                var allStatsData = StatsDAO.Instance.GetAllPaymentStats();
                var allSummaryData = StatsDAO.Instance.GetAllPaymentSummary();
                
                Console.WriteLine($"Retrieved {allStatsData.Rows.Count} payment stats rows and {allSummaryData.Rows.Count} summary rows from DB.");

                // Filter and aggregate summary data for the requested year (all 12 months)
                decimal totalRevenue = 0;
                int totalTransactions = 0;
                int successCount = 0;
                int failedCount = 0;

                foreach (DataRow row in allSummaryData.Rows)
                {
                    try
                    {
                        int rowYear = Convert.ToInt32(row["Year"]);
                        
                        if (rowYear == year)
                        {
                            totalRevenue += Convert.ToDecimal(row["TotalRevenue"]);
                            totalTransactions += Convert.ToInt32(row["TotalTransactions"]);
                            successCount += Convert.ToInt32(row["SuccessCount"]);
                            failedCount += Convert.ToInt32(row["FailedCount"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing summary row: {ex.Message}");
                    }
                }

                Console.WriteLine($"Yearly summary: Revenue={totalRevenue}, Transactions={totalTransactions}, Success={successCount}, Failed={failedCount}");

                report.TotalRevenue = totalRevenue;
                report.TotalTransactions = totalTransactions;
                report.SuccessfulTransactions = successCount;
                report.FailedTransactions = failedCount;
                report.SuccessRate = totalTransactions > 0 ? Math.Round((decimal)successCount / totalTransactions * 100, 2) : 0;

                // Aggregate payment method stats for the entire year
                // Group by payment_method across all 12 months
                var paymentMethodAggregates = new Dictionary<string, PaymentMethodStatsViewModel>();

                foreach (DataRow row in allStatsData.Rows)
                {
                    try
                    {
                        int rowYear = Convert.ToInt32(row["Year"]);
                        
                        if (rowYear != year)
                            continue;

                        string paymentMethod = row["PaymentMethod"]?.ToString() ?? "Unknown";
                        int total = Convert.ToInt32(row["TotalTransactions"]);
                        decimal amount = Convert.ToDecimal(row["TotalAmount"]);
                        int success = Convert.ToInt32(row["SuccessCount"]);
                        int failed = Convert.ToInt32(row["FailedCount"]);

                        if (!paymentMethodAggregates.ContainsKey(paymentMethod))
                        {
                            paymentMethodAggregates[paymentMethod] = new PaymentMethodStatsViewModel
                            {
                                PaymentMethod = paymentMethod,
                                TotalTransactions = 0,
                                TotalAmount = 0,
                                SuccessCount = 0,
                                FailedCount = 0
                            };
                        }

                        paymentMethodAggregates[paymentMethod].TotalTransactions += total;
                        paymentMethodAggregates[paymentMethod].TotalAmount += amount;
                        paymentMethodAggregates[paymentMethod].SuccessCount += success;
                        paymentMethodAggregates[paymentMethod].FailedCount += failed;

                        Console.WriteLine($"Aggregating {paymentMethod}: +{total} transactions, +{amount:N0} VND");
                    }
                    catch (Exception rowEx)
                    {
                        Console.WriteLine($"Error processing payment stats row: {rowEx.Message}");
                        continue;
                    }
                }

                // Calculate success rate for each payment method and add to report
                foreach (var kvp in paymentMethodAggregates)
                {
                    var method = kvp.Value;
                    method.SuccessRate = method.TotalTransactions > 0 
                        ? Math.Round((decimal)method.SuccessCount / method.TotalTransactions * 100, 2) 
                        : 0;
                    report.PaymentMethods.Add(method);
                }

                Console.WriteLine($"=== Yearly Payment Stats Complete: {report.PaymentMethods.Count} payment methods for year {year} ===");

                if (report.PaymentMethods.Count == 0)
                {
                    string message = $"Không có dữ liệu thanh toán cho năm {year}.";
                    Console.WriteLine(message);
                    return BusinessResult.SuccessResult(message, report);
                }

                return BusinessResult.SuccessResult($"Tải báo cáo thanh toán năm {year} thành công.", report);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== EXCEPTION in GetPaymentStatsReportYearly: {ex.Message} ===");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Lấy thống kê chuyến bay cho toàn bộ năm - Yearly view
        /// </summary>
        public BusinessResult GetFlightStatsReportYearly(int year)
        {
            try
            {
                var report = new FlightStatsReportViewModel();
                
                // Get flight stats for all 12 months of the year
                for (int month = 1; month <= 12; month++)
                {
                    var dataTable = StatsDAO.Instance.GetFlightStats(year, month);
                    
                    if (dataTable == null || dataTable.Rows.Count == 0)
                        continue;
                    
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
                        }
                        catch (Exception rowEx)
                        {
                            Console.WriteLine($"Error processing row: {rowEx.Message}");
                            continue;
                        }
                    }
                }
                
                if (report.FlightDetails.Count == 0)
                {
                    return BusinessResult.SuccessResult($"Không có dữ liệu chuyến bay cho năm {year}.", report);
                }
                
                // Calculate yearly totals
                report.TotalFlights = report.FlightDetails.Count;
                report.TotalRevenue = Math.Round(report.FlightDetails.Sum(f => f.Revenue), 2);
                report.TotalPassengers = report.FlightDetails.Sum(f => f.TotalPassengers);
                report.AverageOccupancyRate = report.FlightDetails.Count > 0 
                    ? Math.Round((decimal)report.FlightDetails.Average(f => f.OccupancyRate), 2) 
                    : 0;

                return BusinessResult.SuccessResult($"Tải báo cáo chuyến bay năm {year} thành công.", report);
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
                var data = StatsDAO.Instance.GetMonthlyRevenueReport(fromDate, toDate);
                
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
                var data = StatsDAO.Instance.GetCabinClassStatistics(fromDate, toDate);
                
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
        
        /// <summary>
        /// Lấy thống kê chuyến bay theo tuyến bay phổ biến nhất
        /// </summary>
        public BusinessResult GetTopRoutesByFlightCount(int year)
        {
            try
            {
                var data = StatsDAO.Instance.GetTopRoutesByFlightCount(year);
                
                if (data == null || data.Rows.Count == 0)
                {
                    return BusinessResult.SuccessResult("Không có dữ liệu thống kê tuyến bay phổ biến.", null);
                }
                
                return BusinessResult.SuccessResult("Tải thống kê tuyến bay phổ biến thành công.", data);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Lấy thống kê chuyến bay theo máy bay được sử dụng nhiều nhất
        /// </summary>
        public BusinessResult GetTopAircraftsByFlightCount(int year)
        {
            try
            {
                var data = StatsDAO.Instance.GetTopAircraftsByFlightCount(year);
                
                if (data == null || data.Rows.Count == 0)
                {
                    return BusinessResult.SuccessResult("Không có dữ liệu thống kê máy bay phổ biến.", null);
                }
                
                return BusinessResult.SuccessResult("Tải thống kê máy bay phổ biến thành công.", data);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }
    }
}