using DAO.Seat;
using DTO.Seat;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace BUS.Seat
{
    public class SeatBUS
    {
        private readonly SeatDAO _seatDAO;

        public SeatBUS()
        {
            _seatDAO = new SeatDAO();
        }

        #region Lấy danh sách tất cả ghế (Cơ bản)
        public List<SeatDTO> GetAllSeats()
        {
            try
            {
                return _seatDAO.GetAllSeats();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách ghế cơ bản: {ex.Message}", ex);
            }
        }
        #endregion

        #region Lấy danh sách tất cả ghế kèm chi tiết (Hạng ghế, Máy bay)
        /// <summary>
        /// Lấy danh sách tất cả ghế bao gồm thông tin chi tiết về hạng ghế và máy bay.
        /// </summary>
        public List<SeatDTO> GetAllSeatsWithDetails()
        {
            try
            {
                return _seatDAO.GetAllSeatsWithDetails();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách ghế kèm chi tiết: {ex.Message}", ex);
            }
        }
        #endregion

        #region Lọc danh sách ghế
        public List<SeatDTO> FilterSeats(int? aircraftId, int? classId)
        {
            try
            {
                return _seatDAO.FilterSeats(aircraftId, classId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lọc danh sách ghế: {ex.Message}", ex);
            }
        }
        #endregion

        #region Tìm kiếm ghế
        public List<SeatDTO> SearchSeats(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return _seatDAO.GetAllSeats();

                return _seatDAO.SearchSeats(keyword.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm ghế: {ex.Message}", ex);
            }
        }
        #endregion

        #region Xem thông tin ghế
        public SeatDTO? GetSeatById(int seatId)
        {
            try
            {
                return _seatDAO.GetSeatById(seatId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xem thông tin ghế: {ex.Message}", ex);
            }
        }
        #endregion

        #region Tạo ghế mới
        public bool AddSeat(SeatDTO seat, out string message)
        {
            message = string.Empty;

            if (!seat.IsValid(out string error))
            {
                message = error;
                return false;
            }

            // Có thể thêm logic kiểm tra trùng lặp (nếu cần)

            try
            {
                bool result = _seatDAO.InsertSeat(seat);
                message = result ? "Thêm ghế mới thành công" : "Không thể thêm ghế";
                return result;
            }
            catch (Exception ex)
            {
                message = $"Lỗi khi thêm ghế mới: {ex.Message}";
                return false;
            }
        }
        #endregion

        //#region Sửa thông tin ghế
        //public bool UpdateSeat(SeatDTO seat, out string message)
        //{
        //    message = string.Empty;

        //    // 1. Kiểm tra validation (chủ yếu là SeatNumber trong DTO)
        //    if (!seat.IsValid(out string error))
        //    {
        //        message = error;
        //        return false;
        //    }

        //    // 2. [LOGIC NGHIỆP VỤ] Kiểm tra ghế có đang được sử dụng không (Giả định DAO có hàm kiểm tra)
        //    // if (_seatDAO.IsSeatCurrentlyInUse(seat.SeatId))
        //    // {
        //    //     message = "Không thể cập nhật: Ghế đang được đặt trên chuyến bay.";
        //    //     return false;
        //    // }

        //    try
        //    {
        //        // Gọi DAO (chỉ cập nhật SeatNumber)
        //        bool result = _seatDAO.UpdateSeat(seat);
        //        message = result ? "Cập nhật ghế thành công" : "Không thể cập nhật ghế (Có thể ID không tồn tại)";
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        message = $"Lỗi khi cập nhật ghế: {ex.Message}";
        //        return false;
        //    }
        //}
        //#endregion

        //#region Xóa ghế
        //public bool DeleteSeat(int seatId, out string message)
        //{
        //    message = string.Empty;

        //    if (seatId <= 0)
        //    {
        //        message = "ID ghế không hợp lệ";
        //        return false;
        //    }

        //    // 1. [LOGIC NGHIỆP VỤ] Kiểm tra ghế có đang được sử dụng không (Giả định DAO có hàm kiểm tra)
        //    // if (_seatDAO.IsSeatCurrentlyInUse(seatId))
        //    // {
        //    //     message = "Không thể xóa: Ghế đang được đặt trên chuyến bay.";
        //    //     return false;
        //    // }

        //    try
        //    {
        //        bool result = _seatDAO.DeleteSeat(seatId);
        //        message = result ? "Xóa ghế thành công" : "Không thể xóa ghế (Có thể ID không tồn tại)";
        //        return result;
        //    }
        //    catch (MySqlConnector.MySqlException ex) when (ex.Number == 1451)
        //    {
        //        // Xử lý lỗi ràng buộc khóa ngoại (ví dụ: ghế đang có vé liên quan trong bảng flight_seats)
        //        message = "Không thể xóa ghế vì ghế này đang được liên kết với một hoặc nhiều chuyến bay (Flight Seats).";
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        message = $"Lỗi khi xóa ghế: {ex.Message}";
        //        return false;
        //    }
        //}
        //#endregion
        // File: BUS.Seat.SeatBUS

        // ... (Các phương thức GetAll/Filter/Search/AddSeat giữ nguyên) ...

        #region Sửa thông tin ghế
        public bool UpdateSeat(SeatDTO seat, out string message)
        {
            message = string.Empty;

            // 1. Kiểm tra validation 
            if (!seat.IsValid(out string error))
            {
                message = error;
                return false;
            }

            // 2. [LOGIC NGHIỆP VỤ] Kiểm tra ghế có đang được đặt không
            if (_seatDAO.IsSeatCurrentlyInUse(seat.SeatId))
            {
                message = "Không thể cập nhật thông tin ghế: Ghế đang được đặt hoặc bị chặn trên một chuyến bay.";
                return false;
            }

            try
            {
                // Gọi DAO (Cập nhật SeatNumber, AircraftId, ClassId)
                bool result = _seatDAO.UpdateSeat(seat);
                message = result ? "Cập nhật ghế thành công" : "Không thể cập nhật ghế (Có thể ID không tồn tại)";
                return result;
            }
            catch (Exception ex)
            {
                message = $"Lỗi khi cập nhật ghế: {ex.Message}";
                return false;
            }
        }
        #endregion

        #region Xóa ghế
        public bool DeleteSeat(int seatId, out string message)
        {
            message = string.Empty;

            if (seatId <= 0)
            {
                message = "ID ghế không hợp lệ";
                return false;
            }

            // 1. [LOGIC NGHIỆP VỤ] Kiểm tra ghế có đang được đặt không 
            if (_seatDAO.IsSeatCurrentlyInUse(seatId))
            {
                message = "Không thể xóa: Ghế đang được đặt hoặc bị chặn trên một chuyến bay.";
                return false;
            }

            try
            {
                bool result = _seatDAO.DeleteSeat(seatId);
                message = result ? "Xóa ghế thành công" : "Không thể xóa ghế (Có thể ID không tồn tại)";
                return result;
            }
            catch (MySqlConnector.MySqlException ex) when (ex.Number == 1451)
            {
                // Mã lỗi 1451 (Cannot delete or update a parent row: a foreign key constraint fails)
                message = "Không thể xóa ghế vì ghế này đang được liên kết với dữ liệu chuyến bay (Flight Seats).";
                return false;
            }
            catch (Exception ex)
            {
                message = $"Lỗi khi xóa ghế: {ex.Message}";
                return false;
            }
        }
        #endregion
    }
}