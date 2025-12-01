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
        public string SizeLimit { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }

        public CarryOnBaggageDTO() { }

        public CarryOnBaggageDTO(int carryOnId, int weightKg, string sizeLimit, string description, bool isDefault)
        {
            CarryOnId = carryOnId;
            WeightKg = weightKg;
            SizeLimit = sizeLimit;
            Description = description;
            IsDefault = isDefault;
        }
    }
}
