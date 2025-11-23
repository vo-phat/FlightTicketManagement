using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Seat.SubFeatures {
    public class FlightSeatControl : UserControl {
        private const string ACTION_COL = "Action";
        private const string TXT_EDIT_PRICE = "Giá";
        private const string TXT_BLOCK = "Chặn";
        private const string TXT_UNBLOCK = "Bỏ chặn";
        private const string SEP = " / ";

        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight;
        private Label lblTitle;
        private UnderlinedComboBox cbFlight, cbCabin, cbStatus;
        private UnderlinedTextField txtSeat;
        private PrimaryButton btnSearch, btnBulkPrice, btnBulkBlock;
        private TableCustom table;

        private List<Row> datasource = new();

        public FlightSeatControl() {
            InitializeComponent();
            SeedDemo();
            ApplyFilter();
        }

        private void InitializeComponent() {
            SuspendLayout();
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label {
                Text = "🧾 Ghế theo chuyến bay (Inventory)",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            filterLeft = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false };
            cbFlight = new UnderlinedComboBox("Chuyến bay", new object[] { "VN123 • 02/11 08:30", "VN456 • 05/11 18:30" }) { Width = 260, Margin = new Padding(0, 0, 24, 0) };
            cbCabin = new UnderlinedComboBox("Hạng", new object[] { "Tất cả", "Economy", "Business" }) { Width = 200, Margin = new Padding(0, 0, 24, 0) };
            cbStatus = new UnderlinedComboBox("Trạng thái", new object[] { "Tất cả", "AVAILABLE", "BOOKED", "BLOCKED" }) { Width = 200, Margin = new Padding(0, 0, 24, 0) };
            txtSeat = new UnderlinedTextField("Số ghế", "") { Width = 120, Margin = new Padding(0, 0, 24, 0) };
            filterLeft.Controls.AddRange(new Control[] { cbFlight, cbCabin, cbStatus, txtSeat });

            filterRight = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            btnSearch = new PrimaryButton("🔍 Lọc") { Width = 90, Height = 36 };
            btnBulkPrice = new PrimaryButton("💲 Set giá theo hạng") { Width = 170, Height = 36, Margin = new Padding(12, 0, 0, 0) };
            btnBulkBlock = new PrimaryButton("🚫 Block hàng loạt") { Width = 160, Height = 36, Margin = new Padding(12, 0, 0, 0) };
            filterRight.Controls.Add(btnSearch);
            filterRight.Controls.Add(btnBulkPrice);
            filterRight.Controls.Add(btnBulkBlock);

            filterWrap = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(24, 16, 24, 0), ColumnCount = 2 };
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filterWrap.Controls.Add(filterLeft, 0, 0);
            filterWrap.Controls.Add(filterRight, 1, 0);

            table = new TableCustom {
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
            table.Columns.Add("basePrice", "Giá cơ bản");
            table.Columns.Add("status", "Trạng thái");
            var colAction = new DataGridViewTextBoxColumn { Name = ACTION_COL, HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells };
            var colHidden = new DataGridViewTextBoxColumn { Name = "flightSeatIdHidden", Visible = false };
            table.Columns.Add(colAction);
            table.Columns.Add(colHidden);

            table.CellFormatting += (s, e) => {
                if (e.RowIndex < 0) return;
                var col = table.Columns[e.ColumnIndex].Name;
                if (col == "basePrice" && e.Value != null && decimal.TryParse(e.Value.ToString(), out var v)) {
                    e.Value = v.ToString("#,0");
                    e.FormattingApplied = true;
                }
            };

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            btnSearch.Click += (_, __) => ApplyFilter();
            btnBulkPrice.Click += (_, __) => MessageBox.Show("[DEMO] Set giá theo cabin.", "Bulk price");
            btnBulkBlock.Click += (_, __) => MessageBox.Show("[DEMO] Block dải ghế.", "Bulk block");

            var legend = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, WrapContents = false, Padding = new Padding(24, 8, 24, 0) };
            legend.Controls.Add(MakeLegend("AVAILABLE", Color.FromArgb(76, 175, 80)));
            legend.Controls.Add(MakeLegend("BOOKED", Color.FromArgb(158, 158, 158)));
            legend.Controls.Add(MakeLegend("BLOCKED", Color.FromArgb(244, 67, 54)));

            root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, BackColor = Color.Transparent };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(filterWrap, 0, 1);
            root.Controls.Add(legend, 0, 2);
            root.Controls.Add(table, 0, 3);

            Controls.Add(root);
            ResumeLayout(false);
        }

        private Control MakeLegend(string text, Color color) {
            var panel = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0, 0, 16, 0) };
            panel.Controls.Add(new Panel { Width = 16, Height = 16, BackColor = color, Margin = new Padding(6, 8, 6, 0) });
            panel.Controls.Add(new Label { Text = text, AutoSize = true, Margin = new Padding(0, 6, 0, 0) });
            return panel;
        }

        private void SeedDemo() {
            datasource = new List<Row>();
            var rnd = new Random(7);
            foreach (var row in Enumerable.Range(1, 28)) {
                foreach (var col in new[] { 'A', 'B', 'C', 'D', 'E', 'F' }) {
                    var cabin = row <= 4 ? "Business" : "Economy";
                    var status = (row % 9 == 0) ? "BLOCKED" : ((row % 7 == 0) ? "BOOKED" : "AVAILABLE");
                    datasource.Add(new Row {
                        FlightSeatId = row * 10 + (col - 'A'),
                        SeatNumber = $"{row}{col}",
                        ClassName = cabin,
                        BasePrice = cabin == "Business" ? 3200000 : 1200000,
                        Status = status
                    });
                }
            }
        }

        private void ApplyFilter() {
            string cabin = cbCabin.SelectedItem?.ToString() ?? "Tất cả";
            string status = cbStatus.SelectedItem?.ToString() ?? "Tất cả";
            string kw = (txtSeat.Text ?? "").Trim().ToUpperInvariant();

            IEnumerable<Row> q = datasource;
            if (cabin != "Tất cả") q = q.Where(x => x.ClassName == cabin);
            if (status != "Tất cả") q = q.Where(x => x.Status == status);
            if (!string.IsNullOrEmpty(kw)) q = q.Where(x => x.SeatNumber.ToUpperInvariant().Contains(kw));

            table.Rows.Clear();
            foreach (var it in q) {
                table.Rows.Add(it.SeatNumber, it.ClassName, it.BasePrice, it.Status, null, it.FlightSeatId);
            }
        }

        // === action col ===
        private (Rectangle rcEdit, Rectangle rcToggle) GetRects(Rectangle bounds, Font font, string toggleText) {
            int pad = 6, x = bounds.Left + pad, y = bounds.Top + (bounds.Height - font.Height) / 2;
            var flags = TextFormatFlags.NoPadding;
            var szE = TextRenderer.MeasureText(TXT_EDIT_PRICE, font, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szT = TextRenderer.MeasureText(toggleText, font, Size.Empty, flags);
            var rcE = new Rectangle(new Point(x, y), szE); x += szE.Width + szS.Width;
            var rcT = new Rectangle(new Point(x, y), szT);
            return (rcE, rcT);
        }

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var st = table.Rows[e.RowIndex].Cells["status"].Value?.ToString() ?? "AVAILABLE";
            var toggle = st == "BLOCKED" ? TXT_UNBLOCK : TXT_BLOCK;

            var font = e.CellStyle.Font ?? table.Font;
            var r = GetRects(e.CellBounds, font, toggle);
            var link = Color.FromArgb(0, 92, 175);
            var sep = Color.FromArgb(120, 120, 120);

            TextRenderer.DrawText(e.Graphics, TXT_EDIT_PRICE, font, r.rcEdit.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcEdit.Right, r.rcEdit.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, toggle, font, r.rcToggle.Location, link, TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }

            var st = table.Rows[e.RowIndex].Cells["status"].Value?.ToString() ?? "AVAILABLE";
            var toggle = st == "BLOCKED" ? TXT_UNBLOCK : TXT_BLOCK;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font, toggle);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcEdit.Contains(p) || r.rcToggle.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var st = table.Rows[e.RowIndex].Cells["status"].Value?.ToString() ?? "AVAILABLE";
            var toggle = st == "BLOCKED" ? TXT_UNBLOCK : TXT_BLOCK;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font, toggle);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            var seatId = row.Cells["flightSeatIdHidden"].Value?.ToString() ?? "";
            var seatNo = row.Cells["seatNumber"].Value?.ToString() ?? "";

            if (r.rcEdit.Contains(p)) {
                MessageBox.Show($"[DEMO] Sửa giá ghế {seatNo}", "Set price", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcToggle.Contains(p)) {
                if (st == "BLOCKED") {
                    row.Cells["status"].Value = "AVAILABLE";
                    MessageBox.Show($"[DEMO] Bỏ chặn ghế {seatNo}", "Unblock");
                } else {
                    row.Cells["status"].Value = "BLOCKED";
                    MessageBox.Show($"[DEMO] Chặn ghế {seatNo}", "Block");
                }
            }
        }

        private class Row {
            public int FlightSeatId { get; set; }
            public string SeatNumber { get; set; } = "";
            public string ClassName { get; set; } = "";
            public decimal BasePrice { get; set; }
            public string Status { get; set; } = "AVAILABLE";
        }
    }
}
