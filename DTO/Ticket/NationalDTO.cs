using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
    public class NationalDTO
    {
        public int NationalId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string PhoneCode { get; set; }

        // Hiển thị đẹp trên ComboBox
        public string DisplayName => $"{CountryName} ({CountryCode})";
    }

}
