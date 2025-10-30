using BUS.Aircraft;
using DTO.Aircraft;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Aircraft.SubFeatures
{
    public class AircraftListControl : UserControl
    {
        private readonly AircraftBUS _bus = new AircraftBUS();
        private DataGridView table;

        // Khai báo các control tìm kiếm
        private UnderlinedTextField txtAirlineId, txtModel, txtManufacturer, txtCapacity;
        private PrimaryButton btnSearch;
        private PrimaryButton btnAdd;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        // Sự kiện giao tiếp với Control cha
        public event Action<AircraftDTO>? ViewRequested;
        public event Action<AircraftDTO>? RequestEdit;
        public event Action? DataChanged;

        public AircraftListControl()
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
            var lblTitle = new Label
            {
                Text = "🛩 Danh sách máy bay",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(40, 55, 77),
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(24, 20, 0, 12)
            };

            // === PANEL BỘ LỌC (NHIỀU INPUT) ===
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
            txtAirlineId = new UnderlinedTextField("Mã hãng (ID)", "")
            {
                Width = 140,
                Margin = new Padding(6, 4, 6, 4),
                InheritParentBackColor = true,
                LineThickness = 1
            };
            txtModel = new UnderlinedTextField("Model", "")
            {
                Width = 180,
                Margin = new Padding(6, 4, 6, 4),
                InheritParentBackColor = true,
                LineThickness = 1
            };
            txtManufacturer = new UnderlinedTextField("Hãng sản xuất", "")
            {
                Width = 180,
                Margin = new Padding(6, 4, 6, 4),
                InheritParentBackColor = true,
                LineThickness = 1
            };
            txtCapacity = new UnderlinedTextField("Sức chứa", "")
            {
                Width = 120,
                Margin = new Padding(6, 4, 6, 4),
                InheritParentBackColor = true,
                LineThickness = 1
            };

            btnSearch = new PrimaryButton("🔍 Tìm")
            {
                Width = 90,
                Height = 40,
                Margin = new Padding(10, 6, 6, 6),
            };

            btnAdd = new PrimaryButton("➕ Thêm")
            {
                Width = 110,
                Height = 40,
                Margin = new Padding(6),
            };

            // Gọi RefreshList() khi bấm Tìm
            btnSearch.Click += (s, e) => RefreshList();
            btnAdd.Click += (s, e) => RequestEdit?.Invoke(new AircraftDTO());

            filterPanel.Controls.AddRange(new Control[] {
                txtAirlineId, txtModel, txtManufacturer, txtCapacity, btnSearch, btnAdd
            });

            // === BẢNG DANH SÁCH TÙY CHỈNH (TableCustom) ===
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

            // 1. Cấu hình các Cột (Giữ nguyên)
            table.Columns.Add("airlineId", "Mã hãng");
            table.Columns.Add("model", "Model");
            table.Columns.Add("manufacturer", "Hãng sản xuất");
            table.Columns.Add("capacity", "Sức chứa");
            table.Columns.Add(ACTION_COL, "Thao tác");
            table.Columns[ACTION_COL].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            table.Columns[ACTION_COL].Width = 160;
            table.Columns.Add("aircraftIdHidden", "ID");
            table.Columns["aircraftIdHidden"].Visible = false;

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

        // Phương thức Tải danh sách và LỌC ĐA TIÊU CHÍ
        public void RefreshList()
        {
            if (_isRefreshing) return;
            _isRefreshing = true;

            try
            {
                // 1. Lấy toàn bộ danh sách (hoặc chỉ cần lấy 1 lần nếu cache)
                List<AircraftDTO> filteredList = _bus.GetAllAircrafts();

                // 2. Lấy giá trị tìm kiếm
                string searchAirline = txtAirlineId.Text?.Trim().ToLower() ?? "";
                string searchModel = txtModel.Text?.Trim().ToLower() ?? "";
                string searchManu = txtManufacturer.Text?.Trim().ToLower() ?? "";
                string searchCap = txtCapacity.Text?.Trim().ToLower() ?? "";

                // 3. Thực hiện LỌC BẰNG LINQ (từng thuộc tính)

                // Lọc theo Airline ID (chuyển đổi ID thành chuỗi để so sánh)
                if (!string.IsNullOrWhiteSpace(searchAirline))
                {
                    filteredList = filteredList
                        .Where(a => a.AirlineId.ToString().Contains(searchAirline))
                        .ToList();
                }

                // Lọc theo Model
                if (!string.IsNullOrWhiteSpace(searchModel))
                {
                    filteredList = filteredList
                        .Where(a => a.Model != null && a.Model.ToLower().Contains(searchModel))
                        .ToList();
                }

                // Lọc theo Manufacturer
                if (!string.IsNullOrWhiteSpace(searchManu))
                {
                    filteredList = filteredList
                        .Where(a => a.Manufacturer != null && a.Manufacturer.ToLower().Contains(searchManu))
                        .ToList();
                }

                // Lọc theo Capacity (chuyển đổi Capacity sang chuỗi để so sánh)
                if (!string.IsNullOrWhiteSpace(searchCap))
                {
                    filteredList = filteredList
                        .Where(a => a.Capacity.HasValue && a.Capacity.Value.ToString().Contains(searchCap))
                        .ToList();
                }

                // 4. Đổ dữ liệu đã lọc vào bảng
                table.Rows.Clear();
                foreach (var a in filteredList)
                {
                    table.Rows.Add(
                        a.AirlineId,
                        a.Model ?? "N/A",
                        a.Manufacturer ?? "N/A",
                        a.Capacity.HasValue ? a.Capacity.Value.ToString() : "N/A",
                        null,
                        a.AircraftId
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

        // ======= Các phương thức vẽ và xử lý click giữ nguyên (GetRects, Table_CellPainting, Table_CellMouseMove, Table_CellMouseClick) =======

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
            int id = Convert.ToInt32(row.Cells["aircraftIdHidden"].Value);
            int airlineId = Convert.ToInt32(row.Cells["airlineId"].Value);
            string model = row.Cells["model"].Value?.ToString();
            string manufacturer = row.Cells["manufacturer"].Value?.ToString();
            string capacityStr = row.Cells["capacity"].Value?.ToString();
            int? capacity = capacityStr != "N/A" && int.TryParse(capacityStr, out int cap) ? cap : (int?)null;

            var dto = new AircraftDTO(id, airlineId, model, manufacturer, capacity);

            if (r.rcView.Contains(p))
                ViewRequested?.Invoke(dto);
            else if (r.rcEdit.Contains(p))
                RequestEdit?.Invoke(dto);
            else if (r.rcDel.Contains(p))
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa máy bay '{model}'?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string message;
                    bool ok = _bus.DeleteAircraft(id, out message);
                    MessageBox.Show(message, ok ? "Thành công" : "Lỗi",
                        MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                    if (ok) RefreshList();
                }
            }
        }
    }
}