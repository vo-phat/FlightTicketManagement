using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography; // Cần thiết cho HMACSHA512
using System.Text;
using System.Web; // Cần thiết cho HttpUtility.UrlEncode

namespace DAL.Payment
{
    public class PaymentVNPAYDAO
    {
        // =================================================================
        // ✨ FIX 1: THÊM HÀM HMACSHA512 ĐỂ KHẮC PHỤC LỖI CS0103 (Name does not exist)
        // =================================================================
        /// <summary>
        /// Tính toán HMAC SHA-512 và trả về chuỗi Hexadecimal chữ thường
        /// </summary>
        private string HmacSHA512(string key, string data)
        {
            // Key: HashSecret của VNPAY
            var keyBytes = Encoding.UTF8.GetBytes(key);
            // Data: signData đã sắp xếp
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(dataBytes);

                // Chuyển kết quả hash (byte array) sang chuỗi Hexadecimal 
                // và chuyển sang chữ thường (.ToLower()) là bắt buộc
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public string CreatePaymentUrl(DTO.Payment.PaymentVNPAYDTO dto)
        {
            // Đảm bảo các chuỗi không null và loại bỏ khoảng trắng dư thừa
            dto.ReturnUrl = dto.ReturnUrl?.Trim() ?? "";
            // Đảm bảo OrderInfo không null, mặc dù nó có thể chứa tiếng Việt
            dto.OrderInfo = dto.OrderInfo?.Trim() ?? "";
            dto.IpAddress = dto.IpAddress?.Trim() ?? "";

            // VNPAY yêu cầu các tham số phải được sắp xếp theo Key (Alphabetical order)
            var vnpParams = new SortedList<string, string>
            {
                // Phiên bản và Lệnh giao dịch
                {"vnp_Version", "2.1.0"},
                {"vnp_Command", "pay"},
                
                // Thông tin Merchant
                {"vnp_TmnCode", dto.Vnp_TmnCode},
                
                // Thông tin giao dịch
                {"vnp_Amount", (dto.Amount * 100).ToString()}, // Số tiền phải nhân 100
                {"vnp_CreateDate", dto.CreateDate},
                {"vnp_CurrCode", "VND"},
                {"vnp_IpAddr", dto.IpAddress},
                {"vnp_Locale", "vn"},
                {"vnp_OrderInfo", dto.OrderInfo},
                {"vnp_OrderType", "other"},
                {"vnp_ReturnUrl", dto.ReturnUrl},
                {"vnp_TxnRef", dto.TxnRef} // Mã tham chiếu (mã giao dịch)
            };

            // =================================================================
            // FIX 2: ĐẢM BẢO LOGIC TẠO CHỮ KÝ ĐÚNG
            // =================================================================

            // 1. Build signData: Chuỗi ký tự (key=value&key=value...) 
            // KHÔNG URL encode bất cứ thứ gì trong chuỗi này.
            string signData = string.Join("&", vnpParams.Select(kv => $"{kv.Key}={kv.Value}"));

            // 2. Tạo chữ ký HMAC SHA512 (sử dụng hàm đã thêm)
            string secureHash = HmacSHA512(dto.Vnp_HashSecret, signData);

            // 3. Tạo query string: CHỈ URL encode các GIÁ TRỊ (kv.Value)
            string query = string.Join("&", vnpParams.Select(kv => $"{kv.Key}={HttpUtility.UrlEncode(kv.Value)}"));

            // 4. Trả về URL hoàn chỉnh
            return $"{dto.Vnp_Url}?{query}&vnp_SecureHash={secureHash}";
        }
    }
}