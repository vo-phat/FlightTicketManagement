using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
        public class RefundTicketDTO
    {
        public int AdminId { get; set; }      // người xử lý hoàn vé
        public int TicketId { get; set; }
        public string Status { get; set; }        // CANCELLED
        public decimal TicketPrice { get; set; }

        public bool IsRefundable { get; set; }

        public int RefundFeePercent { get; set; } // 0, 10, 20...
    }
}
