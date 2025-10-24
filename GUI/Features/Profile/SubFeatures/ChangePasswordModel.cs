using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;

namespace GUI.Features.Profile.SubFeatures {
    public class ChangePasswordModel {
        public string CurrentPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
    }

    public class ChangePasswordControl : UserControl {
        public event Action<ChangePasswordModel>? OnChangePasswordRequested;

        private UnderlinedTextField txtCurrent, txtNew, txtConfirm;
        private Label lblTitle;
        private readonly int _accountId;

        public ChangePasswordControl(int accountId) {
            _accountId = accountId;
            InitializeComponent();
        }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label {
                Text = "🔐 Đổi mật khẩu",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            var grid = new TableLayoutPanel {
                Dock = DockStyle.Top,
                Padding = new Padding(24, 12, 24, 0),
                AutoSize = true,
                ColumnCount = 2
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            txtCurrent = new UnderlinedTextField("Mật khẩu hiện tại", "") { MinimumSize = new Size(0, 56), Width = 280, PasswordChar = '•' };
            txtNew = new UnderlinedTextField("Mật khẩu mới", "") { MinimumSize = new Size(0, 56), Width = 280, PasswordChar = '•' };
            txtConfirm = new UnderlinedTextField("Xác nhận mật khẩu mới", "") { MinimumSize = new Size(0, 56), Width = 280, PasswordChar = '•' };

            grid.Controls.Add(txtCurrent, 0, 0);
            grid.Controls.Add(new Panel(), 1, 0);
            grid.Controls.Add(txtNew, 0, 1);
            grid.Controls.Add(new Panel(), 1, 1);
            grid.Controls.Add(txtConfirm, 0, 2);
            grid.Controls.Add(new Panel(), 1, 2);

            var btnSave = new PrimaryButton("🔄 Đổi mật khẩu") { Width = 160, Height = 36 };
            btnSave.Click += (s, e) => {
                if (txtNew.Text != txtConfirm.Text) {
                    MessageBox.Show("Xác nhận mật khẩu không khớp.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                OnChangePasswordRequested?.Invoke(new ChangePasswordModel {
                    CurrentPassword = txtCurrent.Text,
                    NewPassword = txtNew.Text
                });
            };

            var btnRow = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(24, 8, 24, 0)
            };
            btnRow.Controls.Add(btnSave);

            var main = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(grid, 0, 1);
            main.Controls.Add(new Panel(), 0, 2);

            Controls.Add(main);
            Controls.Add(btnRow);
        }
    }
}
