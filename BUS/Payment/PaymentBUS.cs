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

        public PaymentBUS()
        {
            _paymentDAO = new PaymentDAO();
        }

        #region 📋 Lấy danh sách tất cả thanh toán
        /// <summary>
        /// Lấy tất cả payments (không có thông tin booking)
        /// </summary>
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

        /// <summary>
        /// Lấy tất cả payments với thông tin đầy đủ (có booking, account)
        /// </summary>
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
        /// <summary>
        /// Lấy danh sách payments của các bookings đang PENDING
        /// </summary>
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
        /// <summary>
        /// Lấy thông tin chi tiết của 1 payment
        /// </summary>
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
        /// Xử lý thanh toán: Cập nhật payment status -> SUCCESS và booking status -> CONFIRMED
        /// Sử dụng transaction trong DAL để đảm bảo tính nhất quán
        /// </summary>
        public bool ProcessPayment(int paymentId, out string message)
        {
            message = string.Empty;

            try
            {
                // Bước 1: Lấy thông tin payment
                var payment = _paymentDAO.GetPaymentById(paymentId);
                if (payment == null)
                {
                    message = "Không tìm thấy thông tin thanh toán.";
                    return false;
                }

                // Bước 2: Kiểm tra điều kiện thanh toán
                if (!payment.CanProcessPayment())
                {
                    message = "Không thể xử lý thanh toán. ";

                    if (!payment.IsPaymentPending())
                        message += "Payment không ở trạng thái PENDING. ";

                    if (!payment.IsBookingPending())
                        message += "Booking không ở trạng thái PENDING.";

                    return false;
                }

                // Bước 3: Kiểm tra số tiền có khớp không
                if (!payment.IsAmountMatched())
                {
                    message = $"Cảnh báo: Số tiền payment ({payment.Amount:N0} VND) " +
                             $"không khớp với tổng tiền booking ({payment.BookingTotalAmount:N0} VND). " +
                             $"Chênh lệch: {Math.Abs(payment.GetAmountDifference()):N0} VND";
                    // Vẫn cho phép thanh toán nhưng cảnh báo
                }

                // Bước 4: Gọi Payment Gateway (giả lập)
                bool gatewayResult = MockPaymentGateway(payment.Amount);

                if (!gatewayResult)
                {
                    message = "Thanh toán thất bại! Cổng thanh toán từ chối giao dịch.";
                    // Có thể cập nhật status thành FAILED
                    _paymentDAO.UpdatePaymentStatus(paymentId, PaymentDTO.STATUS_FAILED);
                    return false;
                }

                // Bước 5: Xử lý thanh toán với transaction
                bool result = _paymentDAO.ProcessPayment(paymentId, payment.BookingId);

                if (result)
                {
                    message = $"✅ Thanh toán thành công!\n" +
                             $"   - Payment ID: {paymentId} -> SUCCESS\n" +
                             $"   - Booking ID: {payment.BookingId} -> CONFIRMED\n" +
                             $"   - Số tiền: {payment.Amount:N0} VND\n" +
                             $"   - Email: {payment.AccountEmail}";
                    return true;
                }
                else
                {
                    message = "Không thể xử lý thanh toán. Vui lòng kiểm tra lại dữ liệu.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi trong quá trình thanh toán - " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Giả lập cổng thanh toán
        /// </summary>
        private bool MockPaymentGateway(decimal amount)
        {
            // Giả lập thanh toán:
            // - Luôn thất bại nếu số tiền <= 0
            // - Thành công 95% cho các giao dịch hợp lệ
            if (amount <= 0)
                return false;

            return new Random().Next(0, 100) < 95;
        }
        #endregion

        #region ➕ Thêm Payment mới
        /// <summary>
        /// Thêm payment mới
        /// </summary>
        public bool AddPayment(PaymentDTO payment, out string message)
        {
            message = string.Empty;

            try
            {
                // Validate
                if (!payment.IsValid(out string validationError))
                {
                    message = "Dữ liệu thanh toán không hợp lệ: " + validationError;
                    return false;
                }

                // Thêm vào DB
                bool result = _paymentDAO.InsertPayment(payment);

                if (result)
                {
                    message = "Thêm payment thành công!";
                    return true;
                }
                else
                {
                    message = "Không thể thêm payment vào cơ sở dữ liệu.";
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
        /// <summary>
        /// Cập nhật thông tin payment
        /// </summary>
        public bool UpdatePayment(PaymentDTO payment, out string message)
        {
            message = string.Empty;

            try
            {
                // Validate
                if (!payment.IsValid(out string validationError))
                {
                    message = "Dữ liệu thanh toán không hợp lệ: " + validationError;
                    return false;
                }

                // Kiểm tra payment tồn tại
                var existing = _paymentDAO.GetPaymentById(payment.PaymentId);
                if (existing == null)
                {
                    message = "Không tìm thấy payment cần cập nhật.";
                    return false;
                }

                // Không cho phép sửa payment đã SUCCESS
                if (existing.Status == PaymentDTO.STATUS_SUCCESS)
                {
                    message = "Không thể sửa payment đã thành công!";
                    return false;
                }

                // Cập nhật
                bool result = _paymentDAO.UpdatePayment(payment);

                if (result)
                {
                    message = "Cập nhật payment thành công!";
                    return true;
                }
                else
                {
                    message = "Không thể cập nhật payment.";
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
        /// <summary>
        /// Xóa payment
        /// </summary>
        public bool DeletePayment(int paymentId, out string message)
        {
            message = string.Empty;

            try
            {
                if (paymentId <= 0)
                {
                    message = "Payment ID không hợp lệ.";
                    return false;
                }

                // Kiểm tra payment tồn tại
                var payment = _paymentDAO.GetPaymentById(paymentId);
                if (payment == null)
                {
                    message = "Không tìm thấy payment cần xóa.";
                    return false;
                }

                // Không cho phép xóa payment đã SUCCESS
                if (payment.Status == PaymentDTO.STATUS_SUCCESS)
                {
                    message = "Không thể xóa payment đã thành công!";
                    return false;
                }

                // Không cho phép xóa nếu booking đã CONFIRMED
                if (payment.BookingStatus == PaymentDetailDTO.BOOKING_STATUS_CONFIRMED)
                {
                    message = "Không thể xóa payment của booking đã xác nhận!";
                    return false;
                }

                // Xóa
                bool result = _paymentDAO.DeletePayment(paymentId);

                if (result)
                {
                    message = "Xóa payment thành công!";
                    return true;
                }
                else
                {
                    message = "Không thể xóa payment.";
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
        /// <summary>
        /// Tìm kiếm payment theo từ khóa
        /// </summary>
        public List<PaymentDetailDTO> SearchPayments(string keyword, out string message)
        {
            message = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    message = "Vui lòng nhập từ khóa tìm kiếm.";
                    return new List<PaymentDetailDTO>();
                }

                var results = _paymentDAO.SearchPayments(keyword.Trim());

                if (results.Count == 0)
                {
                    message = $"Không tìm thấy kết quả nào cho từ khóa '{keyword}'.";
                }
                else
                {
                    message = $"Tìm thấy {results.Count} kết quả.";
                }

                return results;
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi khi tìm kiếm payment - " + ex.Message;
                return new List<PaymentDetailDTO>();
            }
        }
        #endregion

        #region 📊 Thống kê Payment
        /// <summary>
        /// Lấy tổng số tiền đã thanh toán thành công
        /// </summary>
        public decimal GetTotalSuccessfulPayments()
        {
            try
            {
                var payments = _paymentDAO.GetAllPayments();
                return payments
                    .Where(p => p.Status == PaymentDTO.STATUS_SUCCESS)
                    .Sum(p => p.Amount);
            }
            catch (Exception ex)
            {
                throw new Exception("BUS: Lỗi khi tính tổng thanh toán - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Đếm số payment theo trạng thái
        /// </summary>
        public Dictionary<string, int> GetPaymentCountByStatus()
        {
            try
            {
                var payments = _paymentDAO.GetAllPayments();
                return new Dictionary<string, int>
                {
                    { PaymentDTO.STATUS_PENDING, payments.Count(p => p.Status == PaymentDTO.STATUS_PENDING) },
                    { PaymentDTO.STATUS_SUCCESS, payments.Count(p => p.Status == PaymentDTO.STATUS_SUCCESS) },
                    { PaymentDTO.STATUS_FAILED, payments.Count(p => p.Status == PaymentDTO.STATUS_FAILED) }
                };
            }
            catch (Exception ex)
            {
                throw new Exception("BUS: Lỗi khi đếm payment - " + ex.Message, ex);
            }
        }
        #endregion

        #region 🔄 Cập nhật trạng thái
        /// <summary>
        /// Cập nhật trạng thái payment
        /// </summary>
        public bool UpdatePaymentStatus(int paymentId, string newStatus, out string message)
        {
            message = string.Empty;

            try
            {
                // Validate status
                string normalized = newStatus.Trim().ToUpper();
                if (normalized != PaymentDTO.STATUS_PENDING &&
                    normalized != PaymentDTO.STATUS_SUCCESS &&
                    normalized != PaymentDTO.STATUS_FAILED)
                {
                    message = "Trạng thái không hợp lệ.";
                    return false;
                }

                bool result = _paymentDAO.UpdatePaymentStatus(paymentId, normalized);

                if (result)
                {
                    message = $"Cập nhật trạng thái payment thành {normalized}!";
                    return true;
                }
                else
                {
                    message = "Không thể cập nhật trạng thái payment.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi khi cập nhật trạng thái - " + ex.Message;
                return false;
            }
        }
        #endregion
    }
}