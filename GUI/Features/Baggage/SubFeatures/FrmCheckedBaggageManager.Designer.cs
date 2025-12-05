namespace GUI.Features.Baggage.SubFeatures
{
    partial class FrmCheckedBaggageManager
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
            txtCheckedId = new GUI.Components.Inputs.UnderlinedTextField();
            txtWeightKg = new GUI.Components.Inputs.UnderlinedTextField();
            txtDescription = new GUI.Components.Inputs.UnderlinedTextField();
            txtPrice = new GUI.Components.Inputs.UnderlinedTextField();
            btnAdd = new GUI.Components.Buttons.PrimaryButton();
            btnUpdate = new GUI.Components.Buttons.PrimaryButton();
            btnClear = new GUI.Components.Buttons.PrimaryButton();
            btnDelete = new GUI.Components.Buttons.PrimaryButton();
            dgvChecked = new GUI.Components.Tables.TableCustom();
            ((System.ComponentModel.ISupportInitialize)dgvChecked).BeginInit();
            SuspendLayout();
            // 
            // txtCheckedId
            // 
            txtCheckedId.BackColor = Color.Transparent;
            txtCheckedId.FocusedLineThickness = 3;
            txtCheckedId.InheritParentBackColor = true;
            txtCheckedId.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtCheckedId.LabelText = "txtCheckedId";
            txtCheckedId.LineColor = Color.FromArgb(40, 40, 40);
            txtCheckedId.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtCheckedId.LineThickness = 2;
            txtCheckedId.Location = new Point(23, 8);
            txtCheckedId.Name = "txtCheckedId";
            txtCheckedId.Padding = new Padding(0, 4, 0, 8);
            txtCheckedId.PasswordChar = '\0';
            txtCheckedId.PlaceholderText = "Placeholder";
            txtCheckedId.ReadOnly = false;
            txtCheckedId.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtCheckedId.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtCheckedId.Size = new Size(188, 67);
            txtCheckedId.TabIndex = 0;
            txtCheckedId.TextForeColor = Color.FromArgb(30, 30, 30);
            txtCheckedId.UnderlineSpacing = 2;
            txtCheckedId.UseSystemPasswordChar = false;
            // 
            // txtWeightKg
            // 
            txtWeightKg.BackColor = Color.Transparent;
            txtWeightKg.FocusedLineThickness = 3;
            txtWeightKg.InheritParentBackColor = true;
            txtWeightKg.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtWeightKg.LabelText = "txtWeightKg";
            txtWeightKg.LineColor = Color.FromArgb(40, 40, 40);
            txtWeightKg.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtWeightKg.LineThickness = 2;
            txtWeightKg.Location = new Point(33, 81);
            txtWeightKg.Name = "txtWeightKg";
            txtWeightKg.Padding = new Padding(0, 4, 0, 8);
            txtWeightKg.PasswordChar = '\0';
            txtWeightKg.PlaceholderText = "Placeholder";
            txtWeightKg.ReadOnly = false;
            txtWeightKg.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtWeightKg.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtWeightKg.Size = new Size(188, 68);
            txtWeightKg.TabIndex = 1;
            txtWeightKg.TextForeColor = Color.FromArgb(90, 90, 90);
            txtWeightKg.UnderlineSpacing = 2;
            txtWeightKg.UseSystemPasswordChar = false;
            // 
            // txtDescription
            // 
            txtDescription.BackColor = Color.Transparent;
            txtDescription.FocusedLineThickness = 3;
            txtDescription.InheritParentBackColor = true;
            txtDescription.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtDescription.LabelText = "txtDescription";
            txtDescription.LineColor = Color.FromArgb(40, 40, 40);
            txtDescription.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtDescription.LineThickness = 2;
            txtDescription.Location = new Point(251, 80);
            txtDescription.Name = "txtDescription";
            txtDescription.Padding = new Padding(0, 4, 0, 8);
            txtDescription.PasswordChar = '\0';
            txtDescription.PlaceholderText = "Placeholder";
            txtDescription.ReadOnly = false;
            txtDescription.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtDescription.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtDescription.Size = new Size(188, 68);
            txtDescription.TabIndex = 3;
            txtDescription.TextForeColor = Color.FromArgb(30, 30, 30);
            txtDescription.UnderlineSpacing = 2;
            txtDescription.UseSystemPasswordChar = false;
            // 
            // txtPrice
            // 
            txtPrice.BackColor = Color.Transparent;
            txtPrice.FocusedLineThickness = 3;
            txtPrice.InheritParentBackColor = true;
            txtPrice.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtPrice.LabelText = "txtPrice";
            txtPrice.LineColor = Color.FromArgb(40, 40, 40);
            txtPrice.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtPrice.LineThickness = 2;
            txtPrice.Location = new Point(251, 7);
            txtPrice.Name = "txtPrice";
            txtPrice.Padding = new Padding(0, 4, 0, 8);
            txtPrice.PasswordChar = '\0';
            txtPrice.PlaceholderText = "Placeholder";
            txtPrice.ReadOnly = false;
            txtPrice.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtPrice.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtPrice.Size = new Size(188, 67);
            txtPrice.TabIndex = 2;
            txtPrice.TextForeColor = Color.FromArgb(30, 30, 30);
            txtPrice.UnderlineSpacing = 2;
            txtPrice.UseSystemPasswordChar = false;
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
            btnAdd.Location = new Point(478, 8);
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
            btnAdd.TabIndex = 4;
            btnAdd.Text = "btnAdd";
            btnAdd.TextAlign = ContentAlignment.MiddleLeft;
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.WordWrap = false;
            btnAdd.Click += btnAdd_Click;
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
            btnUpdate.Location = new Point(493, 96);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnUpdate.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnUpdate.NormalForeColor = Color.White;
            btnUpdate.Padding = new Padding(24, 10, 24, 10);
            btnUpdate.PreferredMaxWidth = 0;
            btnUpdate.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnUpdate.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnUpdate.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnUpdate.Size = new Size(180, 52);
            btnUpdate.TabIndex = 5;
            btnUpdate.Text = "btnUpdate";
            btnUpdate.TextAlign = ContentAlignment.MiddleLeft;
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.WordWrap = false;
            btnUpdate.Click += btnUpdate_Click;
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
            btnClear.Location = new Point(757, 96);
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
            btnClear.TabIndex = 7;
            btnClear.Text = "btnClear";
            btnClear.TextAlign = ContentAlignment.MiddleLeft;
            btnClear.UseVisualStyleBackColor = false;
            btnClear.WordWrap = false;
            btnClear.Click += btnClear_Click;
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
            btnDelete.Location = new Point(767, 8);
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
            btnDelete.TabIndex = 6;
            btnDelete.Text = "btnDelete";
            btnDelete.TextAlign = ContentAlignment.MiddleLeft;
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.WordWrap = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // dgvChecked
            // 
            dgvChecked.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvChecked.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvChecked.BackgroundColor = Color.White;
            dgvChecked.BorderColor = Color.FromArgb(40, 40, 40);
            dgvChecked.BorderStyle = BorderStyle.None;
            dgvChecked.BorderThickness = 2;
            dgvChecked.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvChecked.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvChecked.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvChecked.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChecked.CornerRadius = 16;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvChecked.DefaultCellStyle = dataGridViewCellStyle3;
            dgvChecked.EnableHeadersVisualStyles = false;
            dgvChecked.GridColor = Color.FromArgb(230, 235, 240);
            dgvChecked.HeaderBackColor = Color.White;
            dgvChecked.HeaderForeColor = Color.FromArgb(126, 185, 232);
            dgvChecked.HoverBackColor = Color.FromArgb(232, 245, 255);
            dgvChecked.Location = new Point(23, 154);
            dgvChecked.MultiSelect = false;
            dgvChecked.Name = "dgvChecked";
            dgvChecked.RowAltBackColor = Color.FromArgb(248, 250, 252);
            dgvChecked.RowBackColor = Color.White;
            dgvChecked.RowForeColor = Color.FromArgb(33, 37, 41);
            dgvChecked.RowHeadersVisible = false;
            dgvChecked.RowHeadersWidth = 51;
            dgvChecked.RowTemplate.Height = 40;
            dgvChecked.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dgvChecked.SelectionForeColor = Color.White;
            dgvChecked.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChecked.Size = new Size(1114, 274);
            dgvChecked.TabIndex = 8;
            // 
            // FrmCheckedBaggageManager
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgvChecked);
            Controls.Add(btnClear);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(txtDescription);
            Controls.Add(txtPrice);
            Controls.Add(txtWeightKg);
            Controls.Add(txtCheckedId);
            Name = "FrmCheckedBaggageManager";
            Size = new Size(1520, 743);
            Load += FrmCheckedBaggageManager_Load;
            ((System.ComponentModel.ISupportInitialize)dgvChecked).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Components.Inputs.UnderlinedTextField txtCheckedId;
        private Components.Inputs.UnderlinedTextField txtWeightKg;
        private Components.Inputs.UnderlinedTextField txtDescription;
        private Components.Inputs.UnderlinedTextField txtPrice;
        private Components.Buttons.PrimaryButton btnAdd;
        private Components.Buttons.PrimaryButton btnUpdate;
        private Components.Buttons.PrimaryButton btnClear;
        private Components.Buttons.PrimaryButton btnDelete;
        private Components.Tables.TableCustom dgvChecked;
    }
}
