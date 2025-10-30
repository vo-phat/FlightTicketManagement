using DAL.Payment;
using DTO.Payment;
using System;

namespace BUS.Payment
{
    public class PaymentVNPAYBUS
    {
        private readonly PaymentVNPAYDAO _dao = new PaymentVNPAYDAO();

        public string CreatePayment(long amount, string orderInfo)
        {
            var dto = new PaymentVNPAYDTO
            {
                Vnp_TmnCode = "LRPN1ROC",
                Vnp_HashSecret = "P5A5E57P1A1X8OX53ZU714A0LL1CFWWKT",
                Vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
                ReturnUrl = "https://f4d12858ad9f.ngrok-free.app/vnpay_return",
                TxnRef = DateTime.Now.Ticks.ToString(),
                OrderInfo = orderInfo,
                Amount = amount *100,
                IpAddress = "127.0.0.1",
                CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            return _dao.CreatePaymentUrl(dto);
        }
    }
}
