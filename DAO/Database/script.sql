DROP DATABASE IF EXISTS `FlightTicketManagement`;

CREATE DATABASE IF NOT EXISTS `FlightTicketManagement`
  DEFAULT CHARACTER SET utf8mb4
  DEFAULT COLLATE utf8mb4_unicode_ci;

USE `FlightTicketManagement`;

-- 1. Tài khoản
CREATE TABLE accounts (
    account_id              INT AUTO_INCREMENT PRIMARY KEY,
    email                   VARCHAR(100) NOT NULL UNIQUE,
    `password`              VARCHAR(255) NOT NULL,
    failed_attempts         INT NOT NULL DEFAULT 5,
    is_active               BOOLEAN NOT NULL DEFAULT TRUE,
    created_at              DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);
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
CREATE TABLE roles (
    role_id                 INT AUTO_INCREMENT PRIMARY KEY,
    role_code               VARCHAR(50) NOT NULL UNIQUE,
    role_name               VARCHAR(100) NOT NULL
);
INSERT INTO roles (role_code, role_name) VALUES
('ADMIN', 'Quản trị viên'),
('STAFF', 'Nhân viên'),
('USER',  'Người dùng');

-- 3. Gán vai trò cho tài khoản (N-N)
CREATE TABLE account_role (
    account_id INT NOT NULL,
    role_id    INT NOT NULL,
    PRIMARY KEY (account_id, role_id),
    FOREIGN KEY (account_id) REFERENCES accounts(account_id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES roles(role_id) ON DELETE CASCADE
);
INSERT INTO account_role (account_id, role_id) VALUES (1, 1) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (2, 2) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (3, 3) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (4, 1) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (5, 2) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (6, 3) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (7, 1) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);
INSERT INTO account_role (account_id, role_id) VALUES (8, 2) ON DUPLICATE KEY UPDATE role_id = VALUES(role_id);

-- 4. Quyền
CREATE TABLE permissions (
    permission_id INT AUTO_INCREMENT PRIMARY KEY,
    permission_code     VARCHAR(100) NOT NULL UNIQUE,
    permission_name     VARCHAR(150) NOT NULL
);
INSERT INTO permissions (permission_code, permission_name) VALUES
('home.view',             	'Xem trang chủ'),
('flights.read',          	'Tra cứu chuyến bay'),
('flights.create',        	'Tạo chuyến bay'),
('fare_rules.manage',     	'Quản lý quy tắc giá vé'),
('tickets.create_search', 	'Đặt chỗ'),
('tickets.mine',          	'Xem đặt chỗ của tôi'),
('tickets.operate',       	'Vé của tôi'),
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


