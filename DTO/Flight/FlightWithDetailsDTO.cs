using System;

namespace DTO.Flight
{
    /// <summary>
    /// DTO chứa thông tin đầy đủ của chuyến bay kèm theo chi tiết sân bay và máy bay
    /// </summary>
    public class FlightWithDetailsDTO
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public int AircraftId { get; set; }
        public int RouteId { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public decimal BasePrice { get; set; }
        public string? Note { get; set; }
        public FlightStatus Status { get; set; }

        // Thông tin sân bay
        public int DepartureAirportId { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DepartureAirportName { get; set; }
        public string DepartureCity { get; set; }
        
        public int ArrivalAirportId { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string ArrivalAirportName { get; set; }
        public string ArrivalCity { get; set; }

        // Thông tin máy bay (optional)
        public string AircraftModel { get; set; }
        public string AircraftManufacturer { get; set; }
        public int? AircraftCapacity { get; set; }

        // Tính toán số ghế còn trống
        public int AvailableSeats { get; set; }

        /// <summary>
        /// Format: SGN (TP. Hồ Chí Minh)
        /// </summary>
        public string DepartureAirportDisplay => $"{DepartureAirportCode} ({DepartureCity})";

        /// <summary>
        /// Format: HAN (Hà Nội)
        /// </summary>
        public string ArrivalAirportDisplay => $"{ArrivalAirportCode} ({ArrivalCity})";

        public TimeSpan? GetFlightDuration()
        {
            if (DepartureTime.HasValue && ArrivalTime.HasValue)
            {
                return ArrivalTime.Value - DepartureTime.Value;
            }
            return null;
        }

        public override string ToString()
        {
            return $"{FlightNumber}: {DepartureAirportCode} → {ArrivalAirportCode} " +
                   $"({DepartureTime:dd/MM/yyyy HH:mm} - {ArrivalTime:HH:mm})";
        }
    }
}
