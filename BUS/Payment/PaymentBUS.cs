using System;
using System.Collections.Generic;
using System.Linq;
using DAO.Payment;
using DTO.Payment;

namespace BUS.Payment
{
    public class PaymentBUS
    {
        private readonly PaymentDAO _paymentDAO;

        // Constants trạng thái để tránh hardcode string (Nên đưa vào class Constant chung)
        private const string STATUS_PENDING = "PENDING";
        private const string STATUS_CONFIRMED = "CONFIRMED";
        private const string STATUS_SUCCESS = "SUCCESS";
        private const string STATUS_FAILED = "FAILED";

        public PaymentBUS()
        {
            _paymentDAO = new PaymentDAO();
        }

        #region 📋 Lấy danh sách tất cả thanh toán
        public List<PaymentDTO> GetAllPayments()
        {
            try
            {
                return _paymentDAO.GetAllPayments();
            }
            catch (Exception ex)
            {
                throw new Exception("BUS: Lỗi khi lấy danh sách thanh toán - " + ex.Message, ex);
            }
        }

        public List<PaymentDetailDTO> GetAllPaymentsWithDetails()
        {
            try
            {
                return _paymentDAO.GetAllPaymentsWithDetails();
            }
            catch (Exception ex)
            {
                throw new Exception("BUS: Lỗi khi lấy danh sách thanh toán chi tiết - " + ex.Message, ex);
            }
        }
        #endregion

        #region 🧾 Lấy danh sách Bookings có trạng thái PENDING
        public List<PaymentDetailDTO> GetPendingBookingsPayments()
        {
            try
            {
                return _paymentDAO.GetPendingBookingsPayments();
            }
            catch (Exception ex)
            {
                throw new Exception("BUS: Lỗi khi lấy danh sách Booking Pending - " + ex.Message, ex);
            }
        }
        #endregion

        #region 🔍 Xem chi tiết Payment
        public PaymentDetailDTO GetPaymentDetail(int paymentId)
        {
            try
            {
                if (paymentId <= 0)
                    throw new ArgumentException("Payment ID không hợp lệ");

                var payment = _paymentDAO.GetPaymentById(paymentId);
                if (payment == null)
                    throw new Exception($"Không tìm thấy payment với ID {paymentId}");

                return payment;
            }
            catch (Exception ex)
            {
                throw new Exception("BUS: Lỗi khi lấy chi tiết thanh toán - " + ex.Message, ex);
            }
        }
        #endregion

