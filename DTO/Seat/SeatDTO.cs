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

        // Thêm các trường private mới
        private string _className;
        private string _aircraftModel;
        private string _aircraftManufacturer;
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

        // Thêm các Public Property mới (Không thêm validation chi tiết nếu không cần)
        public string ClassName
        {
            get => _className;
            set => _className = value;
        }

        public string AircraftModel
        {
            get => _aircraftModel;
            set => _aircraftModel = value;
        }

        public string AircraftManufacturer
        {
            get => _aircraftManufacturer;
            set => _aircraftManufacturer = value;
        }
        #endregion

        #region Constructors
        public SeatDTO() { }

        // Constructor gốc (chỉ có 3 tham số - dùng cho Insert)
        public SeatDTO(int aircraftId, string seatNumber, int classId)
        {
            AircraftId = aircraftId;
            SeatNumber = seatNumber;
            ClassId = classId;
        }

        // Constructor gốc (chỉ có 4 tham số - dùng cho GetAll, Filter, Search, GetById)
        public SeatDTO(int seatId, int aircraftId, string seatNumber, int classId)
        {
            SeatId = seatId;
            AircraftId = aircraftId;
            SeatNumber = seatNumber;
            ClassId = classId;
        }

        // Constructor MỚI (7 tham số - dùng cho GetAllSeatsWithDetails)
        public SeatDTO(int seatId, int aircraftId, string seatNumber, int classId,
                       string className, string aircraftModel, string aircraftManufacturer)
        {
            SeatId = seatId;
            AircraftId = aircraftId;
            SeatNumber = seatNumber;
            ClassId = classId;

            // Gán các thuộc tính mới
            ClassName = className;
            AircraftModel = aircraftModel;
            AircraftManufacturer = aircraftManufacturer;
        }
        #endregion

        #region Validation
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            // ... (Giữ nguyên logic validation gốc) ...
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
        // Cập nhật ToString() để bao gồm thông tin chi tiết (nếu có)
        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(_className) && !string.IsNullOrWhiteSpace(_aircraftModel))
            {
                return $"Ghế {_seatNumber} ({_className}) - Máy bay: {_aircraftManufacturer} {_aircraftModel}";
            }
            return $"Ghế {_seatNumber} (Aircraft {_aircraftId}, Class {_classId})";
        }

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