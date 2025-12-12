using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.TicketDAO
{
    public class RefundDAO
    {
        public bool HasCompletedRefund(int ticketId)
        {
            string sql = @"
            SELECT COUNT(*)
            FROM refunds
            WHERE ticket_id = @ticketId
              AND refund_status = 'COMPLETED';
        ";

            using var conn = DbConnection.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ticketId", ticketId);

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public void InsertRefund(
            int ticketId,
            decimal refundAmount,
            decimal refundFee,
            int adminId,
            MySqlTransaction tran)
        {
            string sql = @"
            INSERT INTO refunds
            (ticket_id, refund_amount, refund_fee, refund_status, processed_by)
            VALUES
            (@ticketId, @amount, @fee, 'COMPLETED', @adminId);
        ";

            using var cmd = new MySqlCommand(sql, tran.Connection, tran);
            cmd.Parameters.AddWithValue("@ticketId", ticketId);
            cmd.Parameters.AddWithValue("@amount", refundAmount);
            cmd.Parameters.AddWithValue("@fee", refundFee);
            cmd.Parameters.AddWithValue("@adminId", adminId);
            cmd.ExecuteNonQuery();
        }
    }

}
