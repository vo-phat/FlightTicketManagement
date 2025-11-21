using DTO.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.TicketDAO;
namespace BUS.Ticket
{
    public class TicketBUS
    {
        private readonly TicketDAO _ticketDAO;

        public TicketBUS()
        {
            _ticketDAO = new TicketDAO();
        }

        public List<TicketDTO> GetAllTickets()
        {
            //_ticketDAO = new TicketDAO();
            //Console.WriteLine("BUS: Đang gọi DAO để lấy tất cả vé...");
            return _ticketDAO.GetAllTickets();
        }     
    }
}
