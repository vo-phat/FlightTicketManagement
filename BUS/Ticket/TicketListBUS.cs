using DAO.TicketDAO;
using DTO.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Ticket
{
    public class TicketListBUS
    {
        TicketListDAO dao = new TicketListDAO();

        public List<TicketListDTO> GetAllTickets()
        {
            return dao.GetAllTickets();
        }

        public List<TicketListDTO> SearchTickets(string keyword, string status)
        {
            return dao.SearchTickets(keyword, status);
        }

        public bool CancelTicket(int id)
        {
            return dao.UpdateStatus(id, "CANCELLED");
        }

        public bool RefundTicket(int id)
        {
            return dao.UpdateStatus(id, "REFUNDED");
        }
    }


}
