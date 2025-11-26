# 🔧 KHẮC PHỤC LỖI CRITICAL - CHUYẾN BAY & THỐNG KÊ

## 📌 FIX 1: FlightDAO.cs - SQL Typo (Dòng 338)

**File:** `DAO/Flight/FlightDAO.cs`

**Tìm:**
```csharp
ORDER BY departue_time DESC";
```

**Sửa thành:**
```csharp
ORDER BY departure_time DESC";
```

---

## 📌 FIX 2: FlightBUS.cs - Nullable Warnings

**File:** `BUS/Flight/FlightBUS.cs`

### 2A. Dòng 18 - Singleton field
**Tìm:**
```csharp
private static FlightBUS _instance;
```

**Sửa thành:**
```csharp
private static FlightBUS? _instance = null;
```

### 2B. Dòng 116 - DepartureTime null check
**Tìm:**
```csharp
if (FlightDAO.Instance.IsFlightNumberExists(
    flight.FlightNumber,
    flight.DepartureTime.Value))
```

**Sửa thành:**
```csharp
if (flight.DepartureTime.HasValue && FlightDAO.Instance.IsFlightNumberExists(
    flight.FlightNumber,
    flight.DepartureTime.Value))
```

### 2C. Dòng 186 - DepartureTime null check
**Tìm:**
```csharp
if (FlightDAO.Instance.IsFlightNumberExists(
    flight.FlightNumber,
    flight.DepartureTime.Value,
    flight.FlightId))
```

**Sửa thành:**
```csharp
if (flight.DepartureTime.HasValue && FlightDAO.Instance.IsFlightNumberExists(
    flight.FlightNumber,
    flight.DepartureTime.Value,
    flight.FlightId))
```

---

## 📌 FIX 3: FlightListControl.cs - Disable "Mọi thời điểm" cho User

**File:** `GUI/Features/Flight/SubFeatures/FlightListControl.cs`

**Tìm (khoảng dòng 108-113):**
```csharp
if (_role == AppRole.User)
{
    textFieldMaChuyenBay.Visible = false;
    checkBoxTimKiemMaChuyenBay.Visible = false;
}
```

**Sửa thành:**
```csharp
if (_role == AppRole.User)
{
    textFieldMaChuyenBay.Visible = false;
    checkBoxTimKiemMaChuyenBay.Visible = false;
    checkBoxTimKiemMaChuyenBay.Checked = false;  // ✅ THÊM
    checkBoxTimKiemMaChuyenBay.Enabled = false;  // ✅ THÊM
}
```

---

## 📌 FIX 4: Database - Thêm Indexes cho Performance

**File:** Chạy script SQL này trên database `flightTicketManagement`

```sql
USE `flightTicketManagement`;

-- Index cho filter departure_time (rất quan trọng!)
CREATE INDEX idx_flights_departure_time ON Flights(departure_time);

-- Index cho filter status
CREATE INDEX idx_flights_status ON Flights(status);

-- Index cho thống kê payment_date
CREATE INDEX idx_payments_date ON Payments(payment_date);

-- Index cho join booking
CREATE INDEX idx_payments_booking ON Payments(booking_id, status);

-- Kiểm tra indexes đã tạo
SHOW INDEX FROM Flights WHERE Key_name LIKE 'idx_%';
SHOW INDEX FROM Payments WHERE Key_name LIKE 'idx_%';
```

---

## 📌 FIX 5: StatsDAO.cs - Loại trừ Bookings CANCELLED

**File:** `DAO/Stats/StatsDAO.cs`

**Tìm (trong GetRevenueSummary):**
```csharp
string query = @"
    SELECT 
        SUM(amount) AS TotalRevenue,
        COUNT(payment_id) AS TotalTransactions
    FROM 
        Payments
    WHERE 
        status = 'SUCCESS' 
        AND YEAR(payment_date) = @year";
```

**Sửa thành:**
```csharp
string query = @"
    SELECT 
        SUM(p.amount) AS TotalRevenue,
        COUNT(p.payment_id) AS TotalTransactions
    FROM 
        Payments p
    JOIN Bookings b ON p.booking_id = b.booking_id
    WHERE 
        p.status = 'SUCCESS' 
        AND b.status IN ('CONFIRMED')
        AND YEAR(p.payment_date) = @year";
```

**Áp dụng tương tự cho:**
- `GetMonthlyRevenue` method
- `GetRevenueByRoute` method (đã có JOIN rồi, chỉ cần thêm điều kiện `b.status`)

---

## ✅ CHECKLIST SAU KHI FIX

- [ ] Build project thành công (0 errors)
- [ ] Test FlightListControl với role User → Checkbox bị disable
- [ ] Test tạo chuyến bay mới → Không còn nullable warning
- [ ] Test lọc chuyến bay theo ngày → Query nhanh hơn
- [ ] Test báo cáo thống kê → Loại trừ bookings CANCELLED

---

## 📊 IMPACT ANALYSIS

| Fix | Severity | Impact | Test Priority |
|-----|----------|--------|---------------|
| SQL Typo | 🔴 Critical | Crash runtime | HIGH |
| Nullable | 🟠 High | Compile warning | MEDIUM |
| User checkbox | 🟠 High | Security bug | HIGH |
| DB Indexes | 🔴 Critical | Performance 10x+ | HIGH |
| Stats query | 🟠 High | Doanh thu sai | HIGH |
