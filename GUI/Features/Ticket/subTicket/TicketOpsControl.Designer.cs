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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dgvTicketOpsControl = new GUI.Components.Tables.TableCustom();
            txtSearchTicket = new GUI.Components.Inputs.UnderlinedTextField();
            cboSearchTicket = new GUI.Components.Inputs.UnderlinedComboBox();
            btnSearchTicket = new GUI.Components.Buttons.PrimaryButton();
            ((System.ComponentModel.ISupportInitialize)dgvTicketOpsControl).BeginInit();
            SuspendLayout();
            // 
            // dgvTicketOpsControl
            // 
            dgvTicketOpsControl.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvTicketOpsControl.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvTicketOpsControl.BackgroundColor = Color.White;
            dgvTicketOpsControl.BorderColor = Color.FromArgb(40, 40, 40);
            dgvTicketOpsControl.BorderStyle = BorderStyle.None;
            dgvTicketOpsControl.BorderThickness = 2;
            dgvTicketOpsControl.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTicketOpsControl.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvTicketOpsControl.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvTicketOpsControl.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTicketOpsControl.CornerRadius = 16;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvTicketOpsControl.DefaultCellStyle = dataGridViewCellStyle3;
            dgvTicketOpsControl.EnableHeadersVisualStyles = false;
            dgvTicketOpsControl.GridColor = Color.FromArgb(230, 235, 240);
            dgvTicketOpsControl.HeaderBackColor = Color.White;
            dgvTicketOpsControl.HeaderForeColor = Color.FromArgb(126, 185, 232);
            dgvTicketOpsControl.HoverBackColor = Color.FromArgb(232, 245, 255);
            dgvTicketOpsControl.Location = new Point(3, 245);
            dgvTicketOpsControl.MultiSelect = false;
            dgvTicketOpsControl.Name = "dgvTicketOpsControl";
            dgvTicketOpsControl.RowAltBackColor = Color.FromArgb(248, 250, 252);
            dgvTicketOpsControl.RowBackColor = Color.White;
            dgvTicketOpsControl.RowForeColor = Color.FromArgb(33, 37, 41);
            dgvTicketOpsControl.RowHeadersVisible = false;
            dgvTicketOpsControl.RowHeadersWidth = 51;
            dgvTicketOpsControl.RowTemplate.Height = 40;
            dgvTicketOpsControl.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dgvTicketOpsControl.SelectionForeColor = Color.White;
            dgvTicketOpsControl.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTicketOpsControl.Size = new Size(1451, 329);
            dgvTicketOpsControl.TabIndex = 3;
            dgvTicketOpsControl.CellContentClick += dgvTicketNumberHistory_CellContentClick;
            // 
            // txtSearchTicket
            // 
            txtSearchTicket.BackColor = Color.Transparent;
            txtSearchTicket.FocusedLineThickness = 3;
            txtSearchTicket.InheritParentBackColor = true;
            txtSearchTicket.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtSearchTicket.LabelText = "Nhãn";
            txtSearchTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtSearchTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtSearchTicket.LineThickness = 2;
            txtSearchTicket.Location = new Point(74, 118);
            txtSearchTicket.Name = "txtSearchTicket";
            txtSearchTicket.Padding = new Padding(0, 4, 0, 8);
            txtSearchTicket.PasswordChar = '\0';
            txtSearchTicket.PlaceholderText = "Placeholder";
            txtSearchTicket.ReadOnly = false;
            txtSearchTicket.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtSearchTicket.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtSearchTicket.Size = new Size(188, 70);
            txtSearchTicket.TabIndex = 4;
            txtSearchTicket.TextForeColor = Color.FromArgb(30, 30, 30);
            txtSearchTicket.UnderlineSpacing = 2;
            txtSearchTicket.UseSystemPasswordChar = false;
            txtSearchTicket.Load += txtSearchTicket_Load;
            txtSearchTicket.KeyUp += KeyUp_ticket;
            // 
            // cboSearchTicket
            // 
            cboSearchTicket.BackColor = Color.Transparent;
            cboSearchTicket.DataSource = null;
            cboSearchTicket.DisplayMember = "";
            cboSearchTicket.DropDownStyle = ComboBoxStyle.DropDown;
            cboSearchTicket.LabelText = "Label";
            cboSearchTicket.Location = new Point(316, 118);
            cboSearchTicket.MinimumSize = new Size(140, 56);
            cboSearchTicket.Name = "cboSearchTicket";
            cboSearchTicket.SelectedIndex = -1;
            cboSearchTicket.SelectedItem = null;
            cboSearchTicket.SelectedText = "";
            cboSearchTicket.SelectedValue = null;
            cboSearchTicket.Size = new Size(188, 70);
            cboSearchTicket.TabIndex = 5;
            cboSearchTicket.ValueMember = "";
            cboSearchTicket.SelectedIndexChanged += cbo_changedIndex;
            cboSearchTicket.Load += underlinedComboBox1_Load;
            // 
            // btnSearchTicket
            // 
            btnSearchTicket.AutoSize = true;
            btnSearchTicket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSearchTicket.BackColor = Color.FromArgb(155, 209, 243);
            btnSearchTicket.BorderColor = Color.FromArgb(40, 40, 40);
            btnSearchTicket.BorderThickness = 2;
            btnSearchTicket.CornerRadius = 22;
            btnSearchTicket.EnableHoverEffects = true;
            btnSearchTicket.FlatAppearance.BorderSize = 0;
            btnSearchTicket.FlatStyle = FlatStyle.Flat;
            btnSearchTicket.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSearchTicket.ForeColor = Color.White;
            btnSearchTicket.HoverBackColor = Color.White;
            btnSearchTicket.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnSearchTicket.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnSearchTicket.Icon = null;
            btnSearchTicket.IconSize = new Size(22, 22);
            btnSearchTicket.IconSpacing = 10;
            btnSearchTicket.Location = new Point(627, 136);
            btnSearchTicket.Name = "btnSearchTicket";
            btnSearchTicket.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnSearchTicket.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnSearchTicket.NormalForeColor = Color.White;
            btnSearchTicket.Padding = new Padding(24, 10, 24, 10);
            btnSearchTicket.PreferredMaxWidth = 0;
            btnSearchTicket.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnSearchTicket.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnSearchTicket.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnSearchTicket.Size = new Size(143, 52);
            btnSearchTicket.TabIndex = 6;
            btnSearchTicket.Text = "Search";
            btnSearchTicket.TextAlign = ContentAlignment.MiddleLeft;
            btnSearchTicket.UseVisualStyleBackColor = false;
            btnSearchTicket.WordWrap = false;
            btnSearchTicket.Click += primaryButton1_Click;
            // 
            // TicketOpsControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnSearchTicket);
            Controls.Add(cboSearchTicket);
            Controls.Add(txtSearchTicket);
            Controls.Add(dgvTicketOpsControl);
            Name = "TicketOpsControl";
            Size = new Size(1457, 577);
            ((System.ComponentModel.ISupportInitialize)dgvTicketOpsControl).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Components.Tables.TableCustom dgvTicketOpsControl;
        private Components.Inputs.UnderlinedTextField txtSearchTicket;
        private Components.Inputs.UnderlinedComboBox cboSearchTicket;
        private Components.Buttons.PrimaryButton btnSearchTicket;
    }
}
