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
            cboStatusTicket = new GUI.Components.Inputs.UnderlinedComboBox();
            txtBookingCodeTicket = new GUI.Components.Inputs.UnderlinedTextField();
            txtPassengerNameTicket = new GUI.Components.Inputs.UnderlinedTextField();
            txtFlightNumberTicket = new GUI.Components.Inputs.UnderlinedTextField();
            btnSearchOpsTicket = new GUI.Components.Buttons.PrimaryButton();
            dgvListOpsTicket = new GUI.Components.Tables.TableCustom();
            ((System.ComponentModel.ISupportInitialize)dgvListOpsTicket).BeginInit();
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
            // cboStatusTicket
            // 
            cboStatusTicket.BackColor = Color.Transparent;
            cboStatusTicket.LabelText = "Trạng thái";
            cboStatusTicket.Location = new Point(675, 174);
            cboStatusTicket.MinimumSize = new Size(140, 56);
            cboStatusTicket.Name = "cboStatusTicket";
            cboStatusTicket.SelectedIndex = -1;
            cboStatusTicket.SelectedItem = null;
            cboStatusTicket.SelectedText = "";
            cboStatusTicket.Size = new Size(188, 70);
            cboStatusTicket.TabIndex = 45;
            // 
            // txtBookingCodeTicket
            // 
            txtBookingCodeTicket.BackColor = Color.Transparent;
            txtBookingCodeTicket.FocusedLineThickness = 3;
            txtBookingCodeTicket.InheritParentBackColor = true;
            txtBookingCodeTicket.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtBookingCodeTicket.LabelText = "Mã đặt chỗ";
            txtBookingCodeTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtBookingCodeTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtBookingCodeTicket.LineThickness = 2;
            txtBookingCodeTicket.Location = new Point(28, 174);
            txtBookingCodeTicket.Name = "txtBookingCodeTicket";
            txtBookingCodeTicket.Padding = new Padding(0, 4, 0, 8);
            txtBookingCodeTicket.PasswordChar = '\0';
            txtBookingCodeTicket.PlaceholderText = "Placeholder";
            txtBookingCodeTicket.ReadOnly = false;
            txtBookingCodeTicket.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtBookingCodeTicket.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtBookingCodeTicket.Size = new Size(188, 63);
            txtBookingCodeTicket.TabIndex = 44;
            txtBookingCodeTicket.TextForeColor = Color.FromArgb(30, 30, 30);
            txtBookingCodeTicket.UnderlineSpacing = 2;
            txtBookingCodeTicket.UseSystemPasswordChar = false;
            // 
            // txtPassengerNameTicket
            // 
            txtPassengerNameTicket.BackColor = Color.Transparent;
            txtPassengerNameTicket.FocusedLineThickness = 3;
            txtPassengerNameTicket.InheritParentBackColor = true;
            txtPassengerNameTicket.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtPassengerNameTicket.LabelText = "Hành khách";
            txtPassengerNameTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtPassengerNameTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtPassengerNameTicket.LineThickness = 2;
            txtPassengerNameTicket.Location = new Point(444, 174);
            txtPassengerNameTicket.Name = "txtPassengerNameTicket";
            txtPassengerNameTicket.Padding = new Padding(0, 4, 0, 8);
            txtPassengerNameTicket.PasswordChar = '\0';
            txtPassengerNameTicket.PlaceholderText = "Placeholder";
            txtPassengerNameTicket.ReadOnly = false;
            txtPassengerNameTicket.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtPassengerNameTicket.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtPassengerNameTicket.Size = new Size(188, 63);
            txtPassengerNameTicket.TabIndex = 43;
            txtPassengerNameTicket.TextForeColor = Color.FromArgb(30, 30, 30);
            txtPassengerNameTicket.UnderlineSpacing = 2;
            txtPassengerNameTicket.UseSystemPasswordChar = false;
            // 
            // txtFlightNumberTicket
            // 
            txtFlightNumberTicket.BackColor = Color.Transparent;
            txtFlightNumberTicket.FocusedLineThickness = 3;
            txtFlightNumberTicket.InheritParentBackColor = true;
            txtFlightNumberTicket.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtFlightNumberTicket.LabelText = "Mã chuyến bay";
            txtFlightNumberTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtFlightNumberTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtFlightNumberTicket.LineThickness = 2;
            txtFlightNumberTicket.Location = new Point(245, 174);
            txtFlightNumberTicket.Name = "txtFlightNumberTicket";
            txtFlightNumberTicket.Padding = new Padding(0, 4, 0, 8);
            txtFlightNumberTicket.PasswordChar = '\0';
            txtFlightNumberTicket.PlaceholderText = "Placeholder";
            txtFlightNumberTicket.ReadOnly = false;
            txtFlightNumberTicket.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtFlightNumberTicket.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtFlightNumberTicket.Size = new Size(188, 63);
            txtFlightNumberTicket.TabIndex = 42;
            txtFlightNumberTicket.TextForeColor = Color.FromArgb(30, 30, 30);
            txtFlightNumberTicket.UnderlineSpacing = 2;
            txtFlightNumberTicket.UseSystemPasswordChar = false;
            // 
            // btnSearchOpsTicket
            // 
            btnSearchOpsTicket.AutoSize = true;
            btnSearchOpsTicket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSearchOpsTicket.BackColor = Color.FromArgb(155, 209, 243);
            btnSearchOpsTicket.BorderColor = Color.FromArgb(40, 40, 40);
            btnSearchOpsTicket.BorderThickness = 2;
            btnSearchOpsTicket.CornerRadius = 22;
            btnSearchOpsTicket.EnableHoverEffects = true;
            btnSearchOpsTicket.FlatAppearance.BorderSize = 0;
            btnSearchOpsTicket.FlatStyle = FlatStyle.Flat;
            btnSearchOpsTicket.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSearchOpsTicket.ForeColor = Color.White;
            btnSearchOpsTicket.HoverBackColor = Color.White;
            btnSearchOpsTicket.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnSearchOpsTicket.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnSearchOpsTicket.Icon = null;
            btnSearchOpsTicket.IconSize = new Size(22, 22);
            btnSearchOpsTicket.IconSpacing = 10;
            btnSearchOpsTicket.Location = new Point(889, 185);
            btnSearchOpsTicket.Name = "btnSearchOpsTicket";
            btnSearchOpsTicket.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnSearchOpsTicket.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnSearchOpsTicket.NormalForeColor = Color.White;
            btnSearchOpsTicket.Padding = new Padding(24, 10, 24, 10);
            btnSearchOpsTicket.PreferredMaxWidth = 0;
            btnSearchOpsTicket.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnSearchOpsTicket.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnSearchOpsTicket.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnSearchOpsTicket.Size = new Size(112, 52);
            btnSearchOpsTicket.TabIndex = 36;
            btnSearchOpsTicket.Text = "Lọc";
            btnSearchOpsTicket.TextAlign = ContentAlignment.MiddleLeft;
            btnSearchOpsTicket.UseVisualStyleBackColor = false;
            btnSearchOpsTicket.WordWrap = false;
            // 
            // dgvListOpsTicket
            // 
            dgvListOpsTicket.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(248, 250, 252);
            dgvListOpsTicket.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvListOpsTicket.BackgroundColor = Color.White;
            dgvListOpsTicket.BorderColor = Color.FromArgb(40, 40, 40);
            dgvListOpsTicket.BorderStyle = BorderStyle.None;
            dgvListOpsTicket.BorderThickness = 2;
            dgvListOpsTicket.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvListOpsTicket.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle5.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvListOpsTicket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvListOpsTicket.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvListOpsTicket.CornerRadius = 16;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle6.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle6.SelectionForeColor = Color.White;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvListOpsTicket.DefaultCellStyle = dataGridViewCellStyle6;
            dgvListOpsTicket.EnableHeadersVisualStyles = false;
            dgvListOpsTicket.GridColor = Color.FromArgb(230, 235, 240);
            dgvListOpsTicket.HeaderBackColor = Color.White;
            dgvListOpsTicket.HeaderForeColor = Color.FromArgb(126, 185, 232);
            dgvListOpsTicket.HoverBackColor = Color.FromArgb(232, 245, 255);
            dgvListOpsTicket.Location = new Point(18, 348);
            dgvListOpsTicket.MultiSelect = false;
            dgvListOpsTicket.Name = "dgvListOpsTicket";
            dgvListOpsTicket.RowAltBackColor = Color.FromArgb(248, 250, 252);
            dgvListOpsTicket.RowBackColor = Color.White;
            dgvListOpsTicket.RowForeColor = Color.FromArgb(33, 37, 41);
            dgvListOpsTicket.RowHeadersVisible = false;
            dgvListOpsTicket.RowHeadersWidth = 51;
            dgvListOpsTicket.RowTemplate.Height = 40;
            dgvListOpsTicket.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dgvListOpsTicket.SelectionForeColor = Color.White;
            dgvListOpsTicket.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvListOpsTicket.Size = new Size(1174, 333);
            dgvListOpsTicket.TabIndex = 35;
            // 
            // TicketOpsControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cboStatusTicket);
            Controls.Add(txtBookingCodeTicket);
            Controls.Add(txtPassengerNameTicket);
            Controls.Add(txtFlightNumberTicket);
            Controls.Add(btnSearchOpsTicket);
            Controls.Add(dgvListOpsTicket);
            Controls.Add(label1);
            Name = "TicketOpsControl";
            Size = new Size(1226, 774);
            ((System.ComponentModel.ISupportInitialize)dgvListOpsTicket).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Components.Inputs.UnderlinedComboBox cboStatusTicket;
        private Components.Inputs.UnderlinedTextField txtBookingCodeTicket;
        private Components.Inputs.UnderlinedTextField txtPassengerNameTicket;
        private Components.Inputs.UnderlinedTextField txtFlightNumberTicket;
        private Components.Buttons.PrimaryButton btnSearchOpsTicket;
        private Components.Tables.TableCustom dgvListOpsTicket;
    }
}
