-- Script tạo dữ liệu test cho Payments
-- Giả sử đã có: Flights, Routes, Airports, Flight_Seats, Bookings, Booking_Passengers, Tickets

USE flightticketmanagement;

-- 1. Tạo bookings test (nếu chưa có)
INSERT INTO Bookings (booking_code, booking_date, status, total_amount, passenger_name, passenger_email, passenger_phone)
VALUES 
    ('BK2025001', '2025-01-15 10:30:00', 'CONFIRMED', 5000000, 'Nguyen Van A', 'nguyenvana@gmail.com', '0901234567'),
    ('BK2025002', '2025-02-20 14:20:00', 'CONFIRMED', 3500000, 'Tran Thi B', 'tranthib@gmail.com', '0912345678'),
    ('BK2025003', '2025-03-10 09:15:00', 'CONFIRMED', 7200000, 'Le Van C', 'levanc@gmail.com', '0923456789'),
    ('BK2025004', '2025-04-25 16:45:00', 'CONFIRMED', 4800000, 'Pham Thi D', 'phamthid@gmail.com', '0934567890'),
    ('BK2025005', '2025-05-18 11:00:00', 'CONFIRMED', 6500000, 'Hoang Van E', 'hoangvane@gmail.com', '0945678901'),
    ('BK2025006', '2025-06-22 13:30:00', 'CONFIRMED', 5500000, 'Vo Thi F', 'vothif@gmail.com', '0956789012'),
    ('BK2025007', '2025-07-14 08:20:00', 'CONFIRMED', 8900000, 'Dang Van G', 'dangvang@gmail.com', '0967890123'),
    ('BK2025008', '2025-08-30 15:10:00', 'CONFIRMED', 4200000, 'Bui Thi H', 'buithih@gmail.com', '0978901234'),
    ('BK2025009', '2025-09-12 10:45:00', 'CONFIRMED', 6800000, 'Do Van I', 'dovani@gmail.com', '0989012345'),
    ('BK2025010', '2025-10-08 12:00:00', 'CONFIRMED', 7500000, 'Ngo Thi K', 'ngothik@gmail.com', '0990123456')
ON DUPLICATE KEY UPDATE booking_code=booking_code;

-- 2. Lấy booking IDs vừa tạo
SET @booking1 = (SELECT booking_id FROM Bookings WHERE booking_code = 'BK2025001' LIMIT 1);
SET @booking2 = (SELECT booking_id FROM Bookings WHERE booking_code = 'BK2025002' LIMIT 1);
SET @booking3 = (SELECT booking_id FROM Bookings WHERE booking_code = 'BK2025003' LIMIT 1);
SET @booking4 = (SELECT booking_id FROM Bookings WHERE booking_code = 'BK2025004' LIMIT 1);
SET @booking5 = (SELECT booking_id FROM Bookings WHERE booking_code = 'BK2025005' LIMIT 1);
SET @booking6 = (SELECT booking_id FROM Bookings WHERE booking_code = 'BK2025006' LIMIT 1);
SET @booking7 = (SELECT booking_id FROM Bookings WHERE booking_code = 'BK2025007' LIMIT 1);
SET @booking8 = (SELECT booking_id FROM Bookings WHERE booking_code = 'BK2025008' LIMIT 1);
SET @booking9 = (SELECT booking_id FROM Bookings WHERE booking_code = 'BK2025009' LIMIT 1);
SET @booking10 = (SELECT booking_id FROM Bookings WHERE booking_code = 'BK2025010' LIMIT 1);

-- 3. Tạo Payments cho các bookings
INSERT INTO Payments (booking_id, payment_method, amount, payment_date, status, transaction_id)
VALUES 
    (@booking1, 'CREDIT_CARD', 5000000, '2025-01-15 10:35:00', 'SUCCESS', 'TXN2025001'),
    (@booking2, 'BANK_TRANSFER', 3500000, '2025-02-20 14:25:00', 'SUCCESS', 'TXN2025002'),
    (@booking3, 'CREDIT_CARD', 7200000, '2025-03-10 09:20:00', 'SUCCESS', 'TXN2025003'),
    (@booking4, 'MOMO', 4800000, '2025-04-25 16:50:00', 'SUCCESS', 'TXN2025004'),
    (@booking5, 'CREDIT_CARD', 6500000, '2025-05-18 11:05:00', 'SUCCESS', 'TXN2025005'),
    (@booking6, 'VNPAY', 5500000, '2025-06-22 13:35:00', 'SUCCESS', 'TXN2025006'),
    (@booking7, 'CREDIT_CARD', 8900000, '2025-07-14 08:25:00', 'SUCCESS', 'TXN2025007'),
    (@booking8, 'BANK_TRANSFER', 4200000, '2025-08-30 15:15:00', 'SUCCESS', 'TXN2025008'),
    (@booking9, 'CREDIT_CARD', 6800000, '2025-09-12 10:50:00', 'SUCCESS', 'TXN2025009'),
    (@booking10, 'MOMO', 7500000, '2025-10-08 12:05:00', 'SUCCESS', 'TXN2025010')
ON DUPLICATE KEY UPDATE transaction_id=transaction_id;

