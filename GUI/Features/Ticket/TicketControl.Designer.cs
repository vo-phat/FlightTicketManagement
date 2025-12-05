using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Ticket {
    partial class TicketControl {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent() {
            pnlHeaderTicket = new FlowLayoutPanel();
            btnFrmPassengerInfoTiket = new GUI.Components.Buttons.PrimaryButton();
            btnOpsTicket = new GUI.Components.Buttons.SecondaryButton();
            btnHistoryTicketAdmin = new GUI.Components.Buttons.SecondaryButton();

            pnlContentTicket = new Panel();
            pnlBookingSearch = new Panel();
            pnlHistoryTicket = new Panel();
            pnlTicketOps = new Panel();
            pnlFrmPassengerInfo = new Panel();

            pnlHeaderTicket.SuspendLayout();
            pnlContentTicket.SuspendLayout();
            SuspendLayout();

            // ============================================================
            // HEADER PANEL
            // ============================================================
            pnlHeaderTicket.Controls.Add(btnFrmPassengerInfoTiket);
            pnlHeaderTicket.Controls.Add(btnOpsTicket);
            pnlHeaderTicket.Controls.Add(btnHistoryTicketAdmin);
            pnlHeaderTicket.Dock = DockStyle.Top;
            pnlHeaderTicket.Height = 56;
            pnlHeaderTicket.BackColor = Color.White;
            pnlHeaderTicket.Padding = new Padding(24, 12, 0, 0);
            pnlHeaderTicket.AutoSize = true;
            pnlHeaderTicket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlHeaderTicket.WrapContents = false;
            pnlHeaderTicket.Name = "pnlHeaderTicket";

            // ============================================================
            // BUTTON – THÔNG TIN KHÁCH HÀNG (PRIMARY)
            // ============================================================
            btnFrmPassengerInfoTiket.AutoSize = true;
            btnFrmPassengerInfoTiket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnFrmPassengerInfoTiket.Text = "Thông tin khách hàng";
            btnFrmPassengerInfoTiket.Padding = new Padding(24, 10, 24, 10);
            btnFrmPassengerInfoTiket.Click += btnFrmPassengerInfoTiket_Click;

            // ============================================================
            // BUTTON – QUẢN LÝ VÉ (SECONDARY)
            // ============================================================
            btnOpsTicket.AutoSize = true;
            btnOpsTicket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnOpsTicket.Text = "Quản lý vé";
            btnOpsTicket.Padding = new Padding(24, 10, 24, 10);
            btnOpsTicket.Click += btnOpsTicket_Click;

            // ============================================================
            // BUTTON – LỊCH SỬ VÉ (SECONDARY)
            // ============================================================
            btnHistoryTicketAdmin.AutoSize = true;
            btnHistoryTicketAdmin.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnHistoryTicketAdmin.Text = "Lịch sử vé";
            btnHistoryTicketAdmin.Padding = new Padding(24, 10, 24, 10);
            btnHistoryTicketAdmin.Click += btnHistoryTicketAdmin_Click;

            // ============================================================
            // CONTENT PANEL
            // ============================================================
            pnlContentTicket.Controls.Add(pnlFrmPassengerInfo);
            pnlContentTicket.Controls.Add(pnlTicketOps);
            pnlContentTicket.Controls.Add(pnlHistoryTicket);
            pnlContentTicket.Controls.Add(pnlBookingSearch);
            pnlContentTicket.Dock = DockStyle.Fill;
            pnlContentTicket.Name = "pnlContentTicket";

            pnlBookingSearch.Dock = DockStyle.Fill;
            pnlHistoryTicket.Dock = DockStyle.Fill;
            pnlTicketOps.Dock = DockStyle.Fill;
            pnlFrmPassengerInfo.Dock = DockStyle.Fill;

            // ============================================================
            // ROOT CONTROL
            // ============================================================
            Controls.Add(pnlContentTicket);
            Controls.Add(pnlHeaderTicket);
            BackColor = Color.WhiteSmoke;
            Name = "TicketControl";
            Size = new Size(1649, 810);
            Load += TicketControl_Load;

            pnlHeaderTicket.ResumeLayout(false);
            pnlHeaderTicket.PerformLayout();
            pnlContentTicket.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        // ============================================================
        // FIELD DECLARATIONS (NO INSTANTIATION HERE)
        // ============================================================
        private FlowLayoutPanel pnlHeaderTicket;

        private Button btnFrmPassengerInfoTiket;
        private Button btnOpsTicket;
        private Button btnHistoryTicketAdmin;

        private Panel pnlContentTicket;
        private Panel pnlBookingSearch;
        private Panel pnlHistoryTicket;
        private Panel pnlTicketOps;
        private Panel pnlFrmPassengerInfo;
    }
}
