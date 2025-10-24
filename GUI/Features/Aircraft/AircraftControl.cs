using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.Aircraft.SubFeatures;

namespace GUI.Features.Aircraft {
    public class AircraftControl : UserControl {
        private Button btnList, btnCreate;
        private Panel contentPanel;
        private AircraftListControl list;
        private AircraftCreateControl create;
        private AircraftDetailControl detail;

        public AircraftControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            btnList = new PrimaryButton("Danh sách máy bay");
            btnCreate = new SecondaryButton("Tạo máy bay mới");
            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);

            var top = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            top.Controls.AddRange(new Control[] { btnList, btnCreate });

            contentPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.WhiteSmoke };

            list = new AircraftListControl { Dock = DockStyle.Fill };
            create = new AircraftCreateControl { Dock = DockStyle.Fill };
            detail = new AircraftDetailControl { Dock = DockStyle.Fill };

            contentPanel.Controls.Add(list);
            contentPanel.Controls.Add(create);
            contentPanel.Controls.Add(detail);

            Controls.Add(contentPanel);
            Controls.Add(top);

            SwitchTab(0);
            list.BringToFront();
        }

        private void SwitchTab(int idx) {
            list.Visible = (idx == 0);
            create.Visible = (idx == 1);
            detail.Visible = false;

            // cập nhật style nút
            var top = btnList.Parent as FlowLayoutPanel;
            top!.Controls.Clear();
            if (idx == 0) {
                btnList = new PrimaryButton("Danh sách máy bay");
                btnCreate = new SecondaryButton("Tạo máy bay mới");
            } else {
                btnList = new SecondaryButton("Danh sách máy bay");
                btnCreate = new PrimaryButton("Tạo máy bay mới");
            }
            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);
            top.Controls.AddRange(new Control[] { btnList, btnCreate });

            // ép UC hiển thị
            if (idx == 0) list.BringToFront();
            else if (idx == 1) create.BringToFront();
        }
    }
}
