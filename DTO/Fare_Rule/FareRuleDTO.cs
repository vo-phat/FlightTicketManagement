using System;

namespace DTO.Fare_Rule
{
    public class FareRuleDTO
    {
        #region Private Fields
        private int _ruleId;
        private int _routeId;
        private int _classId;
        private string _routeName = string.Empty;
        private string _cabinClass = string.Empty;
        private string _fareType = string.Empty;
        private string _season = string.Empty;
        private DateTime _effectiveDate;
        private DateTime _expiryDate;
        private string _description = string.Empty;
        private decimal _price;
        #endregion

        #region Public Properties
        public int RuleId
        {
            get => _ruleId;
            set
            {
                if (value < 0) throw new ArgumentException("RuleId cannot be negative");
                _ruleId = value;
            }
        }

        public int RouteId
        {
            get => _routeId;
            set
            {
                if (value <= 0) throw new ArgumentException("RouteId must be greater than 0");
                _routeId = value;
            }
        }

        public int ClassId
        {
            get => _classId;
            set
            {
                if (value <= 0) throw new ArgumentException("ClassId must be greater than 0");
                _classId = value;
            }
        }

        public string RouteName
        {
            get => _routeName;
            set => _routeName = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        public string CabinClass
        {
            get => _cabinClass;
            set => _cabinClass = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        public string FareType
        {
            get => _fareType;
            set => _fareType = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        public string Season
        {
            get => _season;
            set => _season = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        public DateTime EffectiveDate
        {
            get => _effectiveDate;
            set => _effectiveDate = value;
        }

        public DateTime ExpiryDate
        {
            get => _expiryDate;
            set => _expiryDate = value;
        }

        public string Description
        {
            get => _description;
            set => _description = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0m) throw new ArgumentException("Price cannot be negative");
                _price = value;
            }
        }
        #endregion

        #region Constructors
        public FareRuleDTO()
        {
            _effectiveDate = DateTime.MinValue;
            _expiryDate = DateTime.MinValue;
        }

        public FareRuleDTO(int routeId, int classId, string fareType, string season, DateTime effectiveDate, DateTime expiryDate, string description, decimal price)
            : this()
        {
            RouteId = routeId;
            ClassId = classId;
            FareType = fareType;
            Season = season;
            EffectiveDate = effectiveDate;
            ExpiryDate = expiryDate;
            Description = description;
            Price = price;
        }

        public FareRuleDTO(int ruleId, int routeId, int classId,
                           string routeName, string cabinClass,
                           string fareType, string season,
                           DateTime effectiveDate, DateTime expiryDate,
                           string description, decimal price)
            : this(routeId, classId, fareType, season, effectiveDate, expiryDate, description, price)
        {
            RuleId = ruleId;
            RouteName = routeName;
            CabinClass = cabinClass;
        }
        #endregion

        #region Validation
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (_routeId <= 0)
            {
                errorMessage = "RouteId must be provided and greater than 0";
                return false;
            }

            if (_classId <= 0)
            {
                errorMessage = "ClassId must be provided and greater than 0";
                return false;
            }

            if (_price < 0m)
            {
                errorMessage = "Price cannot be negative";
                return false;
            }

            if (_effectiveDate != DateTime.MinValue && _expiryDate != DateTime.MinValue)
            {
                if (_expiryDate < _effectiveDate)
                {
                    errorMessage = "ExpiryDate cannot be earlier than EffectiveDate";
                    return false;
                }
            }

            // Optional: basic checks for length of textual fields
            if (_fareType.Length > 100)
            {
                errorMessage = "FareType too long";
                return false;
            }

            if (_season.Length > 50)
            {
                errorMessage = "Season too long";
                return false;
            }

            return true;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            var route = string.IsNullOrEmpty(_routeName) ? $"RouteId={_routeId}" : _routeName;
            var cls = string.IsNullOrEmpty(_cabinClass) ? $"ClassId={_classId}" : _cabinClass;
            return $"FareRule #{_ruleId}: {route} - {cls} [{_fareType}] {(_price):0.00}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || GetType() != obj.GetType()) return false;
            var other = (FareRuleDTO)obj;
            return _ruleId == other._ruleId;
        }

        public override int GetHashCode() => _ruleId.GetHashCode();
        #endregion

        #region Helper
        public FareRuleDTO Clone()
        {
            return new FareRuleDTO(
                _ruleId,
                _routeId,
                _classId,
                _routeName,
                _cabinClass,
                _fareType,
                _season,
                _effectiveDate,
                _expiryDate,
                _description,
                _price
            );
        }
        #endregion
    }
}
