using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Ticket
{
     public  class TicketFilterDTO
    {


        // Vé
        public string? TicketNumber { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string? Status { get; set; }

        // Hành khách
        public string? PassengerName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PassengerPhone { get; set; }
        public string? PassportNumber { get; set; }


        //flight_seat
        public Int64? BasePrice { get; set; }
        // Chuyến bay
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        // seat_number
        public string? SeatNumber { get; set; }
        // airport name
        public string? AirportName { get; set; }
        // airline name
        public string? AirlineName { get; set; }
        // chuwa cho fare_rule
    }
}
