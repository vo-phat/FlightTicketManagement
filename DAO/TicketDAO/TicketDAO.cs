using DTO.Ticket;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.TicketDAO
{
    public class TicketDAO
    {
        // Không khai báo biến thành viên ở đây, ngoại trừ các hằng số nếu cần.

        // Phương thức để lấy tất cả các vé
        public List<TicketDTO> GetAllTickets()
        {
            // 1. Tạo danh sách kết quả BÊN TRONG phương thức
            var listTickets = new List<TicketDTO>();

            // 2. Luôn dùng 'using' để quản lý kết nối
            using (MySqlConnection conn = DbConnection.GetConnection()) // Giả sử anh có lớp DbConnection helper
            {
                try
                {
                    conn.Open();

                    // 3. Liệt kê rõ các cột cần lấy
                    string sqlQuery = "SELECT ticket_id, ticket_passenger_id, flight_seat_id, ticket_number, issue_date, status FROM tickets";

                    using (MySqlCommand cmd = new MySqlCommand(sqlQuery, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 4. Tạo đối tượng DTO mới cho mỗi dòng dữ liệu
                                var ticket = new TicketDTO
                                {
                                    TicketId = reader.GetInt32("ticket_id"),
                                    PassengerId = reader.GetInt32("ticket_passenger_id"),
                                    FlightSeatId = reader.GetInt32("flight_seat_id"),
                                    TicketNumber = reader.GetString("ticket_number"),
                                    IssueDate = reader.GetDateTime("issue_date"),
                                    // Chuyển đổi từ chuỗi trong DB sang enum trong DTO
                                    Status = Enum.Parse<TicketStatus>(reader.GetString("status"), true) // true để không phân biệt hoa/thường
                                };

                                // 5. Thêm đối tượng mới tạo vào danh sách
                                listTickets.Add(ticket);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi ở đây
                    Console.WriteLine("Lỗi khi lấy danh sách vé: " + ex.Message);
                }
                // Kết nối sẽ tự đóng ở đây
            }

            // 6. Trả về danh sách kết quả
            return listTickets;
        }

    }
}
