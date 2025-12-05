using DTO.Baggage;
using DAO.BaggageDAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BUS.Baggage
{
    public class CheckedBaggageBUS
    {
        private readonly CheckedBaggageDAO dao = new CheckedBaggageDAO();

        public List<CheckedBaggageDTO> GetAll()
        {
            return dao.GetAll();
        }

        public bool Add(CheckedBaggageDTO dto)
        {
            return dao.Insert(dto);
        }

        public bool Update(CheckedBaggageDTO dto)
        {
            return dao.Update(dto);
        }

        public bool Delete(int id)
        {
            return dao.Delete(id);
        }
        public List<CheckedBaggageDTO> GetAllCheckedBaggage()
        {
            DAO.BaggageDAO.CheckedBaggageDAO checkedBaggageDAO = new DAO.BaggageDAO.CheckedBaggageDAO();
            return checkedBaggageDAO.GetAll();
        }
    }
}
