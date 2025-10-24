using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Tables;

namespace FlightTicketManagement.GUI.Features.Settings.SubFeatures {
    internal class AssignByRoleControl : UserControl {
        // Lọc module
        private UnderlinedComboBox cboModule;

        // Ma trận
        private TableCustom table;               // dùng TableCustom thay cho DataGridView
        private FlowLayoutPanel selectAllRow;

        // Hành động
        private FlowLayoutPanel actions;
        private Button btnSave, btnClearAll;

        // Data
        private List<PermissionItem> _allPerms = new();
        private List<RoleItem> _roles = new();
        private Dictionary<int, HashSet<int>> _roleToPermIds = new(); // roleId -> set(permissionId)
        private bool _suspendReload = false;

        public AssignByRoleControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.White;

            var title = new Label {
                Text = "🧩 Phân quyền theo Vai trò",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Filters (Module only) =========================================
            var topRow = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 8, 24, 0),
                WrapContents = false
            };

            // Khai báo giống mẫu _cbTimezone (label + items + MinimumSize + Width + Margin)
            cboModule = new UnderlinedComboBox("Module", new object[] { "(Tất cả)" }) {
                MinimumSize = new Size(0, 56),
                Width = 200,
                Margin = new Padding(0, 6, 24, 6)
            };
            cboModule.SelectedIndexChanged += (_, __) => { if (!_suspendReload) ReloadTable(); };

            topRow.Controls.Add(cboModule);