-- 5. Vai trò - Quyền (N-N)
CREATE TABLE role_permissions (
    role_id       INT NOT NULL,
    permission_id INT NOT NULL,
    PRIMARY KEY (role_id, permission_id),
    FOREIGN KEY (role_id) REFERENCES roles(role_id) ON DELETE CASCADE,
    FOREIGN KEY (permission_id) REFERENCES permissions(permission_id) ON DELETE CASCADE
);
-- USER
INSERT IGNORE INTO role_permissions (role_id, permission_id)
SELECT r.role_id, p.permission_id
FROM roles r
JOIN permissions p ON p.permission_code IN (
    'home.view',
    'tickets.create_search',
    'tickets.mine',
    'tickets.operate',
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

-- 5. Hãng hàng không
CREATE TABLE Airlines (
    airline_id INT AUTO_INCREMENT PRIMARY KEY,
    airline_code VARCHAR(10) UNIQUE NOT NULL,
    airline_name VARCHAR(100) NOT NULL,
    country VARCHAR(100)
);
INSERT INTO `Airlines` (`airline_code`, `airline_name`, `country`) VALUES 
('VN', 'Vietnam Airlines', 	'Vietnam'),
('VJ', 'Vietjet Air', 		'Vietnam'),
('QH', 'Bamboo Airways', 	'Vietnam'),
('AA', 'American Airlines', 'American'),
('OZ', 'Asiana Airlines', 	'Korean');

-- 6. Sân bay
CREATE TABLE Airports (
    airport_id INT AUTO_INCREMENT PRIMARY KEY,
    airport_code VARCHAR(10) UNIQUE NOT NULL,
    airport_name VARCHAR(100) NOT NULL,
    city VARCHAR(100),
    country VARCHAR(100)
);
INSERT INTO `Airports` (`airport_code`, `airport_name`, `city`, `country`) VALUES 
('HAN', 'Noi Bai International Airport', 		'Hanoi', 				'Vietnam'),
('SGN', 'Tan Son Nhat International Airport', 	'Ho Chi Minh City', 	'Vietnam'),
('DAD', 'Da Nang International Airport', 		'Da Nang', 				'Vietnam'),
('YYJ', 'Vitoria International Airport', 		'Victoria', 			'Canada'),
('GMP', 'Gimpo International Airport', 			'Seoul', 				'Korean');

-- 7. Máy bay
CREATE TABLE Aircrafts (
    aircraft_id INT AUTO_INCREMENT PRIMARY KEY,
    airline_id INT,
    model VARCHAR(100), 
    manufacturer VARCHAR(100),
    capacity INT,
    FOREIGN KEY (airline_id) REFERENCES Airlines(airline_id)
);
INSERT INTO `Aircrafts` (`airline_id`, `model`, `manufacturer`, `capacity`) VALUES
(1, 'Airbus A350-900', 'Airbus', 18),
(1, 'Boeing 787-10', 'Boeing', 18),
(2, 'Airbus A321neo', 'Airbus', 18),
(2, 'Airbus A320neo', 'Airbus', 18),
(3, 'Embraer E195-E2', 'Embraer', 18),
(3, 'Boeing 787-9 Dreamliner', 'Boeing', 18),
(4, 'Boeing 737 MAX 8', 'Boeing', 18),
(4, 'Airbus A321XLR', 'Airbus', 18),
(5, 'Airbus A380-800', 'Airbus', 18),
(5, 'Boeing 777-200ER', 'Boeing', 18);

-- 8. Tuyến bay
CREATE TABLE Routes (
    route_id INT AUTO_INCREMENT PRIMARY KEY,
    departure_place_id INT,
    arrival_place_id INT,
    distance_km INT,
    duration_minutes INT,
    FOREIGN KEY (departure_place_id) REFERENCES Airports(airport_id),
    FOREIGN KEY (arrival_place_id) REFERENCES Airports(airport_id)
);
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
CREATE TABLE Flights (
    flight_id INT AUTO_INCREMENT PRIMARY KEY,
    flight_number VARCHAR(20) NOT NULL,
    aircraft_id INT,
    route_id INT,
    departure_time DATETIME,
    arrival_time DATETIME,
    base_price DECIMAL(12,2) NOT NULL DEFAULT 0,
    note TEXT,
    `status` ENUM('SCHEDULED','DELAYED','CANCELLED','COMPLETED') NOT NULL DEFAULT 'SCHEDULED',
    is_deleted BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY (aircraft_id) REFERENCES Aircrafts(aircraft_id),
    FOREIGN KEY (route_id) REFERENCES Routes(route_id)
);
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
CREATE TABLE Cabin_Classes (
    class_id INT AUTO_INCREMENT PRIMARY KEY,
    class_name VARCHAR(50) NOT NULL,
    `description` VARCHAR(255)
);
INSERT INTO `Cabin_Classes` (`class_name`, `description`) VALUES 
('First', 'First class description'),
('Business', 'Business class description'),
('Premium Economy', 'Premium Economy class description'),
('Economy', 'Economy class description');

-- 11. Ghế ngồi trên máy bay
CREATE TABLE Seats (
    seat_id INT AUTO_INCREMENT PRIMARY KEY,
    aircraft_id INT,
    seat_number VARCHAR(10),
    class_id INT,
    FOREIGN KEY (aircraft_id) REFERENCES Aircrafts(aircraft_id),
    FOREIGN KEY (class_id) REFERENCES Cabin_Classes(class_id)
);
INSERT INTO `Seats` (`seat_id`, `aircraft_id`, `seat_number`, `class_id`) VALUES
-- Aircraft 1 (seat_id: 1-18)
(1, 1, '1A', 1), (2, 1, '1B', 1), (3, 1, '1C', 1), (4, 1, '1D', 1), (5, 1, '1E', 1), (6, 1, '1F', 1),
(7, 1, '2A', 2), (8, 1, '2B', 2), (9, 1, '2C', 2), (10, 1, '2D', 2), (11, 1, '2E', 2), (12, 1, '2F', 2),
(13, 1, '3A', 3), (14, 1, '3B', 3), (15, 1, '3C', 3), (16, 1, '3D', 3), (17, 1, '3E', 3), (18, 1, '3F', 3),

-- Aircraft 2 (seat_id: 19-36)
(19, 2, '1A', 1), (20, 2, '1B', 1), (21, 2, '1C', 1), (22, 2, '1D', 1), (23, 2, '1E', 1), (24, 2, '1F', 1),
(25, 2, '2A', 2), (26, 2, '2B', 2), (27, 2, '2C', 2), (28, 2, '2D', 2), (29, 2, '2E', 2), (30, 2, '2F', 2),
(31, 2, '3A', 3), (32, 2, '3B', 3), (33, 2, '3C', 3), (34, 2, '3D', 3), (35, 2, '3E', 3), (36, 2, '3F', 3),

-- Aircraft 3 (seat_id: 37-54)
(37, 3, '1A', 1), (38, 3, '1B', 1), (39, 3, '1C', 1), (40, 3, '1D', 1), (41, 3, '1E', 1), (42, 3, '1F', 1),
(43, 3, '2A', 2), (44, 3, '2B', 2), (45, 3, '2C', 2), (46, 3, '2D', 2), (47, 3, '2E', 2), (48, 3, '2F', 2),
(49, 3, '3A', 3), (50, 3, '3B', 3), (51, 3, '3C', 3), (52, 3, '3D', 3), (53, 3, '3E', 3), (54, 3, '3F', 3),

-- Aircraft 4 (seat_id: 55-72)
(55, 4, '1A', 1), (56, 4, '1B', 1), (57, 4, '1C', 1), (58, 4, '1D', 1), (59, 4, '1E', 1), (60, 4, '1F', 1),
(61, 4, '2A', 2), (62, 4, '2B', 2), (63, 4, '2C', 2), (64, 4, '2D', 2), (65, 4, '2E', 2), (66, 4, '2F', 2),
(67, 4, '3A', 3), (68, 4, '3B', 3), (69, 4, '3C', 3), (70, 4, '3D', 3), (71, 4, '3E', 3), (72, 4, '3F', 3),

-- Aircraft 5 (seat_id: 73-90)
(73, 5, '1A', 1), (74, 5, '1B', 1), (75, 5, '1C', 1), (76, 5, '1D', 1), (77, 5, '1E', 1), (78, 5, '1F', 1),
(79, 5, '2A', 2), (80, 5, '2B', 2), (81, 5, '2C', 2), (82, 5, '2D', 2), (83, 5, '2E', 2), (84, 5, '2F', 2),
(85, 5, '3A', 3), (86, 5, '3B', 3), (87, 5, '3C', 3), (88, 5, '3D', 3), (89, 5, '3E', 3), (90, 5, '3F', 3),

-- Aircraft 6 (seat_id: 91-108)
(91, 6, '1A', 1), (92, 6, '1B', 1), (93, 6, '1C', 1), (94, 6, '1D', 1), (95, 6, '1E', 1), (96, 6, '1F', 1),
(97, 6, '2A', 2), (98, 6, '2B', 2), (99, 6, '2C', 2), (100, 6, '2D', 2), (101, 6, '2E', 2), (102, 6, '2F', 2),
(103, 6, '3A', 3), (104, 6, '3B', 3), (105, 6, '3C', 3), (106, 6, '3D', 3), (107, 6, '3E', 3), (108, 6, '3F', 3),

-- Aircraft 7 (seat_id: 109-126)
(109, 7, '1A', 1), (110, 7, '1B', 1), (111, 7, '1C', 1), (112, 7, '1D', 1), (113, 7, '1E', 1), (114, 7, '1F', 1),
(115, 7, '2A', 2), (116, 7, '2B', 2), (117, 7, '2C', 2), (118, 7, '2D', 2), (119, 7, '2E', 2), (120, 7, '2F', 2),
(121, 7, '3A', 3), (122, 7, '3B', 3), (123, 7, '3C', 3), (124, 7, '3D', 3), (125, 7, '3E', 3), (126, 7, '3F', 3),

-- Aircraft 8 (seat_id: 127-144)
(127, 8, '1A', 1), (128, 8, '1B', 1), (129, 8, '1C', 1), (130, 8, '1D', 1), (131, 8, '1E', 1), (132, 8, '1F', 1),
(133, 8, '2A', 2), (134, 8, '2B', 2), (135, 8, '2C', 2), (136, 8, '2D', 2), (137, 8, '2E', 2), (138, 8, '2F', 2),
(139, 8, '3A', 3), (140, 8, '3B', 3), (141, 8, '3C', 3), (142, 8, '3D', 3), (143, 8, '3E', 3), (144, 8, '3F', 3),

-- Aircraft 9 (seat_id: 145-162)
(145, 9, '1A', 1), (146, 9, '1B', 1), (147, 9, '1C', 1), (148, 9, '1D', 1), (149, 9, '1E', 1), (150, 9, '1F', 1),
(151, 9, '2A', 2), (152, 9, '2B', 2), (153, 9, '2C', 2), (154, 9, '2D', 2), (155, 9, '2E', 2), (156, 9, '2F', 2),
(157, 9, '3A', 3), (158, 9, '3B', 3), (159, 9, '3C', 3), (160, 9, '3D', 3), (161, 9, '3E', 3), (162, 9, '3F', 3),

-- Aircraft 10 (seat_id: 163-180)
(163, 10, '1A', 1), (164, 10, '1B', 1), (165, 10, '1C', 1), (166, 10, '1D', 1), (167, 10, '1E', 1), (168, 10, '1F', 1),
(169, 10, '2A', 2), (170, 10, '2B', 2), (171, 10, '2C', 2), (172, 10, '2D', 2), (173, 10, '2E', 2), (174, 10, '2F', 2),
(175, 10, '3A', 3), (176, 10, '3B', 3), (177, 10, '3C', 3), (178, 10, '3D', 3), (179, 10, '3E', 3), (180, 10, '3F', 3);


-- 12. Ghế cho từng chuyến bay
CREATE TABLE Flight_Seats (
    flight_seat_id INT AUTO_INCREMENT PRIMARY KEY,
    flight_id INT,
    seat_id INT,
    base_price DECIMAL(12,2),
    seat_status ENUM('AVAILABLE','BOOKED','BLOCKED') DEFAULT 'AVAILABLE',
    FOREIGN KEY (flight_id) REFERENCES Flights(flight_id),
    FOREIGN KEY (seat_id) REFERENCES Seats(seat_id)
);
INSERT INTO `Flight_Seats` (`flight_id`, `seat_id`, `base_price`, `seat_status`) VALUES
-- Flight 1 (18 ghế) - aircraft_id = 8 (seat_id: 127-144)
(1, 127, 900, 'AVAILABLE'),  (1, 128, 900, 'BOOKED'),     (1, 129, 900, 'BLOCKED'),    
(1, 130, 900, 'AVAILABLE'),  (1, 131, 900, 'BOOKED'),     (1, 132, 900, 'AVAILABLE'),
(1, 133, 550, 'BOOKED'),     (1, 134, 550, 'AVAILABLE'),  (1, 135, 550, 'BLOCKED'),    
(1, 136, 550, 'AVAILABLE'),  (1, 137, 550, 'BOOKED'),     (1, 138, 550, 'AVAILABLE'),
(1, 139, 350, 'BOOKED'),     (1, 140, 350, 'AVAILABLE'),  (1, 141, 350, 'BLOCKED'),    
(1, 142, 350, 'AVAILABLE'),  (1, 143, 350, 'BOOKED'),     (1, 144, 350, 'AVAILABLE'),

-- Flight 2 (9 ghế) - aircraft_id = 9 (seat_id: 145-162)
(2, 145, 900, 'AVAILABLE'),  (2, 146, 900, 'BOOKED'),     (2, 147, 900, 'BLOCKED'),
(2, 151, 550, 'BOOKED'),     (2, 152, 550, 'AVAILABLE'),  (2, 153, 550, 'BLOCKED'),
(2, 157, 350, 'BOOKED'),     (2, 158, 350, 'BLOCKED'),    (2, 159, 350, 'AVAILABLE'),

-- Flight 3 (9 ghế) - aircraft_id = 10 (seat_id: 163-180)
(3, 163, 900, 'AVAILABLE'),  (3, 164, 900, 'BOOKED'),     (3, 165, 900, 'BLOCKED'),
(3, 169, 550, 'BLOCKED'),    (3, 170, 550, 'AVAILABLE'),  (3, 171, 550, 'BOOKED'),
(3, 175, 350, 'BOOKED'),     (3, 176, 350, 'BLOCKED'),    (3, 177, 350, 'AVAILABLE'),

-- Flight 4 (9 ghế) - aircraft_id = 1 (seat_id: 1-18)
(4, 1,  900, 'AVAILABLE'),   (4, 2,  900, 'BOOKED'),      (4, 3,  900, 'AVAILABLE'),
(4, 7,  550, 'AVAILABLE'),   (4, 8,  550, 'BOOKED'),      (4, 9,  550, 'AVAILABLE'),
(4, 13, 350, 'AVAILABLE'),   (4, 14, 350, 'BLOCKED'),     (4, 15, 350, 'AVAILABLE'),

-- Flight 5 (9 ghế) - aircraft_id = 9 (seat_id: 145-162)
(5, 145, 900, 'AVAILABLE'),  (5, 146, 900, 'BOOKED'),     (5, 147, 900, 'BLOCKED'),
(5, 151, 550, 'BLOCKED'),    (5, 152, 550, 'AVAILABLE'),  (5, 153, 550, 'BOOKED'),
(5, 157, 350, 'BOOKED'),     (5, 158, 350, 'BLOCKED'),    (5, 159, 350, 'AVAILABLE'),

-- Flight 6 (9 ghế) - aircraft_id = 10 (seat_id: 163-180)
(6, 163, 900, 'AVAILABLE'),  (6, 164, 900, 'BOOKED'),     (6, 165, 900, 'BLOCKED'),
(6, 169, 550, 'BLOCKED'),    (6, 170, 550, 'AVAILABLE'),  (6, 171, 550, 'BOOKED'),
(6, 175, 350, 'BOOKED'),     (6, 176, 350, 'BLOCKED'),    (6, 177, 350, 'AVAILABLE'),

-- Flight 7 (18 ghế) - aircraft_id = 8 (seat_id: 127-144)
(7, 127, 900, 'AVAILABLE'),  (7, 128, 900, 'BOOKED'),     (7, 129, 900, 'BLOCKED'),    
(7, 130, 900, 'AVAILABLE'),  (7, 131, 900, 'BOOKED'),     (7, 132, 900, 'AVAILABLE'),
(7, 133, 550, 'BOOKED'),     (7, 134, 550, 'AVAILABLE'),  (7, 135, 550, 'BLOCKED'),    
(7, 136, 550, 'AVAILABLE'),  (7, 137, 550, 'BOOKED'),     (7, 138, 550, 'AVAILABLE'),
(7, 139, 350, 'BOOKED'),     (7, 140, 350, 'AVAILABLE'),  (7, 141, 350, 'BLOCKED'),    
(7, 142, 350, 'AVAILABLE'),  (7, 143, 350, 'BOOKED'),     (7, 144, 350, 'AVAILABLE'),

-- Flight 8 (9 ghế) - aircraft_id = 10 (seat_id: 163-180)
(8, 163, 900, 'AVAILABLE'),  (8, 164, 900, 'BOOKED'),     (8, 165, 900, 'BLOCKED'),
(8, 169, 550, 'BLOCKED'),    (8, 170, 550, 'AVAILABLE'),  (8, 171, 550, 'BOOKED'),
(8, 175, 350, 'BOOKED'),     (8, 176, 350, 'BLOCKED'),    (8, 177, 350, 'AVAILABLE'),

-- Flight 9 (18 ghế) - aircraft_id = 5 (seat_id: 73-90)
(9, 73, 900, 'AVAILABLE'),   (9, 74, 900, 'BOOKED'),      (9, 75, 900, 'BLOCKED'),     
(9, 76, 900, 'AVAILABLE'),   (9, 77, 900, 'BOOKED'),      (9, 78, 900, 'AVAILABLE'),
(9, 79, 550, 'BOOKED'),      (9, 80, 550, 'AVAILABLE'),   (9, 81, 550, 'BLOCKED'),     
(9, 82, 550, 'AVAILABLE'),   (9, 83, 550, 'BOOKED'),      (9, 84, 550, 'AVAILABLE'),
(9, 85, 350, 'BOOKED'),      (9, 86, 350, 'AVAILABLE'),   (9, 87, 350, 'BLOCKED'),     
(9, 88, 350, 'AVAILABLE'),   (9, 89, 350, 'BOOKED'),      (9, 90, 350, 'AVAILABLE'),

-- Flight 10 (18 ghế) - aircraft_id = 3 (seat_id: 37-54)
(10, 37, 900, 'AVAILABLE'),  (10, 38, 900, 'BOOKED'),     (10, 39, 900, 'BLOCKED'),    
(10, 40, 900, 'AVAILABLE'),  (10, 41, 900, 'BOOKED'),     (10, 42, 900, 'AVAILABLE'),
(10, 43, 550, 'BOOKED'),     (10, 44, 550, 'AVAILABLE'),  (10, 45, 550, 'BLOCKED'),    
(10, 46, 550, 'AVAILABLE'),  (10, 47, 550, 'BOOKED'),     (10, 48, 550, 'AVAILABLE'),
(10, 49, 350, 'BOOKED'),     (10, 50, 350, 'AVAILABLE'),  (10, 51, 350, 'BLOCKED'),    
(10, 52, 350, 'AVAILABLE'),  (10, 53, 350, 'BOOKED'),     (10, 54, 350, 'AVAILABLE'),

-- Flight 11 (18 ghế) - aircraft_id = 6 (seat_id: 91-108)
(11, 91, 900, 'AVAILABLE'),  (11, 92, 900, 'BOOKED'),     (11, 93, 900, 'BLOCKED'),    
(11, 94, 900, 'AVAILABLE'),  (11, 95, 900, 'BOOKED'),     (11, 96, 900, 'AVAILABLE'),
(11, 97, 550, 'BOOKED'),     (11, 98, 550, 'AVAILABLE'),  (11, 99, 550, 'BLOCKED'),    
(11, 100, 550, 'AVAILABLE'), (11, 101, 550, 'BOOKED'),    (11, 102, 550, 'AVAILABLE'),
(11, 103, 350, 'BOOKED'),    (11, 104, 350, 'AVAILABLE'), (11, 105, 350, 'BLOCKED'),   
(11, 106, 350, 'AVAILABLE'), (11, 107, 350, 'BOOKED'),    (11, 108, 350, 'AVAILABLE'),

-- Flight 12 (18 ghế) - aircraft_id = 5 (seat_id: 73-90)
(12, 73, 900, 'AVAILABLE'),  (12, 74, 900, 'BOOKED'),     (12, 75, 900, 'BLOCKED'),    
(12, 76, 900, 'AVAILABLE'),  (12, 77, 900, 'BOOKED'),     (12, 78, 900, 'AVAILABLE'),
(12, 79, 550, 'BOOKED'),     (12, 80, 550, 'AVAILABLE'),  (12, 81, 550, 'BLOCKED'),    
(12, 82, 550, 'AVAILABLE'),  (12, 83, 550, 'BOOKED'),     (12, 84, 550, 'AVAILABLE'),
(12, 85, 350, 'BOOKED'),     (12, 86, 350, 'AVAILABLE'),  (12, 87, 350, 'BLOCKED'),    
(12, 88, 350, 'AVAILABLE'),  (12, 89, 350, 'BOOKED'),     (12, 90, 350, 'AVAILABLE'),

-- Flight 13 (18 ghế) - aircraft_id = 5 (seat_id: 73-90)
(13, 73, 900, 'AVAILABLE'),  (13, 74, 900, 'BOOKED'),     (13, 75, 900, 'BLOCKED'),    
(13, 76, 900, 'AVAILABLE'),  (13, 77, 900, 'BOOKED'),     (13, 78, 900, 'AVAILABLE'),
(13, 79, 550, 'BOOKED'),     (13, 80, 550, 'AVAILABLE'),  (13, 81, 550, 'BLOCKED'),    
(13, 82, 550, 'AVAILABLE'),  (13, 83, 550, 'BOOKED'),     (13, 84, 550, 'AVAILABLE'),
(13, 85, 350, 'BOOKED'),     (13, 86, 350, 'AVAILABLE'),  (13, 87, 350, 'BLOCKED'),    
(13, 88, 350, 'AVAILABLE'),  (13, 89, 350, 'BOOKED'),     (13, 90, 350, 'AVAILABLE'),

-- Flight 14 (18 ghế) - aircraft_id = 4 (seat_id: 55-72)
(14, 55, 900, 'AVAILABLE'),  (14, 56, 900, 'BOOKED'),     (14, 57, 900, 'BLOCKED'),    
(14, 58, 900, 'AVAILABLE'),  (14, 59, 900, 'BOOKED'),     (14, 60, 900, 'AVAILABLE'),
(14, 61, 550, 'BOOKED'),     (14, 62, 550, 'AVAILABLE'),  (14, 63, 550, 'BLOCKED'),    
(14, 64, 550, 'AVAILABLE'),  (14, 65, 550, 'BOOKED'),     (14, 66, 550, 'AVAILABLE'),
(14, 67, 350, 'BOOKED'),     (14, 68, 350, 'AVAILABLE'),  (14, 69, 350, 'BLOCKED'),    
(14, 70, 350, 'AVAILABLE'),  (14, 71, 350, 'BOOKED'),     (14, 72, 350, 'AVAILABLE'),

-- Flight 15 (18 ghế) - aircraft_id = 4 (seat_id: 55-72)
(15, 55, 900, 'AVAILABLE'),  (15, 56, 900, 'BOOKED'),     (15, 57, 900, 'BLOCKED'),    
(15, 58, 900, 'AVAILABLE'),  (15, 59, 900, 'BOOKED'),     (15, 60, 900, 'AVAILABLE'),
(15, 61, 550, 'BOOKED'),     (15, 62, 550, 'AVAILABLE'),  (15, 63, 550, 'BLOCKED'),    
(15, 64, 550, 'AVAILABLE'),  (15, 65, 550, 'BOOKED'),     (15, 66, 550, 'AVAILABLE'),
(15, 67, 350, 'BOOKED'),     (15, 68, 350, 'AVAILABLE'),  (15, 69, 350, 'BLOCKED'),    
(15, 70, 350, 'AVAILABLE'),  (15, 71, 350, 'BOOKED'),     (15, 72, 350, 'AVAILABLE'),

-- Flight 16 (18 ghế) - aircraft_id = 4 (seat_id: 55-72)
(16, 55, 900, 'AVAILABLE'),  (16, 56, 900, 'BOOKED'),     (16, 57, 900, 'BLOCKED'),    
(16, 58, 900, 'AVAILABLE'),  (16, 59, 900, 'BOOKED'),     (16, 60, 900, 'AVAILABLE'),
(16, 61, 550, 'BOOKED'),     (16, 62, 550, 'AVAILABLE'),  (16, 63, 550, 'BLOCKED'),    
(16, 64, 550, 'AVAILABLE'),  (16, 65, 550, 'BOOKED'),     (16, 66, 550, 'AVAILABLE'),
(16, 67, 350, 'BOOKED'),     (16, 68, 350, 'AVAILABLE'),  (16, 69, 350, 'BLOCKED'),    
(16, 70, 350, 'AVAILABLE'),  (16, 71, 350, 'BOOKED'),     (16, 72, 350, 'AVAILABLE'),

-- Flight 17 (18 ghế) - aircraft_id = 4 (seat_id: 55-72)
(17, 55, 900, 'AVAILABLE'),  (17, 56, 900, 'BOOKED'),     (17, 57, 900, 'BLOCKED'),    
(17, 58, 900, 'AVAILABLE'),  (17, 59, 900, 'BOOKED'),     (17, 60, 900, 'AVAILABLE'),
(17, 61, 550, 'BOOKED'),     (17, 62, 550, 'AVAILABLE'),  (17, 63, 550, 'BLOCKED'),    
(17, 64, 550, 'AVAILABLE'),  (17, 65, 550, 'BOOKED'),     (17, 66, 550, 'AVAILABLE'),
(17, 67, 350, 'BOOKED'),     (17, 68, 350, 'AVAILABLE'),  (17, 69, 350, 'BLOCKED'),    
(17, 70, 350, 'AVAILABLE'),  (17, 71, 350, 'BOOKED'),     (17, 72, 350, 'AVAILABLE'),

-- Flight 18 (9 ghế) - aircraft_id = 7 (seat_id: 109-126)
(18, 109, 900, 'AVAILABLE'), (18, 110, 900, 'BOOKED'),    (18, 111, 900, 'BLOCKED'),
(18, 115, 550, 'BLOCKED'),   (18, 116, 550, 'AVAILABLE'), (18, 117, 550, 'BOOKED'),
(18, 121, 350, 'BOOKED'),    (18, 122, 350, 'AVAILABLE'), (18, 123, 350, 'BLOCKED'),

-- Flight 19 (9 ghế) - aircraft_id = 10 (seat_id: 163-180)
(19, 163, 900, 'AVAILABLE'), (19, 164, 900, 'BOOKED'),    (19, 165, 900, 'BLOCKED'),
(19, 169, 550, 'BLOCKED'),   (19, 170, 550, 'AVAILABLE'), (19, 171, 550, 'BOOKED'),
(19, 175, 350, 'BOOKED'),    (19, 176, 350, 'BLOCKED'),   (19, 177, 350, 'AVAILABLE'),

-- Flight 20 (18 ghế) - aircraft_id = 8 (seat_id: 127-144)
(20, 127, 900, 'AVAILABLE'), (20, 128, 900, 'BOOKED'),    (20, 129, 900, 'BLOCKED'),   
(20, 130, 900, 'AVAILABLE'), (20, 131, 900, 'BOOKED'),    (20, 132, 900, 'AVAILABLE'),
(20, 133, 550, 'BOOKED'),    (20, 134, 550, 'AVAILABLE'), (20, 135, 550, 'BLOCKED'),   
(20, 136, 550, 'AVAILABLE'), (20, 137, 550, 'BOOKED'),    (20, 138, 550, 'AVAILABLE'),
(20, 139, 350, 'BOOKED'),    (20, 140, 350, 'AVAILABLE'), (20, 141, 350, 'BLOCKED'),   
(20, 142, 350, 'AVAILABLE'), (20, 143, 350, 'BOOKED'),    (20, 144, 350, 'AVAILABLE');


-- 13. Quy tắc giá vé
CREATE TABLE Fare_Rules (
    rule_id INT AUTO_INCREMENT PRIMARY KEY,
    route_id INT,
    class_id INT,
    fare_type VARCHAR(50) DEFAULT 'Standard',
    season ENUM('PEAK','OFFPEAK','NORMAL') DEFAULT 'NORMAL',
    effective_date DATE,
    expiry_date DATE,
    `description` VARCHAR(255),
    price DECIMAL(12,2),
    FOREIGN KEY (route_id) REFERENCES Routes(route_id),
    FOREIGN KEY (class_id) REFERENCES Cabin_Classes(class_id)
);
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
CREATE TABLE Bookings (
    booking_id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT,
    booking_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    trip_type ENUM('ONE_WAY','MULTI_WAY', 'ROUND_TRIP') NOT NULL DEFAULT 'ONE_WAY',
    `status` ENUM('PENDING','CONFIRMED','CANCELLED','REFUNDED') DEFAULT 'PENDING',
    total_amount DECIMAL(12,2),
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id)
);
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