        #region 💳 Xử lý Thanh toán (Payment Processing)
        /// <summary>
        /// Xử lý thanh toán: Chỉ cho phép khi Booking CONFIRMED và Payment PENDING.
        /// Chuyển trạng thái Payment -> SUCCESS.
        /// </summary>
        public bool ProcessPayment(int paymentId, out string message)
        {
            message = string.Empty;

            try
            {
                // Bước 1: Lấy thông tin payment hiện tại
                var payment = _paymentDAO.GetPaymentById(paymentId);
                if (payment == null)
                {
                    message = "Không tìm thấy thông tin thanh toán.";
                    return false;
                }

                // Bước 2: Kiểm tra điều kiện nghiệp vụ
                // 2.1. Thanh toán phải đang ở trạng thái PENDING
                if (!payment.Status.Equals(STATUS_PENDING, StringComparison.OrdinalIgnoreCase))
                {
                    message = $"Không thể xử lý. Thanh toán đang ở trạng thái '{payment.Status}'.";
                    return false;
                }

                // 2.2. Booking phải ĐÃ ĐƯỢC XÁC NHẬN (CONFIRMED) thì mới thu tiền
                if (!payment.BookingStatus.Equals(STATUS_CONFIRMED, StringComparison.OrdinalIgnoreCase))
                {
                    message = $"Không thể thanh toán. Booking chưa được xác nhận (Trạng thái hiện tại: {payment.BookingStatus}).";
                    return false;
                }

                // Bước 3: Kiểm tra số tiền (Cảnh báo nếu không khớp, nhưng không chặn)
                if (payment.Amount != payment.BookingTotalAmount)
                {
                    // Logic tùy chọn: Có thể log warning hoặc trả về message cảnh báo kèm theo
                    // message = $"Lưu ý: Số tiền thanh toán ({payment.Amount:N0}) khác tổng tiền Booking ({payment.BookingTotalAmount:N0}).";
                }

                // Bước 4: Gọi Payment Gateway (Giả lập)
                bool gatewayResult = MockPaymentGateway(payment.Amount);
                if (!gatewayResult)
                {
                    message = "Giao dịch bị từ chối bởi cổng thanh toán.";
                    // Cập nhật trạng thái FAILED
                    _paymentDAO.UpdatePaymentStatus(paymentId, STATUS_FAILED);
                    return false;
                }

                // Bước 5: Gọi DAO để cập nhật trạng thái trong Database
                // Lưu ý: Hàm DAO mới chỉ cần paymentId
                bool result = _paymentDAO.ProcessPayment(paymentId);

                if (result)
                {
                    message = $"✅ Thanh toán thành công!\n" +
                              $"   - Payment ID: {paymentId}\n" +
                              $"   - Số tiền: {payment.Amount:N0} VND";
                    return true;
                }
                else
                {
                    // Trường hợp này xảy ra nếu giữa lúc check ở Bước 2 và lúc update ở Bước 5, 
                    // trạng thái booking hoặc payment đã bị thay đổi bởi người khác.
                    message = "Lỗi: Dữ liệu đã thay đổi trong quá trình xử lý. Vui lòng làm mới và thử lại.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi hệ thống khi xử lý thanh toán - " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Giả lập cổng thanh toán
        /// </summary>
        private bool MockPaymentGateway(decimal amount)
        {
            if (amount <= 0) return false;
            // Tỷ lệ thành công 95%
            return new Random().Next(0, 100) < 95;
        }
        #endregion

        #region ➕ Thêm Payment mới
        public bool AddPayment(PaymentDTO payment, out string message)
        {
            message = string.Empty;
            try
            {
                if (!payment.IsValid(out string validationError))
                {
                    message = "Dữ liệu không hợp lệ: " + validationError;
                    return false;
                }

                bool result = _paymentDAO.InsertPayment(payment);
                if (result)
                {
                    message = "Thêm thanh toán thành công!";
                    return true;
                }
                else
                {
                    message = "Không thể thêm thanh toán vào cơ sở dữ liệu.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi khi thêm payment - " + ex.Message;
                return false;
            }
        }
        #endregion

        #region ✏️ Cập nhật Payment
        public bool UpdatePayment(PaymentDTO payment, out string message)
        {
            message = string.Empty;
            try
            {
                if (!payment.IsValid(out string validationError))
                {
                    message = "Dữ liệu không hợp lệ: " + validationError;
                    return false;
                }

                var existing = _paymentDAO.GetPaymentById(payment.PaymentId);
                if (existing == null)
                {
                    message = "Không tìm thấy thanh toán cần cập nhật.";
                    return false;
                }

                if (existing.Status.Equals(STATUS_SUCCESS, StringComparison.OrdinalIgnoreCase))
                {
                    message = "Không thể sửa đổi thanh toán đã thành công!";
                    return false;
                }

                bool result = _paymentDAO.UpdatePayment(payment);
                if (result)
                {
                    message = "Cập nhật thành công!";
                    return true;
                }
                else
                {
                    message = "Không thể cập nhật.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi khi cập nhật payment - " + ex.Message;
                return false;
            }
        }
        #endregion

        #region ❌ Xóa Payment
        public bool DeletePayment(int paymentId, out string message)
        {
            message = string.Empty;
            try
            {
                if (paymentId <= 0)
                {
                    message = "ID không hợp lệ.";
                    return false;
                }

                var payment = _paymentDAO.GetPaymentById(paymentId);
                if (payment == null)
                {
                    message = "Không tìm thấy dữ liệu.";
                    return false;
                }

                // Logic bảo vệ dữ liệu:
                // 1. Không xóa payment đã thành công
                if (payment.Status.Equals(STATUS_SUCCESS, StringComparison.OrdinalIgnoreCase))
                {
                    message = "Không thể xóa thanh toán đã thành công!";
                    return false;
                }

                // 2. Không xóa payment của Booking đã Confirmed (vì Booking này đã giữ chỗ)
                if (payment.BookingStatus.Equals(STATUS_CONFIRMED, StringComparison.OrdinalIgnoreCase))
                {
                    message = "Không thể xóa thanh toán của Booking đã xác nhận!";
                    return false;
                }

                bool result = _paymentDAO.DeletePayment(paymentId);
                if (result)
                {
                    message = "Xóa thành công!";
                    return true;
                }
                else
                {
                    message = "Không thể xóa dữ liệu.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi khi xóa payment - " + ex.Message;
                return false;
            }
        }
        #endregion

        #region 🔍 Tìm kiếm Payment
        public List<PaymentDetailDTO> SearchPayments(string keyword, out string message)
        {
            message = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    message = "Vui lòng nhập từ khóa.";
                    return new List<PaymentDetailDTO>();
                }

                var results = _paymentDAO.SearchPayments(keyword.Trim());
                message = results.Count > 0 ? $"Tìm thấy {results.Count} kết quả." : "Không tìm thấy kết quả nào.";
                return results;
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi tìm kiếm - " + ex.Message;
                return new List<PaymentDetailDTO>();
            }
        }
        #endregion

        #region 📊 Thống kê Payment
        public decimal GetTotalSuccessfulPayments()
        {
            try
            {
                // Lưu ý: Nếu dữ liệu lớn, nên chuyển logic này xuống DAO (SELECT SUM...)
                var payments = _paymentDAO.GetAllPayments();
                return payments
                    .Where(p => p.Status.Equals(STATUS_SUCCESS, StringComparison.OrdinalIgnoreCase))
                    .Sum(p => p.Amount);
            }
            catch (Exception ex)
            {
                throw new Exception("BUS: Lỗi thống kê - " + ex.Message, ex);
            }
        }

        public Dictionary<string, int> GetPaymentCountByStatus()
        {
            try
            {
                // Lưu ý: Nếu dữ liệu lớn, nên chuyển logic này xuống DAO (SELECT COUNT... GROUP BY)
                var payments = _paymentDAO.GetAllPayments();
                return new Dictionary<string, int>
                {
                    { STATUS_PENDING, payments.Count(p => p.Status.Equals(STATUS_PENDING, StringComparison.OrdinalIgnoreCase)) },
                    { STATUS_SUCCESS, payments.Count(p => p.Status.Equals(STATUS_SUCCESS, StringComparison.OrdinalIgnoreCase)) },
                    { STATUS_FAILED, payments.Count(p => p.Status.Equals(STATUS_FAILED, StringComparison.OrdinalIgnoreCase)) }
                };
            }
            catch (Exception ex)
            {
                throw new Exception("BUS: Lỗi thống kê - " + ex.Message, ex);
            }
        }
        #endregion

        #region 🔄 Cập nhật trạng thái thủ công
        public bool UpdatePaymentStatus(int paymentId, string newStatus, out string message)
        {
            message = string.Empty;
            try
            {
                string normalized = newStatus.Trim().ToUpper();
                if (normalized != STATUS_PENDING && normalized != STATUS_SUCCESS && normalized != STATUS_FAILED)
                {
                    message = "Trạng thái không hợp lệ.";
                    return false;
                }

                bool result = _paymentDAO.UpdatePaymentStatus(paymentId, normalized);
                if (result)
                {
                    message = $"Cập nhật trạng thái thành {normalized}!";
                    return true;
                }

                message = "Không thể cập nhật trạng thái.";
                return false;
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi cập nhật trạng thái - " + ex.Message;
                return false;
            }
        }
        #endregion
    }
}