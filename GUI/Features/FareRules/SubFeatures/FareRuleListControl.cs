using BUS.Fare_Rule;
using BUS.Route;
using BUS.CabinClass;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Features.FareRules;
using DTO.Fare_Rule;

namespace GUI.Features.FareRules.SubFeatures
{
    public class FareRuleListControl : UserControl
    {
        // ====== UI ======
        private TableLayoutPanel root;
        private Label lblTitle;
        private UnderlinedTextField txtCode;
        private UnderlinedComboBox cbFareType, cbRoute, cbCabin, cbSeason;
        private PrimaryButton btnSearch;
        private SecondaryButton btnClear;
        private TableCustom table;
        private readonly BindingSource _bs = new BindingSource();

        private FareRuleBUS bus = new FareRuleBUS();

        // ====== Data model ======
        public class FareRuleRow
        {
            public int RuleId { get; set; }
            public string RouteName { get; set; }
            public string CabinClass { get; set; }
            public string FareType { get; set; }
            public string Season { get; set; }
            public DateTime EffectiveDate { get; set; }
            public DateTime ExpiryDate { get; set; }
            public decimal Price { get; set; }
        }

        private List<FareRuleRow> _allRows = new();

        public FareRuleListControl()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadFareRules();
            FareRuleCreateControl.OnFareRuleAdded += LoadFareRules;
        }

