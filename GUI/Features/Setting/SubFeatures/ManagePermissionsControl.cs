using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Setting.SubFeatures {
    // DTO & Service/Provider interface (khớp mô hình 3 lớp)
    public sealed class PermissionDto {
        public int PermissionId { get; set; }
        public string PermissionCode { get; set; } = "";
        public string PermissionName { get; set; } = "";
    }
    public interface IPermissionService {
        Task<BindingList<PermissionDto>> SearchAsync(string? code, string? name);
        Task<(bool ok, string? error, int? id)> CreateAsync(string code, string name);
        Task<(bool ok, string? error)> UpdateAsync(int id, string code, string name);
        Task<(bool ok, string? error)> DeleteAsync(int id);
    }
    public interface IPermissionProvider {
        bool Has(string permCode);
    }

    public class ManagePermissionsControl : UserControl {
        // UI
        private TableCustom table;
        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight;
        private Label lblTitle;
        private UnderlinedTextField txtCode, txtName;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        private readonly BindingSource _bs = new();

        // Optional: service & provider (có thể null => chế độ demo)
        private readonly IPermissionService? _svc;
        private readonly IPermissionProvider? _perm;

        public ManagePermissionsControl() : this(null, null) { }

        public ManagePermissionsControl(IPermissionService? svc, IPermissionProvider? perm) {
            _svc = svc; _perm = perm;
            InitializeComponent();
        }

        private void InitializeComponent() {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label {
                Text = "🔐 Danh sách quyền (Permissions)",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // Filters (Mã / Tên quyền) + nút Tìm / Thêm
            filterLeft = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false };
            txtCode = new UnderlinedTextField("Mã quyền", "") { Width = 300, Margin = new Padding(0, 0, 24, 0) };
            txtName = new UnderlinedTextField("Tên quyền", "") { Width = 300, Margin = new Padding(0, 0, 24, 0) };
            filterLeft.Controls.AddRange(new Control[] { txtCode, txtName });

            filterRight = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
            var btnSearch = new PrimaryButton("🔍 Tìm") { Width = 100, Height = 36, Margin = new Padding(0, 6, 0, 6) };
            var btnAdd = new SecondaryButton("➕ Thêm") { Width = 100, Height = 36, Margin = new Padding(12, 6, 0, 6) };
            filterRight.Controls.Add(btnSearch);
            filterRight.Controls.Add(btnAdd);

            // Wrapper cho filter
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

            // Table
            table = new TableCustom {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 24),
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            table.Columns.Add("permissionCode", "Mã quyền");
            table.Columns.Add("permissionName", "Tên quyền");

            var colAction = new DataGridViewTextBoxColumn {
                Name = ACTION_COL,
                HeaderText = "Thao tác",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            table.Columns.Add(colAction);
            var colHiddenId = new DataGridViewTextBoxColumn { Name = "permissionIdHidden", Visible = false };
            table.Columns.Add(colHiddenId);

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // Root
            root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(filterWrap, 0, 1);
            root.Controls.Add(table, 0, 2);

            Controls.Add(root);
            ResumeLayout(false);

            // Events
            Load += async (_, __) => await ReloadAsync();
            btnSearch.Click += async (_, __) => await ReloadAsync();
            txtCode.TextChanged += (_, __) => { /* optional live search */ };
            txtName.TextChanged += (_, __) => { /* optional live search */ };

            btnAdd.Enabled = _perm?.Has("SYSTEM.PERMISSION.MANAGE") ?? true;
            btnAdd.Click += async (_, __) => await OnAddAsync();
        }

        // ====== Data load ======
        private async Task ReloadAsync() {
            table.Rows.Clear();
            BindingList<PermissionDto> data;

            if (_svc == null) {
                // DEMO rows nếu chưa gắn service
                data = new BindingList<PermissionDto>(new[] {
                    new PermissionDto { PermissionId = 1, PermissionCode = "FLIGHT.MANAGE", PermissionName = "Quản lý chuyến bay" },
                    new PermissionDto { PermissionId = 2, PermissionCode = "SYSTEM.PERMISSION.MANAGE", PermissionName = "Quản trị quyền" },
                    new PermissionDto { PermissionId = 3, PermissionCode = "REPORTS.VIEW", PermissionName = "Xem báo cáo" },
                }.ToList());
            } else {
                data = await _svc.SearchAsync(
                    string.IsNullOrWhiteSpace(txtCode.Text) ? null : txtCode.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtName.Text) ? null : txtName.Text.Trim()
                );
            }

            foreach (var p in data) {
                table.Rows.Add(p.PermissionCode, p.PermissionName, null, p.PermissionId);
            }
        }

        private async Task OnAddAsync() {
            using var f = new PermissionEditForm("Thêm quyền (demo)");
            if (f.ShowDialog(this) != DialogResult.OK) return;

            // thêm tạm vào bảng
            table.Rows.Add(f.PermissionCode, f.PermissionName, null, 999);
            MessageBox.Show($"Đã thêm quyền (demo): {f.PermissionCode}");
            await Task.CompletedTask;
        }

        // ====== Action drawing (Xem / Sửa / Xóa) ======
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

            bool canManage = _perm?.Has("SYSTEM.PERMISSION.MANAGE") ?? true;
            Color link = Color.FromArgb(0, 92, 175);
            Color sep = Color.FromArgb(120, 120, 120);
            Color del = canManage ? Color.FromArgb(220, 53, 69) : Color.FromArgb(180, 180, 180);

            // "Xem" luôn khả dụng
            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, r.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);

            // "Sửa" / "Xóa" theo quyền
            var editColor = canManage ? link : Color.FromArgb(180, 180, 180);
            TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, r.rcEdit.Location, editColor, TextFormatFlags.NoPadding);
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

            bool canManage = _perm?.Has("SYSTEM.PERMISSION.MANAGE") ?? true;
            bool hover = r.rcView.Contains(p) || (canManage && (r.rcEdit.Contains(p) || r.rcDel.Contains(p)));
            table.Cursor = hover ? Cursors.Hand : Cursors.Default;
        }

        private async void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells["permissionIdHidden"].Value);
            string code = row.Cells["permissionCode"].Value?.ToString() ?? "(n/a)";
            string name = row.Cells["permissionName"].Value?.ToString() ?? "(n/a)";

            if (r.rcView.Contains(p)) {
                using var frm = new PermissionDetailForm(code, name);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(FindForm());
                return;
            }

            bool canManage = _perm?.Has("SYSTEM.PERMISSION.MANAGE") ?? true;
            if (!canManage) return;

            if (r.rcEdit.Contains(p)) {
                using var frm = new PermissionEditForm("Sửa quyền") { PermissionCode = code, PermissionName = name };
                if (frm.ShowDialog(FindForm()) != DialogResult.OK) return;
                if (_svc == null) { MessageBox.Show("Chưa gắn PermissionService."); return; }
                var (ok, err) = await _svc.UpdateAsync(id, frm.PermissionCode, frm.PermissionName);
                if (!ok) { MessageBox.Show(err ?? "Không cập nhật được quyền.", "Lỗi"); return; }
                await ReloadAsync();
            } else if (r.rcDel.Contains(p)) {
                if (MessageBox.Show($"Xoá quyền '{code}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                if (_svc == null) { MessageBox.Show("Chưa gắn PermissionService."); return; }
                var (ok, err) = await _svc.DeleteAsync(id);
                if (!ok) { MessageBox.Show(err ?? "Không xoá được quyền.\n" + err, "Lỗi"); return; }
                await ReloadAsync();
            }
        }
    }

    // ===== Form xem nhanh =====
    internal sealed class PermissionDetailForm : Form {
        public PermissionDetailForm(string code, string name) {
            Text = $"Chi tiết quyền {code}";
            Size = new Size(640, 360);
            BackColor = Color.White;

            var panel = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 2,
                Padding = new Padding(24)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            panel.Controls.Add(new Label { Text = "Mã quyền:", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 0);
            panel.Controls.Add(new Label { Text = code, AutoSize = true, Font = new Font("Segoe UI", 10) }, 1, 0);
            panel.Controls.Add(new Label { Text = "Tên quyền:", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 1);
            panel.Controls.Add(new Label { Text = name, AutoSize = true, Font = new Font("Segoe UI", 10) }, 1, 1);

            var btnClose = new PrimaryButton("Đóng") { Width = 100, Height = 36, Anchor = AnchorStyles.Right };
            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            flow.Controls.Add(btnClose);
            panel.Controls.Add(flow, 0, 2);
            panel.SetColumnSpan(flow, 2);

            btnClose.Click += (_, __) => Close();
            Controls.Add(panel);
        }
    }

    // ===== Form chỉnh sửa (tái sử dụng form trước, layout giống custom style) =====
    internal sealed class PermissionEditForm : Form {
        private readonly UnderlinedTextField txtCode = new("Mã quyền", "") { Dock = DockStyle.Fill, Margin = new Padding(0, 6, 0, 6) };
        private readonly UnderlinedTextField txtName = new("Tên quyền", "") { Dock = DockStyle.Fill, Margin = new Padding(0, 6, 0, 6) };
        private readonly Button btnOk = new PrimaryButton("OK") { Width = 100, Height = 36 };
        private readonly Button btnCancel = new SecondaryButton("Huỷ") { Width = 100, Height = 36 };

        public string PermissionCode { get => txtCode.Text; set => txtCode.Text = value; }
        public string PermissionName { get => txtName.Text; set => txtName.Text = value; }

        public PermissionEditForm(string title) {
            Text = title;
            Size = new Size(560, 260);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            var grid = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                ColumnCount = 2,
                RowCount = 3
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            grid.Controls.Add(new Label { Text = "Mã quyền", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 0);
            grid.Controls.Add(txtCode, 1, 0);
            grid.Controls.Add(new Label { Text = "Tên quyền", AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 1);
            grid.Controls.Add(txtName, 1, 1);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            buttons.Controls.Add(btnOk);
            buttons.Controls.Add(btnCancel);
            grid.Controls.Add(buttons, 0, 2);
            grid.SetColumnSpan(buttons, 2);

            btnOk.Click += (_, __) => DialogResult = DialogResult.OK;
            btnCancel.Click += (_, __) => DialogResult = DialogResult.Cancel;

            Controls.Add(grid);
        }
    }
}
