using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Security.Cryptography; // Đã thêm để sử dụng HMACSHA512

namespace BUS.Payment
{
    public class PaymentVNPAYListener
    {
        private readonly string _prefix;
        private readonly string _hashSecret;
        private HttpListener _listener;

        public event Action<NameValueCollection> OnPaymentReceived; // callback về GUI

        public PaymentVNPAYListener(string prefix, string hashSecret)
        {
            _prefix = prefix; // ví dụ: "http://localhost:5000/vnpay_return/"
            _hashSecret = hashSecret;
        }

        public void Start()
        {
            _listener = new HttpListener();
            // *Lưu ý: Nếu dùng localhost:port, bạn cần chạy cmd với quyền Admin để HttpListener hoạt động
            _listener.Prefixes.Add(_prefix);
            _listener.Start();

            Task.Run(async () =>
            {
                while (_listener.IsListening)
                {
                    try
                    {
                        var context = await _listener.GetContextAsync();
                        HandleRequest(context);
                    }
                    catch { }
                }
            });
        }

        public void Stop()
        {
            _listener?.Stop();
        }

        private void HandleRequest(HttpListenerContext context)
        {
            // Lấy chuỗi truy vấn (query string)
            var query = context.Request.Url.Query;

            // Bỏ dấu '?' ở đầu
            if (query.StartsWith("?"))
            {
                query = query.Substring(1);
            }

            // Phân tích chuỗi truy vấn thô (không decode)
            var rawParams = GetRawQueryParameters(query);

            // Xác thực SecureHash
            string vnpSecureHash = rawParams.ContainsKey("vnp_SecureHash") ? rawParams["vnp_SecureHash"] : "";

            // Xây dựng signData từ các tham số thô (raw) đã sắp xếp
            string signData = BuildSignData(rawParams);

            // Tính toán hash
            string computedHash = HmacSHA512(_hashSecret, signData);

            // Dùng HttpUtility.ParseQueryString để lấy các giá trị đã decode 
            // cho mục đích xử lý logic (như đọc OrderInfo, Amount)
            var decodedParams = HttpUtility.ParseQueryString(context.Request.Url.Query);


            string responseText;
            // So sánh hash (bắt buộc phải IgnoreCase vì VNPAY trả về chữ hoa)
            if (!string.IsNullOrEmpty(vnpSecureHash) && computedHash.Equals(vnpSecureHash, StringComparison.OrdinalIgnoreCase))
            {
                // Kiểm tra vnp_ResponseCode (bắt buộc theo quy tắc VNPAY)
                if (decodedParams["vnp_ResponseCode"] == "00")
                {
                    // Mã 00: Thành công
                    responseText = "<html><body><h1>00</h1><p>Confirm Success</p></body></html>"; // Trả về 00
                    OnPaymentReceived?.Invoke(decodedParams); // gửi dữ liệu đã decode về GUI
                }
                else
                {
                    // Mã khác 00: Thất bại, nhưng chữ ký hợp lệ
                    responseText = $"<html><body><h1>{decodedParams["vnp_ResponseCode"]}</h1><p>Payment Failed</p></body></html>";
                }
            }
            else
            {
                // Sai chữ ký
                responseText = "<html><body><h1>97</h1><p>Invalid Signature</p></body></html>"; // Trả về 97
            }

            // VNPAY yêu cầu trả về response code 
            // Ví dụ: 00 (Thành công), 97 (Sai chữ ký), 99 (Lỗi khác)...
            context.Response.ContentType = "text/html";
            byte[] buffer = Encoding.UTF8.GetBytes(responseText);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        // ----------------------------------------------------------------------------------
        // Sửa hàm BuildSignData để nhận Dictionary<string, string> đã được phân tích thô
        // ----------------------------------------------------------------------------------
        private string BuildSignData(Dictionary<string, string> queryParams)
        {
            // Lấy tất cả các keys và sắp xếp theo Alphabetical order
            var sortedKeys = queryParams.Keys.ToList();
            sortedKeys.Sort(StringComparer.Ordinal);

            var sb = new StringBuilder();
            foreach (var key in sortedKeys)
            {
                // Bỏ vnp_SecureHash khi tạo chuỗi hash
                if (key == "vnp_SecureHash") continue;

                // Các tham số khác có thể cần bỏ qua (như vnp_BankCode nếu không có)
                if (key.StartsWith("vnp_") && string.IsNullOrEmpty(queryParams[key])) continue;

                if (sb.Length > 0) sb.Append("&");

                // Sử dụng giá trị thô (raw value) để đảm bảo khớp với chữ ký gốc
                sb.Append($"{key}={queryParams[key]}");
            }
            return sb.ToString();
        }

        // ----------------------------------------------------------------------------------
        // Hàm tiện ích để phân tích query string mà không decode giá trị
        // ----------------------------------------------------------------------------------
        private Dictionary<string, string> GetRawQueryParameters(string query)
        {
            var parameters = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(query)) return parameters;

            // Tách các cặp key=value
            var pairs = query.Split('&');
            foreach (var pair in pairs)
            {
                // Tách key và value
                var parts = pair.Split(new[] { '=' }, 2);
                if (parts.Length == 2)
                {
                    // Key đã được decode (do URL), Value là RAW (chưa decode)
                    // Chúng ta sử dụng HttpUtility.UrlDecode cho key để chuẩn hóa (Optional nhưng nên có)
                    // và lấy value thô (parts[1])
                    string key = HttpUtility.UrlDecode(parts[0]);
                    string value = parts[1];
                    // Thêm vào Dictionary. Nếu có tham số trùng tên, sẽ dùng cái cuối cùng
                    parameters[key] = value;
                }
            }
            return parameters;
        }


        // Hàm HmacSHA512 (Đã đúng)
        private static string HmacSHA512(string key, string input)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}