using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.Payments.SubFeatures;

namespace GUI.Features.Payments {
    public class PaymentsControl : UserControl {
        private Button btnPOS;
        private Panel contentPanel;
        private PaymentsPOSControl posControl;

        public PaymentsControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            btnPOS = new PrimaryButton("POS / Thanh toán thường");
            //btnPOS.Click += (_, __) => SwitchTab(0);

            var top = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            top.Controls.AddRange(new Control[] { btnPOS });

            contentPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.WhiteSmoke };

            posControl = new PaymentsPOSControl { Dock = DockStyle.Fill };
            

            contentPanel.Controls.Add(posControl);
           

            Controls.Add(contentPanel);
            Controls.Add(top);

            //SwitchTab(0);
            posControl.BringToFront();
        }

        //private void SwitchTab(int idx) {
        //    posControl.Visible = (idx == 0);
          

        //    var top = btnPOS.Parent as FlowLayoutPanel;
        //    top!.Controls.Clear();
        //    if (idx == 0) {
        //        btnPOS = new PrimaryButton("POS / Thanh toán thường");

        //    } else {
        //        btnPOS = new SecondaryButton("POS / Thanh toán thường");

        //    }
        //    btnPOS.Click += (_, __) => SwitchTab(0);
           
        //    top.Controls.AddRange(new Control[] { btnPOS });

          
        //}
    }
}
