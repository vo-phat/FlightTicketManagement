
using BUS.Stats;
using DTO.Stats;
using GUI.Components.Buttons;
using GUI.Components.Tables; // Sử dụng TableCustom
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq; // Cần cho ToDictionary
using System.Windows.Forms;
// Không cần using System.Windows.Forms.DataVisualization.Charting nữa

namespace GUI.Features.Stats
{
    public class StatsControl : UserControl
    {
        private TableLayoutPanel mainLayout;
        private Label lblTitle;
        private FlowLayoutPanel filterPanel;

        // Bộ lọc theo Năm
        private NumericUpDown numYear;
        private PrimaryButton btnLoad;

        // Thẻ tóm tắt
        private FlowLayoutPanel summaryPanel;
        private Label lblTotalRevenue, lblTotalTransactions;

        // Bảng dữ liệu
        private TableCustom tblMonthlyData; // Bảng chi tiết Tháng
        private TableCustom tblTopRoutes;   // Bảng chi tiết Tuyến

        public StatsControl()
        {
            InitializeControl();
            LoadDefaultReport(); // Tải báo cáo mặc định
        }

        private void InitializeControl()
        {
            this.Controls.Clear();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(232, 240, 252); // Nền xám nhạt

            // 1. Layout chính (4 hàng)
            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Color.Transparent
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Title
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Filter
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Summary Cards
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Content (2 bảng)
            this.Controls.Add(mainLayout);

            // 2. Title
            lblTitle = new Label
            {
                Text = "📈 Báo cáo doanh thu",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };
            mainLayout.Controls.Add(lblTitle, 0, 0);

            // 3. Filter Panel (Chỉ có NĂM)
            filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 12, 24, 12)
            };
            mainLayout.Controls.Add(filterPanel, 0, 1);

