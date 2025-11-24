using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;

namespace GUI.Features.Setting.SubFeatures {
    internal class AccountDetailForm : Form {
        public AccountDetailForm(string email, string roles, bool isActive, int failedAttempts, DateTime createdAt) {
            Text = $"Chi tiết tài khoản {email}";
            Size = new Size(520, 340);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            var grid = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                ColumnCount = 2,
                RowCount = 6
            };

            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Email
            grid.Controls.Add(new Label {
                Text = "Email:",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            }, 0, 0);
            grid.Controls.Add(new Label {
                Text = email,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            }, 1, 0);

            // Vai trò
            grid.Controls.Add(new Label {
                Text = "Vai trò:",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            }, 0, 2);
            grid.Controls.Add(new Label {
                Text = roles,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            }, 1, 2);

            // Trạng thái
            var statusText = isActive ? "Đang hoạt động" : "Đã khóa";
            grid.Controls.Add(new Label {
                Text = "Trạng thái:",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            }, 0, 3);
            grid.Controls.Add(new Label {
                Text = statusText,
                AutoSize = true,
                ForeColor = isActive ? Color.ForestGreen : Color.Firebrick,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            }, 1, 3);

            // Số lần nhập sai còn lại
            grid.Controls.Add(new Label {
                Text = "Lần nhập sai còn lại:",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            }, 0, 4);
            grid.Controls.Add(new Label {
                Text = failedAttempts.ToString(),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            }, 1, 4);

            // Ngày tạo
            grid.Controls.Add(new Label {
                Text = "Ngày tạo:",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            }, 0, 5);
            grid.Controls.Add(new Label {
                Text = createdAt.ToString("dd/MM/yyyy HH:mm"),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            }, 1, 5);

            // Nút Đóng
            var btnClose = new PrimaryButton("Đóng") { Width = 100, Height = 36 };
            var buttons = new FlowLayoutPanel {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 16, 0, 0),
                Height = 52
            };
            buttons.Controls.Add(btnClose);

            var outer = new Panel { Dock = DockStyle.Fill };
            outer.Controls.Add(grid);
            outer.Controls.Add(buttons);

            btnClose.Click += (_, __) => Close();

            Controls.Add(outer);
        }
    }
}
