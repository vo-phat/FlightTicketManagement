using BUS.Common;
using DAO.Flight;
using DTO.Flight;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUS.Flight
{
    /// <summary>
    /// Business Logic Layer cho Flight
    /// Xử lý các nghiệp vụ liên quan đến chuyến bay
    /// </summary>
    public class FlightBUS
    {
        #region Singleton Pattern

        private static FlightBUS _instance;
        private static readonly object _lock = new object();

        private FlightBUS() { }

        public static FlightBUS Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FlightBUS();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Business Rules Constants

        // Các rule nghiệp vụ có thể config
        private const int MIN_BOOKING_HOURS_BEFORE_DEPARTURE = 2; // Đặt trước ít nhất 2 giờ
        private const int MAX_FLIGHT_DURATION_HOURS = 24; // Chuyến bay tối đa 24 giờ
        private const int MIN_FLIGHT_DURATION_MINUTES = 30; // Chuyến bay tối thiểu 30 phút

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Lấy tất cả chuyến bay
        /// </summary>
        public BusinessResult GetAllFlights()
        {
            try
            {
                var flights = FlightDAO.Instance.GetAll();
                return BusinessResult.SuccessResult("Lấy danh sách chuyến bay thành công", flights);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Lấy chuyến bay theo ID
        /// </summary>
        public BusinessResult GetFlightById(int flightId)
        {
            try
            {
                if (flightId <= 0)
                    return BusinessResult.FailureResult("ID chuyến bay không hợp lệ");

                var flight = FlightDAO.Instance.GetById(flightId);

                if (flight == null)
                    return BusinessResult.FailureResult($"Không tìm thấy chuyến bay ID {flightId}");

                return BusinessResult.SuccessResult("Lấy thông tin chuyến bay thành công", flight);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Tạo chuyến bay mới
        /// </summary>
        public BusinessResult CreateFlight(FlightDTO flight)
        {
            try
            {
                // 1. Validate cơ bản (DTO validation)
                if (!flight.IsValid(out string validationError))
                {
                    return BusinessResult.FailureResult(validationError, "VALIDATION_ERROR");
                }

                // 2. Business rules validation
                var businessValidation = ValidateFlightBusinessRules(flight, isNewFlight: true);
                if (!businessValidation.Success)
                {
                    return businessValidation;
                }

                // 3. Kiểm tra trùng số hiệu chuyến bay
                if (FlightDAO.Instance.IsFlightNumberExists(
                    flight.FlightNumber,
                    flight.DepartureTime.Value))
                {
                    return BusinessResult.FailureResult(
                        $"Số hiệu chuyến bay '{flight.FlightNumber}' đã tồn tại vào ngày {flight.DepartureTime:dd/MM/yyyy}",
                        "DUPLICATE_FLIGHT_NUMBER");
                }

                // 4. Thực hiện insert
                long newId = FlightDAO.Instance.Insert(flight);
                flight.FlightId = (int)newId;

                return BusinessResult.SuccessResult(
                    $"Tạo chuyến bay '{flight.FlightNumber}' thành công!",
                    flight);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Cập nhật chuyến bay
        /// </summary>
        public BusinessResult UpdateFlight(FlightDTO flight)
        {
            try
            {
                // 1. Validate cơ bản
                if (!flight.IsValid(out string validationError))
                {
                    return BusinessResult.FailureResult(validationError, "VALIDATION_ERROR");
                }

                // 2. Kiểm tra chuyến bay có tồn tại không
                var existingFlight = FlightDAO.Instance.GetById(flight.FlightId);
                if (existingFlight == null)
                {
                    return BusinessResult.FailureResult($"Không tìm thấy chuyến bay ID {flight.FlightId}");
                }

                // 3. Business rules validation
                var businessValidation = ValidateFlightBusinessRules(flight, isNewFlight: false);
                if (!businessValidation.Success)
                {
                    return businessValidation;
                }

                // 4. Kiểm tra có thể cập nhật không (dựa vào status)
                if (!CanModifyFlight(existingFlight.Status))
                {
                    return BusinessResult.FailureResult(
                        $"Không thể sửa chuyến bay đã {existingFlight.Status.GetDescription()}",
                        "CANNOT_MODIFY");
                }

                // 5.Kiểm tra chuyển đổi trạng thái (nếu status CŨ và MỚI khác nhau)
                if (existingFlight.Status != flight.Status)
                {
                    if (!existingFlight.Status.CanTransitionTo(flight.Status))
                    {
                        return BusinessResult.FailureResult(
                            $"Không thể chuyển trạng thái từ '{existingFlight.Status.GetDescription()}' sang '{flight.Status.GetDescription()}'",
                            "INVALID_STATUS_TRANSITION");
                    }
                }

                // 6. Kiểm tra trùng số hiệu (loại trừ chính nó)
                if (FlightDAO.Instance.IsFlightNumberExists(
                    flight.FlightNumber,
                    flight.DepartureTime.Value,
                    flight.FlightId))
                {
                    return BusinessResult.FailureResult(
                        $"Số hiệu chuyến bay '{flight.FlightNumber}' đã tồn tại vào ngày {flight.DepartureTime:dd/MM/yyyy}",
                        "DUPLICATE_FLIGHT_NUMBER");
                }

                // 7. Thực hiện update
                bool success = FlightDAO.Instance.Update(flight);

                if (success)
                    return BusinessResult.SuccessResult($"Cập nhật chuyến bay '{flight.FlightNumber}' thành công!", flight);
                else
                    return BusinessResult.FailureResult("Cập nhật thất bại (không có hàng nào được thay đổi trong DB)");
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Xóa chuyến bay
        /// </summary>
        public BusinessResult DeleteFlight(int flightId)
        {
            try
            {
                // 1. Kiểm tra chuyến bay có tồn tại không
                var flight = FlightDAO.Instance.GetById(flightId);
                if (flight == null)
                {
                    return BusinessResult.FailureResult($"Không tìm thấy chuyến bay ID {flightId}");
                }

                // 2. Kiểm tra có thể xóa không
                if (!CanDeleteFlight(flight.Status))
                {
                    return BusinessResult.FailureResult(
                        $"Không thể xóa chuyến bay đã {flight.Status.GetDescription()}",
                        "CANNOT_DELETE");
                }

                // 3. Thực hiện xóa
                bool success = FlightDAO.Instance.Delete(flightId);

                if (success)
                    return BusinessResult.SuccessResult($"Xóa chuyến bay '{flight.FlightNumber}' thành công!");
                else
                    return BusinessResult.FailureResult("Xóa thất bại");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi foreign key constraint
                if (ex.Message.Contains("dữ liệu liên quan"))
                {
                    return BusinessResult.FailureResult(
                        "Không thể xóa chuyến bay vì đã có vé hoặc ghế ngồi được đặt",
                        "HAS_RELATED_DATA");
                }
                return BusinessResult.ExceptionResult(ex);
            }
        }

        #endregion

        #region Search & Filter
        public BusinessResult SuggestNextFlightNumber(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return BusinessResult.FailureResult("Cần có tiền tố (prefix) để gợi ý.");
            }

            try
            {
                int lastNumber = FlightDAO.Instance.GetLastFlightNumberNumeric(prefix);

                int nextNumber = lastNumber + 1;

                // Nếu bạn muốn padding (VD: VN001, VN124), dùng:
                 string nextFlightNumber = $"{prefix.ToUpper()}{nextNumber:D3}"; 

                // Format không padding (VD: VN1, VN124)
                //string nextFlightNumber = $"{prefix.ToUpper()}{nextNumber}";

                return BusinessResult.SuccessResult("Gợi ý số hiệu chuyến bay tiếp theo", nextFlightNumber);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }
        public BusinessResult SearchFlightsByNumber(string flightNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(flightNumber))
                    return BusinessResult.FailureResult("Vui lòng nhập số hiệu chuyến bay");

                var flights = FlightDAO.Instance.SearchByFlightNumber(flightNumber);
                return BusinessResult.SuccessResult($"Tìm thấy {flights.Count} chuyến bay", flights);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Lấy chuyến bay theo trạng thái
        /// </summary>
        public BusinessResult GetFlightsByStatus(FlightStatus status)
        {
            try
            {
                var flights = FlightDAO.Instance.GetByStatus(status);
                return BusinessResult.SuccessResult($"Tìm thấy {flights.Count} chuyến bay {status.GetDescription()}", flights);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }
        public BusinessResult GetFlightsByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                if (toDate < fromDate)
                    return BusinessResult.FailureResult("Ngày kết thúc phải sau ngày bắt đầu");

                var flights = FlightDAO.Instance.GetByDateRange(fromDate, toDate);
                return BusinessResult.SuccessResult($"Tìm thấy {flights.Count} chuyến bay", flights);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }
        public BusinessResult SearchFlightsForDisplay(
            string? flightNumber,
            int? departureAirportId,
            int? arrivalAirportId,
            DateTime? departureDate,
            int? cabinClassId)
        {
            try
            {
                // 1. Validate inputs

                // Bỏ kiểm tra (departureDate == null) vì giờ đây nó được phép null

                if (departureAirportId.HasValue && arrivalAirportId.HasValue && departureAirportId == arrivalAirportId)
                    return BusinessResult.FailureResult("Nơi cất cánh và hạ cánh không được trùng nhau");

                if (arrivalAirportId.HasValue && !departureAirportId.HasValue)
                    return BusinessResult.FailureResult("Vui lòng chọn nơi cất cánh trước khi chọn nơi hạ cánh");

                // 2. Call DAO (truyền thêm flightNumber và departureDate nullable)
                var flights = FlightDAO.Instance.SearchFlightsForDisplay(
                    flightNumber,
                    departureAirportId,
                    arrivalAirportId,
                    departureDate,
                    cabinClassId
                );

                return BusinessResult.SuccessResult($"Tìm thấy {flights.Rows.Count} chuyến bay", flights);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        #endregion

        #region Status Management

        /// <summary>
        /// Cập nhật trạng thái chuyến bay
        /// </summary>
        public BusinessResult UpdateFlightStatus(int flightId, FlightStatus newStatus)
        {
            try
            {
                // 1. Lấy thông tin chuyến bay hiện tại
                var flight = FlightDAO.Instance.GetById(flightId);
                if (flight == null)
                {
                    return BusinessResult.FailureResult($"Không tìm thấy chuyến bay ID {flightId}");
                }

                // 2. Kiểm tra có thể chuyển trạng thái không
                if (!flight.Status.CanTransitionTo(newStatus))
                {
                    return BusinessResult.FailureResult(
                        $"Không thể chuyển từ trạng thái '{flight.Status.GetDescription()}' sang '{newStatus.GetDescription()}'",
                        "INVALID_STATUS_TRANSITION");
                }

                // 3. Thực hiện cập nhật
                bool success = FlightDAO.Instance.UpdateStatus(flightId, newStatus);

                if (success)
                {
                    return BusinessResult.SuccessResult(
                        $"Đã chuyển trạng thái chuyến bay '{flight.FlightNumber}' sang '{newStatus.GetDescription()}'");
                }
                else
                {
                    return BusinessResult.FailureResult("Cập nhật trạng thái thất bại");
                }
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Delay chuyến bay
        /// </summary>
        public BusinessResult DelayFlight(int flightId, DateTime newDepartureTime, DateTime newArrivalTime, string reason)
        {
            try
            {
                var flight = FlightDAO.Instance.GetById(flightId);
                if (flight == null)
                {
                    return BusinessResult.FailureResult($"Không tìm thấy chuyến bay ID {flightId}");
                }

                // Cập nhật thông tin
                flight.DepartureTime = newDepartureTime;
                flight.ArrivalTime = newArrivalTime;
                flight.Status = FlightStatus.DELAYED;

                bool success = FlightDAO.Instance.Update(flight);

                if (success)
                {
                    return BusinessResult.SuccessResult(
                        $"Chuyến bay '{flight.FlightNumber}' đã được chuyển sang trạng thái DELAYED\n" +
                        $"Lý do: {reason}");
                }
                else
                {
                    return BusinessResult.FailureResult("Cập nhật thất bại");
                }
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Hủy chuyến bay
        /// </summary>
        public BusinessResult CancelFlight(int flightId, string reason)
        {
            try
            {
                var result = UpdateFlightStatus(flightId, FlightStatus.CANCELLED);

                if (result.Success)
                {
                    result.Message += $"\nLý do hủy: {reason}";
                }

                return result;
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        #endregion

        #region Business Rules Validation

        /// <summary>
        /// Validate các business rules cho chuyến bay
        /// </summary>
        private BusinessResult ValidateFlightBusinessRules(FlightDTO flight, bool isNewFlight)
        {
            var errors = new List<string>();

            // Rule 1: Thời gian khởi hành phải trong tương lai (nếu là chuyến bay mới)
            if (isNewFlight && flight.DepartureTime.HasValue)
            {
                var minDepartureTime = DateTime.Now.AddHours(MIN_BOOKING_HOURS_BEFORE_DEPARTURE);
                if (flight.DepartureTime.Value < minDepartureTime)
                {
                    errors.Add($"Chuyến bay phải được tạo trước thời gian khởi hành ít nhất {MIN_BOOKING_HOURS_BEFORE_DEPARTURE} giờ");
                }
            }

            // Rule 2: Thời gian bay phải hợp lý
            if (flight.DepartureTime.HasValue && flight.ArrivalTime.HasValue)
            {
                var duration = flight.GetFlightDuration();
                if (duration.HasValue)
                {
                    if (duration.Value.TotalMinutes < MIN_FLIGHT_DURATION_MINUTES)
                    {
                        errors.Add($"Thời gian bay tối thiểu là {MIN_FLIGHT_DURATION_MINUTES} phút");
                    }

                    if (duration.Value.TotalHours > MAX_FLIGHT_DURATION_HOURS)
                    {
                        errors.Add($"Thời gian bay tối đa là {MAX_FLIGHT_DURATION_HOURS} giờ");
                    }
                }
            }

            // Trả về kết quả
            if (errors.Count > 0)
            {
                return BusinessResult.ValidationError(errors);
            }

            return BusinessResult.SuccessResult();
        }

        /// <summary>
        /// Kiểm tra có thể sửa chuyến bay không
        /// </summary>
        private bool CanModifyFlight(FlightStatus status)
        {
            // Chỉ có thể sửa chuyến bay SCHEDULED hoặc DELAYED
            return status == FlightStatus.SCHEDULED || status == FlightStatus.DELAYED;
        }

        /// <summary>
        /// Kiểm tra có thể xóa chuyến bay không
        /// </summary>
        private bool CanDeleteFlight(FlightStatus status)
        {
            // Chỉ có thể xóa chuyến bay SCHEDULED (chưa có ai đặt)
            return status == FlightStatus.SCHEDULED;
        }

        #endregion

        #region Statistics

        /// <summary>
        /// Thống kê số lượng chuyến bay theo trạng thái
        /// </summary>
        public BusinessResult GetFlightStatistics()
        {
            try
            {
                var stats = new
                {
                    Total = FlightDAO.Instance.CountAll(),
                    Scheduled = FlightDAO.Instance.CountByStatus(FlightStatus.SCHEDULED),
                    Delayed = FlightDAO.Instance.CountByStatus(FlightStatus.DELAYED),
                    Cancelled = FlightDAO.Instance.CountByStatus(FlightStatus.CANCELLED),
                    Completed = FlightDAO.Instance.CountByStatus(FlightStatus.COMPLETED)
                };

                return BusinessResult.SuccessResult("Lấy thống kê thành công", stats);
            }
            catch (Exception ex)
            {
                return BusinessResult.ExceptionResult(ex);
            }
        }

        #endregion

       
    }
}