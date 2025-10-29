using System;

namespace DTO.Seat
{
    public class SeatDTO
    {
        #region Private Fields
        private int _seatId;
        private int _aircraftId;
        private string _seatNumber;
        private int _classId;
        #endregion

        #region Public Properties
        public int SeatId
        {
            get => _seatId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Seat ID không thể âm");
                _seatId = value;
            }
        }

        public int AircraftId
        {
            get => _aircraftId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Aircraft ID không hợp lệ");
                _aircraftId = value;
            }
        }

        public string SeatNumber
        {
            get => _seatNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Số ghế không được để trống");
                _seatNumber = value.Trim().ToUpper();
            }
        }

        public int ClassId
        {
            get => _classId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Class ID không hợp lệ");
                _classId = value;
            }
        }
        #endregion

        #region Constructors
        public SeatDTO() { }

        public SeatDTO(int aircraftId, string seatNumber, int classId)
        {
            AircraftId = aircraftId;
            SeatNumber = seatNumber;
            ClassId = classId;
        }

        public SeatDTO(int seatId, int aircraftId, string seatNumber, int classId)
        {
            SeatId = seatId;
            AircraftId = aircraftId;
            SeatNumber = seatNumber;
            ClassId = classId;
        }
        #endregion

        #region Validation
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(_seatNumber))
            {
                errorMessage = "Số ghế không được để trống";
                return false;
            }

            if (_aircraftId <= 0)
            {
                errorMessage = "Aircraft ID không hợp lệ";
                return false;
            }

            if (_classId <= 0)
            {
                errorMessage = "Class ID không hợp lệ";
                return false;
            }

            return true;
        }
        #endregion

        #region Overrides
        public override string ToString() => $"Ghế {_seatNumber} (Aircraft {_aircraftId}, Class {_classId})";

        public override bool Equals(object obj)
        {
            if (obj is not SeatDTO other)
                return false;
            return _seatId == other._seatId;
        }

        public override int GetHashCode() => _seatId.GetHashCode();
        #endregion
    }
}
