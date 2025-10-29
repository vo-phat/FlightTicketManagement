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
            btnHistoryTicket = new Button();
            btnOpsTicket = new Button();
            btnCreateFindTicket = new Button();
            pnlContentTicket = new Panel();
            pnlTicketOps = new Panel();
            pnlHistoryTicket = new Panel();
            pnlBookingSearch = new Panel();
            pnlHeaderTicket.SuspendLayout();
            pnlContentTicket.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeaderTicket
            // 
            pnlHeaderTicket.Controls.Add(btnHistoryTicket);
            pnlHeaderTicket.Controls.Add(btnOpsTicket);
            pnlHeaderTicket.Controls.Add(btnCreateFindTicket);
            pnlHeaderTicket.Location = new Point(3, 3);
            pnlHeaderTicket.Name = "pnlHeaderTicket";
            pnlHeaderTicket.Size = new Size(1136, 83);
            pnlHeaderTicket.TabIndex = 0;
            // 
            // btnHistoryTicket
            // 
            btnHistoryTicket.Location = new Point(484, 21);
            btnHistoryTicket.Name = "btnHistoryTicket";
            btnHistoryTicket.Size = new Size(195, 29);
            btnHistoryTicket.TabIndex = 2;
            btnHistoryTicket.Text = "Lịch sử vé Admin";
            btnHistoryTicket.UseVisualStyleBackColor = true;
            btnHistoryTicket.Click += btnHistoryTicket_Click;
            // 
            // btnOpsTicket
            // 
            btnOpsTicket.Location = new Point(238, 21);
            btnOpsTicket.Name = "btnOpsTicket";
            btnOpsTicket.Size = new Size(195, 29);
            btnOpsTicket.TabIndex = 1;
            btnOpsTicket.Text = "Quản lý vé";
            btnOpsTicket.UseVisualStyleBackColor = true;
            btnOpsTicket.Click += btnOpsTicket_Click;
            // 
            // btnCreateFindTicket
            // 
            btnCreateFindTicket.Location = new Point(23, 21);
            btnCreateFindTicket.Name = "btnCreateFindTicket";
            btnCreateFindTicket.Size = new Size(176, 29);
            btnCreateFindTicket.TabIndex = 0;
            btnCreateFindTicket.Text = "Tạo/Tìm đặt chỗ";
            btnCreateFindTicket.UseVisualStyleBackColor = true;
            btnCreateFindTicket.Click += btnBookingAndSearchTicket_Click;
            // 
            // pnlContentTicket
            // 
            pnlContentTicket.Controls.Add(pnlTicketOps);
            pnlContentTicket.Controls.Add(pnlHistoryTicket);
            pnlContentTicket.Controls.Add(pnlBookingSearch);
            pnlContentTicket.Location = new Point(3, 92);
            pnlContentTicket.Name = "pnlContentTicket";
            pnlContentTicket.Size = new Size(1197, 665);
            pnlContentTicket.TabIndex = 1;
            // 
            // pnlTicketOps
            // 
            pnlTicketOps.Location = new Point(720, 162);
            pnlTicketOps.Name = "pnlTicketOps";
            pnlTicketOps.Size = new Size(250, 125);
            pnlTicketOps.TabIndex = 2;
            // 
            // pnlHistoryTicket
            // 
            pnlHistoryTicket.Location = new Point(429, 464);
            pnlHistoryTicket.Name = "pnlHistoryTicket";
            pnlHistoryTicket.Size = new Size(250, 125);
            pnlHistoryTicket.TabIndex = 1;
            // 
            // pnlBookingSearch
            // 
            pnlBookingSearch.Location = new Point(85, 254);
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
            Size = new Size(1203, 760);
            pnlHeaderTicket.ResumeLayout(false);
            pnlContentTicket.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeaderTicket;
        private Button btnOpsTicket;
        private Button btnCreateFindTicket;
        private Panel pnlContentTicket;
        private Panel pnlTicketOps;
        private Panel pnlHistoryTicket;
        private Panel pnlBookingSearch;
        private Button btnHistoryTicket;
    }
}
