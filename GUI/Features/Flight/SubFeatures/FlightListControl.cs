﻿using BUS.Flight;
using DAO.Airport;
using DAO.CabinClass;
using DAO.Flight;
using System.Data;
using GUI.MainApp;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Flight.SubFeatures
{
    public partial class FlightListControl : UserControl
    {
        private DataTable _airportData;
        private bool _isUpdatingComboBoxes = false;
        private readonly AppRole _role;
        public event Action<int> OnBookFlightRequested;

        public FlightListControl() : this(AppRole.Admin)
        {
        }
        public FlightListControl(AppRole role)
        {
            _role = role;
            InitializeComponent();
        }
        #region Data Loading
        private void LoadFlightData()
        {
            try
            {
                var result = FlightBUS.Instance.SearchFlightsForDisplay(
                    null, null, dateTimeNgayDi.Value, null);

                if (result.Success)
                {
                    danhSachChuyenBay.DataSource = result.GetData<DataTable>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chuyến bay: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FlightCreateControl_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;

            danhSachChuyenBay.AutoGenerateColumns = false;
            //Role Admin và Staff mới được xem cột Trạng thái
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

            LoadInitialData();
            LoadFlightData();
        }
        private void timChuyenBay_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Thu thập dữ liệu
                // Nếu không chọn, giá trị sẽ là DBNull.Value, và Convert sẽ trả về null
                int? departureAirportId = (noiCatCanh.SelectedValue != null && noiCatCanh.SelectedValue != DBNull.Value)
                    ? Convert.ToInt32(noiCatCanh.SelectedValue) : (int?)null;

                int? arrivalAirportId = (noiHaCanh.SelectedValue != null && noiHaCanh.SelectedValue != DBNull.Value)
                    ? Convert.ToInt32(noiHaCanh.SelectedValue) : (int?)null;

                int? cabinClassId = (cbHangVe.SelectedValue != null && cbHangVe.SelectedValue != DBNull.Value)
                    ? Convert.ToInt32(cbHangVe.SelectedValue) : (int?)null;

                DateTime departureDate = dateTimeNgayDi.Value;
                bool isRoundTrip = (khuHoi_MotChieu.SelectedItem?.ToString() == "Khứ hồi");

                // 2. Gọi FlightBUS
                var result = FlightBUS.Instance.SearchFlightsForDisplay(
                    departureAirportId,
                    arrivalAirportId,
                    departureDate,
                    cabinClassId
                );

                // 3. Hiển thị kết quả
                if (result.Success)
                {
                    DataTable dtFlights = result.GetData<DataTable>();
                    danhSachChuyenBay.DataSource = dtFlights;

                    if (dtFlights.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy chuyến bay nào phù hợp với tiêu chí của bạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (isRoundTrip)
                    {
                        // TODO: Xử lý tìm chuyến về
                    }
                }
                else
                {
                    MessageBox.Show(result.GetFullErrorMessage(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    danhSachChuyenBay.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không mong muốn: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadInitialData()
        {
            try
            {
                _isUpdatingComboBoxes = true; 

                // 1. Tải Hạng vé (Giữ nguyên từ bước trước)
                DataTable dtCabinClasses = CabinClassDAO.Instance.GetAllCabinClasses();
                DataRow allCabinRow = dtCabinClasses.NewRow();
                allCabinRow["class_id"] = DBNull.Value;
                allCabinRow["class_name"] = "Tất cả hạng vé";
                dtCabinClasses.Rows.InsertAt(allCabinRow, 0);
                cbHangVe.DataSource = dtCabinClasses;
                cbHangVe.DisplayMember = "class_name";
                cbHangVe.ValueMember = "class_id";
                cbHangVe.SelectedValue = 1;

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
                    MessageBox.Show($"(Demo) Admin: Xem chi tiết chuyến bay ID: {flightId} ({flightNumber})", "Xem");
                }
                else if (rcEdit.Contains(clickLocation))
                {
                    MessageBox.Show($"(Demo) Admin: Sửa chuyến bay ID: {flightId}", "Sửa");
                }
                else if (rcDel.Contains(clickLocation))
                {
                    MessageBox.Show($"(Demo) Admin: Xóa chuyến bay ID: {flightId}", "Xóa");
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
    }
}