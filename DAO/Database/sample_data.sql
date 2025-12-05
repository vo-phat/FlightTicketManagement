USE `FlightTicketManagement`;

/* =========================================================
   SAMPLE DATA FOR FLIGHTS AND STATISTICS
   Chạy file này sau khi đã chạy script.sql và data.sql
   ========================================================= */

-- Disable foreign key checks for easier data insertion
SET FOREIGN_KEY_CHECKS=0;

-- 1. National (Quốc gia)
INSERT INTO national (country_name, country_code, phone_code) VALUES
('Vietnam', 'VN', '+84'),
('United States', 'US', '+1'),
('United Kingdom', 'GB', '+44'),
('Japan', 'JP', '+81'),
('South Korea', 'KR', '+82'),
('China', 'CN', '+86'),
('Singapore', 'SG', '+65'),
('Thailand', 'TH', '+66'),
('Malaysia', 'MY', '+60'),
('Indonesia', 'ID', '+62'),
('Philippines', 'PH', '+63'),
('Australia', 'AU', '+61'),
('France', 'FR', '+33'),
('Germany', 'DE', '+49'),
('Canada', 'CA', '+1')
ON DUPLICATE KEY UPDATE country_name = VALUES(country_name);

-- 2. Airlines (Hãng hàng không)
INSERT INTO Airlines (airline_code, airline_name, country) VALUES
('VN', 'Vietnam Airlines', 'Vietnam'),
('VJ', 'VietJet Air', 'Vietnam'),
('BB', 'Bamboo Airways', 'Vietnam'),
('QH', 'Bamboo Airways', 'Vietnam'),
('VU', 'Vietravel Airlines', 'Vietnam')
ON DUPLICATE KEY UPDATE airline_name = VALUES(airline_name);

-- 3. Airports (Sân bay)
INSERT INTO Airports (airport_code, airport_name, city, country) VALUES
('SGN', 'Tan Son Nhat International Airport', 'Ho Chi Minh', 'Vietnam'),
('HAN', 'Noi Bai International Airport', 'Ha Noi', 'Vietnam'),
('DAD', 'Da Nang International Airport', 'Da Nang', 'Vietnam'),
('CXR', 'Cam Ranh International Airport', 'Nha Trang', 'Vietnam'),
('PQC', 'Phu Quoc International Airport', 'Phu Quoc', 'Vietnam'),
('VCA', 'Can Tho International Airport', 'Can Tho', 'Vietnam'),
('UIH', 'Phu Cat Airport', 'Quy Nhon', 'Vietnam'),
('DLI', 'Lien Khuong Airport', 'Da Lat', 'Vietnam'),
('HPH', 'Cat Bi International Airport', 'Hai Phong', 'Vietnam'),
('VII', 'Vinh International Airport', 'Vinh', 'Vietnam')
ON DUPLICATE KEY UPDATE airport_name = VALUES(airport_name);

-- 4. Aircrafts (Máy bay)
INSERT INTO Aircrafts (airline_id, model, manufacturer, capacity) VALUES
(1, 'A321', 'Airbus', 220),
(1, 'A350-900', 'Airbus', 305),
(1, 'Boeing 787-9', 'Boeing', 294),
(2, 'A321neo', 'Airbus', 230),
(2, 'A320', 'Airbus', 180),
(3, 'Boeing 787-9', 'Boeing', 294),
(3, 'Embraer E195', 'Embraer', 132),
(4, 'A320neo', 'Airbus', 186),
(5, 'A321neo', 'Airbus', 220)
ON DUPLICATE KEY UPDATE model = VALUES(model);

-- 5. Routes (Tuyến bay)
INSERT INTO Routes (departure_place_id, arrival_place_id, distance_km, duration_minutes) VALUES
-- SGN routes
(1, 2, 1160, 130),  -- SGN -> HAN
(1, 3, 610, 80),    -- SGN -> DAD
(1, 4, 320, 55),    -- SGN -> CXR
(1, 5, 300, 50),    -- SGN -> PQC
(1, 6, 170, 40),    -- SGN -> VCA
(1, 8, 300, 50),    -- SGN -> DLI

