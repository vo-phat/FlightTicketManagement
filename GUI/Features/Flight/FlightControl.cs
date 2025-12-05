using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.Flight.SubFeatures;
using GUI.Features.Setting; // để dùng Perm.*

namespace GUI.Features.Flight {
    public class FlightControl : UserControl {
        private Button btnList;
        private Button btnCreate;
        private FlightListControl listControl;
        private FlightDetailControl detailControl;
        private FlightCreateControl createControl;

        // Public property để MainForm có thể subscribe event
        public FlightListControl ListControl => listControl;

        // ===== Permission =======================================================
        private readonly Func<string, bool> _hasPerm;
        private bool _canList;
        private bool _canCreate;

        // Constructor mặc định – dùng cho MainForm hiện tại: new FlightControl()
        // Mặc định: cho phép tất cả => 2 tab đều hiển thị
        public FlightControl() : this(_ => true) { }

        // Constructor đầy đủ – sau này MainForm truyền HasPerm vào
        public FlightControl(Func<string, bool> hasPerm) {
            _hasPerm = hasPerm ?? (_ => true);
            InitializeComponent();
            ApplyPermissions();
        }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            btnList = new PrimaryButton("Danh sách chuyến bay");
            btnCreate = new SecondaryButton("Tạo chuyến bay mới");

            btnList.Click += (s, e) => SwitchTab(0);
            btnCreate.Click += (s, e) => SwitchTab(2);

            var buttonPanel = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            buttonPanel.Controls.Add(btnList);
            buttonPanel.Controls.Add(btnCreate);

            listControl = new FlightListControl(_hasPerm) { Dock = DockStyle.Fill };
            detailControl = new FlightDetailControl { Dock = DockStyle.Fill };
            createControl = new FlightCreateControl { Dock = DockStyle.Fill };

            Controls.Add(listControl);
            Controls.Add(detailControl);
            Controls.Add(createControl);
            Controls.Add(buttonPanel);

            // Không gọi SwitchTab ở đây, để ApplyPermissions quyết định tab đầu tiên
        }

        // Áp dụng quyền cho 2 tab
        private void ApplyPermissions() {
            // Nếu sau này bạn tách riêng:
            // _canList = _hasPerm(Perm.Flights_List);
            // còn hiện tại xài luôn Flights_Read cho tab danh sách
            _canList = _hasPerm(Perm.Flights_Read);
            _canCreate = _hasPerm(Perm.Flights_Create);

            // Ẩn/hiện nút theo quyền
            btnList.Visible = _canList;
            btnCreate.Visible = _canCreate;

            // Không có quyền nào -> ẩn 3 control, show message
            if (!_canList && !_canCreate) {
                listControl.Visible = false;
                detailControl.Visible = false;
                createControl.Visible = false;

                var lbl = new Label {
                    Text = "Bạn không có quyền truy cập chức năng Chuyến bay.",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 11, FontStyle.Italic)
                };
                Controls.Add(lbl);
                lbl.BringToFront();
                return;
            }

            // Ưu tiên mở tab danh sách nếu có quyền, không thì mở tab tạo
            if (_canList)
                SwitchTab(0);
            else if (_canCreate)
                SwitchTab(2);
        }

        private void SwitchTab(int idx) {
            // Chặn nhảy vào tab không có quyền
            if (idx == 0 && !_canList) return;
            if (idx == 2 && !_canCreate) return;

            listControl.Visible = (idx == 0);
            detailControl.Visible = (idx == 1);
            createControl.Visible = (idx == 2);

            var buttonPanel = btnList.Parent as FlowLayoutPanel;
            if (buttonPanel != null) {
                buttonPanel.Controls.Clear();

                if (idx == 0) {
                    btnList = new PrimaryButton("Danh sách chuyến bay");
                    btnCreate = new SecondaryButton("Tạo chuyến bay mới");
                } else if (idx == 1) {
                    btnList = new SecondaryButton("Danh sách chuyến bay");
                    btnCreate = new SecondaryButton("Tạo chuyến bay mới");
                } else { // idx == 2
                    btnList = new SecondaryButton("Danh sách chuyến bay");
                    btnCreate = new PrimaryButton("Tạo chuyến bay mới");
                }

                // gán lại sự kiện click
                btnList.Click += (s, e) => SwitchTab(0);
                btnCreate.Click += (s, e) => SwitchTab(2);

                // vẫn phải tôn trọng quyền
                btnList.Visible = _canList;
                btnCreate.Visible = _canCreate;

                buttonPanel.Controls.Add(btnList);
                buttonPanel.Controls.Add(btnCreate);
            }
        }

        public void ShowCreateForm(DTO.Flight.FlightDTO? flight = null) {
            createControl.LoadFlight(flight);
            SwitchTab(2);
        }
    }
}
