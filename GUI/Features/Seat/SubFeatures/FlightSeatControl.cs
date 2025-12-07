using BUS.FlightSeat;
using DTO.FlightSeat;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Seat.SubFeatures
{
    public class FlightSeatControl : UserControl
    {
        // --- Constants ---
        private const int RowLabelWidth = 40;
        private const int SeatWidth = 70;
        private const int SeatHeight = 45;
        private const int AisleWidth = 30;
        private const int SeatGap = 6;

        // --- Logic & Data ---
        private readonly FlightSeatBUS _bus = new();
        private List<FlightSeatDTO> datasource = new();

        // --- UI Elements ---
        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight, legend;
        private Label lblTitle;

        // <--- THAY ĐỔI: Thay TextField bằng 2 ComboBox ---
        private UnderlinedComboBox cbFlight;
        private UnderlinedComboBox cbAircraft;
        // -------------------------------------------------

        private PrimaryButton btnSearch;
        private SecondaryButton btnClear;

        // Container chính
        private Panel mapHost;
        private TableLayoutPanel mainStack;
        private ToolTip tip;

        public FlightSeatControl()
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
                Text = "🛫 Quản lý ghế (Dạng sơ đồ)",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // 2. Filter Layout
            filterLeft = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false };

            // <--- THAY ĐỔI: Khởi tạo 2 ComboBox Lọc ---
            cbFlight = new UnderlinedComboBox("Chuyến bay", Array.Empty<object>())
            {
                Width = 200,
                Margin = new Padding(0, 0, 24, 0)
            };
            // Cài đặt style (nếu control hỗ trợ InnerCombo)
            if (cbFlight.InnerCombo != null) cbFlight.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

            cbAircraft = new UnderlinedComboBox("Máy bay", Array.Empty<object>())
            {
                Width = 220,
                Margin = new Padding(0, 0, 24, 0)
            };
            if (cbAircraft.InnerCombo != null) cbAircraft.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

            filterLeft.Controls.Add(cbFlight);
            filterLeft.Controls.Add(cbAircraft);
            // ------------------------------------------

            filterRight = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            btnSearch = new PrimaryButton("🔍 Tìm kiếm") { Width = 110, Height = 36 };
            btnClear = new SecondaryButton("⟲ Làm mới") { Width = 100, Height = 36, Margin = new Padding(12, 0, 0, 0) };
            filterRight.Controls.Add(btnSearch);
            filterRight.Controls.Add(btnClear);

            filterWrap = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(24, 16, 24, 0), ColumnCount = 2 };
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filterWrap.Controls.Add(filterLeft, 0, 0);
            filterWrap.Controls.Add(filterRight, 1, 0);

            // 3. Legend
            legend = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 10, 24, 10),
                WrapContents = false
            };
            legend.Controls.Add(Badge("AVAILABLE", Color.FromArgb(232, 245, 233), Color.FromArgb(27, 94, 32)));
            legend.Controls.Add(Badge("BOOKED", Color.FromArgb(236, 239, 241), Color.FromArgb(55, 71, 79)));
            legend.Controls.Add(Badge("BLOCKED", Color.FromArgb(255, 235, 238), Color.FromArgb(183, 28, 28)));

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
                ColumnCount = 1,
                RowCount = 0,
                Padding = new Padding(0, 0, 0, 50)
            };
            mainStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            mapHost.Controls.Add(mainStack);

            // Events
            btnSearch.Click += (_, __) => ApplyFilter();

            // Nút Làm mới: Reset filters và load lại data gốc
            btnClear.Click += (_, __) =>
            {
                cbFlight.SelectedIndex = 0;
                cbAircraft.SelectedIndex = 0;
                LoadData();
            };

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

        private Control Badge(string status, Color bg, Color fg)
        {
            var p = new Panel { BackColor = bg, Height = 24, Padding = new Padding(10, 3, 10, 3), Margin = new Padding(0, 0, 12, 0), AutoSize = true };
            p.Controls.Add(new Label { Text = status, AutoSize = true, ForeColor = fg, Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            return p;
        }

        // --------------------------- DATA LOGIC ---------------------------
        private void LoadData()
        {
            try
            {
                datasource = _bus.GetAllWithDetails();

                // <--- CẬP NHẬT: Load dữ liệu vào ComboBox ---
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
            // 1. Load Chuyến bay
            var flights = datasource.Select(x => x.FlightName).Distinct().OrderBy(x => x).ToArray();
            cbFlight.Items.Clear();
            cbFlight.Items.Add("Tất cả");
            cbFlight.Items.AddRange(flights);
            cbFlight.SelectedIndex = 0;

            // 2. Load Máy bay
            var aircrafts = datasource.Select(x => x.AircraftName).Distinct().OrderBy(x => x).ToArray();
            cbAircraft.Items.Clear();
            cbAircraft.Items.Add("Tất cả");
            cbAircraft.Items.AddRange(aircrafts);
            cbAircraft.SelectedIndex = 0;
        }

        private void ApplyFilter()
        {
            // Bắt đầu từ toàn bộ dữ liệu
            var filtered = datasource.AsEnumerable();

            // 1. Lọc theo Chuyến bay
            if (cbFlight.SelectedIndex > 0 && cbFlight.SelectedItem != null)
            {
                string selectedFlight = cbFlight.SelectedItem.ToString();
                filtered = filtered.Where(x => x.FlightName == selectedFlight);
            }

            // 2. Lọc theo Máy bay
            if (cbAircraft.SelectedIndex > 0 && cbAircraft.SelectedItem != null)
            {
                string selectedAircraft = cbAircraft.SelectedItem.ToString();
                filtered = filtered.Where(x => x.AircraftName == selectedAircraft);
            }

            // Render kết quả
            RenderData(filtered.ToList());
        }

        // --------------------------- RENDER MAP (GIỮ NGUYÊN) ---------------------------
        private void RenderData(List<FlightSeatDTO> data)
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

            var grouped = data.GroupBy(x => new { x.FlightId, x.FlightName, x.AircraftName })
                              .OrderBy(g => g.Key.FlightId);

            foreach (var group in grouped)
            {
                mainStack.RowCount++;
                mainStack.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var card = CreateFlightMapCard(group.Key.FlightName, group.Key.AircraftName, group.ToList());
                mainStack.Controls.Add(card, 0, mainStack.RowCount - 1);
            }

            mainStack.ResumeLayout();
        }

        private GroupBox CreateFlightMapCard(string flightName, string aircraftName, List<FlightSeatDTO> seats)
        {
            var gb = new GroupBox
            {
                Text = $"   ✈  {flightName.ToUpper()}   |   MÁY BAY: {aircraftName.ToUpper()}   ",
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

            // Logic an toàn để tránh lỗi trùng key
            var seatDict = new Dictionary<string, FlightSeatDTO>();
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

                    if (seatDict.TryGetValue(sn, out FlightSeatDTO seat))
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

        // --------------------------- BUTTONS & ACTIONS (GIỮ NGUYÊN) ---------------------------
        private Button MakeSeatButton(FlightSeatDTO seat)
        {
            var btn = new Button
            {
                Text = $"{seat.SeatNumber}\n{seat.BasePrice:#,0}",
                Font = new Font("Segoe UI", 8f),
                Size = new Size(SeatWidth - SeatGap, SeatHeight),
                Margin = new Padding(SeatGap / 2),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = seat
            };

            switch (seat.SeatStatus)
            {
                case "AVAILABLE":
                    btn.BackColor = Color.FromArgb(232, 245, 233); btn.ForeColor = Color.FromArgb(27, 94, 32);
                    btn.FlatAppearance.BorderColor = Color.FromArgb(165, 214, 167);
                    break;
                case "BOOKED":
                    btn.BackColor = Color.FromArgb(236, 239, 241); btn.ForeColor = Color.FromArgb(84, 110, 122);
                    break;
                case "BLOCKED":
                    btn.BackColor = Color.FromArgb(255, 235, 238); btn.ForeColor = Color.FromArgb(198, 40, 40);
                    break;
                default:
                    btn.BackColor = Color.Gray;
                    break;
            }

            btn.Click += (s, e) => ShowActionMenu(seat, btn);
            return btn;
        }

        private void ShowActionMenu(FlightSeatDTO seat, Control anchor)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("ℹ️ Xem chi tiết", null, (_, __) => HandleView(seat));
            menu.Items.Add("✏️ Sửa thông tin", null, (_, __) => HandleEdit(seat));

            if (seat.SeatStatus == "AVAILABLE")
            {
                menu.Items.Add(new ToolStripSeparator());
                var itemBlock = new ToolStripMenuItem("🔒 Chặn ghế này") { ForeColor = Color.Red };
                itemBlock.Click += (_, __) => HandleBlock(seat);
                menu.Items.Add(itemBlock);
            }
            menu.Show(anchor, 0, anchor.Height);
        }

        private void HandleView(FlightSeatDTO selected)
        {
            MessageBox.Show($"Chi tiết ghế {selected.SeatNumber}\nTrạng thái: {selected.SeatStatus}\nGiá: {selected.BasePrice:#,0}đ", "Thông tin");
        }

        private void HandleEdit(FlightSeatDTO selected)
        {
            try
            {
                var editForm = new EditFlightSeatForm(
                    selected.FlightSeatId,
                    selected.AircraftId,
                    selected.SeatId,
                    selected.ClassId,
                    selected.BasePrice
                );

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    var updated = new FlightSeatDTO(
                        selected.FlightSeatId,
                        selected.FlightId,
                        editForm.SelectedSeatId,
                        editForm.NewPrice,
                        selected.SeatStatus
                    );

                    if (_bus.UpdateFlightSeat(updated, out string msg))
                    {
                        MessageBox.Show("✅ " + msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Áp dụng lại bộ lọc hiện tại sau khi sửa
                        ApplyFilter();
                    }
                    else
                    {
                        MessageBox.Show("❌ " + msg, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleBlock(FlightSeatDTO selected)
        {
            if (MessageBox.Show($"Bạn có chắc chắn muốn CHẶN ghế {selected.SeatNumber}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_bus.UpdateSeatStatus(selected.FlightSeatId, "BLOCKED", out string msg))
                {
                    MessageBox.Show("✅ Đã chặn ghế thành công.");
                    ApplyFilter();
                }
                else
                {
                    MessageBox.Show("❌ " + msg);
                }
            }
        }
    }
}