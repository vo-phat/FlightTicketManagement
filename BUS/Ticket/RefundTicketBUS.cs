using DAO;
using DAO.TicketDAO;
using DTO.Ticket;
using System;

namespace BUS.Ticket
{
    public class RefundTicketBUS
    {
        private readonly RefundDAO _refundDao = new();
        private readonly TicketDAO _ticketDao = new();
        private readonly TicketHistoryDAO _historyDao = new();

        public void Refund(RefundTicketDTO dto)
        {
            // 1️⃣ Trạng thái vé
            if (dto.Status != "CANCELLED")
                throw new Exception("Chỉ hoàn vé đã hủy");

            // 2️⃣ Luật hoàn tiền (BOOL – đã sửa)
            if (!dto.IsRefundable)
                throw new Exception("Vé này không hỗ trợ hoàn tiền");

            // 3️⃣ Chống hoàn 2 lần
            if (_refundDao.HasCompletedRefund(dto.TicketId))
                throw new Exception("Vé đã được hoàn tiền");

            // 4️⃣ Tính tiền (ghi nhận nghiệp vụ)
            decimal refundFee = dto.TicketPrice * dto.RefundFeePercent / 100;
            decimal refundAmount = dto.TicketPrice - refundFee;

            using var conn = DbConnection.GetConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                // 5️⃣ Insert refund (ghi nhận)
                _refundDao.InsertRefund(
                    dto.TicketId,
                    refundAmount,
                    refundFee,
                    dto.AdminId,
                    tran
                );

                // 6️⃣ Update trạng thái vé
                _ticketDao.UpdateStatus(
                    dto.TicketId,
                    "REFUNDED",
                    tran
                );

                // 7️⃣ Ghi lịch sử
                _historyDao.Insert(
                    dto.TicketId,
                    "CANCELLED",
                    "REFUNDED",
                    dto.AdminId,
                    "Refund recorded",
                    tran
                );

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}
