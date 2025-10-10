CREATE DATABASE IF NOT EXISTS `AirlineBookingSystem`
  DEFAULT CHARACTER SET utf8mb4
  DEFAULT COLLATE utf8mb4_0900_ai_ci;

USE `AirlineBookingSystem`;

-- 1. Người dùng
CREATE TABLE Accounts (
    account_id INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(100) UNIQUE NOT NULL,
    `password` VARCHAR(255) NOT NULL
);

-- 2. Vai trò 
CREATE TABLE Roles (
    role_id INT AUTO_INCREMENT PRIMARY KEY,
    role_name VARCHAR(50)
);

-- 3. Liên kết User - Role (N-N)
CREATE TABLE User_Role (
	user_role_id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT,
    role_id INT,
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id),
    FOREIGN KEY (role_id) REFERENCES Roles(role_id)
);

-- 4. Hãng hàng không
CREATE TABLE Airlines (
    airline_id INT AUTO_INCREMENT PRIMARY KEY,
    airline_code VARCHAR(10) UNIQUE NOT NULL,
    airline_name VARCHAR(100) NOT NULL,
    country VARCHAR(100)
);

-- 5. Sân bay
CREATE TABLE Airports (
    airport_id INT AUTO_INCREMENT PRIMARY KEY,
    airport_code VARCHAR(10) UNIQUE NOT NULL,
    airport_name VARCHAR(100) NOT NULL,
    city VARCHAR(100),
    country VARCHAR(100)
);

-- 6. Máy bay
CREATE TABLE Aircrafts (
    aircraft_id INT AUTO_INCREMENT PRIMARY KEY,
    airline_id INT,
    model VARCHAR(100), 
    manufacturer VARCHAR(100),
    capacity INT,
    FOREIGN KEY (airline_id) REFERENCES Airlines(airline_id)
);

-- 7. Tuyến bay
CREATE TABLE Routes (
    route_id INT AUTO_INCREMENT PRIMARY KEY,
    from_airport_id INT,
    to_airport_id INT,
    distance_km INT,
    duration_minutes INT,
    FOREIGN KEY (from_airport_id) REFERENCES Airports(airport_id),
    FOREIGN KEY (to_airport_id) REFERENCES Airports(airport_id)
);

-- 8. Chuyến bay
CREATE TABLE Flights (
    flight_id INT AUTO_INCREMENT PRIMARY KEY,
    flight_number VARCHAR(20) NOT NULL,
    aircraft_id INT,
    route_id INT,
    departure_time DATETIME,
    arrival_time DATETIME,
    `status` ENUM('SCHEDULED','DELAYED','CANCELLED','COMPLETED') DEFAULT 'SCHEDULED',
    FOREIGN KEY (aircraft_id) REFERENCES Aircrafts(aircraft_id),
    FOREIGN KEY (route_id) REFERENCES Routes(route_id)
);

-- 9. Hạng vé
CREATE TABLE Cabin_Classes (
    class_id INT AUTO_INCREMENT PRIMARY KEY,
    class_name VARCHAR(50) NOT NULL, -- Economy, Business, First
    `description` VARCHAR(255)
);

-- 10. Ghế ngồi trên máy bay (chỉ định danh ghế, không lưu trạng thái)
CREATE TABLE Seats (
    seat_id INT AUTO_INCREMENT PRIMARY KEY,
    aircraft_id INT,
    seat_number VARCHAR(10),
    class_id INT,
    FOREIGN KEY (aircraft_id) REFERENCES Aircrafts(aircraft_id),
    FOREIGN KEY (class_id) REFERENCES Cabin_Classes(class_id)
);

-- 11. Ghế cho từng chuyến bay (trạng thái quản lý theo chuyến)
CREATE TABLE Flight_Seats (
    flight_seat_id INT AUTO_INCREMENT PRIMARY KEY,
    flight_id INT,
    seat_id INT,
    base_price DECIMAL(12,2),
    seat_status ENUM('AVAILABLE','BOOKED','BLOCKED') DEFAULT 'AVAILABLE',
    FOREIGN KEY (flight_id) REFERENCES Flights(flight_id),
    FOREIGN KEY (seat_id) REFERENCES Seats(seat_id)
);

