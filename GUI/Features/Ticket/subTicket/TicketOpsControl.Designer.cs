namespace GUI.Features.Ticket.subTicket
{
    partial class TicketOpsControl
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
            cboStatusOpsTicket = new ComboBox();
            txtPassengerNameOpsTicket = new TextBox();
            label4 = new Label();
            txtFlightNumberOpsTicket = new TextBox();
            label3 = new Label();
            txtBookingCodeOpsTicket = new TextBox();
            label2 = new Label();
            label1 = new Label();
            btnSearchOpsTicket = new Button();
            label5 = new Label();
            dgvOpsTickets = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvOpsTickets).BeginInit();
            SuspendLayout();
            // 
            // cboStatusOpsTicket
            // 
            cboStatusOpsTicket.FormattingEnabled = true;
            cboStatusOpsTicket.Location = new Point(681, 139);
            cboStatusOpsTicket.Name = "cboStatusOpsTicket";
            cboStatusOpsTicket.Size = new Size(151, 28);
            cboStatusOpsTicket.TabIndex = 26;
            // 
            // txtPassengerNameOpsTicket
            // 
            txtPassengerNameOpsTicket.Location = new Point(487, 139);
            txtPassengerNameOpsTicket.Name = "txtPassengerNameOpsTicket";
            txtPassengerNameOpsTicket.Size = new Size(125, 27);
            txtPassengerNameOpsTicket.TabIndex = 24;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(487, 103);
            label4.Name = "label4";
            label4.Size = new Size(86, 20);
            label4.TabIndex = 23;
            label4.Text = "Hành khách";
            // 
            // txtFlightNumberOpsTicket
            // 
            txtFlightNumberOpsTicket.Location = new Point(286, 139);
            txtFlightNumberOpsTicket.Name = "txtFlightNumberOpsTicket";
            txtFlightNumberOpsTicket.Size = new Size(125, 27);
            txtFlightNumberOpsTicket.TabIndex = 22;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(286, 103);
            label3.Name = "label3";
            label3.Size = new Size(108, 20);
            label3.TabIndex = 21;
            label3.Text = "Mã chuyến bay";
            // 
            // txtBookingCodeOpsTicket
            // 
            txtBookingCodeOpsTicket.Location = new Point(29, 139);
            txtBookingCodeOpsTicket.Name = "txtBookingCodeOpsTicket";
            txtBookingCodeOpsTicket.Size = new Size(125, 27);
            txtBookingCodeOpsTicket.TabIndex = 20;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(29, 103);
            label2.Name = "label2";
            label2.Size = new Size(84, 20);
            label2.TabIndex = 19;
            label2.Text = "Mã đặt chỗ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 51);
            label1.Name = "label1";
            label1.Size = new Size(245, 20);
            label1.TabIndex = 18;
            label1.Text = "Quản lý vé(Check-in/ đổi trạng thái)";
            // 
            // btnSearchOpsTicket
            // 
            btnSearchOpsTicket.Location = new Point(856, 139);
            btnSearchOpsTicket.Name = "btnSearchOpsTicket";
            btnSearchOpsTicket.Size = new Size(94, 29);
            btnSearchOpsTicket.TabIndex = 17;
            btnSearchOpsTicket.Text = "Lọc";
            btnSearchOpsTicket.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(690, 103);
            label5.Name = "label5";
            label5.Size = new Size(75, 20);
            label5.TabIndex = 27;
            label5.Text = "Trạng thái";
            // 
            // dgvOpsTickets
            // 
            dgvOpsTickets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOpsTickets.Location = new Point(48, 288);
            dgvOpsTickets.Name = "dgvOpsTickets";
            dgvOpsTickets.RowHeadersWidth = 51;
            dgvOpsTickets.Size = new Size(1057, 188);
            dgvOpsTickets.TabIndex = 28;
            // 
            // TicketOpsControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgvOpsTickets);
            Controls.Add(label5);
            Controls.Add(cboStatusOpsTicket);
            Controls.Add(txtPassengerNameOpsTicket);
            Controls.Add(label4);
            Controls.Add(txtFlightNumberOpsTicket);
            Controls.Add(label3);
            Controls.Add(txtBookingCodeOpsTicket);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSearchOpsTicket);
            Name = "TicketOpsControl";
            Size = new Size(1226, 774);
            ((System.ComponentModel.ISupportInitialize)dgvOpsTickets).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cboStatusOpsTicket;
        private TextBox txtPassengerNameOpsTicket;
        private Label label4;
        private TextBox txtFlightNumberOpsTicket;
        private Label label3;
        private TextBox txtBookingCodeOpsTicket;
        private Label label2;
        private Label label1;
        private Button btnSearchOpsTicket;
        private Label label5;
        private DataGridView dgvOpsTickets;
    }
}
