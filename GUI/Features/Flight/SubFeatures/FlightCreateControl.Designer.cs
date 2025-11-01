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
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.inputLayout = new System.Windows.Forms.TableLayoutPanel();
            this.txtFlightNumber = new GUI.Components.Inputs.UnderlinedTextField();
            this.cbAircraft = new GUI.Components.Inputs.UnderlinedComboBox();
            this.cbRoute = new GUI.Components.Inputs.UnderlinedComboBox();
            this.dtpDepartureTime = new GUI.Components.Inputs.DateTimePickerCustom();
            this.dtpArrivalTime = new GUI.Components.Inputs.DateTimePickerCustom();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSave = new GUI.Components.Buttons.PrimaryButton();
            this.previewTable = new GUI.Components.Tables.TableCustom();
            this.mainLayout.SuspendLayout();
            this.inputLayout.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(24, 20);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(24, 20, 24, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(273, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "➕ Tạo chuyến bay";
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.lblTitle, 0, 0);
            this.mainLayout.Controls.Add(this.inputLayout, 0, 1);
            this.mainLayout.Controls.Add(this.buttonPanel, 0, 2);
            this.mainLayout.Controls.Add(this.previewTable, 0, 3);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 4;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(800, 600);
            this.mainLayout.TabIndex = 0;
            // 
            // inputLayout
            // 
            this.inputLayout.AutoSize = true;
            this.inputLayout.ColumnCount = 2;
            this.inputLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.inputLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.inputLayout.Controls.Add(this.txtFlightNumber, 0, 0);
            this.inputLayout.Controls.Add(this.cbAircraft, 0, 1);
            this.inputLayout.Controls.Add(this.cbRoute, 1, 1);
            this.inputLayout.Controls.Add(this.dtpDepartureTime, 0, 2);
            this.inputLayout.Controls.Add(this.dtpArrivalTime, 1, 2);
            this.inputLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputLayout.Padding = new System.Windows.Forms.Padding(24, 12, 24, 12);
            this.inputLayout.Location = new System.Drawing.Point(3, 60);
            this.inputLayout.Name = "inputLayout";
            this.inputLayout.RowCount = 3;
            this.inputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.inputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.inputLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.inputLayout.Size = new System.Drawing.Size(794, 240);
            this.inputLayout.TabIndex = 1;
            // 
            // txtFlightNumber
            // 
            this.txtFlightNumber.BackColor = System.Drawing.Color.Transparent;
            this.inputLayout.SetColumnSpan(this.txtFlightNumber, 2);
            this.txtFlightNumber.LabelText = "Số hiệu chuyến bay (VD: VN123)";
            this.txtFlightNumber.Location = new System.Drawing.Point(27, 15);
            this.txtFlightNumber.Margin = new System.Windows.Forms.Padding(3, 3, 24, 3);
            this.txtFlightNumber.MinimumSize = new System.Drawing.Size(0, 56);
            this.txtFlightNumber.Name = "txtFlightNumber";
            this.txtFlightNumber.PlaceholderText = "";
            this.txtFlightNumber.Size = new System.Drawing.Size(400, 56);
            this.txtFlightNumber.TabIndex = 0;
            // 
            // cbAircraft
            // 
            this.cbAircraft.BackColor = System.Drawing.Color.Transparent;
            this.cbAircraft.DataSource = null;
            this.cbAircraft.DisplayMember = "";
            this.cbAircraft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbAircraft.LabelText = "Chọn máy bay";
            this.cbAircraft.Location = new System.Drawing.Point(27, 87);
            this.cbAircraft.Margin = new System.Windows.Forms.Padding(3, 3, 24, 3);
            this.cbAircraft.MinimumSize = new System.Drawing.Size(140, 56);
            this.cbAircraft.Name = "cbAircraft";
            this.cbAircraft.SelectedIndex = -1;
            this.cbAircraft.SelectedItem = null;
            this.cbAircraft.SelectedText = "";
            this.cbAircraft.SelectedValue = null;
            this.cbAircraft.Size = new System.Drawing.Size(346, 56);
            this.cbAircraft.TabIndex = 1;
            this.cbAircraft.ValueMember = "";
            // 
            // cbRoute
            // 
            this.cbRoute.BackColor = System.Drawing.Color.Transparent;
            this.cbRoute.DataSource = null;
            this.cbRoute.DisplayMember = "";
            this.cbRoute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbRoute.LabelText = "Chọn tuyến bay";
            this.cbRoute.Location = new System.Drawing.Point(400, 87);
            this.cbRoute.Margin = new System.Windows.Forms.Padding(3, 3, 24, 3);
            this.cbRoute.MinimumSize = new System.Drawing.Size(140, 56);
            this.cbRoute.Name = "cbRoute";
            this.cbRoute.SelectedIndex = -1;
            this.cbRoute.SelectedItem = null;
            this.cbRoute.SelectedText = "";
            this.cbRoute.SelectedValue = null;
            this.cbRoute.Size = new System.Drawing.Size(370, 56);
            this.cbRoute.TabIndex = 2;
            this.cbRoute.ValueMember = "";
            // 
            // dtpDepartureTime
            // 
            this.dtpDepartureTime.BackColor = System.Drawing.Color.Transparent;
            this.dtpDepartureTime.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpDepartureTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDepartureTime.LabelText = "Thời gian cất cánh";
            this.dtpDepartureTime.Location = new System.Drawing.Point(27, 159);
            this.dtpDepartureTime.Margin = new System.Windows.Forms.Padding(3, 3, 24, 3);
            this.dtpDepartureTime.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDepartureTime.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpDepartureTime.Name = "dtpDepartureTime";
            this.dtpDepartureTime.Padding = new System.Windows.Forms.Padding(0, 4, 0, 8);
            this.dtpDepartureTime.PlaceholderText = "";
            this.dtpDepartureTime.Size = new System.Drawing.Size(346, 66);
            this.dtpDepartureTime.TabIndex = 3;
            this.dtpDepartureTime.Value = new System.DateTime(2025, 10, 31, 16, 26, 48, 810);
            // 
            // dtpArrivalTime
            // 
            this.dtpArrivalTime.BackColor = System.Drawing.Color.Transparent;
            this.dtpArrivalTime.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpArrivalTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpArrivalTime.LabelText = "Thời gian hạ cánh";
            this.dtpArrivalTime.Location = new System.Drawing.Point(400, 159);
            this.dtpArrivalTime.Margin = new System.Windows.Forms.Padding(3, 3, 24, 3);
            this.dtpArrivalTime.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpArrivalTime.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpArrivalTime.Name = "dtpArrivalTime";
            this.dtpArrivalTime.Padding = new System.Windows.Forms.Padding(0, 4, 0, 8);
            this.dtpArrivalTime.PlaceholderText = "";
            this.dtpArrivalTime.Size = new System.Drawing.Size(370, 66);
            this.dtpArrivalTime.TabIndex = 4;
            this.dtpArrivalTime.Value = new System.DateTime(2025, 10, 31, 16, 26, 48, 811);
            // 
            // buttonPanel
            // 
            this.buttonPanel.AutoSize = true;
            this.buttonPanel.Controls.Add(this.btnSave);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonPanel.Location = new System.Drawing.Point(3, 303);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Padding = new System.Windows.Forms.Padding(24, 0, 24, 12);
            this.buttonPanel.Size = new System.Drawing.Size(794, 60);
            this.buttonPanel.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Location = new System.Drawing.Point(607, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 45);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "💾 Lưu chuyến bay";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // previewTable (Bảng xem trước, tạm thời vô hiệu hóa)
            // 
            this.previewTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewTable.Location = new System.Drawing.Point(24, 380);
            this.previewTable.Margin = new System.Windows.Forms.Padding(24, 12, 24, 24);
            this.previewTable.Name = "previewTable";
            this.previewTable.Size = new System.Drawing.Size(752, 196);
            this.previewTable.TabIndex = 3;
            this.previewTable.Visible = false; // Tạm ẩn đi
            // 
            // FlightCreateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(240)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.mainLayout);
            this.Name = "FlightCreateControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.FlightCreateControl_Load);
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            this.inputLayout.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            this.ResumeLayout(false);
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
    }
}