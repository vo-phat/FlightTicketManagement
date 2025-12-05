using DAO.BaggageDAO;
using DTO.Baggage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Baggage
{
    public class TicketBaggageBUS
    {
        private readonly TicketBaggageDAO dao = new TicketBaggageDAO();

        public List<TicketBaggageDTO> GetByTicketId(int ticketId)
        {
            return dao.GetByTicketId(ticketId);
        }

        public bool Add(TicketBaggageDTO dto)
        {
            return dao.Insert(dto);
        }

        public bool Delete(int id)
        {
            return dao.Delete(id);
        }
    }
}