-- 4. Tạo Booking_Passengers (nếu chưa có)
INSERT INTO Booking_Passengers (booking_id, passenger_name, passenger_type, date_of_birth, passport_number, nationality)
SELECT 
    booking_id,
    passenger_name,
    'ADULT',
    '1990-01-01',
    CONCAT('P', LPAD(booking_id, 8, '0')),
    'VN'
FROM Bookings 
WHERE booking_code IN ('BK2025001', 'BK2025002', 'BK2025003', 'BK2025004', 'BK2025005', 
                       'BK2025006', 'BK2025007', 'BK2025008', 'BK2025009', 'BK2025010')
ON DUPLICATE KEY UPDATE passenger_name=passenger_name;

-- 5. Tạo Tickets liên kết với Flight_Seats (giả sử có flight_seats sẵn)
-- Lấy một số flight_seat_id có sẵn
SET @fs1 = (SELECT flight_seat_id FROM Flight_Seats LIMIT 1 OFFSET 0);
SET @fs2 = (SELECT flight_seat_id FROM Flight_Seats LIMIT 1 OFFSET 1);
SET @fs3 = (SELECT flight_seat_id FROM Flight_Seats LIMIT 1 OFFSET 2);
SET @fs4 = (SELECT flight_seat_id FROM Flight_Seats LIMIT 1 OFFSET 3);
SET @fs5 = (SELECT flight_seat_id FROM Flight_Seats LIMIT 1 OFFSET 4);
SET @fs6 = (SELECT flight_seat_id FROM Flight_Seats LIMIT 1 OFFSET 5);
SET @fs7 = (SELECT flight_seat_id FROM Flight_Seats LIMIT 1 OFFSET 6);
SET @fs8 = (SELECT flight_seat_id FROM Flight_Seats LIMIT 1 OFFSET 7);
SET @fs9 = (SELECT flight_seat_id FROM Flight_Seats LIMIT 1 OFFSET 8);
SET @fs10 = (SELECT flight_seat_id FROM Flight_Seats LIMIT 1 OFFSET 9);

-- Lấy booking_passenger_id
SET @bp1 = (SELECT booking_passenger_id FROM Booking_Passengers WHERE booking_id = @booking1 LIMIT 1);
SET @bp2 = (SELECT booking_passenger_id FROM Booking_Passengers WHERE booking_id = @booking2 LIMIT 1);
SET @bp3 = (SELECT booking_passenger_id FROM Booking_Passengers WHERE booking_id = @booking3 LIMIT 1);
SET @bp4 = (SELECT booking_passenger_id FROM Booking_Passengers WHERE booking_id = @booking4 LIMIT 1);
SET @bp5 = (SELECT booking_passenger_id FROM Booking_Passengers WHERE booking_id = @booking5 LIMIT 1);
SET @bp6 = (SELECT booking_passenger_id FROM Booking_Passengers WHERE booking_id = @booking6 LIMIT 1);
SET @bp7 = (SELECT booking_passenger_id FROM Booking_Passengers WHERE booking_id = @booking7 LIMIT 1);
SET @bp8 = (SELECT booking_passenger_id FROM Booking_Passengers WHERE booking_id = @booking8 LIMIT 1);
SET @bp9 = (SELECT booking_passenger_id FROM Booking_Passengers WHERE booking_id = @booking9 LIMIT 1);
SET @bp10 = (SELECT booking_passenger_id FROM Booking_Passengers WHERE booking_id = @booking10 LIMIT 1);

-- Insert Tickets
INSERT INTO Tickets (ticket_passenger_id, flight_seat_id, ticket_number, ticket_price, baggage_allowance, status, issue_date)
VALUES 
    (@bp1, @fs1, 'TK2025001', 5000000, 23, 'ISSUED', '2025-01-15 10:35:00'),
    (@bp2, @fs2, 'TK2025002', 3500000, 23, 'ISSUED', '2025-02-20 14:25:00'),
    (@bp3, @fs3, 'TK2025003', 7200000, 30, 'ISSUED', '2025-03-10 09:20:00'),
    (@bp4, @fs4, 'TK2025004', 4800000, 23, 'ISSUED', '2025-04-25 16:50:00'),
    (@bp5, @fs5, 'TK2025005', 6500000, 30, 'ISSUED', '2025-05-18 11:05:00'),
    (@bp6, @fs6, 'TK2025006', 5500000, 23, 'ISSUED', '2025-06-22 13:35:00'),
    (@bp7, @fs7, 'TK2025007', 8900000, 30, 'ISSUED', '2025-07-14 08:25:00'),
    (@bp8, @fs8, 'TK2025008', 4200000, 23, 'ISSUED', '2025-08-30 15:15:00'),
    (@bp9, @fs9, 'TK2025009', 6800000, 30, 'ISSUED', '2025-09-12 10:50:00'),
    (@bp10, @fs10, 'TK2025010', 7500000, 30, 'ISSUED', '2025-10-08 12:05:00')
ON DUPLICATE KEY UPDATE ticket_number=ticket_number;

-- Kiểm tra kết quả
SELECT 
    COUNT(*) as TotalPayments,
    SUM(amount) as TotalRevenue,
    YEAR(payment_date) as Year
FROM Payments 
WHERE status = 'SUCCESS'
GROUP BY YEAR(payment_date);

SELECT 'Đã tạo xong dữ liệu Payment test cho năm 2025!' as Message;
