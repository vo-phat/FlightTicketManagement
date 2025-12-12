using DAO.FlightSeat;
using DAO;
using DAO.TicketDAO;
using DTO.Ticket;

namespace BUS.Ticket
{
    public class CancelTicketBUS
    {
        private readonly TicketDAO _ticketDao = new();
        private readonly FlightSeatDAO _seatDao = new();
        private readonly TicketHistoryDAO _historyDao = new();

        public void CancelTicket(
            TicketListDTO dto,
            int adminId, string reason)
        {
            // 1️⃣ CHECK NGHIỆP VỤ
            if (dto.Status != "BOOKED")
                throw new Exception("Chỉ được hủy vé BOOKED");

            using var conn = DbConnection.GetConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                // 2️⃣ UPDATE VÉ
                _ticketDao.UpdateStatus(
                    dto.TicketId,
                    "CANCELLED",
                    tran
                );

                // 3️⃣ TRẢ GHẾ
                _seatDao.ReleaseSeatByTicketId(
                    dto.TicketId,
                    tran
                );

                // 4️⃣ LƯU LỊCH SỬ
                _historyDao.Insert(
                    dto.TicketId,
                    dto.Status,
                    "CANCELLED",
                    adminId,
                    reason,
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
