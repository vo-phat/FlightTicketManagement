using DAO.TicketDAO;
using DTO.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Ticket
{
    public class NationalBUS
    {
        private readonly NationalDAO dao = new NationalDAO();

        public List<NationalDTO> GetAllNationals()
        {
            return dao.GetAll();
        }
    }
}
