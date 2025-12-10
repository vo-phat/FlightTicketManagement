using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.TicketDAO;
using DTO.Ticket;
namespace BUS.Ticket
{
    public class SaveTicketRequestBUS
    {
        private readonly SaveTicketRequestDAO _dao;

        public SaveTicketRequestBUS()
        {
            _dao = new SaveTicketRequestDAO();
        }

        // ============================
        // 1) ONE WAY
        // ============================
        public int SaveOneWay(List<TicketBookingRequestDTO> outbound, int accountId)
        {
            if (outbound == null || outbound.Count == 0)
                throw new System.Exception("Danh sách vé chiều đi rỗng.");

            return _dao.CreateBookingOneWay(outbound, accountId);
        }

        // ============================
        // 2) ROUND TRIP (2 list)
        // ============================
        public int SaveRoundTrip(
            List<TicketBookingRequestDTO> outbound,
            List<TicketBookingRequestDTO> inbound,
            int accountId)
        {
            if (outbound == null || inbound == null)
                throw new System.Exception("Outbound hoặc inbound NULL.");

            if (outbound.Count != inbound.Count)
                throw new System.Exception("Số lượng khách chiều đi và về không khớp.");

            return _dao.CreateBookingRoundTrip(outbound, inbound, accountId);
        }
    }
}
