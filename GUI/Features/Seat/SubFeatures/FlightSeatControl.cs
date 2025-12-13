using BUS.FlightSeat;
using BUS.CabinClass;
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
        private readonly CabinClassBUS _cabinClassBUS = new();
        private List<FlightSeatDTO> datasource = new();

        // ✅ Event để notify các view khác cần refresh
        public event EventHandler? DataUpdated;

        // --- UI Elements ---
        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight, legend;
        private Label lblTitle;

        private UnderlinedComboBox cbFlight;
        private UnderlinedComboBox cbAircraft;
        private UnderlinedComboBox cbClass;

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

            cbFlight = new UnderlinedComboBox("Chuyến bay", Array.Empty<object>())
            {
                Width = 200,
                Margin = new Padding(0, 0, 24, 0)
            };
            if (cbFlight.InnerCombo != null) cbFlight.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

            cbAircraft = new UnderlinedComboBox("Máy bay", Array.Empty<object>())
            {
                Width = 220,
                Margin = new Padding(0, 0, 24, 0)
            };
            if (cbAircraft.InnerCombo != null) cbAircraft.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

            cbClass = new UnderlinedComboBox("Hạng ghế", Array.Empty<object>())
            {
                Width = 180,
                Margin = new Padding(0, 0, 24, 0)
            };
            if (cbClass.InnerCombo != null) cbClass.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

            filterLeft.Controls.Add(cbFlight);
            filterLeft.Controls.Add(cbAircraft);
            filterLeft.Controls.Add(cbClass);

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

            btnClear.Click += (_, __) =>
            {
                cbFlight.SelectedIndex = 0;
                cbAircraft.SelectedIndex = 0;
                cbClass.SelectedIndex = 0;
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
            // Chuyến bay
            var flights = datasource.Select(x => x.FlightName).Distinct().OrderBy(x => x).ToArray();
            cbFlight.Items.Clear();
            cbFlight.Items.Add("Tất cả");
            cbFlight.Items.AddRange(flights);
            cbFlight.SelectedIndex = 0;

            // Máy bay
            var aircrafts = datasource.Select(x => x.AircraftName).Distinct().OrderBy(x => x).ToArray();
            cbAircraft.Items.Clear();
            cbAircraft.Items.Add("Tất cả");
            cbAircraft.Items.AddRange(aircrafts);
            cbAircraft.SelectedIndex = 0;

            // Hạng ghế - Load từ database
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
                .ToArray();

            cbClass.Items.Clear();
            cbClass.Items.Add("Tất cả");
            cbClass.Items.AddRange(classes);
            cbClass.SelectedIndex = 0;
        }

        private void ApplyFilter()
        {
            var filtered = datasource.AsEnumerable();

            if (cbFlight.SelectedIndex > 0 && cbFlight.SelectedItem != null)
            {
                string selectedFlight = cbFlight.SelectedItem.ToString();
                filtered = filtered.Where(x => x.FlightName == selectedFlight);
            }

            if (cbAircraft.SelectedIndex > 0 && cbAircraft.SelectedItem != null)
            {
                string selectedAircraft = cbAircraft.SelectedItem.ToString();
                filtered = filtered.Where(x => x.AircraftName == selectedAircraft);
            }

            if (cbClass.SelectedIndex > 0 && cbClass.SelectedItem != null)
            {
                string selectedClass = cbClass.SelectedItem.ToString();
                filtered = filtered.Where(x => x.ClassName == selectedClass);
            }

            RenderData(filtered.ToList());
        }

        // --------------------------- RENDER MAP ---------------------------
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

        // --------------------------- BUTTONS & ACTIONS ---------------------------
        private Button MakeSeatButton(FlightSeatDTO seat)
        {
            var btn = new Button
            {
                Text = $"{seat.SeatNumber}\n{seat.BasePrice:#,0}",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                Size = new Size(SeatWidth - SeatGap, SeatHeight),
                Margin = new Padding(SeatGap / 2),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = seat
            };

            btn.FlatAppearance.BorderSize = 2;

            switch (seat.SeatStatus)
            {
                case "AVAILABLE":
                    btn.BackColor = Color.FromArgb(232, 245, 233);
                    btn.ForeColor = Color.FromArgb(27, 94, 32);
                    btn.FlatAppearance.BorderColor = Color.FromArgb(76, 175, 80);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 230, 201);
                    break;
                case "BOOKED":
                    btn.BackColor = Color.FromArgb(236, 239, 241);
                    btn.ForeColor = Color.FromArgb(55, 71, 79);
                    btn.FlatAppearance.BorderColor = Color.FromArgb(176, 190, 197);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 225, 228);
                    break;
                case "BLOCKED":
                    btn.BackColor = Color.FromArgb(255, 235, 238);
                    btn.ForeColor = Color.FromArgb(183, 28, 28);
                    btn.FlatAppearance.BorderColor = Color.FromArgb(229, 115, 115);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 205, 210);
                    break;
                default:
                    btn.BackColor = Color.Gray;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = Color.DarkGray;
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

            menu.Items.Add(new ToolStripSeparator());

            if (seat.SeatStatus == "AVAILABLE")
            {
                // AVAILABLE -> BLOCKED
                var itemBlock = new ToolStripMenuItem("🔒 Chặn ghế này") { ForeColor = Color.FromArgb(220, 53, 69) };
                itemBlock.Click += (_, __) => HandleBlock(seat);
                menu.Items.Add(itemBlock);
            }
            else if (seat.SeatStatus == "BLOCKED")
            {
                // BLOCKED -> AVAILABLE
                var itemUnblock = new ToolStripMenuItem("🔓 Mở khóa ghế này") { ForeColor = Color.FromArgb(40, 167, 69) };
                itemUnblock.Click += (_, __) => HandleUnblock(seat);
                menu.Items.Add(itemUnblock);
            }
            else if (seat.SeatStatus == "BOOKED")
            {
                // BOOKED -> AVAILABLE
                var itemUnbook = new ToolStripMenuItem("❌ Hủy đặt ghế") { ForeColor = Color.FromArgb(255, 193, 7) };
                itemUnbook.Click += (_, __) => HandleUnbook(seat);
                menu.Items.Add(itemUnbook);
            }

            menu.Show(anchor, 0, anchor.Height);
        }

        private void HandleView(FlightSeatDTO selected)
        {
            MessageBox.Show($"Chi tiết ghế {selected.SeatNumber}\nTrạng thái: {selected.SeatStatus}\nGiá: {selected.BasePrice:#,0}đ", "Thông tin");
        }

        // ✅ MÉTHODE CORRIGÉE - Remplacer dans FlightSeatControl.cs

        private void HandleEdit(FlightSeatDTO selected)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[BEFORE EDIT] FlightSeatId={selected.FlightSeatId}, SeatId={selected.SeatId}, ClassName={selected.ClassName}");

                // ✅ Truyền đầy đủ tham số: seatId, seatNumber, classId, price
                var editForm = new EditFlightSeatForm(
                    selected.FlightSeatId,
                    selected.SeatId,
                    selected.SeatNumber,
                    selected.ClassId,
                    selected.BasePrice
                );

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // ✅ BƯỚC 1: Cập nhật class_id trong bảng Seats (nếu thay đổi)
                    if (editForm.SelectedClassId != selected.ClassId)
                    {
                        if (!_bus.UpdateSeatClass(selected.SeatId, editForm.SelectedClassId, out string seatMsg))
                        {
                            MessageBox.Show("❌ " + seatMsg, "Lỗi cập nhật hạng ghế", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        System.Diagnostics.Debug.WriteLine($"[SEAT UPDATE] seat_id={selected.SeatId}, new class_id={editForm.SelectedClassId}");
                    }

                    // ✅ BƯỚC 2: Cập nhật giá trong bảng Flight_Seats (giữ nguyên seat_id)
                    var updated = new FlightSeatDTO(
                        selected.FlightSeatId,
                        selected.FlightId,
                        selected.AircraftId,
                        selected.SeatId,  // ✅ Giữ nguyên seat_id
                        editForm.SelectedClassId,  // ✅ Class ID mới
                        editForm.NewPrice,
                        selected.SeatStatus,
                        selected.FlightName,
                        selected.AircraftName,
                        selected.AircraftCapacity,
                        selected.SeatNumber,
                        selected.ClassName
                    );

                    System.Diagnostics.Debug.WriteLine($"[SENDING TO BUS] FlightSeatId={updated.FlightSeatId}, SeatId={updated.SeatId}");

                    if (_bus.UpdateFlightSeatPrice(updated.FlightSeatId, updated.BasePrice, out string msg))
                    {
                        System.Diagnostics.Debug.WriteLine($"[UPDATE SUCCESS] Message: {msg}");

                        MessageBox.Show("✅ " + msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // ✅ FIX: Reset filters và reload data
                        cbFlight.SelectedIndex = 0;
                        cbAircraft.SelectedIndex = 0;
                        cbClass.SelectedIndex = 0;

                        System.Diagnostics.Debug.WriteLine("[CALLING LoadData() with filters reset]");
                        LoadData();

                        System.Diagnostics.Debug.WriteLine($"[AFTER LoadData] Total seats: {datasource.Count}");

                        // ✅ Removed: RefreshSeatList (SeatListControl removed)
                    }
                    else
                    {
                        MessageBox.Show("❌ " + msg, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HandleEdit Exception] {ex.Message}");
                MessageBox.Show("Lỗi khi mở form sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HandleBlock(FlightSeatDTO selected)
        {
            if (MessageBox.Show($"Bạn có chắc chắn muốn CHẶN ghế {selected.SeatNumber}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_bus.UpdateSeatStatus(selected.FlightSeatId, "BLOCKED", out string msg))
                {
                    MessageBox.Show(" Đã chặn ghế thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ApplyFilter();
                }
                else
                {
                    MessageBox.Show(" " + msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void HandleUnblock(FlightSeatDTO selected)
        {
            if (MessageBox.Show($"Bạn có chắc chắn muốn MỞ KHÓA ghế {selected.SeatNumber}?\n\nGhế sẽ chuyển sang trạng thái AVAILABLE.", "Xác nhận mở khóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_bus.UpdateSeatStatus(selected.FlightSeatId, "AVAILABLE", out string msg))
                {
                    MessageBox.Show(" Đã mở khóa ghế thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ApplyFilter();
                }
                else
                {
                    MessageBox.Show(" " + msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void HandleUnbook(FlightSeatDTO selected)
        {
            if (MessageBox.Show($"Bạn có chắc chắn muốn HỦY ĐẶT ghế {selected.SeatNumber}?\n\nGhế sẽ chuyển sang trạng thái AVAILABLE.", "Xác nhận hủy đặt", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_bus.UpdateSeatStatus(selected.FlightSeatId, "AVAILABLE", out string msg))
                {
                    MessageBox.Show("✅ Đã hủy đặt ghế thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ApplyFilter();
                }
                else
                {
                    MessageBox.Show("❌ " + msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}