-- 12. Quy tắc giá vé (Fare Rules, tách logic định giá)
CREATE TABLE Fare_Rules (
    rule_id INT AUTO_INCREMENT PRIMARY KEY,
    route_id INT,
    class_id INT,
    fare_type VARCHAR(50) DEFAULT 'Standard', -- Saver, Flex, Promo...
    season ENUM('PEAK','OFFPEAK','NORMAL') DEFAULT 'NORMAL',
    effective_date DATE,
    expiry_date DATE,
    `description` VARCHAR(255),
    price DECIMAL(12,2),
    FOREIGN KEY (route_id) REFERENCES Routes(route_id),
    FOREIGN KEY (class_id) REFERENCES Cabin_Classes(class_id)
);

-- 13. Đặt chỗ
CREATE TABLE Bookings (
    booking_id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT,
    booking_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    `status` ENUM('PENDING','CONFIRMED','CANCELLED','REFUNDED') DEFAULT 'PENDING',
    total_amount DECIMAL(12,2),
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id)
);

-- 14. Hồ sơ hành khách (Profile lâu dài)
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

-- 15. Hành khách trong từng booking
CREATE TABLE Booking_Passengers (
    booking_passenger_id INT AUTO_INCREMENT PRIMARY KEY,
    booking_id INT,
    profile_id INT,
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id),
    FOREIGN KEY (profile_id) REFERENCES Passenger_Profiles(profile_id)
);

-- 16. Vé
CREATE TABLE Tickets (
    ticket_id INT AUTO_INCREMENT PRIMARY KEY,
    ticket_passenger_id INT,
    flight_seat_id INT,
    ticket_number VARCHAR(50) UNIQUE,
    issue_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    `status` ENUM('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED') DEFAULT 'BOOKED',
    FOREIGN KEY (ticket_passenger_id) REFERENCES Booking_Passengers(booking_passenger_id),
    FOREIGN KEY (flight_seat_id) REFERENCES Flight_Seats(flight_seat_id)
);

-- 17. Lịch sử trạng thái vé
CREATE TABLE Ticket_History (
    history_id INT AUTO_INCREMENT PRIMARY KEY,
    ticket_id INT,
    old_status ENUM('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED'),
    new_status ENUM('BOOKED','CONFIRMED','CHECKED_IN','BOARDED','CANCELLED','REFUNDED'),
    changed_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ticket_id) REFERENCES Tickets(ticket_id)
);

-- 18. Thanh toán
CREATE TABLE Payments (
    payment_id INT AUTO_INCREMENT PRIMARY KEY,
    booking_id INT,
    amount DECIMAL(12,2),
    payment_method ENUM('CREDIT_CARD','BANK_TRANSFER','E_WALLET','CASH'),
    payment_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    status ENUM('SUCCESS','FAILED','PENDING') DEFAULT 'PENDING',
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
);

-- 19. Thông báo
CREATE TABLE Notifications (
    notification_id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT,
    message TEXT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    is_read BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id)
);

-- 20. Hành lý
CREATE TABLE Baggage (
    baggage_id INT AUTO_INCREMENT PRIMARY KEY,
    ticket_id INT NOT NULL,                       -- Liên kết với vé
    flight_id INT NOT NULL,                       -- Liên kết với chuyến bay
    baggage_tag VARCHAR(20) UNIQUE,               -- Mã nhãn hành lý
    baggage_type ENUM('CHECKED','CARRY_ON','SPECIAL') DEFAULT 'CHECKED', -- Ký gửi, xách tay, đặc biệt
    weight_kg DECIMAL(5,2) NOT NULL,             -- Trọng lượng thực tế
    allowed_weight_kg DECIMAL(5,2) NOT NULL,     -- Trọng lượng miễn phí theo cabin class / hạng vé
    fee DECIMAL(12,2) DEFAULT 0.00,              -- Phí phát sinh nếu vượt chuẩn
    `status` ENUM('CREATED','CHECKED_IN','LOADED','IN_TRANSIT','CLAIMED','LOST') DEFAULT 'CREATED',
    special_handling VARCHAR(100),               -- Mô tả xử lý đặc biệt (ví dụ dụng cụ thể thao)
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
