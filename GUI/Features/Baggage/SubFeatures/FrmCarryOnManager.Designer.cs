namespace GUI.Features.Baggage.SubFeatures
{
    partial class FrmCarryOnManager
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
            dgvCarryOn = new GUI.Components.Tables.TableCustom();
            txtCarryOnId = new GUI.Components.Inputs.UnderlinedTextField();
            btnUpdate = new GUI.Components.Buttons.PrimaryButton();
            cbClass = new GUI.Components.Inputs.UnderlinedComboBox();
            txtWeightKg = new GUI.Components.Inputs.UnderlinedTextField();
            txtSizeLimit = new GUI.Components.Inputs.UnderlinedTextField();
            txtDescription = new GUI.Components.Inputs.UnderlinedTextField();
            underlinedTextField5 = new GUI.Components.Inputs.UnderlinedTextField();
            btnAdd = new GUI.Components.Buttons.PrimaryButton();
            btnDelete = new GUI.Components.Buttons.PrimaryButton();
            chkIsDefault = new CheckBox();
            btnClear = new GUI.Components.Buttons.PrimaryButton();
            ((System.ComponentModel.ISupportInitialize)dgvCarryOn).BeginInit();
            SuspendLayout();
            // 
            // dgvCarryOn
            // 
            dgvCarryOn.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvCarryOn.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvCarryOn.BackgroundColor = Color.White;
            dgvCarryOn.BorderColor = Color.FromArgb(40, 40, 40);
            dgvCarryOn.BorderStyle = BorderStyle.None;
            dgvCarryOn.BorderThickness = 2;
            dgvCarryOn.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCarryOn.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvCarryOn.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvCarryOn.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCarryOn.CornerRadius = 16;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvCarryOn.DefaultCellStyle = dataGridViewCellStyle3;
            dgvCarryOn.EnableHeadersVisualStyles = false;
            dgvCarryOn.GridColor = Color.FromArgb(230, 235, 240);
            dgvCarryOn.HeaderBackColor = Color.White;
            dgvCarryOn.HeaderForeColor = Color.FromArgb(126, 185, 232);
            dgvCarryOn.HoverBackColor = Color.FromArgb(232, 245, 255);
            dgvCarryOn.Location = new Point(10, 208);
            dgvCarryOn.MultiSelect = false;
            dgvCarryOn.Name = "dgvCarryOn";
            dgvCarryOn.RowAltBackColor = Color.FromArgb(248, 250, 252);
            dgvCarryOn.RowBackColor = Color.White;
            dgvCarryOn.RowForeColor = Color.FromArgb(33, 37, 41);
            dgvCarryOn.RowHeadersVisible = false;
            dgvCarryOn.RowHeadersWidth = 51;
            dgvCarryOn.RowTemplate.Height = 40;
            dgvCarryOn.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dgvCarryOn.SelectionForeColor = Color.White;
            dgvCarryOn.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCarryOn.Size = new Size(1181, 261);
            dgvCarryOn.TabIndex = 0;
            // 
            // txtCarryOnId
            // 
            txtCarryOnId.BackColor = Color.Transparent;
            txtCarryOnId.FocusedLineThickness = 3;
            txtCarryOnId.InheritParentBackColor = true;
            txtCarryOnId.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtCarryOnId.LabelText = "CarryOn ID";
            txtCarryOnId.LineColor = Color.FromArgb(40, 40, 40);
            txtCarryOnId.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtCarryOnId.LineThickness = 2;
            txtCarryOnId.Location = new Point(15, 11);
            txtCarryOnId.Name = "txtCarryOnId";
            txtCarryOnId.Padding = new Padding(0, 4, 0, 8);
            txtCarryOnId.PasswordChar = '\0';
            txtCarryOnId.PlaceholderText = "Placeholder";
            txtCarryOnId.ReadOnly = false;
            txtCarryOnId.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtCarryOnId.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtCarryOnId.Size = new Size(188, 68);
            txtCarryOnId.TabIndex = 1;
            txtCarryOnId.TextForeColor = Color.FromArgb(30, 30, 30);
            txtCarryOnId.UnderlineSpacing = 2;
            txtCarryOnId.UseSystemPasswordChar = false;
            // 
            // btnUpdate
            // 
            btnUpdate.AutoSize = true;
            btnUpdate.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnUpdate.BackColor = Color.FromArgb(155, 209, 243);
            btnUpdate.BorderColor = Color.FromArgb(40, 40, 40);
            btnUpdate.BorderThickness = 2;
            btnUpdate.CornerRadius = 22;
            btnUpdate.EnableHoverEffects = true;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.HoverBackColor = Color.White;
            btnUpdate.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnUpdate.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnUpdate.Icon = null;
            btnUpdate.IconSize = new Size(22, 22);
            btnUpdate.IconSpacing = 10;
            btnUpdate.Location = new Point(606, 135);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnUpdate.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnUpdate.NormalForeColor = Color.White;
            btnUpdate.Padding = new Padding(24, 10, 24, 10);
            btnUpdate.PreferredMaxWidth = 0;
            btnUpdate.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnUpdate.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnUpdate.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnUpdate.Size = new Size(115, 52);
            btnUpdate.TabIndex = 2;
            btnUpdate.Text = "Sửa";
            btnUpdate.TextAlign = ContentAlignment.MiddleLeft;
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.WordWrap = false;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // cbClass
            // 
            cbClass.BackColor = Color.Transparent;
            cbClass.DataSource = null;
            cbClass.DisplayMember = "";
            cbClass.DropDownStyle = ComboBoxStyle.DropDown;
            cbClass.LabelText = "cbClass";
            cbClass.Location = new Point(677, 11);
            cbClass.MinimumSize = new Size(140, 56);
            cbClass.Name = "cbClass";
            cbClass.SelectedIndex = -1;
            cbClass.SelectedItem = null;
            cbClass.SelectedText = "";
            cbClass.SelectedValue = null;
            cbClass.Size = new Size(188, 70);
            cbClass.TabIndex = 3;
            cbClass.ValueMember = "";
            // 
            // txtWeightKg
            // 
            txtWeightKg.BackColor = Color.Transparent;
            txtWeightKg.FocusedLineThickness = 3;
            txtWeightKg.InheritParentBackColor = false;
            txtWeightKg.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtWeightKg.LabelText = "Weight kg";
            txtWeightKg.LineColor = Color.FromArgb(40, 40, 40);
            txtWeightKg.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtWeightKg.LineThickness = 2;
            txtWeightKg.Location = new Point(220, 22);
            txtWeightKg.Name = "txtWeightKg";
            txtWeightKg.Padding = new Padding(0, 4, 0, 8);
            txtWeightKg.PasswordChar = '\0';
            txtWeightKg.PlaceholderText = "Placeholder";
            txtWeightKg.ReadOnly = false;
            txtWeightKg.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtWeightKg.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtWeightKg.Size = new Size(188, 68);
            txtWeightKg.TabIndex = 4;
            txtWeightKg.TextForeColor = Color.FromArgb(30, 30, 30);
            txtWeightKg.UnderlineSpacing = 2;
            txtWeightKg.UseSystemPasswordChar = false;
            // 
            // txtSizeLimit
            // 
            txtSizeLimit.BackColor = Color.Transparent;
            txtSizeLimit.FocusedLineThickness = 3;
            txtSizeLimit.InheritParentBackColor = true;
            txtSizeLimit.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtSizeLimit.LabelText = "Size limit";
            txtSizeLimit.LineColor = Color.FromArgb(40, 40, 40);
            txtSizeLimit.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtSizeLimit.LineThickness = 2;
            txtSizeLimit.Location = new Point(454, 11);
            txtSizeLimit.Name = "txtSizeLimit";
            txtSizeLimit.Padding = new Padding(0, 4, 0, 8);
            txtSizeLimit.PasswordChar = '\0';
            txtSizeLimit.PlaceholderText = "Placeholder";
            txtSizeLimit.ReadOnly = false;
            txtSizeLimit.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtSizeLimit.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtSizeLimit.Size = new Size(188, 68);
            txtSizeLimit.TabIndex = 5;
            txtSizeLimit.TextForeColor = Color.FromArgb(30, 30, 30);
            txtSizeLimit.UnderlineSpacing = 2;
            txtSizeLimit.UseSystemPasswordChar = false;
            // 
            // txtDescription
            // 
            txtDescription.BackColor = Color.Transparent;
            txtDescription.FocusedLineThickness = 3;
            txtDescription.InheritParentBackColor = true;
            txtDescription.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtDescription.LabelText = "Description";
            txtDescription.LineColor = Color.FromArgb(40, 40, 40);
            txtDescription.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtDescription.LineThickness = 2;
            txtDescription.Location = new Point(15, 119);
            txtDescription.Name = "txtDescription";
            txtDescription.Padding = new Padding(0, 4, 0, 8);
            txtDescription.PasswordChar = '\0';
            txtDescription.PlaceholderText = "Placeholder";
            txtDescription.ReadOnly = false;
            txtDescription.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtDescription.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtDescription.Size = new Size(188, 68);
            txtDescription.TabIndex = 6;
            txtDescription.TextForeColor = Color.FromArgb(30, 30, 30);
            txtDescription.UnderlineSpacing = 2;
            txtDescription.UseSystemPasswordChar = false;
            // 
            // underlinedTextField5
            // 
            underlinedTextField5.BackColor = Color.Transparent;
            underlinedTextField5.FocusedLineThickness = 3;
            underlinedTextField5.InheritParentBackColor = true;
            underlinedTextField5.LabelForeColor = Color.FromArgb(70, 70, 70);
            underlinedTextField5.LabelText = "Nhãn";
            underlinedTextField5.LineColor = Color.FromArgb(40, 40, 40);
            underlinedTextField5.LineColorFocused = Color.FromArgb(0, 92, 175);
            underlinedTextField5.LineThickness = 2;
            underlinedTextField5.Location = new Point(232, 119);
            underlinedTextField5.Name = "underlinedTextField5";
            underlinedTextField5.Padding = new Padding(0, 4, 0, 8);
            underlinedTextField5.PasswordChar = '\0';
            underlinedTextField5.PlaceholderText = "Placeholder";
            underlinedTextField5.ReadOnly = false;
            underlinedTextField5.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            underlinedTextField5.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            underlinedTextField5.Size = new Size(188, 68);
            underlinedTextField5.TabIndex = 7;
            underlinedTextField5.TextForeColor = Color.FromArgb(30, 30, 30);
            underlinedTextField5.UnderlineSpacing = 2;
            underlinedTextField5.UseSystemPasswordChar = false;
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
            btnAdd.Location = new Point(441, 135);
            btnAdd.Name = "btnAdd";
            btnAdd.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnAdd.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnAdd.NormalForeColor = Color.White;
            btnAdd.Padding = new Padding(24, 10, 24, 10);
            btnAdd.PreferredMaxWidth = 0;
            btnAdd.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnAdd.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnAdd.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnAdd.Size = new Size(133, 52);
            btnAdd.TabIndex = 8;
            btnAdd.Text = "Thêm";
            btnAdd.TextAlign = ContentAlignment.MiddleLeft;
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.WordWrap = false;
            btnAdd.Click += btnAdd_Click_1;
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
            btnDelete.Location = new Point(749, 135);
            btnDelete.Name = "btnDelete";
            btnDelete.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnDelete.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnDelete.NormalForeColor = Color.White;
            btnDelete.Padding = new Padding(24, 10, 24, 10);
            btnDelete.PreferredMaxWidth = 0;
            btnDelete.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnDelete.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnDelete.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnDelete.Size = new Size(116, 52);
            btnDelete.TabIndex = 9;
            btnDelete.Text = "Xóa";
            btnDelete.TextAlign = ContentAlignment.MiddleLeft;
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.WordWrap = false;
            btnDelete.Click += btnDelete_Click_1;
            // 
            // chkIsDefault
            // 
            chkIsDefault.AutoSize = true;
            chkIsDefault.Location = new Point(948, 39);
            chkIsDefault.Name = "chkIsDefault";
            chkIsDefault.Size = new Size(80, 24);
            chkIsDefault.TabIndex = 10;
            chkIsDefault.Text = "Default";
            chkIsDefault.UseVisualStyleBackColor = true;
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
            btnClear.Location = new Point(882, 135);
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
            btnClear.TabIndex = 11;
            btnClear.Text = "btnClear";
            btnClear.TextAlign = ContentAlignment.MiddleLeft;
            btnClear.UseVisualStyleBackColor = false;
            btnClear.WordWrap = false;
            btnClear.Click += btnClear_Click_1;
            // 
            // FrmCarryOnManager
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnClear);
            Controls.Add(chkIsDefault);
            Controls.Add(btnDelete);
            Controls.Add(btnAdd);
            Controls.Add(underlinedTextField5);
            Controls.Add(txtDescription);
            Controls.Add(txtSizeLimit);
            Controls.Add(txtWeightKg);
            Controls.Add(cbClass);
            Controls.Add(btnUpdate);
            Controls.Add(txtCarryOnId);
            Controls.Add(dgvCarryOn);
            Name = "FrmCarryOnManager";
            Size = new Size(1512, 766);
            ((System.ComponentModel.ISupportInitialize)dgvCarryOn).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Components.Tables.TableCustom dgvCarryOn;
        private Components.Inputs.UnderlinedTextField txtCarryOnId;
        private Components.Buttons.PrimaryButton btnUpdate;
        private Components.Inputs.UnderlinedComboBox cbClass;
        private Components.Inputs.UnderlinedTextField txtWeightKg;
        private Components.Inputs.UnderlinedTextField txtSizeLimit;
        private Components.Inputs.UnderlinedTextField txtDescription;
        private Components.Inputs.UnderlinedTextField underlinedTextField5;
        private Components.Buttons.PrimaryButton btnAdd;
        private Components.Buttons.PrimaryButton btnDelete;
        private CheckBox chkIsDefault;
        private Components.Buttons.PrimaryButton btnClear;
    }
}
