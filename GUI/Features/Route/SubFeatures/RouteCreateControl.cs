using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using GUI.Components.Tables;

namespace GUI.Features.Route.SubFeatures {
    public class RouteCreateControl : UserControl {
        public RouteCreateControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // Title
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label { Text = "➕ Tạo tuyến bay mới", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
            titlePanel.Controls.Add(lblTitle);

            // Inputs (2 x 2 + thêm hàng khoảng cách/thời lượng)
            var inputPanel = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 12, 24, 0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 3
            };
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            for (int i = 0; i < 3; i++) inputPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            var txtFrom = new UnderlinedTextField("Sân bay đi (mã)", "") { MinimumSize = new Size(0, 56), Width = 250 };
            var txtTo = new UnderlinedTextField("Sân bay đến (mã)", "") { MinimumSize = new Size(0, 56), Width = 250 };
            var txtDist = new UnderlinedTextField("Khoảng cách (km)", "") { MinimumSize = new Size(0, 56), Width = 250 };
            var txtDur = new UnderlinedTextField("Thời lượng (phút)", "") { MinimumSize = new Size(0, 56), Width = 250 };

            inputPanel.Controls.Add(txtFrom, 0, 0);
            inputPanel.Controls.Add(txtTo, 1, 0);
            inputPanel.Controls.Add(txtDist, 0, 1);
            inputPanel.Controls.Add(txtDur, 1, 1);

            // buffer hàng cuối (để dễ thêm field sau)
            inputPanel.Controls.Add(new Panel { Height = 1, Dock = DockStyle.Top }, 0, 2);

            // ✅ Fix chiều cao hàng (không cắt underline)
            for (int r = 0; r < inputPanel.RowCount; r++) {
                int h = 0;
                for (int c = 0; c < inputPanel.ColumnCount; c++) {
                    var ctl = inputPanel.GetControlFromPosition(c, r);
                    if (ctl != null) h = Math.Max(h, ctl.GetPreferredSize(Size.Empty).Height + ctl.Margin.Vertical);
                }
                inputPanel.RowStyles[r] = new RowStyle(SizeType.Absolute, Math.Max(72, h + 2));
            }

            // Buttons
            var btnCreate = new PrimaryButton("💾 Lưu tuyến") { Height = 40, Width = 140, Margin = new Padding(0, 12, 0, 12), Anchor = AnchorStyles.Right };
            var buttonRow = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(24, 0, 24, 0), WrapContents = false };
            buttonRow.Controls.Add(btnCreate);

            // Table preview
            var table = new TableCustom {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 4),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            table.Columns.Add("fromAirport", "Sân bay đi");
            table.Columns.Add("toAirport", "Sân bay đến");
            table.Columns.Add("distance", "Khoảng cách (km)");
            table.Columns.Add("duration", "Thời lượng (phút)");
            for (int i = 0; i < 4; i++) table.Rows.Add("", "", "", "");

            // Layout tổng
            var main = new TableLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.Transparent, ColumnCount = 1, RowCount = 4 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            main.Controls.Add(titlePanel, 0, 0);
            main.Controls.Add(inputPanel, 0, 1);
            main.Controls.Add(buttonRow, 0, 2);
            main.Controls.Add(table, 0, 3);

            Controls.Clear();
            Controls.Add(main);
        }
    }
}
