using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;

namespace GUI.Features.Airport {
    public class AirportControl : UserControl {
        private Button btnList, btnCreate;
        private SubFeatures.AirportListControl list;
        private SubFeatures.AirportCreateControl create;
        private SubFeatures.AirportDetailControl detail;

        public AirportControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill; BackColor = Color.WhiteSmoke;

            btnList = new PrimaryButton("Danh sách sân bay");
            btnCreate = new SecondaryButton("Tạo sân bay");
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

            list = new SubFeatures.AirportListControl { Dock = DockStyle.Fill };
            create = new SubFeatures.AirportCreateControl { Dock = DockStyle.Fill };
            detail = new SubFeatures.AirportDetailControl { Dock = DockStyle.Fill };

            Controls.Add(list); Controls.Add(create); Controls.Add(detail); Controls.Add(top);
            SwitchTab(0);
        }

        private void SwitchTab(int idx) {
            list.Visible = (idx == 0);
            create.Visible = (idx == 1);
            detail.Visible = false;

            var top = btnList.Parent as FlowLayoutPanel; top!.Controls.Clear();
            if (idx == 0) { btnList = new PrimaryButton("Danh sách sân bay"); btnCreate = new SecondaryButton("Tạo sân bay"); } else { btnList = new SecondaryButton("Danh sách sân bay"); btnCreate = new PrimaryButton("Tạo sân bay"); }
            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);
            top.Controls.AddRange(new Control[] { btnList, btnCreate });
        }
    }
}