-- HAN routes
(2, 1, 1160, 130),  -- HAN -> SGN
(2, 3, 610, 75),    -- HAN -> DAD
(2, 4, 910, 110),   -- HAN -> CXR
(2, 5, 1430, 160),  -- HAN -> PQC
(2, 9, 100, 35),    -- HAN -> HPH

-- DAD routes
(3, 1, 610, 80),    -- DAD -> SGN
(3, 2, 610, 75),    -- DAD -> HAN
(3, 4, 290, 45),    -- DAD -> CXR

-- Other routes
(4, 1, 320, 55),    -- CXR -> SGN
(5, 1, 300, 50),    -- PQC -> SGN
(6, 1, 170, 40)     -- VCA -> SGN
ON DUPLICATE KEY UPDATE distance_km = VALUES(distance_km);

-- 6. Cabin Classes (Hạng ghế)
INSERT INTO Cabin_Classes (class_name) VALUES
('Economy'),
('Premium Economy'),
('Business'),
('First Class')
ON DUPLICATE KEY UPDATE class_name = VALUES(class_name);

-- 7. Flights (Chuyến bay) - Tạo dữ liệu cho 3 tháng gần đây
INSERT INTO Flights (flight_number, aircraft_id, route_id, departure_time, arrival_time, status) VALUES
-- Tháng 9/2024
('VN201', 1, 1, '2024-09-01 06:00:00', '2024-09-01 08:10:00', 'COMPLETED'),
('VN202', 2, 2, '2024-09-01 08:30:00', '2024-09-01 10:00:00', 'COMPLETED'),
('VJ301', 4, 3, '2024-09-02 07:15:00', '2024-09-02 08:10:00', 'COMPLETED'),
('VJ302', 5, 4, '2024-09-02 09:00:00', '2024-09-02 10:00:00', 'COMPLETED'),
('BB401', 6, 5, '2024-09-03 10:30:00', '2024-09-03 11:20:00', 'COMPLETED'),
('VN203', 3, 1, '2024-09-05 14:00:00', '2024-09-05 16:10:00', 'COMPLETED'),
('VN204', 1, 2, '2024-09-06 16:30:00', '2024-09-06 18:00:00', 'COMPLETED'),
('VJ303', 4, 6, '2024-09-07 05:45:00', '2024-09-07 06:25:00', 'COMPLETED'),
('BB402', 7, 3, '2024-09-08 11:15:00', '2024-09-08 12:10:00', 'COMPLETED'),
('VN205', 2, 7, '2024-09-10 13:30:00', '2024-09-10 16:40:00', 'COMPLETED'),

-- Tháng 10/2024
('VN206', 1, 1, '2024-10-01 06:00:00', '2024-10-01 08:10:00', 'COMPLETED'),
('VN207', 2, 2, '2024-10-01 08:30:00', '2024-10-01 10:00:00', 'COMPLETED'),
('VJ304', 4, 3, '2024-10-02 07:15:00', '2024-10-02 08:10:00', 'COMPLETED'),
('VJ305', 5, 4, '2024-10-03 09:00:00', '2024-10-03 10:00:00', 'COMPLETED'),
('BB403', 6, 5, '2024-10-04 10:30:00', '2024-10-04 11:20:00', 'COMPLETED'),
('VN208', 3, 1, '2024-10-05 14:00:00', '2024-10-05 16:10:00', 'COMPLETED'),
('VN209', 1, 2, '2024-10-06 16:30:00', '2024-10-06 18:00:00', 'DELAYED'),
('VJ306', 4, 6, '2024-10-07 05:45:00', '2024-10-07 06:25:00', 'COMPLETED'),
('BB404', 7, 3, '2024-10-08 11:15:00', '2024-10-08 12:10:00', 'COMPLETED'),
('VN210', 2, 7, '2024-10-10 13:30:00', '2024-10-10 16:40:00', 'COMPLETED'),
('VN211', 1, 8, '2024-10-12 15:00:00', '2024-10-12 16:15:00', 'COMPLETED'),
('VJ307', 5, 9, '2024-10-14 07:00:00', '2024-10-14 08:50:00', 'COMPLETED'),
('BB405', 6, 10, '2024-10-16 09:30:00', '2024-10-16 12:10:00', 'COMPLETED'),
('VN212', 2, 11, '2024-10-18 11:45:00', '2024-10-18 12:20:00', 'COMPLETED'),
('VJ308', 4, 12, '2024-10-20 13:15:00', '2024-10-20 14:30:00', 'CANCELLED'),

