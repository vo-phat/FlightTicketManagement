using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Profile
{
    public class ProfileDTO
    {
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? NumberPhone { get; set; }
        public string? Email { get; set; }
        public string? Passport { get; set; }
        public string? Nationality { get; set; }
        public DateTime FlightDate { get; set; }
        public string? NumberSeat { get; set; }

    }
}