            // ===== Select-all per column ========================================
            selectAllRow = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 6, 24, 6),
                WrapContents = true
            };

            // ===== Table (TableCustom) ==========================================
            table = new TableCustom {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 4),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,               // cần tương tác checkbox
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            table.CurrentCellDirtyStateChanged += (s, e) => {
                if (table.IsCurrentCellDirty) table.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            table.CellValueChanged += Table_CellValueChanged;

            // ===== Footer actions ===============================================
            actions = new FlowLayoutPanel {
                Dock = DockStyle.Bottom,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(24, 8, 24, 8)
            };
            btnSave = new PrimaryButton("💾 Lưu");
            btnClearAll = new SecondaryButton("Bỏ chọn tất cả");

            btnSave.Click += (_, __) => SaveAllRoles();
            btnClearAll.Click += (_, __) => ClearAll();

            actions.Controls.AddRange(new Control[] { btnSave, btnClearAll });

            // ===== Layout tổng ===================================================
            var main = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                BackColor = Color.Transparent
            };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            main.Controls.Add(title, 0, 0);
            main.Controls.Add(topRow, 0, 1);
            main.Controls.Add(selectAllRow, 0, 2);
            main.Controls.Add(table, 0, 3);
            main.Controls.Add(actions, 0, 4);

            Controls.Add(main);

            LoadData();
        }

        private void LoadData() {
            _suspendReload = true;

            _allPerms = PermissionRepository.GetAllPermissions();
            _roles = PermissionRepository.GetAllRoles();

            // Rebuild module list
            var groups = _allPerms.Select(p => p.Group ?? "Misc").Distinct().OrderBy(x => x).ToList();
            cboModule.Items.Clear();
            cboModule.Items.Add("(Tất cả)");
            foreach (var g in groups) cboModule.Items.Add(g);
            cboModule.SelectedIndex = 0;

            // Load mapping role -> permissions
            _roleToPermIds.Clear();
            foreach (var r in _roles)
                _roleToPermIds[r.RoleId] = PermissionRepository.GetPermissionIdsOfRole(r.RoleId);

            // Build UI matrix
            BuildTableColumns();
            BuildSelectAllRow();

            _suspendReload = false;
            ReloadTable();
        }

        private void BuildTableColumns() {
            table.Rows.Clear();
            table.Columns.Clear();

            // Cột mô tả permission
            var colPerm = new DataGridViewTextBoxColumn {
                Name = "Permission",
                HeaderText = "Permission (module.action)",
                ReadOnly = true,
                FillWeight = 320
            };
            table.Columns.Add(colPerm);

            // Cột checkbox cho từng role
            foreach (var role in _roles) {
                var col = new DataGridViewCheckBoxColumn {
                    Name = $"Role_{role.RoleId}",
                    HeaderText = role.Name,
                    ThreeState = false,
                    TrueValue = true,
                    FalseValue = false,
                    FillWeight = 80
                };
                table.Columns.Add(col);
            }
        }

        private void BuildSelectAllRow() {
            selectAllRow.Controls.Clear();
            selectAllRow.Controls.Add(new Label {
                Text = "Chọn tất cả theo cột:",
                AutoSize = true,
                Margin = new Padding(0, 6, 8, 0)
            });

            foreach (var role in _roles) {
                var cb = new CheckBox {
                    Text = role.Name,
                    AutoSize = true,
                    Tag = role.RoleId,
                    Margin = new Padding(8, 4, 0, 0)
                };
                cb.CheckedChanged += (s, e) => {
                    var rid = (int)((CheckBox)s).Tag;
                    SetColumnAll(rid, cb.Checked);
                };
                selectAllRow.Controls.Add(cb);
            }
        }

        private void ReloadTable() {
            if (table.Columns.Count == 0) BuildTableColumns();

            var moduleFilter = cboModule.SelectedItem?.ToString();

            var view = _allPerms
                .Where(p =>
                    moduleFilter == "(Tất cả)"
                    || string.IsNullOrEmpty(moduleFilter)
                    || p.Group == moduleFilter)
                .OrderBy(p => p.Group)
                .ThenBy(p => p.DisplayName)
                .ToList();

            table.Rows.Clear();
            foreach (var p in view) {
                int rowIdx = table.Rows.Add();
                var label = string.IsNullOrEmpty(p.Group)
                    ? $"{p.DisplayName}  ({p.Code})"
                    : $"[{p.Group}] {p.DisplayName}  ({p.Code})";
                table.Rows[rowIdx].Cells[0].Value = label;
                table.Rows[rowIdx].Tag = p;

                for (int i = 0; i < _roles.Count; i++) {
                    var role = _roles[i];
                    bool granted = _roleToPermIds.TryGetValue(role.RoleId, out var set) && set.Contains(p.PermissionId);
                    table.Rows[rowIdx].Cells[i + 1].Value = granted;
                }
            }
        }

        private void SetColumnAll(int roleId, bool check) {
            foreach (DataGridViewRow row in table.Rows) {
                int colIdx = ColIndexByRoleId(roleId);
                if (colIdx > 0) row.Cells[colIdx].Value = check;
            }

            if (_roleToPermIds.TryGetValue(roleId, out var set)) {
                if (check) {
                    foreach (DataGridViewRow row in table.Rows)
                        if (row.Tag is PermissionItem p) set.Add(p.PermissionId);
                } else set.Clear();
            }
        }

        private int ColIndexByRoleId(int roleId) {
            for (int i = 0; i < _roles.Count; i++)
                if (_roles[i].RoleId == roleId) return i + 1; // +1 vì col0 là label
            return -1;
        }

        private void Table_CellValueChanged(object? sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex <= 0) return;
            var row = table.Rows[e.RowIndex];
            if (row.Tag is not PermissionItem p) return;

            var role = _roles[e.ColumnIndex - 1];
            bool granted = Convert.ToBoolean(row.Cells[e.ColumnIndex].Value ?? false);

            if (!_roleToPermIds.TryGetValue(role.RoleId, out var set)) {
                set = new HashSet<int>();
                _roleToPermIds[role.RoleId] = set;
            }
            if (granted) set.Add(p.PermissionId);
            else set.Remove(p.PermissionId);
        }

        private void SaveAllRoles() {
            foreach (var role in _roles) {
                var set = _roleToPermIds.TryGetValue(role.RoleId, out var s) ? s : new HashSet<int>();
                PermissionRepository.SavePermissionsForRole(role.RoleId, set);
            }
            MessageBox.Show("Đã lưu ma trận quyền cho tất cả vai trò.", "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ClearAll() {
            foreach (DataGridViewRow row in table.Rows)
                for (int c = 1; c < table.Columns.Count; c++)
                    row.Cells[c].Value = false;

            foreach (var rid in _roles.Select(r => r.RoleId))
                _roleToPermIds[rid].Clear();
        }
    }
}
