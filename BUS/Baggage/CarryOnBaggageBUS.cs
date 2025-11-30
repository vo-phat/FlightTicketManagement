using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.BaggageDAO;
using DTO.Baggage;
namespace BUS.Baggage
{
    public class CarryOnBaggageBUS
    {
        public List<CarryOnBaggageDTO> GetAllCarryOnBaggage()
        {
            CarryBaggageDAO carryOnBaggageDAO = new CarryBaggageDAO();
            return carryOnBaggageDAO.GetAllCarryOnBaggage();
        }
    }
}
