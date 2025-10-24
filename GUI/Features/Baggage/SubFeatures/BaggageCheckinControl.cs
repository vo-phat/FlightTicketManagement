using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;

namespace GUI.Features.Baggage.SubFeatures {
    public class BaggageCheckinControl : UserControl {
        public class BaggageData {
            public int BaggageId { get; set; }       // sau khi tạo
            public string BaggageTag { get; set; } = "";
            public string Type { get; set; } = "CHECKED";
            public decimal WeightKg { get; set; }
            public decimal AllowedWeightKg { get; set; }
            public decimal Fee { get; set; }
            public string Status { get; set; } = "CHECKED_IN";
            public int FlightId { get; set; }
            public int TicketId { get; set; }
        }

        public event Action<BaggageData>? OnCreated;

        private UnderlinedTextField txtTicketNumber, txtFlightId, txtBaggageTag, txtType, txtWeight, txtAllowed, txtSpecial, txtFee;
        private Label lblTitle;

        public BaggageCheckinControl() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // Header (nút lưu bên phải tiêu đề)
            var header = new Panel { Dock = DockStyle.Top, Height = 64, Padding = new Padding(24, 16, 24, 0) };
            lblTitle = new Label { Text = "🧳 Gán tag / Check-in hành lý", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold), Dock = DockStyle.Left };
            var btnCreate = new PrimaryButton("💾 Lưu & In tag") { Width = 160, Height = 40, Dock = DockStyle.Right };
            btnCreate.Click += (s, e) => DoCreate();
            header.Controls.Add(btnCreate);
            header.Controls.Add(lblTitle);

            var grid = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 8, 24, 0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 5
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            txtTicketNumber = new UnderlinedTextField("Số vé (Tickets.ticket_number)", "");
            txtFlightId = new UnderlinedTextField("Mã chuyến bay (Flights.flight_id)", "");
            txtBaggageTag = new UnderlinedTextField("Mã nhãn (Baggage.baggage_tag)", "");
            txtType = new UnderlinedTextField("Loại (CHECKED/CARRY_ON/SPECIAL)", "CHECKED");
            txtWeight = new UnderlinedTextField("Cân nặng thực tế (kg)", "");
            txtAllowed = new UnderlinedTextField("Định mức miễn cước (kg)", "");
            txtSpecial = new UnderlinedTextField("Xử lý đặc biệt (optional)", "");
            txtFee = new UnderlinedTextField("Phí phát sinh (VND)", "0");

            txtTicketNumber.Width = txtFlightId.Width = txtBaggageTag.Width = 280;
            txtType.Width = txtWeight.Width = txtAllowed.Width = txtSpecial.Width = txtFee.Width = 280;

            grid.Controls.Add(txtTicketNumber, 0, 0);
            grid.Controls.Add(txtFlightId, 1, 0);

            grid.Controls.Add(txtBaggageTag, 0, 1);
            grid.Controls.Add(txtType, 1, 1);

            grid.Controls.Add(txtWeight, 0, 2);
            grid.Controls.Add(txtAllowed, 1, 2);

            grid.Controls.Add(txtSpecial, 0, 3);
            grid.Controls.Add(txtFee, 1, 3);

            var main = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(header, 0, 0);
            main.Controls.Add(grid, 0, 1);
            main.Controls.Add(new Panel(), 0, 2);

            Controls.Add(main);
        }

        private void DoCreate() {
            // TODO:
            // 1) Từ ticket_number -> Tickets.ticket_id & Flights.flight_id (JOIN theo schema dự án)
            // 2) Tính fee = max(0, weight - allowed) * policy
            var data = new BaggageData {
                BaggageId = new Random().Next(1000, 9999),
                BaggageTag = txtBaggageTag.Text,
                Type = txtType.Text,
                WeightKg = decimal.TryParse(txtWeight.Text, out var w) ? w : 0,
                AllowedWeightKg = decimal.TryParse(txtAllowed.Text, out var aw) ? aw : 0,
                Fee = decimal.TryParse(txtFee.Text, out var f) ? f : 0,
                Status = "CHECKED_IN",
                FlightId = int.TryParse(txtFlightId.Text, out var fid) ? fid : 0,
                TicketId = 0 // TODO: map từ ticket_number
            };
            MessageBox.Show("Đã check-in hành lý " + data.BaggageTag, "Baggage");
            OnCreated?.Invoke(data);
        }
    }
}
