using System;
using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Tables;

namespace FlightTicketManagement.GUI.Features.Baggage.SubFeatures {
    public class BaggageDetailControl : UserControl {
        private Label lblTitle;
        private TableLayoutPanel grid;
        private Panel card;
        private TableCustom historyTable;

        public BaggageDetailControl() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label {
                Text = "📦 Theo dõi / Chi tiết hành lý",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            card = new Panel {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(16),
                Margin = new Padding(24, 8, 24, 24),
                Dock = DockStyle.Fill
            };

            var secTitle = new Label {
                Text = "Thông tin hành lý",
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 16),
                Dock = DockStyle.Top
            };

            grid = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            card.Controls.Add(grid);
            card.Controls.Add(secTitle);

            historyTable = new TableCustom {
                Dock = DockStyle.Bottom,
                Height = 160,
                Margin = new Padding(0, 12, 0, 0),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            historyTable.Columns.Add("changedAt", "Thời điểm");
            historyTable.Columns.Add("oldStatus", "Trạng thái cũ");
            historyTable.Columns.Add("newStatus", "Trạng thái mới");
            card.Controls.Add(historyTable);

            var main = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 2
            };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Add(main);
        }

        private static Label Key(string text) => new Label {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            Margin = new Padding(0, 6, 12, 6)
        };
        private static Label Val() => new Label {
            Text = "",
            AutoSize = true,
            Font = new Font("Segoe UI", 10f, FontStyle.Regular),
            Margin = new Padding(0, 6, 0, 6)
        };

        public void LoadBaggageInfo(BaggageListControl.BaggageRow row) {
            grid.Controls.Clear();
            grid.RowStyles.Clear();

            void AddRow(string k, string v, int r) {
                grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                grid.Controls.Add(Key(k), 0, r);
                var val = Val(); val.Text = v;
                grid.Controls.Add(val, 1, r);
            }

            int rIdx = 0;
            AddRow("Mã nhãn:", row.BaggageTag, rIdx++);
            AddRow("Loại:", row.Type, rIdx++);
            AddRow("Cân nặng (kg):", row.WeightKg.ToString("0.##"), rIdx++);
            AddRow("Định mức (kg):", row.AllowedWeightKg.ToString("0.##"), rIdx++);
            AddRow("Phí:", row.Fee.ToString("0.##"), rIdx++);
            AddRow("Trạng thái:", row.Status, rIdx++);
            AddRow("Mã chuyến bay:", row.FlightId.ToString(), rIdx++);
            AddRow("Mã vé:", row.TicketId.ToString(), rIdx++);

            // DEMO lịch sử (thay bằng SELECT Baggage_History ORDER BY changed_at)
            historyTable.Rows.Clear();
            historyTable.Rows.Add(DateTime.Now.AddMinutes(-40).ToString("HH:mm dd/MM"), "CREATED", "CHECKED_IN");
            historyTable.Rows.Add(DateTime.Now.AddMinutes(-10).ToString("HH:mm dd/MM"), "CHECKED_IN", "LOADED");
        }
    }
}
