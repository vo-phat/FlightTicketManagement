using System;
using System.Text.RegularExpressions;
using DTO.Flight;

namespace BUS.Flight
{
    /// <summary>
    /// Helper class for validating flight business rules
    /// </summary>
    public static class FlightValidationHelper
    {
        private const decimal MIN_RECOMMENDED_PRICE = 500000; // 500k VNĐ
        private const int MIN_FUTURE_MINUTES = 30; // Chuyến bay phải tạo trước ít nhất 30 phút
        
        // Regex: 2 chữ cái in hoa + 3-4 số
        private static readonly Regex FlightCodeRegex = new Regex(@"^[A-Z]{2}\d{3,4}$", RegexOptions.Compiled);

        #region Flight Code Validation

        /// <summary>
        /// Validate flight code format (VD: VN123, VJ4567)
        /// </summary>
        public static bool IsValidFlightCode(string flightCode, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(flightCode))
            {
                errorMessage = "Mã chuyến bay không được để trống";
                return false;
            }

            flightCode = flightCode.Trim().ToUpper();

            if (!FlightCodeRegex.IsMatch(flightCode))
            {
                errorMessage = "Mã chuyến bay không hợp lệ. Định dạng: 2 chữ cái + 3-4 số (VD: VN123, VJ4567)";
                return false;
            }

            return true;
        }

        #endregion

        #region Time Validation

        /// <summary>
        /// Validate departure time is in the future (at least MIN_FUTURE_MINUTES from now)
        /// </summary>
        public static bool IsValidDepartureTime(DateTime departureTime, out string errorMessage)
        {
            errorMessage = string.Empty;

            DateTime now = DateTime.Now;
            TimeSpan timeDiff = departureTime - now;

            if (timeDiff.TotalMinutes < 0)
            {
                errorMessage = "Không thể tạo chuyến bay trong quá khứ";
                return false;
            }

            if (timeDiff.TotalMinutes < MIN_FUTURE_MINUTES)
            {
                errorMessage = $"Thời gian khởi hành phải sau thời điểm hiện tại ít nhất {MIN_FUTURE_MINUTES} phút";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate arrival time is after departure time
        /// </summary>
        public static bool IsValidArrivalTime(DateTime departureTime, DateTime arrivalTime, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (arrivalTime <= departureTime)
            {
                errorMessage = "Thời gian đến phải sau thời gian khởi hành";
                return false;
            }

            TimeSpan duration = arrivalTime - departureTime;
            
            // Kiểm tra chuyến bay không quá dài (VD: >24 giờ có thể là lỗi nhập liệu)
            if (duration.TotalHours > 24)
            {
                errorMessage = "Thời lượng chuyến bay không hợp lý (>24 giờ). Vui lòng kiểm tra lại";
                return false;
            }

            return true;
        }

        #endregion

        #region Price Validation

        /// <summary>
        /// Validate base price and return warning if too low
        /// </summary>
        public static bool IsValidBasePrice(decimal basePrice, out string errorMessage, out string? warningMessage)
        {
            errorMessage = string.Empty;
            warningMessage = null;

            if (basePrice < 0)
            {
                errorMessage = "Giá vé không thể âm";
                return false;
            }

            if (basePrice == 0)
            {
                errorMessage = "Giá vé phải lớn hơn 0";
                return false;
            }

            // Warning nếu giá quá thấp
            if (basePrice < MIN_RECOMMENDED_PRICE)
            {
                warningMessage = $"⚠️ Giá quá thấp so với thị trường (< {MIN_RECOMMENDED_PRICE:N0} VNĐ)";
            }

            return true;
        }

        #endregion

        #region Route Validation

        /// <summary>
        /// Validate departure and arrival airports are different
        /// </summary>
        public static bool AreAirportsDifferent(int departureAirportId, int arrivalAirportId, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (departureAirportId == arrivalAirportId)
            {
                errorMessage = "Sân bay đi và sân bay đến không được trùng nhau";
                return false;
            }

            return true;
        }

        #endregion

        #region Flight Duration

        /// <summary>
        /// Calculate and format flight duration
        /// </summary>
        public static string FormatFlightDuration(DateTime departureTime, DateTime arrivalTime)
        {
            TimeSpan duration = arrivalTime - departureTime;
            
            int hours = (int)duration.TotalHours;
            int minutes = duration.Minutes;

            return $"{hours} giờ {minutes} phút";
        }

        /// <summary>
        /// Calculate flight duration in minutes
        /// </summary>
        public static int CalculateDurationMinutes(DateTime departureTime, DateTime arrivalTime)
        {
            TimeSpan duration = arrivalTime - departureTime;
            return (int)duration.TotalMinutes;
        }

        #endregion

        #region Conflict Checking (will be implemented in BUS layer with DAO access)

        /// <summary>
        /// Check if aircraft is available during the flight time
        /// This will be implemented in FlightBUS with database access
        /// </summary>
        public static bool IsAircraftAvailable(int aircraftId, DateTime departureTime, DateTime arrivalTime)
        {
            // This method signature is for documentation
            // Actual implementation will be in FlightBUS.CheckAircraftConflict()
            throw new NotImplementedException("This method should be called from FlightBUS layer");
        }

        #endregion

        #region Complete Validation

        /// <summary>
        /// Perform complete validation for creating a flight
        /// Returns list of errors and warnings
        /// </summary>
        public static ValidationResult ValidateFlightForCreation(FlightDTO flight, int departureAirportId, int arrivalAirportId)
        {
            var result = new ValidationResult();

            // 1. Validate flight code format
            if (!IsValidFlightCode(flight.FlightNumber, out string codeError))
            {
                result.Errors.Add(codeError);
            }

            // 2. Validate airports
            if (!AreAirportsDifferent(departureAirportId, arrivalAirportId, out string airportError))
            {
                result.Errors.Add(airportError);
            }

            // 3. Validate times
            if (flight.DepartureTime.HasValue)
            {
                if (!IsValidDepartureTime(flight.DepartureTime.Value, out string depError))
                {
                    result.Errors.Add(depError);
                }

                if (flight.ArrivalTime.HasValue)
                {
                    if (!IsValidArrivalTime(flight.DepartureTime.Value, flight.ArrivalTime.Value, out string arrError))
                    {
                        result.Errors.Add(arrError);
                    }
                }
            }

            // 4. Validate price
            if (!IsValidBasePrice(flight.BasePrice, out string priceError, out string? priceWarning))
            {
                result.Errors.Add(priceError);
            }
            else if (!string.IsNullOrEmpty(priceWarning))
            {
                result.Warnings.Add(priceWarning);
            }

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Validation result containing errors and warnings
    /// </summary>
    public class ValidationResult
    {
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();

        public bool IsValid => Errors.Count == 0;
        public bool HasWarnings => Warnings.Count > 0;

        public string GetErrorMessage()
        {
            return string.Join("\n", Errors);
        }

        public string GetWarningMessage()
        {
            return string.Join("\n", Warnings);
        }
    }
}
