using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Ticket;
using MySqlConnector;
using System.Collections.Generic;
namespace DAO.TicketDAO
{

    public class TicketListDAO
    {
        // ================================
        // GET ALL TICKETS
        // ================================
        public List<TicketListDTO> GetAllTickets()
        {
            string sql = BaseQuery();
            return ExecuteQuery(sql, null, null);
        }

        // ================================
        // SEARCH TICKETS
        // ================================
        public List<TicketListDTO> SearchTickets(string keyword, string status)
        {
            string sql = BaseQuery() +
            @" WHERE 
                (t.ticket_number LIKE @kw
                OR pp.full_name LIKE @kw
                OR f.flight_number LIKE @kw) ";

            if (status != "ALL")
                sql += " AND t.status = @statusF ";

            return ExecuteQuery(sql, keyword, status);
        }

        // ================================
        // UPDATE STATUS (Cancel / Refund)
        // ================================
        public bool UpdateStatus(int ticketId, string newStatus)
        {
            string sql = @"UPDATE tickets SET status = @status WHERE ticket_id = @tid";

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@tid", ticketId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ================================
        // BASE JOIN QUERY
        // ================================
        private string BaseQuery()
        {
            return @"
        SELECT 
            t.ticket_id,
            t.total_price,
            t.ticket_number,
            pp.full_name,
            f.flight_number,

            CONCAT(a1.airport_code, ' → ', a2.airport_code) AS route_name,

            f.departure_time,
             s.seat_number AS seat_code,
            fs.base_price,
            t.status,
            arp.refund_fee_percent,
            arp.is_refundable
          
            
        FROM tickets t

        JOIN booking_passengers bp 
            ON t.ticket_passenger_id = bp.booking_passenger_id

        JOIN passenger_profiles pp 
            ON bp.profile_id = pp.profile_id

        JOIN flight_seats fs 
            ON t.flight_seat_id = fs.flight_seat_id

        JOIN seats s 
            ON fs.seat_id = s.seat_id

        JOIN flights f 
            ON fs.flight_id = f.flight_id

        JOIN routes r 
            ON f.route_id = r.route_id

        JOIN airports a1 
            ON r.departure_place_id = a1.airport_id

        JOIN airports a2 
            ON r.arrival_place_id = a2.airport_id
        LEFT JOIN ticket_refund_policy arp 
            ON s.class_id  = arp.class_id 
    ";
        }
        /// mới thêm đang test

        // ================================
        // EXECUTE + MAP DTO
        // ================================
        private List<TicketListDTO> ExecuteQuery(string sql, string keyword, string status)
        {
            List<TicketListDTO> list = new List<TicketListDTO>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                if (keyword != null)
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                if (status != null && status != "ALL")
                    cmd.Parameters.AddWithValue("@statusF", status);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new TicketListDTO
                    {
                        TicketId = reader.GetInt32("ticket_id"),
                        TicketNumber = reader.GetString("ticket_number"),
                        PassengerName = reader.GetString("full_name"),
                        FlightNumber = reader.GetString("flight_number"),
                        Route = reader.GetString("route_name"),
                        DepartureTime = reader.GetDateTime("departure_time"),
                        SeatCode = reader.GetString("seat_code"),
                        Price = reader.GetDecimal("total_price"),
                        Status = reader.GetString("status"),
                        IsRefundable = reader.GetBoolean("is_refundable"),
                        RefundFeePercent = reader.GetInt16("refund_fee_percent")
                         
                     });
                }
            }

            return list;
        }
    }
}
