namespace DTO.Profile {
    public class ProfileInfoDto {
        public int AccountId { get; set; }
        public string Email { get; set; } = "";
        public string FullName { get; set; } = "";
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string PassportNumber { get; set; } = "";
        public string Nationality { get; set; } = "";
    }
}
