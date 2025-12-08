using DAO.Seat;
using DTO.Seat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Seat
{
    public class OpenSeatSelectorBUS
    {
        private readonly OpenSeatSelectorDAO _dao = new();

        public List<SeatSelectDTO> GetOpenSeats(int flightId, int classId)
        {
            return _dao.GetOpenSeats(flightId, classId);
        }
    }
}
