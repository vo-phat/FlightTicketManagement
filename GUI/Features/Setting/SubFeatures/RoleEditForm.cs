using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using DTO.Permissions;

namespace GUI.Features.Setting.SubFeatures {
    internal class RoleEditForm : Form {
        private UnderlinedTextField txtCode;
        private UnderlinedTextField txtName;
        private Button btnOk, btnCancel;

        public string RoleCode => txtCode.Text.Trim();
        public string RoleName => txtName.Text.Trim();

        public RoleEditForm(RoleItem? role = null) {
            InitializeComponent(role);
        }

        private void InitializeComponent(RoleItem? role) {
            Text = role == null ? "Thêm vai trò" : "Sửa vai trò";
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(600, 600);

            // ===== Root container =================================================
            var root = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                ColumnCount = 1,
                RowCount = 3
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));         // title
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));     // card
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));         // buttons

            // ===== Title =========================================================
            var lblTitle = new Label {
                Text = role == null ? "Thêm vai trò mới" : "Chỉnh sửa vai trò",
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 12)
            };
            root.Controls.Add(lblTitle, 0, 0);

            // ===== Card chứa thông tin vai trò ===================================
            var card = new Panel {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                BackColor = Color.FromArgb(245, 248, 252),
                BorderStyle = BorderStyle.FixedSingle
            };

            var cardLayout = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            cardLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // hint
            cardLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // code
            cardLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // name

            var lblHint = new Label {
                Text = "Nhập thông tin vai trò hệ thống:",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 8)
            };
            cardLayout.Controls.Add(lblHint, 0, 0);

            // --- Mã vai trò ------------------------------------------------------
            txtCode = new UnderlinedTextField("Mã vai trò (ví dụ: ADMIN, STAFF, USER)", "") {
                Dock = DockStyle.Top,
                Margin = new Padding(0, 0, 0, 8)
            };
            cardLayout.Controls.Add(txtCode, 0, 1);

            // --- Tên vai trò -----------------------------------------------------
            txtName = new UnderlinedTextField("Tên hiển thị (ví dụ: Quản trị viên, Nhân viên)", "") {
                Dock = DockStyle.Top,
                Margin = new Padding(0, 0, 0, 0)
            };
            cardLayout.Controls.Add(txtName, 0, 2);

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

            Controls.Add(root);

            // ===== Bind dữ liệu khi sửa ==========================================
            if (role != null) {
                txtCode.Text = role.Code;
                txtName.Text = role.Name;
            }

            // ===== Events ========================================================
            btnOk.Click += (_, __) => OnSave();
            btnCancel.Click += (_, __) => DialogResult = DialogResult.Cancel;

            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private void OnSave() {
            var code = RoleCode;
            var name = RoleName;

            if (string.IsNullOrWhiteSpace(code)) {
                MessageBox.Show("Mã vai trò không được để trống.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(name)) {
                MessageBox.Show("Tên vai trò không được để trống.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
