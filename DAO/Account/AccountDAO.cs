using DAO.Database;
using DTO.Account;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace DAO.Account
{
    public class AccountDAO : BaseDAO
    {
        // ✅ Lấy tất cả tài khoản
        public List<AccountDTO> GetAll()
        {
            var list = new List<AccountDTO>();
            string query = "SELECT account_id, email, password, created_at FROM accounts ORDER BY account_id;";

            ExecuteReader(query, reader => list.Add(MapReaderToDTO(reader)));
            return list;
        }

        // ✅ Lấy 1 tài khoản theo email
        public AccountDTO GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            AccountDTO dto = null;
            string query = "SELECT account_id, email, password, created_at FROM accounts WHERE email = @email LIMIT 1;";
            var parameters = new Dictionary<string, object> { { "@email", email } };

            ExecuteReader(query, reader => dto = MapReaderToDTO(reader), parameters);
            return dto;
        }

        // ✅ Kiểm tra email đã tồn tại chưa
        public bool Exists(string email)
        {
            string query = "SELECT COUNT(*) FROM accounts WHERE email = @email;";
            var parameters = new Dictionary<string, object> { { "@email", email } };
            int count = Convert.ToInt32(ExecuteScalar(query, parameters));
            return count > 0;
        }

        // ✅ Thêm tài khoản mới (mã hóa mật khẩu trước khi lưu)
        public bool Insert(AccountDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Email hoặc mật khẩu không hợp lệ.");

            if (Exists(dto.Email))
                throw new InvalidOperationException("Email đã được sử dụng.");

            string query = "INSERT INTO accounts (email, password) VALUES (@email, @password);";
            var parameters = new Dictionary<string, object>
            {
                { "@email", dto.Email },
                { "@password", HashPassword(dto.Password) } // 🔐 SHA256 mã hóa
            };

            return ExecuteNonQuery(query, parameters) > 0;
        }

        // ✅ Xác thực đăng nhập (so sánh hash)
        public AccountDTO Authenticate(string email, string plainPassword)
        {
            AccountDTO dto = null;

            string query = @"
        SELECT a.account_id, a.email, a.password, a.created_at, r.role_name
        FROM accounts a
        LEFT JOIN user_role ur ON a.account_id = ur.account_id
        LEFT JOIN roles r ON ur.role_id = r.role_id
        WHERE a.email = @email
        LIMIT 1;
    ";

            var parameters = new Dictionary<string, object> { { "@email", email } };

            ExecuteReader(query, reader =>
            {
                string dbPassword = GetString(reader, "password");
                string hashedInput = HashPassword(plainPassword);

                if (dbPassword == hashedInput)
                {
                    dto = new AccountDTO(
                        GetInt32(reader, "account_id"),
                        GetString(reader, "email"),
                        dbPassword,
                        GetDateTime(reader, "created_at") ?? DateTime.Now
                    );
                    dto.RoleName = GetString(reader, "role_name"); // 👈 Thêm dòng này
                }
            }, parameters);

            return dto;
        }




        // ✅ Đổi mật khẩu (có mã hóa lại)
        public bool ChangePassword(int accountId, string currentPassword, string newPassword)
        {
            // 🔹 Lấy hash từ DB
            string querySelect = "SELECT password FROM accounts WHERE account_id = @id LIMIT 1;";
            var parameters = new Dictionary<string, object> { { "@id", accountId } };
            string dbPassword = null;

            ExecuteReader(querySelect, reader => dbPassword = GetString(reader, "password"), parameters);

            if (dbPassword == null)
                throw new Exception("Không tìm thấy tài khoản.");

            // 🔹 Hash mật khẩu hiện tại
            string hashedCurrent = HashPassword(currentPassword);

            if (hashedCurrent != dbPassword)
                throw new UnauthorizedAccessException("Mật khẩu hiện tại không đúng.");

            // 🔹 Hash và cập nhật mật khẩu mới
            string queryUpdate = "UPDATE accounts SET password = @newPass WHERE account_id = @id;";
            var updateParams = new Dictionary<string, object>
    {
        { "@newPass", HashPassword(newPassword) },
        { "@id", accountId }
    };

            return ExecuteNonQuery(queryUpdate, updateParams) > 0;
        }


        // ✅ Hàm băm SHA-256
        private string HashPassword(string input)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder();
                foreach (var b in bytes)
                    sb.Append(b.ToString("x2")); // hex chuỗi
                return sb.ToString();
            }
        }

        // ✅ Map MySQL → DTO
        private AccountDTO MapReaderToDTO(MySqlDataReader reader)
        {
            int id = GetInt32(reader, "account_id");
            string email = GetString(reader, "email");
            string password = GetString(reader, "password");
            DateTime createdAt = GetDateTime(reader, "created_at") ?? DateTime.Now;
            return new AccountDTO(id, email, password, createdAt);
        }
    }
}
