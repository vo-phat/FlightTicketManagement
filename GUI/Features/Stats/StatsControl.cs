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
            InitializeFilterControls();
            btnLoad.Click += btnLoad_Click;
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

            // Populate months
            for (int i = 1; i <= 12; i++)
            {
                comboBoxMonth.Items.Add(new { Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i), Value = i });
            }
            comboBoxMonth.DisplayMember = "Text";
            comboBoxMonth.ValueMember = "Value";
            comboBoxMonth.SelectedIndex = DateTime.Now.Month - 1;
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
        }

        private void LoadRevenueReport(int year)
        {
            var result = StatsBUS.Instance.GetRevenueReport(year);
            if (result.Success && result.Data is RevenueReportViewModel report)
            {
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

                // DataGridView for routes
                dgvRevenueRoutes.DataSource = report.RouteBreakdown;
                dgvRevenueRoutes.Columns["TuyenBay"].HeaderText = "Tuyến bay";
                dgvRevenueRoutes.Columns["DoanhThu"].HeaderText = "Doanh thu";
                dgvRevenueRoutes.Columns["SoChuyenBay"].HeaderText = "Số chuyến bay";
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFlightStatsReport(int year, int month)
        {
            var result = StatsBUS.Instance.GetFlightStatsReport(year, month);
            if (result.Success && result.Data is FlightStatsReportViewModel report)
            {
                dgvFlights.DataSource = report.FlightDetails;
                // Customize column headers if needed
                dgvFlights.Columns["FlightId"].HeaderText = "ID Chuyến bay";
                dgvFlights.Columns["FlightCode"].HeaderText = "Mã chuyến bay";
                dgvFlights.Columns["Route"].HeaderText = "Tuyến";
                dgvFlights.Columns["DepartureTime"].HeaderText = "Giờ đi";
                dgvFlights.Columns["ArrivalTime"].HeaderText = "Giờ đến";
                dgvFlights.Columns["TotalSeats"].HeaderText = "Tổng ghế";
                dgvFlights.Columns["BookedSeats"].HeaderText = "Ghế đã đặt";
                dgvFlights.Columns["OccupancyRate"].HeaderText = "Tỷ lệ lấp đầy (%)";
                dgvFlights.Columns["TotalPassengers"].HeaderText = "Số hành khách";
                dgvFlights.Columns["Revenue"].HeaderText = "Doanh thu";
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvFlights.DataSource = null;
            }
        }

        private void LoadPaymentStatsReport(int year, int month)
        {
            var result = StatsBUS.Instance.GetPaymentStatsReport(year, month);
            if (result.Success && result.Data is PaymentStatsReportViewModel report)
            {
                // Chart for payment methods
                chartPayments.Series.Clear();
                chartPayments.Titles.Clear();
                chartPayments.Titles.Add("Thống kê phương thức thanh toán");
                var series = new Series("Số tiền")
                {
                    ChartType = SeriesChartType.Pie
                };
                chartPayments.Series.Add(series);

                foreach (var method in report.PaymentMethods)
                {
                    series.Points.AddXY(method.PaymentMethod, method.TotalAmount);
                }

                // DataGridView for payment details
                dgvPayments.DataSource = report.PaymentMethods;
                dgvPayments.Columns["PaymentMethod"].HeaderText = "Phương thức";
                dgvPayments.Columns["TotalTransactions"].HeaderText = "Số giao dịch";
                dgvPayments.Columns["TotalAmount"].HeaderText = "Tổng tiền";
                dgvPayments.Columns["SuccessCount"].HeaderText = "Thành công";
                dgvPayments.Columns["FailedCount"].HeaderText = "Thất bại";
                dgvPayments.Columns["SuccessRate"].HeaderText = "Tỷ lệ thành công (%)";
            }
            else
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvPayments.DataSource = null;
            }
        }
    }
}
