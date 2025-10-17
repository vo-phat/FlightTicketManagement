using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Tables;

namespace FlightTicketManagement.GUI.Features.Seat.SubFeatures {
    public class SeatTemplateControl : UserControl {
        private TableLayoutPanel _root;
        private TableLayoutPanel _columns;          // 3 cột (Left-Center-Right)
        private TableLayoutPanel _titleBar;         // thanh tiêu đề + nút phải

        private Label _title;

        // Left: aircraft list
        private TableCustom _tblAircraft;
        //private UnderlinedTextField _txtSearchAircraft;
        //private Button _btnSearchAircraft;

        // Top filter
        private FlowLayoutPanel _legend;            // LEGEND gắn trên thanh filter (phải)
        private UnderlinedComboBox _cbAircraft;
        private UnderlinedComboBox _cbCabin;
        private UnderlinedTextField _txtFindSeat;
        private Button _btnLoadTemplate;

        // Center: map
        private SeatMapPanel _map;
        private Panel _scrollHost;                  // bọc map để AutoScroll

        // Right: inspector & bulk
        private Panel _inspectorCard;
        private Label _lblSeatTitle, _lblSeatSub;
        private UnderlinedComboBox _cbAssignClass, _cbAssignPos;
        private UnderlinedTextField _txtSeatNote;
        private Button _btnToggleBlock, _btnApplySeat;

        private GroupBox _bulkBox;
        private UnderlinedTextField _txtRowFrom, _txtRowTo, _txtPattern;
        private UnderlinedComboBox _cbBulkCabin, _cbBulkClass;
        private Button _btnBulkAdd, _btnBulkClear;

        // Header actions
        private FlowLayoutPanel _topActions;
        private Button _btnSaveTemplate, _btnUndo;

        // state tạm cho template
        private readonly HashSet<SeatMapPanel.SeatKey> _existing = new();
        private readonly HashSet<SeatMapPanel.SeatKey> _pending = new();

        // ==== Kích thước ghế để tính viewport (chỉnh theo SeatMapPanel của bạn) ====
        private const int SeatSize = 28;   // w/h 1 seat
        private const int SeatGap = 4;    // khoảng cách seat
        private const int Pad = 12;   // padding viền panel vẽ

        public SeatTemplateControl() {
            InitializeComponent();
            SeedDemoAircraft();
            BuildLegend();
            WireEvents();
        }

