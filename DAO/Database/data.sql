--   admin     → 8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918
--   staff123  → ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f
--   user123   → 397a6f558fbecb63c19f78146d3665bd41fda95e9800cf4fce4a28d8acc57aef

USE `FlightTicketManagement`;

-- 1. Tài khoản
INSERT INTO accounts (email, `password`, failed_attempts, is_active, created_at) VALUES
('admin',					'8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW()),
('staff@test.com',			'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 5, 1, NOW()),
('user@test.com',			'397a6f558fbecb63c19f78146d3665bd41fda95e9800cf4fce4a28d8acc57aef', 5, 1, NOW()),
('hongqui@sv.sgu.edu.vn',	'8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW()),
('phamnam@hotmail.com',		'8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW()),
('vophat@outlook.com.vn',	'8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW()),
('phuocnam@yahoo.com',		'8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW()),
('quangphong@gmail.com',	'8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW());

-- 2. Vai trò
INSERT INTO roles (role_code, role_name) VALUES
('ADMIN', 'Quản trị viên'),
('STAFF', 'Nhân viên'),
('USER',  'Người dùng');

-- 3. Tài khoản - Vai trò
INSERT INTO account_role (account_id, role_id) VALUES (1, 1) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (2, 2) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (3, 3) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (4, 1) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (5, 2) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (6, 3) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (7, 1) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (8, 2) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);

-- 4. Quyền
INSERT INTO permissions (permission_code, permission_name) VALUES
('home.view',             	'Xem trang chủ'),
('flights.read',          	'Tra cứu chuyến bay'),
('flights.create',        	'Tạo chuyến bay'),
('fare_rules.manage',     	'Quản lý quy tắc giá vé'),
('tickets.create_search', 	'Đặt chỗ'),
('tickets.mine',          	'Xem đặt chỗ của tôi'),
('tickets.operate',       	'Vận hành vé'),
('tickets.history',       	'Xem lịch sử vé'),
('baggage.checkin',       	'Check-in hành lý'),
('baggage.track',         	'Theo dõi hành lý'),
('baggage.report',        	'Báo cáo hành lý thất lạc'),
('catalogs.airlines',     	'Quản lý hãng hàng không'),
('catalogs.aircrafts',    	'Quản lý máy bay'),
('catalogs.airports',     	'Quản lý sân bay'),
('catalogs.routes',       	'Quản lý tuyến bay'),
('catalogs.cabin_classes',	'Quản lý hạng vé'),
('catalogs.seats',        	'Quản lý ghế máy bay'),
('payments.pos',          	'Thanh toán POS'),
('customers.profiles',    	'Hồ sơ khách hàng'),
('accounts.manage',       	'Quản lý tài khoản & phân quyền'),
('notifications.read',    	'Xem thông báo'),
('reports.view',          	'Xem báo cáo'),
('system.roles',          	'Cấu hình vai trò & phân quyền');

-- 5. Quyền - Vai trò
-- USER
INSERT IGNORE INTO role_permissions (role_id, permission_id)
SELECT r.role_id, p.permission_id
FROM roles r
JOIN permissions p ON p.permission_code IN (
    'home.view',
    'tickets.create_search',
    'tickets.mine',
    'customers.profiles',
    'notifications.read'
)
WHERE r.role_code = 'USER';

-- STAFF
INSERT IGNORE INTO role_permissions (role_id, permission_id)
SELECT r.role_id, p.permission_id
FROM roles r
JOIN permissions p ON p.permission_code IN (
    'home.view',
    'flights.read',
    'tickets.create_search',
    'tickets.operate',
    'tickets.history',
    'baggage.checkin',
    'baggage.track',
    'payments.pos',
    'customers.profiles',
    'notifications.read',
    'reports.view'
)
WHERE r.role_code = 'STAFF';

-- ADMIN (tất cả quyền)
INSERT IGNORE INTO role_permissions (role_id, permission_id)
SELECT r.role_id, p.permission_id
FROM roles r
JOIN permissions p
WHERE r.role_code = 'ADMIN';
-- *************************************************************************************** --

-- 5. Hãng hàng không
INSERT INTO `Airlines` (`airline_code`, `airline_name`, `country`) VALUES 
('VN', 'Vietnam Airlines', 	'Vietnam'),
('VJ', 'Vietjet Air', 		'Vietnam'),
('QH', 'Bamboo Airways', 	'Vietnam'),
('AA', 'American Airlines', 'American'),
('OZ', 'Asiana Airlines', 	'Korean');

-- 6. Sân bay
INSERT INTO `Airports` (`airport_code`, `airport_name`, `city`, `country`) VALUES 
('HAN', 'Noi Bai International Airport', 		'Hanoi', 				'Vietnam'),
('SGN', 'Tan Son Nhat International Airport', 	'Ho Chi Minh City', 	'Vietnam'),
('DAD', 'Da Nang International Airport', 		'Da Nang', 				'Vietnam'),
('YYJ', 'Vitoria International Airport', 		'Victoria', 			'Canada'),
('GMP', 'Gimpo International Airport', 			'Seoul', 				'Korean');

-- 7. Máy bay
INSERT INTO `Aircrafts` (`airline_id`, `model`, `manufacturer`, `capacity`) VALUES
(1, 'Airbus A350-900', 'Airbus', 305),
(1, 'Boeing 787-10', 'Boeing', 318),
(2, 'Airbus A321neo', 'Airbus', 244),
(2, 'Airbus A320neo', 'Airbus', 188),
(3, 'Embraer E195-E2', 'Embraer', 132),
(3, 'Boeing 787-9 Dreamliner', 'Boeing', 296),
(4, 'Boeing 737 MAX 8', 'Boeing', 178),
(4, 'Airbus A321XLR', 'Airbus', 220),
(5, 'Airbus A380-800', 'Airbus', 525),
(5, 'Boeing 777-200ER', 'Boeing', 315);

-- 8. Tuyến bay
INSERT INTO `Routes` (`departure_place_id`, `arrival_place_id`, `distance_km`, `duration_minutes`) VALUES 
(1, 2, 1802, 230),
(2, 1, 1689, 96),
(4, 5, 1858, 90),
(5, 4, 644, 228),
(2, 3, 812, 102),
(3, 4, 1882, 93),
(1, 5, 532, 265),
(1, 4, 633, 91),
(2, 5, 1574, 113),
(5, 4, 1794, 268);

-- 9. Chuyến bay
INSERT INTO `Flights` (`flight_number`, `aircraft_id`, `route_id`, `departure_time`, `arrival_time`, `status`) VALUES 
('QH772', 8,  4,  '2025-09-04 20:25:43', '2025-09-05 00:38:43', 'COMPLETED'),
('QH135', 9,  3,  '2024-12-24 05:46:41', '2024-12-24 08:19:41', 'COMPLETED'),
('VJ134', 10, 3,  '2025-12-14 12:34:52', '2025-12-14 16:24:52', 'CANCELLED'),
('BL984', 1,  6,  '2024-01-10 09:20:44', '2024-01-10 12:24:44', 'COMPLETED'),
('VJ487', 9,  5,  '2024-11-14 10:52:11', '2024-11-14 12:13:11', 'DELAYED'),
('QH851', 10, 1,  '2025-01-16 18:06:52', '2025-01-16 20:47:52', 'DELAYED'),
('QH800', 1,  7,  '2024-06-22 06:15:50', '2024-06-22 08:39:50', 'SCHEDULED'),
('VJ354', 10, 6,  '2025-03-10 12:53:29', '2025-03-10 15:46:29', 'DELAYED'),
('BL607', 5,  10, '2024-08-07 07:46:09', '2024-08-07 12:41:09', 'DELAYED'),
('BL529', 3,  5,  '2025-02-21 14:05:00', '2025-02-21 18:21:00', 'CANCELLED'),
('QH924', 6,  10, '2025-01-27 17:53:42', '2025-01-27 20:22:42', 'COMPLETED'),
('VJ164', 5,  3,  '2025-09-09 07:28:42', '2025-09-09 10:47:42', 'SCHEDULED'),
('VN182', 5,  8,  '2025-09-11 09:06:37', '2025-09-11 10:27:37', 'DELAYED'),
('VJ501', 4,  3,  '2024-07-07 19:01:29', '2024-07-07 21:19:29', 'COMPLETED'),
('BL813', 4,  2,  '2025-01-28 22:00:44', '2025-01-29 01:20:44', 'DELAYED'),
('BL768', 4,  2,  '2024-03-01 23:53:59', '2024-03-02 03:54:59', 'DELAYED'),
('QH933', 4,  3,  '2024-06-12 16:46:12', '2024-06-12 19:46:12', 'SCHEDULED'),
('VJ145', 7,  4,  '2025-07-13 11:50:14', '2025-07-13 15:23:14', 'COMPLETED'),
('VN133', 10, 4,  '2024-06-23 08:56:17', '2024-06-23 13:01:17', 'CANCELLED'),
('VJ307', 8,  7,  '2025-06-15 07:14:38', '2025-06-15 10:23:38', 'DELAYED');

-- 10. Hạng ghế
INSERT INTO `Cabin_Classes` (`class_name`, `description`) VALUES 
('First', 'First class description'),
('Business', 'Business class description'),
('Premium Economy', 'Premium Economy class description'),
('Economy', 'Economy class description');

-- 11. Ghế cho trên mỗi máy bay
INSERT INTO `Seats` (`aircraft_id`, `seat_number`, `class_id`) VALUES
(1, '1A', 1),  (1, '1B', 1),
(1, '2A', 1),  (1, '2B', 1),
(1, '3A', 2),  (1, '3B', 2),
(1, '4A', 2),  (1, '4B', 2),
(1, '5A', 2),  (1, '5B', 2),
(1, '6A', 2),  (1, '6B', 2),
(1, '7A', 3),  (1, '7B', 3),
(1, '8A', 3),  (1, '8B', 3),
(1, '9A', 3),  (1, '9B', 3),
(1, '10A', 4), (1, '10B', 4),
-- ------------------------------------------------------
(2, '1A', 1),  (2, '1B', 1), (2, '1C', 1),  (2, '1D', 1),
(2, '2A', 2),  (2, '2B', 2), (2, '2C', 2),  (2, '2D', 2),
(2, '3A', 2),  (2, '3B', 2), (2, '3C', 2),  (2, '3D', 2),
(2, '4A', 3),  (2, '4B', 3), (2, '4C', 3),  (2, '4D', 3),
(2, '5A', 3),  (2, '5B', 3), (2, '5C', 4),  (2, '5D', 4),
-- ------------------------------------------------------
(3, '1A', 1), (3, '1B', 1), (3, '1C', 1), (3, '1D', 1),
(3, '2A', 2), (3, '2B', 2), (3, '2C', 2), (3, '2D', 2),
(3, '3A', 2), (3, '3B', 2), (3, '3C', 2), (3, '3D', 2),
(3, '4A', 3), (3, '4B', 3), (3, '4C', 3), (3, '4D', 3),
(3, '5A', 4), (3, '5B', 4), (3, '5C', 4), (3, '5D', 4),
(3, '6A', 4), (3, '6B', 4), (3, '6C', 4), (3, '6D', 4),
-- ------------------------------------------------------
(4, '1A', 1),(4,'1B',1),(4,'1C',1),(4,'1D',1),
(4, '2A', 1),(4,'2B',1),(4,'2C',1),(4,'2D',1),
(4, '5A',2),(4,'5B',2),(4,'5C',2),(4,'5D',2),(4,'5E',2),(4,'5F',2),
(4, '6A',2),(4,'6B',2),(4,'6C',2),(4,'6D',2),(4,'6E',2),(4,'6F',2),
(4, '10A',3),(4,'10B',3),(4,'10C',3),(4,'10D',3),(4,'10E',3),(4,'10F',3),
(4, '20A',4),(4,'20B',4),(4,'20C',4),(4,'20D',4),(4,'20E',4),(4,'20F',4),
(4, '21A',4),(4,'21B',4),(4,'21C',4),(4,'21D',4),(4,'21E',4),(4,'21F',4),
-- ------------------------------------------------------------------------
(5,'1A',1),(5,'1B',1),
(5,'2A',1),(5,'2B',1),
(5,'5A',2),(5,'5B',2),(5,'5C',2),
(5,'6A',2),(5,'6B',2),(5,'6C',2),
(5,'10A',3),(5,'10B',3),(5,'10C',3),(5,'10D',3),
(5,'20A',4),(5,'20B',4),(5,'20C',4),(5,'20D',4),(5,'20E',4),(5,'20F',4),
(5,'21A',4),(5,'21B',4),(5,'21C',4),(5,'21D',4),(5,'21E',4),(5,'21F',4),
-- ------------------------------------------------------------------------
(6,'1A',1),(6,'1B',1),(6,'1C',1),
(6,'2A',1),(6,'2B',1),(6,'2C',1),
(6,'5A',2),(6,'5B',2),(6,'5C',2),(6,'5D',2),
(6,'6A',2),(6,'6B',2),(6,'6C',2),(6,'6D',2),
(6,'10A',3),(6,'10B',3),(6,'10C',3),(6,'10D',3),(6,'10E',3),
(6,'30A',4),(6,'30B',4),(6,'30C',4),(6,'30D',4),(6,'30E',4),(6,'30F',4),
(6,'31A',4),(6,'31B',4),(6,'31C',4),(6,'31D',4),(6,'31E',4),(6,'31F',4),
-- ------------------------------------------------------------------------
(7,'1A',1),(7,'1B',1),
(7,'2A',1),(7,'2B',1),
(7,'10A',2),(7,'10B',2),(7,'10C',2),(7,'10D',2),
(7,'11A',2),(7,'11B',2),(7,'11C',2),(7,'11D',2),
(7,'25A',3),(7,'25B',3),(7,'25C',3),(7,'25D',3),
(7,'50A',4),(7,'50B',4),(7,'50C',4),(7,'50D',4),(7,'50E',4),(7,'50F',4),
(7,'51A',4),(7,'51B',4),(7,'51C',4),(7,'51D',4),(7,'51E',4),(7,'51F',4),
-- -----------------------------------------------------------------------
(8,'1A',1),(8,'1B',1),
(8,'2A',2),(8,'2B',2),
(8,'5A',3),(8,'5B',3),(8,'5C',3),
(8,'10A',4),(8,'10B',4),(8,'10C',4),(8,'10D',4),
(8,'11A',4),(8,'11B',4),(8,'11C',4),(8,'11D',4),
-- ----------------------------------------------
(9,'1A',1),(9,'1B',1),
(9,'2A',2),(9,'2B',2),
(9,'3A',3),(9,'3B',3),
(9,'4A',4),(9,'4B',4),
-- --------------------
(10,'1A',1),(10,'1B',1),(10,'1C',1),
(10,'5A',2),(10,'5B',2),(10,'5C',2),(10,'5D',2),
(10,'12A',3),(10,'12B',3),(10,'12C',3),(10,'12D',3),
(10,'25A',4),(10,'25B',4),(10,'25C',4),(10,'25D',4),(10,'25E',4),(10,'25F',4),
(10,'26A',4),(10,'26B',4),(10,'26C',4),(10,'26D',4),(10,'26E',4),(10,'26F',4);

-- 12. Ghế cho từng chuyến bay
INSERT INTO `Flight_Seats` (`flight_id`, `seat_id`, `base_price`, `seat_status`) VALUES
(2, 203, 900,  'AVAILABLE'),
(2, 204, 900,  'BOOKED'),
(2, 205, 550,  'BLOCKED'),
(2, 206, 550,  'AVAILABLE'),
(2, 207, 350,  'BOOKED'),
(2, 208, 350,  'BLOCKED'),
(2, 209, 200,   'AVAILABLE'),
(2, 210, 200,   'BOOKED'),
-- ---------------------------------
(4,  1,   900,  'AVAILABLE'),
(4,  4,   900,  'AVAILABLE'),
(4,  5,   550,  'AVAILABLE'),
(4,  8,   550,  'AVAILABLE'),
(4,  9,   550,  'AVAILABLE'), 
(4,  12,  550,  'AVAILABLE'),
(4,  13,  350,  'AVAILABLE'),
(4,  16,  350,  'AVAILABLE'),
(4,  17,  350,  'AVAILABLE'),
(4,  19,  200,   'AVAILABLE'),
-- ------------------------------
(7,  160,  900,  'AVAILABLE'),
(7,  161,  900,  'BOOKED'),
(7,  162,  900,  'BLOCKED'),
(7,  163,  900,  'AVAILABLE'),
(7,  164,  550,  'BOOKED'),
(7,  166,  550,  'AVAILABLE'),
(7,  167,  550,  'BLOCKED'),
(7,  168,  550,  'AVAILABLE'),
(7,  170,  550,  'BOOKED'),
(7,  171,  550,  'AVAILABLE'),
(7,  172,  350,  'BLOCKED'),
(7,  174,  350,  'AVAILABLE'),
(7,  175,  350,  'BOOKED'),
(7,  176,  200,   'AVAILABLE'),
(7,  178,  200,   'BOOKED'),
(7,  180,  200,	 'AVAILABLE'),
(7,  181,  200,	 'BLOCKED'),
(7,  182,  200,	 'AVAILABLE'),
(7,  184,  200,	 'BOOKED'),
(7,  186,  200,	 'AVAILABLE'),
-- ----------------------------------
(11, 129, 900, 'AVAILABLE'),
(11, 131, 900, 'BOOKED'),
(11, 133, 900, 'BLOCKED'),
(11, 134, 900, 'AVAILABLE'),
(11, 135, 550, 'BOOKED'),
(11, 137, 550, 'AVAILABLE'),
(11, 138, 550, 'BLOCKED'),
(11, 139, 550, 'AVAILABLE'),
(11, 141, 550, 'BOOKED'),
(11, 143, 350, 'AVAILABLE'),
(11, 145, 350, 'BOOKED'),
(11, 147, 350, 'BLOCKED'),
(11, 149, 200,  'AVAILABLE'),
(11, 152, 200,  'BOOKED'),
(11, 156, 200,  'BLOCKED'),
-- --------------------------
(18, 21, 900, 'AVAILABLE'),
(18, 24, 900, 'BOOKED'),   
(18, 26, 550, 'BLOCKED'), 
(18, 27, 550, 'AVAILABLE'), 
(18, 29, 550, 'BOOKED'),
(18, 32, 550, 'AVAILABLE'),
(18, 33, 350, 'BLOCKED'), 
(18, 35, 350, 'AVAILABLE'), 
(18, 37, 350, 'BOOKED'), 
(18, 39, 200,  'AVAILABLE');
-- ----------------------------

-- 13. Quy tắc gía vé
INSERT INTO `Fare_Rules` (`route_id`,`class_id`,`fare_type`,`season`,`effective_date`,`expiry_date`,`description`,`price`) VALUES
(1,	 1,	'Standard',	'PEAK',		'2025-01-01',	'2025-12-31',	'Peak First Class Fare',	 	900.00),
(1,	 4,	'Saver',	'OFFPEAK',	'2025-02-01',	'2025-06-30',	'Discount Economy Fare',	 	200.00),
(2,	 2,	'Flex',		'NORMAL',	'2025-03-01',	'2025-12-31', 	'Flexible Business Fare',	 	550.00),
(3,	 3,	'Standard',	'PEAK',		'2025-01-15',	'2025-05-15',	'Premium Eco Peak Season',	 	350.00),
(4,	 4,	'Promo',	'NORMAL',	'2025-04-01',	'2025-09-30',	'Promo Economy Ticket',		 	200.00),
(5,	 1,	'Flex',		'PEAK',		'2025-06-01',	'2025-12-31',	'Flexible First Class Fare', 	900.00),
(6,	 2,	'Saver',	'OFFPEAK',	'2025-02-10',	'2025-04-20',	'Saver Business Class',		 	550.00),
(7,	 3,	'Standard',	'NORMAL',	'2025-01-01',	'2025-10-31',	'Standard Premium Eco',		 	350.00),
(8,	 4,	'Flex',		'PEAK',		'2025-03-15',	'2025-12-31',	'Flexible Economy Summer Fare',	200.00),
(9,	 1,	'Promo',	'OFFPEAK',	'2025-01-10',	'2025-04-30',	'First Class Promo Winter',		900.00),
(10, 2,	'Standard',	'NORMAL',	'2025-05-01',	'2025-12-01',	'Business Standard Ticket',		550.00),
(2,	 3,	'Flex',		'PEAK',		'2025-01-20',	'2025-11-30',	'Premium Eco Flexible Fare',	350.00),
(3,  4,	'Saver',	'NORMAL',	'2025-02-01',	'2025-10-01',	'Saver Economy Regular',		200.00),
(6,  1,	'Standard',	'PEAK',		'2025-06-10',   '2025-09-10',	'Peak First Class Route 6',		900.00),
(7,  2,	'Promo',	'OFFPEAK',	'2025-01-05',	'2025-03-31',	'Business Promo Fare',			550.00);

-- 14. Đặt chỗ
INSERT INTO `Bookings` (account_id, booking_date, trip_type, status, total_amount) VALUES
(4, '2024-11-05 10:22:11', 'ONE_WAY', 	 'CONFIRMED',   350.00),
(5, '2025-01-14 08:15:33', 'ROUND_TRIP', 'PENDING', 	720.00),
(6, '2024-12-22 19:41:02', 'ONE_WAY', 	 'CANCELLED', 	200.00),
(7, '2025-02-03 14:05:47', 'MULTI_WAY',  'CONFIRMED', 	1120.00),
(8, '2024-10-29 09:55:20', 'ROUND_TRIP', 'REFUNDED', 	650.00),
(4, '2025-03-11 17:22:59', 'ONE_WAY', 	 'CONFIRMED', 	450.00),
(5, '2024-09-13 11:02:16', 'MULTI_WAY',  'PENDING', 	900.00),
(6, '2025-04-20 20:14:37', 'ONE_WAY', 	 'CONFIRMED', 	200.00),
(7, '2024-08-18 06:45:12', 'ROUND_TRIP', 'CANCELLED', 	740.00),
(8, '2025-02-27 15:39:29', 'ONE_WAY', 	 'CONFIRMED', 	300.00),
(4, '2024-07-09 13:26:51', 'MULTI_WAY',  'PENDING', 	1050.00),
(5, '2025-01-25 18:33:44', 'ROUND_TRIP', 'CONFIRMED', 	880.00),
(6, '2024-06-30 21:10:18', 'ONE_WAY', 	 'REFUNDED', 	220.00),
(7, '2025-03-02 12:49:06', 'MULTI_WAY',  'CONFIRMED', 	990.00),
(8, '2024-09-21 08:12:57', 'ONE_WAY', 	 'PENDING', 	250.00);

-- 15. Hồ sơ khách hàng
INSERT INTO `Passenger_Profiles`
(account_id, full_name, date_of_birth, phone_number, passport_number, nationality) VALUES
(1, 'Administrator',          '1990-01-01', '0901234567', 'C1234567', 'Vietnam'),
(2, 'Staff Test',             '1995-04-12', '0912345678', 'C2345678', 'Vietnam'),
(3, 'User Test',              '1998-09-20', '0938765432', 'C3456789', 'Vietnam'),
(4, 'Hồng Quí',               '2003-05-17', '0981122334', 'C4567891', 'Vietnam'),
(5, 'Phạm Nam',               '1999-11-02', '0974455667', 'C5678912', 'Vietnam'),
(6, 'Võ Phát',                '2002-03-28', '0967788990', 'C6789123', 'Vietnam'),
(7, 'Phước Nam',              '1997-07-08', '0945566778', 'C7891234', 'Vietnam'),
(8, 'Quang Phong',            '2001-12-25', '0933344556', 'C8912345', 'Vietnam');

-- 16. Hành khách - Đặt chỗ
INSERT INTO Booking_Passengers (booking_id, profile_id) VALUES
(1, 4),
(2, 5),
(3, 6),
(4, 7),
(4, 8),
(5, 8),
(6, 4),
(7, 5),
(7, 6),
(8, 6),
(9, 7),
(10, 8),
(11, 4),
(11, 5),
(12, 5),
(13, 6),
(14, 7),
(14, 8),
(15, 8),
(15, 4);

-- 17. Vé
INSERT INTO Tickets (ticket_passenger_id, flight_seat_id, ticket_number, segment_no, segment_type, `status`) VALUES
(1,  1,   'TK202500001', 1, 'OUTBOUND', 'CONFIRMED'),
(2,  2,   'TK202500002', 1, 'OUTBOUND', 'BOOKED'),
(3,  3,   'TK202500003', 1, 'OUTBOUND', 'BOOKED'),
(4,  4,   'TK202500004', 1, 'OUTBOUND', 'CONFIRMED'),
(5,  5,   'TK202500005', 1, 'OUTBOUND', 'BOOKED'),
(6,  10,  'TK202500006', 1, 'OUTBOUND', 'BOOKED'),
(7,  12,  'TK202500007', 1, 'OUTBOUND', 'CONFIRMED'),
(8,  14,  'TK202500008', 1, 'OUTBOUND', 'BOOKED'),
(9,  16,  'TK202500009', 1, 'OUTBOUND', 'BOOKED'),
(10, 18,  'TK202500010', 1, 'OUTBOUND', 'CONFIRMED'),
(11, 25,  'TK202500011', 1, 'OUTBOUND', 'BOOKED'),
(12, 26,  'TK202500012', 1, 'OUTBOUND', 'BOOKED'),
(13, 28,  'TK202500013', 1, 'OUTBOUND', 'CHECKED_IN'),
(14, 30,  'TK202500014', 1, 'OUTBOUND', 'CONFIRMED'),
(15, 32,  'TK202500015', 1, 'OUTBOUND', 'BOOKED'),
(16, 40,  'TK202500016', 1, 'OUTBOUND', 'BOARDED'),
(17, 42,  'TK202500017', 1, 'OUTBOUND', 'CONFIRMED'),
(18, 45,  'TK202500018', 1, 'OUTBOUND', 'BOOKED'),
(19, 48,  'TK202500019', 1, 'OUTBOUND', 'CHECKED_IN'),
(20, 50,  'TK202500020', 1, 'OUTBOUND', 'BOOKED');

-- 18. Lịch sử đặt vé
INSERT INTO Ticket_History (ticket_id, old_status, new_status, changed_at) VALUES
(1,  'BOOKED', 		'CONFIRMED', 	'2025-01-02 09:12:00'),
(2,  'BOOKED', 		'CONFIRMED', 	'2025-01-05 10:20:00'),
(2,  'CONFIRMED', 	'CHECKED_IN', 	'2025-01-06 08:15:00'),
(3,  'BOOKED', 		'CANCELLED', 	'2025-01-03 11:44:00'),
(4,  'BOOKED', 		'CONFIRMED', 	'2025-02-01 15:30:00'),
(5,  'BOOKED', 		'CONFIRMED', 	'2025-02-10 18:05:00'),
(6,  'BOOKED', 		'CONFIRMED', 	'2025-01-12 16:20:00'),
(6,  'CONFIRMED', 	'CHECKED_IN', 	'2025-01-13 06:55:00'),
(7,  'BOOKED', 		'CONFIRMED', 	'2025-03-03 09:55:00'),
(8,  'BOOKED', 		'CONFIRMED', 	'2025-03-10 13:44:00'),
(8,  'CONFIRMED', 	'CANCELLED', 	'2025-03-11 07:22:00'),
(9,  'BOOKED', 		'CONFIRMED', 	'2025-03-21 12:00:00'),
(9,  'CONFIRMED', 	'CHECKED_IN', 	'2025-03-22 08:45:00'),
(10, 'BOOKED', 		'CONFIRMED', 	'2025-04-01 19:30:00'),
(11, 'BOOKED', 		'CONFIRMED', 	'2025-04-11 09:14:00'),
(11, 'CONFIRMED', 	'CHECKED_IN', 	'2025-04-12 05:30:00'),
(11, 'CHECKED_IN', 	'BOARDED', 		'2025-04-12 07:10:00'),
(12, 'BOOKED', 		'CONFIRMED', 	'2025-05-02 11:50:00'),
(13, 'BOOKED', 		'CONFIRMED', 	'2025-05-06 15:10:00'),
(14, 'BOOKED', 		'CONFIRMED', 	'2025-05-14 20:55:00'),
(14, 'CONFIRMED', 	'CHECKED_IN', 	'2025-05-15 06:30:00'),
(15, 'BOOKED', 		'CANCELLED', 	'2025-05-03 14:15:00'),
(16, 'BOOKED', 		'CONFIRMED', 	'2025-06-01 10:25:00'),
(16, 'CONFIRMED', 	'CHECKED_IN', 	'2025-06-01 18:40:00'),
(16, 'CHECKED_IN', 	'BOARDED', 		'2025-06-02 06:00:00'),
(17, 'BOOKED', 		'CONFIRMED', 	'2025-06-12 09:00:00'),
(18, 'BOOKED', 		'CONFIRMED', 	'2025-06-20 13:33:00'),
(18, 'CONFIRMED', 	'REFUNDED', 	'2025-06-21 09:40:00'),
(19, 'BOOKED', 		'CONFIRMED', 	'2025-06-25 08:44:00'),
(19, 'CONFIRMED', 	'CHECKED_IN', 	'2025-06-26 05:22:00'),
(20, 'BOOKED', 		'REFUNDED', 	'2025-07-01 14:20:00');

-- 19. Thanh toán
INSERT INTO Payments (booking_id, amount, payment_method, payment_date, `status`) VALUES
(1,   350.00,  'CREDIT_CARD',   '2024-11-05 11:00:00', 'SUCCESS'),
(2,   720.00,  'E_WALLET',      '2025-01-14 08:30:00', 'SUCCESS'),
(3,   200.00,  'CASH',          '2024-12-22 20:00:00', 'FAILED'),
(4,   1120.00, 'BANK_TRANSFER', '2025-02-03 15:10:00', 'SUCCESS'),
(5,   650.00,  'CREDIT_CARD',   '2024-10-29 10:10:00', 'SUCCESS'),
(6,   450.00,  'CASH',          '2025-03-11 18:00:00', 'SUCCESS'),
(7,   900.00,  'BANK_TRANSFER', '2024-09-13 12:00:00', 'PENDING'),
(8,   200.00,  'E_WALLET',      '2025-04-20 20:30:00', 'SUCCESS'),
(9,   740.00,  'CREDIT_CARD',   '2024-08-18 07:10:00', 'SUCCESS'),
(10,  300.00,  'CASH',          '2025-02-27 16:00:00', 'SUCCESS'),
(11,  1050.00, 'E_WALLET',      '2024-07-09 14:00:00', 'SUCCESS'),
(12,  880.00,  'CREDIT_CARD',   '2025-01-25 18:45:00', 'SUCCESS'),
(13,  220.00,  'BANK_TRANSFER', '2024-06-30 21:30:00', 'FAILED'),
(14,  990.00,  'E_WALLET',      '2025-03-02 13:00:00', 'SUCCESS'),
(15,  250.00,  'CREDIT_CARD',   '2024-09-21 09:00:00', 'FAILED');

-- 20. Hành lý
INSERT INTO Baggage (ticket_id, flight_id, baggage_tag, baggage_type, weight_kg, allowed_weight_kg, fee, `status`, special_handling) VALUES
(1,  2,  'BG10001', 'CHECKED',   18.0,  20.0,  0.00,    'CHECKED_IN',  NULL),
(2,  2,  'BG10002', 'CHECKED',   22.5,  20.0,  37.50,   'LOADED',      NULL),
(3,  2,  'BG10003', 'CARRY_ON',  8.0,   10.0,  0.00,    'CHECKED_IN',  NULL),
(4,  4,  'BG10004', 'CHECKED',   28.0,  30.0,  0.00,    'LOADED',      'FRAGILE'),
(5,  4,  'BG10005', 'SPECIAL',   32.0,  25.0,  105.00,  'CHECKED_IN',  'SPORT_EQUIPMENT'),
(6,  4,  'BG10006', 'CHECKED',   19.0,  20.0,  0.00,    'CHECKED_IN',   NULL),
(7,  4,  'BG10007', 'CHECKED',   34.0,  35.0,  0.00,    'IN_TRANSIT',   'PRIORITY'),
(8,  4,  'BG10008', 'CARRY_ON',  7.5,   10.0,  0.00,    'CHECKED_IN',   NULL),
(9,  7,  'BG10009', 'CHECKED',   21.0,  20.0,  15.00,   'LOADED', 		NULL),
(10, 7,  'BG10010', 'CHECKED',   19.0,  20.0,  0.00,    'CHECKED_IN',   NULL),
(11, 7,  'BG10011', 'SPECIAL',   29.5,  25.0,  67.50,   'IN_TRANSIT',   'MUSICAL_INSTRUMENT'),
(12, 7,  'BG10012', 'CHECKED',   24.0,  25.0,  0.00,    'CHECKED_IN',   NULL),
(13, 11, 'BG10013', 'CHECKED',   28.0,  30.0,  0.00,    'LOADED',       NULL),
(14, 11, 'BG10014', 'CHECKED',   20.5,  20.0,  7.50,    'IN_TRANSIT',   NULL),
(15, 18, 'BG10015', 'CARRY_ON',  9.0,   10.0,  0.00,    'CHECKED_IN',   NULL),
(16, 18, 'BG10016', 'CHECKED',   27.0,  25.0,  30.00,   'LOADED',       NULL),
(17, 18, 'BG10017', 'CHECKED',   18.0,  20.0,  0.00,    'CLAIMED', 		NULL),
(18, 18, 'BG10018', 'SPECIAL',   33.0,  25.0,  120.00,  'LOST', 		'SPORT_EQUIPMENT');

-- 21. Lịch sử trạng thái hành lý
INSERT INTO Baggage_History (baggage_id, old_status, new_status, changed_at) VALUES
(1,  'CREATED',		'CHECKED_IN',	'2025-01-02 08:00:00'),
(2,  'CREATED',		'CHECKED_IN',	'2025-01-05 09:20:00'),
(2,  'CHECKED_IN',	'LOADED',		'2025-01-05 10:00:00'),
(3,  'CREATED',		'CHECKED_IN',	'2025-01-03 11:00:00'),
(4,  'CREATED',		'CHECKED_IN',	'2025-02-01 13:40:00'),
(4,  'CHECKED_IN',	'LOADED',		'2025-02-01 15:10:00'),
(5,  'CREATED',		'CHECKED_IN',	'2025-02-10 17:00:00'),
(6,  'CREATED',		'CHECKED_IN',	'2025-03-11 17:30:00'),
(7,  'CREATED',		'CHECKED_IN',	'2025-03-21 08:00:00'),
(7,  'CHECKED_IN',	'LOADED',		'2025-03-21 09:30:00'),
(7,  'LOADED',		'IN_TRANSIT',	'2025-03-21 12:00:00'),
(8,  'CREATED',		'CHECKED_IN',	'2025-03-10 13:00:00'),
(9,  'CREATED',		'CHECKED_IN',	'2025-03-21 10:00:00'),
(9,  'CHECKED_IN',	'LOADED',		'2025-03-21 12:45:00'),
(10, 'CREATED',		'CHECKED_IN',	'2025-02-27 15:00:00'),
(11, 'CREATED',		'CHECKED_IN',	'2025-04-12 05:00:00'),
(11, 'CHECKED_IN',	'IN_TRANSIT',	'2025-04-12 06:45:00'),
(12, 'CREATED',		'CHECKED_IN',	'2025-03-02 10:00:00'),
(13, 'CREATED',		'CHECKED_IN',	'2025-04-11 08:30:00'),
(13, 'CHECKED_IN',	'LOADED',		'2025-04-11 10:10:00'),
(14, 'CREATED',		'CHECKED_IN',	'2025-05-14 19:50:00'),
(14, 'CHECKED_IN',	'IN_TRANSIT',	'2025-05-15 06:10:00'),
(15, 'CREATED',		'CHECKED_IN',	'2025-06-01 12:00:00'),
(16, 'CREATED',		'CHECKED_IN',	'2025-06-02 18:00:00'),
(16, 'CHECKED_IN',	'LOADED',		'2025-06-02 19:30:00'),
(17, 'CREATED',		'CHECKED_IN',	'2025-06-12 07:20:00'),
(17, 'CHECKED_IN',	'CLAIMED',		'2025-06-12 09:10:00'),
(18, 'CREATED',		'CHECKED_IN',	'2025-06-20 13:10:00'),
(18, 'CHECKED_IN',  'LOST',			'2025-06-21 08:00:00');

-- 22. Hành lý xách tay
INSERT INTO carryon_baggage (weight_kg, class_id, size_limit, `description`, is_default) VALUES
(14, 1, '56x36x23 cm', 'Xách tay mặc định First Class 14kg', 	 1),
(10, 2, '56x36x23 cm', 'Xách tay mặc định Business 10kg', 		 1),
(10, 3, '56x36x23 cm', 'Xách tay mặc định Premium Economy 10kg', 1),
(7,  4, '56x36x23 cm', 'Xách tay mặc định Economy 7kg', 		 1),
(3,  4, '40x30x15 cm', 'Túi cá nhân nhỏ 3kg', 					 0),
(5,  4, '45x35x20 cm', 'Balô nhỏ 5kg',     						 0);

-- 23. Kiểm tra hành lý
INSERT INTO checked_baggage (weight_kg, price, `description`) VALUES
(10, 200000, 'Gói ký gửi 10kg – tiêu chuẩn'),
(15, 320000, 'Gói ký gửi 15kg'),
(20, 450000, 'Gói ký gửi 20kg – phổ biến nhất'),
(25, 580000, 'Gói ký gửi 25kg'),
(30, 720000, 'Gói ký gửi 30kg – hành lý nhiều'),
(35, 850000, 'Gói ký gửi 35kg'),
(40, 990000, 'Gói ký gửi 40kg – tối đa theo quy định');

-- 24. Quốc gia
INSERT INTO `national` (country_name, country_code, phone_code) VALUES
('United States', 	'US',  '+1'),
('Japan', 			'JP',  '+81'),
('South Korea', 	'KR',  '+82'),
('France', 			'FR',  '+33'),
('Vietnam', 		'VN',  '+84');

-- 25. Vé - Hành lý
INSERT INTO `ticket_baggage` (`id`, `ticket_id`, `baggage_type`, `carryon_id`, `checked_id`, `quantity`, `note`) VALUES
(25, 1, 'carry_on',  1, 	NULL,  1,  'Xách tay mặc định 7kg'),
(26, 1, 'carry_on',  3, 	NULL,  1,  'Túi cá nhân nhỏ 3kg'),
(27, 1, 'checked',   NULL, 	3,     1,  'Khách mua thêm 20kg'),
(28, 2, 'carry_on',  1, 	NULL,  1,  'Xách tay mặc định 7kg'),
(29, 2, 'checked',   NULL, 	4, 	   1,  'Khách mua 25kg'),
(30, 3, 'carry_on',  2, 	NULL,  1,  'Hành lý xách tay 10kg – hạng thương gia'),
(31, 3, 'checked',   NULL, 	5,     1,  'Mua 30kg ký gửi'),
(32, 4, 'carry_on',  1, 	NULL,  1,  '7kg xách tay'),
(33, 4, 'carry_on',  3, 	NULL,  1,  'Túi cá nhân 3kg'),
(34, 5, 'carry_on',  1, 	NULL,  1,  'Xách tay 7kg'),
(35, 5, 'checked',   NULL,  2,     1,  'Mua 15kg'),
(36, 5, 'checked',   NULL,  3,     1,  'Mua thêm 20kg'),
(37, 1, 'carry_on',  1, 	NULL,  1,  'Xách tay 1 kg');