-- 15. Hồ sơ hành khách
CREATE TABLE Passenger_Profiles (
    profile_id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT,
    full_name VARCHAR(100),
    date_of_birth DATE,
    phone_number VARCHAR(255),
    passport_number VARCHAR(50),
    nationality VARCHAR(50),
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id)
);
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

-- 16. Hành khách trong từng booking
CREATE TABLE Booking_Passengers (
    booking_passenger_id INT AUTO_INCREMENT PRIMARY KEY,
    booking_id INT,
    profile_id INT,
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id),
    FOREIGN KEY (profile_id) REFERENCES Passenger_Profiles(profile_id)
);
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
CREATE TABLE Tickets (
    ticket_id INT AUTO_INCREMENT PRIMARY KEY,
    ticket_passenger_id INT,
    flight_seat_id INT,
    ticket_number VARCHAR(50) UNIQUE,
    issue_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    segment_no INT NULL,
    segment_type ENUM('OUTBOUND','INBOUND') NULL,
    `status` ENUM('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED') DEFAULT 'BOOKED',
    `total_price` decimal(10,2) NOT NULL DEFAULT 0.00,
    FOREIGN KEY (ticket_passenger_id) REFERENCES Booking_Passengers(booking_passenger_id),
    FOREIGN KEY (flight_seat_id) REFERENCES Flight_Seats(flight_seat_id)
);
INSERT INTO Tickets (ticket_id, ticket_passenger_id, flight_seat_id, ticket_number, segment_no, segment_type, `status`,`total_price`) VALUES
-- Flight 2 (flight_seat_id từ Flight_Seats của flight_id=2)
(1,  1,  1,  'TK202500001', 1, 'OUTBOUND', 'CONFIRMED', 900.00),   -- seat 129 (1A First)
(2,  2,  2,  'TK202500002', 1, 'OUTBOUND', 'BOOKED', 550.00),      -- seat 130 (1B First)
(3,  3,  3,  'TK202500003', 1, 'OUTBOUND', 'BOOKED', 350.00),      -- seat 133 (1E Business)
(4,  4,  4,  'TK202500004', 1, 'OUTBOUND', 'CONFIRMED', 200.00),   -- seat 134 (1F Business)
(5,  5,  5,  'TK202500005', 1, 'OUTBOUND', 'BOOKED', 900.00),      -- seat 137 (2A Premium Economy)
(6,  6,  6,  'TK202500006', 1, 'OUTBOUND', 'BOOKED', 550.00),      -- seat 138 (2B Premium Economy)
(7,  7,  7,  'TK202500007', 1, 'OUTBOUND', 'CONFIRMED', 350.00),   -- seat 141 (2E Economy)
(8,  8,  8,  'TK202500008', 1, 'OUTBOUND', 'BOOKED', 200.00),      -- seat 142 (2F Economy)

