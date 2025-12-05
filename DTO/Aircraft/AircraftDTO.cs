using System;

namespace DTO.Aircraft
{
    /// <summary>
    /// DTO cho máy bay Vietnam Airlines
    /// Đã loại bỏ AirlineId vì project chỉ quản lý Vietnam Airlines
    /// </summary>
    public class AircraftDTO
    {
        #region Private Fields
        private int _aircraftId;
        private string _registrationNumber;  // VN-A###, VN-B###
        private string _model;               // nullable in DB, but keep string (allow null via setter)
        private string _manufacturer;        // nullable in DB, but keep string (allow null via setter)
        private int? _capacity;              // capacity may be NULL in DB -> use nullable int
        private int? _manufactureYear;       // năm sản xuất
        private string _status;              // ACTIVE, MAINTENANCE, RETIRED
        #endregion

        #region Public Properties
        public int AircraftId
        {
            get => _aircraftId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Aircraft ID không thể âm");
                _aircraftId = value;
            }
        }

        /// <summary>
        /// Số hiệu đăng ký máy bay (VN-A###, VN-B###)
        /// </summary>
        public string RegistrationNumber
        {
            get => _registrationNumber;
            set => _registrationNumber = string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpper();
        }

        /// <summary>
        /// Tên model (ví dụ: 'Boeing 787'). Có thể null nếu không biết.
        /// </summary>
        public string Model
        {
            get => _model;
            set => _model = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        /// <summary>
        /// Nhà sản xuất (ví dụ: 'Boeing', 'Airbus'). Có thể null.
        /// </summary>
        public string Manufacturer
        {
            get => _manufacturer;
            set => _manufacturer = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        /// <summary>
        /// Sức chứa (số ghế). Nullable vì cột DB cho phép NULL.
        /// Nếu gán giá trị, phải >= 1.
        /// </summary>
        public int? Capacity
        {
            get => _capacity;
            set
            {
                if (value.HasValue && value.Value < 1)
                    throw new ArgumentException("Capacity phải lớn hơn 0 nếu có giá trị");
                _capacity = value;
            }
        }

        /// <summary>
        /// Năm sản xuất máy bay
        /// </summary>
        public int? ManufactureYear
        {
            get => _manufactureYear;
            set
            {
                if (value.HasValue && (value.Value < 1900 || value.Value > DateTime.Now.Year + 2))
                    throw new ArgumentException("Năm sản xuất không hợp lệ");
                _manufactureYear = value;
            }
        }

        /// <summary>
        /// Trạng thái máy bay: ACTIVE, MAINTENANCE, RETIRED
        /// </summary>
        public string Status
        {
            get => _status ?? "ACTIVE";
            set => _status = string.IsNullOrWhiteSpace(value) ? "ACTIVE" : value.Trim().ToUpper();
        }

        /// <summary>
        /// Text hiển thị: VN-A801 - Boeing 787-9 (274 ghế)
        /// </summary>
        public string DisplayText => $"{RegistrationNumber} - {Model}" + 
                                     (Capacity.HasValue ? $" ({Capacity} ghế)" : "");
        #endregion

        #region Constructors
        public AircraftDTO() { }

        /// <summary>
        /// Constructor để insert (không có id) - Vietnam Airlines
        /// </summary>
        public AircraftDTO(string registrationNumber, string model, string manufacturer, int? capacity, 
                          int? manufactureYear = null, string status = "ACTIVE")
        {
            RegistrationNumber = registrationNumber;
            Model = model;
            Manufacturer = manufacturer;
            Capacity = capacity;
            ManufactureYear = manufactureYear;
            Status = status;
        }

        /// <summary>
        /// Constructor đầy đủ (có id) - Vietnam Airlines
        /// </summary>
        public AircraftDTO(int aircraftId, string registrationNumber, string model, string manufacturer, 
                          int? capacity, int? manufactureYear = null, string status = "ACTIVE")
        {
            AircraftId = aircraftId;
            RegistrationNumber = registrationNumber;
            Model = model;
            Manufacturer = manufacturer;
            Capacity = capacity;
            ManufactureYear = manufactureYear;
            Status = status;
        }
        #endregion

        #region Validation Methods
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(_registrationNumber))
            {
                errorMessage = "Số hiệu đăng ký không được để trống";
                return false;
            }

            if (!_registrationNumber.StartsWith("VN-"))
            {
                errorMessage = "Số hiệu đăng ký phải bắt đầu bằng 'VN-' (Vietnam Airlines)";
                return false;
            }

            if (!string.IsNullOrEmpty(_model) && _model.Length > 100)
            {
                errorMessage = "Model không được quá 100 ký tự";
                return false;
            }

            if (!string.IsNullOrEmpty(_manufacturer) && _manufacturer.Length > 100)
            {
                errorMessage = "Manufacturer không được quá 100 ký tự";
                return false;
            }

            if (_capacity.HasValue && _capacity.Value < 1)
            {
                errorMessage = "Capacity phải lớn hơn 0";
                return false;
            }

            if (_manufactureYear.HasValue && (_manufactureYear.Value < 1900 || _manufactureYear.Value > DateTime.Now.Year + 2))
            {
                errorMessage = "Năm sản xuất không hợp lệ";
                return false;
            }

            var validStatuses = new[] { "ACTIVE", "MAINTENANCE", "RETIRED" };
            if (!string.IsNullOrEmpty(_status) && !validStatuses.Contains(_status.ToUpper()))
            {
                errorMessage = "Status phải là ACTIVE, MAINTENANCE hoặc RETIRED";
                return false;
            }

            return true;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            var regPart = !string.IsNullOrEmpty(_registrationNumber) ? _registrationNumber : "N/A";
            var modelPart = !string.IsNullOrEmpty(_model) ? _model : "(unknown model)";
            var manufPart = !string.IsNullOrEmpty(_manufacturer) ? $" by {_manufacturer}" : "";
            var capPart = _capacity.HasValue ? $", {_capacity.Value} seats" : "";
            var yearPart = _manufactureYear.HasValue ? $", Year: {_manufactureYear.Value}" : "";
            var statusPart = !string.IsNullOrEmpty(_status) ? $" [{_status}]" : "";
            return $"Vietnam Airlines - {regPart}: {modelPart}{manufPart}{capPart}{yearPart}{statusPart}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (AircraftDTO)obj;
            return _aircraftId == other._aircraftId;
        }

        public override int GetHashCode()
        {
            return _aircraftId.GetHashCode();
        }
        #endregion

        #region Helper Methods
        public AircraftDTO Clone()
        {
            return new AircraftDTO(
                _aircraftId,
                _registrationNumber,
                _model,
                _manufacturer,
                _capacity,
                _manufactureYear,
                _status
            );
        }
        #endregion
    }
}
