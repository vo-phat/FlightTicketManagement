using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;

namespace GUI.Features.Setting.SubFeatures {
    internal class AddAccountForm : Form {
        private readonly UnderlinedTextField txtEmail = new("Email", "") { Dock = DockStyle.Fill, Margin = new Padding(0, 6, 0, 6) };
        private readonly UnderlinedTextField txtFullName = new("Họ tên", "") { Dock = DockStyle.Fill, Margin = new Padding(0, 6, 0, 6) };
        private readonly UnderlinedTextField txtPassword = new("Mật khẩu", "") { Dock = DockStyle.Fill, Margin = new Padding(0, 6, 0, 6) };

        private readonly ComboBox cboRole = new() { Dock = DockStyle.Fill, Margin = new Padding(0, 6, 0, 6) };

        private readonly Button btnOk = new PrimaryButton("Thêm") { Width = 100, Height = 36 };
        private readonly Button btnCancel = new SecondaryButton("Huỷ") { Width = 100, Height = 36 };

        public string Email => txtEmail.Text;
        public string FullName => txtFullName.Text;
        public string Password => txtPassword.Text;
        public int RoleId => (cboRole.SelectedItem is RoleItem r) ? r.RoleId : 0;

        public AddAccountForm() {
            Text = "Thêm tài khoản";
            Size = new Size(520, 360);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            var grid = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                ColumnCount = 2,
                RowCount = 5
            };

            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            grid.Controls.Add(new Label { Text = "Email", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 0);
            grid.Controls.Add(txtEmail, 1, 0);

            grid.Controls.Add(new Label { Text = "Họ tên", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 1);
            grid.Controls.Add(txtFullName, 1, 1);

            grid.Controls.Add(new Label { Text = "Mật khẩu", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 2);
            grid.Controls.Add(txtPassword, 1, 2);

            grid.Controls.Add(new Label { Text = "Vai trò", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 3);
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.Items.AddRange(PermissionRepository.GetAllRoles().ToArray());
            cboRole.DisplayMember = nameof(RoleItem.Name);
            if (cboRole.Items.Count > 0) cboRole.SelectedIndex = 0;
            grid.Controls.Add(cboRole, 1, 3);

            var buttons = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            buttons.Controls.Add(btnOk);
            buttons.Controls.Add(btnCancel);
            grid.Controls.Add(buttons, 0, 4);
            grid.SetColumnSpan(buttons, 2);

            btnOk.Click += (_, __) => {
                if (string.IsNullOrWhiteSpace(Email)) {
                    MessageBox.Show("Email không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult = DialogResult.OK;
            };
            btnCancel.Click += (_, __) => DialogResult = DialogResult.Cancel;

            Controls.Add(grid);
        }
    }
}
