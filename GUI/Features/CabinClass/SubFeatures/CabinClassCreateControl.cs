using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using GUI.Components.Inputs;

namespace GUI.Features.CabinClass.SubFeatures {
    public class CabinClassCreateControl : UserControl {
        // để dễ reuse/SetInitialData sau này
        private UnderlinedTextField _txtCode, _txtName, _txtPriority, _txtBaggage, _txtPitch, _txtDesc;
        private UnderlinedComboBox _cbTier;

        public CabinClassCreateControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label { Text = "➕ Tạo Cabin Class", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
            titlePanel.Controls.Add(lblTitle);

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

            _txtCode = new UnderlinedTextField("Mã cabin (duy nhất)", "") { MinimumSize = new Size(0, 56), Width = 220, Margin = new Padding(0, 6, 24, 6) };
            _txtName = new UnderlinedTextField("Tên cabin", "") { MinimumSize = new Size(0, 56), Width = 300, Margin = new Padding(0, 6, 24, 6) };
            _cbTier = new UnderlinedComboBox("Thứ hạng", new object[] { "Economy", "Premium Economy", "Business", "First" }) { MinimumSize = new Size(0, 56), Width = 240, Margin = new Padding(0, 6, 24, 6) };
            _txtPriority = new UnderlinedTextField("Độ ưu tiên (số nhỏ ưu tiên cao)", "0") { MinimumSize = new Size(0, 56), Width = 260, Margin = new Padding(0, 6, 24, 6) };
            _txtBaggage = new UnderlinedTextField("Hành lý mặc định (kg)", "20") { MinimumSize = new Size(0, 56), Width = 220, Margin = new Padding(0, 6, 24, 6) };
            _txtPitch = new UnderlinedTextField("Seat pitch (inch)", "31") { MinimumSize = new Size(0, 56), Width = 220, Margin = new Padding(0, 6, 24, 6) };

            // hàng mô tả toàn dòng
            _txtDesc = new UnderlinedTextField("Mô tả", "") { MinimumSize = new Size(0, 56), Width = 680, Margin = new Padding(0, 6, 24, 6) };

            inputs.Controls.Add(_txtCode, 0, 0);
            inputs.Controls.Add(_txtName, 1, 0);
            inputs.Controls.Add(_cbTier, 0, 1);
            inputs.Controls.Add(_txtPriority, 1, 1);
            inputs.Controls.Add(_txtBaggage, 0, 2);
            inputs.Controls.Add(_txtPitch, 1, 2);

            // thêm hàng mô tả
            inputs.RowCount = 4; inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            inputs.SetColumnSpan(_txtDesc, 2);
            inputs.Controls.Add(_txtDesc, 0, 3);

            // ✅ fix chiều cao từng hàng (không cắt underline)
            for (int r = 0; r < inputs.RowCount; r++) {
                int h = 0;
                for (int c = 0; c < inputs.ColumnCount; c++) {
                    var ctl = inputs.GetControlFromPosition(c, r);
                    if (ctl != null) h = Math.Max(h, ctl.GetPreferredSize(Size.Empty).Height + ctl.Margin.Vertical);
                }
                inputs.RowStyles[r] = new RowStyle(SizeType.Absolute, System.Math.Max(72, h + 2));
            }

            // chặn ký tự không phải số
            void OnlyDigits(UnderlinedTextField t) {
                t.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };
            }
            OnlyDigits(_txtPriority); OnlyDigits(_txtBaggage); OnlyDigits(_txtPitch);

            // Buttons
            var btnSave = new PrimaryButton("💾 Lưu Cabin") { Width = 140, Height = 40, Margin = new Padding(0, 12, 0, 12) };
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
            table.Columns.Add("cabinCode", "Mã");
            table.Columns.Add("cabinName", "Tên cabin");
            table.Columns.Add("tier", "Thứ hạng");
            table.Columns.Add("priority", "Ưu tiên");
            table.Columns.Add("baggage", "Hành lý (kg)");
            table.Columns.Add("pitch", "Pitch (in)");
            for (int i = 0; i < 3; i++) table.Rows.Add("", "", "", "", "", "");

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

            // demo save
            btnSave.Click += (_, __) => {
                table.Rows.Insert(0,
                //_txtCode.Value, _txtName.Value, _cbTier.SelectedText,
                //_txtPriority.Value, _txtBaggage.Value, _txtPitch.Value);
                MessageBox.Show("Đã lưu Cabin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information));
            };
        }

        // (tuỳ chọn) cho phép nạp dữ liệu khi sửa
        public void SetInitialData(string code, string name, string tier, string priority, string baggage, string pitch, string desc) {
            //_txtCode.Value = code; _txtName.Value = name; _cbTier.SelectedText = tier;
            //_txtPriority.Value = priority; _txtBaggage.Value = baggage; _txtPitch.Value = pitch; _txtDesc.Value = desc;
        }
    }
}
