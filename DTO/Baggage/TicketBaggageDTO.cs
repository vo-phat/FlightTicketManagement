using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Baggage
{
    public class TicketBaggageDTO
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string BaggageType { get; set; } // carry_on / checked
        public int? CarryOnId { get; set; }
        public int? CheckedId { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }

        // Dùng khi JOIN để hiển thị
        public int Kg { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
