using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using BUS.Auth;
using DTO.Permissions;

namespace GUI.Features.Setting.SubFeatures {
    internal class AccountRoleEditForm : Form {
        private readonly int _accountId;
        private CheckedListBox lstRoles;
        private UnderlinedTextField txtPassword;
        private CheckBox chkUnlock;
        private Button btnOk, btnCancel;

        private readonly RolePermissionService _service = new RolePermissionService();
        private List<RoleItem> _allRoles = new();

        public AccountRoleEditForm(int accountId) {
            _accountId = accountId;
            InitializeComponent();
        }

        private void InitializeComponent() {
            Text = "Phân quyền cho tài khoản";
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(640, 500);

            // Lấy danh sách role từ BUS (EF + LINQ)
            _allRoles = _service.GetAllRoles();

            // ===== Root container =================================================
            var root = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                ColumnCount = 1,
                RowCount = 3
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));            // title
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));        // card
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));            // buttons

            // ===== Title =========================================================
            var lblTitle = new Label {
                Text = "Phân quyền cho tài khoản",
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 12)
            };
            root.Controls.Add(lblTitle, 0, 0);

            // ===== Card chứa danh sách role + mật khẩu + checkbox unlock ========
            var card = new Panel {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                BackColor = Color.FromArgb(245, 248, 252),
                BorderStyle = BorderStyle.FixedSingle
            };

            var cardLayout = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };
            cardLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // hint
            cardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));    // roles
            cardLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // password
            cardLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // unlock

            var lblHint = new Label {
                Text = "Chọn một hoặc nhiều vai trò áp dụng cho tài khoản:",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 8)
            };
            cardLayout.Controls.Add(lblHint, 0, 0);

            // --- Roles list ------------------------------------------------------
            lstRoles = new CheckedListBox {
                Dock = DockStyle.Fill,
                CheckOnClick = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                BackColor = Color.FromArgb(245, 248, 252)
            };

            foreach (var r in _allRoles)
                lstRoles.Items.Add(r, false);

            lstRoles.DisplayMember = nameof(RoleItem.Name);
            cardLayout.Controls.Add(lstRoles, 0, 1);

            // --- Khu vực mật khẩu mới ------------------------------------------
            var pwdLayout = new TableLayoutPanel {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(0, 12, 0, 0)
            };
            pwdLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            pwdLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var lblPwd = new Label {
                Text = "Mật khẩu mới (tùy chọn):",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Margin = new Padding(0, 6, 8, 0)
            };

            txtPassword = new UnderlinedTextField("Mật khẩu mới (bỏ trống nếu không đổi)", "") {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 0)
            };
            txtPassword.PasswordChar = '•';

            pwdLayout.Controls.Add(lblPwd, 0, 0);
            pwdLayout.Controls.Add(txtPassword, 1, 0);

            cardLayout.Controls.Add(pwdLayout, 0, 2);

            // --- Checkbox mở khóa -----------------------------------------------
            chkUnlock = new CheckBox {
                Text = "Mở khóa tài khoản (nếu đang bị khóa)",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Margin = new Padding(0, 12, 0, 0)
            };
            // bạn có thể set mặc định là true nếu muốn thường xuyên mở khóa khi chỉnh
            chkUnlock.Checked = true;

            cardLayout.Controls.Add(chkUnlock, 0, 3);

            card.Controls.Add(cardLayout);
            root.Controls.Add(card, 0, 1);

            // ===== Buttons =======================================================
            btnOk = new PrimaryButton("Lưu") { Width = 120, Height = 38 };
            btnCancel = new SecondaryButton("Huỷ") { Width = 120, Height = 38 };

            var buttons = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 16, 0, 0),
                AutoSize = true
            };
            buttons.Controls.Add(btnOk);
            buttons.Controls.Add(btnCancel);

            root.Controls.Add(buttons, 0, 2);

            // ===== Events ========================================================
            btnOk.Click += (_, __) => SaveRolesAndSecurity();
            btnCancel.Click += (_, __) => DialogResult = DialogResult.Cancel;

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Controls.Add(root);

            // Load role hiện tại sau khi UI sẵn sàng
            LoadCurrentRoles();
        }

        private void LoadCurrentRoles() {
            var roleIds = _service.GetRoleIdsOfAccount(_accountId);

            for (int i = 0; i < lstRoles.Items.Count; i++) {
                if (lstRoles.Items[i] is RoleItem r)
                    lstRoles.SetItemChecked(i, roleIds.Contains(r.RoleId));
            }
        }

        private void SaveRolesAndSecurity() {
            var roleIds = new List<int>();

            for (int i = 0; i < lstRoles.Items.Count; i++) {
                if (!lstRoles.GetItemChecked(i)) continue;
                if (lstRoles.Items[i] is RoleItem r)
                    roleIds.Add(r.RoleId);
            }

            try {
                // 1. Lưu roles
                _service.SaveRolesForAccount(_accountId, roleIds);

                // 2. Nếu có nhập mật khẩu mới -> đổi mật khẩu
                var newPassword = txtPassword.Text.Trim();
                if (!string.IsNullOrEmpty(newPassword)) {
                    _service.UpdatePasswordForAccount(_accountId, newPassword);
                }

                // 3. Reset failed_attempts
                _service.ResetFailedAttempts(_accountId);

                // 4. Nếu admin tick "Mở khóa tài khoản" -> Unlock
                if (chkUnlock.Checked) {
                    _service.UnlockAccount(_accountId);
                }

                MessageBox.Show(
                    "Đã lưu vai trò và thông tin bảo mật cho tài khoản.",
                    "Phân quyền",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
            } catch (Exception ex) {
                MessageBox.Show(
                    "Không thể lưu thay đổi: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
