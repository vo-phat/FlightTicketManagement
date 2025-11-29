using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO.Baggage;
namespace BUS.Baggage
{
    public class CheckedBaggageBUS
    {
        public List<CheckedBaggageDTO> GetAllCheckedBaggage()
        {
            DAO.BaggageDAO.CheckedBaggageDAO checkedBaggageDAO = new DAO.BaggageDAO.CheckedBaggageDAO();
            return checkedBaggageDAO.GetAll();
        }
    }
}
