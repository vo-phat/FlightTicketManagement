using System;
using System.Text.RegularExpressions;
using DTO.Profile;

namespace BUS.Validation {
    public static class ProfileValidator {
        // Regex đơn giản cho email
        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public static void ValidateProfile(ProfileInfoDto dto) {
            if (dto == null)
                throw new ArgumentException("Dữ liệu hồ sơ không hợp lệ.");

            // Email
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new Exception("Email không được để trống.");

            if (!EmailRegex.IsMatch(dto.Email))
                throw new Exception("Định dạng email không hợp lệ.");

            // Họ tên
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new Exception("Họ và tên không được để trống.");

            if (dto.FullName.Length > 255)
                throw new Exception("Họ và tên quá dài.");

            // Ngày sinh (không được lớn hơn hiện tại)
            if (dto.DateOfBirth.HasValue && dto.DateOfBirth.Value.Date > DateTime.Today)
                throw new Exception("Ngày sinh không được lớn hơn ngày hiện tại.");

            // Số điện thoại
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber) &&
                dto.PhoneNumber.Length > 20)
                throw new Exception("Số điện thoại quá dài.");

            // Số hộ chiếu
            if (!string.IsNullOrWhiteSpace(dto.PassportNumber) &&
                dto.PassportNumber.Length > 50)
                throw new Exception("Số hộ chiếu quá dài.");

            // Quốc tịch
            if (!string.IsNullOrWhiteSpace(dto.Nationality) &&
                dto.Nationality.Length > 50)
                throw new Exception("Quốc tịch quá dài.");
        }

        public static void ValidateChangePassword(string currentPassword, string newPassword) {
            if (string.IsNullOrWhiteSpace(currentPassword))
                throw new Exception("Mật khẩu hiện tại không được để trống.");

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new Exception("Mật khẩu mới không được để trống.");

            if (newPassword.Length < 6)
                throw new Exception("Mật khẩu mới phải có ít nhất 6 ký tự.");

            if (newPassword == currentPassword)
                throw new Exception("Mật khẩu mới phải khác mật khẩu hiện tại.");
        }
    }
}
