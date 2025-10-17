using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Tables;

namespace FlightTicketManagement.GUI.Features.Airline.SubFeatures {
    public class AirlineCreateControl : UserControl {
        public AirlineCreateControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // Title
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label { Text = "➕ Tạo hãng hàng không", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
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

            // Theo script.sql: airline_code (unique), airline_name, country
            var txtCode = new UnderlinedTextField("Mã hãng (duy nhất)", "") { MinimumSize = new Size(0, 56), Width = 220, Margin = new Padding(0, 6, 24, 6) };
            var txtName = new UnderlinedTextField("Tên hãng", "") { MinimumSize = new Size(0, 56), Width = 320, Margin = new Padding(0, 6, 24, 6) };

            var cbCountry = new UnderlinedComboBox(
    "Quốc gia",
    new object[] { "Việt Nam", "Nhật Bản", "Hàn Quốc", "Singapore", "Thái Lan", "Hoa Kỳ", "Anh", "Pháp", "Úc", "Canada" }
) { MinimumSize = new Size(0, 56), Width = 240, Margin = new Padding(0, 6, 24, 6) };

            cbCountry.BackColor = this.BackColor;
            // lấy giá trị khi lưu:
            var country = cbCountry.SelectedText;

            inputPanel.Controls.Add(txtCode, 0, 0);
            inputPanel.Controls.Add(txtName, 1, 0);
            inputPanel.Controls.Add(cbCountry, 0, 1);

            // fix chiều cao hàng theo PreferredSize (tránh cắt underline)
            for (int r = 0; r < inputPanel.RowCount; r++) {
                int h = 0;
                for (int c = 0; c < inputPanel.ColumnCount; c++) {
                    var ctl = inputPanel.GetControlFromPosition(c, r);
                    if (ctl != null) h = Math.Max(h, ctl.GetPreferredSize(Size.Empty).Height + ctl.Margin.Vertical);
                }
                inputPanel.RowStyles[r] = new RowStyle(SizeType.Absolute, Math.Max(72, h + 2));
            }

            // Buttons
            var btnCreate = new PrimaryButton("💾 Lưu hãng") { Height = 40, Width = 140, Margin = new Padding(0, 12, 0, 12), Anchor = AnchorStyles.Right };
            var buttonRow = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(24, 0, 24, 0), WrapContents = false };
            buttonRow.Controls.Add(btnCreate);

            // Preview table
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
            table.Columns.Add("airlineCode", "Mã");
            table.Columns.Add("airlineName", "Tên hãng");
            table.Columns.Add("country", "Quốc gia");
            for (int i = 0; i < 4; i++) table.Rows.Add("", "", "");

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
