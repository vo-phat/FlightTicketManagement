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
       public List<TicketHistory> GetAllTicketHistories()
        {
            using (var context = new DbFlightTicketContext())
            {
                return context.TicketsHistory.ToList();
            }
        }
    }
}
