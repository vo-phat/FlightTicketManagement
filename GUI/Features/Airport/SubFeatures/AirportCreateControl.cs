using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using GUI.Components.Inputs;

namespace GUI.Features.Airport.SubFeatures {
    public class AirportCreateControl : UserControl {
        private UnderlinedTextField _txtCode, _txtName, _txtCity;
        private UnderlinedComboBox _cbCountry, _cbTimezone;

        public AirportCreateControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            // Title
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label { Text = "➕ Tạo sân bay", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
            titlePanel.Controls.Add(lblTitle);

            // Inputs
            var inputs = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 12, 24, 0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 3
            };
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            for (int i = 0; i < 3; i++) inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            _txtCode = new UnderlinedTextField("Mã IATA", "") { MinimumSize = new Size(0, 56), Width = 200, Margin = new Padding(0, 6, 24, 6) };
            _txtName = new UnderlinedTextField("Tên sân bay", "") { MinimumSize = new Size(0, 56), Width = 320, Margin = new Padding(0, 6, 24, 6) };
            _txtCity = new UnderlinedTextField("Thành phố", "") { MinimumSize = new Size(0, 56), Width = 240, Margin = new Padding(0, 6, 24, 6) };
            _cbCountry = new UnderlinedComboBox("Quốc gia", new object[] { "Việt Nam", "Nhật Bản", "Hàn Quốc", "Singapore", "Thái Lan", "Hoa Kỳ", "Anh", "Pháp", "Úc", "Canada" }) { MinimumSize = new Size(0, 56), Width = 240, Margin = new Padding(0, 6, 24, 6) };
            _cbTimezone = new UnderlinedComboBox("Múi giờ", new object[] { "UTC−5", "UTC−4", "UTC", "UTC+1", "UTC+7", "UTC+8", "UTC+9" }) { MinimumSize = new Size(0, 56), Width = 200, Margin = new Padding(0, 6, 24, 6) };

            inputs.Controls.Add(_txtCode, 0, 0);
            inputs.Controls.Add(_txtName, 1, 0);
            inputs.Controls.Add(_txtCity, 0, 1);
            inputs.Controls.Add(_cbCountry, 1, 1);
            inputs.Controls.Add(_cbTimezone, 0, 2);

            // ✅ fix chiều cao hàng (không cắt underline)
            for (int r = 0; r < inputs.RowCount; r++) {
                int h = 0;
                for (int c = 0; c < inputs.ColumnCount; c++) {
                    var ctl = inputs.GetControlFromPosition(c, r);
                    if (ctl != null) h = System.Math.Max(h, ctl.GetPreferredSize(Size.Empty).Height + ctl.Margin.Vertical);
                }
                inputs.RowStyles[r] = new RowStyle(SizeType.Absolute, System.Math.Max(72, h + 2));
            }

            // Buttons
            var btnSave = new PrimaryButton("💾 Lưu sân bay") { Width = 150, Height = 40, Margin = new Padding(0, 12, 0, 12) };
            var buttonRow = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(24, 0, 24, 0), WrapContents = false };
            buttonRow.Controls.Add(btnSave);

            // Preview table (optional)
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
            table.Columns.Add("airportCode", "IATA");
            table.Columns.Add("airportName", "Tên sân bay");
            table.Columns.Add("city", "Thành phố");
            table.Columns.Add("country", "Quốc gia");
            table.Columns.Add("timezone", "Múi giờ");
            for (int i = 0; i < 3; i++) table.Rows.Add("", "", "", "", "");

            // Layout tổng
            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, BackColor = Color.Transparent };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            main.Controls.Add(titlePanel, 0, 0);
            main.Controls.Add(inputs, 0, 1);
            main.Controls.Add(buttonRow, 0, 2);
            main.Controls.Add(table, 0, 3);

            Controls.Add(main);

            // Behaviors
            _txtCode.TextChanged += (_, __) => {
                var t = _txtCode.Text ?? string.Empty;
                // lọc chỉ A–Z
                var filtered = new string(t.Where(char.IsLetter).ToArray()).ToUpperInvariant();
                if (filtered.Length > 3) filtered = filtered.Substring(0, 3);

                if (filtered != _txtCode.Text) {
                    _txtCode.Text = filtered;
                    // KHÔNG cần SelectionStart; đa số trường hợp caret sẽ ở cuối sau khi set Text
                }
            };
        }
    }
}
