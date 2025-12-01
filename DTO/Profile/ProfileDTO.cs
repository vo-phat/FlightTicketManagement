using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Profile
{
    public class ProfileDTO
    {
        // ============================
        // ACCOUNT INFO
        // ============================

        public int? AccountId { get; set; }          // account_id
        public string? Email { get; set; }           // email
        public string? Password { get; set; }        // password (hash)
        public DateTime? CreatedAt { get; set; }     // created_at

        // ============================
        // PROFILE INFO
        // ============================

        public int? ProfileId { get; set; }          // profile_id
        public string? FullName { get; set; }        // full_name
        public DateTime? DateOfBirth { get; set; }   // date_of_birth
        public string? PhoneNumber { get; set; }     // phone_number
        public string? PassportNumber { get; set; }  // passport_number
        public string? Nationality { get; set; }     // nationality (VN/US/JP)
    }
}