        private void InitializeComponent()
        {
            // ===== Table Styles =====
            var cHeader = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(126, 185, 232),
                Padding = new Padding(12, 10, 12, 10)
            };
            var cAlt = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
            var cCell = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(33, 37, 41),
                Padding = new Padding(12, 6, 12, 6),
                SelectionBackColor = Color.FromArgb(155, 209, 243),
                SelectionForeColor = Color.White
            };

            // ===== Root Layout =====
            root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(232, 240, 252),
                ColumnCount = 1,
                RowCount = 3
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Title
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Filter
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Table

            // ===== Title =====
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0) };
            lblTitle = new Label
            {
                Text = "✈️ Danh sách Quy tắc vé",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold)
            };
            titlePanel.Controls.Add(lblTitle);
            root.Controls.Add(titlePanel, 0, 0);

            // ===== Filter Section =====
            var filterShell = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Padding = new Padding(24, 12, 24, 4),
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2
            };

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = false,             
                Height = 220,               
                ColumnCount = 2,
                RowCount = 3,
                Margin = new Padding(0, 0, 0, 8)
            };

            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));

            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));


            txtCode = new UnderlinedTextField("Mã quy tắc", "") { Width = 300, Margin = new Padding(0, 2, 24, 2) };
            cbCabin = new UnderlinedComboBox("Hạng vé", Array.Empty<object>()) { Width = 300, Margin = new Padding(0, 2, 24, 2) };
            cbFareType = new UnderlinedComboBox("Loại vé", new object[] { "Tất cả", "Standard", "Flex", "Saver", "Promo" }) { Width = 300, Margin = new Padding(0, 2, 24, 2) };
            cbRoute = new UnderlinedComboBox("Tuyến bay", Array.Empty<object>()) { Width = 300, Margin = new Padding(12, 2, 0, 2) };
            cbSeason = new UnderlinedComboBox("Mùa", new object[] { "Tất cả", "PEAK", "NORMAL", "OFFPEAK" }) { Width = 300, Margin = new Padding(12, 2, 0, 2) };


            grid.Controls.Add(txtCode, 0, 0);
            grid.Controls.Add(cbRoute, 1, 0);
            grid.Controls.Add(cbCabin, 0, 1);
            grid.Controls.Add(cbSeason, 1, 1);
            grid.Controls.Add(cbFareType, 0, 2);

            // Buttons Row
            var buttonRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(24, 0, 24, 0)
            };

            btnSearch = new PrimaryButton("🔎 Tìm kiếm") { Width = 120, Height = 40, Margin = new Padding(12, 8, 0, 8) };
            btnClear = new SecondaryButton("↺ Xóa lọc") { Width = 110, Height = 40, Margin = new Padding(12, 8, 0, 8) };

            btnSearch.Click += (_, __) => ApplyFilter();
            btnClear.Click += (_, __) =>
            {
                txtCode.Text = "";
                cbRoute.SelectedIndex = cbCabin.SelectedIndex = cbFareType.SelectedIndex = cbSeason.SelectedIndex = 0;
                ApplyFilter();
            };

            buttonRow.Controls.Add(btnSearch);
            buttonRow.Controls.Add(btnClear);
            filterShell.Controls.Add(grid, 0, 0);
            filterShell.Controls.Add(buttonRow, 0, 1);
            root.Controls.Add(filterShell, 0, 1);

            // ===== Table =====
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 8, 24, 24),
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            table.ColumnHeadersDefaultCellStyle = cHeader;
            table.AlternatingRowsDefaultCellStyle = cAlt;
            table.DefaultCellStyle = cCell;

            table.Columns.AddRange(new DataGridViewColumn[] {
                new DataGridViewTextBoxColumn { Name="ruleId", HeaderText="Mã", DataPropertyName=nameof(FareRuleRow.RuleId), MinimumWidth=70  },
                new DataGridViewTextBoxColumn { Name="routeName", HeaderText="Tuyến bay", DataPropertyName=nameof(FareRuleRow.RouteName), MinimumWidth=150 },
                new DataGridViewTextBoxColumn { Name="cabinClass", HeaderText="Hạng vé", DataPropertyName=nameof(FareRuleRow.CabinClass), MinimumWidth=120 },
                new DataGridViewTextBoxColumn { Name="fareType", HeaderText="Loại vé", DataPropertyName=nameof(FareRuleRow.FareType), MinimumWidth=110 },
                new DataGridViewTextBoxColumn { Name="season", HeaderText="Mùa", DataPropertyName=nameof(FareRuleRow.Season), MinimumWidth=90 },
                new DataGridViewTextBoxColumn { Name="effectiveDate", HeaderText="Hiệu lực", DataPropertyName=nameof(FareRuleRow.EffectiveDate), MinimumWidth=120, DefaultCellStyle = new DataGridViewCellStyle{ Format = "dd/MM/yyyy" } },
                new DataGridViewTextBoxColumn { Name="expiryDate", HeaderText="Hết hạn", DataPropertyName=nameof(FareRuleRow.ExpiryDate), MinimumWidth=120, DefaultCellStyle = new DataGridViewCellStyle{ Format = "dd/MM/yyyy" } },
                new DataGridViewTextBoxColumn { Name="price", HeaderText="Giá (₫)", DataPropertyName=nameof(FareRuleRow.Price), MinimumWidth=110, DefaultCellStyle = new DataGridViewCellStyle{ Format = "N0" } },
                new DataGridViewButtonColumn { Name="action", HeaderText="Xem Chi Tiết", Text="Xem", UseColumnTextForButtonValue = true, MinimumWidth=80, AutoSizeMode = DataGridViewAutoSizeColumnMode.None, Width = 80 }
            });

            table.CellContentClick += Table_CellContentClick;

            table.DataSource = _bs;
            root.Controls.Add(table, 0, 2);
            Controls.Add(root);
        }

        private void Table_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var col = table.Columns[e.ColumnIndex];
            if (col == null) return;

            if (col.Name == "action")
            {
                var item = table.Rows[e.RowIndex].DataBoundItem as FareRuleRow;
                if (item == null) return;
                ShowDetail(item.RuleId);
            }
        }

        private void ShowDetail(int ruleId)
        {
            try
            {
                FareRuleDTO dto = bus.GetById(ruleId);
                if (dto == null)
                {
                    MessageBox.Show($"Không tìm thấy quy tắc vé #{ruleId}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var parent = FindParentFareRulesControl();
                if (parent != null)
                {
                    parent.ShowDetail(dto);
                    return;
                }

                var det = new FareRuleDetailControl();
                det.LoadRule(dto.RuleId, dto.RouteName, dto.CabinClass, dto.FareType, dto.Season, dto.EffectiveDate, dto.ExpiryDate, dto.Price, dto.Description);
                using var frm = new Form
                {
                    Text = $"Chi tiết Quy tắc #{dto.RuleId}",
                    StartPosition = FormStartPosition.CenterParent,
                    ClientSize = det.Size
                };
                det.Dock = DockStyle.Fill;
                frm.Controls.Add(det);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở chi tiết:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private GUI.Features.FareRules.FareRulesControl? FindParentFareRulesControl()
        {
            Control? p = Parent;
            while (p != null)
            {
                if (p is GUI.Features.FareRules.FareRulesControl frc) return frc;
                p = p.Parent;
            }
            return null;
        }

        private void LoadComboBoxes()
        {
            try
            {
                // Tuyến bay
                var routeBus = new RouteBUS();
                var routeDict = routeBus.GetRouteDisplayList();

                cbRoute.Items.Clear();
                cbRoute.Items.Add("Tất cả");
                foreach (var kv in routeDict)
                    cbRoute.Items.Add(kv.Value);
                cbRoute.SelectedIndex = 0;

                // Hạng vé
                var cabinBus = new CabinClassBUS();
                var cabins = cabinBus.GetAllCabinClasses();

                cbCabin.Items.Clear();
                cbCabin.Items.Add("Tất cả");
                foreach (var c in cabins)
                    cbCabin.Items.Add(c.ClassName);
                cbCabin.SelectedIndex = 0;

                cbFareType.SelectedIndex = 0;
                cbSeason.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải combobox:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadFareRules()
        {
            var dtos = bus.GetAll();
            var rows = dtos.Select(x => new FareRuleRow
            {
                RuleId = x.RuleId,
                RouteName = x.RouteName,
                CabinClass = x.CabinClass,
                FareType = x.FareType,
                Season = x.Season,
                EffectiveDate = x.EffectiveDate,
                ExpiryDate = x.ExpiryDate,
                Price = x.Price
            }).ToList();

            _allRows = rows;
            _bs.DataSource = new BindingList<FareRuleRow>(_allRows);
            table.DataSource = _bs;
        }

        private void ApplyFilter()
        {
            IEnumerable<FareRuleRow> q = _allRows;

            var code = (txtCode.Text ?? "").Trim();
            var route = cbRoute.SelectedItem?.ToString();
            var cabin = cbCabin.SelectedItem?.ToString();
            var fareType = cbFareType.SelectedItem?.ToString();
            var season = cbSeason.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(code))
                q = q.Where(x => x.RuleId.ToString().Contains(code, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(route) && route != "Tất cả")
                q = q.Where(x => x.RouteName.Contains(route));
            if (!string.IsNullOrEmpty(cabin) && cabin != "Tất cả")
                q = q.Where(x => x.CabinClass == cabin);
            if (!string.IsNullOrEmpty(fareType) && fareType != "Tất cả")
                q = q.Where(x => x.FareType == fareType);
            if (!string.IsNullOrEmpty(season) && season != "Tất cả")
                q = q.Where(x => x.Season == season);

            _bs.DataSource = new BindingList<FareRuleRow>(q.ToList());
        }
    }
}
