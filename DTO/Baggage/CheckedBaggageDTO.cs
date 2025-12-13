using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Baggage
{
    public class CheckedBaggageDTO
    {
        public int CheckedId { get; set; }
        public int WeightKg { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

}
