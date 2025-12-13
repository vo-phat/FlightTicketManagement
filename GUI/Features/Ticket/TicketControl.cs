using BUS.Ticket;
using DTO.Ticket;
using GUI.Features.Ticket.subTicket;
using GUI.Features.Baggage.SubFeatures;
using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;

namespace GUI.Features.Ticket {
    public partial class TicketControl : UserControl {
        // Child controls
        private BookingSearchControl bookingSearchControl;
        private HistoryTicketControl historyTicketControl;
        private TicketOpsControl ticketOpsControl;
        private frmPassengerInfoControl frmPassengerInfoControl;

        // Tab index (cho dễ đọc, tránh magic number)
        private const int TAB_BOOKING = 0;
        private const int TAB_HISTORY = 1;
        private const int TAB_TICKET_OPS = 2;
        private const int TAB_PASSENGER_INFO = 3;
        public TicketControl() {
            InitializeComponent();
            InitializeChildControls();

            // Event mở quản lý hành lý
            ticketOpsControl.OnOpenBaggageManager += TicketOpsControl_OnOpenBaggageManager;

            // Mặc định mở trang tìm kiếm vé
            switchTab(TAB_BOOKING);
        }

        /// <summary>
        /// Load booking data và chuyển sang tab Thông tin khách hàng
        /// </summary>
        public void LoadBookingData(DTO.Booking.BookingRequestDTO outboundBooking, DTO.Booking.BookingRequestDTO returnBooking = null)
        {
            // Chuyển sang tab Thông tin khách hàng
            switchTab(TAB_BOOKING);
            
            // Truyền dữ liệu booking vào control (both outbound and return if applicable)
            frmPassengerInfoControl.LoadBookingRequest(outboundBooking, returnBooking);
        }

        // Khởi tạo và gắn các UserControl con vào các panel tương ứng
        private void InitializeChildControls() {
            //bookingSearchControl = new BookingSearchControl();
            historyTicketControl = new HistoryTicketControl();
            ticketOpsControl = new TicketOpsControl();
            frmPassengerInfoControl = new frmPassengerInfoControl();

            // Dock panel con
            pnlBookingSearch.Dock = DockStyle.Fill;
            pnlHistoryTicket.Dock = DockStyle.Fill;
            pnlTicketOps.Dock = DockStyle.Fill;
            pnlFrmPassengerInfo.Dock = DockStyle.Fill;

            // Dock control con
            //bookingSearchControl.Dock = DockStyle.Fill;
            historyTicketControl.Dock = DockStyle.Fill;
            ticketOpsControl.Dock = DockStyle.Fill;
            frmPassengerInfoControl.Dock = DockStyle.Fill;

            // Add control con vào panel
            //pnlBookingSearch.Controls.Add(bookingSearchControl);
            pnlHistoryTicket.Controls.Add(historyTicketControl);
            pnlTicketOps.Controls.Add(ticketOpsControl);
            pnlFrmPassengerInfo.Controls.Add(frmPassengerInfoControl);

            // Đảm bảo content panel chứa đủ 4 panel con
            //pnlContentTicket.Controls.Add(pnlBookingSearch);
            pnlContentTicket.Controls.Add(pnlHistoryTicket);
            pnlContentTicket.Controls.Add(pnlTicketOps);
            pnlContentTicket.Controls.Add(pnlFrmPassengerInfo);

            // Header luôn dock trên cùng (phòng trường hợp designer chỉnh)
            pnlHeaderTicket.Dock = DockStyle.Top;
            pnlHeaderTicket.Height = 60;
        }

        private void TicketOpsControl_OnOpenBaggageManager(int ticketId) {
            MessageBox.Show($"Mở quản lý hành lý cho vé có ID: {ticketId}");

            var frm = new FrmTicketBaggageManager(ticketId) {
                Dock = DockStyle.Fill
            };

            pnlTicketOps.Controls.Clear();
            pnlTicketOps.Controls.Add(frm);
            frm.BringToFront();
            // Tạo control quản lý hành lý
            var frmBaggage = new FrmTicketBaggageManager(ticketId)
            {
                Dock = DockStyle.Fill
            };

            // Đăng ký sự kiện: Khi bấm nút Back -> Quay lại giao diện cũ
            frmBaggage.OnClose += () =>
            {
                // 1. Xóa form hành lý đi
                pnlTicketOps.Controls.Remove(frmBaggage);
                frmBaggage.Dispose();

                // 2. Hiển thị lại ticketOpsControl gốc
                // (Giả sử bạn đã add ticketOpsControl vào pnlTicketOps từ đầu)
                if (!pnlTicketOps.Controls.Contains(ticketOpsControl))
                {
                    pnlTicketOps.Controls.Add(ticketOpsControl);
                }
                ticketOpsControl.Visible = true;
                ticketOpsControl.BringToFront();
            };

            // Ẩn giao diện cũ (Ops) và hiện giao diện mới (Baggage)
            // Cách 1: Xóa tạm thời (Tiết kiệm ram, nhưng mất trạng thái search cũ)
            // pnlTicketOps.Controls.Clear(); 

            // Cách 2: Ẩn đi (Giữ nguyên trạng thái search cũ - Khuyên dùng)
            ticketOpsControl.Visible = false;

            pnlTicketOps.Controls.Add(frmBaggage);
            frmBaggage.BringToFront();
        }

