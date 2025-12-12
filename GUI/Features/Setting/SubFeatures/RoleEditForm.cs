using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using DTO.Permissions;
using BUS.Auth;

namespace GUI.Features.Setting.SubFeatures {
    internal class RoleEditForm : Form {
        private UnderlinedTextField txtCode;
        private UnderlinedTextField txtName;
        private Button btnOk, btnCancel;

        // Nếu form tạo role mới, service trả về id mới -> lưu ở đây để caller dùng
        public int? CreatedRoleId { get; private set; }

        // Expose values for caller if needed
        public string RoleCode => txtCode.Text.Trim();
        public string RoleName => txtName.Text.Trim();

        // internal state
        private readonly RoleItem? _editingRole;
        private readonly RolePermissionService _service = new();

        public RoleEditForm(RoleItem? role = null) {
            _editingRole = role;
            InitializeComponent(role);
        }

        private void InitializeComponent(RoleItem? role) {
            Text = role == null ? "Thêm vai trò" : "Sửa vai trò";
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(600, 600); // nhỏ lại hợp lý

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
                // Không cho sửa mã role khi edit (an toàn)
                txtCode.Enabled = false;
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

            try {
                if (_editingRole == null) {
                    // Create new role via service
                    var req = new DTO.Permissions.CreateRoleRequest(code, name);
                    int newId = _service.CreateRole(req);
                    // nếu không exception => success
                    CreatedRoleId = newId;
                    DialogResult = DialogResult.OK;
                } else {
                    // Update existing role (only name allowed)
                    var req = new DTO.Permissions.UpdateRoleRequest(_editingRole.RoleId, name);
                    bool ok = _service.UpdateRole(req);
                    if (!ok) {
                        MessageBox.Show("Cập nhật vai trò thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    DialogResult = DialogResult.OK;
                }
            } catch (Exception ex) {
                // Show meaningful message and keep form open for correction
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
