// TRONG FILE: GUI/Features/Flight/SubFeatures/FlightDetailControl.cs
// THAY THẾ TOÀN BỘ NỘI DUNG FILE BẰNG MÃ NÀY:

using BUS.Flight;
using DTO.Flight;
using GUI.Components.Buttons;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Flight.SubFeatures
{
    public partial class FlightDetailControl : UserControl
    {
        public event Action OnBackToListRequested;

        private Label lblTitle;
        private Label vFlightNumber, vRoute, vAircraft, vDeparture, vArrival, vStatus, vSeats;
        private Button btnBack;

        public FlightDetailControl()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.BackColor = Color.FromArgb(232, 240, 252);
            this.Dock = DockStyle.Fill;

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Title
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Back Button
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Content
            this.Controls.Add(mainLayout);

            lblTitle = new Label
            {
                Text = "✈️ Chi tiết chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };
            mainLayout.Controls.Add(lblTitle, 0, 0);

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 12, 24, 0)
            };
            btnBack = new SecondaryButton("⬅️ Quay lại danh sách");
            btnBack.Click += (s, e) => OnBackToListRequested?.Invoke();
            buttonPanel.Controls.Add(btnBack);
            mainLayout.Controls.Add(buttonPanel, 0, 1);

            var card = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(16),
                Margin = new Padding(24, 8, 24, 24),
                Dock = DockStyle.Fill
            };
            mainLayout.Controls.Add(card, 0, 2);

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            card.Controls.Add(grid);

            Label Key(string t) => new Label { Text = t, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Margin = new Padding(0, 6, 12, 6) };
            Label Val() => new Label { AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Regular), Margin = new Padding(0, 6, 0, 6) };

            int r = 0;
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Số hiệu:"), 0, r); vFlightNumber = Val(); grid.Controls.Add(vFlightNumber, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Tuyến bay:"), 0, r); vRoute = Val(); grid.Controls.Add(vRoute, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Máy bay:"), 0, r); vAircraft = Val(); grid.Controls.Add(vAircraft, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Khởi hành:"), 0, r); vDeparture = Val(); grid.Controls.Add(vDeparture, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hạ cánh:"), 0, r); vArrival = Val(); grid.Controls.Add(vArrival, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Trạng thái:"), 0, r); vStatus = Val(); grid.Controls.Add(vStatus, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Ghế trống:"), 0, r); vSeats = Val(); grid.Controls.Add(vSeats, 1, r++);
        }

        public void LoadFlightDetails(int flightId)
        {
            // Reset dữ liệu cũ
            vFlightNumber.Text = vRoute.Text = vAircraft.Text = vDeparture.Text = vArrival.Text = vStatus.Text = vSeats.Text = "(Đang tải...)";

            // Gọi BUS để lấy FlightDTO
            var result = FlightBUS.Instance.GetFlightById(flightId);

            if (result.Success)
            {
                var flight = result.GetData<FlightDTO>();
                vFlightNumber.Text = flight.FlightNumber;
                vRoute.Text = $"(Route ID: {flight.RouteId})";
                vAircraft.Text = $"(Aircraft ID: {flight.AircraftId})";
                vDeparture.Text = flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm");
                vArrival.Text = flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm");
                vStatus.Text = flight.Status.GetDescription();

                vSeats.Text = "(Chưa có dữ liệu)";
            }
            else
            {
                MessageBox.Show(result.GetFullErrorMessage(), "Lỗi tải chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Quay lại danh sách nếu lỗi
                OnBackToListRequested?.Invoke();
            }
        }
    }
}