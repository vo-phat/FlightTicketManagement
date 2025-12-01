
using BUS.Stats;
using DTO.Stats;
using GUI.Components.Buttons;
using GUI.Components.Tables; // Sá»­ dá»¥ng TableCustom
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq; // Cáº§n cho ToDictionary
using System.Windows.Forms;
// KhÃ´ng cáº§n using System.Windows.Forms.DataVisualization.Charting ná»¯a

namespace GUI.Features.Stats
{
    public class StatsControl : UserControl
    {
        private TableLayoutPanel mainLayout;
        private Label lblTitle;
        private FlowLayoutPanel filterPanel;

        // Bá»™ lá»c theo NÄƒm
        private NumericUpDown numYear;
        private PrimaryButton btnLoad;

        // Tháº» tÃ³m táº¯t
        private FlowLayoutPanel summaryPanel;
        private Label lblTotalRevenue, lblTotalTransactions;

        // Báº£ng dá»¯ liá»‡u
        private TableCustom tblMonthlyData; // Báº£ng chi tiáº¿t ThÃ¡ng
        private TableCustom tblTopRoutes;   // Báº£ng chi tiáº¿t Tuyáº¿n

        public StatsControl()
        {
            InitializeControl();
            LoadDefaultReport(); // Táº£i bÃ¡o cÃ¡o máº·c Ä‘á»‹nh
        }

        private void InitializeControl()
        {
            this.Controls.Clear();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(232, 240, 252); // Ná»n xÃ¡m nháº¡t

            // 1. Layout chÃ­nh (4 hÃ ng)
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
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Content (2 báº£ng)
            this.Controls.Add(mainLayout);

            // 2. Title
            lblTitle = new Label
            {
                Text = "ðŸ“ˆ BÃ¡o cÃ¡o doanh thu",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };
            mainLayout.Controls.Add(lblTitle, 0, 0);

            // 3. Filter Panel (Chá»‰ cÃ³ NÄ‚M)
            filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 12, 24, 12)
            };
            mainLayout.Controls.Add(filterPanel, 0, 1);

