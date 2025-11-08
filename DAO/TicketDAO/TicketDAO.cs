using DTO.BaggageDTO;
using DTO.Ticket;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DAO.TicketDAO
{
    public class TicketDAO
    {
        
        public List<TicketDTO> GetAllTickets()
        {
            
            var listTickets = new List<TicketDTO>();
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string sqlQuery = "SELECT ticket_id, ticket_passenger_id, flight_seat_id, ticket_number, issue_date, status FROM tickets";
                    using (MySqlCommand cmd = new MySqlCommand(sqlQuery, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                               
                                var ticket = new TicketDTO
                                {
                                    TicketId = reader.GetInt32("ticket_id"),
                                    PassengerId = reader.GetInt32("ticket_passenger_id"),
                                    FlightSeatId = reader.GetInt32("flight_seat_id"),
                                    TicketNumber = reader.GetString("ticket_number"),
                                    IssueDate = reader.GetDateTime("issue_date"), // đọc đúng
                                    
                                    Status = Enum.Parse<TicketStatus>(reader.GetString("status"), true)
                                };

                                listTickets.Add(ticket);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi lấy tất cả vé: " + ex.Message);
                }
            }

            Console.WriteLine($"Tổng số vé đọc được: {listTickets.Count}");
            return listTickets;
        }

    }
}
