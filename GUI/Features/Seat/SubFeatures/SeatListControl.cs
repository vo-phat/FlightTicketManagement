using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Seat.SubFeatures
{
    public class SeatListControl : UserControl
    {
        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight;
        private Label lblTitle;

        private UnderlinedComboBox cbAircraft, cbClass;
        private UnderlinedTextField txtSeat;
        private PrimaryButton btnSearch;
        private SecondaryButton btnClear;

        private TableCustom table;
        private System.Windows.Forms.Timer debounce;

        private List<Row> datasource = new();

        public SeatListControl() { InitializeComponent(); SeedDemo(); ApplyFilter(); }

        private void InitializeComponent()
        {
            SuspendLayout();
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label
            {
                Text = "🪑 Danh sách ghế",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // Filters
            filterLeft = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false };
            cbAircraft = new UnderlinedComboBox("Máy bay", new object[] { "Tất cả", "A320", "B737" }) { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            cbClass = new UnderlinedComboBox("Hạng", new object[] { "Tất cả", "Economy", "Business" }) { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            txtSeat = new UnderlinedTextField("Số ghế (VD: 12A)", "") { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            filterLeft.Controls.AddRange(new Control[] { cbAircraft, cbClass, txtSeat });

            filterRight = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            btnSearch = new PrimaryButton("🔍 Tìm kiếm") { Width = 110, Height = 36 };
            btnClear = new SecondaryButton("⟲ Xóa lọc") { Width = 100, Height = 36, Margin = new Padding(12, 0, 0, 0) };
            filterRight.Controls.Add(btnSearch);
            filterRight.Controls.Add(btnClear);

            filterWrap = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(24, 16, 24, 0), ColumnCount = 2 };
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filterWrap.Controls.Add(filterLeft, 0, 0);
            filterWrap.Controls.Add(filterRight, 1, 0);

            // Table
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 24),
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            table.Columns.Add("seatNumber", "Số ghế");
            table.Columns.Add("className", "Hạng");
            table.Columns.Add("aircraft", "Máy bay");
            table.Columns.Add("createdAt", "Ngày tạo");
            var colAction = new DataGridViewTextBoxColumn { Name = ACTION_COL, HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells };
            table.Columns.Add(colAction);
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "seatIdHidden", Visible = false });

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // Events
            btnSearch.Click += (_, __) => ApplyFilter();
            btnClear.Click += (_, __) => { cbAircraft.SelectedIndex = 0; cbClass.SelectedIndex = 0; txtSeat.Text = ""; ApplyFilter(); };
            txtSeat.TextChanged += (_, __) => { debounce.Stop(); debounce.Start(); };

            debounce = new System.Windows.Forms.Timer { Interval = 280 };
            debounce.Tick += (_, __) => { debounce.Stop(); ApplyFilter(); };

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

        private void SeedDemo()
        {
            var rnd = new Random(1);
            foreach (var craft in new[] { "A320", "B737" })
            {
                for (int i = 1; i <= 40; i++)
                {
                    foreach (char c in new[] { 'A', 'B', 'C', 'D', 'E', 'F' })
                    {
                        datasource.Add(new Row
                        {
                            SeatId = i * 10 + (c - 'A'),
                            SeatNumber = $"{i}{c}",
                            ClassName = i <= 6 ? "Business" : "Economy",
                            Aircraft = craft,
                            CreatedAt = DateTime.Now.AddDays(-rnd.Next(180))
                        });
                    }
                }
            }
        }

        private void ApplyFilter()
        {
            string ac = cbAircraft.SelectedItem?.ToString() ?? "Tất cả";
            string cl = cbClass.SelectedItem?.ToString() ?? "Tất cả";
            string key = (txtSeat.Text ?? "").Trim().ToUpper();

            var q = datasource.AsEnumerable();
            if (ac != "Tất cả") q = q.Where(x => x.Aircraft == ac);
            if (cl != "Tất cả") q = q.Where(x => x.ClassName == cl);
            if (!string.IsNullOrEmpty(key)) q = q.Where(x => x.SeatNumber.Contains(key));

            table.Rows.Clear();
            foreach (var x in q)
            {
                table.Rows.Add(x.SeatNumber, x.ClassName, x.Aircraft, x.CreatedAt.ToString("yyyy-MM-dd"), null, x.SeatId);
            }
        }

        // ===== Action links drawing like CabinClassListControl =====
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDel) GetRects(Rectangle bounds, Font font)
        {
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

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e)
        {
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

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcEdit.Contains(p) || r.rcDel.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            var seatId = row.Cells["seatIdHidden"].Value?.ToString() ?? "";

            if (r.rcView.Contains(p))
            {
                MessageBox.Show($"Xem ghế #{seatId}", "Xem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (r.rcEdit.Contains(p))
            {
                MessageBox.Show($"Sửa ghế #{seatId}", "Sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (r.rcDel.Contains(p))
            {
                if (MessageBox.Show("Xóa ghế này?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    MessageBox.Show("Đã xóa (demo).");
            }
        }

        private class Row
        {
            public int SeatId { get; set; }
            public string SeatNumber { get; set; } = "";
            public string ClassName { get; set; } = "";
            public string Aircraft { get; set; } = "";
            public DateTime CreatedAt { get; set; }
        }
    }
}