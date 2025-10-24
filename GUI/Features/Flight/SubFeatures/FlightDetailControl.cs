using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Flight.SubFeatures {
    public class FlightDetailControl : UserControl {
        private TableLayoutPanel main;        // root giống FlightListControl
        private Label lblTitle;
        private Panel card;                   // khung trắng
        private TableLayoutPanel grid;        // bảng 2 cột key/value như bản đầu

        // giữ field để set nhanh; đồng thời gán Name để bạn vẫn Controls["..."] được
        private Label valueFlightId, valueDeparturePlace, valueArrivalPlace,
                      valueDepartureTime, valueArrivalTime, valueSeatAvailable;

        public FlightDetailControl() {
            InitializeComponent();
            BuildLayout();
        }

        private void InitializeComponent() {
            SuspendLayout();
            // 
            // FlightDetailControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Name = "FlightDetailControl";
            Size = new Size(1460, 430);
            ResumeLayout(false);
        }

        private static Label Key(string text) => new Label {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            Margin = new Padding(0, 6, 12, 6)
        };

        private static Label Val(string name) => new Label {
            Name = name,
            Text = "",
            AutoSize = true,
            Font = new Font("Segoe UI", 10f, FontStyle.Regular),
            Margin = new Padding(0, 6, 0, 6)
        };

        private void BuildLayout() {
            // ===== Title =====
            lblTitle = new Label {
                Text = "🧾 Chi tiết chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Card trắng chứa grid 2 cột =====
            // ===== Card trắng chứa grid 2 cột =====
            card = new Panel {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(16),
                Margin = new Padding(24, 8, 24, 24),
                Dock = DockStyle.Fill
            };

            // Hàng tiêu đề nhỏ trong card
            var secTitle = new Label {
                Text = "Thông tin chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 16),
                Dock = DockStyle.Top
            };

            // Bảng grid
            grid = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // ✅ Thêm tiêu đề trước, grid sau
            card.Controls.Add(grid);
            card.Controls.Add(secTitle);

            // ===== Các dòng (đúng thứ tự & index, KHÔNG trùng như trước) =====
            // 1
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Mã chuyến bay:"), 0, 0);
            valueFlightId = Val("valueFlightId");
            grid.Controls.Add(valueFlightId, 1, 0);

            // 2
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Nơi cất cánh:"), 0, 1);
            valueDeparturePlace = Val("valueDeparturePlace");
            grid.Controls.Add(valueDeparturePlace, 1, 1);

            // 3
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Nơi hạ cánh:"), 0, 2);
            valueArrivalPlace = Val("valueArrivalPlace");
            grid.Controls.Add(valueArrivalPlace, 1, 2);

            // 4
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Giờ cất cánh:"), 0, 3);
            valueDepartureTime = Val("valueDepartureTime");
            grid.Controls.Add(valueDepartureTime, 1, 3);

            // 5  (⚠️ fix: KHÔNG còn trùng row 4)
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Giờ hạ cánh:"), 0, 4);
            valueArrivalTime = Val("valueArrivalTime");
            grid.Controls.Add(valueArrivalTime, 1, 4);

            // 6
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Số ghế trống:"), 0, 5);
            valueSeatAvailable = Val("valueSeatAvailable");
            grid.Controls.Add(valueSeatAvailable, 1, 5);

            card.Controls.Add(grid);
            grid.BringToFront();

            // ===== Bottom actions (nút Đóng giống list) =====
            var bottom = new FlowLayoutPanel {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(0, 12, 12, 12)
            };
            var btnClose = new Button { Text = "Đóng", AutoSize = true };
            btnClose.Click += (_, __) => FindForm()?.Close();
            bottom.Controls.Add(btnClose);
            card.Controls.Add(bottom);

            // ===== Main =====
            main = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 2
            };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Clear();
            Controls.Add(main);
        }

        // Giữ đúng chữ ký FlightListControl đang gọi
        public void LoadFlightInfo(string flightId, string departurePlace, string arrivalPlace,
                                   string departureTime, string arrivalTime, string seatAvailable) {
            // set qua field…
            valueFlightId.Text = flightId ?? "";
            valueDeparturePlace.Text = departurePlace ?? "";
            valueArrivalPlace.Text = arrivalPlace ?? "";
            valueDepartureTime.Text = departureTime ?? "";
            valueArrivalTime.Text = arrivalTime ?? "";
            valueSeatAvailable.Text = seatAvailable ?? "";

            // …và bạn vẫn có thể truy cập qua Controls["valueXxx"] nếu cần
        }
    }
}
