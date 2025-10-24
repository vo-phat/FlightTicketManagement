using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;

namespace GUI.Features.Airline {
    public class AirlineControl : UserControl {
        private Button btnList, btnCreate;
        private SubFeatures.AirlineListControl list;
        private SubFeatures.AirlineCreateControl create;
        private SubFeatures.AirlineDetailControl detail;

        public AirlineControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            btnList = new PrimaryButton("Danh sách hãng");
            btnCreate = new SecondaryButton("Tạo hãng mới");
            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);

            var top = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            top.Controls.Add(btnList);
            top.Controls.Add(btnCreate);

            list = new SubFeatures.AirlineListControl { Dock = DockStyle.Fill };
            create = new SubFeatures.AirlineCreateControl { Dock = DockStyle.Fill };
            detail = new SubFeatures.AirlineDetailControl { Dock = DockStyle.Fill };

            Controls.Add(list);
            Controls.Add(create);
            Controls.Add(detail);
            Controls.Add(top);

            SwitchTab(0);
        }

        private void SwitchTab(int idx) {
            list.Visible = (idx == 0);
            create.Visible = (idx == 1);
            detail.Visible = false;

            var top = btnList.Parent as FlowLayoutPanel;
            top!.Controls.Clear();

            if (idx == 0) { btnList = new PrimaryButton("Danh sách hãng"); btnCreate = new SecondaryButton("Tạo hãng mới"); } else { btnList = new SecondaryButton("Danh sách hãng"); btnCreate = new PrimaryButton("Tạo hãng mới"); }

            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);

            top.Controls.Add(btnList);
            top.Controls.Add(btnCreate);
        }
    }
}
