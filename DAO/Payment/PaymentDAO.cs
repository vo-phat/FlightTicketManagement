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

        #region 🔹 Xử lý thanh toán (Transaction - QUAN TRỌNG!)
        /// <summary>
        /// Xử lý thanh toán: Cập nhật payment status -> SUCCESS và booking status -> CONFIRMED
        /// Sử dụng transaction để đảm bảo tính nhất quán dữ liệu
        /// </summary>
        public bool ProcessPayment(int paymentId, int bookingId)
        {
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                connection = DatabaseConnection.GetConnection();
                connection.Open();
                transaction = connection.BeginTransaction();

                // 1. Cập nhật payment status -> SUCCESS
                string updatePaymentQuery = "UPDATE payments SET status = 'SUCCESS' WHERE payment_id = @paymentId AND status = 'PENDING'";
                using (var cmd1 = new MySqlCommand(updatePaymentQuery, connection, transaction))
                {
                    cmd1.Parameters.AddWithValue("@paymentId", paymentId);
                    int rowsAffected1 = cmd1.ExecuteNonQuery();
                    if (rowsAffected1 == 0)
                    {
                        transaction.Rollback();
                        return false; // Payment không tồn tại hoặc không ở trạng thái PENDING
                    }
                }

                // 2. Cập nhật booking status -> CONFIRMED
                string updateBookingQuery = "UPDATE bookings SET status = 'CONFIRMED' WHERE booking_id = @bookingId AND status = 'PENDING'";
                using (var cmd2 = new MySqlCommand(updateBookingQuery, connection, transaction))
                {
                    cmd2.Parameters.AddWithValue("@bookingId", bookingId);
                    int rowsAffected2 = cmd2.ExecuteNonQuery();
                    if (rowsAffected2 == 0)
                    {
                        transaction.Rollback();
                        return false; // Booking không tồn tại hoặc không ở trạng thái PENDING
                    }
                }

                // 3. Commit transaction nếu cả 2 đều thành công
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                transaction?.Rollback();
                throw new Exception($"Lỗi khi xử lý thanh toán (Payment ID: {paymentId}, Booking ID: {bookingId}): " + ex.Message, ex);
            }
            finally
            {
                transaction?.Dispose();
                connection?.Close();
                connection?.Dispose();
            }
        }
        #endregion
    }
}