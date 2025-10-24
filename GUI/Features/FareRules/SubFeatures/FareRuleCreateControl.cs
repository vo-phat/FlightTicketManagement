using System;
using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Tables;

namespace FlightTicketManagement.GUI.Features.FareRules.SubFeatures {
    public class FareRuleCreateControl : UserControl {
        public FareRuleCreateControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            // Title
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label { Text = "➕ Tạo quy tắc", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
            titlePanel.Controls.Add(lblTitle);

            // Inputs (4 hàng x 2 cột) - GIỮ NGUYÊN CẤU TRÚC
            var inputs = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 12, 24, 0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 4
            };
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            for (int i = 0; i < 4; i++) inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            // ======= FIELDS THEO DB Fare_Rules =======
            // Hàng 1
            var txtRoute = new UnderlinedTextField("Tuyến bay", "") {
                MinimumSize = new Size(0, 64),
                Width = 300,
                Margin = new Padding(0, 6, 24, 6)
            };
            var cbCabin = new UnderlinedComboBox("Hạng vé (Cabin Class)", new object[] {"THƯƠNG GIA", "PHỔ THÔNG" }) { 
                MinimumSize = new Size(0, 64), 
                Width = 300, Margin = new Padding(0, 6, 24, 6) 
            };

            // Hàng 2
            var cbFareType = new UnderlinedComboBox("Loại vé (fare_type)", new object[] { "TIÊU CHUẨN", "TIẾT KIỆM", "KHUYẾN MÃI" }) { 
                MinimumSize = new Size(0, 64), 
                Width = 300, 
                Margin = new Padding(0, 6, 24, 6) 
            };

            var cbSeason = new UnderlinedComboBox("Mùa", new object[] { "CAO ĐIỂM", "TRUNG ĐIỂM", "THẤP ĐIỂM" }) { 
                MinimumSize = new Size(0, 64), Width = 300, 
                Margin = new Padding(0, 6, 24, 6) 
            };

            // Hàng 3 (ngày hiệu lực / hết hạn)
            var dtEffective = new DateTimePicker {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                MinDate = new DateTime(2000, 1, 1),
                Height = 36,
                Width = 300
            };
            var wrapEffective = new UnderlinedTextField("Hiệu lực đến", "") {
                MinimumSize = new Size(0, 64),
                Width = 300,
                Margin = new Padding(0, 6, 24, 6)
            };
            wrapEffective.Controls.Add(dtEffective); dtEffective.Location = new Point(0, wrapEffective.Height / 2);

            var dtExpiry = new DateTimePicker {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                MinDate = new DateTime(2000, 1, 1),
                Height = 36,
                Width = 300
            };
            var wrapExpiry = new UnderlinedTextField("Ngày hết hạn", "") {
                MinimumSize = new Size(0, 64),
                Width = 300,
                Margin = new Padding(0, 6, 24, 6)
            };
            wrapExpiry.Controls.Add(dtExpiry); dtExpiry.Location = new Point(0, wrapExpiry.Height / 2);

            // Hàng 4 (giá / mô tả)
            var txtPrice = new UnderlinedTextField("Giá", "") {
                MinimumSize = new Size(0, 64),
                Width = 300,
                Margin = new Padding(0, 6, 24, 6)
            };
            txtPrice.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };

            var txtDescription = new UnderlinedTextField("Mô tả", "") {
                MinimumSize = new Size(0, 64),
                Width = 300,
                Margin = new Padding(0, 6, 24, 6)
            };

            // Gắn vào lưới (giữ đúng vị trí 2 cột × 4 hàng)
            inputs.Controls.Add(txtRoute, 0, 0);
            inputs.Controls.Add(cbCabin, 1, 0);
            inputs.Controls.Add(cbFareType, 0, 1);
            inputs.Controls.Add(cbSeason, 1, 1);
            inputs.Controls.Add(wrapEffective, 0, 2);
            inputs.Controls.Add(wrapExpiry, 1, 2);
            inputs.Controls.Add(txtPrice, 0, 3);
            inputs.Controls.Add(txtDescription, 1, 3);

            // ✅ fix chiều cao từng hàng theo PreferredSize để không cắt underline
            for (int r = 0; r < inputs.RowCount; r++) {
                int h = 0;
                for (int c = 0; c < inputs.ColumnCount; c++) {
                    var ctl = inputs.GetControlFromPosition(c, r);
                    if (ctl != null) h = Math.Max(h, ctl.GetPreferredSize(Size.Empty).Height + ctl.Margin.Vertical);
                }
                inputs.RowStyles[r] = new RowStyle(SizeType.Absolute, Math.Max(72, h + 2));
            }

            // Buttons
            var btnSave = new PrimaryButton("💾 Lưu quy tắc") { Width = 140, Height = 40, Margin = new Padding(0, 12, 0, 12) };
            var buttonRow = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(24, 0, 24, 0),
                WrapContents = false
            };
            buttonRow.Controls.Add(btnSave);

            // Preview table (đã đổi cột đúng DB)
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
            table.Columns.Add("route", "Tuyến");
            table.Columns.Add("cabin", "Hạng vé");
            table.Columns.Add("fareType", "Loại vé");
            table.Columns.Add("season", "Mùa");
            table.Columns.Add("effective", "Hiệu lực");
            table.Columns.Add("expiry", "Hết hạn");
            table.Columns.Add("price", "Giá");
            for (int i = 0; i < 3; i++) table.Rows.Add("", "", "", "", "", "", "");

            // Layout tổng (GIỮ NGUYÊN)
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

            // Demo lưu (chưa kết nối DB thực)
            btnSave.Click += (_, __) => {
                // TODO: map route -> route_id, cabin -> class_id khi có danh mục
                MessageBox.Show(
                    "Đã lưu (mô phỏng) Fare Rule với các trường:\n" +
                    $"- Tuyến: {txtRoute.Text}\n" +
                    $"- Hạng vé: {cbCabin.SelectedText}\n" +
                    $"- Loại vé: {cbFareType.SelectedText}\n" +
                    $"- Mùa: {cbSeason.SelectedText}\n" +
                    $"- Hiệu lực: {dtEffective.Value:dd/MM/yyyy}\n" +
                    $"- Hết hạn: {dtExpiry.Value:dd/MM/yyyy}\n" +
                    $"- Giá: {txtPrice.Text}\n" +
                    $"- Mô tả: {txtDescription.Text}\n\n" +
                    "(Khi bạn cấp dữ liệu thật cho Airports/Routes/Cabin_Classes, mình sẽ bind combobox theo ID).",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
        }
    }
}
