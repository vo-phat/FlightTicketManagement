using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Features.Airport.SubFeatures;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Airport.SubFeatures {
    public class AirportListControl : UserControl {
        private TableCustom table;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight;
        private Label lblTitle;
        private UnderlinedTextField txtCode, txtName, txtCity;
        private UnderlinedComboBox cbCountry;

        public AirportListControl() { InitializeComponent(); }

        private void InitializeComponent()
        {
            SuspendLayout();

            // ===== Root =====
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // ===== Title =====
            lblTitle = new Label
            {
                Text = "🏟️ Danh sách sân bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Filters =====
            filterLeft = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            txtCode = new UnderlinedTextField("Mã IATA", "") { 
                Width = 200, 
                Margin = new Padding(0, 0, 16, 0) 
            };
            txtName = new UnderlinedTextField("Tên sân bay", "") { 
                Width = 200, 
                Margin = new Padding(0, 0, 16, 0) 
            };
            txtCity = new UnderlinedTextField("Thành phố", "") { 
                Width = 200, 
                Margin = new Padding(0, 0, 16, 0) 
            };
            cbCountry = new UnderlinedComboBox("Quốc gia", new object[] { "Tất cả", "Việt Nam", "Nhật Bản", "Hàn Quốc", "Singapore", "Thái Lan", "Hoa Kỳ", "Anh", "Pháp", "Úc", "Canada" }) {
                Height = 64,
                Width = 200
            };
            filterLeft.Controls.AddRange(new Control[] { txtCode, txtName, txtCity, cbCountry });

            filterRight = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
            var btnSearch = new PrimaryButton("🔍 Tìm kiếm") { 
                Width = 100, Height = 36 
            };
            filterRight.Controls.Add(btnSearch);

            filterWrap = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 12, 24, 0),
                ColumnCount = 2,
                RowCount = 1
            };
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filterWrap.Controls.Add(filterLeft, 0, 0);
            filterWrap.Controls.Add(filterRight, 1, 0);

            // ===== Table =====
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 24),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Cột dữ liệu bám DB Airports (timezone là hiển thị/derive, DB bạn chưa có cột này)
            table.Columns.Add("airportCode", "IATA");         // Airports.airport_code
            table.Columns.Add("airportName", "Tên sân bay");  // Airports.airport_name
            table.Columns.Add("city", "Thành phố");           // Airports.city
            table.Columns.Add("country", "Quốc gia");         // Airports.country

            // Cột thao tác — NHỚ đặt Name khớp ACTION_COL
            var colAction = new DataGridViewTextBoxColumn
            {
                Name = ACTION_COL,
                HeaderText = "Thao tác",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            table.Columns.Add(colAction);

            // Cột ẩn ID — NHỚ Name để event truy cập được
            var colHiddenId = new DataGridViewTextBoxColumn
            {
                Name = "airportIdHidden",
                HeaderText = "",
                Visible = false
            };
            table.Columns.Add(colHiddenId);

            // Demo rows (có thể gỡ khi bind DB)
            table.Rows.Add("SGN", "Tân Sơn Nhất", "TP.HCM", "Việt Nam", null, 1);
            table.Rows.Add("HAN", "Nội Bài", "Hà Nội", "Việt Nam", null, 2);
            table.Rows.Add("NRT", "Narita", "Tokyo", "Nhật Bản", null, 3);

            // Events cho cột Thao tác
            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // ===== Root layout =====
            root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 3
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(filterWrap, 0, 1);
            root.Controls.Add(table, 0, 2);

            Controls.Clear();
            Controls.Add(root);

            ResumeLayout(false);
        }

        // ===== Action links =====
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDel) GetRects(Rectangle b, Font f) {
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
        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;
            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
            var f = e.CellStyle.Font ?? table.Font; var r = GetRects(e.CellBounds, f);
            Color link = Color.FromArgb(0, 92, 175), sep = Color.FromArgb(120, 120, 120), del = Color.FromArgb(220, 53, 69);
            TextRenderer.DrawText(e.Graphics, TXT_VIEW, f, r.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, f, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_EDIT, f, r.rcEdit.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, f, new Point(r.rcEdit.Right, r.rcEdit.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_DEL, f, r.rcDel.Location, del, TextFormatFlags.NoPadding);
        }
        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }
            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var f = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, f);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcEdit.Contains(p) || r.rcDel.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }
        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var f = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, f);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            string id = row.Cells["airportIdHidden"].Value?.ToString() ?? "";
            string code = row.Cells["airportCode"].Value?.ToString() ?? "(n/a)";
            string name = row.Cells["airportName"].Value?.ToString() ?? "(n/a)";
            string city = row.Cells["city"].Value?.ToString() ?? "(n/a)";
            string country = row.Cells["country"].Value?.ToString() ?? "(n/a)";

            if (r.rcView.Contains(p)) {
                using var frm = new AirportDetailForm(code, name, city, country);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(FindForm());
            } else if (r.rcEdit.Contains(p)) {
                MessageBox.Show($"Sửa sân bay #{id} ({code})", "Sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcDel.Contains(p)) {
                MessageBox.Show($"Xóa sân bay #{id} ({code})", "Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    internal class AirportDetailForm : Form {
        public AirportDetailForm(string code, string name, string city, string country) {
            Text = $"Chi tiết sân bay {code}";
            Size = new Size(820, 520); BackColor = Color.White;
            var detail = new AirportDetailControl { Dock = DockStyle.Fill };
            detail.LoadAirport(code, name, city, country);
            Controls.Add(detail);
        }
    }
}
