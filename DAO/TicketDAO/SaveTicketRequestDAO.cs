using Microsoft.Data.SqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.TicketDAO
{
       public class SaveTicketRequestDAO
        {
        public int CreateBooking(List<DTO.Ticket.TicketBookingRequestDTO> dtos, int? accountId, string tripType = "ONE_WAY")
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        int bookingId = 0;
                        decimal totalAmount = 0;

                        // ======================================
                        // 1. INSERT BOOKING
                        // ======================================
                        string insertBookingSql = @"
                            INSERT INTO bookings (account_id, booking_date, trip_type, status, total_amount)
                            VALUES (@accId, NOW(), @tripType, 'PENDING', 0);
                            SELECT LAST_INSERT_ID();";

                        using (var cmd = new MySqlCommand(insertBookingSql, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@accId", accountId);
                            cmd.Parameters.AddWithValue("@tripType", tripType);

                            bookingId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // ======================================
                        // 2. LOOP QUA TỪNG VÉ
                        // ======================================
                        foreach (var dto in dtos)
                        {
                            // 2.1 INSERT passenger_profile
                            int profileId = 0;

                            // ===========================================
                            // 1) LUÔN TÌM PROFILE CŨ THEO FULLNAME + DOB + PASSPORT
                            // ===========================================
                            string findProfileSql = @"
                                        SELECT profile_id
                                        FROM passenger_profiles
                                        WHERE full_name = @name
                                          AND date_of_birth = @dob
                                          AND passport_number = @passport
                                        LIMIT 1;";

                            using (var cmd = new MySqlCommand(findProfileSql, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@name", dto.FullName ?? "");
                                cmd.Parameters.AddWithValue("@dob", dto.DateOfBirth ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@passport", dto.PassportNumber ?? "");

                                var existing = cmd.ExecuteScalar();
                                if (existing != null)
                                {
                                    profileId = Convert.ToInt32(existing);
                                }
                            }

                            // ===========================================
                            // 2) Nếu không tìm thấy → tạo mới
                            // ===========================================
                            if (profileId == 0)
                            {
                                string insertProfileSql = @"
                                        INSERT INTO passenger_profiles
                                            (account_id, full_name, date_of_birth, phone_number, passport_number, nationality)
                                        VALUES
                                            (@accId, @name, @dob, @phone, @passport, @nation);
                                        SELECT LAST_INSERT_ID();";

                                using (var cmd = new MySqlCommand(insertProfileSql, conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@accId", dto.AccountId);
                                    cmd.Parameters.AddWithValue("@name", dto.FullName ?? "");
                                    cmd.Parameters.AddWithValue("@dob", dto.DateOfBirth ?? (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@phone", dto.PhoneNumber ?? "");
                                    cmd.Parameters.AddWithValue("@passport", dto.PassportNumber ?? "");
                                    cmd.Parameters.AddWithValue("@nation", dto.Nationality ?? "");

                                    profileId = Convert.ToInt32(cmd.ExecuteScalar());
                                }

                                // Nếu user có account_id → tạo liên kết vào profile_passenger
                                if (dto.AccountId.HasValue)
                                {
                                    string insertRelationSql = @"
                                        INSERT INTO profile_passenger (account_id, profile_id)
                                        VALUES (@accId, @pId);";

                                    using (var cmd = new MySqlCommand(insertRelationSql, conn, tran))
                                    {
                                        cmd.Parameters.AddWithValue("@accId", dto.AccountId.Value);
                                        cmd.Parameters.AddWithValue("@pId", profileId);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }



                            // 2.2 INSERT booking_passengers
                            string insertBookPassSql = @"
                                INSERT INTO booking_passengers (booking_id, profile_id)
                                VALUES (@bid, @pid);
                                SELECT LAST_INSERT_ID();";

                            int bookingPassengerId;
                            using (var cmd = new MySqlCommand(insertBookPassSql, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@bid", bookingId);
                                cmd.Parameters.AddWithValue("@pid", profileId);

                                bookingPassengerId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // 2.3 UPDATE seat booked
                            using (var cmd = new MySqlCommand("UPDATE flight_seats SET seat_status = 'BOOKED' WHERE flight_seat_id = @fsid", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@fsid", dto.FlightSeatId);
                                cmd.ExecuteNonQuery();
                            }

                            // 2.4 INSERT ticket (CHUẨN MYSQL)
                            string ticketNumber = dto.TicketNumber ?? GenerateTicketNumber();

                            string insertTicketSql = @"
                                INSERT INTO tickets (
                                    ticket_passenger_id, flight_seat_id, ticket_number,
                                    issue_date, segment_no, segment_type, status
                                )
                                VALUES (
                                    @tpId, @fsid, @tNum,
                                    NOW(), 1, 'OUTBOUND', 'BOOKED'
                                );
                                SELECT LAST_INSERT_ID();";

                            int ticketId;
                            using (var cmd = new MySqlCommand(insertTicketSql, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@tpId", bookingPassengerId);
                                cmd.Parameters.AddWithValue("@fsid", dto.FlightSeatId);
                                cmd.Parameters.AddWithValue("@tNum", ticketNumber);

                                ticketId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // 2.5 INSERT BAGGAGE (HỢP VỚI CHECK CONSTRAINT)
                            if (dto.CarryOnId.HasValue)
                            {
                                using (var cmd = new MySqlCommand(@"
                                    INSERT INTO ticket_baggage
                                        (ticket_id, baggage_type, carryon_id, checked_id, quantity, note)
                                    VALUES
                                        (@tid, 'carry_on', @cid, NULL, @qty, @note)", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@tid", ticketId);
                                    cmd.Parameters.AddWithValue("@cid", dto.CarryOnId.Value);
                                    cmd.Parameters.AddWithValue("@qty", dto.Quantity ?? 1);
                                    cmd.Parameters.AddWithValue("@note", dto.BaggageNote ?? "");
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else if (dto.CheckedId.HasValue)
                            {
                                using (var cmd = new MySqlCommand(@"
                                    INSERT INTO ticket_baggage
                                        (ticket_id, baggage_type, carryon_id, checked_id, quantity, note)
                                    VALUES
                                        (@tid, 'checked', NULL, @chkId, @qty, @note)", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@tid", ticketId);
                                    cmd.Parameters.AddWithValue("@chkId", dto.CheckedId.Value);
                                    cmd.Parameters.AddWithValue("@qty", dto.Quantity ?? 1);
                                    cmd.Parameters.AddWithValue("@note", dto.BaggageNote ?? "");
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // 2.6 CỘNG TIỀN VÉ
                            totalAmount += dto.TicketPrice ?? 0;
                        }

                        // ======================================
                        // 3. UPDATE TOTAL BOOKING
                        // ======================================
                        using (var cmd = new MySqlCommand(@"
                            UPDATE bookings
                            SET total_amount = @total, status = 'CONFIRMED'
                            WHERE booking_id = @bid", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@total", totalAmount);
                            cmd.Parameters.AddWithValue("@bid", bookingId);
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return bookingId;
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        private string GenerateTicketNumber()
        {
            return "TK" + DateTime.Now.Ticks;
        }


    }
}
