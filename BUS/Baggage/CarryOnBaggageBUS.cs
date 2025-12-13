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
        private readonly CarryOnBaggageDAO dao = new CarryOnBaggageDAO();

        public List<CarryOnBaggageDTO> GetAll()
        {
            return dao.GetAll();
        }

        public bool Add(CarryOnBaggageDTO dto)
        {
            // Nếu set default → clear default cùng class trước
            if (dto.IsDefault)
                dao.RemoveDefaultForClass(dto.ClassId);

            return dao.Insert(dto);
        }

        public bool Update(CarryOnBaggageDTO dto)
        {
            if (dto.IsDefault)
                dao.RemoveDefaultForClass(dto.ClassId);

            return dao.Update(dto);
        }

        public bool Delete(int id)
        {
            return dao.Delete(id);
        }
    }

}
