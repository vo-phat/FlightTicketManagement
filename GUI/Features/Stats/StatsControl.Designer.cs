namespace GUI.Features.Stats
{
    partial class StatsControl
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
            this.tabControlStats = new System.Windows.Forms.TabControl();
            this.tabPageRevenue = new System.Windows.Forms.TabPage();
            this.tabPageFlights = new System.Windows.Forms.TabPage();
            this.tabPagePayments = new System.Windows.Forms.TabPage();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.labelYear = new System.Windows.Forms.Label();
            this.comboBoxYear = new System.Windows.Forms.ComboBox();
            this.labelMonth = new System.Windows.Forms.Label();
            this.comboBoxMonth = new System.Windows.Forms.ComboBox();
            this.chartRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvRevenueRoutes = new System.Windows.Forms.DataGridView();
            this.lblRevenueSummary = new System.Windows.Forms.Label();
            this.dgvFlights = new System.Windows.Forms.DataGridView();
            this.lblFlightSummary = new System.Windows.Forms.Label();
            this.chartPayments = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvPayments = new System.Windows.Forms.DataGridView();
            this.lblPaymentSummary = new System.Windows.Forms.Label();
            this.tabPageRoutesAircrafts = new System.Windows.Forms.TabPage();
            this.dgvTopRoutes = new System.Windows.Forms.DataGridView();
            this.dgvTopAircrafts = new System.Windows.Forms.DataGridView();
            this.lblRoutesAircraftsSummary = new System.Windows.Forms.Label();
            this.tabControlStats.SuspendLayout();
            this.tabPageRevenue.SuspendLayout();
            this.tabPageFlights.SuspendLayout();
            this.tabPagePayments.SuspendLayout();
            this.panelFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevenueRoutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFlights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPayments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).BeginInit();
            this.tabPageRoutesAircrafts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopRoutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopAircrafts)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlStats
            // 
            this.tabControlStats.Controls.Add(this.tabPageRevenue);
            this.tabControlStats.Controls.Add(this.tabPageFlights);
            this.tabControlStats.Controls.Add(this.tabPagePayments);
            this.tabControlStats.Controls.Add(this.tabPageRoutesAircrafts);
            this.tabControlStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlStats.Location = new System.Drawing.Point(0, 50);
            this.tabControlStats.Name = "tabControlStats";
            this.tabControlStats.SelectedIndex = 0;
            this.tabControlStats.Size = new System.Drawing.Size(800, 400);
            this.tabControlStats.TabIndex = 0;
            // 
            // tabPageRevenue
            // 
            this.tabPageRevenue.Controls.Add(this.dgvRevenueRoutes);
            this.tabPageRevenue.Controls.Add(this.lblRevenueSummary);
            this.tabPageRevenue.Controls.Add(this.chartRevenue);
            this.tabPageRevenue.Location = new System.Drawing.Point(4, 22);
            this.tabPageRevenue.Name = "tabPageRevenue";
            this.tabPageRevenue.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRevenue.Size = new System.Drawing.Size(792, 374);
            this.tabPageRevenue.TabIndex = 0;
            this.tabPageRevenue.Text = "Doanh thu";
            this.tabPageRevenue.UseVisualStyleBackColor = true;
            // 
            // tabPageFlights
            // 
            this.tabPageFlights.Controls.Add(this.dgvFlights);
            this.tabPageFlights.Controls.Add(this.lblFlightSummary);
            this.tabPageFlights.Location = new System.Drawing.Point(4, 22);
            this.tabPageFlights.Name = "tabPageFlights";
            this.tabPageFlights.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFlights.Size = new System.Drawing.Size(792, 374);
            this.tabPageFlights.TabIndex = 1;
            this.tabPageFlights.Text = "Chuyến bay";
            this.tabPageFlights.UseVisualStyleBackColor = true;
            // 
            // tabPagePayments
            // 
            this.tabPagePayments.Controls.Add(this.dgvPayments);
            this.tabPagePayments.Controls.Add(this.lblPaymentSummary);
            this.tabPagePayments.Controls.Add(this.chartPayments);
            this.tabPagePayments.Location = new System.Drawing.Point(4, 22);
            this.tabPagePayments.Name = "tabPagePayments";
            this.tabPagePayments.Size = new System.Drawing.Size(792, 374);
            this.tabPagePayments.TabIndex = 2;
            this.tabPagePayments.Text = "Thanh toán";
            this.tabPagePayments.UseVisualStyleBackColor = true;
            // 
            // panelFilters
            // 
            this.panelFilters.Controls.Add(this.comboBoxMonth);
            this.panelFilters.Controls.Add(this.labelMonth);
            this.panelFilters.Controls.Add(this.comboBoxYear);
            this.panelFilters.Controls.Add(this.labelYear);
            this.panelFilters.Controls.Add(this.btnLoad);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 0);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new System.Drawing.Size(800, 50);
            this.panelFilters.TabIndex = 1;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(350, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Tải dữ liệu";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // labelYear
            // 
            this.labelYear.AutoSize = true;
            this.labelYear.Location = new System.Drawing.Point(12, 17);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(29, 13);
            this.labelYear.TabIndex = 1;
            this.labelYear.Text = "Năm";
            // 
            // comboBoxYear
            // 
            this.comboBoxYear.FormattingEnabled = true;
            this.comboBoxYear.Location = new System.Drawing.Point(47, 14);
            this.comboBoxYear.Name = "comboBoxYear";
            this.comboBoxYear.Size = new System.Drawing.Size(121, 21);
            this.comboBoxYear.TabIndex = 2;
            // 
            // labelMonth
            // 
            this.labelMonth.AutoSize = true;
            this.labelMonth.Location = new System.Drawing.Point(180, 17);
            this.labelMonth.Name = "labelMonth";
            this.labelMonth.Size = new System.Drawing.Size(38, 13);
            this.labelMonth.TabIndex = 3;
            this.labelMonth.Text = "Tháng";
            // 
            // comboBoxMonth
            // 
            this.comboBoxMonth.FormattingEnabled = true;
            this.comboBoxMonth.Location = new System.Drawing.Point(224, 14);
            this.comboBoxMonth.Name = "comboBoxMonth";
            this.comboBoxMonth.Size = new System.Drawing.Size(120, 21);
            this.comboBoxMonth.TabIndex = 4;
            // 
            // chartRevenue
            // 
            this.chartRevenue.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartRevenue.Location = new System.Drawing.Point(3, 3);
            this.chartRevenue.Name = "chartRevenue";
            this.chartRevenue.Size = new System.Drawing.Size(786, 200);
            this.chartRevenue.TabIndex = 0;
            this.chartRevenue.Text = "chart1";
            // 
            // lblRevenueSummary
            // 
            this.lblRevenueSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRevenueSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblRevenueSummary.Location = new System.Drawing.Point(3, 203);
            this.lblRevenueSummary.Name = "lblRevenueSummary";
            this.lblRevenueSummary.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblRevenueSummary.Size = new System.Drawing.Size(786, 30);
            this.lblRevenueSummary.TabIndex = 2;
            this.lblRevenueSummary.Text = "Tổng doanh thu: 0 VND";
            // 
            // dgvRevenueRoutes
            // 
            this.dgvRevenueRoutes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRevenueRoutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRevenueRoutes.Location = new System.Drawing.Point(3, 233);
            this.dgvRevenueRoutes.Name = "dgvRevenueRoutes";
            this.dgvRevenueRoutes.Size = new System.Drawing.Size(786, 138);
            this.dgvRevenueRoutes.TabIndex = 1;
            // 
            // lblFlightSummary
            // 
            this.lblFlightSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFlightSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblFlightSummary.Location = new System.Drawing.Point(3, 3);
            this.lblFlightSummary.Name = "lblFlightSummary";
            this.lblFlightSummary.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblFlightSummary.Size = new System.Drawing.Size(786, 30);
            this.lblFlightSummary.TabIndex = 1;
            this.lblFlightSummary.Text = "Tổng chuyến bay: 0";
            // 
            // dgvFlights
            // 
            this.dgvFlights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFlights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFlights.Location = new System.Drawing.Point(3, 33);
            this.dgvFlights.Name = "dgvFlights";
            this.dgvFlights.Size = new System.Drawing.Size(786, 338);
            this.dgvFlights.TabIndex = 0;
            // 
            // chartPayments
            // 
            this.chartPayments.Dock = System.Windows.Forms.DockStyle.Left;
            this.chartPayments.Location = new System.Drawing.Point(0, 0);
            this.chartPayments.Name = "chartPayments";
            this.chartPayments.Size = new System.Drawing.Size(300, 374);
            this.chartPayments.TabIndex = 0;
            this.chartPayments.Text = "chart2";
            // 
            // lblPaymentSummary
            // 
            this.lblPaymentSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPaymentSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblPaymentSummary.Location = new System.Drawing.Point(300, 0);
            this.lblPaymentSummary.Name = "lblPaymentSummary";
            this.lblPaymentSummary.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblPaymentSummary.Size = new System.Drawing.Size(492, 30);
            this.lblPaymentSummary.TabIndex = 2;
            this.lblPaymentSummary.Text = "Tổng giao dịch: 0";
            // 
            // dgvPayments
            // 
            this.dgvPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPayments.Location = new System.Drawing.Point(300, 30);
            this.dgvPayments.Name = "dgvPayments";
            this.dgvPayments.Size = new System.Drawing.Size(492, 344);
            this.dgvPayments.TabIndex = 1;
            // 
            // tabPageRoutesAircrafts
            // 
            this.tabPageRoutesAircrafts.Controls.Add(this.dgvTopAircrafts);
            this.tabPageRoutesAircrafts.Controls.Add(this.dgvTopRoutes);
            this.tabPageRoutesAircrafts.Controls.Add(this.lblRoutesAircraftsSummary);
            this.tabPageRoutesAircrafts.Location = new System.Drawing.Point(4, 22);
            this.tabPageRoutesAircrafts.Name = "tabPageRoutesAircrafts";
            this.tabPageRoutesAircrafts.Size = new System.Drawing.Size(792, 374);
            this.tabPageRoutesAircrafts.TabIndex = 3;
            this.tabPageRoutesAircrafts.Text = "Tuyến bay & Máy bay";
            this.tabPageRoutesAircrafts.UseVisualStyleBackColor = true;
            // 
            // dgvTopRoutes
            // 
            this.dgvTopRoutes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTopRoutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTopRoutes.Location = new System.Drawing.Point(0, 30);
            this.dgvTopRoutes.Name = "dgvTopRoutes";
            this.dgvTopRoutes.Size = new System.Drawing.Size(792, 172);
            this.dgvTopRoutes.TabIndex = 1;
            // 
            // dgvTopAircrafts
            // 
            this.dgvTopAircrafts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTopAircrafts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvTopAircrafts.Location = new System.Drawing.Point(0, 202);
            this.dgvTopAircrafts.Name = "dgvTopAircrafts";
            this.dgvTopAircrafts.Size = new System.Drawing.Size(792, 172);
            this.dgvTopAircrafts.TabIndex = 2;
            // 
            // lblRoutesAircraftsSummary
            // 
            this.lblRoutesAircraftsSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRoutesAircraftsSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblRoutesAircraftsSummary.Location = new System.Drawing.Point(0, 0);
            this.lblRoutesAircraftsSummary.Name = "lblRoutesAircraftsSummary";
            this.lblRoutesAircraftsSummary.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblRoutesAircraftsSummary.Size = new System.Drawing.Size(792, 30);
            this.lblRoutesAircraftsSummary.TabIndex = 0;
            this.lblRoutesAircraftsSummary.Text = "Thống kê tuyến bay và máy bay";
            // 
            // StatsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlStats);
            this.Controls.Add(this.panelFilters);
            this.Name = "StatsControl";
            this.Size = new System.Drawing.Size(800, 450);
            this.tabControlStats.ResumeLayout(false);
            this.tabPageRevenue.ResumeLayout(false);
            this.tabPageFlights.ResumeLayout(false);
            this.tabPagePayments.ResumeLayout(false);
            this.tabPageRoutesAircrafts.ResumeLayout(false);
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevenueRoutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFlights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPayments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopRoutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopAircrafts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlStats;
        private System.Windows.Forms.TabPage tabPageRevenue;
        private System.Windows.Forms.TabPage tabPageFlights;
        private System.Windows.Forms.TabPage tabPagePayments;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label labelYear;
        private System.Windows.Forms.ComboBox comboBoxYear;
        private System.Windows.Forms.Label labelMonth;
        private System.Windows.Forms.ComboBox comboBoxMonth;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenue;
        private System.Windows.Forms.DataGridView dgvRevenueRoutes;
        private System.Windows.Forms.Label lblRevenueSummary;
        private System.Windows.Forms.DataGridView dgvFlights;
        private System.Windows.Forms.Label lblFlightSummary;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPayments;
        private System.Windows.Forms.DataGridView dgvPayments;
        private System.Windows.Forms.Label lblPaymentSummary;
        private System.Windows.Forms.TabPage tabPageRoutesAircrafts;
        private System.Windows.Forms.DataGridView dgvTopRoutes;
        private System.Windows.Forms.DataGridView dgvTopAircrafts;
        private System.Windows.Forms.Label lblRoutesAircraftsSummary;
    }
}