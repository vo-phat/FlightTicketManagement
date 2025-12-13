using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
    public class TicketHistory
    {
        [Key]
        public int history_id { get; set; }
        public int ticket_id { get; set; }
        public string old_status { get; set; }
        public string new_status { get; set; }
        public DateTime changed_at { get; set; }
    }
}
