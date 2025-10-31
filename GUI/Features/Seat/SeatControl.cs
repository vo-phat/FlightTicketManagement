using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;

namespace GUI.Features.Seat
{
    public class SeatControl : UserControl
    {
        private Panel header;
        private FlowLayoutPanel tabs;

        // Chúng ta giữ chỉ số tab hiện tại để render lại dải nút theo Primary/Secondary
        private int currentIndex = 0;

        // Nội dung từng tab
        private Control current;
        private SubFeatures.SeatListControl seatList;
        private SubFeatures.SeatCreateControl seatCreate;
        private SubFeatures.FlightSeatControl flightSeats;
        private SubFeatures.SeatMapControl seatMap;

        public SeatControl()
        {
            InitializeComponent();
            SwitchTab(0);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // --- NỀN TRẮNG ---
            Dock = DockStyle.Fill;
            BackColor = Color.White;

            // Header trắng, autosize theo nội dung, không cố định chiều cao để tránh khuất
            header = new Panel
            {
                Dock = DockStyle.Top,
                BackColor = Color.White,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            // Dải tab: Dock Top, AutoSize để không bị cắt nút
            tabs = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(24, 8, 24, 8),
                WrapContents = false,
                BackColor = Color.White
            };

            header.Controls.Add(tabs);
            Controls.Add(header);

            // Khởi tạo các sub control (nội dung)
            seatList = new SubFeatures.SeatListControl { Dock = DockStyle.Fill };
            seatCreate = new SubFeatures.SeatCreateControl { Dock = DockStyle.Fill };
            flightSeats = new SubFeatures.FlightSeatControl { Dock = DockStyle.Fill };
            seatMap = new SubFeatures.SeatMapControl { Dock = DockStyle.Fill };

            Controls.Add(seatList);
            Controls.Add(seatCreate);
            Controls.Add(flightSeats);
            Controls.Add(seatMap);

            ResumeLayout(false);
            PerformLayout();

            // Render lần đầu dải tab (tương ứng currentIndex = 0)
            RebuildTabs();
        }

        /// <summary>
        /// Rebuild lại thanh tab sao cho tab đang chọn là PrimaryButton,
        /// các tab khác là SecondaryButton. Cách này đảm bảo “giống hệt”
        /// giao diện mẫu của bạn (không chỉ đổi màu/viền).
        /// </summary>
        private void RebuildTabs()
        {
            tabs.SuspendLayout();
            tabs.Controls.Clear();

            Control MakeTabButton(string text, int index)
            {
                Button b = (index == currentIndex)
                    ? new PrimaryButton(text)   // Tab đang chọn -> Primary
                    : new SecondaryButton(text); // Tab còn lại -> Secondary

                b.Height = 36;
                b.Margin = new Padding(0, 0, 12, 0);
                b.BackColor = Color.White;            // bảo đảm nền nút là trắng
                b.FlatAppearance.MouseOverBackColor = Color.White;
                b.FlatAppearance.MouseDownBackColor = Color.White;
                b.Click += (_, __) => {
                    if (currentIndex != index)
                    {
                        SwitchTab(index);
                    }
                };
                return b;
            }

            tabs.Controls.Add(MakeTabButton("Danh sách ghế", 0));
            tabs.Controls.Add(MakeTabButton("Tạo ghế", 1));
            tabs.Controls.Add(MakeTabButton("Ghế theo chuyến", 2));
            tabs.Controls.Add(MakeTabButton("Sơ đồ ghế", 3));

            tabs.ResumeLayout(true);
        }

        private void SwitchTab(int idx)
        {
            currentIndex = idx;

            // 1) Cập nhật giao diện nút (primary/secondary)
            RebuildTabs();

            // 2) Hiển thị nội dung tương ứng
            if (current != null) current.Visible = false;

            current = idx switch
            {
                0 => seatList,
                1 => seatCreate,
                2 => flightSeats,
                _ => seatMap
            };

            current.Visible = true;
            current.BringToFront();
        }
    }
}