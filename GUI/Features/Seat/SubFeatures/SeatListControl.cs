using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using BUS.Seat; // Thêm namespace BUS
using DTO.Seat; // Thêm namespace DTO
using System.Threading.Tasks;

namespace GUI.Features.Seat.SubFeatures
{
    public class SeatListControl : UserControl
    {
        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        public readonly SeatBUS _seatBUS;
        public event Action<int> ViewOrEditRequested;
        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight;
        private Label lblTitle;

        // Giữ lại UnderlinedComboBoxs vì chúng là custom components của bạn
        private UnderlinedComboBox cbAircraft, cbClass;
        private UnderlinedTextField txtSeat;
        private PrimaryButton btnSearch;
        private SecondaryButton btnClear;

        private TableCustom table;
        private System.Windows.Forms.Timer debounce;

        private List<SeatDTO> datasource = new();
        public event Action<int> EditRequested;
        public SeatListControl()
        {
            _seatBUS = new SeatBUS();
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label
            {
                Text = "🪑 Danh sách ghế",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // Filters
            filterLeft = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false };
            cbAircraft = new UnderlinedComboBox("Máy bay", new object[] { "Tất cả" }) { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            cbClass = new UnderlinedComboBox("Hạng", new object[] { "Tất cả" }) { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            txtSeat = new UnderlinedTextField("Số ghế (VD: 12A)", "") { Width = 160, Margin = new Padding(0, 0, 24, 0) };
            filterLeft.Controls.AddRange(new Control[] { cbAircraft, cbClass, txtSeat });

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

            // Table
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 24),
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToAddRows = false, // <--- Đã THÊM dòng này để loại bỏ hàng thừa
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            table.Columns.Add("seatNumber", "Số ghế");
            table.Columns.Add("className", "Hạng");
            table.Columns.Add("aircraft", "Máy bay");

            var colAction = new DataGridViewTextBoxColumn { Name = ACTION_COL, HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells };
            table.Columns.Add(colAction);
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "seatIdHidden", Visible = false });

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // Events
            btnSearch.Click += (_, __) => ApplyFilter();
            btnClear.Click += (_, __) => { cbAircraft.SelectedIndex = 0; cbClass.SelectedIndex = 0; txtSeat.Text = ""; ApplyFilter(); };

            // Sử dụng debounce cho txtSeat
            txtSeat.TextChanged += (_, __) => { debounce.Stop(); debounce.Start(); };

            cbAircraft.SelectedIndexChanged += (_, __) => ApplyFilter();
            cbClass.SelectedIndexChanged += (_, __) => ApplyFilter();

            debounce = new System.Windows.Forms.Timer { Interval = 280 };
            debounce.Tick += (_, __) => { debounce.Stop(); ApplyFilter(); };

            // Root
            root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(filterWrap, 0, 1);
            root.Controls.Add(table, 0, 2);

            Controls.Clear();
            Controls.Add(root);
            ResumeLayout(false);
        }

        // Thay thế SeedDemo bằng LoadData
        public async void LoadData()
        {
            try
            {
                // Gọi phương thức BUS mới để lấy dữ liệu chi tiết
                var seatsWithDetails = _seatBUS.GetAllSeatsWithDetails();

                // Cập nhật datasource
                datasource = seatsWithDetails;

                // Cập nhật ComboBox
                UpdateFilterComboBoxes(seatsWithDetails);

                // Áp dụng bộ lọc và hiển thị dữ liệu
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu ghế: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                datasource = new List<SeatDTO>();
                ApplyFilter();
            }
        }

        private void UpdateFilterComboBoxes(List<SeatDTO> data)
        {
            // Tạm ngắt sự kiện để tránh gọi ApplyFilter
            cbAircraft.SelectedIndexChanged -= (_, __) => ApplyFilter();
            cbClass.SelectedIndexChanged -= (_, __) => ApplyFilter();

            // Cập nhật cbAircraft
            var aircrafts = data.Select(x => $"{x.AircraftManufacturer} {x.AircraftModel}").Distinct().OrderBy(x => x).ToList();
            cbAircraft.Items.Clear();
            cbAircraft.Items.Add("Tất cả");
            cbAircraft.Items.AddRange(aircrafts.Cast<object>().ToArray());
            cbAircraft.SelectedIndex = 0;

            // Cập nhật cbClass
            var classes = data.Select(x => x.ClassName).Distinct().OrderBy(x => x).ToList();
            cbClass.Items.Clear();
            cbClass.Items.Add("Tất cả");
            cbClass.Items.AddRange(classes.Cast<object>().ToArray());
            cbClass.SelectedIndex = 0;

            // Bật lại sự kiện
            cbAircraft.SelectedIndexChanged += (_, __) => ApplyFilter();
            cbClass.SelectedIndexChanged += (_, __) => ApplyFilter();
        }

