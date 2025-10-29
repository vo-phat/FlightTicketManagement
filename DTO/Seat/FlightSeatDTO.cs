using System;

namespace DTO.FlightSeat
{
    public class FlightSeatDTO
    {
        #region Private Fields
        private int _flightSeatId;
        private int _flightId;
        private int _seatId;
        private decimal _basePrice;
        private string _seatStatus;
        #endregion

        #region Public Properties
        public int FlightSeatId
        {
            get => _flightSeatId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("FlightSeat ID không thể âm");
                _flightSeatId = value;
            }
        }

        public int FlightId
        {
            get => _flightId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Flight ID không hợp lệ");
                _flightId = value;
            }
        }

        public int SeatId
        {
            get => _seatId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Seat ID không hợp lệ");
                _seatId = value;
            }
        }

        public decimal BasePrice
        {
            get => _basePrice;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Giá cơ bản không thể âm");
                _basePrice = value;
            }
        }

        public string SeatStatus
        {
            get => _seatStatus;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Trạng thái ghế không được để trống");
                _seatStatus = value.Trim().ToUpper();
            }
        }
        #endregion

        #region Constructors
        public FlightSeatDTO() { }

        public FlightSeatDTO(int flightId, int seatId, decimal basePrice, string seatStatus)
        {
            FlightId = flightId;
            SeatId = seatId;
            BasePrice = basePrice;
            SeatStatus = seatStatus;
        }

        public FlightSeatDTO(int flightSeatId, int flightId, int seatId, decimal basePrice, string seatStatus)
        {
            FlightSeatId = flightSeatId;
            FlightId = flightId;
            SeatId = seatId;
            BasePrice = basePrice;
            SeatStatus = seatStatus;
        }
        #endregion

        #region Validation
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (_flightId <= 0)
            {
                errorMessage = "Flight ID không hợp lệ";
                return false;
            }

            if (_seatId <= 0)
            {
                errorMessage = "Seat ID không hợp lệ";
                return false;
            }

            if (_basePrice < 0)
            {
                errorMessage = "Giá cơ bản không thể âm";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_seatStatus))
            {
                errorMessage = "Trạng thái ghế không được để trống";
                return false;
            }

            return true;
        }
        #endregion

        #region Overrides
        public override string ToString() => $"FlightSeat #{_flightSeatId}: Seat {_seatId}, Price {_basePrice:C}, Status {_seatStatus}";

        public override bool Equals(object obj)
        {
            if (obj is not FlightSeatDTO other)
                return false;
            return _flightSeatId == other._flightSeatId;
        }

        public override int GetHashCode() => _flightSeatId.GetHashCode();
        #endregion
    }
}
