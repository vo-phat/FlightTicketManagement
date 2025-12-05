-- ============================================
-- SCRIPT SETUP DATABASE CHO FLIGHT TICKET MANAGEMENT
-- Chạy script này trong MySQL Workbench hoặc phpMyAdmin
-- ============================================

-- Tạo database (nếu chưa có)
CREATE DATABASE IF NOT EXISTS flightticketmanagement 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

USE flightticketmanagement;

-- ============================================
-- 1. TẠO CÁC BẢNG
-- ============================================

-- (Bảng Airlines, Airports, Aircraft, etc. đã có trong database.txt)
-- Script này chỉ tạo các INDEX cần thiết cho PERFORMANCE

-- ============================================
-- 2. TẠO INDEXES ĐỂ TĂNG PERFORMANCE
-- ============================================

-- FIX 4: Thêm indexes cho các cột thường xuyên tìm kiếm

-- Index cho Flights.departure_time (tìm kiếm theo ngày, filter "Hôm nay", "Ngày mai")
CREATE INDEX IF NOT EXISTS idx_flights_departure_time 
ON Flights(departure_time);

-- Index cho Flights.status (filter theo trạng thái: SCHEDULED, DELAYED, etc.)
CREATE INDEX IF NOT EXISTS idx_flights_status 
ON Flights(status);

-- Index cho Payments.payment_date (thống kê doanh thu theo tháng/năm)
CREATE INDEX IF NOT EXISTS idx_payments_payment_date 
ON Payments(payment_date);

-- Composite index cho Bookings (status + flight_id) - dùng cho Stats queries
CREATE INDEX IF NOT EXISTS idx_bookings_status_flight 
ON Bookings(status, flight_id);

-- ============================================
-- 3. KIỂM TRA KẾT QUẢ
-- ============================================

-- Hiển thị tất cả indexes của bảng Flights
SHOW INDEX FROM Flights;

-- Hiển thị tất cả indexes của bảng Payments
SHOW INDEX FROM Payments;

-- Hiển thị tất cả indexes của bảng Bookings
SHOW INDEX FROM Bookings;

-- ============================================
-- 4. TEST CONNECTION
-- ============================================

-- Kiểm tra số lượng chuyến bay
SELECT COUNT(*) AS TotalFlights FROM Flights;

-- Kiểm tra số lượng bookings
SELECT COUNT(*) AS TotalBookings FROM Bookings;

-- Kiểm tra dữ liệu sample
SELECT 
    f.flight_number,
    f.departure_time,
    f.status,
    CONCAT(ap1.city, ' → ', ap2.city) AS route
FROM Flights f
JOIN Routes r ON f.route_id = r.route_id
JOIN Airports ap1 ON r.origin_airport_id = ap1.airport_id
JOIN Airports ap2 ON r.destination_airport_id = ap2.airport_id
LIMIT 5;

-- ============================================
-- DONE! Database đã sẵn sàng
-- Bây giờ có thể chạy application
-- ============================================
