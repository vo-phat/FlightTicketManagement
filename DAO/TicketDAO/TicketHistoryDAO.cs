using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Ticket;
using DAO.TicketDAO;

namespace DAO.TicketDAO
{
    public class TicketHistoryDAO
    {
       public List<TicketHistoryDTO> GetAllTicketHistories()
        {
            using (var context = new DbFlightTicketContext())
            {

                var data = (from h in context.TicketsHistory
                            join t in context.Tickets
                                on h.ticket_id equals t.ticket_id
                            select new TicketHistoryDTO
                            {
                                TicketNumber = t.ticket_number,
                                issue_date = t.issue_date,
                                status = t.status,
                                old_status = h.old_status,
                                new_status = h.new_status,
                                changed_at = h.changed_at,
                              

                            }).ToList();
                return data;
            }
        }
    }
}
