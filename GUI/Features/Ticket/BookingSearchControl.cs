using System;
using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Tables;

namespace FlightTicketManagement.GUI.Features.Ticket {
    public class BookingSearchControl : UserControl {
        private TableCustom table;
        private Label lblTitle;
        private UnderlinedTextField txtBookingCode, txtPassengerPhone, txtPassengerEmail;
        private DateTimePickerCustom dtDeparture;
        private UnderlinedComboBox cboStatus;
        private UnderlinedTextField quickEmail, quickPhone, quickSeats;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_ISSUE = "Xuất vé";
        private const string TXT_CANCEL = "Hủy";
        private const string SEP = " / ";

        public BookingSearchControl() { Initialize(); }

        private void Initialize() {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // Title
            lblTitle = new Label {
                Text = "🎟 Tạo / Tìm đặt chỗ",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // Filters
            var left = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight
            };
            txtBookingCode = new UnderlinedTextField("Mã đặt chỗ", "") { Width = 150, Margin = new Padding(0, 0, 24, 0) };
            txtPassengerPhone = new UnderlinedTextField("SĐT hành khách", "") { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            txtPassengerEmail = new UnderlinedTextField("Email hành khách", "") { Width = 220, Margin = new Padding(0, 0, 24, 0) };
            dtDeparture = new DateTimePickerCustom("Ngày bay", "") { Width = 170, Margin = new Padding(0, 0, 24, 0) };

            cboStatus = new UnderlinedComboBox("Trạng thái", new object[] { "ALL", "NEW", "ISSUED", "CANCELLED" }) { Width = 150, Margin = new Padding(0, 0, 24, 0) };
            cboStatus.SelectedIndex = 0;

            left.Controls.AddRange(new Control[] { txtBookingCode, txtPassengerPhone, txtPassengerEmail, dtDeparture, cboStatus });

            var right = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
            var btnSearch = new PrimaryButton("🔍 Lọc") { Width = 120, Height = 36 };
            right.Controls.Add(btnSearch);

            var filterWrap = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 10, 24, 0),
                ColumnCount = 2
            };
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            filterWrap.Controls.Add(left, 0, 0);
            filterWrap.Controls.Add(right, 1, 0);

