namespace DTO.Stats
{
    public class PaymentMethodStatsViewModel
    {
        public string PaymentMethod { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalAmount { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public decimal SuccessRate { get; set; }
    }
}
