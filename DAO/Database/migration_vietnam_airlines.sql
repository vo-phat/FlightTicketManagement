-- ============================================================================
-- MIGRATION: Chuyển đổi sang mô hình quản lý chỉ VIETNAM AIRLINES
-- Date: 2025-12-04
-- ============================================================================

USE `FlightTicketManagement`;

SET FOREIGN_KEY_CHECKS = 0;

-- BƯỚC 1: Backup dữ liệu Airlines hiện tại (nếu cần)
-- CREATE TABLE Airlines_backup AS SELECT * FROM Airlines;

-- BƯỚC 2: Cập nhật bảng Aircrafts
-- Thêm cột mới
ALTER TABLE Aircrafts 
    ADD COLUMN registration_number VARCHAR(20) AFTER aircraft_id,
    ADD COLUMN manufacture_year INT AFTER capacity,
    ADD COLUMN `status` ENUM('ACTIVE','MAINTENANCE','RETIRED') DEFAULT 'ACTIVE' AFTER manufacture_year;

-- Migrate dữ liệu: Tạo registration_number từ aircraft_id
UPDATE Aircrafts 
SET registration_number = CONCAT('VN-A', LPAD(aircraft_id, 3, '0'))
WHERE registration_number IS NULL;

-- Thêm UNIQUE constraint
ALTER TABLE Aircrafts ADD UNIQUE KEY `uk_registration` (registration_number);

-- Xóa foreign key constraint với Airlines
ALTER TABLE Aircrafts DROP FOREIGN KEY IF EXISTS `fk_aircraft_airline`;
ALTER TABLE Aircrafts DROP FOREIGN KEY IF EXISTS `Aircrafts_ibfk_1`;

-- Xóa cột airline_id
ALTER TABLE Aircrafts DROP COLUMN airline_id;

-- BƯỚC 3: XÓA HOÀN TOÀN bảng Airlines (không còn cần thiết)
DROP TABLE IF EXISTS Airlines;

-- BƯỚC 4: Cập nhật dữ liệu mẫu
-- Đảm bảo tất cả máy bay đều có status
UPDATE Aircrafts SET `status` = 'ACTIVE' WHERE `status` IS NULL;

-- BƯỚC 5: Thêm comment cho bảng
ALTER TABLE Aircrafts COMMENT = 'Máy bay của Vietnam Airlines';

SET FOREIGN_KEY_CHECKS = 1;

-- ============================================================================
-- DỮ LIỆU MẪU VIETNAM AIRLINES
-- ============================================================================

-- Thêm máy bay mẫu nếu bảng trống
INSERT INTO Aircrafts (registration_number, model, manufacturer, capacity, manufacture_year, `status`)
VALUES 
    ('VN-A801', 'Boeing 787-9', 'Boeing', 274, 2015, 'ACTIVE'),
    ('VN-A802', 'Boeing 787-9', 'Boeing', 274, 2016, 'ACTIVE'),
    ('VN-A803', 'Boeing 787-10', 'Boeing', 310, 2018, 'ACTIVE'),
    ('VN-A861', 'Airbus A350-900', 'Airbus', 305, 2017, 'ACTIVE'),
    ('VN-A862', 'Airbus A350-900', 'Airbus', 305, 2018, 'ACTIVE'),
    ('VN-A863', 'Airbus A350-900', 'Airbus', 305, 2019, 'ACTIVE'),
    ('VN-A891', 'Airbus A321neo', 'Airbus', 220, 2019, 'ACTIVE'),
    ('VN-A892', 'Airbus A321neo', 'Airbus', 220, 2020, 'ACTIVE'),
    ('VN-A893', 'Airbus A321neo', 'Airbus', 220, 2020, 'ACTIVE'),
    ('VN-A870', 'Boeing 777-200ER', 'Boeing', 294, 2010, 'ACTIVE'),
    ('VN-A140', 'Airbus A321-200', 'Airbus', 190, 2012, 'MAINTENANCE'),
    ('VN-A150', 'Airbus A321-200', 'Airbus', 190, 2013, 'ACTIVE')
ON DUPLICATE KEY UPDATE registration_number = registration_number;

-- ============================================================================
-- VERIFICATION QUERIES
-- ============================================================================
-- SELECT * FROM Aircrafts ORDER BY registration_number;
-- SHOW CREATE TABLE Aircrafts;
