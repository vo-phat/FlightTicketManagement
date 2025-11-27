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
    internal class ManageAccountRolesControl : UserControl {
        private readonly RolePermissionService _service = new RolePermissionService();
        private TableCustom table;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Khóa";
        private const string SEP = " / ";

        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight;
        private Label lblTitle;
        private UnderlinedTextField txtEmail;
        private UnderlinedComboBox cbRole;

        private List<UserItem> _allUsers = new();
        private List<RoleItem> _allRoles = new();

        public ManageAccountRolesControl() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // ===== Title =========================================================
            lblTitle = new Label {
                Text = "👤 Danh sách tài khoản & Quyền",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Filters =======================================================
            filterLeft = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false
            };

            txtEmail = new UnderlinedTextField("Email", "") {
                Width = 260,
                Margin = new Padding(0, 0, 24, 0)
            };

            cbRole = new UnderlinedComboBox("Vai trò", new object[] { "Tất cả quyền" }) {
                MinimumSize = new Size(0, 72),
                Width = 220,
                Margin = new Padding(0, 6, 24, 6)
            };

            filterLeft.Controls.AddRange(new Control[] { txtEmail, cbRole });

            filterRight = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };

            var btnSearch = new PrimaryButton("🔍 Tìm") {
                Width = 100,
                Height = 36,
                Margin = new Padding(0, 6, 0, 6)
            };
            var btnAdd = new SecondaryButton("➕ Thêm tài khoản") {
                Width = 160,
                Height = 36,
                Margin = new Padding(12, 6, 0, 6)
            };

            filterRight.Controls.Add(btnSearch);
            filterRight.Controls.Add(btnAdd);

            filterWrap = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 16, 24, 0),
                ColumnCount = 2
            };
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filterWrap.Controls.Add(filterLeft, 0, 0);
            filterWrap.Controls.Add(filterRight, 1, 0);

            // ===== Table =========================================================
            table = new TableCustom {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false
            };

            table.Columns.Add("email", "Email");
            table.Columns.Add("roles", "Vai trò");

            var colAction = new DataGridViewTextBoxColumn {
                Name = ACTION_COL,
                HeaderText = "Thao tác",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            table.Columns.Add(colAction);

            var colHiddenId = new DataGridViewTextBoxColumn {
                Name = "accountIdHidden",
                Visible = false
            };
            table.Columns.Add(colHiddenId);
            var colIsActive = new DataGridViewTextBoxColumn {
                Name = "isActiveHidden",
                Visible = false
            };
            table.Columns.Add(colIsActive);

            var colFailedAttempts = new DataGridViewTextBoxColumn {
                Name = "failedAttemptsHidden",
                Visible = false
            };
            table.Columns.Add(colFailedAttempts);

            var colCreatedAt = new DataGridViewTextBoxColumn {
                Name = "createdAtHidden",
                Visible = false
            };
            table.Columns.Add(colCreatedAt);

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // ===== Root layout ===================================================
            root = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(24, 12, 24, 24)
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(filterWrap, 0, 1);
            root.Controls.Add(table, 0, 2);

            Controls.Add(root);
            ResumeLayout(false);

            // ===== Events ========================================================
            Load += (_, __) => ReloadAll();
            btnSearch.Click += (_, __) => ReloadTable();
            btnAdd.Click += (_, __) => OpenAddAccountDialog();
            cbRole.SelectedIndexChanged += (_, __) => ReloadTable();
        }

        // =======================================================================
        // Data
        // =======================================================================
        private void ReloadAll() {
            _allUsers = _service.GetAllUsers();
            _allRoles = _service.GetAllRoles();

            cbRole.Items.Clear();
            cbRole.Items.Add("(Tất cả)");
            foreach (var r in _allRoles) cbRole.Items.Add(r);
            cbRole.SelectedIndex = 0;

            ReloadTable();
        }

        private void ReloadTable() {
            table.Rows.Clear();

            var emailFilter = txtEmail.Text?.Trim().ToLower();
            RoleItem? selectedRole = cbRole.SelectedItem as RoleItem;

            foreach (var u in _allUsers) {
                var rolesOfAccount = _service.GetRoleIdsOfAccount(u.AccountId);
                var roleNames = _allRoles
                    .Where(r => rolesOfAccount.Contains(r.RoleId))
                    .Select(r => r.Name)
                    .ToList();
                var roleText = roleNames.Any() ? string.Join(", ", roleNames) : "(Chưa gán)";

                // filter
                if (!string.IsNullOrWhiteSpace(emailFilter) &&
                    !u.Email.ToLower().Contains(emailFilter)) continue;

                if (selectedRole != null && !(selectedRole is string)) {
                    if (!rolesOfAccount.Contains(selectedRole.RoleId)) continue;
                }

                table.Rows.Add(u.Email, roleText, null, u.AccountId, u.IsActive, u.FailedAttempts, u.CreatedAt);
            }
        }

        // =======================================================================
        // Add account
        // =======================================================================
        private void OpenAddAccountDialog() {
            using var f = new AddAccountForm();
            if (f.ShowDialog(FindForm()) != DialogResult.OK)
                return;

            try {
                // isActive = true (mặc định kích hoạt tài khoản)
                _service.CreateAccountWithRoles(
                    f.Email,
                    f.Password,
                    f.RoleId
                );

                MessageBox.Show(
                    "Thêm tài khoản mới thành công.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ReloadAll();
            } catch (Exception ex) {
                MessageBox.Show(
                    "Không thể thêm tài khoản: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =======================================================================
        // Action column drawing (Xem / Phân quyền / Xóa)
        // =======================================================================
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDel) GetRects(Rectangle cellBounds, Font font) {
            int pad = 6;
            int x = cellBounds.Left + pad;
            int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;

            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szE = TextRenderer.MeasureText(TXT_EDIT, font, Size.Empty, flags);
            var szD = TextRenderer.MeasureText(TXT_DEL, font, Size.Empty, flags);

            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcE = new Rectangle(new Point(x, y), szE); x += szE.Width + szS.Width;
            var rcD = new Rectangle(new Point(x, y), szD);
            return (rcV, rcE, rcD);
        }

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var font = e.CellStyle.Font ?? table.Font;
            var r = GetRects(e.CellBounds, font);

            Color link = Color.FromArgb(0, 92, 175);
            Color sep = Color.FromArgb(120, 120, 120);
            Color del = Color.FromArgb(220, 53, 69);

            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, r.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, r.rcEdit.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcEdit.Right, r.rcEdit.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_DEL, font, r.rcDel.Location, del, TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            table.Cursor = (r.rcView.Contains(p) || r.rcEdit.Contains(p) || r.rcDel.Contains(p))
                ? Cursors.Hand
                : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            int accountId = Convert.ToInt32(row.Cells["accountIdHidden"].Value);
            string email = row.Cells["email"].Value?.ToString() ?? "(n/a)";
            string roles = row.Cells["roles"].Value?.ToString() ?? "(n/a)";

            if (r.rcView.Contains(p)) {
                OpenAccountDetail(accountId);
            } else if (r.rcEdit.Contains(p)) {
                using var frm = new AccountRoleEditForm(accountId);
                if (frm.ShowDialog(FindForm()) == DialogResult.OK) {
                    ReloadTable();
                }
            } else if (r.rcDel.Contains(p)) {
                if (MessageBox.Show($"Khóa tài khoản '{email}'?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                try {
                    _service.DeleteAccount(accountId);

                    MessageBox.Show(
                        "Đã khóa tài khoản thành công.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    ReloadAll();
                } catch (Exception ex) {
                    MessageBox.Show(
                        "Không thể khóa tài khoản: " + ex.Message,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }
        private void OpenAccountDetail(int accountId) {
            var user = _allUsers.FirstOrDefault(u => u.AccountId == accountId);
            if (user == null) {
                MessageBox.Show("Không tìm thấy thông tin tài khoản.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var roleIds = _service.GetRoleIdsOfAccount(accountId);
            var roleNames = _allRoles
                .Where(r => roleIds.Contains(r.RoleId))
                .Select(r => r.Name)
                .ToList();

            string rolesText = roleNames.Any() ? string.Join(", ", roleNames) : "(Chưa gán)";

            using var frm = new AccountDetailForm(
                user.Email,
                rolesText,
                user.IsActive,
                user.FailedAttempts,
                user.CreatedAt
            );

            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(FindForm());
        }
    }
}
