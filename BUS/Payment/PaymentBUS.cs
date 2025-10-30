using System;
using System.Collections.Generic;
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

        #region Lấy danh sách tất cả thanh toán
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
        #endregion

        #region 🔹 Lấy danh sách thanh toán của các booking đang Pending
        public List<PaymentDTO> GetPaymentsOfPendingBookings()
        {
            try
            {
                return _paymentDAO.GetPaymentsOfPendingBookings();
            }
            catch (Exception ex)
            {
                throw new Exception("BUS: Lỗi khi lấy thanh toán của các booking Pending - " + ex.Message, ex);
            }
        }
        #endregion

        #region Thêm thanh toán mới
        public bool InsertPayment(PaymentDTO payment, out string message)
        {
            message = string.Empty;

            try
            {
                if (!payment.IsValid(out string validationError))
                {
                    message = validationError;
                    return false;
                }

                bool result = _paymentDAO.InsertPayment(payment);
                message = result ? "Thêm thanh toán thành công" : "Không thể thêm thanh toán";
                return result;
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi khi thêm thanh toán - " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Cập nhật thanh toán
        public bool UpdatePayment(PaymentDTO payment, out string message)
        {
            message = string.Empty;

            try
            {
                if (!payment.IsValid(out string validationError))
                {
                    message = validationError;
                    return false;
                }

                bool result = _paymentDAO.UpdatePayment(payment);
                message = result ? "Cập nhật thanh toán thành công" : "Không thể cập nhật thanh toán";
                return result;
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi khi cập nhật thanh toán - " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Xóa thanh toán
        public bool DeletePayment(int paymentId, out string message)
        {
            message = string.Empty;

            try
            {
                bool result = _paymentDAO.DeletePayment(paymentId);
                message = result ? "Xóa thanh toán thành công" : "Không thể xóa thanh toán";
                return result;
            }
            catch (Exception ex)
            {
                message = "BUS: Lỗi khi xóa thanh toán - " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Tìm kiếm thanh toán
        public List<PaymentDTO> SearchPayments(string keyword)
        {
            try
            {
                return _paymentDAO.SearchPayments(keyword);
            }
            catch (Exception ex)
            {
                throw new Exception("BUS: Lỗi khi tìm kiếm thanh toán - " + ex.Message, ex);
            }
        }
        #endregion
    }
}
