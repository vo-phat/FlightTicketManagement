using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using BUS.Payment;
using DTO.Payment;

namespace GUI.Features.Payments.SubFeatures
{
    public class PaymentsPOSControl : UserControl
    {
        #region Fields
        private TableCustom table;
        private Label lblTitle;
        private UnderlinedTextField txtSearch;
        private UnderlinedComboBox cboPaymentStatus, cboBookingStatus;
        private PrimaryButton btnRefresh, btnShowAll, btnShowPending, btnProcessPayment;
        private SecondaryButton btnDelete;
        private FlowLayoutPanel filterPanel;
        private TableLayoutPanel mainLayout;

        private readonly PaymentBUS paymentBUS = new PaymentBUS();

        // Column names
        private const string COL_PAYMENT_ID = "paymentId";
        private const string COL_BOOKING_ID = "bookingId";
        private const string COL_AMOUNT = "amount";
        private const string COL_METHOD = "method";
        private const string COL_DATE = "date";
        private const string COL_PAYMENT_STATUS = "paymentStatus";
        private const string COL_BOOKING_STATUS = "bookingStatus";
        private const string COL_EMAIL = "email";
        private const string COL_ACTION = "action";

        // Action buttons text
        private const string TXT_VIEW = "Xem";
        private const string TXT_DELETE = "Xóa";
        private const string SEP = " | ";
        #endregion

        #region Constructor
        public PaymentsPOSControl()
        {
            InitializeComponent();
            LoadAllPayments();
        }
        #endregion

        #region Initialize Components
        private void InitializeComponent()
        {
            // Main Layout
            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(20)
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Title
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120)); // Filters
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Table

            // Title
            lblTitle = new Label
            {
                Text = "QUẢN LÝ THANH TOÁN",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Filter Panel
            InitializeFilterPanel();

            // Table
            InitializeTable();

            // Add controls to main layout
            mainLayout.Controls.Add(lblTitle, 0, 0);
            mainLayout.Controls.Add(filterPanel, 0, 1);
            mainLayout.Controls.Add(table, 0, 2);

            // Add main layout to UserControl
            this.Controls.Add(mainLayout);
            this.BackColor = Color.FromArgb(240, 244, 248);
            this.Size = new Size(1200, 800);
        }

        private void InitializeFilterPanel()
        {
            filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(0, 10, 0, 10)
            };

            // Search box
            txtSearch = new UnderlinedTextField
            {
                LabelText = "🔍 Tìm kiếm (ID, Email, Số tiền...)",
                Width = 250,
                Margin = new Padding(0, 0, 10, 0)
            };
            txtSearch.TextChanged += (s, e) => PerformSearch();

            // Payment Status Filter
            cboPaymentStatus = new UnderlinedComboBox
            {
                LabelText = "Trạng thái Payment",
                Width = 150,
                Margin = new Padding(0, 0, 10, 0)
            };
            cboPaymentStatus.Items.AddRange(new object[] { "TẤT CẢ", "PENDING", "SUCCESS", "FAILED" });
            cboPaymentStatus.SelectedIndex = 0;
            cboPaymentStatus.SelectedIndexChanged += (s, e) => PerformSearch();

            // Booking Status Filter
            cboBookingStatus = new UnderlinedComboBox
            {
                LabelText = "Trạng thái Booking",
                Width = 150,
                Margin = new Padding(0, 0, 10, 0)
            };
            cboBookingStatus.Items.AddRange(new object[] { "TẤT CẢ", "PENDING", "CONFIRMED", "CANCELLED", "REFUNDED" });
            cboBookingStatus.SelectedIndex = 0;
            cboBookingStatus.SelectedIndexChanged += (s, e) => PerformSearch();

            // Buttons
            btnShowAll = new PrimaryButton
            {
                Text = "📋 Tất cả",
                Width = 120,
                Height = 40,
                Margin = new Padding(10, 0, 5, 0)
            };
            btnShowAll.Click += BtnShowAll_Click;

            btnShowPending = new PrimaryButton
            {
                Text = "⏳ Hóa đơn cần thanh toán",
                Width = 150,
                Height = 40,
                Margin = new Padding(5, 0, 5, 0)
            };
            btnShowPending.Click += BtnShowPending_Click;

            btnProcessPayment = new PrimaryButton
            {
                Text = "💳 Thanh toán",
                Width = 130,
                Height = 40,
                BackColor = Color.FromArgb(40, 167, 69),
                Margin = new Padding(5, 0, 5, 0)
            };
            btnProcessPayment.Click += BtnProcessPayment_Click;

            btnDelete = new SecondaryButton
            {
                Text = "🗑️ Xóa",
                Width = 100,
                Height = 40,
                Margin = new Padding(5, 0, 5, 0)
            };
            btnDelete.Click += BtnDelete_Click;

