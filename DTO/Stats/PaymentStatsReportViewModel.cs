using System.Collections.Generic;

namespace DTO.Stats
{
    public class PaymentStatsReportViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalTransactions { get; set; }
        public int SuccessfulTransactions { get; set; }
        public int FailedTransactions { get; set; }
        public decimal SuccessRate { get; set; }
        public List<PaymentMethodStatsViewModel> PaymentMethods { get; set; } = new List<PaymentMethodStatsViewModel>();
    }
}
