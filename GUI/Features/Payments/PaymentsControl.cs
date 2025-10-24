using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Buttons;

namespace FlightTicketManagement.GUI.Features.Payments {
    public class PaymentsControl : UserControl {
        private Button btnPOS, btnOnline;
        private Panel contentPanel;
        private PaymentsPOSControl posControl;
        private PaymentsOnlineControl onlineControl;

        public PaymentsControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            btnPOS = new PrimaryButton("POS / Thanh toán thường");
            btnOnline = new SecondaryButton("Thanh toán online");
            btnPOS.Click += (_, __) => SwitchTab(0);
            btnOnline.Click += (_, __) => SwitchTab(1);

            var top = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            top.Controls.AddRange(new Control[] { btnPOS, btnOnline });

            contentPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.WhiteSmoke };

            posControl = new PaymentsPOSControl { Dock = DockStyle.Fill };
            onlineControl = new PaymentsOnlineControl { Dock = DockStyle.Fill };

            contentPanel.Controls.Add(posControl);
            contentPanel.Controls.Add(onlineControl);

            Controls.Add(contentPanel);
            Controls.Add(top);

            SwitchTab(0);
            posControl.BringToFront();
        }

        private void SwitchTab(int idx) {
            posControl.Visible = (idx == 0);
            onlineControl.Visible = (idx == 1);

            var top = btnPOS.Parent as FlowLayoutPanel;
            top!.Controls.Clear();
            if (idx == 0) {
                btnPOS = new PrimaryButton("POS / Thanh toán thường");
                btnOnline = new SecondaryButton("Thanh toán online");
            } else {
                btnPOS = new SecondaryButton("POS / Thanh toán thường");
                btnOnline = new PrimaryButton("Thanh toán online");
            }
            btnPOS.Click += (_, __) => SwitchTab(0);
            btnOnline.Click += (_, __) => SwitchTab(1);
            top.Controls.AddRange(new Control[] { btnPOS, btnOnline });

            if (idx == 0) posControl.BringToFront(); else onlineControl.BringToFront();
        }
    }
}
