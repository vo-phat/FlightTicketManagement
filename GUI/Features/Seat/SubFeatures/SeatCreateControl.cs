using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;

namespace GUI.Features.Seat.SubFeatures
{
    public class SeatCreateControl : UserControl
    {
        private TableLayoutPanel root, form;
        private Label lblTitle;

        private UnderlinedComboBox cbAircraft, cbClass;
        private UnderlinedTextField txtSeat;
        private PrimaryButton btnSave;
        private SecondaryButton btnReset;

        public SeatCreateControl() { InitializeComponent(); }

        private void InitializeComponent()
        {
            SuspendLayout();
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label
            {
                Text = "➕ Tạo ghế",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            form = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24),
                ColumnCount = 2
            };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            cbAircraft = new UnderlinedComboBox("Máy bay", new object[] { "A320", "B737" }) { Width = 260 };
            cbClass = new UnderlinedComboBox("Hạng ghế", new object[] { "Economy", "Business" }) { Width = 260 };
            txtSeat = new UnderlinedTextField("Số ghế (VD: 12A)", "") { Width = 260 };

            form.Controls.Add(new Label { Text = "Máy bay", AutoSize = true, Margin = new Padding(0, 8, 8, 8) }, 0, 0);
            form.Controls.Add(cbAircraft, 1, 0);
            form.Controls.Add(new Label { Text = "Hạng ghế", AutoSize = true, Margin = new Padding(0, 8, 8, 8) }, 0, 1);
            form.Controls.Add(cbClass, 1, 1);
            form.Controls.Add(new Label { Text = "Số ghế", AutoSize = true, Margin = new Padding(0, 8, 8, 8) }, 0, 2);
            form.Controls.Add(txtSeat, 1, 2);

            var actions = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(24, 6, 24, 6), WrapContents = false };
            btnSave = new PrimaryButton("💾 Lưu") { Width = 100, Height = 36 };
            btnReset = new SecondaryButton("⟲ Làm lại") { Width = 110, Height = 36, Margin = new Padding(12, 0, 0, 0) };
            btnSave.Click += Save_Click;
            btnReset.Click += (_, __) => { txtSeat.Text = ""; cbAircraft.SelectedIndex = -1; cbClass.SelectedIndex = -1; };
            actions.Controls.AddRange(new Control[] { btnSave, btnReset });

            root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(form, 0, 1);
            root.Controls.Add(new Panel(), 0, 2);

            Controls.Add(root);
            ResumeLayout(false);
        }

        private void Save_Click(object? sender, EventArgs e)
        {
            if (cbAircraft.SelectedIndex < 0) { MessageBox.Show("Vui lòng chọn máy bay"); return; }
            if (cbClass.SelectedIndex < 0) { MessageBox.Show("Vui lòng chọn hạng ghế"); return; }
            var seat = (txtSeat.Text ?? "").Trim().ToUpper();
            if (!Regex.IsMatch(seat, "^[1-9][0-9]*[A-F]$")) { MessageBox.Show("Số ghế không hợp lệ (VD: 12A)"); return; }

            // TODO: Gọi service lưu vào DB (Seats)
            MessageBox.Show("Đã tạo ghế (demo)");
        }
    }
}