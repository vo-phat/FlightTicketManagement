using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BUS.Validation {
    public static class AccountValidator {
        public static string ValidateEmail(string raw) {
            if (string.IsNullOrWhiteSpace(raw))
                throw new ArgumentException("Email/Tài khoản không được để trống.");

            raw = raw.Trim();

            // Case đặc biệt "admin"
            if (raw.Equals("admin", StringComparison.OrdinalIgnoreCase))
                return "admin";

            // Không dài quá
            if (raw.Length > 100)
                throw new ArgumentException("Email quá dài (tối đa 100 ký tự).");

            // Basic strict email rule (dễ hiểu hơn MailAddress)
            if (!Regex.IsMatch(raw, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
                throw new ArgumentException("Email không hợp lệ. Vui lòng nhập đúng định dạng.");

            // Chuẩn hóa theo MailAddress (lowercase domain)
            var addr = new MailAddress(raw);
            return addr.Address;
        }

        public static void ValidatePassword(string password) {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Mật khẩu không được để trống.");

            if (password.Length < 6)
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự.");
        }
    }
}
