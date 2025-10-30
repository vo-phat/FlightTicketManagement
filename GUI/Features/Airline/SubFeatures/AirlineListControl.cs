using BUS.Airline;
using DTO.Airline;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Airline.SubFeatures
{
    public class AirlineListControl : UserControl
    {
        private readonly AirlineBUS _bus = new AirlineBUS();
        private DataGridView table;

        // Khai báo các control tìm kiếm
        private UnderlinedTextField txtCode, txtName, txtCountry;
        private PrimaryButton btnSearch, btnAdd;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        // Sự kiện giao tiếp với Control cha
        public event Action<AirlineDTO>? ViewRequested;
        public event Action<AirlineDTO>? RequestEdit;
        public event Action? DataChanged;

        public AirlineListControl()
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
            var lblTitle = new Label { Text = "✈️ Danh sách hãng hàng không", Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point), ForeColor = Color.FromArgb(40, 55, 77), AutoSize = true, Dock = DockStyle.Top, Padding = new Padding(24, 20, 0, 12) };

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
            txtCode = new UnderlinedTextField("Mã hãng", "") { Width = 140, Margin = new Padding(6, 4, 6, 4), InheritParentBackColor = true, LineThickness = 1 };
            txtName = new UnderlinedTextField("Tên hãng", "") { Width = 200, Margin = new Padding(6, 4, 6, 4), InheritParentBackColor = true, LineThickness = 1 };
            txtCountry = new UnderlinedTextField("Quốc gia", "") { Width = 180, Margin = new Padding(6, 4, 6, 4), InheritParentBackColor = true, LineThickness = 1 };

            btnSearch = new PrimaryButton("🔍 Tìm") { Width = 90, Height = 40, Margin = new Padding(10, 6, 6, 6) };
            btnAdd = new PrimaryButton("➕ Thêm") { Width = 110, Height = 40, Margin = new Padding(6) };

            btnSearch.Click += (s, e) => RefreshList();
            btnAdd.Click += (s, e) => RequestEdit?.Invoke(new AirlineDTO());

            filterPanel.Controls.AddRange(new Control[] { txtCode, txtName, txtCountry, btnSearch, btnAdd });

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
            table.Columns.Add("airlineCode", "Mã hãng");
            table.Columns.Add("airlineName", "Tên hãng");
            table.Columns.Add("country", "Quốc gia");
            table.Columns.Add(ACTION_COL, "Thao tác");
            table.Columns[ACTION_COL].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            table.Columns[ACTION_COL].Width = 160;
            table.Columns.Add("airlineIdHidden", "ID");
            table.Columns["airlineIdHidden"].Visible = false;

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
                // 1. Lấy toàn bộ danh sách 
                List<AirlineDTO> filteredList = _bus.GetAllAirlines();

                // 2. Lấy giá trị tìm kiếm
                string searchCode = txtCode.Text?.Trim().ToLower() ?? "";
                string searchName = txtName.Text?.Trim().ToLower() ?? "";
                string searchCountry = txtCountry.Text?.Trim().ToLower() ?? "";

                // 3. Thực hiện LỌC BẰNG LINQ (từng thuộc tính)

                if (!string.IsNullOrWhiteSpace(searchCode))
                {
                    filteredList = filteredList
                        .Where(a => a.AirlineCode != null && a.AirlineCode.ToLower().Contains(searchCode))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(searchName))
                {
                    filteredList = filteredList
                        .Where(a => a.AirlineName != null && a.AirlineName.ToLower().Contains(searchName))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(searchCountry))
                {
                    filteredList = filteredList
                        .Where(a => a.Country != null && a.Country.ToLower().Contains(searchCountry))
                        .ToList();
                }

                // 4. Đổ dữ liệu đã lọc vào bảng
                table.Rows.Clear();
                foreach (var a in filteredList)
                {
                    table.Rows.Add(
                        a.AirlineCode,
                        a.AirlineName,
                        a.Country ?? "N/A",
                        null,
                        a.AirlineId
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
            int id = Convert.ToInt32(row.Cells["airlineIdHidden"].Value);
            string code = row.Cells["airlineCode"].Value?.ToString() ?? "";
            string name = row.Cells["airlineName"].Value?.ToString() ?? "";
            string country = row.Cells["country"].Value?.ToString() ?? "";

            var dto = new AirlineDTO(id, code, name, country);

            if (r.rcView.Contains(p))
                ViewRequested?.Invoke(dto);
            else if (r.rcEdit.Contains(p))
                RequestEdit?.Invoke(dto);
            else if (r.rcDel.Contains(p))
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa hãng '{name}'?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string message;
                    bool ok = _bus.DeleteAirline(id, out message);
                    MessageBox.Show(message, ok ? "Thành công" : "Lỗi",
                        MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                    if (ok) RefreshList();
                }
            }
        }
    }
}