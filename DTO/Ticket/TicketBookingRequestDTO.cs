using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
    public class TicketBookingRequestDTO
    {
        // ============================
        // 1. Passenger / Profile info
        // ============================

        public int? AccountId { get; set; }         // nếu user login
        public string? FullName { get; set; }       // tên hành khách
        public DateTime? DateOfBirth { get; set; }  // ngày sinh
        public string? PhoneNumber { get; set; }    // số điện thoại
        public string? PassportNumber { get; set; } // số hộ chiếu
        public string? Nationality { get; set; }    // quốc tịch (VN, US,…)
        public string? Email { get; set; }          // email liên hệ

        // ============================
        // 2. Flight info
        // ============================

        public int? FlightId { get; set; }          // chuyến bay đã chọn
        public DateTime? FlightDate { get; set; }   // ngày bay

        // ============================
        // 3. Seat info
        // ============================

        public int? SeatId { get; set; }            // seat_id trong bảng seat
        public int? FlightSeatId { get; set; }      // flight_seat_id nếu chọn ghế từ bảng flight_seat
        public string? SeatNumber { get; set; }     // VD: "12A"
        public int? ClassId { get; set; }           // hạng ghế (economy/business/vip)

        // ============================
        // 4. Extra baggage (mua thêm)
        // ============================

        public int? CarryOnId { get; set; }         // hành lý xách tay thêm (nếu có)
        public int? CheckedId { get; set; }         // hành lý ký gửi mua thêm
        public int? Quantity { get; set; } =  1;        // số lượng kiện hành lý, mặc định = 1
        public string? BaggageNote { get; set; }    // ghi chú riêng cho hành lý

        // Hiển thị gộp (GUI)
        public string? BaggageDisplayText { get; set; } // VD: "20kg - 450.000đ"

        // ============================
        // 5. Ticket info
        // ============================

        public string? TicketNumber { get; set; }   // BUS có thể auto-generate
        public string? Note { get; set; }           // ghi chú của vé
    }

}
