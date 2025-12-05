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

-- 2. Vai trò
CREATE TABLE roles (
    role_id                 INT AUTO_INCREMENT PRIMARY KEY,
    role_code               VARCHAR(50) NOT NULL UNIQUE,
    role_name               VARCHAR(100) NOT NULL
);

-- 3. Gán vai trò cho tài khoản (N-N)
CREATE TABLE account_role (
    account_id INT NOT NULL,
    role_id    INT NOT NULL,
    PRIMARY KEY (account_id, role_id),
    FOREIGN KEY (account_id) REFERENCES accounts(account_id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES roles(role_id) ON DELETE CASCADE
);

-- 4. Quyền
CREATE TABLE permissions (
    permission_id INT AUTO_INCREMENT PRIMARY KEY,
    permission_code     VARCHAR(100) NOT NULL UNIQUE,
    permission_name     VARCHAR(150) NOT NULL
);

-- 5. Vai trò - Quyền (N-N)
CREATE TABLE role_permissions (
    role_id       INT NOT NULL,
    permission_id INT NOT NULL,
    PRIMARY KEY (role_id, permission_id),
    FOREIGN KEY (role_id) REFERENCES roles(role_id) ON DELETE CASCADE,
    FOREIGN KEY (permission_id) REFERENCES permissions(permission_id) ON DELETE CASCADE
);

-- 5. Hãng hàng không
CREATE TABLE Airlines (
    airline_id INT AUTO_INCREMENT PRIMARY KEY,
    airline_code VARCHAR(10) UNIQUE NOT NULL,
    airline_name VARCHAR(100) NOT NULL,
    country VARCHAR(100)
);

-- 6. Sân bay
CREATE TABLE Airports (
    airport_id INT AUTO_INCREMENT PRIMARY KEY,
    airport_code VARCHAR(10) UNIQUE NOT NULL,
    airport_name VARCHAR(100) NOT NULL,
    city VARCHAR(100),
    country VARCHAR(100)
);

-- 7. Máy bay
CREATE TABLE Aircrafts (
    aircraft_id INT AUTO_INCREMENT PRIMARY KEY,
    airline_id INT,
    model VARCHAR(100), 
    manufacturer VARCHAR(100),
    capacity INT,
    FOREIGN KEY (airline_id) REFERENCES Airlines(airline_id)
);

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

-- 10. Hạng ghế
CREATE TABLE Cabin_Classes (
    class_id INT AUTO_INCREMENT PRIMARY KEY,
    class_name VARCHAR(50) NOT NULL,
    `description` VARCHAR(255)
);

-- 11. Ghế ngồi trên máy bay
CREATE TABLE Seats (
    seat_id INT AUTO_INCREMENT PRIMARY KEY,
    aircraft_id INT,
    seat_number VARCHAR(10),
    class_id INT,
    FOREIGN KEY (aircraft_id) REFERENCES Aircrafts(aircraft_id),
    FOREIGN KEY (class_id) REFERENCES Cabin_Classes(class_id)
);

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

-- 16. Hành khách trong từng booking
CREATE TABLE Booking_Passengers (
    booking_passenger_id INT AUTO_INCREMENT PRIMARY KEY,
    booking_id INT,
    profile_id INT,
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id),
    FOREIGN KEY (profile_id) REFERENCES Passenger_Profiles(profile_id)
);

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
    FOREIGN KEY (ticket_passenger_id) REFERENCES Booking_Passengers(booking_passenger_id),
    FOREIGN KEY (flight_seat_id) REFERENCES Flight_Seats(flight_seat_id)
);

-- 18. Lịch sử trạng thái vé
CREATE TABLE Ticket_History (
    history_id INT AUTO_INCREMENT PRIMARY KEY,
    ticket_id INT,
    old_status ENUM('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED'),
    new_status ENUM('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED'),
    changed_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ticket_id) REFERENCES Tickets(ticket_id)
);

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

-- 21. Lịch sử trạng thái hành lý
CREATE TABLE Baggage_History (
    history_id INT AUTO_INCREMENT PRIMARY KEY,
    baggage_id INT NOT NULL,
    old_status ENUM('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST'),
    new_status ENUM('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST'),
    changed_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (baggage_id) REFERENCES Baggage(baggage_id)
);

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

-- Checked baggage
CREATE TABLE checked_baggage (
  checked_id INT AUTO_INCREMENT PRIMARY KEY,
  weight_kg INT NOT NULL,
  price DECIMAL(10,2) NOT NULL,
  description VARCHAR(255)
);

-- National
CREATE TABLE national (
  national_id INT AUTO_INCREMENT PRIMARY KEY,
  country_name VARCHAR(100) NOT NULL UNIQUE,
  country_code CHAR(2) NOT NULL UNIQUE,
  phone_code VARCHAR(5)
);

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

-- Update existing flights with default price (if any exist)
UPDATE Flights SET base_price = 1000000 WHERE base_price = 0;

-- Create index for better query performance
CREATE INDEX idx_flights_is_deleted ON Flights(is_deleted);

-- Update existing data to ensure all flights are marked as not deleted
UPDATE Flights SET is_deleted = FALSE WHERE is_deleted IS NULL;

SELECT 'Migration completed successfully!' AS Status;