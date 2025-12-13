using DAO.Payment;
using DTO.Payment;
using DTO.Ticket;
using MySqlConnector;
using System;
using System.Collections.Generic;
namespace DAO.TicketDAO
{
    public class SaveTicketRequestDAO
    {
        // ---------------------------------------------------
        //  PUBLIC API
        // ---------------------------------------------------

        /// <summary>
        /// Tạo booking một chiều
        /// </summary>
        public int CreateBookingOneWay(List<TicketBookingRequestDTO> passengers, int accountId)
        {
            // ✅ Validation
            ValidatePassengers(passengers);

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) Tính tổng tiền
                        decimal totalAmount = passengers.Sum(p => p.TicketPrice ?? 0);

                        // 2) Tạo booking
                        int bookingId = InsertBooking(conn, tran, accountId, "ONE_WAY", totalAmount);

                        // 3) Xử lý từng hành khách
                        foreach (var dto in passengers)
                        {
                            int profileId = GetOrCreatePassengerProfile(conn, tran, dto, accountId);
                            int bookPassengerId = InsertBookingPassenger(conn, tran, bookingId, profileId);

                            int ticketId = InsertTicket(conn, tran, bookPassengerId, dto, "OUTBOUND");
                            InsertBaggage(conn, tran, ticketId, dto);
                            UpdateSeatStatus(conn, tran, dto.FlightSeatId);
                        }
                        // 4) GHI NHẬN PAYMENT
                        var paymentDto = new PaymentDTO
                        {
                            BookingId = bookingId,
                            Amount = totalAmount,
                            PaymentMethod = "CREDIT_CARD", // hoặc lấy từ UI
                            PaymentDate = DateTime.Now,
                            Status = "SUCCESS"
                        };

