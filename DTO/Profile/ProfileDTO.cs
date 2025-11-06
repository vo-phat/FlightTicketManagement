using System;

namespace DTO.Profile
{
    public class ProfileDTO
    {
        private int _profileId;
        private int _accountId;
        private string _fullName;
        private DateTime? _dateOfBirth;
        private string _phoneNumber;
        private string _passportNumber;
        private string _nationality;

        // 🧩 Mở rộng (JOIN thêm)
        private string _email;      // từ bảng Accounts
        private string _roleName;   // từ Roles/User_Role nếu cần

        // ======== PROPERTIES ========

        public int ProfileId
        {
            get => _profileId;
            set => _profileId = value;
        }

        public int AccountId
        {
            get => _accountId;
            set => _accountId = value;
        }

        public string FullName
        {
            get => _fullName;
            set => _fullName = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set => _dateOfBirth = value;
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string PassportNumber
        {
            get => _passportNumber;
            set => _passportNumber = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string Nationality
        {
            get => _nationality;
            set => _nationality = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        // 🧩 mở rộng
        public string Email
        {
            get => _email;
            set => _email = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string RoleName
        {
            get => _roleName;
            set => _roleName = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        // ======== CONSTRUCTORS ========

        public ProfileDTO() { }

        public ProfileDTO(
            int profileId, int accountId, string fullName,
            DateTime? dateOfBirth, string phoneNumber,
            string passportNumber, string nationality,
            string email = null, string roleName = null)
        {
            ProfileId = profileId;
            AccountId = accountId;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            PassportNumber = passportNumber;
            Nationality = nationality;
            Email = email;
            RoleName = roleName;
        }

        // ======== OVERRIDES ========

        public override string ToString() =>
            $"{FullName ?? "Chưa có tên"}" + (string.IsNullOrEmpty(RoleName) ? "" : $" ({RoleName})");

        public override bool Equals(object obj)
        {
            if (obj is not ProfileDTO other) return false;
            return _profileId == other._profileId;
        }

        public override int GetHashCode() => _profileId.GetHashCode();
    }
}
