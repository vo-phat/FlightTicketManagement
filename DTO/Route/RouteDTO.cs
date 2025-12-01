using System;

namespace DTO.Route
{
    public class RouteDTO
    {
        public int RouteId { get; set; }
        public int? DepartureAirportId { get; set; }
        public int? ArrivalAirportId { get; set; }
        public int? DurationMinutes { get; set; }
        public int? DistanceKm { get; set; }

        // Display properties
        public string DepartureAirportName { get; set; }
        public string ArrivalAirportName { get; set; }

        public RouteDTO()
        {
            DepartureAirportName = string.Empty;
            ArrivalAirportName = string.Empty;
        }

        public RouteDTO(int routeId, int? departureAirportId, int? arrivalAirportId, 
            int? durationMinutes, int? distanceKm)
        {
            RouteId = routeId;
            DepartureAirportId = departureAirportId;
            ArrivalAirportId = arrivalAirportId;
            DurationMinutes = durationMinutes;
            DistanceKm = distanceKm;
            DepartureAirportName = string.Empty;
            ArrivalAirportName = string.Empty;
        }

        public bool IsValid(out string message)
        {
            if (!DepartureAirportId.HasValue || DepartureAirportId.Value <= 0)
            {
                message = "Departure airport is required";
                return false;
            }
            if (!ArrivalAirportId.HasValue || ArrivalAirportId.Value <= 0)
            {
                message = "Arrival airport is required";
                return false;
            }
            if (DepartureAirportId == ArrivalAirportId)
            {
                message = "Departure and arrival airports cannot be the same";
                return false;
            }
            message = string.Empty;
            return true;
        }
    }
}
