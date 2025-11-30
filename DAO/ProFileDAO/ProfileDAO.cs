using DTO.Profile;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.ProfileDAO
{
    public class ProfileDAO
    {
        public ProfileDTO? GetProfileByAccountId(int accountId)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
            SELECT 
                a.account_id,
                a.email,
                a.password,
                a.created_at,
                p.profile_id,
                p.full_name,
                p.date_of_birth,
                p.phone_number,
                p.passport_number,
                p.nationality
            FROM accounts a
            JOIN passenger_profiles p ON a.account_id = p.account_id
            WHERE a.account_id = @accountId
            LIMIT 1;
        ";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@accountId", accountId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;

                        return new ProfileDTO
                        {
                            AccountId = reader.GetInt32("account_id"),
                            Email = reader["email"]?.ToString(),
                            Password = reader["password"]?.ToString(),
                            CreatedAt = reader["created_at"] is DBNull ? null : reader.GetDateTime("created_at"),

                            ProfileId = reader.GetInt32("profile_id"),
                            FullName = reader["full_name"]?.ToString(),
                            DateOfBirth = reader["date_of_birth"] is DBNull ? null : reader.GetDateTime("date_of_birth"),
                            PhoneNumber = reader["phone_number"]?.ToString(),
                            PassportNumber = reader["passport_number"]?.ToString(),
                            Nationality = reader["nationality"]?.ToString()
                        };
                    }
                }
            }
        }

    }

}
