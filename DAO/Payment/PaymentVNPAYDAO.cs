using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DAL.Payment
{
    public class PaymentVNPAYDAO
    {
        public string CreatePaymentUrl(DTO.Payment.PaymentVNPAYDTO dto)
        {
            // Ensure values trimmed
            dto.ReturnUrl = dto.ReturnUrl?.Trim() ?? "";
            dto.OrderInfo = dto.OrderInfo?.Trim() ?? "";
            dto.IpAddress = dto.IpAddress?.Trim() ?? "";

            var vnpParams = new SortedList<string, string>
            {
                {"vnp_Version", "2.1.0"},
                {"vnp_Command", "pay"},
                {"vnp_TmnCode", dto.Vnp_TmnCode},
                {"vnp_Amount", dto.Amount.ToString()},
                {"vnp_CreateDate", dto.CreateDate},
                {"vnp_CurrCode", "VND"},
                {"vnp_IpAddr", dto.IpAddress},
                {"vnp_Locale", "vn"},
                {"vnp_OrderInfo", dto.OrderInfo},
                {"vnp_OrderType", "other"},
                {"vnp_ReturnUrl", dto.ReturnUrl},
                {"vnp_TxnRef", dto.TxnRef}
            };

            // Query string for redirect (URL-encode values)
            string query = string.Join("&", vnpParams.Select(kv => $"{kv.Key}={HttpUtility.UrlEncode(kv.Value)}"));

            // Build signData for HMAC: keys sorted, values NOT URL-encoded (common pattern)
            string signData = string.Join("&", vnpParams.Select(kv => $"{kv.Key}={kv.Value}"));

            // Debug (optional) - log signData and secureHash to console or file for verification
            // Console.WriteLine("signData: " + signData);

            string secureHash = HmacSHA512(dto.Vnp_HashSecret, signData);
            Console.WriteLine("SignData: " + signData);
            Console.WriteLine("SecureHash: " + secureHash);
            return $"{dto.Vnp_Url}?{query}&vnp_SecureHash={secureHash}";
        }

        private static string HmacSHA512(string key, string input)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
