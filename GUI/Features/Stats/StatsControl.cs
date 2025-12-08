using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BUS.Flight;
using BUS.Payment;
using BUS.Stats;
using DTO.Flight;
using DTO.Stats;
using GUI.Components.Buttons;

namespace GUI.Features.Stats {
    public class StatsControl : UserControl {
        private TableLayoutPanel mainPanel = null!;
        private Panel headerPanel = null!;
        private Label lblTitle = null!;
        private DateTimePicker dtpFromDate = null!;
        private DateTimePicker dtpToDate = null!;
        private Button btnRefresh = null!;
        
        // Flight Stats
        private Panel flightStatsPanel = null!;
        private Label lblFlightStatsTitle = null!;
        private Label lblTotalFlights = null!;
        private Label lblScheduledFlights = null!;
        private Label lblDelayedFlights = null!;
        private Label lblCancelledFlights = null!;
        private Label lblCompletedFlights = null!;
        
        // Payment Stats
        private Panel paymentStatsPanel = null!;
        private Label lblPaymentStatsTitle = null!;
        private Label lblTotalRevenue = null!;
        private Label lblPendingPayments = null!;
        private Label lblSuccessfulPayments = null!;
        private Label lblFailedPayments = null!;
        
        // Monthly Report
        private Panel monthlyReportPanel = null!;
        private Label lblMonthlyReportTitle = null!;
        private DataGridView dgvMonthlyReport = null!;
        
        // Cabin Class Statistics
        private Panel cabinClassStatsPanel = null!;
        private Label lblCabinClassStatsTitle = null!;
        private DataGridView dgvCabinClassStats = null!;

        // Flight Details Statistics
        private Panel flightDetailsPanel = null!;
        private Label lblFlightDetailsTitle = null!;
        private DataGridView dgvFlightDetails = null!;
        private Label lblFlightDetailsInfo = null!;

        // Payment Statistics
        private Panel paymentDetailsPanel = null!;
        private Label lblPaymentDetailsTitle = null!;
        private DataGridView dgvPaymentDetails = null!;
        private Label lblPaymentDetailsInfo = null!;

        private readonly FlightBUS _flightBUS;
        private readonly PaymentBUS _paymentBUS;
        private readonly StatsBUS _statsBUS;

        public StatsControl() {
            _flightBUS = FlightBUS.Instance;
            _paymentBUS = new PaymentBUS();
            _statsBUS = StatsBUS.Instance;
            InitializeComponent();
            LoadStatistics();
        }

