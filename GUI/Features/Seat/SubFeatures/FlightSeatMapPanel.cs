using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FlightTicketManagement.GUI.Features.Seat.SubFeatures {
    public class FlightSeatMapPanel : Panel {
        public int Rows { get; set; } = 30;
        public string Pattern { get; set; } = "ABCDEF";
        public List<SeatVM> Seats { get; } = new();

        public int CellW { get; set; } = 34;
        public int CellH { get; set; } = 34;
        public int GapX { get; set; } = 8;
        public int GapY { get; set; } = 6;
        public int MarginLeft { get; set; } = 48;
        public int MarginTop { get; set; } = 24;

        private readonly ToolTip _tip = new ToolTip();
        public event Action<SeatVM, Keys>? SeatClicked;

        public FlightSeatMapPanel() {
            DoubleBuffered = true;
            BackColor = Color.White;
            ResizeRedraw = true;
            MouseMove += OnMouseMove;
            MouseClick += OnMouseClick;
        }

        private void OnMouseClick(object? s, MouseEventArgs e) {
            var seat = HitSeat(e.Location);
            if (seat != null) SeatClicked?.Invoke(seat, Control.ModifierKeys);
        }

        private void OnMouseMove(object? s, MouseEventArgs e) {
            var seat = HitSeat(e.Location);
            if (seat == null) { _tip.SetToolTip(this, ""); return; }
            var st = seat.Status.ToString().ToUpper();
            var price = seat.BasePrice > 0 ? seat.BasePrice.ToString("#,0 ₫") : "—";
            _tip.SetToolTip(this, $"Ghế {seat}\n{seat.Cabin} • {seat.FareCode}\nGiá: {price}\nTrạng thái: {st}");
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
                    var seat = Seats.FirstOrDefault(x => x.Row == r && x.Col == col);
                    var rect = new Rectangle(MarginLeft + c * (CellW + GapX), MarginTop + (r - 1) * (CellH + GapY), CellW, CellH);

                    Color bg = Color.FromArgb(245, 247, 250);
                    Color border = Color.FromArgb(200, 205, 210);
                    Color fg = Color.FromArgb(60, 60, 60);
                    if (seat != null) {
                        switch (seat.Status) {
                            case SeatStatus.Booked: bg = Color.FromArgb(220, 235, 255); border = Color.FromArgb(0, 92, 175); fg = Color.FromArgb(0, 92, 175); break;
                            case SeatStatus.Blocked: bg = Color.FromArgb(255, 235, 238); border = Color.FromArgb(214, 47, 61); fg = Color.FromArgb(183, 28, 28); break;
                        }
                    }

                    using (var b = new SolidBrush(bg)) g.FillRectangle(b, rect);
                    using (var p = new Pen(border)) g.DrawRectangle(p, rect);
                    TextRenderer.DrawText(g, $"{r}{col}", new Font("Segoe UI", 8.5f, FontStyle.Bold),
                        rect, fg, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
        }

        private SeatVM? HitSeat(Point p) {
            for (int r = 1; r <= Rows; r++)
                for (int c = 0; c < Pattern.Length; c++) {
                    var rect = new Rectangle(MarginLeft + c * (CellW + GapX), MarginTop + (r - 1) * (CellH + GapY), CellW, CellH);
                    if (rect.Contains(p)) return Seats.FirstOrDefault(x => x.Row == r && x.Col == Pattern[c].ToString());
                }
            return null;
        }

        public void BindSeats(IEnumerable<SeatVM> data, int rows, string pattern) {
            Seats.Clear();
            Seats.AddRange(data);
            Rows = rows;
            Pattern = pattern;
            Invalidate();
        }
    }
}
