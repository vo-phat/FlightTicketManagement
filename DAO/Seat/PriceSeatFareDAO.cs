using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Seat
{
    public class PriceSeatFareDAO
    {
        /// <summary>
        /// Lấy giá fare theo flight_id và thời điểm hiện tại
        /// - Không có rule -> trả về 0
        /// - Có nhiều rule -> ưu tiên PEAK > NORMAL > OFFPEAK
        /// </summary>
        public decimal GetFarePriceByFlight(int flightId, DateTime now)
        {
            using var conn = DbConnection.GetConnection();
            conn.Open();

            string sql = @"
                SELECT 
                    IFNULL(r.price, 0) AS fare_price
                FROM flights f
                LEFT JOIN fare_rules r
                    ON f.route_id = r.route_id
                   AND @now BETWEEN r.effective_date AND r.expiry_date
                WHERE f.flight_id = @flightId
                ORDER BY
                    CASE r.season
                        WHEN 'PEAK' THEN 1
                        WHEN 'NORMAL' THEN 2
                        WHEN 'OFFPEAK' THEN 3
                        ELSE 4
                    END
                LIMIT 1;
            ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@flightId", flightId);
            cmd.Parameters.AddWithValue("@now", now);

            var result = cmd.ExecuteScalar();

            return result == null || result == DBNull.Value
                ? 0
                : Convert.ToDecimal(result);
        }
    }
}
