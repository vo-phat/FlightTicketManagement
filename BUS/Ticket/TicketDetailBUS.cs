using DAO.TicketDAO;
using DTO.Ticket;
using DTO.Ticket.DTO.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Ticket
{
    public class TicketDetailBUS
    {
        private readonly TicketDetailDAO _dao = new TicketDetailDAO();

        public TicketDetailDTO GetTicketDetail(int ticketId)
        {
            if (ticketId <= 0)
                throw new ArgumentException("TicketId không hợp lệ");

            var detail = _dao.GetTicketDetail(ticketId);

            if (detail == null)
                throw new Exception("Không tìm thấy vé");

            return detail;
        }
    }
}

