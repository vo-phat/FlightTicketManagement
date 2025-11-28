using System;
using System.Collections.Generic;
using DAO.Route;
using DTO.Route;

namespace BUS.Route
{
    public class RouteBUS
    {
        private readonly RouteDAO routeDAO;

        public RouteBUS()
        {
            routeDAO = new RouteDAO();
        }

        #region Lấy danh sách tuyến bay
        public List<RouteDTO> GetAllRoutes()
        {
            try
            {
                return routeDAO.GetAllRoutes();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tuyến bay: " + ex.Message, ex);
            }
        }
        #endregion

        #region Thêm tuyến bay mới
        public bool AddRoute(RouteDTO route, out string message)
        {
            message = string.Empty;

            // Kiểm tra hợp lệ
            if (!route.IsValid(out string validationError))
            {
                message = validationError;
                return false;
            }

            try
            {
                bool result = routeDAO.InsertRoute(route);
                message = result ? "Thêm tuyến bay thành công" : "Không thể thêm tuyến bay";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi thêm tuyến bay: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Cập nhật tuyến bay
        public bool UpdateRoute(RouteDTO route, out string message)
        {
            message = string.Empty;

            if (!route.IsValid(out string validationError))
            {
                message = validationError;
                return false;
            }

            try
            {
                bool result = routeDAO.UpdateRoute(route);
                message = result ? "Cập nhật tuyến bay thành công" : "Không thể cập nhật tuyến bay";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi cập nhật tuyến bay: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Xóa tuyến bay
        public bool DeleteRoute(int routeId, out string message)
        {
            message = string.Empty;

            if (routeId <= 0)
            {
                message = "ID tuyến bay không hợp lệ";
                return false;
            }

            try
            {
                bool result = routeDAO.DeleteRoute(routeId);
                message = result ? "Xóa tuyến bay thành công" : "Không thể xóa tuyến bay";
                return result;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi xóa tuyến bay: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Tìm kiếm tuyến bay
        public List<RouteDTO> SearchRoutes(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return routeDAO.GetAllRoutes();

                return routeDAO.SearchRoutes(keyword.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm tuyến bay: " + ex.Message, ex);
            }
        }
        public Dictionary<int, string> GetRouteDisplayList()
        {
            return routeDAO.GetRouteDisplayList();
        }

        public RouteDTO GetRouteById(int routeId)
        {
            try
            {
                return routeDAO.GetRouteById(routeId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy tuyến bay ID {routeId}: {ex.Message}", ex);
            }
        }
        #endregion
    }
}
