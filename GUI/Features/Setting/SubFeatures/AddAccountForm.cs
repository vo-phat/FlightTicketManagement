using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using BUS.Auth;
using DTO.Permissions;

namespace GUI.Features.Setting.SubFeatures {
    internal class AddAccountForm : Form {
        private readonly UnderlinedTextField txtEmail = new("Email", "") {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 6, 0, 6)
        };

        private readonly UnderlinedTextField txtPassword = new("Mật khẩu", "") {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 6, 0, 6)
        };

        private readonly ComboBox cboRole = new() {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 6, 0, 6),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        private readonly Button btnOk = new PrimaryButton("Thêm") {
            Width = 120,
            Height = 38
        };

        private readonly Button btnCancel = new SecondaryButton("Huỷ") {
            Width = 120,
            Height = 38
        };

        private readonly RolePermissionService _service = new RolePermissionService();

        // ===== Properties để màn khác lấy giá trị =====
        public string Email => txtEmail.Text.Trim();
        public string Password => txtPassword.Text.Trim();
        public int RoleId => (cboRole.SelectedItem is RoleItem r) ? r.RoleId : 0;

        public AddAccountForm() {
            Text = "Thêm tài khoản";
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(600, 650);

            // ===== Root layout ===================================================
            var root = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                ColumnCount = 1,
                RowCount = 3
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));          // title
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));      // card
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));          // buttons

            // ===== Title =========================================================
            var lblTitle = new Label {
                Text = "Thêm tài khoản mới",
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 12)
            };
            root.Controls.Add(lblTitle, 0, 0);

            // ===== Card thông tin ===============================================
            var card = new Panel {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                BackColor = Color.FromArgb(245, 248, 252),
                BorderStyle = BorderStyle.FixedSingle
            };

            var grid = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(8)
            };

            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            Font bold = new Font("Segoe UI", 10, FontStyle.Bold);

            // Email
            grid.Controls.Add(new Label {
                Text = "Email",
                AutoSize = true,
                Font = bold,
                Margin = new Padding(0, 6, 0, 6)
            }, 0, 0);
            grid.Controls.Add(txtEmail, 1, 0);

            // Mật khẩu
            grid.Controls.Add(new Label {
                Text = "Mật khẩu",
                AutoSize = true,
                Font = bold,
                Margin = new Padding(0, 6, 0, 6)
            }, 0, 1);
            txtPassword.PasswordChar = '•';
            grid.Controls.Add(txtPassword, 1, 1);

            // Vai trò
            grid.Controls.Add(new Label {
                Text = "Vai trò",
                AutoSize = true,
                Font = bold,
                Margin = new Padding(0, 6, 0, 6)
            }, 0, 2);

            var roles = _service.GetAllRoles();
            cboRole.Items.AddRange(roles.ToArray());
            cboRole.DisplayMember = nameof(RoleItem.Name);
            if (cboRole.Items.Count > 0) cboRole.SelectedIndex = 0;

            grid.Controls.Add(cboRole, 1, 2);

            card.Controls.Add(grid);
            root.Controls.Add(card, 0, 1);

            // ===== Buttons =======================================================
            var buttons = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 16, 0, 0),
                AutoSize = true
            };
            buttons.Controls.Add(btnOk);
            buttons.Controls.Add(btnCancel);

            root.Controls.Add(buttons, 0, 2);

            // ===== Event nút =====================================================
            btnOk.Click += (_, __) => {
                if (string.IsNullOrWhiteSpace(Email)) {
                    MessageBox.Show("Email không được để trống.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(Password)) {
                    MessageBox.Show("Mật khẩu không được để trống.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (RoleId <= 0) {
                    MessageBox.Show("Vui lòng chọn vai trò.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult = DialogResult.OK;
            };

            btnCancel.Click += (_, __) => DialogResult = DialogResult.Cancel;

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Controls.Add(root);
        }
    }
}
