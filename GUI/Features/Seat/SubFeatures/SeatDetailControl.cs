using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Tables;

namespace GUI.Features.Seat.SubFeatures {
    /// <summary>
    /// Chi tiết ghế (tĩnh - per Aircraft).
    /// Không hiển thị trạng thái AVAILABLE/BOOKED/BLOCKED hay base_price (đó là dữ liệu Flight_Seats).
    /// </summary>
    public class SeatDetailControl : UserControl {
        private Label lblSeat, lblCabin, lblAircraft, lblRef;
        private TableLayoutPanel root;
        private PrimaryButton btnEdit, btnDelete;

        public SeatDetailControl() {
            InitializeComponent();
        }

        public void LoadSeat(SeatStaticVM vm) {
            lblSeat.Text = $"Số ghế: {vm.SeatNumber}";
            lblCabin.Text = $"Hạng ghế: {vm.CabinName}";
            lblAircraft.Text = $"Máy bay: {vm.AircraftInfo}";
            lblRef.Text = $"Được tham chiếu ở {vm.FlightsReferenced} chuyến; gần nhất: {vm.LastSeenAt:yyyy-MM-dd}";
        }

        private void InitializeComponent() {
            Dock = DockStyle.Fill; BackColor = Color.White;

            var title = new Label {
                Text = "🔎 Chi tiết ghế (per Aircraft)",
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 8),
                Dock = DockStyle.Top
            };

            lblSeat = new Label { Text = "Số ghế: -", AutoSize = true, Font = new Font("Segoe UI", 12), Padding = new Padding(24, 6, 24, 6) };
            lblCabin = new Label { Text = "Hạng ghế: -", AutoSize = true, Font = new Font("Segoe UI", 12), Padding = new Padding(24, 6, 24, 6) };
            lblAircraft = new Label { Text = "Máy bay: -", AutoSize = true, Font = new Font("Segoe UI", 12), Padding = new Padding(24, 6, 24, 6) };
            lblRef = new Label { Text = "Tham chiếu: -", AutoSize = true, Font = new Font("Segoe UI", 11), ForeColor = Color.DimGray, Padding = new Padding(24, 12, 24, 6) };

            btnEdit = new PrimaryButton("✎ Sửa") { Width = 100, Height = 36, Margin = new Padding(0, 0, 12, 0) };
            btnDelete = new PrimaryButton("🗑 Xóa") { Width = 100, Height = 36 };
            var actionRow = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(24, 8, 24, 8) };
            actionRow.Controls.Add(btnEdit);
            actionRow.Controls.Add(btnDelete);

            root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6, BackColor = Color.White };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.Controls.Add(title, 0, 0);
            root.Controls.Add(lblSeat, 0, 1);
            root.Controls.Add(lblCabin, 0, 2);
            root.Controls.Add(lblAircraft, 0, 3);
            root.Controls.Add(lblRef, 0, 4);
            root.Controls.Add(actionRow, 0, 5);

            Controls.Add(root);

            // Demo behaviors
            btnEdit.Click += (_, __) => MessageBox.Show("[DEMO] Sửa ghế (đổi seat_number/cabin).");
            btnDelete.Click += (_, __) => MessageBox.Show("[DEMO] Xóa ghế (sẽ bị chặn nếu tham chiếu trong Flight_Seats).");
        }

        public class SeatStaticVM {
            public string SeatId { get; set; } = "";
            public string SeatNumber { get; set; } = "";
            public string CabinName { get; set; } = "";
            public string AircraftInfo { get; set; } = "";
            public int FlightsReferenced { get; set; }
            public DateTime LastSeenAt { get; set; }
        }
    }
}
