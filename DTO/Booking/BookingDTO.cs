namespace DTO.Booking
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public int AccountId { get; set; }
        public int FlightId { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int PassengerCount { get; set; }

        public BookingDTO() { }

        public BookingDTO(int bookingId, int accountId, int flightId, DateTime bookingDate, 
                         string status, decimal totalAmount, int passengerCount)
        {
            BookingId = bookingId;
            AccountId = accountId;
            FlightId = flightId;
            BookingDate = bookingDate;
            Status = status;
            TotalAmount = totalAmount;
            PassengerCount = passengerCount;
        }
    }
}
