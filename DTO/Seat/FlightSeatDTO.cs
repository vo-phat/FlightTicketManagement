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

        #region Public Display Properties (Dùng cho JOIN)
        public int AircraftId { get; set; }
        public int AircraftCapacity { get; set; } = 0;    // THÊM: Sức chứa máy bay
        public string AircraftName { get; set; } = "";    // Tên máy bay (Manufacturer Model)
        public string SeatNumber { get; set; } = "";      // Số ghế
        public string ClassName { get; set; } = "";
        public string FlightName { get; set; } = "";      // Tên chuyến bay (ví dụ: VN123)
        #endregion

        public int ClassId { get; set; }

        #region Public Properties (Chính)
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

        // Constructor cơ bản
        public FlightSeatDTO(int flightSeatId, int flightId, int seatId,
            decimal basePrice, string seatStatus)
        {
            FlightSeatId = flightSeatId;
            FlightId = flightId;
            SeatId = seatId;
            BasePrice = basePrice;
            SeatStatus = seatStatus;
        }

        // Constructor đầy đủ (JOIN)
        public FlightSeatDTO(int flightSeatId, int flightId, int seatId, int classId,
            decimal basePrice, string seatStatus,
            string aircraftName, string seatNumber, string className)
        {
            FlightSeatId = flightSeatId;
            FlightId = flightId;
            SeatId = seatId;
            ClassId = classId;
            BasePrice = basePrice;
            SeatStatus = seatStatus;
            AircraftName = aircraftName;
            SeatNumber = seatNumber;
            ClassName = className;
        }

        public FlightSeatDTO(int flightSeatId, int flightId, int aircraftId, int seatId, int classId,
            decimal basePrice, string seatStatus,
            string flightName, string aircraftName, string seatNumber, string className)
        {
            FlightSeatId = flightSeatId;
            FlightId = flightId;
            AircraftId = aircraftId;
            SeatId = seatId;
            ClassId = classId;
            BasePrice = basePrice;
            SeatStatus = seatStatus;
            FlightName = flightName;
            AircraftName = aircraftName;
            SeatNumber = seatNumber;
            ClassName = className;
        }

        public FlightSeatDTO(int flightSeatId, int flightId, int aircraftId, int seatId, int classId,
            decimal basePrice, string seatStatus,
            string aircraftName, string seatNumber, string className)
        {
            FlightSeatId = flightSeatId;
            FlightId = flightId;
            AircraftId = aircraftId;
            SeatId = seatId;
            ClassId = classId;
            BasePrice = basePrice;
            SeatStatus = seatStatus;
            AircraftName = aircraftName;
            SeatNumber = seatNumber;
            ClassName = className;
        }

        // THÊM: Constructor với AircraftCapacity
        public FlightSeatDTO(int flightSeatId, int flightId, int aircraftId, int seatId, int classId,
            decimal basePrice, string seatStatus,
            string flightName, string aircraftName, int aircraftCapacity, string seatNumber, string className)
        {
            FlightSeatId = flightSeatId;
            FlightId = flightId;
            AircraftId = aircraftId;
            SeatId = seatId;
            ClassId = classId;
            BasePrice = basePrice;
            SeatStatus = seatStatus;
            FlightName = flightName;
            AircraftName = aircraftName;
            AircraftCapacity = aircraftCapacity;
            SeatNumber = seatNumber;
            ClassName = className;
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
        public override string ToString() =>
            $"FlightSeat #{_flightSeatId}: {AircraftName} - {SeatNumber} ({ClassName}), Giá {_basePrice:#,0}₫, Trạng thái {_seatStatus}";

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