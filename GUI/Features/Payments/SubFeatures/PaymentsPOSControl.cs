using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using BUS.Payment;
using DTO.Payment;

namespace GUI.Features.Payments.SubFeatures
{
    public class PaymentsPOSControl : UserControl
    {
        private TableCustom table;
        private Label lblTitle;
        private UnderlinedTextField txtBookingId, txtAmount, txtNote;
        private UnderlinedComboBox cboStatus;
        private DateTimePickerCustom dtFrom, dtTo;

        private readonly PaymentBUS paymentBUS = new PaymentBUS(); // ✅ Thêm BUS

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_REFUND = "Hoàn";
        private const string SEP = " / ";

        public PaymentsPOSControl() { InitializeComponent(); }

        private void InitializeComponent()
        {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // ===== Title =====
            lblTitle = new Label
            {
                Text = "💵 POS / Thanh toán tại quầy (CASH)",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Filter =====
            var filterLeft = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight
            };
            txtBookingId = new UnderlinedTextField("Booking ID", "") { Width = 140, Margin = new Padding(0, 0, 24, 0) };
            dtFrom = new DateTimePickerCustom("Từ ngày", "") { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            dtTo = new DateTimePickerCustom("Đến ngày", "") { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            cboStatus = new UnderlinedComboBox("Trạng thái", new object[] { "ALL", "PENDING", "SUCCESS", "FAILED" }) { Width = 150 };
            cboStatus.SelectedIndex = 0;

            filterLeft.Controls.AddRange(new Control[] {
                txtBookingId, dtFrom, dtTo, cboStatus
            });

            var btnSearch = new PrimaryButton("🔍 Lọc") { Width = 120, Height = 36 };
            btnSearch.Click += BtnSearch_Click; // ✅ Gọi DB
            var filterRight = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
            filterRight.Controls.Add(btnSearch);

            var filterWrap = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 10, 24, 0),
                ColumnCount = 2
            };
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            filterWrap.Controls.Add(filterLeft, 0, 0);
            filterWrap.Controls.Add(filterRight, 1, 0);

            // ===== Quick Create (POS charge) =====
            var quickCreate = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 8, 24, 0),
                ColumnCount = 4
            };
            txtAmount = new UnderlinedTextField("Số tiền (VND)", "") { Width = 180, Margin = new Padding(0, 0, 16, 0) };
            txtNote = new UnderlinedTextField("Ghi chú (tuỳ chọn)", "") { Width = 260, Margin = new Padding(0, 0, 16, 0) };
            var btnCharge = new PrimaryButton("💰 Thu tiền (CASH)") { Width = 160, Height = 36 };
            btnCharge.Click += BtnCharge_Click; // ✅ Gọi BUS.InsertPayment()
            quickCreate.Controls.Add(new Label { Text = "Tạo giao dịch nhanh:", AutoSize = true, Margin = new Padding(0, 8, 16, 0) }, 0, 0);
            quickCreate.Controls.Add(txtAmount, 1, 0);
            quickCreate.Controls.Add(txtNote, 2, 0);
            quickCreate.Controls.Add(btnCharge, 3, 0);

            // ===== Table =====
            table = new TableCustom
            {
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
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "amount", HeaderText = "Số tiền" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "method", HeaderText = "Phương thức" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "paymentDate", HeaderText = "Ngày thanh toán" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "status", HeaderText = "Trạng thái" });
            table.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = ACTION_COL,
                HeaderText = "Thao tác",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 200,
            });

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // ===== Main layout =====
            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, BackColor = Color.Transparent };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(filterWrap, 0, 1);
            main.Controls.Add(quickCreate, 0, 2);
            main.Controls.Add(table, 0, 3);

            Controls.Clear();
            Controls.Add(main);
            ResumeLayout(false);

            LoadPayments(); // ✅ load khi khởi động
        }

        // ===== Load danh sách từ DB =====
        private void LoadPayments()
        {
            table.Rows.Clear();
            var payments = paymentBUS.GetAllPayments();

            foreach (var p in payments)
            {
                if (p.PaymentMethod.ToUpper() != "CASH") continue; // chỉ hiện POS
                table.Rows.Add(p.PaymentId, p.BookingId, p.Amount.ToString("N0"),
                    p.PaymentMethod, p.PaymentDate.ToString("dd/MM HH:mm"), p.Status, null);
            }
        }

        // ===== Sự kiện Lọc =====
        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            string keyword = txtBookingId.Text.Trim();
            var list = string.IsNullOrEmpty(keyword)
                ? paymentBUS.GetAllPayments()
                : paymentBUS.SearchPayments(keyword);

            table.Rows.Clear();
            foreach (var p in list)
            {
                if (p.PaymentMethod.ToUpper() != "CASH") continue;
                if (cboStatus.SelectedItem?.ToString() != "ALL" && !p.Status.Equals(cboStatus.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase))
                    continue;

                table.Rows.Add(p.PaymentId, p.BookingId, p.Amount.ToString("N0"),
                    p.PaymentMethod, p.PaymentDate.ToString("dd/MM HH:mm"), p.Status, null);
            }
        }

        // ===== Sự kiện Thu tiền =====
        private void BtnCharge_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtBookingId.Text, out int bookingId))
                {
                    MessageBox.Show("Vui lòng nhập Booking ID hợp lệ!");
                    return;
                }
                if (!decimal.TryParse(txtAmount.Text, out decimal amount))
                {
                    MessageBox.Show("Vui lòng nhập số tiền hợp lệ!");
                    return;
                }

                var payment = new PaymentDTO
                {
                    BookingId = bookingId,
                    Amount = amount,
                    PaymentMethod = "CASH",
                    PaymentDate = DateTime.Now,
                    Status = "SUCCESS"
                };

                if (paymentBUS.InsertPayment(payment, out string msg))
                {
                    MessageBox.Show(msg, "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPayments();
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo thanh toán POS: " + ex.Message);
            }
        }

        // ===== Các phần xử lý Action column giữ nguyên (View/Refund) =====
        private (Rectangle rcView, Rectangle rcRefund) GetRects(Rectangle cellBounds, Font font)
        {
            int pad = 6;
            int x = cellBounds.Left + pad;
            int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;
            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szR = TextRenderer.MeasureText(TXT_REFUND, font, Size.Empty, flags);
            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcR = new Rectangle(new Point(x, y), szR);
            return (rcV, rcR);
        }

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;
            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
            var font = e.CellStyle.Font ?? table.Font;
            var r = GetRects(e.CellBounds, font);
            var link = Color.FromArgb(0, 92, 175); var sep = Color.FromArgb(120, 120, 120);
            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, r.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_REFUND, font, r.rcRefund.Location, Color.FromArgb(220, 53, 69), TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }
            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcRefund.Contains(p)) ? Cursors.Hand : Cursors.Default;
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
            string pid = row.Cells["paymentId"].Value?.ToString() ?? "";

            if (r.rcView.Contains(p))
            {
                using (var frm = new PaymentDetailForm(row))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(FindForm());
                }
            }
            else if (r.rcRefund.Contains(p))
            {
                MessageBox.Show($"Yêu cầu hoàn tiền cho Payment #{pid}.", "Refund", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
