namespace DTO.Payment
{
    public class PaymentVNPAYDTO
    {
        public string Vnp_TmnCode { get; set; }
        public string Vnp_HashSecret { get; set; }
        public string Vnp_Url { get; set; }
        public string ReturnUrl { get; set; }

        public string TxnRef { get; set; }         // Mã giao dịch
        public string OrderInfo { get; set; }      // Thông tin đơn hàng
        public long Amount { get; set; }           // Số tiền (nhân 100)
        public string IpAddress { get; set; }      // IP của người dùng
        public string CreateDate { get; set; }     // yyyyMMddHHmmss
    }
}
