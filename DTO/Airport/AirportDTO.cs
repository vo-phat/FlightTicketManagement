using System;

namespace DTO.Airport
{
    public class AirportDTO
    {
        #region Private Fields
        private int _airportId;
        private string _airportCode;
        private string _airportName;
        private string _city;
        private string _country;
        #endregion

        #region Public Properties
        public int AirportId
        {
            get => _airportId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Airport ID không thể âm");
                _airportId = value;
            }
        }

        public string AirportCode
        {
            get => _airportCode;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã sân bay không được để trống");

                if (value.Length > 10)
                    throw new ArgumentException("Mã sân bay không được quá 10 ký tự");

                _airportCode = value.Trim().ToUpper();
            }
        }

        public string AirportName
        {
            get => _airportName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên sân bay không được để trống");

                if (value.Length > 100)
                    throw new ArgumentException("Tên sân bay không được quá 100 ký tự");

                _airportName = value.Trim();
            }
        }

        public string City
        {
            get => _city;
            set => _city = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string Country
        {
            get => _country;
            set => _country = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
        #endregion

        #region Constructors
        public AirportDTO() { }

        public AirportDTO(string airportCode, string airportName, string city, string country)
        {
            AirportCode = airportCode;
            AirportName = airportName;
            City = city;
            Country = country;
        }

        public AirportDTO(int airportId, string airportCode, string airportName, string city, string country)
        {
            AirportId = airportId;
            AirportCode = airportCode;
            AirportName = airportName;
            City = city;
            Country = country;
        }
        #endregion

        #region Validation Methods
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(_airportCode))
            {
                errorMessage = "Mã sân bay không được để trống";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_airportName))
            {
                errorMessage = "Tên sân bay không được để trống";
                return false;
            }

            if (_airportCode.Length > 10)
            {
                errorMessage = "Mã sân bay không được quá 10 ký tự";
                return false;
            }

            if (_airportName.Length > 100)
            {
                errorMessage = "Tên sân bay không được quá 100 ký tự";
                return false;
            }

            return true;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return $"{_airportCode} - {_airportName}" +
                   (!string.IsNullOrEmpty(_city) ? $", {_city}" : "") +
                   (!string.IsNullOrEmpty(_country) ? $" ({_country})" : "");
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (AirportDTO)obj;
            return _airportId == other._airportId;
        }

        public override int GetHashCode()
        {
            return _airportId.GetHashCode();
        }
        #endregion

        #region Helper Methods
        public AirportDTO Clone()
        {
            return new AirportDTO(
                _airportId,
                _airportCode,
                _airportName,
                _city,
                _country
            );
        }
        #endregion
    }
}
