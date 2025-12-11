using DAO;
using DAO.EF;
using DAO.Models;
using DAO.TicketDAO;
using DTO.Ticket;
using Microsoft.Identity.Client;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAO.TicketDAO
{
    public class TicketHistoryDAO
    {
        public List<TicketHistoryDTO> GetAllTicketHistories(int accountId)
        {
            var list = new List<TicketHistoryDTO>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                using (var cmd = new MySqlCommand(sqlBase, conn))
                {
                    // BẮT BUỘC PHẢI TRUYỀN PARAMETER
                    cmd.Parameters.AddWithValue("@AccountId", accountId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dto = new TicketHistoryDTO
                            {
                                TicketNumber = reader["TicketNumber"].ToString(),
                                PassengerName = reader["PassengerName"].ToString(),
                                FlightCode = reader["FlightCode"].ToString(),
                                DepartureAirport = reader["DepartureAirport"].ToString(),
                                ArrivalAirport = reader["ArrivalAirport"].ToString(),
                                DepartureTime = reader.GetDateTime(reader.GetOrdinal("DepartureTime")),
                                SeatCode = reader["SeatCode"].ToString(),
                                Status = reader["Status"].ToString(),
                                BaggageSummary = reader["BaggageSummary"] == DBNull.Value
                                    ? null
                                    : reader["BaggageSummary"].ToString()
                            };

                            list.Add(dto);
                        }
                    }
                }
            }

            return list;
        }



        string sqlBase =
                "SELECT " +
                "    t.ticket_number AS TicketNumber, " +
                "    pp.full_name AS PassengerName, " +
                "    f.flight_number AS FlightCode, " +
                "    ap_from.airport_code AS DepartureAirport, " +
                "    ap_to.airport_code AS ArrivalAirport, " +
                "    f.departure_time AS DepartureTime, " +
                "    s.seat_number AS SeatCode, " +
                "    t.status AS Status, " +

                // HÀNH LÝ GỘP
                "    GROUP_CONCAT( " +
                "        CASE " +
                "            WHEN tb.baggage_type = 'carry_on' THEN CONCAT('Xách tay ', cb.weight_kg, 'kg', IF(tb.note IS NOT NULL AND tb.note <> '', CONCAT(' (', tb.note, ')'), '')) " +
                "            WHEN tb.baggage_type = 'checked' THEN CONCAT('Ký gửi ', chb.weight_kg, 'kg', IF(tb.note IS NOT NULL AND tb.note <> '', CONCAT(' (', tb.note, ')'), '')) " +
                "        END " +
                "    SEPARATOR ', ') AS BaggageSummary " +

                "FROM accounts a " +
                "JOIN bookings b ON a.account_id = b.account_id " +
                "JOIN booking_passengers bp ON b.booking_id = bp.booking_id " +
                "JOIN passenger_profiles pp ON bp.profile_id = pp.profile_id " +
                "JOIN tickets t ON bp.booking_passenger_id = t.ticket_passenger_id " +
                "JOIN flight_seats fs ON t.flight_seat_id = fs.flight_seat_id " +
                "JOIN seats s ON fs.seat_id = s.seat_id " +
                "JOIN flights f ON fs.flight_id = f.flight_id " +
                "JOIN routes r ON f.route_id = r.route_id " +
                "JOIN airports ap_from ON r.departure_place_id = ap_from.airport_id " +
                "JOIN airports ap_to ON r.arrival_place_id = ap_to.airport_id " +

                // JOIN HÀNH LÝ
                "LEFT JOIN ticket_baggage tb ON t.ticket_id = tb.ticket_id " +
                "LEFT JOIN carryon_baggage cb ON tb.carryon_id = cb.carryon_id " +
                "LEFT JOIN checked_baggage chb ON tb.checked_id = chb.checked_id " +

                "WHERE a.account_id = @AccountId " +

                // GROUP BY để không bị nhân dòng
                "GROUP BY " +
                "    t.ticket_number, pp.full_name, f.flight_number, " +
                "    ap_from.airport_code, ap_to.airport_code, " +
                "    f.departure_time, s.seat_number, t.status " +

                "ORDER BY t.issue_date DESC;";

    }
}
