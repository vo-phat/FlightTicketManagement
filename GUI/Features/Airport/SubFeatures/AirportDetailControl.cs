using System.Drawing;
using System.Windows.Forms;

namespace FlightTicketManagement.GUI.Features.Airport.SubFeatures {
    public class AirportDetailControl : UserControl {
        private Label vCode, vName, vCity, vCountry, vTz;

        public AirportDetailControl() { InitializeComponent(); BuildLayout(); }

        private void InitializeComponent() { Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252); }

        private static Label Key(string t) => new Label { Text = t, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Margin = new Padding(0, 6, 12, 6) };
        private static Label Val(string n) => new Label { Name = n, AutoSize = true, Font = new Font("Segoe UI", 10f), Margin = new Padding(0, 6, 0, 6) };

        private void BuildLayout() {
            var title = new Label { Text = "🛫 Chi tiết sân bay", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold), Padding = new Padding(24, 20, 24, 0), Dock = DockStyle.Top };
            var card = new Panel { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(16), Margin = new Padding(24, 8, 24, 24), Dock = DockStyle.Fill };

            var grid = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int r = 0;
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Mã IATA:"), 0, r); vCode = Val("vCode"); grid.Controls.Add(vCode, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Tên sân bay:"), 0, r); vName = Val("vName"); grid.Controls.Add(vName, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Thành phố:"), 0, r); vCity = Val("vCity"); grid.Controls.Add(vCity, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Quốc gia:"), 0, r); vCountry = Val("vCountry"); grid.Controls.Add(vCountry, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Múi giờ:"), 0, r); vTz = Val("vTz"); grid.Controls.Add(vTz, 1, r++);

            card.Controls.Add(grid);
            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 12, 12, 12) };
            var btnClose = new Button { Text = "Đóng", AutoSize = true }; btnClose.Click += (_, __) => FindForm()?.Close();
            bottom.Controls.Add(btnClose); card.Controls.Add(bottom);

            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(title, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Add(main);
        }

        public void LoadAirport(string code, string name, string city, string country, string tz) {
            vCode.Text = code; vName.Text = name; vCity.Text = city; vCountry.Text = country; vTz.Text = tz;
        }
    }
}
