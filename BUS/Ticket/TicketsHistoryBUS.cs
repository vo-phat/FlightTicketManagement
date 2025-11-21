using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Ticket;
using DAO.TicketDAO;
namespace BUS.Ticket
{
    public class TicketsHistoryBUS
    {
        public List<TicketHistory> GetAllTicketHistories()
        {
            TicketHistoryDAO ticketHistoryDAO = new TicketHistoryDAO();
            return ticketHistoryDAO.GetAllTicketHistories();
        }
    }
}
