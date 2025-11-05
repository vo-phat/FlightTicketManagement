using System;
using System.Collections.Generic;
using DAO.FlightSeat;
using DTO.FlightSeat;
using System.Linq; // Cần cho LINQ và HashSet

namespace BUS.FlightSeat
{
    public class FlightSeatBUS
    {
        private readonly FlightSeatDAO _dao;
        // Danh sách trạng thái hợp lệ trong database (Enum trong SQL)
        private static readonly HashSet<string> ValidStatuses = new HashSet<string> { "AVAILABLE", "BOOKED", "BLOCKED" };

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
                throw new Exception($"Lỗi khi lấy danh sách ghế của chuyến bay: {ex.Message}", ex);
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
                throw new Exception($"Lỗi khi lọc danh sách ghế theo chuyến bay: {ex.Message}", ex);
            }
        }
        #endregion

        #region Cập nhật trạng thái ghế
        public bool UpdateSeatStatus(int flightSeatId, string newStatus, out string message)
        {
            message = string.Empty;
            string statusUpper = newStatus.Trim().ToUpper();

            if (flightSeatId <= 0)
            {
                message = "ID ghế không hợp lệ";
                return false;
            }

            // [LOGIC NGHIỆP VỤ] Kiểm tra trạng thái hợp lệ
            if (!ValidStatuses.Contains(statusUpper))
            {
                message = $"Trạng thái '{newStatus}' không hợp lệ. Chỉ chấp nhận AVAILABLE, BOOKED, hoặc BLOCKED.";
                return false;
            }

            try
            {
                bool result = _dao.UpdateSeatStatus(flightSeatId, statusUpper);
                message = result ? "Cập nhật trạng thái ghế thành công" : "Không thể cập nhật trạng thái";
                return result;
            }
            catch (Exception ex)
            {
                message = $"Lỗi khi cập nhật trạng thái ghế: {ex.Message}";
                return false;
            }
        }
        #endregion

        #region Cập nhật đầy đủ thông tin ghế (flight_id, seat_id, base_price, seat_status)
        public bool UpdateFlightSeat(FlightSeatDTO dto, out string message)
        {
            message = string.Empty;

            if (dto.FlightSeatId <= 0)
            {
                message = "ID ghế chuyến bay không hợp lệ.";
                return false;
            }

            if (dto.FlightId <= 0)
            {
                message = "Chưa chọn chuyến bay hợp lệ.";
                return false;
            }

            if (dto.SeatId <= 0)
            {
                message = "Chưa chọn ghế hợp lệ.";
                return false;
            }

            if (dto.BasePrice < 0)
            {
                message = "Giá cơ bản không thể âm.";
                return false;
            }

            string statusUpper = (dto.SeatStatus ?? "").Trim().ToUpper();
            if (!ValidStatuses.Contains(statusUpper))
            {
                message = $"Trạng thái '{dto.SeatStatus}' không hợp lệ. Chỉ chấp nhận AVAILABLE, BOOKED, hoặc BLOCKED.";
                return false;
            }

            try
            {
                bool result = _dao.UpdateFlightSeat(dto);
                message = result ? "Cập nhật thông tin ghế thành công!" : "Không thể cập nhật thông tin ghế.";
                return result;
            }
            catch (Exception ex)
            {
                message = $"Lỗi khi cập nhật thông tin ghế: {ex.Message}";
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
                throw new Exception($"Lỗi khi xem sơ đồ ghế: {ex.Message}", ex);
            }
        }
        #endregion

        #region Danh sách tất cả ghế theo máy bay
        public List<FlightSeatDTO> GetAllWithDetails()
        {
            try
            {
                return _dao.GetAllFlightSeats();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tải danh sách ghế theo máy bay: {ex.Message}", ex);
            }
        }
        #endregion
    }
}