-- Tháng 11/2024
('VN213', 1, 1, '2024-11-01 06:00:00', '2024-11-01 08:10:00', 'COMPLETED'),
('VN214', 2, 2, '2024-11-01 08:30:00', '2024-11-01 10:00:00', 'COMPLETED'),
('VJ309', 4, 3, '2024-11-02 07:15:00', '2024-11-02 08:10:00', 'COMPLETED'),
('VJ310', 5, 4, '2024-11-03 09:00:00', '2024-11-03 10:00:00', 'COMPLETED'),
('BB406', 6, 5, '2024-11-04 10:30:00', '2024-11-04 11:20:00', 'COMPLETED'),
('VN215', 3, 1, '2024-11-05 14:00:00', '2024-11-05 16:10:00', 'COMPLETED'),
('VN216', 1, 2, '2024-11-06 16:30:00', '2024-11-06 18:00:00', 'COMPLETED'),
('VJ311', 4, 6, '2024-11-07 05:45:00', '2024-11-07 06:25:00', 'COMPLETED'),
('BB407', 7, 3, '2024-11-08 11:15:00', '2024-11-08 12:10:00', 'DELAYED'),
('VN217', 2, 7, '2024-11-10 13:30:00', '2024-11-10 16:40:00', 'COMPLETED'),
('VN218', 1, 8, '2024-11-12 15:00:00', '2024-11-12 16:15:00', 'COMPLETED'),
('VJ312', 5, 9, '2024-11-14 07:00:00', '2024-11-14 08:50:00', 'COMPLETED'),
('BB408', 6, 10, '2024-11-16 09:30:00', '2024-11-16 12:10:00', 'COMPLETED'),
('VN219', 2, 11, '2024-11-18 11:45:00', '2024-11-18 12:20:00', 'COMPLETED'),
('VJ313', 4, 12, '2024-11-20 13:15:00', '2024-11-20 14:30:00', 'COMPLETED'),
('BB409', 7, 13, '2024-11-22 14:30:00', '2024-11-22 15:15:00', 'COMPLETED'),
('VN220', 3, 14, '2024-11-24 16:00:00', '2024-11-24 16:55:00', 'COMPLETED'),
('VJ314', 5, 15, '2024-11-26 08:15:00', '2024-11-26 09:05:00', 'COMPLETED'),
('BB410', 6, 16, '2024-11-28 10:45:00', '2024-11-28 11:25:00', 'COMPLETED'),

-- Tháng 12/2024 (scheduled)
('VN221', 1, 1, '2024-12-01 06:00:00', '2024-12-01 08:10:00', 'SCHEDULED'),
('VN222', 2, 2, '2024-12-01 08:30:00', '2024-12-01 10:00:00', 'SCHEDULED'),
('VJ315', 4, 3, '2024-12-02 07:15:00', '2024-12-02 08:10:00', 'SCHEDULED'),
('VJ316', 5, 4, '2024-12-03 09:00:00', '2024-12-03 10:00:00', 'SCHEDULED'),
('BB411', 6, 5, '2024-12-04 10:30:00', '2024-12-04 11:20:00', 'SCHEDULED'),
('VN223', 3, 1, '2024-12-05 14:00:00', '2024-12-05 16:10:00', 'SCHEDULED'),
('VN224', 1, 2, '2024-12-06 16:30:00', '2024-12-06 18:00:00', 'SCHEDULED'),
('VJ317', 4, 6, '2024-12-07 05:45:00', '2024-12-07 06:25:00', 'SCHEDULED'),
('BB412', 7, 3, '2024-12-08 11:15:00', '2024-12-08 12:10:00', 'SCHEDULED'),
('VN225', 2, 7, '2024-12-10 13:30:00', '2024-12-10 16:40:00', 'SCHEDULED')
ON DUPLICATE KEY UPDATE status = VALUES(status);

