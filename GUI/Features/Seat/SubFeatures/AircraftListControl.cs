using BUS.Seat;
using BUS.CabinClass;
using DTO.Seat;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Seat.SubFeatures
{
    public class AircraftListControl : UserControl
    {
        // --- Constants ---
        private const int RowLabelWidth = 40;
        private const int SeatWidth = 70;
        private const int SeatHeight = 45;
        private const int AisleWidth = 30;
        private const int SeatGap = 6;

        // --- Logic & Data ---
        private readonly SeatBUS _seatBUS = new();
        private readonly CabinClassBUS _cabinClassBUS = new();
        private List<SeatDTO> datasource = new();

        // ✅ Event để trigger detail view
        public event Action<int> SeatSelected;

        // --- UI Elements ---
        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight, legend;
        private Label lblTitle;

        private UnderlinedComboBox cbAircraft;

        private PrimaryButton btnSearch;
       
private SecondaryButton btnClear;

        // Container chính
        private Panel mapHost;
        private TableLayoutPanel mainStack;
        private ToolTip tip;

        public AircraftListControl()
        {
            InitializeComponent();
            LoadData();
        }

        // --------------------------- UI INIT ---------------------------
        private void InitializeComponent()
        {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);
            tip = new ToolTip();

            // 1. Title
            lblTitle = new Label
            {
                Text = "✈️ Danh sách máy bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // 2. Filter Layout
            filterLeft = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false };

            cbAircraft = new UnderlinedComboBox("Máy bay", Array.Empty<object>())
            {
                Width = 220,
                Margin = new Padding(0, 0, 24, 0)
            };
            if (cbAircraft.InnerCombo != null) cbAircraft.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

            filterLeft.Controls.Add(cbAircraft);

            filterRight = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            btnSearch = new PrimaryButton("🔍 Tìm kiếm") { Width = 110, Height = 36 };
            btnClear = new SecondaryButton("⟲ Xóa lọc") { Width = 100, Height = 36, Margin = new Padding(12, 0, 0, 0) };
            filterRight.Controls.Add(btnSearch);
            filterRight.Controls.Add(btnClear);

            filterWrap = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(24, 16, 24, 0), ColumnCount = 2 };
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filterWrap.Controls.Add(filterLeft, 0, 0);
            filterWrap.Controls.Add(filterRight, 1, 0);

            // 3. Legend
            legend = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(24, 6, 24, 0), WrapContents = false };
            legend.Controls.Add(Badge("Ghế hiện có", Color.FromArgb(232, 245, 233), Color.FromArgb(27, 94, 32)));
            legend.Controls.Add(Badge("Vị trí trống", Color.WhiteSmoke, Color.Gray));

            // 4. Map Container
            mapHost = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(24, 10, 24, 24)
            };

            mainStack = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1
            };
            mapHost.Controls.Add(mainStack);

            // Events
            btnSearch.Click += (_, __) => ApplyFilter();
            btnClear.Click += (_, __) => { cbAircraft.SelectedIndex = 0; ApplyFilter(); };
            cbAircraft.SelectedIndexChanged += (_, __) => ApplyFilter();

            // Root Layout
            root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4 };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(filterWrap, 0, 1);
            root.Controls.Add(legend, 0, 2);
            root.Controls.Add(mapHost, 0, 3);

            Controls.Add(root);
            ResumeLayout(false);
        }

        private Control Badge(string text, Color bg, Color fg)
        {
            var p = new Panel { BackColor = bg, Height = 24, Padding = new Padding(10, 3, 10, 3), Margin = new Padding(0, 0, 12, 0), AutoSize = true };
            p.Controls.Add(new Label { Text = text, AutoSize = true, ForeColor = fg, Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            return p;
        }

        // --------------------------- DATA LOGIC ---------------------------
        public void LoadData()
        {
            try
            {
                datasource = _seatBUS.GetAllSeatsWithDetails();
                LoadFilterComboboxes();
                RenderData(datasource);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu:\n" + ex.Message);
            }
        }

        private void LoadFilterComboboxes()
        {
            // Máy bay
            var aircrafts = datasource.Select(x => x.AircraftModel).Distinct().OrderBy(x => x).ToArray();
            cbAircraft.Items.Clear();
            cbAircraft.Items.Add("Tất cả");
            cbAircraft.Items.AddRange(aircrafts);
            cbAircraft.SelectedIndex = 0;
        }

        private void ApplyFilter()
        {
            var filtered = datasource.AsEnumerable();

            if (cbAircraft.SelectedIndex > 0 && cbAircraft.SelectedItem != null)
            {
                string selectedAircraft = cbAircraft.SelectedItem.ToString();
                filtered = filtered.Where(x => x.AircraftModel == selectedAircraft);
            }

            RenderData(filtered.ToList());
        }

        // --------------------------- RENDER MAP ---------------------------
        private void RenderData(List<SeatDTO> data)
        {
            mainStack.SuspendLayout();
            mainStack.Controls.Clear();
            mainStack.RowStyles.Clear();
            mainStack.RowCount = 0;

            if (data == null || data.Count == 0)
            {
                var lbl = new Label { Text = "Không có dữ liệu phù hợp.", Font = new Font("Segoe UI", 12, FontStyle.Italic), AutoSize = true, Padding = new Padding(10) };
                mainStack.Controls.Add(lbl);
                mainStack.ResumeLayout();
                return;
            }

            var grouped = data.GroupBy(x => new { x.AircraftId, x.AircraftModel })
                              .OrderBy(g => g.Key.AircraftModel);

            foreach (var group in grouped)
            {
                mainStack.RowCount++;
                mainStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var card = CreateAircraftMapCard(group.Key.AircraftModel, group.ToList());
                mainStack.Controls.Add(card, 0, mainStack.RowCount - 1);
            }

            mainStack.ResumeLayout();
        }

        private GroupBox CreateAircraftMapCard(string aircraftName, List<SeatDTO> seats)
        {
            var gb = new GroupBox
            {
                Text = $"   ✈  {aircraftName}   ",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Padding = new Padding(10, 40, 10, 20),
                AutoSize = true,
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(0, 51, 102)
            };

            char[] cols = { 'A', 'B', 'C', 'D', 'E', 'F' };
            int aisleIndex = 3;
            int maxRow = seats.Select(s => int.TryParse(new string(s.SeatNumber.Where(char.IsDigit).ToArray()), out int n) ? n : 0).Max();
            if (maxRow == 0) maxRow = 10;

            var seatGrid = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = cols.Length + 2,
                BackColor = Color.White
            };

            seatGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, RowLabelWidth));
            for (int i = 0; i < cols.Length; i++)
            {
                if (i == aisleIndex) seatGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, AisleWidth));
                seatGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, SeatWidth));
            }

            seatGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            seatGrid.Controls.Add(new Label(), 0, 0);
            int hCol = 1;
            for (int i = 0; i < cols.Length; i++)
            {
                if (i == aisleIndex) seatGrid.Controls.Add(new Panel(), hCol++, 0);
                seatGrid.Controls.Add(new Label { Text = cols[i].ToString(), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, ForeColor = Color.Gray }, hCol++, 0);
            }

            var seatDict = new Dictionary<string, SeatDTO>();
            foreach (var s in seats)
            {
                if (!seatDict.ContainsKey(s.SeatNumber)) seatDict.Add(s.SeatNumber, s);
            }

            for (int r = 1; r <= maxRow; r++)
            {
                seatGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, SeatHeight + SeatGap));
                seatGrid.Controls.Add(new Label { Text = r.ToString(), TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Gray }, 0, r);

                int cCol = 1;
                for (int c = 0; c < cols.Length; c++)
                {
                    if (c == aisleIndex) seatGrid.Controls.Add(new Panel(), cCol++, r);
                    string sn = $"{r}{cols[c]}";

                    if (seatDict.TryGetValue(sn, out SeatDTO seat))
                        seatGrid.Controls.Add(MakeSeatButton(seat), cCol++, r);
                    else
                        seatGrid.Controls.Add(new Panel { BackColor = Color.WhiteSmoke }, cCol++, r);
                }
            }

            var centerWrapper = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 1
            };
            centerWrapper.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            seatGrid.Anchor = AnchorStyles.None;
            centerWrapper.Controls.Add(seatGrid, 0, 0);
            gb.Controls.Add(centerWrapper);

            return gb;
        }

        // --------------------------- BUTTONS & ACTIONS ---------------------------
        private Button MakeSeatButton(SeatDTO seat)
        {
            var btn = new Button
            {
                Text = $"{seat.SeatNumber}\n{seat.ClassName}",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                Size = new Size(SeatWidth - SeatGap, SeatHeight),
                Margin = new Padding(SeatGap / 2),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = seat
            };

            btn.FlatAppearance.BorderSize = 2;
            
            // ✅ Màu xanh cho tất cả ghế
            btn.BackColor = Color.FromArgb(232, 245, 233);
            btn.ForeColor = Color.FromArgb(27, 94, 32);
            btn.FlatAppearance.BorderColor = Color.FromArgb(76, 175, 80);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 230, 201);

            tip.SetToolTip(btn, $"Ghế {seat.SeatNumber} • {seat.ClassName}\nClick để xem chi tiết");

            btn.Click += (s, e) => SeatSelected?.Invoke(seat.SeatId);
            return btn;
        }
    }
}