-- Flight 4 (flight_seat_id từ Flight_Seats của flight_id=4)
(9,  9,  9,  'TK202500009', 1, 'OUTBOUND', 'BOOKED', 900.00),      -- seat 1 (1A First)
(10, 10, 10, 'TK202500010', 1, 'OUTBOUND', 'CONFIRMED', 550.00),   -- seat 4 (1D First)
(11, 11, 11, 'TK202500011', 1, 'OUTBOUND', 'BOOKED', 350.00),      -- seat 5 (1E Business)
(12, 12, 12, 'TK202500012', 1, 'OUTBOUND', 'BOOKED', 200.00),      -- seat 8 (1H Business)
(13, 13, 13, 'TK202500013', 1, 'OUTBOUND', 'CHECKED_IN', 900.00),  -- seat 9 (2A Premium Economy)
(14, 14, 14, 'TK202500014', 1, 'OUTBOUND', 'CONFIRMED', 550.00),   -- seat 12 (2D Premium Economy)

-- Flight 7 (flight_seat_id từ Flight_Seats của flight_id=7)
(15, 15, 15, 'TK202500015', 1, 'OUTBOUND', 'BOOKED', 900.00),      -- seat 113 (1A First)
(16, 16, 16, 'TK202500016', 1, 'OUTBOUND', 'BOARDED', 550.00),     -- seat 114 (1B First)
(17, 17, 17, 'TK202500017', 1, 'OUTBOUND', 'CONFIRMED', 350.00),   -- seat 115 (1C First)
(18, 18, 18, 'TK202500018', 1, 'OUTBOUND', 'BOOKED', 200.00),      -- seat 116 (1D First)
(19, 19, 19, 'TK202500019', 1, 'OUTBOUND', 'CHECKED_IN', 900.00),  -- seat 117 (1E Business)
(20, 20, 20, 'TK202500020', 1, 'OUTBOUND', 'BOOKED', 550.00),      -- seat 118 (1F Business)

