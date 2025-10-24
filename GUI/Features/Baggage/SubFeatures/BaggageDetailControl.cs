using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Tables;

namespace GUI.Features.Baggage.SubFeatures {
    public class BaggageDetailControl : UserControl {
        private Label lblTitle;
        private TableLayoutPanel grid;
        private Panel card;
        private Label secTitle;
        private TableLayoutPanel main;
        private TableCustom historyTable;

        public BaggageDetailControl() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            lblTitle = new Label();
            card = new Panel();
            grid = new TableLayoutPanel();
            secTitle = new Label();
            main = new TableLayoutPanel();
            card.SuspendLayout();
            main.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(3, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(100, 23);
            lblTitle.TabIndex = 0;
            // 
            // card
            // 
            card.Controls.Add(grid);
            card.Controls.Add(secTitle);
            card.Location = new Point(3, 26);
            card.Name = "card";
            card.Size = new Size(194, 71);
            card.TabIndex = 1;
            // 
            // grid
            // 
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            grid.Location = new Point(0, 0);
            grid.Name = "grid";
            grid.Size = new Size(200, 100);
            grid.TabIndex = 0;
            // 
            // secTitle
            // 
            secTitle.Location = new Point(0, 0);
            secTitle.Name = "secTitle";
            secTitle.Size = new Size(100, 23);
            secTitle.TabIndex = 1;
            // 
            // main
            // 
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(card, 0, 1);
            main.Location = new Point(0, 0);
            main.Name = "main";
            main.RowStyles.Add(new RowStyle());
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            main.Size = new Size(200, 100);
            main.TabIndex = 0;
            // 
            // BaggageDetailControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Controls.Add(main);
            Name = "BaggageDetailControl";
            Size = new Size(1460, 409);
            card.ResumeLayout(false);
            main.ResumeLayout(false);
            ResumeLayout(false);
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
