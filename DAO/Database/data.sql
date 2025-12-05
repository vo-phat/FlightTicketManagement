--   admin     → 8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918
--   staff123  → ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f
--   user123   → 397a6f558fbecb63c19f78146d3665bd41fda95e9800cf4fce4a28d8acc57aef

USE `FlightTicketManagement`;

-- 1. Tài khoản
INSERT INTO accounts (email, `password`, failed_attempts, is_active, created_at) VALUES
('admin',					'8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW()),
('staff@test.com',			'8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW()),
('user@test.com',			'8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, NOW()),
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
(1, 'Airbus A350-900', 'Airbus', 16),
(1, 'Boeing 787-10', 'Boeing', 16),
(2, 'Airbus A321neo', 'Airbus', 16),
(2, 'Airbus A320neo', 'Airbus', 16),
(3, 'Embraer E195-E2', 'Embraer', 16),
(3, 'Boeing 787-9 Dreamliner', 'Boeing', 16),
(4, 'Boeing 737 MAX 8', 'Boeing', 16),
(4, 'Airbus A321XLR', 'Airbus', 16),
(5, 'Airbus A380-800', 'Airbus', 16),
(5, 'Boeing 777-200ER', 'Boeing', 16);

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
INSERT INTO `Flights` (`flight_number`, `aircraft_id`, `route_id`, `departure_time`, `arrival_time`, `base_price`, `note`, `status`, `is_deleted`) VALUES
('QH772', 8,  4,  '2025-12-15 20:25:43', '2025-12-16 00:38:43', 1200000, 'Chuyến bay quốc tế', 'SCHEDULED', FALSE),
('QH135', 9,  3,  '2025-11-28 05:46:41', '2025-11-28 08:19:41', 850000, NULL, 'COMPLETED', FALSE),
('VJ134', 10,  3,  '2025-11-25 12:34:52', '2025-11-25 16:24:52', 920000, 'Chuyến bay bị hủy do thời tiết', 'CANCELLED', FALSE),
('BL984', 1,  6,  '2025-12-18 09:20:44', '2025-12-18 12:24:44', 1500000, NULL, 'SCHEDULED', FALSE),
('VJ487', 9,  5,  '2025-12-03 10:52:11', '2025-12-03 12:13:11', 780000, 'Chuyến bay nội địa', 'DELAYED', FALSE),
('QH851', 10,  1,  '2025-12-02 18:06:52', '2025-12-02 20:47:52', 950000, NULL, 'DELAYED', FALSE),
('QH800', 1,  7,  '2025-12-02 06:15:50', '2025-12-02 08:39:50', 1100000, NULL, 'COMPLETED', FALSE),
('VJ354', 10,  6,  '2025-11-29 12:53:29', '2025-11-29 15:46:29', 1350000, 'Chuyến bay quốc tế', 'DELAYED', FALSE),
('BL607', 5,  10, '2025-12-18 07:46:09', '2025-12-18 12:41:09', 2100000, 'Chuyến bay dài', 'DELAYED', FALSE),
('BL529', 3,  5,  '2025-11-28 14:05:00', '2025-11-28 18:21:00', 880000, NULL, 'CANCELLED', FALSE),
('QH924', 6,  10, '2025-12-16 17:53:42', '2025-12-16 20:22:42', 1950000, NULL, 'SCHEDULED', FALSE),
('VJ164', 5,  3,  '2025-12-18 07:28:42', '2025-12-18 10:47:42', 820000, NULL, 'SCHEDULED', FALSE),
('VN182', 5,  8,  '2025-12-23 09:06:37', '2025-12-23 10:27:37', 750000, 'Chuyến bay nội địa', 'DELAYED', FALSE),
('VJ501', 4,  3,  '2025-12-12 19:01:29', '2025-12-12 21:19:29', 890000, NULL, 'SCHEDULED', FALSE),
('BL813', 4,  2,  '2025-11-27 22:00:44', '2025-11-28 01:20:44', 1250000, NULL, 'DELAYED', FALSE),
('BL768', 4,  2,  '2025-12-13 23:53:59', '2025-12-14 03:54:59', 1180000, NULL, 'DELAYED', FALSE),
('QH933', 4,  3,  '2025-12-08 16:46:12', '2025-12-08 19:46:12', 870000, NULL, 'SCHEDULED', FALSE),
('VJ145', 7,  4,  '2025-11-26 11:50:14', '2025-11-26 15:23:14', 1450000, 'Chuyến bay quốc tế', 'COMPLETED', FALSE),
('VN133', 10, 4,  '2025-11-25 08:56:17', '2025-11-25 13:01:17', 1320000, NULL, 'CANCELLED', FALSE),
('VJ307', 8,  7,  '2025-11-27 07:14:38', '2025-11-27 10:23:38', 1080000, NULL, 'DELAYED', FALSE);

-- 10. Hạng ghế
INSERT INTO `Cabin_Classes` (`class_name`, `description`) VALUES 
('First', 'First class description'),
('Business', 'Business class description'),
('Premium Economy', 'Premium Economy class description'),
('Economy', 'Economy class description');

-- 11. Ghế cho trên mỗi máy bay
INSERT INTO `Seats` (`seat_id`, `aircraft_id`, `seat_number`, `class_id`) VALUES
-- Aircraft 1 (seat_id: 1-16)
(1, 1, '1A', 1), (2, 1, '1B', 1), (3, 1, '1C', 1), (4, 1, '1D', 1),
(5, 1, '1E', 2), (6, 1, '1F', 2), (7, 1, '1G', 2), (8, 1, '1H', 2),
(9, 1, '2A', 3), (10, 1, '2B', 3), (11, 1, '2C', 3), (12, 1, '2D', 3),
(13, 1, '2E', 4), (14, 1, '2F', 4), (15, 1, '2G', 4), (16, 1, '2H', 4),

-- Aircraft 2 (seat_id: 17-32)
(17, 2, '1A', 1), (18, 2, '1B', 1), (19, 2, '1C', 1), (20, 2, '1D', 1),
(21, 2, '1E', 2), (22, 2, '1F', 2), (23, 2, '1G', 2), (24, 2, '1H', 2),
(25, 2, '2A', 3), (26, 2, '2B', 3), (27, 2, '2C', 3), (28, 2, '2D', 3),
(29, 2, '2E', 4), (30, 2, '2F', 4), (31, 2, '2G', 4), (32, 2, '2H', 4),

-- Aircraft 3 (seat_id: 33-48)
(33, 3, '1A', 1), (34, 3, '1B', 1), (35, 3, '1C', 1), (36, 3, '1D', 1),
(37, 3, '1E', 2), (38, 3, '1F', 2), (39, 3, '1G', 2), (40, 3, '1H', 2),
(41, 3, '2A', 3), (42, 3, '2B', 3), (43, 3, '2C', 3), (44, 3, '2D', 3),
(45, 3, '2E', 4), (46, 3, '2F', 4), (47, 3, '2G', 4), (48, 3, '2H', 4),

-- Aircraft 4 (seat_id: 49-64)
(49, 4, '1A', 1), (50, 4, '1B', 1), (51, 4, '1C', 1), (52, 4, '1D', 1),
(53, 4, '1E', 2), (54, 4, '1F', 2), (55, 4, '1G', 2), (56, 4, '1H', 2),
(57, 4, '2A', 3), (58, 4, '2B', 3), (59, 4, '2C', 3), (60, 4, '2D', 3),
(61, 4, '2E', 4), (62, 4, '2F', 4), (63, 4, '2G', 4), (64, 4, '2H', 4),

-- Aircraft 5 (seat_id: 65-80)
(65, 5, '1A', 1), (66, 5, '1B', 1), (67, 5, '1C', 1), (68, 5, '1D', 1),
(69, 5, '1E', 2), (70, 5, '1F', 2), (71, 5, '1G', 2), (72, 5, '1H', 2),
(73, 5, '2A', 3), (74, 5, '2B', 3), (75, 5, '2C', 3), (76, 5, '2D', 3),
(77, 5, '2E', 4), (78, 5, '2F', 4), (79, 5, '2G', 4), (80, 5, '2H', 4),

-- Aircraft 6 (seat_id: 81-96)
(81, 6, '1A', 1), (82, 6, '1B', 1), (83, 6, '1C', 1), (84, 6, '1D', 1),
(85, 6, '1E', 2), (86, 6, '1F', 2), (87, 6, '1G', 2), (88, 6, '1H', 2),
(89, 6, '2A', 3), (90, 6, '2B', 3), (91, 6, '2C', 3), (92, 6, '2D', 3),
(93, 6, '2E', 4), (94, 6, '2F', 4), (95, 6, '2G', 4), (96, 6, '2H', 4),

-- Aircraft 7 (seat_id: 97-112)
(97, 7, '1A', 1), (98, 7, '1B', 1), (99, 7, '1C', 1), (100, 7, '1D', 1),
(101, 7, '1E', 2), (102, 7, '1F', 2), (103, 7, '1G', 2), (104, 7, '1H', 2),
(105, 7, '2A', 3), (106, 7, '2B', 3), (107, 7, '2C', 3), (108, 7, '2D', 3),
(109, 7, '2E', 4), (110, 7, '2F', 4), (111, 7, '2G', 4), (112, 7, '2H', 4),

-- Aircraft 8 (seat_id: 113-128)
(113, 8, '1A', 1), (114, 8, '1B', 1), (115, 8, '1C', 1), (116, 8, '1D', 1),
(117, 8, '1E', 2), (118, 8, '1F', 2), (119, 8, '1G', 2), (120, 8, '1H', 2),
(121, 8, '2A', 3), (122, 8, '2B', 3), (123, 8, '2C', 3), (124, 8, '2D', 3),
(125, 8, '2E', 4), (126, 8, '2F', 4), (127, 8, '2G', 4), (128, 8, '2H', 4),

-- Aircraft 9 (seat_id: 129-144)
(129, 9, '1A', 1), (130, 9, '1B', 1), (131, 9, '1C', 1), (132, 9, '1D', 1),
(133, 9, '1E', 2), (134, 9, '1F', 2), (135, 9, '1G', 2), (136, 9, '1H', 2),
(137, 9, '2A', 3), (138, 9, '2B', 3), (139, 9, '2C', 3), (140, 9, '2D', 3),
(141, 9, '2E', 4), (142, 9, '2F', 4), (143, 9, '2G', 4), (144, 9, '2H', 4),

-- Aircraft 10 (seat_id: 145-160)
(145, 10, '1A', 1), (146, 10, '1B', 1), (147, 10, '1C', 1), (148, 10, '1D', 1),
(149, 10, '1E', 2), (150, 10, '1F', 2), (151, 10, '1G', 2), (152, 10, '1H', 2),
(153, 10, '2A', 3), (154, 10, '2B', 3), (155, 10, '2C', 3), (156, 10, '2D', 3),
(157, 10, '2E', 4), (158, 10, '2F', 4), (159, 10, '2G', 4), (160, 10, '2H', 4);

-- 12. Ghế cho từng chuyến bay
INSERT INTO `Flight_Seats` (`flight_id`, `seat_id`, `base_price`, `seat_status`) VALUES
-- Flight 2: aircraft_id = 9 (seat_id: 129-144)
(2, 129, 900, 'AVAILABLE'),  -- 1A First
(2, 130, 900, 'BOOKED'),     -- 1B First
(2, 133, 550, 'BLOCKED'),    -- 1E Business
(2, 134, 550, 'AVAILABLE'),  -- 1F Business
(2, 137, 350, 'BOOKED'),     -- 2A Premium Economy
(2, 138, 350, 'BLOCKED'),    -- 2B Premium Economy
(2, 141, 200, 'AVAILABLE'),  -- 2E Economy
(2, 142, 200, 'BOOKED'),     -- 2F Economy

-- Flight 4: aircraft_id = 1 (seat_id: 1-16)
(4, 1,  900, 'AVAILABLE'),   -- 1A First
(4, 4,  900, 'AVAILABLE'),   -- 1D First
(4, 5,  550, 'AVAILABLE'),   -- 1E Business
(4, 8,  550, 'AVAILABLE'),   -- 1H Business
(4, 9,  350, 'AVAILABLE'),   -- 2A Premium Economy
(4, 12, 350, 'AVAILABLE'),   -- 2D Premium Economy
(4, 13, 200, 'AVAILABLE'),   -- 2E Economy
(4, 16, 200, 'AVAILABLE'),   -- 2H Economy

-- Flight 7: aircraft_id = 8 (seat_id: 113-128)
(7, 113, 900, 'AVAILABLE'),  -- 1A First
(7, 114, 900, 'BOOKED'),     -- 1B First
(7, 115, 900, 'BLOCKED'),    -- 1C First
(7, 116, 900, 'AVAILABLE'),  -- 1D First
(7, 117, 550, 'BOOKED'),     -- 1E Business
(7, 118, 550, 'AVAILABLE'),  -- 1F Business
(7, 119, 550, 'BLOCKED'),    -- 1G Business
(7, 120, 550, 'AVAILABLE'),  -- 1H Business
(7, 121, 350, 'BOOKED'),     -- 2A Premium Economy
(7, 122, 350, 'AVAILABLE'),  -- 2B Premium Economy
(7, 123, 350, 'BLOCKED'),    -- 2C Premium Economy
(7, 124, 350, 'AVAILABLE'),  -- 2D Premium Economy
(7, 125, 200, 'BOOKED'),     -- 2E Economy
(7, 126, 200, 'AVAILABLE'),  -- 2F Economy
(7, 127, 200, 'BLOCKED'),    -- 2G Economy
(7, 128, 200, 'AVAILABLE'),  -- 2H Economy

-- Flight 11: aircraft_id = 6 (seat_id: 81-96)
(11, 81,  900, 'AVAILABLE'), -- 1A First
(11, 82,  900, 'BOOKED'),    -- 1B First
(11, 83,  900, 'BLOCKED'),   -- 1C First
(11, 84,  900, 'AVAILABLE'), -- 1D First
(11, 85,  550, 'BOOKED'),    -- 1E Business
(11, 86,  550, 'AVAILABLE'), -- 1F Business
(11, 87,  550, 'BLOCKED'),   -- 1G Business
(11, 88,  550, 'AVAILABLE'), -- 1H Business
(11, 89,  350, 'BOOKED'),    -- 2A Premium Economy
(11, 90,  350, 'AVAILABLE'), -- 2B Premium Economy
(11, 91,  350, 'BLOCKED'),   -- 2C Premium Economy
(11, 92,  350, 'AVAILABLE'), -- 2D Premium Economy
(11, 93,  200, 'BOOKED'),    -- 2E Economy
(11, 94,  200, 'AVAILABLE'), -- 2F Economy
(11, 95,  200, 'BLOCKED'),   -- 2G Economy
(11, 96,  200, 'AVAILABLE'), -- 2H Economy

-- Flight 18: aircraft_id = 7 (seat_id: 97-112)
(18, 97,  900, 'AVAILABLE'), -- 1A First
(18, 98,  900, 'BOOKED'),    -- 1B First
(18, 101, 550, 'BLOCKED'),   -- 1E Business
(18, 102, 550, 'AVAILABLE'), -- 1F Business
(18, 105, 350, 'BOOKED'),    -- 2A Premium Economy
(18, 106, 350, 'AVAILABLE'), -- 2B Premium Economy
(18, 109, 200, 'BLOCKED'),   -- 2E Economy
(18, 110, 200, 'AVAILABLE'), -- 2F Economy

-- Flight 1 (16 ghế) - aircraft_id = 8
(1, 113, 900, 'AVAILABLE'),  -- 1A First
(1, 114, 900, 'BOOKED'),     -- 1B First
(1, 115, 900, 'BLOCKED'),    -- 1C First
(1, 116, 900, 'AVAILABLE'),  -- 1D First
(1, 117, 550, 'BOOKED'),     -- 1E Business
(1, 118, 550, 'AVAILABLE'),  -- 1F Business
(1, 119, 550, 'BLOCKED'),    -- 1G Business
(1, 120, 550, 'AVAILABLE'),  -- 1H Business
(1, 121, 350, 'BOOKED'),     -- 2A Premium Economy
(1, 122, 350, 'AVAILABLE'),  -- 2B Premium Economy
(1, 123, 350, 'BLOCKED'),    -- 2C Premium Economy
(1, 124, 350, 'AVAILABLE'),  -- 2D Premium Economy
(1, 125, 200, 'BOOKED'),     -- 2E Economy
(1, 126, 200, 'AVAILABLE'),  -- 2F Economy
(1, 127, 200, 'BLOCKED'),    -- 2G Economy
(1, 128, 200, 'AVAILABLE'),  -- 2H Economy

-- Flight 3 (8 ghế) - aircraft_id = 10
(3, 145, 900, 'AVAILABLE'),  -- 1A First
(3, 146, 900, 'BOOKED'),     -- 1B First
(3, 149, 550, 'BLOCKED'),    -- 1E Business
(3, 150, 550, 'AVAILABLE'),  -- 1F Business
(3, 153, 350, 'BOOKED'),     -- 2A Premium Economy
(3, 154, 350, 'BLOCKED'),    -- 2B Premium Economy
(3, 157, 200, 'AVAILABLE'),  -- 2E Economy
(3, 158, 200, 'BOOKED'),     -- 2F Economy

-- Flight 5 (8 ghế) - aircraft_id = 9
(5, 129, 900, 'AVAILABLE'),  -- 1A First
(5, 130, 900, 'BOOKED'),     -- 1B First
(5, 133, 550, 'BLOCKED'),    -- 1E Business
(5, 134, 550, 'AVAILABLE'),  -- 1F Business
(5, 137, 350, 'BOOKED'),     -- 2A Premium Economy
(5, 138, 350, 'BLOCKED'),    -- 2B Premium Economy
(5, 141, 200, 'AVAILABLE'),  -- 2E Economy
(5, 142, 200, 'BOOKED'),     -- 2F Economy

-- Flight 6 (8 ghế) - aircraft_id = 10
(6, 145, 900, 'AVAILABLE'),  -- 1A First
(6, 146, 900, 'BOOKED'),     -- 1B First
(6, 149, 550, 'BLOCKED'),    -- 1E Business
(6, 150, 550, 'AVAILABLE'),  -- 1F Business
(6, 153, 350, 'BOOKED'),     -- 2A Premium Economy
(6, 154, 350, 'BLOCKED'),    -- 2B Premium Economy
(6, 157, 200, 'AVAILABLE'),  -- 2E Economy
(6, 158, 200, 'BOOKED'),     -- 2F Economy

-- Flight 8 (8 ghế) - aircraft_id = 10
(8, 145, 900, 'AVAILABLE'),  -- 1A First
(8, 146, 900, 'BOOKED'),     -- 1B First
(8, 149, 550, 'BLOCKED'),    -- 1E Business
(8, 150, 550, 'AVAILABLE'),  -- 1F Business
(8, 153, 350, 'BOOKED'),     -- 2A Premium Economy
(8, 154, 350, 'BLOCKED'),    -- 2B Premium Economy
(8, 157, 200, 'AVAILABLE'),  -- 2E Economy
(8, 158, 200, 'BOOKED'),     -- 2F Economy

-- Flight 9 (16 ghế) - aircraft_id = 5
(9, 65, 900, 'AVAILABLE'),   -- 1A First
(9, 66, 900, 'BOOKED'),      -- 1B First
(9, 67, 900, 'BLOCKED'),     -- 1C First
(9, 68, 900, 'AVAILABLE'),   -- 1D First
(9, 69, 550, 'BOOKED'),      -- 1E Business
(9, 70, 550, 'AVAILABLE'),   -- 1F Business
(9, 71, 550, 'BLOCKED'),     -- 1G Business
(9, 72, 550, 'AVAILABLE'),   -- 1H Business
(9, 73, 350, 'BOOKED'),      -- 2A Premium Economy
(9, 74, 350, 'AVAILABLE'),   -- 2B Premium Economy
(9, 75, 350, 'BLOCKED'),     -- 2C Premium Economy
(9, 76, 350, 'AVAILABLE'),   -- 2D Premium Economy
(9, 77, 200, 'BOOKED'),      -- 2E Economy
(9, 78, 200, 'AVAILABLE'),   -- 2F Economy
(9, 79, 200, 'BLOCKED'),     -- 2G Economy
(9, 80, 200, 'AVAILABLE'),   -- 2H Economy

-- Flight 10 (16 ghế) - aircraft_id = 3
(10, 33, 900, 'AVAILABLE'),  -- 1A First
(10, 34, 900, 'BOOKED'),     -- 1B First
(10, 35, 900, 'BLOCKED'),    -- 1C First
(10, 36, 900, 'AVAILABLE'),  -- 1D First
(10, 37, 550, 'BOOKED'),     -- 1E Business
(10, 38, 550, 'AVAILABLE'),  -- 1F Business
(10, 39, 550, 'BLOCKED'),    -- 1G Business
(10, 40, 550, 'AVAILABLE'),  -- 1H Business
(10, 41, 350, 'BOOKED'),     -- 2A Premium Economy
(10, 42, 350, 'AVAILABLE'),  -- 2B Premium Economy
(10, 43, 350, 'BLOCKED'),    -- 2C Premium Economy
(10, 44, 350, 'AVAILABLE'),  -- 2D Premium Economy
(10, 45, 200, 'BOOKED'),     -- 2E Economy
(10, 46, 200, 'AVAILABLE'),  -- 2F Economy
(10, 47, 200, 'BLOCKED'),    -- 2G Economy
(10, 48, 200, 'AVAILABLE'),  -- 2H Economy

-- Flight 12 (16 ghế) - aircraft_id = 5
(12, 65, 900, 'AVAILABLE'),
(12, 66, 900, 'BOOKED'),
(12, 67, 900, 'BLOCKED'),
(12, 68, 900, 'AVAILABLE'),
(12, 69, 550, 'BOOKED'),
(12, 70, 550, 'AVAILABLE'),
(12, 71, 550, 'BLOCKED'),
(12, 72, 550, 'AVAILABLE'),
(12, 73, 350, 'BOOKED'),
(12, 74, 350, 'AVAILABLE'),
(12, 75, 350, 'BLOCKED'),
(12, 76, 350, 'AVAILABLE'),
(12, 77, 200, 'BOOKED'),
(12, 78, 200, 'AVAILABLE'),
(12, 79, 200, 'BLOCKED'),
(12, 80, 200, 'AVAILABLE'),

-- Flight 13 (16 ghế) - aircraft_id = 5
(13, 65, 900, 'AVAILABLE'),
(13, 66, 900, 'BOOKED'),
(13, 67, 900, 'BLOCKED'),
(13, 68, 900, 'AVAILABLE'),
(13, 69, 550, 'BOOKED'),
(13, 70, 550, 'AVAILABLE'),
(13, 71, 550, 'BLOCKED'),
(13, 72, 550, 'AVAILABLE'),
(13, 73, 350, 'BOOKED'),
(13, 74, 350, 'AVAILABLE'),
(13, 75, 350, 'BLOCKED'),
(13, 76, 350, 'AVAILABLE'),
(13, 77, 200, 'BOOKED'),
(13, 78, 200, 'AVAILABLE'),
(13, 79, 200, 'BLOCKED'),
(13, 80, 200, 'AVAILABLE'),

-- Flight 14 (16 ghế) - aircraft_id = 4
(14, 49, 900, 'AVAILABLE'),
(14, 50, 900, 'BOOKED'),
(14, 51, 900, 'BLOCKED'),
(14, 52, 900, 'AVAILABLE'),
(14, 53, 550, 'BOOKED'),
(14, 54, 550, 'AVAILABLE'),
(14, 55, 550, 'BLOCKED'),
(14, 56, 550, 'AVAILABLE'),
(14, 57, 350, 'BOOKED'),
(14, 58, 350, 'AVAILABLE'),
(14, 59, 350, 'BLOCKED'),
(14, 60, 350, 'AVAILABLE'),
(14, 61, 200, 'BOOKED'),
(14, 62, 200, 'AVAILABLE'),
(14, 63, 200, 'BLOCKED'),
(14, 64, 200, 'AVAILABLE'),

-- Flight 15 (16 ghế) - aircraft_id = 4
(15, 49, 900, 'AVAILABLE'),
(15, 50, 900, 'BOOKED'),
(15, 51, 900, 'BLOCKED'),
(15, 52, 900, 'AVAILABLE'),
(15, 53, 550, 'BOOKED'),
(15, 54, 550, 'AVAILABLE'),
(15, 55, 550, 'BLOCKED'),
(15, 56, 550, 'AVAILABLE'),
(15, 57, 350, 'BOOKED'),
(15, 58, 350, 'AVAILABLE'),
(15, 59, 350, 'BLOCKED'),
(15, 60, 350, 'AVAILABLE'),
(15, 61, 200, 'BOOKED'),
(15, 62, 200, 'AVAILABLE'),
(15, 63, 200, 'BLOCKED'),
(15, 64, 200, 'AVAILABLE'),

-- Flight 16 (16 ghế) - aircraft_id = 4
(16, 49, 900, 'AVAILABLE'),
(16, 50, 900, 'BOOKED'),
(16, 51, 900, 'BLOCKED'),
(16, 52, 900, 'AVAILABLE'),
(16, 53, 550, 'BOOKED'),
(16, 54, 550, 'AVAILABLE'),
(16, 55, 550, 'BLOCKED'),
(16, 56, 550, 'AVAILABLE'),
(16, 57, 350, 'BOOKED'),
(16, 58, 350, 'AVAILABLE'),
(16, 59, 350, 'BLOCKED'),
(16, 60, 350, 'AVAILABLE'),
(16, 61, 200, 'BOOKED'),
(16, 62, 200, 'AVAILABLE'),
(16, 63, 200, 'BLOCKED'),
(16, 64, 200, 'AVAILABLE'),

-- Flight 17 (16 ghế) - aircraft_id = 4
(17, 49, 900, 'AVAILABLE'),
(17, 50, 900, 'BOOKED'),
(17, 51, 900, 'BLOCKED'),
(17, 52, 900, 'AVAILABLE'),
(17, 53, 550, 'BOOKED'),
(17, 54, 550, 'AVAILABLE'),
(17, 55, 550, 'BLOCKED'),
(17, 56, 550, 'AVAILABLE'),
(17, 57, 350, 'BOOKED'),
(17, 58, 350, 'AVAILABLE'),
(17, 59, 350, 'BLOCKED'),
(17, 60, 350, 'AVAILABLE'),
(17, 61, 200, 'BOOKED'),
(17, 62, 200, 'AVAILABLE'),
(17, 63, 200, 'BLOCKED'),
(17, 64, 200, 'AVAILABLE'),

-- Flight 19 (8 ghế) - aircraft_id = 10
(19, 145, 900, 'AVAILABLE'),
(19, 146, 900, 'BOOKED'),
(19, 149, 550, 'BLOCKED'),
(19, 150, 550, 'AVAILABLE'),
(19, 153, 350, 'BOOKED'),
(19, 154, 350, 'BLOCKED'),
(19, 157, 200, 'AVAILABLE'),
(19, 158, 200, 'BOOKED'),

-- Flight 20 (16 ghế) - aircraft_id = 8
(20, 113, 900, 'AVAILABLE'),
(20, 114, 900, 'BOOKED'),
(20, 115, 900, 'BLOCKED'),
(20, 116, 900, 'AVAILABLE'),
(20, 117, 550, 'BOOKED'),
(20, 118, 550, 'AVAILABLE'),
(20, 119, 550, 'BLOCKED'),
(20, 120, 550, 'AVAILABLE'),
(20, 121, 350, 'BOOKED'),
(20, 122, 350, 'AVAILABLE'),
(20, 123, 350, 'BLOCKED'),
(20, 124, 350, 'AVAILABLE'),
(20, 125, 200, 'BOOKED'),
(20, 126, 200, 'AVAILABLE'),
(20, 127, 200, 'BLOCKED'),
(20, 128, 200, 'AVAILABLE');


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
INSERT INTO Tickets (ticket_id, ticket_passenger_id, flight_seat_id, ticket_number, segment_no, segment_type, `status`) VALUES
-- Flight 2 (flight_seat_id từ Flight_Seats của flight_id=2)
(1,  1,  1,  'TK202500001', 1, 'OUTBOUND', 'CONFIRMED'),   -- seat 129 (1A First)
(2,  2,  2,  'TK202500002', 1, 'OUTBOUND', 'BOOKED'),      -- seat 130 (1B First)
(3,  3,  3,  'TK202500003', 1, 'OUTBOUND', 'BOOKED'),      -- seat 133 (1E Business)
(4,  4,  4,  'TK202500004', 1, 'OUTBOUND', 'CONFIRMED'),   -- seat 134 (1F Business)
(5,  5,  5,  'TK202500005', 1, 'OUTBOUND', 'BOOKED'),      -- seat 137 (2A Premium Economy)
(6,  6,  6,  'TK202500006', 1, 'OUTBOUND', 'BOOKED'),      -- seat 138 (2B Premium Economy)
(7,  7,  7,  'TK202500007', 1, 'OUTBOUND', 'CONFIRMED'),   -- seat 141 (2E Economy)
(8,  8,  8,  'TK202500008', 1, 'OUTBOUND', 'BOOKED'),      -- seat 142 (2F Economy)

-- Flight 4 (flight_seat_id từ Flight_Seats của flight_id=4)
(9,  9,  9,  'TK202500009', 1, 'OUTBOUND', 'BOOKED'),      -- seat 1 (1A First)
(10, 10, 10, 'TK202500010', 1, 'OUTBOUND', 'CONFIRMED'),   -- seat 4 (1D First)
(11, 11, 11, 'TK202500011', 1, 'OUTBOUND', 'BOOKED'),      -- seat 5 (1E Business)
(12, 12, 12, 'TK202500012', 1, 'OUTBOUND', 'BOOKED'),      -- seat 8 (1H Business)
(13, 13, 13, 'TK202500013', 1, 'OUTBOUND', 'CHECKED_IN'),  -- seat 9 (2A Premium Economy)
(14, 14, 14, 'TK202500014', 1, 'OUTBOUND', 'CONFIRMED'),   -- seat 12 (2D Premium Economy)

-- Flight 7 (flight_seat_id từ Flight_Seats của flight_id=7)
(15, 15, 15, 'TK202500015', 1, 'OUTBOUND', 'BOOKED'),      -- seat 113 (1A First)
(16, 16, 16, 'TK202500016', 1, 'OUTBOUND', 'BOARDED'),     -- seat 114 (1B First)
(17, 17, 17, 'TK202500017', 1, 'OUTBOUND', 'CONFIRMED'),   -- seat 115 (1C First)
(18, 18, 18, 'TK202500018', 1, 'OUTBOUND', 'BOOKED'),      -- seat 116 (1D First)
(19, 19, 19, 'TK202500019', 1, 'OUTBOUND', 'CHECKED_IN'),  -- seat 117 (1E Business)
(20, 20, 20, 'TK202500020', 1, 'OUTBOUND', 'BOOKED'),      -- seat 118 (1F Business)

-- Flight 11 (flight_seat_id từ Flight_Seats của flight_id=11)
(21, 1,  21, 'TK202500021', 1, 'OUTBOUND', 'CONFIRMED'),   -- seat 81 (1A First)
(22, 2,  22, 'TK202500022', 1, 'OUTBOUND', 'BOOKED'),      -- seat 82 (1B First)
(23, 3,  23, 'TK202500023', 1, 'OUTBOUND', 'CHECKED_IN'),  -- seat 83 (1C First)
(24, 4,  24, 'TK202500024', 1, 'OUTBOUND', 'CONFIRMED'),   -- seat 84 (1D First)
(25, 5,  25, 'TK202500025', 1, 'OUTBOUND', 'BOOKED'),      -- seat 85 (1E Business)

-- Flight 18 (flight_seat_id từ Flight_Seats của flight_id=18)
(26, 6,  26, 'TK202500026', 1, 'OUTBOUND', 'CONFIRMED'),   -- seat 97 (1A First)
(27, 7,  27, 'TK202500027', 1, 'OUTBOUND', 'BOOKED'),      -- seat 98 (1B First)
(28, 8,  28, 'TK202500028', 1, 'OUTBOUND', 'CHECKED_IN'),  -- seat 101 (1E Business)
(29, 9,  29, 'TK202500029', 1, 'OUTBOUND', 'CONFIRMED'),   -- seat 102 (1F Business)
(30, 10, 30, 'TK202500030', 1, 'OUTBOUND', 'BOOKED');      -- seat 105 (2A Premium Economy)



-- 18. Lịch sử đặt vé
INSERT INTO Ticket_History (ticket_id, old_status, new_status, changed_at) VALUES
-- Ticket 1 (Flight 2)
(1,  'BOOKED', 'CONFIRMED', '2025-01-02 09:12:00'),

-- Ticket 2 (Flight 2)
(2,  'BOOKED', 'CONFIRMED', '2025-01-05 10:20:00'),
(2,  'CONFIRMED', 'CHECKED_IN', '2025-01-06 08:15:00'),

-- Ticket 3 (Flight 2)
(3,  'BOOKED', 'CANCELLED', '2025-01-03 11:44:00'),

-- Ticket 4 (Flight 2)
(4,  'BOOKED', 'CONFIRMED', '2025-02-01 15:30:00'),

-- Ticket 5 (Flight 2)
(5,  'BOOKED', 'CONFIRMED', '2025-02-10 18:05:00'),

-- Ticket 6 (Flight 2)
(6,  'BOOKED', 'CONFIRMED', '2025-01-12 16:20:00'),
(6,  'CONFIRMED', 'CHECKED_IN', '2025-01-13 06:55:00'),

-- Ticket 7 (Flight 2)
(7,  'BOOKED', 'CONFIRMED', '2025-03-03 09:55:00'),

-- Ticket 8 (Flight 2)
(8,  'BOOKED', 'CONFIRMED', '2025-03-10 13:44:00'),
(8,  'CONFIRMED', 'CANCELLED', '2025-03-11 07:22:00'),

-- Ticket 9 (Flight 4)
(9,  'BOOKED', 'CONFIRMED', '2025-03-21 12:00:00'),
(9,  'CONFIRMED', 'CHECKED_IN', '2025-03-22 08:45:00'),

-- Ticket 10 (Flight 4)
(10, 'BOOKED', 'CONFIRMED', '2025-04-01 19:30:00'),

-- Ticket 11 (Flight 4)
(11, 'BOOKED', 'CONFIRMED', '2025-04-11 09:14:00'),
(11, 'CONFIRMED', 'CHECKED_IN', '2025-04-12 05:30:00'),

-- Ticket 12 (Flight 4)
(12, 'BOOKED', 'CONFIRMED', '2025-05-02 11:50:00'),

-- Ticket 13 (Flight 4)
(13, 'BOOKED', 'CONFIRMED', '2025-05-06 15:10:00'),
(13, 'CONFIRMED', 'CHECKED_IN', '2025-05-06 18:30:00'),

-- Ticket 14 (Flight 4)
(14, 'BOOKED', 'CONFIRMED', '2025-05-14 20:55:00'),

-- Ticket 15 (Flight 7)
(15, 'BOOKED', 'CANCELLED', '2025-05-03 14:15:00'),

-- Ticket 16 (Flight 7)
(16, 'BOOKED', 'CONFIRMED', '2025-06-01 10:25:00'),
(16, 'CONFIRMED', 'CHECKED_IN', '2025-06-01 18:40:00'),
(16, 'CHECKED_IN', 'BOARDED', '2025-06-02 06:00:00'),

-- Ticket 17 (Flight 7)
(17, 'BOOKED', 'CONFIRMED', '2025-06-12 09:00:00'),

-- Ticket 18 (Flight 7)
(18, 'BOOKED', 'CONFIRMED', '2025-06-20 13:33:00'),
(18, 'CONFIRMED', 'REFUNDED', '2025-06-21 09:40:00'),

-- Ticket 19 (Flight 7)
(19, 'BOOKED', 'CONFIRMED', '2025-06-25 08:44:00'),
(19, 'CONFIRMED', 'CHECKED_IN', '2025-06-26 05:22:00'),

-- Ticket 20 (Flight 7)
(20, 'BOOKED', 'REFUNDED', '2025-07-01 14:20:00'),

-- Ticket 21 (Flight 11)
(21, 'BOOKED', 'CONFIRMED', '2025-01-20 10:15:00'),

-- Ticket 22 (Flight 11)
(22, 'BOOKED', 'CONFIRMED', '2025-01-21 14:30:00'),

-- Ticket 23 (Flight 11)
(23, 'BOOKED', 'CONFIRMED', '2025-01-22 09:45:00'),
(23, 'CONFIRMED', 'CHECKED_IN', '2025-01-27 16:00:00'),

-- Ticket 24 (Flight 11)
(24, 'BOOKED', 'CONFIRMED', '2025-01-23 11:20:00'),

-- Ticket 25 (Flight 11)
(25, 'BOOKED', 'CONFIRMED', '2025-01-24 15:50:00'),

-- Ticket 26 (Flight 18)
(26, 'BOOKED', 'CONFIRMED', '2025-07-05 08:30:00'),

-- Ticket 27 (Flight 18)
(27, 'BOOKED', 'CONFIRMED', '2025-07-06 10:15:00'),

-- Ticket 28 (Flight 18)
(28, 'BOOKED', 'CONFIRMED', '2025-07-07 13:20:00'),
(28, 'CONFIRMED', 'CHECKED_IN', '2025-07-13 10:00:00'),

-- Ticket 29 (Flight 18)
(29, 'BOOKED', 'CONFIRMED', '2025-07-08 16:45:00'),

-- Ticket 30 (Flight 18)
(30, 'BOOKED', 'CONFIRMED', '2025-07-09 09:30:00');

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
INSERT INTO Baggage (baggage_id, ticket_id, flight_id, baggage_tag, baggage_type, weight_kg, allowed_weight_kg, fee, `status`, special_handling) VALUES
-- Flight 2 (flight_id = 2)
(1,  1,  2,  'BG10001', 'CHECKED',   18.0,  20.0,  0.00,    'CHECKED_IN',  NULL),
(2,  2,  2,  'BG10002', 'CHECKED',   22.5,  20.0,  37.50,   'LOADED',      NULL),
(3,  3,  2,  'BG10003', 'CARRY_ON',  8.0,   10.0,  0.00,    'CHECKED_IN',  NULL),
(4,  4,  2,  'BG10004', 'CHECKED',   19.5,  20.0,  0.00,    'LOADED',      NULL),
(5,  5,  2,  'BG10005', 'CHECKED',   21.0,  20.0,  15.00,   'CHECKED_IN',  NULL),
(6,  7,  2,  'BG10006', 'CARRY_ON',  7.5,   10.0,  0.00,    'CHECKED_IN',  NULL),

-- Flight 4 (flight_id = 4)
(7,  9,  4,  'BG10007', 'CHECKED',   28.0,  30.0,  0.00,    'LOADED',      'FRAGILE'),
(8,  10, 4,  'BG10008', 'SPECIAL',   32.0,  25.0,  105.00,  'CHECKED_IN',  'SPORT_EQUIPMENT'),
(9,  11, 4,  'BG10009', 'CHECKED',   19.0,  20.0,  0.00,    'CHECKED_IN',  NULL),
(10, 12, 4,  'BG10010', 'CHECKED',   34.0,  35.0,  0.00,    'IN_TRANSIT',  'PRIORITY'),
(11, 13, 4,  'BG10011', 'CARRY_ON',  7.5,   10.0,  0.00,    'CHECKED_IN',  NULL),
(12, 14, 4,  'BG10012', 'CHECKED',   24.5,  25.0,  0.00,    'CHECKED_IN',  NULL),

-- Flight 7 (flight_id = 7)
(13, 16, 7,  'BG10013', 'CHECKED',   21.0,  20.0,  15.00,   'LOADED',      NULL),
(14, 17, 7,  'BG10014', 'CHECKED',   19.0,  20.0,  0.00,    'CHECKED_IN',  NULL),
(15, 18, 7,  'BG10015', 'SPECIAL',   29.5,  25.0,  67.50,   'IN_TRANSIT',  'MUSICAL_INSTRUMENT'),
(16, 19, 7,  'BG10016', 'CHECKED',   24.0,  25.0,  0.00,    'CHECKED_IN',  NULL),
(17, 20, 7,  'BG10017', 'CARRY_ON',  9.0,   10.0,  0.00,    'CHECKED_IN',  NULL),

-- Flight 11 (flight_id = 11)
(18, 21, 11, 'BG10018', 'CHECKED',   28.0,  30.0,  0.00,    'LOADED',      NULL),
(19, 22, 11, 'BG10019', 'CHECKED',   20.5,  20.0,  7.50,    'IN_TRANSIT',  NULL),
(20, 23, 11, 'BG10020', 'CARRY_ON',  9.0,   10.0,  0.00,    'CHECKED_IN',  NULL),
(21, 24, 11, 'BG10021', 'CHECKED',   18.5,  20.0,  0.00,    'LOADED',      NULL),
(22, 25, 11, 'BG10022', 'CHECKED',   22.0,  20.0,  30.00,   'CHECKED_IN',  NULL),

-- Flight 18 (flight_id = 18)
(23, 26, 18, 'BG10023', 'CHECKED',   27.0,  25.0,  30.00,   'LOADED',      NULL),
(24, 27, 18, 'BG10024', 'CHECKED',   18.0,  20.0,  0.00,    'CLAIMED',     NULL),
(25, 28, 18, 'BG10025', 'SPECIAL',   33.0,  25.0,  120.00,  'LOST',        'SPORT_EQUIPMENT'),
(26, 29, 18, 'BG10026', 'CHECKED',   19.5,  20.0,  0.00,    'CHECKED_IN',  NULL),
(27, 30, 18, 'BG10027', 'CARRY_ON',  8.5,   10.0,  0.00,    'CHECKED_IN',  NULL);

-- 21. Lịch sử trạng thái hành lý
INSERT INTO Baggage_History (baggage_id, old_status, new_status, changed_at) VALUES
-- Baggage 1 (Flight 2)
(1,  'CREATED', 'CHECKED_IN', '2025-01-02 08:00:00'),

-- Baggage 2 (Flight 2)
(2,  'CREATED', 'CHECKED_IN', '2025-01-05 09:20:00'),
(2,  'CHECKED_IN', 'LOADED', '2025-01-05 10:00:00'),

-- Baggage 3 (Flight 2)
(3,  'CREATED', 'CHECKED_IN', '2025-01-03 11:00:00'),

-- Baggage 4 (Flight 2)
(4,  'CREATED', 'CHECKED_IN', '2025-02-01 13:40:00'),
(4,  'CHECKED_IN', 'LOADED', '2025-02-01 15:10:00'),

-- Baggage 5 (Flight 2)
(5,  'CREATED', 'CHECKED_IN', '2025-02-10 17:00:00'),

-- Baggage 6 (Flight 2)
(6,  'CREATED', 'CHECKED_IN', '2025-03-03 09:30:00'),

-- Baggage 7 (Flight 4)
(7,  'CREATED', 'CHECKED_IN', '2025-03-21 08:00:00'),
(7,  'CHECKED_IN', 'LOADED', '2025-03-21 09:30:00'),

-- Baggage 8 (Flight 4)
(8,  'CREATED', 'CHECKED_IN', '2025-04-01 17:00:00'),

-- Baggage 9 (Flight 4)
(9,  'CREATED', 'CHECKED_IN', '2025-04-11 08:00:00'),

-- Baggage 10 (Flight 4)
(10, 'CREATED', 'CHECKED_IN', '2025-05-02 10:30:00'),
(10, 'CHECKED_IN', 'LOADED', '2025-05-02 11:45:00'),
(10, 'LOADED', 'IN_TRANSIT', '2025-05-02 13:00:00'),

-- Baggage 11 (Flight 4)
(11, 'CREATED', 'CHECKED_IN', '2025-05-06 14:00:00'),

-- Baggage 12 (Flight 4)
(12, 'CREATED', 'CHECKED_IN', '2025-05-14 19:00:00'),

-- Baggage 13 (Flight 7)
(13, 'CREATED', 'CHECKED_IN', '2025-06-01 09:00:00'),
(13, 'CHECKED_IN', 'LOADED', '2025-06-01 10:45:00'),

-- Baggage 14 (Flight 7)
(14, 'CREATED', 'CHECKED_IN', '2025-06-12 07:30:00'),

-- Baggage 15 (Flight 7)
(15, 'CREATED', 'CHECKED_IN', '2025-06-20 12:00:00'),
(15, 'CHECKED_IN', 'LOADED', '2025-06-20 13:30:00'),
(15, 'LOADED', 'IN_TRANSIT', '2025-06-20 15:00:00'),

-- Baggage 16 (Flight 7)
(16, 'CREATED', 'CHECKED_IN', '2025-06-25 07:00:00'),

-- Baggage 17 (Flight 7)
(17, 'CREATED', 'CHECKED_IN', '2025-07-01 13:00:00'),

-- Baggage 18 (Flight 11)
(18, 'CREATED', 'CHECKED_IN', '2025-01-20 09:00:00'),
(18, 'CHECKED_IN', 'LOADED', '2025-01-20 10:30:00'),

-- Baggage 19 (Flight 11)
(19, 'CREATED', 'CHECKED_IN', '2025-01-21 13:00:00'),
(19, 'CHECKED_IN', 'LOADED', '2025-01-21 14:30:00'),
(19, 'LOADED', 'IN_TRANSIT', '2025-01-21 16:00:00'),

-- Baggage 20 (Flight 11)
(20, 'CREATED', 'CHECKED_IN', '2025-01-22 08:30:00'),

-- Baggage 21 (Flight 11)
(21, 'CREATED', 'CHECKED_IN', '2025-01-23 10:00:00'),
(21, 'CHECKED_IN', 'LOADED', '2025-01-23 11:30:00'),

-- Baggage 22 (Flight 11)
(22, 'CREATED', 'CHECKED_IN', '2025-01-24 14:30:00'),

-- Baggage 23 (Flight 18)
(23, 'CREATED', 'CHECKED_IN', '2025-07-05 07:00:00'),
(23, 'CHECKED_IN', 'LOADED', '2025-07-05 08:30:00'),

-- Baggage 24 (Flight 18)
(24, 'CREATED', 'CHECKED_IN', '2025-07-06 09:00:00'),
(24, 'CHECKED_IN', 'LOADED', '2025-07-06 10:30:00'),
(24, 'LOADED', 'IN_TRANSIT', '2025-07-06 12:00:00'),
(24, 'IN_TRANSIT', 'CLAIMED', '2025-07-06 14:30:00'),

-- Baggage 25 (Flight 18)
(25, 'CREATED', 'CHECKED_IN', '2025-07-07 12:00:00'),
(25, 'CHECKED_IN', 'LOADED', '2025-07-07 13:30:00'),
(25, 'LOADED', 'LOST', '2025-07-07 16:00:00'),

-- Baggage 26 (Flight 18)
(26, 'CREATED', 'CHECKED_IN', '2025-07-08 15:00:00'),

-- Baggage 27 (Flight 18)
(27, 'CREATED', 'CHECKED_IN', '2025-07-09 08:00:00');
--

INSERT INTO carryon_baggage (weight_kg, class_id, size_limit, description, is_default) VALUES
-- Default carry-on theo class
(14, 1, '56x36x23 cm', 'Xách tay mặc định First Class 14kg', 1),
(10, 2, '56x36x23 cm', 'Xách tay mặc định Business 10kg', 1),
(10, 3, '56x36x23 cm', 'Xách tay mặc định Premium Economy 10kg', 1),
(7,  4, '56x36x23 cm', 'Xách tay mặc định Economy 7kg', 1),

-- Extra items (tùy chọn mua thêm)
(3,  4, '40x30x15 cm', 'Túi cá nhân nhỏ 3kg', 0),
(5,  4, '45x35x20 cm', 'Balô nhỏ 5kg', 0);

--
-- Đang đổ dữ liệu cho bảng `checked_baggage`
--

INSERT INTO checked_baggage (weight_kg, price, description) VALUES
(10, 200000, 'Gói ký gửi 10kg – tiêu chuẩn'),
(15, 320000, 'Gói ký gửi 15kg'),
(20, 450000, 'Gói ký gửi 20kg – phổ biến nhất'),
(25, 580000, 'Gói ký gửi 25kg'),
(30, 720000, 'Gói ký gửi 30kg – hành lý nhiều'),
(35, 850000, 'Gói ký gửi 35kg'),
(40, 990000, 'Gói ký gửi 40kg – tối đa theo quy định');
--
-- Đang đổ dữ liệu cho bảng `national`
--


INSERT INTO national (country_name, country_code, phone_code) VALUES
('United States', 'US', '+1'),
('Japan', 'JP', '+81'),
('South Korea', 'KR', '+82'),
('France', 'FR', '+33'),
('Vietnam', 'VN', '+84');

--
-- ticket_baggage - Liên kết vé với hành lý carry-on/checked
INSERT INTO `ticket_baggage` (`id`, `ticket_id`, `baggage_type`, `carryon_id`, `checked_id`, `quantity`, `note`) VALUES
-- Flight 2 Tickets
-- Ticket 1 (First Class)
(1, 1, 'carry_on', 1, NULL, 1, 'Xách tay mặc định First Class 14kg'),
(2, 1, 'checked', NULL, 3, 1, 'Khách mua thêm 20kg'),

-- Ticket 2 (First Class)
(3, 2, 'carry_on', 1, NULL, 1, 'Xách tay mặc định 14kg'),
(4, 2, 'checked', NULL, 4, 1, 'Khách mua 25kg'),

-- Ticket 3 (Business)
(5, 3, 'carry_on', 2, NULL, 1, 'Xách tay mặc định Business 10kg'),

-- Ticket 4 (Business)
(6, 4, 'carry_on', 2, NULL, 1, 'Xách tay 10kg'),
(7, 4, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),

-- Ticket 5 (Premium Economy)
(8, 5, 'carry_on', 3, NULL, 1, 'Xách tay mặc định Premium Economy 10kg'),
(9, 5, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),

-- Ticket 6 (Premium Economy)
(10, 6, 'carry_on', 3, NULL, 1, 'Xách tay 10kg'),

-- Ticket 7 (Economy)
(11, 7, 'carry_on', 4, NULL, 1, 'Xách tay mặc định Economy 7kg'),
(12, 7, 'carry_on', 5, NULL, 1, 'Túi cá nhân nhỏ 3kg'),

-- Ticket 8 (Economy)
(13, 8, 'carry_on', 4, NULL, 1, 'Xách tay 7kg'),

-- Flight 4 Tickets
-- Ticket 9 (First Class)
(14, 9, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(15, 9, 'checked', NULL, 5, 1, 'Ký gửi 30kg'),

-- Ticket 10 (First Class)
(16, 10, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(17, 10, 'checked', NULL, 6, 1, 'Ký gửi 35kg - Sport Equipment'),

-- Ticket 11 (Business)
(18, 11, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(19, 11, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),

-- Ticket 12 (Business)
(20, 12, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(21, 12, 'checked', NULL, 6, 1, 'Ký gửi 35kg - Priority'),

-- Ticket 13 (Premium Economy)
(22, 13, 'carry_on', 3, NULL, 1, 'Premium Economy 10kg'),

-- Ticket 14 (Premium Economy)
(23, 14, 'carry_on', 3, NULL, 1, 'Premium Economy 10kg'),
(24, 14, 'checked', NULL, 4, 1, 'Ký gửi 25kg'),

-- Flight 7 Tickets
-- Ticket 15 (First Class)
(25, 15, 'carry_on', 1, NULL, 1, 'First Class 14kg'),

-- Ticket 16 (First Class)
(26, 16, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(27, 16, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),

-- Ticket 17 (First Class)
(28, 17, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(29, 17, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),

-- Ticket 18 (First Class)
(30, 18, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(31, 18, 'checked', NULL, 5, 1, 'Ký gửi 30kg - Musical Instrument'),

-- Ticket 19 (Business)
(32, 19, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(33, 19, 'checked', NULL, 4, 1, 'Ký gửi 25kg'),

-- Ticket 20 (Business)
(34, 20, 'carry_on', 2, NULL, 1, 'Business 10kg'),

-- Flight 11 Tickets
-- Ticket 21 (First Class)
(35, 21, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(36, 21, 'checked', NULL, 5, 1, 'Ký gửi 30kg'),

-- Ticket 22 (First Class)
(37, 22, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(38, 22, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),

-- Ticket 23 (First Class)
(39, 23, 'carry_on', 1, NULL, 1, 'First Class 14kg'),

-- Ticket 24 (First Class)
(40, 24, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(41, 24, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),

-- Ticket 25 (Business)
(42, 25, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(43, 25, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),

-- Flight 18 Tickets
-- Ticket 26 (First Class)
(44, 26, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(45, 26, 'checked', NULL, 4, 1, 'Ký gửi 25kg'),

-- Ticket 27 (First Class)
(46, 27, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(47, 27, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),

-- Ticket 28 (Business)
(48, 28, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(49, 28, 'checked', NULL, 6, 1, 'Ký gửi 35kg - Sport Equipment'),

-- Ticket 29 (Business)
(50, 29, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(51, 29, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),

-- Ticket 30 (Premium Economy)
(52, 30, 'carry_on', 3, NULL, 1, 'Premium Economy 10kg');
