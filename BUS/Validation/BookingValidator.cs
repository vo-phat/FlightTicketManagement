using System;
using System.Text.RegularExpressions;
using DTO.Ticket;

namespace BUS.Validation
{
    public static class BookingValidator
    {
        /// <summary>
        /// Validate toàn bộ thông tin đặt vé
        /// </summary>
        public static void ValidateBookingRequest(TicketBookingRequestDTO request)
        {
            // 1. FlightId và ClassId bắt buộc
            ValidateRequiredFields(request);

            // 2. Ngày bay không được trong quá khứ
            ValidateFlightDate(request.FlightDate);

            // 3. Cảnh báo nếu đặt vé < 24h hoặc > 365 ngày
            CheckBookingTimeWarning(request.FlightDate, out string warning);

            // 4. Phải chọn ghế (SeatId hoặc SeatNumber)
            ValidateSeatSelection(request);

            // 5. Validate format số ghế (VD: 12A, 5F)
            if (!string.IsNullOrWhiteSpace(request.SeatNumber))
            {
                ValidateSeatNumberFormat(request.SeatNumber);
            }

            // 6. Kiểm tra ghế còn trống (kết nối DB)
            ValidateSeatAvailability(request.FlightId, request.SeatId, request.SeatNumber);
        }

        /// <summary>
        /// Kiểm tra FlightId và ClassId bắt buộc
        /// </summary>
        private static void ValidateRequiredFields(TicketBookingRequestDTO request)
        {
            if (!request.FlightId.HasValue || request.FlightId.Value <= 0)
                throw new ArgumentException("FlightId là bắt buộc và phải lớn hơn 0.");

            if (!request.ClassId.HasValue || request.ClassId.Value <= 0)
                throw new ArgumentException("ClassId (hạng ghế) là bắt buộc và phải lớn hơn 0.");
        }

        /// <summary>
        /// Kiểm tra ngày bay không được trong quá khứ
        /// </summary>
        private static void ValidateFlightDate(DateTime? flightDate)
        {
            if (!flightDate.HasValue)
                throw new ArgumentException("Ngày bay là bắt buộc.");

            DateTime today = DateTime.Today;

            if (flightDate.Value.Date < today)
                throw new ArgumentException("Ngày bay không được trong quá khứ.");
        }

        /// <summary>
        /// Cảnh báo nếu đặt vé < 24h hoặc > 365 ngày
        /// Trả về warning message nếu có
        /// </summary>
        public static string CheckBookingTimeWarning(DateTime? flightDate, out string warning)
        {
            warning = string.Empty;

            if (!flightDate.HasValue)
                return warning;

            DateTime now = DateTime.Now;
            TimeSpan timeUntilFlight = flightDate.Value - now;

            // Đặt vé < 24h trước giờ bay
            if (timeUntilFlight.TotalHours < 24 && timeUntilFlight.TotalHours > 0)
            {
                warning = "⚠️ Cảnh báo: Bạn đang đặt vé trong vòng 24 giờ trước giờ bay. Vui lòng kiểm tra kỹ thời gian.";
            }
            // Đặt vé > 365 ngày trước
            else if (timeUntilFlight.TotalDays > 365)
            {
                warning = "⚠️ Cảnh báo: Bạn đang đặt vé quá xa (hơn 365 ngày). Vui lòng xác nhận lại ngày bay.";
            }

            return warning;
        }

        /// <summary>
        /// Phải chọn ghế (SeatId hoặc SeatNumber)
        /// </summary>
        private static void ValidateSeatSelection(TicketBookingRequestDTO request)
        {
            bool hasSeatId = request.SeatId.HasValue && request.SeatId.Value > 0;
            bool hasSeatNumber = !string.IsNullOrWhiteSpace(request.SeatNumber);

            if (!hasSeatId && !hasSeatNumber)
                throw new ArgumentException("Phải chọn ghế. Vui lòng cung cấp SeatId hoặc SeatNumber.");
        }

        /// <summary>
        /// Validate format số ghế (VD: 12A, 5F, 1A, 99Z)
        /// Format hợp lệ: 1-2 chữ số + 1 chữ cái (A-Z)
        /// </summary>
        private static void ValidateSeatNumberFormat(string seatNumber)
        {
            if (string.IsNullOrWhiteSpace(seatNumber))
                return;

            seatNumber = seatNumber.Trim().ToUpper();

            // Regex: 1-2 chữ số + 1 chữ cái A-Z
            // Ví dụ hợp lệ: 1A, 12A, 5F, 99Z
            if (!Regex.IsMatch(seatNumber, @"^[1-9][0-9]?[A-Z]$"))
                throw new ArgumentException($"Số ghế '{seatNumber}' không hợp lệ. Format đúng: 1-2 chữ số + 1 chữ cái (VD: 12A, 5F).");
        }

        /// <summary>
        /// Kiểm tra ghế còn trống (kết nối DB)
        /// </summary>
        /// <param name="flightId">Mã chuyến bay</param>
        /// <param name="seatId">Mã ghế (nếu có)</param>
        /// <param name="seatNumber">Số ghế (nếu có)</param>
        public static void ValidateSeatAvailability(int? flightId, int? seatId, string seatNumber)
        {
            if (!flightId.HasValue)
            {
                throw new ArgumentException("FlightId là bắt buộc để kiểm tra ghế.");
            }

            var flightSeatDAO = new DAO.FlightSeat.FlightSeatDAO();

            try
            {
                bool isAvailable = false;

                // Ưu tiên kiểm tra theo SeatId
                if (seatId.HasValue && seatId.Value > 0)
                {
                    isAvailable = flightSeatDAO.IsSeatAvailable(flightId.Value, seatId.Value);
                    
                    if (!isAvailable)
                    {
                        var seatInfo = flightSeatDAO.GetFlightSeatInfo(flightId.Value, seatId.Value);
                        string displaySeat = seatInfo?.SeatNumber ?? seatNumber ?? $"SeatId={seatId}";
                        throw new InvalidOperationException($"Ghế '{displaySeat}' đã được đặt hoặc không khả dụng. Vui lòng chọn ghế khác.");
                    }
                }
                // Nếu không có SeatId, kiểm tra theo SeatNumber
                else if (!string.IsNullOrWhiteSpace(seatNumber))
                {
                    isAvailable = flightSeatDAO.IsSeatAvailableBySeatNumber(flightId.Value, seatNumber);
                    
                    if (!isAvailable)
                    {
                        throw new InvalidOperationException($"Ghế '{seatNumber}' đã được đặt hoặc không khả dụng. Vui lòng chọn ghế khác.");
                    }
                }
                else
                {
                    throw new ArgumentException("Phải cung cấp SeatId hoặc SeatNumber để kiểm tra ghế.");
                }
            }
            catch (InvalidOperationException)
            {
                // Re-throw business logic exceptions
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra tình trạng ghế: {ex.Message}", ex);
            }
        }
    }
}
