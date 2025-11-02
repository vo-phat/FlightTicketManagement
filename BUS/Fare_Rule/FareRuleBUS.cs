using DAO.Fare_Rule;
using DTO.Fare_Rule;
using System; // Cần cho ArgumentException, KeyNotFoundException, v.v.
using System.Collections.Generic;

namespace BUS.Fare_Rule
{
    public class FareRuleBUS
    {
        private FareRuleDAO dao = new FareRuleDAO();

        /**
         * Phương thức private để kiểm tra tính hợp lệ của dữ liệu quy tắc vé.
         * Ném ra ArgumentException nếu dữ liệu không hợp lệ.
         * @param dto Dữ liệu quy tắc vé cần kiểm tra.
         * @param isNewRecord Đặt là true nếu đây là bản ghi mới (INSERT),
         * false nếu là bản ghi cũ (UPDATE).
         */
        private void ValidateFareRule(FareRuleDTO dto, bool isNewRecord = false)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Dữ liệu quy tắc vé (DTO) không được rỗng.");

            if (dto.RouteId <= 0)
                throw new ArgumentException("Mã tuyến bay (RouteId) không hợp lệ.", nameof(dto.RouteId));

            if (dto.ClassId <= 0)
                throw new ArgumentException("Mã hạng vé (ClassId) không hợp lệ.", nameof(dto.ClassId));

            if (string.IsNullOrWhiteSpace(dto.FareType))
                throw new ArgumentException("Loại vé (FareType) không được để trống.", nameof(dto.FareType));

            if (string.IsNullOrWhiteSpace(dto.Season))
                throw new ArgumentException("Mùa (Season) không được để trống.", nameof(dto.Season));

            // Giá vé có thể bằng 0 (ví dụ: vé 0 đồng) nhưng không được âm
            if (dto.Price < 0)
                throw new ArgumentException("Giá vé không thể là số âm.", nameof(dto.Price));

            // Chỉ kiểm tra ngày hiệu lực khi THÊM MỚI
            // So sánh .Date (chỉ ngày) với DateTime.Today (ngày hôm nay, lúc 00:00:00)
            if (isNewRecord && dto.EffectiveDate.Date < DateTime.Today)
            {
                throw new ArgumentException("Ngày hiệu lực không thể là một ngày trong quá khứ.", nameof(dto.EffectiveDate));
            }

            if (dto.ExpiryDate < dto.EffectiveDate)
                throw new ArgumentException("Ngày hết hạn không thể trước ngày hiệu lực.", nameof(dto.ExpiryDate));

            // Đảm bảo trường Description không bị null khi lưu xuống DB
            if (dto.Description == null)
            {
                dto.Description = string.Empty;
            }
        }

        /// <summary>
        /// Lấy tất cả các quy tắc vé.
        /// </summary>
        public List<FareRuleDTO> GetAll()
        {
            try
            {
                return dao.GetAll();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Không thể tải danh sách quy tắc vé từ cơ sở dữ liệu.", ex);
            }
        }

        /// <summary>
        /// Lấy một quy tắc vé theo ID.
        /// </summary>
        public FareRuleDTO GetById(int ruleId)
        {
            if (ruleId <= 0)
                throw new ArgumentException("Mã quy tắc không hợp lệ.");

            var dto = dao.GetById(ruleId);
            if (dto == null)
                throw new KeyNotFoundException($"Không tìm thấy quy tắc vé #{ruleId}.");

            return dto;
        }

        /// <summary>
        /// Thêm một quy tắc vé mới.
        /// </summary>
        public bool Insert(FareRuleDTO dto)
        {
            // 1. Validate dữ liệu đầu vào (đánh dấu là bản ghi MỚI)
            ValidateFareRule(dto, isNewRecord: true);

            // 2. Thực hiện Insert
            try
            {
                return dao.Insert(dto);
            }
            catch (Exception ex)
            {
                // Bắt các lỗi tiềm ẩn từ CSDL (ví dụ: trùng khóa,...)
                throw new ApplicationException("Lỗi khi thêm mới quy tắc vé vào cơ sở dữ liệu.", ex);
            }
        }

        /// <summary>
        /// Cập nhật một quy tắc vé hiện có.
        /// </summary>
        public bool Update(FareRuleDTO dto)
        {
            try
            {
                // 1. Validate dữ liệu chung (đánh dấu là bản ghi CŨ)
                ValidateFareRule(dto, isNewRecord: false);

                // 2. Validate dữ liệu riêng cho Update
                if (dto.RuleId <= 0)
                    throw new ArgumentException("Mã quy tắc (RuleId) không hợp lệ để cập nhật.");

                // 3. (Nên có) Kiểm tra sự tồn tại trước khi cập nhật
                if (dao.GetById(dto.RuleId) == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy quy tắc vé #{dto.RuleId} để cập nhật.");
                }

                // 4. Thực hiện Update
                return dao.Update(dto);
            }
            catch (Exception ex)
            {
                // Bắt các lỗi từ validation hoặc DAO
                throw new ApplicationException("Lỗi khi cập nhật quy tắc vé: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Xóa một quy tắc vé.
        /// </summary>
        public bool Delete(int ruleId)
        {
            if (ruleId <= 0)
                throw new ArgumentException("Mã quy tắc không hợp lệ để xóa.");

            try
            {
                // Kiểm tra xem quy tắc có tồn tại trước khi xóa
                if (dao.GetById(ruleId) == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy quy tắc vé #{ruleId} để xóa.");
                }

                return dao.Delete(ruleId);
            }
            catch (Exception ex)
            {
                // Bắt lỗi từ KeyNotFoundException hoặc lỗi CSDL (ví dụ: ràng buộc khóa ngoại)
                throw new ApplicationException("Lỗi trong quá trình xóa quy tắc vé.", ex);
            }
        }
    }
}