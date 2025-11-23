using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.Airline.SubFeatures;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Airline.SubFeatures {
    public class AirlineListControl : UserControl {
        private TableCustom table;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight;
        private Label lblTitle;
        private UnderlinedTextField txtCode, txtName;
        private UnderlinedComboBox cbCountry;

        public AirlineListControl() { 
            InitializeComponent(); 
        }

        private void InitializeComponent() {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label {
                Text = "🏢 Danh sách hãng hàng không",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // Filters
            filterLeft = new FlowLayoutPanel { 
                Dock = DockStyle.Fill, 
                AutoSize = true, 
                WrapContents = false 
            };
            txtCode = new UnderlinedTextField("Mã hãng", "") { 
                Width = 300, 
                Margin = new Padding(0, 0, 24, 0) 
            };
            txtName = new UnderlinedTextField("Tên hãng", "") { 
                Width = 300, 
                Margin = new Padding(0, 0, 24, 0) 
            };

            cbCountry = new UnderlinedComboBox("Quốc gia", new object[] { "Việt Nam", "Nhật Bản", "Hàn Quốc", "Singapore", "Thái Lan", "Hoa Kỳ", "Anh", "Pháp", "Úc", "Canada" }) {
                MinimumSize = new Size(0, 72),
                Width = 240,
                Margin = new Padding(0, 6, 24, 6)
            };

            filterLeft.Controls.AddRange(new Control[] { txtCode, txtName, cbCountry });

            filterRight = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            var btnSearch = new PrimaryButton("🔍 Tìm hãng") { Width = 120, Height = 36 };
            filterRight.Controls.Add(btnSearch);

            filterWrap = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(24, 16, 24, 0), ColumnCount = 2 };
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
                BorderStyle = BorderStyle.None
            };

            // Columns (Airlines)
            table.Columns.Add("airlineCode", "Mã");
            table.Columns.Add("airlineName", "Tên hãng");
            table.Columns.Add("country", "Quốc gia");
            table.Columns.Add("aircraftCount", "Số máy bay");

            var colAction = new DataGridViewTextBoxColumn { Name = ACTION_COL, HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells };
            table.Columns.Add(colAction);
            var colHiddenId = new DataGridViewTextBoxColumn { Name = "airlineIdHidden", Visible = false };
            table.Columns.Add(colHiddenId);

            // demo rows
            table.Rows.Add("VN", "Vietnam Airlines", "Việt Nam", 120, null, 1);
            table.Rows.Add("VJ", "VietJet Air", "Việt Nam", 80, null, 2);
            table.Rows.Add("QH", "Bamboo Airways", "Việt Nam", 35, null, 3);

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
        }

        // ===== Action column drawing (Xem / Sửa / Xóa) =====
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
            table.Cursor = (r.rcView.Contains(p) || r.rcEdit.Contains(p) || r.rcDel.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            string airlineId = row.Cells["airlineIdHidden"].Value?.ToString() ?? "";
            string code = row.Cells["airlineCode"].Value?.ToString() ?? "(n/a)";
            string name = row.Cells["airlineName"].Value?.ToString() ?? "(n/a)";
            string country = row.Cells["country"].Value?.ToString() ?? "(n/a)";
            string aircrafts = row.Cells["aircraftCount"].Value?.ToString() ?? "0";

            if (r.rcView.Contains(p)) {
                using (var frm = new AirlineDetailForm(code, name, country, aircrafts)) {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(FindForm());
                }
            } else if (r.rcEdit.Contains(p)) {
                MessageBox.Show($"Sửa hãng #{airlineId} ({code})", "Sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcDel.Contains(p)) {
                MessageBox.Show($"Xóa hãng #{airlineId} ({code})", "Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    internal class AirlineDetailForm : Form {
        public AirlineDetailForm(string code, string name, string country, string aircrafts) {
            Text = $"Chi tiết hãng {code}";
            Size = new Size(800, 520);
            BackColor = Color.White;

            var detail = new AirlineDetailControl { Dock = DockStyle.Fill };
            detail.LoadAirlineInfo(code, name, country, aircrafts);
            Controls.Add(detail);
        }
    }
}
