using GUI.Features.Baggage;
using GUI.Features.Baggage.SubFeatures;
using System;
using System.Reflection.PortableExecutable;
using System.Windows.Forms;

namespace GUI.Features.Baggage
{
    public partial class BaggageControl : UserControl
    {
        private BaggageCheckinControl baggageCheckinControl;
        private BaggageDetailControl baggageDetailsControl;
        private BaggageListControl baggageListControl;
        private BaggageLostReportControl baggageLostReportControl;

        public BaggageControl()
        {
            InitializeComponent();

            // Khởi tạo control con
            baggageCheckinControl = new BaggageCheckinControl();
            baggageDetailsControl = new BaggageDetailControl();
            baggageListControl = new BaggageListControl();
            baggageLostReportControl = new BaggageLostReportControl();

            // 3 panel TAB: chồng lên nhau
            pnlCheckin.Dock = DockStyle.Fill;
            pnlDetail.Dock = DockStyle.Fill;
            pnlList.Dock = DockStyle.Fill;
            pnlLost.Dock = DockStyle.Fill;

            // UserControl fill panel
            baggageCheckinControl.Dock = DockStyle.Fill;
            baggageDetailsControl.Dock = DockStyle.Fill;
            baggageListControl.Dock = DockStyle.Fill;
            baggageLostReportControl.Dock = DockStyle.Fill;

            // Add đúng chỗ
            pnlCheckin.Controls.Add(baggageCheckinControl);
            pnlDetail.Controls.Add(baggageDetailsControl);
            pnlList.Controls.Add(baggageListControl);
            pnlLost.Controls.Add(baggageLostReportControl);


            // Add button vào header
            pnlHeaderBaggage.Controls.Add(btnCheckinBaggage);
            pnlHeaderBaggage.Controls.Add(btnDetailBaggage);
            pnlHeaderBaggage.Controls.Add(btnListBaggage);
            pnlHeaderBaggage.Controls.Add(btnLostBaggage);
            pnlHeaderBaggage.Dock = DockStyle.Top;
            pnlHeaderBaggage.Height = 56;
            pnlHeaderBaggage.BringToFront();
            ///
            pnlContentBaggage.Controls.Add(pnlCheckin);
            pnlContentBaggage.Controls.Add(pnlDetail);
            pnlContentBaggage.Controls.Add(pnlList);
            pnlContentBaggage.Controls.Add(pnlLost);
            //pnlContentBaggage.Dock = DockStyle.Bottom;
            //pnlContentBaggage.Height = 100;
            pnlContentBaggage.BringToFront();
            // Mặc định show List
            //SwitchTab(2);
        }

        private void btnListBaggage_Click(object sender, EventArgs e) => SwitchTab(2);
        private void btnCheckinBaggage_Click(object sender, EventArgs e) => SwitchTab(0);
        private void btnDetailBaggage_Click(object sender, EventArgs e) => SwitchTab(1);
        private void btnLostBaggage_Click(object sender, EventArgs e) => SwitchTab(3);
      

        public void SwitchTab(int i)
        {
            pnlCheckin.Visible = pnlDetail.Visible = pnlList.Visible = false;

            switch (i)
            {
                case 0:
                    pnlCheckin.Visible = true;
                    //pnlCheckin.BringToFront();
                    break;
                case 1:
                    pnlDetail.Visible = true;
                    //pnlDetail.BringToFront();
                    break;
                case 2:
                    pnlList.Visible = true;
                    //pnlList.BringToFront();
                    break;
                case 3:
                    pnlLost.Visible = true;
                    break;
                default:
                    MessageBox.Show("Index không hợp lệ!");
                    break;
            }
        }

        
    }
}
