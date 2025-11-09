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

        public List<TicketFilterDTO> ListTenTicketsNews()
        {
            return ticketFilterDAO.ReadListTenTicketsNews();
        }

        public List<TicketFilterDTO> ListFilterTickets(
            string? BookingCodeID,
            string? FlightCodeID,
            DateTime? NgayBay,
            string? Status,
            string? PhoneNumber)
        {
            return ticketFilterDAO.ReadListFilterTickets(
                BookingCodeID,
                FlightCodeID,
                NgayBay,
                Status,
                PhoneNumber);
        }

    }
    
}
