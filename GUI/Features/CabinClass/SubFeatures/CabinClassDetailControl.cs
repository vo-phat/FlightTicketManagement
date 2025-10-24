using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.CabinClass.SubFeatures {
    public class CabinClassDetailControl : UserControl {
        private Label vCode, vName, vTier, vPriority, vBaggage, vPitch, vDesc;

        public CabinClassDetailControl() { InitializeComponent(); BuildLayout(); }

        private void InitializeComponent() { Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252); }

        private static Label Key(string t) => new Label { Text = t, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Margin = new Padding(0, 6, 12, 6) };
        private static Label Val(string n) => new Label { Name = n, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Regular), Margin = new Padding(0, 6, 0, 6) };

        private void BuildLayout() {
            var title = new Label { Text = "🛋 Chi tiết Cabin", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold), Padding = new Padding(24, 20, 24, 0), Dock = DockStyle.Top };
            var card = new Panel { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(16), Margin = new Padding(24, 8, 24, 24), Dock = DockStyle.Fill };

            var sec = new Label { Text = "Thông tin", AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Margin = new Padding(0, 0, 0, 16) };
            var grid = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int r = 0;
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Mã cabin:"), 0, r); vCode = Val("vCode"); grid.Controls.Add(vCode, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Tên cabin:"), 0, r); vName = Val("vName"); grid.Controls.Add(vName, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Thứ hạng:"), 0, r); vTier = Val("vTier"); grid.Controls.Add(vTier, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Độ ưu tiên:"), 0, r); vPriority = Val("vPriority"); grid.Controls.Add(vPriority, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hành lý mặc định (kg):"), 0, r); vBaggage = Val("vBaggage"); grid.Controls.Add(vBaggage, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Seat pitch (in):"), 0, r); vPitch = Val("vPitch"); grid.Controls.Add(vPitch, 1, r++);

            var sec2 = new Label { Text = "Mô tả", AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Margin = new Padding(0, 16, 0, 8) };
            vDesc = new Label { AutoSize = true, Font = new Font("Segoe UI", 10f), Text = "—", Margin = new Padding(0, 0, 0, 8) };

            card.Controls.Add(vDesc); card.Controls.Add(sec2);
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

        public void LoadCabin(string code, string name, string tier, string priority, string baggage, string pitch, string desc = "") {
            vCode.Text = code; vName.Text = name; vTier.Text = tier;
            vPriority.Text = priority; vBaggage.Text = baggage; vPitch.Text = pitch;
            vDesc.Text = string.IsNullOrWhiteSpace(desc) ? "—" : desc;
        }
    }
}
