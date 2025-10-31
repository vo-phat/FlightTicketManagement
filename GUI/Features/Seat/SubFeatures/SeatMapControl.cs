using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Buttons;

namespace GUI.Features.Seat.SubFeatures
{
    public class SeatMapControl : UserControl
    {
        // ====== TÙY CHỈNH NHANH ======
        // Bật true nếu SecondaryButton của bạn không tôn trọng BackColor (để dùng Button chuẩn đảm bảo lên màu)
        private const bool USE_PLAIN_BUTTON_FOR_COLOR = true;

        // Kích thước layout
        private const int RowLabelWidth = 56; // độ rộng cột số hàng (vừa 2 chữ số)
        private const int SeatWidth = 84;     // bề ngang cột ghế
        private const int SeatHeight = 52;    // bề cao ghế
        private const int AisleWidth = 40;    // lối đi giữa 3-3
        private const int SeatGap = 8;        // khoảng cách giữa ghế

        private Label lblTitle;
        private TableLayoutPanel root;
        private FlowLayoutPanel filter, legend;
        private UnderlinedComboBox cbFlight, cbAircraft, cbClass;
        private PrimaryButton btnRefresh;

        private Panel mapHost;                 // khung scroll chính
        private TableLayoutPanel centerLayout; // 3 cột: 50% | Auto | 50% để canh giữa
        private FlowLayoutPanel stack;         // chứa các group cabin (Business/Economy)
        private ToolTip tip;

        public SeatMapControl() { InitializeComponent(); BuildDemoMap(); }

        private void InitializeComponent()
        {
            SuspendLayout();
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);
            tip = new ToolTip();

