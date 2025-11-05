using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BUS.FlightSeat;
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
        private TableLayoutPanel centerLayout;
        private FlowLayoutPanel stack;
        private ToolTip tip;

        // Dữ liệu từ BUS
        private readonly FlightSeatBUS _bus = new();
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

            // --- Map host ---
            mapHost = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(32, 16, 32, 72),
                AutoScroll = true,
                BackColor = Color.White
            };

            centerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 3
            };
            centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            stack = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false
            };
            centerLayout.Controls.Add(stack, 1, 0);
            mapHost.Controls.Add(centerLayout);

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
            p.Controls.Add(new Label { Text = status, AutoSize = true, ForeColor = fg });
            return p;
        }

        // --------------------------- LOAD DATA ---------------------------
        private void LoadData()
        {
            try
            {
                seats = _bus.GetAllWithDetails();

                // Đổ combobox Aircraft
                var aircrafts = seats.Select(s => s.AircraftName).Distinct().ToList();
                cbAircraft.Items.Clear();
                cbAircraft.Items.AddRange(aircrafts.Cast<object>().ToArray());
                if (cbAircraft.Items.Count > 0) cbAircraft.SelectedIndex = 0;

                // Đổ combobox Class
                var classes = seats.Select(s => s.ClassName).Distinct().ToList();
                foreach (var c in classes)
                    if (!cbClass.Items.Contains(c))
                        cbClass.Items.Add(c);
                cbClass.SelectedIndex = 0;

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

            string selectedAircraft = cbAircraft.SelectedItem?.ToString() ?? "";
            string selectedClass = cbClass.SelectedItem?.ToString() ?? "Tất cả";

            var filtered = seats
                .Where(s => s.AircraftName == selectedAircraft)
                .ToList();

            if (selectedClass != "Tất cả")
                filtered = filtered.Where(s => s.ClassName == selectedClass).ToList();

            if (filtered.Count == 0)
            {
                stack.Controls.Add(new Label
                {
                    Text = "Không có dữ liệu ghế để hiển thị.",
                    Font = new Font("Segoe UI", 12, FontStyle.Italic),
                    AutoSize = true,
                    Padding = new Padding(8)
                });
                return;
            }

            // Nhóm theo hạng
            var groups = filtered.GroupBy(s => s.ClassName)
                                 .OrderBy(g => g.Key)
                                 .ToList();

            foreach (var group in groups)
            {
                var card = new GroupBox
                {
                    Text = group.Key,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    Padding = new Padding(16),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Margin = new Padding(0, 0, 0, 24)
                };

                var grid = new TableLayoutPanel
                {
                    ColumnCount = 8,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                };

                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, RowLabelWidth));
                for (int i = 0; i < 3; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, SeatWidth));
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, AisleWidth));
                for (int i = 0; i < 3; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, SeatWidth));

                // Gom nhóm theo hàng (VD: 1A,1B,1C,...)
                var rowGroups = group.GroupBy(s =>
                {
                    string num = new string(s.SeatNumber.TakeWhile(char.IsDigit).ToArray());
                    return int.TryParse(num, out int n) ? n : 0;
                })
                .OrderBy(g => g.Key);

                int rowIdx = 0;
                foreach (var row in rowGroups)
                {
                    grid.RowStyles.Add(new RowStyle(SizeType.Absolute, SeatHeight + SeatGap));

                    int rowNo = row.Key;
                    var lb = new Label
                    {
                        Text = rowNo.ToString(),
                        Width = RowLabelWidth,
                        Height = SeatHeight,
                        Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(0, SeatGap / 2, SeatGap, 0)
                    };
                    grid.Controls.Add(lb, 0, rowIdx);

                    // A B C / D E F
                    foreach (var seat in row)
                    {
                        char col = seat.SeatNumber.Last();
                        int colIndex = "ABCDEF".IndexOf(col);
                        if (colIndex == -1) continue;

                        int gridCol = (colIndex < 3) ? colIndex + 1 : colIndex + 2; // bỏ cột lối đi

                        grid.Controls.Add(MakeSeat(seat), gridCol, rowIdx);
                    }

                    // cột lối đi
                    grid.Controls.Add(new Panel { Width = AisleWidth, Height = 1 }, 4, rowIdx);
                    rowIdx++;
                }

                card.Controls.Add(grid);
                stack.Controls.Add(card);
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
            btn.FlatAppearance.BorderSize = 1;
            btn.UseVisualStyleBackColor = false;

            StyleSeat(btn, status);
            tip.SetToolTip(btn, $"{cabinName} • {status} • Giá: {price:n0}₫");

            btn.Click += (_, __) =>
                MessageBox.Show($"Ghế {code}\nHạng: {cabinName}\nTrạng thái: {status}\nGiá: {price:n0}₫",
                    "Chi tiết ghế", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            }
            else if (status == "BOOKED")
            {
                btn.BackColor = Color.FromArgb(236, 239, 241);
                btn.FlatAppearance.BorderColor = Color.FromArgb(176, 190, 197);
                btn.ForeColor = Color.FromArgb(55, 71, 79);
            }
            else // BLOCKED
            {
                btn.BackColor = Color.FromArgb(255, 235, 238);
                btn.FlatAppearance.BorderColor = Color.FromArgb(229, 115, 115);
                btn.ForeColor = Color.FromArgb(183, 28, 28);
            }

            btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
            btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
        }
    }
}
