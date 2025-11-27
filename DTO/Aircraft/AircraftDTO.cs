using System;

namespace DTO.Aircraft
{
    public class AircraftDTO
    {
        #region Private Fields
        private int _aircraftId;
        private int _airlineId;
        private string _model;           // nullable in DB, but keep string (allow null via setter)
        private string _manufacturer;    // nullable in DB, but keep string (allow null via setter)
        private int? _capacity;          // capacity may be NULL in DB -> use nullable int
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

        public int AirlineId
        {
            get => _airlineId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Airline ID phải lớn hơn 0");
                _airlineId = value;
            }
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
        #endregion

        #region Constructors
        public AircraftDTO() { }

        /// <summary>
        /// Constructor để insert (không có id)
        /// </summary>
        public AircraftDTO(int airlineId, string model, string manufacturer, int? capacity)
        {
            AirlineId = airlineId;
            Model = model;
            Manufacturer = manufacturer;
            Capacity = capacity;
        }

        /// <summary>
        /// Constructor đầy đủ (có id)
        /// </summary>
        public AircraftDTO(int aircraftId, int airlineId, string model, string manufacturer, int? capacity)
        {
            AircraftId = aircraftId;
            AirlineId = airlineId;
            Model = model;
            Manufacturer = manufacturer;
            Capacity = capacity;
        }
        #endregion

        #region Validation Methods
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (_airlineId <= 0)
            {
                errorMessage = "Phải chỉ định hãng (AirlineId)";
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

            return true;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            var namePart = !string.IsNullOrEmpty(_model) ? _model : "(unknown model)";
            var manufPart = !string.IsNullOrEmpty(_manufacturer) ? $" by {_manufacturer}" : "";
            var capPart = _capacity.HasValue ? $", Capacity: {_capacity.Value}" : "";
            return $"{namePart}{manufPart}{capPart}";
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
                _airlineId,
                _model,
                _manufacturer,
                _capacity
            );
        }
        #endregion
    }
}
