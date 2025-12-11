using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
    public class TicketHistoryDTO
    {
        public string TicketNumber { get; set; }          // Mã vé
        public string PassengerName { get; set; }         // Tên hành khách
        public string FlightCode { get; set; }            // Mã chuyến bay
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime DepartureTime { get; set; }       // Ngày giờ bay
        public string SeatCode { get; set; }              // Ghế
        public string Status { get; set; }                // Upcoming / Completed / Cancelled
        public string? BaggageSummary { get; set; }       // Hành lý gộp vào 1 dòng
    }


}