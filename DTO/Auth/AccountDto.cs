namespace DTO.Account {
    public class AccountDto {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public const int DEFAULT_FAILED_ATTEMPTS = 5;
    }

}
