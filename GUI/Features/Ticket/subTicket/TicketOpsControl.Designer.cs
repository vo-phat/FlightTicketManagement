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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            label1 = new Label();
            cbFilterTicketStatus = new GUI.Components.Inputs.UnderlinedComboBox();
            txtFilterBookingCode = new GUI.Components.Inputs.UnderlinedTextField();
            txtFilterFlightCode = new GUI.Components.Inputs.UnderlinedTextField();
            btnFilterTickets = new GUI.Components.Buttons.PrimaryButton();
            dgvListFilerTickets = new GUI.Components.Tables.TableCustom();
            mySqlCommand1 = new MySqlConnector.MySqlCommand();
            dtpFilterFlightDate = new GUI.Components.Inputs.DateTimePickerCustom();
            txtFilterPhoneNumber = new GUI.Components.Inputs.UnderlinedTextField();
            ((System.ComponentModel.ISupportInitialize)dgvListFilerTickets).BeginInit();
            SuspendLayout();
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
            // cbFilterTicketStatus
            // 
            cbFilterTicketStatus.BackColor = Color.Transparent;
            cbFilterTicketStatus.LabelText = "Trạng thái";
            cbFilterTicketStatus.Location = new Point(530, 167);
            cbFilterTicketStatus.MinimumSize = new Size(140, 56);
            cbFilterTicketStatus.Name = "cbFilterTicketStatus";
            cbFilterTicketStatus.SelectedIndex = -1;
            cbFilterTicketStatus.SelectedItem = null;
            cbFilterTicketStatus.SelectedText = "";
            cbFilterTicketStatus.Size = new Size(188, 70);
            cbFilterTicketStatus.TabIndex = 45;
            // 
            // txtFilterBookingCode
            // 
            txtFilterBookingCode.BackColor = Color.Transparent;
            txtFilterBookingCode.FocusedLineThickness = 3;
            txtFilterBookingCode.InheritParentBackColor = true;
            txtFilterBookingCode.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtFilterBookingCode.LabelText = "Mã đặt chỗ";
            txtFilterBookingCode.LineColor = Color.FromArgb(40, 40, 40);
            txtFilterBookingCode.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtFilterBookingCode.LineThickness = 2;
            txtFilterBookingCode.Location = new Point(28, 174);
            txtFilterBookingCode.Name = "txtFilterBookingCode";
            txtFilterBookingCode.Padding = new Padding(0, 4, 0, 8);
            txtFilterBookingCode.PasswordChar = '\0';
            txtFilterBookingCode.PlaceholderText = "Placeholder";
            txtFilterBookingCode.ReadOnly = false;
            txtFilterBookingCode.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtFilterBookingCode.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtFilterBookingCode.Size = new Size(188, 63);
            txtFilterBookingCode.TabIndex = 44;
            txtFilterBookingCode.TextForeColor = Color.FromArgb(30, 30, 30);
            txtFilterBookingCode.UnderlineSpacing = 2;
            txtFilterBookingCode.UseSystemPasswordChar = false;
            // 
            // txtFilterFlightCode
            // 
            txtFilterFlightCode.BackColor = Color.Transparent;
            txtFilterFlightCode.FocusedLineThickness = 3;
            txtFilterFlightCode.InheritParentBackColor = true;
            txtFilterFlightCode.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtFilterFlightCode.LabelText = "Mã chuyến bay";
            txtFilterFlightCode.LineColor = Color.FromArgb(40, 40, 40);
            txtFilterFlightCode.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtFilterFlightCode.LineThickness = 2;
            txtFilterFlightCode.Location = new Point(290, 174);
            txtFilterFlightCode.Name = "txtFilterFlightCode";
            txtFilterFlightCode.Padding = new Padding(0, 4, 0, 8);
            txtFilterFlightCode.PasswordChar = '\0';
            txtFilterFlightCode.PlaceholderText = "Placeholder";
            txtFilterFlightCode.ReadOnly = false;
            txtFilterFlightCode.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtFilterFlightCode.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtFilterFlightCode.Size = new Size(188, 63);
            txtFilterFlightCode.TabIndex = 42;
            txtFilterFlightCode.TextForeColor = Color.FromArgb(30, 30, 30);
            txtFilterFlightCode.UnderlineSpacing = 2;
            txtFilterFlightCode.UseSystemPasswordChar = false;
            // 
            // btnFilterTickets
            // 
            btnFilterTickets.AutoSize = true;
            btnFilterTickets.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnFilterTickets.BackColor = Color.FromArgb(155, 209, 243);
            btnFilterTickets.BorderColor = Color.FromArgb(40, 40, 40);
            btnFilterTickets.BorderThickness = 2;
            btnFilterTickets.CornerRadius = 22;
            btnFilterTickets.EnableHoverEffects = true;
            btnFilterTickets.FlatAppearance.BorderSize = 0;
            btnFilterTickets.FlatStyle = FlatStyle.Flat;
            btnFilterTickets.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnFilterTickets.ForeColor = Color.White;
            btnFilterTickets.HoverBackColor = Color.White;
            btnFilterTickets.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnFilterTickets.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnFilterTickets.Icon = null;
            btnFilterTickets.IconSize = new Size(22, 22);
            btnFilterTickets.IconSpacing = 10;
            btnFilterTickets.Location = new Point(557, 272);
            btnFilterTickets.Name = "btnFilterTickets";
            btnFilterTickets.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnFilterTickets.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnFilterTickets.NormalForeColor = Color.White;
            btnFilterTickets.Padding = new Padding(24, 10, 24, 10);
            btnFilterTickets.PreferredMaxWidth = 0;
            btnFilterTickets.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnFilterTickets.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnFilterTickets.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnFilterTickets.Size = new Size(112, 52);
            btnFilterTickets.TabIndex = 36;
            btnFilterTickets.Text = "Lọc";
            btnFilterTickets.TextAlign = ContentAlignment.MiddleLeft;
            btnFilterTickets.UseVisualStyleBackColor = false;
            btnFilterTickets.WordWrap = false;
            btnFilterTickets.Click += btnSearchOpsTicket_Click;
            // 
            // dgvListFilerTickets
            // 
            dgvListFilerTickets.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(248, 250, 252);
            dgvListFilerTickets.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvListFilerTickets.BackgroundColor = Color.White;
            dgvListFilerTickets.BorderColor = Color.FromArgb(40, 40, 40);
            dgvListFilerTickets.BorderStyle = BorderStyle.None;
            dgvListFilerTickets.BorderThickness = 2;
            dgvListFilerTickets.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvListFilerTickets.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle5.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvListFilerTickets.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvListFilerTickets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvListFilerTickets.CornerRadius = 16;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle6.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle6.SelectionForeColor = Color.White;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvListFilerTickets.DefaultCellStyle = dataGridViewCellStyle6;
            dgvListFilerTickets.EnableHeadersVisualStyles = false;
            dgvListFilerTickets.GridColor = Color.FromArgb(230, 235, 240);
            dgvListFilerTickets.HeaderBackColor = Color.White;
            dgvListFilerTickets.HeaderForeColor = Color.FromArgb(126, 185, 232);
            dgvListFilerTickets.HoverBackColor = Color.FromArgb(232, 245, 255);
            dgvListFilerTickets.Location = new Point(28, 355);
            dgvListFilerTickets.MultiSelect = false;
            dgvListFilerTickets.Name = "dgvListFilerTickets";
            dgvListFilerTickets.RowAltBackColor = Color.FromArgb(248, 250, 252);
            dgvListFilerTickets.RowBackColor = Color.White;
            dgvListFilerTickets.RowForeColor = Color.FromArgb(33, 37, 41);
            dgvListFilerTickets.RowHeadersVisible = false;
            dgvListFilerTickets.RowHeadersWidth = 51;
            dgvListFilerTickets.RowTemplate.Height = 40;
            dgvListFilerTickets.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dgvListFilerTickets.SelectionForeColor = Color.White;
            dgvListFilerTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvListFilerTickets.Size = new Size(1550, 312);
            dgvListFilerTickets.TabIndex = 35;
            // 
            // mySqlCommand1
            // 
            mySqlCommand1.CommandTimeout = 0;
            mySqlCommand1.Connection = null;
            mySqlCommand1.Transaction = null;
            mySqlCommand1.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // dtpFilterFlightDate
            // 
            dtpFilterFlightDate.BackColor = Color.Transparent;
            dtpFilterFlightDate.CustomFormat = null;
            dtpFilterFlightDate.LabelText = "Ngày bay";
            dtpFilterFlightDate.Location = new Point(28, 261);
            dtpFilterFlightDate.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpFilterFlightDate.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpFilterFlightDate.Name = "dtpFilterFlightDate";
            dtpFilterFlightDate.Padding = new Padding(0, 4, 0, 8);
            dtpFilterFlightDate.PlaceholderText = "";
            dtpFilterFlightDate.Size = new Size(222, 59);
            dtpFilterFlightDate.TabIndex = 47;
            dtpFilterFlightDate.Value = new DateTime(2025, 10, 30, 10, 46, 34, 110);
            // 
            // txtFilterPhoneNumber
            // 
            txtFilterPhoneNumber.BackColor = Color.Transparent;
            txtFilterPhoneNumber.FocusedLineThickness = 3;
            txtFilterPhoneNumber.InheritParentBackColor = true;
            txtFilterPhoneNumber.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtFilterPhoneNumber.LabelText = "Số điện thoại";
            txtFilterPhoneNumber.LineColor = Color.FromArgb(40, 40, 40);
            txtFilterPhoneNumber.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtFilterPhoneNumber.LineThickness = 2;
            txtFilterPhoneNumber.Location = new Point(299, 261);
            txtFilterPhoneNumber.Name = "txtFilterPhoneNumber";
            txtFilterPhoneNumber.Padding = new Padding(0, 4, 0, 8);
            txtFilterPhoneNumber.PasswordChar = '\0';
            txtFilterPhoneNumber.PlaceholderText = "Placeholder";
            txtFilterPhoneNumber.ReadOnly = false;
            txtFilterPhoneNumber.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtFilterPhoneNumber.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtFilterPhoneNumber.Size = new Size(188, 63);
            txtFilterPhoneNumber.TabIndex = 49;
            txtFilterPhoneNumber.TextForeColor = Color.FromArgb(30, 30, 30);
            txtFilterPhoneNumber.UnderlineSpacing = 2;
            txtFilterPhoneNumber.UseSystemPasswordChar = false;
            txtFilterPhoneNumber.Load += underlinedTextField1_Load;
            // 
            // TicketOpsControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtFilterPhoneNumber);
            Controls.Add(dtpFilterFlightDate);
            Controls.Add(cbFilterTicketStatus);
            Controls.Add(txtFilterBookingCode);
            Controls.Add(txtFilterFlightCode);
            Controls.Add(btnFilterTickets);
            Controls.Add(dgvListFilerTickets);
            Controls.Add(label1);
            Name = "TicketOpsControl";
            Size = new Size(1628, 955);
            ((System.ComponentModel.ISupportInitialize)dgvListFilerTickets).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Components.Inputs.UnderlinedComboBox cbFilterTicketStatus;
        private Components.Inputs.UnderlinedTextField txtFilterBookingCode;
        private Components.Inputs.UnderlinedTextField txtFilterFlightCode;
        private Components.Buttons.PrimaryButton btnFilterTickets;
        private Components.Tables.TableCustom dgvListFilerTickets;
        private MySqlConnector.MySqlCommand mySqlCommand1;
        private Components.Inputs.DateTimePickerCustom dtpFilterFlightDate;
        private Components.Inputs.UnderlinedTextField txtFilterPhoneNumber;
    }
}
