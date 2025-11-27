using BUS.CabinClass;
using DTO.CabinClass;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.CabinClass.SubFeatures
{
    public class CabinClassListControl : UserControl
    {
        private readonly CabinClassBUS _bus = new CabinClassBUS();
        private DataGridView table;

        // Khai báo các control tìm kiếm
        private UnderlinedTextField txtName, txtDescription;
        private PrimaryButton btnSearch, btnAdd;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        public event Action<CabinClassDTO>? ViewRequested;
        public event Action<CabinClassDTO>? RequestEdit;
        public event Action? DataChanged;

        public CabinClassListControl()
        {
            InitializeComponent();
            RefreshList();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(232, 240, 252);
            Dock = DockStyle.Fill;
            AutoScroll = true;

            // === TIÊU ĐỀ ===
            var lblTitle = new Label { Text = "💺 Danh sách hạng ghế", Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.FromArgb(40, 55, 77), AutoSize = true, Dock = DockStyle.Top, Padding = new Padding(24, 20, 0, 12) };

            // === PANEL BỘ LỌC ===
            var filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 8, 24, 8),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.FromArgb(250, 253, 255)
            };

            // --- INPUTS TÙY CHỈNH ---
            txtName = new UnderlinedTextField("Tên hạng ghế", "") { Width = 200, Margin = new Padding(6, 4, 6, 4), LineThickness = 1 };
            txtDescription = new UnderlinedTextField("Mô tả", "") { Width = 300, Margin = new Padding(6, 4, 6, 4), LineThickness = 1 };

            btnSearch = new PrimaryButton("🔍 Tìm") { Width = 90, Height = 40, Margin = new Padding(10, 6, 6, 6) };
            btnAdd = new PrimaryButton("➕ Thêm") { Width = 110, Height = 40, Margin = new Padding(6) };

            btnSearch.Click += (s, e) => RefreshList();
            btnAdd.Click += (s, e) => RequestEdit?.Invoke(new CabinClassDTO());

            filterPanel.Controls.AddRange(new Control[] { txtName, txtDescription, btnSearch, btnAdd });

            // === BẢNG DANH SÁCH TÙY CHỈNH ===
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                CornerRadius = 16,
                BorderThickness = 2,
                BorderColor = Color.FromArgb(200, 200, 200),
            };

            // 1. Cấu hình các Cột 
            table.Columns.Add("name", "Tên hạng ghế");
            table.Columns.Add("description", "Mô tả");
            table.Columns.Add(ACTION_COL, "Thao tác");
            table.Columns[ACTION_COL].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            table.Columns[ACTION_COL].Width = 160;
            table.Columns.Add("classIdHidden", "ID");
            table.Columns["classIdHidden"].Visible = false;

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // === GHÉP TOÀN BỘ GIAO DIỆN ===
            Controls.Clear();
            Controls.Add(table);
            Controls.Add(filterPanel);
            Controls.Add(lblTitle);

            ResumeLayout(false);
        }

        private bool _isRefreshing = false;

        public void RefreshList()
        {
            if (_isRefreshing) return;
            _isRefreshing = true;

            try
            {
                List<CabinClassDTO> filteredList = _bus.GetAllCabinClasses();

                string searchName = txtName.Text?.Trim().ToLower() ?? "";
                string searchDescription = txtDescription.Text?.Trim().ToLower() ?? "";

                // 3. Thực hiện LỌC BẰNG LINQ

                if (!string.IsNullOrWhiteSpace(searchName))
                {
                    filteredList = filteredList
                        .Where(c => c.ClassName != null && c.ClassName.ToLower().Contains(searchName))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(searchDescription))
                {
                    filteredList = filteredList
                        .Where(c => c.Description != null && c.Description.ToLower().Contains(searchDescription))
                        .ToList();
                }

                // 4. Đổ dữ liệu đã lọc vào bảng
                table.Rows.Clear();
                foreach (var c in filteredList)
                {
                    table.Rows.Add(
                        c.ClassName,
                        c.Description ?? "N/A",
                        null,
                        c.ClassId
                    );
                }
                table.InvalidateColumn(table.Columns[ACTION_COL].Index);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        // ... (Các phương thức vẽ và xử lý click giữ nguyên) ...
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDel) GetRects(Rectangle b, Font f)
        {
            int pad = 6, x = b.Left + pad, y = b.Top + (b.Height - f.Height) / 2;
            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText(TXT_VIEW, f, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, f, Size.Empty, flags);
            var szE = TextRenderer.MeasureText(TXT_EDIT, f, Size.Empty, flags);
            var szD = TextRenderer.MeasureText(TXT_DEL, f, Size.Empty, flags);
            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcE = new Rectangle(new Point(x, y), szE); x += szE.Width + szS.Width;
            var rcD = new Rectangle(new Point(x, y), szD);
            return (rcV, rcE, rcD);
        }

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || table.Columns[e.ColumnIndex].Name != ACTION_COL) return;
            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
            var f = e.CellStyle.Font ?? table.Font;
            var r = GetRects(e.CellBounds, f);
            Color link = Color.FromArgb(0, 92, 175), sep = Color.FromArgb(120, 120, 120), del = Color.FromArgb(220, 53, 69);
            TextRenderer.DrawText(e.Graphics, TXT_VIEW, f, r.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, f, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_EDIT, f, r.rcEdit.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, f, new Point(r.rcEdit.Right, r.rcEdit.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_DEL, f, r.rcDel.Location, del, TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }
            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var f = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, f);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcEdit.Contains(p) || r.rcDel.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var f = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, f);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells["classIdHidden"].Value);
            string name = row.Cells["name"].Value?.ToString();
            string description = row.Cells["description"].Value?.ToString();

            var dto = new CabinClassDTO(id, name, description);

            if (r.rcView.Contains(p))
                ViewRequested?.Invoke(dto);
            else if (r.rcEdit.Contains(p))
                RequestEdit?.Invoke(dto);
            else if (r.rcDel.Contains(p))
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa hạng ghế '{name}'?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string message;
                    bool ok = _bus.DeleteCabinClass(id, out message);
                    MessageBox.Show(message, ok ? "Thành công" : "Lỗi",
                        MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                    if (ok) RefreshList();
                }
            }
        }
    }
}
