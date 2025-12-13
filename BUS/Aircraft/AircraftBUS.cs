using System;
using System.Collections.Generic;
using DAO.Aircraft;
using DTO.Aircraft;

namespace BUS.Aircraft
{
    public class AircraftBUS
    {
        private readonly AircraftDAO aircraftDAO;

        public AircraftBUS()
        {
            aircraftDAO = new AircraftDAO();
        }

        #region Lấy danh sách máy bay
        public List<AircraftDTO> GetAllAircrafts()
        {
            try
            {
                return aircraftDAO.GetAllAircrafts();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách máy bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Thêm máy bay mới
        public int AddAircraft(AircraftDTO aircraft, out string message)
        {
            message = string.Empty;

            // Kiểm tra hợp lệ
            if (!aircraft.IsValid(out string validationError))
            {
                message = validationError;
                return 0;
            }

            try
            {
                int newAircraftId = aircraftDAO.InsertAircraft(aircraft);
                if (newAircraftId > 0)
                {
                    message = "Thêm máy bay thành công";
                    return newAircraftId;
                }
                else
                {
                    message = "Không thể thêm máy bay";
                    return 0;
                }
            }
            catch (Exception ex)
            {
                message = "Lỗi khi thêm máy bay: " + ex.Message;
                return 0;
            }
        }
        #endregion

        #region Cập nhật thông tin máy bay
        public bool UpdateAircraft(AircraftDTO aircraft, out string message)
        {
            message = string.Empty;

            // Kiểm tra hợp lệ
            if (!aircraft.IsValid(out string validationError))
            {
                message = validationError;
                return false;
            }

            try
            {
                bool result = aircraftDAO.UpdateAircraft(aircraft);
                message = result ? "Cập nhật máy bay thành công" : "Không thể cập nhật máy bay";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi cập nhật máy bay: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Xóa máy bay
        public bool DeleteAircraft(int aircraftId, out string message)
        {
            message = string.Empty;

            if (aircraftId <= 0)
            {
                message = "ID máy bay không hợp lệ";
                return false;
            }

            try
            {
                bool result = aircraftDAO.DeleteAircraft(aircraftId);
                message = result ? "Xóa máy bay thành công" : "Không thể xóa máy bay";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi xóa máy bay: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Tìm kiếm máy bay
        public List<AircraftDTO> SearchAircrafts(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return aircraftDAO.GetAllAircrafts();

                return aircraftDAO.SearchAircrafts(keyword.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm máy bay: " + ex.Message, ex);
            }
        }
        #endregion
    }
}