            // Quick create booking
            var quickWrap = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 8, 24, 0),
                ColumnCount = 5
            };
            quickWrap.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            quickWrap.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            quickWrap.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            quickWrap.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            quickWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            quickEmail = new UnderlinedTextField("Email KH", "") { Width = 220, Margin = new Padding(16, 0, 16, 0) };
            quickPhone = new UnderlinedTextField("SĐT KH", "") { Width = 160, Margin = new Padding(0, 0, 16, 0) };
            quickSeats = new UnderlinedTextField("Số ghế", "") { Width = 100, Margin = new Padding(0, 0, 16, 0) };

            var btnCreate = new PrimaryButton("➕ Tạo đặt chỗ nhanh") { Width = 190, Height = 36 };
            btnCreate.Click += (_, __) => {
                // TODO: INSERT Bookings(booking_code, account_id?, flight_id?, total_amount, status='NEW', created_at=NOW())
                MessageBox.Show("Đã tạo đặt chỗ (demo).", "Booking", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            quickWrap.Controls.Add(new Label { AutoSize = true, Margin = new Padding(0, 10, 0, 0) }, 0, 0);
            quickWrap.Controls.Add(quickEmail, 1, 0);
            quickWrap.Controls.Add(quickPhone, 2, 0);
            quickWrap.Controls.Add(quickSeats, 3, 0);
            quickWrap.Controls.Add(btnCreate, 4, 0);

            // Table
            table = new TableCustom {
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
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "bookingId", HeaderText = "ID" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "bookingCode", HeaderText = "Mã đặt chỗ" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "passenger", HeaderText = "Hành khách" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "flight", HeaderText = "Chuyến bay" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "amount", HeaderText = "Tổng tiền" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "status", HeaderText = "Trạng thái" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "createdAt", HeaderText = "Ngày tạo" });

            var colAction = new DataGridViewTextBoxColumn {
                Name = ACTION_COL,
                HeaderText = "Thao tác",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 220,
                MinimumWidth = 200
            };
            table.Columns.Add(colAction);

            // demo
            table.Rows.Add(5101, "BK-7N3Q2", "Nguyễn Văn A", "VN123 SGN→HAN", "3,500,000", "NEW", DateTime.Now.AddMinutes(-20).ToString("dd/MM HH:mm"), null);
            table.Rows.Add(5102, "BK-1A9C5", "Trần B", "VN456 DAD→HAN", "2,100,000", "ISSUED", DateTime.Now.AddHours(-2).ToString("dd/MM HH:mm"), null);

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // Main
            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, BackColor = Color.Transparent };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(filterWrap, 0, 1);
            main.Controls.Add(quickWrap, 0, 2);
            main.Controls.Add(table, 0, 3);

            Controls.Clear();
            Controls.Add(main);
            ResumeLayout(false);
        }

        // ==== Action column ====
        private (Rectangle rcView, Rectangle rcIssue, Rectangle rcCancel) GetRects(Rectangle cellBounds, Font font) {
            int pad = 6; int x = cellBounds.Left + pad; int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;
            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szI = TextRenderer.MeasureText(TXT_ISSUE, font, Size.Empty, flags);
            var szC = TextRenderer.MeasureText(TXT_CANCEL, font, Size.Empty, flags);
            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcI = new Rectangle(new Point(x, y), szI); x += szI.Width + szS.Width;
            var rcC = new Rectangle(new Point(x, y), szC);
            return (rcV, rcI, rcC);
        }
        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;
            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
            var font = e.CellStyle.Font ?? table.Font;
            var r = GetRects(e.CellBounds, font);
            var link = Color.FromArgb(0, 92, 175); var sep = Color.FromArgb(120, 120, 120);
            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, r.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_ISSUE, font, r.rcIssue.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcIssue.Right, r.rcIssue.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_CANCEL, font, r.rcCancel.Location, Color.FromArgb(220, 53, 69), TextFormatFlags.NoPadding);
        }
        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }
            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font); var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcIssue.Contains(p) || r.rcCancel.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }
        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font); var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            var code = Convert.ToString(row.Cells["bookingCode"].Value) ?? "";

            if (r.rcView.Contains(p)) {
                using (var frm = new BookingDetailForm(row)) { frm.StartPosition = FormStartPosition.CenterParent; frm.ShowDialog(FindForm()); }
            } else if (r.rcIssue.Contains(p)) {
                // TODO: tạo Tickets cho booking này + cập nhật Bookings.status='ISSUED'
                MessageBox.Show($"Xuất vé cho booking {code} (demo)", "Issue", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcCancel.Contains(p)) {
                // TODO: hủy booking + cập nhật Bookings.status='CANCELLED'
                MessageBox.Show($"Hủy booking {code} (demo)", "Cancel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    internal class BookingDetailForm : Form {
        public BookingDetailForm(DataGridViewRow src) {
            Text = $"Chi tiết đặt chỗ {Convert.ToString(src.Cells["bookingCode"].Value)}";
            Size = new Size(900, 560); BackColor = Color.White;

            var title = new Label {
                Text = "📄 Thông tin đặt chỗ",
                AutoSize = true,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Padding = new Padding(24, 18, 24, 0),
                Dock = DockStyle.Top
            };

            var grid = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(24, 8, 24, 0) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            void Row(string k, string? v) {
                int r = grid.RowCount;
                grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                grid.Controls.Add(new Label { Text = k, AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), Margin = new Padding(0, 6, 12, 6) }, 0, r);
                grid.Controls.Add(new Label { Text = v ?? "", AutoSize = true, Font = new Font("Segoe UI", 10), Margin = new Padding(0, 6, 0, 6) }, 1, r);
                grid.RowCount = r + 1;
            }

            Row("Mã đặt chỗ:", Convert.ToString(src.Cells["bookingCode"].Value));
            Row("Hành khách:", Convert.ToString(src.Cells["passenger"].Value));
            Row("Chuyến bay:", Convert.ToString(src.Cells["flight"].Value));
            Row("Tổng tiền:", Convert.ToString(src.Cells["amount"].Value));
            Row("Trạng thái:", Convert.ToString(src.Cells["status"].Value));
            Row("Ngày tạo:", Convert.ToString(src.Cells["createdAt"].Value));

            // bảng vé (mock)
            var tickets = new TableCustom {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 24),
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            tickets.Columns.Add("ticketId", "Mã vé");
            tickets.Columns.Add("seatNo", "Số ghế");
            tickets.Columns.Add("class", "Hạng");
            tickets.Columns.Add("fare", "Giá");
            tickets.Columns.Add("status", "Trạng thái");
            tickets.Rows.Add(90011, "12A", "Economy", "1,200,000", "BOOKED");
            tickets.Rows.Add(90012, "12B", "Economy", "1,200,000", "BOOKED");

            var pnl = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            pnl.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pnl.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pnl.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            pnl.Controls.Add(title, 0, 0);
            pnl.Controls.Add(grid, 0, 1);
            pnl.Controls.Add(tickets, 0, 2);

            Controls.Add(pnl);
        }
    }
}
