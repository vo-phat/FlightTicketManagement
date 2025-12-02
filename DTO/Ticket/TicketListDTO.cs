using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
    public class TicketListDTO
    {
        public int TicketId { get; set; }
        public string TicketNumber { get; set; }
        public string PassengerName { get; set; }
        public string FlightNumber { get; set; }
        public string Route { get; set; }
        public DateTime DepartureTime { get; set; }
        public string SeatCode { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}
