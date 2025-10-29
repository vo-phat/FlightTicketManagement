using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
    public enum TicketStatus
    {
        UNKNOWN,      // Trạng thái không xác định
        BOOKED,       // Đã đặt
        CONFIRMED,    // Đã xác nhận/xuất vé
        CHECKED_IN,
        BOARDED,      // Đã lên máy bay
        REFUNDED,     // Đã hoàn tiền
        CANCELED      // Đã hủy
    }
    public class TicketDTO
    {
        
        public int TicketId { get; set; }

        
        //public int PassengerId { get; set; }

        
        //public int FlightSeatId { get; set; }

        
        //public string TicketNumber { get; set; }

        
        //public DateTime IssueDate { get; set; }

        
        //public TicketStatus Status { get; set; }

       
    }
}
