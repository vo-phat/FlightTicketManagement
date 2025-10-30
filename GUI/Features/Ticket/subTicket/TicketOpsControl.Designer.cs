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
            underlinedComboBox1 = new GUI.Components.Inputs.UnderlinedComboBox();
            underlinedTextField3 = new GUI.Components.Inputs.UnderlinedTextField();
            underlinedTextField4 = new GUI.Components.Inputs.UnderlinedTextField();
            underlinedTextField5 = new GUI.Components.Inputs.UnderlinedTextField();
            btnQuickCreateBookingTicket = new GUI.Components.Buttons.PrimaryButton();
            dgvBookingsTicket = new GUI.Components.Tables.TableCustom();
            ((System.ComponentModel.ISupportInitialize)dgvBookingsTicket).BeginInit();
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
            // underlinedComboBox1
            // 
            underlinedComboBox1.BackColor = Color.Transparent;
            underlinedComboBox1.LabelText = "Trạng thái";
            underlinedComboBox1.Location = new Point(677, 167);
            underlinedComboBox1.MinimumSize = new Size(140, 56);
            underlinedComboBox1.Name = "underlinedComboBox1";
            underlinedComboBox1.SelectedIndex = -1;
            underlinedComboBox1.SelectedItem = null;
            underlinedComboBox1.SelectedText = "";
            underlinedComboBox1.Size = new Size(188, 70);
            underlinedComboBox1.TabIndex = 45;
            // 
            // underlinedTextField3
            // 
            underlinedTextField3.BackColor = Color.Transparent;
            underlinedTextField3.FocusedLineThickness = 3;
            underlinedTextField3.InheritParentBackColor = true;
            underlinedTextField3.LabelForeColor = Color.FromArgb(70, 70, 70);
            underlinedTextField3.LabelText = "Mã đặt chỗ";
            underlinedTextField3.LineColor = Color.FromArgb(40, 40, 40);
            underlinedTextField3.LineColorFocused = Color.FromArgb(0, 92, 175);
            underlinedTextField3.LineThickness = 2;
            underlinedTextField3.Location = new Point(28, 174);
            underlinedTextField3.Name = "underlinedTextField3";
            underlinedTextField3.Padding = new Padding(0, 4, 0, 8);
            underlinedTextField3.PasswordChar = '\0';
            underlinedTextField3.PlaceholderText = "Placeholder";
            underlinedTextField3.ReadOnly = false;
            underlinedTextField3.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            underlinedTextField3.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            underlinedTextField3.Size = new Size(188, 63);
            underlinedTextField3.TabIndex = 44;
            underlinedTextField3.TextForeColor = Color.FromArgb(30, 30, 30);
            underlinedTextField3.UnderlineSpacing = 2;
            underlinedTextField3.UseSystemPasswordChar = false;
            // 
            // underlinedTextField4
            // 
            underlinedTextField4.BackColor = Color.Transparent;
            underlinedTextField4.FocusedLineThickness = 3;
            underlinedTextField4.InheritParentBackColor = true;
            underlinedTextField4.LabelForeColor = Color.FromArgb(70, 70, 70);
            underlinedTextField4.LabelText = "Hành khách";
            underlinedTextField4.LineColor = Color.FromArgb(40, 40, 40);
            underlinedTextField4.LineColorFocused = Color.FromArgb(0, 92, 175);
            underlinedTextField4.LineThickness = 2;
            underlinedTextField4.Location = new Point(444, 174);
            underlinedTextField4.Name = "underlinedTextField4";
            underlinedTextField4.Padding = new Padding(0, 4, 0, 8);
            underlinedTextField4.PasswordChar = '\0';
            underlinedTextField4.PlaceholderText = "Placeholder";
            underlinedTextField4.ReadOnly = false;
            underlinedTextField4.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            underlinedTextField4.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            underlinedTextField4.Size = new Size(188, 63);
            underlinedTextField4.TabIndex = 43;
            underlinedTextField4.TextForeColor = Color.FromArgb(30, 30, 30);
            underlinedTextField4.UnderlineSpacing = 2;
            underlinedTextField4.UseSystemPasswordChar = false;
            // 
            // underlinedTextField5
            // 
            underlinedTextField5.BackColor = Color.Transparent;
            underlinedTextField5.FocusedLineThickness = 3;
            underlinedTextField5.InheritParentBackColor = true;
            underlinedTextField5.LabelForeColor = Color.FromArgb(70, 70, 70);
            underlinedTextField5.LabelText = "Mã chuyến bay";
            underlinedTextField5.LineColor = Color.FromArgb(40, 40, 40);
            underlinedTextField5.LineColorFocused = Color.FromArgb(0, 92, 175);
            underlinedTextField5.LineThickness = 2;
            underlinedTextField5.Location = new Point(245, 174);
            underlinedTextField5.Name = "underlinedTextField5";
            underlinedTextField5.Padding = new Padding(0, 4, 0, 8);
            underlinedTextField5.PasswordChar = '\0';
            underlinedTextField5.PlaceholderText = "Placeholder";
            underlinedTextField5.ReadOnly = false;
            underlinedTextField5.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            underlinedTextField5.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            underlinedTextField5.Size = new Size(188, 63);
            underlinedTextField5.TabIndex = 42;
            underlinedTextField5.TextForeColor = Color.FromArgb(30, 30, 30);
            underlinedTextField5.UnderlineSpacing = 2;
            underlinedTextField5.UseSystemPasswordChar = false;
            // 
            // btnQuickCreateBookingTicket
            // 
            btnQuickCreateBookingTicket.AutoSize = true;
            btnQuickCreateBookingTicket.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnQuickCreateBookingTicket.BackColor = Color.FromArgb(155, 209, 243);
            btnQuickCreateBookingTicket.BorderColor = Color.FromArgb(40, 40, 40);
            btnQuickCreateBookingTicket.BorderThickness = 2;
            btnQuickCreateBookingTicket.CornerRadius = 22;
            btnQuickCreateBookingTicket.EnableHoverEffects = true;
            btnQuickCreateBookingTicket.FlatAppearance.BorderSize = 0;
            btnQuickCreateBookingTicket.FlatStyle = FlatStyle.Flat;
            btnQuickCreateBookingTicket.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnQuickCreateBookingTicket.ForeColor = Color.White;
            btnQuickCreateBookingTicket.HoverBackColor = Color.White;
            btnQuickCreateBookingTicket.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnQuickCreateBookingTicket.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnQuickCreateBookingTicket.Icon = null;
            btnQuickCreateBookingTicket.IconSize = new Size(22, 22);
            btnQuickCreateBookingTicket.IconSpacing = 10;
            btnQuickCreateBookingTicket.Location = new Point(884, 185);
            btnQuickCreateBookingTicket.Name = "btnQuickCreateBookingTicket";
            btnQuickCreateBookingTicket.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnQuickCreateBookingTicket.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnQuickCreateBookingTicket.NormalForeColor = Color.White;
            btnQuickCreateBookingTicket.Padding = new Padding(24, 10, 24, 10);
            btnQuickCreateBookingTicket.PreferredMaxWidth = 0;
            btnQuickCreateBookingTicket.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnQuickCreateBookingTicket.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnQuickCreateBookingTicket.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnQuickCreateBookingTicket.Size = new Size(112, 52);
            btnQuickCreateBookingTicket.TabIndex = 36;
            btnQuickCreateBookingTicket.Text = "Lọc";
            btnQuickCreateBookingTicket.TextAlign = ContentAlignment.MiddleLeft;
            btnQuickCreateBookingTicket.UseVisualStyleBackColor = false;
            btnQuickCreateBookingTicket.WordWrap = false;
            // 
            // dgvBookingsTicket
            // 
            dgvBookingsTicket.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(248, 250, 252);
            dgvBookingsTicket.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvBookingsTicket.BackgroundColor = Color.White;
            dgvBookingsTicket.BorderColor = Color.FromArgb(40, 40, 40);
            dgvBookingsTicket.BorderStyle = BorderStyle.None;
            dgvBookingsTicket.BorderThickness = 2;
            dgvBookingsTicket.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvBookingsTicket.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle5.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvBookingsTicket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvBookingsTicket.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBookingsTicket.CornerRadius = 16;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle6.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle6.SelectionForeColor = Color.White;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvBookingsTicket.DefaultCellStyle = dataGridViewCellStyle6;
            dgvBookingsTicket.EnableHeadersVisualStyles = false;
            dgvBookingsTicket.GridColor = Color.FromArgb(230, 235, 240);
            dgvBookingsTicket.HeaderBackColor = Color.White;
            dgvBookingsTicket.HeaderForeColor = Color.FromArgb(126, 185, 232);
            dgvBookingsTicket.HoverBackColor = Color.FromArgb(232, 245, 255);
            dgvBookingsTicket.Location = new Point(18, 348);
            dgvBookingsTicket.MultiSelect = false;
            dgvBookingsTicket.Name = "dgvBookingsTicket";
            dgvBookingsTicket.RowAltBackColor = Color.FromArgb(248, 250, 252);
            dgvBookingsTicket.RowBackColor = Color.White;
            dgvBookingsTicket.RowForeColor = Color.FromArgb(33, 37, 41);
            dgvBookingsTicket.RowHeadersVisible = false;
            dgvBookingsTicket.RowHeadersWidth = 51;
            dgvBookingsTicket.RowTemplate.Height = 40;
            dgvBookingsTicket.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dgvBookingsTicket.SelectionForeColor = Color.White;
            dgvBookingsTicket.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBookingsTicket.Size = new Size(1174, 333);
            dgvBookingsTicket.TabIndex = 35;
            // 
            // TicketOpsControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(underlinedComboBox1);
            Controls.Add(underlinedTextField3);
            Controls.Add(underlinedTextField4);
            Controls.Add(underlinedTextField5);
            Controls.Add(btnQuickCreateBookingTicket);
            Controls.Add(dgvBookingsTicket);
            Controls.Add(label1);
            Name = "TicketOpsControl";
            Size = new Size(1226, 774);
            ((System.ComponentModel.ISupportInitialize)dgvBookingsTicket).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Components.Inputs.UnderlinedComboBox underlinedComboBox1;
        private Components.Inputs.UnderlinedTextField underlinedTextField3;
        private Components.Inputs.UnderlinedTextField underlinedTextField4;
        private Components.Inputs.UnderlinedTextField underlinedTextField5;
        private Components.Buttons.PrimaryButton btnQuickCreateBookingTicket;
        private Components.Tables.TableCustom dgvBookingsTicket;
    }
}
