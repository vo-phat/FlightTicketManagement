using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.TicketDAO;
using DTO.Ticket;
namespace BUS.Ticket
{
    public class SaveTicketRequestBUS
    {
        // Placeholder for business logic to save ticket request
        public void SaveTicketRequest(List<TicketBookingRequestDTO> dto, int id ,string trip_type )
        {
            SaveTicketRequestDAO saveTicketRequestDAO = new SaveTicketRequestDAO();
            saveTicketRequestDAO.CreateBooking(dto , id, trip_type);
        }
    }
}
