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
            dgvTicketNumberHistory = new GUI.Components.Tables.TableCustom();
            txtNumberHistoryTicket = new GUI.Components.Inputs.UnderlinedTextField();
            underlinedTextField2 = new GUI.Components.Inputs.UnderlinedTextField();
            btnSearchTicketHistory = new GUI.Components.Buttons.PrimaryButton();
            ((System.ComponentModel.ISupportInitialize)dgvTicketNumberHistory).BeginInit();
            SuspendLayout();
            // 
            // dgvTicketNumberHistory
            // 
            dgvTicketNumberHistory.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvTicketNumberHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvTicketNumberHistory.BackgroundColor = Color.White;
            dgvTicketNumberHistory.BorderColor = Color.FromArgb(40, 40, 40);
            dgvTicketNumberHistory.BorderStyle = BorderStyle.None;
            dgvTicketNumberHistory.BorderThickness = 2;
            dgvTicketNumberHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTicketNumberHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvTicketNumberHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvTicketNumberHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTicketNumberHistory.CornerRadius = 16;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvTicketNumberHistory.DefaultCellStyle = dataGridViewCellStyle3;
            dgvTicketNumberHistory.EnableHeadersVisualStyles = false;
            dgvTicketNumberHistory.GridColor = Color.FromArgb(230, 235, 240);
            dgvTicketNumberHistory.HeaderBackColor = Color.White;
            dgvTicketNumberHistory.HeaderForeColor = Color.FromArgb(126, 185, 232);
            dgvTicketNumberHistory.HoverBackColor = Color.FromArgb(232, 245, 255);
            dgvTicketNumberHistory.Location = new Point(135, 380);
            dgvTicketNumberHistory.MultiSelect = false;
            dgvTicketNumberHistory.Name = "dgvTicketNumberHistory";
            dgvTicketNumberHistory.RowAltBackColor = Color.FromArgb(248, 250, 252);
            dgvTicketNumberHistory.RowBackColor = Color.White;
            dgvTicketNumberHistory.RowForeColor = Color.FromArgb(33, 37, 41);
            dgvTicketNumberHistory.RowHeadersVisible = false;
            dgvTicketNumberHistory.RowHeadersWidth = 51;
            dgvTicketNumberHistory.RowTemplate.Height = 40;
            dgvTicketNumberHistory.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dgvTicketNumberHistory.SelectionForeColor = Color.White;
            dgvTicketNumberHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTicketNumberHistory.Size = new Size(944, 188);
            dgvTicketNumberHistory.TabIndex = 2;
            dgvTicketNumberHistory.CellContentClick += tableCustom1_CellContentClick;
            // 
            // txtNumberHistoryTicket
            // 
            txtNumberHistoryTicket.BackColor = Color.Transparent;
            txtNumberHistoryTicket.FocusedLineThickness = 3;
            txtNumberHistoryTicket.InheritParentBackColor = true;
            txtNumberHistoryTicket.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtNumberHistoryTicket.LabelText = "TiketNumber";
            txtNumberHistoryTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtNumberHistoryTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtNumberHistoryTicket.LineThickness = 2;
            txtNumberHistoryTicket.Location = new Point(133, 223);
            txtNumberHistoryTicket.Name = "txtNumberHistoryTicket";
            txtNumberHistoryTicket.Padding = new Padding(0, 4, 0, 8);
            txtNumberHistoryTicket.PasswordChar = '\0';
            txtNumberHistoryTicket.PlaceholderText = "Placeholder";
            txtNumberHistoryTicket.ReadOnly = false;
            txtNumberHistoryTicket.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtNumberHistoryTicket.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtNumberHistoryTicket.Size = new Size(188, 62);
            txtNumberHistoryTicket.TabIndex = 3;
            txtNumberHistoryTicket.TextForeColor = Color.FromArgb(30, 30, 30);
            txtNumberHistoryTicket.UnderlineSpacing = 2;
            txtNumberHistoryTicket.UseSystemPasswordChar = false;
            txtNumberHistoryTicket.Load += TicketNumberHistoryTicket_Load;
            // 
            // underlinedTextField2
            // 
            underlinedTextField2.BackColor = Color.Transparent;
            underlinedTextField2.FocusedLineThickness = 3;
            underlinedTextField2.InheritParentBackColor = true;
            underlinedTextField2.LabelForeColor = Color.FromArgb(70, 70, 70);
            underlinedTextField2.LabelText = "Nhãn";
            underlinedTextField2.LineColor = Color.FromArgb(40, 40, 40);
            underlinedTextField2.LineColorFocused = Color.FromArgb(0, 92, 175);
            underlinedTextField2.LineThickness = 2;
            underlinedTextField2.Location = new Point(379, 223);
            underlinedTextField2.Name = "underlinedTextField2";
            underlinedTextField2.Padding = new Padding(0, 4, 0, 8);
            underlinedTextField2.PasswordChar = '\0';
            underlinedTextField2.PlaceholderText = "Placeholder";
            underlinedTextField2.ReadOnly = false;
            underlinedTextField2.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            underlinedTextField2.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            underlinedTextField2.Size = new Size(188, 68);
            underlinedTextField2.TabIndex = 4;
            underlinedTextField2.TextForeColor = Color.FromArgb(30, 30, 30);
            underlinedTextField2.UnderlineSpacing = 2;
            underlinedTextField2.UseSystemPasswordChar = false;
            // 
            // btnSearchTicketHistory
            // 
            btnSearchTicketHistory.AutoSize = true;
            btnSearchTicketHistory.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSearchTicketHistory.BackColor = Color.FromArgb(155, 209, 243);
            btnSearchTicketHistory.BorderColor = Color.FromArgb(40, 40, 40);
            btnSearchTicketHistory.BorderThickness = 2;
            btnSearchTicketHistory.CornerRadius = 22;
            btnSearchTicketHistory.EnableHoverEffects = true;
            btnSearchTicketHistory.FlatAppearance.BorderSize = 0;
            btnSearchTicketHistory.FlatStyle = FlatStyle.Flat;
            btnSearchTicketHistory.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSearchTicketHistory.ForeColor = Color.White;
            btnSearchTicketHistory.HoverBackColor = Color.White;
            btnSearchTicketHistory.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnSearchTicketHistory.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnSearchTicketHistory.Icon = null;
            btnSearchTicketHistory.IconSize = new Size(22, 22);
            btnSearchTicketHistory.IconSpacing = 10;
            btnSearchTicketHistory.Location = new Point(813, 223);
            btnSearchTicketHistory.Name = "btnSearchTicketHistory";
            btnSearchTicketHistory.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnSearchTicketHistory.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnSearchTicketHistory.NormalForeColor = Color.White;
            btnSearchTicketHistory.Padding = new Padding(24, 10, 24, 10);
            btnSearchTicketHistory.PreferredMaxWidth = 0;
            btnSearchTicketHistory.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnSearchTicketHistory.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnSearchTicketHistory.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnSearchTicketHistory.Size = new Size(143, 52);
            btnSearchTicketHistory.TabIndex = 5;
            btnSearchTicketHistory.Text = "Search";
            btnSearchTicketHistory.TextAlign = ContentAlignment.MiddleLeft;
            btnSearchTicketHistory.UseVisualStyleBackColor = false;
            btnSearchTicketHistory.WordWrap = false;
            btnSearchTicketHistory.Click += btnSearchHistoryTicket;
            // 
            // HistoryTicketControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnSearchTicketHistory);
            Controls.Add(underlinedTextField2);
            Controls.Add(txtNumberHistoryTicket);
            Controls.Add(dgvTicketNumberHistory);
            Name = "HistoryTicketControl";
            Size = new Size(1219, 763);
            ((System.ComponentModel.ISupportInitialize)dgvTicketNumberHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Components.Tables.TableCustom dgvTicketNumberHistory;
        private Components.Inputs.UnderlinedTextField txtNumberHistoryTicket;
        private Components.Inputs.UnderlinedTextField underlinedTextField2;
        private Components.Buttons.PrimaryButton btnSearchTicketHistory;
    }
}
