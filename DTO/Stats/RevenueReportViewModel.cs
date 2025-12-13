using System.Data;

namespace DTO.Stats
{
    /// <summary>
    /// DTO/ViewModel để chứa dữ liệu báo cáo doanh thu
    /// </summary>
    public class RevenueReportViewModel
    {
        // 1. Thẻ tóm tắt
        public decimal TotalRevenue { get; set; }
        public int TotalTransactions { get; set; }

        // 2. Dữ liệu cho bảng tháng
        public DataTable MonthlyBreakdown { get; set; }

        // 3. Dữ liệu cho bảng tuyến
        public DataTable RouteBreakdown { get; set; }
    }
}