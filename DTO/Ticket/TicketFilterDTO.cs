using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
     public  class TicketFilterDTO
    {
        public string BookingCode { get; set; }
        public string FlightCode { get; set; }
        public DateTime? FlightDate { get; set; }
        public string ContactInfo { get; set; }
        public string Status { get; set; }

    }
}
