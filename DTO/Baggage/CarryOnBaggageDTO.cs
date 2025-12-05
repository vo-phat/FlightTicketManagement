using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Baggage
{
    public class CarryOnBaggageDTO
    {
        public int CarryOnId { get; set; }
        public int WeightKg { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }   // thêm mới
        public string SizeLimit { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
    }


}