-- Flight 11 (flight_seat_id từ Flight_Seats của flight_id=11)
(21, 1,  21, 'TK202500021', 1, 'OUTBOUND', 'CONFIRMED', 900.00),   -- seat 81 (1A First)
(22, 2,  22, 'TK202500022', 1, 'OUTBOUND', 'BOOKED', 550.00),      -- seat 82 (1B First)
(23, 3,  23, 'TK202500023', 1, 'OUTBOUND', 'CHECKED_IN', 350.00),  -- seat 83 (1C First)
(24, 4,  24, 'TK202500024', 1, 'OUTBOUND', 'CONFIRMED', 200.00),   -- seat 84 (1D First)
(25, 5,  25, 'TK202500025', 1, 'OUTBOUND', 'BOOKED', 900.00),      -- seat 85 (1E Business)

-- Flight 18 (flight_seat_id từ Flight_Seats của flight_id=18)
(26, 6,  26, 'TK202500026', 1, 'OUTBOUND', 'CONFIRMED', 900.00),   -- seat 97 (1A First)
(27, 7,  27, 'TK202500027', 1, 'OUTBOUND', 'BOOKED', 550.00),      -- seat 98 (1B First)
(28, 8,  28, 'TK202500028', 1, 'OUTBOUND', 'CHECKED_IN', 350.00),  -- seat 101 (1E Business)
(29, 9,  29, 'TK202500029', 1, 'OUTBOUND', 'CONFIRMED', 200.00),   -- seat 102 (1F Business)
(30, 10, 30, 'TK202500030', 1, 'OUTBOUND', 'BOOKED', 900.00);      -- seat 105 (2A Premium Economy)


