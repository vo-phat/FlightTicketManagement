using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using DAO;
using Dapper;
using DTO.BaggageDTO;
namespace DAO.BaggageDAO
{
    public static class BaggageCheckinDAO
    {
        public static bool checkConect()
        {
            using var conn = DbConnection.GetConnection();

            return conn.Ping();

        }

        public static List<BaggageDTO> getAllBaggage()
        {
            string sql = "SELECT * FROM Baggages;"; // Lấy tất cả các cột từ bảng Baggages

            try
            {
                using (var conn = DbConnection.GetConnection())
                {
                    // Dùng Dapper để thực thi và ánh xạ kết quả
                    var baggageList = conn.Query<BaggageDTO>(sql).ToList();
                    return baggageList;
                }
            }
            catch (Exception ex)
            {
                // Luôn ghi lại lỗi để biết chuyện gì đang xảy ra
                Console.WriteLine($"[DAO ERROR] Lỗi khi lấy danh sách hành lý: {ex.Message}");

                // Trả về danh sách rỗng thay vì null để an toàn hơn
                return new List<BaggageDTO>();
            }
        }
    } 
    
}