                        new PaymentDAO().RecordPayment(paymentDto, tran);
                        tran.Commit();
                        return bookingId;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception($"Lỗi khi tạo booking một chiều: {ex.Message}", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Tạo booking khứ hồi - Tạo 2 vé cho mỗi hành khách
        /// </summary>
        public int CreateBookingRoundTrip(
            List<TicketBookingRequestDTO> outbound,
            List<TicketBookingRequestDTO> inbound,
            int accountId)
        {
            // ✅ Validation
            if (outbound.Count != inbound.Count)
                throw new Exception("Số lượng hành khách chiều đi và về phải bằng nhau.");

            ValidatePassengers(outbound);
            ValidatePassengers(inbound);

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) Tính tổng tiền (cả 2 chiều)
                        decimal totalAmount = outbound.Sum(p => p.TicketPrice ?? 0)
                                            + inbound.Sum(p => p.TicketPrice ?? 0);

                        // 2) Tạo booking
                        int bookingId = InsertBooking(conn, tran, accountId, "ROUND_TRIP", totalAmount);

                        // 3) Xử lý từng cặp hành khách
                        for (int i = 0; i < outbound.Count; i++)
                        {
                            // Chỉ tạo 1 passenger_profile cho mỗi người
                            int profileId = GetOrCreatePassengerProfile(conn, tran, outbound[i], accountId);
                            int bookPassengerId = InsertBookingPassenger(conn, tran, bookingId, profileId);

                            // TICKET CHIỀU ĐI
                            int ticketIdOut = InsertTicket(conn, tran, bookPassengerId, outbound[i], "OUTBOUND");
                            InsertBaggage(conn, tran, ticketIdOut, outbound[i]);
                            UpdateSeatStatus(conn, tran, outbound[i].FlightSeatId);

                            // TICKET CHIỀU VỀ
                            int ticketIdIn = InsertTicket(conn, tran, bookPassengerId, inbound[i], "INBOUND");
                            InsertBaggage(conn, tran, ticketIdIn, inbound[i]);
                            UpdateSeatStatus(conn, tran, inbound[i].FlightSeatId);
                        }
                        // 4) GHI NHẬN PAYMENT
                        var paymentDto = new PaymentDTO
                        {
                            BookingId = bookingId,
                            Amount = totalAmount,
                            PaymentMethod = "CREDIT_CARD",
                            PaymentDate = DateTime.Now,
                            Status = "SUCCESS"
                        };

                        new PaymentDAO().RecordPayment(paymentDto, tran);

                        tran.Commit();
                        return bookingId;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception($"Lỗi khi tạo booking khứ hồi: {ex.Message}", ex);
                    }
                }
            }
        }

        // ---------------------------------------------------
        //  HELPER FUNCTIONS
        // ---------------------------------------------------

        /// <summary>
        /// Validation dữ liệu hành khách
        /// </summary>
        private void ValidatePassengers(List<TicketBookingRequestDTO> passengers)
        {
            if (passengers == null || passengers.Count == 0)
                throw new Exception("Danh sách hành khách rỗng.");

            foreach (var p in passengers)
            {
                if (string.IsNullOrWhiteSpace(p.FullName))
                    throw new Exception("Tên hành khách không được để trống.");

                if (string.IsNullOrWhiteSpace(p.PassportNumber))
                    throw new Exception("Số hộ chiếu không được để trống.");

                if (p.FlightSeatId == 0)
                    throw new Exception($"Hành khách {p.FullName} chưa chọn ghế.");

                if (!p.TicketPrice.HasValue || p.TicketPrice <= 0)
                    throw new Exception($"Giá vé của {p.FullName} không hợp lệ.");
            }
        }

        /// <summary>
        /// Tạo booking record
        /// </summary>
        private int InsertBooking(MySqlConnection conn, MySqlTransaction tran,
                                  int accountId, string type, decimal totalAmount)
        {
            string sql = @"
                INSERT INTO bookings (account_id, booking_date, trip_type, status, total_amount)
                VALUES (@acc, NOW(), @type, 'CONFIRMED', @total);
                SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@acc", accountId);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@total", totalAmount);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Tìm hoặc tạo passenger_profile (bao gồm cả email)
        /// </summary>
        private int GetOrCreatePassengerProfile(MySqlConnection conn, MySqlTransaction tran,
                                        TicketBookingRequestDTO dto, int accountId)
        {
            // 1) Tìm profile đã tồn tại theo tên + hộ chiếu
            string findSql = @"
        SELECT profile_id 
        FROM passenger_profiles
        WHERE full_name = @name 
          AND passport_number = @passport
        LIMIT 1;";

            using (var cmd = new MySqlCommand(findSql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@name", dto.FullName);
                cmd.Parameters.AddWithValue("@passport", dto.PassportNumber);

                var exist = cmd.ExecuteScalar();
                if (exist != null)
                    return Convert.ToInt32(exist);
            }

            // 2) Tạo mới profile (không có email)
            string insertSql = @"
        INSERT INTO passenger_profiles
            (account_id, full_name, date_of_birth, phone_number,
             passport_number, nationality)
        VALUES
            (@acc, @name, @dob, @phone, @pass, @nation);
        SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(insertSql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@acc", accountId);
                cmd.Parameters.AddWithValue("@name", dto.FullName);
                cmd.Parameters.AddWithValue("@dob", dto.DateOfBirth);
                cmd.Parameters.AddWithValue("@phone", dto.PhoneNumber ?? "");
                cmd.Parameters.AddWithValue("@pass", dto.PassportNumber);
                cmd.Parameters.AddWithValue("@nation", dto.Nationality ?? "VN");

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        /// <summary>
        /// Liên kết booking với passenger
        /// </summary>
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

        /// <summary>
        /// Tạo ticket (bao gồm giá vé)
        /// </summary>
        private int InsertTicket(MySqlConnection conn, MySqlTransaction tran,
                         int bookingPassengerId,
                         TicketBookingRequestDTO dto,
                         string segment)
        {
            string sql = @"
        INSERT INTO tickets
            (ticket_passenger_id, flight_seat_id, ticket_number, issue_date,
             segment_no, segment_type, status, total_price)
        VALUES
            (@tp, @fs, @num, NOW(), 1, @seg, 'BOOKED', @total);
        SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@tp", bookingPassengerId);
                cmd.Parameters.AddWithValue("@fs", dto.FlightSeatId);
                cmd.Parameters.AddWithValue("@num", GenerateTicketNumber());
                cmd.Parameters.AddWithValue("@seg", segment);
                cmd.Parameters.AddWithValue("@total", dto.TicketPrice ?? 0);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        /// <summary>
        /// Lưu thông tin hành lý
        /// </summary>
        private void InsertBaggage(MySqlConnection conn, MySqlTransaction tran,
                                   int ticketId, TicketBookingRequestDTO dto)
        {
            // ✅ Lưu carry-on (luôn có)
            if (dto.CarryOnId.HasValue && dto.CarryOnId > 0)
            {
                InsertBaggageRecord(conn, tran, ticketId, "carry_on", dto.CarryOnId.Value, null, 1, "");
            }

            // ✅ Lưu checked (nếu có chọn)
            if (dto.CheckedId.HasValue && dto.CheckedId > 0)
            {
                InsertBaggageRecord(conn, tran, ticketId, "checked", null, dto.CheckedId.Value,
                                   dto.Quantity ?? 1, dto.BaggageNote ?? "");
            }
        }

        /// <summary>
        /// Insert 1 record vào ticket_baggage
        /// </summary>
        private void InsertBaggageRecord(MySqlConnection conn, MySqlTransaction tran,
                                         int ticketId, string type,
                                         int? carryOnId, int? checkedId,
                                         int quantity, string note)
        {
            string sql = @"
                INSERT INTO ticket_baggage
                    (ticket_id, baggage_type, carryon_id, checked_id, quantity, note)
                VALUES
                    (@tid, @type, @carry, @chk, @qty, @note);";

            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@tid", ticketId);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@carry", carryOnId.HasValue ? (object)carryOnId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@chk", checkedId.HasValue ? (object)checkedId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@qty", quantity);
                cmd.Parameters.AddWithValue("@note", note);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Cập nhật trạng thái ghế thành BOOKED
        /// </summary>
        private void UpdateSeatStatus(MySqlConnection conn, MySqlTransaction tran, int flightSeatId)
        {
            string sql = "UPDATE flight_seats SET seat_status='BOOKED' WHERE flight_seat_id=@id";

            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@id", flightSeatId);
                int affected = cmd.ExecuteNonQuery();

                if (affected == 0)
                    throw new Exception($"Không tìm thấy ghế với FlightSeatId={flightSeatId}");
            }
        }

        /// <summary>
        /// Tạo mã vé duy nhất
        /// </summary>
        private string GenerateTicketNumber()
        {
            return "TK" + DateTime.Now.Ticks.ToString().Substring(8);
        }
    }
}