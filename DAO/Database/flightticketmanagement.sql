-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th12 11, 2025 lúc 11:54 AM
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `accounts`
--

INSERT INTO `accounts` (`account_id`, `email`, `password`, `failed_attempts`, `is_active`, `created_at`) VALUES
(1, 'admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, '2025-12-05 17:28:05'),
(2, 'staff@test.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, '2025-12-05 17:28:05'),
(3, 'user@test.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, '2025-12-05 17:28:05'),
(4, 'hongqui@sv.sgu.edu.vn', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, '2025-12-05 17:28:05'),
(5, 'phamnam@hotmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, '2025-12-05 17:28:05'),
(6, 'vophat@outlook.com.vn', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, '2025-12-05 17:28:05'),
(7, 'phuocnam@yahoo.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, '2025-12-05 17:28:05'),
(8, 'quangphong@gmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 5, 1, '2025-12-05 17:28:05'),
(9, 'qui@gmail.com', 'bcb15f821479b4d5772bd0ca866c00ad5f926e3580720659cc80d39c9d09802a', 5, 1, '2025-12-05 18:26:58');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `account_role`
--

CREATE TABLE `account_role` (
  `account_id` int(11) NOT NULL,
  `role_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `account_role`
--

INSERT INTO `account_role` (`account_id`, `role_id`) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 1),
(5, 2),
(6, 3),
(7, 1),
(8, 2),
(9, 3);

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `aircrafts`
--

INSERT INTO `aircrafts` (`aircraft_id`, `airline_id`, `model`, `manufacturer`, `capacity`) VALUES
(1, 1, 'Airbus A350-900', 'Airbus', 18),
(2, 1, 'Boeing 787-10', 'Boeing', 18),
(3, 2, 'Airbus A321neo', 'Airbus', 18),
(4, 2, 'Airbus A320neo', 'Airbus', 18),
(5, 3, 'Embraer E195-E2', 'Embraer', 18),
(6, 3, 'Boeing 787-9 Dreamliner', 'Boeing', 18),
(7, 4, 'Boeing 737 MAX 8', 'Boeing', 18),
(8, 4, 'Airbus A321XLR', 'Airbus', 18),
(9, 5, 'Airbus A380-800', 'Airbus', 18),
(10, 5, 'Boeing 777-200ER', 'Boeing', 18);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `airlines`
--

CREATE TABLE `airlines` (
  `airline_id` int(11) NOT NULL,
  `airline_code` varchar(10) NOT NULL,
  `airline_name` varchar(100) NOT NULL,
  `country` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `airlines`
--

INSERT INTO `airlines` (`airline_id`, `airline_code`, `airline_name`, `country`) VALUES
(1, 'VN', 'Vietnam Airlines', 'Vietnam'),
(2, 'VJ', 'Vietjet Air', 'Vietnam'),
(3, 'QH', 'Bamboo Airways', 'Vietnam'),
(4, 'AA', 'American Airlines', 'American'),
(5, 'OZ', 'Asiana Airlines', 'Korean');

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `airports`
--

INSERT INTO `airports` (`airport_id`, `airport_code`, `airport_name`, `city`, `country`) VALUES
(1, 'HAN', 'Noi Bai International Airport', 'Hanoi', 'Vietnam'),
(2, 'SGN', 'Tan Son Nhat International Airport', 'Ho Chi Minh City', 'Vietnam'),
(3, 'DAD', 'Da Nang International Airport', 'Da Nang', 'Vietnam'),
(4, 'YYJ', 'Vitoria International Airport', 'Victoria', 'Canada'),
(5, 'GMP', 'Gimpo International Airport', 'Seoul', 'Korean');

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `baggage`
--

INSERT INTO `baggage` (`baggage_id`, `ticket_id`, `flight_id`, `baggage_tag`, `baggage_type`, `weight_kg`, `allowed_weight_kg`, `fee`, `status`, `special_handling`, `created_at`, `updated_at`) VALUES
(1, 1, 2, 'BG10001', 'CHECKED', 18.00, 20.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(2, 2, 2, 'BG10002', 'CHECKED', 22.50, 20.00, 37.50, 'LOADED', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(3, 3, 2, 'BG10003', 'CARRY_ON', 8.00, 10.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(4, 4, 2, 'BG10004', 'CHECKED', 19.50, 20.00, 0.00, 'LOADED', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(5, 5, 2, 'BG10005', 'CHECKED', 21.00, 20.00, 15.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(6, 7, 2, 'BG10006', 'CARRY_ON', 7.50, 10.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(7, 9, 4, 'BG10007', 'CHECKED', 28.00, 30.00, 0.00, 'LOADED', 'FRAGILE', '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(8, 10, 4, 'BG10008', 'SPECIAL', 32.00, 25.00, 105.00, 'CHECKED_IN', 'SPORT_EQUIPMENT', '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(9, 11, 4, 'BG10009', 'CHECKED', 19.00, 20.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(10, 12, 4, 'BG10010', 'CHECKED', 34.00, 35.00, 0.00, 'IN_TRANSIT', 'PRIORITY', '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(11, 13, 4, 'BG10011', 'CARRY_ON', 7.50, 10.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(12, 14, 4, 'BG10012', 'CHECKED', 24.50, 25.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(13, 16, 7, 'BG10013', 'CHECKED', 21.00, 20.00, 15.00, 'LOADED', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(14, 17, 7, 'BG10014', 'CHECKED', 19.00, 20.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(15, 18, 7, 'BG10015', 'SPECIAL', 29.50, 25.00, 67.50, 'IN_TRANSIT', 'MUSICAL_INSTRUMENT', '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(16, 19, 7, 'BG10016', 'CHECKED', 24.00, 25.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(17, 20, 7, 'BG10017', 'CARRY_ON', 9.00, 10.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(18, 21, 11, 'BG10018', 'CHECKED', 28.00, 30.00, 0.00, 'LOADED', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(19, 22, 11, 'BG10019', 'CHECKED', 20.50, 20.00, 7.50, 'IN_TRANSIT', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(20, 23, 11, 'BG10020', 'CARRY_ON', 9.00, 10.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(21, 24, 11, 'BG10021', 'CHECKED', 18.50, 20.00, 0.00, 'LOADED', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(22, 25, 11, 'BG10022', 'CHECKED', 22.00, 20.00, 30.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(23, 26, 18, 'BG10023', 'CHECKED', 27.00, 25.00, 30.00, 'LOADED', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(24, 27, 18, 'BG10024', 'CHECKED', 18.00, 20.00, 0.00, 'CLAIMED', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(25, 28, 18, 'BG10025', 'SPECIAL', 33.00, 25.00, 120.00, 'LOST', 'SPORT_EQUIPMENT', '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(26, 29, 18, 'BG10026', 'CHECKED', 19.50, 20.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05'),
(27, 30, 18, 'BG10027', 'CARRY_ON', 8.50, 10.00, 0.00, 'CHECKED_IN', NULL, '2025-12-05 17:28:05', '2025-12-05 17:28:05');

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `baggage_history`
--

INSERT INTO `baggage_history` (`history_id`, `baggage_id`, `old_status`, `new_status`, `changed_at`) VALUES
(1, 1, 'CREATED', 'CHECKED_IN', '2025-01-02 08:00:00'),
(2, 2, 'CREATED', 'CHECKED_IN', '2025-01-05 09:20:00'),
(3, 2, 'CHECKED_IN', 'LOADED', '2025-01-05 10:00:00'),
(4, 3, 'CREATED', 'CHECKED_IN', '2025-01-03 11:00:00'),
(5, 4, 'CREATED', 'CHECKED_IN', '2025-02-01 13:40:00'),
(6, 4, 'CHECKED_IN', 'LOADED', '2025-02-01 15:10:00'),
(7, 5, 'CREATED', 'CHECKED_IN', '2025-02-10 17:00:00'),
(8, 6, 'CREATED', 'CHECKED_IN', '2025-03-03 09:30:00'),
(9, 7, 'CREATED', 'CHECKED_IN', '2025-03-21 08:00:00'),
(10, 7, 'CHECKED_IN', 'LOADED', '2025-03-21 09:30:00'),
(11, 8, 'CREATED', 'CHECKED_IN', '2025-04-01 17:00:00'),
(12, 9, 'CREATED', 'CHECKED_IN', '2025-04-11 08:00:00'),
(13, 10, 'CREATED', 'CHECKED_IN', '2025-05-02 10:30:00'),
(14, 10, 'CHECKED_IN', 'LOADED', '2025-05-02 11:45:00'),
(15, 10, 'LOADED', 'IN_TRANSIT', '2025-05-02 13:00:00'),
(16, 11, 'CREATED', 'CHECKED_IN', '2025-05-06 14:00:00'),
(17, 12, 'CREATED', 'CHECKED_IN', '2025-05-14 19:00:00'),
(18, 13, 'CREATED', 'CHECKED_IN', '2025-06-01 09:00:00'),
(19, 13, 'CHECKED_IN', 'LOADED', '2025-06-01 10:45:00'),
(20, 14, 'CREATED', 'CHECKED_IN', '2025-06-12 07:30:00'),
(21, 15, 'CREATED', 'CHECKED_IN', '2025-06-20 12:00:00'),
(22, 15, 'CHECKED_IN', 'LOADED', '2025-06-20 13:30:00'),
(23, 15, 'LOADED', 'IN_TRANSIT', '2025-06-20 15:00:00'),
(24, 16, 'CREATED', 'CHECKED_IN', '2025-06-25 07:00:00'),
(25, 17, 'CREATED', 'CHECKED_IN', '2025-07-01 13:00:00'),
(26, 18, 'CREATED', 'CHECKED_IN', '2025-01-20 09:00:00'),
(27, 18, 'CHECKED_IN', 'LOADED', '2025-01-20 10:30:00'),
(28, 19, 'CREATED', 'CHECKED_IN', '2025-01-21 13:00:00'),
(29, 19, 'CHECKED_IN', 'LOADED', '2025-01-21 14:30:00'),
(30, 19, 'LOADED', 'IN_TRANSIT', '2025-01-21 16:00:00'),
(31, 20, 'CREATED', 'CHECKED_IN', '2025-01-22 08:30:00'),
(32, 21, 'CREATED', 'CHECKED_IN', '2025-01-23 10:00:00'),
(33, 21, 'CHECKED_IN', 'LOADED', '2025-01-23 11:30:00'),
(34, 22, 'CREATED', 'CHECKED_IN', '2025-01-24 14:30:00'),
(35, 23, 'CREATED', 'CHECKED_IN', '2025-07-05 07:00:00'),
(36, 23, 'CHECKED_IN', 'LOADED', '2025-07-05 08:30:00'),
(37, 24, 'CREATED', 'CHECKED_IN', '2025-07-06 09:00:00'),
(38, 24, 'CHECKED_IN', 'LOADED', '2025-07-06 10:30:00'),
(39, 24, 'LOADED', 'IN_TRANSIT', '2025-07-06 12:00:00'),
(40, 24, 'IN_TRANSIT', 'CLAIMED', '2025-07-06 14:30:00'),
(41, 25, 'CREATED', 'CHECKED_IN', '2025-07-07 12:00:00'),
(42, 25, 'CHECKED_IN', 'LOADED', '2025-07-07 13:30:00'),
(43, 25, 'LOADED', 'LOST', '2025-07-07 16:00:00'),
(44, 26, 'CREATED', 'CHECKED_IN', '2025-07-08 15:00:00'),
(45, 27, 'CREATED', 'CHECKED_IN', '2025-07-09 08:00:00');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `bookings`
--

CREATE TABLE `bookings` (
  `booking_id` int(11) NOT NULL,
  `account_id` int(11) DEFAULT NULL,
  `booking_date` datetime DEFAULT current_timestamp(),
  `trip_type` enum('ONE_WAY','MULTI_WAY','ROUND_TRIP') NOT NULL DEFAULT 'ONE_WAY',
  `status` enum('PENDING','CONFIRMED','CANCELLED','REFUNDED') DEFAULT 'PENDING',
  `total_amount` decimal(12,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `bookings`
--

INSERT INTO `bookings` (`booking_id`, `account_id`, `booking_date`, `trip_type`, `status`, `total_amount`) VALUES
(1, 4, '2024-11-05 10:22:11', 'ONE_WAY', 'CONFIRMED', 350.00),
(2, 5, '2025-01-14 08:15:33', 'ROUND_TRIP', 'PENDING', 720.00),
(3, 6, '2024-12-22 19:41:02', 'ONE_WAY', 'CANCELLED', 200.00),
(4, 7, '2025-02-03 14:05:47', 'MULTI_WAY', 'CONFIRMED', 1120.00),
(5, 8, '2024-10-29 09:55:20', 'ROUND_TRIP', 'REFUNDED', 650.00),
(6, 4, '2025-03-11 17:22:59', 'ONE_WAY', 'CONFIRMED', 450.00),
(7, 5, '2024-09-13 11:02:16', 'MULTI_WAY', 'PENDING', 900.00),
(8, 6, '2025-04-20 20:14:37', 'ONE_WAY', 'CONFIRMED', 200.00),
(9, 7, '2024-08-18 06:45:12', 'ROUND_TRIP', 'CANCELLED', 740.00),
(10, 8, '2025-02-27 15:39:29', 'ONE_WAY', 'CONFIRMED', 300.00),
(11, 4, '2024-07-09 13:26:51', 'MULTI_WAY', 'PENDING', 1050.00),
(12, 5, '2025-01-25 18:33:44', 'ROUND_TRIP', 'CONFIRMED', 880.00),
(13, 6, '2024-06-30 21:10:18', 'ONE_WAY', 'REFUNDED', 220.00),
(14, 7, '2025-03-02 12:49:06', 'MULTI_WAY', 'CONFIRMED', 990.00),
(15, 8, '2024-09-21 08:12:57', 'ONE_WAY', 'PENDING', 250.00),
(19, 2, '2025-12-08 16:26:34', '', 'CONFIRMED', 2000000.00),
(20, 2, '2025-12-08 16:26:35', '', 'CONFIRMED', 2000000.00),
(21, 2, '2025-12-08 16:26:50', '', 'CONFIRMED', 2000000.00),
(22, 2, '2025-12-08 16:26:51', '', 'CONFIRMED', 2000000.00),
(23, 2, '2025-12-08 16:26:51', '', 'CONFIRMED', 2000000.00),
(24, 2, '2025-12-08 16:26:52', '', 'CONFIRMED', 2000000.00),
(25, 2, '2025-12-08 16:26:52', '', 'CONFIRMED', 2000000.00),
(26, 2, '2025-12-08 16:26:52', '', 'CONFIRMED', 2000000.00),
(27, 2, '2025-12-08 16:26:52', '', 'CONFIRMED', 2000000.00),
(28, 2, '2025-12-08 16:26:52', '', 'CONFIRMED', 2000000.00),
(29, 2, '2025-12-08 16:26:52', '', 'CONFIRMED', 2000000.00),
(30, 2, '2025-12-08 16:26:53', '', 'CONFIRMED', 2000000.00),
(31, 2, '2025-12-08 16:26:53', '', 'CONFIRMED', 2000000.00),
(32, 2, '2025-12-08 16:26:53', '', 'CONFIRMED', 2000000.00),
(33, 2, '2025-12-08 16:26:53', '', 'CONFIRMED', 2000000.00),
(34, 2, '2025-12-08 16:26:53', '', 'CONFIRMED', 2000000.00),
(35, 2, '2025-12-08 16:26:54', '', 'CONFIRMED', 2000000.00),
(36, 2, '2025-12-08 16:26:54', '', 'CONFIRMED', 2000000.00),
(37, 2, '2025-12-08 16:29:28', '', 'CONFIRMED', 2000000.00),
(38, 2, '2025-12-08 16:52:40', '', 'CONFIRMED', 0.00),
(39, 2, '2025-12-08 16:54:34', '', 'CONFIRMED', 2000000.00),
(43, 2, '2025-12-11 16:16:17', 'ONE_WAY', 'CONFIRMED', 580900.00);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `booking_passengers`
--

CREATE TABLE `booking_passengers` (
  `booking_passenger_id` int(11) NOT NULL,
  `booking_id` int(11) DEFAULT NULL,
  `profile_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `booking_passengers`
--

INSERT INTO `booking_passengers` (`booking_passenger_id`, `booking_id`, `profile_id`) VALUES
(1, 1, 4),
(2, 2, 5),
(3, 3, 6),
(4, 4, 7),
(5, 4, 8),
(6, 5, 8),
(7, 6, 4),
(8, 7, 5),
(9, 7, 6),
(10, 8, 6),
(11, 9, 7),
(12, 10, 8),
(13, 11, 4),
(14, 11, 5),
(15, 12, 5),
(16, 13, 6),
(17, 14, 7),
(18, 14, 8),
(19, 15, 8),
(20, 15, 4),
(24, 19, 13),
(25, 19, 14),
(26, 20, 15),
(27, 20, 16),
(28, 21, 17),
(29, 21, 18),
(30, 22, 19),
(31, 22, 20),
(32, 23, 21),
(33, 23, 22),
(34, 24, 23),
(35, 24, 24),
(36, 25, 25),
(37, 25, 26),
(38, 26, 27),
(39, 26, 28),
(40, 27, 29),
(41, 27, 30),
(42, 28, 31),
(43, 28, 32),
(44, 29, 33),
(45, 29, 34),
(46, 30, 35),
(47, 30, 36),
(48, 31, 37),
(49, 31, 38),
(50, 32, 39),
(51, 32, 40),
(52, 33, 41),
(53, 33, 42),
(54, 34, 43),
(55, 34, 44),
(56, 35, 45),
(57, 35, 46),
(58, 36, 47),
(59, 36, 48),
(60, 37, 49),
(61, 37, 50),
(62, 39, 2),
(63, 39, 51),
(67, 43, 2);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `cabin_classes`
--

CREATE TABLE `cabin_classes` (
  `class_id` int(11) NOT NULL,
  `class_name` varchar(50) NOT NULL,
  `description` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `cabin_classes`
--

INSERT INTO `cabin_classes` (`class_id`, `class_name`, `description`) VALUES
(1, 'First', 'First class description'),
(2, 'Business', 'Business class description'),
(3, 'Premium Economy', 'Premium Economy class description'),
(4, 'Economy', 'Economy class description');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `carryon_baggage`
--

CREATE TABLE `carryon_baggage` (
  `carryon_id` int(11) NOT NULL,
  `weight_kg` int(11) NOT NULL,
  `class_id` int(11) NOT NULL,
  `size_limit` varchar(50) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `is_default` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `carryon_baggage`
--

INSERT INTO `carryon_baggage` (`carryon_id`, `weight_kg`, `class_id`, `size_limit`, `description`, `is_default`) VALUES
(1, 14, 1, '56x36x23 cm', 'Xách tay mặc định First Class 14kg', 1),
(2, 10, 2, '56x36x23 cm', 'Xách tay mặc định Business 10kg', 1),
(3, 10, 3, '56x36x23 cm', 'Xách tay mặc định Premium Economy 10kg', 1),
(4, 7, 4, '56x36x23 cm', 'Xách tay mặc định Economy 7kg', 1),
(5, 3, 4, '40x30x15 cm', 'Túi cá nhân nhỏ 3kg', 0),
(6, 5, 4, '45x35x20 cm', 'Balô nhỏ 5kg', 0);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `checked_baggage`
--

CREATE TABLE `checked_baggage` (
  `checked_id` int(11) NOT NULL,
  `weight_kg` int(11) NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `description` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `checked_baggage`
--

INSERT INTO `checked_baggage` (`checked_id`, `weight_kg`, `price`, `description`) VALUES
(1, 10, 200000.00, 'Gói ký gửi 10kg – tiêu chuẩn'),
(2, 15, 320000.00, 'Gói ký gửi 15kg'),
(3, 20, 450000.00, 'Gói ký gửi 20kg – phổ biến nhất'),
(4, 25, 580000.00, 'Gói ký gửi 25kg'),
(5, 30, 720000.00, 'Gói ký gửi 30kg – hành lý nhiều'),
(6, 35, 850000.00, 'Gói ký gửi 35kg'),
(7, 40, 990000.00, 'Gói ký gửi 40kg – tối đa theo quy định');

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `fare_rules`
--

INSERT INTO `fare_rules` (`rule_id`, `route_id`, `class_id`, `fare_type`, `season`, `effective_date`, `expiry_date`, `description`, `price`) VALUES
(1, 1, 1, 'Standard', 'PEAK', '2025-01-01', '2025-12-31', 'Peak First Class Fare', 900.00),
(2, 1, 4, 'Saver', 'OFFPEAK', '2025-02-01', '2025-06-30', 'Discount Economy Fare', 200.00),
(3, 2, 2, 'Flex', 'NORMAL', '2025-03-01', '2025-12-31', 'Flexible Business Fare', 550.00),
(4, 3, 3, 'Standard', 'PEAK', '2025-01-15', '2025-05-15', 'Premium Eco Peak Season', 350.00),
(5, 4, 4, 'Promo', 'NORMAL', '2025-04-01', '2025-09-30', 'Promo Economy Ticket', 200.00),
(6, 5, 1, 'Flex', 'PEAK', '2025-06-01', '2025-12-31', 'Flexible First Class Fare', 900.00),
(7, 6, 2, 'Saver', 'OFFPEAK', '2025-02-10', '2025-04-20', 'Saver Business Class', 550.00),
(8, 7, 3, 'Standard', 'NORMAL', '2025-01-01', '2025-10-31', 'Standard Premium Eco', 350.00),
(9, 8, 4, 'Flex', 'PEAK', '2025-03-15', '2025-12-31', 'Flexible Economy Summer Fare', 200.00),
(10, 9, 1, 'Promo', 'OFFPEAK', '2025-01-10', '2025-04-30', 'First Class Promo Winter', 900.00),
(11, 10, 2, 'Standard', 'NORMAL', '2025-05-01', '2025-12-01', 'Business Standard Ticket', 550.00),
(12, 2, 3, 'Flex', 'PEAK', '2025-01-20', '2025-11-30', 'Premium Eco Flexible Fare', 350.00),
(13, 3, 4, 'Saver', 'NORMAL', '2025-02-01', '2025-10-01', 'Saver Economy Regular', 200.00),
(14, 6, 1, 'Standard', 'PEAK', '2025-06-10', '2025-09-10', 'Peak First Class Route 6', 900.00),
(15, 7, 2, 'Promo', 'OFFPEAK', '2025-01-05', '2025-03-31', 'Business Promo Fare', 550.00);

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
  `base_price` decimal(12,2) NOT NULL DEFAULT 0.00,
  `note` text DEFAULT NULL,
  `status` enum('SCHEDULED','DELAYED','CANCELLED','COMPLETED') NOT NULL DEFAULT 'SCHEDULED',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `flights`
--

INSERT INTO `flights` (`flight_id`, `flight_number`, `aircraft_id`, `route_id`, `departure_time`, `arrival_time`, `base_price`, `note`, `status`, `is_deleted`) VALUES
(1, 'QH772', 8, 4, '2025-12-15 20:25:43', '2025-12-16 00:38:43', 1200000.00, 'Chuyến bay quốc tế', 'SCHEDULED', 0),
(2, 'QH135', 9, 3, '2025-11-28 05:46:41', '2025-11-28 08:19:41', 850000.00, NULL, 'COMPLETED', 0),
(3, 'VJ134', 10, 3, '2025-11-25 12:34:52', '2025-11-25 16:24:52', 920000.00, 'Chuyến bay bị hủy do thời tiết', 'CANCELLED', 0),
(4, 'BL984', 1, 6, '2025-12-18 09:20:44', '2025-12-18 12:24:44', 1500000.00, NULL, 'SCHEDULED', 0),
(5, 'VJ487', 9, 5, '2025-12-03 10:52:11', '2025-12-03 12:13:11', 780000.00, 'Chuyến bay nội địa', 'DELAYED', 0),
(6, 'QH851', 10, 1, '2025-12-02 18:06:52', '2025-12-02 20:47:52', 950000.00, NULL, 'DELAYED', 0),
(7, 'QH800', 1, 7, '2025-12-02 06:15:50', '2025-12-02 08:39:50', 1100000.00, NULL, 'COMPLETED', 0),
(8, 'VJ354', 10, 6, '2025-11-29 12:53:29', '2025-11-29 15:46:29', 1350000.00, 'Chuyến bay quốc tế', 'DELAYED', 0),
(9, 'BL607', 5, 10, '2025-12-18 07:46:09', '2025-12-18 12:41:09', 2100000.00, 'Chuyến bay dài', 'DELAYED', 0),
(10, 'BL529', 3, 5, '2025-11-28 14:05:00', '2025-11-28 18:21:00', 880000.00, NULL, 'CANCELLED', 0),
(11, 'QH924', 6, 10, '2025-12-16 17:53:42', '2025-12-16 20:22:42', 1950000.00, NULL, 'SCHEDULED', 0),
(12, 'VJ164', 5, 3, '2025-12-18 07:28:42', '2025-12-18 10:47:42', 820000.00, NULL, 'SCHEDULED', 0),
(13, 'VN182', 5, 8, '2025-12-23 09:06:37', '2025-12-23 10:27:37', 750000.00, 'Chuyến bay nội địa', 'DELAYED', 0),
(14, 'VJ501', 4, 3, '2025-12-12 19:01:29', '2025-12-12 21:19:29', 890000.00, NULL, 'SCHEDULED', 0),
(15, 'BL813', 4, 2, '2025-11-27 22:00:44', '2025-11-28 01:20:44', 1250000.00, NULL, 'DELAYED', 0),
(16, 'BL768', 4, 2, '2025-12-13 23:53:59', '2025-12-14 03:54:59', 1180000.00, NULL, 'DELAYED', 0),
(17, 'QH933', 4, 3, '2025-12-08 16:46:12', '2025-12-08 19:46:12', 870000.00, NULL, 'SCHEDULED', 0),
(18, 'VJ145', 7, 4, '2025-11-26 11:50:14', '2025-11-26 15:23:14', 1450000.00, 'Chuyến bay quốc tế', 'COMPLETED', 0),
(19, 'VN133', 10, 4, '2025-11-25 08:56:17', '2025-11-25 13:01:17', 1320000.00, NULL, 'CANCELLED', 0),
(20, 'VJ307', 8, 7, '2025-11-27 07:14:38', '2025-11-27 10:23:38', 1080000.00, NULL, 'DELAYED', 0);

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `flight_seats`
--

INSERT INTO `flight_seats` (`flight_seat_id`, `flight_id`, `seat_id`, `base_price`, `seat_status`) VALUES
(1, 1, 127, 900.00, 'BOOKED'),
(2, 1, 128, 900.00, 'BOOKED'),
(3, 1, 129, 900.00, 'BLOCKED'),
(4, 1, 130, 900.00, 'AVAILABLE'),
(5, 1, 131, 900.00, 'BOOKED'),
(6, 1, 132, 900.00, 'AVAILABLE'),
(7, 1, 133, 550.00, 'BOOKED'),
(8, 1, 134, 550.00, 'AVAILABLE'),
(9, 1, 135, 550.00, 'BLOCKED'),
(10, 1, 136, 550.00, 'AVAILABLE'),
(11, 1, 137, 550.00, 'BOOKED'),
(12, 1, 138, 550.00, 'AVAILABLE'),
(13, 1, 139, 350.00, 'BOOKED'),
(14, 1, 140, 350.00, 'AVAILABLE'),
(15, 1, 141, 350.00, 'BLOCKED'),
(16, 1, 142, 350.00, 'AVAILABLE'),
(17, 1, 143, 350.00, 'BOOKED'),
(18, 1, 144, 350.00, 'AVAILABLE'),
(19, 2, 145, 900.00, 'AVAILABLE'),
(20, 2, 146, 900.00, 'BOOKED'),
(21, 2, 147, 900.00, 'BLOCKED'),
(22, 2, 151, 550.00, 'BOOKED'),
(23, 2, 152, 550.00, 'AVAILABLE'),
(24, 2, 153, 550.00, 'BLOCKED'),
(25, 2, 157, 350.00, 'BOOKED'),
(26, 2, 158, 350.00, 'BLOCKED'),
(27, 2, 159, 350.00, 'AVAILABLE'),
(28, 3, 163, 900.00, 'AVAILABLE'),
(29, 3, 164, 900.00, 'BOOKED'),
(30, 3, 165, 900.00, 'BLOCKED'),
(31, 3, 169, 550.00, 'BLOCKED'),
(32, 3, 170, 550.00, 'AVAILABLE'),
(33, 3, 171, 550.00, 'BOOKED'),
(34, 3, 175, 350.00, 'BOOKED'),
(35, 3, 176, 350.00, 'BLOCKED'),
(36, 3, 177, 350.00, 'AVAILABLE'),
(37, 4, 1, 900.00, 'AVAILABLE'),
(38, 4, 2, 900.00, 'BOOKED'),
(39, 4, 3, 900.00, 'AVAILABLE'),
(40, 4, 7, 550.00, 'AVAILABLE'),
(41, 4, 8, 550.00, 'BOOKED'),
(42, 4, 9, 550.00, 'AVAILABLE'),
(43, 4, 13, 350.00, 'AVAILABLE'),
(44, 4, 14, 350.00, 'BLOCKED'),
(45, 4, 15, 350.00, 'AVAILABLE'),
(46, 5, 145, 900.00, 'AVAILABLE'),
(47, 5, 146, 900.00, 'BOOKED'),
(48, 5, 147, 900.00, 'BLOCKED'),
(49, 5, 151, 550.00, 'BLOCKED'),
(50, 5, 152, 550.00, 'AVAILABLE'),
(51, 5, 153, 550.00, 'BOOKED'),
(52, 5, 157, 350.00, 'BOOKED'),
(53, 5, 158, 350.00, 'BLOCKED'),
(54, 5, 159, 350.00, 'AVAILABLE'),
(55, 6, 163, 900.00, 'AVAILABLE'),
(56, 6, 164, 900.00, 'BOOKED'),
(57, 6, 165, 900.00, 'BLOCKED'),
(58, 6, 169, 550.00, 'BLOCKED'),
(59, 6, 170, 550.00, 'AVAILABLE'),
(60, 6, 171, 550.00, 'BOOKED'),
(61, 6, 175, 350.00, 'BOOKED'),
(62, 6, 176, 350.00, 'BLOCKED'),
(63, 6, 177, 350.00, 'AVAILABLE'),
(64, 7, 127, 900.00, 'AVAILABLE'),
(65, 7, 128, 900.00, 'BOOKED'),
(66, 7, 129, 900.00, 'BLOCKED'),
(67, 7, 130, 900.00, 'AVAILABLE'),
(68, 7, 131, 900.00, 'BOOKED'),
(69, 7, 132, 900.00, 'AVAILABLE'),
(70, 7, 133, 550.00, 'BOOKED'),
(71, 7, 134, 550.00, 'AVAILABLE'),
(72, 7, 135, 550.00, 'BLOCKED'),
(73, 7, 136, 550.00, 'AVAILABLE'),
(74, 7, 137, 550.00, 'BOOKED'),
(75, 7, 138, 550.00, 'AVAILABLE'),
(76, 7, 139, 350.00, 'BOOKED'),
(77, 7, 140, 350.00, 'AVAILABLE'),
(78, 7, 141, 350.00, 'BLOCKED'),
(79, 7, 142, 350.00, 'AVAILABLE'),
(80, 7, 143, 350.00, 'BOOKED'),
(81, 7, 144, 350.00, 'AVAILABLE'),
(82, 8, 163, 900.00, 'AVAILABLE'),
(83, 8, 164, 900.00, 'BOOKED'),
(84, 8, 165, 900.00, 'BLOCKED'),
(85, 8, 169, 550.00, 'BLOCKED'),
(86, 8, 170, 550.00, 'AVAILABLE'),
(87, 8, 171, 550.00, 'BOOKED'),
(88, 8, 175, 350.00, 'BOOKED'),
(89, 8, 176, 350.00, 'BLOCKED'),
(90, 8, 177, 350.00, 'AVAILABLE'),
(91, 9, 73, 900.00, 'AVAILABLE'),
(92, 9, 74, 900.00, 'BOOKED'),
(93, 9, 75, 900.00, 'BLOCKED'),
(94, 9, 76, 900.00, 'AVAILABLE'),
(95, 9, 77, 900.00, 'BOOKED'),
(96, 9, 78, 900.00, 'AVAILABLE'),
(97, 9, 79, 550.00, 'BOOKED'),
(98, 9, 80, 550.00, 'AVAILABLE'),
(99, 9, 81, 550.00, 'BLOCKED'),
(100, 9, 82, 550.00, 'AVAILABLE'),
(101, 9, 83, 550.00, 'BOOKED'),
(102, 9, 84, 550.00, 'AVAILABLE'),
(103, 9, 85, 350.00, 'BOOKED'),
(104, 9, 86, 350.00, 'AVAILABLE'),
(105, 9, 87, 350.00, 'BLOCKED'),
(106, 9, 88, 350.00, 'AVAILABLE'),
(107, 9, 89, 350.00, 'BOOKED'),
(108, 9, 90, 350.00, 'AVAILABLE'),
(109, 10, 37, 900.00, 'AVAILABLE'),
(110, 10, 38, 900.00, 'BOOKED'),
(111, 10, 39, 900.00, 'BLOCKED'),
(112, 10, 40, 900.00, 'AVAILABLE'),
(113, 10, 41, 900.00, 'BOOKED'),
(114, 10, 42, 900.00, 'AVAILABLE'),
(115, 10, 43, 550.00, 'BOOKED'),
(116, 10, 44, 550.00, 'AVAILABLE'),
(117, 10, 45, 550.00, 'BLOCKED'),
(118, 10, 46, 550.00, 'AVAILABLE'),
(119, 10, 47, 550.00, 'BOOKED'),
(120, 10, 48, 550.00, 'AVAILABLE'),
(121, 10, 49, 350.00, 'BOOKED'),
(122, 10, 50, 350.00, 'AVAILABLE'),
(123, 10, 51, 350.00, 'BLOCKED'),
(124, 10, 52, 350.00, 'AVAILABLE'),
(125, 10, 53, 350.00, 'BOOKED'),
(126, 10, 54, 350.00, 'AVAILABLE'),
(127, 11, 91, 900.00, 'AVAILABLE'),
(128, 11, 92, 900.00, 'BOOKED'),
(129, 11, 93, 900.00, 'BLOCKED'),
(130, 11, 94, 900.00, 'AVAILABLE'),
(131, 11, 95, 900.00, 'BOOKED'),
(132, 11, 96, 900.00, 'AVAILABLE'),
(133, 11, 97, 550.00, 'BOOKED'),
(134, 11, 98, 550.00, 'AVAILABLE'),
(135, 11, 99, 550.00, 'BLOCKED'),
(136, 11, 100, 550.00, 'AVAILABLE'),
(137, 11, 101, 550.00, 'BOOKED'),
(138, 11, 102, 550.00, 'AVAILABLE'),
(139, 11, 103, 350.00, 'BOOKED'),
(140, 11, 104, 350.00, 'AVAILABLE'),
(141, 11, 105, 350.00, 'BLOCKED'),
(142, 11, 106, 350.00, 'AVAILABLE'),
(143, 11, 107, 350.00, 'BOOKED'),
(144, 11, 108, 350.00, 'AVAILABLE'),
(145, 12, 73, 900.00, 'BOOKED'),
(146, 12, 74, 900.00, 'BOOKED'),
(147, 12, 75, 900.00, 'BLOCKED'),
(148, 12, 76, 900.00, 'AVAILABLE'),
(149, 12, 77, 900.00, 'BOOKED'),
(150, 12, 78, 900.00, 'AVAILABLE'),
(151, 12, 79, 550.00, 'BOOKED'),
(152, 12, 80, 550.00, 'AVAILABLE'),
(153, 12, 81, 550.00, 'BLOCKED'),
(154, 12, 82, 550.00, 'AVAILABLE'),
(155, 12, 83, 550.00, 'BOOKED'),
(156, 12, 84, 550.00, 'AVAILABLE'),
(157, 12, 85, 350.00, 'BOOKED'),
(158, 12, 86, 350.00, 'AVAILABLE'),
(159, 12, 87, 350.00, 'BLOCKED'),
(160, 12, 88, 350.00, 'AVAILABLE'),
(161, 12, 89, 350.00, 'BOOKED'),
(162, 12, 90, 350.00, 'AVAILABLE'),
(163, 13, 73, 900.00, 'AVAILABLE'),
(164, 13, 74, 900.00, 'BOOKED'),
(165, 13, 75, 900.00, 'BLOCKED'),
(166, 13, 76, 900.00, 'AVAILABLE'),
(167, 13, 77, 900.00, 'BOOKED'),
(168, 13, 78, 900.00, 'AVAILABLE'),
(169, 13, 79, 550.00, 'BOOKED'),
(170, 13, 80, 550.00, 'AVAILABLE'),
(171, 13, 81, 550.00, 'BLOCKED'),
(172, 13, 82, 550.00, 'AVAILABLE'),
(173, 13, 83, 550.00, 'BOOKED'),
(174, 13, 84, 550.00, 'AVAILABLE'),
(175, 13, 85, 350.00, 'BOOKED'),
(176, 13, 86, 350.00, 'AVAILABLE'),
(177, 13, 87, 350.00, 'BLOCKED'),
(178, 13, 88, 350.00, 'AVAILABLE'),
(179, 13, 89, 350.00, 'BOOKED'),
(180, 13, 90, 350.00, 'AVAILABLE'),
(181, 14, 55, 900.00, 'AVAILABLE'),
(182, 14, 56, 900.00, 'BOOKED'),
(183, 14, 57, 900.00, 'BLOCKED'),
(184, 14, 58, 900.00, 'AVAILABLE'),
(185, 14, 59, 900.00, 'BOOKED'),
(186, 14, 60, 900.00, 'AVAILABLE'),
(187, 14, 61, 550.00, 'BOOKED'),
(188, 14, 62, 550.00, 'AVAILABLE'),
(189, 14, 63, 550.00, 'BLOCKED'),
(190, 14, 64, 550.00, 'AVAILABLE'),
(191, 14, 65, 550.00, 'BOOKED'),
(192, 14, 66, 550.00, 'AVAILABLE'),
(193, 14, 67, 350.00, 'BOOKED'),
(194, 14, 68, 350.00, 'AVAILABLE'),
(195, 14, 69, 350.00, 'BLOCKED'),
(196, 14, 70, 350.00, 'AVAILABLE'),
(197, 14, 71, 350.00, 'BOOKED'),
(198, 14, 72, 350.00, 'AVAILABLE'),
(199, 15, 55, 900.00, 'AVAILABLE'),
(200, 15, 56, 900.00, 'BOOKED'),
(201, 15, 57, 900.00, 'BLOCKED'),
(202, 15, 58, 900.00, 'AVAILABLE'),
(203, 15, 59, 900.00, 'BOOKED'),
(204, 15, 60, 900.00, 'AVAILABLE'),
(205, 15, 61, 550.00, 'BOOKED'),
(206, 15, 62, 550.00, 'AVAILABLE'),
(207, 15, 63, 550.00, 'BLOCKED'),
(208, 15, 64, 550.00, 'AVAILABLE'),
(209, 15, 65, 550.00, 'BOOKED'),
(210, 15, 66, 550.00, 'AVAILABLE'),
(211, 15, 67, 350.00, 'BOOKED'),
(212, 15, 68, 350.00, 'AVAILABLE'),
(213, 15, 69, 350.00, 'BLOCKED'),
(214, 15, 70, 350.00, 'AVAILABLE'),
(215, 15, 71, 350.00, 'BOOKED'),
(216, 15, 72, 350.00, 'AVAILABLE'),
(217, 16, 55, 900.00, 'AVAILABLE'),
(218, 16, 56, 900.00, 'BOOKED'),
(219, 16, 57, 900.00, 'BLOCKED'),
(220, 16, 58, 900.00, 'AVAILABLE'),
(221, 16, 59, 900.00, 'BOOKED'),
(222, 16, 60, 900.00, 'AVAILABLE'),
(223, 16, 61, 550.00, 'BOOKED'),
(224, 16, 62, 550.00, 'AVAILABLE'),
(225, 16, 63, 550.00, 'BLOCKED'),
(226, 16, 64, 550.00, 'AVAILABLE'),
(227, 16, 65, 550.00, 'BOOKED'),
(228, 16, 66, 550.00, 'AVAILABLE'),
(229, 16, 67, 350.00, 'BOOKED'),
(230, 16, 68, 350.00, 'AVAILABLE'),
(231, 16, 69, 350.00, 'BLOCKED'),
(232, 16, 70, 350.00, 'AVAILABLE'),
(233, 16, 71, 350.00, 'BOOKED'),
(234, 16, 72, 350.00, 'AVAILABLE'),
(235, 17, 55, 900.00, 'AVAILABLE'),
(236, 17, 56, 900.00, 'BOOKED'),
(237, 17, 57, 900.00, 'BLOCKED'),
(238, 17, 58, 900.00, 'AVAILABLE'),
(239, 17, 59, 900.00, 'BOOKED'),
(240, 17, 60, 900.00, 'AVAILABLE'),
(241, 17, 61, 550.00, 'BOOKED'),
(242, 17, 62, 550.00, 'AVAILABLE'),
(243, 17, 63, 550.00, 'BLOCKED'),
(244, 17, 64, 550.00, 'AVAILABLE'),
(245, 17, 65, 550.00, 'BOOKED'),
(246, 17, 66, 550.00, 'AVAILABLE'),
(247, 17, 67, 350.00, 'BOOKED'),
(248, 17, 68, 350.00, 'AVAILABLE'),
(249, 17, 69, 350.00, 'BLOCKED'),
(250, 17, 70, 350.00, 'AVAILABLE'),
(251, 17, 71, 350.00, 'BOOKED'),
(252, 17, 72, 350.00, 'AVAILABLE'),
(253, 18, 109, 900.00, 'AVAILABLE'),
(254, 18, 110, 900.00, 'BOOKED'),
(255, 18, 111, 900.00, 'BLOCKED'),
(256, 18, 115, 550.00, 'BLOCKED'),
(257, 18, 116, 550.00, 'AVAILABLE'),
(258, 18, 117, 550.00, 'BOOKED'),
(259, 18, 121, 350.00, 'BOOKED'),
(260, 18, 122, 350.00, 'AVAILABLE'),
(261, 18, 123, 350.00, 'BLOCKED'),
(262, 19, 163, 900.00, 'AVAILABLE'),
(263, 19, 164, 900.00, 'BOOKED'),
(264, 19, 165, 900.00, 'BLOCKED'),
(265, 19, 169, 550.00, 'BLOCKED'),
(266, 19, 170, 550.00, 'AVAILABLE'),
(267, 19, 171, 550.00, 'BOOKED'),
(268, 19, 175, 350.00, 'BOOKED'),
(269, 19, 176, 350.00, 'BLOCKED'),
(270, 19, 177, 350.00, 'AVAILABLE'),
(271, 20, 127, 900.00, 'AVAILABLE'),
(272, 20, 128, 900.00, 'BOOKED'),
(273, 20, 129, 900.00, 'BLOCKED'),
(274, 20, 130, 900.00, 'AVAILABLE'),
(275, 20, 131, 900.00, 'BOOKED'),
(276, 20, 132, 900.00, 'AVAILABLE'),
(277, 20, 133, 550.00, 'BOOKED'),
(278, 20, 134, 550.00, 'AVAILABLE'),
(279, 20, 135, 550.00, 'BLOCKED'),
(280, 20, 136, 550.00, 'AVAILABLE'),
(281, 20, 137, 550.00, 'BOOKED'),
(282, 20, 138, 550.00, 'AVAILABLE'),
(283, 20, 139, 350.00, 'BOOKED'),
(284, 20, 140, 350.00, 'AVAILABLE'),
(285, 20, 141, 350.00, 'BLOCKED'),
(286, 20, 142, 350.00, 'AVAILABLE'),
(287, 20, 143, 350.00, 'BOOKED'),
(288, 20, 144, 350.00, 'AVAILABLE');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `national`
--

CREATE TABLE `national` (
  `national_id` int(11) NOT NULL,
  `country_name` varchar(100) NOT NULL,
  `country_code` char(2) NOT NULL,
  `phone_code` varchar(5) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `national`
--

INSERT INTO `national` (`national_id`, `country_name`, `country_code`, `phone_code`) VALUES
(1, 'United States', 'US', '+1'),
(2, 'Japan', 'JP', '+81'),
(3, 'South Korea', 'KR', '+82'),
(4, 'France', 'FR', '+33'),
(5, 'Vietnam', 'VN', '+84');

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `passenger_profiles`
--

INSERT INTO `passenger_profiles` (`profile_id`, `account_id`, `full_name`, `date_of_birth`, `phone_number`, `passport_number`, `nationality`) VALUES
(1, 1, 'Administrator', '1990-01-01', '0901234567', 'C1234567', 'Vietnam'),
(2, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'Vietnam'),
(3, 3, 'User Test', '1998-09-20', '0938765432', 'C3456789', 'Vietnam'),
(4, 4, 'Hồng Quí', '2003-05-17', '0981122334', 'C4567891', 'Vietnam'),
(5, 5, 'Phạm Nam', '1999-11-02', '0974455667', 'C5678912', 'Vietnam'),
(6, 6, 'Võ Phát', '2002-03-28', '0967788990', 'C6789123', 'Vietnam'),
(7, 7, 'Phước Nam', '1997-07-08', '0945566778', 'C7891234', 'Vietnam'),
(8, 8, 'Quang Phong', '2001-12-25', '0933344556', 'C8912345', 'Vietnam'),
(9, 9, NULL, NULL, NULL, NULL, NULL),
(13, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(14, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(15, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(16, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(17, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(18, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(19, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(20, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(21, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(22, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(23, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(24, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(25, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(26, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(27, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(28, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(29, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(30, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(31, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(32, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(33, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(34, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(35, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(36, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(37, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(38, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(39, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(40, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(41, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(42, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(43, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(44, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(45, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(46, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(47, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'United States'),
(48, NULL, '1', '2025-12-08', '1', '1', 'United States'),
(49, 2, 'Staff Test', '1995-04-12', '0912345678', 'C2345678', 'Japan'),
(50, NULL, 'qui_tesst', '2025-12-08', '1231', '1212', 'Japan'),
(51, NULL, '12', '2025-12-08', '12', '12', 'United States');

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `payments`
--

INSERT INTO `payments` (`payment_id`, `booking_id`, `amount`, `payment_method`, `payment_date`, `status`) VALUES
(1, 1, 350.00, 'CREDIT_CARD', '2024-11-05 11:00:00', 'SUCCESS'),
(2, 2, 720.00, 'E_WALLET', '2025-01-14 08:30:00', 'SUCCESS'),
(3, 3, 200.00, 'CASH', '2024-12-22 20:00:00', 'FAILED'),
(4, 4, 1120.00, 'BANK_TRANSFER', '2025-02-03 15:10:00', 'SUCCESS'),
(5, 5, 650.00, 'CREDIT_CARD', '2024-10-29 10:10:00', 'SUCCESS'),
(6, 6, 450.00, 'CASH', '2025-03-11 18:00:00', 'SUCCESS'),
(7, 7, 900.00, 'BANK_TRANSFER', '2024-09-13 12:00:00', 'PENDING'),
(8, 8, 200.00, 'E_WALLET', '2025-04-20 20:30:00', 'SUCCESS'),
(9, 9, 740.00, 'CREDIT_CARD', '2024-08-18 07:10:00', 'SUCCESS'),
(10, 10, 300.00, 'CASH', '2025-02-27 16:00:00', 'SUCCESS'),
(11, 11, 1050.00, 'E_WALLET', '2024-07-09 14:00:00', 'SUCCESS'),
(12, 12, 880.00, 'CREDIT_CARD', '2025-01-25 18:45:00', 'SUCCESS'),
(13, 13, 220.00, 'BANK_TRANSFER', '2024-06-30 21:30:00', 'FAILED'),
(14, 14, 990.00, 'E_WALLET', '2025-03-02 13:00:00', 'SUCCESS'),
(15, 15, 250.00, 'CREDIT_CARD', '2024-09-21 09:00:00', 'FAILED');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `permissions`
--

CREATE TABLE `permissions` (
  `permission_id` int(11) NOT NULL,
  `permission_code` varchar(100) NOT NULL,
  `permission_name` varchar(150) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `roles`
--

INSERT INTO `roles` (`role_id`, `role_code`, `role_name`) VALUES
(1, 'ADMIN', 'Quản trị viên'),
(2, 'STAFF', 'Nhân viên'),
(3, 'USER', 'Người dùng');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `role_permissions`
--

CREATE TABLE `role_permissions` (
  `role_id` int(11) NOT NULL,
  `permission_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `role_permissions`
--

INSERT INTO `role_permissions` (`role_id`, `permission_id`) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 4),
(1, 5),
(1, 6),
(1, 7),
(1, 8),
(1, 9),
(1, 10),
(1, 11),
(1, 12),
(1, 13),
(1, 14),
(1, 15),
(1, 16),
(1, 17),
(1, 18),
(1, 19),
(1, 20),
(1, 21),
(1, 22),
(1, 23),
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
(3, 5),
(3, 6),
(3, 19),
(3, 21);

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `routes`
--

INSERT INTO `routes` (`route_id`, `departure_place_id`, `arrival_place_id`, `distance_km`, `duration_minutes`) VALUES
(1, 1, 2, 1802, 230),
(2, 2, 1, 1689, 96),
(3, 4, 5, 1858, 90),
(4, 5, 4, 644, 228),
(5, 2, 3, 812, 102),
(6, 3, 4, 1882, 93),
(7, 1, 5, 532, 265),
(8, 1, 4, 633, 91),
(9, 2, 5, 1574, 113),
(10, 5, 4, 1794, 268);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `seats`
--

CREATE TABLE `seats` (
  `seat_id` int(11) NOT NULL,
  `aircraft_id` int(11) DEFAULT NULL,
  `seat_number` varchar(10) DEFAULT NULL,
  `class_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `seats`
--

INSERT INTO `seats` (`seat_id`, `aircraft_id`, `seat_number`, `class_id`) VALUES
(1, 1, '1A', 1),
(2, 1, '1B', 1),
(3, 1, '1C', 1),
(4, 1, '1D', 1),
(5, 1, '1E', 1),
(6, 1, '1F', 1),
(7, 1, '2A', 2),
(8, 1, '2B', 2),
(9, 1, '2C', 2),
(10, 1, '2D', 2),
(11, 1, '2E', 2),
(12, 1, '2F', 2),
(13, 1, '3A', 3),
(14, 1, '3B', 3),
(15, 1, '3C', 3),
(16, 1, '3D', 3),
(17, 1, '3E', 3),
(18, 1, '3F', 3),
(19, 2, '1A', 1),
(20, 2, '1B', 1),
(21, 2, '1C', 1),
(22, 2, '1D', 1),
(23, 2, '1E', 1),
(24, 2, '1F', 1),
(25, 2, '2A', 2),
(26, 2, '2B', 2),
(27, 2, '2C', 2),
(28, 2, '2D', 2),
(29, 2, '2E', 2),
(30, 2, '2F', 2),
(31, 2, '3A', 3),
(32, 2, '3B', 3),
(33, 2, '3C', 3),
(34, 2, '3D', 3),
(35, 2, '3E', 3),
(36, 2, '3F', 3),
(37, 3, '1A', 1),
(38, 3, '1B', 1),
(39, 3, '1C', 1),
(40, 3, '1D', 1),
(41, 3, '1E', 1),
(42, 3, '1F', 1),
(43, 3, '2A', 2),
(44, 3, '2B', 2),
(45, 3, '2C', 2),
(46, 3, '2D', 2),
(47, 3, '2E', 2),
(48, 3, '2F', 2),
(49, 3, '3A', 3),
(50, 3, '3B', 3),
(51, 3, '3C', 3),
(52, 3, '3D', 3),
(53, 3, '3E', 3),
(54, 3, '3F', 3),
(55, 4, '1A', 1),
(56, 4, '1B', 1),
(57, 4, '1C', 1),
(58, 4, '1D', 1),
(59, 4, '1E', 1),
(60, 4, '1F', 1),
(61, 4, '2A', 2),
(62, 4, '2B', 2),
(63, 4, '2C', 2),
(64, 4, '2D', 2),
(65, 4, '2E', 2),
(66, 4, '2F', 2),
(67, 4, '3A', 3),
(68, 4, '3B', 3),
(69, 4, '3C', 3),
(70, 4, '3D', 3),
(71, 4, '3E', 3),
(72, 4, '3F', 3),
(73, 5, '1A', 1),
(74, 5, '1B', 1),
(75, 5, '1C', 1),
(76, 5, '1D', 1),
(77, 5, '1E', 1),
(78, 5, '1F', 1),
(79, 5, '2A', 2),
(80, 5, '2B', 2),
(81, 5, '2C', 2),
(82, 5, '2D', 2),
(83, 5, '2E', 2),
(84, 5, '2F', 2),
(85, 5, '3A', 3),
(86, 5, '3B', 3),
(87, 5, '3C', 3),
(88, 5, '3D', 3),
(89, 5, '3E', 3),
(90, 5, '3F', 3),
(91, 6, '1A', 1),
(92, 6, '1B', 1),
(93, 6, '1C', 1),
(94, 6, '1D', 1),
(95, 6, '1E', 1),
(96, 6, '1F', 1),
(97, 6, '2A', 2),
(98, 6, '2B', 2),
(99, 6, '2C', 2),
(100, 6, '2D', 2),
(101, 6, '2E', 2),
(102, 6, '2F', 2),
(103, 6, '3A', 3),
(104, 6, '3B', 3),
(105, 6, '3C', 3),
(106, 6, '3D', 3),
(107, 6, '3E', 3),
(108, 6, '3F', 3),
(109, 7, '1A', 1),
(110, 7, '1B', 1),
(111, 7, '1C', 1),
(112, 7, '1D', 1),
(113, 7, '1E', 1),
(114, 7, '1F', 1),
(115, 7, '2A', 2),
(116, 7, '2B', 2),
(117, 7, '2C', 2),
(118, 7, '2D', 2),
(119, 7, '2E', 2),
(120, 7, '2F', 2),
(121, 7, '3A', 3),
(122, 7, '3B', 3),
(123, 7, '3C', 3),
(124, 7, '3D', 3),
(125, 7, '3E', 3),
(126, 7, '3F', 3),
(127, 8, '1A', 1),
(128, 8, '1B', 1),
(129, 8, '1C', 1),
(130, 8, '1D', 1),
(131, 8, '1E', 1),
(132, 8, '1F', 1),
(133, 8, '2A', 2),
(134, 8, '2B', 2),
(135, 8, '2C', 2),
(136, 8, '2D', 2),
(137, 8, '2E', 2),
(138, 8, '2F', 2),
(139, 8, '3A', 3),
(140, 8, '3B', 3),
(141, 8, '3C', 3),
(142, 8, '3D', 3),
(143, 8, '3E', 3),
(144, 8, '3F', 3),
(145, 9, '1A', 1),
(146, 9, '1B', 1),
(147, 9, '1C', 1),
(148, 9, '1D', 1),
(149, 9, '1E', 1),
(150, 9, '1F', 1),
(151, 9, '2A', 2),
(152, 9, '2B', 2),
(153, 9, '2C', 2),
(154, 9, '2D', 2),
(155, 9, '2E', 2),
(156, 9, '2F', 2),
(157, 9, '3A', 3),
(158, 9, '3B', 3),
(159, 9, '3C', 3),
(160, 9, '3D', 3),
(161, 9, '3E', 3),
(162, 9, '3F', 3),
(163, 10, '1A', 1),
(164, 10, '1B', 1),
(165, 10, '1C', 1),
(166, 10, '1D', 1),
(167, 10, '1E', 1),
(168, 10, '1F', 1),
(169, 10, '2A', 2),
(170, 10, '2B', 2),
(171, 10, '2C', 2),
(172, 10, '2D', 2),
(173, 10, '2E', 2),
(174, 10, '2F', 2),
(175, 10, '3A', 3),
(176, 10, '3B', 3),
(177, 10, '3C', 3),
(178, 10, '3D', 3),
(179, 10, '3E', 3),
(180, 10, '3F', 3);

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
  `status` enum('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED') DEFAULT 'BOOKED',
  `total_price` decimal(10,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `tickets`
--

INSERT INTO `tickets` (`ticket_id`, `ticket_passenger_id`, `flight_seat_id`, `ticket_number`, `issue_date`, `segment_no`, `segment_type`, `status`, `total_price`) VALUES
(1, 1, 1, 'TK202500001', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CONFIRMED', 0.00),
(2, 2, 2, 'TK202500002', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(3, 3, 3, 'TK202500003', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(4, 4, 4, 'TK202500004', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CONFIRMED', 0.00),
(5, 5, 5, 'TK202500005', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(6, 6, 6, 'TK202500006', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(7, 7, 7, 'TK202500007', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CONFIRMED', 0.00),
(8, 8, 8, 'TK202500008', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(9, 9, 9, 'TK202500009', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(10, 10, 10, 'TK202500010', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CONFIRMED', 0.00),
(11, 11, 11, 'TK202500011', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(12, 12, 12, 'TK202500012', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(13, 13, 13, 'TK202500013', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CHECKED_IN', 0.00),
(14, 14, 14, 'TK202500014', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CONFIRMED', 0.00),
(15, 15, 15, 'TK202500015', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(16, 16, 16, 'TK202500016', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOARDED', 0.00),
(17, 17, 17, 'TK202500017', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CONFIRMED', 0.00),
(18, 18, 18, 'TK202500018', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(19, 19, 19, 'TK202500019', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CHECKED_IN', 0.00),
(20, 20, 20, 'TK202500020', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(21, 1, 21, 'TK202500021', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CONFIRMED', 0.00),
(22, 2, 22, 'TK202500022', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(23, 3, 23, 'TK202500023', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CHECKED_IN', 0.00),
(24, 4, 24, 'TK202500024', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CONFIRMED', 0.00),
(25, 5, 25, 'TK202500025', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(26, 6, 26, 'TK202500026', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CONFIRMED', 0.00),
(27, 7, 27, 'TK202500027', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(28, 8, 28, 'TK202500028', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CHECKED_IN', 0.00),
(29, 9, 29, 'TK202500029', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'CONFIRMED', 0.00),
(30, 10, 30, 'TK202500030', '2025-12-05 17:28:05', 1, 'OUTBOUND', 'BOOKED', 0.00),
(33, 24, 1, 'TK639008079942206305', '2025-12-08 16:26:34', 1, 'OUTBOUND', 'BOOKED', 0.00),
(34, 25, 1, 'TK639008079942256990', '2025-12-08 16:26:34', 1, 'OUTBOUND', 'BOOKED', 0.00),
(35, 26, 1, 'TK639008079955711557', '2025-12-08 16:26:35', 1, 'OUTBOUND', 'BOOKED', 0.00),
(36, 27, 1, 'TK639008079955718615', '2025-12-08 16:26:35', 1, 'OUTBOUND', 'BOOKED', 0.00),
(37, 28, 1, 'TK639008080106000570', '2025-12-08 16:26:50', 1, 'OUTBOUND', 'BOOKED', 0.00),
(38, 29, 1, 'TK639008080106041666', '2025-12-08 16:26:50', 1, 'OUTBOUND', 'BOOKED', 0.00),
(39, 30, 1, 'TK639008080115348386', '2025-12-08 16:26:51', 1, 'OUTBOUND', 'BOOKED', 0.00),
(40, 31, 1, 'TK639008080115352929', '2025-12-08 16:26:51', 1, 'OUTBOUND', 'BOOKED', 0.00),
(41, 32, 1, 'TK639008080119676603', '2025-12-08 16:26:51', 1, 'OUTBOUND', 'BOOKED', 0.00),
(42, 33, 1, 'TK639008080119682841', '2025-12-08 16:26:51', 1, 'OUTBOUND', 'BOOKED', 0.00),
(43, 34, 1, 'TK639008080121253994', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(44, 35, 1, 'TK639008080121257467', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(45, 36, 1, 'TK639008080122467179', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(46, 37, 1, 'TK639008080122471483', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(47, 38, 1, 'TK639008080123742732', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(48, 39, 1, 'TK639008080123748507', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(49, 40, 1, 'TK639008080124863354', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(50, 41, 1, 'TK639008080124870715', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(51, 42, 1, 'TK639008080127440976', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(52, 43, 1, 'TK639008080127446825', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(53, 44, 1, 'TK639008080128781911', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(54, 45, 1, 'TK639008080128787554', '2025-12-08 16:26:52', 1, 'OUTBOUND', 'BOOKED', 0.00),
(55, 46, 1, 'TK639008080130863893', '2025-12-08 16:26:53', 1, 'OUTBOUND', 'BOOKED', 0.00),
(56, 47, 1, 'TK639008080130871106', '2025-12-08 16:26:53', 1, 'OUTBOUND', 'BOOKED', 0.00),
(57, 48, 1, 'TK639008080132383853', '2025-12-08 16:26:53', 1, 'OUTBOUND', 'BOOKED', 0.00),
(58, 49, 1, 'TK639008080132389589', '2025-12-08 16:26:53', 1, 'OUTBOUND', 'BOOKED', 0.00),
(59, 50, 1, 'TK639008080133654181', '2025-12-08 16:26:53', 1, 'OUTBOUND', 'BOOKED', 0.00),
(60, 51, 1, 'TK639008080133657482', '2025-12-08 16:26:53', 1, 'OUTBOUND', 'BOOKED', 0.00),
(61, 52, 1, 'TK639008080135104655', '2025-12-08 16:26:53', 1, 'OUTBOUND', 'BOOKED', 0.00),
(62, 53, 1, 'TK639008080135110533', '2025-12-08 16:26:53', 1, 'OUTBOUND', 'BOOKED', 0.00),
(63, 54, 1, 'TK639008080136546016', '2025-12-08 16:26:53', 1, 'OUTBOUND', 'BOOKED', 0.00),
(64, 55, 1, 'TK639008080136551828', '2025-12-08 16:26:53', 1, 'OUTBOUND', 'BOOKED', 0.00),
(65, 56, 1, 'TK639008080140398915', '2025-12-08 16:26:54', 1, 'OUTBOUND', 'BOOKED', 0.00),
(66, 57, 1, 'TK639008080140405481', '2025-12-08 16:26:54', 1, 'OUTBOUND', 'BOOKED', 0.00),
(67, 58, 1, 'TK639008080141657952', '2025-12-08 16:26:54', 1, 'OUTBOUND', 'BOOKED', 0.00),
(68, 59, 1, 'TK639008080141664311', '2025-12-08 16:26:54', 1, 'OUTBOUND', 'BOOKED', 0.00),
(69, 60, 1, 'TK639008081685320335', '2025-12-08 16:29:28', 1, 'OUTBOUND', 'BOOKED', 0.00),
(70, 61, 1, 'TK639008081685329717', '2025-12-08 16:29:28', 1, 'OUTBOUND', 'BOOKED', 0.00),
(71, 62, 1, 'TK639008096743510037', '2025-12-08 16:54:34', 1, 'OUTBOUND', 'BOOKED', 0.00),
(72, 63, 1, 'TK639008096743535376', '2025-12-08 16:54:34', 1, 'OUTBOUND', 'BOOKED', 0.00),
(75, 67, 145, 'TK5775275546', '2025-12-11 16:16:17', 1, 'OUTBOUND', 'BOOKED', 580900.00);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `ticket_baggage`
--

CREATE TABLE `ticket_baggage` (
  `id` int(11) NOT NULL,
  `ticket_id` int(11) NOT NULL,
  `baggage_type` enum('carry_on','checked') NOT NULL,
  `carryon_id` int(11) DEFAULT NULL,
  `checked_id` int(11) DEFAULT NULL,
  `quantity` int(11) DEFAULT 1,
  `note` varchar(255) DEFAULT NULL
) ;

--
-- Đang đổ dữ liệu cho bảng `ticket_baggage`
--

INSERT INTO `ticket_baggage` (`id`, `ticket_id`, `baggage_type`, `carryon_id`, `checked_id`, `quantity`, `note`) VALUES
(1, 1, 'carry_on', 1, NULL, 1, 'Xách tay mặc định First Class 14kg'),
(2, 1, 'checked', NULL, 3, 1, 'Khách mua thêm 20kg'),
(3, 2, 'carry_on', 1, NULL, 1, 'Xách tay mặc định 14kg'),
(4, 2, 'checked', NULL, 4, 1, 'Khách mua 25kg'),
(5, 3, 'carry_on', 2, NULL, 1, 'Xách tay mặc định Business 10kg'),
(6, 4, 'carry_on', 2, NULL, 1, 'Xách tay 10kg'),
(7, 4, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),
(8, 5, 'carry_on', 3, NULL, 1, 'Xách tay mặc định Premium Economy 10kg'),
(9, 5, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),
(10, 6, 'carry_on', 3, NULL, 1, 'Xách tay 10kg'),
(11, 7, 'carry_on', 4, NULL, 1, 'Xách tay mặc định Economy 7kg'),
(12, 7, 'carry_on', 5, NULL, 1, 'Túi cá nhân nhỏ 3kg'),
(13, 8, 'carry_on', 4, NULL, 1, 'Xách tay 7kg'),
(14, 9, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(15, 9, 'checked', NULL, 5, 1, 'Ký gửi 30kg'),
(16, 10, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(17, 10, 'checked', NULL, 6, 1, 'Ký gửi 35kg - Sport Equipment'),
(18, 11, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(19, 11, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),
(20, 12, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(21, 12, 'checked', NULL, 6, 1, 'Ký gửi 35kg - Priority'),
(22, 13, 'carry_on', 3, NULL, 1, 'Premium Economy 10kg'),
(23, 14, 'carry_on', 3, NULL, 1, 'Premium Economy 10kg'),
(24, 14, 'checked', NULL, 4, 1, 'Ký gửi 25kg'),
(25, 15, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(26, 16, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(27, 16, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),
(28, 17, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(29, 17, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),
(30, 18, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(31, 18, 'checked', NULL, 5, 1, 'Ký gửi 30kg - Musical Instrument'),
(32, 19, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(33, 19, 'checked', NULL, 4, 1, 'Ký gửi 25kg'),
(34, 20, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(35, 21, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(36, 21, 'checked', NULL, 5, 1, 'Ký gửi 30kg'),
(37, 22, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(38, 22, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),
(39, 23, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(40, 24, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(41, 24, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),
(42, 25, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(43, 25, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),
(44, 26, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(45, 26, 'checked', NULL, 4, 1, 'Ký gửi 25kg'),
(46, 27, 'carry_on', 1, NULL, 1, 'First Class 14kg'),
(47, 27, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),
(48, 28, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(49, 28, 'checked', NULL, 6, 1, 'Ký gửi 35kg - Sport Equipment'),
(50, 29, 'carry_on', 2, NULL, 1, 'Business 10kg'),
(51, 29, 'checked', NULL, 3, 1, 'Ký gửi 20kg'),
(52, 30, 'carry_on', 3, NULL, 1, 'Premium Economy 10kg'),
(54, 33, 'carry_on', 1, NULL, 1, ''),
(55, 34, 'carry_on', 1, NULL, 1, ''),
(56, 35, 'carry_on', 1, NULL, 1, ''),
(57, 36, 'carry_on', 1, NULL, 1, ''),
(58, 37, 'carry_on', 1, NULL, 1, ''),
(59, 38, 'carry_on', 1, NULL, 1, ''),
(60, 39, 'carry_on', 1, NULL, 1, ''),
(61, 40, 'carry_on', 1, NULL, 1, ''),
(62, 41, 'carry_on', 1, NULL, 1, ''),
(63, 42, 'carry_on', 1, NULL, 1, ''),
(64, 43, 'carry_on', 1, NULL, 1, ''),
(65, 44, 'carry_on', 1, NULL, 1, ''),
(66, 45, 'carry_on', 1, NULL, 1, ''),
(67, 46, 'carry_on', 1, NULL, 1, ''),
(68, 47, 'carry_on', 1, NULL, 1, ''),
(69, 48, 'carry_on', 1, NULL, 1, ''),
(70, 49, 'carry_on', 1, NULL, 1, ''),
(71, 50, 'carry_on', 1, NULL, 1, ''),
(72, 51, 'carry_on', 1, NULL, 1, ''),
(73, 52, 'carry_on', 1, NULL, 1, ''),
(74, 53, 'carry_on', 1, NULL, 1, ''),
(75, 54, 'carry_on', 1, NULL, 1, ''),
(76, 55, 'carry_on', 1, NULL, 1, ''),
(77, 56, 'carry_on', 1, NULL, 1, ''),
(78, 57, 'carry_on', 1, NULL, 1, ''),
(79, 58, 'carry_on', 1, NULL, 1, ''),
(80, 59, 'carry_on', 1, NULL, 1, ''),
(81, 60, 'carry_on', 1, NULL, 1, ''),
(82, 61, 'carry_on', 1, NULL, 1, ''),
(83, 62, 'carry_on', 1, NULL, 1, ''),
(84, 63, 'carry_on', 1, NULL, 1, ''),
(85, 64, 'carry_on', 1, NULL, 1, ''),
(86, 65, 'carry_on', 1, NULL, 1, ''),
(87, 66, 'carry_on', 1, NULL, 1, ''),
(88, 67, 'carry_on', 1, NULL, 1, ''),
(89, 68, 'carry_on', 1, NULL, 1, ''),
(90, 69, 'carry_on', 1, NULL, 1, 'test'),
(91, 70, 'carry_on', 1, NULL, 1, 'test_hành lý'),
(92, 71, 'carry_on', 1, NULL, 1, ''),
(93, 72, 'carry_on', 1, NULL, 1, ''),
(94, 75, 'carry_on', 1, NULL, 1, ''),
(95, 75, 'checked', NULL, 4, 1, 'test database 1');

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `ticket_history`
--

INSERT INTO `ticket_history` (`history_id`, `ticket_id`, `old_status`, `new_status`, `changed_at`) VALUES
(1, 1, 'BOOKED', 'CONFIRMED', '2025-01-02 09:12:00'),
(2, 2, 'BOOKED', 'CONFIRMED', '2025-01-05 10:20:00'),
(3, 2, 'CONFIRMED', 'CHECKED_IN', '2025-01-06 08:15:00'),
(4, 3, 'BOOKED', 'CANCELLED', '2025-01-03 11:44:00'),
(5, 4, 'BOOKED', 'CONFIRMED', '2025-02-01 15:30:00'),
(6, 5, 'BOOKED', 'CONFIRMED', '2025-02-10 18:05:00'),
(7, 6, 'BOOKED', 'CONFIRMED', '2025-01-12 16:20:00'),
(8, 6, 'CONFIRMED', 'CHECKED_IN', '2025-01-13 06:55:00'),
(9, 7, 'BOOKED', 'CONFIRMED', '2025-03-03 09:55:00'),
(10, 8, 'BOOKED', 'CONFIRMED', '2025-03-10 13:44:00'),
(11, 8, 'CONFIRMED', 'CANCELLED', '2025-03-11 07:22:00'),
(12, 9, 'BOOKED', 'CONFIRMED', '2025-03-21 12:00:00'),
(13, 9, 'CONFIRMED', 'CHECKED_IN', '2025-03-22 08:45:00'),
(14, 10, 'BOOKED', 'CONFIRMED', '2025-04-01 19:30:00'),
(15, 11, 'BOOKED', 'CONFIRMED', '2025-04-11 09:14:00'),
(16, 11, 'CONFIRMED', 'CHECKED_IN', '2025-04-12 05:30:00'),
(17, 12, 'BOOKED', 'CONFIRMED', '2025-05-02 11:50:00'),
(18, 13, 'BOOKED', 'CONFIRMED', '2025-05-06 15:10:00'),
(19, 13, 'CONFIRMED', 'CHECKED_IN', '2025-05-06 18:30:00'),
(20, 14, 'BOOKED', 'CONFIRMED', '2025-05-14 20:55:00'),
(21, 15, 'BOOKED', 'CANCELLED', '2025-05-03 14:15:00'),
(22, 16, 'BOOKED', 'CONFIRMED', '2025-06-01 10:25:00'),
(23, 16, 'CONFIRMED', 'CHECKED_IN', '2025-06-01 18:40:00'),
(24, 16, 'CHECKED_IN', 'BOARDED', '2025-06-02 06:00:00'),
(25, 17, 'BOOKED', 'CONFIRMED', '2025-06-12 09:00:00'),
(26, 18, 'BOOKED', 'CONFIRMED', '2025-06-20 13:33:00'),
(27, 18, 'CONFIRMED', 'REFUNDED', '2025-06-21 09:40:00'),
(28, 19, 'BOOKED', 'CONFIRMED', '2025-06-25 08:44:00'),
(29, 19, 'CONFIRMED', 'CHECKED_IN', '2025-06-26 05:22:00'),
(30, 20, 'BOOKED', 'REFUNDED', '2025-07-01 14:20:00'),
(31, 21, 'BOOKED', 'CONFIRMED', '2025-01-20 10:15:00'),
(32, 22, 'BOOKED', 'CONFIRMED', '2025-01-21 14:30:00'),
(33, 23, 'BOOKED', 'CONFIRMED', '2025-01-22 09:45:00'),
(34, 23, 'CONFIRMED', 'CHECKED_IN', '2025-01-27 16:00:00'),
(35, 24, 'BOOKED', 'CONFIRMED', '2025-01-23 11:20:00'),
(36, 25, 'BOOKED', 'CONFIRMED', '2025-01-24 15:50:00'),
(37, 26, 'BOOKED', 'CONFIRMED', '2025-07-05 08:30:00'),
(38, 27, 'BOOKED', 'CONFIRMED', '2025-07-06 10:15:00'),
(39, 28, 'BOOKED', 'CONFIRMED', '2025-07-07 13:20:00'),
(40, 28, 'CONFIRMED', 'CHECKED_IN', '2025-07-13 10:00:00'),
(41, 29, 'BOOKED', 'CONFIRMED', '2025-07-08 16:45:00'),
(42, 30, 'BOOKED', 'CONFIRMED', '2025-07-09 09:30:00');

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
-- Chỉ mục cho bảng `carryon_baggage`
--
ALTER TABLE `carryon_baggage`
  ADD PRIMARY KEY (`carryon_id`),
  ADD KEY `class_id` (`class_id`);

--
-- Chỉ mục cho bảng `checked_baggage`
--
ALTER TABLE `checked_baggage`
  ADD PRIMARY KEY (`checked_id`);

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
  ADD KEY `route_id` (`route_id`),
  ADD KEY `idx_flights_is_deleted` (`is_deleted`);

--
-- Chỉ mục cho bảng `flight_seats`
--
ALTER TABLE `flight_seats`
  ADD PRIMARY KEY (`flight_seat_id`),
  ADD KEY `flight_id` (`flight_id`),
  ADD KEY `seat_id` (`seat_id`);

--
-- Chỉ mục cho bảng `national`
--
ALTER TABLE `national`
  ADD PRIMARY KEY (`national_id`),
  ADD UNIQUE KEY `country_name` (`country_name`),
  ADD UNIQUE KEY `country_code` (`country_code`);

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
-- Chỉ mục cho bảng `ticket_baggage`
--
ALTER TABLE `ticket_baggage`
  ADD PRIMARY KEY (`id`),
  ADD KEY `ticket_id` (`ticket_id`),
  ADD KEY `carryon_id` (`carryon_id`),
  ADD KEY `checked_id` (`checked_id`);

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
  MODIFY `account_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT cho bảng `aircrafts`
--
ALTER TABLE `aircrafts`
  MODIFY `aircraft_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT cho bảng `airlines`
--
ALTER TABLE `airlines`
  MODIFY `airline_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `airports`
--
ALTER TABLE `airports`
  MODIFY `airport_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `baggage`
--
ALTER TABLE `baggage`
  MODIFY `baggage_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- AUTO_INCREMENT cho bảng `baggage_history`
--
ALTER TABLE `baggage_history`
  MODIFY `history_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=46;

--
-- AUTO_INCREMENT cho bảng `bookings`
--
ALTER TABLE `bookings`
  MODIFY `booking_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=44;

--
-- AUTO_INCREMENT cho bảng `booking_passengers`
--
ALTER TABLE `booking_passengers`
  MODIFY `booking_passenger_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=68;

--
-- AUTO_INCREMENT cho bảng `cabin_classes`
--
ALTER TABLE `cabin_classes`
  MODIFY `class_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT cho bảng `carryon_baggage`
--
ALTER TABLE `carryon_baggage`
  MODIFY `carryon_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT cho bảng `checked_baggage`
--
ALTER TABLE `checked_baggage`
  MODIFY `checked_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT cho bảng `fare_rules`
--
ALTER TABLE `fare_rules`
  MODIFY `rule_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT cho bảng `flights`
--
ALTER TABLE `flights`
  MODIFY `flight_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT cho bảng `flight_seats`
--
ALTER TABLE `flight_seats`
  MODIFY `flight_seat_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=289;

--
-- AUTO_INCREMENT cho bảng `national`
--
ALTER TABLE `national`
  MODIFY `national_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `passenger_profiles`
--
ALTER TABLE `passenger_profiles`
  MODIFY `profile_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=52;

--
-- AUTO_INCREMENT cho bảng `payments`
--
ALTER TABLE `payments`
  MODIFY `payment_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

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
  MODIFY `route_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT cho bảng `seats`
--
ALTER TABLE `seats`
  MODIFY `seat_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=181;

--
-- AUTO_INCREMENT cho bảng `tickets`
--
ALTER TABLE `tickets`
  MODIFY `ticket_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=76;

--
-- AUTO_INCREMENT cho bảng `ticket_baggage`
--
ALTER TABLE `ticket_baggage`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `ticket_history`
--
ALTER TABLE `ticket_history`
  MODIFY `history_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=43;

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
-- Các ràng buộc cho bảng `carryon_baggage`
--
ALTER TABLE `carryon_baggage`
  ADD CONSTRAINT `carryon_baggage_ibfk_1` FOREIGN KEY (`class_id`) REFERENCES `cabin_classes` (`class_id`);

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
-- Các ràng buộc cho bảng `ticket_baggage`
--
ALTER TABLE `ticket_baggage`
  ADD CONSTRAINT `ticket_baggage_ibfk_1` FOREIGN KEY (`ticket_id`) REFERENCES `tickets` (`ticket_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `ticket_baggage_ibfk_2` FOREIGN KEY (`carryon_id`) REFERENCES `carryon_baggage` (`carryon_id`),
  ADD CONSTRAINT `ticket_baggage_ibfk_3` FOREIGN KEY (`checked_id`) REFERENCES `checked_baggage` (`checked_id`);

--
-- Các ràng buộc cho bảng `ticket_history`
--
ALTER TABLE `ticket_history`
  ADD CONSTRAINT `ticket_history_ibfk_1` FOREIGN KEY (`ticket_id`) REFERENCES `tickets` (`ticket_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
