using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Tables;

namespace GUI.Features.Setting.SubFeatures {
    internal class ManageAccountRolesControl : UserControl {
        private ListBox lstAccounts;
        private CheckedListBox lstRoles;
        private TableCustom tblEffectivePerms; // thay ListView bằng TableCustom
        private Button btnSaveRoles, btnRefresh;

        private List<UserItem> _allUsers = new();
        private List<RoleItem> _allRoles = new();

        public ManageAccountRolesControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.White;

            var title = new Label {
                Text = "👤 Quản lý quyền cho Tài khoản",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ==== Left: Accounts in GroupBox (có viền) ===========================
            var grpAccounts = new GroupBox {
                Text = "Tài khoản",
                Dock = DockStyle.Left,
                Width = 360
            };

            lstAccounts = new ListBox {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None
            };
            lstAccounts.SelectedIndexChanged += (_, __) => LoadAccountDetail();
            grpAccounts.Controls.Add(lstAccounts);

            // ==== Right: Roles + Effective permissions ===========================
            var rightPanel = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(8)
            };
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Roles box
            var grpRoles = new GroupBox { Text = "Vai trò được gán", Dock = DockStyle.Fill };
            lstRoles = new CheckedListBox {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                CheckOnClick = true
            };
            grpRoles.Controls.Add(lstRoles);

            // Effective permissions (TableCustom)
            var grpPerms = new GroupBox { Text = "Quyền hiệu lực", Dock = DockStyle.Fill };
            tblEffectivePerms = new TableCustom {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            // Cột
            tblEffectivePerms.Columns.Add("permName", "Permission");
            tblEffectivePerms.Columns.Add("permCode", "Code");
            tblEffectivePerms.Columns.Add("permModule", "Module");
            grpPerms.Controls.Add(tblEffectivePerms);

            // Actions
            var actions = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 8, 0, 8),
                AutoSize = true
            };
            btnSaveRoles = new PrimaryButton("💾 Lưu thay đổi");
            btnSaveRoles.Click += (_, __) => SaveRoles();
            btnRefresh = new SecondaryButton("Làm mới");
            btnRefresh.Click += (_, __) => ReloadAll();
            actions.Controls.AddRange(new Control[] { btnSaveRoles, btnRefresh });

            rightPanel.Controls.Add(grpRoles, 0, 0);
            rightPanel.Controls.Add(grpPerms, 1, 0);
            rightPanel.SetColumnSpan(actions, 2);
            rightPanel.Controls.Add(actions, 0, 1);

            Controls.Add(rightPanel);
            Controls.Add(grpAccounts);
            Controls.Add(title);

            ReloadAll();
        }

        private void ReloadAll() {
            _allUsers = PermissionRepository.GetAllUsers();
            _allRoles = PermissionRepository.GetAllRoles();

            lstRoles.Items.Clear();
            foreach (var r in _allRoles) lstRoles.Items.Add(r, false);

            LoadAccounts();
            if (lstAccounts.Items.Count > 0) lstAccounts.SelectedIndex = 0;
        }

        private void LoadAccounts() {
            lstAccounts.BeginUpdate();
            lstAccounts.Items.Clear();
            foreach (var u in _allUsers)
                lstAccounts.Items.Add(u);
            lstAccounts.DisplayMember = nameof(UserItem.Email);
            lstAccounts.EndUpdate();
        }

        private void LoadAccountDetail() {
            // clear bảng hiệu lực
            tblEffectivePerms.Rows.Clear();
            // clear tick roles
            for (int i = 0; i < lstRoles.Items.Count; i++) lstRoles.SetItemChecked(i, false);

            if (lstAccounts.SelectedItem is not UserItem u) return;

            // Tick roles của account
            var roleIds = PermissionRepository.GetRoleIdsOfAccount(u.AccountId);
            for (int i = 0; i < lstRoles.Items.Count; i++) {
                if (lstRoles.Items[i] is RoleItem r)
                    lstRoles.SetItemChecked(i, roleIds.Contains(r.RoleId));
            }

            // Nạp quyền hiệu lực (union quyền từ các vai trò)
            var eff = PermissionRepository.GetEffectivePermissionIdsOfAccount(u.AccountId);
            var all = PermissionRepository.GetAllPermissions().ToDictionary(p => p.PermissionId);
            foreach (var pid in eff.OrderBy(x => x)) {
                if (!all.TryGetValue(pid, out var p)) continue;
                tblEffectivePerms.Rows.Add(p.DisplayName, p.Code, p.Group);
            }
        }

        private void SaveRoles() {
            if (lstAccounts.SelectedItem is not UserItem u) return;

            var roleIds = new List<int>();
            for (int i = 0; i < lstRoles.Items.Count; i++) {
                if (!lstRoles.GetItemChecked(i)) continue;
                if (lstRoles.Items[i] is RoleItem r) roleIds.Add(r.RoleId);
            }

            PermissionRepository.SaveRolesForAccount(u.AccountId, roleIds);
            MessageBox.Show("Đã lưu vai trò cho tài khoản.", "Quản lý quyền", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // reload effective perms
            LoadAccountDetail();
        }
    }
}
