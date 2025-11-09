using DTO.Ticket;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.TicketDAO
{
    public class TicketFilterDAO
    {
        public List<TicketFilterDTO> ReadListFilterTickets(
            string? BookingCodeID,
            string? FlightCodeID,
            DateTime? NgayBay,
            string? Status,
            string? PhoneNumber)
        {
            List<TicketFilterDTO> ListFilterTickets = new List<TicketFilterDTO>();

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string sqlQuery =
                        "SELECT " +
                        " t.ticket_number             AS TicketNumber, " +
                        " t.issue_date                AS IssuedDate, " +
                        " t.status                    AS Status, " +

                        " p.full_name                 AS PassengerName, " +
                        " p.date_of_birth             AS DateOfBirth, " +
                        " p.phone_number              AS PassengerPhone, " +
                        " p.passport_number           AS PassportNumber, " +

                        " f.departure_time            AS DepartureTime, " +
                        " f.arrival_time              AS ArrivalTime, " +

                        " fs.base_price               AS BasePrice, " +
                        " s.seat_number               AS SeatNumber, " +
                        " dep.airport_name            AS AirportName, " +
                        " al.airline_name             AS AirlineName " +

                        "FROM Tickets t " +
                        "JOIN Flight_Seats fs        ON t.flight_seat_id = fs.flight_seat_id " +
                        "JOIN Seats s                ON fs.seat_id = s.seat_id " +
                        "JOIN Flights f              ON fs.flight_id = f.flight_id " +
                        "JOIN Aircrafts ac           ON f.aircraft_id = ac.aircraft_id " +
                        "JOIN Airlines al            ON ac.airline_id = al.airline_id " +
                        "JOIN Routes r               ON f.route_id = r.route_id " +
                        "JOIN Airports dep           ON r.departure_place_id = dep.airport_id " +
                        "JOIN Booking_Passengers bp  ON t.ticket_passenger_id = bp.booking_passenger_id " +
                        "JOIN Bookings b             ON b.booking_id = bp.booking_id " +
                        "JOIN Passenger_Profiles p   ON bp.profile_id = p.profile_id " +
                        "WHERE 1 = 1 "+ 

                        "AND (@BookingCodeID IS NULL OR b.booking_id = @BookingCodeID) " +
                        "AND (@FlightCodeID IS NULL OR f.flight_id = @FlightCodeID ) " +
                        "AND (@NgayBay IS NULL OR DATE(f.departure_time) = DATE(@NgayBay)) " +
                        "AND (@Status IS NULL OR t.status = @Status) " +
                        "AND (@PhoneNumber IS NULL OR p.phone_number = @PhoneNumber);";

                    using (MySqlCommand cmd = new MySqlCommand(sqlQuery, conn))
                    {
                        // Thêm từng tham số, kiểm tra null trước khi add
                        cmd.Parameters.AddWithValue("@BookingCodeID", string.IsNullOrEmpty(BookingCodeID) ? DBNull.Value : BookingCodeID);
                        cmd.Parameters.AddWithValue("@FlightCodeID", string.IsNullOrEmpty(FlightCodeID) ? DBNull.Value : FlightCodeID);
                        cmd.Parameters.AddWithValue("@NgayBay", NgayBay.Value.ToString("yyyy-dd-MM"));


                        cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(Status) ? DBNull.Value : Status);
                        cmd.Parameters.AddWithValue("@PhoneNumber", string.IsNullOrEmpty(PhoneNumber) ? DBNull.Value : PhoneNumber);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var ticket = new TicketFilterDTO
                                {
                                    TicketNumber = reader["TicketNumber"] as string,
                                    IssuedDate = reader["IssuedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["IssuedDate"]),
                                    Status = reader["Status"] as string,

                                    PassengerName = reader["PassengerName"] as string,
                                    DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DateOfBirth"]),
                                    PassengerPhone = reader["PassengerPhone"] as string,
                                    PassportNumber = reader["PassportNumber"] as string,

                                    DepartureTime = reader["DepartureTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DepartureTime"]),
                                    ArrivalTime = reader["ArrivalTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ArrivalTime"]),

                                    BasePrice = reader["BasePrice"] == DBNull.Value ? (long?)null : Convert.ToInt64(reader["BasePrice"]),
                                    SeatNumber = reader["SeatNumber"] as string,
                                    AirportName = reader["AirportName"] as string,
                                    AirlineName = reader["AirlineName"] as string
                                };

                                ListFilterTickets.Add(ticket);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy danh sách vé đã lọc: " + ex.Message);
            }

            return ListFilterTickets;
        }

        public List<TicketFilterDTO> ReadListTenTicketsNews()
        {
            List<TicketFilterDTO> ListFilterTickets = new List<TicketFilterDTO>();
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string sqlQuery =
                                "SELECT " +
                                " t.ticket_number             AS TicketNumber, " +
                                " t.issue_date                AS IssuedDate, " +
                                " t.status                    AS Status, " +

                                " p.full_name                 AS PassengerName, " +
                                " p.date_of_birth             AS DateOfBirth, " +
                                " p.phone_number              AS PassengerPhone, " +
                                " p.passport_number           AS PassportNumber, " +

                                " f.departure_time            AS DepartureTime, " +
                                " f.arrival_time              AS ArrivalTime, " +

                                " fs.base_price               AS BasePrice," +
                                " s.seat_number               AS SeatNumber, " +
                                " dep.airport_name            AS AirportName, " +
                                " al.airline_name             AS AirlineName " +

                                "FROM Tickets t " +
                                "JOIN Flight_Seats fs        ON t.flight_seat_id = fs.flight_seat_id " +
                                "JOIN Seats s                ON fs.seat_id = s.seat_id " +
                                "JOIN Flights f              ON fs.flight_id = f.flight_id " +
                                "JOIN Aircrafts ac           ON f.aircraft_id = ac.aircraft_id " +
                                "JOIN Airlines al            ON ac.airline_id = al.airline_id " +
                                "JOIN Routes r               ON f.route_id = r.route_id " +
                                "JOIN Airports dep           ON r.departure_place_id = dep.airport_id " +
                                "JOIN Booking_Passengers bp  ON t.ticket_passenger_id = bp.booking_passenger_id " +
                                "JOIN Passenger_Profiles p   ON bp.profile_id = p.profile_id " +
                                "WHERE 1 = 1;";

                    using (MySqlCommand cmd = new MySqlCommand(sqlQuery, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var ticket = new TicketFilterDTO
                                {
                                    // Vé
                                    TicketNumber = reader.IsDBNull("TicketNumber") ? null : reader.GetString("TicketNumber"),
                                    IssuedDate = reader.IsDBNull("IssuedDate") ? null : reader.GetDateTime("IssuedDate"),
                                    Status = reader.IsDBNull("Status") ? null : reader.GetString("Status"),

                                    //// Hành khách
                                    PassengerName = reader.IsDBNull("PassengerName") ? null : reader.GetString("PassengerName"),
                                    DateOfBirth = reader.IsDBNull("DateOfBirth") ? null : reader.GetDateTime("DateOfBirth"),
                                    PassengerPhone = reader.IsDBNull("PassengerPhone") ? null : reader.GetString("PassengerPhone"),
                                    PassportNumber = reader.IsDBNull("PassportNumber") ? null : reader.GetString("PassportNumber"),

                                    //// Chuyến bay
                                    DepartureTime = reader.IsDBNull("DepartureTime") ? null : reader.GetDateTime("DepartureTime"),
                                    ArrivalTime = reader.IsDBNull("ArrivalTime") ? null : reader.GetDateTime("ArrivalTime"),

                                    //// Ghế, sân bay, hãng bay
                                    BasePrice = reader.IsDBNull("BasePrice") ? null : reader.GetInt64("BasePrice"),
                                    SeatNumber = reader.IsDBNull("SeatNumber") ? null : reader.GetString("SeatNumber"),
                                    AirportName = reader.IsDBNull("AirportName") ? null : reader.GetString("AirportName"),
                                    AirlineName = reader.IsDBNull("AirlineName") ? null : reader.GetString("AirlineName"),
                                };


                                ListFilterTickets.Add(ticket);
                            }


                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy danh sách vé đã lọc: " + ex.Message);
            }
            return ListFilterTickets;

        }

    }
}

