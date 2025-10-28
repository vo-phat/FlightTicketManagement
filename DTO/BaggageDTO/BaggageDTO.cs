using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.BaggageDTO
{
    public class BaggageDTO
    {
        public String BaggageTag { get; set; }
        public int NumerTicket { get; set; }
        public int FightID { get; set; }
        public string Type { get; set; }
        public double ActualWeight { get; set; }
        public double AllowedWeight { get; set; }
        public string SpecialHandlingNotes { get; set; }
        public DateTime CreateTime{get; set;}

    }
}
