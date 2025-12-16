# 🛫 Flight Ticket Management System

Hệ thống quản lý vé máy bay được xây dựng bằng C# WinForms và MySQL.

## 📋 Mục lục

- [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
- [Cài đặt Database](#cài-đặt-database)
- [Chạy ứng dụng](#chạy-ứng-dụng)
- [Tài khoản đăng nhập](#tài-khoản-đăng-nhập)
- [Tính năng chính](#tính-năng-chính)

---

## 🖥️ Yêu cầu hệ thống

- **OS**: Windows 10 trở lên
- **.NET SDK**: .NET 6.0 hoặc cao hơn
- **Database**: MySQL 8.0 hoặc cao hơn
- **IDE** (khuyến nghị): Visual Studio 2022 hoặc Visual Studio Code

---

## 🗄️ Cài đặt Database

### Bước 1: Cài đặt MySQL

Tải và cài đặt MySQL từ [https://dev.mysql.com/downloads/mysql/](https://dev.mysql.com/downloads/mysql/)

### Bước 2: Tạo Database

1. Mở MySQL Workbench hoặc Command Line
2. Chạy file SQL để tạo database:

```bash
mysql -u root -p < DAO/Database/database.sql
```

Hoặc trong MySQL Workbench:
- File → Open SQL Script → Chọn `DAO/Database/database.sql`
- Execute (Ctrl + Shift + Enter)

### Bước 3: Cấu hình kết nối

File cấu hình kết nối database nằm tại: `DAO/DAOBase.cs`

Mặc định:
```csharp
Host: localhost
Port: 3306
Database: FlightTicketManagement
Username: root
Password: (mật khẩu MySQL của bạn)
```

---

## 🚀 Chạy ứng dụng

### Sử dụng Visual Studio

1. Mở solution: `FlightTicketManagement.sln`
2. Set project `GUI` làm startup project
3. Nhấn F5 hoặc Run

### Sử dụng Command Line

```bash
cd gui
dotnet run
```

---

## 🔐 Tài khoản đăng nhập

Hệ thống có 3 loại tài khoản với các quyền khác nhau:

### 👨‍💼 ADMIN (Quản trị viên)

| Email | Mật khẩu | Mô tả |
|-------|----------|-------|
| `admin` | `admin` | Tài khoản quản trị chính |
| `hongqui@sv.sgu.edu.vn` | `admin` | Tài khoản quản trị phụ |
| `phuocnam@yahoo.com` | `admin` | Tài khoản quản trị phụ |

**Quyền hạn**: Toàn quyền - Quản lý tất cả các chức năng của hệ thống

---

### 👔 STAFF (Nhân viên)

| Email | Mật khẩu | Mô tả |
|-------|----------|-------|
| `staff@test.com` | `admin` | Tài khoản nhân viên test |
| `phamnam@hotmail.com` | `admin` | Tài khoản nhân viên |
| `quangphong@gmail.com` | `admin` | Tài khoản nhân viên |

**Quyền hạn**: 
- Tra cứu chuyến bay
- Đặt chỗ cho khách hàng
- Quản lý vé
- Check-in hành lý
- Theo dõi hành lý
- Thanh toán POS
- Xem báo cáo

---

### 👤 USER (Người dùng/Khách hàng)

| Email | Mật khẩu | Mô tả |
|-------|----------|-------|
| `user@test.com` | `admin` | Tài khoản khách hàng test |
| `vophat@outlook.com.vn` | `admin` | Tài khoản khách hàng |

**Quyền hạn**:
- Tìm kiếm chuyến bay
- Đặt vé
- Xem đặt chỗ của tôi
- Quản lý vé của tôi
- Quản lý hồ sơ cá nhân
- Xem thông báo

---

## ✨ Tính năng chính

### 🎫 Quản lý vé
- Đặt vé một chiều / khứ hồi / nhiều chặng
- Chọn hạng ghế (First, Business, Premium Economy, Economy)
- Quản lý trạng thái vé (Đã đặt, Đã xác nhận, Đã check-in, Đã lên máy bay)
- Hủy và hoàn vé

### ✈️ Quản lý chuyến bay
- Tạo và quản lý chuyến bay
- Phân bổ ghế ngồi
- Theo dõi trạng thái chuyến bay (Đã lên lịch, Đang bay, Hoàn thành, Đã hủy, Trì hoãn)
- Tìm kiếm và lọc chuyến bay nâng cao

### 🎒 Quản lý hành lý
- Check-in hành lý xách tay (Carry-on)
- Check-in hành lý ký gửi (Checked)
- Xử lý hành lý đặc biệt (Special)
- Theo dõi trạng thái hành lý
- Báo cáo hành lý thất lạc

### 💳 Thanh toán
- Nhiều phương thức thanh toán (Thẻ tín dụng, Chuyển khoản, Ví điện tử, Tiền mặt)
- Theo dõi trạng thái thanh toán
- Lịch sử giao dịch

### 📊 Báo cáo và Thống kê
- Thống kê doanh thu theo thời gian
- Thống kê chuyến bay
- Thống kê thanh toán
- Thống kê tuyến bay và máy bay
- Biểu đồ trực quan

### 🏢 Quản lý danh mục
- Hãng hàng không
- Máy bay
- Sân bay
- Tuyến bay
- Hạng ghế
- Ghế ngồi
- Quy tắc giá vé

### 👥 Quản lý tài khoản
- Đăng ký, đăng nhập, quên mật khẩu
- Phân quyền theo vai trò (Role-based Access Control)
- Quản lý hồ sơ hành khách
- Khóa/mở khóa tài khoản

---

## 📁 Cấu trúc Project

```
FlightTicketManagement/
├── DAO/              # Data Access Objects - Truy xuất database
├── DTO/              # Data Transfer Objects - Các model dữ liệu
├── BUS/              # Business Logic - Xử lý nghiệp vụ
└── gui/              # Giao diện người dùng (WinForms)
    ├── Components/   # Các component tái sử dụng
    └── Features/     # Các chức năng chính
```

---

## 🔧 Khắc phục sự cố

### Lỗi kết nối Database

Kiểm tra:
1. MySQL Server đã chạy chưa
2. Thông tin kết nối trong `DAO/DAOBase.cs` đúng chưa
3. Firewall có chặn kết nối MySQL không

### Lỗi đăng nhập

- Đảm bảo database đã được import đầy đủ
- Thử tài khoản `admin` / `admin`
- Kiểm tra table `accounts` có dữ liệu không

---

## 📞 Hỗ trợ

Nếu gặp vấn đề, vui lòng liên hệ:
- Email: [email hỗ trợ của bạn]
- GitHub Issues: [link repository]

---

## 📄 License

[Loại license bạn sử dụng - MIT, GPL, etc.]

---

**Chúc bạn sử dụng hệ thống thành công! ✈️**
