using System;
using System.Collections.Generic;

namespace BUS.Common
{
    // Class đại diện cho kết quả của một business operation
 
    public class BusinessResult
    {
        #region Properties
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string ErrorCode { get; set; }
        public string TechnicalMessage { get; set; }
        public List<string> ValidationErrors { get; set; }

        #endregion

        #region Constructors
        public BusinessResult()
        {
            ValidationErrors = new List<string>();
        }
        public BusinessResult(bool success, string message)
        {
            Success = success;
            Message = message;
            ValidationErrors = new List<string>();
        }
        public BusinessResult(bool success, string message, object data)
        {
            Success = success;
            Message = message;
            Data = data;
            ValidationErrors = new List<string>();
        }

        #endregion

        #region Static Factory Methods
        public static BusinessResult SuccessResult(string message = "Thao tác thành công")
        {
            return new BusinessResult(true, message);
        }
        public static BusinessResult SuccessResult(string message, object data)
        {
            return new BusinessResult(true, message, data);
        }
        public static BusinessResult FailureResult(string message)
        {
            return new BusinessResult(false, message);
        }
        public static BusinessResult FailureResult(string message, string errorCode)
        {
            return new BusinessResult(false, message)
            {
                ErrorCode = errorCode
            };
        }
        public static BusinessResult ExceptionResult(Exception ex)
        {
            return new BusinessResult(false, "Đã xảy ra lỗi trong quá trình xử lý")
            {
                TechnicalMessage = ex.Message,
                ErrorCode = "EXCEPTION"
            };
        }
        public static BusinessResult ValidationError(List<string> errors)
        {
            return new BusinessResult(false, "Dữ liệu không hợp lệ")
            {
                ValidationErrors = errors,
                ErrorCode = "VALIDATION_ERROR"
            };
        }

        public static BusinessResult ValidationError(string error)
        {
            return new BusinessResult(false, error)
            {
                ValidationErrors = new List<string> { error },
                ErrorCode = "VALIDATION_ERROR"
            };
        }

        #endregion

        #region Helper Methods
        public void AddValidationError(string error)
        {
            ValidationErrors.Add(error);
        }

        public string GetFullErrorMessage()
        {
            if (Success)
                return Message;

            var fullMessage = Message;

            if (ValidationErrors.Count > 0)
            {
                fullMessage += "\n\nChi tiết lỗi:";
                foreach (var error in ValidationErrors)
                {
                    fullMessage += $"\n- {error}";
                }
            }

            return fullMessage;
        }
        public T GetData<T>()
        {
            if (Data == null)
                return default(T);

            return (T)Data;
        }

        #endregion
    }
}