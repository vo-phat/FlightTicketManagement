using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Flight
{
    public class FlightDTO
    {
        #region Private Fields

        private int _flightId;
        private string _flightNumber;
        private int _aircraftId;
        private int _routeId;
        private DateTime? _departureTime;
        private DateTime? _arrivalTime;
        private FlightStatus _status;

        #endregion

        #region Public Properties
        public int FlightId
        {
            get => _flightId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Flight ID không thể âm");
                _flightId = value;
            }
        }

        public string FlightNumber
        {
            get => _flightNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Số hiệu chuyến bay không được để trống");

                if (value.Length > 20)
                    throw new ArgumentException("Số hiệu chuyến bay không được quá 20 ký tự");

                _flightNumber = value.Trim().ToUpper();
            }
        }

        public int AircraftId
        {
            get => _aircraftId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Aircraft ID phải lớn hơn 0");
                _aircraftId = value;
            }
        }

        public int RouteId
        {
            get => _routeId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Route ID phải lớn hơn 0");
                _routeId = value;
            }
        }

        public DateTime? DepartureTime
        {
            get => _departureTime;
            set => _departureTime = value;
        }

        public DateTime? ArrivalTime
        {
            get => _arrivalTime;
            set => _arrivalTime = value;
        }

        public FlightStatus Status
        {
            get => _status;
            set => _status = value;
        }
        #endregion

        #region Constructors
        public FlightDTO()
        {
            _status = FlightStatus.SCHEDULED;
        }
        public FlightDTO(string flightNumber, int aircraftId, int routeId,
                         DateTime? departureTime, DateTime? arrivalTime)
        {
            FlightNumber = flightNumber;
            AircraftId = aircraftId;
            RouteId = routeId;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            _status = FlightStatus.SCHEDULED;
        }
        public FlightDTO(int flightId, string flightNumber, int aircraftId, int routeId,
                         DateTime? departureTime, DateTime? arrivalTime, FlightStatus status)
        {
            FlightId = flightId;
            FlightNumber = flightNumber;
            AircraftId = aircraftId;
            RouteId = routeId;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            Status = status;
        }
        #endregion

        #region Validation Methods
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(_flightNumber))
            {
                errorMessage = "Số hiệu chuyến bay không được để trống";
                return false;
            }

            if (_aircraftId <= 0)
            {
                errorMessage = "Phải chọn máy bay";
                return false;
            }

            if (_routeId <= 0)
            {
                errorMessage = "Phải chọn tuyến bay";
                return false;
            }

            if (!_departureTime.HasValue)
            {
                errorMessage = "Phải nhập thời gian khởi hành";
                return false;
            }

            if (!_arrivalTime.HasValue)
            {
                errorMessage = "Phải nhập thời gian đến";
                return false;
            }

            // Validate datetime range
            if (_departureTime.Value.Year < 2000 || _departureTime.Value.Year > 2100)
            {
                errorMessage = "Thời gian khởi hành không hợp lệ (năm phải từ 2000-2100)";
                return false;
            }

            if (_arrivalTime.Value.Year < 2000 || _arrivalTime.Value.Year > 2100)
            {
                errorMessage = "Thời gian đến không hợp lệ (năm phải từ 2000-2100)";
                return false;
            }

            // Validate arrival after departure
            if (_arrivalTime.Value <= _departureTime.Value)
            {
                errorMessage = "Thời gian đến phải sau thời gian khởi hành";
                return false;
            }

            return true;
        }

        public TimeSpan? GetFlightDuration()
        {
            if(_departureTime.HasValue && _arrivalTime.HasValue)
            {
                return _arrivalTime.Value - _departureTime.Value;
            }
            return null;
        }
        #endregion

        #region
        public override string ToString()
        {
            return $"Flight {_flightNumber} - {_status.GetDescription()}"+
                   $"({_departureTime:dd/MM/yyyy HH:mm} - {_arrivalTime:dd/MM/yyyy HH:mm})";
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (FlightDTO)obj;
            return _flightId == other._flightId;
        }
        public override int GetHashCode()
        {
            return _flightId.GetHashCode();
        }
        #endregion

        #region Helper Methods
        public FlightDTO Clone()
        {
            return new FlightDTO(
                _flightId,
                _flightNumber,
                _aircraftId,
                _routeId,
                _departureTime,
                _arrivalTime,
                _status
            );
        }
        #endregion
    }
}
