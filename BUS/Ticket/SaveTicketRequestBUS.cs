using System;
using System.Collections.Generic;
using System.Linq;
using DAO.TicketDAO;
using DTO.Ticket;

namespace BUS.Ticket
{
    public class SaveTicketRequestBUS
    {
        private readonly SaveTicketRequestDAO _dao;

        public SaveTicketRequestBUS()
        {
            _dao = new SaveTicketRequestDAO();
        }

        // ============================
        // 1) ONE WAY BOOKING
        // ============================
        public int SaveOneWay(List<TicketBookingRequestDTO> outbound, int accountId)
        {
            try
            {
                // ✅ Validation
                if (outbound == null || outbound.Count == 0)
                    throw new Exception("Danh sách hành khách rỗng.");

                if (accountId <= 0)
                    throw new Exception("Account ID không hợp lệ.");

                // ✅ Validation chi tiết từng hành khách
                ValidatePassengerList(outbound);

                // ✅ Gọi DAO để lưu
                int bookingId = _dao.CreateBookingOneWay(outbound, accountId);

                if (bookingId <= 0)
                    throw new Exception("Không thể tạo booking.");

                return bookingId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BUS - SaveOneWay: {ex.Message}", ex);
            }
        }

        // ============================
        // 2) ROUND TRIP BOOKING
        // ============================
        public int SaveRoundTrip(
            List<TicketBookingRequestDTO> outbound,
            List<TicketBookingRequestDTO> inbound,
            int accountId)
        {
            try
            {
                // ✅ Validation cơ bản
                if (outbound == null || inbound == null)
                    throw new Exception("Danh sách hành khách chiều đi hoặc chiều về bị NULL.");

                if (outbound.Count == 0)
                    throw new Exception("Danh sách hành khách chiều đi rỗng.");

                if (inbound.Count == 0)
                    throw new Exception("Danh sách hành khách chiều về rỗng.");

                if (outbound.Count != inbound.Count)
                    throw new Exception($"Số lượng hành khách không khớp: Chiều đi ({outbound.Count}) ≠ Chiều về ({inbound.Count}).");

                if (accountId <= 0)
                    throw new Exception("Account ID không hợp lệ.");

                // ✅ Validation chi tiết
                ValidatePassengerList(outbound, "chiều đi");
                ValidatePassengerList(inbound, "chiều về");

                // ✅ Kiểm tra thông tin cá nhân khớp nhau
                ValidateMatchingPassengers(outbound, inbound);

                // ✅ Gọi DAO để lưu
                int bookingId = _dao.CreateBookingRoundTrip(outbound, inbound, accountId);

                if (bookingId <= 0)
                    throw new Exception("Không thể tạo booking.");

                return bookingId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BUS - SaveRoundTrip: {ex.Message}", ex);
            }
        }

        // ============================
        // PRIVATE VALIDATION HELPERS
        // ============================

        /// <summary>
        /// Validate từng hành khách trong danh sách
        /// </summary>
        private void ValidatePassengerList(List<TicketBookingRequestDTO> passengers, string tripLabel = "")
        {
            string label = string.IsNullOrEmpty(tripLabel) ? "" : $" ({tripLabel})";

            for (int i = 0; i < passengers.Count; i++)
            {
                var p = passengers[i];
                int index = i + 1;

                // ✅ Thông tin cơ bản
                if (string.IsNullOrWhiteSpace(p.FullName))
                    throw new Exception($"Hành khách {index}{label}: Tên không được để trống.");

                if (string.IsNullOrWhiteSpace(p.PassportNumber))
                    throw new Exception($"Hành khách {index}{label}: Số hộ chiếu không được để trống.");

                if (!p.DateOfBirth.HasValue)
                    throw new Exception($"Hành khách {index}{label}: Ngày sinh không hợp lệ.");

                if (p.DateOfBirth.Value > DateTime.Now)
                    throw new Exception($"Hành khách {index}{label}: Ngày sinh không thể là tương lai.");

                // ✅ Ghế
                if (p.FlightSeatId == 0)
                    throw new Exception($"Hành khách {index}{label} ({p.FullName}): Chưa chọn ghế.");

                if (string.IsNullOrWhiteSpace(p.SeatNumber))
                    throw new Exception($"Hành khách {index}{label} ({p.FullName}): Số ghế không hợp lệ.");

                // ✅ Giá vé
                if (!p.TicketPrice.HasValue || p.TicketPrice <= 0)
                    throw new Exception($"Hành khách {index}{label} ({p.FullName}): Giá vé không hợp lệ ({p.TicketPrice}).");

                // ✅ Flight & Class
                if (p.FlightId == 0)
                    throw new Exception($"Hành khách {index}{label} ({p.FullName}): FlightId không hợp lệ.");

                if (p.ClassId == 0)
                    throw new Exception($"Hành khách {index}{label} ({p.FullName}): ClassId không hợp lệ.");
            }
        }

        /// <summary>
        /// Kiểm tra thông tin cá nhân của hành khách chiều đi/về có khớp nhau không
        /// </summary>
        private void ValidateMatchingPassengers(
            List<TicketBookingRequestDTO> outbound,
            List<TicketBookingRequestDTO> inbound)
        {
            for (int i = 0; i < outbound.Count; i++)
            {
                var ob = outbound[i];
                var ib = inbound[i];
                int index = i + 1;

                if (ob.FullName != ib.FullName)
                    throw new Exception($"Hành khách {index}: Tên không khớp (Đi: {ob.FullName}, Về: {ib.FullName}).");

                if (ob.PassportNumber != ib.PassportNumber)
                    throw new Exception($"Hành khách {index}: Hộ chiếu không khớp (Đi: {ob.PassportNumber}, Về: {ib.PassportNumber}).");

                if (ob.DateOfBirth != ib.DateOfBirth)
                    throw new Exception($"Hành khách {index}: Ngày sinh không khớp.");
            }
        }

        // ============================
        // HELPER - Tính tổng tiền (Optional)
        // ============================

        /// <summary>
        /// Tính tổng tiền của booking (dùng để hiển thị hoặc log)
        /// </summary>
        public decimal CalculateTotalAmount(List<TicketBookingRequestDTO> passengers)
        {
            if (passengers == null || passengers.Count == 0)
                return 0;

            return passengers.Sum(p => p.TicketPrice ?? 0);
        }

        /// <summary>
        /// Tính tổng tiền booking khứ hồi
        /// </summary>
        public decimal CalculateTotalAmountRoundTrip(
            List<TicketBookingRequestDTO> outbound,
            List<TicketBookingRequestDTO> inbound)
        {
            return CalculateTotalAmount(outbound) + CalculateTotalAmount(inbound);
        }
    }
}