namespace GUI.Features.Baggage.SubFeatures
{
    partial class FrmTicketBaggageManager
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
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            txtTicketBaggageId = new GUI.Components.Inputs.UnderlinedTextField();
            txtQuantity = new GUI.Components.Inputs.UnderlinedTextField();
            txtNote = new GUI.Components.Inputs.UnderlinedTextField();
            cbBaggageType = new GUI.Components.Inputs.UnderlinedComboBox();
            cbBaggageList = new GUI.Components.Inputs.UnderlinedComboBox();
            dgvTicketBaggage = new GUI.Components.Tables.TableCustom();
            btnAdd = new GUI.Components.Buttons.PrimaryButton();
            btnDelete = new GUI.Components.Buttons.PrimaryButton();
            btnClear = new GUI.Components.Buttons.PrimaryButton();
            ((System.ComponentModel.ISupportInitialize)dgvTicketBaggage).BeginInit();
            SuspendLayout();
            // 
            // txtTicketBaggageId
            // 
            txtTicketBaggageId.BackColor = Color.Transparent;
            txtTicketBaggageId.FocusedLineThickness = 3;
            txtTicketBaggageId.InheritParentBackColor = true;
            txtTicketBaggageId.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtTicketBaggageId.LabelText = "txtTicketBaggageId";
            txtTicketBaggageId.LineColor = Color.FromArgb(40, 40, 40);
            txtTicketBaggageId.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtTicketBaggageId.LineThickness = 2;
            txtTicketBaggageId.Location = new Point(88, 85);
            txtTicketBaggageId.Name = "txtTicketBaggageId";
            txtTicketBaggageId.Padding = new Padding(0, 4, 0, 8);
            txtTicketBaggageId.PasswordChar = '\0';
            txtTicketBaggageId.PlaceholderText = "Placeholder";
            txtTicketBaggageId.ReadOnly = false;
            txtTicketBaggageId.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtTicketBaggageId.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtTicketBaggageId.Size = new Size(188, 64);
            txtTicketBaggageId.TabIndex = 0;
            txtTicketBaggageId.TextForeColor = Color.FromArgb(30, 30, 30);
            txtTicketBaggageId.UnderlineSpacing = 2;
            txtTicketBaggageId.UseSystemPasswordChar = false;
            // 
            // txtQuantity
            // 
            txtQuantity.BackColor = Color.Transparent;
            txtQuantity.FocusedLineThickness = 3;
            txtQuantity.InheritParentBackColor = true;
            txtQuantity.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtQuantity.LabelText = "txtQuantity";
            txtQuantity.LineColor = Color.FromArgb(40, 40, 40);
            txtQuantity.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtQuantity.LineThickness = 2;
            txtQuantity.Location = new Point(302, 85);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Padding = new Padding(0, 4, 0, 8);
            txtQuantity.PasswordChar = '\0';
            txtQuantity.PlaceholderText = "Placeholder";
            txtQuantity.ReadOnly = false;
            txtQuantity.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtQuantity.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtQuantity.Size = new Size(188, 64);
            txtQuantity.TabIndex = 1;
            txtQuantity.TextForeColor = Color.FromArgb(30, 30, 30);
            txtQuantity.UnderlineSpacing = 2;
            txtQuantity.UseSystemPasswordChar = false;
            // 
            // txtNote
            // 
            txtNote.BackColor = Color.Transparent;
            txtNote.FocusedLineThickness = 3;
            txtNote.InheritParentBackColor = true;
            txtNote.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtNote.LabelText = "txtNote";
            txtNote.LineColor = Color.FromArgb(40, 40, 40);
            txtNote.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtNote.LineThickness = 2;
            txtNote.Location = new Point(496, 85);
            txtNote.Name = "txtNote";
            txtNote.Padding = new Padding(0, 4, 0, 8);
            txtNote.PasswordChar = '\0';
            txtNote.PlaceholderText = "Placeholder";
            txtNote.ReadOnly = false;
            txtNote.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtNote.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtNote.Size = new Size(188, 64);
            txtNote.TabIndex = 2;
            txtNote.TextForeColor = Color.FromArgb(30, 30, 30);
            txtNote.UnderlineSpacing = 2;
            txtNote.UseSystemPasswordChar = false;
            // 
            // cbBaggageType
            // 
            cbBaggageType.BackColor = Color.Transparent;
            cbBaggageType.DataSource = null;
            cbBaggageType.DisplayMember = "";
            cbBaggageType.DropDownStyle = ComboBoxStyle.DropDown;
            cbBaggageType.LabelText = "cbBaggageType";
            cbBaggageType.Location = new Point(726, 79);
            cbBaggageType.MinimumSize = new Size(140, 56);
            cbBaggageType.Name = "cbBaggageType";
            cbBaggageType.SelectedIndex = -1;
            cbBaggageType.SelectedItem = null;
            cbBaggageType.SelectedText = "";
            cbBaggageType.SelectedValue = null;
            cbBaggageType.Size = new Size(188, 70);
            cbBaggageType.TabIndex = 3;
            cbBaggageType.ValueMember = "";
            //cbBaggageType.Load += underlinedComboBox1_Load;
            // 
            // cbBaggageList
            // 
            cbBaggageList.BackColor = Color.Transparent;
            cbBaggageList.DataSource = null;
            cbBaggageList.DisplayMember = "";
            cbBaggageList.DropDownStyle = ComboBoxStyle.DropDown;
            cbBaggageList.LabelText = "cbBaggageList";
            cbBaggageList.Location = new Point(938, 79);
            cbBaggageList.MinimumSize = new Size(140, 56);
            cbBaggageList.Name = "cbBaggageList";
            cbBaggageList.SelectedIndex = -1;
            cbBaggageList.SelectedItem = null;
            cbBaggageList.SelectedText = "";
            cbBaggageList.SelectedValue = null;
            cbBaggageList.Size = new Size(188, 70);
            cbBaggageList.TabIndex = 4;
            cbBaggageList.ValueMember = "";
            // 
            // dgvTicketBaggage
            // 
            dgvTicketBaggage.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(248, 250, 252);
            dgvTicketBaggage.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            dgvTicketBaggage.BackgroundColor = Color.White;
            dgvTicketBaggage.BorderColor = Color.FromArgb(40, 40, 40);
            dgvTicketBaggage.BorderStyle = BorderStyle.None;
            dgvTicketBaggage.BorderThickness = 2;
            dgvTicketBaggage.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTicketBaggage.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.White;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle8.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle8.SelectionBackColor = Color.White;
            dataGridViewCellStyle8.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dgvTicketBaggage.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            dgvTicketBaggage.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTicketBaggage.CornerRadius = 16;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.White;
            dataGridViewCellStyle9.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle9.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle9.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle9.SelectionForeColor = Color.White;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.False;
            dgvTicketBaggage.DefaultCellStyle = dataGridViewCellStyle9;
            dgvTicketBaggage.EnableHeadersVisualStyles = false;
            dgvTicketBaggage.GridColor = Color.FromArgb(230, 235, 240);
            dgvTicketBaggage.HeaderBackColor = Color.White;
            dgvTicketBaggage.HeaderForeColor = Color.FromArgb(126, 185, 232);
            dgvTicketBaggage.HoverBackColor = Color.FromArgb(232, 245, 255);
            dgvTicketBaggage.Location = new Point(51, 295);
            dgvTicketBaggage.MultiSelect = false;
            dgvTicketBaggage.Name = "dgvTicketBaggage";
            dgvTicketBaggage.RowAltBackColor = Color.FromArgb(248, 250, 252);
            dgvTicketBaggage.RowBackColor = Color.White;
            dgvTicketBaggage.RowForeColor = Color.FromArgb(33, 37, 41);
            dgvTicketBaggage.RowHeadersVisible = false;
            dgvTicketBaggage.RowHeadersWidth = 51;
            dgvTicketBaggage.RowTemplate.Height = 40;
            dgvTicketBaggage.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dgvTicketBaggage.SelectionForeColor = Color.White;
            dgvTicketBaggage.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTicketBaggage.Size = new Size(1001, 227);
            dgvTicketBaggage.TabIndex = 5;
            // 
            // btnAdd
            // 
            btnAdd.AutoSize = true;
            btnAdd.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnAdd.BackColor = Color.FromArgb(155, 209, 243);
            btnAdd.BorderColor = Color.FromArgb(40, 40, 40);
            btnAdd.BorderThickness = 2;
            btnAdd.CornerRadius = 22;
            btnAdd.EnableHoverEffects = true;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.HoverBackColor = Color.White;
            btnAdd.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnAdd.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnAdd.Icon = null;
            btnAdd.IconSize = new Size(22, 22);
            btnAdd.IconSpacing = 10;
            btnAdd.Location = new Point(162, 200);
            btnAdd.Name = "btnAdd";
            btnAdd.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnAdd.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnAdd.NormalForeColor = Color.White;
            btnAdd.Padding = new Padding(24, 10, 24, 10);
            btnAdd.PreferredMaxWidth = 0;
            btnAdd.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnAdd.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnAdd.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnAdd.Size = new Size(150, 52);
            btnAdd.TabIndex = 6;
            btnAdd.Text = "btnAdd";
            btnAdd.TextAlign = ContentAlignment.MiddleLeft;
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.WordWrap = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnDelete
            // 
            btnDelete.AutoSize = true;
            btnDelete.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnDelete.BackColor = Color.FromArgb(155, 209, 243);
            btnDelete.BorderColor = Color.FromArgb(40, 40, 40);
            btnDelete.BorderThickness = 2;
            btnDelete.CornerRadius = 22;
            btnDelete.EnableHoverEffects = true;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.HoverBackColor = Color.White;
            btnDelete.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnDelete.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnDelete.Icon = null;
            btnDelete.IconSize = new Size(22, 22);
            btnDelete.IconSpacing = 10;
            btnDelete.Location = new Point(358, 200);
            btnDelete.Name = "btnDelete";
            btnDelete.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnDelete.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnDelete.NormalForeColor = Color.White;
            btnDelete.Padding = new Padding(24, 10, 24, 10);
            btnDelete.PreferredMaxWidth = 0;
            btnDelete.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnDelete.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnDelete.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnDelete.Size = new Size(174, 52);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "btnDelete";
            btnDelete.TextAlign = ContentAlignment.MiddleLeft;
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.WordWrap = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnClear
            // 
            btnClear.AutoSize = true;
            btnClear.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnClear.BackColor = Color.FromArgb(155, 209, 243);
            btnClear.BorderColor = Color.FromArgb(40, 40, 40);
            btnClear.BorderThickness = 2;
            btnClear.CornerRadius = 22;
            btnClear.EnableHoverEffects = true;
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnClear.ForeColor = Color.White;
            btnClear.HoverBackColor = Color.White;
            btnClear.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnClear.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnClear.Icon = null;
            btnClear.IconSize = new Size(22, 22);
            btnClear.IconSpacing = 10;
            btnClear.Location = new Point(566, 200);
            btnClear.Name = "btnClear";
            btnClear.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnClear.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnClear.NormalForeColor = Color.White;
            btnClear.Padding = new Padding(24, 10, 24, 10);
            btnClear.PreferredMaxWidth = 0;
            btnClear.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnClear.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnClear.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnClear.Size = new Size(160, 52);
            btnClear.TabIndex = 8;
            btnClear.Text = "btnClear";
            btnClear.TextAlign = ContentAlignment.MiddleLeft;
            btnClear.UseVisualStyleBackColor = false;
            btnClear.WordWrap = false;
            btnClear.Click += btnClear_Click;
            // 
            // FrmTicketBaggageManager
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnClear);
            Controls.Add(btnDelete);
            Controls.Add(btnAdd);
            Controls.Add(dgvTicketBaggage);
            Controls.Add(cbBaggageList);
            Controls.Add(cbBaggageType);
            Controls.Add(txtNote);
            Controls.Add(txtQuantity);
            Controls.Add(txtTicketBaggageId);
            Name = "FrmTicketBaggageManager";
            Size = new Size(1508, 745);
            ((System.ComponentModel.ISupportInitialize)dgvTicketBaggage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Components.Inputs.UnderlinedTextField txtTicketBaggageId;
        private Components.Inputs.UnderlinedTextField txtQuantity;
        private Components.Inputs.UnderlinedTextField txtNote;
        private Components.Inputs.UnderlinedComboBox cbBaggageType;
        private Components.Inputs.UnderlinedComboBox cbBaggageList;
        private Components.Tables.TableCustom dgvTicketBaggage;
        private Components.Buttons.PrimaryButton btnAdd;
        private Components.Buttons.PrimaryButton btnDelete;
        private Components.Buttons.PrimaryButton btnClear;
    }
}