-- 18. Lịch sử trạng thái vé
CREATE TABLE Ticket_History (
    history_id INT AUTO_INCREMENT PRIMARY KEY,
    ticket_id INT,
    old_status ENUM('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED'),
    new_status ENUM('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED'),
    changed_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ticket_id) REFERENCES Tickets(ticket_id)
);
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
CREATE TABLE Payments (
    payment_id INT AUTO_INCREMENT PRIMARY KEY,
    booking_id INT,
    amount DECIMAL(12,2),
    payment_method ENUM('CREDIT_CARD','BANK_TRANSFER','E_WALLET','CASH'),
    payment_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    `status` ENUM('SUCCESS','FAILED','PENDING') DEFAULT 'PENDING',
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
);
INSERT INTO Payments (booking_id, amount, payment_method, payment_date, `status`) VALUES
(1,   350.00,  'CREDIT_CARD',   '2024-11-05 11:00:00', 'SUCCESS'),
(2,   720.00,  'E_WALLET',      '2025-01-14 08:30:00', 'PENDING'),
(3,   200.00,  'CASH',          '2024-12-22 20:00:00', 'FAILED'),
(4,   1120.00, 'BANK_TRANSFER', '2025-02-03 15:10:00', 'SUCCESS'),
(5,   650.00,  'CREDIT_CARD',   '2024-10-29 10:10:00', 'PENDING'),
(6,   450.00,  'CASH',          '2025-03-11 18:00:00', 'PENDING'),
(7,   900.00,  'BANK_TRANSFER', '2024-09-13 12:00:00', 'PENDING'),
(8,   200.00,  'E_WALLET',      '2025-04-20 20:30:00', 'SUCCESS'),
(9,   740.00,  'CREDIT_CARD',   '2024-08-18 07:10:00', 'PENDING'),
(10,  300.00,  'CASH',          '2025-02-27 16:00:00', 'PENDING'),
(11,  1050.00, 'E_WALLET',      '2024-07-09 14:00:00', 'PENDING'),
(12,  880.00,  'CREDIT_CARD',   '2025-01-25 18:45:00', 'SUCCESS'),
(13,  220.00,  'BANK_TRANSFER', '2024-06-30 21:30:00', 'FAILED'),
(14,  990.00,  'E_WALLET',      '2025-03-02 13:00:00', 'SUCCESS'),
(15,  250.00,  'CREDIT_CARD',   '2024-09-21 09:00:00', 'FAILED');

