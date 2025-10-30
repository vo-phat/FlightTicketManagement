using System;

namespace DTO.Payment
{
    public class PaymentDTO
    {
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

                if (value.Length > 50)
                    throw new ArgumentException("Phương thức thanh toán không được quá 50 ký tự");

                _paymentMethod = value.Trim().ToUpper();
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
                if (normalized != "PENDING" && normalized != "SUCCESS" && normalized != "FAILED")
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

            if (_paymentMethod.Length > 50)
            {
                errorMessage = "Phương thức thanh toán không được quá 50 ký tự";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_status))
            {
                errorMessage = "Trạng thái thanh toán không được để trống";
                return false;
            }

            if (_status != "PENDING" && _status != "SUCCESS" && _status != "FAILED")
            {
                errorMessage = "Trạng thái không hợp lệ (chỉ chấp nhận: PENDING, SUCCESS, FAILED)";
                return false;
            }

            return true;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return $"PaymentID: {_paymentId}, BookingID: {_bookingId}, " +
                   $"Amount: {_amount:N0} VND, Method: {_paymentMethod}, Status: {_status}";
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
        public PaymentDTO Clone()
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
        #endregion
    }
}
