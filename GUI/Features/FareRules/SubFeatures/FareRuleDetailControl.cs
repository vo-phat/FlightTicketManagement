using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.FareRules.SubFeatures {
    public class FareRuleDetailControl : UserControl {
        private Label vRuleId, vRoute, vCabin, vFareType, vSeason, vEffective, vExpiry, vPrice, vDesc;

        public FareRuleDetailControl() {
            InitializeComponent();
            BuildLayout();
        }

        private void InitializeComponent() {
            SuspendLayout();
            BackColor = Color.FromArgb(232, 240, 252);
            Name = "FareRuleDetailControl";
            Size = new Size(1000, 600);
            ResumeLayout(false);
        }

        private static Label Key(string t) =>
            new Label { Text = t, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Margin = new Padding(0, 6, 12, 6) };

        private static Label Val(string n) =>
            new Label { Name = n, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Regular), Margin = new Padding(0, 6, 0, 6) };

        private void BuildLayout() {
            // ===== Title =====
            var lblTitle = new Label {
                Text = "📜 Chi tiết Quy tắc vé",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Card =====
            var card = new Panel {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(16),
                Margin = new Padding(24, 8, 24, 24),
                Dock = DockStyle.Fill
            };

            // ===== Grid thông tin =====
            var grid = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int r = 0;
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Mã quy tắc:"), 0, r); vRuleId = Val("vRuleId"); grid.Controls.Add(vRuleId, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Tuyến bay:"), 0, r); vRoute = Val("vRoute"); grid.Controls.Add(vRoute, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hạng vé:"), 0, r); vCabin = Val("vCabin"); grid.Controls.Add(vCabin, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Loại vé:"), 0, r); vFareType = Val("vFareType"); grid.Controls.Add(vFareType, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Mùa:"), 0, r); vSeason = Val("vSeason"); grid.Controls.Add(vSeason, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hiệu lực từ:"), 0, r); vEffective = Val("vEffective"); grid.Controls.Add(vEffective, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hết hạn vào:"), 0, r); vExpiry = Val("vExpiry"); grid.Controls.Add(vExpiry, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Giá vé (₫):"), 0, r); vPrice = Val("vPrice"); grid.Controls.Add(vPrice, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Mô tả:"), 0, r); vDesc = Val("vDesc"); grid.Controls.Add(vDesc, 1, r++);

            card.Controls.Add(grid);

            // ===== Bottom =====
            var bottom = new FlowLayoutPanel {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(0, 12, 12, 12)
            };
            var btnClose = new Button {
                Text = "Đóng",
                AutoSize = true,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                BackColor = Color.FromArgb(0, 92, 175),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (_, __) => FindForm()?.Close();
            bottom.Controls.Add(btnClose);

            card.Controls.Add(bottom);

            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Add(main);
        }

        // === Hàm nhận dữ liệu từ list ===
        public void LoadRule(int ruleId, string route, string cabin, string fareType,
                             string season, DateTime eff, DateTime exp, decimal price, string desc) {
            vRuleId.Text = ruleId.ToString();
            vRoute.Text = route;
            vCabin.Text = cabin;
            vFareType.Text = fareType;
            vSeason.Text = season;
            vEffective.Text = eff.ToString("dd/MM/yyyy");
            vExpiry.Text = exp.ToString("dd/MM/yyyy");
            vPrice.Text = $"{price:N0}";
            vDesc.Text = desc;
        }
    }
}
