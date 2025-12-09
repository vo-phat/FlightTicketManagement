using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Ticket;
using DAO.TicketDAO;
namespace BUS.Ticket
{
    public class TicketsHistoryBUS
    {
        private readonly TicketHistoryDAO _ticketDao;

        public TicketsHistoryBUS()
        {
            _ticketDao = new TicketHistoryDAO();
        }

        public List<TicketHistoryDTO> GetAll(int accountId)
        {
            return _ticketDao.GetAllTicketHistories(accountId);
        }

        // 1. Tìm theo mã vé
        public List<TicketHistoryDTO> FilterByTicketNumber(List<TicketHistoryDTO> source, string ticketNumber)
        {
            if (string.IsNullOrWhiteSpace(ticketNumber)) return source;

            return source
                .Where(c => c.TicketNumber.Contains(ticketNumber, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // 2. Lọc theo trạng thái
        public List<TicketHistoryDTO> FilterByStatus(List<TicketHistoryDTO> source, string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return source;

            return source
                .Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // 3. Lọc theo ngày bay
        public List<TicketHistoryDTO> FilterByDate(List<TicketHistoryDTO> source, DateTime? from, DateTime? to)
        {
            if (from == null && to == null) return source;

            return source
                .Where(c =>
                    (from == null || c.DepartureTime >= from.Value) &&
                    (to == null || c.DepartureTime <= to.Value)
                )
                .ToList();
        }

        // 4. Lọc theo tên hành khách
        public List<TicketHistoryDTO> FilterByPassenger(List<TicketHistoryDTO> source, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return source;

            return source
                .Where(c => c.PassengerName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // 5. Lọc theo mã chuyến bay
        public List<TicketHistoryDTO> FilterByFlightCode(List<TicketHistoryDTO> source, string flightCode)
        {
            if (string.IsNullOrWhiteSpace(flightCode)) return source;

            return source
                .Where(c => c.FlightCode.Contains(flightCode, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
