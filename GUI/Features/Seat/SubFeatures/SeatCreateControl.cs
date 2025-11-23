using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using GUI.Components.Inputs;

namespace GUI.Features.Seat.SubFeatures {
    public class SeatCreateControl : UserControl {
        private UnderlinedTextField txtSeat;
        private UnderlinedComboBox cbAircraft, cbClass;
        private PrimaryButton btnSave;

        public SeatCreateControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // Title
            var title = new Label {
                Text = "➕ Tạo ghế (per Aircraft)",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // Inputs
            var inputs = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 12, 24, 0),
                ColumnCount = 3,
                RowCount = 1
            };
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));

            cbAircraft = new UnderlinedComboBox("Máy bay", new object[] { "A320", "B737" }) { Width = 260, MinimumSize = new Size(0, 56), Margin = new Padding(0, 0, 24, 0) };
            cbClass = new UnderlinedComboBox("Hạng ghế", new object[] { "Economy", "Business", "First" }) { Width = 220, MinimumSize = new Size(0, 56), Margin = new Padding(0, 0, 24, 0) };
            txtSeat = new UnderlinedTextField("Số ghế (VD: 12A)", "") { Width = 160, MinimumSize = new Size(0, 56) };

            inputs.Controls.Add(cbAircraft, 0, 0);
            inputs.Controls.Add(cbClass, 1, 0);
            inputs.Controls.Add(txtSeat, 2, 0);

            // Actions
            btnSave = new PrimaryButton("💾 Lưu") { Width = 120, Height = 40, Margin = new Padding(0, 12, 0, 12) };
            var actions = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(24, 0, 24, 0) };
            actions.Controls.Add(btnSave);

            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, BackColor = Color.Transparent };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(title, 0, 0);
            main.Controls.Add(inputs, 0, 1);
            main.Controls.Add(actions, 0, 2);

            Controls.Add(main);

            // Behavior (demo)
            txtSeat.TextChanged += (_, __) => {
                var t = txtSeat.Text ?? string.Empty;
                var num = new string(t.TakeWhile(char.IsDigit).ToArray());
                var alpha = new string(t.SkipWhile(char.IsDigit).Where(char.IsLetter).ToArray()).ToUpperInvariant();
                if (alpha.Length > 2) alpha = alpha.Substring(0, 2);
                var norm = num + alpha;
                if (norm != t) txtSeat.Text = norm;
            };

            btnSave.Click += (_, __) => {
                var ac = cbAircraft.SelectedItem?.ToString() ?? "";
                var cl = cbClass.SelectedItem?.ToString() ?? "";
                var seat = (txtSeat.Text ?? "").Trim().ToUpperInvariant();
                if (string.IsNullOrEmpty(ac) || string.IsNullOrEmpty(cl) || string.IsNullOrEmpty(seat)) {
                    MessageBox.Show("Vui lòng nhập đủ Máy bay / Hạng ghế / Số ghế.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // DEMO only: In real app, call INSERT INTO Seats(...)
                MessageBox.Show($"[DEMO]\nĐã lưu ghế {seat}\nAircraft: {ac}\nCabin: {cl}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSeat.Text = "";
            };
        }
    }
}
