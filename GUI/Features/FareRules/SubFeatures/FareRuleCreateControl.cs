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
            var lblTitle = new Label { Text = "➕ Tạo Fare Rule", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
            titlePanel.Controls.Add(lblTitle);

            // Inputs (3 hàng x 2 cột)
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

            var txtCode = new UnderlinedTextField("Mã rule (duy nhất)", "") { MinimumSize = new Size(0, 56), Width = 240, Margin = new Padding(0, 6, 24, 6) };
            var txtName = new UnderlinedTextField("Tên rule", "") { MinimumSize = new Size(0, 56), Width = 320, Margin = new Padding(0, 6, 24, 6) };
            var cbCabin = new UnderlinedComboBox("Hạng vé", new object[] { "Economy", "Premium Economy", "Business", "First" }) { MinimumSize = new Size(0, 56), Width = 240, Margin = new Padding(0, 6, 24, 6) };
            var cbRefund = new UnderlinedComboBox("Hoàn vé", new object[] { "Có thể hoàn", "Không hoàn" }) { MinimumSize = new Size(0, 56), Width = 200, Margin = new Padding(0, 6, 24, 6) };

            var txtChangeFee = new UnderlinedTextField("Phí đổi (₫)", "") { MinimumSize = new Size(0, 56), Width = 200, Margin = new Padding(0, 6, 24, 6) };
            var txtBaggage = new UnderlinedTextField("Hành lý (kg)", "") { MinimumSize = new Size(0, 56), Width = 200, Margin = new Padding(0, 6, 24, 6) };

            var txtMinStay = new UnderlinedTextField("Lưu trú tối thiểu (đêm)", "") { MinimumSize = new Size(0, 56), Width = 240, Margin = new Padding(0, 6, 24, 6) };
            var txtMaxStay = new UnderlinedTextField("Lưu trú tối đa (đêm)", "") { MinimumSize = new Size(0, 56), Width = 240, Margin = new Padding(0, 6, 24, 6) };
            var txtAdvance = new UnderlinedTextField("Mua trước tối thiểu (ngày)", "") { MinimumSize = new Size(0, 56), Width = 260, Margin = new Padding(0, 6, 24, 6) };

            inputs.Controls.Add(txtCode, 0, 0);
            inputs.Controls.Add(txtName, 1, 0);
            inputs.Controls.Add(cbCabin, 0, 1);
            inputs.Controls.Add(cbRefund, 1, 1);
            inputs.Controls.Add(txtChangeFee, 0, 2);
            inputs.Controls.Add(txtBaggage, 1, 2);
            inputs.Controls.Add(txtMinStay, 0, 3);
            inputs.Controls.Add(txtMaxStay, 1, 3);
            // có thể thêm txtAdvance vào hàng mới nếu muốn
            // inputs.RowCount = 5; inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); inputs.Controls.Add(txtAdvance, 0, 4);

            // ✅ fix chiều cao từng hàng theo PreferredSize để không cắt underline
            for (int r = 0; r < inputs.RowCount; r++) {
                int h = 0;
                for (int c = 0; c < inputs.ColumnCount; c++) {
                    var ctl = inputs.GetControlFromPosition(c, r);
                    if (ctl != null) h = Math.Max(h, ctl.GetPreferredSize(Size.Empty).Height + ctl.Margin.Vertical);
                }
                inputs.RowStyles[r] = new RowStyle(SizeType.Absolute, Math.Max(72, h + 2));
            }

            // Chặn ký tự không phải số cho các field số
            void OnlyDigits(UnderlinedTextField t) {
                t.KeyPress += (s, e) => {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
                };
            }
            OnlyDigits(txtChangeFee); OnlyDigits(txtBaggage); OnlyDigits(txtMinStay); OnlyDigits(txtMaxStay); OnlyDigits(txtAdvance);

            // Buttons
            var btnSave = new PrimaryButton("💾 Lưu rule") { Width = 140, Height = 40, Margin = new Padding(0, 12, 0, 12) };
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
            table.Columns.Add("ruleCode", "Mã");
            table.Columns.Add("ruleName", "Tên rule");
            table.Columns.Add("cabinClass", "Hạng vé");
            table.Columns.Add("refundable", "Hoàn vé");
            table.Columns.Add("changeFee", "Phí đổi (₫)");
            table.Columns.Add("baggage", "Hành lý (kg)");
            for (int i = 0; i < 3; i++) table.Rows.Add("", "", "", "", "", "");

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

            // Demo lưu
            btnSave.Click += (_, __) => {
                var refundable = cbRefund.SelectedText.StartsWith("Có") ? "Có" : "Không";
                MessageBox.Show(
                    $"Đã lưu Fare Rule:\n",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
        }
    }
}
