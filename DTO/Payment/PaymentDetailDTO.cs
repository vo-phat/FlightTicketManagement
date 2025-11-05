using System;

namespace DTO.Payment
{
    public class PaymentDetailDTO : PaymentDTO
    {
        #region Booking Status Constants
        public const string BOOKING_STATUS_PENDING = "PENDING";
        public const string BOOKING_STATUS_CONFIRMED = "CONFIRMED";
        public const string BOOKING_STATUS_CANCELLED = "CANCELLED";
        public const string BOOKING_STATUS_REFUNDED = "REFUNDED";
        #endregion

        #region Private Fields
        private int _accountId;
        private DateTime _bookingDate;
        private string _bookingStatus;
        private decimal _bookingTotalAmount;
        private string _accountEmail;
        #endregion

        #region Public Properties
        public int AccountId
        {
            get => _accountId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Account ID phải lớn hơn 0");
                _accountId = value;
            }
        }

        public DateTime BookingDate
        {
            get => _bookingDate;
            set => _bookingDate = value;
        }

        public string BookingStatus
        {
            get => _bookingStatus;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Trạng thái booking không được để trống");

                string normalized = value.Trim().ToUpper();
                if (normalized != BOOKING_STATUS_PENDING &&
                    normalized != BOOKING_STATUS_CONFIRMED &&
                    normalized != BOOKING_STATUS_CANCELLED &&
                    normalized != BOOKING_STATUS_REFUNDED)
                    throw new ArgumentException("Trạng thái booking không hợp lệ");

                _bookingStatus = normalized;
            }
        }

        public decimal BookingTotalAmount
        {
            get => _bookingTotalAmount;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Tổng tiền booking không thể âm");
                _bookingTotalAmount = value;
            }
        }

        public string AccountEmail
        {
            get => _accountEmail;
            set => _accountEmail = value?.Trim();
        }
        #endregion

        #region Constructors
        public PaymentDetailDTO() : base() { }

        public PaymentDetailDTO(
            int paymentId,
            int bookingId,
            decimal amount,
            string paymentMethod,
            DateTime paymentDate,
            string status,
            int accountId,
            DateTime bookingDate,
            string bookingStatus,
            decimal bookingTotalAmount,
            string accountEmail = ""
        ) : base(paymentId, bookingId, amount, paymentMethod, paymentDate, status)
        {
            AccountId = accountId;
            BookingDate = bookingDate;
            BookingStatus = bookingStatus;
            BookingTotalAmount = bookingTotalAmount;
            AccountEmail = accountEmail;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return base.ToString() +
                   $"\n  Booking Date: {_bookingDate:dd/MM/yyyy HH:mm}, " +
                   $"Booking Status: {_bookingStatus}, " +
                   $"Booking Total: {_bookingTotalAmount:N0} VND" +
                   $"\n  Account: {_accountEmail}";
        }

        public override PaymentDTO Clone()
        {
            return new PaymentDetailDTO(
                PaymentId,
                BookingId,
                Amount,
                PaymentMethod,
                PaymentDate,
                Status,
                _accountId,
                _bookingDate,
                _bookingStatus,
                _bookingTotalAmount,
                _accountEmail
            );
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Helper Methods
        public string GetBookingStatusDisplay()
        {
            switch (_bookingStatus)
            {
                case BOOKING_STATUS_PENDING:
                    return "Đang chờ";
                case BOOKING_STATUS_CONFIRMED:
                    return "Đã xác nhận";
                case BOOKING_STATUS_CANCELLED:
                    return "Đã hủy";
                case BOOKING_STATUS_REFUNDED:
                    return "Đã hoàn tiền";
                default:
                    return _bookingStatus;
            }
        }

        public bool IsBookingPending()
        {
            return _bookingStatus == BOOKING_STATUS_PENDING;
        }

        public bool IsPaymentPending()
        {
            return Status == STATUS_PENDING;
        }

        public bool CanProcessPayment()
        {
            // Chỉ có thể thanh toán nếu cả booking và payment đều đang pending
            return IsBookingPending() && IsPaymentPending();
        }

        public decimal GetAmountDifference()
        {
            // So sánh số tiền payment với tổng tiền booking
            return Amount - _bookingTotalAmount;
        }

        public bool IsAmountMatched()
        {
            // Kiểm tra số tiền payment có khớp với booking không
            return Math.Abs(GetAmountDifference()) < 0.01m; // Sai số < 0.01 VND
        }
        #endregion
    }
}