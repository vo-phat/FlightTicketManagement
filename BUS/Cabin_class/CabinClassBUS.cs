using System;
using System.Collections.Generic;
using DAO.CabinClass;
using DTO.CabinClass;

namespace BUS.CabinClass
{
    public class CabinClassBUS
    {
        private readonly CabinClassDAO cabinClassDAO;

        public CabinClassBUS()
        {
            cabinClassDAO = new CabinClassDAO();
        }

        #region Lấy danh sách tất cả hạng ghế
        public List<CabinClassDTO> GetAllCabinClasses()
        {
            try
            {
                return cabinClassDAO.GetAllCabinClasses();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hạng ghế: " + ex.Message, ex);
            }
        }
        #endregion

        #region Thêm hạng ghế mới
        public bool AddCabinClass(CabinClassDTO cabinClass, out string message)
        {
            message = string.Empty;

            // Kiểm tra hợp lệ
            if (!cabinClass.IsValid(out string validationError))
            {
                message = validationError;
                return false;
            }

            try
            {
                bool result = cabinClassDAO.InsertCabinClass(cabinClass);
                message = result ? "Thêm hạng ghế thành công" : "Không thể thêm hạng ghế";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi thêm hạng ghế: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Cập nhật thông tin hạng ghế
        public bool UpdateCabinClass(CabinClassDTO cabinClass, out string message)
        {
            message = string.Empty;

            if (!cabinClass.IsValid(out string validationError))
            {
                message = validationError;
                return false;
            }

            try
            {
                bool result = cabinClassDAO.UpdateCabinClass(cabinClass);
                message = result ? "Cập nhật hạng ghế thành công" : "Không thể cập nhật hạng ghế";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi cập nhật hạng ghế: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Xóa hạng ghế
        public bool DeleteCabinClass(int classId, out string message)
        {
            message = string.Empty;

            if (classId <= 0)
            {
                message = "ID hạng ghế không hợp lệ";
                return false;
            }

            try
            {
                bool result = cabinClassDAO.DeleteCabinClass(classId);
                message = result ? "Xóa hạng ghế thành công" : "Không thể xóa hạng ghế";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi xóa hạng ghế: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Tìm kiếm hạng ghế
        public List<CabinClassDTO> SearchCabinClasses(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return cabinClassDAO.GetAllCabinClasses();

                return cabinClassDAO.SearchCabinClasses(keyword.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm hạng ghế: " + ex.Message, ex);
            }
        }
        #endregion
    }
}