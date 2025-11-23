namespace DTO.Auth {
    public class AccountDto {
        private int _accountId;
        private string _email;
        private string _password;
        private int _failedAttempts = 5;
        private bool _isActive;
        private DateTime _createdAt;

        public const int DEFAULT_FAILED_ATTEMPTS = 5;

        public int AccountId {
            get => _accountId;
            set {
                if (value < 0) throw new ArgumentException("AccountId không hợp lệ.");
                _accountId = value;
            }
        }

        public string Email {
            get => _email;
            set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email/Tài khoản không được để trống.");

                value = value.Trim();

                // Trường hợp đặc biệt duy nhất: admin
                if (value.Equals("admin", StringComparison.OrdinalIgnoreCase)) {
                    _email = "admin";
                    return;
                }

                // Các trường hợp còn lại bắt buộc phải là email hợp lệ
                if (value.Length > 100)
                    throw new ArgumentException("Email quá dài (tối đa 100 ký tự).");

                try {
                    var addr = new System.Net.Mail.MailAddress(value);
                    _email = addr.Address; // email hợp lệ
                } catch {
                    throw new ArgumentException("Email không hợp lệ. Vui lòng nhập đúng định dạng.");
                }
            }
        }

        public string Password {
            get => _password;
            set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mật khẩu không được để trống.");

                if (value.Length < 6)
                    throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự.");

                _password = value;
            }
        }

        public int FailedAttempts {
            get => _failedAttempts;
            set {
                if (value < 0) value = 0;
                _failedAttempts = value;
            }
        }

        public bool IsActive {
            get => _isActive;
            set => _isActive = value;
        }

        public DateTime CreatedAt {
            get => _createdAt;
            set => _createdAt = value;
        }
    }
}