            btnRefresh = new PrimaryButton
            {
                Text = "🔄 Làm mới",
                Width = 120,
                Height = 40,
                Margin = new Padding(5, 0, 0, 0)
            };
            btnRefresh.Click += (s, e) => LoadAllPayments();

            // Add to panel
            filterPanel.Controls.Add(txtSearch);
            filterPanel.Controls.Add(cboPaymentStatus);
            filterPanel.Controls.Add(cboBookingStatus);
            filterPanel.Controls.Add(btnShowAll);
            filterPanel.Controls.Add(btnShowPending);
            filterPanel.Controls.Add(btnProcessPayment);
            filterPanel.Controls.Add(btnDelete);
            filterPanel.Controls.Add(btnRefresh);
        }

        private void InitializeTable()
        {
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Define columns
            table.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = COL_PAYMENT_ID, HeaderText = "Payment ID", Width = 80 },
                new DataGridViewTextBoxColumn { Name = COL_BOOKING_ID, HeaderText = "Booking ID", Width = 80 },
                new DataGridViewTextBoxColumn { Name = COL_AMOUNT, HeaderText = "Số tiền", Width = 100 },
                new DataGridViewTextBoxColumn { Name = COL_METHOD, HeaderText = "Phương thức", Width = 120 },
                new DataGridViewTextBoxColumn { Name = COL_DATE, HeaderText = "Ngày thanh toán", Width = 130 },
                new DataGridViewTextBoxColumn { Name = COL_PAYMENT_STATUS, HeaderText = "TT Payment", Width = 90 },
                new DataGridViewTextBoxColumn { Name = COL_BOOKING_STATUS, HeaderText = "TT Booking", Width = 100 },
                new DataGridViewTextBoxColumn { Name = COL_EMAIL, HeaderText = "Email", Width = 150 },
                new DataGridViewTextBoxColumn { Name = COL_ACTION, HeaderText = "Thao tác", Width = 120 }
            });

            // Events
            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;
            table.CellFormatting += Table_CellFormatting;
        }
        #endregion

        #region Load Data Methods
        /// <summary>
        /// Load tất cả payments với thông tin đầy đủ
        /// </summary>
        private void LoadAllPayments()
        {
            try
            {
                table.Rows.Clear();
                var payments = paymentBUS.GetAllPaymentsWithDetails();

                foreach (var p in payments)
                {
                    AddPaymentToTable(p);
                }

                //UpdateStatusBar($"Hiển thị {payments.Count} payment(s)");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load chỉ các bookings có trạng thái PENDING
        /// </summary>
        private void LoadPendingBookings()
        {
            try
            {
                table.Rows.Clear();
                var payments = paymentBUS.GetPendingBookingsPayments();

                foreach (var p in payments)
                {
                    AddPaymentToTable(p);
                }

                //UpdateStatusBar($"Hiển thị {payments.Count} booking(s) đang chờ");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Thêm payment vào table
        /// </summary>
        private void AddPaymentToTable(PaymentDetailDTO p)
        {
            table.Rows.Add(
                p.PaymentId,
                p.BookingId,
                p.Amount.ToString("N0") + " ₫",
                p.GetPaymentMethodDisplay(),
                p.PaymentDate.ToString("dd/MM/yyyy HH:mm"),
                p.GetStatusDisplay(),
                p.GetBookingStatusDisplay(),
                p.AccountEmail,
                null // Action column
            );

            // Store original object in Tag for later use
            table.Rows[table.Rows.Count - 1].Tag = p;
        }
        #endregion

        #region Search & Filter
        /// <summary>
        /// Tìm kiếm và lọc dữ liệu
        /// </summary>
        private void PerformSearch()
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                string paymentStatus = cboPaymentStatus.SelectedItem?.ToString();
                string bookingStatus = cboBookingStatus.SelectedItem?.ToString();

                // Get all payments
                var allPayments = paymentBUS.GetAllPaymentsWithDetails();

                // Filter by keyword
                if (!string.IsNullOrEmpty(keyword))
                {
                    allPayments = allPayments.Where(p =>
                        p.PaymentId.ToString().Contains(keyword) ||
                        p.BookingId.ToString().Contains(keyword) ||
                        p.Amount.ToString().Contains(keyword) ||
                        p.AccountEmail.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                        p.PaymentMethod.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                // Filter by payment status
                if (paymentStatus != "TẤT CẢ")
                {
                    allPayments = allPayments.Where(p =>
                        p.Status.Equals(paymentStatus, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                // Filter by booking status
                if (bookingStatus != "TẤT CẢ")
                {
                    allPayments = allPayments.Where(p =>
                        p.BookingStatus.Equals(bookingStatus, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                // Display results
                table.Rows.Clear();
                foreach (var p in allPayments)
                {
                    AddPaymentToTable(p);
                }

                //UpdateStatusBar($"Tìm thấy {allPayments.Count} kết quả");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Button Events
        private void BtnShowAll_Click(object sender, EventArgs e)
        {
            // Reset filters
            txtSearch.Text = "";
            cboPaymentStatus.SelectedIndex = 0;
            cboBookingStatus.SelectedIndex = 0;
            LoadAllPayments();
        }

        private void BtnShowPending_Click(object sender, EventArgs e)
        {
            LoadPendingBookings();
        }

        /// <summary>
        /// Xử lý thanh toán
        /// </summary>
        private void BtnProcessPayment_Click(object sender, EventArgs e)
        {
            if (table.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một payment để thanh toán!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = table.SelectedRows[0];
            var payment = selectedRow.Tag as PaymentDetailDTO;

            if (payment == null)
            {
                MessageBox.Show("Không thể lấy thông tin payment!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra điều kiện thanh toán
            if (!payment.CanProcessPayment())
            {
                string reason = "";
                if (!payment.IsPaymentPending())
                    reason = $"Payment đang ở trạng thái '{payment.GetStatusDisplay()}'";
                else if (!payment.IsBookingPending())
                    reason = $"Booking đang ở trạng thái '{payment.GetBookingStatusDisplay()}'";

                MessageBox.Show(
                    $"Không thể xử lý thanh toán!\n\n{reason}\n\n" +
                    "Chỉ có thể thanh toán khi:\n" +
                    "- Payment ở trạng thái PENDING\n" +
                    "- Booking ở trạng thái PENDING",
                    "Không thể thanh toán",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Hiển thị thông tin xác nhận
            var confirmMsg = $"XÁC NHẬN THANH TOÁN\n\n" +
                           $"Payment ID: {payment.PaymentId}\n" +
                           $"Booking ID: {payment.BookingId}\n" +
                           $"Số tiền: {payment.Amount:N0} ₫\n" +
                           $"Phương thức: {payment.GetPaymentMethodDisplay()}\n" +
                           $"Khách hàng: {payment.AccountEmail}\n\n" +
                           $"Bạn có chắc chắn muốn thanh toán?";

            if (MessageBox.Show(confirmMsg, "Xác nhận thanh toán",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            // Xử lý thanh toán
            try
            {
                bool success = paymentBUS.ProcessPayment(payment.PaymentId, out string message);

                if (success)
                {
                    MessageBox.Show(message, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reload data
                    LoadAllPayments();
                }
                else
                {
                    MessageBox.Show(message, "Thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xử lý thanh toán: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xóa payment
        /// </summary>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (table.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = table.SelectedRows[0];
            var payment = selectedRow.Tag as PaymentDetailDTO;

            if (payment == null)
            {
                MessageBox.Show("Không thể lấy thông tin hóa đơn!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xác nhận xóa
            var confirmMsg = $"XÁC NHẬN XÓA HÓA ĐƠN\n\n" +
                           $"Payment ID: {payment.PaymentId}\n" +
                           $"Booking ID: {payment.BookingId}\n" +
                           $"Số tiền: {payment.Amount:N0} ₫\n\n" +
                           $"⚠️ Hành động này không thể hoàn tác!\n\n" +
                           $"Bạn có chắc chắn muốn xóa?";

            if (MessageBox.Show(confirmMsg, "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            // Xóa payment
            try
            {
                bool success = paymentBUS.DeletePayment(payment.PaymentId, out string message);

                if (success)
                {
                    MessageBox.Show(message, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reload data
                    LoadAllPayments();
                }
                else
                {
                    MessageBox.Show(message, "Thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa hóa đơn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Table Events - Action Column
        private (Rectangle rcView, Rectangle rcDelete) GetActionRects(Rectangle cellBounds, Font font)
        {
            int pad = 6;
            int x = cellBounds.Left + pad;
            int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;
            var flags = TextFormatFlags.NoPadding;

            var szView = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szSep = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szDelete = TextRenderer.MeasureText(TXT_DELETE, font, Size.Empty, flags);

            var rcView = new Rectangle(new Point(x, y), szView);
            x += szView.Width + szSep.Width;
            var rcDelete = new Rectangle(new Point(x, y), szDelete);

            return (rcView, rcDelete);
        }

        private void Table_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != COL_ACTION) return;

            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var font = e.CellStyle.Font ?? table.Font;
            var rects = GetActionRects(e.CellBounds, font);

            var colorView = Color.FromArgb(0, 92, 175);
            var colorDelete = Color.FromArgb(220, 53, 69);
            var colorSep = Color.FromArgb(120, 120, 120);

            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, rects.rcView.Location,
                colorView, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font,
                new Point(rects.rcView.Right, rects.rcView.Top), colorSep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_DELETE, font, rects.rcDelete.Location,
                colorDelete, TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                table.Cursor = Cursors.Default;
                return;
            }

            if (table.Columns[e.ColumnIndex].Name != COL_ACTION)
            {
                table.Cursor = Cursors.Default;
                return;
            }

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(rect, font);
            var mousePoint = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            table.Cursor = (rects.rcView.Contains(mousePoint) || rects.rcDelete.Contains(mousePoint))
                ? Cursors.Hand
                : Cursors.Default;
        }

        private void Table_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != COL_ACTION) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(rect, font);
            var mousePoint = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            var payment = row.Tag as PaymentDetailDTO;

            if (payment == null) return;

            if (rects.rcView.Contains(mousePoint))
            {
                // TODO: Mở form xem chi tiết
                ShowPaymentDetail(payment);
            }
            else if (rects.rcDelete.Contains(mousePoint))
            {
                // Xóa payment
                DeletePaymentFromAction(payment);
            }
        }
        #endregion

        #region Table Formatting
        private void Table_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Format payment status với màu sắc
            if (table.Columns[e.ColumnIndex].Name == COL_PAYMENT_STATUS)
            {
                string status = e.Value?.ToString() ?? "";
                switch (status)
                {
                    case "Pending":
                        e.CellStyle.ForeColor = Color.FromArgb(255, 193, 7); // Yellow
                        e.CellStyle.Font = new Font(table.Font, FontStyle.Bold);
                        break;
                    case "Success":
                        e.CellStyle.ForeColor = Color.FromArgb(40, 167, 69); // Green
                        e.CellStyle.Font = new Font(table.Font, FontStyle.Bold);
                        break;
                    case "Failed":
                        e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69); // Red
                        e.CellStyle.Font = new Font(table.Font, FontStyle.Bold);
                        break;
                }
            }

            // Format booking status với màu sắc
            if (table.Columns[e.ColumnIndex].Name == COL_BOOKING_STATUS)
            {
                string status = e.Value?.ToString() ?? "";
                switch (status)
                {
                    case "Pending":
                        e.CellStyle.ForeColor = Color.FromArgb(255, 193, 7);
                        break;
                    case "Confirm":
                        e.CellStyle.ForeColor = Color.FromArgb(40, 167, 69);
                        break;
                    case "Cancelled":
                    case "Refunded":
                        e.CellStyle.ForeColor = Color.FromArgb(108, 117, 125); // Gray
                        break;
                }
            }
        }
        #endregion

        #region Helper Methods
        private void ShowPaymentDetail(PaymentDetailDTO payment)
        {
            // TODO: Implement detail form
            var detailMsg = $"CHI TIẾT PAYMENT\n\n" +
                          $"Payment ID: {payment.PaymentId}\n" +
                          $"Booking ID: {payment.BookingId}\n" +
                          $"Account ID: {payment.AccountId}\n" +
                          $"Email: {payment.AccountEmail}\n\n" +
                          $"Số tiền: {payment.Amount:N0} ₫\n" +
                          $"Phương thức: {payment.GetPaymentMethodDisplay()}\n" +
                          $"Ngày thanh toán: {payment.PaymentDate:dd/MM/yyyy HH:mm}\n\n" +
                          $"Trạng thái Payment: {payment.GetStatusDisplay()}\n" +
                          $"Trạng thái Booking: {payment.GetBookingStatusDisplay()}\n\n" +
                          $"Ngày đặt: {payment.BookingDate:dd/MM/yyyy HH:mm}\n" +
                          $"Tổng tiền booking: {payment.BookingTotalAmount:N0} ₫\n" +
                          $"Chênh lệch: {payment.GetAmountDifference():N0} ₫";

            MessageBox.Show(detailMsg, "Chi tiết hóa đơn",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeletePaymentFromAction(PaymentDetailDTO payment)
        {
            var confirmMsg = $"Xóa Hóa Đơn #{payment.PaymentId}?";

            if (MessageBox.Show(confirmMsg, "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                bool success = paymentBUS.DeletePayment(payment.PaymentId, out string message);

                if (success)
                {
                    MessageBox.Show(message, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAllPayments();
                }
                else
                {
                    MessageBox.Show(message, "Thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void UpdateStatusBar(string message)
        //{
        //    // Update title with count
        //    lblTitle.Text = $"QUẢN LÝ THANH TOÁN - {message}";
        //}
        #endregion
    }
}