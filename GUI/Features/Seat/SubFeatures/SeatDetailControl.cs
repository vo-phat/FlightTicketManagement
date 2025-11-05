using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BUS.Seat;
using DTO.Seat;

namespace GUI.Features.Seat.SubFeatures
{
    public class SeatDetailControl : UserControl
    {
        // Các Controls để hiển thị giá trị tĩnh
        private Label vSeatNumber, vAircraft, vClass;

        // Logic
        private SeatDTO _currentSeat;
        private readonly SeatBUS _seatBUS;

        // Sự kiện báo đóng (Tùy chọn, giữ lại nếu muốn có nút Đóng)
        public event EventHandler CloseRequested;

        public SeatDetailControl()
        {
            _seatBUS = new SeatBUS();
            InitializeComponent();
            BuildLayout();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // Khởi tạo cơ bản
            BackColor = Color.FromArgb(232, 240, 252);
            Name = "SeatDetailControl";
            Size = new Size(1074, 527);
            ResumeLayout(false);

        }

        // Helper cho Key (Giữ nguyên từ RouteDetailControl)
        private static Label Key(string t) => new Label
        {
            Text = t,
            AutoSize = true,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            Margin = new Padding(0, 6, 12, 6)
        };

        // Helper cho Value (Giữ nguyên từ RouteDetailControl)
        private static Label Val(string n) => new Label
        {
            Name = n,
            AutoSize = true,
            Font = new Font("Segoe UI", 10f),
            Margin = new Padding(0, 6, 0, 6)
        };

        private void BuildLayout()
        {
            // Tiêu đề
            var title = new Label { Text = "ℹ️ Chi tiết ghế", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold), Padding = new Padding(24, 20, 24, 0), Dock = DockStyle.Top };

            // Card chứa nội dung chi tiết
            var card = new Panel { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(16), Margin = new Padding(24, 8, 24, 24), Dock = DockStyle.Fill };

            // Grid Layout cho Form (2 cột)
            var grid = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(8) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Khởi tạo các Label Value
            vSeatNumber = Val("vSeatNumber");
            vAircraft = Val("vAircraft");
            vClass = Val("vClass");

            int r = 0;

            // Dòng 1: Số ghế
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Số ghế:"), 0, r);
            grid.Controls.Add(vSeatNumber, 1, r++);

            // Dòng 2: Máy bay
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Máy bay (Model/Hãng):"), 0, r);
            grid.Controls.Add(vAircraft, 1, r++);

            // Dòng 3: Hạng ghế
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Hạng ghế:"), 0, r);
            grid.Controls.Add(vClass, 1, r++);

            card.Controls.Add(grid);

            // Nút Đóng (Tùy chọn)
            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 12, 12, 12) };
            var btnClose = new Button { Text = "Đóng", AutoSize = true };
            btnClose.Click += (_, __) => CloseRequested?.Invoke(this, EventArgs.Empty);
            bottom.Controls.Add(btnClose);
            card.Controls.Add(bottom);
            // Trong SeatDetailControl.cs
            
            btnClose.Click += (_, __) => CloseRequested?.Invoke(this, EventArgs.Empty);
            // Main Layout
            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(title, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Add(main);
        }

        /// <summary>
        /// Nạp dữ liệu ghế vào form chi tiết.
        /// </summary>
        /// <param name="seatId">ID của ghế cần nạp.</param>
        public void LoadSeat(int seatId)
        {
            _currentSeat = null;

            try
            {
                // Lấy dữ liệu chi tiết của ghế
                _currentSeat = _seatBUS.GetAllSeatsWithDetails().FirstOrDefault(s => s.SeatId == seatId);

                if (_currentSeat == null)
                {
                    MessageBox.Show($"Không tìm thấy ghế có ID: {seatId}.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DisplayEmptyData();
                    return;
                }

                DisplayCurrentSeatData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp dữ liệu ghế: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DisplayEmptyData();
            }
        }

        private void DisplayEmptyData()
        {
            vSeatNumber.Text = "N/A";
            vAircraft.Text = "N/A";
            vClass.Text = "N/A";
        }

        private void DisplayCurrentSeatData()
        {
            if (_currentSeat == null) return;

            vSeatNumber.Text = _currentSeat.SeatNumber;
            vAircraft.Text = $"{_currentSeat.AircraftManufacturer} {_currentSeat.AircraftModel}";
            vClass.Text = _currentSeat.ClassName;
        }

  
    }
}