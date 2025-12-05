using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace GUI.Components.Tables {
    [DesignerCategory("Code")]
    [ToolboxItem(true)]
    public class TableCustom : DataGridView {
        // ==== Bảng màu (match SecondaryButton) ====
        private Color _borderColor = Color.FromArgb(40, 40, 40);     // outline
        private Color _headerFore = Color.FromArgb(126, 185, 232);  // #7EB9E8
        private Color _headerBack = Color.White;
        private Color _rowBack = Color.White;
        private Color _rowAltBack = Color.FromArgb(248, 250, 252);  // rất nhạt
        private Color _rowFore = Color.FromArgb(33, 37, 41);     // gần đen
        private Color _hoverBack = Color.FromArgb(232, 245, 255);  // nhẹ
        private Color _selectBack = Color.FromArgb(155, 209, 243);  // #9BD1F3
        private Color _selectFore = Color.White;

        private int _cornerRadius = 16;
        private int _borderThickness = 2;

        private int _hoverRowIndex = -1;

        public TableCustom() {
            // Tối ưu vẽ
            DoubleBuffered = true; // protected trong subclass -> OK
            EnableHeadersVisualStyles = false;

            // Tổng quan
            BackgroundColor = Color.White;
            BorderStyle = BorderStyle.None;
            GridColor = Color.FromArgb(230, 235, 240); // đường kẻ ngang rất nhẹ

            // Header
            ColumnHeadersVisible = true;
            Console.WriteLine($"[TableCustom Constructor] ColumnHeadersVisible set to: {ColumnHeadersVisible}");
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle {
                BackColor = _headerBack,
                ForeColor = _headerFore,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(12, 10, 12, 10),
                SelectionBackColor = _headerBack,
                SelectionForeColor = _headerFore,
                WrapMode = DataGridViewTriState.False
            };
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ColumnHeadersHeight = 44;
            Console.WriteLine($"[TableCustom Constructor] ColumnHeadersHeight set to: {ColumnHeadersHeight}");

            // Hàng
            DefaultCellStyle = new DataGridViewCellStyle {
                BackColor = _rowBack,
                ForeColor = _rowFore,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                Padding = new Padding(12, 6, 12, 6),
                SelectionBackColor = _selectBack,
                SelectionForeColor = _selectFore
            };
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle {
                BackColor = _rowAltBack
            };

            // Viền ô
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            // Chọn & resize
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            MultiSelect = false;
            AllowUserToResizeRows = false;
            RowHeadersVisible = false;
            RowTemplate.Height = 40;

            // Region bo góc
            Padding = new Padding(2);
            Resize += (_, __) => UpdateRegionRounded();
        }

        // ====== Public properties để tinh chỉnh nhanh ======
        [Category("Theming")] public Color BorderColor { get => _borderColor; set { _borderColor = value; Invalidate(); } }
        [Category("Theming")] public int BorderThickness { get => _borderThickness; set { _borderThickness = Math.Max(1, value); UpdateRegionRounded(); Invalidate(); } }
        [Category("Theming")] public int CornerRadius { get => _cornerRadius; set { _cornerRadius = Math.Max(0, value); UpdateRegionRounded(); Invalidate(); } }
        [Category("Theming")] public Color HeaderForeColor { get => _headerFore; set { _headerFore = value; ColumnHeadersDefaultCellStyle.ForeColor = value; Invalidate(); } }
        [Category("Theming")] public Color HeaderBackColor { get => _headerBack; set { _headerBack = value; ColumnHeadersDefaultCellStyle.BackColor = value; Invalidate(); } }
        [Category("Theming")] public Color RowBackColor { get => _rowBack; set { _rowBack = value; DefaultCellStyle.BackColor = value; Invalidate(); } }
        [Category("Theming")] public Color RowAltBackColor { get => _rowAltBack; set { _rowAltBack = value; AlternatingRowsDefaultCellStyle.BackColor = value; Invalidate(); } }
        [Category("Theming")] public Color RowForeColor { get => _rowFore; set { _rowFore = value; DefaultCellStyle.ForeColor = value; Invalidate(); } }
        [Category("Theming")] public Color HoverBackColor { get => _hoverBack; set { _hoverBack = value; Invalidate(); } }
        [Category("Theming")] public Color SelectionBackColor { get => _selectBack; set { _selectBack = value; DefaultCellStyle.SelectionBackColor = value; Invalidate(); } }
        [Category("Theming")] public Color SelectionForeColor { get => _selectFore; set { _selectFore = value; DefaultCellStyle.SelectionForeColor = value; Invalidate(); } }

        // ====== Hover effect từng dòng ======
        protected override void OnCellMouseEnter(DataGridViewCellEventArgs e) {
            base.OnCellMouseEnter(e);
            if (e.RowIndex >= 0 && e.RowIndex < Rows.Count) {
                // khôi phục hàng hover cũ nếu có
                RestoreRowBack(_hoverRowIndex);
                // set hover mới (nếu không đang selected)
                if (!Rows[e.RowIndex].Selected) {
                    Rows[e.RowIndex].DefaultCellStyle.BackColor = _hoverBack;
                }
                _hoverRowIndex = e.RowIndex;
            }
        }
        protected override void OnCellMouseLeave(DataGridViewCellEventArgs e) {
            base.OnCellMouseLeave(e);
            if (e.RowIndex == _hoverRowIndex) {
                RestoreRowBack(_hoverRowIndex);
                _hoverRowIndex = -1;
            }
        }
        private void RestoreRowBack(int rowIndex) {
            if (rowIndex < 0 || rowIndex >= Rows.Count) return;
            if (Rows[rowIndex].Selected) return; // giữ màu chọn
            Rows[rowIndex].DefaultCellStyle.BackColor = (rowIndex % 2 == 0) ? _rowBack : _rowAltBack;
        }

        // ====== Vẽ border + bo góc (outline giống SecondaryButton) ======
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Viền ngoài
            using var pen = new Pen(_borderColor, _borderThickness) { Alignment = PenAlignment.Inset };
            var r = ClientRectangle;
            r.Width -= 1; r.Height -= 1;
            using var path = GetRoundRect(r, _cornerRadius);
            e.Graphics.DrawPath(pen, path);
        }

        private void UpdateRegionRounded() {
            if (Width <= 0 || Height <= 0) return;
            var r = ClientRectangle;
            r.Inflate(-_borderThickness, -_borderThickness);
            using var path = GetRoundRect(r, _cornerRadius);
            Region?.Dispose();
            Region = new Region(path);
            Invalidate();
        }

        private static GraphicsPath GetRoundRect(Rectangle r, int radius) {
            int maxRad = Math.Min(r.Width, r.Height) / 2;
            int rad = Math.Max(0, Math.Min(radius, maxRad));
            int d = rad * 2;

            var path = new GraphicsPath();
            if (d <= 0) { path.AddRectangle(r); return path; }
            path.AddArc(r.Left, r.Top, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