        private void InitializeComponent() {
            this.SuspendLayout();

            // Main panel
            mainPanel = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                BackColor = Color.FromArgb(232, 240, 252),
                Padding = new Padding(20)
            };
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Left column
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Right column
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F)); // Header
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F)); // Flight stats
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F)); // Payment stats
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Monthly report & cabin stats
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 350F)); // Flight details & Payment details

            // Header panel
            InitializeHeader();
            
            // Flight statistics panel
            InitializeFlightStatsPanel();
            
            // Payment statistics panel
            InitializePaymentStatsPanel();
            
            // Monthly report panel
            InitializeMonthlyReportPanel();
            
            // Cabin class statistics panel
            InitializeCabinClassStatsPanel();

            // Flight details statistics panel
            InitializeFlightDetailsPanel();

            // Payment details statistics panel
            InitializePaymentDetailsPanel();

            mainPanel.SetColumnSpan(headerPanel, 2); // Header spans both columns
            mainPanel.Controls.Add(headerPanel, 0, 0);
            
            mainPanel.SetColumnSpan(flightStatsPanel, 2); // Flight stats spans both columns
            mainPanel.Controls.Add(flightStatsPanel, 0, 1);
            
            mainPanel.SetColumnSpan(paymentStatsPanel, 2); // Payment stats spans both columns
            mainPanel.Controls.Add(paymentStatsPanel, 0, 2);
            
            mainPanel.Controls.Add(monthlyReportPanel, 0, 3);
            mainPanel.Controls.Add(cabinClassStatsPanel, 1, 3);
            
            mainPanel.Controls.Add(flightDetailsPanel, 0, 4); // Flight details left column
            mainPanel.Controls.Add(paymentDetailsPanel, 1, 4); // Payment details right column

            this.Controls.Add(mainPanel);
            this.ResumeLayout(false);
        }

        private void InitializeHeader() {
            headerPanel = new Panel {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            lblTitle = new Label {
                Text = "📈 BÁO CÁO THỐNG KÊ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            Label lblFrom = new Label {
                Text = "Từ ngày:",
                Font = new Font("Segoe UI", 9),
                AutoSize = true,
                Location = new Point(20, 55)
            };

            dtpFromDate = new DateTimePicker {
                Format = DateTimePickerFormat.Short,
                Location = new Point(90, 52),
                Width = 120,
                Value = DateTime.Now.AddMonths(-1)
            };

            Label lblTo = new Label {
                Text = "Đến ngày:",
                Font = new Font("Segoe UI", 9),
                AutoSize = true,
                Location = new Point(230, 55)
            };

            dtpToDate = new DateTimePicker {
                Format = DateTimePickerFormat.Short,
                Location = new Point(310, 52),
                Width = 120,
                Value = DateTime.Now
            };

            btnRefresh = new PrimaryButton("Làm mới") {
                Location = new Point(450, 50),
                Size = new Size(100, 30)
            };
            btnRefresh.Click += BtnRefresh_Click;

            headerPanel.Controls.AddRange(new Control[] { lblTitle, lblFrom, dtpFromDate, lblTo, dtpToDate, btnRefresh });
        }

        private void InitializeFlightStatsPanel() {
            flightStatsPanel = new Panel {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
                Margin = new Padding(0, 10, 0, 0)
            };

            lblFlightStatsTitle = new Label {
                Text = "✈️ THỐNG KÊ CHUYẾN BAY",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            lblTotalFlights = CreateStatLabel("Tổng số chuyến bay: 0", new Point(20, 50), Color.FromArgb(52, 73, 94));
            lblScheduledFlights = CreateStatLabel("Đã lên lịch: 0", new Point(20, 80), Color.FromArgb(52, 152, 219));
            lblDelayedFlights = CreateStatLabel("Bị hoãn: 0", new Point(20, 110), Color.FromArgb(230, 126, 34));
            lblCancelledFlights = CreateStatLabel("Đã hủy: 0", new Point(20, 140), Color.FromArgb(231, 76, 60));
            lblCompletedFlights = CreateStatLabel("Hoàn thành: 0", new Point(300, 80), Color.FromArgb(46, 204, 113));

            flightStatsPanel.Controls.AddRange(new Control[] { 
                lblFlightStatsTitle, lblTotalFlights, lblScheduledFlights, 
                lblDelayedFlights, lblCancelledFlights, lblCompletedFlights 
            });
        }

        private void InitializePaymentStatsPanel() {
            paymentStatsPanel = new Panel {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
                Margin = new Padding(0, 10, 0, 0)
            };

            lblPaymentStatsTitle = new Label {
                Text = "💰 THỐNG KÊ DOANH THU",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            lblTotalRevenue = CreateStatLabel("Tổng doanh thu: 0 VNĐ", new Point(20, 50), Color.FromArgb(39, 174, 96));
            lblSuccessfulPayments = CreateStatLabel("Thanh toán thành công: 0", new Point(20, 80), Color.FromArgb(46, 204, 113));
            lblPendingPayments = CreateStatLabel("Đang chờ: 0", new Point(20, 110), Color.FromArgb(241, 196, 15));
            lblFailedPayments = CreateStatLabel("Thất bại: 0", new Point(20, 140), Color.FromArgb(231, 76, 60));

            paymentStatsPanel.Controls.AddRange(new Control[] { 
                lblPaymentStatsTitle, lblTotalRevenue, lblSuccessfulPayments, 
                lblPendingPayments, lblFailedPayments 
            });
        }

        private void InitializeMonthlyReportPanel() {
            monthlyReportPanel = new Panel {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
                Margin = new Padding(0, 10, 0, 0)
            };

            lblMonthlyReportTitle = new Label {
                Text = "📊 BÁO CÁO CHI TIẾT",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            dgvMonthlyReport = new DataGridView {
                Location = new Point(20, 50),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Width = 650,
                Height = 250,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle {
                    BackColor = Color.FromArgb(0, 92, 175),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }
            };

            // Add columns
            dgvMonthlyReport.Columns.Add("Month", "Tháng/Năm");
            dgvMonthlyReport.Columns.Add("TotalFlights", "Số chuyến bay");
            dgvMonthlyReport.Columns.Add("CompletedFlights", "Chuyến hoàn thành");
            dgvMonthlyReport.Columns.Add("Revenue", "Doanh thu (VNĐ)");
            dgvMonthlyReport.Columns.Add("SuccessfulPayments", "Thanh toán thành công");

            monthlyReportPanel.Controls.AddRange(new Control[] { lblMonthlyReportTitle, dgvMonthlyReport });
        }

        private void InitializeCabinClassStatsPanel() {
            cabinClassStatsPanel = new Panel {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
                Margin = new Padding(10, 10, 0, 0)
            };

            lblCabinClassStatsTitle = new Label {
                Text = "🎫 THỐNG KÊ THEO HẠNG VÉ",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            dgvCabinClassStats = new DataGridView {
                Location = new Point(20, 50),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Width = 650,
                Height = 250,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle {
                    BackColor = Color.FromArgb(0, 92, 175),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }
            };

            // Add columns
            dgvCabinClassStats.Columns.Add("CabinClass", "Hạng vé");
            dgvCabinClassStats.Columns.Add("TotalTickets", "Số vé đã bán");
            dgvCabinClassStats.Columns.Add("Revenue", "Doanh thu (VNĐ)");
            dgvCabinClassStats.Columns.Add("BookingRate", "Tỷ lệ đặt (%)");
            dgvCabinClassStats.Columns.Add("AvgPrice", "Giá TB (VNĐ)");

            cabinClassStatsPanel.Controls.AddRange(new Control[] { lblCabinClassStatsTitle, dgvCabinClassStats });
        }

        private void InitializeFlightDetailsPanel() {
            flightDetailsPanel = new Panel {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
                Margin = new Padding(0, 10, 0, 0)
            };

            lblFlightDetailsTitle = new Label {
                Text = "✈️ CHI TIẾT CHUYẾN BAY ĐI",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            lblFlightDetailsInfo = new Label {
                Text = "Tổng chuyến: 0 | Doanh thu: 0 VNĐ | Hành khách: 0 | Tỷ lệ lấp đầy TB: 0%",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(20, 40)
            };

            dgvFlightDetails = new DataGridView {
                Location = new Point(20, 65),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Width = 650,
                Height = 250,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle {
                    BackColor = Color.FromArgb(0, 92, 175),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }
            };

            // Add columns
            dgvFlightDetails.Columns.Add("FlightCode", "Mã chuyến");
            dgvFlightDetails.Columns.Add("Route", "Tuyến đường");
            dgvFlightDetails.Columns.Add("DepartureTime", "Giờ cất cánh");
            dgvFlightDetails.Columns.Add("ArrivalTime", "Giờ hạ cánh");
            dgvFlightDetails.Columns.Add("TotalSeats", "Tổng ghế");
            dgvFlightDetails.Columns.Add("BookedSeats", "Ghế đã đặt");
            dgvFlightDetails.Columns.Add("OccupancyRate", "Tỷ lệ lấp đầy (%)");
            dgvFlightDetails.Columns.Add("TotalPassengers", "Hành khách");
            dgvFlightDetails.Columns.Add("Revenue", "Doanh thu (VNĐ)");

            flightDetailsPanel.Controls.AddRange(new Control[] { lblFlightDetailsTitle, lblFlightDetailsInfo, dgvFlightDetails });
        }

        private void InitializePaymentDetailsPanel() {
            paymentDetailsPanel = new Panel {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
                Margin = new Padding(10, 10, 0, 0)
            };

            lblPaymentDetailsTitle = new Label {
                Text = "💳 CHI TIẾT THANH TOÁN",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            lblPaymentDetailsInfo = new Label {
                Text = "Tổng doanh thu: 0 VNĐ | Tổng giao dịch: 0 | Thành công: 0 | Thất bại: 0 | Tỷ lệ thành công: 0%",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(20, 40)
            };

            dgvPaymentDetails = new DataGridView {
                Location = new Point(20, 65),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Width = 650,
                Height = 250,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle {
                    BackColor = Color.FromArgb(0, 92, 175),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }
            };

            // Add columns
            dgvPaymentDetails.Columns.Add("PaymentMethod", "Phương thức");
            dgvPaymentDetails.Columns.Add("TotalTransactions", "Tổng GD");
            dgvPaymentDetails.Columns.Add("TotalAmount", "Tổng tiền (VNĐ)");
            dgvPaymentDetails.Columns.Add("SuccessCount", "Thành công");
            dgvPaymentDetails.Columns.Add("FailedCount", "Thất bại");
            dgvPaymentDetails.Columns.Add("SuccessRate", "Tỷ lệ TC (%)");

            paymentDetailsPanel.Controls.AddRange(new Control[] { lblPaymentDetailsTitle, lblPaymentDetailsInfo, dgvPaymentDetails });
        }

        private Label CreateStatLabel(string text, Point location, Color foreColor) {
            return new Label {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = foreColor,
                AutoSize = true,
                Location = location
            };
        }

        private void BtnRefresh_Click(object? sender, EventArgs e) {
            LoadStatistics();
        }

        private void LoadStatistics() {
            try {
                DateTime fromDate = dtpFromDate.Value.Date;
                DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);

                // Load flight statistics
                LoadFlightStatistics(fromDate, toDate);

                // Load payment statistics
                LoadPaymentStatistics();

                // Load monthly report
                LoadMonthlyReport(fromDate, toDate);
                
                // Load cabin class statistics
                LoadCabinClassStatistics(fromDate, toDate);

                // Load flight details
                LoadFlightDetails(fromDate, toDate);

                // Load payment details
                LoadPaymentDetails(fromDate, toDate);
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi tải thống kê: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFlightStatistics(DateTime fromDate, DateTime toDate) {
            try {
                var stats = _flightBUS.GetFlightStatsByDateRange(fromDate, toDate);
                
                lblTotalFlights.Text = $"Tổng số chuyến bay: {stats["Tổng số chuyến bay"]}";
                lblScheduledFlights.Text = $"Đã lên lịch: {stats["Đã lên lịch"]}";
                lblDelayedFlights.Text = $"Bị hoãn: {stats["Bị hoãn"]}";
                lblCancelledFlights.Text = $"Đã hủy: {stats["Đã hủy"]}";
                lblCompletedFlights.Text = $"Hoàn thành: {stats["Hoàn thành"]}";
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi tải thống kê chuyến bay: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadPaymentStatistics() {
            try {
                decimal totalRevenue = _paymentBUS.GetTotalSuccessfulPayments();
                var countByStatus = _paymentBUS.GetPaymentCountByStatus();

                lblTotalRevenue.Text = $"Tổng doanh thu: {totalRevenue:N0} VNĐ";
                lblSuccessfulPayments.Text = $"Thanh toán thành công: {countByStatus.GetValueOrDefault("SUCCESS", 0)}";
                lblPendingPayments.Text = $"Đang chờ: {countByStatus.GetValueOrDefault("PENDING", 0)}";
                lblFailedPayments.Text = $"Thất bại: {countByStatus.GetValueOrDefault("FAILED", 0)}";
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi tải thống kê thanh toán: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadMonthlyReport(DateTime fromDate, DateTime toDate) {
            try {
                dgvMonthlyReport.Rows.Clear();

                // Get monthly revenue report from BUS
                var result = _statsBUS.GetMonthlyRevenueReport(fromDate, toDate);

                if (result.Success && result.Data is System.Data.DataTable monthlyReport && monthlyReport.Rows.Count > 0) {
                    foreach (System.Data.DataRow row in monthlyReport.Rows) {
                        string monthYear = row["month_year"]?.ToString() ?? "";
                        int totalFlights = Convert.ToInt32(row["total_flights"]);
                        int completedFlights = Convert.ToInt32(row["completed_flights"]);
                        decimal totalRevenue = Convert.ToDecimal(row["total_revenue"]);
                        int successfulPayments = Convert.ToInt32(row["successful_payments"]);

                        dgvMonthlyReport.Rows.Add(
                            monthYear,
                            totalFlights,
                            completedFlights,
                            totalRevenue.ToString("N0"),
                            successfulPayments
                        );
                    }
                } else {
                    dgvMonthlyReport.Rows.Add("Không có dữ liệu", "-", "-", "-", "-");
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi tải báo cáo tháng: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadCabinClassStatistics(DateTime fromDate, DateTime toDate) {
            try {
                dgvCabinClassStats.Rows.Clear();

                // Get cabin class statistics from BUS
                var result = _statsBUS.GetCabinClassStatistics(fromDate, toDate);

                if (result.Success && result.Data is System.Data.DataTable cabinStats && cabinStats.Rows.Count > 0) {
                    foreach (System.Data.DataRow row in cabinStats.Rows) {
                        string cabinClassName = row["cabin_class_name"]?.ToString() ?? "";
                        int totalTickets = Convert.ToInt32(row["total_tickets"]);
                        decimal revenue = Convert.ToDecimal(row["total_revenue"]);
                        decimal bookingRate = Convert.ToDecimal(row["booking_rate"]);
                        decimal avgPrice = totalTickets > 0 ? revenue / totalTickets : 0;

                        dgvCabinClassStats.Rows.Add(
                            cabinClassName,
                            totalTickets,
                            revenue.ToString("N0"),
                            bookingRate.ToString("F1"),
                            avgPrice.ToString("N0")
                        );
                    }
                } else {
                    dgvCabinClassStats.Rows.Add("Không có dữ liệu", "-", "-", "-", "-");
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi tải thống kê hạng vé: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadFlightDetails(DateTime fromDate, DateTime toDate) {
            try {
                dgvFlightDetails.Rows.Clear();
                lblFlightDetailsInfo.Text = "Đang tải dữ liệu...";

                // Get year and month from the selected date
                int year = fromDate.Year;
                int month = fromDate.Month;

                // Call BUS to get flight stats
                var result = _statsBUS.GetFlightStatsReport(year, month);

                if (result.Success && result.Data is FlightStatsReportViewModel report) {
                    if (report.FlightDetails != null && report.FlightDetails.Count > 0) {
                        foreach (var flight in report.FlightDetails) {
                            dgvFlightDetails.Rows.Add(
                                flight.FlightCode,
                                flight.Route,
                                flight.DepartureTime,
                                flight.ArrivalTime,
                                flight.TotalSeats,
                                flight.BookedSeats,
                                flight.OccupancyRate,
                                flight.TotalPassengers,
                                flight.Revenue.ToString("N0")
                            );
                        }

                        // Update summary info
                        lblFlightDetailsInfo.Text = 
                            $"Tổng chuyến: {report.TotalFlights} | " +
                            $"Doanh thu: {report.TotalRevenue:N0} VNĐ | " +
                            $"Hành khách: {report.TotalPassengers} | " +
                            $"Tỷ lệ lấp đầy TB: {report.AverageOccupancyRate}%";
                    } else {
                        dgvFlightDetails.Rows.Add("Không có dữ liệu", "-", "-", "-", "-", "-", "-", "-", "-");
                        lblFlightDetailsInfo.Text = "Không có dữ liệu chuyến bay trong tháng được chọn";
                    }
                } else {
                    // Show error message
                    string errorMsg = result.Message ?? "Lỗi không xác định";
                    dgvFlightDetails.Rows.Add(errorMsg, "-", "-", "-", "-", "-", "-", "-", "-");
                    lblFlightDetailsInfo.Text = $"Lỗi: {errorMsg}";
                    Console.WriteLine($"Error loading flight stats: {errorMsg}");
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Exception in LoadFlightDetails: {ex}");
                MessageBox.Show($"Lỗi khi tải chi tiết chuyến bay:\n{ex.Message}\n\n{ex.InnerException?.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadPaymentDetails(DateTime fromDate, DateTime toDate) {
            try {
                dgvPaymentDetails.Rows.Clear();
                lblPaymentDetailsInfo.Text = "Đang tải dữ liệu...";

                int year = fromDate.Year;
                int month = fromDate.Month;

                var result = _statsBUS.GetPaymentStatsReport(year, month);

                if (result.Success && result.Data is PaymentStatsReportViewModel report) {
                    if (report.PaymentMethods != null && report.PaymentMethods.Count > 0) {
                        foreach (var method in report.PaymentMethods) {
                            dgvPaymentDetails.Rows.Add(
                                method.PaymentMethod,
                                method.TotalTransactions,
                                method.TotalAmount.ToString("N0"),
                                method.SuccessCount,
                                method.FailedCount,
                                method.SuccessRate
                            );
                        }

                        lblPaymentDetailsInfo.Text =
                            $"Tổng doanh thu: {report.TotalRevenue:N0} VNĐ | " +
                            $"Tổng giao dịch: {report.TotalTransactions} | " +
                            $"Thành công: {report.SuccessfulTransactions} | " +
                            $"Thất bại: {report.FailedTransactions} | " +
                            $"Tỷ lệ thành công: {report.SuccessRate}%";
                    } else {
                        dgvPaymentDetails.Rows.Add("Không có dữ liệu", "-", "-", "-", "-", "-");
                        lblPaymentDetailsInfo.Text = "Không có dữ liệu thanh toán trong tháng được chọn";
                    }
                } else {
                    string errorMsg = result.Message ?? "Lỗi không xác định";
                    dgvPaymentDetails.Rows.Add(errorMsg, "-", "-", "-", "-", "-");
                    lblPaymentDetailsInfo.Text = $"Lỗi: {errorMsg}";
                    MessageBox.Show($"Lỗi khi tải thống kê thanh toán: {errorMsg}", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi tải chi tiết thanh toán:\n{ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}