using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.Airline;
using DTO.Airline;
namespace BUS.Airline
{
    public class AirlineBUS
    {
        private readonly AirlineDAO airlineDAO;
        public AirlineBUS()
        {
            airlineDAO = new AirlineDAO();
        }
        #region Lấy danh sách hãng hàng không
        public List<AirlineDTO> GetAllAirlines()
        {
            try
            {
                return airlineDAO.GetAllAirlines();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hãng hàng không: " + ex.Message, ex);
            }
        }
        #endregion

        #region Thêm hãng hàng không mới
        public bool AddAirline(AirlineDTO airline, out string message)
        {
            message = string.Empty;
            // Kiểm tra hợp lệ
            if (!airline.IsValid(out string validationError))
            {
                message = validationError;
                return false;
            }
            try
            {
                bool result = airlineDAO.InsertAirline(airline);
                message = result ? "Thêm hãng hàng không thành công" : "Không thể thêm hãng hàng không";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi thêm hãng hàng không: " + ex.Message;
                return false;
            }
        }
        #endregion
        #region Cập nhật thông tin hãng hàng không
        public bool UpdateAirline(AirlineDTO airline, out string message)
        {
            message = string.Empty;
            // Kiểm tra hợp lệ
            if (!airline.IsValid(out string validationError))
            {
                message = validationError;
                return false;
            }
            try
            {
                bool result = airlineDAO.UpdateAirline(airline);
                message = result ? "Cập nhật hãng hàng không thành công" : "Không thể cập nhật hãng hàng không";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi cập nhật hãng hàng không: " + ex.Message;
                return false;
            }
        }
        #endregion
        #region Xóa hãng hàng không
        public bool DeleteAirline(int airlineId, out string message)
        {
            message = string.Empty;
            try
            {
                bool result = airlineDAO.DeleteAirline(airlineId);
                message = result ? "Xóa hãng hàng không thành công" : "Không thể xóa hãng hàng không";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi xóa hãng hàng không: " + ex.Message;
                return false;
            }
        }

        #endregion

        #region Tìm kiếm hãng hàng không theo tên hoặc mã
        public List<AirlineDTO> SearchAirlines(string keyword)
        {
            try
            {
                return airlineDAO.SearchAirlines(keyword);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm hãng hàng không: " + ex.Message, ex);
            }
        }
        #endregion
    }
}
