using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.CabinClass.SubFeatures {
    public class CabinClassListControl : UserControl {
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
        private UnderlinedComboBox cbTier;

        public CabinClassListControl() { InitializeComponent(); }

        private void InitializeComponent() {
            SuspendLayout();
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label {
                Text = "🛋 Danh sách Cabin Classes",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // Filters
            filterLeft = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false };
            txtCode = new UnderlinedTextField("Mã cabin", "") { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            txtName = new UnderlinedTextField("Tên cabin", "") { Width = 240, Margin = new Padding(0, 0, 24, 0) };
            cbTier = new UnderlinedComboBox("Thứ hạng", new object[] { "Tất cả", "Economy", "Premium Economy", "Business", "First" }) { Width = 220, Margin = new Padding(0, 0, 24, 0) };
            filterLeft.Controls.AddRange(new Control[] { txtCode, txtName, cbTier });

            filterRight = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            var btnSearch = new PrimaryButton("🔍 Tìm kiếm") { Width = 90, Height = 36 };
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
            table.Columns.Add("cabinCode", "Mã");
            table.Columns.Add("cabinName", "Tên cabin");
            table.Columns.Add("tier", "Thứ hạng");                // Economy/Business/...
            table.Columns.Add("priority", "Độ ưu tiên");          // số nhỏ hơn = ưu tiên cao
            table.Columns.Add("defaultBaggage", "Hành lý (kg)");  // mặc định
            table.Columns.Add("seatPitch", "Pitch (in)");         // tùy chọn
            var colAction = new DataGridViewTextBoxColumn { Name = ACTION_COL, HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells };
            table.Columns.Add(colAction);
            var colHiddenId = new DataGridViewTextBoxColumn { Name = "cabinIdHidden", Visible = false };
            table.Columns.Add(colHiddenId);

            // demo
            table.Rows.Add("Y", "Economy", "Economy", 3, 20, 31, null, 1);
            table.Rows.Add("W", "Premium Economy", "Premium Economy", 2, 25, 35, null, 2);
            table.Rows.Add("C", "Business", "Business", 1, 32, 40, null, 3);
            table.Rows.Add("F", "First", "First", 0, 40, 45, null, 4);

            // định dạng số
            table.CellFormatting += (s, e) => {
                if (e.RowIndex < 0) return;
                var name = table.Columns[e.ColumnIndex].Name;
                if ((name == "defaultBaggage" || name == "seatPitch" || name == "priority") && e.Value != null) {
                    if (decimal.TryParse(e.Value.ToString(), out var v)) { e.Value = v.ToString("#,0"); e.FormattingApplied = true; }
                }
            };

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

        // ===== Action links =====
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDel) GetRects(Rectangle bounds, Font font) {
            int pad = 6, x = bounds.Left + pad, y = bounds.Top + (bounds.Height - font.Height) / 2;
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
            Color link = Color.FromArgb(0, 92, 175), sep = Color.FromArgb(120, 120, 120), del = Color.FromArgb(220, 53, 69);
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
            string id = row.Cells["cabinIdHidden"].Value?.ToString() ?? "";
            string code = row.Cells["cabinCode"].Value?.ToString() ?? "(n/a)";
            string name = row.Cells["cabinName"].Value?.ToString() ?? "(n/a)";
            string tier = row.Cells["tier"].Value?.ToString() ?? "(n/a)";

            if (r.rcView.Contains(p)) {
                using var frm = new CabinClassDetailForm(code, name, tier,
                    row.Cells["priority"].Value?.ToString() ?? "0",
                    row.Cells["defaultBaggage"].Value?.ToString() ?? "0",
                    row.Cells["seatPitch"].Value?.ToString() ?? "0");
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(FindForm());
            } else if (r.rcEdit.Contains(p)) {
                MessageBox.Show($"Sửa cabin #{id} ({code})", "Sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcDel.Contains(p)) {
                MessageBox.Show($"Xóa cabin #{id} ({code})", "Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    internal class CabinClassDetailForm : Form {
        public CabinClassDetailForm(string code, string name, string tier, string priority, string baggage, string pitch) {
            Text = $"Chi tiết Cabin {code}";
            Size = new Size(820, 520); BackColor = Color.White;
            var detail = new CabinClassDetailControl { Dock = DockStyle.Fill };
            detail.LoadCabin(code, name, tier, priority, baggage, pitch);
            Controls.Add(detail);
        }
    }
}
