using BUS.Airport;
using DTO.Airport;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Airport.SubFeatures
{
    public class AirportListControl : UserControl
    {
        private readonly AirportBUS _bus = new AirportBUS();
        private DataGridView table;
        private UnderlinedTextField txtCode;
        private UnderlinedTextField txtName;
        private UnderlinedTextField txtCity;
        private UnderlinedComboBox cbCountry;
        private Button btnSearch, btnAdd;
        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        public event Action<AirportDTO>? ViewRequested;
        public event Action<AirportDTO>? RequestEdit;
        public event Action? DataChanged;

        public AirportListControl()
        {
            InitializeComponent();
            LoadCountries();
            RefreshList();
        }

        // Lưu ý: Cần khai báo các biến private field như sau (nếu chưa có):
        // private UnderlinedTextField txtCode;
        // private UnderlinedTextField txtName;
        // private UnderlinedTextField txtCity;
        // private UnderlinedComboBox cbCountry;
        // private PrimaryButton btnSearch;
        // private SecondaryButton btnAdd; // Hoặc PrimaryButton nếu bạn muốn nó nổi bật như nút Search
        // private TableCustom table;
        // private const string ACTION_COL = "actionCol"; // Đảm bảo hằng số này đã được định nghĩa

        private void InitializeComponent()
        {
            // Cần đảm bảo đã using các namespace chứa components tùy chỉnh của bạn
            // ví dụ: using GUI.Components.Inputs;
            // using GUI.Components.Tables;
            // using GUI.Components.Buttons;

            // ... (Phần SuspendLayout, BackColor, lblTitle không thay đổi) ...

            SuspendLayout();
            BackColor = Color.FromArgb(232, 240, 252);
            Dock = DockStyle.Fill;
            AutoScroll = true;

            // === TIÊU ĐỀ ===
            var lblTitle = new Label
            {
                Text = "✈️ Danh sách sân bay",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(40, 55, 77),
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(24, 20, 0, 12)
            };

            // === PANEL BỘ LỌC (Dùng Custom Inputs & Buttons) ===
            var filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 8, 24, 8),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.FromArgb(250, 253, 255)
            };

            // --- INPUTS TÙY CHỈNH ---
            txtCode = new UnderlinedTextField("Mã IATA", "Ví dụ: SGN")
            {
                Width = 140,
                Margin = new Padding(6, 4, 6, 4),
                InheritParentBackColor = true,
                LineThickness = 1
            };

            txtName = new UnderlinedTextField("Tên sân bay", "Sân bay quốc tế...")
            {
                Width = 200,
                Margin = new Padding(6, 4, 6, 4),
                InheritParentBackColor = true,
                LineThickness = 1
            };

            txtCity = new UnderlinedTextField("Thành phố", "Hồ Chí Minh")
            {
                Width = 180,
                Margin = new Padding(6, 4, 6, 4),
                InheritParentBackColor = true,
                LineThickness = 1
            };

            cbCountry = new UnderlinedComboBox("Quốc gia", Array.Empty<string>())
            {
                Width = 180,
                Margin = new Padding(6, 4, 6, 4),
            };

            // --- BUTTON TÙY CHỈNH ---
            btnSearch = new PrimaryButton("🔍 Tìm")
            {
                Width = 90,
                Height = 40,
                Margin = new Padding(10, 6, 6, 6),
            };

            // Giữ nút Thêm là PrimaryButton, nhưng cần lưu ý nó sẽ có màu xanh brand của Primary
            btnAdd = new PrimaryButton("➕ Thêm")
            {
                Width = 110,
                Height = 40,
                Margin = new Padding(6),
            };

            btnSearch.Click += (s, e) => RefreshList();
            btnAdd.Click += (s, e) => OnAddClicked();

            filterPanel.Controls.AddRange(new Control[] { txtCode, txtName, txtCity, cbCountry, btnSearch, btnAdd });

            // === BẢNG DANH SÁCH TÙY CHỈNH (TableCustom) ===
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                CornerRadius = 16,
                BorderThickness = 2,
                BorderColor = Color.FromArgb(200, 200, 200),
            };

            // 1. Cấu hình các Cột
            table.Columns.Add("airportCode", "Mã IATA");
            table.Columns.Add("airportName", "Tên sân bay");
            table.Columns.Add("city", "Thành phố");
            table.Columns.Add("country", "Quốc gia");

            // Thêm cột Thao tác
            table.Columns.Add(ACTION_COL, "Thao tác");
            // Chiều rộng cho 3 nút: 3 * 40px/nút + padding = 140-160px là hợp lý.
            table.Columns[ACTION_COL].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            table.Columns[ACTION_COL].Width = 160;

            table.Columns.Add("airportIdHidden", "ID");
            table.Columns["airportIdHidden"].Visible = false;

          
            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // === GHÉP TOÀN BỘ GIAO DIỆN ===
            Controls.Clear();
            Controls.Add(table);
            Controls.Add(filterPanel);
            Controls.Add(lblTitle);

            ResumeLayout(false);
        }
        private void LoadCountries()
        {
            try
            {
                cbCountry.Items.Clear();
                cbCountry.Items.Add("Tất cả");

                var list = _bus.GetAllAirports();
                var countries = list.Select(a => a.Country)
                                    .Where(c => !string.IsNullOrWhiteSpace(c))
                                    .Distinct()
                                    .OrderBy(c => c)
                                    .ToList();

                foreach (var c in countries)
                    cbCountry.Items.Add(c);

                cbCountry.SelectedIndex = 0;
            }
            catch
            {
                cbCountry.Items.Clear();
                cbCountry.Items.Add("Tất cả");
                cbCountry.SelectedIndex = 0;
            }
        }

        private bool _isRefreshing = false;

        public void RefreshList()
        {
            if (_isRefreshing) return;
            _isRefreshing = true;

            try
            {
                string keyword = $"{txtCode.Text} {txtName.Text} {txtCity.Text}".Trim();
                List<AirportDTO> list;

                if (string.IsNullOrWhiteSpace(keyword) && (cbCountry.SelectedItem == null || cbCountry.SelectedItem.ToString() == "Tất cả"))
                    list = _bus.GetAllAirports();
                else
                    list = _bus.SearchAirports(keyword);

                if (cbCountry.SelectedItem != null && cbCountry.SelectedItem.ToString() != "Tất cả")
                    list = list.Where(a => a.Country == cbCountry.SelectedItem.ToString()).ToList();

                table.Rows.Clear();
                foreach (var a in list)
                {
                    table.Rows.Add(a.AirportCode, a.AirportName, a.City, a.Country, null, a.AirportId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void OnAddClicked()
        {
            RequestEdit?.Invoke(new AirportDTO());
        }

        // ======= Vẽ nút thao tác =======
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDel) GetRects(Rectangle b, Font f)
        {
            int pad = 6, x = b.Left + pad, y = b.Top + (b.Height - f.Height) / 2;
            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText(TXT_VIEW, f, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, f, Size.Empty, flags);
            var szE = TextRenderer.MeasureText(TXT_EDIT, f, Size.Empty, flags);
            var szD = TextRenderer.MeasureText(TXT_DEL, f, Size.Empty, flags);
            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcE = new Rectangle(new Point(x, y), szE); x += szE.Width + szS.Width;
            var rcD = new Rectangle(new Point(x, y), szD);
            return (rcV, rcE, rcD);
        }

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || table.Columns[e.ColumnIndex].Name != ACTION_COL) return;
            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
            var f = e.CellStyle.Font ?? table.Font;
            var r = GetRects(e.CellBounds, f);
            Color link = Color.FromArgb(0, 92, 175), sep = Color.FromArgb(120, 120, 120), del = Color.FromArgb(220, 53, 69);
            TextRenderer.DrawText(e.Graphics, TXT_VIEW, f, r.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, f, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_EDIT, f, r.rcEdit.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, f, new Point(r.rcEdit.Right, r.rcEdit.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_DEL, f, r.rcDel.Location, del, TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }
            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var f = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, f);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcEdit.Contains(p) || r.rcDel.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var f = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, f);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells["airportIdHidden"].Value);
            string code = row.Cells["airportCode"].Value?.ToString() ?? "";
            string name = row.Cells["airportName"].Value?.ToString() ?? "";
            string city = row.Cells["city"].Value?.ToString() ?? "";
            string country = row.Cells["country"].Value?.ToString() ?? "";

            var dto = new AirportDTO(id, code, name, city, country);

            if (r.rcView.Contains(p))
                ViewRequested?.Invoke(dto);
            else if (r.rcEdit.Contains(p))
                RequestEdit?.Invoke(dto);
            else if (r.rcDel.Contains(p))
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa sân bay '{name}'?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string message;
                    bool ok = _bus.DeleteAirport(id, out message);
                    MessageBox.Show(message, ok ? "Thành công" : "Lỗi",
                        MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                    if (ok) RefreshList();
                }
            }
        }
    }
}