            filterPanel.Controls.Add(new Label
            {
                Text = "Chá»n nÄƒm bÃ¡o cÃ¡o:",
                Font = new Font("Segoe UI", 10f),
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 8, 5, 0),
                AutoSize = true
            });

            numYear = new NumericUpDown
            {
                Minimum = 2020,
                Maximum = 2030, // Cho phÃ©p xem tÆ°Æ¡ng lai (náº¿u cáº§n)
                // Dá»¯ liá»‡u trong database.txt táº­p trung vÃ o 2024, 2025
                Value = 2024,
                Width = 100,
                Font = new Font("Segoe UI", 10f),
                Margin = new Padding(0, 5, 15, 0)
            };
            filterPanel.Controls.Add(numYear);

            btnLoad = new PrimaryButton("ðŸ” Xem bÃ¡o cÃ¡o");
            btnLoad.Click += BtnLoad_Click;
            filterPanel.Controls.Add(btnLoad);

            var btnExport = new PrimaryButton("ðŸ“Š Xuáº¥t Excel") { Margin = new Padding(8, 0, 0, 0) };
            btnExport.Click += BtnExport_Click;
            filterPanel.Controls.Add(btnExport);

            // 4. Summary Panel (CÃ¡c tháº» tÃ³m táº¯t)
            summaryPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 0, 24, 12)
            };
            lblTotalRevenue = CreateSummaryCard("Tá»•ng Doanh thu (NÄƒm)", "0 VND");
            lblTotalTransactions = CreateSummaryCard("Tá»•ng Giao dá»‹ch (NÄƒm)", "0");
            summaryPanel.Controls.Add(lblTotalRevenue);
            summaryPanel.Controls.Add(lblTotalTransactions);
            mainLayout.Controls.Add(summaryPanel, 0, 2);

            // 5. Khu vá»±c ná»™i dung (chia 2 cá»™t)
            var contentSplit = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 0, 24, 24)
            };
            // Cá»™t bÃªn trÃ¡i 40%, bÃªn pháº£i 60%
            contentSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            contentSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
            mainLayout.Controls.Add(contentSplit, 0, 3);

            // 5a. Báº£ng Chi tiáº¿t ThÃ¡ng (BÃªn trÃ¡i)
            tblMonthlyData = new TableCustom
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 12, 0),
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            tblMonthlyData.Columns.Add("Month", "ThÃ¡ng");
            tblMonthlyData.Columns.Add("Revenue", "Doanh thu (VND)");
            tblMonthlyData.Columns["Revenue"].DefaultCellStyle.Format = "N0";
            tblMonthlyData.Columns["Revenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tblMonthlyData.Columns["Month"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tblMonthlyData.Columns["Revenue"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            contentSplit.Controls.Add(tblMonthlyData, 0, 0);

            // 5b. Báº£ng Top KhÃ¡ch hÃ ng (BÃªn pháº£i - chi tiáº¿t hÆ¡n)
            tblTopRoutes = new TableCustom
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(12, 0, 0, 0),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            tblTopRoutes.Columns.Add("Route", "Top 5 KhÃ¡ch hÃ ng");
            tblTopRoutes.Columns.Add("Flights", "Sá»‘ giao dá»‹ch");
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

        // Helper táº¡o tháº» tÃ³m táº¯t
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
                ForeColor = Color.FromArgb(0, 92, 175), // MÃ u xanh
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
                // Táº¡o SaveFileDialog
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                    sfd.FileName = $"BaoCaoDoanhThu_{(int)numYear.Value}.csv";
                    sfd.Title = "Xuáº¥t bÃ¡o cÃ¡o doanh thu";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var writer = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                        {
                            // Header
                            writer.WriteLine($"BÃO CÃO DOANH THU NÄ‚M {(int)numYear.Value}");
                            writer.WriteLine($"NgÃ y xuáº¥t: {DateTime.Now:dd/MM/yyyy HH:mm}");
                            writer.WriteLine();
                            
                            writer.WriteLine($"Tá»•ng doanh thu,{lblTotalRevenue.Controls[0].Text}");
                            writer.WriteLine($"Tá»•ng giao dá»‹ch,{lblTotalTransactions.Controls[0].Text}");
                            writer.WriteLine();

                            // Monthly data
                            writer.WriteLine("DOANH THU THEO THÃNG");
                            writer.WriteLine("ThÃ¡ng,Doanh thu (VND)");
                            foreach (DataGridViewRow row in tblMonthlyData.Rows)
                            {
                                if (row.IsNewRow) continue;
                                writer.WriteLine($"{row.Cells[0].Value},{row.Cells[1].Value}");
                            }
                            writer.WriteLine();

                            // Top customers
                            writer.WriteLine("TOP 5 KHÃCH HÃ€NG");
                            writer.WriteLine("KhÃ¡ch hÃ ng,Sá»‘ giao dá»‹ch,Doanh thu (VND)");
                            foreach (DataGridViewRow row in tblTopRoutes.Rows)
                            {
                                if (row.IsNewRow) continue;
                                writer.WriteLine($"{row.Cells[0].Value},{row.Cells[1].Value},{row.Cells[2].Value}");
                            }
                        }

                        MessageBox.Show($"Xuáº¥t bÃ¡o cÃ¡o thÃ nh cÃ´ng!\nFile: {sfd.FileName}", "ThÃ nh cÃ´ng", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lá»—i khi xuáº¥t bÃ¡o cÃ¡o: {ex.Message}", "Lá»—i", 
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
                MessageBox.Show(result.GetFullErrorMessage(), "Lá»—i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var report = result.GetData<RevenueReportViewModel>();

            // 1. Cáº­p nháº­t Tháº» TÃ³m táº¯t
            (lblTotalRevenue.Controls[0] as Label).Text = $"{report.TotalRevenue:N0} VND";
            (lblTotalTransactions.Controls[0] as Label).Text = $"{report.TotalTransactions:N0}";

            // 2. Cáº­p nháº­t Báº£ng Chi tiáº¿t ThÃ¡ng
            tblMonthlyData.Rows.Clear();
            var culture = new CultureInfo("vi-VN"); // "ThÃ¡ng 1", "ThÃ¡ng 2"

            // Chuyá»ƒn data sang Dictionary (ThÃ¡ng -> DoanhThu)
            var monthlyData = report.MonthlyBreakdown.AsEnumerable()
                .ToDictionary(
                    row => row.Field<int>("Thang"), // Key
                    row => row.Field<decimal>("DoanhThu") // Value
                );

            // LuÃ´n hiá»ƒn thá»‹ 12 thÃ¡ng
            for (int i = 1; i <= 12; i++)
            {
                string monthName = culture.DateTimeFormat.GetMonthName(i);
                monthName = char.ToUpper(monthName[0]) + monthName.Substring(1); // "ThÃ¡ng 1"

                decimal revenue = 0;
                if (monthlyData.ContainsKey(i))
                {
                    revenue = monthlyData[i];
                }

                tblMonthlyData.Rows.Add(monthName, revenue);
            }

            // 3. Cáº­p nháº­t Báº£ng Top KhÃ¡ch hÃ ng
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
                // Hiá»ƒn thá»‹ thÃ´ng bÃ¡o náº¿u khÃ´ng cÃ³ dá»¯ liá»‡u
                tblTopRoutes.Rows.Add("KhÃ´ng cÃ³ dá»¯ liá»‡u", 0, 0);
            }
        }
    }
}
