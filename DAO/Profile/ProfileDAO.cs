using DAO.Database;
using DTO.Profile;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace DAO.Profile
{
    public class ProfileDAO : BaseDAO
    {
        public ProfileDTO GetProfileByAccountId(int accountId)
        {
            ProfileDTO dto = null;

            string query = @"
                SELECT 
                    p.profile_id,
                    a.account_id,
                    a.email,
                    p.full_name,
                    p.date_of_birth,
                    p.phone_number,
                    p.passport_number,
                    p.nationality,
                    r.role_name
                FROM accounts a
                LEFT JOIN passenger_profiles p ON a.account_id = p.account_id
                LEFT JOIN user_role ur ON a.account_id = ur.account_id
                LEFT JOIN roles r ON ur.role_id = r.role_id
                WHERE a.account_id = @account_id
                LIMIT 1;
            ";

            var parameters = new Dictionary<string, object>
            {
                { "@account_id", accountId }
            };

            ExecuteReader(query, reader =>
            {
                dto = new ProfileDTO(
                    GetInt32(reader, "profile_id"),
                    GetInt32(reader, "account_id"),
                    GetString(reader, "full_name"),
                    GetDateTime(reader, "date_of_birth"),
                    GetString(reader, "phone_number"),
                    GetString(reader, "passport_number"),
                    GetString(reader, "nationality"),
                    GetString(reader, "email"),
                    GetString(reader, "role_name")
                );
            }, parameters);

            return dto;
        }
        public bool UpdateProfile(ProfileDTO dto)
        {
            string queryAccount = @"
        UPDATE accounts 
        SET email = @Email
        WHERE account_id = @AccountId;";

            string queryProfile = @"
        UPDATE passenger_profiles
        SET full_name = @FullName,
            date_of_birth = @DOB,
            phone_number = @Phone,
            passport_number = @Passport,
            nationality = @Nationality
        WHERE account_id = @AccountId;";

            var parameters = new Dictionary<string, object>
    {
        { "@Email", dto.Email },
        { "@FullName", dto.FullName },
        { "@DOB", dto.DateOfBirth },
        { "@Phone", dto.PhoneNumber },
        { "@Passport", dto.PassportNumber },
        { "@Nationality", dto.Nationality },
        { "@AccountId", dto.AccountId }
    };

            // Gộp 2 query trong 1 transaction
            return ExecuteTransaction(new List<Action<MySqlConnection, MySqlTransaction>>
    {
        (conn, trans) =>
        {
            var cmd1 = new MySqlCommand(queryAccount, conn, trans);
            foreach (var p in parameters) cmd1.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
            cmd1.ExecuteNonQuery();
        },
        (conn, trans) =>
        {
            var cmd2 = new MySqlCommand(queryProfile, conn, trans);
            foreach (var p in parameters) cmd2.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
            cmd2.ExecuteNonQuery();
        }
    });
        }

    }
}
