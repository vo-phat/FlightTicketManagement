using DAO.Database;
using DTO.Fare_Rule;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace DAO.Fare_Rule
{
    public class FareRuleDAO : BaseDAO
    {
        public List<FareRuleDTO> GetAll()
        {
            var list = new List<FareRuleDTO>();

            string query = @"
                SELECT 
                    fr.rule_id,
                    fr.route_id,
                    fr.class_id,
                    CONCAT(dep.airport_code, ' → ', arr.airport_code) AS RouteName,
                    cc.class_name AS CabinClass,
                    fr.fare_type,
                    fr.season,
                    fr.effective_date,
                    fr.expiry_date,
                    fr.description,
                    fr.price
                FROM fare_rules fr
                JOIN routes r ON fr.route_id = r.route_id
                JOIN airports dep ON r.departure_place_id = dep.airport_id
                JOIN airports arr ON r.arrival_place_id = arr.airport_id
                JOIN cabin_classes cc ON fr.class_id = cc.class_id
                ORDER BY fr.rule_id;
            ";

            ExecuteReader(query, reader =>
            {
                list.Add(MapReaderToDTO(reader));
            });

            return list;
        }

        public FareRuleDTO GetById(int ruleId)
        {
            FareRuleDTO dto = null;
            string query = @"
                SELECT 
                    fr.rule_id,
                    fr.route_id,
                    fr.class_id,
                    CONCAT(dep.airport_code, ' → ', arr.airport_code) AS RouteName,
                    cc.class_name AS CabinClass,
                    fr.fare_type,
                    fr.season,
                    fr.effective_date,
                    fr.expiry_date,
                    fr.description,
                    fr.price
                FROM fare_rules fr
                JOIN routes r ON fr.route_id = r.route_id
                JOIN airports dep ON r.departure_place_id = dep.airport_id
                JOIN airports arr ON r.arrival_place_id = arr.airport_id
                JOIN cabin_classes cc ON fr.class_id = cc.class_id
                WHERE fr.rule_id = @ruleId
                LIMIT 1;
            ";


            var parameters = new Dictionary<string, object> { { "@ruleId", ruleId } };

            ExecuteReader(query, reader =>
            {
                dto = MapReaderToDTO(reader);
            }, parameters);

            return dto;
        }


        private FareRuleDTO MapReaderToDTO(MySqlDataReader reader)
        {
            int ruleId = GetInt32(reader, "rule_id");
            int routeId = GetInt32(reader, "route_id");
            int classId = GetInt32(reader, "class_id");
            string routeName = GetString(reader, "RouteName");
            string cabinClass = GetString(reader, "CabinClass");
            string fareType = GetString(reader, "fare_type");
            string season = GetString(reader, "season");

            DateTime effectiveDate = DateTime.MinValue;
            DateTime expiryDate = DateTime.MinValue;

            try
            {
                // Nếu MySQL trả về kiểu DATETIME thì dùng GetDateTime
                if (!reader.IsDBNull(reader.GetOrdinal("effective_date")))
                    effectiveDate = reader.GetDateTime("effective_date");

                if (!reader.IsDBNull(reader.GetOrdinal("expiry_date")))
                    expiryDate = reader.GetDateTime("expiry_date");
            }
            catch
            {
                // Nếu trả về chuỗi (do view hoặc CAST trong SQL) thì parse thủ công
                string effStr = reader["effective_date"]?.ToString();
                string expStr = reader["expiry_date"]?.ToString();

                DateTime.TryParseExact(effStr, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out effectiveDate);

                DateTime.TryParseExact(expStr, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out expiryDate);
            }

            string description = GetString(reader, "description");
            decimal price = GetDecimal(reader, "price") ?? 0m;

            return new FareRuleDTO(
                ruleId, routeId, classId,
                routeName, cabinClass,
                fareType, season,
                effectiveDate, expiryDate,
                description, price
            );
        }


        public bool Insert(FareRuleDTO dto)
        {
            string query = @"
                INSERT INTO fare_rules (route_id, class_id, fare_type, season, effective_date, expiry_date, description, price)
                VALUES (@route_id, @class_id, @fare_type, @season, @effective_date, @expiry_date, @description, @price);
            ";

            var parameters = new Dictionary<string, object>
            {
                {"@route_id", dto.RouteId},
                {"@class_id", dto.ClassId},
                {"@fare_type", dto.FareType},
                {"@season", dto.Season},
                {"@effective_date", dto.EffectiveDate},
                {"@expiry_date", dto.ExpiryDate},
                {"@description", dto.Description},
                {"@price", dto.Price}
            };

            return ExecuteNonQuery(query, parameters) > 0;
        }
        public bool Update(FareRuleDTO dto)
        {
            string query = @"
                UPDATE fare_rules
                SET
                    route_id = @route_id,
                    class_id = @class_id,
                    fare_type = @fare_type,
                    season = @season,
                    effective_date = @effective_date,
                    expiry_date = @expiry_date,
                    description = @description,
                    price = @price
                WHERE rule_id = @rule_id;
            ";

            var parameters = new Dictionary<string, object>
            {
                { "@route_id", dto.RouteId },
                { "@class_id", dto.ClassId },
                { "@fare_type", dto.FareType },
                { "@season", dto.Season },
                { "@effective_date", dto.EffectiveDate },
                { "@expiry_date", dto.ExpiryDate },
                { "@description", dto.Description },
                { "@price", dto.Price },
                { "@rule_id", dto.RuleId }
            };

            try
            {
                int rows = ExecuteNonQuery(query, parameters);
                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật fare_rule: " + ex.Message, ex);
            }
        }
        public bool Delete(int ruleId)
        {
            string query = "DELETE FROM fare_rules WHERE rule_id = @rule_id;";

            var parameters = new Dictionary<string, object>
            {
                { "@rule_id", ruleId }
            };

            try
            {
                // ExecuteNonQuery trả về số lượng hàng bị ảnh hưởng.
                return ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                // Ném ngoại lệ để BUS có thể bắt và xử lý.
                throw new Exception($"Lỗi khi xóa quy tắc vé #{ruleId}: " + ex.Message, ex);
            }
        }
    }
}
