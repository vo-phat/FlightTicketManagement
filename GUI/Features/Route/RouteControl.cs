using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Buttons;

namespace FlightTicketManagement.GUI.Features.Route {
    public class RouteControl : UserControl {
        private Button btnList, btnCreate;
        private SubFeatures.RouteListControl list;
        private SubFeatures.RouteCreateControl create;
        private SubFeatures.RouteDetailControl detail;

        public RouteControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            btnList = new PrimaryButton("Danh sách tuyến bay");
            btnCreate = new SecondaryButton("Tạo tuyến bay mới");
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

            list = new SubFeatures.RouteListControl { Dock = DockStyle.Fill };
            create = new SubFeatures.RouteCreateControl { Dock = DockStyle.Fill };
            detail = new SubFeatures.RouteDetailControl { Dock = DockStyle.Fill };

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

            if (idx == 0) { btnList = new PrimaryButton("Danh sách tuyến bay"); btnCreate = new SecondaryButton("Tạo tuyến bay mới"); } else { btnList = new SecondaryButton("Danh sách tuyến bay"); btnCreate = new PrimaryButton("Tạo tuyến bay mới"); }

            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);
            top.Controls.Add(btnList);
            top.Controls.Add(btnCreate);
        }
    }
}
