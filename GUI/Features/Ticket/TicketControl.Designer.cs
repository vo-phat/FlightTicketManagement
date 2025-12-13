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

        private void InitializeComponent()
        {
            pnlHeaderTicket = new FlowLayoutPanel();
            btnFrmPassengerInfoTiket = new GUI.Components.Buttons.PrimaryButton();
            btnOpsTicket = new GUI.Components.Buttons.SecondaryButton();
            btnHistoryTicketAdmin = new GUI.Components.Buttons.SecondaryButton();
            pnlContentTicket = new Panel();
            pnlFrmPassengerInfo = new Panel();
            pnlTicketOps = new Panel();
            pnlHistoryTicket = new Panel();
            pnlBookingSearch = new Panel();
            pnlHeaderTicket.SuspendLayout();
            pnlContentTicket.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeaderTicket
            // 
            pnlHeaderTicket.AutoSize = true;
            pnlHeaderTicket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlHeaderTicket.BackColor = Color.White;
            pnlHeaderTicket.Controls.Add(btnFrmPassengerInfoTiket);
            pnlHeaderTicket.Controls.Add(btnOpsTicket);
            pnlHeaderTicket.Controls.Add(btnHistoryTicketAdmin);
            pnlHeaderTicket.Dock = DockStyle.Top;
            pnlHeaderTicket.Location = new Point(0, 0);
            pnlHeaderTicket.Name = "pnlHeaderTicket";
            pnlHeaderTicket.Padding = new Padding(24, 12, 0, 0);
            pnlHeaderTicket.Size = new Size(1649, 70);
            pnlHeaderTicket.TabIndex = 1;
            pnlHeaderTicket.WrapContents = false;
            // 
            // btnFrmPassengerInfoTiket
            // 
            btnFrmPassengerInfoTiket.AutoSize = true;
            btnFrmPassengerInfoTiket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnFrmPassengerInfoTiket.BackColor = Color.FromArgb(155, 209, 243);
            btnFrmPassengerInfoTiket.BorderColor = Color.FromArgb(40, 40, 40);
            btnFrmPassengerInfoTiket.BorderThickness = 2;
            btnFrmPassengerInfoTiket.CornerRadius = 22;
            btnFrmPassengerInfoTiket.EnableHoverEffects = true;
            btnFrmPassengerInfoTiket.FlatStyle = FlatStyle.Flat;
            btnFrmPassengerInfoTiket.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnFrmPassengerInfoTiket.ForeColor = Color.White;
            btnFrmPassengerInfoTiket.HoverBackColor = Color.White;
            btnFrmPassengerInfoTiket.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnFrmPassengerInfoTiket.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnFrmPassengerInfoTiket.Icon = null;
            btnFrmPassengerInfoTiket.IconSize = new Size(22, 22);
            btnFrmPassengerInfoTiket.IconSpacing = 10;
            btnFrmPassengerInfoTiket.Location = new Point(27, 15);
            btnFrmPassengerInfoTiket.Name = "btnFrmPassengerInfoTiket";
            btnFrmPassengerInfoTiket.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnFrmPassengerInfoTiket.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnFrmPassengerInfoTiket.NormalForeColor = Color.White;
            btnFrmPassengerInfoTiket.Padding = new Padding(24, 10, 24, 10);
            btnFrmPassengerInfoTiket.PreferredMaxWidth = 0;
            btnFrmPassengerInfoTiket.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnFrmPassengerInfoTiket.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnFrmPassengerInfoTiket.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnFrmPassengerInfoTiket.Size = new Size(142, 52);
            btnFrmPassengerInfoTiket.TabIndex = 0;
            btnFrmPassengerInfoTiket.Text = "Đặt vé";
            btnFrmPassengerInfoTiket.TextAlign = ContentAlignment.MiddleLeft;
            btnFrmPassengerInfoTiket.UseVisualStyleBackColor = false;
            btnFrmPassengerInfoTiket.WordWrap = false;
            btnFrmPassengerInfoTiket.Click += btnFrmPassengerInfoTiket_Click;
            // 
            // btnOpsTicket
            // 
            btnOpsTicket.AutoSize = true;
            btnOpsTicket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnOpsTicket.BackColor = Color.White;
            btnOpsTicket.BorderColor = Color.FromArgb(40, 40, 40);
            btnOpsTicket.BorderThickness = 2;
            btnOpsTicket.CornerRadius = 22;
            btnOpsTicket.EnableHoverEffects = true;
            btnOpsTicket.FlatStyle = FlatStyle.Flat;
            btnOpsTicket.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnOpsTicket.ForeColor = Color.FromArgb(155, 209, 243);
            btnOpsTicket.HoverBackColor = Color.FromArgb(155, 209, 243);
            btnOpsTicket.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnOpsTicket.HoverForeColor = Color.White;
            btnOpsTicket.Icon = null;
            btnOpsTicket.IconSize = new Size(22, 22);
            btnOpsTicket.IconSpacing = 10;
            btnOpsTicket.Location = new Point(175, 15);
            btnOpsTicket.Name = "btnOpsTicket";
            btnOpsTicket.NormalBackColor = Color.White;
            btnOpsTicket.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnOpsTicket.NormalForeColor = Color.FromArgb(155, 209, 243);
            btnOpsTicket.Padding = new Padding(24, 10, 24, 10);
            btnOpsTicket.PreferredMaxWidth = 0;
            btnOpsTicket.PressedBackColor = Color.FromArgb(120, 191, 239);
            btnOpsTicket.PressedBorderColor = Color.FromArgb(31, 111, 178);
            btnOpsTicket.PressedForeColor = Color.White;
            btnOpsTicket.Size = new Size(181, 52);
            btnOpsTicket.TabIndex = 1;
            btnOpsTicket.Text = "Quản lý vé";
            btnOpsTicket.TextAlign = ContentAlignment.MiddleLeft;
            btnOpsTicket.UseVisualStyleBackColor = false;
            btnOpsTicket.WordWrap = false;
            btnOpsTicket.Click += btnOpsTicket_Click;
            // 
            // btnHistoryTicketAdmin
            // 
            btnHistoryTicketAdmin.AutoSize = true;
            btnHistoryTicketAdmin.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnHistoryTicketAdmin.BackColor = Color.White;
            btnHistoryTicketAdmin.BorderColor = Color.FromArgb(40, 40, 40);
            btnHistoryTicketAdmin.BorderThickness = 2;
            btnHistoryTicketAdmin.CornerRadius = 22;
            btnHistoryTicketAdmin.EnableHoverEffects = true;
            btnHistoryTicketAdmin.FlatStyle = FlatStyle.Flat;
            btnHistoryTicketAdmin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnHistoryTicketAdmin.ForeColor = Color.FromArgb(155, 209, 243);
            btnHistoryTicketAdmin.HoverBackColor = Color.FromArgb(155, 209, 243);
            btnHistoryTicketAdmin.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnHistoryTicketAdmin.HoverForeColor = Color.White;
            btnHistoryTicketAdmin.Icon = null;
            btnHistoryTicketAdmin.IconSize = new Size(22, 22);
            btnHistoryTicketAdmin.IconSpacing = 10;
            btnHistoryTicketAdmin.Location = new Point(362, 15);
            btnHistoryTicketAdmin.Name = "btnHistoryTicketAdmin";
            btnHistoryTicketAdmin.NormalBackColor = Color.White;
            btnHistoryTicketAdmin.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnHistoryTicketAdmin.NormalForeColor = Color.FromArgb(155, 209, 243);
            btnHistoryTicketAdmin.Padding = new Padding(24, 10, 24, 10);
            btnHistoryTicketAdmin.PreferredMaxWidth = 0;
            btnHistoryTicketAdmin.PressedBackColor = Color.FromArgb(120, 191, 239);
            btnHistoryTicketAdmin.PressedBorderColor = Color.FromArgb(31, 111, 178);
            btnHistoryTicketAdmin.PressedForeColor = Color.White;
            btnHistoryTicketAdmin.Size = new Size(174, 52);
            btnHistoryTicketAdmin.TabIndex = 2;
            btnHistoryTicketAdmin.Text = "Lịch sử vé";
            btnHistoryTicketAdmin.TextAlign = ContentAlignment.MiddleLeft;
            btnHistoryTicketAdmin.UseVisualStyleBackColor = false;
            btnHistoryTicketAdmin.WordWrap = false;
            btnHistoryTicketAdmin.Click += btnHistoryTicketAdmin_Click;
            // 
            // pnlContentTicket
            // 
            pnlContentTicket.Controls.Add(pnlFrmPassengerInfo);
            pnlContentTicket.Controls.Add(pnlTicketOps);
            pnlContentTicket.Controls.Add(pnlHistoryTicket);
            pnlContentTicket.Controls.Add(pnlBookingSearch);
            pnlContentTicket.Dock = DockStyle.Fill;
            pnlContentTicket.Location = new Point(0, 70);
            pnlContentTicket.Name = "pnlContentTicket";
            pnlContentTicket.Size = new Size(1649, 740);
            pnlContentTicket.TabIndex = 0;
            // 
            // pnlFrmPassengerInfo
            // 
            pnlFrmPassengerInfo.Dock = DockStyle.Fill;
            pnlFrmPassengerInfo.Location = new Point(0, 0);
            pnlFrmPassengerInfo.Name = "pnlFrmPassengerInfo";
            pnlFrmPassengerInfo.Size = new Size(1649, 740);
            pnlFrmPassengerInfo.TabIndex = 0;
            // 
            // pnlTicketOps
            // 
            pnlTicketOps.Dock = DockStyle.Fill;
            pnlTicketOps.Location = new Point(0, 0);
            pnlTicketOps.Name = "pnlTicketOps";
            pnlTicketOps.Size = new Size(1649, 740);
            pnlTicketOps.TabIndex = 1;
            // 
            // pnlHistoryTicket
            // 
            pnlHistoryTicket.Dock = DockStyle.Fill;
            pnlHistoryTicket.Location = new Point(0, 0);
            pnlHistoryTicket.Name = "pnlHistoryTicket";
            pnlHistoryTicket.Size = new Size(1649, 740);
            pnlHistoryTicket.TabIndex = 2;
            // 
            // pnlBookingSearch
            // 
            pnlBookingSearch.Dock = DockStyle.Fill;
            pnlBookingSearch.Location = new Point(0, 0);
            pnlBookingSearch.Name = "pnlBookingSearch";
            pnlBookingSearch.Size = new Size(1649, 740);
            pnlBookingSearch.TabIndex = 3;
            // 
            // TicketControl
            // 
            BackColor = Color.WhiteSmoke;
            Controls.Add(pnlContentTicket);
            Controls.Add(pnlHeaderTicket);
            Name = "TicketControl";
            Size = new Size(1649, 810);
            Load += TicketControl_Load;
            pnlHeaderTicket.ResumeLayout(false);
            pnlHeaderTicket.PerformLayout();
            pnlContentTicket.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // ============================================================
        // FIELD DECLARATIONS (NO INSTANTIATION HERE)
        // ============================================================
        private FlowLayoutPanel pnlHeaderTicket;

        private Panel pnlContentTicket;
        private Panel pnlBookingSearch;
        private Panel pnlHistoryTicket;
        private Panel pnlTicketOps;
        private Panel pnlFrmPassengerInfo;
        private Components.Buttons.PrimaryButton btnFrmPassengerInfoTiket;
        private Components.Buttons.SecondaryButton btnOpsTicket;
        private Components.Buttons.SecondaryButton btnHistoryTicketAdmin;
    }
}
