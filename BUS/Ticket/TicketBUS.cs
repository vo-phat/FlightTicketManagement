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
            return _ticketDAO.GetAllTickets();
        }

        public List<TicketDTO> SearchTickets(string passengerName, string ticketNumber, TicketStatus status)
        {
            passengerName = passengerName?.Trim();
            ticketNumber = ticketNumber?.Trim().ToUpper();

            var allTickets = _ticketDAO.SearchTickets(passengerName, ticketNumber, status);

            var oneYearAgo = DateTime.Now.AddYears(-1);
            var filteredList = allTickets.Where(ticket => ticket.IssueDate >= oneYearAgo).ToList();

            return filteredList;
        }

        public TicketDTO GetTicketById(int ticketId)
        {
            if (ticketId <= 0)
            {
                return null;
            }
            return _ticketDAO.GetTicketById(ticketId);
        }

        public bool UpdateTicketStatus(int ticketId, TicketStatus newStatus)
        {
            var currentTicket = _ticketDAO.GetTicketById(ticketId);
            if (currentTicket == null)
            {
                return false;
            }

            if (currentTicket.Status == TicketStatus.CANCELED || currentTicket.Status == TicketStatus.REFUNDED)
            {
                return false;
            }

            if (newStatus == TicketStatus.CHECKED_IN && currentTicket.Status != TicketStatus.CONFIRMED)
            {
                return false;
            }

            return _ticketDAO.UpdateStatus(ticketId, newStatus);
        }
    }
}
