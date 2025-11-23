using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.Seat.SubFeatures; // SeatListControl, SeatCreateControl, FlightSeatControl, SeatMapControl, SeatMapMode

namespace GUI.Features.Seat {
    public class SeatControl : UserControl {
        // Header + dải nút tab
        private Panel header;
        private FlowLayoutPanel tabBar;

        // Chỉ số tab hiện tại để render Primary/Secondary
        private int currentIndex = 0;

        // Nội dung từng tab (5 tab như file gốc dùng TabControl)
        private Control current;
        private SeatListControl seatList;
        private SeatCreateControl seatCreate;
        private SeatMapControl tabMapAircraft;   // Sơ đồ (theo máy bay)
        private FlightSeatControl flightSeats;   // Ghế theo chuyến
        private SeatMapControl tabMapFlight;     // Sơ đồ (theo chuyến)

        public SeatControl() {
            InitializeComponent();
            SwitchTab(0); // Mặc định vào "Danh sách ghế"
        }

        private void InitializeComponent() {
            SuspendLayout();

            // Nền giống file gốc
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // Header trắng chứa dải nút
            header = new Panel {
                Dock = DockStyle.Top,
                BackColor = Color.White,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            tabBar = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(24, 8, 24, 8),
                WrapContents = false,
                BackColor = Color.White
            };

            header.Controls.Add(tabBar);
            Controls.Add(header);

            // Khởi tạo các sub-controls tương ứng 5 tab như bản TabControl
            seatList = new SeatListControl { Dock = DockStyle.Fill, Visible = false };
            seatCreate = new SeatCreateControl { Dock = DockStyle.Fill, Visible = false };
            tabMapAircraft = new SeatMapControl(SeatMapMode.PerAircraft) { Dock = DockStyle.Fill, Visible = false };
            flightSeats = new FlightSeatControl { Dock = DockStyle.Fill, Visible = false };
            tabMapFlight = new SeatMapControl(SeatMapMode.PerFlight) { Dock = DockStyle.Fill, Visible = false };

            // Thêm vào form (thứ tự không quan trọng vì sẽ BringToFront khi switch)
            Controls.Add(seatList);
            Controls.Add(seatCreate);
            Controls.Add(tabMapAircraft);
            Controls.Add(flightSeats);
            Controls.Add(tabMapFlight);

            ResumeLayout(false);
            PerformLayout();

            // Vẽ dải nút lần đầu
            RebuildTabBar();
        }

        /// <summary>
        /// Dựng lại dải nút: tab đang chọn -> PrimaryButton, tab khác -> SecondaryButton.
        /// </summary>
        private void RebuildTabBar() {
            tabBar.SuspendLayout();
            tabBar.Controls.Clear();

            Control MakeTabButton(string text, int index) {
                var btn = (index == currentIndex)
                    ? (Button)new PrimaryButton(text)
                    : new SecondaryButton(text);

                btn.Height = 36;
                btn.Margin = new Padding(0, 0, 12, 0);

                // Nền trắng để đồng bộ header
                btn.BackColor = Color.White;
                if (btn.FlatAppearance != null) {
                    btn.FlatAppearance.MouseOverBackColor = Color.White;
                    btn.FlatAppearance.MouseDownBackColor = Color.White;
                }

                btn.Click += (_, __) => {
                    if (currentIndex != index) SwitchTab(index);
                };
                return btn;
            }

            // 5 tab đúng nhãn như file TabControl
            tabBar.Controls.Add(MakeTabButton("Danh sách ghế", 0));
            tabBar.Controls.Add(MakeTabButton("Tạo ghế", 1));
            tabBar.Controls.Add(MakeTabButton("Sơ đồ (theo máy bay)", 2));
            tabBar.Controls.Add(MakeTabButton("Ghế theo chuyến", 3));
            tabBar.Controls.Add(MakeTabButton("Sơ đồ (theo chuyến)", 4));

            tabBar.ResumeLayout(true);
        }

        private void SwitchTab(int idx) {
            currentIndex = idx;

            // 1) Cập nhật hiển thị nút (Primary/Secondary)
            RebuildTabBar();

            // 2) Ẩn tab cũ nếu có
            if (current != null) current.Visible = false;

            // 3) Chọn control tương ứng 5 tab
            current = idx switch {
                0 => seatList,       // Danh sách ghế
                1 => seatCreate,     // Tạo ghế
                2 => tabMapAircraft, // Sơ đồ (theo máy bay)
                3 => flightSeats,    // Ghế theo chuyến
                4 => tabMapFlight,   // Sơ đồ (theo chuyến)
                _ => seatList
            };

            current.Visible = true;
            current.BringToFront();
        }
    }
}
