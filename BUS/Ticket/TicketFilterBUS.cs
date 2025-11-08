using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Ticket;
using DAO.TicketDAO;

namespace BUS.Ticket
{
   
    public class TicketFilterBUS
    {
        private TicketFilterDAO ticketFilterDAO = new TicketFilterDAO();

        public List<TicketFilterDTO> ReadListTicketsFilter()
        {
            return ticketFilterDAO.ListFilterTickets();
        }
    }
    
}