            filterPanel.Controls.Add(new Label
            {
                Text = "Chọn năm báo cáo:",
                Font = new Font("Segoe UI", 10f),
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 8, 5, 0),
                AutoSize = true
            });

            numYear = new NumericUpDown
            {
                Minimum = 2020,
                Maximum = 2030, // Cho phép xem tương lai (nếu cần)
                // Dữ liệu trong database.txt tập trung vào 2024, 2025
                Value = 2024,
                Width = 100,
                Font = new Font("Segoe UI", 10f),
                Margin = new Padding(0, 5, 15, 0)
            };
            filterPanel.Controls.Add(numYear);

            btnLoad = new PrimaryButton("🔍 Xem báo cáo");
            btnLoad.Click += BtnLoad_Click;
            filterPanel.Controls.Add(btnLoad);

            var btnExport = new PrimaryButton("📊 Xuất Excel") { Margin = new Padding(8, 0, 0, 0) };
            btnExport.Click += BtnExport_Click;
            filterPanel.Controls.Add(btnExport);

            // 4. Summary Panel (Các thẻ tóm tắt)
            summaryPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 0, 24, 12)
            };
            lblTotalRevenue = CreateSummaryCard("Tổng Doanh thu (Năm)", "0 VND");
            lblTotalTransactions = CreateSummaryCard("Tổng Giao dịch (Năm)", "0");
            summaryPanel.Controls.Add(lblTotalRevenue);
            summaryPanel.Controls.Add(lblTotalTransactions);
            mainLayout.Controls.Add(summaryPanel, 0, 2);

            // 5. Khu vực nội dung (chia 2 cột)
            var contentSplit = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 0, 24, 24)
            };
            // Cột bên trái 40%, bên phải 60%
            contentSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            contentSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
            mainLayout.Controls.Add(contentSplit, 0, 3);

            // 5a. Bảng Chi tiết Tháng (Bên trái)
            tblMonthlyData = new TableCustom
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 12, 0),
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            tblMonthlyData.Columns.Add("Month", "Tháng");
            tblMonthlyData.Columns.Add("Revenue", "Doanh thu (VND)");
            tblMonthlyData.Columns["Revenue"].DefaultCellStyle.Format = "N0";
            tblMonthlyData.Columns["Revenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tblMonthlyData.Columns["Month"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tblMonthlyData.Columns["Revenue"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            contentSplit.Controls.Add(tblMonthlyData, 0, 0);

            // 5b. Bảng Top Khách hàng (Bên phải - chi tiết hơn)
            tblTopRoutes = new TableCustom
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(12, 0, 0, 0),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            tblTopRoutes.Columns.Add("Route", "Top 5 Khách hàng");
            tblTopRoutes.Columns.Add("Flights", "Số giao dịch");
            tblTopRoutes.Columns.Add("Revenue", "Doanh thu (VND)");

            tblTopRoutes.Columns["Route"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tblTopRoutes.Columns["Route"].FillWeight = 50;
            
            tblTopRoutes.Columns["Flights"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tblTopRoutes.Columns["Flights"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            tblTopRoutes.Columns["Flights"].Width = 120;
            
            tblTopRoutes.Columns["Revenue"].DefaultCellStyle.Format = "N0";
            tblTopRoutes.Columns["Revenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tblTopRoutes.Columns["Revenue"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tblTopRoutes.Columns["Revenue"].FillWeight = 35;

            contentSplit.Controls.Add(tblTopRoutes, 1, 0);
        }

        // Helper tạo thẻ tóm tắt
        private Label CreateSummaryCard(string title, string value)
        {
            var lbl = new Label
            {
                BackColor = Color.White,
                Width = 240,
                Height = 90,
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 16, 0),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Text = $"{title}\n"
            };
            var valLabel = new Label
            {
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175), // Màu xanh
                Text = value,
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.BottomLeft,
                Height = 40
            };
            lbl.Controls.Add(valLabel);
            return lbl;
        }

        private void LoadDefaultReport()
        {
            LoadReport((int)numYear.Value);
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            LoadReport((int)numYear.Value);
        }

        private void BtnExport_Click(object? sender, EventArgs e)
        {
            try
            {
                // Tạo SaveFileDialog
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                    sfd.FileName = $"BaoCaoDoanhThu_{(int)numYear.Value}.csv";
                    sfd.Title = "Xuất báo cáo doanh thu";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var writer = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                        {
                            // Header
                            writer.WriteLine($"BÁO CÁO DOANH THU NĂM {(int)numYear.Value}");
                            writer.WriteLine($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}");
                            writer.WriteLine();
                            
                            writer.WriteLine($"Tổng doanh thu,{lblTotalRevenue.Controls[0].Text}");
                            writer.WriteLine($"Tổng giao dịch,{lblTotalTransactions.Controls[0].Text}");
                            writer.WriteLine();

                            // Monthly data
                            writer.WriteLine("DOANH THU THEO THÁNG");
                            writer.WriteLine("Tháng,Doanh thu (VND)");
                            foreach (DataGridViewRow row in tblMonthlyData.Rows)
                            {
                                if (row.IsNewRow) continue;
                                writer.WriteLine($"{row.Cells[0].Value},{row.Cells[1].Value}");
                            }
                            writer.WriteLine();

                            // Top customers
                            writer.WriteLine("TOP 5 KHÁCH HÀNG");
                            writer.WriteLine("Khách hàng,Số giao dịch,Doanh thu (VND)");
                            foreach (DataGridViewRow row in tblTopRoutes.Rows)
                            {
                                if (row.IsNewRow) continue;
                                writer.WriteLine($"{row.Cells[0].Value},{row.Cells[1].Value},{row.Cells[2].Value}");
                            }
                        }

                        MessageBox.Show($"Xuất báo cáo thành công!\nFile: {sfd.FileName}", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất báo cáo: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {

        }

        private void LoadReport(int year)
        {
            var result = StatsBUS.Instance.GetRevenueReport(year);
            if (!result.Success)
            {
                MessageBox.Show(result.GetFullErrorMessage(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var report = result.GetData<RevenueReportViewModel>();

            // 1. Cập nhật Thẻ Tóm tắt
            (lblTotalRevenue.Controls[0] as Label).Text = $"{report.TotalRevenue:N0} VND";
            (lblTotalTransactions.Controls[0] as Label).Text = $"{report.TotalTransactions:N0}";

            // 2. Cập nhật Bảng Chi tiết Tháng
            tblMonthlyData.Rows.Clear();
            var culture = new CultureInfo("vi-VN"); // "Tháng 1", "Tháng 2"

            // Chuyển data sang Dictionary (Tháng -> DoanhThu)
            var monthlyData = report.MonthlyBreakdown.AsEnumerable()
                .ToDictionary(
                    row => row.Field<int>("Thang"), // Key
                    row => row.Field<decimal>("DoanhThu") // Value
                );

            // Luôn hiển thị 12 tháng
            for (int i = 1; i <= 12; i++)
            {
                string monthName = culture.DateTimeFormat.GetMonthName(i);
                monthName = char.ToUpper(monthName[0]) + monthName.Substring(1); // "Tháng 1"

                decimal revenue = 0;
                if (monthlyData.ContainsKey(i))
                {
                    revenue = monthlyData[i];
                }

                tblMonthlyData.Rows.Add(monthName, revenue);
            }

            // 3. Cập nhật Bảng Top Khách hàng
            tblTopRoutes.Rows.Clear();
            
            if (report.RouteBreakdown != null && report.RouteBreakdown.Rows.Count > 0)
            {
                foreach (DataRow row in report.RouteBreakdown.Rows)
                {
                    string customer = row["TuyenBay"]?.ToString() ?? "N/A";
                    int transactions = row.Table.Columns.Contains("SoChuyenBay") 
                        ? Convert.ToInt32(row["SoChuyenBay"]) 
                        : 0;
                    decimal revenue = Convert.ToDecimal(row["DoanhThu"]);
                    
                    tblTopRoutes.Rows.Add(customer, transactions, revenue);
                }
            }
            else
            {
                // Hiển thị thông báo nếu không có dữ liệu
                tblTopRoutes.Rows.Add("Không có dữ liệu", 0, 0);
            }
        }
    }
}