-- 20. Hành lý
CREATE TABLE Baggage (
    baggage_id INT AUTO_INCREMENT PRIMARY KEY,
    ticket_id INT NOT NULL,
    flight_id INT NOT NULL,
    baggage_tag VARCHAR(20) UNIQUE,
    baggage_type ENUM('CHECKED','CARRY_ON','SPECIAL') DEFAULT 'CHECKED',
    weight_kg DECIMAL(5,2) NOT NULL,
    allowed_weight_kg DECIMAL(5,2) NOT NULL,
    fee DECIMAL(12,2) DEFAULT 0.00,
    `status` ENUM('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST') DEFAULT 'CREATED',
    special_handling VARCHAR(100),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (ticket_id) REFERENCES Tickets(ticket_id),
    FOREIGN KEY (flight_id) REFERENCES Flights(flight_id)
);
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
CREATE TABLE Baggage_History (
    history_id INT AUTO_INCREMENT PRIMARY KEY,
    baggage_id INT NOT NULL,
    old_status ENUM('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST'),
    new_status ENUM('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST'),
    changed_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (baggage_id) REFERENCES Baggage(baggage_id)
);
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

-- Carry-on baggage
CREATE TABLE carryon_baggage (
  carryon_id INT AUTO_INCREMENT PRIMARY KEY,
  weight_kg INT NOT NULL,
  class_id INT NOT NULL,
  size_limit VARCHAR(50),
  description VARCHAR(255),
  is_default TINYINT(1) DEFAULT 1,
  FOREIGN KEY (class_id) REFERENCES Cabin_Classes(class_id)
);
INSERT INTO carryon_baggage (weight_kg, class_id, size_limit, description, is_default) VALUES
-- Default carry-on theo class
(14, 1, '56x36x23 cm', 'Xách tay mặc định First Class 14kg', 1),
(10, 2, '56x36x23 cm', 'Xách tay mặc định Business 10kg', 1),
(10, 3, '56x36x23 cm', 'Xách tay mặc định Premium Economy 10kg', 1),
(7,  4, '56x36x23 cm', 'Xách tay mặc định Economy 7kg', 1),

