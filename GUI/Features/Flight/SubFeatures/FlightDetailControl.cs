using BUS.Auth;
using DTO.Auth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DTO.Flight;

namespace GUI.Features.Flight.SubFeatures
{
    public class FlightDetailControl : UserControl
    {
        public event EventHandler? CloseRequested;
        public event Action<DTO.Booking.BookingRequestDTO>? NavigateToBookingRequested;
        private FlightWithDetailsDTO _currentFlight = null!;
        private Button btnBookFlight = null!;
        private List<DTO.Booking.BookingRequestDTO> _confirmedBookings = new List<DTO.Booking.BookingRequestDTO>();
        private Label vFlightNumber = null!;
        private Label vAircraftModel = null!;
        private Label vAircraftManufacturer = null!;
        private Label vDepartureAirport = null!;
        private Label vArrivalAirport = null!;
        private Label vDepartureTime = null!;
        private Label vArrivalTime = null!;
        private Label vDuration = null!;
        private Label vStatus = null!;
        private Label vAvailableSeats = null!;
        private Label vRouteInfo = null!;
        private Label vNote = null!;

        public FlightDetailControl()
        {
            InitializeComponent();
            BuildLayout(); // Initialize all controls
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // FlightDetailControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Name = "FlightDetailControl";

            Size = new Size(2110, 1064);
            ResumeLayout(false);
        }

        private static Label CreateKeyLabel(string text) => new Label
        {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            ForeColor = Color.FromArgb(60, 60, 60),
            Margin = new Padding(0, 8, 12, 8)
        };

        private static Label CreateValueLabel(string name) => new Label
        {
            Name = name,
            Text = "-",
            AutoSize = true,
            Font = new Font("Segoe UI", 11f),
            ForeColor = Color.FromArgb(40, 40, 40),
            Margin = new Padding(0, 8, 0, 8)
        };

