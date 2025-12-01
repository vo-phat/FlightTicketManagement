using BUS.Security;
using BUS.Validation;
using DAO.Repositories;
using DTO.Account;
using DTO.Profile;
using System;

namespace BUS.Profile {
    /// <summary>
    /// Lớp trung gian giữa GUI và DAO cho chức năng hồ sơ cá nhân + đổi mật khẩu.
    /// </summary>
    public class ProfileService {
        private readonly ProfileDao _profileDao;
        private readonly AccountDao _accountDao;

        public ProfileService() {
            _profileDao = new ProfileDao();
            _accountDao = new AccountDao();
        }

        /// <summary>
        /// Lấy thông tin hồ sơ theo accountId.
        /// ProfileDao đã JOIN accounts + Passenger_Profiles.
        /// </summary>
        public ProfileInfoDto GetProfile(int accountId) {
            if (accountId <= 0)
                throw new ArgumentException("accountId không hợp lệ.");

            var dto = _profileDao.GetByAccountId(accountId);

            // Nếu không có record nào (accounts không tồn tại) thì báo lỗi
            if (dto == null)
                throw new Exception("Không tìm thấy tài khoản.");

            return dto;
        }

        /// <summary>
        /// Cập nhật hồ sơ cá nhân:
        /// - Validate bằng ProfileValidator
        /// - Gọi ProfileDao.Save để update accounts.email + Passenger_Profiles.*
        /// </summary>
        public void UpdateProfile(ProfileInfoDto dto) {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Business validation
            ProfileValidator.ValidateProfile(dto);

            // Lưu xuống DB
            _profileDao.Save(dto);
        }

        /// <summary>
        /// Đổi mật khẩu:
        /// - Validate mật khẩu mới bằng ProfileValidator.ValidateChangePassword
        /// - Lấy account hiện tại (cần có hash mật khẩu)
        /// - So sánh mật khẩu hiện tại
        /// - Hash mật khẩu mới và update qua AccountDao.UpdatePassword
        /// </summary>
        public void ChangePassword(int accountId, string currentPassword, string newPassword) {
            if (accountId <= 0)
                throw new ArgumentException("accountId không hợp lệ.");

            // Kiểm tra format mật khẩu
            ProfileValidator.ValidateChangePassword(currentPassword, newPassword);

            // TODO: tùy bạn đang có DAO nào để lấy account theo Id.
            // Cách đơn giản: thêm method GetById trong AccountDao (gần giống GetByEmail)
            AccountDto? account = GetAccountById(accountId);
            if (account == null)
                throw new Exception("Không tìm thấy tài khoản.");

            // Kiểm tra mật khẩu hiện tại
            if (!PasswordHasher.Verify(currentPassword, account.Password))
                throw new Exception("Mật khẩu hiện tại không chính xác.");

            // Hash mật khẩu mới
            string newHash = PasswordHasher.Hash(newPassword);

            // Cập nhật vào DB
            _accountDao.UpdatePassword(accountId, newHash);
        }

        /// <summary>
        /// Hàm wrapper tách riêng để dễ thay đổi nếu sau này bạn dùng cách lấy khác.
        /// YÊU CẦU: bạn hiện thực thêm GetById trong AccountDao (DAO layer).
        /// </summary>
        private AccountDto? GetAccountById(int accountId) {
             return _accountDao.GetById(accountId);
        }
    }
}
