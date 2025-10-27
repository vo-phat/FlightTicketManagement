//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using MySqlConnector;
//using DAO.Database;
//using DTO.Flight;

//namespace DAO.Flight
//{
//    public static class FlightDAO
//    {
//        // SQL lấy danh sách chuyến bay + mã sân bay đi/đến + số ghế AVAILABLE
//        private const string SqlGetAll = @"
//            SELECT
//                f.flight_id,
//                f.flight_number,
//                dep.airport_code   AS from_code,
//                arr.airport_code   AS to_code,
//                f.departure_time,
//                f.arrival_time,
//                f.status,
//                COALESCE(SUM(CASE WHEN fs.seat_status = 'AVAILABLE' THEN 1 ELSE 0 END), 0) AS seat_available
//            FROM Flights f
//            JOIN Routes r       ON f.route_id = r.route_id
//            JOIN Airports dep   ON r.departure_place_id = dep.airport_id
//            JOIN Airports arr   ON r.arrival_place_id   = arr.airport_id
//            LEFT JOIN Flight_Seats fs ON fs.flight_id = f.flight_id
//            GROUP BY
//                f.flight_id, f.flight_number, from_code, to_code, f.departure_time, f.arrival_time, f.status
//            ORDER BY f.departure_time;";

//        public static async Task<List<FlightListItemDTO>> GetAllAsync()
//        {
//            var list = new List<FlightListItemDTO>();

//            using var conn = await DatabaseConnection.OpenAsync();
//            using var cmd = new MySqlCommand(SqlGetAll, conn);

//            using var reader = await cmd.ExecuteReaderAsync();
//            while (await reader.ReadAsync())
//            {
//                var dto = MapReaderToDto(reader);
//                list.Add(dto);
//            }

//            return list;
//        }

//        // Ánh xạ 1 dòng dữ liệu -> DTO (tuân thủ validation trong DTO)
//        private static FlightListItemDTO MapReaderToDto(MySqlDataReader reader)
//        {
//            // Lấy giá trị an toàn
//            int flightId = reader.GetInt32(reader.GetOrdinal("flight_id"));
//            string flightNumber = reader.GetString(reader.GetOrdinal("flight_number"));
//            string fromCode = reader.GetString(reader.GetOrdinal("from_code"));
//            string toCode = reader.GetString(reader.GetOrdinal("to_code"));
//            DateTime departureTime = reader.GetDateTime(reader.GetOrdinal("departure_time"));
//            DateTime arrivalTime = reader.GetDateTime(reader.GetOrdinal("arrival_time"));
//            string status = reader.GetString(reader.GetOrdinal("status"));
//            int seatAvailable = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("seat_available")));

//            // Gán qua property để chạy validation (đúng format thầy yêu cầu)
//            var dto = new FlightListItemDTO
//            {
//                FlightId = flightId,
//                FlightNumber = flightNumber,
//                FromAirportCode = fromCode,
//                ToAirportCode = toCode,
//                DepartureTime = departureTime,
//                ArrivalTime = arrivalTime,
//                Status = status,
//                SeatAvailable = seatAvailable
//            };

//            return dto;
//        }
//    }
//}
