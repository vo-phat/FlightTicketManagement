using System.Drawing;
using System.Windows.Forms;

namespace FlightTicketManagement.GUI.Features.FareRules.SubFeatures {
    public class FareRuleDetailControl : UserControl {
        private Label vCode, vName, vCabin, vRefund, vFee, vBag, vMinStay, vMaxStay, vAdv, vNotes;

        public FareRuleDetailControl() { InitializeComponent(); BuildLayout(); }

        private void InitializeComponent() {
            SuspendLayout();
            // 
            // FareRuleDetailControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Name = "FareRuleDetailControl";
            Size = new Size(1460, 577);
            ResumeLayout(false);
        }

        private static Label Key(string t) => new Label { Text = t, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Margin = new Padding(0, 6, 12, 6) };
        private static Label Val(string n) => new Label { Name = n, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Regular), Margin = new Padding(0, 6, 0, 6) };

        private void BuildLayout() {
            var title = new Label { Text = "📜 Chi tiết Fare Rule", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold), Padding = new Padding(24, 20, 24, 0), Dock = DockStyle.Top };
            var card = new Panel { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(16), Margin = new Padding(24, 8, 24, 24), Dock = DockStyle.Fill };

            var sec = new Label { Text = "Thông tin cơ bản", AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Margin = new Padding(0, 0, 0, 16) };
            var grid = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int r = 0;
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Mã rule:"), 0, r); vCode = Val("vCode"); grid.Controls.Add(vCode, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Tên rule:"), 0, r); vName = Val("vName"); grid.Controls.Add(vName, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hạng vé:"), 0, r); vCabin = Val("vCabin"); grid.Controls.Add(vCabin, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hoàn vé:"), 0, r); vRefund = Val("vRefund"); grid.Controls.Add(vRefund, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Phí đổi (₫):"), 0, r); vFee = Val("vFee"); grid.Controls.Add(vFee, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hành lý (kg):"), 0, r); vBag = Val("vBag"); grid.Controls.Add(vBag, 1, r++);

            var sec2 = new Label { Text = "Điều kiện bổ sung", AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Margin = new Padding(0, 16, 0, 16) };
            var grid2 = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            grid2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
            grid2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            int r2 = 0;
            grid2.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid2.Controls.Add(Key("Lưu trú tối thiểu (đêm):"), 0, r2); vMinStay = Val("vMinStay"); grid2.Controls.Add(vMinStay, 1, r2++);
            grid2.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid2.Controls.Add(Key("Lưu trú tối đa (đêm):"), 0, r2); vMaxStay = Val("vMaxStay"); grid2.Controls.Add(vMaxStay, 1, r2++);
            grid2.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid2.Controls.Add(Key("Mua trước tối thiểu (ngày):"), 0, r2); vAdv = Val("vAdv"); grid2.Controls.Add(vAdv, 1, r2++);
            grid2.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid2.Controls.Add(Key("Ghi chú:"), 0, r2); vNotes = Val("vNotes"); grid2.Controls.Add(vNotes, 1, r2++);

            card.Controls.Add(grid2); card.Controls.Add(sec2);
            card.Controls.Add(grid); card.Controls.Add(sec);

            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 12, 12, 12) };
            var btnClose = new Button { Text = "Đóng", AutoSize = true }; btnClose.Click += (_, __) => FindForm()?.Close();
            bottom.Controls.Add(btnClose); card.Controls.Add(bottom);

            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(title, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Add(main);
        }

        public void LoadRule(string code, string name, string cabin, string refundable, string fee, string baggage,
                             string minStay = "", string maxStay = "", string advance = "", string notes = "") {
            vCode.Text = code; vName.Text = name; vCabin.Text = cabin;
            vRefund.Text = refundable; vFee.Text = fee; vBag.Text = baggage;
            vMinStay.Text = minStay; vMaxStay.Text = maxStay; vAdv.Text = advance; vNotes.Text = notes;
        }
    }
}
