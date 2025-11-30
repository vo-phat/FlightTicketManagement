using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
    public class TicketHistoryDTO
    {
        public string TicketNumber { get; set; }
        public DateTime issue_date { get; set; }
        public string status { get; set; }
        public string old_status { get; set; }
        public string new_status { get; set; }
        public DateTime changed_at { get; set; }
    }
}