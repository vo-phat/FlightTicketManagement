using DTO.Ticket.DTO.Ticket;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.TicketDAO
{
    public class TicketDetailDAO
    {
        public TicketDetailDTO GetTicketDetail(int ticketId)
        {
            using var conn = DbConnection.GetConnection();
            conn.Open();

            string sql = @"
                SELECT
                    t.ticket_id,
                    t.ticket_number,
                    t.status,
                    t.total_price,

                    b.booking_id,

                    pp.full_name,
                    pp.date_of_birth,
                    pp.passport_number,
                    pp.nationality,

                    f.flight_number,
                    f.departure_time,
                    f.arrival_time,

                    dep.airport_code AS dep_code,
                    arr.airport_code AS arr_code,

                    s.seat_number,
                    cc.class_name,

                    rp.is_refundable,
                    rp.refund_fee_percent

                FROM tickets t
                JOIN booking_passengers bp 
                    ON t.ticket_passenger_id = bp.booking_passenger_id
                JOIN bookings b 
                    ON bp.booking_id = b.booking_id
                JOIN passenger_profiles pp 
                    ON bp.profile_id = pp.profile_id
                JOIN flight_seats fs 
                    ON t.flight_seat_id = fs.flight_seat_id
                JOIN seats s 
                    ON fs.seat_id = s.seat_id
                JOIN cabin_classes cc 
                    ON s.class_id = cc.class_id
                JOIN flights f 
                    ON fs.flight_id = f.flight_id
                JOIN routes r 
                    ON f.route_id = r.route_id
                JOIN airports dep 
                    ON r.departure_place_id = dep.airport_id
                JOIN airports arr 
                    ON r.arrival_place_id = arr.airport_id
                LEFT JOIN ticket_refund_policy rp 
                    ON cc.class_id = rp.class_id
                WHERE t.ticket_id = @ticketId
                LIMIT 1;
            ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ticketId", ticketId);

            using var rd = cmd.ExecuteReader();

            if (!rd.Read()) return null;

            return new TicketDetailDTO
            {
                TicketId = rd.GetInt32("ticket_id"),
                TicketNumber = rd.GetString("ticket_number"),
                Status = rd.GetString("status"),
                TotalPrice = rd.GetDecimal("total_price"),

                BookingId = rd.GetInt32("booking_id"),

                PassengerName = rd.GetString("full_name"),
                DateOfBirth = rd["date_of_birth"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(rd["date_of_birth"]),
                PassportNumber = rd.GetString("passport_number"),
                Nationality = rd.GetString("nationality"),

                FlightNumber = rd.GetString("flight_number"),
                DepartureTime = rd.GetDateTime("departure_time"),
                ArrivalTime = rd.GetDateTime("arrival_time"),
                Route = $"{rd.GetString("dep_code")} → {rd.GetString("arr_code")}",

                SeatNumber = rd.GetString("seat_number"),
                CabinClass = rd.GetString("class_name"),

                IsRefundable = rd["is_refundable"] != DBNull.Value
                         && Convert.ToBoolean(rd["is_refundable"]),

                RefundFeePercent = rd["refund_fee_percent"] == DBNull.Value
                    ? 0
                    : Convert.ToInt32(rd["refund_fee_percent"])

            };
        }
    }
}
