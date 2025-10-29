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
            
            Console.WriteLine($"qui cute   sfkjasjfklasfj Đang đọc vé có ID");
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
                            // CHỈ SỬ DỤNG VÒNG LẶP WHILE
                            while (reader.Read())
                            {
                                // Mọi logic xử lý cho một dòng phải nằm BÊN TRONG vòng lặp này

                                // In ra Console để kiểm tra
                                //Console.WriteLine($"Đang đọc vé có ID: {reader.GetString("status")}");

                                // Tạo đối tượng TicketDTO
                                var ticket = new TicketDTO
                                {
                                    TicketId = reader.GetInt32("ticket_id"),
                                    PassengerId = reader.GetInt32("ticket_passenger_id"),
                                    FlightSeatId = reader.GetInt32("flight_seat_id"),
                                    TicketNumber = reader.GetString("ticket_number"),
                                    IssueDate = reader.GetDateTime("issue_date"), // đọc đúng
                                    
                                    Status = Enum.Parse<TicketStatus>(reader.GetString("status"), true)
                                    // Bỏ qua issue_date vì anh đã comment lại
                                };

                                // Thêm vào danh sách
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
