using System;
using System.Collections.Generic;
using DTO.Account;
using DAO.Repositories;
using BUS.Security;
using BUS.Services;
using BUS.Validation;

namespace BUS.Auth {
    public class AuthService {
        private readonly AccountDao _accountDao = new AccountDao();

        // Lưu OTP trong bộ nhớ: email -> (code, expiredAt)
        private static readonly Dictionary<string, (string Code, DateTime ExpiredAt)> _otpStore
            = new Dictionary<string, (string, DateTime)>();

        // Đăng nhập, trả về AccountDto nếu thành công, ngược lại ném Exception
        public AccountDto Login(string email, string password) {
            AccountValidator.ValidateEmail(email);

            bool isRawAdmin = string.Equals(email.Trim(), "admin", StringComparison.OrdinalIgnoreCase);
            if (!isRawAdmin) {
                AccountValidator.ValidatePassword(password);
            } else {
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Mật khẩu không được để trống.");
            }

            var account = _accountDao.GetByEmail(email);

            if (account == null)
                throw new Exception("Tài khoản không tồn tại.");

            bool isAdminAccount = string.Equals(account.Email, "admin", StringComparison.OrdinalIgnoreCase);

            if (!isAdminAccount && !account.IsActive)
                throw new Exception("Tài khoản bạn đã bị khóa. Vui lòng liên hệ quản trị viên!!!");

            if (!PasswordHasher.Verify(password, account.Password)) {
                if (isAdminAccount) {
                    throw new Exception("Mật khẩu không chính xác.");
                }

                int remaining = account.FailedAttempts;

                if (remaining <= 1) {
                    _accountDao.DecreaseFailedAttempts(account.AccountId);
                    _accountDao.LockAccount(account.AccountId);
                    throw new Exception("Tài khoản bạn đã bị khóa. Vui lòng liên hệ quản trị viên!!!");
                } else {
                    _accountDao.DecreaseFailedAttempts(account.AccountId);
                    remaining--;
                    throw new Exception($"Mật khẩu không chính xác. Bạn còn {remaining} lần thử trước khi tài khoản bị khóa.");
                }
            }

            if (!isAdminAccount)
                _accountDao.ResetFailedAttemptsToDefault(account.AccountId);

            return account;
        }

        // Đăng ký
        public AccountDto Register(string email, string password) {
            AccountValidator.ValidateEmail(email);
            AccountValidator.ValidatePassword(password);

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
            AccountValidator.ValidateEmail(email);

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
            AccountValidator.ValidateEmail(email);
            AccountValidator.ValidatePassword(newPassword);

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
