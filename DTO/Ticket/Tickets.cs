using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
    public class Tickets
    {
        public int ticket_id { get; set; }
        public int ticket_passenger_id { get; set; }
        public int flight_seat_id { get; set; }
        public string ticket_number { get; set; }
        public DateTime issue_date { get; set; }
        public string status { get; set; }
    }
}
