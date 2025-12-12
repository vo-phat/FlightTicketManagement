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

        // Định nghĩa tên cột
        private const string COL_PAYMENT_ID = "paymentId";
        private const string COL_BOOKING_ID = "bookingId";
        private const string COL_AMOUNT = "amount";
        private const string COL_METHOD = "method";
        private const string COL_DATE = "date";
        private const string COL_PAYMENT_STATUS = "paymentStatus";
        private const string COL_BOOKING_STATUS = "bookingStatus";
        private const string COL_EMAIL = "email";
        private const string COL_ACTION = "action";

        // Định nghĩa trạng thái (Constant)
        private const string STATUS_PENDING = "PENDING";
        private const string STATUS_CONFIRMED = "CONFIRMED";
        private const string STATUS_SUCCESS = "SUCCESS";
        private const string STATUS_FAILED = "FAILED";
        private const string STATUS_CANCELLED = "CANCELLED";
        private const string STATUS_REFUNDED = "REFUNDED";

        // Text hiển thị
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

        #region Initialize Components (Giao diện giữ nguyên)
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

            // Add controls
            mainLayout.Controls.Add(lblTitle, 0, 0);
            mainLayout.Controls.Add(filterPanel, 0, 1);
            mainLayout.Controls.Add(table, 0, 2);

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
                LabelText = "🔍 Tìm kiếm (ID, Email...)",
                Width = 250,
                Margin = new Padding(0, 0, 10, 0)
            };
            txtSearch.TextChanged += (s, e) => PerformSearch();

            // Payment Status Filter
            cboPaymentStatus = new UnderlinedComboBox
            {
                LabelText = "Trạng thái thanh toán",
                Width = 150,
                Margin = new Padding(0, 0, 10, 0)
            };
            cboPaymentStatus.Items.AddRange(new object[] { "TẤT CẢ", STATUS_PENDING, STATUS_SUCCESS, STATUS_FAILED });
            cboPaymentStatus.SelectedIndex = 0;
            cboPaymentStatus.SelectedIndexChanged += (s, e) => PerformSearch();

            // Booking Status Filter
            cboBookingStatus = new UnderlinedComboBox
            {
                LabelText = "Trạng thái hóa đơn",
                Width = 150,
                Margin = new Padding(0, 0, 10, 0)
            };
            cboBookingStatus.Items.AddRange(new object[] { "TẤT CẢ", STATUS_PENDING, STATUS_CONFIRMED, STATUS_CANCELLED, STATUS_REFUNDED });
            cboBookingStatus.SelectedIndex = 0;
            cboBookingStatus.SelectedIndexChanged += (s, e) => PerformSearch();

            // Buttons (Giữ nguyên text và kích thước cũ)
            btnShowAll = new PrimaryButton { Text = "📋 Tất cả", Width = 120, Height = 40, Margin = new Padding(10, 0, 5, 0) };
            btnShowAll.Click += BtnShowAll_Click;

            btnShowPending = new PrimaryButton { Text = "⏳ Hóa đơn cần thanh toán", Width = 180, Height = 40, Margin = new Padding(5, 0, 5, 0) };
            btnShowPending.Click += BtnShowPending_Click;

            btnProcessPayment = new PrimaryButton { Text = "💳 Thanh toán", Width = 130, Height = 40, BackColor = Color.FromArgb(40, 167, 69), Margin = new Padding(5, 0, 5, 0) };
            btnProcessPayment.Click += BtnProcessPayment_Click;

            btnDelete = new SecondaryButton { Text = "🗑️ Xóa", Width = 100, Height = 40, Margin = new Padding(5, 0, 5, 0) };
            btnDelete.Click += BtnDelete_Click;

            btnRefresh = new PrimaryButton { Text = "🔄 Làm mới", Width = 120, Height = 40, Margin = new Padding(5, 0, 0, 0) };
            btnRefresh.Click += (s, e) => LoadAllPayments();

            filterPanel.Controls.AddRange(new Control[] { txtSearch, cboPaymentStatus, cboBookingStatus, btnShowAll, btnShowPending, btnProcessPayment, btnDelete, btnRefresh });
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

            table.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = COL_PAYMENT_ID, HeaderText = "Payment ID", Width = 80 },
                new DataGridViewTextBoxColumn { Name = COL_BOOKING_ID, HeaderText = "Booking ID", Width = 80 },
                new DataGridViewTextBoxColumn { Name = COL_AMOUNT, HeaderText = "Số tiền", Width = 100 },
                new DataGridViewTextBoxColumn { Name = COL_METHOD, HeaderText = "Phương thức", Width = 120 },
                new DataGridViewTextBoxColumn { Name = COL_DATE, HeaderText = "Ngày thanh toán", Width = 130 },
                new DataGridViewTextBoxColumn { Name = COL_PAYMENT_STATUS, HeaderText = "TT Thanh toán", Width = 90 },
                new DataGridViewTextBoxColumn { Name = COL_BOOKING_STATUS, HeaderText = "TT Hóa đơn", Width = 100 },
                new DataGridViewTextBoxColumn { Name = COL_EMAIL, HeaderText = "Email", Width = 150 },
                new DataGridViewTextBoxColumn { Name = COL_ACTION, HeaderText = "Thao tác", Width = 120 }
            });

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;
            table.CellFormatting += Table_CellFormatting;
        }
        #endregion

        #region Load Data Methods
        private void LoadAllPayments()
        {
            try
            {
                table.Rows.Clear();
                var payments = paymentBUS.GetAllPaymentsWithDetails();
                foreach (var p in payments) AddPaymentToTable(p);
            }
            catch (Exception ex)
            {
                ShowError("Lỗi khi tải dữ liệu", ex);
            }
        }

        private void LoadPendingBookings()
        {
            try
            {
                table.Rows.Clear();
                var payments = paymentBUS.GetPendingBookingsPayments();
                foreach (var p in payments) AddPaymentToTable(p);
            }
            catch (Exception ex)
            {
                ShowError("Lỗi khi tải dữ liệu chờ xử lý", ex);
            }
        }

        private void AddPaymentToTable(PaymentDetailDTO p)
        {
            int idx = table.Rows.Add(
                p.PaymentId,
                p.BookingId,
                p.Amount.ToString("N0") + " ₫",
                p.GetPaymentMethodDisplay(),
                p.PaymentDate.ToString("dd/MM/yyyy HH:mm"),
                p.GetStatusDisplay(),
                p.GetBookingStatusDisplay(),
                p.AccountEmail,
                null
            );
            table.Rows[idx].Tag = p;
        }
        #endregion

        #region Search & Filter
        private void PerformSearch()
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                string pStatus = cboPaymentStatus.SelectedItem?.ToString() ?? "TẤT CẢ";
                string bStatus = cboBookingStatus.SelectedItem?.ToString() ?? "TẤT CẢ";

                var list = paymentBUS.GetAllPaymentsWithDetails();

                if (!string.IsNullOrEmpty(keyword))
                {
                    list = list.Where(p =>
                        p.PaymentId.ToString().Contains(keyword) ||
                        p.BookingId.ToString().Contains(keyword) ||
                        p.Amount.ToString().Contains(keyword) ||
                        p.AccountEmail.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                if (pStatus != "TẤT CẢ") list = list.Where(p => p.Status.Equals(pStatus, StringComparison.OrdinalIgnoreCase)).ToList();
                if (bStatus != "TẤT CẢ") list = list.Where(p => p.BookingStatus.Equals(bStatus, StringComparison.OrdinalIgnoreCase)).ToList();

                table.Rows.Clear();
                foreach (var item in list) AddPaymentToTable(item);
            }
            catch (Exception ex) { ShowError("Lỗi tìm kiếm", ex); }
        }
        #endregion

        #region Main Logic (ĐÃ CẬP NHẬT LOGIC MỚI)

        private void BtnProcessPayment_Click(object sender, EventArgs e)
        {
            var payment = GetSelectedPayment();
            if (payment == null) return;

            // --- KIỂM TRA ĐIỀU KIỆN ---
            bool isPaymentPending = payment.Status.Equals(STATUS_PENDING, StringComparison.OrdinalIgnoreCase);
            bool isBookingConfirmed = payment.BookingStatus.Equals(STATUS_PENDING, StringComparison.OrdinalIgnoreCase);

            if (!isPaymentPending)
            {
                MessageBox.Show($"Thanh toán này đang ở trạng thái '{payment.GetStatusDisplay()}', không thể xử lý lại.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isBookingConfirmed)
            {
                MessageBox.Show($"Hóa đơn hiện đang ở trạng thái '{payment.GetBookingStatusDisplay()}'.\n" +
                                "Chỉ có thể thanh toán cho các Hóa đơn đã được ĐANG CHỜ (PENDING).",
                                "Không thể thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- XÁC NHẬN ---
            var confirmMsg = $"XÁC NHẬN THANH TOÁN\n\n" +
                             $"Payment ID: {payment.PaymentId}\n" +
                             $"Booking ID: {payment.BookingId}\n" +
                             $"Số tiền: {payment.Amount:N0} ₫\n" +
                             $"Khách hàng: {payment.AccountEmail}\n\n" +
                             "Bạn có chắc chắn muốn duyệt thanh toán này?";

            if (MessageBox.Show(confirmMsg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool success = paymentBUS.ProcessPayment(payment.PaymentId, out string message);
                    if (success)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllPayments();
                    }
                    else
                    {
                        MessageBox.Show(message, "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex) { ShowError("Lỗi xử lý thanh toán", ex); }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var payment = GetSelectedPayment();
            if (payment != null) ExecuteDeletePayment(payment);
        }

        // Hàm xóa dùng chung
        private void ExecuteDeletePayment(PaymentDetailDTO payment)
        {
            if (payment.Status.Equals(STATUS_SUCCESS, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Không thể xóa hóa đơn đã thanh toán thành công!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (payment.BookingStatus.Equals(STATUS_CONFIRMED, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Không thể xóa thanh toán của Hóa đơn đã được giữ chỗ (Confirmed)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa Payment ID #{payment.PaymentId}?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    if (paymentBUS.DeletePayment(payment.PaymentId, out string message))
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllPayments();
                    }
                    else
                    {
                        MessageBox.Show(message, "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex) { ShowError("Lỗi khi xóa", ex); }
            }
        }

        private PaymentDetailDTO GetSelectedPayment()
        {
            if (table.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            return table.SelectedRows[0].Tag as PaymentDetailDTO;
        }

        private void ShowError(string title, Exception ex)
        {
            MessageBox.Show($"{title}: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region Event Handlers (Buttons)
        private void BtnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cboPaymentStatus.SelectedIndex = 0;
            cboBookingStatus.SelectedIndex = 0;
            LoadAllPayments();
        }

        private void BtnShowPending_Click(object sender, EventArgs e)
        {
            LoadPendingBookings();
        }
        #endregion

        #region Table Styling & Actions
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
            if (e.RowIndex < 0 || table.Columns[e.ColumnIndex].Name != COL_ACTION) return;

            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var font = e.CellStyle.Font ?? table.Font;
            var rects = GetActionRects(e.CellBounds, font);

            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, rects.rcView.Location, Color.FromArgb(0, 92, 175), TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rects.rcView.Right, rects.rcView.Top), Color.Gray, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_DELETE, font, rects.rcDelete.Location, Color.FromArgb(220, 53, 69), TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || table.Columns[e.ColumnIndex].Name != COL_ACTION)
            {
                table.Cursor = Cursors.Default;
                return;
            }
            table.Cursor = Cursors.Hand;
        }

        private void Table_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || table.Columns[e.ColumnIndex].Name != COL_ACTION) return;

            var row = table.Rows[e.RowIndex];
            var payment = row.Tag as PaymentDetailDTO;
            if (payment == null) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var rects = GetActionRects(rect, table.DefaultCellStyle.Font);

            // Xử lý tọa độ click
            int clickX = e.X;
            if (clickX < 50) ShowPaymentDetail(payment);
            else ExecuteDeletePayment(payment);
        }

        private void Table_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            string colName = table.Columns[e.ColumnIndex].Name;
            string status = e.Value.ToString().ToUpper();

            if (colName == COL_PAYMENT_STATUS)
            {
                e.CellStyle.Font = new Font(table.Font, FontStyle.Bold);
                if (status.Contains(STATUS_PENDING)) e.CellStyle.ForeColor = Color.FromArgb(255, 193, 7); // Vàng
                else if (status.Contains(STATUS_SUCCESS)) e.CellStyle.ForeColor = Color.FromArgb(40, 167, 69); // Xanh lá
                else if (status.Contains(STATUS_FAILED)) e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69); // Đỏ
            }
            else if (colName == COL_BOOKING_STATUS)
            {
                if (status.Contains(STATUS_PENDING)) e.CellStyle.ForeColor = Color.FromArgb(255, 193, 7);
                else if (status.Contains(STATUS_CONFIRMED)) e.CellStyle.ForeColor = Color.FromArgb(40, 167, 69);
                else if (status.Contains(STATUS_CANCELLED) || status.Contains(STATUS_REFUNDED)) e.CellStyle.ForeColor = Color.Gray;
            }
        }
        #endregion

        #region Helper UI
        private void ShowPaymentDetail(PaymentDetailDTO payment)
        {
            // Tính toán chênh lệch (nếu có)
            decimal diff = payment.Amount - payment.BookingTotalAmount;
            string diffText = diff == 0 ? "Khớp" : (diff > 0 ? $"+{diff:N0} (Dư)" : $"{diff:N0} (Thiếu)");

            // Xây dựng nội dung chi tiết
            var msg = new System.Text.StringBuilder();

            msg.AppendLine("============== THÔNG TIN THANH TOÁN ==============");
            msg.AppendLine($"🔹 Mã giao dịch (Payment ID): {payment.PaymentId}");
            msg.AppendLine($"🔹 Số tiền thanh toán:       {payment.Amount:N0} VNĐ");
            msg.AppendLine($"🔹 Phương thức:              {payment.GetPaymentMethodDisplay()}");
            msg.AppendLine($"🔹 Ngày giao dịch:           {payment.PaymentDate:dd/MM/yyyy HH:mm:ss}");
            msg.AppendLine($"🔹 Trạng thái:               {payment.GetStatusDisplay()}");
            msg.AppendLine();

            msg.AppendLine("============== THÔNG TIN HÓA ĐƠN ==============");
            msg.AppendLine($"🔸 Mã đặt chỗ (Booking ID):  {payment.BookingId}");
            msg.AppendLine($"🔸 Ngày đặt vé:              {payment.BookingDate:dd/MM/yyyy}");
            msg.AppendLine($"🔸 Tổng tiền vé:             {payment.BookingTotalAmount:N0} VNĐ");
            msg.AppendLine($"🔸 Trạng thái Booking:       {payment.GetBookingStatusDisplay()}");
            msg.AppendLine($"🔸 Kiểm tra đối soát:        {diffText}");
            msg.AppendLine();

            msg.AppendLine("============== THÔNG TIN KHÁCH HÀNG =============");
            msg.AppendLine($"👤 Account ID:               {payment.AccountId}");
            msg.AppendLine($"📧 Email khách hàng:         {payment.AccountEmail}");

            // Lưu ý: Nếu muốn hiện Tên hành khách, cần JOIN thêm bảng passenger_profiles trong DAO
            // Hiện tại ta dùng Email tài khoản làm thông tin người thanh toán.

            MessageBox.Show(msg.ToString(), "Chi tiết giao dịch", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

    }
}