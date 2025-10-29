using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.BaggageDTO
{
    public class Ticket
    {
        // Tương ứng ô "Số vé"
        public int TicketNumber { get; set; }

        // Các thuộc tính tiềm năng khác
        public int FlightId { get; set; } // Để liên kết đến chuyến bay
        public string PassengerName { get; set; }
        public string SeatNumber { get; set; }
        public string TicketClass { get; set; } // Hạng vé: Economy, Business
        public string Status { get; set; } // Ví dụ: CONFIRMED, CHECKED_IN
    }
}
