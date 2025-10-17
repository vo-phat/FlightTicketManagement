using System;
using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Tables;

namespace FlightTicketManagement.GUI.Features.Airport.SubFeatures {
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

        private void InitializeComponent() {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);
            lblTitle = new Label();
            // Filters
            filterLeft = new FlowLayoutPanel();
            txtCode = new UnderlinedTextField("Mã IATA", "");
            txtName = new UnderlinedTextField("Tên sân bay", "");
            txtCity = new UnderlinedTextField("Thành phố", "");
            cbCountry = new UnderlinedComboBox("Quốc gia", new object[] { "Tất cả", "Việt Nam", "Nhật Bản", "Hàn Quốc", "Singapore", "Thái Lan", "Hoa Kỳ", "Anh", "Pháp", "Úc", "Canada" });
            filterLeft.Controls.AddRange(new Control[] { txtCode, txtName, txtCity, cbCountry });
            filterRight = new FlowLayoutPanel();
            PrimaryButton btnSearch = new PrimaryButton("Tìm");
            filterRight.Controls.Add(btnSearch);
            filterWrap = new TableLayoutPanel();
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filterWrap.Controls.Add(filterLeft, 0, 0);
            filterWrap.Controls.Add(filterRight, 1, 0);
            // Table
            table = new TableCustom();
            table.Columns.Add("airportCode", "IATA");
            table.Columns.Add("airportName", "Tên sân bay");
            table.Columns.Add("city", "Thành phố");
            table.Columns.Add("country", "Quốc gia");
            table.Columns.Add("timezone", "Múi giờ");
            DataGridViewTextBoxColumn colAction = new DataGridViewTextBoxColumn();
            table.Columns.Add(colAction);
            DataGridViewTextBoxColumn colHiddenId = new DataGridViewTextBoxColumn();
            table.Columns.Add(colHiddenId);
            // demo rows
            table.Rows.Add("SGN", "Tân Sơn Nhất", "TP.HCM", "Việt Nam", "UTC+7", null, 1);
            table.Rows.Add("HAN", "Nội Bài", "Hà Nội", "Việt Nam", "UTC+7", null, 2);
            table.Rows.Add("NRT", "Narita", "Tokyo", "Nhật Bản", "UTC+9", null, 3);
            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;
            // Root
            root = new TableLayoutPanel();
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(filterWrap, 0, 1);
            root.Controls.Add(table, 0, 2);
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
            string tz = row.Cells["timezone"].Value?.ToString() ?? "(n/a)";

            if (r.rcView.Contains(p)) {
                using var frm = new AirportDetailForm(code, name, city, country, tz);
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
        public AirportDetailForm(string code, string name, string city, string country, string tz) {
            Text = $"Chi tiết sân bay {code}";
            Size = new Size(820, 520); BackColor = Color.White;
            var detail = new AirportDetailControl { Dock = DockStyle.Fill };
            detail.LoadAirport(code, name, city, country, tz);
            Controls.Add(detail);
        }
    }
}
