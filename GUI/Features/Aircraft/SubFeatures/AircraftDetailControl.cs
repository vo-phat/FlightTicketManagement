using System.Drawing;
using System.Windows.Forms;

namespace FlightTicketManagement.GUI.Features.Aircraft.SubFeatures {
    public class AircraftDetailControl : UserControl {
        private Label vAirline, vModel, vManu, vCap, vSeats;

        public AircraftDetailControl() { InitializeComponent(); BuildLayout(); }

        private void InitializeComponent() {
            SuspendLayout();
            // 
            // AircraftDetailControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Name = "AircraftDetailControl";
            Size = new Size(1460, 493);
            ResumeLayout(false);
        }

        private static Label Key(string t) => new Label { Text = t, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Margin = new Padding(0, 6, 12, 6) };
        private static Label Val(string n) => new Label { Name = n, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Regular), Margin = new Padding(0, 6, 0, 6) };

        private void BuildLayout() {
            var title = new Label { Text = "🛩 Chi tiết máy bay", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold), Padding = new Padding(24, 20, 24, 0), Dock = DockStyle.Top };
            var card = new Panel { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(16), Margin = new Padding(24, 8, 24, 24), Dock = DockStyle.Fill };

            var sec = new Label { Text = "Thông tin cơ bản", AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Margin = new Padding(0, 0, 0, 16) };
            var grid = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int r = 0;
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hãng:"), 0, r); vAirline = Val("vAirline"); grid.Controls.Add(vAirline, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Model:"), 0, r); vModel = Val("vModel"); grid.Controls.Add(vModel, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hãng sản xuất:"), 0, r); vManu = Val("vManu"); grid.Controls.Add(vManu, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Sức chứa (ghế):"), 0, r); vCap = Val("vCap"); grid.Controls.Add(vCap, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Số ghế cấu hình:"), 0, r); vSeats = Val("vSeats"); grid.Controls.Add(vSeats, 1, r++);

            card.Controls.Add(grid);
            card.Controls.Add(sec);

            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 12, 12, 12) };
            var btnClose = new Button { Text = "Đóng", AutoSize = true }; btnClose.Click += (_, __) => FindForm()?.Close(); bottom.Controls.Add(btnClose);
            card.Controls.Add(bottom);

            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(title, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Add(main);
        }

        public void LoadAircraftInfo(string airline, string model, string manufacturer, string capacity, string seats) {
            vAirline.Text = airline ?? "";
            vModel.Text = model ?? "";
            vManu.Text = manufacturer ?? "";
            vCap.Text = capacity ?? "0";
            vSeats.Text = seats ?? "0";
        }
    }
}
