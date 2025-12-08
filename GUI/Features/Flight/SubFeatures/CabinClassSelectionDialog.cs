using BUS.Auth;
using DTO.Booking;
using DTO.Flight;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Flight.SubFeatures
{
    public class CabinClassSelectionDialog : Form
    {
        private FlightWithDetailsDTO _flight;
        private string _selectedCabinClass;
        private int _selectedCabinClassId;

        // Property để lấy thông tin đặt vé sau khi dialog đóng
        public BookingRequestDTO BookingRequest { get; private set; }

        public CabinClassSelectionDialog(FlightWithDetailsDTO flight)
        {
            _flight = flight;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Chọn hạng vé";
            Size = new Size(600, 500);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;

            // Title
            var lblTitle = new Label
            {
                Text = "🎫 CHỌN HẠNG VÉ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            Controls.Add(lblTitle);

            // Flight info panel
            var flightInfoPanel = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(540, 80),
                BackColor = Color.FromArgb(232, 240, 252),
                Padding = new Padding(12)
            };

            var lblFlightNumber = new Label
            {
                Text = $"Chuyến bay: {_flight.FlightNumber}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = true,
                Location = new Point(12, 12)
            };
            flightInfoPanel.Controls.Add(lblFlightNumber);

            var lblRoute = new Label
            {
                Text = $"Từ: {_flight.DepartureAirportCode} - {_flight.DepartureCity} → Đến: {_flight.ArrivalAirportCode} - {_flight.ArrivalCity}",
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(12, 38)
            };
            flightInfoPanel.Controls.Add(lblRoute);

            var lblTime = new Label
            {
                Text = $"Khởi hành: {_flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm")}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(100, 100, 100),
                AutoSize = true,
                Location = new Point(12, 60)
            };
            flightInfoPanel.Controls.Add(lblTime);

            Controls.Add(flightInfoPanel);

            // Cabin classes
            var lblSelectClass = new Label
            {
                Text = "Chọn hạng vé:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 155)
            };
            Controls.Add(lblSelectClass);

            // Cabin class options with IDs
            CreateCabinClassOption("First Class", "Hạng Nhất", "✈️", Color.FromArgb(255, 215, 0), 20, 185, 1);
            CreateCabinClassOption("Business", "Hạng Thương Gia", "💼", Color.FromArgb(100, 149, 237), 20, 245, 2);
            CreateCabinClassOption("Premium Economy", "Hạng Phổ Thông Đặc Biệt", "🎫", Color.FromArgb(60, 179, 113), 20, 305, 3);
            CreateCabinClassOption("Economy", "Hạng Phổ Thông", "🪑", Color.FromArgb(169, 169, 169), 20, 365, 4);

            // Ticket Quantity Selector
            var lblQuantity = new Label
            {
                Text = "Số lượng vé:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 427)
            };
            Controls.Add(lblQuantity);

            var numQuantity = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 10,
                Value = 1,
                Width = 60,
                Font = new Font("Segoe UI", 10),
                Location = new Point(130, 425)
            };
            Controls.Add(numQuantity);

            // Buttons
            var btnConfirm = new Button
            {
                Text = "Xác nhận",
                Size = new Size(120, 40),
                Location = new Point(340, 420),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(_selectedCabinClass))
                {
                    MessageBox.Show("Vui lòng chọn hạng vé.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo thông tin đặt vé
                BookingRequest = new BookingRequestDTO
                {
                    AccountId = UserSession.CurrentAccountId,
                    FlightId = _flight.FlightId,
                    CabinClassId = _selectedCabinClassId,
                    CabinClassName = _selectedCabinClass,
                    TicketQuantity = (int)numQuantity.Value, // Thêm số lượng vé
                    BookingDate = DateTime.Now,
                    FlightNumber = _flight.FlightNumber,
                    DepartureAirportCode = _flight.DepartureAirportCode,
                    ArrivalAirportCode = _flight.ArrivalAirportCode,
                    DepartureTime = _flight.DepartureTime
                };

                DialogResult = DialogResult.OK;
                Close();
            };
            Controls.Add(btnConfirm);

            var btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(100, 40),
                Location = new Point(470, 420),
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            Controls.Add(btnCancel);
        }

        private void CreateCabinClassOption(string className, string displayName, string icon, Color color, int x, int y, int cabinClassId)
        {
            var panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(540, 50),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = (className, cabinClassId)
            };

            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 18),
                AutoSize = true,
                Location = new Point(12, 10)
            };
            panel.Controls.Add(lblIcon);

            var lblName = new Label
            {
                Text = displayName,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = color,
                AutoSize = true,
                Location = new Point(60, 15)
            };
            panel.Controls.Add(lblName);

            // Click event for the entire panel
            EventHandler clickHandler = (s, e) =>
            {
                _selectedCabinClass = className;
                _selectedCabinClassId = cabinClassId;
                
                // Highlight selected panel
                foreach (Control ctrl in Controls)
                {
                    if (ctrl is Panel p && p.Tag is ValueTuple<string, int>)
                    {
                        p.BackColor = Color.White;
                        p.BorderStyle = BorderStyle.FixedSingle;
                    }
                }
                
                panel.BackColor = Color.FromArgb(232, 240, 252);
                panel.BorderStyle = BorderStyle.FixedSingle;
            };

            panel.Click += clickHandler;
            lblIcon.Click += clickHandler;
            lblName.Click += clickHandler;

            Controls.Add(panel);
        }
    }
}
