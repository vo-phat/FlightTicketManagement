using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Buttons;
using DTO.Seat;

namespace GUI.Features.Seat
{
    public class SeatControl : UserControl
    {
        private Panel header;
        private FlowLayoutPanel tabs;

        private int currentIndex = 0;
        private const int DETAIL_TAB_INDEX = 3; // ✅ Updated: 2->3 (now 3 tabs)

        private Control current;
        // ✅ ADDED: AircraftListControl
        private SubFeatures.AircraftListControl aircraftList;
        private SubFeatures.FlightSeatControl flightSeats;
        private SubFeatures.SeatMapControl seatMap;
        private SubFeatures.SeatDetailControl seatDetail;

        public SeatControl()
        {
            InitializeComponent();
            RebuildTabs();
            SwitchTab(0);
        }

        private void InitializeComponent()
        {
            // ✅ Added: aircraftList initialization
            aircraftList = new SubFeatures.AircraftListControl { Dock = DockStyle.Fill };
            flightSeats = new SubFeatures.FlightSeatControl { Dock = DockStyle.Fill };
            seatMap = new SubFeatures.SeatMapControl { Dock = DockStyle.Fill };
            seatDetail = new SubFeatures.SeatDetailControl { Dock = DockStyle.Fill };

            header = new Panel();
            tabs = new FlowLayoutPanel();

            // ✅ Wire-up: AircraftList -> Detail view
            aircraftList.SeatSelected += (seatId) => SwitchToDetailTab(seatId);
            
            // 1. Từ Detail -> List (Đóng)
            seatDetail.CloseRequested += SeatDetail_CloseRequested;

            header.SuspendLayout();
            SuspendLayout();

            // Setup Header và Tabs 
            header.Controls.Add(tabs);
            header.Dock = DockStyle.Top;
            header.BackColor = Color.White;
            header.AutoSize = true;
            header.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            header.Name = "header";
            header.Size = new Size(200, 100);
            header.TabIndex = 0;

            tabs.Dock = DockStyle.Top;
            tabs.AutoSize = true;
            tabs.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tabs.Padding = new Padding(24, 8, 24, 8);
            tabs.WrapContents = false;
            tabs.BackColor = Color.White;
            tabs.Name = "tabs";
            tabs.Size = new Size(200, 100);
            tabs.TabIndex = 0;

            // Setup Sub Controls
            // ✅ Added: aircraftList setup
            aircraftList.BackColor = Color.FromArgb(232, 240, 252);
            aircraftList.Dock = DockStyle.Fill;
            flightSeats.BackColor = Color.FromArgb(232, 240, 252);
            flightSeats.Dock = DockStyle.Fill;
            seatMap.BackColor = Color.FromArgb(232, 240, 252);
            seatMap.Dock = DockStyle.Fill;
            seatDetail.Dock = DockStyle.Fill;

            // Main Container
            BackColor = Color.White;
            Controls.Add(header);
            // ✅ Added: Controls.Add(aircraftList)
            Controls.Add(aircraftList);
            Controls.Add(flightSeats);
            Controls.Add(seatMap);
            Controls.Add(seatDetail);

            Name = "SeatControl";
            Size = new Size(1074, 527);

            header.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void SeatDetail_CloseRequested(object sender, EventArgs e)
        {
            SwitchTab(0); // Chuyển trở lại tab danh sách (index 0)
        }

        // ✅ Removed: RefreshSeatList() - no seatList to refresh

        private void RebuildTabs()
        {
            tabs.SuspendLayout();
            tabs.Controls.Clear();

            Control MakeTabButton(string text, int index)
            {
                Button b = (index == currentIndex)
                    ? new PrimaryButton(text)
                    : new SecondaryButton(text);

                b.Height = 36;
                b.Margin = new Padding(0, 0, 12, 0);
                b.BackColor = Color.White;
                b.FlatAppearance.MouseOverBackColor = Color.White;
                b.FlatAppearance.MouseDownBackColor = Color.White;
                b.Click += (_, __) =>
                {
                    if (currentIndex != index)
                    {
                        SwitchTab(index);
                    }
                };
                return b;
            }

            // ✅ Now 3 tabs: 0=Danh sách máy bay, 1=Ghế theo chuyến, 2=Sơ đồ ghế
            tabs.Controls.Add(MakeTabButton("✈️ Danh sách máy bay", 0));
            tabs.Controls.Add(MakeTabButton("🎫 Ghế theo chuyến", 1));
            tabs.Controls.Add(MakeTabButton("🗺️ Sơ đồ ghế", 2));

            tabs.ResumeLayout(true);
        }

        private void SwitchTab(int idx)
        {
            currentIndex = idx;

            // Cập nhật giao diện nút 
            if (idx != DETAIL_TAB_INDEX)
            {
                RebuildTabs();
            }

            // Hiển thị nội dung tương ứng
            if (current != null) current.Visible = false;

            // ✅ Updated indices: 0=AircraftList, 1=FlightSeats, 2=SeatMap
            current = idx switch
            {
                0 => aircraftList,
                1 => flightSeats,
                2 => seatMap,
                DETAIL_TAB_INDEX => seatDetail,
                _ => aircraftList  // Default to AircraftList
            };

            // ✅ Tự động refresh khi chuyển vào các tab
            if (idx == 0 && aircraftList != null)
            {
                System.Diagnostics.Debug.WriteLine("[SeatControl] Switching to AircraftListControl, calling LoadData()...");
                aircraftList.LoadData();
            }
            else if (idx == 2 && seatMap != null)
            {
                System.Diagnostics.Debug.WriteLine("[SeatControl] Switching to SeatMapControl, calling Refresh()...");
                seatMap.Refresh();
            }

            // Đảm bảo Header luôn nằm trên các tab nội dung
            current.Visible = true;
            current.BringToFront();
            header.BringToFront();
        }

        /// <summary>
        /// Xử lý hành động XEM CHI TIẾT. Chuyển sang tab Detail (Tab 3).
        /// </summary>
        public void SwitchToDetailTab(int seatId)
        {
            seatDetail.LoadSeat(seatId);
            SwitchTab(DETAIL_TAB_INDEX);
        }
    }
}