        // ==========================
        // Sự kiện click của 3 tab
        // ==========================

        private void btnHistoryTicketAdmin_Click(object sender, EventArgs e) {
            switchTab(TAB_HISTORY);
        }

        private void btnOpsTicket_Click(object sender, EventArgs e) {
            switchTab(TAB_TICKET_OPS);
        }

        private void btnFrmPassengerInfoTiket_Click(object sender, EventArgs e) {
            switchTab(TAB_BOOKING);
        }

        // ==========================
        // TAB SWITCHER
        // ==========================

        public void switchTab(int index) {
            // Ẩn tất cả panel nội dung
            pnlBookingSearch.Visible = false;
            pnlHistoryTicket.Visible = false;
            pnlTicketOps.Visible = false;
            pnlFrmPassengerInfo.Visible = false;

            // Mở panel tương ứng
            switch (index) {
                case TAB_BOOKING:
                    pnlFrmPassengerInfo.Visible = true;
                    break;
                case TAB_HISTORY:
                    pnlHistoryTicket.Visible = true;
                    break;
                case TAB_TICKET_OPS:
                    pnlTicketOps.Visible = true;
                    break;
                //case TAB_PASSENGER_INFO:
                //    pnlFrmPassengerInfo.Visible = true;
                //    break;
            }

            // Rebuild header: tab active = PrimaryButton, inactive = SecondaryButton
            RebuildHeaderButtons(index);
        }

        // ==========================
        // HEADER BUTTONS
        // ==========================

        private void RebuildHeaderButtons(int activeIndex) {
            // Xoá hết nút header cũ
            pnlHeaderTicket.Controls.Clear();

            // --- Tab: Thông tin khách hàng (idx = 3) ---
            if (activeIndex == TAB_PASSENGER_INFO)
                btnFrmPassengerInfoTiket = new PrimaryButton("Nhâp thông tin hành khách");
            else
                //btnFrmPassengerInfoTiket = new SecondaryButton("Thông tin khách hàng");
                btnFrmPassengerInfoTiket = new PrimaryButton("Nhâp thông tin hành khách");

            btnFrmPassengerInfoTiket.AutoSize = true;
            btnFrmPassengerInfoTiket.Click += btnFrmPassengerInfoTiket_Click;
            pnlHeaderTicket.Controls.Add(btnFrmPassengerInfoTiket);

            // --- Tab: Quản lý vé (idx = 2) ---
            if (activeIndex == TAB_TICKET_OPS)
            {
                btnOpsTicket = new SecondaryButton("Quản lý vé");
                //btnOpsTicket = new PrimaryButton("Quản lý vé");
            }
            else
                btnOpsTicket = new SecondaryButton("Quản lý vé");

            btnOpsTicket.AutoSize = true;
            btnOpsTicket.Click += btnOpsTicket_Click;
            pnlHeaderTicket.Controls.Add(btnOpsTicket);

            // --- Tab: Lịch sử vé (idx = 1) ---
            if (activeIndex == TAB_HISTORY)
                //btnHistoryTicketAdmin = new PrimaryButton("Lịch sử vé");
            btnHistoryTicketAdmin = new SecondaryButton("Lịch sử vé của tôi");
            else
                btnHistoryTicketAdmin = new SecondaryButton("Lịch sử vé của tôi");

            btnHistoryTicketAdmin.AutoSize = true;
            btnHistoryTicketAdmin.Click += btnHistoryTicketAdmin_Click;
            pnlHeaderTicket.Controls.Add(btnHistoryTicketAdmin);
        }

        // ==========================
        // Event trống giữ nguyên
        // ==========================

        private void TicketControl_Load(object sender, EventArgs e) {
        }

        private void pnlHeaderTicket_Paint(object sender, PaintEventArgs e) {
        }
    }
}
