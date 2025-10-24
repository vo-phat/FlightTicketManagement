using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Buttons;

namespace FlightTicketManagement.GUI.Features.Ticket {
    public class TicketControl : UserControl {
        private Button btnSearchCreate, btnOps;
        private Panel contentPanel;
        private BookingSearchControl searchControl;
        private TicketOpsControl opsControl;

        public TicketControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            btnSearchCreate = new PrimaryButton("Tạo/Tìm đặt chỗ");
            btnOps = new SecondaryButton("Quản lý vé");
            btnSearchCreate.Click += (_, __) => SwitchTab(0);
            btnOps.Click += (_, __) => SwitchTab(1);

            var top = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            top.Controls.AddRange(new Control[] { btnSearchCreate, btnOps });

            contentPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.WhiteSmoke };
            searchControl = new BookingSearchControl { Dock = DockStyle.Fill };
            opsControl = new TicketOpsControl { Dock = DockStyle.Fill };

            contentPanel.Controls.Add(searchControl);
            contentPanel.Controls.Add(opsControl);

            Controls.Add(contentPanel);
            Controls.Add(top);

            SwitchTab(0);
            searchControl.BringToFront();
        }

        private void SwitchTab(int idx) {
            searchControl.Visible = (idx == 0);
            opsControl.Visible = (idx == 1);

            var top = btnSearchCreate.Parent as FlowLayoutPanel;
            top!.Controls.Clear();
            if (idx == 0) {
                btnSearchCreate = new PrimaryButton("Tạo/Tìm đặt chỗ");
                btnOps = new SecondaryButton("Quản lý vé");
            } else {
                btnSearchCreate = new SecondaryButton("Tạo/Tìm đặt chỗ");
                btnOps = new PrimaryButton("Quản lý vé");
            }
            btnSearchCreate.Click += (_, __) => SwitchTab(0);
            btnOps.Click += (_, __) => SwitchTab(1);
            top.Controls.AddRange(new Control[] { btnSearchCreate, btnOps });

            if (idx == 0) searchControl.BringToFront(); else opsControl.BringToFront();
        }
    }
}