        private void BuildLayout()
        {
            // Title
            var title = new Label
            {
                Text = "✈️ Chi tiết chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 55, 77),
                Padding = new Padding(24, 20, 24, 12),
                Dock = DockStyle.Top
            };

            // Main card
            var card = new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(24),
                Margin = new Padding(24, 8, 24, 24),
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Info grid
            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                Padding = new Padding(0, 0, 0, 16)
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int row = 0;

            // Flight Information Section
            var sectionFlight = new Label
            {
                Text = "📋 THÔNG TIN CHUYẾN BAY",
                AutoSize = true,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                Margin = new Padding(0, 0, 0, 12),
                Dock = DockStyle.Top
            };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(sectionFlight, 0, row);
            grid.SetColumnSpan(sectionFlight, 2);
            row++;

            // Flight Number
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Số hiệu chuyến bay:"), 0, row);
            vFlightNumber = CreateValueLabel("vFlightNumber");
            vFlightNumber.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            vFlightNumber.ForeColor = Color.FromArgb(0, 92, 175);
            grid.Controls.Add(vFlightNumber, 1, row++);

            // Status
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Trạng thái:"), 0, row);
            vStatus = CreateValueLabel("vStatus");
            grid.Controls.Add(vStatus, 1, row++);

            // Route Section
            var separator1 = new Panel { Height = 2, BackColor = Color.FromArgb(220, 220, 220), Margin = new Padding(0, 16, 0, 16), Dock = DockStyle.Top };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(separator1, 0, row);
            grid.SetColumnSpan(separator1, 2);
            row++;

            var sectionRoute = new Label
            {
                Text = "🌍 TUYẾN BAY",
                AutoSize = true,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                Margin = new Padding(0, 0, 0, 12),
                Dock = DockStyle.Top
            };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(sectionRoute, 0, row);
            grid.SetColumnSpan(sectionRoute, 2);
            row++;

            // Departure Airport
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Sân bay đi:"), 0, row);
            vDepartureAirport = CreateValueLabel("vDepartureAirport");
            vDepartureAirport.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            grid.Controls.Add(vDepartureAirport, 1, row++);

            // Arrival Airport
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Sân bay đến:"), 0, row);
            vArrivalAirport = CreateValueLabel("vArrivalAirport");
            vArrivalAirport.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            grid.Controls.Add(vArrivalAirport, 1, row++);

            // Route Info
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Mã tuyến:"), 0, row);
            vRouteInfo = CreateValueLabel("vRouteInfo");
            grid.Controls.Add(vRouteInfo, 1, row++);

            // Schedule Section
            var separator2 = new Panel { Height = 2, BackColor = Color.FromArgb(220, 220, 220), Margin = new Padding(0, 16, 0, 16), Dock = DockStyle.Top };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(separator2, 0, row);
            grid.SetColumnSpan(separator2, 2);
            row++;

            var sectionSchedule = new Label
            {
                Text = "🕐 LỊCH TRÌNH",
                AutoSize = true,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                Margin = new Padding(0, 0, 0, 12),
                Dock = DockStyle.Top
            };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(sectionSchedule, 0, row);
            grid.SetColumnSpan(sectionSchedule, 2);
            row++;

            // Departure Time
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Giờ khởi hành:"), 0, row);
            vDepartureTime = CreateValueLabel("vDepartureTime");
            vDepartureTime.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            vDepartureTime.ForeColor = Color.FromArgb(46, 125, 50);
            grid.Controls.Add(vDepartureTime, 1, row++);

            // Arrival Time
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Giờ đến dự kiến:"), 0, row);
            vArrivalTime = CreateValueLabel("vArrivalTime");
            vArrivalTime.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            vArrivalTime.ForeColor = Color.FromArgb(211, 47, 47);
            grid.Controls.Add(vArrivalTime, 1, row++);

            // Duration
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Thời gian bay:"), 0, row);
            vDuration = CreateValueLabel("vDuration");
            grid.Controls.Add(vDuration, 1, row++);

            // Aircraft Section
            var separator3 = new Panel { Height = 2, BackColor = Color.FromArgb(220, 220, 220), Margin = new Padding(0, 16, 0, 16), Dock = DockStyle.Top };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(separator3, 0, row);
            grid.SetColumnSpan(separator3, 2);
            row++;

            var sectionAircraft = new Label
            {
                Text = "✈️ THÔNG TIN MÁY BAY",
                AutoSize = true,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                Margin = new Padding(0, 0, 0, 12),
                Dock = DockStyle.Top
            };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(sectionAircraft, 0, row);
            grid.SetColumnSpan(sectionAircraft, 2);
            row++;

            // Aircraft Model
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Model máy bay:"), 0, row);
            vAircraftModel = CreateValueLabel("vAircraftModel");
            grid.Controls.Add(vAircraftModel, 1, row++);

            // Aircraft Manufacturer
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Nhà sản xuất:"), 0, row);
            vAircraftManufacturer = CreateValueLabel("vAircraftManufacturer");
            grid.Controls.Add(vAircraftManufacturer, 1, row++);

            // Available Seats
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Số ghế còn trống:"), 0, row);
            vAvailableSeats = CreateValueLabel("vAvailableSeats");
            vAvailableSeats.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            vAvailableSeats.ForeColor = Color.FromArgb(46, 125, 50);
            grid.Controls.Add(vAvailableSeats, 1, row++);

            // Notes Section
            var separator4 = new Panel { Height = 2, BackColor = Color.FromArgb(220, 220, 220), Margin = new Padding(0, 16, 0, 16), Dock = DockStyle.Top };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(separator4, 0, row);
            grid.SetColumnSpan(separator4, 2);
            row++;

            var sectionNotes = new Label
            {
                Text = "📝 GHI CHÚ",
                AutoSize = true,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                Margin = new Padding(0, 0, 0, 12),
                Dock = DockStyle.Top
            };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(sectionNotes, 0, row);
            grid.SetColumnSpan(sectionNotes, 2);
            row++;

            // Note
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Ghi chú:"), 0, row);
            vNote = CreateValueLabel("vNote");
            vNote.MaximumSize = new Size(500, 0);
            vNote.AutoSize = true;
            grid.Controls.Add(vNote, 1, row++);

            card.Controls.Add(grid);

            // Bottom buttons
            var bottom = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(0, 12, 0, 0)
            };

