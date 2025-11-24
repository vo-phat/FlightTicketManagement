using System;

namespace DTO.Route
{
    public class RouteDTO
    {
        #region Private Fields
        private int _routeId;
        private int _departurePlaceId;
        private int _arrivalPlaceId;
        private int? _distanceKm;
        private int? _durationMinutes;
        #endregion

        #region Public Properties
        public int RouteId
        {
            get => _routeId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Route ID không thể âm");
                _routeId = value;
            }
        }

        public int DeparturePlaceId
        {
            get => _departurePlaceId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Departure Place ID phải lớn hơn 0");
                _departurePlaceId = value;
            }
        }

        public int ArrivalPlaceId
        {
            get => _arrivalPlaceId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Arrival Place ID phải lớn hơn 0");
                _arrivalPlaceId = value;
            }
        }

        public int? DistanceKm
        {
            get => _distanceKm;
            set
            {
                if (value.HasValue && value.Value < 0)
                    throw new ArgumentException("Khoảng cách (km) không thể âm");
                _distanceKm = value;
            }
        }

        public int? DurationMinutes
        {
            get => _durationMinutes;
            set
            {
                if (value.HasValue && value.Value < 0)
                    throw new ArgumentException("Thời gian bay (phút) không thể âm");
                _durationMinutes = value;
            }
        }
        #endregion

        #region Constructors
        public RouteDTO() { }

        public RouteDTO(int departurePlaceId, int arrivalPlaceId, int? distanceKm, int? durationMinutes)
        {
            DeparturePlaceId = departurePlaceId;
            ArrivalPlaceId = arrivalPlaceId;
            DistanceKm = distanceKm;
            DurationMinutes = durationMinutes;
        }

        public RouteDTO(int routeId, int departurePlaceId, int arrivalPlaceId, int? distanceKm, int? durationMinutes)
        {
            RouteId = routeId;
            DeparturePlaceId = departurePlaceId;
            ArrivalPlaceId = arrivalPlaceId;
            DistanceKm = distanceKm;
            DurationMinutes = durationMinutes;
        }
        #endregion

        #region Validation Methods
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (_departurePlaceId <= 0)
            {
                errorMessage = "Phải chọn sân bay khởi hành hợp lệ";
                return false;
            }

            if (_arrivalPlaceId <= 0)
            {
                errorMessage = "Phải chọn sân bay đến hợp lệ";
                return false;
            }

            if (_departurePlaceId == _arrivalPlaceId)
            {
                errorMessage = "Sân bay khởi hành và đến không được trùng nhau";
                return false;
            }

            if (_distanceKm.HasValue && _distanceKm.Value < 0)
            {
                errorMessage = "Khoảng cách không thể âm";
                return false;
            }

            if (_durationMinutes.HasValue && _durationMinutes.Value < 0)
            {
                errorMessage = "Thời gian bay không thể âm";
                return false;
            }

            return true;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            string dist = _distanceKm.HasValue ? $"{_distanceKm.Value} km" : "N/A";
            string dur = _durationMinutes.HasValue ? $"{_durationMinutes.Value} phút" : "N/A";
            return $"Route #{_routeId}: {_departurePlaceId} → {_arrivalPlaceId} ({dist}, {dur})";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (RouteDTO)obj;
            return _routeId == other._routeId;
        }

        public override int GetHashCode()
        {
            return _routeId.GetHashCode();
        }
        #endregion

        #region Helper Methods
        public RouteDTO Clone()
        {
            return new RouteDTO(
                _routeId,
                _departurePlaceId,
                _arrivalPlaceId,
                _distanceKm,
                _durationMinutes
            );
        }
        #endregion
    }
}
