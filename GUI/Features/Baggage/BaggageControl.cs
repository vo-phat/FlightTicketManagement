using GUI.Features.Baggage.SubFeatures;
using System;
using System.Windows.Forms;
using GUI.Components.Buttons;

namespace GUI.Features.Baggage {
    public partial class BaggageControl : UserControl {
        // Child controls
        //private BaggageCheckinControl baggageCheckinControl;
        //private BaggageDetailControl baggageDetailsControl;
        //private BaggageListControl baggageListControl;
        //private BaggageLostReportControl baggageLostReportControl;
        private FrmCarryOnManager frmCarryOnManager;
        private FrmCheckedBaggageManager frmCheckedBaggageManager;

        // Tab index
        private const int TAB_CHECKIN = 0;
        private const int TAB_DETAIL = 1;
        private const int TAB_LIST = 2;
        private const int TAB_LOST = 3;

        public BaggageControl() {
            InitializeComponent();
            InitializeChildControls();

            // Mặc định: mở tab Checkin
            SwitchTab(TAB_CHECKIN);
        }

        /// <summary>
        /// Khởi tạo các UserControl con và gắn vào panel.
        /// </summary>
        private void InitializeChildControls() {
            // Nếu sau này cần thì mở lại 4 control con này
            //baggageCheckinControl = new BaggageCheckinControl();
            //baggageDetailsControl = new BaggageDetailControl();
            //baggageListControl = new BaggageListControl();
            //baggageLostReportControl = new BaggageLostReportControl();

            frmCarryOnManager = new FrmCarryOnManager();
            frmCheckedBaggageManager = new FrmCheckedBaggageManager();

            // Panel nội dung Dock Fill
            pnlCheckin.Dock = DockStyle.Fill;
            pnlDetail.Dock = DockStyle.Fill;
            pnlList.Dock = DockStyle.Fill;
            pnlLost.Dock = DockStyle.Fill;

            // Gắn control Checkin
            frmCarryOnManager.Dock = DockStyle.Fill;
            pnlCheckin.Controls.Clear();
            pnlCheckin.Controls.Add(frmCarryOnManager);

            // Detail/List tạm thời chưa gắn gì
            //pnlDetail.Controls.Add(baggageDetailsControl);
            //pnlList.Controls.Add(baggageListControl);

            // Lost -> FrmCheckedBaggageManager
            frmCheckedBaggageManager.Dock = DockStyle.Fill;
            pnlLost.Controls.Clear();
            pnlLost.Controls.Add(frmCheckedBaggageManager);

            // Content + Header layout
            pnlContentBaggage.Dock = DockStyle.Fill;
            pnlHeaderBaggage.Dock = DockStyle.Top;
            pnlHeaderBaggage.Height = 60;
        }

        // ==========================
        // EVENT HANDLER CÁC NÚT TAB
        // ==========================

        private void btnCheckinBaggage_Click(object sender, EventArgs e) {
            SwitchTab(TAB_CHECKIN);
        }

        private void btnDetailBaggage_Click(object sender, EventArgs e) {
            SwitchTab(TAB_DETAIL);
        }

        private void btnListBaggage_Click(object sender, EventArgs e) {
            SwitchTab(TAB_LIST);
        }

        private void btnLostBaggage_Click(object sender, EventArgs e) {
            SwitchTab(TAB_LOST);
        }

        // ==========================
        // SWITCH TAB
        // ==========================

        public void SwitchTab(int index) {
            // Ẩn tất cả
            pnlCheckin.Visible = false;
            pnlDetail.Visible = false;
            pnlList.Visible = false;
            pnlLost.Visible = false;

            switch (index) {
                case TAB_CHECKIN:
                    pnlCheckin.Visible = true;
                    break;

                case TAB_DETAIL:
                    pnlDetail.Visible = true;
                    break;

                case TAB_LIST:
                    pnlList.Visible = true;
                    break;

                case TAB_LOST:
                    pnlLost.Visible = true;
                    break;

                default:
                    // có thể log lỗi, nhưng không cần MessageBox cho UI chính
                    break;
            }

            // Đổi Primary/Secondary cho nút theo tab đang chọn
            RebuildHeaderButtons(index);
        }

        // ==========================
        // HEADER BUTTONS (Primary/Secondary)
        // ==========================

        private void RebuildHeaderButtons(int activeIndex) {
            pnlHeaderBaggage.Controls.Clear();

            // --- Tab 0: Danh sách hành lý ---
            if (activeIndex == TAB_LIST)
                btnListBaggage = new PrimaryButton("Danh sách hành lý");
            else
                btnListBaggage = new SecondaryButton("Danh sách hành lý");

            btnListBaggage.AutoSize = true;
            btnListBaggage.Click += btnListBaggage_Click;
            // nếu hiện tại bạn chưa muốn show tab List:
            // btnListBaggage.Visible = false;
            pnlHeaderBaggage.Controls.Add(btnListBaggage);

            // --- Tab 1: Gắn tag / Checkin ---
            if (activeIndex == TAB_CHECKIN)
                btnCheckinBaggage = new PrimaryButton("Gắn tag/ Checkin");
            else
                btnCheckinBaggage = new SecondaryButton("Gắn tag/ Checkin");

            btnCheckinBaggage.AutoSize = true;
            btnCheckinBaggage.Click += btnCheckinBaggage_Click;
            pnlHeaderBaggage.Controls.Add(btnCheckinBaggage);

            // --- Tab 2: Theo dõi / Chi tiết ---
            if (activeIndex == TAB_DETAIL)
                btnDetailBaggage = new PrimaryButton("Theo dõi/ Chi tiết");
            else
                btnDetailBaggage = new SecondaryButton("Theo dõi/ Chi tiết");

            btnDetailBaggage.AutoSize = true;
            btnDetailBaggage.Click += btnDetailBaggage_Click;
            pnlHeaderBaggage.Controls.Add(btnDetailBaggage);

            // --- Tab 3: Báo cáo thất lạc ---
            if (activeIndex == TAB_LOST)
                btnLostBaggage = new PrimaryButton("Báo cáo thất lạc");
            else
                btnLostBaggage = new SecondaryButton("Báo cáo thất lạc");

            btnLostBaggage.AutoSize = true;
            btnLostBaggage.Click += btnLostBaggage_Click;
            // nếu muốn ẩn tab Lost giống code cũ:
            // btnLostBaggage.Visible = false;
            pnlHeaderBaggage.Controls.Add(btnLostBaggage);
        }

        private void pnlDetail_Paint(object sender, PaintEventArgs e) {
        }
    }
}
