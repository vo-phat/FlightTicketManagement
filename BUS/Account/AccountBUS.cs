using DAO.Account;
using DTO.Account;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BUS.Account
{
    public class AccountBUS
    {
        private readonly AccountDAO dao = new AccountDAO();

        // 🔹 Đăng nhập
        public AccountDTO Authenticate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email không được để trống.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Mật khẩu không được để trống.");

            // Kiểm tra định dạng email
            if (!IsValidEmail(email))
                throw new ArgumentException("Định dạng email không hợp lệ.");

            var acc = dao.Authenticate(email.Trim(), password);
            if (acc == null)
                throw new UnauthorizedAccessException("Sai email hoặc mật khẩu.");

            return acc;
        }

        // 🔹 Đăng ký tài khoản mới
        public bool Insert(AccountDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("Dữ liệu tài khoản không hợp lệ.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email không được để trống.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Mật khẩu không được để trống.");

            if (!IsValidEmail(dto.Email))
                throw new ArgumentException("Định dạng email không hợp lệ.");

            if (!IsStrongPassword(dto.Password))
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự, gồm cả chữ và số.");

            if (dao.Exists(dto.Email))
                throw new InvalidOperationException("Email đã được sử dụng.");

            return dao.Insert(dto);
        }

        // 🔹 Cập nhật mật khẩu
        public bool ChangePassword(int accountId, string currentPassword, string newPassword)
        {
            if (accountId <= 0)
                throw new ArgumentException("ID tài khoản không hợp lệ.");

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Mật khẩu mới không được để trống.");

            if (!IsStrongPassword(newPassword))
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự, gồm cả chữ và số.");

            return dao.ChangePassword(accountId, currentPassword, newPassword);
        }

        // 🔹 Lấy toàn bộ tài khoản (chỉ cho admin)
        public List<AccountDTO> GetAll()
        {
            return dao.GetAll();
        }

        // ======================================
        // 🔸 HÀM HỖ TRỢ VALIDATION
        // ======================================

        // Kiểm tra định dạng email
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        // Kiểm tra mật khẩu có đủ mạnh không
        private bool IsStrongPassword(string password)
        {
            return password.Length >= 6 &&
                   Regex.IsMatch(password, @"[A-Za-z]") &&
                   Regex.IsMatch(password, @"\d");
        }
    }
}
