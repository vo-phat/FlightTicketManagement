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
    }
}