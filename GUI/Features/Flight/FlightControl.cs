using FlightTicketManagement.GUI.Features.Flight.SubFeatures;
using FlightTicketManagement.GUI.Components.Buttons;

namespace FlightTicketManagement.GUI.Features.Flight {
    public class FlightControl : UserControl {
        private Button btnList;
        private Button btnCreate;
        private FlightListControl listControl;
        private FlightDetailControl detailControl;
        private FlightCreateControl createControl;

        public FlightControl() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.WhiteSmoke;

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

            listControl = new FlightListControl { Dock = DockStyle.Fill };
            detailControl = new FlightDetailControl { Dock = DockStyle.Fill };
            createControl = new FlightCreateControl { Dock = DockStyle.Fill };

            this.Controls.Add(listControl);
            this.Controls.Add(detailControl);
            this.Controls.Add(createControl);
            this.Controls.Add(buttonPanel);

            SwitchTab(0);
        }

        private void SwitchTab(int idx) {
            listControl.Visible = (idx == 0);
            detailControl.Visible = (idx == 1);
            createControl.Visible = (idx == 2);

            // Xóa các button cũ khỏi panel
            var buttonPanel = btnList.Parent as FlowLayoutPanel;
            if (buttonPanel != null) {
                buttonPanel.Controls.Clear();
                if (idx == 0) {
                    btnList = new PrimaryButton("Danh sách chuyến bay");
                    btnCreate = new SecondaryButton("Tạo chuyến bay mới");
                } else if (idx == 1) {
                    btnList = new SecondaryButton("Danh sách chuyến bay");
                    btnCreate = new SecondaryButton("Tạo chuyến bay mới");
                } else {
                    btnList = new SecondaryButton("Danh sách chuyến bay");
                    btnCreate = new PrimaryButton("Tạo chuyến bay mới");
                }
                btnList.Click += (s, e) => SwitchTab(0);
                btnCreate.Click += (s, e) => SwitchTab(2);
                buttonPanel.Controls.Add(btnList);
                buttonPanel.Controls.Add(btnCreate);
            }
        }
    }
}
