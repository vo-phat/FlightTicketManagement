using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Flight.SubFeatures
{
    partial class FlightCreateControl
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
            lblTitle = new Label();
            mainLayout = new TableLayoutPanel();
            inputLayout = new TableLayoutPanel();
            txtFlightNumber = new UnderlinedTextField();
            cbAircraft = new UnderlinedComboBox();
            cbRoute = new UnderlinedComboBox();
            dtpDepartureTime = new DateTimePickerCustom();
            dtpArrivalTime = new DateTimePickerCustom();
            cbStatus = new UnderlinedComboBox();
            buttonPanel = new FlowLayoutPanel();
            btnSave = new PrimaryButton();
            previewTable = new TableCustom();
            mainLayout.SuspendLayout();
            inputLayout.SuspendLayout();
            buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)previewTable).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.Location = new Point(24, 20);
            lblTitle.Margin = new Padding(24, 20, 24, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(262, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "➕ Tạo chuyến bay";
            // 
            // mainLayout
            // 
            mainLayout.ColumnCount = 1;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.Controls.Add(lblTitle, 0, 0);
            mainLayout.Controls.Add(inputLayout, 0, 1);
            mainLayout.Controls.Add(buttonPanel, 0, 2);
            mainLayout.Controls.Add(previewTable, 0, 3);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Location = new Point(0, 0);
            mainLayout.Name = "mainLayout";
            mainLayout.RowCount = 4;
            mainLayout.RowStyles.Add(new RowStyle());
            mainLayout.RowStyles.Add(new RowStyle());
            mainLayout.RowStyles.Add(new RowStyle());
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.Size = new Size(800, 600);
            mainLayout.TabIndex = 0;
            // 
            // inputLayout
            // 
            inputLayout.AutoSize = true;
            inputLayout.ColumnCount = 2;
            inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            inputLayout.Controls.Add(txtFlightNumber, 0, 0);
            inputLayout.Controls.Add(cbAircraft, 0, 1);
            inputLayout.Controls.Add(cbRoute, 1, 1);
            inputLayout.Controls.Add(dtpDepartureTime, 0, 2);
            inputLayout.Controls.Add(dtpArrivalTime, 1, 2);
            inputLayout.Controls.Add(cbStatus, 1, 0);
            inputLayout.Dock = DockStyle.Top;
            inputLayout.Location = new Point(3, 60);
            inputLayout.Name = "inputLayout";
            inputLayout.Padding = new Padding(24, 12, 24, 12);
            inputLayout.RowCount = 3;
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            inputLayout.Size = new Size(794, 240);
            inputLayout.TabIndex = 1;
            // 
            // txtFlightNumber
            // 
            txtFlightNumber.BackColor = Color.Transparent;
            txtFlightNumber.Dock = DockStyle.Fill;
            txtFlightNumber.FocusedLineThickness = 3;
            txtFlightNumber.InheritParentBackColor = true;
            txtFlightNumber.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtFlightNumber.LabelText = "Số hiệu chuyến bay";
            txtFlightNumber.LineColor = Color.FromArgb(40, 40, 40);
            txtFlightNumber.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtFlightNumber.LineThickness = 2;
            txtFlightNumber.Location = new Point(27, 15);
            txtFlightNumber.Margin = new Padding(3, 3, 24, 3);
            txtFlightNumber.MinimumSize = new Size(0, 56);
            txtFlightNumber.Name = "txtFlightNumber";
            txtFlightNumber.Padding = new Padding(0, 4, 0, 8);
            txtFlightNumber.PasswordChar = '\0';
            txtFlightNumber.PlaceholderText = "\"VD: VN123\"";
            txtFlightNumber.ReadOnly = false;
            txtFlightNumber.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtFlightNumber.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtFlightNumber.Size = new Size(346, 66);
            txtFlightNumber.TabIndex = 0;
            txtFlightNumber.TextForeColor = Color.FromArgb(30, 30, 30);
            txtFlightNumber.UnderlineSpacing = 2;
            txtFlightNumber.UseSystemPasswordChar = false;
            // 
            // cbAircraft
            // 
            cbAircraft.BackColor = Color.Transparent;
            cbAircraft.DataSource = null;
            cbAircraft.DisplayMember = "";
            cbAircraft.Dock = DockStyle.Fill;
            cbAircraft.LabelText = "Chọn máy bay";
            cbAircraft.Location = new Point(27, 87);
            cbAircraft.Margin = new Padding(3, 3, 24, 3);
            cbAircraft.MinimumSize = new Size(140, 56);
            cbAircraft.Name = "cbAircraft";
            cbAircraft.SelectedIndex = -1;
            cbAircraft.SelectedItem = null;
            cbAircraft.SelectedText = "";
            cbAircraft.SelectedValue = null;
            cbAircraft.Size = new Size(346, 66);
            cbAircraft.TabIndex = 1;
            cbAircraft.ValueMember = "";
            // 
            // cbRoute
            // 
            cbRoute.BackColor = Color.Transparent;
            cbRoute.DataSource = null;
            cbRoute.DisplayMember = "";
            cbRoute.Dock = DockStyle.Fill;
            cbRoute.LabelText = "Chọn tuyến bay";
            cbRoute.Location = new Point(400, 87);
            cbRoute.Margin = new Padding(3, 3, 24, 3);
            cbRoute.MinimumSize = new Size(140, 56);
            cbRoute.Name = "cbRoute";
            cbRoute.SelectedIndex = -1;
            cbRoute.SelectedItem = null;
            cbRoute.SelectedText = "";
            cbRoute.SelectedValue = null;
            cbRoute.Size = new Size(346, 66);
            cbRoute.TabIndex = 2;
            cbRoute.ValueMember = "";
            // 
            // dtpDepartureTime
            // 
            dtpDepartureTime.BackColor = Color.Transparent;
            dtpDepartureTime.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpDepartureTime.Dock = DockStyle.Fill;
            dtpDepartureTime.LabelText = "Thời gian cất cánh";
            dtpDepartureTime.Location = new Point(27, 159);
            dtpDepartureTime.Margin = new Padding(3, 3, 24, 3);
            dtpDepartureTime.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpDepartureTime.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpDepartureTime.Name = "dtpDepartureTime";
            dtpDepartureTime.Padding = new Padding(0, 4, 0, 8);
            dtpDepartureTime.PlaceholderText = "";
            dtpDepartureTime.Size = new Size(346, 66);
            dtpDepartureTime.TabIndex = 3;
            dtpDepartureTime.Value = new DateTime(2025, 10, 31, 16, 26, 48, 810);
            // 
            // dtpArrivalTime
            // 
            dtpArrivalTime.BackColor = Color.Transparent;
            dtpArrivalTime.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpArrivalTime.Dock = DockStyle.Fill;
            dtpArrivalTime.LabelText = "Thời gian hạ cánh";
            dtpArrivalTime.Location = new Point(400, 159);
            dtpArrivalTime.Margin = new Padding(3, 3, 24, 3);
            dtpArrivalTime.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpArrivalTime.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpArrivalTime.Name = "dtpArrivalTime";
            dtpArrivalTime.Padding = new Padding(0, 4, 0, 8);
            dtpArrivalTime.PlaceholderText = "";
            dtpArrivalTime.Size = new Size(346, 66);
            dtpArrivalTime.TabIndex = 4;
            dtpArrivalTime.Value = new DateTime(2025, 10, 31, 16, 26, 48, 811);
            //
            // cbStatus 
            //
            cbStatus.BackColor = Color.Transparent;
            cbStatus.DataSource = null;
            cbStatus.DisplayMember = "";
            cbStatus.Dock = DockStyle.Fill;
            cbStatus.LabelText = "Trạng thái";
            cbStatus.Location = new Point(400, 87);
            cbStatus.Margin = new Padding(3, 3, 24, 3);
            cbStatus.MinimumSize = new Size(140, 56);
            cbStatus.Name = "cbStatus";
            cbStatus.SelectedIndex = -1;
            cbStatus.SelectedItem = null;
            cbStatus.SelectedText = "";
            cbStatus.SelectedValue = null;
            cbStatus.Size = new Size(346, 66);
            cbStatus.TabIndex = 5;
            cbStatus.ValueMember = "";
            // 
            // buttonPanel
            // 
            buttonPanel.AutoSize = true;
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Location = new Point(3, 306);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Padding = new Padding(24, 0, 24, 12);
            buttonPanel.Size = new Size(794, 64);
            buttonPanel.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.AutoSize = true;
            btnSave.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSave.BackColor = Color.FromArgb(155, 209, 243);
            btnSave.BorderColor = Color.FromArgb(40, 40, 40);
            btnSave.BorderThickness = 2;
            btnSave.CornerRadius = 22;
            btnSave.EnableHoverEffects = true;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.HoverBackColor = Color.White;
            btnSave.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnSave.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnSave.Icon = null;
            btnSave.IconSize = new Size(22, 22);
            btnSave.IconSpacing = 10;
            btnSave.Location = new Point(521, 3);
            btnSave.Name = "btnSave";
            btnSave.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnSave.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnSave.NormalForeColor = Color.White;
            btnSave.Padding = new Padding(24, 10, 24, 10);
            btnSave.PreferredMaxWidth = 0;
            btnSave.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnSave.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnSave.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnSave.Size = new Size(222, 46);
            btnSave.TabIndex = 0;
            btnSave.Text = "💾 Lưu chuyến bay";
            btnSave.TextAlign = ContentAlignment.MiddleLeft;
            btnSave.UseVisualStyleBackColor = false;
            btnSave.WordWrap = false;
            btnSave.Click += btnSave_Click;
            // 
            // previewTable
            // 
            previewTable.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            previewTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            previewTable.BackgroundColor = Color.White;
            previewTable.BorderColor = Color.FromArgb(40, 40, 40);
            previewTable.BorderStyle = BorderStyle.None;
            previewTable.BorderThickness = 2;
            previewTable.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            previewTable.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            previewTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            previewTable.ColumnHeadersHeight = 44;
            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            previewTable.CornerRadius = 16;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            previewTable.DefaultCellStyle = dataGridViewCellStyle3;
            previewTable.Dock = DockStyle.Fill;
            previewTable.EnableHeadersVisualStyles = false;
            previewTable.GridColor = Color.FromArgb(230, 235, 240);
            previewTable.HeaderBackColor = Color.White;
            previewTable.HeaderForeColor = Color.FromArgb(126, 185, 232);
            previewTable.HoverBackColor = Color.FromArgb(232, 245, 255);
            previewTable.Location = new Point(24, 385);
            previewTable.Margin = new Padding(24, 12, 24, 24);
            previewTable.MultiSelect = false;
            previewTable.Name = "previewTable";
            previewTable.RowAltBackColor = Color.FromArgb(248, 250, 252);
            previewTable.RowBackColor = Color.White;
            previewTable.RowForeColor = Color.FromArgb(33, 37, 41);
            previewTable.RowHeadersVisible = false;
            previewTable.SelectionBackColor = Color.FromArgb(155, 209, 243);
            previewTable.SelectionForeColor = Color.White;
            previewTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            previewTable.Size = new Size(752, 191);
            previewTable.TabIndex = 3;
            previewTable.Visible = false;
            // 
            // FlightCreateControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(232, 240, 252);
            Controls.Add(mainLayout);
            Name = "FlightCreateControl";
            Size = new Size(800, 600);
            Load += FlightCreateControl_Load;
            mainLayout.ResumeLayout(false);
            mainLayout.PerformLayout();
            inputLayout.ResumeLayout(false);
            buttonPanel.ResumeLayout(false);
            buttonPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)previewTable).EndInit();
            ResumeLayout(false);
            
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.TableLayoutPanel inputLayout;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private GUI.Components.Buttons.PrimaryButton btnSave;
        private GUI.Components.Inputs.UnderlinedTextField txtFlightNumber;
        private GUI.Components.Inputs.UnderlinedComboBox cbAircraft;
        private GUI.Components.Inputs.UnderlinedComboBox cbRoute;
        private GUI.Components.Inputs.DateTimePickerCustom dtpDepartureTime;
        private GUI.Components.Inputs.DateTimePickerCustom dtpArrivalTime;
        private GUI.Components.Tables.TableCustom previewTable;
        private UnderlinedComboBox cbStatus;
    }
}