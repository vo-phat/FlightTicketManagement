using System;
using System.Text.RegularExpressions;

namespace DTO.Account
{
    public class AccountDTO
    {
        #region Private Fields
        private int _accountId;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private DateTime _createdAt;
        public string RoleName { get; set; }
        #endregion

        #region Public Properties
        public int AccountId
        {
            get => _accountId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Account ID không thể âm");
                _accountId = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email không được trống");

                var trimmed = value.Trim();
                if (trimmed.Length > 100)
                    throw new ArgumentException("Email không quá 100 ký tự");

                _email = trimmed;
            }
        }

        // Note: DTO stores the password value as provided (hashed handling belongs to service layer)
        public string Password
        {
            get => _password;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(Password));

                var trimmed = value.Trim();
                if (trimmed.Length > 255)
                    throw new ArgumentException("Password không quá 255 ký tự");

                _password = trimmed;
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }
        #endregion

        #region Constructors
        public AccountDTO()
        {
            _createdAt = DateTime.UtcNow;
        }

        public AccountDTO(string email, string password) : this()
        {
            Email = email;
            Password = password;
        }

        public AccountDTO(int accountId, string email, string password, DateTime createdAt)
        {
            AccountId = accountId;
            Email = email;
            Password = password;
            CreatedAt = createdAt;
        }
        #endregion

        #region Validation
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(_email))
            {
                errorMessage = "Email không được trống";
                return false;
            }

            if (_email.Length > 100)
            {
                errorMessage = "Email không quá 100 ký tự";
                return false;
            }

            if (!Regex.IsMatch(_email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorMessage = "Email không hợp lệ";
                return false;
            }

            if (string.IsNullOrEmpty(_password) || _password.Length < 6)
            {
                errorMessage = "Password ít nhất 6 ký tự";
                return false;
            }

            if (_password.Length > 255)
            {
                errorMessage = "Password không quá 255 ký tự";
                return false;
            }

            return true;
        }
        #endregion

        #region Overrides
        public override string ToString() => $"Account #{_accountId}: {_email}";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (AccountDTO)obj;
            return _accountId == other._accountId;
        }

        public override int GetHashCode() => _accountId.GetHashCode();
        #endregion

        #region Helper
        public AccountDTO Clone() =>
            new AccountDTO(_accountId, _email, _password, _createdAt);
        #endregion
    }
}
