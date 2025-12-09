using System;
using System.Collections.Generic;

namespace DTO.Booking
{
    /// <summary>
    /// DTO chứa thông tin đặt vé của user
    /// </summary>
    public class BookingRequestDTO
    {
        public int AccountId { get; set; }
        public int FlightId { get; set; }
        public int CabinClassId { get; set; }
        public string CabinClassName { get; set; }
        public DateTime BookingDate { get; set; }
        public int TicketCount { get; set; } = 1; // Số lượng vé đặt
        
        // Round-trip support
        public bool IsRoundTrip { get; set; } = false;
        public Guid? GroupBookingId { get; set; } // Link outbound + return bookings
        
        // Thông tin chuyến bay (để hiển thị)
        public string FlightNumber { get; set; }
        public string DepartureAirportCode { get; set; }
        public string ArrivalAirportCode { get; set; }
        public DateTime? DepartureTime { get; set; }

        /// <summary>
        /// Lấy thông tin đặt vé: FlightId, CabinClassId, số lượng vé, và loại vé
        /// </summary>
        /// <returns>Tuple chứa (FlightId, CabinClassId, TicketCount, IsRoundTrip)</returns>
        public (int FlightId, int CabinClassId, int TicketCount, bool IsRoundTrip) GetBookingInfo()
        {
            return (FlightId, CabinClassId, TicketCount, IsRoundTrip);
        }
    }

    /// <summary>
    /// Danh sách yêu cầu đặt vé (có thể nhiều người cùng 1 chuyến bay)
    /// </summary>
    public class BookingRequestListDTO
    {
        public List<BookingRequestDTO> BookingRequests { get; set; }

        public BookingRequestListDTO()
        {
            BookingRequests = new List<BookingRequestDTO>();
        }

        public void AddBooking(int accountId, int flightId, int cabinClassId, string cabinClassName, 
            string flightNumber, string departureCode, string arrivalCode, DateTime? departureTime,
            int ticketCount = 1)
        {
            BookingRequests.Add(new BookingRequestDTO
            {
                AccountId = accountId,
                FlightId = flightId,
                CabinClassId = cabinClassId,
                CabinClassName = cabinClassName,
                BookingDate = DateTime.Now,
                TicketCount = ticketCount,
                FlightNumber = flightNumber,
                DepartureAirportCode = departureCode,
                ArrivalAirportCode = arrivalCode,
                DepartureTime = departureTime
            });
        }
    }
}
