using System;
using System.Collections.Generic;
using DAO.Database;
using DTO.Profile;
using MySqlConnector;

namespace DAO.Repositories
{
    public class ProfileDao : BaseDAO
    {
        private const string ACCOUNT_TABLE = "accounts";
        private const string PROFILE_TABLE = "Passenger_Profiles";

        /// <summary>
        /// Lấy thông tin profile theo account_id
        /// JOIN accounts + Passenger_Profiles để lấy Email + thông tin hồ sơ.
        /// </summary>
        public ProfileInfoDto GetByAccountId(int accountId)
        {
            ProfileInfoDto profile = null;

            string query = $@"
                SELECT a.account_id,
                       a.email,
                       p.full_name,
                       p.date_of_birth,
                       p.phone_number,
                       p.passport_number,
                       p.nationality
                FROM {ACCOUNT_TABLE} a
                LEFT JOIN {PROFILE_TABLE} p ON p.account_id = a.account_id
                WHERE a.account_id = @account_id";

            var parameters = new Dictionary<string, object>
            {
                ["@account_id"] = accountId
            };

            ExecuteReader(query, reader => {
                profile = new ProfileInfoDto
                {
                    AccountId = GetInt32(reader, "account_id"),
                    Email = GetString(reader, "email") ?? string.Empty,
                    FullName = GetString(reader, "full_name") ?? string.Empty,
                    DateOfBirth = GetDateTime(reader, "date_of_birth"),
                    PhoneNumber = GetString(reader, "phone_number") ?? string.Empty,
                    PassportNumber = GetString(reader, "passport_number") ?? string.Empty,
                    Nationality = GetString(reader, "nationality") ?? string.Empty
                };
            }, parameters);

            return profile;
        }

        /// <summary>
        /// Lưu / cập nhật hồ sơ:
        /// - Update email trong bảng accounts
        /// - Insert hoặc Update bản ghi trong Passenger_Profiles
        /// </summary>
        public void Save(ProfileInfoDto profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            // 1. Update email trong accounts
            string updateAccountSql = $@"
                UPDATE {ACCOUNT_TABLE}
                SET email = @Email
                WHERE account_id = @AccountId";

            var accountParams = new Dictionary<string, object>
            {
                ["@Email"] = profile.Email,
                ["@AccountId"] = profile.AccountId
            };

            ExecuteNonQuery(updateAccountSql, accountParams);

            // 2. Kiểm tra xem profile đã tồn tại chưa
            string checkSql = $@"
                SELECT COUNT(*)
                FROM {PROFILE_TABLE}
                WHERE account_id = @AccountId";

            var checkParams = new Dictionary<string, object>
            {
                ["@AccountId"] = profile.AccountId
            };

            var result = ExecuteScalar(checkSql, checkParams);
            int count = Convert.ToInt32(result);

            // Params chung cho insert/update
            var profileParams = new Dictionary<string, object>
            {
                ["@AccountId"] = profile.AccountId,
                ["@FullName"] = (object?)profile.FullName ?? DBNull.Value,
                ["@Dob"] = (object?)profile.DateOfBirth ?? DBNull.Value,
                ["@Phone"] = (object?)profile.PhoneNumber ?? DBNull.Value,
                ["@Passport"] = (object?)profile.PassportNumber ?? DBNull.Value,
                ["@Nationality"] = (object?)profile.Nationality ?? DBNull.Value
            };

            if (count == 0)
            {
                // 3a. Chưa có -> INSERT mới
                string insertSql = $@"
                    INSERT INTO {PROFILE_TABLE}
                        (account_id, full_name, date_of_birth, phone_number, passport_number, nationality)
                    VALUES
                        (@AccountId, @FullName, @Dob, @Phone, @Passport, @Nationality);";

                ExecuteNonQuery(insertSql, profileParams);
            }
            else
            {
                // 3b. Đã có -> UPDATE
                string updateProfileSql = $@"
                    UPDATE {PROFILE_TABLE}
                    SET full_name      = @FullName,
                        date_of_birth  = @Dob,
                        phone_number   = @Phone,
                        passport_number= @Passport,
                        nationality    = @Nationality
                    WHERE account_id   = @AccountId;";

                ExecuteNonQuery(updateProfileSql, profileParams);
            }
        }
    }
}