-- Extra items (tùy chọn mua thêm)
(3,  4, '40x30x15 cm', 'Túi cá nhân nhỏ 3kg', 0),
(5,  4, '45x35x20 cm', 'Balô nhỏ 5kg', 0);

-- Checked baggage
CREATE TABLE checked_baggage (
  checked_id INT AUTO_INCREMENT PRIMARY KEY,
  weight_kg INT NOT NULL,
  price DECIMAL(10,2) NOT NULL,
  description VARCHAR(255)
);
INSERT INTO checked_baggage (weight_kg, price, description) VALUES
(10, 200000, 'Gói ký gửi 10kg – tiêu chuẩn'),
(15, 320000, 'Gói ký gửi 15kg'),
(20, 450000, 'Gói ký gửi 20kg – phổ biến nhất'),
(25, 580000, 'Gói ký gửi 25kg'),
(30, 720000, 'Gói ký gửi 30kg – hành lý nhiều'),
(35, 850000, 'Gói ký gửi 35kg'),
(40, 990000, 'Gói ký gửi 40kg – tối đa theo quy định');

-- National
CREATE TABLE national (
  national_id INT AUTO_INCREMENT PRIMARY KEY,
  country_name VARCHAR(100) NOT NULL UNIQUE,
  country_code CHAR(2) NOT NULL UNIQUE,
  phone_code VARCHAR(5)
);
INSERT INTO national (country_name, country_code, phone_code) VALUES
('United States', 'US', '+1'),
('Japan', 'JP', '+81'),
('South Korea', 'KR', '+82'),
('France', 'FR', '+33'),
('Vietnam', 'VN', '+84');

-- Ticket baggage linking
CREATE TABLE ticket_baggage (
  id INT AUTO_INCREMENT PRIMARY KEY,
  ticket_id INT NOT NULL,
  baggage_type ENUM('carry_on','checked') NOT NULL,
  carryon_id INT NULL,
  checked_id INT NULL,
  quantity INT DEFAULT 1,
  note VARCHAR(255),
  FOREIGN KEY (ticket_id) REFERENCES Tickets(ticket_id) ON DELETE CASCADE,
  FOREIGN KEY (carryon_id) REFERENCES carryon_baggage(carryon_id),
  FOREIGN KEY (checked_id) REFERENCES checked_baggage(checked_id),
  CONSTRAINT chk_baggage_valid CHECK (
      (baggage_type = 'carry_on' AND carryon_id IS NOT NULL AND checked_id IS NULL)
      OR
      (baggage_type = 'checked' AND checked_id IS NOT NULL AND carryon_id IS NULL)
  )
);
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

-- Update existing flights with default price (if any exist)
UPDATE Flights SET base_price = 1000000 WHERE base_price = 0;

-- Create index for better query performance
CREATE INDEX idx_flights_is_deleted ON Flights(is_deleted);

-- Update existing data to ensure all flights are marked as not deleted
UPDATE Flights SET is_deleted = FALSE WHERE is_deleted IS NULL;

SELECT 'Migration completed successfully!' AS Status;

--
-- Cấu trúc bảng cho bảng `ticket_refund_policy`
--

CREATE TABLE `ticket_refund_policy` (
  `policy_id` int(11) NOT NULL,
  `class_id` int(11) NOT NULL,
  `is_refundable` tinyint(1) NOT NULL,
  `refund_fee_percent` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `ticket_refund_policy`
--

INSERT INTO `ticket_refund_policy` (`policy_id`, `class_id`, `is_refundable`, `refund_fee_percent`) VALUES
(1, 1, 1, 0),
(2, 2, 1, 10),
(3, 3, 1, 20),
(4, 4, 0, 100);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `ticket_status_history`
--

CREATE TABLE `ticket_status_history` (
  `id` int(11) NOT NULL,
  `ticket_id` int(11) NOT NULL,
  `old_status` enum('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED') DEFAULT NULL,
  `new_status` enum('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED') DEFAULT NULL,
  `changed_by` int(11) DEFAULT NULL,
  `change_reason` varchar(255) DEFAULT NULL,
  `changed_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;


--
-- Các ràng buộc cho bảng `ticket_refund_policy`
--
ALTER TABLE `ticket_refund_policy`
  ADD CONSTRAINT `fk_trp_class` FOREIGN KEY (`class_id`) REFERENCES `cabin_classes` (`class_id`);

--
-- Các ràng buộc cho bảng `ticket_status_history`
--
ALTER TABLE `ticket_status_history`
  ADD CONSTRAINT `ticket_status_history_ibfk_1` FOREIGN KEY (`ticket_id`) REFERENCES `tickets` (`ticket_id`);
COMMIT;
