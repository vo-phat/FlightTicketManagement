using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Tables;

namespace FlightTicketManagement.GUI.Features.Seat.SubFeatures {
    public class FlightSeatControl : UserControl {
        private const string ACTION_COL = "Action";
        private const string STATUS_COL = "status";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_BLOCK = "Chặn";
        private const string SEP = " / ";

        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight;
        private Label lblTitle;

        private UnderlinedComboBox cbFlight, cbClass;
        private UnderlinedTextField txtSeat;
        private PrimaryButton btnSearch; 
        private SecondaryButton btnClear;

        private TableCustom table;

        private List<Row> datasource = new();

        public FlightSeatControl() { InitializeComponent(); SeedDemo(); ApplyFilter(); }

        private void InitializeComponent() {
            SuspendLayout();
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label {
                Text = "✈️ Ghế theo chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // Filters
            filterLeft = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false };
            cbFlight = new UnderlinedComboBox("Chuyến bay", new object[] { "Tất cả", "VN001", "VN002", "VN003" }) { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            cbClass = new UnderlinedComboBox("Hạng", new object[] { "Tất cả", "Economy", "Business" }) { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            txtSeat = new UnderlinedTextField("Số ghế", "") { Width = 140, Margin = new Padding(0, 0, 24, 0) };
            filterLeft.Controls.AddRange(new Control[] { cbFlight, cbClass, txtSeat });

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
            table = new TableCustom {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 24),
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            table.Columns.Add("flight", "Chuyến bay");
            table.Columns.Add("seatNumber", "Số ghế");
            table.Columns.Add("className", "Hạng");
            table.Columns.Add("basePrice", "Giá cơ bản (₫)");
            table.Columns.Add(STATUS_COL, "Trạng thái");
            var colAction = new DataGridViewTextBoxColumn { Name = ACTION_COL, HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells };
            table.Columns.Add(colAction);

            table.CellFormatting += (s, e) => {
                if (e.RowIndex < 0) return;
                var name = table.Columns[e.ColumnIndex].Name;
                if (name == "basePrice" && e.Value != null && int.TryParse(e.Value.ToString(), out var v)) {
                    e.Value = v.ToString("#,0"); e.FormattingApplied = true;
                }
            };
            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // Events
            btnSearch.Click += (_, __) => ApplyFilter();
            btnClear.Click += (_, __) => { cbFlight.SelectedIndex = 0; cbClass.SelectedIndex = 0; txtSeat.Text = ""; ApplyFilter(); };

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

        private void SeedDemo() {
            foreach (var f in new[] { "VN001", "VN002" }) {
                for (int i = 1; i <= 30; i++) foreach (char c in new[] { 'A', 'B', 'C', 'D', 'E', 'F' }) {
                        var status = (i % 13 == 0) ? "BLOCKED" : ((i + c) % 5 == 0 ? "BOOKED" : "AVAILABLE");
                        datasource.Add(new Row {
                            Flight = f,
                            SeatNumber = $"{i}{c}",
                            ClassName = i <= 6 ? "Business" : "Economy",
                            BasePrice = i <= 6 ? 1_800_000 : 900_000,
                            Status = status
                        });
                    }
            }
        }

        private void ApplyFilter() {
            string fl = cbFlight.SelectedItem?.ToString() ?? "Tất cả";
            string cl = cbClass.SelectedItem?.ToString() ?? "Tất cả";
            string key = (txtSeat.Text ?? "").Trim().ToUpper();

            var q = datasource.AsEnumerable();
            if (fl != "Tất cả") q = q.Where(x => x.Flight == fl);
            if (cl != "Tất cả") q = q.Where(x => x.ClassName == cl);
            if (!string.IsNullOrEmpty(key)) q = q.Where(x => x.SeatNumber.Contains(key));

            table.Rows.Clear();
            foreach (var x in q) table.Rows.Add(x.Flight, x.SeatNumber, x.ClassName, x.BasePrice, x.Status, null);
        }

        // ===== Badges + action links =====
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcBlock) GetRects(Rectangle bounds, Font font) {
            int pad = 6, x = bounds.Left + pad, y = bounds.Top + (bounds.Height - font.Height) / 2;
            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szE = TextRenderer.MeasureText(TXT_EDIT, font, Size.Empty, flags);
            var szB = TextRenderer.MeasureText(TXT_BLOCK, font, Size.Empty, flags);
            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcE = new Rectangle(new Point(x, y), szE); x += szE.Width + szS.Width;
            var rcB = new Rectangle(new Point(x, y), szB);
            return (rcV, rcE, rcB);
        }

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return;
            var name = table.Columns[e.ColumnIndex].Name;

            if (name == STATUS_COL) {
                e.Handled = true;
                e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                var status = table.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
                var (bg, fg) = status switch {
                    "AVAILABLE" => (Color.FromArgb(220, 248, 225), Color.FromArgb(26, 115, 52)),
                    "BOOKED" => (Color.FromArgb(227, 230, 233), Color.FromArgb(66, 66, 66)),
                    _ => (Color.FromArgb(255, 230, 230), Color.FromArgb(179, 38, 30))
                };
                var r = new Rectangle(e.CellBounds.Left + 8, e.CellBounds.Top + 6, e.CellBounds.Width - 16, e.CellBounds.Height - 12);
                using var b = new SolidBrush(bg);
                e.Graphics.FillRectangle(b, r);
                TextRenderer.DrawText(e.Graphics, status, table.Font, r, fg,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            if (name == ACTION_COL) {
                e.Handled = true;
                e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                var font = e.CellStyle.Font ?? table.Font;
                var r = GetRects(e.CellBounds, font);
                Color link = Color.FromArgb(0, 92, 175), sep = Color.FromArgb(120, 120, 120), warn = Color.FromArgb(220, 53, 69);
                TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, r.rcView.Location, link, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, r.rcEdit.Location, link, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcEdit.Right, r.rcEdit.Top), sep, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, TXT_BLOCK, font, r.rcBlock.Location, warn, TextFormatFlags.NoPadding);
                table.Cursor = Cursors.Hand;
                return;
            }
        }

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcEdit.Contains(p) || r.rcBlock.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            string seat = row.Cells["seatNumber"].Value?.ToString() ?? "(n/a)";
            string flight = row.Cells["flight"].Value?.ToString() ?? "(n/a)";

            if (r.rcView.Contains(p)) {
                MessageBox.Show($"Xem ghế {seat} - {flight}", "Xem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcEdit.Contains(p)) {
                MessageBox.Show($"Sửa ghế {seat} - {flight}", "Sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcBlock.Contains(p)) {
                MessageBox.Show($"Chặn ghế {seat} - {flight}", "Chặn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private class Row {
            public string Flight { get; set; } = "";
            public string SeatNumber { get; set; } = "";
            public string ClassName { get; set; } = "";
            public int BasePrice { get; set; }
            public string Status { get; set; } = "";
        }
    }
}
