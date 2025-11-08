namespace GUI.Features.Ticket
{
    partial class TicketControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlHeaderTicket = new Panel();
            btnFrmPassengerInfoTiket = new GUI.Components.Buttons.PrimaryButton();
            btnHistoryTicketAdmin = new GUI.Components.Buttons.PrimaryButton();
            btnCreateAndFindSeat = new GUI.Components.Buttons.PrimaryButton();
            btnOpsTicket = new GUI.Components.Buttons.PrimaryButton();
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
            pnlHeaderTicket.Controls.Add(btnFrmPassengerInfoTiket);
            pnlHeaderTicket.Controls.Add(btnHistoryTicketAdmin);
            pnlHeaderTicket.Controls.Add(btnCreateAndFindSeat);
            pnlHeaderTicket.Controls.Add(btnOpsTicket);
            pnlHeaderTicket.Location = new Point(3, 3);
            pnlHeaderTicket.Name = "pnlHeaderTicket";
            pnlHeaderTicket.Size = new Size(1316, 83);
            pnlHeaderTicket.TabIndex = 0;
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
            btnFrmPassengerInfoTiket.FlatAppearance.BorderSize = 0;
            btnFrmPassengerInfoTiket.FlatStyle = FlatStyle.Flat;
            btnFrmPassengerInfoTiket.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnFrmPassengerInfoTiket.ForeColor = Color.White;
            btnFrmPassengerInfoTiket.HoverBackColor = Color.White;
            btnFrmPassengerInfoTiket.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnFrmPassengerInfoTiket.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnFrmPassengerInfoTiket.Icon = null;
            btnFrmPassengerInfoTiket.IconSize = new Size(22, 22);
            btnFrmPassengerInfoTiket.IconSpacing = 10;
            btnFrmPassengerInfoTiket.Location = new Point(15, 19);
            btnFrmPassengerInfoTiket.Name = "btnFrmPassengerInfoTiket";
            btnFrmPassengerInfoTiket.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnFrmPassengerInfoTiket.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnFrmPassengerInfoTiket.NormalForeColor = Color.White;
            btnFrmPassengerInfoTiket.Padding = new Padding(24, 10, 24, 10);
            btnFrmPassengerInfoTiket.PreferredMaxWidth = 0;
            btnFrmPassengerInfoTiket.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnFrmPassengerInfoTiket.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnFrmPassengerInfoTiket.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnFrmPassengerInfoTiket.Size = new Size(287, 52);
            btnFrmPassengerInfoTiket.TabIndex = 9;
            btnFrmPassengerInfoTiket.Text = "Thông tin khách hàng";
            btnFrmPassengerInfoTiket.TextAlign = ContentAlignment.MiddleRight;
            btnFrmPassengerInfoTiket.UseVisualStyleBackColor = false;
            btnFrmPassengerInfoTiket.WordWrap = false;
            btnFrmPassengerInfoTiket.Click += btnFrmPassengerInfoTiket_Click;
            // 
            // btnHistoryTicketAdmin
            // 
            btnHistoryTicketAdmin.AutoSize = true;
            btnHistoryTicketAdmin.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnHistoryTicketAdmin.BackColor = Color.FromArgb(155, 209, 243);
            btnHistoryTicketAdmin.BorderColor = Color.FromArgb(40, 40, 40);
            btnHistoryTicketAdmin.BorderThickness = 2;
            btnHistoryTicketAdmin.CornerRadius = 22;
            btnHistoryTicketAdmin.EnableHoverEffects = true;
            btnHistoryTicketAdmin.FlatStyle = FlatStyle.Flat;
            btnHistoryTicketAdmin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnHistoryTicketAdmin.ForeColor = Color.White;
            btnHistoryTicketAdmin.HoverBackColor = Color.White;
            btnHistoryTicketAdmin.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnHistoryTicketAdmin.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnHistoryTicketAdmin.Icon = null;
            btnHistoryTicketAdmin.IconSize = new Size(22, 22);
            btnHistoryTicketAdmin.IconSpacing = 10;
            btnHistoryTicketAdmin.Location = new Point(882, 19);
            btnHistoryTicketAdmin.Name = "btnHistoryTicketAdmin";
            btnHistoryTicketAdmin.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnHistoryTicketAdmin.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnHistoryTicketAdmin.NormalForeColor = Color.White;
            btnHistoryTicketAdmin.Padding = new Padding(24, 10, 24, 10);
            btnHistoryTicketAdmin.PreferredMaxWidth = 0;
            btnHistoryTicketAdmin.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnHistoryTicketAdmin.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnHistoryTicketAdmin.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnHistoryTicketAdmin.Size = new Size(174, 52);
            btnHistoryTicketAdmin.TabIndex = 8;
            btnHistoryTicketAdmin.Text = "Lịch sử vé";
            btnHistoryTicketAdmin.TextAlign = ContentAlignment.MiddleLeft;
            btnHistoryTicketAdmin.UseVisualStyleBackColor = false;
            btnHistoryTicketAdmin.WordWrap = false;
            btnHistoryTicketAdmin.Click += btnHistoryTicketAdmin_Click;
            // 
            // btnCreateAndFindSeat
            // 
            btnCreateAndFindSeat.AutoSize = true;
            btnCreateAndFindSeat.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnCreateAndFindSeat.BackColor = Color.FromArgb(155, 209, 243);
            btnCreateAndFindSeat.BorderColor = Color.FromArgb(40, 40, 40);
            btnCreateAndFindSeat.BorderThickness = 2;
            btnCreateAndFindSeat.CornerRadius = 22;
            btnCreateAndFindSeat.EnableHoverEffects = true;
            btnCreateAndFindSeat.FlatAppearance.BorderSize = 0;
            btnCreateAndFindSeat.FlatStyle = FlatStyle.Flat;
            btnCreateAndFindSeat.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCreateAndFindSeat.ForeColor = Color.White;
            btnCreateAndFindSeat.HoverBackColor = Color.White;
            btnCreateAndFindSeat.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnCreateAndFindSeat.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnCreateAndFindSeat.Icon = null;
            btnCreateAndFindSeat.IconSize = new Size(22, 22);
            btnCreateAndFindSeat.IconSpacing = 10;
            btnCreateAndFindSeat.Location = new Point(359, 19);
            btnCreateAndFindSeat.Name = "btnCreateAndFindSeat";
            btnCreateAndFindSeat.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnCreateAndFindSeat.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnCreateAndFindSeat.NormalForeColor = Color.White;
            btnCreateAndFindSeat.Padding = new Padding(24, 10, 24, 10);
            btnCreateAndFindSeat.PreferredMaxWidth = 0;
            btnCreateAndFindSeat.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnCreateAndFindSeat.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnCreateAndFindSeat.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnCreateAndFindSeat.Size = new Size(105, 52);
            btnCreateAndFindSeat.TabIndex = 6;
            btnCreateAndFindSeat.Text = "Bỏ";
            btnCreateAndFindSeat.TextAlign = ContentAlignment.MiddleRight;
            btnCreateAndFindSeat.UseVisualStyleBackColor = false;
            btnCreateAndFindSeat.WordWrap = false;
            btnCreateAndFindSeat.Click += btnCreateAndFindSeat_Click;
            // 
            // btnOpsTicket
            // 
            btnOpsTicket.AutoSize = true;
            btnOpsTicket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnOpsTicket.BackColor = Color.FromArgb(155, 209, 243);
            btnOpsTicket.BorderColor = Color.FromArgb(40, 40, 40);
            btnOpsTicket.BorderThickness = 2;
            btnOpsTicket.CornerRadius = 22;
            btnOpsTicket.EnableHoverEffects = true;
            btnOpsTicket.FlatStyle = FlatStyle.Flat;
            btnOpsTicket.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnOpsTicket.ForeColor = Color.White;
            btnOpsTicket.HoverBackColor = Color.White;
            btnOpsTicket.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnOpsTicket.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnOpsTicket.Icon = null;
            btnOpsTicket.IconSize = new Size(22, 22);
            btnOpsTicket.IconSpacing = 10;
            btnOpsTicket.Location = new Point(650, 19);
            btnOpsTicket.Name = "btnOpsTicket";
            btnOpsTicket.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnOpsTicket.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnOpsTicket.NormalForeColor = Color.White;
            btnOpsTicket.Padding = new Padding(24, 10, 24, 10);
            btnOpsTicket.PreferredMaxWidth = 0;
            btnOpsTicket.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnOpsTicket.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnOpsTicket.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnOpsTicket.Size = new Size(181, 52);
            btnOpsTicket.TabIndex = 7;
            btnOpsTicket.Text = "Quản lý vé";
            btnOpsTicket.TextAlign = ContentAlignment.MiddleLeft;
            btnOpsTicket.UseVisualStyleBackColor = false;
            btnOpsTicket.WordWrap = false;
            btnOpsTicket.Click += btnOpsTicket_Click;
            // 
            // pnlContentTicket
            // 
            pnlContentTicket.Controls.Add(pnlFrmPassengerInfo);
            pnlContentTicket.Controls.Add(pnlTicketOps);
            pnlContentTicket.Controls.Add(pnlHistoryTicket);
            pnlContentTicket.Controls.Add(pnlBookingSearch);
            pnlContentTicket.Location = new Point(0, 128);
            pnlContentTicket.Name = "pnlContentTicket";
            pnlContentTicket.Size = new Size(1621, 588);
            pnlContentTicket.TabIndex = 1;
            // 
            // pnlFrmPassengerInfo
            // 
            pnlFrmPassengerInfo.Location = new Point(571, 421);
            pnlFrmPassengerInfo.Name = "pnlFrmPassengerInfo";
            pnlFrmPassengerInfo.Size = new Size(250, 125);
            pnlFrmPassengerInfo.TabIndex = 3;
            // 
            // pnlTicketOps
            // 
            pnlTicketOps.Location = new Point(931, 254);
            pnlTicketOps.Name = "pnlTicketOps";
            pnlTicketOps.Size = new Size(250, 125);
            pnlTicketOps.TabIndex = 2;
            // 
            // pnlHistoryTicket
            // 
            pnlHistoryTicket.Location = new Point(556, 254);
            pnlHistoryTicket.Name = "pnlHistoryTicket";
            pnlHistoryTicket.Size = new Size(250, 125);
            pnlHistoryTicket.TabIndex = 1;
            // 
            // pnlBookingSearch
            // 
            pnlBookingSearch.Location = new Point(193, 254);
            pnlBookingSearch.Name = "pnlBookingSearch";
            pnlBookingSearch.Size = new Size(250, 125);
            pnlBookingSearch.TabIndex = 0;
            // 
            // TicketControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlContentTicket);
            Controls.Add(pnlHeaderTicket);
            Name = "TicketControl";
            Size = new Size(1649, 810);
            Load += TicketControl_Load;
            pnlHeaderTicket.ResumeLayout(false);
            pnlHeaderTicket.PerformLayout();
            pnlContentTicket.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeaderTicket;
        private Button btnCreateFindTicket;
        private Panel pnlContentTicket;
        private Panel pnlTicketOps;
        private Panel pnlHistoryTicket;
        private Panel pnlBookingSearch;
        private Button btnHistoryTicket;
        //private Components.Buttons.PrimaryButton btnCreateFindTicket;
        //private Components.Buttons.PrimaryButton btnHistoryTicket;
        private Components.Buttons.PrimaryButton btnCreateAndFindSeat;
        private Components.Buttons.PrimaryButton btnOpsTicket;
        private Components.Buttons.PrimaryButton btnHistoryTicketAdmin;
        private Components.Buttons.PrimaryButton btnFrmPassengerInfoTiket;
        private Panel pnlFrmPassengerInfo;
    }
}
