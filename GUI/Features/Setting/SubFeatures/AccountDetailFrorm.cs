using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;

namespace GUI.Features.Setting.SubFeatures {
    internal class AccountDetailForm : Form {
        public AccountDetailForm(string email, string roles, bool isActive, int failedAttempts, DateTime createdAt) {

            Text = "Chi tiết tài khoản";
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            // Kích thước form đẹp
            Size = new Size(600, 380);

            // Panel bọc để bố cục đẹp hơn
            var container = new Panel {
                Dock = DockStyle.Fill,
                Padding = new Padding(32),
                BackColor = Color.White
            };

            // Card panel ở giữa
            var card = new Panel {
                Dock = DockStyle.Top,
                Padding = new Padding(24),
                BackColor = Color.FromArgb(245, 248, 252),
                BorderStyle = BorderStyle.FixedSingle,
                Height = 280
            };

            // Grid nội dung
            var grid = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(8),
                AutoSize = true
            };

            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            Font bold = new Font("Segoe UI", 10, FontStyle.Bold);
            Font normal = new Font("Segoe UI", 10, FontStyle.Regular);

            // Hàm helper thêm dòng
            void AddRow(string label, string value, int row, Color? valueColor = null) {
                grid.Controls.Add(new Label {
                    Text = label,
                    AutoSize = true,
                    Font = bold,
                    Margin = new Padding(0, 8, 0, 8)
                }, 0, row);

                grid.Controls.Add(new Label {
                    Text = value,
                    AutoSize = true,
                    ForeColor = valueColor ?? Color.Black,
                    Font = normal,
                    Margin = new Padding(0, 8, 0, 8)
                }, 1, row);
            }

            // Dữ liệu
            AddRow("Email:", email, 0);
            AddRow("Vai trò:", roles, 1);
            AddRow("Trạng thái:",
                   isActive ? "Đang hoạt động" : "Đã khóa",
                   2,
                   isActive ? Color.ForestGreen : Color.Firebrick);
            AddRow("Lần nhập sai còn lại:", failedAttempts.ToString(), 3);
            AddRow("Ngày tạo:", createdAt.ToString("dd/MM/yyyy HH:mm"), 4);

            card.Controls.Add(grid);

            // Nút đóng
            var btnClose = new PrimaryButton("Đóng") {
                Width = 120,
                Height = 38,
                Margin = new Padding(0, 20, 0, 0)
            };
            btnClose.Click += (_, __) => Close();

            var btnPanel = new FlowLayoutPanel {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 60
            };

            btnPanel.Controls.Add(btnClose);

            container.Controls.Add(card);
            container.Controls.Add(btnPanel);
            Controls.Add(container);
        }
    }
}
