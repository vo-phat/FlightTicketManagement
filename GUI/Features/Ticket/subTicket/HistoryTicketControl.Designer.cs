namespace GUI.Features.Ticket.subTicket
{
    partial class HistoryTicketControl
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dgvTickets = new GUI.Components.Tables.TableCustom();
            txtTicketNumber = new GUI.Components.Inputs.UnderlinedTextField();
            btnFilter = new GUI.Components.Buttons.PrimaryButton();
            cbStatus = new GUI.Components.Inputs.UnderlinedComboBox();
            dtTo = new GUI.Components.Inputs.DateTimePickerCustom();
            dtFrom = new GUI.Components.Inputs.DateTimePickerCustom();
            ((System.ComponentModel.ISupportInitialize)dgvTickets).BeginInit();
            SuspendLayout();
            // 
            // dgvTickets
            // 
            dgvTickets.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvTickets.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvTickets.BackgroundColor = Color.White;
            dgvTickets.BorderColor = Color.FromArgb(40, 40, 40);
            dgvTickets.BorderStyle = BorderStyle.None;
            dgvTickets.BorderThickness = 2;
            dgvTickets.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTickets.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvTickets.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvTickets.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTickets.CornerRadius = 16;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvTickets.DefaultCellStyle = dataGridViewCellStyle3;
            dgvTickets.EnableHeadersVisualStyles = false;
            dgvTickets.GridColor = Color.FromArgb(230, 235, 240);
            dgvTickets.HeaderBackColor = Color.White;
            dgvTickets.HeaderForeColor = Color.FromArgb(126, 185, 232);
            dgvTickets.HoverBackColor = Color.FromArgb(232, 245, 255);
            dgvTickets.Location = new Point(6, 111);
            dgvTickets.MultiSelect = false;
            dgvTickets.Name = "dgvTickets";
            dgvTickets.RowAltBackColor = Color.FromArgb(248, 250, 252);
            dgvTickets.RowBackColor = Color.White;
            dgvTickets.RowForeColor = Color.FromArgb(33, 37, 41);
            dgvTickets.RowHeadersVisible = false;
            dgvTickets.RowHeadersWidth = 51;
            dgvTickets.RowTemplate.Height = 40;
            dgvTickets.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dgvTickets.SelectionForeColor = Color.White;
            dgvTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTickets.Size = new Size(1213, 487);
            dgvTickets.TabIndex = 2;
            dgvTickets.CellContentClick += tableCustom1_CellContentClick;
            // 
            // txtTicketNumber
            // 
            txtTicketNumber.BackColor = Color.Transparent;
            txtTicketNumber.FocusedLineThickness = 3;
            txtTicketNumber.InheritParentBackColor = true;
            txtTicketNumber.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtTicketNumber.LabelText = "TiketNumber";
            txtTicketNumber.LineColor = Color.FromArgb(40, 40, 40);
            txtTicketNumber.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtTicketNumber.LineThickness = 2;
            txtTicketNumber.Location = new Point(27, 35);
            txtTicketNumber.Name = "txtTicketNumber";
            txtTicketNumber.Padding = new Padding(0, 4, 0, 8);
            txtTicketNumber.PasswordChar = '\0';
            txtTicketNumber.PlaceholderText = "Placeholder";
            txtTicketNumber.ReadOnly = false;
            txtTicketNumber.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtTicketNumber.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtTicketNumber.Size = new Size(188, 62);
            txtTicketNumber.TabIndex = 3;
            txtTicketNumber.TextForeColor = Color.FromArgb(30, 30, 30);
            txtTicketNumber.UnderlineSpacing = 2;
            txtTicketNumber.UseSystemPasswordChar = false;
            // 
            // btnFilter
            // 
            btnFilter.AutoSize = true;
            btnFilter.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnFilter.BackColor = Color.FromArgb(155, 209, 243);
            btnFilter.BorderColor = Color.FromArgb(40, 40, 40);
            btnFilter.BorderThickness = 2;
            btnFilter.CornerRadius = 22;
            btnFilter.EnableHoverEffects = true;
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.FlatStyle = FlatStyle.Flat;
            btnFilter.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnFilter.ForeColor = Color.White;
            btnFilter.HoverBackColor = Color.White;
            btnFilter.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnFilter.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnFilter.Icon = null;
            btnFilter.IconSize = new Size(22, 22);
            btnFilter.IconSpacing = 10;
            btnFilter.Location = new Point(892, 53);
            btnFilter.Name = "btnFilter";
            btnFilter.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnFilter.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnFilter.NormalForeColor = Color.White;
            btnFilter.Padding = new Padding(24, 10, 24, 10);
            btnFilter.PreferredMaxWidth = 0;
            btnFilter.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnFilter.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnFilter.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnFilter.Size = new Size(143, 52);
            btnFilter.TabIndex = 5;
            btnFilter.Text = "Search";
            btnFilter.TextAlign = ContentAlignment.MiddleLeft;
            btnFilter.UseVisualStyleBackColor = false;
            btnFilter.WordWrap = false;
            btnFilter.Click += btnFilter_click;
            // 
            // cbStatus
            // 
            cbStatus.BackColor = Color.Transparent;
            cbStatus.DataSource = null;
            cbStatus.DisplayMember = "";
            cbStatus.DropDownStyle = ComboBoxStyle.DropDown;
            cbStatus.LabelText = "status";
            cbStatus.Location = new Point(243, 35);
            cbStatus.MinimumSize = new Size(140, 56);
            cbStatus.Name = "cbStatus";
            cbStatus.SelectedIndex = -1;
            cbStatus.SelectedItem = null;
            cbStatus.SelectedText = "";
            cbStatus.SelectedValue = null;
            cbStatus.Size = new Size(188, 70);
            cbStatus.TabIndex = 6;
            cbStatus.ValueMember = "";
            // 
            // dtTo
            // 
            dtTo.BackColor = Color.Transparent;
            dtTo.CustomFormat = null;
            dtTo.EnableTime = false;
            dtTo.LabelText = "To";
            dtTo.Location = new Point(671, 35);
            dtTo.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtTo.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtTo.Name = "dtTo";
            dtTo.Padding = new Padding(0, 4, 0, 8);
            dtTo.PlaceholderText = "";
            dtTo.ShowUpDown = false;
            dtTo.ShowUpDownWhenTime = true;
            dtTo.Size = new Size(188, 78);
            dtTo.TabIndex = 7;
            dtTo.TimeFormat = "dd/MM/yyyy HH:mm";
            dtTo.Value = new DateTime(2025, 12, 9, 14, 5, 38, 18);
            // 
            // dtFrom
            // 
            dtFrom.BackColor = Color.Transparent;
            dtFrom.CustomFormat = null;
            dtFrom.EnableTime = false;
            dtFrom.LabelText = "from";
            dtFrom.Location = new Point(465, 35);
            dtFrom.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtFrom.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtFrom.Name = "dtFrom";
            dtFrom.Padding = new Padding(0, 4, 0, 8);
            dtFrom.PlaceholderText = "";
            dtFrom.ShowUpDown = false;
            dtFrom.ShowUpDownWhenTime = true;
            dtFrom.Size = new Size(188, 78);
            dtFrom.TabIndex = 8;
            dtFrom.TimeFormat = "dd/MM/yyyy HH:mm";
            dtFrom.Value = new DateTime(2025, 12, 9, 14, 5, 41, 134);
            // 
            // HistoryTicketControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dtFrom);
            Controls.Add(dtTo);
            Controls.Add(cbStatus);
            Controls.Add(btnFilter);
            Controls.Add(txtTicketNumber);
            Controls.Add(dgvTickets);
            Name = "HistoryTicketControl";
            Size = new Size(1219, 763);
            ((System.ComponentModel.ISupportInitialize)dgvTickets).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Components.Tables.TableCustom dgvTickets;
        private Components.Inputs.UnderlinedTextField txtTicketNumber;
        private Components.Inputs.UnderlinedComboBox cbStatus;
        private Components.Inputs.DateTimePickerCustom dtTo;
        private Components.Inputs.DateTimePickerCustom dtFrom;
        private Components.Buttons.PrimaryButton btnFilter;
    }
}
