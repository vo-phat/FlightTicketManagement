using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;

namespace GUI.Features.Setting.SubFeatures {
    internal class AccountDetailForm : Form {
        public AccountDetailForm(string email, string fullName, string roles) {
            Text = $"Chi tiết tài khoản {email}";
            Size = new Size(520, 260);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            var grid = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                ColumnCount = 2,
                RowCount = 4
            };

            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            grid.Controls.Add(new Label { Text = "Email:", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 0);
            grid.Controls.Add(new Label { Text = email, AutoSize = true }, 1, 0);

            grid.Controls.Add(new Label { Text = "Họ tên:", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 1);
            grid.Controls.Add(new Label { Text = fullName, AutoSize = true }, 1, 1);

            grid.Controls.Add(new Label { Text = "Vai trò:", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 2);
            grid.Controls.Add(new Label { Text = roles, AutoSize = true }, 1, 2);

            var btnClose = new PrimaryButton("Đóng") { Width = 100, Height = 36 };
            var buttons = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            buttons.Controls.Add(btnClose);
            grid.Controls.Add(buttons, 0, 3);
            grid.SetColumnSpan(buttons, 2);

            btnClose.Click += (_, __) => Close();
            Controls.Add(grid);
        }
    }
}
