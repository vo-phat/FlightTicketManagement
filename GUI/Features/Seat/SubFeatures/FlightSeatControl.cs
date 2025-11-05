using BUS.FlightSeat;
using DTO.FlightSeat;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Seat.SubFeatures
{
    public class FlightSeatControl : UserControl
    {
        private const string ACTION_COL = "Action";
        private const string STATUS_COL = "status";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_BLOCK = "Chặn";
        private const string SEP = " / ";

        private readonly FlightSeatBUS _bus = new();
        private List<FlightSeatDTO> datasource = new();

        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight;
        private Label lblTitle;
        private UnderlinedComboBox cbAircraft, cbClass , cbStatus;
        private UnderlinedTextField txtSeat;
        private PrimaryButton btnSearch;
        private SecondaryButton btnClear;
        private TableCustom table;

        public FlightSeatControl()
        {
            InitializeComponent();
            LoadData();
        }

        // --------------------------- UI ---------------------------
        private void InitializeComponent()
        {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label
            {
                Text = "🛫 Danh sách ghế theo máy bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Bộ lọc =====
            filterLeft = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false };

            cbAircraft = new UnderlinedComboBox("Máy bay", new object[] { "Tất cả" }) { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            cbClass = new UnderlinedComboBox("Hạng", new object[] { "Tất cả", "Economy", "Business" }) { Width = 140, Margin = new Padding(0, 0, 24, 0) };

            // Khởi tạo cbStatus với tất cả các trạng thái
            cbStatus = new UnderlinedComboBox("Trạng thái", new object[]
            {
        "Tất cả", "AVAILABLE", "BOOKED", "BLOCKED"
            })
            {
                Width = 140,
                Margin = new Padding(0, 0, 24, 0)
            };

            txtSeat = new UnderlinedTextField("Số ghế", "") { Width = 120, Margin = new Padding(0, 0, 24, 0) };

            // SỬA LỖI: Thêm cbStatus vào filterLeft
            filterLeft.Controls.AddRange(new Control[] { cbAircraft, cbClass, txtSeat, cbStatus });


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

            // ===== Bảng =====
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 24),
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoGenerateColumns = false
            };

            table.Columns.Clear();
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "flightSeatIdHidden", Visible = false });
            table.Columns.Add("aircraftName", "Máy bay");
            table.Columns.Add("seatNumber", "Số ghế");
            table.Columns.Add("className", "Hạng");
            table.Columns.Add("basePrice", "Giá cơ bản (₫)");
            table.Columns.Add(STATUS_COL, "Trạng thái");

            var colAction = new DataGridViewTextBoxColumn
            {
                Name = ACTION_COL,
                HeaderText = "Thao tác",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            table.Columns.Add(colAction);

            // Sự kiện vẽ & click
            table.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (table.Columns[e.ColumnIndex].Name == "basePrice" && e.Value != null && decimal.TryParse(e.Value.ToString(), out var v))
                {
                    e.Value = v.ToString("#,0");
                    e.FormattingApplied = true;
                }
            };
            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // Sự kiện bộ lọc
            btnSearch.Click += (_, __) => ApplyFilter();
            btnClear.Click += (_, __) =>
            {
                cbAircraft.SelectedIndex = 0;
                cbClass.SelectedIndex = 0;
                cbStatus.SelectedIndex = 0; // Đã thêm
                txtSeat.Text = "";
                ApplyFilter();
            };

            // ===== Root =====
            root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(filterWrap, 0, 1);
            root.Controls.Add(table, 0, 2);

            Controls.Add(root);
            ResumeLayout(false);
        }

        // --------------------------- LOAD DỮ LIỆU ---------------------------
        private void LoadData()
        {
            try
            {
                datasource = _bus.GetAllWithDetails();
                ApplyFilter();
                LoadAircraftsToFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách ghế máy bay:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAircraftsToFilter()
        {
            var aircrafts = datasource.Select(x => x.AircraftName).Distinct().OrderBy(n => n).ToList();
            cbAircraft.Items.Clear();
            cbAircraft.Items.Add("Tất cả");
            foreach (var a in aircrafts) cbAircraft.Items.Add(a);
            cbAircraft.SelectedIndex = 0;
        }

        private void ApplyFilter()
        {
            string aircraft = cbAircraft.SelectedItem?.ToString() ?? "Tất cả";
            string cl = cbClass.SelectedItem?.ToString() ?? "Tất cả";
            string status = cbStatus.SelectedItem?.ToString() ?? "Tất cả";
            string key = (txtSeat.Text ?? "").Trim().ToUpper();

            var q = datasource.AsEnumerable();
            if (aircraft != "Tất cả") q = q.Where(x => x.AircraftName == aircraft);
            if (cl != "Tất cả") q = q.Where(x => x.ClassName == cl);
            if (status != "Tất cả")
               q = q.Where(x => x.SeatStatus.Equals(status, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(key)) q = q.Where(x => x.SeatNumber.Contains(key, StringComparison.OrdinalIgnoreCase));

            table.Rows.Clear();
            foreach (var x in q)
                table.Rows.Add(
                    x.FlightSeatId,
                    x.AircraftName,
                    x.SeatNumber,
                    x.ClassName,
                    x.BasePrice,
                    x.SeatStatus,
                    ""
                );
        }

        // --------------------------- VẼ ACTION ---------------------------
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcBlock) GetRects(Rectangle bounds, Font font)
        {
            int pad = 6, x = bounds.Left + pad, y = bounds.Top + (bounds.Height - font.Height) / 2;
            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szE = TextRenderer.MeasureText(TXT_EDIT, font, Size.Empty, flags);
            var szB = TextRenderer.MeasureText(TXT_BLOCK, font, Size.Empty, flags);
            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcE = new Rectangle(new Point(x, y), szE); x += szE.Width + szS.Width;
            var rcB = new Rectangle(new Point(x, y), szB);
            return (rcV, rcE, rcB);
        }

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var name = table.Columns[e.ColumnIndex].Name;

            if (name == STATUS_COL)
            {
                e.Handled = true;
                e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                var status = table.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
                var (bg, fg) = status switch
                {
                    "AVAILABLE" => (Color.FromArgb(220, 248, 225), Color.FromArgb(26, 115, 52)),
                    "BOOKED" => (Color.FromArgb(227, 230, 233), Color.FromArgb(66, 66, 66)),
                    _ => (Color.FromArgb(255, 230, 230), Color.FromArgb(179, 38, 30))
                };
                var r = new Rectangle(e.CellBounds.Left + 8, e.CellBounds.Top + 6, e.CellBounds.Width - 16, e.CellBounds.Height - 12);
                using var b = new SolidBrush(bg);
                e.Graphics.FillRectangle(b, r);
                TextRenderer.DrawText(e.Graphics, status, table.Font, r, fg, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            if (name == ACTION_COL)
            {
                e.Handled = true;
                e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                var font = e.CellStyle.Font ?? table.Font;
                var r = GetRects(e.CellBounds, font);
                Color link = Color.FromArgb(0, 92, 175), sep = Color.FromArgb(120, 120, 120), warn = Color.FromArgb(220, 53, 69);
                TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, r.rcView.Location, link, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, r.rcEdit.Location, link, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcEdit.Right, r.rcEdit.Top), sep, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, TXT_BLOCK, font, r.rcBlock.Location, warn, TextFormatFlags.NoPadding);
                return;
            }
        }

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || table.Columns[e.ColumnIndex].Name != ACTION_COL)
            {
                table.Cursor = Cursors.Default;
                return;
            }

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcEdit.Contains(p) || r.rcBlock.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }

        // --------------------------- CLICK ACTION ---------------------------
        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var idValue = table.Rows[e.RowIndex].Cells["flightSeatIdHidden"].Value;
            if (idValue == null || !int.TryParse(idValue.ToString(), out int flightSeatId))
            {
                MessageBox.Show("Không thể xác định Flight Seat ID.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var selected = datasource.FirstOrDefault(x => x.FlightSeatId == flightSeatId);
            if (selected == null)
            {
                MessageBox.Show("Không thể xác định dữ liệu ghế để sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (r.rcView.Contains(p))
            {
                MessageBox.Show(
                    $"Máy bay: {selected.AircraftName}\nGhế: {selected.SeatNumber}\nHạng: {selected.ClassName}\nGiá: {selected.BasePrice:#,0}₫\nTrạng thái: {selected.SeatStatus}",
                    "Chi tiết ghế", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (r.rcEdit.Contains(p))
            {
                using var formEdit = new EditFlightSeatForm(
                    selected.FlightSeatId,
                    selected.AircraftId,
                    selected.SeatId,
                    selected.ClassId,
                    selected.BasePrice
                );

                if (formEdit.ShowDialog() == DialogResult.OK)
                {
                    var dto = new FlightSeatDTO(
                        flightSeatId: formEdit.FlightSeatId,
                        flightId: selected.FlightId,
                        seatId: formEdit.SelectedSeatId,
                        basePrice: formEdit.NewPrice,
                        seatStatus: selected.SeatStatus
                    );

                    bool ok = _bus.UpdateFlightSeat(dto, out string msg);
                    if (ok)
                    {
                        MessageBox.Show("Cập nhật thông tin ghế thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show(msg, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
