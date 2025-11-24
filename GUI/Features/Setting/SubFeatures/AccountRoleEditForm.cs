using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Buttons;
using BUS.Auth;              // <-- thêm
using DTO.Permissions;       // <-- thêm

namespace GUI.Features.Setting.SubFeatures {
    internal class AccountRoleEditForm : Form {
        private readonly int _accountId;
        private CheckedListBox lstRoles;
        private Button btnOk, btnCancel;

        private readonly RolePermissionService _service = new RolePermissionService(); // <-- thêm
        private List<RoleItem> _allRoles = new();

        public AccountRoleEditForm(int accountId) {
            _accountId = accountId;
            InitializeComponent();
        }

        private void InitializeComponent() {
            Text = "Phân quyền (vai trò) cho tài khoản";
            Size = new Size(420, 360);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            // Lấy danh sách role từ BUS (EF + LINQ)
            _allRoles = _service.GetAllRoles();

            var grid = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                ColumnCount = 1,
                RowCount = 3
            };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            grid.Controls.Add(new Label {
                Text = "Chọn vai trò cho tài khoản:",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 8)
            }, 0, 0);

            lstRoles = new CheckedListBox {
                Dock = DockStyle.Fill,
                CheckOnClick = true,
                BorderStyle = BorderStyle.None
            };

            foreach (var r in _allRoles)
                lstRoles.Items.Add(r, false);

            lstRoles.DisplayMember = nameof(RoleItem.Name);
            grid.Controls.Add(lstRoles, 0, 1);

            btnOk = new PrimaryButton("Lưu") { Width = 100, Height = 36 };
            btnCancel = new SecondaryButton("Huỷ") { Width = 100, Height = 36 };

            var buttons = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            buttons.Controls.Add(btnOk);
            buttons.Controls.Add(btnCancel);
            grid.Controls.Add(buttons, 0, 2);

            btnOk.Click += (_, __) => SaveRoles();
            btnCancel.Click += (_, __) => DialogResult = DialogResult.Cancel;

            Controls.Add(grid);

            LoadCurrentRoles();
        }

        private void LoadCurrentRoles() {
            // lấy role hiện tại của account từ BUS
            var roleIds = _service.GetRoleIdsOfAccount(_accountId);

            for (int i = 0; i < lstRoles.Items.Count; i++) {
                if (lstRoles.Items[i] is RoleItem r)
                    lstRoles.SetItemChecked(i, roleIds.Contains(r.RoleId));
            }
        }

        private void SaveRoles() {
            var roleIds = new List<int>();

            for (int i = 0; i < lstRoles.Items.Count; i++) {
                if (!lstRoles.GetItemChecked(i)) continue;
                if (lstRoles.Items[i] is RoleItem r)
                    roleIds.Add(r.RoleId);
            }

            // lưu qua BUS, BUS gọi xuống DAO/EF
            _service.SaveRolesForAccount(_accountId, roleIds);

            MessageBox.Show("Đã lưu vai trò cho tài khoản.", "Phân quyền",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }
    }
}
