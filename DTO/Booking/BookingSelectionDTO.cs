namespace DTO.Booking
{
    public class BookingSelectionDTO
    {
        public int BookingId { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string DepartureAirport { get; set; } = string.Empty;
        public string ArrivalAirport { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int PassengerCount { get; set; }
        public DateTime BookingDate { get; set; }
        public string CabinClassName { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int AdultCount { get; set; }
        public int ChildrenCount { get; set; }
        public bool IsRoundTrip { get; set; }
        public DateTime? ReturnDate { get; set; }

        public BookingSelectionDTO() { }

        public BookingSelectionDTO(int bookingId, string flightNumber, string departureAirport,
                                  string arrivalAirport, DateTime departureTime, DateTime arrivalTime,
                                  string status, decimal totalAmount, int passengerCount, DateTime bookingDate)
        {
            BookingId = bookingId;
            FlightNumber = flightNumber;
            DepartureAirport = departureAirport;
            ArrivalAirport = arrivalAirport;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            Status = status;
            TotalAmount = totalAmount;
            PassengerCount = passengerCount;
            BookingDate = bookingDate;
        }
    }
}
