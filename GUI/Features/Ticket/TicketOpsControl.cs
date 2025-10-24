using System;
using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Tables;

namespace FlightTicketManagement.GUI.Features.Ticket {
    public class TicketOpsControl : UserControl {
        private TableCustom table;
        private Label lblTitle;
        private UnderlinedTextField txtBookingCode, txtFlightNo, txtPassenger;
        private UnderlinedComboBox cboStatus;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_CHECKIN = "Check-in";
        private const string TXT_VOID = "Hủy vé";
        private const string SEP = " / ";

        public TicketOpsControl() { Initialize(); }

        private void Initialize() {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label {
                Text = "🧾 Quản lý vé (check-in / đổi trạng thái)",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            var left = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false, FlowDirection = FlowDirection.LeftToRight };
            txtBookingCode = new UnderlinedTextField("Mã đặt chỗ", "") { Width = 150, Margin = new Padding(0, 0, 24, 0) };
            txtFlightNo = new UnderlinedTextField("Mã chuyến bay", "") { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            txtPassenger = new UnderlinedTextField("Hành khách", "") { Width = 200, Margin = new Padding(0, 0, 24, 0) };
            cboStatus = new UnderlinedComboBox("Trạng thái", new object[] { "ALL", "BOOKED", "CHECKED_IN", "CANCELLED", "USED" }) { Width = 150, Margin = new Padding(0, 0, 24, 0) };
            cboStatus.SelectedIndex = 0;
            left.Controls.AddRange(new Control[] { txtBookingCode, txtFlightNo, txtPassenger, cboStatus });

            var right = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            var btnSearch = new PrimaryButton("🔍 Lọc") { Width = 120, Height = 36 };
            right.Controls.Add(btnSearch);

            var filters = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(24, 10, 24, 0), ColumnCount = 2 };
            filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            filters.Controls.Add(left, 0, 0);
            filters.Controls.Add(right, 1, 0);

            table = new TableCustom {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 24),
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "ticketId", HeaderText = "Mã vé" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "bookingCode", HeaderText = "Mã đặt chỗ" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "flight", HeaderText = "Chuyến bay" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "seatNo", HeaderText = "Ghế" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "class", HeaderText = "Hạng" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "fare", HeaderText = "Giá" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "status", HeaderText = "Trạng thái" });
            table.Columns.Add(new DataGridViewTextBoxColumn {
                Name = ACTION_COL,
                HeaderText = "Thao tác",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 220,
                MinimumWidth = 200
            });

            // demo
            table.Rows.Add(91001, "BK-7N3Q2", "VN123 SGN→HAN", "12A", "Economy", "1,200,000", "BOOKED", null);
            table.Rows.Add(91002, "BK-7N3Q2", "VN123 SGN→HAN", "12B", "Economy", "1,200,000", "BOOKED", null);
            table.Rows.Add(92011, "BK-1A9C5", "VN456 DAD→HAN", "2C", "Business", "2,100,000", "CHECKED_IN", null);

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(filters, 0, 1);
            main.Controls.Add(table, 0, 2);

            Controls.Clear();
            Controls.Add(main);
            ResumeLayout(false);
        }

        // Action rendering
        private (Rectangle rcView, Rectangle rcCheckin, Rectangle rcVoid) GetRects(Rectangle b, Font f) {
            int pad = 6; int x = b.Left + pad; int y = b.Top + (b.Height - f.Height) / 2;
            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText(TXT_VIEW, f, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, f, Size.Empty, flags);
            var szCk = TextRenderer.MeasureText(TXT_CHECKIN, f, Size.Empty, flags);
            var szVo = TextRenderer.MeasureText(TXT_VOID, f, Size.Empty, flags);
            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcC = new Rectangle(new Point(x, y), szCk); x += szCk.Width + szS.Width;
            var rcX = new Rectangle(new Point(x, y), szVo);
            return (rcV, rcC, rcX);
        }
        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return; if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;
            e.Handled = true; e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
            var f = e.CellStyle.Font ?? table.Font; var r = GetRects(e.CellBounds, f);
            var link = Color.FromArgb(0, 92, 175); var sep = Color.FromArgb(120, 120, 120);
            TextRenderer.DrawText(e.Graphics, TXT_VIEW, f, r.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, f, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_CHECKIN, f, r.rcCheckin.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, f, new Point(r.rcCheckin.Right, r.rcCheckin.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_VOID, f, r.rcVoid.Location, Color.FromArgb(220, 53, 69), TextFormatFlags.NoPadding);
        }
        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }
            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var f = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, f);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcCheckin.Contains(p) || r.rcVoid.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }
        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var f = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, f); var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            var row = table.Rows[e.RowIndex];

            if (r.rcView.Contains(p)) {
                MessageBox.Show($"Vé #{row.Cells["ticketId"].Value}\nBooking: {row.Cells["bookingCode"].Value}\nGhế: {row.Cells["seatNo"].Value}", "Chi tiết vé");
            } else if (r.rcCheckin.Contains(p)) {
                // TODO: UPDATE Tickets.status='CHECKED_IN'
                MessageBox.Show($"Check-in vé #{row.Cells["ticketId"].Value} (demo)", "Check-in", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcVoid.Contains(p)) {
                // TODO: UPDATE Tickets.status='CANCELLED' (quy tắc hoàn/hủy)
                MessageBox.Show($"Hủy vé #{row.Cells["ticketId"].Value} (demo)", "Hủy vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
