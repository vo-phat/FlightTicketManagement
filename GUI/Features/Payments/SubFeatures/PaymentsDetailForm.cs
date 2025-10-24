using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Payments.SubFeatures {
    internal class PaymentDetailForm : Form {
        public PaymentDetailForm(DataGridViewRow srcRow) {
            Text = $"Chi tiết thanh toán #{srcRow.Cells["paymentId"].Value}";
            Size = new Size(700, 420);
            BackColor = Color.White;

            var title = new Label {
                Text = "🧾 Thông tin thanh toán",
                AutoSize = true,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            var card = new Panel {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(16),
                Margin = new Padding(24, 8, 24, 24),
                Dock = DockStyle.Fill
            };

            var grid = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            void Row(string key, string? val) {
                grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                grid.Controls.Add(new Label { Text = key, AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), Margin = new Padding(0, 6, 12, 6) }, 0, grid.RowCount - 1);
                grid.Controls.Add(new Label { Text = val ?? "", AutoSize = true, Font = new Font("Segoe UI", 10), Margin = new Padding(0, 6, 0, 6) }, 1, grid.RowCount - 1);
            }

            Row("Mã thanh toán:", Convert.ToString(srcRow.Cells["paymentId"].Value));
            Row("Booking ID:", Convert.ToString(srcRow.Cells["bookingId"].Value));
            Row("Số tiền:", Convert.ToString(srcRow.Cells["amount"].Value));

            if (srcRow.DataGridView?.Columns.Contains("txnRef") == true) {
                Row("Mã tham chiếu:", Convert.ToString(srcRow.Cells["txnRef"].Value));
            }

            Row("Phương thức:", Convert.ToString(srcRow.Cells["method"].Value));
            Row("Ngày thanh toán:", Convert.ToString(srcRow.Cells["paymentDate"].Value));
            Row("Trạng thái:", Convert.ToString(srcRow.Cells["status"].Value));


            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 12, 12, 12) };
            var btnClose = new Button { Text = "Đóng", AutoSize = true };
            btnClose.Click += (_, __) => Close();
            bottom.Controls.Add(btnClose);

            card.Controls.Add(bottom);
            card.Controls.Add(grid);

            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(title, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Add(main);
        }
    }
}
