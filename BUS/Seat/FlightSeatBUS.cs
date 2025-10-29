using System;
using System.Collections.Generic;
using DAO.FlightSeat;
using DTO.FlightSeat;

namespace BUS.FlightSeat
{
    public class FlightSeatBUS
    {
        private readonly FlightSeatDAO _dao;

        public FlightSeatBUS()
        {
            _dao = new FlightSeatDAO();
        }

        #region Lấy danh sách ghế theo chuyến bay
        public List<FlightSeatDTO> GetSeatsByFlight(int flightId)
        {
            try
            {
                return _dao.GetSeatsByFlight(flightId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách ghế của chuyến bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Lọc danh sách ghế theo chuyến bay
        public List<FlightSeatDTO> FilterSeats(int flightId, string? status, decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                return _dao.FilterSeats(flightId, status, minPrice, maxPrice);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lọc danh sách ghế theo chuyến bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Cập nhật trạng thái ghế
        public bool UpdateSeatStatus(int flightSeatId, string newStatus, out string message)
        {
            message = string.Empty;

            if (flightSeatId <= 0)
            {
                message = "ID ghế chuyến bay không hợp lệ";
                return false;
            }

            if (string.IsNullOrWhiteSpace(newStatus))
            {
                message = "Trạng thái mới không hợp lệ";
                return false;
            }

            try
            {
                bool result = _dao.UpdateSeatStatus(flightSeatId, newStatus.Trim().ToUpper());
                message = result ? "Cập nhật trạng thái ghế thành công" : "Không thể cập nhật trạng thái";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi cập nhật trạng thái ghế: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Xem sơ đồ ghế
        public List<FlightSeatDTO> GetSeatMap(int? flightId, int? aircraftId, int? classId)
        {
            try
            {
                return _dao.GetSeatMap(flightId, aircraftId, classId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xem sơ đồ ghế: " + ex.Message, ex);
            }
        }
        #endregion
    }
}
