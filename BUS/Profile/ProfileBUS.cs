using DAO.Profile;
using DAO.Account;
using DTO.Profile;
using System;
using System.Text.RegularExpressions;

namespace BUS.Profile
{
    public class ProfileBUS
    {
        private readonly ProfileDAO profileDao = new ProfileDAO();
        private readonly AccountDAO accountDao = new AccountDAO();

        public ProfileDTO GetProfileByAccountId(int accountId)
        {
            if (accountId <= 0)
                throw new ArgumentException("ID tài khoản không hợp lệ.");

            return profileDao.GetProfileByAccountId(accountId);
        }

        public bool UpdateProfile(ProfileDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // 🧩 Kiểm tra email
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email không được để trống.");

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(dto.Email))
                throw new ArgumentException("Định dạng email không hợp lệ.");

            // 🔎 Kiểm tra email đã tồn tại ở tài khoản khác chưa
            var existing = accountDao.GetByEmail(dto.Email);
            if (existing != null && existing.AccountId != dto.AccountId)
                throw new ArgumentException("Email này đã được sử dụng bởi tài khoản khác.");

            // 🧩 Kiểm tra họ tên
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("Họ tên không được để trống.");

            if (dto.FullName.Length > 100)
                throw new ArgumentException("Họ tên không được vượt quá 100 ký tự.");

            // 🧩 Kiểm tra ngày sinh
            if (dto.DateOfBirth.HasValue && dto.DateOfBirth.Value > DateTime.Now)
                throw new ArgumentException("Ngày sinh không được lớn hơn ngày hiện tại.");

            // 🧩 Kiểm tra số điện thoại
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                var phoneRegex = new Regex(@"^[0-9]{9,10}$");
                if (!phoneRegex.IsMatch(dto.PhoneNumber))
                    throw new ArgumentException("Số điện thoại không hợp lệ (chỉ gồm 9–10 chữ số).");
            }

            // 🧩 Kiểm tra hộ chiếu
            if (!string.IsNullOrWhiteSpace(dto.PassportNumber) && dto.PassportNumber.Length > 20)
                throw new ArgumentException("Số hộ chiếu không được vượt quá 20 ký tự.");

            // 🧩 Kiểm tra quốc tịch
            if (!string.IsNullOrWhiteSpace(dto.Nationality) && dto.Nationality.Length > 50)
                throw new ArgumentException("Tên quốc tịch không được vượt quá 50 ký tự.");

            // ✅ Gửi xuống DAO cập nhật cả 2 bảng:
            // - Accounts (email)
            // - Passenger_Profiles (thông tin cá nhân)
            return profileDao.UpdateProfile(dto);
        }
    }
}
