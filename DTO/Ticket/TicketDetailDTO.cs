using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
    namespace DTO.Ticket
    {
        public class TicketDetailDTO
        {
            // Ticket
            public int TicketId { get; set; }
            public string TicketNumber { get; set; }
            public string Status { get; set; }
            public decimal TotalPrice { get; set; }

            // Passenger
            public string PassengerName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string PassportNumber { get; set; }
            public string Nationality { get; set; }

            // Flight
            public string FlightNumber { get; set; }
            public DateTime DepartureTime { get; set; }
            public DateTime ArrivalTime { get; set; }
            public string Route { get; set; }

            // Seat
            public string SeatNumber { get; set; }
            public string CabinClass { get; set; }

            // Refund policy
            public bool IsRefundable { get; set; }
            public int RefundFeePercent { get; set; }

            // Booking (để xem payment / history)
            public int BookingId { get; set; }
        }
    }

}
