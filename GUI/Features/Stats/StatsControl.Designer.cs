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
            this.pnlHeaderStats = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRevenue = new GUI.Components.Buttons.PrimaryButton();
            this.btnFlights = new GUI.Components.Buttons.SecondaryButton();
            this.btnPayments = new GUI.Components.Buttons.SecondaryButton();
            this.btnRoutes = new GUI.Components.Buttons.SecondaryButton();
            this.btnAirplanes = new GUI.Components.Buttons.SecondaryButton();
            this.pnlContentStats = new System.Windows.Forms.Panel();
            this.pnlRevenue = new System.Windows.Forms.Panel();
            this.pnlFlights = new System.Windows.Forms.Panel();
            this.pnlPayments = new System.Windows.Forms.Panel();
            this.pnlRoutes = new System.Windows.Forms.Panel();
            this.pnlAirplanes = new System.Windows.Forms.Panel();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.labelYear = new System.Windows.Forms.Label();
            this.comboBoxYear = new System.Windows.Forms.ComboBox();
            this.labelMonth = new System.Windows.Forms.Label();
            this.comboBoxMonth = new System.Windows.Forms.ComboBox();
            this.chartRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvRevenueRoutes = new GUI.Components.Tables.TableCustom();
            this.lblRevenueSummary = new System.Windows.Forms.Label();
            this.dgvFlights = new GUI.Components.Tables.TableCustom();
            this.lblFlightSummary = new System.Windows.Forms.Label();
            this.chartPayments = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvPayments = new GUI.Components.Tables.TableCustom();
            this.lblPaymentSummary = new System.Windows.Forms.Label();
            this.dgvTopRoutes = new GUI.Components.Tables.TableCustom();
            this.dgvTopAircrafts = new GUI.Components.Tables.TableCustom();
            this.lblRoutesSummary = new System.Windows.Forms.Label();
            this.lblAircraftsSummary = new System.Windows.Forms.Label();
            this.pnlHeaderStats.SuspendLayout();
            this.pnlContentStats.SuspendLayout();
            this.pnlRevenue.SuspendLayout();
            this.pnlFlights.SuspendLayout();
            this.pnlPayments.SuspendLayout();
            this.pnlRoutes.SuspendLayout();
            this.pnlAirplanes.SuspendLayout();
            this.panelFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevenueRoutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFlights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartPayments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopRoutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopAircrafts)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeaderStats
            // 
            this.pnlHeaderStats.AutoSize = true;
            this.pnlHeaderStats.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlHeaderStats.BackColor = System.Drawing.Color.White;
            this.pnlHeaderStats.Controls.Add(this.btnRevenue);
            this.pnlHeaderStats.Controls.Add(this.btnFlights);
            this.pnlHeaderStats.Controls.Add(this.btnPayments);
            this.pnlHeaderStats.Controls.Add(this.btnRoutes);
            this.pnlHeaderStats.Controls.Add(this.btnAirplanes);
            this.pnlHeaderStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeaderStats.Location = new System.Drawing.Point(0, 50);
            this.pnlHeaderStats.Name = "pnlHeaderStats";
            this.pnlHeaderStats.Padding = new System.Windows.Forms.Padding(24, 12, 0, 0);
            this.pnlHeaderStats.Size = new System.Drawing.Size(800, 70);
            this.pnlHeaderStats.TabIndex = 2;
            this.pnlHeaderStats.WrapContents = false;
            // 
            // btnRevenue
            // 
            this.btnRevenue.AutoSize = true;
            this.btnRevenue.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRevenue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnRevenue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnRevenue.BorderThickness = 2;
            this.btnRevenue.CornerRadius = 22;
            this.btnRevenue.EnableHoverEffects = true;
            this.btnRevenue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRevenue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRevenue.ForeColor = System.Drawing.Color.White;
            this.btnRevenue.HoverBackColor = System.Drawing.Color.White;
            this.btnRevenue.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(92)))), ((int)(((byte)(175)))));
            this.btnRevenue.HoverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(92)))), ((int)(((byte)(175)))));
            this.btnRevenue.Icon = null;
            this.btnRevenue.IconSize = new System.Drawing.Size(22, 22);
            this.btnRevenue.IconSpacing = 10;
            this.btnRevenue.Location = new System.Drawing.Point(27, 15);
            this.btnRevenue.Name = "btnRevenue";
            this.btnRevenue.NormalBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnRevenue.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnRevenue.NormalForeColor = System.Drawing.Color.White;
            this.btnRevenue.Padding = new System.Windows.Forms.Padding(24, 10, 24, 10);
            this.btnRevenue.PreferredMaxWidth = 0;
            this.btnRevenue.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.btnRevenue.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(92)))), ((int)(((byte)(175)))));
            this.btnRevenue.PressedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(92)))), ((int)(((byte)(175)))));
            this.btnRevenue.Size = new System.Drawing.Size(169, 52);
            this.btnRevenue.TabIndex = 0;
            this.btnRevenue.Text = "Doanh thu";
            this.btnRevenue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRevenue.UseVisualStyleBackColor = false;
            this.btnRevenue.WordWrap = false;
            this.btnRevenue.Click += new System.EventHandler(this.btnRevenue_Click);
            // 
            // btnFlights
            // 
            this.btnFlights.AutoSize = true;
            this.btnFlights.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFlights.BackColor = System.Drawing.Color.White;
            this.btnFlights.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnFlights.BorderThickness = 2;
            this.btnFlights.CornerRadius = 22;
            this.btnFlights.EnableHoverEffects = true;
            this.btnFlights.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFlights.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnFlights.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnFlights.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnFlights.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(92)))), ((int)(((byte)(175)))));
            this.btnFlights.HoverForeColor = System.Drawing.Color.White;
            this.btnFlights.Icon = null;
            this.btnFlights.IconSize = new System.Drawing.Size(22, 22);
            this.btnFlights.IconSpacing = 10;
            this.btnFlights.Location = new System.Drawing.Point(202, 15);
            this.btnFlights.Name = "btnFlights";
            this.btnFlights.NormalBackColor = System.Drawing.Color.White;
            this.btnFlights.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnFlights.NormalForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnFlights.Padding = new System.Windows.Forms.Padding(24, 10, 24, 10);
            this.btnFlights.PreferredMaxWidth = 0;
            this.btnFlights.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(191)))), ((int)(((byte)(239)))));
            this.btnFlights.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(111)))), ((int)(((byte)(178)))));
            this.btnFlights.PressedForeColor = System.Drawing.Color.White;
            this.btnFlights.Size = new System.Drawing.Size(177, 52);
            this.btnFlights.TabIndex = 1;
            this.btnFlights.Text = "Chuyến bay";
            this.btnFlights.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFlights.UseVisualStyleBackColor = false;
            this.btnFlights.WordWrap = false;
            this.btnFlights.Click += new System.EventHandler(this.btnFlights_Click);
            // 
            // btnPayments
            // 
            this.btnPayments.AutoSize = true;
            this.btnPayments.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPayments.BackColor = System.Drawing.Color.White;
            this.btnPayments.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnPayments.BorderThickness = 2;
            this.btnPayments.CornerRadius = 22;
            this.btnPayments.EnableHoverEffects = true;
            this.btnPayments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayments.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnPayments.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnPayments.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnPayments.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(92)))), ((int)(((byte)(175)))));
            this.btnPayments.HoverForeColor = System.Drawing.Color.White;
            this.btnPayments.Icon = null;
            this.btnPayments.IconSize = new System.Drawing.Size(22, 22);
            this.btnPayments.IconSpacing = 10;
            this.btnPayments.Location = new System.Drawing.Point(385, 15);
            this.btnPayments.Name = "btnPayments";
            this.btnPayments.NormalBackColor = System.Drawing.Color.White;
            this.btnPayments.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnPayments.NormalForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnPayments.Padding = new System.Windows.Forms.Padding(24, 10, 24, 10);
            this.btnPayments.PreferredMaxWidth = 0;
            this.btnPayments.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(191)))), ((int)(((byte)(239)))));
            this.btnPayments.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(111)))), ((int)(((byte)(178)))));
            this.btnPayments.PressedForeColor = System.Drawing.Color.White;
            this.btnPayments.Size = new System.Drawing.Size(174, 52);
            this.btnPayments.TabIndex = 2;
            this.btnPayments.Text = "Thanh toán";
            this.btnPayments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPayments.UseVisualStyleBackColor = false;
            this.btnPayments.WordWrap = false;
            this.btnPayments.Click += new System.EventHandler(this.btnPayments_Click);
            // 
            // btnRoutes
            // 
            this.btnRoutes.AutoSize = true;
            this.btnRoutes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRoutes.BackColor = System.Drawing.Color.White;
            this.btnRoutes.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnRoutes.BorderThickness = 2;
            this.btnRoutes.CornerRadius = 22;
            this.btnRoutes.EnableHoverEffects = true;
            this.btnRoutes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoutes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRoutes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnRoutes.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnRoutes.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(92)))), ((int)(((byte)(175)))));
            this.btnRoutes.HoverForeColor = System.Drawing.Color.White;
            this.btnRoutes.Icon = null;
            this.btnRoutes.IconSize = new System.Drawing.Size(22, 22);
            this.btnRoutes.IconSpacing = 10;
            this.btnRoutes.Location = new System.Drawing.Point(565, 15);
            this.btnRoutes.Name = "btnRoutes";
            this.btnRoutes.NormalBackColor = System.Drawing.Color.White;
            this.btnRoutes.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnRoutes.NormalForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnRoutes.Padding = new System.Windows.Forms.Padding(24, 10, 24, 10);
            this.btnRoutes.PreferredMaxWidth = 0;
            this.btnRoutes.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(191)))), ((int)(((byte)(239)))));
            this.btnRoutes.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(111)))), ((int)(((byte)(178)))));
            this.btnRoutes.PressedForeColor = System.Drawing.Color.White;
            this.btnRoutes.Size = new System.Drawing.Size(161, 52);
            this.btnRoutes.TabIndex = 3;
            this.btnRoutes.Text = "Tuyến bay";
            this.btnRoutes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRoutes.UseVisualStyleBackColor = false;
            this.btnRoutes.WordWrap = false;
            this.btnRoutes.Click += new System.EventHandler(this.btnRoutes_Click);
            // 
            // btnAirplanes
            // 
            this.btnAirplanes.AutoSize = true;
            this.btnAirplanes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAirplanes.BackColor = System.Drawing.Color.White;
            this.btnAirplanes.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnAirplanes.BorderThickness = 2;
            this.btnAirplanes.CornerRadius = 22;
            this.btnAirplanes.EnableHoverEffects = true;
            this.btnAirplanes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAirplanes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnAirplanes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnAirplanes.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnAirplanes.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(92)))), ((int)(((byte)(175)))));
            this.btnAirplanes.HoverForeColor = System.Drawing.Color.White;
            this.btnAirplanes.Icon = null;
            this.btnAirplanes.IconSize = new System.Drawing.Size(22, 22);
            this.btnAirplanes.IconSpacing = 10;
            this.btnAirplanes.Location = new System.Drawing.Point(732, 15);
            this.btnAirplanes.Name = "btnAirplanes";
            this.btnAirplanes.NormalBackColor = System.Drawing.Color.White;
            this.btnAirplanes.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnAirplanes.NormalForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(209)))), ((int)(((byte)(243)))));
            this.btnAirplanes.Padding = new System.Windows.Forms.Padding(24, 10, 24, 10);
            this.btnAirplanes.PreferredMaxWidth = 0;
            this.btnAirplanes.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(191)))), ((int)(((byte)(239)))));
            this.btnAirplanes.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(111)))), ((int)(((byte)(178)))));
            this.btnAirplanes.PressedForeColor = System.Drawing.Color.White;
            this.btnAirplanes.Size = new System.Drawing.Size(145, 52);
            this.btnAirplanes.TabIndex = 4;
            this.btnAirplanes.Text = "Máy bay";
            this.btnAirplanes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAirplanes.UseVisualStyleBackColor = false;
            this.btnAirplanes.WordWrap = false;
            this.btnAirplanes.Click += new System.EventHandler(this.btnAirplanes_Click);
            // 
            // pnlContentStats
            // 
            this.pnlContentStats.Controls.Add(this.pnlRevenue);
            this.pnlContentStats.Controls.Add(this.pnlFlights);
            this.pnlContentStats.Controls.Add(this.pnlPayments);
            this.pnlContentStats.Controls.Add(this.pnlRoutes);
            this.pnlContentStats.Controls.Add(this.pnlAirplanes);
            this.pnlContentStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContentStats.Name = "pnlContentStats";
            this.pnlContentStats.TabIndex = 3;
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
            // 
            // pnlRevenue
            // 
            this.pnlRevenue.Controls.Add(this.dgvRevenueRoutes);
            this.pnlRevenue.Controls.Add(this.lblRevenueSummary);
            this.pnlRevenue.Controls.Add(this.chartRevenue);
            this.pnlRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRevenue.Name = "pnlRevenue";
            this.pnlRevenue.TabIndex = 0;
            // 
            // pnlFlights
            // 
            this.pnlFlights.Controls.Add(this.dgvFlights);
            this.pnlFlights.Controls.Add(this.lblFlightSummary);
            this.pnlFlights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFlights.Name = "pnlFlights";
            this.pnlFlights.TabIndex = 1;
            this.pnlFlights.BackColor = System.Drawing.Color.White;
            // 
            // lblFlightSummary
            // 
            this.lblFlightSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFlightSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblFlightSummary.Location = new System.Drawing.Point(0, 0);
            this.lblFlightSummary.Name = "lblFlightSummary";
            this.lblFlightSummary.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblFlightSummary.Size = new System.Drawing.Size(800, 40);
            this.lblFlightSummary.TabIndex = 1;
            this.lblFlightSummary.Text = "Tổng chuyến bay: 0";
            this.lblFlightSummary.AutoSize = false;
            this.lblFlightSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            // 
            // dgvFlights
            // 
            this.dgvFlights.AllowUserToAddRows = false;
            this.dgvFlights.AllowUserToDeleteRows = false;
            this.dgvFlights.AutoGenerateColumns = true;
            this.dgvFlights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFlights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFlights.Location = new System.Drawing.Point(0, 40);
            this.dgvFlights.Name = "dgvFlights";
            this.dgvFlights.ReadOnly = true;
            this.dgvFlights.RowHeadersVisible = false;
            this.dgvFlights.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFlights.Size = new System.Drawing.Size(800, 290);
            this.dgvFlights.TabIndex = 0;
            this.dgvFlights.MultiSelect = false;
            this.dgvFlights.BackgroundColor = System.Drawing.Color.White;
            this.dgvFlights.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFlights.RowTemplate.Height = 35;
            this.dgvFlights.ColumnHeadersHeight = 40;
            // 
            // lblFlightSummary
            // 
            this.lblFlightSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFlightSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblFlightSummary.Location = new System.Drawing.Point(0, 0);
            this.lblFlightSummary.Name = "lblFlightSummary";
            this.lblFlightSummary.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblFlightSummary.Size = new System.Drawing.Size(800, 40);
            this.lblFlightSummary.TabIndex = 1;
            this.lblFlightSummary.Text = "Tổng chuyến bay: 0";
            this.lblFlightSummary.AutoSize = false;
            this.lblFlightSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            // 
            // dgvFlights
            // 
            this.dgvFlights.AllowUserToAddRows = false;
            this.dgvFlights.AllowUserToDeleteRows = false;
            this.dgvFlights.AutoGenerateColumns = true;
            this.dgvFlights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFlights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFlights.Location = new System.Drawing.Point(0, 40);
            this.dgvFlights.Name = "dgvFlights";
            this.dgvFlights.ReadOnly = true;
            this.dgvFlights.RowHeadersVisible = false;
            this.dgvFlights.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFlights.Size = new System.Drawing.Size(800, 290);
            this.dgvFlights.TabIndex = 0;
            this.dgvFlights.MultiSelect = false;
            this.dgvFlights.BackgroundColor = System.Drawing.Color.White;
            this.dgvFlights.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFlights.RowTemplate.Height = 35;
            this.dgvFlights.ColumnHeadersHeight = 40;
            // 
            // lblFlightSummary
            // 
            this.lblFlightSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFlightSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblFlightSummary.Location = new System.Drawing.Point(0, 0);
            this.lblFlightSummary.Name = "lblFlightSummary";
            this.lblFlightSummary.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblFlightSummary.Size = new System.Drawing.Size(800, 40);
            this.lblFlightSummary.TabIndex = 1;
            this.lblFlightSummary.Text = "Tổng chuyến bay: 0";
            this.lblFlightSummary.AutoSize = false;
            this.lblFlightSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            // 
            // dgvFlights
            // 
            this.dgvFlights.AllowUserToAddRows = false;
            this.dgvFlights.AllowUserToDeleteRows = false;
            this.dgvFlights.AutoGenerateColumns = true;
            this.dgvFlights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFlights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFlights.Location = new System.Drawing.Point(0, 40);
            this.dgvFlights.Name = "dgvFlights";
            this.dgvFlights.ReadOnly = true;
            this.dgvFlights.RowHeadersVisible = false;
            this.dgvFlights.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFlights.Size = new System.Drawing.Size(800, 290);
            this.dgvFlights.TabIndex = 0;
            this.dgvFlights.MultiSelect = false;
            this.dgvFlights.BackgroundColor = System.Drawing.Color.White;
            this.dgvFlights.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFlights.RowTemplate.Height = 35;
            this.dgvFlights.ColumnHeadersHeight = 40;
            // 
            // pnlPayments
            // 
            this.pnlPayments.Controls.Add(this.dgvPayments);
            this.pnlPayments.Controls.Add(this.lblPaymentSummary);
            this.pnlPayments.Controls.Add(this.chartPayments);
            this.pnlPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPayments.Name = "pnlPayments";
            this.pnlPayments.TabIndex = 2;
            // 
            // pnlRoutes
            // 
            this.pnlRoutes.Controls.Add(this.dgvTopRoutes);
            this.pnlRoutes.Controls.Add(this.lblRoutesSummary);
            this.pnlRoutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRoutes.Name = "pnlRoutes";
            this.pnlRoutes.TabIndex = 3;
            // 
            // pnlAirplanes
            // 
            this.pnlAirplanes.Controls.Add(this.dgvTopAircrafts);
            this.pnlAirplanes.Controls.Add(this.lblAircraftsSummary);
            this.pnlAirplanes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAirplanes.Name = "pnlAirplanes";
            this.pnlAirplanes.TabIndex = 4;
            // 
            // chartRevenue
            // 
            this.chartRevenue.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartRevenue.Location = new System.Drawing.Point(0, 0);
            this.chartRevenue.Name = "chartRevenue";
            this.chartRevenue.Size = new System.Drawing.Size(800, 200);
            this.chartRevenue.TabIndex = 0;
            this.chartRevenue.Text = "chart1";
            // 
            // lblRevenueSummary
            // 
            this.lblRevenueSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRevenueSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblRevenueSummary.Location = new System.Drawing.Point(0, 200);
            this.lblRevenueSummary.Name = "lblRevenueSummary";
            this.lblRevenueSummary.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblRevenueSummary.Size = new System.Drawing.Size(800, 30);
            this.lblRevenueSummary.TabIndex = 2;
            this.lblRevenueSummary.Text = "Tổng doanh thu: 0 VND";
            // 
            // dgvRevenueRoutes
            // 
            this.dgvRevenueRoutes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRevenueRoutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRevenueRoutes.Location = new System.Drawing.Point(0, 230);
            this.dgvRevenueRoutes.Name = "dgvRevenueRoutes";
            this.dgvRevenueRoutes.Size = new System.Drawing.Size(800, 100);
            this.dgvRevenueRoutes.TabIndex = 1;
            // 
            // lblFlightSummary
            // 
            this.lblFlightSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFlightSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblFlightSummary.Location = new System.Drawing.Point(0, 0);
            this.lblFlightSummary.Name = "lblFlightSummary";
            this.lblFlightSummary.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblFlightSummary.Size = new System.Drawing.Size(800, 30);
            this.lblFlightSummary.TabIndex = 1;
            this.lblFlightSummary.Text = "Tổng chuyến bay: 0";
            // 
            // dgvFlights
            // 
            this.dgvFlights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFlights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFlights.Location = new System.Drawing.Point(0, 30);
            this.dgvFlights.Name = "dgvFlights";
            this.dgvFlights.Size = new System.Drawing.Size(800, 300);
            this.dgvFlights.TabIndex = 0;
            this.dgvFlights.AllowUserToAddRows = false;
            this.dgvFlights.AllowUserToDeleteRows = false;
            this.dgvFlights.ReadOnly = true;
            this.dgvFlights.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFlights.MultiSelect = false;
            this.dgvFlights.AutoGenerateColumns = true;
            this.dgvFlights.RowHeadersVisible = false;
            // 
            // chartPayments
            // 
            this.chartPayments.Dock = System.Windows.Forms.DockStyle.Left;
            this.chartPayments.Location = new System.Drawing.Point(0, 0);
            this.chartPayments.Name = "chartPayments";
            this.chartPayments.Size = new System.Drawing.Size(300, 330);
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
            this.dgvPayments.Size = new System.Drawing.Size(500, 300);
            this.dgvPayments.TabIndex = 1;
            // 
            // dgvTopRoutes
            // 
            this.dgvTopRoutes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTopRoutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTopRoutes.Location = new System.Drawing.Point(0, 30);
            this.dgvTopRoutes.Name = "dgvTopRoutes";
            this.dgvTopRoutes.Size = new System.Drawing.Size(800, 300);
            this.dgvTopRoutes.TabIndex = 1;
            // 
            // dgvTopAircrafts
            // 
            this.dgvTopAircrafts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTopAircrafts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTopAircrafts.Location = new System.Drawing.Point(0, 30);
            this.dgvTopAircrafts.Name = "dgvTopAircrafts";
            this.dgvTopAircrafts.Size = new System.Drawing.Size(800, 300);
            this.dgvTopAircrafts.TabIndex = 2;
            // 
            // lblRoutesSummary
            // 
            this.lblRoutesSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRoutesSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblRoutesSummary.Location = new System.Drawing.Point(0, 0);
            this.lblRoutesSummary.Name = "lblRoutesSummary";
            this.lblRoutesSummary.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblRoutesSummary.Size = new System.Drawing.Size(800, 30);
            this.lblRoutesSummary.TabIndex = 0;
            this.lblRoutesSummary.Text = "Thống kê tuyến bay";
            // 
            // lblAircraftsSummary
            // 
            this.lblAircraftsSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAircraftsSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblAircraftsSummary.Location = new System.Drawing.Point(0, 0);
            this.lblAircraftsSummary.Name = "lblAircraftsSummary";
            this.lblAircraftsSummary.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblAircraftsSummary.Size = new System.Drawing.Size(800, 30);
            this.lblAircraftsSummary.TabIndex = 0;
            this.lblAircraftsSummary.Text = "Thống kê máy bay";
            // 
            // StatsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.pnlContentStats);
            this.Controls.Add(this.pnlHeaderStats);
            this.Controls.Add(this.panelFilters);
            this.Name = "StatsControl";
            this.Size = new System.Drawing.Size(800, 450);
            this.pnlHeaderStats.ResumeLayout(false);
            this.pnlHeaderStats.PerformLayout();
            this.pnlContentStats.ResumeLayout(false);
            this.pnlRevenue.ResumeLayout(false);
            this.pnlFlights.ResumeLayout(false);
            this.pnlPayments.ResumeLayout(false);
            this.pnlRoutes.ResumeLayout(false);
            this.pnlAirplanes.ResumeLayout(false);
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

        private System.Windows.Forms.FlowLayoutPanel pnlHeaderStats;
        private GUI.Components.Buttons.PrimaryButton btnRevenue;
        private GUI.Components.Buttons.SecondaryButton btnFlights;
        private GUI.Components.Buttons.SecondaryButton btnPayments;
        private GUI.Components.Buttons.SecondaryButton btnRoutes;
        private GUI.Components.Buttons.SecondaryButton btnAirplanes;
        private System.Windows.Forms.Panel pnlContentStats;
        private System.Windows.Forms.Panel pnlRevenue;
        private System.Windows.Forms.Panel pnlFlights;
        private System.Windows.Forms.Panel pnlPayments;
        private System.Windows.Forms.Panel pnlRoutes;
        private System.Windows.Forms.Panel pnlAirplanes;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label labelYear;
        private System.Windows.Forms.ComboBox comboBoxYear;
        private System.Windows.Forms.Label labelMonth;
        private System.Windows.Forms.ComboBox comboBoxMonth;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenue;
        private GUI.Components.Tables.TableCustom dgvRevenueRoutes;
        private System.Windows.Forms.Label lblRevenueSummary;
        private GUI.Components.Tables.TableCustom dgvFlights;
        private System.Windows.Forms.Label lblFlightSummary;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPayments;
        private GUI.Components.Tables.TableCustom dgvPayments;
        private System.Windows.Forms.Label lblPaymentSummary;
        private GUI.Components.Tables.TableCustom dgvTopRoutes;
        private GUI.Components.Tables.TableCustom dgvTopAircrafts;
        private System.Windows.Forms.Label lblRoutesSummary;
        private System.Windows.Forms.Label lblAircraftsSummary;
    }
}