using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BUS.Flight;
using BUS.Payment;
using DTO.Flight;
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

        private readonly FlightBUS _flightBUS;
        private readonly PaymentBUS _paymentBUS;

        public StatsControl() {
            _flightBUS = FlightBUS.Instance;
            _paymentBUS = new PaymentBUS();
            InitializeComponent();
            LoadStatistics();
        }

        private void InitializeComponent() {
            this.SuspendLayout();

            // Main panel
            mainPanel = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                BackColor = Color.FromArgb(232, 240, 252),
                Padding = new Padding(20)
            };
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Left column
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Right column
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F)); // Header
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F)); // Flight stats
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F)); // Payment stats
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Monthly report & cabin stats

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

            mainPanel.SetColumnSpan(headerPanel, 2); // Header spans both columns
            mainPanel.Controls.Add(headerPanel, 0, 0);
            
            mainPanel.SetColumnSpan(flightStatsPanel, 2); // Flight stats spans both columns
            mainPanel.Controls.Add(flightStatsPanel, 0, 1);
            
            mainPanel.SetColumnSpan(paymentStatsPanel, 2); // Payment stats spans both columns
            mainPanel.Controls.Add(paymentStatsPanel, 0, 2);
            
            mainPanel.Controls.Add(monthlyReportPanel, 0, 3);
            mainPanel.Controls.Add(cabinClassStatsPanel, 1, 3);

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

                // Get monthly revenue report from database
                var monthlyReport = DAO.Flight.FlightDAO.Instance.GetMonthlyRevenueReport(fromDate, toDate);

                if (monthlyReport != null && monthlyReport.Rows.Count > 0) {
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

                // Get cabin class statistics from database
                var cabinStats = DAO.Flight.FlightDAO.Instance.GetCabinClassStatistics(fromDate, toDate);

                if (cabinStats != null && cabinStats.Rows.Count > 0) {
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
    }
}
