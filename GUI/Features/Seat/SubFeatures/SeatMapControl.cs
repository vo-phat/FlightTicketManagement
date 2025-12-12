using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BUS.FlightSeat;
using BUS.CabinClass;
using DTO.FlightSeat;
using GUI.Components.Buttons;
using GUI.Components.Inputs;

namespace GUI.Features.Seat.SubFeatures
{
    public class SeatMapControl : UserControl
    {
        private const bool USE_PLAIN_BUTTON_FOR_COLOR = true;

        // Kích thước layout
        private const int RowLabelWidth = 56;
        private const int SeatWidth = 84;
        private const int SeatHeight = 52;
        private const int AisleWidth = 40;
        private const int SeatGap = 8;

        private Label lblTitle;
        private TableLayoutPanel root;
        private FlowLayoutPanel filter, legend;
        private UnderlinedComboBox cbFlight, cbAircraft, cbClass;
        private PrimaryButton btnRefresh;
        private Panel mapHost;
        private TableLayoutPanel stack;
        private ToolTip tip;

        // Dữ liệu từ BUS
        private readonly FlightSeatBUS _bus = new();
        private readonly CabinClassBUS _cabinClassBUS = new();
        private List<FlightSeatDTO> seats = new();

        public SeatMapControl()
        {
            InitializeComponent();
            LoadData();
        }

        // --------------------------- INIT ---------------------------
        private void InitializeComponent()
        {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);
            tip = new ToolTip();

            lblTitle = new Label
            {
                Text = "🗺️ Sơ đồ ghế (Seat Map)",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // --- Bộ lọc ---
            filter = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 12, 24, 12),
                WrapContents = false
            };

            cbFlight = new UnderlinedComboBox("Chuyến bay", Array.Empty<object>())
            { Width = 160, Margin = new Padding(0, 0, 24, 0) };

            cbAircraft = new UnderlinedComboBox("Máy bay", Array.Empty<object>())
            { Width = 180, Margin = new Padding(0, 0, 24, 0) };

            cbClass = new UnderlinedComboBox("Hạng", new object[] { "Tất cả" })
            { Width = 160, Margin = new Padding(0, 0, 24, 0) };

            btnRefresh = new PrimaryButton("⟳ Làm mới")
            { Width = 110, Height = 36 };

            btnRefresh.Click += (_, __) => RefreshSeatMap();

            filter.Controls.AddRange(new Control[] { cbFlight, cbAircraft, cbClass, btnRefresh });

