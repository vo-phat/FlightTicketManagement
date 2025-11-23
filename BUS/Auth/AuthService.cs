using System;
using System.Collections.Generic;
using DTO.Auth;
using DAO.Account;
using BUS.Security;
using BUS.Services;

namespace BUS.Auth {
    public class AuthService {
        private readonly AccountDao _accountDao = new AccountDao();

        // Lưu OTP trong bộ nhớ: email -> (code, expiredAt)
        private static readonly Dictionary<string, (string Code, DateTime ExpiredAt)> _otpStore
            = new Dictionary<string, (string, DateTime)>();

        // Đăng nhập, trả về AccountDto nếu thành công, ngược lại ném Exception
        public AccountDto Login(string email, string password) {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Tài khoản không được để trống.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Mật khẩu không được để trống.");

            var account = _accountDao.GetByEmail(email);
            if (account == null)
                throw new Exception("Tài khoản không tồn tại.");

            bool isAdminAccount = string.Equals(account.Email, "admin", StringComparison.OrdinalIgnoreCase);

            // Nếu không phải admin thì kiểm tra khóa
            if (!isAdminAccount && !account.IsActive) {
                throw new Exception("Tài khoản bạn đã bị khóa. Vui lòng liên hệ quản trị viên!!!");
            }

            // Nếu mật khẩu sai
            if (!PasswordHasher.Verify(password, account.Password)) {

                // Admin: chỉ báo sai mật khẩu, không trừ lượt / không khóa
                if (isAdminAccount) {
                    throw new Exception("Mật khẩu không chính xác.");
                }

                // Tài khoản thường: trừ lượt
                int remaining = account.FailedAttempts;

                if (remaining <= 1) {
                    // Lần này sai nữa là hết lượt -> khóa
                    _accountDao.DecreaseFailedAttempts(account.AccountId); // về 0
                    _accountDao.LockAccount(account.AccountId);

                    throw new Exception("Tài khoản bạn đã bị khóa. Vui lòng liên hệ quản trị viên!!!");
                } else {
                    // Trừ 1 và báo còn bao nhiêu
                    _accountDao.DecreaseFailedAttempts(account.AccountId);
                    remaining = remaining - 1;

                    throw new Exception($"Mật khẩu không chính xác. Bạn còn {remaining} lần thử trước khi tài khoản bị khóa.");
                }
            }

            // Đúng mật khẩu
            if (!isAdminAccount) {
                _accountDao.ResetFailedAttemptsToDefault(account.AccountId);
            }

            return account;
        }

        // Đăng ký
        public AccountDto Register(string email, string password) {
            if (_accountDao.ExistsByEmail(email))
                throw new Exception("Tài khoản đã tồn tại.");

            // Validation ở DTO setter
            var dto = new AccountDto {
                Email = email,
                Password = PasswordHasher.Hash(password),
                FailedAttempts = AccountDto.DEFAULT_FAILED_ATTEMPTS,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            int newId = _accountDao.Create(dto);
            dto.AccountId = newId;
            return dto;
        }

        // Gửi mã OTP reset password
        public void SendResetPasswordCode(string email) {
            var account = _accountDao.GetByEmail(email);
            if (account == null)
                throw new Exception("Tài khoản không tồn tại.");

            // Tạo mã 6 số
            var random = new Random();
            string code = random.Next(100000, 999999).ToString();

            var expiredAt = DateTime.Now.AddMinutes(10);

            _otpStore[email] = (code, expiredAt);

            // Gửi email
            EmailService.SendOtp(email, code);
        }

        // Xác nhận OTP và đổi mật khẩu
        public void ResetPassword(string email, string otpCode, string newPassword, string confirmPassword) {
            if (!_otpStore.TryGetValue(email, out var otpInfo))
                throw new Exception("Bạn chưa yêu cầu mã xác thực hoặc mã đã hết hạn.");

            if (otpInfo.ExpiredAt < DateTime.Now)
                throw new Exception("Mã xác thực đã hết hạn. Vui lòng yêu cầu lại mã mới.");

            if (!string.Equals(otpInfo.Code, otpCode?.Trim(), StringComparison.Ordinal))
                throw new Exception("Mã xác thực không chính xác.");

            if (newPassword != confirmPassword)
                throw new Exception("Mật khẩu xác nhận không khớp.");

            var account = _accountDao.GetByEmail(email);
            if (account == null)
                throw new Exception("Tài khoản không tồn tại.");

            // Validation ở DTO setter
            account.Password = PasswordHasher.Hash(newPassword);
            _accountDao.UpdatePassword(account.AccountId, account.Password);

            // Xóa OTP sau khi dùng
            _otpStore.Remove(email);
        }
    }
}
