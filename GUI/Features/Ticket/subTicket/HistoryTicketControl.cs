using System;
using System.Collections.Generic;
using System.Drawing; // Thêm để dùng Color, Font
using System.Linq;
using System.Windows.Forms;
using BUS.Ticket;
using DTO.Ticket;

namespace GUI.Features.Ticket.subTicket
{
    public partial class HistoryTicketControl : UserControl
    {
        private readonly TicketsHistoryBUS _ticketBus;
        private List<TicketHistoryDTO> _allTickets;
        private int _accountId = 2;

        public HistoryTicketControl()
        {
            InitializeComponent();

            _ticketBus = new TicketsHistoryBUS();

            // Cấu hình sự kiện
            this.Load += HistoryTicketControl_Load;
            btnFilter.Click += btnFilter_click;

            // Placeholder cho ô tìm kiếm
            txtTicketNumber.PlaceholderText = "Nhập mã vé...";
        }

        private void HistoryTicketControl_Load(object sender, EventArgs e)
        {
            InitStatusCombo();
            StyleGrid(); // <--- Gọi hàm trang trí giao diện
            SetupGridColumns(); // <--- Cấu hình cột
            LoadTicketHistory();
        }

        // =========================================================
        // 1. TRANG TRÍ GIAO DIỆN GRID (Style)
        // =========================================================
        private void StyleGrid()
        {
            // --- Cấu hình chung ---
            dgvTickets.BackgroundColor = Color.White;
            dgvTickets.BorderStyle = BorderStyle.None;
            dgvTickets.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Chỉ kẻ ngang
            dgvTickets.GridColor = Color.FromArgb(240, 240, 240); // Đường kẻ mờ
            dgvTickets.RowHeadersVisible = false; // Ẩn cột thừa bên trái
            dgvTickets.AllowUserToResizeRows = false;

            // QUAN TRỌNG: Giãn cột ra toàn màn hình
            dgvTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // --- Header Style (Cao, Đậm, Phẳng) ---
            dgvTickets.EnableHeadersVisualStyles = false;
            dgvTickets.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvTickets.ColumnHeadersHeight = 50; // Header cao
            dgvTickets.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvTickets.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 100, 100); // Chữ xám đậm
            dgvTickets.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvTickets.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Căn giữa tiêu đề

            // --- Row Style (Cao, Thoáng) ---
            dgvTickets.RowTemplate.Height = 50; // Dòng dữ liệu cao
            dgvTickets.DefaultCellStyle.BackColor = Color.White;
            dgvTickets.DefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            dgvTickets.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvTickets.DefaultCellStyle.SelectionBackColor = Color.FromArgb(235, 245, 255); // Xanh nhạt khi chọn
            dgvTickets.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 92, 175); // Chữ xanh đậm
            dgvTickets.DefaultCellStyle.Padding = new Padding(5, 0, 0, 0);
            dgvTickets.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Căn giữa dữ liệu
        }

        // =========================================================
        // 2. CẤU HÌNH CỘT (Dùng FillWeight thay vì Width)
        // =========================================================
        private void SetupGridColumns()
        {
            dgvTickets.Columns.Clear();

            // Cột 1: Mã vé
            AddColumn("TicketNumber", "MÃ VÉ", 12);

            // Cột 2: Hành khách (Căn trái cho đẹp tên)
            var colName = AddColumn("PassengerName", "HÀNH KHÁCH", 18);
            colName.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colName.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // Cột 3: Chuyến bay
            AddColumn("FlightCode", "CHUYẾN BAY", 10);

            // Cột 4: Lộ trình (Gộp Đi/Đến cho gọn hoặc để riêng tùy ý)
            AddColumn("DepartureAirport", "ĐI", 8);
            AddColumn("ArrivalAirport", "ĐẾN", 8);

            // Cột 5: Ngày giờ
            var colDate = AddColumn("DepartureTime", "KHỞI HÀNH", 15);
            colDate.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

            // Cột 6: Ghế
            var colSeat = AddColumn("SeatCode", "GHẾ", 8);
            colSeat.DefaultCellStyle.ForeColor = Color.SeaGreen;
            colSeat.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // Cột 7: Trạng thái
            AddColumn("Status", "TRẠNG THÁI", 10);

            // Cột 8: Hành lý
            var colBag = AddColumn("BaggageSummary", "HÀNH LÝ", 11);
            colBag.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        // Helper thêm cột nhanh
        private DataGridViewTextBoxColumn AddColumn(string propertyName, string headerText, float weight)
        {
            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = propertyName,
                HeaderText = headerText,
                FillWeight = weight // <--- Dùng cái này để chia tỷ lệ %
            };
            dgvTickets.Columns.Add(col);
            return col;
        }

        // ... (Giữ nguyên các hàm InitStatusCombo, LoadTicketHistory, btnFilter_click, ApplyFilter) ...

        private void InitStatusCombo()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Tất cả");
            cbStatus.Items.Add("BOOKED"); // Nên đồng bộ với Enum trong database nếu có
            cbStatus.Items.Add("CONFIRMED");
            cbStatus.Items.Add("CHECKED_IN");
            cbStatus.Items.Add("BOARDED");
            cbStatus.Items.Add("CANCELLED");
            cbStatus.Items.Add("REFUNDED");
            cbStatus.SelectedIndex = 0;
        }

        private void LoadTicketHistory()
        {
            _allTickets = _ticketBus.GetAll(_accountId);
            dgvTickets.DataSource = _allTickets;
        }

        private void btnFilter_click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allTickets == null) return;

            var result = _allTickets.AsEnumerable(); // Chuyển sang IEnumerable để query linh hoạt

            // 1. Lọc theo Mã vé
            string ticketNumber = txtTicketNumber.Text.Trim();
            if (!string.IsNullOrWhiteSpace(ticketNumber))
            {
                result = result.Where(c => c.TicketNumber != null &&
                                           c.TicketNumber.Contains(ticketNumber, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Lọc theo Trạng thái
            if (cbStatus.SelectedItem != null)
            {
                string status = cbStatus.SelectedItem.ToString();
                if (status != "Tất cả")
                {
                    result = result.Where(c => c.Status != null &&
                                               c.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
                }
            }

            // 3. Lọc theo ngày bay
            // Reset giờ về 00:00:00 và 23:59:59 để so sánh ngày chính xác
            DateTime from = dtFrom.Value.Date;
            DateTime to = dtTo.Value.Date.AddDays(1).AddTicks(-1);

            result = result.Where(c => c.DepartureTime >= from && c.DepartureTime <= to);

            dgvTickets.DataSource = result.ToList();
        }

        private void tableCustom1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}