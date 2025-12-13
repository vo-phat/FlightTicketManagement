using System;
using System.Collections.Generic;
using MySqlConnector;
using DTO.Payment;
using DAO.Database;

namespace DAO.Payment
{
    public class PaymentDAO
    {
        #region 🔹 Lấy danh sách tất cả thanh toán (PaymentDTO thuần)
        public List<PaymentDTO> GetAllPayments()
        {
            List<PaymentDTO> payments = new();
            string query = "SELECT payment_id, booking_id, amount, payment_method, payment_date, status FROM payments ORDER BY payment_date DESC";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new PaymentDTO(
                                reader.GetInt32("payment_id"),
                                reader.GetInt32("booking_id"),
                                reader.GetDecimal("amount"),
                                reader.GetString("payment_method"),
                                reader.GetDateTime("payment_date"),
                                reader.GetString("status")
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách thanh toán: " + ex.Message, ex);
            }

            return payments;
        }
        #endregion

        #region 🔹 Lấy danh sách thanh toán với thông tin đầy đủ (PaymentDetailDTO)
        public List<PaymentDetailDTO> GetAllPaymentsWithDetails()
        {
            List<PaymentDetailDTO> payments = new();
            string query = @"
                SELECT 
                    p.payment_id,
                    p.booking_id,
                    p.amount,
                    p.payment_method,
                    p.payment_date,
                    p.status,
                    b.account_id,
                    b.booking_date,
                    b.status AS booking_status,
                    b.total_amount AS booking_total_amount,
                    a.email AS account_email
                FROM payments p
                INNER JOIN bookings b ON p.booking_id = b.booking_id
                LEFT JOIN accounts a ON b.account_id = a.account_id
                ORDER BY p.payment_date DESC";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new PaymentDetailDTO(
                                reader.GetInt32("payment_id"),
                                reader.GetInt32("booking_id"),
                                reader.GetDecimal("amount"),
                                reader.GetString("payment_method"),
                                reader.GetDateTime("payment_date"),
                                reader.GetString("status"),
                                reader.GetInt32("account_id"),
                                reader.GetDateTime("booking_date"),
                                reader.GetString("booking_status"),
                                reader.GetDecimal("booking_total_amount"),
                                reader.IsDBNull(reader.GetOrdinal("account_email")) ? "" : reader.GetString("account_email")
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách thanh toán với thông tin chi tiết: " + ex.Message, ex);
            }

            return payments;
        }
        #endregion

        #region 🔹 Lấy danh sách bookings có trạng thái PENDING (với thông tin đầy đủ)
        public List<PaymentDetailDTO> GetPendingBookingsPayments()
        {
            List<PaymentDetailDTO> payments = new();
            string query = @"
                SELECT 
                    p.payment_id,
                    p.booking_id,
                    p.amount,
                    p.payment_method,
                    p.payment_date,
                    p.status,
                    b.account_id,
                    b.booking_date,
                    b.status AS booking_status,
                    b.total_amount AS booking_total_amount,
                    a.email AS account_email
                FROM payments p
                INNER JOIN bookings b ON p.booking_id = b.booking_id
                LEFT JOIN accounts a ON b.account_id = a.account_id
                WHERE UPPER(b.status) = 'PENDING'
                ORDER BY p.payment_date DESC";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new PaymentDetailDTO(
                                reader.GetInt32("payment_id"),
                                reader.GetInt32("booking_id"),
                                reader.GetDecimal("amount"),
                                reader.GetString("payment_method"),
                                reader.GetDateTime("payment_date"),
                                reader.GetString("status"),
                                reader.GetInt32("account_id"),
                                reader.GetDateTime("booking_date"),
                                reader.GetString("booking_status"),
                                reader.GetDecimal("booking_total_amount"),
                                reader.IsDBNull(reader.GetOrdinal("account_email")) ? "" : reader.GetString("account_email")
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách thanh toán của các booking Pending: " + ex.Message, ex);
            }

