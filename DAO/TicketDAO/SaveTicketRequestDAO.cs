using DTO.Ticket;
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
        // ---------------------------------------------------
        //  PUBLIC API
        // ---------------------------------------------------

        // ✔ One-Way Booking
        public int CreateBookingOneWay(List<TicketBookingRequestDTO> passengers, int accountId)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        int bookingId = InsertBooking(conn, tran, accountId, "ONE_WAY");

                        foreach (var dto in passengers)
                        {
                            int profileId = GetOrCreatePassengerProfile(conn, tran, dto);
                            int bookPassengerId = InsertBookingPassenger(conn, tran, bookingId, profileId);

                            int ticketId = InsertTicket(conn, tran, bookPassengerId, dto, "OUTBOUND");
                            InsertBaggage(conn, tran, ticketId, dto);

                            UpdateSeatStatus(conn, tran, dto.FlightSeatId);
                        }

                        tran.Commit();
                        return bookingId;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }


        // ✔ Round Trip Booking — Tạo 2 vé / người
        public int CreateBookingRoundTrip(
            List<TicketBookingRequestDTO> outbound,
            List<TicketBookingRequestDTO> inbound,
            int accountId)
        {
            if (outbound.Count != inbound.Count)
                throw new Exception("Outbound và inbound phải có cùng số lượng hành khách.");

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        int bookingId = InsertBooking(conn, tran, accountId, "ROUND_TRIP");

                        for (int i = 0; i < outbound.Count; i++)
                        {
                            // 1) Chỉ tạo 1 passenger_profile cho mỗi hành khách
                            int profileId = GetOrCreatePassengerProfile(conn, tran, outbound[i]);
                            int bookPassengerId = InsertBookingPassenger(conn, tran, bookingId, profileId);

                            // 2) TICKET OUTBOUND
                            int ticketIdOut = InsertTicket(conn, tran, bookPassengerId, outbound[i], "OUTBOUND");
                            InsertBaggage(conn, tran, ticketIdOut, outbound[i]);
                            UpdateSeatStatus(conn, tran, outbound[i].FlightSeatId);

                            // 3) TICKET INBOUND
                            int ticketIdIn = InsertTicket(conn, tran, bookPassengerId, inbound[i], "INBOUND");
                            InsertBaggage(conn, tran, ticketIdIn, inbound[i]);
                            UpdateSeatStatus(conn, tran, inbound[i].FlightSeatId);
                        }

                        tran.Commit();
                        return bookingId;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }


        // ---------------------------------------------------
        //  HELPER FUNCTIONS (TÁCH GỌN + DÙNG CHUNG)
        // ---------------------------------------------------

        private int InsertBooking(MySqlConnection conn, MySqlTransaction tran,
                                  int accountId, string type)
        {
            string sql = @"
                INSERT INTO bookings (account_id, booking_date, trip_type, status, total_amount)
                VALUES (@acc, NOW(), @type, 'CONFIRMED', 0);
                SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@acc", accountId);
                cmd.Parameters.AddWithValue("@type", type);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        // ✔ Tìm hoặc tạo passenger_profile
        private int GetOrCreatePassengerProfile(MySqlConnection conn, MySqlTransaction tran,
                                                TicketBookingRequestDTO dto)
        {
            string findSql = @"
                SELECT profile_id FROM passenger_profiles
                WHERE full_name=@name AND passport_number=@passport LIMIT 1;";

            using (var cmd = new MySqlCommand(findSql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@name", dto.FullName);
                cmd.Parameters.AddWithValue("@passport", dto.PassportNumber);
                var exist = cmd.ExecuteScalar();

                if (exist != null)
                    return Convert.ToInt32(exist);
            }

            string insertSql = @"
                INSERT INTO passenger_profiles
                    (account_id, full_name, date_of_birth, phone_number, passport_number, nationality)
                VALUES
                    (@acc, @name, @dob, @phone, @pass, @nation);
                SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(insertSql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@acc", dto.AccountId);
                cmd.Parameters.AddWithValue("@name", dto.FullName);
                cmd.Parameters.AddWithValue("@dob", dto.DateOfBirth);
                cmd.Parameters.AddWithValue("@phone", dto.PhoneNumber);
                cmd.Parameters.AddWithValue("@pass", dto.PassportNumber);
                cmd.Parameters.AddWithValue("@nation", dto.Nationality);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        private int InsertBookingPassenger(MySqlConnection conn, MySqlTransaction tran,
                                           int bookingId, int profileId)
        {
            string sql = @"
                INSERT INTO booking_passengers (booking_id, profile_id)
                VALUES (@bid, @pid);
                SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@bid", bookingId);
                cmd.Parameters.AddWithValue("@pid", profileId);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        private int InsertTicket(MySqlConnection conn, MySqlTransaction tran,
                                 int bookingPassengerId,
                                 TicketBookingRequestDTO dto,
                                 string segment)
        {
            string sql = @"
                INSERT INTO tickets
                    (ticket_passenger_id, flight_seat_id, ticket_number, issue_date,
                     segment_no, segment_type, status)
                VALUES
                    (@tp, @fs, @num, NOW(), 1, @seg, 'BOOKED');
                SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@tp", bookingPassengerId);
                cmd.Parameters.AddWithValue("@fs", dto.FlightSeatId);
                cmd.Parameters.AddWithValue("@num", GenerateTicketNumber());
                cmd.Parameters.AddWithValue("@seg", segment);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        private void InsertBaggage(MySqlConnection conn, MySqlTransaction tran,
                                   int ticketId, TicketBookingRequestDTO dto)
        {
            if (dto.CheckedId == null && dto.CarryOnId == null)
                return;

            string sql = @"
                INSERT INTO ticket_baggage
                    (ticket_id, baggage_type, carryon_id, checked_id, quantity, note)
                VALUES
                    (@tid, @type, @carry, @chk, @qty, @note);";

            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                string type = dto.CheckedId.HasValue ? "checked" : "carry_on";

                cmd.Parameters.AddWithValue("@tid", ticketId);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@carry", dto.CarryOnId);
                cmd.Parameters.AddWithValue("@chk", dto.CheckedId);
                cmd.Parameters.AddWithValue("@qty", dto.Quantity ?? 1);
                cmd.Parameters.AddWithValue("@note", dto.BaggageNote ?? "");

                cmd.ExecuteNonQuery();
            }
        }


        private void UpdateSeatStatus(MySqlConnection conn, MySqlTransaction tran, int flightSeatId)
        {
            string sql = "UPDATE flight_seats SET seat_status='BOOKED' WHERE flight_seat_id=@id";

            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@id", flightSeatId);
                cmd.ExecuteNonQuery();
            }
        }


        private string GenerateTicketNumber()
        {
            return "TK" + DateTime.Now.Ticks.ToString().Substring(8);
        }
    }
}
