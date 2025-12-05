using System;

namespace DTO.Flight
{
    /// <summary>
    /// ViewModel cho FlightList - chứa thông tin JOIN từ nhiều bảng
    /// </summary>
    public class FlightViewModel
    {
        // From Flights table
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public FlightStatus Status { get; set; }

        // From Route → Airport (JOIN)
        public string DepartureAirportCode { get; set; }  // VD: SGN
        public string DepartureAirportName { get; set; }  // VD: Tân Sơn Nhất
        public string ArrivalAirportCode { get; set; }    // VD: HAN
        public string ArrivalAirportName { get; set; }    // VD: Nội Bài

        // From Aircraft (JOIN - nếu cần)
        public string AircraftModel { get; set; }

        // Calculated
        public int AvailableSeats { get; set; }  // COUNT từ Flight_Seats

        public FlightViewModel()
        {
        }

        public FlightViewModel(int flightId, string flightNumber,
            DateTime? departureTime, DateTime? arrivalTime, FlightStatus status,
            string departureAirportCode, string departureAirportName,
            string arrivalAirportCode, string arrivalAirportName,
            int availableSeats = 0)
        {
            FlightId = flightId;
            FlightNumber = flightNumber;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            Status = status;
            DepartureAirportCode = departureAirportCode;
            DepartureAirportName = departureAirportName;
            ArrivalAirportCode = arrivalAirportCode;
            ArrivalAirportName = arrivalAirportName;
            AvailableSeats = availableSeats;
        }

        public override string ToString()
        {
            return $"{FlightNumber}: {DepartureAirportCode} → {ArrivalAirportCode}";
        }
    }
}