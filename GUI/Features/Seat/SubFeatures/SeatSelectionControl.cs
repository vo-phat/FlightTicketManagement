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
    public class SeatSelectionControl : UserControl {
        private Label _title, _subtitle;

        // filters
        private UnderlinedComboBox _cbCabin, _cbStatus;
        private UnderlinedTextField _txtFindSeat;

        // map + legend
        private FlightSeatMapPanel _map;
        private Panel _legend;

        // summary right
        private Panel _summaryCard;
        private TableCustom _tblChosen;
        private Label _lblTotal;
        private Button _btnConfirm, _btnClear;

        private readonly List<SeatVM> _seats = new();
        private readonly List<SeatVM> _chosen = new();

        public SeatSelectionControl() {
            InitializeComponent();
            BuildLegend();
            LoadDemoSeats();
            WireEvents();
        }

        private void InitializeComponent() {
            SuspendLayout();
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            _title = new Label {
                Text = "🧾 Chọn ghế",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };
            _subtitle = new Label {
                Text = "Chuyến VN210 • SGN-HAN • Hôm nay",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Padding = new Padding(24, 0, 24, 8),
                Dock = DockStyle.Top,
                ForeColor = Color.FromArgb(90, 90, 90)
            };

            var filter = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(24, 8, 24, 0), WrapContents = false };
            _cbCabin = new UnderlinedComboBox("Cabin", new object[] { "Tất cả", "Economy", "Premium Economy", "Business", "First" }) { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            _cbStatus = new UnderlinedComboBox("Trạng thái", new object[] { "AVAILABLE", "All" }) { Width = 140, Margin = new Padding(0, 0, 24, 0) };
            _txtFindSeat = new UnderlinedTextField("Tìm ghế (VD: 12A)", "") { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            filter.Controls.AddRange(new Control[] { _cbCabin, _cbStatus, _txtFindSeat });

            var columns = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Padding = new Padding(24, 12, 24, 24) };
            columns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            columns.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360));

            var left = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
            left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            _legend = new Panel { Dock = DockStyle.Top, Height = 28, BackColor = Color.Transparent, Margin = new Padding(0, 0, 0, 6) };
            _map = new FlightSeatMapPanel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Rows = 30, Pattern = "ABCDEF" };
            left.Controls.Add(_legend, 0, 0);
            left.Controls.Add(_map, 0, 1);

            _summaryCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(12) };
            var sumTitle = new Label { Text = "Ghế đã chọn (0)", AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold), Margin = new Padding(0, 0, 0, 6) };
            _tblChosen = new TableCustom {
                Dock = DockStyle.Top,
                Height = 260,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };
            _tblChosen.Columns.Add("seat", "Ghế");
            _tblChosen.Columns.Add("cabin", "Cabin");
            _tblChosen.Columns.Add("fare", "Fare");
            _tblChosen.Columns.Add("price", "Giá");
            var colAct = new DataGridViewTextBoxColumn { Name = "act", HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells };
            _tblChosen.Columns.Add(colAct);

            _lblTotal = new Label { Text = "Tổng: 0 ₫", AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold), Margin = new Padding(0, 12, 0, 8) };
            _btnConfirm = new PrimaryButton("Xác nhận ghế") { Height = 34, Width = 160, Margin = new Padding(0, 0, 8, 0) };
            _btnClear = new SecondaryButton("Bỏ chọn tất cả") { Height = 34, Width = 140 };

            var sumBtns = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
            sumBtns.Controls.AddRange(new Control[] { _btnConfirm, _btnClear });

            _summaryCard.Controls.Add(sumBtns);
            _summaryCard.Controls.Add(_lblTotal);
            _summaryCard.Controls.Add(_tblChosen);
            _summaryCard.Controls.Add(sumTitle);

            columns.Controls.Add(left, 0, 0);
            columns.Controls.Add(_summaryCard, 1, 0);

            var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.Controls.Add(_title, 0, 0);
            root.Controls.Add(_subtitle, 0, 1);
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
            _map.SeatClicked += (seat, keys) => {
                if (seat.Status != SeatStatus.Available) {
                    MessageBox.Show("Ghế không khả dụng.", "Thông báo");
                    return;
                }
                // toggle chọn
                var exist = _chosen.FirstOrDefault(x => x.Row == seat.Row && x.Col == seat.Col);
                if (exist != null) _chosen.Remove(exist);
                else _chosen.Add(seat);

                RefreshChosenTable();
            };

            _btnClear.Click += (_, __) => { _chosen.Clear(); RefreshChosenTable(); };

            _btnConfirm.Click += (_, __) => {
                if (_chosen.Count == 0) { MessageBox.Show("Bạn chưa chọn ghế.", "Thông báo"); return; }
                var total = _chosen.Sum(x => x.BasePrice);
                MessageBox.Show($"Đã giữ chỗ {_chosen.Count} ghế. Tổng {total:#,0} ₫", "Xác nhận");
            };
        }

        private void LoadDemoSeats() {
            _seats.Clear();
            for (int r = 1; r <= 30; r++) {
                foreach (var col in "ABCDEF") {
                    var s = new SeatVM {
                        Row = r,
                        Col = col.ToString(),
                        Cabin = r <= 3 ? "Business" : "Economy",
                        Class = r <= 3 ? "Business" : "Economy",
                        Status = SeatStatus.Available,
                        BasePrice = r <= 3 ? 3500000 : 1200000,
                        FareCode = r <= 3 ? "BUS-FLEX" : "ECON-SAVER",
                    };
                    _seats.Add(s);
                }
            }
            // BOOKED/BLOCKED demo
            _seats.First(x => x.Row == 5 && x.Col == "A").Status = SeatStatus.Booked;
            _seats.First(x => x.Row == 10 && x.Col == "C").Status = SeatStatus.Blocked;

            _map.BindSeats(_seats, 30, "ABCDEF");
        }

        private void RefreshChosenTable() {
            _tblChosen.Rows.Clear();
            foreach (var s in _chosen) {
                _tblChosen.Rows.Add(s.ToString(), s.Cabin, s.FareCode, s.BasePrice.ToString("#,0 ₫"), "Xóa");
            }
            var total = _chosen.Sum(x => x.BasePrice);
            _lblTotal.Text = $"Tổng: {total:#,0} ₫";

            // click “Xóa”
            _tblChosen.CellMouseClick -= OnChosenCellClick;
            _tblChosen.CellMouseClick += OnChosenCellClick;
            // update title
            var title = _summaryCard.Controls.OfType<Label>().FirstOrDefault(l => l.Text.StartsWith("Ghế đã chọn"));
            if (title != null) title.Text = $"Ghế đã chọn ({_chosen.Count})";
        }

        private void OnChosenCellClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0) return;
            var colName = _tblChosen.Columns[e.ColumnIndex].Name;
            if (colName != "act") return;

            var seatCode = _tblChosen.Rows[e.RowIndex].Cells["seat"].Value?.ToString() ?? "";
            if (seatCode.Length >= 2) {
                var col = seatCode[^1].ToString();
                if (int.TryParse(seatCode.Substring(0, seatCode.Length - 1), out var row)) {
                    var rm = _chosen.FirstOrDefault(x => x.Row == row && x.Col == col);
                    if (rm != null) _chosen.Remove(rm);
                    RefreshChosenTable();
                }
            }
        }
    }
}