            return payments;
        }
        #endregion

        #region 🔹 Lấy chi tiết payment theo ID
        public PaymentDetailDTO GetPaymentById(int paymentId)
        {
            string query = @"
                SELECT 
                    p.payment_id,
                    p.booking_id,
                    p.amount,
                    p.payment_method,
                    p.payment_date,
                    p.status,
                    b.account_id,
                    b.booking_date,
                    b.status AS booking_status,
                    b.total_amount AS booking_total_amount,
                    a.email AS account_email
                FROM payments p
                INNER JOIN bookings b ON p.booking_id = b.booking_id
                LEFT JOIN accounts a ON b.account_id = a.account_id
                WHERE p.payment_id = @id";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", paymentId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PaymentDetailDTO(
                                    reader.GetInt32("payment_id"),
                                    reader.GetInt32("booking_id"),
                                    reader.GetDecimal("amount"),
                                    reader.GetString("payment_method"),
                                    reader.GetDateTime("payment_date"),
                                    reader.GetString("status"),
                                    reader.GetInt32("account_id"),
                                    reader.GetDateTime("booking_date"),
                                    reader.GetString("booking_status"),
                                    reader.GetDecimal("booking_total_amount"),
                                    reader.IsDBNull(reader.GetOrdinal("account_email")) ? "" : reader.GetString("account_email")
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin payment ID {paymentId}: " + ex.Message, ex);
            }

            return null;
        }
        #endregion

        #region 🔹 Thêm thanh toán mới
        public bool InsertPayment(PaymentDTO payment)
        {
            string query = @"INSERT INTO payments (booking_id, amount, payment_method, payment_date, status)
                             VALUES (@booking_id, @amount, @method, @date, @status)";
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@booking_id", payment.BookingId);
                        command.Parameters.AddWithValue("@amount", payment.Amount);
                        command.Parameters.AddWithValue("@method", payment.PaymentMethod);
                        command.Parameters.AddWithValue("@date", payment.PaymentDate);
                        command.Parameters.AddWithValue("@status", payment.Status);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm thanh toán: " + ex.Message, ex);
            }
        }
        #endregion

        #region 🔹 Cập nhật thông tin thanh toán
        public bool UpdatePayment(PaymentDTO payment)
        {
            string query = @"UPDATE payments
                             SET booking_id = @booking_id,
                                 amount = @amount,
                                 payment_method = @method,
                                 payment_date = @date,
                                 status = @status
                             WHERE payment_id = @id";
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@booking_id", payment.BookingId);
                        command.Parameters.AddWithValue("@amount", payment.Amount);
                        command.Parameters.AddWithValue("@method", payment.PaymentMethod);
                        command.Parameters.AddWithValue("@date", payment.PaymentDate);
                        command.Parameters.AddWithValue("@status", payment.Status);
                        command.Parameters.AddWithValue("@id", payment.PaymentId);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật thanh toán: " + ex.Message, ex);
            }
        }
        #endregion

        #region 🔹 Cập nhật trạng thái Payment
        public bool UpdatePaymentStatus(int paymentId, string newStatus)
        {
            string query = "UPDATE payments SET status = @status WHERE payment_id = @id";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@status", newStatus.Trim().ToUpper());
                        command.Parameters.AddWithValue("@id", paymentId);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật trạng thái payment ID {paymentId}: " + ex.Message, ex);
            }
        }
        #endregion

        #region 🔹 Cập nhật trạng thái Booking
        public bool UpdateBookingStatus(int bookingId, string newStatus)
        {
            var allowedStatuses = new HashSet<string> { "PENDING", "CONFIRMED", "CANCELLED", "REFUNDED" };
            string normalizedStatus = newStatus.Trim().ToUpper();

            if (!allowedStatuses.Contains(normalizedStatus))
                throw new ArgumentException($"Trạng thái không hợp lệ: {newStatus}");

            string query = "UPDATE bookings SET status = @newStatus WHERE booking_id = @id";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@newStatus", normalizedStatus);
                        command.Parameters.AddWithValue("@id", bookingId);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật trạng thái Booking ID {bookingId}: {ex.Message}", ex);
            }
        }
        #endregion

        #region 🔹 Xóa thanh toán theo ID
        public bool DeletePayment(int paymentId)
        {
            string query = "DELETE FROM payments WHERE payment_id = @id";
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", paymentId);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa thanh toán: " + ex.Message, ex);
            }
        }
        #endregion

        #region 🔹 Tìm kiếm thanh toán với thông tin đầy đủ
        public List<PaymentDetailDTO> SearchPayments(string keyword)
        {
            List<PaymentDetailDTO> results = new();
            string query = @"
                SELECT 
                    p.payment_id,
                    p.booking_id,
                    p.amount,
                    p.payment_method,
                    p.payment_date,
                    p.status,
                    b.account_id,
                    b.booking_date,
                    b.status AS booking_status,
                    b.total_amount AS booking_total_amount,
                    a.email AS account_email
                FROM payments p
                INNER JOIN bookings b ON p.booking_id = b.booking_id
                LEFT JOIN accounts a ON b.account_id = a.account_id
                WHERE CAST(p.payment_id AS CHAR) LIKE @kw
                    OR CAST(p.booking_id AS CHAR) LIKE @kw
                    OR CAST(p.amount AS CHAR) LIKE @kw
                    OR p.payment_method LIKE @kw
                    OR p.status LIKE @kw
                    OR b.status LIKE @kw
                    OR a.email LIKE @kw
                ORDER BY p.payment_date DESC";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new PaymentDetailDTO(
                                    reader.GetInt32("payment_id"),
                                    reader.GetInt32("booking_id"),
                                    reader.GetDecimal("amount"),
                                    reader.GetString("payment_method"),
                                    reader.GetDateTime("payment_date"),
                                    reader.GetString("status"),
                                    reader.GetInt32("account_id"),
                                    reader.GetDateTime("booking_date"),
                                    reader.GetString("booking_status"),
                                    reader.GetDecimal("booking_total_amount"),
                                    reader.IsDBNull(reader.GetOrdinal("account_email")) ? "" : reader.GetString("account_email")
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm thanh toán: " + ex.Message, ex);
            }

            return results;
        }
        #endregion

        #region 🔹 Xử lý thanh toán (Cập nhật logic mới)
        /// <summary>
        /// Xử lý thanh toán: Chỉ cho phép khi Payment PENDING và Booking CONFIRMED.
        /// Chuyển trạng thái Payment -> SUCCESS.
        /// </summary>
        public bool ProcessPayment(int paymentId)
        {
            // Sử dụng UPDATE với JOIN để kiểm tra chéo điều kiện của cả bảng payments và bookings
            // Điều kiện: payment.status = PENDING VÀ booking.status = CONFIRMED
            string query = @"UPDATE payments p
                             INNER JOIN bookings b ON p.booking_id = b.booking_id
                             SET p.status = 'SUCCESS'
                             WHERE p.payment_id = @paymentId
                               AND p.status = 'PENDING'
                               AND b.status = 'CONFIRMED'";

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@paymentId", paymentId);

                        // ExecuteNonQuery trả về số dòng bị ảnh hưởng
                        // > 0 nghĩa là tìm thấy bản ghi thỏa mãn điều kiện và đã update thành công
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xử lý thanh toán (ID: {paymentId}): " + ex.Message, ex);
            }
        }
        public bool RecordPayment(PaymentDTO payment, MySqlTransaction tran)
        {
            string query = @"
        INSERT INTO payments (booking_id, amount, payment_method, payment_date, status)
        VALUES (@booking_id, @amount, @method, @date, @status)";

            using (var command = new MySqlCommand(query, tran.Connection, tran))
            {
                command.Parameters.AddWithValue("@booking_id", payment.BookingId);
                command.Parameters.AddWithValue("@amount", payment.Amount);
                command.Parameters.AddWithValue("@method", payment.PaymentMethod);
                command.Parameters.AddWithValue("@date", payment.PaymentDate);
                command.Parameters.AddWithValue("@status", payment.Status);

                return command.ExecuteNonQuery() > 0;
            }
        }
        #endregion
    }
}