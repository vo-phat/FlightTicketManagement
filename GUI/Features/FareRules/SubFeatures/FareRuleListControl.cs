using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace FlightTicketManagement.GUI.Features.FareRules.SubFeatures {
    public class FareRuleListControl : UserControl {
        // ====== UI ======
        private TableLayoutPanel root;
        private Label lblTitle;
        private UnderlinedTextField txtCode;
        private UnderlinedComboBox cbFareType, cbRoute, cbCabin, cbSeason;
        private PrimaryButton btnSearch;
        private SecondaryButton btnClear;
        private TableCustom table;

        private readonly BindingSource _bs = new BindingSource();

        // ====== Consts ======
        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        // ====== Data Model (khớp DB) ======
        public class FareRuleRow {
            public int RuleId { get; set; }              // Fare_Rules.rule_id
            public string RouteName { get; set; }        // SGN → HAN
            public string CabinClass { get; set; }       // Cabin_Classes.class_name
            public string FareType { get; set; }         // Fare_Rules.fare_type
            public string Season { get; set; }           // PEAK/OFFPEAK/NORMAL
            public DateTime EffectiveDate { get; set; }  // Fare_Rules.effective_date
            public DateTime ExpiryDate { get; set; }     // Fare_Rules.expiry_date
            public decimal Price { get; set; }           // Fare_Rules.price
        }

        private List<FareRuleRow> _allRows = new();

        public FareRuleListControl() {
            InitializeComponent();
            LoadDemoData();
        }

        // ================== Initialize UI ==================
        private void InitializeComponent() {
            var cHeader = new DataGridViewCellStyle {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(126, 185, 232),
                Padding = new Padding(12, 10, 12, 10)
            };
            var cAlt = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            var cCell = new DataGridViewCellStyle {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(33, 37, 41),
                Padding = new Padding(12, 6, 12, 6),
                SelectionBackColor = Color.FromArgb(155, 209, 243),
                SelectionForeColor = Color.White
            };

            // ===== Root: kích thước như Create =====
            root = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(232, 240, 252),
                ColumnCount = 1,
                RowCount = 3
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // Title tự co
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // Filter tự co (PreferredSize)
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // Table chiếm còn lại

            // ===== Title =====
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0) };
            lblTitle = new Label {
                Text = "✈️ Danh sách Quy tắc vé",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold)
            };
            titlePanel.Controls.Add(lblTitle);
            root.Controls.Add(titlePanel, 0, 0);

            // ===== Filter (2 cột x 3 hàng) – AutoSize giống Create =====
            var filterShell = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(232, 240, 252),
                Padding = new Padding(24, 12, 24, 4),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                RowCount = 2
            };
            filterShell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            filterShell.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // grid inputs
            filterShell.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // row buttons

            // Grid inputs: 2 cột x 3 hàng
            var grid = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 3,
                Margin = new Padding(0, 0, 0, 8)
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            for (int i = 0; i < 3; i++) grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // tạm, sẽ set lại theo PreferredSize

            // Controls (đồng bộ kích thước với Create)
            txtCode = new UnderlinedTextField("Mã quy tắc", "") { 
                MinimumSize = new Size(0, 64),
                Width = 300,
                Margin = new Padding(0, 6, 24, 6)
            };
            cbCabin = new UnderlinedComboBox("Hạng vé", new object[] { "PHỔ THÔNG", "THƯƠNG GIA" }) { 
                MinimumSize = new Size(0, 64),
                Width = 300,
                Margin = new Padding(0, 6, 24, 6)
            };
            cbFareType = new UnderlinedComboBox("Loại vé", new object[] { "TIÊU CHUẨN", "KHUYẾN MÃI", "TIẾT KIỆM" }) { 
                MinimumSize = new Size(0, 64),
                Width = 300,
                Margin = new Padding(0, 6, 24, 6) 
            };

            cbRoute = new UnderlinedComboBox("Tuyến bay", new object[] { "Tất cả", "SGN → HAN", "HAN → DAD", "SGN → PQC" }) { 
                MinimumSize = new Size(0, 64),
                Width = 300,
                Margin = new Padding(12, 6, 0, 6) 
            };
            cbSeason = new UnderlinedComboBox("Mùa", new object[] { "Tất cả", "CAO ĐIỂM", "TRUNG ĐIỂM", "THẤP ĐIỂM" }) { 
                MinimumSize = new Size(0, 64),
                Width = 300,
                Margin = new Padding(12, 6, 0, 6) 
            };

            grid.Controls.Add(txtCode, 0, 0);
            grid.Controls.Add(cbCabin, 0, 1);
            grid.Controls.Add(cbFareType, 0, 2);

            grid.Controls.Add(cbRoute, 1, 0);
            grid.Controls.Add(cbSeason, 1, 1);
            // (1,2) để trống cho thoáng

            // ✅ Set chiều cao từng hàng theo PreferredSize (giống Create)
            for (int r = 0; r < grid.RowCount; r++) {
                int h = 0;
                for (int c = 0; c < grid.ColumnCount; c++) {
                    var ctl = grid.GetControlFromPosition(c, r);
                    if (ctl != null) h = Math.Max(h, ctl.GetPreferredSize(Size.Empty).Height + ctl.Margin.Vertical);
                }
                grid.RowStyles[r] = new RowStyle(SizeType.Absolute, Math.Max(72, h + 2)); // >=72 để không cắt underline
            }

            // Hàng nút (góc phải dưới)
            var buttonRow = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(24, 0, 24, 0),
                WrapContents = false
            };
            btnSearch = new PrimaryButton("🔎 Tìm kiếm") { Width = 120, Height = 40, Margin = new Padding(12, 8, 0, 8) };
            btnClear = new SecondaryButton("↺ Xoá lọc") { Width = 110, Height = 40, Margin = new Padding(12, 8, 0, 8) };
            btnSearch.Click += (_, __) => ApplyFilter();
            btnClear.Click += (_, __) => { txtCode.Text = cbRoute.Text = cbFareType.Text = string.Empty; cbCabin.SelectedIndex = -1; cbSeason.SelectedIndex = -1; ApplyFilter(); };
            buttonRow.Controls.Add(btnSearch);
            buttonRow.Controls.Add(btnClear);

            filterShell.Controls.Add(grid, 0, 0);
            filterShell.Controls.Add(buttonRow, 0, 1);
            root.Controls.Add(filterShell, 0, 1);

            // ===== Table =====
            table = new TableCustom {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 8, 24, 24), // sát filter, không tạo khoảng trống
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            table.EnableHeadersVisualStyles = false;
            table.ColumnHeadersDefaultCellStyle = cHeader;
            table.ColumnHeadersHeight = 44;
            table.AlternatingRowsDefaultCellStyle = cAlt;
            table.DefaultCellStyle = cCell;

            table.Columns.AddRange(new DataGridViewColumn[] {
        new DataGridViewTextBoxColumn { Name="ruleId",       HeaderText="Mã",        DataPropertyName=nameof(FareRuleRow.RuleId),        MinimumWidth=70  },
        new DataGridViewTextBoxColumn { Name="routeName",    HeaderText="Tuyến bay", DataPropertyName=nameof(FareRuleRow.RouteName),     MinimumWidth=150 },
        new DataGridViewTextBoxColumn { Name="cabinClass",   HeaderText="Hạng vé",   DataPropertyName=nameof(FareRuleRow.CabinClass),    MinimumWidth=120 },
        new DataGridViewTextBoxColumn { Name="fareType",     HeaderText="Loại vé",   DataPropertyName=nameof(FareRuleRow.FareType),      MinimumWidth=110 },
        new DataGridViewTextBoxColumn { Name="season",       HeaderText="Mùa",       DataPropertyName=nameof(FareRuleRow.Season),        MinimumWidth=90  },
        new DataGridViewTextBoxColumn { Name="effectiveDate",HeaderText="Hiệu lực",  DataPropertyName=nameof(FareRuleRow.EffectiveDate), MinimumWidth=120, DefaultCellStyle = new DataGridViewCellStyle{ Format = "dd/MM/yyyy" } },
        new DataGridViewTextBoxColumn { Name="expiryDate",   HeaderText="Hết hạn",   DataPropertyName=nameof(FareRuleRow.ExpiryDate),    MinimumWidth=120, DefaultCellStyle = new DataGridViewCellStyle{ Format = "dd/MM/yyyy" } },
        new DataGridViewTextBoxColumn { Name="price",        HeaderText="Giá (₫)",   DataPropertyName=nameof(FareRuleRow.Price),         MinimumWidth=110, DefaultCellStyle = new DataGridViewCellStyle{ Format = "N0" } },
        new DataGridViewTextBoxColumn { Name=ACTION_COL,     HeaderText="Thao tác",  MinimumWidth=160, ReadOnly=true },
    });
            table.DataSource = _bs;

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            try {
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                    null, table, new object[] { true });
            } catch { }

            root.Controls.Add(table, 0, 2);
            Controls.Add(root);
        }

        // ================== Data Binding ==================
        public void LoadDemoData() {
            BindData(new[] {
                new FareRuleRow{ RuleId=3, RouteName="SGN → HAN", CabinClass="THƯƠNG GIA",          FareType="KHUYẾN MÃI", Season="TRUNG ĐIỂM",  EffectiveDate=DateTime.Today,           ExpiryDate=DateTime.Today.AddMonths(2), Price=1200000 },
                new FareRuleRow{ RuleId=2, RouteName="HAN → DAD", CabinClass="PHỔ THÔNG",         FareType="TIÊU CHUẨN",  Season="THẤP ĐIỂM",    EffectiveDate=DateTime.Today.AddDays(-7), ExpiryDate=DateTime.Today.AddMonths(1), Price=3500000 },
                new FareRuleRow{ RuleId=1, RouteName="SGN → PQC", CabinClass="PHỔ THÔNG",  FareType="TIẾT KIỆM", Season="CAO ĐIỂM", EffectiveDate=DateTime.Today.AddDays(-30),ExpiryDate=DateTime.Today.AddMonths(3), Price=1500000 },
            });
        }

        public void BindData(IEnumerable<FareRuleRow> rows) {
            _allRows = rows?.ToList() ?? new List<FareRuleRow>();
            _bs.DataSource = new BindingList<FareRuleRow>(_allRows);
            table.Invalidate();
        }

        private void ApplyFilter() {
            IEnumerable<FareRuleRow> q = _allRows;

            var code = (txtCode.Text ?? "").Trim();
            var route = (cbRoute.Text ?? "").Trim();
            var cabin = cbCabin.SelectedItem?.ToString();
            var fareType = (cbFareType.Text ?? "").Trim();
            var seasonSel = cbSeason.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(code))
                q = q.Where(x => x.RuleId.ToString().Contains(code, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(route))
                q = q.Where(x => (x.RouteName ?? "").IndexOf(route, StringComparison.OrdinalIgnoreCase) >= 0);
            if (!string.IsNullOrEmpty(cabin))
                q = q.Where(x => string.Equals(x.CabinClass, cabin, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(fareType))
                q = q.Where(x => (x.FareType ?? "").IndexOf(fareType, StringComparison.OrdinalIgnoreCase) >= 0);
            if (!string.IsNullOrEmpty(seasonSel) && seasonSel != "Tất cả")
                q = q.Where(x => string.Equals(x.Season, seasonSel, StringComparison.OrdinalIgnoreCase));

            _bs.DataSource = new BindingList<FareRuleRow>(q.ToList());
            table.Invalidate();
        }

        // ================== Action Column ==================
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDel) GetRects(Rectangle cellBounds, Font font) {
            int pad = 6; int x = cellBounds.Left + pad; int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;
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

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
            var font = e.CellStyle.Font ?? table.Font;
            var r = GetRects(e.CellBounds, font);
            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, r.rcView.Location, Color.FromArgb(0, 92, 175));
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcView.Right, r.rcView.Top), Color.Gray);
            TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, r.rcEdit.Location, Color.FromArgb(0, 92, 175));
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcEdit.Right, r.rcEdit.Top), Color.Gray);
            TextRenderer.DrawText(e.Graphics, TXT_DEL, font, r.rcDel.Location, Color.FromArgb(220, 53, 69));
        }

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcEdit.Contains(p) || r.rcDel.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            int ruleId = Convert.ToInt32(row.Cells["ruleId"].Value);

            if (r.rcView.Contains(p)) {
                if (r.rcView.Contains(p)) {
                    if (table.Rows[e.RowIndex].DataBoundItem is FareRuleRow data) { // 👈 đổi row → data
                        var detail = new FareRuleDetailControl();
                        detail.LoadRule(
                            ruleId: data.RuleId,
                            route: data.RouteName,
                            cabin: data.CabinClass,
                            fareType: data.FareType,
                            season: data.Season,
                            eff: data.EffectiveDate,
                            exp: data.ExpiryDate,
                            price: data.Price,
                            desc: "(Mô tả đang cập nhật)"
                        );

                        var frm = new Form {
                            Text = $"Chi tiết quy tắc #{data.RuleId}",
                            StartPosition = FormStartPosition.CenterParent,
                            Size = new Size(1010, 640),
                            MinimumSize = new Size(780, 460),
                            BackColor = Color.White
                        };
                        frm.Controls.Add(detail);
                        frm.ShowDialog(FindForm());
                    }
                }
            } else if (r.rcEdit.Contains(p)) {
                MessageBox.Show($"Chỉnh sửa Rule #{ruleId}", "Sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcDel.Contains(p)) {
                MessageBox.Show($"Xóa Rule #{ruleId} (chưa tích hợp xóa thật).", "Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}
