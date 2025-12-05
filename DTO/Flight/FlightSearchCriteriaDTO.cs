using System;

namespace DTO.Flight
{
    /// <summary>
    /// DTO cho tiêu chí tìm kiếm chuyến bay
    /// </summary>
    public class FlightSearchCriteriaDTO
    {
        // Tìm theo sân bay
        public int? DepartureAirportId { get; set; }
        public int? ArrivalAirportId { get; set; }

        // Tìm theo ngày
        public DateTime? DepartureDate { get; set; }       // Ngày khởi hành cụ thể
        public DateTime? DepartureDateFrom { get; set; }    // Từ ngày
        public DateTime? DepartureDateTo { get; set; }      // Đến ngày

        // Tìm theo thông tin khác
        public string FlightNumber { get; set; }            // Số hiệu chuyến bay
        public FlightStatus? Status { get; set; }           // Trạng thái chuyến bay
        public int? AircraftId { get; set; }                // Máy bay cụ thể
        public int? RouteId { get; set; }                   // Tuyến bay cụ thể

        // Lọc theo hạng ghế và số ghế trống
        public int? ClassId { get; set; }                   // Hạng ghế (Economy, Business...)
        public int? MinAvailableSeats { get; set; }         // Số ghế trống tối thiểu

        // Sắp xếp
        public string SortBy { get; set; } = "DepartureTime";  // DepartureTime, Price, AvailableSeats
        public string SortOrder { get; set; } = "ASC";          // ASC, DESC

        // Phân trang
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public FlightSearchCriteriaDTO() { }

        /// <summary>
        /// Kiểm tra có ít nhất một tiêu chí tìm kiếm không
        /// </summary>
        public bool HasAnyCriteria()
        {
            return DepartureAirportId.HasValue ||
                   ArrivalAirportId.HasValue ||
                   DepartureDate.HasValue ||
                   DepartureDateFrom.HasValue ||
                   DepartureDateTo.HasValue ||
                   !string.IsNullOrWhiteSpace(FlightNumber) ||
                   Status.HasValue ||
                   AircraftId.HasValue ||
                   RouteId.HasValue ||
                   ClassId.HasValue ||
                   MinAvailableSeats.HasValue;
        }

        /// <summary>
        /// Validate các tiêu chí tìm kiếm
        /// </summary>
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            // Kiểm tra ngày
            if (DepartureDateFrom.HasValue && DepartureDateTo.HasValue)
            {
                if (DepartureDateFrom.Value > DepartureDateTo.Value)
                {
                    errorMessage = "Ngày bắt đầu phải trước ngày kết thúc.";
                    return false;
                }

                // Không cho phép tìm quá xa trong tương lai (ví dụ: 1 năm)
                if ((DepartureDateTo.Value - DateTime.Today).TotalDays > 365)
                {
                    errorMessage = "Không thể tìm kiếm chuyến bay quá 1 năm trong tương lai.";
                    return false;
                }
            }

            // Kiểm tra số ghế trống
            if (MinAvailableSeats.HasValue && MinAvailableSeats.Value < 0)
            {
                errorMessage = "Số ghế trống tối thiểu phải >= 0.";
                return false;
            }

            // Kiểm tra sort order
            if (!string.IsNullOrWhiteSpace(SortOrder))
            {
                SortOrder = SortOrder.ToUpper();
                if (SortOrder != "ASC" && SortOrder != "DESC")
                {
                    errorMessage = "SortOrder chỉ nhận giá trị ASC hoặc DESC.";
                    return false;
                }
            }

            // Kiểm tra phân trang
            if (PageNumber.HasValue && PageNumber.Value < 1)
            {
                errorMessage = "PageNumber phải >= 1.";
                return false;
            }

            if (PageSize.HasValue && (PageSize.Value < 1 || PageSize.Value > 100))
            {
                errorMessage = "PageSize phải từ 1 đến 100.";
                return false;
            }

            return true;
        }
    }
}
