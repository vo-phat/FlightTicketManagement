using System;
using System.Collections.Generic;
using MySqlConnector;
using DTO.Payment;
using DAO.Database;

namespace DAO.Payment
{
    public class PaymentDAO
    {
        #region Lấy danh sách tất cả thanh toán
        public List<PaymentDTO> GetAllPayments()
        {
            List<PaymentDTO> payments = new List<PaymentDTO>();
            string query = "SELECT payment_id, booking_id, amount, payment_method, payment_date, status FROM payments";

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
                            var payment = new PaymentDTO(
                                reader.GetInt32("payment_id"),
                                reader.GetInt32("booking_id"),
                                reader.GetDecimal("amount"),
                                reader.GetString("payment_method"),
                                reader.GetDateTime("payment_date"),
                                reader.GetString("status")
                            );
                            payments.Add(payment);
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

        #region 🔹 Lấy danh sách thanh toán của các booking đang Pending
        public List<PaymentDTO> GetPaymentsOfPendingBookings()
        {
            List<PaymentDTO> payments = new List<PaymentDTO>();
            string query = @"
                SELECT p.payment_id, p.booking_id, p.amount, p.payment_method, p.payment_date, p.status
                FROM payments p
                INNER JOIN bookings b ON p.booking_id = b.booking_id
                WHERE b.status = 'Pending';";

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
                            var payment = new PaymentDTO(
                                reader.GetInt32("payment_id"),
                                reader.GetInt32("booking_id"),
                                reader.GetDecimal("amount"),
                                reader.GetString("payment_method"),
                                reader.GetDateTime("payment_date"),
                                reader.GetString("status")
                            );
                            payments.Add(payment);
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

        #region Thêm thanh toán mới
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

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm thanh toán: " + ex.Message, ex);
            }
        }
        #endregion

        #region Cập nhật thông tin thanh toán
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

                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật thanh toán: " + ex.Message, ex);
            }
        }
        #endregion

        #region Xóa thanh toán theo ID
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
                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa thanh toán: " + ex.Message, ex);
            }
        }
        #endregion

        #region Tìm kiếm thanh toán
        public List<PaymentDTO> SearchPayments(string keyword)
        {
            List<PaymentDTO> results = new List<PaymentDTO>();
            string query = @"SELECT payment_id, booking_id, amount, payment_method, payment_date, status
                             FROM payments
                             WHERE payment_id LIKE @kw
                                OR booking_id LIKE @kw
                                OR amount LIKE @kw
                                OR payment_method LIKE @kw
                                OR status LIKE @kw";
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
                                var payment = new PaymentDTO(
                                    reader.GetInt32("payment_id"),
                                    reader.GetInt32("booking_id"),
                                    reader.GetDecimal("amount"),
                                    reader.GetString("payment_method"),
                                    reader.GetDateTime("payment_date"),
                                    reader.GetString("status")
                                );
                                results.Add(payment);
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
    }
}
