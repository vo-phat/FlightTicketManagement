using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using BUS.Auth;
using DTO.Permissions;

namespace GUI.Features.Setting.SubFeatures {
    public class AssignByRoleControl : UserControl {
        private readonly RolePermissionService _service = new RolePermissionService();

        // Lọc module
        private UnderlinedComboBox cboModule;

        // Ma trận
        private TableCustom table;
        private FlowLayoutPanel selectAllRow;

        // Hành động
        private FlowLayoutPanel actions;
        private Button btnSave, btnClearAll;

        // Data
        private List<PermissionItem> _allPerms = new();
        private List<RoleItem> _roles = new();
        private Dictionary<int, HashSet<int>> _roleToPermIds = new(); // roleId -> set(permissionId)
        private bool _suspendReload = false;

        // Trạng thái cho Role
        private int? _selectedRoleId = null;
        private Button btnAddRole, btnEditRole, btnDeleteRole;

        public AssignByRoleControl() {
            InitializeComponent();
        }

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
                ReadOnly = false,
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
            table.ColumnHeaderMouseClick += Table_ColumnHeaderMouseClick;

            // ===== Footer actions ===============================================
            actions = new FlowLayoutPanel {
                Dock = DockStyle.Bottom,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(24, 8, 24, 8)
            };
            btnAddRole = new SecondaryButton("➕ Thêm vai trò");
            btnEditRole = new SecondaryButton("✏️ Sửa vai trò");
            btnDeleteRole = new SecondaryButton("🗑 Xóa vai trò");
            btnSave = new PrimaryButton("💾 Lưu");
            btnClearAll = new SecondaryButton("Bỏ chọn tất cả");

            btnAddRole.Click += (_, __) => AddRole();
            btnEditRole.Click += (_, __) => EditRole();
            btnDeleteRole.Click += (_, __) => DeleteRole();

            btnSave.Click += (_, __) => SaveAllRoles();
            btnClearAll.Click += (_, __) => ClearAll();

            actions.Controls.AddRange(new Control[] { btnSave, btnClearAll, btnDeleteRole, btnEditRole, btnAddRole });

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

        /// <summary>
        /// Tải dữ liệu từ service (permissions + roles + role->perms)
        /// </summary>
        private void LoadData() {
            _suspendReload = true;

            _allPerms = _service.GetAllPermissions();
            _roles = _service.GetAllRoles();

            var groups = _allPerms.Select(p => p.Group ?? "Misc").Distinct().OrderBy(x => x).ToList();
            cboModule.Items.Clear();
            cboModule.Items.Add("(Tất cả)");
            foreach (var g in groups) cboModule.Items.Add(g);
            cboModule.SelectedIndex = 0;

            _roleToPermIds.Clear();
            foreach (var r in _roles)
                _roleToPermIds[r.RoleId] = _service.GetPermissionIdsOfRole(r.RoleId) ?? new HashSet<int>();

            BuildTableColumns();
            BuildSelectAllRow();

            _suspendReload = false;
            ReloadTable();
        }

        /// <summary>
        /// Xây dựng cột (gọi lại khi roles thay đổi)
        /// - Cột admin được đặt ReadOnly = true (hiển thị checked và không cho thay đổi)
        /// </summary>
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

            // Cột checkbox cho từng role (thêm động)
            foreach (var role in _roles) {
                var col = new DataGridViewCheckBoxColumn {
                    Name = $"Role_{role.RoleId}",
                    HeaderText = role.Name,
                    ThreeState = false,
                    TrueValue = true,
                    FalseValue = false,
                    FillWeight = 80
                };

                // Quy ước: nếu role là quản trị viên (dựa trên role.Code) -> disable editing
                bool isAdmin = IsAdminRole(role);
                if (isAdmin) {
                    col.ReadOnly = true; // không cho thay đổi checkbox
                }

                table.Columns.Add(col);
            }
        }

        /// <summary>
        /// Build hàng select-all phía trên dựa trên _roles hiện tại
        /// </summary>
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

                // Nếu role admin -> disable checkbox select-all UI (vì cột admin mặc định full checked)
                if (IsAdminRole(role)) {
                    cb.Enabled = false;
                    cb.Checked = true;
                }

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

