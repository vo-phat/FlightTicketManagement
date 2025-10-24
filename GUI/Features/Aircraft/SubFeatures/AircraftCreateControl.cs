using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using GUI.Components.Tables;

namespace GUI.Features.Aircraft.SubFeatures {
    public class AircraftCreateControl : UserControl {
        public AircraftCreateControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // Title
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label { Text = "➕ Tạo máy bay mới", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
            titlePanel.Controls.Add(lblTitle);

            // Inputs (2 x 2)
            var inputPanel = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 12, 24, 0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 2
            };
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            for (int i = 0; i < 2; i++) inputPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            // Theo script.sql: airline_id, model, manufacturer, capacity
            var txtAirline = new UnderlinedTextField("Mã hãng (Airline code)", "") { MinimumSize = new Size(0, 56), Width = 240, Margin = new Padding(0, 6, 24, 6) };
            var txtModel = new UnderlinedTextField("Model", "") { MinimumSize = new Size(0, 56), Width = 280, Margin = new Padding(0, 6, 24, 6) };
            var txtManu = new UnderlinedTextField("Hãng sản xuất", "") { MinimumSize = new Size(0, 56), Width = 280, Margin = new Padding(0, 6, 24, 6) };
            var txtCap = new UnderlinedTextField("Sức chứa (ghế)", "") { MinimumSize = new Size(0, 56), Width = 180, Margin = new Padding(0, 6, 24, 6) };

            inputPanel.Controls.Add(txtAirline, 0, 0);
            inputPanel.Controls.Add(txtModel, 1, 0);
            inputPanel.Controls.Add(txtManu, 0, 1);
            inputPanel.Controls.Add(txtCap, 1, 1);

            // ✅ Fix chiều cao hàng theo PreferredSize (tránh cắt underline)
            for (int r = 0; r < inputPanel.RowCount; r++) {
                int h = 0;
                for (int c = 0; c < inputPanel.ColumnCount; c++) {
                    var ctl = inputPanel.GetControlFromPosition(c, r);
                    if (ctl != null) h = Math.Max(h, ctl.GetPreferredSize(Size.Empty).Height + ctl.Margin.Vertical);
                }
                inputPanel.RowStyles[r] = new RowStyle(SizeType.Absolute, Math.Max(72, h + 2));
            }

            // Buttons
            var btnCreate = new PrimaryButton("💾 Lưu máy bay") { Height = 40, Width = 160, Margin = new Padding(0, 12, 0, 12), Anchor = AnchorStyles.Right };
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
            table.Columns.Add("airline", "Hãng");
            table.Columns.Add("model", "Model");
            table.Columns.Add("manufacturer", "Hãng sản xuất");
            table.Columns.Add("capacity", "Sức chứa");
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