-- 8. Seats (Ghế ngồi trên máy bay) - Tạo ghế cho máy bay mẫu
-- Aircraft 1 (A321, 220 seats) - Tạo 60 ghế mẫu
INSERT INTO Seats (aircraft_id, seat_number, class_id) VALUES
-- First Class (rows 1-3)
(1, '1A', 4), (1, '1B', 4), (1, '1C', 4), (1, '1D', 4), (1, '1E', 4), (1, '1F', 4),
(1, '2A', 4), (1, '2B', 4), (1, '2C', 4), (1, '2D', 4), (1, '2E', 4), (1, '2F', 4),
(1, '3A', 4), (1, '3B', 4), (1, '3C', 4), (1, '3D', 4), (1, '3E', 4), (1, '3F', 4),
-- Business (rows 4-10)
(1, '4A', 3), (1, '4B', 3), (1, '4C', 3), (1, '4D', 3), (1, '4E', 3), (1, '4F', 3),
(1, '5A', 3), (1, '5B', 3), (1, '5C', 3), (1, '5D', 3), (1, '5E', 3), (1, '5F', 3),
(1, '6A', 3), (1, '6B', 3), (1, '6C', 3), (1, '6D', 3), (1, '6E', 3), (1, '6F', 3),
(1, '7A', 3), (1, '7B', 3), (1, '7C', 3), (1, '7D', 3), (1, '7E', 3), (1, '7F', 3),
-- Premium Economy (rows 11-15)
(1, '11A', 2), (1, '11B', 2), (1, '11C', 2), (1, '11D', 2), (1, '11E', 2), (1, '11F', 2),
(1, '12A', 2), (1, '12B', 2), (1, '12C', 2), (1, '12D', 2), (1, '12E', 2), (1, '12F', 2),
-- Economy (rows 21-23)
(1, '21A', 1), (1, '21B', 1), (1, '21C', 1), (1, '21D', 1), (1, '21E', 1), (1, '21F', 1)
ON DUPLICATE KEY UPDATE seat_number = VALUES(seat_number);

-- 9. Flight_Seats (Ghế cho chuyến bay) - Tạo ghế và giá cho một số chuyến bay mẫu
-- Flight 1 (VN201) - Chỉ tạo 50 ghế mẫu
INSERT INTO Flight_Seats (flight_id, seat_id, base_price, seat_status)
SELECT 1, seat_id,
       CASE 
           WHEN class_id = 4 THEN 8000000  -- First Class
           WHEN class_id = 3 THEN 5000000  -- Business
           WHEN class_id = 2 THEN 3000000  -- Premium Economy
           ELSE 2000000                    -- Economy
       END as price,
       'AVAILABLE'
FROM Seats 
WHERE aircraft_id = 1 
LIMIT 50
ON DUPLICATE KEY UPDATE base_price = VALUES(base_price);

-- NOTE: Sections 9-14 require accounts table to have data
-- Please run data.sql first, or uncomment the section below to create sample accounts