        private void ApplyFilter()
        {
            string ac = cbAircraft.SelectedItem?.ToString() ?? "Tất cả";
            string cl = cbClass.SelectedItem?.ToString() ?? "Tất cả";
            string key = (txtSeat.Text ?? "").Trim().ToUpper();

            var q = datasource.AsEnumerable();

            // Lọc theo Máy bay: Lọc theo manufacturer và model
            if (ac != "Tất cả") q = q.Where(x => $"{x.AircraftManufacturer} {x.AircraftModel}" == ac);

            // Lọc theo Hạng ghế
            if (cl != "Tất cả") q = q.Where(x => x.ClassName == cl);

            // Lọc theo Số ghế
            if (!string.IsNullOrEmpty(key)) q = q.Where(x => x.SeatNumber.Contains(key));

            table.Rows.Clear();
            foreach (var x in q)
            {
                table.Rows.Add(
                    x.SeatNumber,
                    x.ClassName,
                    $"{x.AircraftManufacturer} {x.AircraftModel}",
                    null, // Action column value (sẽ được vẽ lại)
                    x.SeatId
                );
            }
            // Loại bỏ lệnh InvalidateColumn vì Rows.Add đã tự động kích hoạt quá trình vẽ lại.
            // Nếu bạn sử dụng TableCustom tùy chỉnh có thể cần: table.Refresh();
        }

        // ===== Action links drawing (Cải tiến logic kiểm tra dữ liệu) =====
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDel) GetRects(Rectangle bounds, Font font)
        {
            int pad = 6, x = bounds.Left + pad, y = bounds.Top + (bounds.Height - font.Height) / 2;
            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szE = TextRenderer.MeasureText(TXT_EDIT, font, Size.Empty, flags);
            var szD = TextRenderer.MeasureText(TXT_DEL, font, Size.Empty, flags);
            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcE = new Rectangle(new Point(x, y), szE); x += szE.Width + szS.Width;
            var rcD = new Rectangle(new Point(x, y), szD);
            return (rcV, rcE, rcD);
        }

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            // KIỂM TRA QUAN TRỌNG: Chỉ vẽ nếu hàng có dữ liệu (SeatIdHidden có giá trị)
            var hiddenIdCell = table.Rows[e.RowIndex].Cells["seatIdHidden"];
            if (hiddenIdCell.Value == null || string.IsNullOrWhiteSpace(hiddenIdCell.Value.ToString())) return;

            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var font = e.CellStyle.Font ?? table.Font;
            var r = GetRects(e.CellBounds, font);
            Color link = Color.FromArgb(0, 92, 175), sep = Color.FromArgb(120, 120, 120), del = Color.FromArgb(220, 53, 69);
            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, r.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, r.rcEdit.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcEdit.Right, r.rcEdit.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_DEL, font, r.rcDel.Location, del, TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }

            // Kiểm tra dữ liệu
            var hiddenIdCell = table.Rows[e.RowIndex].Cells["seatIdHidden"];
            if (hiddenIdCell.Value == null || string.IsNullOrWhiteSpace(hiddenIdCell.Value.ToString())) { table.Cursor = Cursors.Default; return; }

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcEdit.Contains(p) || r.rcDel.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var row = table.Rows[e.RowIndex];
            var seatIdValue = row.Cells["seatIdHidden"].Value;

            // Kiểm tra ID ghế và thoát nếu hàng không hợp lệ (hàng thừa)
            if (seatIdValue == null || !int.TryParse(seatIdValue.ToString(), out int id)) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            // Xử lý sự kiện Xem/Sửa
            

            if (r.rcView.Contains(p))
            {
                // HÀNH ĐỘNG 1: XEM CHI TIẾT
                ViewOrEditRequested?.Invoke(id); // Giữ nguyên sự kiện VIEW (chuyển sang Detail)
            }
            else if (r.rcEdit.Contains(p))
            {
                // HÀNH ĐỘNG 2: SỬA (Gọi sự kiện Sửa riêng)
                EditRequested?.Invoke(id); // <--- Kích hoạt sự kiện SỬA
            }
            // Xử lý sự kiện Xóa
            else if (r.rcDel.Contains(p))
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa ghế #{id}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (_seatBUS.DeleteSeat(id, out string message))
                    {
                        MessageBox.Show("Đã xóa ghế thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Sau khi xóa thành công, gọi LoadData để tải lại danh sách
                        LoadData();
                    }
                    else
                    {
                        // Hiển thị thông báo lỗi từ Business Logic (ví dụ: ghế đang được sử dụng)
                        MessageBox.Show($"Lỗi khi xóa ghế: {message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}