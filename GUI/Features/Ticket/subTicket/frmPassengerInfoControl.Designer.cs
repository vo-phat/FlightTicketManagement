namespace GUI.Features.Ticket.subTicket
{
    partial class frmPassengerInfoControl
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
            cboNationalityTicket = new GUI.Components.Inputs.UnderlinedComboBox();
            txtPassportNumberTicket = new GUI.Components.Inputs.UnderlinedTextField();
            txtFullNameTicket = new GUI.Components.Inputs.UnderlinedTextField();
            txtEmailTicket = new GUI.Components.Inputs.UnderlinedTextField();
            txtPhoneNumberTicket = new GUI.Components.Inputs.UnderlinedTextField();
            btnAddPassengerTicket = new GUI.Components.Buttons.PrimaryButton();
            dtpFlightDateTicket = new GUI.Components.Inputs.DateTimePickerCustom();
            txtSeatTicket = new GUI.Components.Inputs.UnderlinedTextField();
            btnSelectSeatTicket = new GUI.Components.Buttons.PrimaryButton();
            dgvPassengerListTicket = new GUI.Components.Tables.TableCustom();
            txtAccountIdTicket = new GUI.Components.Inputs.UnderlinedTextField();
            dtpDateOfBirthTicket = new GUI.Components.Inputs.DateTimePickerCustom();
            cboBaggageTicket = new GUI.Components.Inputs.UnderlinedComboBox();
            txtNoteBaggage = new GUI.Components.Inputs.UnderlinedTextField();
            ((System.ComponentModel.ISupportInitialize)dgvPassengerListTicket).BeginInit();
            SuspendLayout();
            // 
            // cboNationalityTicket
            // 
            cboNationalityTicket.BackColor = Color.Transparent;
            cboNationalityTicket.LabelText = "Quốc gia";
            cboNationalityTicket.Location = new Point(322, 128);
            cboNationalityTicket.MinimumSize = new Size(140, 56);
            cboNationalityTicket.Name = "cboNationalityTicket";
            cboNationalityTicket.SelectedIndex = -1;
            cboNationalityTicket.SelectedItem = null;
            cboNationalityTicket.SelectedText = "";
            cboNationalityTicket.Size = new Size(188, 70);
            cboNationalityTicket.TabIndex = 45;
            // 
            // txtPassportNumberTicket
            // 
            txtPassportNumberTicket.BackColor = Color.Transparent;
            txtPassportNumberTicket.FocusedLineThickness = 3;
            txtPassportNumberTicket.InheritParentBackColor = true;
            txtPassportNumberTicket.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtPassportNumberTicket.LabelText = "Hộ chiếu";
            txtPassportNumberTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtPassportNumberTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtPassportNumberTicket.LineThickness = 2;
            txtPassportNumberTicket.Location = new Point(59, 222);
            txtPassportNumberTicket.Name = "txtPassportNumberTicket";
            txtPassportNumberTicket.Padding = new Padding(0, 4, 0, 8);
            txtPassportNumberTicket.PasswordChar = '\0';
            txtPassportNumberTicket.PlaceholderText = "Placeholder";
            txtPassportNumberTicket.ReadOnly = false;
            txtPassportNumberTicket.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtPassportNumberTicket.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtPassportNumberTicket.Size = new Size(188, 63);
            txtPassportNumberTicket.TabIndex = 44;
            txtPassportNumberTicket.TextForeColor = Color.FromArgb(30, 30, 30);
            txtPassportNumberTicket.UnderlineSpacing = 2;
            txtPassportNumberTicket.UseSystemPasswordChar = false;
            // 
            // txtFullNameTicket
            // 
            txtFullNameTicket.BackColor = Color.Transparent;
            txtFullNameTicket.FocusedLineThickness = 3;
            txtFullNameTicket.InheritParentBackColor = true;
            txtFullNameTicket.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtFullNameTicket.LabelText = "Họ và tên";
            txtFullNameTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtFullNameTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtFullNameTicket.LineThickness = 2;
            txtFullNameTicket.Location = new Point(59, 38);
            txtFullNameTicket.Name = "txtFullNameTicket";
            txtFullNameTicket.Padding = new Padding(0, 4, 0, 8);
            txtFullNameTicket.PasswordChar = '\0';
            txtFullNameTicket.PlaceholderText = "Placeholder";
            txtFullNameTicket.ReadOnly = false;
            txtFullNameTicket.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtFullNameTicket.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtFullNameTicket.Size = new Size(188, 63);
            txtFullNameTicket.TabIndex = 42;
            txtFullNameTicket.TextForeColor = Color.FromArgb(30, 30, 30);
            txtFullNameTicket.UnderlineSpacing = 2;
            txtFullNameTicket.UseSystemPasswordChar = false;
            // 
            // txtEmailTicket
            // 
            txtEmailTicket.BackColor = Color.Transparent;
            txtEmailTicket.FocusedLineThickness = 3;
            txtEmailTicket.InheritParentBackColor = true;
            txtEmailTicket.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtEmailTicket.LabelText = "Email hành khách";
            txtEmailTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtEmailTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtEmailTicket.LineThickness = 2;
            txtEmailTicket.Location = new Point(59, 319);
            txtEmailTicket.Name = "txtEmailTicket";
            txtEmailTicket.Padding = new Padding(0, 4, 0, 8);
            txtEmailTicket.PasswordChar = '\0';
            txtEmailTicket.PlaceholderText = "Placeholder";
            txtEmailTicket.ReadOnly = false;
            txtEmailTicket.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtEmailTicket.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtEmailTicket.Size = new Size(188, 63);
            txtEmailTicket.TabIndex = 41;
            txtEmailTicket.TextForeColor = Color.FromArgb(30, 30, 30);
            txtEmailTicket.UnderlineSpacing = 2;
            txtEmailTicket.UseSystemPasswordChar = false;
            // 
            // txtPhoneNumberTicket
            // 
            txtPhoneNumberTicket.BackColor = Color.Transparent;
            txtPhoneNumberTicket.FocusedLineThickness = 3;
            txtPhoneNumberTicket.InheritParentBackColor = true;
            txtPhoneNumberTicket.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtPhoneNumberTicket.LabelText = "SDT hành khách";
            txtPhoneNumberTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtPhoneNumberTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtPhoneNumberTicket.LineThickness = 2;
            txtPhoneNumberTicket.Location = new Point(322, 38);
            txtPhoneNumberTicket.Name = "txtPhoneNumberTicket";
            txtPhoneNumberTicket.Padding = new Padding(0, 4, 0, 8);
            txtPhoneNumberTicket.PasswordChar = '\0';
            txtPhoneNumberTicket.PlaceholderText = "Placeholder";
            txtPhoneNumberTicket.ReadOnly = false;
            txtPhoneNumberTicket.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtPhoneNumberTicket.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtPhoneNumberTicket.Size = new Size(188, 63);
            txtPhoneNumberTicket.TabIndex = 40;
            txtPhoneNumberTicket.TextForeColor = Color.FromArgb(30, 30, 30);
            txtPhoneNumberTicket.UnderlineSpacing = 2;
            txtPhoneNumberTicket.UseSystemPasswordChar = false;
            // 
            // btnAddPassengerTicket
            // 
            btnAddPassengerTicket.AutoSize = true;
            btnAddPassengerTicket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnAddPassengerTicket.BackColor = Color.FromArgb(155, 209, 243);
            btnAddPassengerTicket.BorderColor = Color.FromArgb(40, 40, 40);
            btnAddPassengerTicket.BorderThickness = 2;
            btnAddPassengerTicket.CornerRadius = 22;
            btnAddPassengerTicket.EnableHoverEffects = true;
            btnAddPassengerTicket.FlatAppearance.BorderSize = 0;
            btnAddPassengerTicket.FlatStyle = FlatStyle.Flat;
            btnAddPassengerTicket.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnAddPassengerTicket.ForeColor = Color.White;
            btnAddPassengerTicket.HoverBackColor = Color.White;
            btnAddPassengerTicket.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnAddPassengerTicket.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnAddPassengerTicket.Icon = null;
            btnAddPassengerTicket.IconSize = new Size(22, 22);
            btnAddPassengerTicket.IconSpacing = 10;
            btnAddPassengerTicket.Location = new Point(799, 330);
            btnAddPassengerTicket.Name = "btnAddPassengerTicket";
            btnAddPassengerTicket.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnAddPassengerTicket.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnAddPassengerTicket.NormalForeColor = Color.White;
            btnAddPassengerTicket.Padding = new Padding(24, 10, 24, 10);
            btnAddPassengerTicket.PreferredMaxWidth = 0;
            btnAddPassengerTicket.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnAddPassengerTicket.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnAddPassengerTicket.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnAddPassengerTicket.Size = new Size(131, 52);
            btnAddPassengerTicket.TabIndex = 39;
            btnAddPassengerTicket.Text = "Nhập";
            btnAddPassengerTicket.TextAlign = ContentAlignment.MiddleLeft;
            btnAddPassengerTicket.UseVisualStyleBackColor = false;
            btnAddPassengerTicket.WordWrap = false;
            btnAddPassengerTicket.Click += btnAddPassengerTicket_Click;
            // 
            // dtpFlightDateTicket
            // 
            dtpFlightDateTicket.BackColor = Color.Transparent;
            dtpFlightDateTicket.CustomFormat = null;
            dtpFlightDateTicket.LabelText = "Ngày bay";
            dtpFlightDateTicket.Location = new Point(322, 222);
            dtpFlightDateTicket.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpFlightDateTicket.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpFlightDateTicket.Name = "dtpFlightDateTicket";
            dtpFlightDateTicket.Padding = new Padding(0, 4, 0, 8);
            dtpFlightDateTicket.PlaceholderText = "";
            dtpFlightDateTicket.Size = new Size(222, 59);
            dtpFlightDateTicket.TabIndex = 46;
            dtpFlightDateTicket.Value = new DateTime(2025, 10, 30, 10, 46, 34, 110);
            // 
            // txtSeatTicket
            // 
            txtSeatTicket.BackColor = Color.Transparent;
            txtSeatTicket.FocusedLineThickness = 3;
            txtSeatTicket.InheritParentBackColor = true;
            txtSeatTicket.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtSeatTicket.LabelText = "Ghế";
            txtSeatTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtSeatTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtSeatTicket.LineThickness = 2;
            txtSeatTicket.Location = new Point(322, 319);
            txtSeatTicket.Name = "txtSeatTicket";
            txtSeatTicket.Padding = new Padding(0, 4, 0, 8);
            txtSeatTicket.PasswordChar = '\0';
            txtSeatTicket.PlaceholderText = "Placeholder";
            txtSeatTicket.ReadOnly = false;
            txtSeatTicket.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtSeatTicket.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtSeatTicket.Size = new Size(188, 63);
            txtSeatTicket.TabIndex = 47;
            txtSeatTicket.TextForeColor = Color.FromArgb(30, 30, 30);
            txtSeatTicket.UnderlineSpacing = 2;
            txtSeatTicket.UseSystemPasswordChar = false;
            // 
            // btnSelectSeatTicket
            // 
            btnSelectSeatTicket.AutoSize = true;
            btnSelectSeatTicket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSelectSeatTicket.BackColor = Color.FromArgb(155, 209, 243);
            btnSelectSeatTicket.BorderColor = Color.FromArgb(40, 40, 40);
            btnSelectSeatTicket.BorderThickness = 2;
            btnSelectSeatTicket.CornerRadius = 22;
            btnSelectSeatTicket.EnableHoverEffects = true;
            btnSelectSeatTicket.FlatAppearance.BorderSize = 0;
            btnSelectSeatTicket.FlatStyle = FlatStyle.Flat;
            btnSelectSeatTicket.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSelectSeatTicket.ForeColor = Color.White;
            btnSelectSeatTicket.HoverBackColor = Color.White;
            btnSelectSeatTicket.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnSelectSeatTicket.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnSelectSeatTicket.Icon = null;
            btnSelectSeatTicket.IconSize = new Size(22, 22);
            btnSelectSeatTicket.IconSpacing = 10;
            btnSelectSeatTicket.Location = new Point(563, 330);
            btnSelectSeatTicket.Name = "btnSelectSeatTicket";
            btnSelectSeatTicket.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnSelectSeatTicket.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnSelectSeatTicket.NormalForeColor = Color.White;
            btnSelectSeatTicket.Padding = new Padding(24, 10, 24, 10);
            btnSelectSeatTicket.PreferredMaxWidth = 0;
            btnSelectSeatTicket.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnSelectSeatTicket.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnSelectSeatTicket.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnSelectSeatTicket.Size = new Size(169, 52);
            btnSelectSeatTicket.TabIndex = 48;
            btnSelectSeatTicket.Text = "Chọn ghế";
            btnSelectSeatTicket.TextAlign = ContentAlignment.MiddleLeft;
            btnSelectSeatTicket.UseVisualStyleBackColor = false;
            btnSelectSeatTicket.WordWrap = false;
            btnSelectSeatTicket.Click += btnSelectSeatTicket_Click;
            // 
            // dgvPassengerListTicket
            // 
            dgvPassengerListTicket.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(248, 250, 252);
            dgvPassengerListTicket.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvPassengerListTicket.BackgroundColor = Color.White;
            dgvPassengerListTicket.BorderColor = Color.FromArgb(40, 40, 40);
            dgvPassengerListTicket.BorderStyle = BorderStyle.None;
            dgvPassengerListTicket.BorderThickness = 2;
            dgvPassengerListTicket.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPassengerListTicket.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle5.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvPassengerListTicket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvPassengerListTicket.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPassengerListTicket.CornerRadius = 16;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle6.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle6.SelectionForeColor = Color.White;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvPassengerListTicket.DefaultCellStyle = dataGridViewCellStyle6;
            dgvPassengerListTicket.EnableHeadersVisualStyles = false;
            dgvPassengerListTicket.GridColor = Color.FromArgb(230, 235, 240);
            dgvPassengerListTicket.HeaderBackColor = Color.White;
            dgvPassengerListTicket.HeaderForeColor = Color.FromArgb(126, 185, 232);
            dgvPassengerListTicket.HoverBackColor = Color.FromArgb(232, 245, 255);
            dgvPassengerListTicket.Location = new Point(40, 424);
            dgvPassengerListTicket.MultiSelect = false;
            dgvPassengerListTicket.Name = "dgvPassengerListTicket";
            dgvPassengerListTicket.RowAltBackColor = Color.FromArgb(248, 250, 252);
            dgvPassengerListTicket.RowBackColor = Color.White;
            dgvPassengerListTicket.RowForeColor = Color.FromArgb(33, 37, 41);
            dgvPassengerListTicket.RowHeadersVisible = false;
            dgvPassengerListTicket.RowHeadersWidth = 51;
            dgvPassengerListTicket.RowTemplate.Height = 40;
            dgvPassengerListTicket.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dgvPassengerListTicket.SelectionForeColor = Color.White;
            dgvPassengerListTicket.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPassengerListTicket.Size = new Size(1190, 259);
            dgvPassengerListTicket.TabIndex = 49;
            dgvPassengerListTicket.CellContentClick += dgvPassengerListTicket_CellContentClick;
            // 
            // txtAccountIdTicket
            // 
            txtAccountIdTicket.BackColor = Color.Transparent;
            txtAccountIdTicket.FocusedLineThickness = 3;
            txtAccountIdTicket.InheritParentBackColor = true;
            txtAccountIdTicket.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtAccountIdTicket.LabelText = "ID của tk(demo hoạt động)";
            txtAccountIdTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtAccountIdTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtAccountIdTicket.LineThickness = 2;
            txtAccountIdTicket.Location = new Point(810, 124);
            txtAccountIdTicket.Name = "txtAccountIdTicket";
            txtAccountIdTicket.Padding = new Padding(0, 4, 0, 8);
            txtAccountIdTicket.PasswordChar = '\0';
            txtAccountIdTicket.PlaceholderText = "Placeholder";
            txtAccountIdTicket.ReadOnly = false;
            txtAccountIdTicket.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtAccountIdTicket.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtAccountIdTicket.Size = new Size(240, 63);
            txtAccountIdTicket.TabIndex = 50;
            txtAccountIdTicket.TextForeColor = Color.FromArgb(30, 30, 30);
            txtAccountIdTicket.UnderlineSpacing = 2;
            txtAccountIdTicket.UseSystemPasswordChar = false;
            // 
            // dtpDateOfBirthTicket
            // 
            dtpDateOfBirthTicket.BackColor = Color.Transparent;
            dtpDateOfBirthTicket.CustomFormat = null;
            dtpDateOfBirthTicket.LabelText = "Ngày sinh";
            dtpDateOfBirthTicket.Location = new Point(59, 128);
            dtpDateOfBirthTicket.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpDateOfBirthTicket.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpDateOfBirthTicket.Name = "dtpDateOfBirthTicket";
            dtpDateOfBirthTicket.Padding = new Padding(0, 4, 0, 8);
            dtpDateOfBirthTicket.PlaceholderText = "";
            dtpDateOfBirthTicket.Size = new Size(222, 59);
            dtpDateOfBirthTicket.TabIndex = 51;
            dtpDateOfBirthTicket.Value = new DateTime(2025, 10, 30, 10, 46, 34, 110);
            // 
            // cboBaggageTicket
            // 
            cboBaggageTicket.BackColor = Color.Transparent;
            cboBaggageTicket.LabelText = "Hành lý mua thêm";
            cboBaggageTicket.Location = new Point(586, 222);
            cboBaggageTicket.MinimumSize = new Size(140, 56);
            cboBaggageTicket.Name = "cboBaggageTicket";
            cboBaggageTicket.SelectedIndex = -1;
            cboBaggageTicket.SelectedItem = null;
            cboBaggageTicket.SelectedText = "";
            cboBaggageTicket.Size = new Size(188, 70);
            cboBaggageTicket.TabIndex = 52;
            cboBaggageTicket.Load += cboBaggageTicket_Load;
            // 
            // txtNoteBaggage
            // 
            txtNoteBaggage.BackColor = Color.Transparent;
            txtNoteBaggage.FocusedLineThickness = 3;
            txtNoteBaggage.InheritParentBackColor = true;
            txtNoteBaggage.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtNoteBaggage.LabelText = "Note";
            txtNoteBaggage.LineColor = Color.FromArgb(40, 40, 40);
            txtNoteBaggage.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtNoteBaggage.LineThickness = 2;
            txtNoteBaggage.Location = new Point(820, 222);
            txtNoteBaggage.Name = "txtNoteBaggage";
            txtNoteBaggage.Padding = new Padding(0, 4, 0, 8);
            txtNoteBaggage.PasswordChar = '\0';
            txtNoteBaggage.PlaceholderText = "Placeholder";
            txtNoteBaggage.ReadOnly = false;
            txtNoteBaggage.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtNoteBaggage.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtNoteBaggage.Size = new Size(188, 70);
            txtNoteBaggage.TabIndex = 53;
            txtNoteBaggage.TextForeColor = Color.FromArgb(30, 30, 30);
            txtNoteBaggage.UnderlineSpacing = 2;
            txtNoteBaggage.UseSystemPasswordChar = false;
            txtNoteBaggage.Load += underlinedTextField1_Load;
            // 
            // frmPassengerInfoControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtNoteBaggage);
            Controls.Add(cboBaggageTicket);
            Controls.Add(dtpDateOfBirthTicket);
            Controls.Add(txtAccountIdTicket);
            Controls.Add(dgvPassengerListTicket);
            Controls.Add(btnSelectSeatTicket);
            Controls.Add(txtSeatTicket);
            Controls.Add(dtpFlightDateTicket);
            Controls.Add(cboNationalityTicket);
            Controls.Add(txtPassportNumberTicket);
            Controls.Add(txtFullNameTicket);
            Controls.Add(txtEmailTicket);
            Controls.Add(txtPhoneNumberTicket);
            Controls.Add(btnAddPassengerTicket);
            Name = "frmPassengerInfoControl";
            Size = new Size(1270, 734);
            ((System.ComponentModel.ISupportInitialize)dgvPassengerListTicket).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Components.Inputs.UnderlinedComboBox cboNationalityTicket;
        private Components.Inputs.UnderlinedTextField txtPassportNumberTicket;
        private Components.Inputs.UnderlinedTextField txtFullNameTicket;
        private Components.Inputs.UnderlinedTextField txtEmailTicket;
        private Components.Inputs.UnderlinedTextField txtPhoneNumberTicket;
        private Components.Buttons.PrimaryButton btnAddPassengerTicket;
        private Components.Inputs.DateTimePickerCustom dtpFlightDateTicket;
        private Components.Inputs.UnderlinedTextField txtSeatTicket;
        private Components.Buttons.PrimaryButton btnSelectSeatTicket;
        private Components.Tables.TableCustom dgvPassengerListTicket;
        private Components.Inputs.UnderlinedTextField txtAccountIdTicket;
        private Components.Inputs.DateTimePickerCustom dtpDateOfBirthTicket;
        private Components.Inputs.UnderlinedComboBox cboBaggageTicket;
        private Components.Inputs.UnderlinedTextField txtNoteBaggage;
    }
}
