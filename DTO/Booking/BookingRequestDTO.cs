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
        public int TicketQuantity { get; set; } = 1; // Số lượng vé đặt
        public DateTime BookingDate { get; set; }
        
        // Thông tin chuyến bay (để hiển thị)
        public string FlightNumber { get; set; }
        public string DepartureAirportCode { get; set; }
        public string ArrivalAirportCode { get; set; }
        public DateTime? DepartureTime { get; set; }

        /// <summary>
        /// Helper method: Trả về thông tin cần thiết cho việc đặt vé
        /// Returns: (FlightId, CabinClassId, TicketQuantity)
        /// </summary>
        public (int FlightId, int CabinClassId, int Quantity) GetBookingInfo()
        {
            return (FlightId, CabinClassId, TicketQuantity);
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
            string flightNumber, string departureCode, string arrivalCode, DateTime? departureTime)
        {
            BookingRequests.Add(new BookingRequestDTO
            {
                AccountId = accountId,
                FlightId = flightId,
                CabinClassId = cabinClassId,
                CabinClassName = cabinClassName,
                BookingDate = DateTime.Now,
                FlightNumber = flightNumber,
                DepartureAirportCode = departureCode,
                ArrivalAirportCode = arrivalCode,
                DepartureTime = departureTime
            });
        }
    }
}
