    using System;

    namespace DTO.Airline
    {
        public class AirlineDTO
        {
            #region Private Fields
            private int _airlineId;
            private string _airlineCode;
            private string _airlineName;
            private string _country;
        #endregion
        public string DisplayText => $"{AirlineId} - {AirlineName}";
        #region Public Properties
        public int AirlineId
            {
                get => _airlineId;
                set
                {
                    if (value < 0)
                        throw new ArgumentException("Airline ID không thể âm");
                    _airlineId = value;
                }
            }

            public string AirlineCode
            {
                get => _airlineCode;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Mã hãng hàng không không được để trống");
                    if (value.Length > 10)
                        throw new ArgumentException("Mã hãng hàng không không được quá 10 ký tự");
                    _airlineCode = value.Trim().ToUpper();
                }
            }

            public string AirlineName
            {
                get => _airlineName;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Tên hãng hàng không không được để trống");
                    if (value.Length > 100)
                        throw new ArgumentException("Tên hãng hàng không không được quá 100 ký tự");
                    _airlineName = value.Trim();
                }
            }

            public string Country
            {
                get => _country;
                set => _country = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
            #endregion

            #region Constructors
            public AirlineDTO() { }

            public AirlineDTO(string airlineCode, string airlineName, string country)
            {
                AirlineCode = airlineCode;
                AirlineName = airlineName;
                Country = country;
            }

            public AirlineDTO(int airlineId, string airlineCode, string airlineName, string country)
            {
                AirlineId = airlineId;
                AirlineCode = airlineCode;
                AirlineName = airlineName;
                Country = country;
            }
            #endregion

            #region Validation Methods
            public bool IsValid(out string errorMessage)
            {
                errorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(_airlineCode))
                {
                    errorMessage = "Mã hãng hàng không không được để trống";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(_airlineName))
                {
                    errorMessage = "Tên hãng hàng không không được để trống";
                    return false;
                }

                if (_airlineCode.Length > 10)
                {
                    errorMessage = "Mã hãng hàng không không được quá 10 ký tự";
                    return false;
                }

                if (_airlineName.Length > 100)
                {
                    errorMessage = "Tên hãng hàng không không được quá 100 ký tự";
                    return false;
                }

                return true;
            }
            #endregion

            #region Overrides
            public override string ToString()
            {
                return $"{_airlineCode} - {_airlineName}" +
                       (!string.IsNullOrEmpty(_country) ? $" ({_country})" : "");
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                    return false;

                var other = (AirlineDTO)obj;
                return _airlineId == other._airlineId;
            }

            public override int GetHashCode()
            {
                return _airlineId.GetHashCode();
            }
            #endregion

            #region Helper Methods
            public AirlineDTO Clone()
            {
                return new AirlineDTO(
                    _airlineId,
                    _airlineCode,
                    _airlineName,
                    _country
                );
            }
        #endregion
        // ✅ Dùng để hiển thị trong ComboBox (ID - Tên hãng)
        

    }
}
