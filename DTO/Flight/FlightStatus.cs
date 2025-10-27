using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace DTO.Flight
{
    public enum FlightStatus
    {
        [Description ("Đã lên lịch")]
        SCHEDULED,
        [Description ("Bị hoãn")]
        DELAYED,
        [Description ("Đã hủy")]
        CANCELLED,
        [Description ("Hoàn thành")]
        COMPLETED
    }
    public static class FlightStatusExtensions
    {
        //Đây là hàm mở rộng cho enum FlightStatus để lấy mô tả của từng trạng thái, bởi vi enum không hỗ trợ viết hàm trực tiếp.
        public static string GetDescription(this FlightStatus status)
        {
            var field = status.GetType().GetField(status.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field,typeof(DescriptionAttribute));
            return attribute?.Description ?? status.ToString();
        }

        public static FlightStatus Parse(string statusString)
        {
            if(Enum.TryParse<FlightStatus>(statusString, true, out var result))
            {
                return result;
            }
            throw new ArgumentException($"Invalid FlightStatus string: {statusString}");
        }
        //Có thể chuyển trạng thái chuyến bay sang trạng thái khác không?
        public static bool CanTransitionTo(this FlightStatus current, FlightStatus target)
        {
            switch (current)
            {
                case FlightStatus.SCHEDULED:
                    return target != FlightStatus.SCHEDULED;

                case FlightStatus.DELAYED:
                    return target == FlightStatus.SCHEDULED ||
                           target == FlightStatus.CANCELLED ||
                           target == FlightStatus.COMPLETED;

                case FlightStatus.CANCELLED:
                    return false;

                case FlightStatus.COMPLETED:
                    return false;

                default:
                    return false;
            }
        }



    }
}
