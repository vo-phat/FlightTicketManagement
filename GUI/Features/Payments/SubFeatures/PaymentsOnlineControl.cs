using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.Payments;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Payments.SubFeatures {
    public class PaymentsOnlineControl : UserControl {
        private TableCustom table;
        private Label lblTitle;

        // bộ lọc
        private UnderlinedTextField txtBookingId, txtTxnRef;
        private UnderlinedComboBox cboMethod, cboStatus; // ✅ thay ComboBox mặc định
        private DateTimePickerCustom dtFrom, dtTo;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_CAPTURE = "Xác nhận";
        private const string TXT_REFUND = "Hoàn";
        private const string SEP = " / ";

        public PaymentsOnlineControl() { InitializeComponent(); }

        private void InitializeComponent() {
            SuspendLayout();

            // ===== Root =====
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // ===== Title =====
            lblTitle = new Label {
                Text = "🌐 Thanh toán Online (Credit Card / Bank / E-Wallet)",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Filters =====
            var filterLeft = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight
            };

            txtBookingId = new UnderlinedTextField("Booking ID", "") { Width = 150, Margin = new Padding(0, 0, 24, 0) };
            txtTxnRef = new UnderlinedTextField("Mã tham chiếu (gateway)", "") { Width = 220, Margin = new Padding(0, 0, 24, 0) };

            cboMethod = new UnderlinedComboBox("Phương thức", new object[] { "ALL", "CREDIT_CARD", "BANK_TRANSFER", "E_WALLET" }) { Width = 170, Margin = new Padding(0, 0, 24, 0) };
            cboMethod.SelectedIndex = 0;

            cboStatus = new UnderlinedComboBox("Trạng thái", new object[] { "ALL", "PENDING", "SUCCESS", "FAILED" }) { Width = 150, Margin = new Padding(0, 0, 24, 0) };
            cboStatus.SelectedIndex = 0;

            dtFrom = new DateTimePickerCustom("Từ ngày", "") { Width = 170, Margin = new Padding(0, 0, 24, 0) };
            dtTo = new DateTimePickerCustom("Đến ngày", "") { Width = 170, Margin = new Padding(0, 0, 24, 0) };

            filterLeft.Controls.AddRange(new Control[] {
                txtBookingId, txtTxnRef, cboMethod, cboStatus, dtFrom, dtTo
            });

            var filterRight = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
            var btnSearch = new PrimaryButton("🔍 Lọc") { Width = 120, Height = 36 };
            btnSearch.Click += (_, __) => { /* TODO: filter DB */ };
            filterRight.Controls.Add(btnSearch);

            var filterWrap = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 10, 24, 0),
                ColumnCount = 2
            };
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            filterWrap.Controls.Add(filterLeft, 0, 0);
            filterWrap.Controls.Add(filterRight, 1, 0);

            // ===== Table =====
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
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "paymentId", HeaderText = "Mã TT" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "bookingId", HeaderText = "Booking ID" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "txnRef", HeaderText = "Mã tham chiếu" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "amount", HeaderText = "Số tiền" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "method", HeaderText = "Phương thức" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "paymentDate", HeaderText = "Ngày thanh toán" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "status", HeaderText = "Trạng thái" });
            table.Columns.Add(new DataGridViewTextBoxColumn {
                Name = ACTION_COL,
                HeaderText = "Thao tác",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 200,
            });

            // demo rows
            table.Rows.Add(20011, 6001, "GW-1A2B3C", "2,450,000", "CREDIT_CARD", DateTime.Now.AddMinutes(-10).ToString("dd/MM HH:mm"), "SUCCESS", null);
            table.Rows.Add(20012, 6002, "GW-9Z8Y7X", "1,200,000", "E_WALLET", DateTime.Now.AddMinutes(-50).ToString("dd/MM HH:mm"), "PENDING", null);

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;


            // ===== Main layout (không bị khuất) =====
            var main = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 3
            };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));          // Title
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));          // Filters
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));     // Table fill
            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(filterWrap, 0, 1);
            main.Controls.Add(table, 0, 2);

            // ép z-order đúng để không bị che
            Controls.Clear();
            Controls.Add(main);
            main.BringToFront();

            ResumeLayout(false);
        }

        // ===== Action column (Xem / Xác nhận / Hoàn) =====
        private (Rectangle rcView, Rectangle rcCapture, Rectangle rcRefund) GetRects(Rectangle cellBounds, Font font) {
            int pad = 6;
            int x = cellBounds.Left + pad;
            int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;
            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szC = TextRenderer.MeasureText(TXT_CAPTURE, font, Size.Empty, flags);
            var szR = TextRenderer.MeasureText(TXT_REFUND, font, Size.Empty, flags);
            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcC = new Rectangle(new Point(x, y), szC); x += szC.Width + szS.Width;
            var rcR = new Rectangle(new Point(x, y), szR);
            return (rcV, rcC, rcR);
        }

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;
            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var font = e.CellStyle.Font ?? table.Font;
            var r = GetRects(e.CellBounds, font);
            var link = Color.FromArgb(0, 92, 175);
            var sep = Color.FromArgb(120, 120, 120);

            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, r.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_CAPTURE, font, r.rcCapture.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcCapture.Right, r.rcCapture.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_REFUND, font, r.rcRefund.Location, Color.FromArgb(220, 53, 69), TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcCapture.Contains(p) || r.rcRefund.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            string pid = Convert.ToString(row.Cells["paymentId"].Value) ?? "";

            if (r.rcView.Contains(p)) {
                using (var frm = new PaymentDetailForm(row)) {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(FindForm());
                }
            } else if (r.rcCapture.Contains(p)) {
                MessageBox.Show($"Đã xác nhận/capture Payment #{pid} (demo).", "Capture",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcRefund.Contains(p)) {
                MessageBox.Show($"Yêu cầu hoàn tiền Payment #{pid} (demo).", "Refund",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
