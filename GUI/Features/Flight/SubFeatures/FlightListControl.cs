using BUS.Flight;
using DAO.Airport;
using DAO.CabinClass;
using DAO.Flight;
using System.Data;
using GUI.MainApp;
using System.Drawing;
using System.Windows.Forms;
<<<<<<< Updated upstream

namespace GUI.Features.Flight.SubFeatures
{
    public partial class FlightListControl : UserControl
    {
        private DataTable _airportData;
        private bool _isUpdatingComboBoxes = false;
        private readonly AppRole _role;
        public event Action<int> OnBookFlightRequested;
        public event Action<int> OnViewFlightDetailRequested;
        public event Action<int> OnEditFlightRequested;

        public FlightListControl() : this(AppRole.Admin)
        {
        }
        public FlightListControl(AppRole role)
        {
            _role = role;
=======
using System.Collections.Generic;
using System.Linq;
using GUI.Components.Buttons;
using GUI.Features.Flight;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using BUS.Flight;
using BUS.Route;
using BUS.Airport;
using BUS.Aircraft;
using DTO.Flight;
using DTO.Route;
using DTO.Airport;

namespace GUI.Features.Flight.SubFeatures {
    public class FlightListControl : UserControl {
        private readonly FlightBUS _flightBUS;
        private readonly RouteBUS _routeBUS;
        private readonly AirportBUS _airportBUS;
        
        private TableCustom table;

        private const string ACTION_COL_NAME = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DELETE = "Xóa";
        private const string SEP = " / ";

        private TableLayoutPanel mainPanel;
        private TableLayoutPanel filterWrapPanel;
        private FlowLayoutPanel filterPanel;
        private FlowLayoutPanel btnPanel;
        private Label lblTitle;
        private UnderlinedTextField txtFlightNumber;
        private DateTimePickerCustom dtpFromDate;
        private DateTimePickerCustom dtpToDate;
        private UnderlinedComboBox cboStatus;

        private Dictionary<int, RouteDTO> _routeCache = new Dictionary<int, RouteDTO>();
        private Dictionary<int, string> _airportNameCache = new Dictionary<int, string>();

        public FlightListControl() {
            _flightBUS = new FlightBUS();
            _routeBUS = new RouteBUS();
            _airportBUS = new AirportBUS();
            
>>>>>>> Stashed changes
            InitializeComponent();
            LoadAirportNames();
            RefreshFlightList();
        }
        #region Data Loading
        public void LoadFlightData()
        {
            try
            {
                DateTime? startDate = (_role == AppRole.Admin || _role == AppRole.Staff) ? (DateTime?)null : DateTime.Today;

                var result = FlightBUS.Instance.SearchFlightsForDisplay(
                    null,       // flightNumber
                    null,       // departureAirportId
                    null,       // arrivalAirportId
                    startDate,  // departureDate
                    null,       // cabinClassId
                    null        // status
                );

<<<<<<< Updated upstream
                if (result.Success)
                {
                    danhSachChuyenBay.DataSource = null; // 1. Hủy binding cũ
                    danhSachChuyenBay.DataSource = result.GetData<DataTable>(); // 2. Gán binding mới
                    danhSachChuyenBay.Refresh(); // 3. Yêu cầu vẽ lại ngay lập tức
                }
                else
                {
                    throw new Exception(result.GetFullErrorMessage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chuyến bay: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
=======
            // ===== Title =====
            lblTitle = new Label {
                Text = "✈️ Danh sách chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Filter row =====
            filterPanel = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            txtFlightNumber = new UnderlinedTextField("Số hiệu chuyến bay", "") {
                Width = 200,
                Margin = new Padding(0, 0, 16, 0)
            };

            dtpFromDate = new DateTimePickerCustom("Từ ngày", "") {
                Width = 220,
                Margin = new Padding(0, 0, 16, 0),
                EnableTime = false,
                TimeFormat = "dd/MM/yyyy"
            };

            dtpToDate = new DateTimePickerCustom("Đến ngày", "") {
                Width = 220,
                Margin = new Padding(0, 0, 16, 0),
                EnableTime = false,
                TimeFormat = "dd/MM/yyyy"
            };

            cboStatus = new UnderlinedComboBox("Trạng thái", new object[] { 
                "Tất cả", "SCHEDULED", "DELAYED", "CANCELLED", "COMPLETED" 
            }) {
                Width = 200,
                Margin = new Padding(0, 0, 16, 0)
            };
            cboStatus.SelectedIndex = 0; // Mặc định "Tất cả"

            filterPanel.Controls.AddRange(new Control[] { txtFlightNumber, dtpFromDate, dtpToDate, cboStatus });

            btnPanel = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
            var btnSearchFlight = new PrimaryButton("🔍 Tìm kiếm") {
                Width = 120,
                Height = 36,
                Margin = new Padding(0, 0, 8, 0)
            };
            btnSearchFlight.Click += (s, e) => RefreshFlightList();

            var btnRefresh = new SecondaryButton("🔄 Làm mới") {
                Width = 120,
                Height = 36,
                Margin = new Padding(0, 0, 0, 0)
            };
            btnRefresh.Click += (s, e) => {
                txtFlightNumber.Text = "";
                dtpFromDate.Value = DateTime.Now.AddDays(-7);
                dtpToDate.Value = DateTime.Now.AddDays(30);
                cboStatus.SelectedIndex = 0;
                RefreshFlightList();
            };

            btnPanel.Controls.Add(btnRefresh);
            btnPanel.Controls.Add(btnSearchFlight);

            filterWrapPanel = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 16, 24, 0),
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1
            };
            filterWrapPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            filterWrapPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filterWrapPanel.Controls.Add(filterPanel, 0, 0);
            filterWrapPanel.Controls.Add(btnPanel, 1, 0);

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

            // Cột hiển thị (khớp DB + nghiệp vụ)
            table.Columns.Add("flightNumber", "Mã chuyến bay");
            table.Columns.Add("fromAirport", "Nơi cất cánh");
            table.Columns.Add("toAirport", "Nơi hạ cánh");
            table.Columns.Add("departureTime", "Giờ cất cánh");
            table.Columns.Add("arrivalTime", "Giờ hạ cánh");
            table.Columns.Add("status", "Trạng thái");
            table.Columns.Add("seatAvailable", "Số ghế trống");

            // Cột Thao tác (vẽ custom link)
            var colAction = new DataGridViewTextBoxColumn {
                Name = ACTION_COL_NAME,
                HeaderText = "Thao tác",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            table.Columns.Add(colAction);

            // Khóa kỹ thuật ẩn: flight_id
            var colIdHidden = new DataGridViewTextBoxColumn {
                Name = "flightIdHidden",
                HeaderText = "",
                Visible = false
            };
            table.Columns.Add(colIdHidden);

            // Event handlers cho cột thao tác
            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // ===== Main panel =====
            mainPanel = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 3
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // Title
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // Filter
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Table

            mainPanel.Controls.Add(lblTitle, 0, 0);
            mainPanel.Controls.Add(filterWrapPanel, 0, 1);
            mainPanel.Controls.Add(table, 0, 2);

            Controls.Clear();
            Controls.Add(mainPanel);

            ResumeLayout(false);
        }

        // === Load & Refresh Data ===
        private void LoadAirportNames()
        {
            try
            {
                var airports = _airportBUS.GetAllAirports();
                _airportNameCache = airports.ToDictionary(a => a.AirportId, a => a.AirportName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sân bay: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshFlightList()
        {
            try
            {
                // Lấy tất cả chuyến bay
                var flights = _flightBUS.GetAllFlights();

                // Áp dụng filter
                string searchNumber = txtFlightNumber?.Text?.Trim() ?? "";
                if (!string.IsNullOrEmpty(searchNumber))
                {
                    flights = flights.Where(f => f.FlightNumber.ToLower().Contains(searchNumber.ToLower())).ToList();
                }

                // Lọc theo ngày
                if (dtpFromDate?.Value != null && dtpToDate?.Value != null)
                {
                    DateTime fromDate = dtpFromDate.Value.Date;
                    DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1); // Cuối ngày

                    flights = flights.Where(f => f.DepartureTime.HasValue && 
                        f.DepartureTime.Value >= fromDate && 
                        f.DepartureTime.Value <= toDate).ToList();
                }

                // Lọc theo trạng thái
                if (cboStatus?.SelectedIndex > 0) // 0 = "Tất cả"
                {
                    string statusStr = cboStatus.SelectedItem.ToString();
                    FlightStatus status = FlightStatusExtensions.Parse(statusStr);
                    flights = flights.Where(f => f.Status == status).ToList();
                }

                // Clear và load lại table
                table.Rows.Clear();
                
                foreach (var flight in flights)
                {
                    // Lấy thông tin route
                    RouteDTO route = null;
                    if (!_routeCache.ContainsKey(flight.RouteId))
                    {
                        try
                        {
                            route = _routeBUS.GetRouteById(flight.RouteId);
                            _routeCache[flight.RouteId] = route;
                        }
                        catch { }
                    }
                    else
                    {
                        route = _routeCache[flight.RouteId];
                    }

                    string fromAirport = "N/A";
                    string toAirport = "N/A";
                    
                    if (route != null)
                    {
                        fromAirport = _airportNameCache.ContainsKey(route.DeparturePlaceId) 
                            ? _airportNameCache[route.DeparturePlaceId] 
                            : $"ID: {route.DeparturePlaceId}";
                        
                        toAirport = _airportNameCache.ContainsKey(route.ArrivalPlaceId) 
                            ? _airportNameCache[route.ArrivalPlaceId] 
                            : $"ID: {route.ArrivalPlaceId}";
                    }

                    string departureTime = flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
                    string arrivalTime = flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
                    string status = flight.Status.GetDescription();

                    table.Rows.Add(
                        flight.FlightNumber,
                        fromAirport,
                        toAirport,
                        departureTime,
                        arrivalTime,
                        status,
                        "N/A", // Seat available - cần query thêm từ flight_seats
                        null,  // Action column
                        flight.FlightId // Hidden ID
                    );
                }

                table.InvalidateColumn(table.Columns[ACTION_COL_NAME].Index);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách chuyến bay: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // === Helpers for Action column ===
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDelete) GetActionRects(Rectangle cellBounds, Font font) {
            int padding = 6;
            int x = cellBounds.Left + padding;
            int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;

            var flags = TextFormatFlags.NoPadding;
            var szView = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szSep = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szEdit = TextRenderer.MeasureText(TXT_EDIT, font, Size.Empty, flags);
            var szDel = TextRenderer.MeasureText(TXT_DELETE, font, Size.Empty, flags);

            var rcView = new Rectangle(new Point(x, y), szView); x += szView.Width + szSep.Width;
            var rcEdit = new Rectangle(new Point(x, y), szEdit); x += szEdit.Width + szSep.Width;
            var rcDel = new Rectangle(new Point(x, y), szDel);

            return (rcView, rcEdit, rcDel);
        }

        private void Table_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) return;

            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var font = e.CellStyle.Font ?? table.Font;
            var rects = GetActionRects(e.CellBounds, font);

            Color link = Color.FromArgb(0, 92, 175);
            Color sep = Color.FromArgb(120, 120, 120);
            Color del = Color.FromArgb(220, 53, 69);

            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, rects.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rects.rcView.Right, rects.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, rects.rcEdit.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rects.rcEdit.Right, rects.rcEdit.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_DELETE, font, rects.rcDelete.Location, del, TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object? sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) { table.Cursor = Cursors.Default; return; }

            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(cellRect, font);

            var p = new Point(e.Location.X + cellRect.Left, e.Location.Y + cellRect.Top);
            bool over = rects.rcView.Contains(p) || rects.rcEdit.Contains(p) || rects.rcDelete.Contains(p);
            table.Cursor = over ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) return;

            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(cellRect, font);
            var p = new Point(e.Location.X + cellRect.Left, e.Location.Y + cellRect.Top);

            var row = table.Rows[e.RowIndex];

            string flightId = row.Cells["flightIdHidden"].Value?.ToString() ?? string.Empty;
            string flightNumber = row.Cells["flightNumber"].Value?.ToString() ?? "(n/a)";
            string fromAirport = row.Cells["fromAirport"].Value?.ToString() ?? "(n/a)";
            string toAirport = row.Cells["toAirport"].Value?.ToString() ?? "(n/a)";
            string departureTime = row.Cells["departureTime"].Value?.ToString() ?? "(n/a)";
            string arrivalTime = row.Cells["arrivalTime"].Value?.ToString() ?? "(n/a)";
            string seatAvailable = row.Cells["seatAvailable"].Value?.ToString() ?? "(n/a)";

            if (rects.rcView.Contains(p)) {
                // Hiển thị chi tiết
                if (int.TryParse(flightId, out int id)) {
                    using (var frm = new FlightDetailForm(id)) {
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.DataChanged += () => RefreshFlightList(); // Refresh khi có thay đổi
                        frm.ShowDialog(FindForm());
                    }
                }
            } else if (rects.rcEdit.Contains(p)) {
                // Mở form sửa chuyến bay
                if (int.TryParse(flightId, out int id)) {
                    using (var editForm = new FlightEditForm(id)) {
                        editForm.StartPosition = FormStartPosition.CenterParent;
                        if (editForm.ShowDialog(FindForm()) == DialogResult.OK) {
                            RefreshFlightList(); // Refresh sau khi sửa thành công
                        }
                    }
                }
            } else if (rects.rcDelete.Contains(p)) {
                // Xóa chuyến bay
                var confirmResult = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa chuyến bay '{flightNumber}'?\n\nLưu ý: Chỉ có thể xóa nếu không có dữ liệu liên quan.",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    if (int.TryParse(flightId, out int id))
                    {
                        if (_flightBUS.DeleteFlight(id, out string message))
                        {
                            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshFlightList(); // Refresh lại danh sách
                        }
                        else
                        {
                            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
>>>>>>> Stashed changes
            }
        }
        private void FlightListControl_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;

<<<<<<< Updated upstream
            danhSachChuyenBay.AutoGenerateColumns = false;

            if (danhSachChuyenBay.Columns.Contains("Column6"))
            {
                bool canViewStatus = (_role == AppRole.Admin || _role == AppRole.Staff);
                danhSachChuyenBay.Columns["Column6"].Visible = canViewStatus;
            }
            if (!danhSachChuyenBay.Columns.Contains("flight_id_hidden"))
            {
                danhSachChuyenBay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "flight_id_hidden",
                    DataPropertyName = "flight_id",
                    Visible = false
                });
            }

            danhSachChuyenBay.CellMouseClick -= Table_CellMouseClick;
            danhSachChuyenBay.CellMouseClick += Table_CellMouseClick;
            danhSachChuyenBay.CellPainting -= Table_CellPainting;
            danhSachChuyenBay.CellPainting += Table_CellPainting;
            danhSachChuyenBay.CellMouseMove -= Table_CellMouseMove;
            danhSachChuyenBay.CellMouseMove += Table_CellMouseMove;

            if (_role == AppRole.User)
            {
                textFieldMaChuyenBay.Visible = false;
                checkBoxTimKiemMaChuyenBay.Visible = false;
                checkBoxTimKiemMaChuyenBay.Checked = false;
                checkBoxTimKiemMaChuyenBay.Enabled = false;
            }

            LoadFlightData();

            LoadInitialData();

            HookEnterKeyEvents();
        }
        private void timChuyenBay_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Lấy mã chuyến bay (nếu có)
                string? flightNumber = string.IsNullOrWhiteSpace(textFieldMaChuyenBay.Text)
                    ? null
                    : textFieldMaChuyenBay.Text.Trim();

                // 2. Lấy sân bay đi (nếu có)
                int? depId = (noiCatCanh.SelectedValue != null && noiCatCanh.SelectedValue != DBNull.Value)
                    ? Convert.ToInt32(noiCatCanh.SelectedValue)
                    : (int?)null;

                // 3. Lấy sân bay đến (nếu có)
                int? arrId = (noiHaCanh.SelectedValue != null && noiHaCanh.SelectedValue != DBNull.Value)
                    ? Convert.ToInt32(noiHaCanh.SelectedValue)
                    : (int?)null;

                // 4. Lấy hạng vé (nếu có)
                int? cabinClassId = (cbHangVe.SelectedValue != null && cbHangVe.SelectedValue != DBNull.Value)
                    ? Convert.ToInt32(cbHangVe.SelectedValue)
                    : (int?)null;

                // 5. Lấy ngày đi
                bool allTime = checkBoxTimKiemMaChuyenBay.Checked;
                // Nếu "Mọi thời điểm" được check -> ngày là null (áp dụng cho Admin/Staff tra cứu)
                // Ngược lại, lấy ngày đã chọn
                DateTime? depDate = allTime ? (DateTime?)null : dateTimeNgayDi.Value;

                // 5b. Lấy trạng thái (nếu có)
                string? statusFilter = null;
                if (cbTrangThai.SelectedValue != null && cbTrangThai.SelectedValue != DBNull.Value)
                {
                    statusFilter = cbTrangThai.SelectedValue.ToString();
                }

                // 6. Gọi BUS (với các tham số đã cập nhật)
                var result = FlightBUS.Instance.SearchFlightsForDisplay(
                    flightNumber,
                    depId,
                    arrId,
                    depDate,
                    cabinClassId,
                    statusFilter
                );

                if (result.Success)
                {
                    // 7. Gán kết quả (là DataTable) cho bảng
                    danhSachChuyenBay.DataSource = result.GetData<DataTable>();

                    if (result.Data is DataTable dt && dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy chuyến bay nào phù hợp với tiêu chí của bạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // 8. Hiển thị lỗi nếu BUS trả về thất bại
                    MessageBox.Show(result.GetFullErrorMessage(), "Lỗi tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadInitialData()
        {
            try
            {
                _isUpdatingComboBoxes = true;

                // 1. Tải Hạng vé
                DataTable dtCabinClasses = CabinClassDAO.Instance.GetAllCabinClasses();
                DataRow allCabinRow = dtCabinClasses.NewRow();
                allCabinRow["class_id"] = DBNull.Value;
                allCabinRow["class_name"] = "Tất cả hạng vé";
                dtCabinClasses.Rows.InsertAt(allCabinRow, 0);
                cbHangVe.DataSource = dtCabinClasses;
                cbHangVe.DisplayMember = "class_name";
                cbHangVe.ValueMember = "class_id";
                cbHangVe.SelectedValue = 1;

                // 1b. Tải Trạng thái chuyến bay (Status)
                DataTable dtStatus = new DataTable();
                dtStatus.Columns.Add("status_value", typeof(string));
                dtStatus.Columns.Add("status_display", typeof(string));
                dtStatus.Rows.Add(DBNull.Value, "Tất cả trạng thái");
                dtStatus.Rows.Add("SCHEDULED", "Đã lên lịch");
                dtStatus.Rows.Add("DELAYED", "Bị hoãn");
                dtStatus.Rows.Add("CANCELLED", "Đã hủy");
                dtStatus.Rows.Add("COMPLETED", "Hoàn thành");

                cbTrangThai.DataSource = dtStatus;
                cbTrangThai.DisplayMember = "status_display";
                cbTrangThai.ValueMember = "status_value";
                cbTrangThai.SelectedIndex = 0;

                // 2. Tải danh sách Sân bay ĐI
                _airportData = AirportDAO.Instance.GetAllAirportsForComboBox();

                DataRow allAirportRow = _airportData.NewRow();
                allAirportRow["airport_id"] = DBNull.Value;
                allAirportRow["DisplayName"] = "(Tất cả)";
                _airportData.Rows.InsertAt(allAirportRow, 0);

                // 3. Gán dữ liệu cho ComboBox "Nơi cất cánh"
                noiCatCanh.DataSource = _airportData.Copy();
                noiCatCanh.DisplayMember = "DisplayName";
                noiCatCanh.ValueMember = "airport_id";
                noiCatCanh.SelectedIndex = 0;

                // 4. Vô hiệu hóa ComboBox "Nơi hạ cánh" ban đầu
                noiHaCanh.DataSource = null;
                noiHaCanh.Enabled = false;
                noiHaCanh.SelectedIndex = -1;

                // 5. Gán dữ liệu cho ComboBox "Hành trình bay"
                khuHoi_MotChieu.Items.Clear();
                khuHoi_MotChieu.Items.Add("Một chiều");
                khuHoi_MotChieu.Items.Add("Khứ hồi");

                // 6. Gán sự kiện
                khuHoi_MotChieu.SelectedIndexChanged -= khuHoi_MotChieu_SelectedIndexChanged;
                khuHoi_MotChieu.SelectedIndexChanged += khuHoi_MotChieu_SelectedIndexChanged;

                // 7. Đặt giá trị mặc định
                khuHoi_MotChieu.SelectedIndex = 0;
                UpdateNgayVeStatus();

                // 8. Phân quyền Admin (Giữ nguyên)
                if (_role != AppRole.Admin)
                {
                    dateTimeNgayDi.MinDate = DateTime.Today;
                    dateTimeNgayVe.MinDate = DateTime.Today;
                }

                // 9. Gán sự kiện
                dateTimeNgayDi.DateTimePicker.ValueChanged -= dateTimeNgayDi_ValueChanged;
                dateTimeNgayDi.DateTimePicker.ValueChanged += dateTimeNgayDi_ValueChanged;

                noiCatCanh.SelectedIndexChanged -= noiCatCanh_SelectedIndexChanged;

                noiCatCanh.SelectedIndexChanged += noiCatCanh_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu ban đầu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isUpdatingComboBoxes = false;
            }
        }

        private void noiCatCanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isUpdatingComboBoxes) return; // Bỏ qua nếu đang tải/cập nhật
            _isUpdatingComboBoxes = true;

            try
            {
                // Lấy vị trí (index) của mục được chọn
                int selectedIndex = noiCatCanh.SelectedIndex;

                // Dựa trên logic LoadInitialData, index 0 là "(Không chọn / Tất cả)"
                // Index -1 là chưa chọn gì
                if (selectedIndex <= 0)
                {
                    // Trường hợp chọn "(Không chọn / Tất cả)" hoặc chưa chọn
                    // -> Vô hiệu hóa và xóa điểm đến
                    noiHaCanh.DataSource = null;
                    noiHaCanh.Enabled = false;
                    noiHaCanh.SelectedIndex = -1;
                }
                else
                {
                    // Trường hợp đã chọn một sân bay đi cụ thể (index > 0)

                    // Lấy giá trị ID từ SelectedValue một cách an toàn
                    object selectedValue = noiCatCanh.SelectedValue;
                    if (selectedValue == null || selectedValue == DBNull.Value)
                    {
                        throw new Exception("Không thể lấy được ID của sân bay đi đã chọn.");
                    }

                    int departureId = Convert.ToInt32(selectedValue);

                    // Lấy danh sách điểm đến hợp lệ từ DAO
                    DataTable dtArrivals = AirportDAO.Instance.GetArrivalAirportsByDeparture(departureId);

                    // Thêm "(Tất cả điểm đến)" vào đầu danh sách
                    DataRow allRow = dtArrivals.NewRow();
                    allRow["airport_id"] = DBNull.Value;
                    allRow["DisplayName"] = "(Tất cả điểm đến)";
                    dtArrivals.Rows.InsertAt(allRow, 0);

                    // Cập nhật ComboBox "Nơi hạ cánh"
                    noiHaCanh.DataSource = dtArrivals;
                    noiHaCanh.DisplayMember = "DisplayName";
                    noiHaCanh.ValueMember = "airport_id";
                    noiHaCanh.SelectedIndex = 0; // Chọn "(Tất cả điểm đến)"

                    // !! QUAN TRỌNG: Kích hoạt ComboBox "Nơi hạ cánh"
                    noiHaCanh.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sân bay đến: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Nếu có lỗi, đảm bảo ComboBox "Đến" bị vô hiệu hóa
                noiHaCanh.DataSource = null;
                noiHaCanh.Enabled = false;
                noiHaCanh.SelectedIndex = -1;
            }
            finally
            {
                _isUpdatingComboBoxes = false; // Mở khóa
            }
        }
        #endregion

        #region Helper
        // Helper vẽ các nút cho Admin/Staff
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDel) GetAdminRects(Rectangle b, Font f)
        {
            int pad = 6, x = pad, y = (b.Height - f.Height) / 2;

            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText("Xem", f, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(" / ", f, Size.Empty, flags);
            var szE = TextRenderer.MeasureText("Sửa", f, Size.Empty, flags);
            var szD = TextRenderer.MeasureText("Xóa", f, Size.Empty, flags);
            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcE = new Rectangle(new Point(x, y), szE); x += szE.Width + szS.Width;
            var rcD = new Rectangle(new Point(x, y), szD);
            return (rcV, rcE, rcD);
        }
        // Helper vẽ nút cho User
        private Rectangle GetUserRects(Rectangle b, Font f)
        {
            int pad = 6, x = pad, y = (b.Height - f.Height) / 2;

            var flags = TextFormatFlags.NoPadding;
            var szB = TextRenderer.MeasureText("Đặt vé", f, Size.Empty, flags);
            return new Rectangle(new Point(x, y), szB);
        }
        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (danhSachChuyenBay.Columns[e.ColumnIndex].Name == "Column8")
            {
                e.Handled = true;
                e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                var font = e.CellStyle.Font ?? danhSachChuyenBay.Font;

                var (rcView, rcEdit, rcDel) = GetAdminRects(e.CellBounds, font);
                var rcBook = GetUserRects(e.CellBounds, font);

                int x = e.CellBounds.Left;
                int y = e.CellBounds.Top;

                if (_role == AppRole.Admin || _role == AppRole.Staff)
                {
                    TextRenderer.DrawText(e.Graphics, "Xem", font, new Point(x + rcView.Left, y + rcView.Top), Color.FromArgb(0, 92, 175), TextFormatFlags.NoPadding);
                    TextRenderer.DrawText(e.Graphics, " / ", font, new Point(x + rcEdit.Left - 10, y + rcView.Top), Color.Gray, TextFormatFlags.NoPadding);
                    TextRenderer.DrawText(e.Graphics, "Sửa", font, new Point(x + rcEdit.Left, y + rcEdit.Top), Color.FromArgb(0, 92, 175), TextFormatFlags.NoPadding);
                    TextRenderer.DrawText(e.Graphics, " / ", font, new Point(x + rcDel.Left - 10, y + rcEdit.Top), Color.Gray, TextFormatFlags.NoPadding);
                    TextRenderer.DrawText(e.Graphics, "Xóa", font, new Point(x + rcDel.Left, y + rcDel.Top), Color.FromArgb(220, 53, 69), TextFormatFlags.NoPadding);
                }
                else if (_role == AppRole.User)
                {
                    TextRenderer.DrawText(e.Graphics, "Đặt vé", font, new Point(x + rcBook.Left, y + rcBook.Top), Color.FromArgb(0, 92, 175), TextFormatFlags.NoPadding);
                }
            }
        }
        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { danhSachChuyenBay.Cursor = Cursors.Default; return; }
            if (danhSachChuyenBay.Columns[e.ColumnIndex].Name != "Column8") { danhSachChuyenBay.Cursor = Cursors.Default; return; }

            var rect = danhSachChuyenBay.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = danhSachChuyenBay[e.ColumnIndex, e.RowIndex].InheritedStyle.Font ?? danhSachChuyenBay.Font;

            var clickLocation = e.Location;
            bool isOver = false;

            if (_role == AppRole.Admin || _role == AppRole.Staff)
            {
                var (rcView, rcEdit, rcDel) = GetAdminRects(rect, font);
                isOver = rcView.Contains(clickLocation) || rcEdit.Contains(clickLocation) || rcDel.Contains(clickLocation);
            }
            else if (_role == AppRole.User)
            {
                var rcBook = GetUserRects(rect, font);
                isOver = rcBook.Contains(clickLocation);
            }
            danhSachChuyenBay.Cursor = isOver ? Cursors.Hand : Cursors.Default;
        }
        #endregion
        private void dateTimeNgayDi_Load(object sender, EventArgs e)
        {

        }

        private void dateTimeNgayVe_Load(object sender, EventArgs e)
        {

        }

        private void noiCatCanh_Load(object sender, EventArgs e)
        {
        }

        private void noiHaCanh_Load(object sender, EventArgs e)
        {
        }
        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (e.Button != MouseButtons.Left) return;

            if (danhSachChuyenBay.Columns[e.ColumnIndex].Name != "Column8") return;

            var rect = danhSachChuyenBay.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = danhSachChuyenBay[e.ColumnIndex, e.RowIndex].InheritedStyle.Font ?? danhSachChuyenBay.Font;

            var clickLocation = e.Location;

            var flightId = Convert.ToInt32(danhSachChuyenBay.Rows[e.RowIndex].Cells["flight_id_hidden"].Value);
            if (flightId == 0) return;

            string flightNumber = danhSachChuyenBay.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
            string departure = danhSachChuyenBay.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
            string arrival = danhSachChuyenBay.Rows[e.RowIndex].Cells["Column3"].Value.ToString();

            if (_role == AppRole.Admin || _role == AppRole.Staff)
            {
                var (rcView, rcEdit, rcDel) = GetAdminRects(rect, font);

                if (rcView.Contains(clickLocation))
                {
                    OnViewFlightDetailRequested?.Invoke(flightId);
                }
                else if (rcEdit.Contains(clickLocation))
                {
                    OnEditFlightRequested?.Invoke(flightId);
                }
                else if (rcDel.Contains(clickLocation))
                {
                    // 1. Hiển thị hộp thoại xác nhận
                    string confirmMessage = $"Bạn có chắc chắn muốn xóa chuyến bay này?\n\n" +
                                            $"Mã chuyến bay: {flightNumber}\n" +
                                            $"Từ: {departure} Đến: {arrival}\n\n" +
                                            "Lưu ý: Hành động này không thể hoàn tác.";

                    var confirmResult = MessageBox.Show(confirmMessage,
                                                        "Xác nhận xóa chuyến bay",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Warning);

                    if (confirmResult == DialogResult.Yes)
                    {
                        // 2. Gọi BUS để xóa
                        var deleteResult = FlightBUS.Instance.DeleteFlight(flightId);

                        // 3. Xử lý kết quả
                        if (deleteResult.Success)
                        {
                            MessageBox.Show(deleteResult.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // 4. Tải lại danh sách
                            LoadFlightData();
                        }
                        else
                        {
                            MessageBox.Show(deleteResult.GetFullErrorMessage(), "Xóa thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else if (_role == AppRole.User)
            {
                var rcBook = GetUserRects(rect, font);

                if (rcBook.Contains(clickLocation))
                {
                    string message = $"Bạn có chắc chắn muốn đặt vé cho chuyến bay:\n\n" +
                                     $"Mã chuyến bay: {flightNumber}\n" +
                                     $"Từ: {departure}\n" +
                                     $"Đến: {arrival}";

                    var confirmResult = MessageBox.Show(
                        message,
                        "Xác nhận đặt vé",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmResult == DialogResult.Yes)
                    {
                        OnBookFlightRequested?.Invoke(flightId);
                    }
                }
            }
        }
        private void khuHoi_MotChieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateNgayVeStatus();
        }
        private void UpdateNgayVeStatus()
        {
            string selectedValue = khuHoi_MotChieu.SelectedItem?.ToString() ?? "";

            if (selectedValue == "Một chiều")
            {
                dateTimeNgayVe.Enabled = false;
            }
            else // "Khứ hồi"
            {
                dateTimeNgayVe.Enabled = true;
            }
        }
        private void dateTimeNgayDi_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDepartureDate = dateTimeNgayDi.Value;
            dateTimeNgayVe.MinDate = selectedDepartureDate;

            if (dateTimeNgayVe.Value < selectedDepartureDate)
            {
                dateTimeNgayVe.Value = selectedDepartureDate;
            }
        }
        private void HookEnterKeyEvents()
        {
            KeyEventHandler enterHandler = (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    timChuyenBay_Click(sender, e);

                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            };
            // 1. Mã chuyến bay (dùng thuộc tính InnerTextBox)
            textFieldMaChuyenBay.InnerTextBox.KeyDown += enterHandler;
            // 2. Nơi cất cánh (dùng thuộc tính ComboBox ta vừa tạo)
            noiCatCanh.ComboBox.KeyDown += enterHandler;
            // 3. Nơi hạ cánh
            noiHaCanh.ComboBox.KeyDown += enterHandler;
            // 4. Hạng vé
            cbHangVe.ComboBox.KeyDown += enterHandler;
            // 5. Ngày đi (dùng thuộc tính DateTimePicker)
            dateTimeNgayDi.DateTimePicker.KeyDown += enterHandler;
            // 6. Ngày về
            dateTimeNgayVe.DateTimePicker.KeyDown += enterHandler;
        }
        private void textFieldMaChuyenBay_Load(object sender, EventArgs e)
        {

        }

        private void checkBoxTimKiemMaChuyenBay_CheckedChanged(object sender, EventArgs e)
        {

=======
    // Popup form bọc FlightDetailControl + nạp dữ liệu
    internal class FlightDetailForm : Form {
        private FlightDetailControl _detail;
        public event Action? DataChanged;

        public FlightDetailForm(int flightId) {
            Text = $"Chi tiết chuyến bay";
            Size = new Size(900, 600);
            BackColor = Color.White;

            _detail = new FlightDetailControl { Dock = DockStyle.Fill };
            _detail.LoadFlightById(flightId);
            _detail.DataChanged += () => DataChanged?.Invoke();

            Controls.Add(_detail);
        }
    }

    // Form sửa chuyến bay
    internal class FlightEditForm : Form {
        private readonly FlightBUS _flightBUS;
        private readonly AircraftBUS _aircraftBUS;
        private readonly RouteBUS _routeBUS;
        private int _flightId;

        private UnderlinedTextField txtFlightNumber;
        private UnderlinedComboBox cboAircraft;
        private UnderlinedComboBox cboRoute;
        private DateTimePickerCustom dtpDeparture;
        private DateTimePickerCustom dtpArrival;
        private UnderlinedComboBox cboStatus;

        public FlightEditForm(int flightId) {
            _flightId = flightId;
            _flightBUS = new FlightBUS();
            _aircraftBUS = new AircraftBUS();
            _routeBUS = new RouteBUS();

            InitializeForm();
            LoadComboBoxData();
            LoadFlightData();
        }

        private void InitializeForm() {
            Text = "Sửa chuyến bay";
            Size = new Size(600, 550);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var titlePanel = new Panel {
                Dock = DockStyle.Top,
                Padding = new Padding(24, 20, 24, 0),
                Height = 60
            };
            var lblTitle = new Label {
                Text = "✏️ Sửa thông tin chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.Black
            };
            titlePanel.Controls.Add(lblTitle);

            var inputPanel = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 12, 24, 0),
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 6
            };
            for (int i = 0; i < 6; i++)
                inputPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));

            txtFlightNumber = new UnderlinedTextField("Số hiệu chuyến bay *", "") { 
                Width = 500,
                Enabled = false // Không cho sửa số hiệu
            };
            inputPanel.Controls.Add(txtFlightNumber, 0, 0);

            cboAircraft = new UnderlinedComboBox("Máy bay *", new object[] { }) {
                Width = 500
            };
            inputPanel.Controls.Add(cboAircraft, 0, 1);

            cboRoute = new UnderlinedComboBox("Tuyến bay *", new object[] { }) { 
                Width = 500
            };
            inputPanel.Controls.Add(cboRoute, 0, 2);

            dtpDeparture = new DateTimePickerCustom("Thời gian khởi hành *", "") { 
                Width = 500, 
                Height = 72,
                EnableTime = true,
                TimeFormat = "dd/MM/yyyy HH:mm",
                ShowUpDownWhenTime = true
            };
            inputPanel.Controls.Add(dtpDeparture, 0, 3);

            dtpArrival = new DateTimePickerCustom("Thời gian hạ cánh *", "") { 
                Width = 500, 
                Height = 72,
                EnableTime = true,
                TimeFormat = "dd/MM/yyyy HH:mm",
                ShowUpDownWhenTime = true
            };
            inputPanel.Controls.Add(dtpArrival, 0, 4);

            cboStatus = new UnderlinedComboBox("Trạng thái", new object[] { 
                "SCHEDULED", "DELAYED", "CANCELLED", "COMPLETED" 
            }) {
                Width = 500
            };
            inputPanel.Controls.Add(cboStatus, 0, 5);

            var buttonPanel = new FlowLayoutPanel {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(24, 12, 24, 24)
            };

            var btnSave = new PrimaryButton("💾 Lưu") {
                Height = 40,
                Width = 120
            };
            btnSave.Click += BtnSave_Click;

            var btnCancel = new SecondaryButton("❌ Hủy") {
                Height = 40,
                Width = 120,
                Margin = new Padding(0, 0, 8, 0)
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnCancel);

            Controls.Add(inputPanel);
            Controls.Add(buttonPanel);
            Controls.Add(titlePanel);
        }

        private void LoadComboBoxData() {
            try {
                var aircrafts = _aircraftBUS.GetAllAircrafts();
                cboAircraft.Items.Clear();
                foreach (var aircraft in aircrafts) {
                    cboAircraft.Items.Add(new ComboBoxItem {
                        Value = aircraft.AircraftId,
                        Text = $"{aircraft.Model} - {aircraft.Manufacturer}"
                    });
                }

                var routes = _routeBUS.GetAllRoutes();
                cboRoute.Items.Clear();
                foreach (var route in routes) {
                    cboRoute.Items.Add(new ComboBoxItem {
                        Value = route.RouteId,
                        Text = $"{route.DeparturePlaceId} → {route.ArrivalPlaceId}"
                    });
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFlightData() {
            try {
                var flight = _flightBUS.GetFlightById(_flightId);
                if (flight == null) {
                    MessageBox.Show("Không tìm thấy chuyến bay!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                txtFlightNumber.Text = flight.FlightNumber;

                // Chọn aircraft
                for (int i = 0; i < cboAircraft.Items.Count; i++) {
                    if (cboAircraft.Items[i] is ComboBoxItem item && 
                        (int)item.Value == flight.AircraftId) {
                        cboAircraft.SelectedIndex = i;
                        break;
                    }
                }

                // Chọn route
                for (int i = 0; i < cboRoute.Items.Count; i++) {
                    if (cboRoute.Items[i] is ComboBoxItem item && 
                        (int)item.Value == flight.RouteId) {
                        cboRoute.SelectedIndex = i;
                        break;
                    }
                }

                dtpDeparture.Value = flight.DepartureTime ?? DateTime.Now;
                dtpArrival.Value = flight.ArrivalTime ?? DateTime.Now.AddHours(2);

                string statusStr = flight.Status.ToString();
                int statusIdx = cboStatus.Items.IndexOf(statusStr);
                if (statusIdx >= 0) {
                    cboStatus.SelectedIndex = statusIdx;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu chuyến bay: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e) {
            // Validation
            if (string.IsNullOrWhiteSpace(txtFlightNumber.Text)) {
                MessageBox.Show("Vui lòng nhập số hiệu chuyến bay!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFlightNumber.Focus();
                return;
            }

            if (cboAircraft.SelectedItem == null) {
                MessageBox.Show("Vui lòng chọn máy bay!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboRoute.SelectedItem == null) {
                MessageBox.Show("Vui lòng chọn tuyến bay!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpDeparture.Value >= dtpArrival.Value) {
                MessageBox.Show("Thời gian hạ cánh phải sau thời gian khởi hành!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
                var aircraftItem = (ComboBoxItem)cboAircraft.SelectedItem;
                var routeItem = (ComboBoxItem)cboRoute.SelectedItem;
                string statusStr = cboStatus.SelectedItem?.ToString() ?? "SCHEDULED";

                var flight = new FlightDTO(
                    _flightId,
                    txtFlightNumber.Text.Trim(),
                    (int)aircraftItem.Value,
                    (int)routeItem.Value,
                    dtpDeparture.Value,
                    dtpArrival.Value,
                    FlightStatusExtensions.Parse(statusStr)
                );

                if (_flightBUS.UpdateFlight(flight, out string message)) {
                    MessageBox.Show(message, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                } else {
                    MessageBox.Show(message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class ComboBoxItem {
            public object Value { get; set; } = 0;
            public string Text { get; set; } = string.Empty;
            public override string ToString() => Text;
>>>>>>> Stashed changes
        }
    }
}