        private void InitializeComponent() {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // ===== TitleBar: Label trái + Buttons phải (cùng hàng) =====
            _title = new Label {
                Text = "💺 Thiết kế sơ đồ ghế theo máy bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Margin = new Padding(0)
            };
            _btnSaveTemplate = new PrimaryButton("Lưu template") { Height = 36, Margin = new Padding(0) };
            _btnUndo = new SecondaryButton("Hoàn tác") { Height = 36, Margin = new Padding(12, 0, 0, 0) };

            var rightActions = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            rightActions.Controls.Add(_btnSaveTemplate);
            rightActions.Controls.Add(_btnUndo);

            _titleBar = new TableLayoutPanel {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(24, 20, 24, 0),
                BackColor = Color.Transparent
            };
            _titleBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f)); // label trái
            _titleBar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));      // nút phải
            _titleBar.Controls.Add(_title, 0, 0);
            _titleBar.Controls.Add(rightActions, 1, 0);

            // ===== Filter (trái) + Legend (phải) trên cùng một hàng =====
            var filterLeft = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            _cbAircraft = new UnderlinedComboBox("Máy bay", new object[] { "VN-A321", "VJ-A320", "QH-B789" }) { Width = 220, Margin = new Padding(0, 0, 24, 0), MinimumSize = new Size(0, 56) };
            _cbCabin = new UnderlinedComboBox("Cabin", new object[] { "Economy", "Premium Economy", "Business", "First" }) { Width = 220, Margin = new Padding(0, 0, 24, 0), MinimumSize = new Size(0, 56) };
            _txtFindSeat = new UnderlinedTextField("Tìm ghế (VD: 12A)", "") { Width = 200, Margin = new Padding(0, 0, 24, 0), MinimumSize = new Size(0, 56) };
            _btnLoadTemplate = new PrimaryButton("Tải template") { Height = 36, Margin = new Padding(0, 10, 0, 10) };
            filterLeft.Controls.AddRange(new Control[] { _cbAircraft, _cbCabin, _txtFindSeat, _btnLoadTemplate });

            _legend = new FlowLayoutPanel {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(12, 12, 0, 0),
                Padding = new Padding(0),
                BackColor = Color.Transparent
            };

            var filterBar = new TableLayoutPanel {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(24, 8, 24, 0),
                BackColor = Color.Transparent
            };
            filterBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f)); // filter trái
            filterBar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));      // legend phải
            filterBar.Controls.Add(filterLeft, 0, 0);
            filterBar.Controls.Add(_legend, 1, 0);

            // ===== 3 columns =====
            _columns = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(24, 12, 24, 24),
                BackColor = Color.Transparent
            };

            _columns.ColumnStyles.Clear();
            _columns.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 500));  // Left
            _columns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));  // Center
            _columns.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 420));  // Right (↑)

            // ===== Left panel =====
            var left = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Transparent
            };
            left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            var leftHeader = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, 0, 6)
            };

            _tblAircraft = new TableCustom {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                RowHeadersVisible = false
            };
            _tblAircraft.Columns.Add("code", "Code");
            _tblAircraft.Columns.Add("type", "Loại");
            _tblAircraft.Columns.Add("seats", "Số ghế");
            var actCol = new DataGridViewTextBoxColumn { Name = "action", HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells };
            _tblAircraft.Columns.Add(actCol);

            left.Controls.Add(leftHeader, 0, 0);
            left.Controls.Add(_tblAircraft, 0, 1);

            // ===== Center: chỉ còn SCROLL HOST + map =====
            var center = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            _scrollHost = new Panel {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                AutoScrollMargin = new Size(8, 8),
                Padding = new Padding(0),
                BackColor = Color.White
            };
            _map = new SeatMapPanel {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Rows = 30,
                Pattern = "ABCDEF",
                Margin = new Padding(0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left   // quan trọng: không Dock.Fill
            };
            _scrollHost.Controls.Add(_map);
            center.Controls.Add(_scrollHost);

            // ===== Right: inspector =====
            _inspectorCard = new Panel {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(12),
                AutoScroll = true,
                AutoScrollMargin = new Size(0, 12)
            };
            var right = BuildInspector();
            _inspectorCard.Controls.Add(right);

            // ===== Compose 3 columns =====
            _columns.Controls.Add(left, 0, 0);
            _columns.Controls.Add(center, 1, 0);
            _columns.Controls.Add(_inspectorCard, 2, 0);

            // ===== Root layout =====
            _root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1, BackColor = Color.Transparent };
            _root.RowStyles.Add(new RowStyle(SizeType.AutoSize));          // TitleBar
            _root.RowStyles.Add(new RowStyle(SizeType.AutoSize));          // Filter+Legend
            _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));     // Content
            _root.Controls.Add(_titleBar, 0, 0);
            _root.Controls.Add(filterBar, 0, 1);
            _root.Controls.Add(_columns, 0, 2);

            Controls.Clear();
            Controls.Add(_root);

            // Cập nhật khi đổi kích thước
            this.Resize += (_, __) => { UpdateMapViewport(); ReflowColumns(); };

            ResumeLayout(false);
        }

        private Control BuildInspector() {
            // Khởi tạo an toàn tất cả field được dùng trong Inspector
            _lblSeatTitle ??= new Label();
            _lblSeatSub ??= new Label();

            _cbAssignClass ??= new UnderlinedComboBox("Gán class",
                new object[] { "Economy", "Premium Economy", "Business", "First" });
            _cbAssignPos ??= new UnderlinedComboBox("Vị trí",
                new object[] { "Window", "Aisle", "Middle", "Extra Legroom" });
            _txtSeatNote ??= new UnderlinedTextField("Ghi chú", "");

            _btnApplySeat ??= new PrimaryButton("Áp dụng");
            _btnToggleBlock ??= new SecondaryButton("Chặn ghế này");

            _bulkBox ??= new GroupBox();
            _txtRowFrom ??= new UnderlinedTextField("Row từ", "1");
            _txtRowTo ??= new UnderlinedTextField("Row đến", "30");
            _txtPattern ??= new UnderlinedTextField("Pattern cột (VD: ABCDEF)", "ABCDEF");
            _cbBulkCabin ??= new UnderlinedComboBox("Cabin áp dụng",
                new object[] { "Economy", "Premium Economy", "Business", "First" });
            _cbBulkClass ??= new UnderlinedComboBox("Class áp dụng",
                new object[] { "Economy", "Premium Economy", "Business", "First" });
            _btnBulkAdd ??= new PrimaryButton("Thêm hàng loạt");
            _btnBulkClear ??= new SecondaryButton("Xóa pending");

            // ==== UI ====
            var wrap = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            wrap.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            wrap.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            wrap.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Header ghế
            var info = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1 };
            _lblSeatTitle.Text = "Chưa chọn ghế";
            _lblSeatTitle.AutoSize = true;
            _lblSeatTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            _lblSeatTitle.Margin = new Padding(0, 0, 0, 2);

            _lblSeatSub.Text = "Máy bay — Cabin —";
            _lblSeatSub.AutoSize = true;
            _lblSeatSub.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            _lblSeatSub.ForeColor = Color.FromArgb(90, 90, 90);
            _lblSeatSub.Margin = new Padding(0, 0, 0, 10);

            // Kích thước rộng hơn để không bị khuất
            _cbAssignClass.Width = 320; _cbAssignClass.Margin = new Padding(0, 0, 0, 6);
            _cbAssignPos.Width = 320; _cbAssignPos.Margin = new Padding(0, 0, 0, 6);
            _txtSeatNote.Width = 320; _txtSeatNote.Margin = new Padding(0, 0, 0, 6);

            _btnApplySeat.Height = 32; _btnApplySeat.Width = 100; _btnApplySeat.Margin = new Padding(0, 4, 0, 6);
            _btnToggleBlock.Height = 32; _btnToggleBlock.Width = 120; _btnToggleBlock.Margin = new Padding(8, 4, 0, 6);

            var seatBtnRow = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, WrapContents = false };
            seatBtnRow.Controls.Add(_btnApplySeat);
            seatBtnRow.Controls.Add(_btnToggleBlock);

            info.Controls.Add(_lblSeatTitle);
            info.Controls.Add(_lblSeatSub);
            info.Controls.Add(_cbAssignClass);
            info.Controls.Add(_cbAssignPos);
            info.Controls.Add(_txtSeatNote);
            info.Controls.Add(seatBtnRow);

            var divider = new Label {
                Height = 1,
                Dock = DockStyle.Top,
                AutoSize = false,
                BackColor = Color.FromArgb(230, 230, 230),
                Margin = new Padding(0, 6, 0, 8)
            };

            // Bulk
            _bulkBox.Text = "Thao tác hàng loạt";
            _bulkBox.Dock = DockStyle.Fill;
            _bulkBox.Padding = new Padding(8);

            var bulkGrid = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                Padding = new Padding(4)
            };
            bulkGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            bulkGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            // Nới rộng cho dễ nhập
            _txtRowFrom.Width = 140; _txtRowFrom.Margin = new Padding(4);
            _txtRowTo.Width = 140; _txtRowTo.Margin = new Padding(4);
            _txtPattern.Width = 280; _txtPattern.Margin = new Padding(4);
            _cbBulkCabin.Width = 280; _cbBulkCabin.Margin = new Padding(4);
            _cbBulkClass.Width = 280; _cbBulkClass.Margin = new Padding(4);

            bulkGrid.Controls.Add(_txtRowFrom, 0, 0);
            bulkGrid.Controls.Add(_txtRowTo, 1, 0);
            bulkGrid.Controls.Add(_txtPattern, 0, 1);
            bulkGrid.Controls.Add(_cbBulkCabin, 1, 1);
            bulkGrid.Controls.Add(_cbBulkClass, 0, 2);

            var bulkBtnRow = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4), WrapContents = false };
            _btnBulkAdd.Height = 32; _btnBulkAdd.Width = 140; _btnBulkAdd.Margin = new Padding(4);
            _btnBulkClear.Height = 32; _btnBulkClear.Width = 120; _btnBulkClear.Margin = new Padding(4);
            bulkBtnRow.Controls.Add(_btnBulkAdd);
            bulkBtnRow.Controls.Add(_btnBulkClear);

            _bulkBox.Controls.Clear();
            _bulkBox.Controls.Add(bulkBtnRow);
            _bulkBox.Controls.Add(bulkGrid);

            wrap.Controls.Add(info, 0, 0);
            wrap.Controls.Add(divider, 0, 1);
            wrap.Controls.Add(_bulkBox, 0, 2);
            return wrap;
        }

        private void BuildLegend() {
            _legend.SuspendLayout();
            _legend.Controls.Clear();

            void AddItem(string text, Color bg, Color border) {
                var swatch = new Panel { Width = 16, Height = 16, BackColor = bg, Margin = new Padding(0, 3, 6, 0) };
                swatch.Paint += (s, e) => { using var p = new Pen(border); e.Graphics.DrawRectangle(p, 0, 0, swatch.Width - 1, swatch.Height - 1); };
                var label = new Label { Text = text, AutoSize = true, Margin = new Padding(0, 3, 12, 0) };
                var wrap = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0, 0, 0, 0) };
                wrap.Controls.Add(swatch);
                wrap.Controls.Add(label);
                _legend.Controls.Add(wrap);
            }

            AddItem("Trống", Color.FromArgb(245, 247, 250), Color.FromArgb(200, 205, 210));
            AddItem("Đã có", Color.FromArgb(220, 235, 255), Color.FromArgb(0, 92, 175));
            AddItem("Pending", Color.FromArgb(219, 242, 228), Color.FromArgb(1, 135, 89));
            AddItem("Blocked", Color.FromArgb(255, 235, 238), Color.FromArgb(214, 47, 61));

            _legend.ResumeLayout();
        }

        private void WireEvents() {
            // chọn aircraft từ bảng
            _tblAircraft.CellMouseClick += (s, e) => {
                if (e.RowIndex < 0) return;
                var code = _tblAircraft.Rows[e.RowIndex].Cells["code"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(code)) {
                    _cbAircraft.SelectedText = code;
                    LoadTemplateForAircraft(code);
                }
            };

            // Tải template theo combobox
            _btnLoadTemplate.Click += (_, __) => {
                var code = _cbAircraft.SelectedText;
                if (string.IsNullOrWhiteSpace(code)) { MessageBox.Show("Chọn máy bay trước.", "Thông báo"); return; }
                LoadTemplateForAircraft(code);
            };

            // Click trên SeatMap → đẩy info qua Inspector
            _map.MouseClick += (s, e) => {
                var hit = _map.HitTest(e.Location);
                if (hit == null) return;
                var (row, col) = hit.Value;
                var key = new SeatMapPanel.SeatKey(row, col);
                _lblSeatTitle.Text = $"Ghế {row}{col}";
                _lblSeatSub.Text = $"{_cbAircraft.SelectedText} • Cabin {_cbCabin.SelectedText}";
                _btnToggleBlock.Text = _map.Existing.Contains(key) ? "Chặn ghế này" : "Chặn ghế này";
            };

            // Áp dụng cho 1 ghế (demo: commit pending → existing)
            _btnApplySeat.Click += (_, __) => {
                var title = _lblSeatTitle.Text;
                if (!title.StartsWith("Ghế ")) { MessageBox.Show("Chưa chọn ghế.", "Thông báo"); return; }
                var code = title.Replace("Ghế ", "");
                var col = code[^1].ToString();
                if (!int.TryParse(code.Substring(0, code.Length - 1), out var row)) return;
                var key = new SeatMapPanel.SeatKey(row, col);
                if (_pending.Contains(key)) { _pending.Remove(key); _existing.Add(key); _map.SetExisting(_existing); _map.SetPending(_pending); }
                MessageBox.Show($"Đã áp dụng cập nhật cho ghế {code}.", "Thông báo");
            };

            _btnBulkAdd.Click += (_, __) => {
                if (!TryParseRows(out int from, out int to)) return;
                var pattern = SanitizePattern(_txtPattern.Text);
                int added = 0, dup = 0;
                for (int r = from; r <= to; r++) {
                    foreach (var ch in pattern) {
                        var key = new SeatMapPanel.SeatKey(r, ch.ToString());
                        if (_existing.Contains(key)) { dup++; continue; }
                        _pending.Add(key); added++;
                    }
                }
                _map.SetPending(_pending);
                MessageBox.Show($"Pending: +{added}, bỏ qua trùng: {dup}.", "Bulk");
            };

            _btnBulkClear.Click += (_, __) => { _pending.Clear(); _map.SetPending(_pending); };

            _btnSaveTemplate.Click += (_, __) => {
                foreach (var k in _pending) _existing.Add(k);
                _pending.Clear();
                _map.SetExisting(_existing);
                _map.SetPending(_pending);
                MessageBox.Show("Đã lưu template máy bay.", "Thông báo");
            };

            _btnUndo.Click += (_, __) => {
                _map.SetExisting(_existing);
                _map.SetPending(_pending);
            };

            // Khi map đổi kích thước (do đổi Rows/Pattern) → reflow cột
            _map.SizeChanged += (_, __) => ReflowColumns();
        }

        // ====== Helpers ======
        private bool TryParseRows(out int from, out int to) {
            from = 0; to = 0;
            return int.TryParse(_txtRowFrom.Text, out from) && int.TryParse(_txtRowTo.Text, out to) && from > 0 && to >= from;
        }

        private static string SanitizePattern(string? raw) {
            if (string.IsNullOrWhiteSpace(raw)) return "ABCDEF";
            var s = new string(raw.ToUpper().Where(char.IsLetter).ToArray());
            return s.Length == 0 ? "ABCDEF" : s;
        }

        private void LoadTemplateForAircraft(string code) {
            _existing.Clear();
            _pending.Clear();

            // demo: 5 hàng đầu đủ ABCDEF
            foreach (var row in Enumerable.Range(1, 5))
                foreach (var ch in "ABCDEF")
                    _existing.Add(new SeatMapPanel.SeatKey(row, ch.ToString()));

            _map.Rows = 30;
            _map.Pattern = "ABCDEF";
            _map.SetExisting(_existing);
            _map.SetPending(_pending);

            UpdateMapViewport();   // tính kích thước "vừa đủ" + scroll khi tràn
            ReflowColumns();       // phân bổ lại bề rộng cột
        }

        private void SeedDemoAircraft() {
            _tblAircraft.Rows.Add("VN-A321", "A321", 180, "Chọn");
            _tblAircraft.Rows.Add("VJ-A320", "A320", 174, "Chọn");
            _tblAircraft.Rows.Add("QH-B789", "B787-9", 294, "Chọn");
        }

        private void UpdateMapViewport() {
            if (_map == null || _scrollHost == null) return;

            int rows = Math.Max(1, _map.Rows);
            int cols = Math.Max(1, (_map.Pattern ?? "").Length);

            // Kích thước "vừa đủ" để không thừa trắng
            int w = cols * SeatSize + (cols - 1) * SeatGap + Pad * 2;
            int h = rows * SeatSize + (rows - 1) * SeatGap + Pad * 2;

            _map.Size = new Size(w, h);

            // Căn giữa khi viewport rộng hơn content, ngược lại neo trái để có scroll
            int clientW = _scrollHost.ClientSize.Width;
            int x = clientW > w ? (clientW - w) / 2 : 0;

            _map.Location = new Point(x, 0);
            _scrollHost.AutoScrollMinSize = _map.Size;

            // Sau khi map thay đổi, reflow lại phân bổ cột để bớt trắng ở center
            ReflowColumns();
        }

        // Phân bổ lại bề rộng cột: ưu tiên bảng trái khi center dư trắng; ngược lại nhường center cho map
        private void ReflowColumns() {
            if (_columns == null || _map == null) return;

            const int LEFT_MIN = 460;      // giảm nhẹ min cho Left để nhường Right
            const int LEFT_MAX = 460;      // vẫn cho phép nới khi cần
            const int RIGHT_FIX = 460;     // ↑ khớp cột phải mới

            // width hiện tại của center (không tính padding cột)
            int leftWidth = (int)_columns.ColumnStyles[0].Width;
            int rightWidth = (int)_columns.ColumnStyles[2].Width; // giữ cố định
            int totalWidth = _columns.ClientSize.Width;
            int centerSpace = Math.Max(0, totalWidth - leftWidth - rightWidth);

            // bề rộng map cần hiển thị (thêm 24px đệm)
            int neededMap = _map.Width + 24;

            // Nếu center dư nhiều so với map → tăng left (ưu tiên bảng)
            if (centerSpace - neededMap > 200 && leftWidth < LEFT_MAX) {
                int give = Math.Min(centerSpace - neededMap - 100, LEFT_MAX - leftWidth);
                _columns.ColumnStyles[0].Width = leftWidth + give;
            }
            // Nếu center thiếu chỗ cho map → giảm left về min
            else if (centerSpace < neededMap && leftWidth > LEFT_MIN) {
                int take = Math.Min(leftWidth - LEFT_MIN, neededMap - centerSpace);
                _columns.ColumnStyles[0].Width = leftWidth - take;
            }

            _columns.ColumnStyles[2].Width = RIGHT_FIX; // đảm bảo cột phải cố định
            _columns.PerformLayout();
        }
    }
}
