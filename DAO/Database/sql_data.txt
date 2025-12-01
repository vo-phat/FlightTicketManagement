-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th10 27, 2025 lúc 02:26 PM
-- Phiên bản máy phục vụ: 10.4.32-MariaDB
-- Phiên bản PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `flightticketmanagement`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `accounts`
--

CREATE TABLE `accounts` (
  `account_id` int(11) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(255) NOT NULL,
  `failed_attempts` int(11) NOT NULL DEFAULT 5,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `created_at` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `accounts`
--

INSERT INTO `accounts` (`account_id`, `email`, `password`, `failed_attempts`, `is_active`, `created_at`) VALUES
(1, 'admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, '2025-11-27 19:01:15'),
(2, 'staff@gmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, '2025-11-27 19:01:15'),
(3, 'user@gmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, '2025-11-27 19:01:15'),
(4, 'abc@gmail.com', 'dd130a849d7b29e5541b05d2f7f86a4acd4f1ec598c1c9438783f56bc4f0ff80', 5, 1, '2025-11-27 19:01:45');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `account_role`
--

CREATE TABLE `account_role` (
  `account_id` int(11) NOT NULL,
  `role_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `account_role`
--

INSERT INTO `account_role` (`account_id`, `role_id`) VALUES
(1, 3),
(2, 2),
(3, 1),
(4, 3);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `aircrafts`
--

CREATE TABLE `aircrafts` (
  `aircraft_id` int(11) NOT NULL,
  `airline_id` int(11) DEFAULT NULL,
  `model` varchar(100) DEFAULT NULL,
  `manufacturer` varchar(100) DEFAULT NULL,
  `capacity` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `aircrafts`
--

INSERT INTO `aircrafts` (`aircraft_id`, `airline_id`, `model`, `manufacturer`, `capacity`) VALUES
(1, 1, 'Boeing 787-9', 'Boeing', 296),
(2, 1, 'Airbus A321neo', 'Airbus', 220),
(3, 2, 'Boeing 737 MAX 8', 'Boeing', 189),
(4, 2, 'Airbus A350-900', 'Airbus', 305),
(5, 3, 'ATR 72-600', 'ATR', 78),
(6, 1, 'Boeing 777-300ER', 'Boeing', 364);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `airlines`
--

CREATE TABLE `airlines` (
  `airline_id` int(11) NOT NULL,
  `airline_code` varchar(10) NOT NULL,
  `airline_name` varchar(100) NOT NULL,
  `country` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `airports`
--

CREATE TABLE `airports` (
  `airport_id` int(11) NOT NULL,
  `airport_code` varchar(10) NOT NULL,
  `airport_name` varchar(100) NOT NULL,
  `city` varchar(100) DEFAULT NULL,
  `country` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `airports`
--

INSERT INTO `airports` (`airport_id`, `airport_code`, `airport_name`, `city`, `country`) VALUES
(1, 'HAN', 'Noi Bai International Airport', 'Hanoi', 'Vietnam'),
(2, 'SGN', 'Tan Son Nhat International Airport', 'Ho Chi Minh City', 'Vietnam'),
(3, 'DAD', 'Da Nang International Airport', 'Da Nang', 'Vietnam'),
(4, 'PQC', 'Phu Quoc International Airport', 'Phu Quoc', 'Vietnam'),
(5, 'CXR', 'Cam Ranh International Airport', 'Nha Trang', 'Vietnam'),
(6, 'HPH', 'Cat Bi International Airport', 'Hai Phong', 'Vietnam');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `baggage`
--

CREATE TABLE `baggage` (
  `baggage_id` int(11) NOT NULL,
  `ticket_id` int(11) NOT NULL,
  `flight_id` int(11) NOT NULL,
  `baggage_tag` varchar(20) DEFAULT NULL,
  `baggage_type` enum('CHECKED','CARRY_ON','SPECIAL') DEFAULT 'CHECKED',
  `weight_kg` decimal(5,2) NOT NULL,
  `allowed_weight_kg` decimal(5,2) NOT NULL,
  `fee` decimal(12,2) DEFAULT 0.00,
  `status` enum('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST') DEFAULT 'CREATED',
  `special_handling` varchar(100) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp(),
  `updated_at` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `baggage_history`
--

CREATE TABLE `baggage_history` (
  `history_id` int(11) NOT NULL,
  `baggage_id` int(11) NOT NULL,
  `old_status` enum('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST') DEFAULT NULL,
  `new_status` enum('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST') DEFAULT NULL,
  `changed_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `bookings`
--

CREATE TABLE `bookings` (
  `booking_id` int(11) NOT NULL,
  `account_id` int(11) DEFAULT NULL,
  `booking_date` datetime DEFAULT current_timestamp(),
  `trip_type` enum('ONE_WAY','ROUND_TRIP') NOT NULL DEFAULT 'ONE_WAY',
  `status` enum('PENDING','CONFIRMED','CANCELLED','REFUNDED') DEFAULT 'PENDING',
  `total_amount` decimal(12,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `booking_passengers`
--

CREATE TABLE `booking_passengers` (
  `booking_passenger_id` int(11) NOT NULL,
  `booking_id` int(11) DEFAULT NULL,
  `profile_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `cabin_classes`
--

CREATE TABLE `cabin_classes` (
  `class_id` int(11) NOT NULL,
  `class_name` varchar(50) NOT NULL,
  `description` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `cabin_classes`
--

INSERT INTO `cabin_classes` (`class_id`, `class_name`, `description`) VALUES
(1, 'Economy', 'Economy class description'),
(2, 'Premium Economy', 'Premium Economy class description'),
(3, 'Business', 'Business class description'),
(4, 'First', 'First class description');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `fare_rules`
--

CREATE TABLE `fare_rules` (
  `rule_id` int(11) NOT NULL,
  `route_id` int(11) DEFAULT NULL,
  `class_id` int(11) DEFAULT NULL,
  `fare_type` varchar(50) DEFAULT 'Standard',
  `season` enum('PEAK','OFFPEAK','NORMAL') DEFAULT 'NORMAL',
  `effective_date` date DEFAULT NULL,
  `expiry_date` date DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `price` decimal(12,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `fare_rules`
--

INSERT INTO `fare_rules` (`rule_id`, `route_id`, `class_id`, `fare_type`, `season`, `effective_date`, `expiry_date`, `description`, `price`) VALUES
(3, 3, 1, 'Standard', 'PEAK', '2022-03-19', '2022-11-18', 'Fare rule for Standard in PEAK', 973.44),
(4, 4, 3, 'Flex', 'OFFPEAK', '2021-03-10', '2021-07-09', 'Fare rule for Flex in OFFPEAK', 786.46),
(5, 3, 4, 'Saver', 'OFFPEAK', '2024-02-01', '2024-06-20', 'Fare rule for Saver in OFFPEAK', 926.22),
(6, 5, 2, 'Flex', 'PEAK', '2020-07-28', '2021-02-10', 'Fare rule for Flex in PEAK', 672.53),
(7, 2, 1, 'Standard', 'PEAK', '2025-03-01', '2025-07-13', 'Fare rule for Standard in PEAK', 718.73),
(8, 4, 2, 'Promo', 'PEAK', '2022-08-10', '2023-02-01', 'Fare rule for Promo in PEAK', 825.60),
(9, 9, 1, 'Promo', 'NORMAL', '2021-04-06', '2022-01-28', 'Fare rule for Promo in NORMAL', 391.57),
(10, 5, 1, 'Standard', 'NORMAL', '2025-05-01', '2025-08-27', 'Fare rule for Standard in NORMAL', 424.46),
(11, 6, 4, 'Promo', 'NORMAL', '2020-12-18', '2021-07-19', 'Fare rule for Promo in NORMAL', 880.66),
(12, 7, 3, 'Saver', 'NORMAL', '2022-06-22', '2022-09-14', 'Fare rule for Saver in NORMAL', 363.30),
(13, 8, 3, 'Flex', 'NORMAL', '2023-04-26', '2023-12-05', 'Fare rule for Flex in NORMAL', 915.35);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `flights`
--

CREATE TABLE `flights` (
  `flight_id` int(11) NOT NULL,
  `flight_number` varchar(20) NOT NULL,
  `aircraft_id` int(11) DEFAULT NULL,
  `route_id` int(11) DEFAULT NULL,
  `departure_time` datetime DEFAULT NULL,
  `arrival_time` datetime DEFAULT NULL,
  `status` enum('SCHEDULED','DELAYED','CANCELLED','COMPLETED') DEFAULT 'SCHEDULED'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `flights`
--

INSERT INTO `flights` (`flight_id`, `flight_number`, `aircraft_id`, `route_id`, `departure_time`, `arrival_time`, `status`) VALUES
(1, 'VN101', 1, 10, '2025-11-28 06:00:00', '2025-11-28 07:53:00', 'SCHEDULED'),
(2, 'VN102', 2, 2, '2025-11-28 08:30:00', '2025-11-28 12:20:00', 'SCHEDULED'),
(3, 'VJ201', 3, 4, '2025-11-28 10:00:00', '2025-11-28 11:30:00', 'SCHEDULED'),
(4, 'VN103', 4, 3, '2025-11-28 13:00:00', '2025-11-28 14:36:00', 'SCHEDULED'),
(5, 'VJ202', 2, 5, '2025-11-28 15:30:00', '2025-11-28 19:18:00', 'SCHEDULED'),
(6, 'VN104', 1, 6, '2025-11-29 07:00:00', '2025-11-29 08:42:00', 'SCHEDULED'),
(7, 'VJ203', 3, 7, '2025-11-29 09:30:00', '2025-11-29 11:03:00', 'SCHEDULED'),
(8, 'VN105', 6, 8, '2025-11-29 12:00:00', '2025-11-29 16:25:00', 'SCHEDULED'),
(9, 'VJ204', 5, 9, '2025-11-29 14:30:00', '2025-11-29 16:01:00', 'SCHEDULED'),
(10, 'VN106', 4, 11, '2025-11-29 17:00:00', '2025-11-29 21:28:00', 'DELAYED'),
(11, 'VN107', 1, 10, '2025-11-30 06:00:00', '2025-11-30 07:53:00', 'SCHEDULED'),
(12, 'VJ205', 2, 2, '2025-11-30 08:00:00', '2025-11-30 11:50:00', 'SCHEDULED'),
(13, 'VN108', 3, 4, '2025-11-30 10:30:00', '2025-11-30 12:00:00', 'CANCELLED'),
(14, 'VJ206', 4, 3, '2025-11-30 13:30:00', '2025-11-30 15:06:00', 'SCHEDULED'),
(15, 'VN109', 6, 5, '2025-11-30 16:00:00', '2025-11-30 19:48:00', 'SCHEDULED');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `flight_seats`
--

CREATE TABLE `flight_seats` (
  `flight_seat_id` int(11) NOT NULL,
  `flight_id` int(11) DEFAULT NULL,
  `seat_id` int(11) DEFAULT NULL,
  `base_price` decimal(12,2) DEFAULT NULL,
  `seat_status` enum('AVAILABLE','BOOKED','BLOCKED') DEFAULT 'AVAILABLE'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `passenger_profiles`
--

CREATE TABLE `passenger_profiles` (
  `profile_id` int(11) NOT NULL,
  `account_id` int(11) DEFAULT NULL,
  `full_name` varchar(100) DEFAULT NULL,
  `date_of_birth` date DEFAULT NULL,
  `phone_number` varchar(255) DEFAULT NULL,
  `passport_number` varchar(50) DEFAULT NULL,
  `nationality` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `passenger_profiles`
--

INSERT INTO `passenger_profiles` (`profile_id`, `account_id`, `full_name`, `date_of_birth`, `phone_number`, `passport_number`, `nationality`) VALUES
(1, 4, NULL, NULL, NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `payments`
--

CREATE TABLE `payments` (
  `payment_id` int(11) NOT NULL,
  `booking_id` int(11) DEFAULT NULL,
  `amount` decimal(12,2) DEFAULT NULL,
  `payment_method` enum('CREDIT_CARD','BANK_TRANSFER','E_WALLET','CASH') DEFAULT NULL,
  `payment_date` datetime DEFAULT current_timestamp(),
  `status` enum('SUCCESS','FAILED','PENDING') DEFAULT 'PENDING'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `permissions`
--

CREATE TABLE `permissions` (
  `permission_id` int(11) NOT NULL,
  `permission_code` varchar(100) NOT NULL,
  `permission_name` varchar(150) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `permissions`
--

INSERT INTO `permissions` (`permission_id`, `permission_code`, `permission_name`) VALUES
(1, 'home.view', 'Xem trang chủ'),
(2, 'flights.read', 'Tra cứu chuyến bay'),
(3, 'flights.create', 'Tạo chuyến bay'),
(4, 'fare_rules.manage', 'Quản lý quy tắc giá vé'),
(5, 'tickets.create_search', 'Đặt chỗ'),
(6, 'tickets.mine', 'Xem đặt chỗ của tôi'),
(7, 'tickets.operate', 'Vận hành vé'),
(8, 'tickets.history', 'Xem lịch sử vé'),
(9, 'baggage.checkin', 'Check-in hành lý'),
(10, 'baggage.track', 'Theo dõi hành lý'),
(11, 'baggage.report', 'Báo cáo hành lý thất lạc'),
(12, 'catalogs.airlines', 'Quản lý hãng hàng không'),
(13, 'catalogs.aircrafts', 'Quản lý máy bay'),
(14, 'catalogs.airports', 'Quản lý sân bay'),
(15, 'catalogs.routes', 'Quản lý tuyến bay'),
(16, 'catalogs.cabin_classes', 'Quản lý hạng vé'),
(17, 'catalogs.seats', 'Quản lý ghế máy bay'),
(18, 'payments.pos', 'Thanh toán POS'),
(19, 'customers.profiles', 'Hồ sơ khách hàng'),
(20, 'accounts.manage', 'Quản lý tài khoản & phân quyền'),
(21, 'notifications.read', 'Xem thông báo'),
(22, 'reports.view', 'Xem báo cáo'),
(23, 'system.roles', 'Cấu hình vai trò & phân quyền');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `roles`
--

CREATE TABLE `roles` (
  `role_id` int(11) NOT NULL,
  `role_code` varchar(50) NOT NULL,
  `role_name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `roles`
--

INSERT INTO `roles` (`role_id`, `role_code`, `role_name`) VALUES
(1, 'USER', 'Người dùng'),
(2, 'STAFF', 'Nhân viên'),
(3, 'ADMIN', 'Quản trị viên');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `role_permissions`
--

CREATE TABLE `role_permissions` (
  `role_id` int(11) NOT NULL,
  `permission_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `role_permissions`
--

INSERT INTO `role_permissions` (`role_id`, `permission_id`) VALUES
(1, 1),
(1, 5),
(1, 6),
(1, 19),
(1, 21),
(2, 1),
(2, 2),
(2, 5),
(2, 7),
(2, 8),
(2, 9),
(2, 10),
(2, 18),
(2, 19),
(2, 21),
(2, 22),
(3, 1),
(3, 2),
(3, 3),
(3, 4),
(3, 5),
(3, 6),
(3, 7),
(3, 8),
(3, 9),
(3, 10),
(3, 11),
(3, 12),
(3, 13),
(3, 14),
(3, 15),
(3, 16),
(3, 17),
(3, 18),
(3, 19),
(3, 20),
(3, 21),
(3, 22),
(3, 23);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `routes`
--

CREATE TABLE `routes` (
  `route_id` int(11) NOT NULL,
  `departure_place_id` int(11) DEFAULT NULL,
  `arrival_place_id` int(11) DEFAULT NULL,
  `distance_km` int(11) DEFAULT NULL,
  `duration_minutes` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `routes`
--

INSERT INTO `routes` (`route_id`, `departure_place_id`, `arrival_place_id`, `distance_km`, `duration_minutes`) VALUES
(2, 4, 3, 1802, 230),
(3, 4, 3, 1689, 96),
(4, 1, 5, 1858, 90),
(5, 4, 2, 644, 228),
(6, 5, 4, 812, 102),
(7, 3, 5, 1882, 93),
(8, 3, 4, 532, 265),
(9, 1, 4, 633, 91),
(10, 2, 1, 1574, 113),
(11, 5, 2, 1794, 268);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `seats`
--

CREATE TABLE `seats` (
  `seat_id` int(11) NOT NULL,
  `aircraft_id` int(11) DEFAULT NULL,
  `seat_number` varchar(10) DEFAULT NULL,
  `class_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `tickets`
--

CREATE TABLE `tickets` (
  `ticket_id` int(11) NOT NULL,
  `ticket_passenger_id` int(11) DEFAULT NULL,
  `flight_seat_id` int(11) DEFAULT NULL,
  `ticket_number` varchar(50) DEFAULT NULL,
  `issue_date` datetime DEFAULT current_timestamp(),
  `segment_no` int(11) DEFAULT NULL,
  `segment_type` enum('OUTBOUND','INBOUND') DEFAULT NULL,
  `status` enum('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED') DEFAULT 'BOOKED'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `ticket_history`
--

CREATE TABLE `ticket_history` (
  `history_id` int(11) NOT NULL,
  `ticket_id` int(11) DEFAULT NULL,
  `old_status` enum('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED') DEFAULT NULL,
  `new_status` enum('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED') DEFAULT NULL,
  `changed_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`account_id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Chỉ mục cho bảng `account_role`
--
ALTER TABLE `account_role`
  ADD PRIMARY KEY (`account_id`,`role_id`),
  ADD KEY `role_id` (`role_id`);

--
-- Chỉ mục cho bảng `aircrafts`
--
ALTER TABLE `aircrafts`
  ADD PRIMARY KEY (`aircraft_id`),
  ADD KEY `airline_id` (`airline_id`);

--
-- Chỉ mục cho bảng `airlines`
--
ALTER TABLE `airlines`
  ADD PRIMARY KEY (`airline_id`),
  ADD UNIQUE KEY `airline_code` (`airline_code`);

--
-- Chỉ mục cho bảng `airports`
--
ALTER TABLE `airports`
  ADD PRIMARY KEY (`airport_id`),
  ADD UNIQUE KEY `airport_code` (`airport_code`);

--
-- Chỉ mục cho bảng `baggage`
--
ALTER TABLE `baggage`
  ADD PRIMARY KEY (`baggage_id`),
  ADD UNIQUE KEY `baggage_tag` (`baggage_tag`),
  ADD KEY `ticket_id` (`ticket_id`),
  ADD KEY `flight_id` (`flight_id`);

--
-- Chỉ mục cho bảng `baggage_history`
--
ALTER TABLE `baggage_history`
  ADD PRIMARY KEY (`history_id`),
  ADD KEY `baggage_id` (`baggage_id`);

--
-- Chỉ mục cho bảng `bookings`
--
ALTER TABLE `bookings`
  ADD PRIMARY KEY (`booking_id`),
  ADD KEY `account_id` (`account_id`);

--
-- Chỉ mục cho bảng `booking_passengers`
--
ALTER TABLE `booking_passengers`
  ADD PRIMARY KEY (`booking_passenger_id`),
  ADD KEY `booking_id` (`booking_id`),
  ADD KEY `profile_id` (`profile_id`);

--
-- Chỉ mục cho bảng `cabin_classes`
--
ALTER TABLE `cabin_classes`
  ADD PRIMARY KEY (`class_id`);

--
-- Chỉ mục cho bảng `fare_rules`
--
ALTER TABLE `fare_rules`
  ADD PRIMARY KEY (`rule_id`),
  ADD KEY `route_id` (`route_id`),
  ADD KEY `class_id` (`class_id`);

--
-- Chỉ mục cho bảng `flights`
--
ALTER TABLE `flights`
  ADD PRIMARY KEY (`flight_id`),
  ADD KEY `aircraft_id` (`aircraft_id`),
  ADD KEY `route_id` (`route_id`);

--
-- Chỉ mục cho bảng `flight_seats`
--
ALTER TABLE `flight_seats`
  ADD PRIMARY KEY (`flight_seat_id`),
  ADD KEY `flight_id` (`flight_id`),
  ADD KEY `seat_id` (`seat_id`);

--
-- Chỉ mục cho bảng `passenger_profiles`
--
ALTER TABLE `passenger_profiles`
  ADD PRIMARY KEY (`profile_id`),
  ADD KEY `account_id` (`account_id`);

--
-- Chỉ mục cho bảng `payments`
--
ALTER TABLE `payments`
  ADD PRIMARY KEY (`payment_id`),
  ADD KEY `booking_id` (`booking_id`);

--
-- Chỉ mục cho bảng `permissions`
--
ALTER TABLE `permissions`
  ADD PRIMARY KEY (`permission_id`),
  ADD UNIQUE KEY `permission_code` (`permission_code`);

--
-- Chỉ mục cho bảng `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`role_id`),
  ADD UNIQUE KEY `role_code` (`role_code`);

--
-- Chỉ mục cho bảng `role_permissions`
--
ALTER TABLE `role_permissions`
  ADD PRIMARY KEY (`role_id`,`permission_id`),
  ADD KEY `permission_id` (`permission_id`);

--
-- Chỉ mục cho bảng `routes`
--
ALTER TABLE `routes`
  ADD PRIMARY KEY (`route_id`),
  ADD KEY `departure_place_id` (`departure_place_id`),
  ADD KEY `arrival_place_id` (`arrival_place_id`);

--
-- Chỉ mục cho bảng `seats`
--
ALTER TABLE `seats`
  ADD PRIMARY KEY (`seat_id`),
  ADD KEY `aircraft_id` (`aircraft_id`),
  ADD KEY `class_id` (`class_id`);

--
-- Chỉ mục cho bảng `tickets`
--
ALTER TABLE `tickets`
  ADD PRIMARY KEY (`ticket_id`),
  ADD UNIQUE KEY `ticket_number` (`ticket_number`),
  ADD KEY `ticket_passenger_id` (`ticket_passenger_id`),
  ADD KEY `flight_seat_id` (`flight_seat_id`);

--
-- Chỉ mục cho bảng `ticket_history`
--
ALTER TABLE `ticket_history`
  ADD PRIMARY KEY (`history_id`),
  ADD KEY `ticket_id` (`ticket_id`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `accounts`
--
ALTER TABLE `accounts`
  MODIFY `account_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT cho bảng `aircrafts`
--
ALTER TABLE `aircrafts`
  MODIFY `aircraft_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `airlines`
--
ALTER TABLE `airlines`
  MODIFY `airline_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `airports`
--
ALTER TABLE `airports`
  MODIFY `airport_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT cho bảng `baggage`
--
ALTER TABLE `baggage`
  MODIFY `baggage_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `baggage_history`
--
ALTER TABLE `baggage_history`
  MODIFY `history_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `bookings`
--
ALTER TABLE `bookings`
  MODIFY `booking_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `booking_passengers`
--
ALTER TABLE `booking_passengers`
  MODIFY `booking_passenger_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `cabin_classes`
--
ALTER TABLE `cabin_classes`
  MODIFY `class_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT cho bảng `fare_rules`
--
ALTER TABLE `fare_rules`
  MODIFY `rule_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT cho bảng `flights`
--
ALTER TABLE `flights`
  MODIFY `flight_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `flight_seats`
--
ALTER TABLE `flight_seats`
  MODIFY `flight_seat_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `passenger_profiles`
--
ALTER TABLE `passenger_profiles`
  MODIFY `profile_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT cho bảng `payments`
--
ALTER TABLE `payments`
  MODIFY `payment_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `permissions`
--
ALTER TABLE `permissions`
  MODIFY `permission_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT cho bảng `roles`
--
ALTER TABLE `roles`
  MODIFY `role_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT cho bảng `routes`
--
ALTER TABLE `routes`
  MODIFY `route_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT cho bảng `seats`
--
ALTER TABLE `seats`
  MODIFY `seat_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `tickets`
--
ALTER TABLE `tickets`
  MODIFY `ticket_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `ticket_history`
--
ALTER TABLE `ticket_history`
  MODIFY `history_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `account_role`
--
ALTER TABLE `account_role`
  ADD CONSTRAINT `account_role_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`account_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `account_role_ibfk_2` FOREIGN KEY (`role_id`) REFERENCES `roles` (`role_id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `aircrafts`
--
ALTER TABLE `aircrafts`
  ADD CONSTRAINT `aircrafts_ibfk_1` FOREIGN KEY (`airline_id`) REFERENCES `airlines` (`airline_id`);

--
-- Các ràng buộc cho bảng `baggage`
--
ALTER TABLE `baggage`
  ADD CONSTRAINT `baggage_ibfk_1` FOREIGN KEY (`ticket_id`) REFERENCES `tickets` (`ticket_id`),
  ADD CONSTRAINT `baggage_ibfk_2` FOREIGN KEY (`flight_id`) REFERENCES `flights` (`flight_id`);

--
-- Các ràng buộc cho bảng `baggage_history`
--
ALTER TABLE `baggage_history`
  ADD CONSTRAINT `baggage_history_ibfk_1` FOREIGN KEY (`baggage_id`) REFERENCES `baggage` (`baggage_id`);

--
-- Các ràng buộc cho bảng `bookings`
--
ALTER TABLE `bookings`
  ADD CONSTRAINT `bookings_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`account_id`);

--
-- Các ràng buộc cho bảng `booking_passengers`
--
ALTER TABLE `booking_passengers`
  ADD CONSTRAINT `booking_passengers_ibfk_1` FOREIGN KEY (`booking_id`) REFERENCES `bookings` (`booking_id`),
  ADD CONSTRAINT `booking_passengers_ibfk_2` FOREIGN KEY (`profile_id`) REFERENCES `passenger_profiles` (`profile_id`);

--
-- Các ràng buộc cho bảng `fare_rules`
--
ALTER TABLE `fare_rules`
  ADD CONSTRAINT `fare_rules_ibfk_1` FOREIGN KEY (`route_id`) REFERENCES `routes` (`route_id`),
  ADD CONSTRAINT `fare_rules_ibfk_2` FOREIGN KEY (`class_id`) REFERENCES `cabin_classes` (`class_id`);

--
-- Các ràng buộc cho bảng `flights`
--
ALTER TABLE `flights`
  ADD CONSTRAINT `flights_ibfk_1` FOREIGN KEY (`aircraft_id`) REFERENCES `aircrafts` (`aircraft_id`),
  ADD CONSTRAINT `flights_ibfk_2` FOREIGN KEY (`route_id`) REFERENCES `routes` (`route_id`);

--
-- Các ràng buộc cho bảng `flight_seats`
--
ALTER TABLE `flight_seats`
  ADD CONSTRAINT `flight_seats_ibfk_1` FOREIGN KEY (`flight_id`) REFERENCES `flights` (`flight_id`),
  ADD CONSTRAINT `flight_seats_ibfk_2` FOREIGN KEY (`seat_id`) REFERENCES `seats` (`seat_id`);

--
-- Các ràng buộc cho bảng `passenger_profiles`
--
ALTER TABLE `passenger_profiles`
  ADD CONSTRAINT `passenger_profiles_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`account_id`);

--
-- Các ràng buộc cho bảng `payments`
--
ALTER TABLE `payments`
  ADD CONSTRAINT `payments_ibfk_1` FOREIGN KEY (`booking_id`) REFERENCES `bookings` (`booking_id`);

--
-- Các ràng buộc cho bảng `role_permissions`
--
ALTER TABLE `role_permissions`
  ADD CONSTRAINT `role_permissions_ibfk_1` FOREIGN KEY (`role_id`) REFERENCES `roles` (`role_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `role_permissions_ibfk_2` FOREIGN KEY (`permission_id`) REFERENCES `permissions` (`permission_id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `routes`
--
ALTER TABLE `routes`
  ADD CONSTRAINT `routes_ibfk_1` FOREIGN KEY (`departure_place_id`) REFERENCES `airports` (`airport_id`),
  ADD CONSTRAINT `routes_ibfk_2` FOREIGN KEY (`arrival_place_id`) REFERENCES `airports` (`airport_id`);

--
-- Các ràng buộc cho bảng `seats`
--
ALTER TABLE `seats`
  ADD CONSTRAINT `seats_ibfk_1` FOREIGN KEY (`aircraft_id`) REFERENCES `aircrafts` (`aircraft_id`),
  ADD CONSTRAINT `seats_ibfk_2` FOREIGN KEY (`class_id`) REFERENCES `cabin_classes` (`class_id`);

--
-- Các ràng buộc cho bảng `tickets`
--
ALTER TABLE `tickets`
  ADD CONSTRAINT `tickets_ibfk_1` FOREIGN KEY (`ticket_passenger_id`) REFERENCES `booking_passengers` (`booking_passenger_id`),
  ADD CONSTRAINT `tickets_ibfk_2` FOREIGN KEY (`flight_seat_id`) REFERENCES `flight_seats` (`flight_seat_id`);

--
-- Các ràng buộc cho bảng `ticket_history`
--
ALTER TABLE `ticket_history`
  ADD CONSTRAINT `ticket_history_ibfk_1` FOREIGN KEY (`ticket_id`) REFERENCES `tickets` (`ticket_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
