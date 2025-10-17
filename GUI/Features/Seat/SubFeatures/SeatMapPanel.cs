using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FlightTicketManagement.GUI.Features.Seat.SubFeatures {
    public class SeatMapPanel : Panel {
        public readonly struct SeatKey : IEquatable<SeatKey> {
            public readonly int Row;
            public readonly string Col;
            public SeatKey(int row, string col) { Row = row; Col = col; }
            public bool Equals(SeatKey other) => Row == other.Row && Col == other.Col;
            public override int GetHashCode() => HashCode.Combine(Row, Col);
            public override string ToString() => $"{Row}{Col}";
        }

        public int Rows { get; set; } = 30;
        public string Pattern { get; set; } = "ABCDEF";
        public HashSet<SeatKey> Existing { get; } = new();
        public HashSet<SeatKey> Pending { get; } = new();

        public int CellW { get; set; } = 34;
        public int CellH { get; set; } = 34;
        public int GapX { get; set; } = 8;
        public int GapY { get; set; } = 6;
        public int MarginLeft { get; set; } = 48;
        public int MarginTop { get; set; } = 24;

        private readonly ToolTip _tip = new ToolTip();

        public SeatMapPanel() {
            DoubleBuffered = true;
            BackColor = Color.White;
            ResizeRedraw = true;
            MouseMove += OnMouseMove;
        }

        private void OnMouseMove(object? s, MouseEventArgs e) {
            var hit = HitTest(e.Location);
            if (hit == null) { _tip.SetToolTip(this, ""); return; }
            var (r, c) = hit.Value;
            var key = new SeatKey(r, c);
            string st = Existing.Contains(key) ? "ĐÃ TỒN TẠI" :
                        Pending.Contains(key) ? "MỚI THÊM" : "TRỐNG";
            _tip.SetToolTip(this, $"Ghế {r}{c} ({st})");
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            for (int c = 0; c < Pattern.Length; c++) {
                var x = MarginLeft + c * (CellW + GapX);
                TextRenderer.DrawText(g, Pattern[c].ToString(), Font, new Point(x + (CellW / 2 - 6), 0), Color.FromArgb(80, 80, 80));
            }

            for (int r = 1; r <= Rows; r++) {
                TextRenderer.DrawText(g, r.ToString(), Font, new Point(6, MarginTop + (r - 1) * (CellH + GapY) + (CellH / 2 - 8)), Color.FromArgb(80, 80, 80));
                for (int c = 0; c < Pattern.Length; c++) {
                    var col = Pattern[c].ToString();
                    var key = new SeatKey(r, col);
                    var rect = CellRect(r, c);

                    Color bg = Color.FromArgb(245, 247, 250);
                    Color border = Color.FromArgb(200, 205, 210);
                    if (Existing.Contains(key)) { bg = Color.FromArgb(220, 235, 255); border = Color.FromArgb(0, 92, 175); } else if (Pending.Contains(key)) { bg = Color.FromArgb(219, 242, 228); border = Color.FromArgb(1, 135, 89); }

                    using (var b = new SolidBrush(bg)) g.FillRectangle(b, rect);
                    using (var p = new Pen(border)) g.DrawRectangle(p, rect);
                    TextRenderer.DrawText(g, $"{r}{col}", new Font("Segoe UI", 8.5f, FontStyle.Bold),
                        rect, Color.FromArgb(60, 60, 60), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
        }

        private Rectangle CellRect(int r, int c) =>
            new Rectangle(MarginLeft + c * (CellW + GapX), MarginTop + (r - 1) * (CellH + GapY), CellW, CellH);

        public (int row, string col)? HitTest(Point p) {
            for (int r = 1; r <= Rows; r++)
                for (int c = 0; c < Pattern.Length; c++)
                    if (CellRect(r, c).Contains(p))
                        return (r, Pattern[c].ToString());
            return null;
        }

        public void SetExisting(IEnumerable<SeatKey> seats) { Existing.Clear(); foreach (var s in seats) Existing.Add(s); Invalidate(); }
        public void SetPending(IEnumerable<SeatKey> seats) { Pending.Clear(); foreach (var s in seats) Pending.Add(s); Invalidate(); }
    }
}