            lblTitle = new Label
            {
                Text = "🗺️ Sơ đồ ghế (Seat Map)",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // Filters
            filter = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(24, 12, 24, 12),
                WrapContents = false
            };
            cbFlight = new UnderlinedComboBox("Chuyến bay", new object[] { "VN001", "VN002", "VN003" }) { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            cbAircraft = new UnderlinedComboBox("Máy bay", new object[] { "Airbus A320", "Boeing 737" }) { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            cbClass = new UnderlinedComboBox("Hạng", new object[] { "Tất cả", "Economy", "Business", "First" }) { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            btnRefresh = new PrimaryButton("⟳ Làm mới") { Width = 110, Height = 36 };
            btnRefresh.Click += (_, __) => { stack.Controls.Clear(); BuildDemoMap(); };
            filter.Controls.AddRange(new Control[] { cbFlight, cbAircraft, cbClass, btnRefresh });

            // Legend
            legend = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(24, 6, 24, 0),
                WrapContents = false
            };
            legend.Controls.Add(Badge("AVAILABLE", Color.FromArgb(232, 245, 233), Color.FromArgb(27, 94, 32)));
            legend.Controls.Add(Badge("BOOKED", Color.FromArgb(236, 239, 241), Color.FromArgb(55, 71, 79)));
            legend.Controls.Add(Badge("BLOCKED", Color.FromArgb(255, 235, 238), Color.FromArgb(183, 28, 28)));

            // Map host – scroll + padding đáy lớn để không khuất nút
            mapHost = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(32, 16, 32, 72),
                AutoScroll = true,
                AutoScrollMargin = new Size(0, 72),
                BackColor = Color.White
            };

            // Layout canh giữa: Dock=Top + AutoSize để cao hơn mapHost -> có scroll
            centerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 3,
                RowCount = 1
            };
            centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            centerLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Stack cabin (TopDown) nằm ở cột giữa để canh giữa toàn bộ sơ đồ
            stack = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0)
            };
            centerLayout.Controls.Add(stack, 1, 0);
            mapHost.Controls.Add(centerLayout);

            // Root
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

        // ===================== BUILD DEMO MAP =====================
        private void BuildDemoMap()
        {
            string selectedClass = cbClass?.SelectedItem?.ToString() ?? "Tất cả";

            // Demo: 2 khoang
            (string Name, int Start, int End)[] cabins = {
                ("Business", 1, 6),
                ("Economy", 7, 30)
            };

            stack.SuspendLayout();
            stack.Controls.Clear();

            foreach (var cabin in cabins)
            {
                if (selectedClass != "Tất cả" &&
                    !cabin.Name.Equals(selectedClass, StringComparison.OrdinalIgnoreCase))
                    continue;

                var card = new GroupBox
                {
                    Text = cabin.Name,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    Padding = new Padding(16),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Margin = new Padding(0, 0, 0, 24)
                };

                // 8 cột: [RowLabel] [A] [B] [C] [Aisle] [D] [E] [F]
                var grid = new TableLayoutPanel
                {
                    ColumnCount = 8,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };

                // Khóa chiều rộng cột để bố cục đều
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, RowLabelWidth)); // 0
                for (int i = 0; i < 3; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, SeatWidth)); // 1..3
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, AisleWidth)); // 4
                for (int i = 0; i < 3; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, SeatWidth)); // 5..7

                int totalRows = cabin.End - cabin.Start + 1;
                grid.RowCount = totalRows;

                for (int r = 0; r < totalRows; r++)
                {
                    grid.RowStyles.Add(new RowStyle(SizeType.Absolute, SeatHeight + SeatGap));
                    int rowNo = cabin.Start + r;

                    // Cột 0: số hàng
                    var lb = new Label
                    {
                        Text = rowNo.ToString(),
                        AutoSize = false,
                        Width = 50, // 👉 tăng chiều rộng
                        Height = SeatHeight,
                        Font = new Font("Segoe UI", 12f, FontStyle.Bold), // 👉 chữ to hơn
                        TextAlign = ContentAlignment.MiddleCenter,       // 👉 căn giữa thay vì căn phải
                        Margin = new Padding(0, SeatGap / 2, SeatGap, 0)
                    };
                    grid.Controls.Add(lb, 0, r);

                    // A B C
                    grid.Controls.Add(MakeSeat(rowNo, 'A', cabin.Name), 1, r);
                    grid.Controls.Add(MakeSeat(rowNo, 'B', cabin.Name), 2, r);
                    grid.Controls.Add(MakeSeat(rowNo, 'C', cabin.Name), 3, r);

                    // Lối đi (panel rỗng giữ chiều cao hàng)
                    grid.Controls.Add(new Panel { Width = AisleWidth, Height = 1 }, 4, r);

                    // D E F
                    grid.Controls.Add(MakeSeat(rowNo, 'D', cabin.Name), 5, r);
                    grid.Controls.Add(MakeSeat(rowNo, 'E', cabin.Name), 6, r);
                    grid.Controls.Add(MakeSeat(rowNo, 'F', cabin.Name), 7, r);
                }

                card.Controls.Add(grid);
                stack.Controls.Add(card);
            }

            stack.ResumeLayout(true);
        }

        // ===================== HELPERS =====================
        private Button MakeSeat(int row, char col, string cabinName)
        {
            string code = $"{row}{col}";
            string status = (row % 13 == 0) ? "BLOCKED"
                          : ((row + col) % 5 == 0 ? "BOOKED" : "AVAILABLE");

            Button btn = USE_PLAIN_BUTTON_FOR_COLOR
                ? new Button()
                : new SecondaryButton();

            btn.Text = code;
            btn.AutoSize = false;
            btn.Size = new Size(SeatWidth - SeatGap, SeatHeight);
            btn.Margin = new Padding(SeatGap / 2);
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.UseCompatibleTextRendering = true;
            btn.UseVisualStyleBackColor = false;

            StyleSeat(btn, status);

            int price = (cabinName == "Business") ? 1_800_000 : 900_000;
            tip.SetToolTip(btn, $"{cabinName} • {status} • Giá: {price:n0}₫");

            btn.Click += (_, __) =>
                MessageBox.Show($"Ghế {code}\nHạng: {cabinName}\nTrạng thái: {status}", "Chi tiết ghế");

            return btn;
        }

        private void StyleSeat(Button btn, string status)
        {
            // Bắt buộc để màu nền tự đặt có hiệu lực và không bị đổi khi hover/nhấn
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

            // giữ màu khi hover/nhấn
            btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
            btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
        }
    }
}