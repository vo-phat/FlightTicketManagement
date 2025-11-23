using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;

namespace GUI.Features.Seat.SubFeatures {
    public enum SeatMapMode { PerAircraft, PerFlight }

    public class SeatMapControl : UserControl {
        private readonly SeatMapMode _mode;
        private Label lblTitle;
        private UnderlinedComboBox cbSelector; // aircraft or flight depending on mode
        private FlowLayoutPanel legend;
        private Panel seatHost;
        private PrimaryButton btnApplyBulk, btnReload;

        // Demo data
        private List<SeatVM> seats = new();

        public SeatMapControl(SeatMapMode mode) {
            _mode = mode;
            InitializeComponent();
            SeedDemo();
            Render();
        }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.White;

            lblTitle = new Label {
                Text = _mode == SeatMapMode.PerAircraft ? "🗺 Sơ đồ ghế theo máy bay" : "🗺 Sơ đồ ghế theo chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 8),
                Dock = DockStyle.Top
            };

            cbSelector = new UnderlinedComboBox(_mode == SeatMapMode.PerAircraft ? "Máy bay" : "Chuyến bay",
                _mode == SeatMapMode.PerAircraft ? new object[] { "A320", "B737" } : new object[] { "VN123 02/11 08:30", "VN456 05/11 18:30" }) {
                Width = 260,
                Margin = new Padding(24, 0, 24, 0)
            };
            btnApplyBulk = new PrimaryButton(_mode == SeatMapMode.PerAircraft ? "Gán hạng hàng loạt" : "Thiết lập giá/chặn hàng loạt") { Width = 200, Height = 36, Margin = new Padding(0, 0, 12, 0) };
            btnReload = new PrimaryButton("↻ Tải lại") { Width = 100, Height = 36 };
            var topRow = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(24, 8, 24, 0), WrapContents = false };
            topRow.Controls.Add(cbSelector);
            topRow.Controls.Add(btnApplyBulk);
            topRow.Controls.Add(btnReload);

            legend = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, WrapContents = false, Padding = new Padding(24, 8, 24, 8) };
            seatHost = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(24) };

            Controls.Add(seatHost);
            Controls.Add(legend);
            Controls.Add(topRow);
            Controls.Add(lblTitle);

            btnReload.Click += (_, __) => { SeedDemo(); Render(); };
            btnApplyBulk.Click += (_, __) => MessageBox.Show(_mode == SeatMapMode.PerAircraft ? "[DEMO] Bulk set cabin" : "[DEMO] Bulk set price/block");
        }

        private void SeedDemo() {
            seats.Clear();
            var aircraft = cbSelector.SelectedItem?.ToString() ?? (_mode == SeatMapMode.PerAircraft ? "A320" : "VN123 02/11 08:30");
            var rnd = new Random(3);
            for (int row = 1; row <= 28; row++) {
                foreach (var col in new[] { 'A', 'B', 'C', 'D', 'E', 'F' }) {
                    var vm = new SeatVM {
                        SeatNumber = $"{row}{col}",
                        CabinName = row <= 4 ? "Business" : "Economy",
                        AircraftOrFlight = aircraft
                    };

                    if (_mode == SeatMapMode.PerFlight) {
                        // trạng thái chỉ có khi per-flight
                        vm.Status = (row % 9 == 0) ? "BLOCKED" : ((row % 7 == 0) ? "BOOKED" : "AVAILABLE");
                        vm.Price = vm.CabinName == "Business" ? 3200000 : 1200000;
                    }

                    seats.Add(vm);
                }
            }
        }

        private void Render() {
            RenderLegend();
            RenderGrid();
        }

        private void RenderLegend() {
            legend.Controls.Clear();
            if (_mode == SeatMapMode.PerAircraft) {
                AddLegend("Business", Color.FromArgb(0, 92, 175));
                AddLegend("Economy", Color.FromArgb(100, 181, 246));
            } else {
                AddLegend("AVAILABLE", Color.FromArgb(76, 175, 80));
                AddLegend("BOOKED", Color.FromArgb(158, 158, 158));
                AddLegend("BLOCKED", Color.FromArgb(244, 67, 54));
            }
        }

        private void AddLegend(string text, Color color) {
            var swatch = new Panel { Width = 16, Height = 16, BackColor = color, Margin = new Padding(6, 10, 6, 0) };
            var lbl = new Label { Text = text, AutoSize = true, Margin = new Padding(0, 8, 12, 0) };
            legend.Controls.Add(swatch); legend.Controls.Add(lbl);
        }

        private void RenderGrid() {
            seatHost.Controls.Clear();

            var rows = seats.GroupBy(s => RowOf(s.SeatNumber)).OrderBy(g => g.Key).ToList();

            var grid = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 6,
                BackColor = Color.White
            };
            for (int c = 0; c < 6; c++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            foreach (var g in rows) {
                grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                int col = 0;
                foreach (var vm in g.OrderBy(x => ColOf(x.SeatNumber))) {
                    var btn = new Button {
                        Text = vm.SeatNumber,
                        Width = 52,
                        Height = 36,
                        Margin = new Padding(6),
                        FlatStyle = FlatStyle.Flat,
                        Tag = vm
                    };
                    btn.FlatAppearance.BorderSize = 0;

                    if (_mode == SeatMapMode.PerAircraft) {
                        btn.BackColor = vm.CabinName == "Business" ? Color.FromArgb(0, 92, 175) : Color.FromArgb(100, 181, 246);
                        btn.ForeColor = Color.White;
                        btn.Click += (_, __) => MessageBox.Show($"[DEMO] Gán hạng cho {vm.SeatNumber} (hiện: {vm.CabinName})");
                    } else {
                        btn.BackColor = vm.Status switch {
                            "AVAILABLE" => Color.FromArgb(76, 175, 80),
                            "BOOKED" => Color.FromArgb(158, 158, 158),
                            "BLOCKED" => Color.FromArgb(244, 67, 54),
                            _ => Color.LightGray
                        };
                        btn.ForeColor = Color.White;
                        var tip = new ToolTip();
                        tip.SetToolTip(btn, $"{vm.SeatNumber} • {vm.CabinName}\nTrạng thái: {vm.Status}\nGiá: {vm.Price:#,0}");
                        btn.Click += (_, __) => MessageBox.Show($"[DEMO] Sửa {vm.SeatNumber}\nTrạng thái: {vm.Status}\nGiá: {vm.Price:#,0}");
                    }

                    grid.Controls.Add(btn, col++, grid.RowCount - 1);
                }
            }

            seatHost.Controls.Add(grid);
        }

        private static int RowOf(string seat) {
            int i = 0; while (i < seat.Length && char.IsDigit(seat[i])) i++;
            return int.Parse(seat.Substring(0, i));
        }
        private static int ColOf(string seat) {
            char letter = seat.Last(c => char.IsLetter(c));
            return letter - 'A';
        }

        private class SeatVM {
            public string SeatNumber { get; set; } = "";
            public string CabinName { get; set; } = "";
            public string AircraftOrFlight { get; set; } = "";
            // Per-flight
            public string Status { get; set; } = "AVAILABLE";
            public decimal Price { get; set; } = 0m;
        }
    }
}
