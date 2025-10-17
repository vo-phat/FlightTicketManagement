using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Buttons;

namespace FlightTicketManagement.GUI.Features.Aircraft {
    public class AircraftControl : UserControl {
        private Button btnList, btnCreate;
        private SubFeatures.AircraftListControl list;
        private SubFeatures.AircraftCreateControl create;
        private SubFeatures.AircraftDetailControl detail;

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

            list = new SubFeatures.AircraftListControl { Dock = DockStyle.Fill };
            create = new SubFeatures.AircraftCreateControl { Dock = DockStyle.Fill };
            detail = new SubFeatures.AircraftDetailControl { Dock = DockStyle.Fill };

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

            if (idx == 0) { btnList = new PrimaryButton("Danh sách máy bay"); btnCreate = new SecondaryButton("Tạo máy bay mới"); } else { btnList = new SecondaryButton("Danh sách máy bay"); btnCreate = new PrimaryButton("Tạo máy bay mới"); }

            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);
            top.Controls.AddRange(new Control[] { btnList, btnCreate });
        }
    }
}