-- OPTIONAL: Create sample accounts if not exists
INSERT INTO accounts (email, password, failed_attempts, is_active, created_at) VALUES
('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW()),
('staff@gmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW()),
('user@gmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW())
ON DUPLICATE KEY UPDATE password = VALUES(password);

-- Skip bookings, passengers, tickets, payments, baggage for now
-- These require accounts table to be populated first
-- Run data.sql before uncommenting these sections

-- 10. Bookings (Đặt chỗ) - Tạo nhiều bookings cho stats
INSERT INTO Bookings (account_id, booking_date, trip_type, status, total_amount) VALUES
-- Tháng 9/2024
(1, '2024-09-01 05:00:00', 'ONE_WAY', 'CONFIRMED', 4000000),
(2, '2024-09-02 06:00:00', 'ROUND_TRIP', 'CONFIRMED', 8000000),
(3, '2024-09-05 07:00:00', 'ONE_WAY', 'CONFIRMED', 3500000),
(1, '2024-09-08 08:00:00', 'ONE_WAY', 'CONFIRMED', 2800000),
(2, '2024-09-10 09:00:00', 'ROUND_TRIP', 'CONFIRMED', 9500000),

-- Tháng 10/2024
(3, '2024-10-01 05:30:00', 'ONE_WAY', 'CONFIRMED', 2500000),
(1, '2024-10-05 07:00:00', 'ONE_WAY', 'CONFIRMED', 3200000),
(2, '2024-10-08 06:30:00', 'ROUND_TRIP', 'CONFIRMED', 10000000),
(3, '2024-10-12 08:00:00', 'ONE_WAY', 'CONFIRMED', 4500000),
(1, '2024-10-15 09:00:00', 'ONE_WAY', 'CONFIRMED', 3800000),
(2, '2024-10-20 10:00:00', 'ROUND_TRIP', 'CANCELLED', 8500000),

-- Tháng 11/2024
(2, '2024-11-01 04:45:00', 'ROUND_TRIP', 'CONFIRMED', 10000000),
(3, '2024-11-03 06:00:00', 'ONE_WAY', 'CONFIRMED', 3200000),
(1, '2024-11-07 07:30:00', 'ONE_WAY', 'CONFIRMED', 2900000),
(2, '2024-11-12 08:00:00', 'ROUND_TRIP', 'CONFIRMED', 11500000),
(3, '2024-11-15 09:00:00', 'ONE_WAY', 'CONFIRMED', 4200000),
(1, '2024-11-20 10:00:00', 'ONE_WAY', 'CONFIRMED', 3600000),
(2, '2024-11-25 11:00:00', 'ROUND_TRIP', 'CONFIRMED', 9800000),

-- Tháng 12/2024
(3, '2024-12-01 06:00:00', 'ONE_WAY', 'PENDING', 3500000),
(1, '2024-12-03 07:00:00', 'ONE_WAY', 'PENDING', 4000000)
ON DUPLICATE KEY UPDATE status = VALUES(status);

-- 11. Passenger_Profiles (Hồ sơ hành khách)
INSERT INTO Passenger_Profiles (account_id, full_name, date_of_birth, phone_number, passport_number, nationality) VALUES
(1, 'Nguyen Van A', '1990-01-15', '0901234567', 'N1234567', 'Vietnam'),
(2, 'Tran Thi B', '1985-05-20', '0912345678', 'N2345678', 'Vietnam'),
(3, 'Le Van C', '1992-08-10', '0923456789', 'N3456789', 'Vietnam'),
(1, 'Nguyen Thi D', '2015-03-25', '', '', 'Vietnam'),
(2, 'Tran Van E', '2018-07-12', '', '', 'Vietnam')
ON DUPLICATE KEY UPDATE full_name = VALUES(full_name);

-- 12. Booking_Passengers (Hành khách trong booking)
INSERT INTO Booking_Passengers (booking_id, profile_id) VALUES
-- September bookings
(1, 1), (2, 2), (2, 5), (3, 3), (4, 1), (5, 2), (5, 5),
-- October bookings
(6, 3), (7, 1), (8, 2), (8, 5), (9, 3), (10, 1), (11, 2),
-- November bookings
(12, 2), (12, 5), (13, 3), (14, 1), (15, 2), (15, 5), (16, 3), (17, 1), (18, 2),
-- December bookings
(19, 3), (20, 1)
ON DUPLICATE KEY UPDATE booking_id = VALUES(booking_id);

-- 13. Tickets (Vé) - Tạo tickets cho các bookings
INSERT INTO Tickets (ticket_passenger_id, flight_seat_id, ticket_number, issue_date, segment_no, segment_type, status) VALUES
-- September tickets
(1, 1, 'VN201-001', '2024-09-01 05:00:00', 1, 'OUTBOUND', 'BOARDED'),
(2, 2, 'VN202-001', '2024-09-02 06:00:00', 1, 'OUTBOUND', 'BOARDED'),
(3, 3, 'VN202-002', '2024-09-02 06:00:00', 2, 'INBOUND', 'BOARDED'),
(4, 4, 'VJ301-001', '2024-09-05 07:00:00', 1, 'OUTBOUND', 'BOARDED'),
(5, 5, 'BB401-001', '2024-09-08 08:00:00', 1, 'OUTBOUND', 'BOARDED'),
(6, 6, 'VN205-001', '2024-09-10 09:00:00', 1, 'OUTBOUND', 'BOARDED'),
(7, 7, 'VN205-002', '2024-09-10 09:00:00', 2, 'INBOUND', 'BOARDED'),

-- October tickets
(8, 8, 'VN206-001', '2024-10-01 05:30:00', 1, 'OUTBOUND', 'BOARDED'),
(9, 9, 'VN208-001', '2024-10-05 07:00:00', 1, 'OUTBOUND', 'BOARDED'),
(10, 10, 'VN209-001', '2024-10-08 06:30:00', 1, 'OUTBOUND', 'BOARDED'),
(11, 11, 'VN209-002', '2024-10-08 06:30:00', 2, 'INBOUND', 'BOARDED'),
(12, 12, 'VN211-001', '2024-10-12 08:00:00', 1, 'OUTBOUND', 'BOARDED'),
(13, 13, 'VJ307-001', '2024-10-15 09:00:00', 1, 'OUTBOUND', 'BOARDED'),

-- November tickets
(15, 14, 'VN213-001', '2024-11-01 04:45:00', 1, 'OUTBOUND', 'BOARDED'),
(16, 15, 'VN213-002', '2024-11-01 04:45:00', 2, 'INBOUND', 'BOARDED'),
(17, 16, 'VJ309-001', '2024-11-03 06:00:00', 1, 'OUTBOUND', 'BOARDED'),
(18, 17, 'VJ311-001', '2024-11-07 07:30:00', 1, 'OUTBOUND', 'BOARDED'),
(19, 18, 'VN218-001', '2024-11-12 08:00:00', 1, 'OUTBOUND', 'BOARDED'),
(20, 19, 'VN218-002', '2024-11-12 08:00:00', 2, 'INBOUND', 'BOARDED'),
(21, 20, 'VJ313-001', '2024-11-15 09:00:00', 1, 'OUTBOUND', 'BOARDED'),
(22, 21, 'VN219-001', '2024-11-20 10:00:00', 1, 'OUTBOUND', 'BOARDED'),
(23, 22, 'BB410-001', '2024-11-25 11:00:00', 1, 'OUTBOUND', 'BOARDED'),
(24, 23, 'BB410-002', '2024-11-25 11:00:00', 2, 'INBOUND', 'BOARDED')
ON DUPLICATE KEY UPDATE status = VALUES(status);

-- 14. Payments (Thanh toán) - Tạo payments cho các bookings
INSERT INTO Payments (booking_id, amount, payment_method, payment_date, status) VALUES
-- September payments
(1, 4000000, 'CREDIT_CARD', '2024-09-01 05:05:00', 'SUCCESS'),
(2, 8000000, 'BANK_TRANSFER', '2024-09-02 06:10:00', 'SUCCESS'),
(3, 3500000, 'E_WALLET', '2024-09-05 07:05:00', 'SUCCESS'),
(4, 2800000, 'CREDIT_CARD', '2024-09-08 08:05:00', 'SUCCESS'),
(5, 9500000, 'BANK_TRANSFER', '2024-09-10 09:05:00', 'SUCCESS'),

-- October payments
(6, 2500000, 'E_WALLET', '2024-10-01 05:35:00', 'SUCCESS'),
(7, 3200000, 'CREDIT_CARD', '2024-10-05 07:05:00', 'SUCCESS'),
(8, 10000000, 'BANK_TRANSFER', '2024-10-08 06:35:00', 'SUCCESS'),
(9, 4500000, 'E_WALLET', '2024-10-12 08:05:00', 'SUCCESS'),
(10, 3800000, 'CREDIT_CARD', '2024-10-15 09:05:00', 'SUCCESS'),
(11, 8500000, 'BANK_TRANSFER', '2024-10-20 10:05:00', 'FAILED'),

-- November payments
(12, 10000000, 'BANK_TRANSFER', '2024-11-01 04:50:00', 'SUCCESS'),
(13, 3200000, 'CREDIT_CARD', '2024-11-03 06:05:00', 'SUCCESS'),
(14, 2900000, 'E_WALLET', '2024-11-07 07:35:00', 'SUCCESS'),
(15, 11500000, 'BANK_TRANSFER', '2024-11-12 08:05:00', 'SUCCESS'),
(16, 4200000, 'CREDIT_CARD', '2024-11-15 09:05:00', 'SUCCESS'),
(17, 3600000, 'E_WALLET', '2024-11-20 10:05:00', 'SUCCESS'),
(18, 9800000, 'BANK_TRANSFER', '2024-11-25 11:05:00', 'SUCCESS'),

-- December payments
(19, 3500000, 'CREDIT_CARD', '2024-12-01 06:05:00', 'PENDING'),
(20, 4000000, 'E_WALLET', '2024-12-03 07:05:00', 'PENDING')
ON DUPLICATE KEY UPDATE status = VALUES(status);

-- 15. Baggage (Hành lý)
INSERT INTO Baggage (ticket_id, flight_id, baggage_tag, baggage_type, weight_kg, allowed_weight_kg, fee, status) VALUES
(1, 1, 'VN201-BAG001', 'CHECKED', 20.5, 23.0, 0, 'CLAIMED'),
(2, 2, 'VN202-BAG001', 'CHECKED', 25.0, 23.0, 50000, 'CLAIMED'),
(3, 3, 'VN202-BAG002', 'CHECKED', 22.0, 23.0, 0, 'CLAIMED'),
(4, 3, 'VJ301-BAG001', 'CHECKED', 18.5, 23.0, 0, 'CLAIMED'),
(5, 4, 'BB401-BAG001', 'CHECKED', 23.0, 23.0, 0, 'CLAIMED'),
(8, 11, 'VN206-BAG001', 'CHECKED', 21.0, 23.0, 0, 'CLAIMED'),
(9, 16, 'VN208-BAG001', 'CHECKED', 24.5, 23.0, 40000, 'CLAIMED'),
(15, 26, 'VN213-BAG001', 'CHECKED', 19.5, 23.0, 0, 'CLAIMED'),
(17, 28, 'VJ309-BAG001', 'CHECKED', 22.5, 23.0, 0, 'CLAIMED'),
(19, 35, 'VN218-BAG001', 'CHECKED', 20.0, 23.0, 0, 'CLAIMED')
ON DUPLICATE KEY UPDATE status = VALUES(status);

-- 16. Fare_Rules (Quy tắc giá vé)
INSERT INTO Fare_Rules (route_id, class_id, fare_type, season, effective_date, expiry_date, description, price) VALUES
-- Route SGN-HAN
(1, 1, 'Standard', 'NORMAL', '2024-01-01', '2024-12-31', 'Economy giá chuẩn', 2000000),
(1, 2, 'Standard', 'NORMAL', '2024-01-01', '2024-12-31', 'Premium Economy giá chuẩn', 3000000),
(1, 3, 'Standard', 'NORMAL', '2024-01-01', '2024-12-31', 'Business giá chuẩn', 5000000),
(1, 4, 'Standard', 'NORMAL', '2024-01-01', '2024-12-31', 'First Class giá chuẩn', 8000000),

-- Route HAN-SGN
(7, 1, 'Standard', 'NORMAL', '2024-01-01', '2024-12-31', 'Economy giá chuẩn', 2000000),
(7, 2, 'Standard', 'NORMAL', '2024-01-01', '2024-12-31', 'Premium Economy giá chuẩn', 3000000),
(7, 3, 'Standard', 'NORMAL', '2024-01-01', '2024-12-31', 'Business giá chuẩn', 5000000),

-- Route SGN-DAD
(2, 1, 'Standard', 'NORMAL', '2024-01-01', '2024-12-31', 'Economy giá chuẩn', 1500000),
(2, 2, 'Promo', 'OFFPEAK', '2024-01-01', '2024-03-31', 'Khuyến mãi mùa thấp điểm', 1200000),

-- Route SGN-CXR
(3, 1, 'Standard', 'NORMAL', '2024-01-01', '2024-12-31', 'Economy giá chuẩn', 1200000),
(3, 1, 'Flex', 'PEAK', '2024-12-01', '2024-12-31', 'Linh hoạt mùa cao điểm', 1800000)
ON DUPLICATE KEY UPDATE price = VALUES(price);

-- Re-enable foreign key checks
SET FOREIGN_KEY_CHECKS=1;

-- Verification queries (uncomment to check data)
-- SELECT COUNT(*) as total_flights FROM Flights;
-- SELECT COUNT(*) as completed_flights FROM Flights WHERE status = 'COMPLETED';
-- SELECT COUNT(*) as scheduled_flights FROM Flights WHERE status = 'SCHEDULED';
-- SELECT COUNT(*) as total_bookings FROM Bookings;
-- SELECT COUNT(*) as total_payments FROM Payments;
-- SELECT SUM(amount) as total_revenue FROM Payments WHERE status = 'SUCCESS';