            // --- Legend ---
            legend = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 6, 24, 0),
                WrapContents = false
            };
            legend.Controls.Add(Badge("AVAILABLE", Color.FromArgb(232, 245, 233), Color.FromArgb(27, 94, 32)));
            legend.Controls.Add(Badge("BOOKED", Color.FromArgb(236, 239, 241), Color.FromArgb(55, 71, 79)));
            legend.Controls.Add(Badge("BLOCKED", Color.FromArgb(255, 235, 238), Color.FromArgb(183, 28, 28)));
            legend.Controls.Add(Badge("EMPTY", Color.FromArgb(250, 250, 250), Color.FromArgb(158, 158, 158)));

            // --- Map host ---
            mapHost = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(32, 16, 32, 72),
                AutoScroll = true,
                BackColor = Color.White
            };

            stack = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1
            };
            stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            mapHost.Controls.Add(stack);

            // --- Root layout ---
            root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4 };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(filter, 0, 1);
            root.Controls.Add(legend, 0, 2);
            root.Controls.Add(mapHost, 0, 3);

            Controls.Add(root);
            ResumeLayout(false);
        }

        private Control Badge(string status, Color bg, Color fg)
        {
            var p = new Panel { BackColor = bg, Height = 24, Padding = new Padding(10, 3, 10, 3), Margin = new Padding(0, 0, 8, 0), AutoSize = true };
            p.Controls.Add(new Label { Text = status, AutoSize = true, ForeColor = fg, Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            return p;
        }

        // --------------------------- LOAD DATA ---------------------------
        private void LoadData()
        {
            try
            {
                seats = _bus.GetAllWithDetails();

                // === Chuyến bay ===
                var flights = seats.Select(s => s.FlightName).Distinct().OrderBy(f => f).ToList();
                cbFlight.Items.Clear();
                cbFlight.Items.Add("Tất cả");
                cbFlight.Items.AddRange(flights.Cast<object>().ToArray());
                cbFlight.SelectedIndex = 0;

                // === Máy bay ===
                var aircrafts = seats.Select(s => s.AircraftName).Distinct().OrderBy(a => a).ToList();
                cbAircraft.Items.Clear();
                cbAircraft.Items.Add("Tất cả");
                cbAircraft.Items.AddRange(aircrafts.Cast<object>().ToArray());
                cbAircraft.SelectedIndex = 0;

                // === Hạng - Load từ database ===
                var allCabinClasses = _cabinClassBUS.GetAllCabinClasses();
                var classOrder = new Dictionary<string, int>
                {
                    { "First", 1 },
                    { "Business", 2 },
                    { "Premium Economy", 3 },
                    { "Economy", 4 }
                };

                var classes = allCabinClasses
                    .Select(c => c.ClassName)
                    .OrderBy(c => classOrder.ContainsKey(c) ? classOrder[c] : 99)
                    .ThenBy(c => c)
                    .ToList();

                cbClass.Items.Clear();
                cbClass.Items.Add("Tất cả");
                cbClass.Items.AddRange(classes.Cast<object>().ToArray());
                cbClass.SelectedIndex = 0;

                // === Events ===
                cbFlight.SelectedIndexChanged += (_, __) => RefreshSeatMap();
                cbAircraft.SelectedIndexChanged += (_, __) => RefreshSeatMap();
                cbClass.SelectedIndexChanged += (_, __) => RefreshSeatMap();

                // Initial load
                RefreshSeatMap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu sơ đồ ghế:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --------------------------- BUILD MAP ---------------------------
        private void RefreshSeatMap()
        {
            stack.Controls.Clear();
            stack.RowStyles.Clear();
            stack.RowCount = 0;

            string selectedFlight = cbFlight.SelectedItem?.ToString() ?? "Tất cả";
            string selectedAircraft = cbAircraft.SelectedItem?.ToString() ?? "Tất cả";
            string selectedClass = cbClass.SelectedItem?.ToString() ?? "Tất cả";

            var filtered = seats.AsEnumerable();

            if (selectedFlight != "Tất cả")
                filtered = filtered.Where(s => s.FlightName == selectedFlight);

            if (selectedAircraft != "Tất cả")
                filtered = filtered.Where(s => s.AircraftName == selectedAircraft);

            if (selectedClass != "Tất cả")
                filtered = filtered.Where(s => s.ClassName == selectedClass);

            var list = filtered.ToList();

            if (list.Count == 0)
            {
                var noDataLabel = new Label
                {
                    Text = "Không có dữ liệu ghế phù hợp với điều kiện tìm kiếm.",
                    Font = new Font("Segoe UI", 12, FontStyle.Italic),
                    AutoSize = true,
                    Padding = new Padding(8),
                    Anchor = AnchorStyles.None
                };
                stack.RowCount++;
                stack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                stack.Controls.Add(noDataLabel, 0, 0);
                return;
            }

            BuildCompleteSeatMap(list);
        }

        private void BuildCompleteSeatMap(List<FlightSeatDTO> filteredSeats)
        {
            // Nhóm theo Chuyến bay và Máy bay
            var groupedByFlightAndAircraft = filteredSeats
                .GroupBy(s => new { s.FlightId, s.FlightName, s.AircraftId, s.AircraftName, s.AircraftCapacity })
                .OrderBy(g => g.Key.FlightName)
                .ThenBy(g => g.Key.AircraftName);

            int rowIndex = 0;
            foreach (var group in groupedByFlightAndAircraft)
            {
                // Lấy tất cả ghế trong nhóm này (không lọc theo Class)
                string selectedFlight = cbFlight.SelectedItem?.ToString() ?? "Tất cả";
                string selectedAircraft = cbAircraft.SelectedItem?.ToString() ?? "Tất cả";

                var baseFilter = seats.AsEnumerable();

                if (selectedFlight != "Tất cả")
                    baseFilter = baseFilter.Where(s => s.FlightName == selectedFlight);
                else
                    baseFilter = baseFilter.Where(s => s.FlightId == group.Key.FlightId);

                if (selectedAircraft != "Tất cả")
                    baseFilter = baseFilter.Where(s => s.AircraftName == selectedAircraft);
                else
                    baseFilter = baseFilter.Where(s => s.AircraftId == group.Key.AircraftId);

                var allSeatsInGroup = baseFilter.ToList();

                // Dictionary highlight cho ghế khớp bộ lọc Class
                var highlightDict = new Dictionary<string, FlightSeatDTO>();
                foreach (var seat in group)
                {
                    highlightDict[seat.SeatNumber] = seat;
                }

                // Cấu hình cột & hàng
                char[] standardColumns = { 'A', 'B', 'C', 'D', 'E', 'F' };
                var sortedCols = standardColumns.ToList();

                int seatsPerRow = sortedCols.Count;
                int estimatedRows = group.Key.AircraftCapacity > 0
                    ? (int)Math.Ceiling((double)group.Key.AircraftCapacity / seatsPerRow)
                    : 0;

                int maxRowFromSeats = allSeatsInGroup
                    .Select(s => int.TryParse(new string(s.SeatNumber.TakeWhile(char.IsDigit).ToArray()), out int n) ? n : 0)
                    .DefaultIfEmpty(0)
                    .Max();

                int maxRow = Math.Max(estimatedRows, maxRowFromSeats);
                if (maxRow == 0) continue;

                var sortedRows = Enumerable.Range(1, maxRow).ToList();

                // Tạo header
                var headerPanel = new FlowLayoutPanel
                {
                    AutoSize = true,
                    WrapContents = false,
                    Margin = new Padding(0, 0, 0, 12)
                };

                headerPanel.Controls.Add(new Label { Width = RowLabelWidth, Height = 30, Text = "" });

                int headerAisleIndex = 3;
                for (int i = 0; i < sortedCols.Count; i++)
                {
                    if (i == headerAisleIndex) headerPanel.Controls.Add(new Panel { Width = AisleWidth, Height = 1 });

                    var colLabel = new Label
                    {
                        Text = sortedCols[i].ToString(),
                        Width = SeatWidth - SeatGap,
                        Height = 30,
                        Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(SeatGap / 2),
                        ForeColor = Color.FromArgb(66, 66, 66)
                    };
                    headerPanel.Controls.Add(colLabel);
                }

                // Tạo GroupBox
                var cardGroupBox = new GroupBox
                {
                    Text = $"   ✈  {group.Key.FlightName.ToUpper()}   |   MÁY BAY: {group.Key.AircraftName.ToUpper()}   (Capacity: {group.Key.AircraftCapacity})   ",
                    Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 51, 102),
                    Padding = new Padding(16, 40, 16, 20),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Margin = new Padding(0, 0, 0, 32),
                    Anchor = AnchorStyles.None
                };

                var grid = new TableLayoutPanel
                {
                    ColumnCount = sortedCols.Count + 2,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    BackColor = Color.White
                };

                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, RowLabelWidth));

                int gridAisleIndex = 3;
                for (int i = 0; i < sortedCols.Count; i++)
                {
                    if (i == gridAisleIndex) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, AisleWidth));
                    grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, SeatWidth));
                }

                int rowIdx = 0;
                foreach (int rowNo in sortedRows)
                {
                    grid.RowStyles.Add(new RowStyle(SizeType.Absolute, SeatHeight + SeatGap));

                    var lb = new Label
                    {
                        Text = rowNo.ToString(),
                        Width = RowLabelWidth,
                        Height = SeatHeight,
                        Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(0, SeatGap / 2, SeatGap, 0),
                        ForeColor = Color.FromArgb(66, 66, 66)
                    };
                    grid.Controls.Add(lb, 0, rowIdx);

                    int gridCol = 1;
                    for (int colIndex = 0; colIndex < sortedCols.Count; colIndex++)
                    {
                        char col = sortedCols[colIndex];
                        string seatNumber = $"{rowNo}{col}";

                        if (colIndex == gridAisleIndex)
                        {
                            grid.Controls.Add(new Panel { Width = AisleWidth, Height = 1, BackColor = Color.Transparent }, gridCol, rowIdx);
                            gridCol++;
                        }

                        if (highlightDict.TryGetValue(seatNumber, out FlightSeatDTO seat))
                        {
                            grid.Controls.Add(MakeSeat(seat), gridCol, rowIdx);
                        }
                        else if (allSeatsInGroup.Any(s => s.SeatNumber == seatNumber))
                        {
                            var dimSeat = allSeatsInGroup.First(s => s.SeatNumber == seatNumber);
                            grid.Controls.Add(MakeDimmedSeat(dimSeat), gridCol, rowIdx);
                        }
                        else
                        {
                            grid.Controls.Add(MakeEmptySeat(seatNumber), gridCol, rowIdx);
                        }
                        gridCol++;
                    }
                    rowIdx++;
                }

                // Wrapper để căn giữa
                var contentStack = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    WrapContents = false
                };
                contentStack.Controls.Add(headerPanel);
                contentStack.Controls.Add(grid);

                var centerWrapper = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    ColumnCount = 1,
                    RowCount = 1
                };
                centerWrapper.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                contentStack.Anchor = AnchorStyles.None;
                centerWrapper.Controls.Add(contentStack, 0, 0);

                cardGroupBox.Controls.Add(centerWrapper);

                stack.RowCount++;
                stack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                stack.Controls.Add(cardGroupBox, 0, rowIndex);
                rowIndex++;
            }
        }

        // --------------------------- SEAT BUTTON ---------------------------
        private Button MakeSeat(FlightSeatDTO seat)
        {
            string code = seat.SeatNumber;
            string status = seat.SeatStatus;
            string cabinName = seat.ClassName;
            decimal price = seat.BasePrice;

            Button btn = USE_PLAIN_BUTTON_FOR_COLOR ? new Button() : new SecondaryButton();
            btn.Text = code;
            btn.AutoSize = false;
            btn.Size = new Size(SeatWidth - SeatGap, SeatHeight);
            btn.Margin = new Padding(SeatGap / 2);
            btn.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.UseVisualStyleBackColor = false;
            btn.Cursor = Cursors.Hand;

            StyleSeat(btn, status);
            tip.SetToolTip(btn, $"{cabinName} • {status} • Giá: {price:n0}₫");

            btn.Click += (_, __) =>
                MessageBox.Show($"Ghế {code}\nHạng: {cabinName}\nTrạng thái: {status}\nGiá: {price:n0}₫",
                    "Chi tiết ghế", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return btn;
        }

        private Panel MakeEmptySeat(string position)
        {
            var panel = new Panel
            {
                Size = new Size(SeatWidth - SeatGap, SeatHeight),
                Margin = new Padding(SeatGap / 2),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            var label = new Label
            {
                Text = "—",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14f),
                ForeColor = Color.FromArgb(189, 189, 189)
            };

            panel.Controls.Add(label);
            tip.SetToolTip(panel, $"Vị trí {position} - Không có ghế");
            return panel;
        }

        private Button MakeDimmedSeat(FlightSeatDTO seat)
        {
            string code = seat.SeatNumber;
            string status = seat.SeatStatus;
            string cabinName = seat.ClassName;
            decimal price = seat.BasePrice;

            Button btn = USE_PLAIN_BUTTON_FOR_COLOR ? new Button() : new SecondaryButton();
            btn.Text = code;
            btn.AutoSize = false;
            btn.Size = new Size(SeatWidth - SeatGap, SeatHeight);
            btn.Margin = new Padding(SeatGap / 2);
            btn.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.UseVisualStyleBackColor = false;
            btn.Cursor = Cursors.Default;
            btn.Enabled = false;

            btn.BackColor = Color.FromArgb(245, 245, 245);
            btn.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btn.ForeColor = Color.FromArgb(158, 158, 158);

            tip.SetToolTip(btn, $"{cabinName} • {status} • Giá: {price:n0}₫ (Không thuộc bộ lọc)");
            return btn;
        }

        private void StyleSeat(Button btn, string status)
        {
            btn.UseVisualStyleBackColor = false;
            if (status == "AVAILABLE")
            {
                btn.BackColor = Color.FromArgb(232, 245, 233);
                btn.FlatAppearance.BorderColor = Color.FromArgb(76, 175, 80);
                btn.ForeColor = Color.FromArgb(27, 94, 32);
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 230, 201);
            }
            else if (status == "BOOKED")
            {
                btn.BackColor = Color.FromArgb(236, 239, 241);
                btn.FlatAppearance.BorderColor = Color.FromArgb(176, 190, 197);
                btn.ForeColor = Color.FromArgb(55, 71, 79);
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 225, 228);
            }
            else // BLOCKED
            {
                btn.BackColor = Color.FromArgb(255, 235, 238);
                btn.FlatAppearance.BorderColor = Color.FromArgb(229, 115, 115);
                btn.ForeColor = Color.FromArgb(183, 28, 28);
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 205, 210);
            }
            btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
        }
    }
}