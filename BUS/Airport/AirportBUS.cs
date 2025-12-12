using DAO.Airport;
using DAO.Route;
using DTO.Airport;
using System;
using System.Collections.Generic;

namespace BUS.Airport
{
    public class AirportBUS
    {
        private readonly AirportDAO airportDAO;

        public AirportBUS()
        {
            airportDAO = new AirportDAO();
        }

        #region Lấy danh sách sân bay
        public List<AirportDTO> GetAllAirports()
        {
            try
            {
                return airportDAO.GetAllAirports();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sân bay: " + ex.Message, ex);
            }
        }

        #endregion

        #region Thêm sân bay mới
        public bool AddAirport(AirportDTO airport, out string message)
        {
            message = string.Empty;

            // Kiểm tra hợp lệ
            if (!airport.IsValid(out string validationError))
            {
                message = validationError;
                return false;
            }

            try
            {
                bool result = airportDAO.InsertAirport(airport);
                message = result ? "Thêm sân bay thành công" : "Không thể thêm sân bay";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi thêm sân bay: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Cập nhật thông tin sân bay
        public bool UpdateAirport(AirportDTO airport, out string message)
        {
            message = string.Empty;

            // Kiểm tra hợp lệ
            if (!airport.IsValid(out string validationError))
            {
                message = validationError;
                return false;
            }

            try
            {
                bool result = airportDAO.UpdateAirport(airport);
                message = result ? "Cập nhật sân bay thành công" : "Không thể cập nhật sân bay";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi cập nhật sân bay: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Xóa sân bay
        public bool DeleteAirport(int airportId, out string message)
        {
            message = string.Empty;

            if (airportId <= 0)
            {
                message = "ID sân bay không hợp lệ";
                return false;
            }

            try
            {
                bool result = airportDAO.DeleteAirport(airportId);
                message = result ? "Xóa sân bay thành công" : "Không thể xóa sân bay";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi xóa sân bay: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Tìm kiếm sân bay
        public List<AirportDTO> SearchAirports(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return airportDAO.GetAllAirports();

                return airportDAO.SearchAirports(keyword.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sân bay: " + ex.Message, ex);
            }
        }
        #endregion
    }
}