                    // Nếu role admin -> force true (hiển thị tick full dòng) và cell ReadOnly = true
                    if (IsAdminRole(role)) {
                        table.Rows[rowIdx].Cells[i + 1].Value = true;
                        table.Rows[rowIdx].Cells[i + 1].ReadOnly = true;
                        // đảm bảo mapping admin chứa full permissions để khi lưu không bị xóa
                        if (!_roleToPermIds.TryGetValue(role.RoleId, out var s)) {
                            s = new HashSet<int>();
                            _roleToPermIds[role.RoleId] = s;
                        }
                        s.Add(p.PermissionId);
                    } else {
                        table.Rows[rowIdx].Cells[i + 1].Value = granted;
                        table.Rows[rowIdx].Cells[i + 1].ReadOnly = false;
                    }
                }
            }
        }

        private void SetColumnAll(int roleId, bool check) {
            int colIdx = ColIndexByRoleId(roleId);
            if (colIdx <= 0) return;

            foreach (DataGridViewRow row in table.Rows) {
                row.Cells[colIdx].Value = check;
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
            // Nếu column này readonly (ví dụ admin), bỏ qua
            if (table.Columns[e.ColumnIndex].ReadOnly || row.Cells[e.ColumnIndex].ReadOnly) return;

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
                try {
                    // Gọi service để lưu.
                    _service.SavePermissionsForRole(role.RoleId, set);
                } catch (Exception ex) {
                    MessageBox.Show($"Lưu quyền cho vai trò {role.Name} thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            MessageBox.Show("Đã lưu ma trận quyền cho tất cả vai trò.", "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ClearAll() {
            foreach (DataGridViewRow row in table.Rows)
                for (int c = 1; c < table.Columns.Count; c++) {
                    // Nếu cột readonly (admin) thì không clear
                    if (table.Columns[c].ReadOnly) continue;
                    row.Cells[c].Value = false;
                }

            foreach (var rid in _roles.Select(r => r.RoleId))
                if (_roleToPermIds.TryGetValue(rid, out var s) && (ColIndexByRoleId(rid) > 0 && !table.Columns[ColIndexByRoleId(rid)].ReadOnly))
                    s.Clear();
        }

        private void HighlightRoleColumn(int columnIndex) {
            // Reset màu tất cả header
            for (int i = 0; i < table.Columns.Count; i++)
                table.Columns[i].HeaderCell.Style.BackColor = Color.White;

            // Tô màu cho cột đang chọn
            table.Columns[columnIndex].HeaderCell.Style.BackColor = Color.FromArgb(210, 230, 255);
        }

        private void ResetHeaderColors() {
            for (int i = 0; i < table.Columns.Count; i++)
                table.Columns[i].HeaderCell.Style.BackColor = Color.White;
        }

        private void Table_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex <= 0) {
                _selectedRoleId = null;
                ResetHeaderColors();
                return;
            }

            var role = _roles[e.ColumnIndex - 1];
            _selectedRoleId = role.RoleId;

            HighlightRoleColumn(e.ColumnIndex);
        }

        // ---------- New: Add / Edit / Delete role ----------
        private void AddRole() {
            using var dlg = new RoleEditForm(null); // form ở chế độ thêm mới
            var dr = dlg.ShowDialog(this);
            if (dr == DialogResult.OK) {
                // Nếu RoleEditForm trả về id mới qua property CreatedRoleId
                int newId = 0;
                try {
                    // dùng reflection-safe check (nếu form không có prop, fall back reload)
                    var prop = dlg.GetType().GetProperty("CreatedRoleId");
                    if (prop != null) newId = (int)(prop.GetValue(dlg) ?? 0);
                } catch { newId = 0; }

                if (newId > 0) {
                    // Insert cột cục bộ để mượt UI
                    var newRole = _service.GetRoleById(newId);
                    if (newRole != null) InsertRoleColumn(newRole);
                    // Ensure mapping exists
                    _roleToPermIds[newId] = _service.GetPermissionIdsOfRole(newId) ?? new HashSet<int>();
                } else {
                    // fallback: reload toàn bộ
                    LoadData();
                }
            }
        }

        private void EditRole() {
            if (_selectedRoleId == null) {
                MessageBox.Show("Vui lòng chọn cột vai trò (click header) để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var role = _roles.FirstOrDefault(r => r.RoleId == _selectedRoleId.Value);
            if (role == null) return;

            using var dlg = new RoleEditForm(role); // truyền role để sửa
            var dr = dlg.ShowDialog(this);
            if (dr == DialogResult.OK) {
                // Re-fetch updated role and update UI header
                var updated = _service.GetRoleById(role.RoleId);
                if (updated != null) UpdateRoleColumn(updated);
            }
        }

        private void DeleteRole() {
            if (_selectedRoleId == null) {
                MessageBox.Show("Vui lòng chọn cột vai trò (click header) để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var role = _roles.FirstOrDefault(r => r.RoleId == _selectedRoleId.Value);
            if (role == null) return;

            // Không cho xóa role admin
            if (IsAdminRole(role)) {
                MessageBox.Show("Không thể xóa vai trò quản trị viên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa vai trò '{role.Name}' không? Hành động này không thể hoàn tác.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try {
                _service.DeleteRole(role.RoleId);
                // nếu không ném exception -> success
                RemoveRoleColumn(role.RoleId);
            } catch (Exception ex) {
                MessageBox.Show($"Xóa vai trò thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- Column helpers: insert / update / remove ----------
        private void InsertRoleColumn(RoleItem newRole) {
            if (newRole == null) return;

            // 1. thêm vào danh sách role UI
            _roles.Add(newRole);

            // 2. thêm column checkbox cuối cùng
            var col = new DataGridViewCheckBoxColumn {
                Name = $"Role_{newRole.RoleId}",
                HeaderText = newRole.Name,
                ThreeState = false,
                TrueValue = true,
                FalseValue = false,
                FillWeight = 80
            };

            // If admin -> readonly
            if (!string.IsNullOrEmpty(newRole.Code) && newRole.Code.Trim().Equals("ADMIN", StringComparison.OrdinalIgnoreCase))
                col.ReadOnly = true;

            table.Columns.Add(col);
            int colIdx = table.Columns.Count - 1;

            // 3. add a select-all checkbox in selectAllRow
            var cb = new CheckBox {
                Text = newRole.Name,
                AutoSize = true,
                Tag = newRole.RoleId,
                Margin = new Padding(8, 4, 0, 0),
            };
            cb.CheckedChanged += (s, e) => {
                var rid = (int)((CheckBox)s).Tag;
                SetColumnAll(rid, cb.Checked);
            };

            if (!string.IsNullOrEmpty(newRole.Code) && newRole.Code.Trim().Equals("ADMIN", StringComparison.OrdinalIgnoreCase)) {
                cb.Enabled = false;
                cb.Checked = true;
            }

            selectAllRow.Controls.Add(cb);

            // 4. populate values for each row based on _roleToPermIds (if exists)
            var existing = _roleToPermIds.TryGetValue(newRole.RoleId, out var ex) ? ex : new HashSet<int>();
            for (int r = 0; r < table.Rows.Count; r++) {
                var row = table.Rows[r];
                if (row.Tag is PermissionItem p) {
                    bool granted = existing.Contains(p.PermissionId);
                    if (!string.IsNullOrEmpty(newRole.Code) && newRole.Code.Trim().Equals("ADMIN", StringComparison.OrdinalIgnoreCase)) {
                        row.Cells[colIdx].Value = true;
                        row.Cells[colIdx].ReadOnly = true;
                        // ensure mapping contains all permissions
                        if (!_roleToPermIds.TryGetValue(newRole.RoleId, out var set)) {
                            set = new HashSet<int>();
                            _roleToPermIds[newRole.RoleId] = set;
                        }
                        set.Add(p.PermissionId);
                    } else {
                        row.Cells[colIdx].Value = granted;
                        row.Cells[colIdx].ReadOnly = false;
                    }
                }
            }

            // ensure mapping exists
            if (!_roleToPermIds.ContainsKey(newRole.RoleId))
                _roleToPermIds[newRole.RoleId] = existing;
        }

        private void UpdateRoleColumn(RoleItem updatedRole) {
            if (updatedRole == null) return;
            // Update in _roles list
            var idx = _roles.FindIndex(r => r.RoleId == updatedRole.RoleId);
            if (idx >= 0) _roles[idx] = updatedRole;

            int colIdx = ColIndexByRoleId(updatedRole.RoleId);
            if (colIdx > 0) {
                table.Columns[colIdx].HeaderText = updatedRole.Name;

                // Update selectAll label
                foreach (Control c in selectAllRow.Controls) {
                    if (c is CheckBox cb && cb.Tag is int rid && rid == updatedRole.RoleId) {
                        cb.Text = updatedRole.Name;
                        break;
                    }
                }

                // If admin status changed (rare), update readonly state
                bool isAdmin = !string.IsNullOrEmpty(updatedRole.Code) && updatedRole.Code.Trim().Equals("ADMIN", StringComparison.OrdinalIgnoreCase);
                table.Columns[colIdx].ReadOnly = isAdmin;
                for (int r = 0; r < table.Rows.Count; r++) {
                    var cell = table.Rows[r].Cells[colIdx];
                    if (isAdmin) {
                        cell.Value = true;
                        cell.ReadOnly = true;
                    } else {
                        // keep existing value from mapping if present
                        if (_roleToPermIds.TryGetValue(updatedRole.RoleId, out var set))
                            cell.Value = set.Contains(((PermissionItem)table.Rows[r].Tag).PermissionId);
                        cell.ReadOnly = false;
                    }
                }

                // Update selectAll checkbox enablement
                foreach (Control c in selectAllRow.Controls) {
                    if (c is CheckBox cb && cb.Tag is int rid && rid == updatedRole.RoleId) {
                        cb.Enabled = !isAdmin;
                        if (isAdmin) cb.Checked = true;
                        break;
                    }
                }
            }
        }

        private void RemoveRoleColumn(int roleId) {
            int colIdx = ColIndexByRoleId(roleId);
            if (colIdx > 0) {
                table.Columns.RemoveAt(colIdx);
            }

            // remove selectAll checkbox
            Control toRemove = null;
            foreach (Control c in selectAllRow.Controls) {
                if (c is CheckBox cb && cb.Tag is int rid && rid == roleId) {
                    toRemove = c;
                    break;
                }
            }
            if (toRemove != null) selectAllRow.Controls.Remove(toRemove);

            // remove from lists
            var role = _roles.FirstOrDefault(r => r.RoleId == roleId);
            if (role != null) _roles.Remove(role);
            if (_roleToPermIds.ContainsKey(roleId)) _roleToPermIds.Remove(roleId);

            _selectedRoleId = null;
        }

        // Helper: cách nhận dạng role admin (dựa trên Role.Code)
        private bool IsAdminRole(RoleItem role) {
            if (role == null) return false;
            var code = (role.Code ?? string.Empty).Trim();
            return code.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);
        }
    }
}
