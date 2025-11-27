using System;
using System.Collections.Generic;
using MySqlConnector;
using DTO.Account;
using DAO.Database;

namespace DAO.Repositories {
    public class AccountDao : BaseDAO {
        private const string TABLE = "accounts";

        public AccountDto GetByEmail(string email) {
            AccountDto account = null;

            string query = $@"
                SELECT account_id, email, password, failed_attempts, is_active, created_at
                FROM {TABLE}
                WHERE email = @email";

            var parameters = new Dictionary<string, object> {
                { "@email", email }
            };

            ExecuteReader(query, reader => {
                account = new AccountDto {
                    AccountId = GetInt32(reader, "account_id"),
                    Email = GetString(reader, "email"),
                    Password = GetString(reader, "password"),
                    FailedAttempts = GetInt32(reader, "failed_attempts"),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                };
            }, parameters);

            return account;
        }

        public bool ExistsByEmail(string email) {
            string query = $@"SELECT COUNT(*) FROM {TABLE} WHERE email = @email";
            var parameters = new Dictionary<string, object> {
                { "@email", email }
            };

            var result = ExecuteScalar(query, parameters);
            int count = Convert.ToInt32(result);
            return count > 0;
        }

        public int Create(AccountDto account) {
            string query = $@"INSERT INTO {TABLE}(email, password, failed_attempts, is_active)
                            VALUES (@email, @password, @failed_attempts, @is_active);";

            var parameters = new Dictionary<string, object> {
                { "@email", account.Email },
                { "@password", account.Password },
                { "@failed_attempts", account.FailedAttempts > 0 ? account.FailedAttempts : AccountDto.DEFAULT_FAILED_ATTEMPTS },
                { "@is_active", account.IsActive }
            };

            return ExecuteInsertAndGetId(query, parameters);
        }


        public void UpdatePassword(int accountId, string hashedPassword) {
            string query = $@"
                UPDATE {TABLE}
                SET password = @password, failed_attempts = 0
                WHERE account_id = @id";

            var parameters = new Dictionary<string, object> {
                { "@password", hashedPassword },
                { "@id", accountId }
            };

            ExecuteNonQuery(query, parameters);
        }

        public void DecreaseFailedAttempts(int accountId) {
            string query = $@"UPDATE {TABLE}
                            SET failed_attempts = CASE
                            WHEN failed_attempts > 0 THEN failed_attempts - 1
                            ELSE 0
                            END
                            WHERE account_id = @id";

            var parameters = new Dictionary<string, object> {
                { "@id", accountId }
            };

            ExecuteNonQuery(query, parameters);
        }

        public void ResetFailedAttemptsToDefault(int accountId) {
            string query = $@"UPDATE {TABLE}
                            SET failed_attempts = @val
                            WHERE account_id = @id";

            var parameters = new Dictionary<string, object> {
                { "@val", AccountDto.DEFAULT_FAILED_ATTEMPTS },
                { "@id", accountId }
            };

            ExecuteNonQuery(query, parameters);
        }

        public void LockAccount(int accountId) {
            string query = $@"UPDATE {TABLE}
                            SET is_active = 0
                            WHERE account_id = @id";

            var parameters = new Dictionary<string, object> {
                { "@id", accountId }
            };

            ExecuteNonQuery(query, parameters);
        }

        public void UnlockAccount(int accountId) {
            string query = @"UPDATE accounts SET is_active = 1 WHERE account_id = @id";

            var parameters = new Dictionary<string, object> {
                { "@id", accountId }
            };

            ExecuteNonQuery(query, parameters);
        }

        public void AssignUserRole(int accountId) {
            const string sql = @"
                INSERT IGNORE INTO account_role (account_id, role_id)
                SELECT @account_id, r.role_id
                FROM roles r
                WHERE r.role_code = 'USER';
            ";

            var parameters = new Dictionary<string, object> {
                ["@account_id"] = accountId
            };

            ExecuteNonQuery(sql, parameters);
        }

        public void CreateEmptyProfile(int accountId) {
            const string sql = @"
                INSERT INTO Passenger_Profiles (account_id)
                VALUES (@account_id)
                ON DUPLICATE KEY UPDATE account_id = account_id;
            ";

            var parameters = new Dictionary<string, object> {
                ["@account_id"] = accountId
            };

            ExecuteNonQuery(sql, parameters);
        }

        public AccountDto GetById(int accountId) {
            AccountDto account = null;

            string query = $@"
                SELECT account_id, email, password, failed_attempts, is_active, created_at
                FROM {TABLE}
                WHERE account_id = @id";

            var parameters = new Dictionary<string, object> {
                { "@id", accountId }
            };

            ExecuteReader(query, reader => {
                account = new AccountDto {
                    AccountId = GetInt32(reader, "account_id"),
                    Email = GetString(reader, "email"),
                    Password = GetString(reader, "password"),
                    FailedAttempts = GetInt32(reader, "failed_attempts"),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                };
            }, parameters);

            return account;
        }
    }
}
