using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Seat
{
    public class SeatSelectDTO
    {
        public int FlightSeatId { get; set; }
        public string SeatNumber { get; set; }
        public decimal Price { get; set; }
        public int ClassId { get; set; }
        public string SeatStatus { get; set; }  // thêm dòng này
    }

}
