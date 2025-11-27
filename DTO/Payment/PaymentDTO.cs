using System;

namespace DTO.Payment
{
    public class PaymentDTO
    {
        #region Constants
        // Payment Methods
        public const string METHOD_CREDIT_CARD = "CREDIT_CARD";
        public const string METHOD_BANK_TRANSFER = "BANK_TRANSFER";
        public const string METHOD_E_WALLET = "E_WALLET";
        public const string METHOD_CASH = "CASH";

        // Payment Status
        public const string STATUS_PENDING = "PENDING";
        public const string STATUS_SUCCESS = "SUCCESS";
        public const string STATUS_FAILED = "FAILED";
        #endregion

        #region Private Fields
        private int _paymentId;
        private int _bookingId;
        private decimal _amount;
        private string _paymentMethod;
        private DateTime _paymentDate;
        private string _status;
        #endregion

        #region Public Properties
        public int PaymentId
        {
            get => _paymentId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Payment ID không thể âm");
                _paymentId = value;
            }
        }

        public int BookingId
        {
            get => _bookingId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Booking ID phải lớn hơn 0");
                _bookingId = value;
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Số tiền thanh toán phải lớn hơn 0");
                _amount = value;
            }
        }

        public string PaymentMethod
        {
            get => _paymentMethod;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Phương thức thanh toán không được để trống");

                string normalized = value.Trim().ToUpper();

                if (normalized != METHOD_CREDIT_CARD &&
                    normalized != METHOD_BANK_TRANSFER &&
                    normalized != METHOD_E_WALLET &&
                    normalized != METHOD_CASH)
                    throw new ArgumentException("Phương thức thanh toán không hợp lệ (chỉ chấp nhận: CREDIT_CARD, BANK_TRANSFER, E_WALLET, CASH)");

                _paymentMethod = normalized;
            }
        }

        public DateTime PaymentDate
        {
            get => _paymentDate;
            set => _paymentDate = value == default ? DateTime.Now : value;
        }

        public string Status
        {
            get => _status;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Trạng thái thanh toán không được để trống");

                string normalized = value.Trim().ToUpper();
                if (normalized != STATUS_PENDING &&
                    normalized != STATUS_SUCCESS &&
                    normalized != STATUS_FAILED)
                    throw new ArgumentException("Trạng thái không hợp lệ (chỉ chấp nhận: PENDING, SUCCESS, FAILED)");

                _status = normalized;
            }
        }
        #endregion

        #region Constructors
        public PaymentDTO() { }

        public PaymentDTO(int bookingId, decimal amount, string paymentMethod, DateTime paymentDate, string status)
        {
            BookingId = bookingId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            PaymentDate = paymentDate;
            Status = status;
        }

        public PaymentDTO(int paymentId, int bookingId, decimal amount, string paymentMethod, DateTime paymentDate, string status)
        {
            PaymentId = paymentId;
            BookingId = bookingId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            PaymentDate = paymentDate;
            Status = status;
        }
        #endregion

        #region Validation Methods
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (_bookingId <= 0)
            {
                errorMessage = "Booking ID phải lớn hơn 0";
                return false;
            }

            if (_amount <= 0)
            {
                errorMessage = "Số tiền thanh toán phải lớn hơn 0";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_paymentMethod))
            {
                errorMessage = "Phương thức thanh toán không được để trống";
                return false;
            }

            if (_paymentMethod != METHOD_CREDIT_CARD &&
                _paymentMethod != METHOD_BANK_TRANSFER &&
                _paymentMethod != METHOD_E_WALLET &&
                _paymentMethod != METHOD_CASH)
            {
                errorMessage = "Phương thức thanh toán không hợp lệ";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_status))
            {
                errorMessage = "Trạng thái thanh toán không được để trống";
                return false;
            }

            if (_status != STATUS_PENDING &&
                _status != STATUS_SUCCESS &&
                _status != STATUS_FAILED)
            {
                errorMessage = "Trạng thái không hợp lệ";
                return false;
            }

            return true;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return $"PaymentID: {_paymentId}, BookingID: {_bookingId}, " +
                   $"Amount: {_amount:N0} VND, Method: {_paymentMethod}, " +
                   $"Date: {_paymentDate:dd/MM/yyyy HH:mm}, Status: {_status}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (PaymentDTO)obj;
            return _paymentId == other._paymentId;
        }

        public override int GetHashCode()
        {
            return _paymentId.GetHashCode();
        }
        #endregion

        #region Helper Methods
        public virtual PaymentDTO Clone()
        {
            return new PaymentDTO(
                _paymentId,
                _bookingId,
                _amount,
                _paymentMethod,
                _paymentDate,
                _status
            );
        }

        public string GetPaymentMethodDisplay()
        {
            switch (_paymentMethod)
            {
                case METHOD_CREDIT_CARD:
                    return "Thẻ tín dụng";
                case METHOD_BANK_TRANSFER:
                    return "Chuyển khoản";
                case METHOD_E_WALLET:
                    return "Ví điện tử";
                case METHOD_CASH:
                    return "Tiền mặt";
                default:
                    return _paymentMethod;
            }
        }

        public string GetStatusDisplay()
        {
            switch (_status)
            {
                case STATUS_PENDING:
                    return "Đang chờ";
                case STATUS_SUCCESS:
                    return "Thành công";
                case STATUS_FAILED:
                    return "Thất bại";
                default:
                    return _status;
            }
        }
        #endregion
    }
}