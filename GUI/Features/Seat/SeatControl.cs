using System;
using System.Drawing;
using System.Linq; // Cần thêm để dùng FirstOrDefault
using System.Windows.Forms;
using GUI.Components.Buttons;
using DTO.Seat; // Cần dùng DTO.Seat để LoadSeatForEdit

namespace GUI.Features.Seat {
    public class SeatControl : UserControl {
        private Panel header;
        private FlowLayoutPanel tabs;

        private int currentIndex = 0;
        private const int DETAIL_TAB_INDEX = 4; // Chỉ mục ẩn cho màn hình chi tiết

        private Control current;
        private SubFeatures.SeatListControl seatList;
        private SubFeatures.SeatCreateControl seatCreate; // Control Tạo/Sửa
        private SubFeatures.FlightSeatControl flightSeats;
        private SubFeatures.SeatMapControl seatMap;
        private SubFeatures.SeatDetailControl seatDetail; // THÊM DETAIL CONTROL

        public SeatControl() {
            InitializeComponent();
            RebuildTabs();
            SwitchTab(0);
        }

        private void InitializeComponent() {
            // Khởi tạo các Sub Controls
            seatList = new SubFeatures.SeatListControl { Dock = DockStyle.Fill };
            seatCreate = new SubFeatures.SeatCreateControl { Dock = DockStyle.Fill };
            flightSeats = new SubFeatures.FlightSeatControl { Dock = DockStyle.Fill };
            seatMap = new SubFeatures.SeatMapControl { Dock = DockStyle.Fill };
            seatDetail = new SubFeatures.SeatDetailControl { Dock = DockStyle.Fill };

            header = new Panel();
            tabs = new FlowLayoutPanel();

            // ĐĂNG KÝ SỰ KIỆN:

            // 1. Từ List -> Detail (VIEW)
            // LƯU Ý: SeatListControl cần phân biệt VIEW và EDIT trong CellMouseClick
            seatList.ViewOrEditRequested += (seatId) => SwitchToDetailTab(seatId);

            // 2. Từ List -> Edit (EDIT)
            // (Đòi hỏi EditRequested là PUBLIC trong SeatListControl)
            seatList.EditRequested += (seatId) => SwitchToEditTab(seatId);

            // 3. Từ Create/Edit -> List (Sau khi Lưu/Hủy)
            // (Đòi hỏi SeatCreated/EditCancelled là PUBLIC trong SeatCreateControl)
            seatCreate.SeatCreated += () => { // Sau khi THÊM thành công
                SwitchTab(0);
                seatList.LoadData();
            };
            seatCreate.EditCancelled += () => { // Sau khi HỦY SỬA HOẶC LƯU SỬA thành công
                SwitchTab(0);
                seatList.LoadData();
            };

            // 4. Từ Detail -> List (Đóng)
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
            seatList.BackColor = Color.FromArgb(232, 240, 252);
            seatList.Dock = DockStyle.Fill;
            seatCreate.BackColor = Color.FromArgb(232, 240, 252);
            seatCreate.Dock = DockStyle.Fill;
            flightSeats.BackColor = Color.FromArgb(232, 240, 252);
            flightSeats.Dock = DockStyle.Fill;
            seatMap.BackColor = Color.FromArgb(232, 240, 252);
            seatMap.Dock = DockStyle.Fill;
            seatDetail.Dock = DockStyle.Fill;

            // Main Container
            BackColor = Color.White;
            Controls.Add(header);
            Controls.Add(seatList);
            Controls.Add(seatCreate);
            Controls.Add(flightSeats);
            Controls.Add(seatMap);
            Controls.Add(seatDetail);

            Name = "SeatControl";
            Size = new Size(1074, 527);

            header.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void SeatDetail_CloseRequested(object sender, EventArgs e) {
            SwitchTab(0); // Chuyển trở lại tab danh sách (index 0)
        }

        private void RebuildTabs() {
            tabs.SuspendLayout();
            tabs.Controls.Clear();

            Control MakeTabButton(string text, int index) {
                Button b = (index == currentIndex)
                    ? new PrimaryButton(text)
                    : new SecondaryButton(text);

                b.Height = 36;
                b.Margin = new Padding(0, 0, 12, 0);
                b.BackColor = Color.White;
                b.FlatAppearance.MouseOverBackColor = Color.White;
                b.FlatAppearance.MouseDownBackColor = Color.White;
                b.Click += (_, __) => {
                    if (currentIndex != index) {
                        SwitchTab(index);
                    }
                };
                return b;
            }

            // Chỉ render các tab chính
            tabs.Controls.Add(MakeTabButton("Danh sách ghế", 0));
            tabs.Controls.Add(MakeTabButton("Tạo ghế", 1));
            tabs.Controls.Add(MakeTabButton("Ghế theo chuyến", 2));
            tabs.Controls.Add(MakeTabButton("Sơ đồ ghế", 3));

            tabs.ResumeLayout(true);
        }

        private void SwitchTab(int idx) {
            currentIndex = idx;

            // 1) Cập nhật giao diện nút 
            if (idx != DETAIL_TAB_INDEX) {
                RebuildTabs();
                // Đảm bảo form Tạo/Sửa ở chế độ Tạo mới khi được chọn từ tab
                if (idx == 1) {
                    // Yêu cầu SeatCreateControl có hàm SetCreateMode()
                    seatCreate.SetCreateMode();
                }
            }

            // 2) Hiển thị nội dung tương ứng
            if (current != null) current.Visible = false;

            current = idx switch {
                0 => seatList,
                1 => seatCreate,
                2 => flightSeats,
                3 => seatMap,
                DETAIL_TAB_INDEX => seatDetail,
                _ => seatList
            };

            // 3) Đảm bảo Header luôn nằm trên các tab nội dung
            current.Visible = true;
            current.BringToFront();
            header.BringToFront();
        }

        /// <summary>
        /// Xử lý hành động SỬA. Lấy DTO và chuyển sang form Create/Edit (Tab 1).
        /// </summary>
        public void SwitchToEditTab(int seatId) {
            // 1️⃣ Chuyển sang tab Create (hiển thị trước)
            SwitchTab(1);

            // 2️⃣ Load dữ liệu combobox (nếu chưa có)
            seatCreate.LoadComboboxData();

            // 3️⃣ Nạp thông tin ghế cần sửa
            seatCreate.LoadSeatForEdit(seatId);
        }


        /// <summary>
        /// Xử lý hành động XEM CHI TIẾT. Chuyển sang tab Detail (Tab 4).
        /// </summary>
        public void SwitchToDetailTab(int seatId) {
            seatDetail.LoadSeat(seatId);
            SwitchTab(DETAIL_TAB_INDEX);
        }
    }
}