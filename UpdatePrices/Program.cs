using MySqlConnector;

string connStr = "Server=localhost;Database=flightticketmanagement;User ID=root;Password=;";

string updateQuery = @"
    UPDATE Flights f
    INNER JOIN Routes r ON f.route_id = r.route_id
    SET f.base_price = CASE
        WHEN r.distance_km < 500 THEN 1000000
        WHEN r.distance_km >= 500 AND r.distance_km < 1000 THEN 1800000
        WHEN r.distance_km >= 1000 AND r.distance_km < 2000 THEN 2800000
        WHEN r.distance_km >= 2000 AND r.distance_km < 3000 THEN 4200000
        WHEN r.distance_km >= 3000 AND r.distance_km < 5000 THEN 6500000
        WHEN r.distance_km >= 5000 AND r.distance_km < 8000 THEN 10000000
        ELSE 15000000
    END,
    f.note = CASE
        WHEN r.distance_km < 500 THEN 'Chuyến bay nội địa ngắn'
        WHEN r.distance_km >= 500 AND r.distance_km < 1000 THEN 'Chuyến bay nội địa trung bình'
        WHEN r.distance_km >= 1000 AND r.distance_km < 2000 THEN 'Chuyến bay nội địa dài'
        WHEN r.distance_km >= 2000 AND r.distance_km < 3000 THEN 'Chuyến bay quốc tế ngắn'
        WHEN r.distance_km >= 3000 AND r.distance_km < 5000 THEN 'Chuyến bay quốc tế trung bình'
        WHEN r.distance_km >= 5000 AND r.distance_km < 8000 THEN 'Chuyến bay quốc tế dài'
        ELSE 'Chuyến bay quốc tế liên lục địa'
    END;";

string selectQuery = @"
    SELECT 
        f.flight_id,
        f.flight_number,
        CONCAT(dep.airport_code, ' → ', arr.airport_code) AS route,
        r.distance_km,
        f.base_price AS base_price_vnd,
        f.note
    FROM Flights f
    INNER JOIN Routes r ON f.route_id = r.route_id
    INNER JOIN Airports dep ON r.departure_place_id = dep.airport_id
    INNER JOIN Airports arr ON r.arrival_place_id = arr.airport_id
    ORDER BY r.distance_km ASC, f.flight_number ASC
    LIMIT 15;";

try
{
    using var conn = new MySqlConnection(connStr);
    conn.Open();
    
    Console.WriteLine("🔄 Đang cập nhật giá cơ bản cho các chuyến bay...\n");
    
    using (var cmd = new MySqlCommand(updateQuery, conn))
    {
        int rowsAffected = cmd.ExecuteNonQuery();
        Console.WriteLine($"✅ Đã cập nhật {rowsAffected} chuyến bay\n");
    }
    
    Console.WriteLine("📋 15 chuyến bay đầu tiên sau khi cập nhật:\n");
    Console.WriteLine("{0,-5} {1,-12} {2,-20} {3,-12} {4,-15} {5,-35}", 
        "ID", "Số hiệu", "Tuyến", "Khoảng cách", "Giá (VNĐ)", "Ghi chú");
    Console.WriteLine(new string('-', 115));
    
    using (var cmd = new MySqlCommand(selectQuery, conn))
    using (var reader = cmd.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine("{0,-5} {1,-12} {2,-20} {3,-12:N0} {4,-15:N0} {5,-35}", 
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetInt32(3),
                reader.GetDecimal(4),
                reader.IsDBNull(5) ? "" : reader.GetString(5));
        }
    }
    
    Console.WriteLine("\n✅ Hoàn tất cập nhật giá!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Lỗi: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}