            var btnClose = new Button
            {
                Text = "← Quay lại",
                AutoSize = true,
                Font = new Font("Segoe UI", 10f),
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Padding = new Padding(16, 8, 16, 8),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (_, __) => CloseRequested?.Invoke(this, EventArgs.Empty);
            bottom.Controls.Add(btnClose);

            // Nút "Đặt vé" - chỉ hiển thị cho User và Staff
            btnBookFlight = new Button
            {
                Text = "🎫 Đặt vé",
                AutoSize = true,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Padding = new Padding(16, 8, 16, 8),
                Cursor = Cursors.Hand,
                Margin = new Padding(8, 0, 0, 0)
            };
            btnBookFlight.FlatAppearance.BorderSize = 0;
            btnBookFlight.Click += BtnBookFlight_Click;
            
            // Thêm nút vào bottom
            bottom.Controls.Add(btnBookFlight);
            
            // Chỉ User và Staff mới thấy nút Đặt vé (Admin không thấy)
            btnBookFlight.Visible = (UserSession.CurrentAppRole == AppRole.User || UserSession.CurrentAppRole == AppRole.Staff);

            card.Controls.Add(bottom);

            // Main layout
            var main = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(title, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Add(main);
        }

        public void LoadFlight(FlightWithDetailsDTO flight)
        {
            if (flight == null) return;

            _currentFlight = flight;

            // Flight info
            vFlightNumber.Text = flight.FlightNumber ?? "-";
            vStatus.Text = GetStatusText(flight.Status);
            UpdateStatusColor(flight.Status);

            // Route info
            vDepartureAirport.Text = flight.DepartureAirportDisplay ?? "-";
            vArrivalAirport.Text = flight.ArrivalAirportDisplay ?? "-";
            vRouteInfo.Text = $"Route ID: {flight.RouteId}";

            // Schedule
            vDepartureTime.Text = flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm") ?? "-";
            vArrivalTime.Text = flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm") ?? "-";
            
            var duration = flight.GetFlightDuration();
            if (duration.HasValue)
            {
                vDuration.Text = $"{duration.Value.Hours} giờ {duration.Value.Minutes} phút";
            }
            else
            {
                vDuration.Text = "-";
            }

            // Aircraft info
            vAircraftModel.Text = !string.IsNullOrEmpty(flight.AircraftModel) ? flight.AircraftModel : "-";
            vAircraftManufacturer.Text = !string.IsNullOrEmpty(flight.AircraftManufacturer) ? flight.AircraftManufacturer : "-";
            vAvailableSeats.Text = flight.AvailableSeats.ToString();

            // Note
            vNote.Text = !string.IsNullOrWhiteSpace(flight.Note) ? flight.Note : "(Không có ghi chú)";
            
            // Cập nhật trạng thái nút "Đặt vé"
            UpdateBookButtonState();
        }

        private void UpdateBookButtonState()
        {
            if (btnBookFlight == null || _currentFlight == null) return;

            // Chỉ cho đặt vé với chuyến bay "Đã lên lịch"
            bool canBook = _currentFlight.Status == FlightStatus.SCHEDULED && _currentFlight.AvailableSeats > 0;
            
            btnBookFlight.Enabled = canBook;
            btnBookFlight.BackColor = canBook 
                ? Color.FromArgb(46, 125, 50) 
                : Color.FromArgb(189, 189, 189);
            
            if (!canBook)
            {
                if (_currentFlight.Status != FlightStatus.SCHEDULED)
                {
                    btnBookFlight.Text = "🚫 Không thể đặt";
                }
                else if (_currentFlight.AvailableSeats <= 0)
                {
                    btnBookFlight.Text = "😔 Hết chỗ";
                }
            }
            else
            {
                btnBookFlight.Text = "🎫 Đặt vé";
            }
        }

        private void BtnBookFlight_Click(object? sender, EventArgs e)
        {
            if (_currentFlight == null) return;
            
            // Kiểm tra lại điều kiện
            if (_currentFlight.Status != FlightStatus.SCHEDULED)
            {
                MessageBox.Show("Chỉ có thể đặt vé cho chuyến bay đã lên lịch.", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_currentFlight.AvailableSeats <= 0)
            {
                MessageBox.Show("Chuyến bay này đã hết chỗ.", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị dialog chọn hạng vé
            using (var dialog = new CabinClassSelectionDialog(_currentFlight))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Chuyển sang trang Thông tin khách hàng với thông tin đặt vé
                    if (dialog.BookingRequest != null)
                    {
                        // Lưu thông tin booking đã xác nhận
                        _confirmedBookings.Add(dialog.BookingRequest);
                        
                        NavigateToBookingRequested?.Invoke(dialog.BookingRequest);
                    }
                }
            }
        }

        private string GetStatusText(FlightStatus status)
        {
            return status switch
            {
                FlightStatus.SCHEDULED => "Đã lên lịch",
                FlightStatus.COMPLETED => "Hoàn thành",
                FlightStatus.CANCELLED => "Đã hủy",
                FlightStatus.DELAYED => "Trì hoãn",
                _ => "Không xác định"
            };
        }

        private void UpdateStatusColor(FlightStatus status)
        {
            switch (status)
            {
                case FlightStatus.SCHEDULED:
                    vStatus.ForeColor = Color.FromArgb(25, 118, 210);
                    vStatus.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
                    break;
                case FlightStatus.COMPLETED:
                    vStatus.ForeColor = Color.FromArgb(76, 175, 80);
                    vStatus.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
                    break;
                case FlightStatus.CANCELLED:
                    vStatus.ForeColor = Color.FromArgb(211, 47, 47);
                    vStatus.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
                    break;
                case FlightStatus.DELAYED:
                    vStatus.ForeColor = Color.FromArgb(255, 152, 0);
                    vStatus.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
                    break;
            };
        }

        /// <summary>
        /// Lấy danh sách các booking đã xác nhận
        /// </summary>
        public List<DTO.Booking.BookingRequestDTO> ConfirmedBooking()
        {
            return new List<DTO.Booking.BookingRequestDTO>(_confirmedBookings);
        }

        /// <summary>
        /// Xóa danh sách booking đã xác nhận
        /// </summary>
        public void ClearConfirmedBookings()
        {
            _confirmedBookings.Clear();
        }
    }
}
