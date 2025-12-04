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
        private FrmCarryOnManager frmCarryOnManager;
        private FrmCheckedBaggageManager frmCheckedBaggageManager;
        public BaggageControl()
        {
            InitializeComponent();

            // Khởi tạo control con
            //baggageCheckinControl = new BaggageCheckinControl();
            //baggageDetailsControl = new BaggageDetailControl();
            //baggageListControl = new BaggageListControl();
            baggageLostReportControl = new BaggageLostReportControl();
            frmCarryOnManager = new FrmCarryOnManager();
            frmCheckedBaggageManager = new FrmCheckedBaggageManager();
            //int id = 1;
            //FrmTicketBaggageManager frmTicketBaggageManager = new FrmTicketBaggageManager(id); // demo, không có dùng

            // 3 panel TAB: chồng lên nhau
            pnlCheckin.Dock = DockStyle.Fill;
            pnlDetail.Dock = DockStyle.Fill;
            pnlList.Dock = DockStyle.Fill;
            pnlLost.Dock = DockStyle.Fill;

            // UserControl fill panel
            frmCarryOnManager.Dock = DockStyle.Fill;
            //baggageDetailsControl.Dock = DockStyle.Fill;
            //baggageListControl.Dock = DockStyle.Fill;
            baggageLostReportControl.Dock = DockStyle.Fill;

            // Add đúng chỗ
            pnlCheckin.Controls.Add(frmCarryOnManager);
            //pnlDetail.Controls.Add(baggageDetailsControl);
            //pnlList.Controls.Add(baggageListControl);
            pnlLost.Controls.Add(frmCheckedBaggageManager);


            // Add button vào header
            //pnlHeaderBaggage.Controls.Add(btnCheckinBaggage);
            //pnlHeaderBaggage.Controls.Add(btnDetailBaggage);
            pnlHeaderBaggage.Controls.Add(btnListBaggage);
            pnlHeaderBaggage.Controls.Add(btnLostBaggage);
            pnlHeaderBaggage.Dock = DockStyle.Top;
            pnlHeaderBaggage.Height = 56;
            pnlHeaderBaggage.BringToFront();
            ///
            pnlContentBaggage.Controls.Add(pnlCheckin);
            //pnlContentBaggage.Controls.Add(pnlDetail);
            pnlContentBaggage.Controls.Add(pnlList);
            pnlContentBaggage.Controls.Add(pnlLost);
            //pnlContentBaggage.Dock = DockStyle.Bottom;
            //pnlContentBaggage.Height = 100;
            pnlContentBaggage.BringToFront();
            // Mặc định show List
            //SwitchTab(2);

            ///
            btnListBaggage.Visible = false;
            btnLostBaggage.Visible = false;
        }

        private void btnListBaggage_Click(object sender, EventArgs e) => SwitchTab(2);

        private void btnCheckinBaggage_Click(object sender, EventArgs e)
        {
            SwitchTab(0);
        }
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

        private void pnlDetail_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
