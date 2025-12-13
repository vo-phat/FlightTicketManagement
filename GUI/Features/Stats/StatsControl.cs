using BUS.Stats;
using DTO.Stats;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI.Features.Stats
{
    public partial class StatsControl : UserControl
    {
        public StatsControl()
        {
            InitializeComponent();
            InitializeCharts();
            InitializeFilterControls();
            InitializeSummaryLabels();
            StyleControls(); // Apply visual styling
            ForceResponsiveLayout(); // Force panels to fill screen
            
            // Wire up button click events
            btnLoad.Click += btnLoad_Click;
            btnRevenue.Click += btnRevenue_Click;
            btnFlights.Click += btnFlights_Click;
            btnPayments.Click += btnPayments_Click;
            btnRoutes.Click += btnRoutes_Click;
            btnAirplanes.Click += btnAirplanes_Click;
            
            // Show Revenue view by default
            ShowView("Revenue");
            
            // Auto-load data for current month/year
            LoadCurrentData();
        }

        private void StyleControls()
        {
            // Common DataGridView styling
            DataGridView[] grids = { dgvRevenueRoutes, dgvFlights, dgvPayments, dgvTopRoutes, dgvTopAircrafts };
            foreach (var dgv in grids)
            {
                dgv.BackgroundColor = System.Drawing.Color.White;
                dgv.BorderStyle = BorderStyle.None;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                dgv.ColumnHeadersHeight = 35;
                dgv.EnableHeadersVisualStyles = false;
                dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(230, 240, 255);
                dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                dgv.RowHeadersVisible = false;
                dgv.RowTemplate.Height = 30;
                dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            }

            // Chart styling
            Chart[] charts = { chartRevenue, chartPayments };
            foreach (var chart in charts)
            {
                chart.BackColor = System.Drawing.Color.White;
                foreach (var area in chart.ChartAreas)
                {
                    area.BackColor = System.Drawing.Color.White;
                }
            }
        }

        private void ForceResponsiveLayout()
        {
            // FIX: Make StatsControl fill parent container
            // Don't set Size.Empty as it causes Chart height = 0 crash!
            this.Dock = DockStyle.Fill; // Fill parent container
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.AutoSize = false;
            
            // COMPLETE REBUILD of Flights panel to ensure responsiveness
            // Clear existing controls from pnlFlights
            pnlFlights.Controls.Clear();
            pnlFlights.SuspendLayout();
            
            // Rebuild with TableLayoutPanel for guaranteed responsive behavior
            var tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = System.Drawing.Color.White
            };
            
            // Define rows: Summary (30px), DataGrid (fill remaining)
            tableLayout.RowStyles.Clear();
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Summary row
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Grid fills rest
            
            // Column takes 100% width
            tableLayout.ColumnStyles.Clear();
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            
            // Add summary label to row 0
            lblFlightSummary.Dock = DockStyle.Fill;
            lblFlightSummary.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            lblFlightSummary.AutoSize = false;
            lblFlightSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            tableLayout.Controls.Add(lblFlightSummary, 0, 0);
            
            // Add DataGridView to row 1
            dgvFlights.Dock = DockStyle.Fill;
            dgvFlights.MinimumSize = new System.Drawing.Size(0, 0);
            dgvFlights.AutoSize = false;
            tableLayout.Controls.Add(dgvFlights, 0, 1);
            
            // Add table layout to panel
            pnlFlights.Controls.Add(tableLayout);
            pnlFlights.ResumeLayout();
            
            // Apply same approach to other panels
            // Force all panels to be responsive
            pnlContentStats.MinimumSize = new System.Drawing.Size(0, 0);
            pnlContentStats.AutoSize = false;
            
            // All stats panels
            Panel[] panels = { pnlRevenue, pnlFlights, pnlPayments, pnlRoutes, pnlAirplanes };
            foreach (var panel in panels)
            {
                panel.MinimumSize = new System.Drawing.Size(0, 0);
                panel.AutoSize = false;
                panel.Dock = DockStyle.Fill;
            }
            
            // Charts
            Chart[] charts = { chartRevenue, chartPayments };
            foreach (var chart in charts)
            {
                chart.Dock = DockStyle.Top;
                chart.Height = 250;
            }
        }

        private void ShowView(string viewName)
        {
            // Hide all panels
            pnlRevenue.Visible = false;
            pnlFlights.Visible = false;
            pnlPayments.Visible = false;
            pnlRoutes.Visible = false;
            pnlAirplanes.Visible = false;

            // Reset all buttons to secondary style (white background, blue text)
            // Both PrimaryButton and SecondaryButton use NormalBackColor/NormalForeColor
            btnRevenue.NormalBackColor = System.Drawing.Color.White;
            btnRevenue.NormalForeColor = System.Drawing.Color.FromArgb(155, 209, 243);
            btnRevenue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnRevenue.Invalidate();
            
            btnFlights.NormalBackColor = System.Drawing.Color.White;
            btnFlights.NormalForeColor = System.Drawing.Color.FromArgb(155, 209, 243);
            btnFlights.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnFlights.Invalidate();
            
            btnPayments.NormalBackColor = System.Drawing.Color.White;
            btnPayments.NormalForeColor = System.Drawing.Color.FromArgb(155, 209, 243);
            btnPayments.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnPayments.Invalidate();
            
            btnRoutes.NormalBackColor = System.Drawing.Color.White;
            btnRoutes.NormalForeColor = System.Drawing.Color.FromArgb(155, 209, 243);
            btnRoutes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnRoutes.Invalidate();
            
            btnAirplanes.NormalBackColor = System.Drawing.Color.White;
            btnAirplanes.NormalForeColor = System.Drawing.Color.FromArgb(155, 209, 243);
            btnAirplanes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnAirplanes.Invalidate();

            // Show selected panel and highlight selected button (blue background, white text, bold font)
            // Also auto-load data if not already loaded
            switch (viewName)
            {
                case "Revenue":
                    pnlRevenue.Visible = true;
                    btnRevenue.NormalBackColor = System.Drawing.Color.FromArgb(155, 209, 243);
                    btnRevenue.NormalForeColor = System.Drawing.Color.White;
                    btnRevenue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
                    btnRevenue.Invalidate();
                    // Auto-load revenue data if not loaded
                    if (dgvRevenueRoutes.Rows.Count == 0 && comboBoxYear.SelectedItem != null)
                    {
                        int year = (int)comboBoxYear.SelectedItem;
                        LoadRevenueReport(year);
                    }
                    break;
                case "Flights":
                    pnlFlights.Visible = true;
                    btnFlights.NormalBackColor = System.Drawing.Color.FromArgb(155, 209, 243);
                    btnFlights.NormalForeColor = System.Drawing.Color.White;
                    btnFlights.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
                    btnFlights.Invalidate();
                    // Auto-load flights data if not loaded
                    if (dgvFlights.Rows.Count == 0 && comboBoxYear.SelectedItem != null && comboBoxMonth.SelectedItem != null)
                    {
                        int year = (int)comboBoxYear.SelectedItem;
                        int month = ((dynamic)comboBoxMonth.SelectedItem).Value;
                        LoadFlightStatsReport(year, month);
                    }
                    break;
                case "Payments":
                    pnlPayments.Visible = true;
                    btnPayments.NormalBackColor = System.Drawing.Color.FromArgb(155, 209, 243);
                    btnPayments.NormalForeColor = System.Drawing.Color.White;
                    btnPayments.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
                    btnPayments.Invalidate();
                    // Auto-load payments data if not loaded
                    if (dgvPayments.Rows.Count == 0 && comboBoxYear.SelectedItem != null && comboBoxMonth.SelectedItem != null)
                    {
                        int year = (int)comboBoxYear.SelectedItem;
                        int month = ((dynamic)comboBoxMonth.SelectedItem).Value;
                        LoadPaymentStatsReport(year, month);
                    }
                    break;
                case "Routes":
                    pnlRoutes.Visible = true;
                    btnRoutes.NormalBackColor = System.Drawing.Color.FromArgb(155, 209, 243);
                    btnRoutes.NormalForeColor = System.Drawing.Color.White;
                    btnRoutes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
                    btnRoutes.Invalidate();
                    // Auto-load routes data if not loaded
                    if (dgvTopRoutes.Rows.Count == 0 && comboBoxYear.SelectedItem != null)
                    {
                        int year = (int)comboBoxYear.SelectedItem;
                        LoadRoutesReport(year);
                    }
                    break;
                case "Airplanes":
                    pnlAirplanes.Visible = true;
                    btnAirplanes.NormalBackColor = System.Drawing.Color.FromArgb(155, 209, 243);
                    btnAirplanes.NormalForeColor = System.Drawing.Color.White;
                    btnAirplanes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
                    btnAirplanes.Invalidate();
                    // Auto-load aircrafts data if not loaded
                    if (dgvTopAircrafts.Rows.Count == 0 && comboBoxYear.SelectedItem != null)
                    {
                        int year = (int)comboBoxYear.SelectedItem;
                        LoadAircraftsReport(year);
                    }
                    break;
            }
        }

        private void btnRevenue_Click(object sender, EventArgs e)
        {
            ShowView("Revenue");
        }

        private void btnFlights_Click(object sender, EventArgs e)
        {
            ShowView("Flights");
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            ShowView("Payments");
        }

        private void btnRoutes_Click(object sender, EventArgs e)
        {
            ShowView("Routes");
            // Load routes data if not already loaded
            if (dgvTopRoutes.Rows.Count == 0 && comboBoxYear.SelectedItem != null)
            {
                int year = (int)comboBoxYear.SelectedItem;
                LoadRoutesReport(year);
            }
        }

        private void btnAirplanes_Click(object sender, EventArgs e)
        {
            ShowView("Airplanes");
            // Load airplanes data if not already loaded
            if (dgvTopAircrafts.Rows.Count == 0 && comboBoxYear.SelectedItem != null)
            {
                int year = (int)comboBoxYear.SelectedItem;
                LoadAircraftsReport(year);
            }
        }

        private void InitializeCharts()
        {
            // Initialize Revenue Chart
            if (chartRevenue.ChartAreas.Count == 0)
            {
                chartRevenue.ChartAreas.Add(new ChartArea("MainArea"));
            }
            
            // Initialize Payment Chart
            if (chartPayments.ChartAreas.Count == 0)
            {
                chartPayments.ChartAreas.Add(new ChartArea("MainArea"));
            }
        }

        private void InitializeFilterControls()
        {
            // Populate years (e.g., last 5 years)
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear; i >= currentYear - 5; i--)
            {
                comboBoxYear.Items.Add(i);
            }
            comboBoxYear.SelectedIndex = 0;

            // Add "Cả năm" option first (value = 0)
            comboBoxMonth.Items.Add(new { Text = "Cả năm", Value = 0 });
            
            // Populate months
            for (int i = 1; i <= 12; i++)
            {
                comboBoxMonth.Items.Add(new { Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i), Value = i });
            }
            comboBoxMonth.DisplayMember = "Text";
            comboBoxMonth.ValueMember = "Value";
            comboBoxMonth.SelectedIndex = 0; // Default to "Cả năm"
        }

        private void InitializeSummaryLabels()
        {
            // Configure summary labels to be visible and properly formatted
            lblRevenueSummary.AutoSize = false;
            lblRevenueSummary.Height = 30;
            lblRevenueSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblRevenueSummary.Text = "Chọn năm/tháng và nhấn 'Tải dữ liệu'";
            
            lblFlightSummary.AutoSize = false;
            lblFlightSummary.Height = 30;
            lblFlightSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblFlightSummary.Text = "Chọn năm/tháng và nhấn 'Tải dữ liệu'";
            
            lblPaymentSummary.AutoSize = false;
            lblPaymentSummary.Height = 30;
            lblPaymentSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblPaymentSummary.Text = "Chọn năm/tháng và nhấn 'Tải dữ liệu'";
            
            lblRoutesSummary.AutoSize = false;
            lblRoutesSummary.Height = 30;
            lblRoutesSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblRoutesSummary.Text = "Thống kê tuyến bay";
            
            lblAircraftsSummary.AutoSize = false;
            lblAircraftsSummary.Height = 30;
            lblAircraftsSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblAircraftsSummary.Text = "Thống kê máy bay";
        }

        private void LoadCurrentData()
        {
            try
            {
                if (comboBoxYear.SelectedItem != null && comboBoxMonth.SelectedItem != null)
                {
                    int year = (int)comboBoxYear.SelectedItem;
                    int month = ((dynamic)comboBoxMonth.SelectedItem).Value;

                    LoadRevenueReport(year);
                    LoadFlightStatsReport(year, month);
                    LoadPaymentStatsReport(year, month);
                    LoadRoutesAndAircraftsReport(year);
                }
            }
            catch (Exception ex)
            {
                // Silent fail on auto-load - user can manually click "Tải dữ liệu" button
                Console.WriteLine($"Auto-load failed: {ex.Message}");
                // Set default messages
                lblRevenueSummary.Text = "Nhấn 'Tải dữ liệu' để xem báo cáo";
                lblFlightSummary.Text = "Nhấn 'Tải dữ liệu' để xem báo cáo";
                lblPaymentSummary.Text = "Nhấn 'Tải dữ liệu' để xem báo cáo";
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (comboBoxYear.SelectedItem == null || comboBoxMonth.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn năm và tháng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int year = (int)comboBoxYear.SelectedItem;
            int month = ((dynamic)comboBoxMonth.SelectedItem).Value;

            LoadRevenueReport(year);
            LoadFlightStatsReport(year, month);
            LoadPaymentStatsReport(year, month);
            LoadRoutesReport(year);
            LoadAircraftsReport(year);
        }

        private void LoadRevenueReport(int year)
        {
            var result = StatsBUS.Instance.GetRevenueReport(year);
            if (result.Success && result.Data is RevenueReportViewModel report)
            {
                // Update summary label - REMOVED as per user request
                // lblRevenueSummary.Text = $"Tổng doanh thu: {report.TotalRevenue:N0} VND  |  Tổng giao dịch: {report.TotalTransactions:N0}";
                lblRevenueSummary.Visible = false; // Hide the label
                
                // Chart
                chartRevenue.Series.Clear();
                chartRevenue.Titles.Clear();
                chartRevenue.Titles.Add($"Doanh thu hàng tháng năm {year}");
                var series = new Series("Doanh thu")
                {
                    ChartType = SeriesChartType.Column
                };
                chartRevenue.Series.Add(series);

                var monthlyData = (DataTable)report.MonthlyBreakdown;
                // Create a full list of months for the chart
                var allMonths = Enumerable.Range(1, 12).Select(m => new { Month = m, Revenue = 0m }).ToList();

                foreach (DataRow row in monthlyData.Rows)
                {
                    int month = Convert.ToInt32(row["Thang"]);
                    decimal revenue = Convert.ToDecimal(row["DoanhThu"]);
                    var monthData = allMonths.FirstOrDefault(m => m.Month == month);
                    if (monthData != null)
                    {
                        allMonths[month - 1] = new { Month = month, Revenue = revenue };
                    }
                }

                foreach (var item in allMonths)
                {
                    series.Points.AddXY(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item.Month), item.Revenue);
                }

                chartRevenue.ChartAreas[0].AxisX.Title = "Tháng";
                chartRevenue.ChartAreas[0].AxisY.Title = "Doanh thu (VND)";
                chartRevenue.ChartAreas[0].AxisY.LabelStyle.Format = "N0";

                // DataGridView for routes
                dgvRevenueRoutes.DataSource = report.RouteBreakdown;
                if (dgvRevenueRoutes.Columns.Count > 0)
                {
                    dgvRevenueRoutes.Columns["TuyenBay"].HeaderText = "Tuyến bay";
                    dgvRevenueRoutes.Columns["DoanhThu"].HeaderText = "Doanh thu";
                    dgvRevenueRoutes.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";
                    dgvRevenueRoutes.Columns["SoChuyenBay"].HeaderText = "Số chuyến bay";
                    dgvRevenueRoutes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblRevenueSummary.Text = "Không có dữ liệu";
            }
        }

        private void LoadFlightStatsReport(int year, int month)
        {
            // If month = 0, load yearly view
            if (month == 0)
            {
                LoadFlightStatsReportYearly(year);
                return;
            }
            
            var result = StatsBUS.Instance.GetFlightStatsReport(year, month);
            if (result.Success && result.Data is FlightStatsReportViewModel report)
            {
                // Update summary label
                lblFlightSummary.Text = $"📊 Tháng {month}/{year}  •  " +
                                       $"Tổng chuyến bay: {report.TotalFlights:N0}  •  " +
                                       $"Tổng hành khách: {report.TotalPassengers:N0}  •  " +
                                       $"Tỷ lệ lấp đầy TB: {report.AverageOccupancyRate:N2}%  •  " +
                                       $"Doanh thu: {report.TotalRevenue:N0} VND";
                
                dgvFlights.DataSource = report.FlightDetails;
                
                // Simplified column configuration - auto-fill like Routes table
                if (dgvFlights.Columns.Count > 0)
                {
                    // Hide unnecessary columns
                    dgvFlights.Columns["FlightId"].Visible = false;
                    dgvFlights.Columns["TotalPassengers"].Visible = false;
                    
                    // Set Vietnamese headers with formatting
                    dgvFlights.Columns["FlightCode"].HeaderText = "Số hiệu CB";
                    dgvFlights.Columns["Route"].HeaderText = "Tuyến bay";
                    dgvFlights.Columns["DepartureTime"].HeaderText = "Giờ khởi hành";
                    dgvFlights.Columns["DepartureTime"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    dgvFlights.Columns["ArrivalTime"].HeaderText = "Giờ đến";
                    dgvFlights.Columns["ArrivalTime"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    dgvFlights.Columns["TotalSeats"].HeaderText = "Tổng ghế";
                    dgvFlights.Columns["BookedSeats"].HeaderText = "Đã đặt";
                    dgvFlights.Columns["OccupancyRate"].HeaderText = "Lấp đầy %";
                    dgvFlights.Columns["OccupancyRate"].DefaultCellStyle.Format = "N2";
                    dgvFlights.Columns["Revenue"].HeaderText = "Doanh thu (VND)";
                    dgvFlights.Columns["Revenue"].DefaultCellStyle.Format = "N0";
                    
                    // Auto-fill columns to use available width
                    dgvFlights.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvFlights.DataSource = null;
                lblFlightSummary.Text = "Không có dữ liệu";
            }
        }

        private void LoadFlightStatsReportYearly(int year)
        {
            var result = StatsBUS.Instance.GetFlightStatsReportYearly(year);
            if (result.Success && result.Data is FlightStatsReportViewModel report)
            {
                // Update summary label for yearly view
                lblFlightSummary.Text = $"📊 CẢ NĂM {year}  •  " +
                                       $"Tổng chuyến bay: {report.TotalFlights:N0}  •  " +
                                       $"Tổng hành khách: {report.TotalPassengers:N0}  •  " +
                                       $"Tỷ lệ lấp đầy TB: {report.AverageOccupancyRate:N2}%  •  " +
                                       $"Doanh thu: {report.TotalRevenue:N0} VND";
                
                dgvFlights.DataSource = report.FlightDetails;
                
                // Disable AutoSizeColumnsMode to use fixed widths
                dgvFlights.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                
                if (dgvFlights.Columns.Count > 0)
                {
                    // Hide unnecessary columns
                    dgvFlights.Columns["FlightId"].Visible = false;
                    dgvFlights.Columns["TotalPassengers"].Visible = false;
                    
                    // Set Vietnamese headers with formatting (same as monthly view)
                    dgvFlights.Columns["FlightCode"].HeaderText = "Số hiệu CB";
                    dgvFlights.Columns["Route"].HeaderText = "Tuyến bay";
                    dgvFlights.Columns["DepartureTime"].HeaderText = "Giờ khởi hành";
                    dgvFlights.Columns["DepartureTime"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    dgvFlights.Columns["ArrivalTime"].HeaderText = "Giờ đến";
                    dgvFlights.Columns["ArrivalTime"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    dgvFlights.Columns["TotalSeats"].HeaderText = "Tổng ghế";
                    dgvFlights.Columns["BookedSeats"].HeaderText = "Đã đặt";
                    dgvFlights.Columns["OccupancyRate"].HeaderText = "Lấp đầy %";
                    dgvFlights.Columns["OccupancyRate"].DefaultCellStyle.Format = "N2";
                    dgvFlights.Columns["Revenue"].HeaderText = "Doanh thu (VND)";
                    dgvFlights.Columns["Revenue"].DefaultCellStyle.Format = "N0";
                    
                    // Auto-fill columns to use available width
                    dgvFlights.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            else
            {
                MessageBox.Show(result.Message ?? $"Không có dữ liệu chuyến bay cho năm {year}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvFlights.DataSource = null;
                lblFlightSummary.Text = "Không có dữ liệu";
            }
        }

        private void LoadPaymentStatsReport(int year, int month)
        {
            // If month = 0, load yearly view
            if (month == 0)
            {
                LoadPaymentStatsReportYearly(year);
                return;
            }
            
            var result = StatsBUS.Instance.GetPaymentStatsReport(year, month);
            
            // Handle both Success and Failure (e.g., no data for period)
            if ((result.Success || !result.Success) && result.Data is PaymentStatsReportViewModel report)
            {
                // Update summary label
                lblPaymentSummary.Text = $"Tổng giao dịch: {report.TotalTransactions:N0}  |  " +
                                        "Thành công: {report.SuccessfulTransactions:N0}  |  " +
                                        "Thất bại: {report.FailedTransactions:N0}  |  " +
                                        "Tỷ lệ TC: {report.SuccessRate:N2}%  |  " +
                                        "Doanh thu: {report.TotalRevenue:N0} VND";
                
                // Chart for payment methods
                chartPayments.Series.Clear();
                chartPayments.Titles.Clear();
                chartPayments.Titles.Add("Thống kê phương thức thanh toán");
                
                if (report.PaymentMethods != null && report.PaymentMethods.Count > 0)
                {
                    var series = new Series("Số tiền")
                    {
                        ChartType = SeriesChartType.Pie
                    };
                    chartPayments.Series.Add(series);

                    foreach (var method in report.PaymentMethods)
                    {
                        var point = series.Points.AddXY(method.PaymentMethod, method.TotalAmount);
                        series.Points[point].Label = $"{method.PaymentMethod}\n{method.TotalAmount:N0} VND";
                    }
                    
                    series.IsValueShownAsLabel = false;
                    chartPayments.Legends.Clear();
                    chartPayments.Legends.Add(new Legend("Legend"));

                    // DataGridView for payment details
                    dgvPayments.DataSource = report.PaymentMethods;
                    if (dgvPayments.Columns.Count > 0)
                    {
                        // Safely set headers with null checking
                        if (dgvPayments.Columns.Contains("PaymentMethod"))
                            dgvPayments.Columns["PaymentMethod"].HeaderText = "Phương thức";
                        if (dgvPayments.Columns.Contains("TotalTransactions"))
                            dgvPayments.Columns["TotalTransactions"].HeaderText = "Số GD";
                        if (dgvPayments.Columns.Contains("TotalAmount"))
                        {
                            dgvPayments.Columns["TotalAmount"].HeaderText = "Tổng tiền";
                            dgvPayments.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
                        }
                        if (dgvPayments.Columns.Contains("SuccessCount"))
                            dgvPayments.Columns["SuccessCount"].HeaderText = "Thành công";
                        if (dgvPayments.Columns.Contains("FailedCount"))
                            dgvPayments.Columns["FailedCount"].HeaderText = "Thất bại";
                        if (dgvPayments.Columns.Contains("SuccessRate"))
                        {
                            dgvPayments.Columns["SuccessRate"].HeaderText = "Tỷ lệ TC (%)";
                            dgvPayments.Columns["SuccessRate"].DefaultCellStyle.Format = "N2";
                        }
                        dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                    
                    // Show message if this was a failure result (no data)
                    if (!result.Success)
                    {
                        MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // No payment methods found
                    dgvPayments.DataSource = null;
                    chartPayments.Series.Clear();
                    lblPaymentSummary.Text = $"Không có dữ liệu thanh toán cho tháng {month:D2}/{year}";
                    MessageBox.Show(result.Message ?? "Không có dữ liệu thanh toán cho khoảng thời gian được chọn.", 
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Error case
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvPayments.DataSource = null;
                chartPayments.Series.Clear();
                lblPaymentSummary.Text = "Không có dữ liệu";
            }
        }

        private void LoadPaymentStatsReportYearly(int year)
        {
            var result = StatsBUS.Instance.GetPaymentStatsReportYearly(year);
            
            if ((result.Success || !result.Success) && result.Data is PaymentStatsReportViewModel report)
            {
                // Update summary label for yearly view
                lblPaymentSummary.Text = $"[NĂM {year}] Tổng giao dịch: {report.TotalTransactions:N0}  |  " +
                                        $"Thành công: {report.SuccessfulTransactions:N0}  |  " +
                                        $"Thất bại: {report.FailedTransactions:N0}  |  " +
                                        $"Tỷ lệ TC: {report.SuccessRate:N2}%  |  " +
                                        $"Doanh thu: {report.TotalRevenue:N0} VND";
                
                // Chart for payment methods
                chartPayments.Series.Clear();
                chartPayments.Titles.Clear();
                chartPayments.Titles.Add($"Thống kê phương thức thanh toán năm {year}");
                
                if (report.PaymentMethods != null && report.PaymentMethods.Count > 0)
                {
                    var series = new Series("Số tiền")
                    {
                        ChartType = SeriesChartType.Pie
                    };
                    chartPayments.Series.Add(series);

                    foreach (var method in report.PaymentMethods)
                    {
                        var point = series.Points.AddXY(method.PaymentMethod, method.TotalAmount);
                        series.Points[point].Label = $"{method.PaymentMethod}\n{method.TotalAmount:N0} VND";
                    }
                    
                    series.IsValueShownAsLabel = false;
                    chartPayments.Legends.Clear();
                    chartPayments.Legends.Add(new Legend("Legend"));

                    // DataGridView for payment details
                    dgvPayments.DataSource = report.PaymentMethods;
                    if (dgvPayments.Columns.Count > 0)
                    {
                        if (dgvPayments.Columns.Contains("PaymentMethod"))
                            dgvPayments.Columns["PaymentMethod"].HeaderText = "Phương thức";
                        if (dgvPayments.Columns.Contains("TotalTransactions"))
                            dgvPayments.Columns["TotalTransactions"].HeaderText = "Số GD";
                        if (dgvPayments.Columns.Contains("TotalAmount"))
                        {
                            dgvPayments.Columns["TotalAmount"].HeaderText = "Tổng tiền";
                            dgvPayments.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
                        }
                        if (dgvPayments.Columns.Contains("SuccessCount"))
                            dgvPayments.Columns["SuccessCount"].HeaderText = "Thành công";
                        if (dgvPayments.Columns.Contains("FailedCount"))
                            dgvPayments.Columns["FailedCount"].HeaderText = "Thất bại";
                        if (dgvPayments.Columns.Contains("SuccessRate"))
                        {
                            dgvPayments.Columns["SuccessRate"].HeaderText = "Tỷ lệ TC (%)";
                            dgvPayments.Columns["SuccessRate"].DefaultCellStyle.Format = "N2";
                        }
                        dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                    
                    if (!result.Success)
                    {
                        MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    dgvPayments.DataSource = null;
                    chartPayments.Series.Clear();
                    lblPaymentSummary.Text = $"Không có dữ liệu thanh toán cho năm {year}";
                    MessageBox.Show(result.Message ?? $"Không có dữ liệu thanh toán cho năm {year}.", 
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvPayments.DataSource = null;
                chartPayments.Series.Clear();
                lblPaymentSummary.Text = "Không có dữ liệu";
            }
        }

        private void LoadRoutesReport(int year)
        {
            try
            {
                var topRoutesResult = StatsBUS.Instance.GetTopRoutesByFlightCount(year);
                if (topRoutesResult.Success)
                {
                    dgvTopRoutes.DataSource = topRoutesResult.Data;
                    if (dgvTopRoutes.Columns.Count > 0)
                    {
                        dgvTopRoutes.Columns["route_name"].HeaderText = "Tuyến bay";
                        dgvTopRoutes.Columns["flight_count"].HeaderText = "Số chuyến bay";
                        dgvTopRoutes.Columns["distance_km"].HeaderText = "Khoảng cách (km)";
                        dgvTopRoutes.Columns["duration_minutes"].HeaderText = "Thời gian bay (phút)";
                        dgvTopRoutes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                    lblRoutesSummary.Text = $"Top tuyến bay năm {year}";
                }
                else
                {
                    MessageBox.Show(topRoutesResult.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAircraftsReport(int year)
        {
            try
            {
                var topAircraftsResult = StatsBUS.Instance.GetTopAircraftsByFlightCount(year);
                if (topAircraftsResult.Success)
                {
                    dgvTopAircrafts.DataSource = topAircraftsResult.Data;
                    if (dgvTopAircrafts.Columns.Count > 0)
                    {
                        dgvTopAircrafts.Columns["model"].HeaderText = "Mẫu máy bay";
                        dgvTopAircrafts.Columns["manufacturer"].HeaderText = "Nhà sản xuất";
                        dgvTopAircrafts.Columns["airline_name"].HeaderText = "Hãng hàng không";
                        dgvTopAircrafts.Columns["flight_count"].HeaderText = "Số chuyến bay";
                        dgvTopAircrafts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                    lblAircraftsSummary.Text = $"Top máy bay năm {year}";
                }
                else
                {
                    MessageBox.Show(topAircraftsResult.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRoutesAndAircraftsReport(int year)
        {
            LoadRoutesReport(year);
            LoadAircraftsReport(year);
        }
    }
}