using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Tables;
using FlightTicketManagement.GUI.Features.Seat.SubFeatures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FlightTicketManagement.GUI.Features.Seat.SubFeatures {
    public class FlightSeatInventoryControl : UserControl {
        private Label _title;
        private FlowLayoutPanel _headerActions;

        // filter
        private UnderlinedTextField _txtFlight;
        private UnderlinedComboBox _cbRoute, _cbDate;
        private Button _btnLoad;

        // left flight list
        private TableCustom _tblFlights;
        private UnderlinedTextField _txtSearchFlight;

        // toolbar (right side)
        private UnderlinedComboBox _cbCabin, _cbStatus;
        private UnderlinedTextField _txtBasePrice;
        private Button _btnApplyCabin, _btnLockCabin, _btnApplyFare;

        // map + legend + inspector
        private FlightSeatMapPanel _map;
        private Panel _legend;
        private Panel _inspectorCard;
        private UnderlinedTextField _txtPrice, _txtPNR, _txtNote;
        private UnderlinedComboBox _cbFare, _cbSeatStatus;
        private Button _btnApplySeat;

        // bulk under inspector
        private UnderlinedTextField _txtRowFrom, _txtRowTo, _txtPattern;
        private Button _btnBulkPrice, _btnBulkBlock, _btnBulkUnblock;

        // state demo
        private readonly List<SeatVM> _seats = new();

        public FlightSeatInventoryControl() {
            InitializeComponent();
            SeedFlights();
            BuildLegend();
            WireEvents();
        }

        private void InitializeComponent() {
            SuspendLayout();
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            _title = new Label {
                Text = "📦 Tồn kho ghế theo chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };
            _headerActions = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(24, 8, 24, 0)
            };
            _headerActions.Controls.Add(new PrimaryButton("Lưu cập nhật") { Height = 36, Margin = new Padding(0, 0, 12, 0) });
            _headerActions.Controls.Add(new SecondaryButton("Hoàn tác") { Height = 36 });

            // Filter bar
            var filter = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(24, 8, 24, 0), WrapContents = false };
            _txtFlight = new UnderlinedTextField("Mã chuyến bay", "") { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            _cbRoute = new UnderlinedComboBox("Chặng", new object[] { "SGN-HAN", "HAN-SGN", "SGN-DAD" }) { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            _cbDate = new UnderlinedComboBox("Ngày bay", new object[] { "Hôm nay", "Ngày mai", "Tuần này" }) { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            _btnLoad = new PrimaryButton("Nạp ghế") { Height = 36 };
            filter.Controls.AddRange(new Control[] { _txtFlight, _cbRoute, _cbDate, _btnLoad });

            // columns layout
            var columns = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Padding = new Padding(24, 12, 24, 24) };
            columns.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360));
            columns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // left flights
            var left = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1 };
            left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            var leftTitle = new Label { Text = "Danh sách chuyến bay", AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            _txtSearchFlight = new UnderlinedTextField("Tìm chuyến bay", "") { Width = 320, Margin = new Padding(0, 6, 0, 6) };
            _tblFlights = new TableCustom {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                RowHeadersVisible = false
            };
            _tblFlights.Columns.Add("flightNo", "Flight");
            _tblFlights.Columns.Add("route", "Chặng");
            _tblFlights.Columns.Add("std", "STD");
            _tblFlights.Columns.Add("sta", "STA");
            var actCol = new DataGridViewTextBoxColumn { Name = "action", HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells };
            _tblFlights.Columns.Add(actCol);

            left.Controls.Add(leftTitle, 0, 0);
            left.Controls.Add(_txtSearchFlight, 0, 1);
            left.Controls.Add(_tblFlights, 0, 2);

            // right: top toolbar + map + inspector
            var right = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
            right.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            right.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360));
            right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 0, 0, 6), WrapContents = false };
            _cbCabin = new UnderlinedComboBox("Cabin", new object[] { "Tất cả", "Economy", "Premium Economy", "Business", "First" }) { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            _cbStatus = new UnderlinedComboBox("Trạng thái", new object[] { "All", "AVAILABLE", "BOOKED", "BLOCKED" }) { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            _txtBasePrice = new UnderlinedTextField("Giá base (₫)", "") { Width = 140, Margin = new Padding(0, 0, 24, 0) };
            _btnApplyCabin = new SecondaryButton("Áp dụng cho cabin") { Height = 32, Margin = new Padding(0, 0, 12, 0) };
            _btnLockCabin = new SecondaryButton("Khóa/Mở cabin") { Height = 32, Margin = new Padding(0, 0, 12, 0) };
            _btnApplyFare = new PrimaryButton("Áp dụng Fare Rules...") { Height = 32 };
            toolbar.Controls.AddRange(new Control[] { _cbCabin, _cbStatus, _txtBasePrice, _btnApplyCabin, _btnLockCabin, _btnApplyFare });

            _legend = new Panel { Dock = DockStyle.Top, Height = 28, BackColor = Color.Transparent, Margin = new Padding(0, 0, 0, 6) };
            _map = new FlightSeatMapPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Rows = 30,
                Pattern = "ABCDEF"
            };

            // inspector
            _inspectorCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(12) };
            var inspector = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1 };
            inspector.Controls.Add(new Label { Text = "Inspector", AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold), Margin = new Padding(0, 0, 0, 6) });
            _txtPrice = new UnderlinedTextField("Base price (₫)", "") { Width = 240, Margin = new Padding(0, 0, 0, 6) };
            _cbFare = new UnderlinedComboBox("Fare rule", new object[] { "—", "ECON-FLEX", "BUS-SAVER" }) { Width = 240, Margin = new Padding(0, 0, 0, 6) };
            _cbSeatStatus = new UnderlinedComboBox("Trạng thái", new object[] { "AVAILABLE", "BOOKED", "BLOCKED" }) { Width = 240, Margin = new Padding(0, 0, 0, 6) };
            _txtPNR = new UnderlinedTextField("PNR (nếu BOOKED)", "") { Width = 240, Margin = new Padding(0, 0, 0, 6) };
            _txtNote = new UnderlinedTextField("Ghi chú", "") { Width = 240, Margin = new Padding(0, 0, 0, 6) };
            _btnApplySeat = new PrimaryButton("Áp dụng") { Height = 32, Width = 100, Margin = new Padding(0, 4, 0, 6) };
            inspector.Controls.AddRange(new Control[] { _txtPrice, _cbFare, _cbSeatStatus, _txtPNR, _txtNote, _btnApplySeat });

            // bulk
            var bulkTitle = new Label { Text = "Cập nhật hàng loạt", AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), Margin = new Padding(0, 12, 0, 6) };
            _txtRowFrom = new UnderlinedTextField("Row từ", "1") { Width = 110, Margin = new Padding(0, 0, 6, 0) };
            _txtRowTo = new UnderlinedTextField("Row đến", "30") { Width = 110, Margin = new Padding(0, 0, 6, 0) };
            _txtPattern = new UnderlinedTextField("Pattern cột", "ABCDEF") { Width = 140, Margin = new Padding(0, 0, 6, 0) };
            var bulkRow = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true };
            bulkRow.Controls.AddRange(new Control[] { _txtRowFrom, _txtRowTo, _txtPattern });

            _btnBulkPrice = new SecondaryButton("Cập nhật giá") { Height = 30, Margin = new Padding(0, 6, 6, 0) };
            _btnBulkBlock = new SecondaryButton("Khóa ghế") { Height = 30, Margin = new Padding(0, 6, 6, 0) };
            _btnBulkUnblock = new SecondaryButton("Gỡ khóa") { Height = 30, Margin = new Padding(0, 6, 6, 0) };
            var bulkBtns = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true };
            bulkBtns.Controls.AddRange(new Control[] { _btnBulkPrice, _btnBulkBlock, _btnBulkUnblock });

            _inspectorCard.Controls.Add(bulkBtns);
            _inspectorCard.Controls.Add(bulkRow);
            _inspectorCard.Controls.Add(bulkTitle);
            _inspectorCard.Controls.Add(inspector);

            // place into right grid
            var rightTop = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1 };
            rightTop.Controls.Add(toolbar);
            rightTop.Controls.Add(_legend);

            right.Controls.Add(rightTop, 0, 0);
            right.Controls.Add(new Panel(), 1, 0); // spacer
            right.Controls.Add(_map, 0, 1);
            right.Controls.Add(_inspectorCard, 1, 1);

            columns.Controls.Add(left, 0, 0);
            columns.Controls.Add(right, 1, 0);

            var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.Controls.Add(_title, 0, 0);
            root.Controls.Add(_headerActions, 0, 1);
            root.Controls.Add(filter, 0, 2);
            root.Controls.Add(columns, 0, 3);

            Controls.Add(root);
            ResumeLayout(false);
        }

        private void BuildLegend() {
            _legend.Controls.Clear();
            var items = new (string text, Color bg, Color border)[] {
                ("AVAILABLE", Color.FromArgb(245,247,250), Color.FromArgb(200,205,210)),
                ("BOOKED",    Color.FromArgb(220,235,255), Color.FromArgb(0,92,175)),
                ("BLOCKED",   Color.FromArgb(255,235,238), Color.FromArgb(214,47,61))
            };
            int x = 0;
            foreach (var it in items) {
                var swatch = new Panel { Width = 16, Height = 16, BackColor = it.bg, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(0, 6, 6, 0) };
                var wrap = new FlowLayoutPanel { AutoSize = true };
                wrap.Controls.Add(swatch);
                wrap.Controls.Add(new Label { Text = it.text, AutoSize = true, Margin = new Padding(0, 6, 12, 0) });
                wrap.Location = new Point(x, 0);
                _legend.Controls.Add(wrap);
                x += 150;
            }
        }

        private void WireEvents() {
            _btnLoad.Click += (_, __) => LoadFlightSeats(_txtFlight.Text, _cbRoute.SelectedText, _cbDate.SelectedText);

            _tblFlights.CellMouseClick += (s, e) => {
                if (e.RowIndex < 0) return;
                var flightNo = _tblFlights.Rows[e.RowIndex].Cells["flightNo"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(flightNo)) LoadFlightSeats(flightNo, "", "");
            };

            _map.SeatClicked += (seat, keys) => {
                _txtPrice.Text = seat.BasePrice > 0 ? seat.BasePrice.ToString() : "";
                _cbFare.SelectedText = string.IsNullOrWhiteSpace(seat.FareCode) ? "—" : seat.FareCode;
                _cbSeatStatus.SelectedText = seat.Status.ToString().ToUpper();
                _txtPNR.Text = seat.PNR ?? "";
                _txtNote.Text = seat.Note ?? "";
            };

            _btnApplySeat.Click += (_, __) => {
                var seat = GetSelectedSeatFromInspector();
                if (seat == null) { MessageBox.Show("Chọn ghế trên bản đồ trước.", "Thông báo"); return; }
                // demo update: tìm và áp giá/trạng thái
                var match = _seats.FirstOrDefault(x => x.Row == seat.Row && x.Col == seat.Col);
                if (match != null) {
                    match.BasePrice = decimal.TryParse(_txtPrice.Text, out var p) ? p : match.BasePrice;
                    match.FareCode = _cbFare.SelectedText == "—" ? "" : _cbFare.SelectedText;
                    match.Status = Enum.TryParse<SeatStatus>(_cbSeatStatus.SelectedText, true, out var st) ? st : match.Status;
                    match.PNR = match.Status == SeatStatus.Booked ? (_txtPNR.Text ?? "") : null;
                    match.Note = _txtNote.Text;
                    _map.Invalidate();
                    MessageBox.Show($"Đã áp dụng cho ghế {match}.", "Thông báo");
                }
            };

            _btnApplyCabin.Click += (_, __) => {
                var cabin = _cbCabin.SelectedText;
                if (string.IsNullOrWhiteSpace(cabin) || cabin == "Tất cả") { MessageBox.Show("Chọn cabin để áp dụng.", "Thông báo"); return; }
                if (!decimal.TryParse(_txtBasePrice.Text, out var price)) { MessageBox.Show("Giá base không hợp lệ.", "Lỗi"); return; }
                foreach (var s in _seats.Where(x => x.Cabin == cabin && x.Status != SeatStatus.Booked)) s.BasePrice = price;
                _map.Invalidate();
                MessageBox.Show($"Đã áp dụng giá {price:#,0} ₫ cho cabin {cabin}.", "Thông báo");
            };

            _btnLockCabin.Click += (_, __) => {
                var cabin = _cbCabin.SelectedText;
                if (string.IsNullOrWhiteSpace(cabin) || cabin == "Tất cả") { MessageBox.Show("Chọn cabin.", "Thông báo"); return; }
                var toBlock = _seats.Where(x => x.Cabin == cabin && x.Status != SeatStatus.Booked);
                bool anyBlocked = toBlock.Any(x => x.Status == SeatStatus.Blocked);
                var target = anyBlocked ? SeatStatus.Available : SeatStatus.Blocked;
                foreach (var s in toBlock) s.Status = target;
                _map.Invalidate();
            };

            _btnBulkPrice.Click += (_, __) => BulkApply(seat => {
                if (decimal.TryParse(_txtBasePrice.Text, out var price) && seat.Status != SeatStatus.Booked)
                    seat.BasePrice = price;
            });
            _btnBulkBlock.Click += (_, __) => BulkApply(seat => { if (seat.Status != SeatStatus.Booked) seat.Status = SeatStatus.Blocked; });
            _btnBulkUnblock.Click += (_, __) => BulkApply(seat => { if (seat.Status == SeatStatus.Blocked) seat.Status = SeatStatus.Available; });
        }

        private void BulkApply(Action<SeatVM> updater) {
            if (!int.TryParse(_txtRowFrom.Text, out var from) || !int.TryParse(_txtRowTo.Text, out var to) || from <= 0 || to < from) {
                MessageBox.Show("Khoảng Row không hợp lệ.", "Lỗi"); return;
            }
            var pattern = new string((_txtPattern.Text ?? "").ToUpper().Where(char.IsLetter).ToArray());
            if (string.IsNullOrEmpty(pattern)) pattern = "ABCDEF";

            int count = 0;
            for (int r = from; r <= to; r++) {
                foreach (var col in pattern) {
                    var s = _seats.FirstOrDefault(x => x.Row == r && x.Col == col.ToString());
                    if (s != null) { updater(s); count++; }
                }
            }
            _map.Invalidate();
            MessageBox.Show($"Đã áp dụng cho {count} ghế.", "Thông báo");
        }

        private SeatVM? GetSelectedSeatFromInspector() {
            // Lấy theo ghế có tooltip gần nhất? Ở demo này, giả định người dùng vừa click ghế,
            // dữ liệu đã đổ vào inspector — nên chọn ghế theo Row/Col nhập tay không khả thi.
            // Ta sẽ tìm ghế gần nhất theo fare dropdown (demo): bỏ qua, chỉ hiển thị message.
            // Trong triển khai thực, bạn nên lưu biến _currentSelectedSeat khi _map.SeatClicked.
            return _currentSelectedSeat;
        }
        private SeatVM? _currentSelectedSeat;

        private void LoadFlightSeats(string flightNo, string route, string date) {
            // DEMO: build 30xABCDEF, random trạng thái
            var rnd = new Random(1);
            _seats.Clear();
            for (int r = 1; r <= 30; r++) {
                foreach (var col in "ABCDEF") {
                    var s = new SeatVM {
                        Row = r,
                        Col = col.ToString(),
                        Cabin = r <= 3 ? "Business" : "Economy",
                        Class = r <= 3 ? "Business" : "Economy",
                        Status = (SeatStatus)(rnd.Next(0, 100) < 10 ? 1 : 0), // ~10% booked
                        BasePrice = r <= 3 ? 3500000 : 1200000,
                        FareCode = r <= 3 ? "BUS-FLEX" : "ECON-SAVER",
                        PNR = null
                    };
                    _seats.Add(s);
                }
            }
            // block một vài ghế
            foreach (var s in _seats.Where(x => x.Row == 10 && (x.Col == "C" || x.Col == "D"))) s.Status = SeatStatus.Blocked;

            _map.BindSeats(_seats, 30, "ABCDEF");
            _map.SeatClicked -= OnSeatClickedInternal;
            _map.SeatClicked += OnSeatClickedInternal;

            MessageBox.Show($"Đã nạp ghế cho chuyến {flightNo}.", "Thông báo");
        }

        private void OnSeatClickedInternal(SeatVM seat, Keys keys) {
            _currentSelectedSeat = seat;
            _txtPrice.Text = seat.BasePrice > 0 ? seat.BasePrice.ToString() : "";
            _cbFare.SelectedText = string.IsNullOrWhiteSpace(seat.FareCode) ? "—" : seat.FareCode;
            _cbSeatStatus.SelectedText = seat.Status.ToString().ToUpper();
            _txtPNR.Text = seat.PNR ?? "";
            _txtNote.Text = seat.Note ?? "";
        }

        private void SeedFlights() {
            _tblFlights.Rows.Add("VN210", "SGN-HAN", "08:00", "10:10", "Chọn");
            _tblFlights.Rows.Add("VJ156", "SGN-DAD", "09:30", "10:40", "Chọn");
            _tblFlights.Rows.Add("QH220", "HAN-SGN", "18:00", "20:10", "Chọn");
        }
    }
}
