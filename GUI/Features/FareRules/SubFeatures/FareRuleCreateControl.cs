using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using BUS.Fare_Rule;
using BUS.Route;
using BUS.CabinClass;
using DTO.Fare_Rule;
using DTO.Route;
using DTO.CabinClass;
using System.Collections.Generic;

namespace GUI.Features.FareRules.SubFeatures
{
    public class FareRuleCreateControl : UserControl
    {
        // --- Controls ---
        private TableCustom table;
        private UnderlinedComboBox cbRoute, cbCabin, cbFareType, cbSeason;
        private UnderlinedTextField txtPrice, txtDescription;
        private UnderlinedDatePicker dtEffective, dtExpiry;
        private PrimaryButton btnAdd;
        private SecondaryButton btnEdit, btnDelete, btnClear;

        // --- Data ---
        private List<RouteDTO> routes;
        private List<CabinClassDTO> cabins;
        private int? selectedRuleId = null;
        public static event Action OnFareRuleAdded;

        public FareRuleCreateControl()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadFareRuleTable();
        }

        // ==========================================
        // 1. INNER CLASSES (HELPER)
        // ==========================================
        private class RouteItem
        {
            public int Id { get; }
            public string Text { get; }
            public RouteItem(int id, string text) { Id = id; Text = text; }
            public override string ToString() => Text;
        }

        // Cập nhật DatePicker để đẹp hơn trên nền xám
        private class UnderlinedDatePicker : UserControl
        {
            private Label lbl;
            private DateTimePicker picker;
            private Panel underline;

            public DateTime Value
            {
                get => picker.Value;
                set => picker.Value = value;
            }

            public UnderlinedDatePicker(string label)
            {
                Height = 60; 
                BackColor = Color.FromArgb(240, 242, 245); 
                lbl = new Label
                {
                    Text = label,
                    Dock = DockStyle.Top,
                    Height = 25,
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular), 
                    ForeColor = Color.Gray,
                    Padding = new Padding(0, 5, 0, 0)
                };

                picker = new DateTimePicker
                {
                    Dock = DockStyle.Top,
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy",
                    Font = new Font("Segoe UI", 11F),
                    Height = 30,
                    BackColor = Color.FromArgb(240, 242, 245) 
                };

                underline = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 2,
                    BackColor = Color.FromArgb(40, 40, 40) 
                };

                this.Controls.Add(underline);
                this.Controls.Add(picker);
                this.Controls.Add(lbl);

                // Hiệu ứng focus
                picker.Enter += (s, e) => underline.BackColor = Color.FromArgb(0, 92, 175);
                picker.Leave += (s, e) => underline.BackColor = Color.FromArgb(40, 40, 40);
            }
        }

        // ==========================================
        // 2. GIAO DIỆN (INITIALIZE)
        // ==========================================
        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // --- A. CONTAINER NHẬP LIỆU (TOP) ---
            var pnlTopContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 260, 
                BackColor = Color.FromArgb(240, 242, 245), 
                Padding = new Padding(20)
            };

            // --- B. GRID INPUTS (4 CỘT) ---
            var tlpInputs = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 150,
                ColumnCount = 4,
                RowCount = 2,
                Margin = new Padding(0)
            };
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            // --- C. KHỞI TẠO CONTROLS ---
            Color grayBg = Color.FromArgb(240, 242, 245);
            Padding inputMargin = new Padding(10, 5, 10, 5);

            // Row 1
            cbRoute = CreateCombo("Tuyến bay", grayBg, inputMargin);
            cbCabin = CreateCombo("Hạng ghế (Cabin Class)", grayBg, inputMargin);
            cbFareType = CreateCombo("Loại vé", grayBg, inputMargin);
            cbFareType.Items.AddRange(new object[] { "Standard", "Flex", "Saver", "Promo" });

            cbSeason = CreateCombo("Mùa", grayBg, inputMargin);
            cbSeason.Items.AddRange(new object[] { "PEAK", "NORMAL", "OFFPEAK" });

            // Row 2
            dtEffective = new UnderlinedDatePicker("Ngày hiệu lực") { Dock = DockStyle.Fill, Margin = inputMargin };
            dtExpiry = new UnderlinedDatePicker("Ngày hết hạn") { Dock = DockStyle.Fill, Margin = inputMargin };

            txtPrice = CreateText("Giá vé (VNĐ)", grayBg, inputMargin);
            txtPrice.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };

            txtDescription = CreateText("Mô tả / Ghi chú", grayBg, inputMargin);

            // Add vào Grid
            tlpInputs.Controls.Add(cbRoute, 0, 0);
            tlpInputs.Controls.Add(cbCabin, 1, 0);
            tlpInputs.Controls.Add(cbFareType, 2, 0);
            tlpInputs.Controls.Add(cbSeason, 3, 0);

            tlpInputs.Controls.Add(dtEffective, 0, 1);
            tlpInputs.Controls.Add(dtExpiry, 1, 1);
            tlpInputs.Controls.Add(txtPrice, 2, 1);
            tlpInputs.Controls.Add(txtDescription, 3, 1);

            // --- D. ACTION BUTTONS (FLOW LAYOUT) ---
            var flpActions = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 5, 10, 0),
                BackColor = Color.Transparent
            };

            // Init Buttons
            Size btnSize = new Size(110, 40);

            btnClear = new SecondaryButton("Làm mới") { Size = btnSize, Margin = new Padding(5) };
            btnClear.Click += (s, e) => RefreshFields();

            btnDelete = new SecondaryButton("Xóa") { Size = btnSize, Margin = new Padding(5), BackColor = Color.FromArgb(255, 235, 238), ForeColor = Color.Red, BorderColor = Color.Red };
            btnDelete.Click += BtnDelete_Click;

            btnEdit = new SecondaryButton("Sửa") { Size = btnSize, Margin = new Padding(5) };
            btnEdit.Click += BtnEdit_Click;

            btnAdd = new PrimaryButton("Thêm") { Size = btnSize, Margin = new Padding(5) };
            btnAdd.Click += BtnAdd_Click;

            flpActions.Controls.Add(btnClear);
            flpActions.Controls.Add(btnDelete);
            flpActions.Controls.Add(btnEdit);
            flpActions.Controls.Add(btnAdd);

            // Lắp ráp Top Panel
            pnlTopContainer.Controls.Add(flpActions); // Dưới
            pnlTopContainer.Controls.Add(tlpInputs);  // Trên

            // --- E. DATA GRID (FILL) ---
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderColor = Color.FromArgb(240, 240, 240),
                BorderThickness = 1,
                ShowCellToolTips = false,
                RowHeadersVisible = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Style Grid Columns
            table.Columns.Add(CreateCol("ruleId", "ID", 5));
            table.Columns.Add(CreateCol("route", "Tuyến bay", 15));
            table.Columns.Add(CreateCol("cabin", "Hạng ghế", 10));
            table.Columns.Add(CreateCol("fareType", "Loại", 10));
            table.Columns.Add(CreateCol("season", "Mùa", 8));
            table.Columns.Add(CreateCol("effective", "Hiệu lực", 10));
            table.Columns.Add(CreateCol("expiry", "Hết hạn", 10));

            var colPrice = CreateCol("price", "Giá (VNĐ)", 12);
            colPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            table.Columns.Add(colPrice);

            table.CellClick += Table_CellClick;

            // --- F. ADD TO CONTROL ---
            this.Controls.Add(table);           // Fill
            this.Controls.Add(pnlTopContainer); // Top
        }
        private UnderlinedComboBox CreateCombo(string label, Color bg, Padding margin)
        {
            return new UnderlinedComboBox
            {
                LabelText = label,
                Dock = DockStyle.Fill,
                BackColor = bg,
                Margin = margin,
                DropDownStyle = ComboBoxStyle.DropDown
            };
        }

        private UnderlinedTextField CreateText(string label, Color bg, Padding margin)
        {
            return new UnderlinedTextField
            {
                LabelText = label,
                Dock = DockStyle.Fill,
                BackColor = bg,
                Margin = margin,
                LineColor = Color.FromArgb(40, 40, 40),
                LineColorFocused = Color.FromArgb(0, 92, 175)
            };
        }

        // Thêm tham số 'align' vào cuối, mặc định là MiddleCenter (Căn giữa)
        private DataGridViewTextBoxColumn CreateCol(string name, string header, float weight,
            DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleLeft)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                FillWeight = weight,
                // Cài đặt căn lề ở đây
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = align }
            };
        }

        // ==========================================
        // 3. LOGIC XỬ LÝ (GIỮ NGUYÊN LOGIC CŨ)
        // ==========================================

        private void LoadComboBoxes()
        {
            try
            {
                var routeBus = new RouteBUS();
                var routeDict = routeBus.GetRouteDisplayList();

                cbRoute.Items.Clear();
                foreach (var kv in routeDict)
                    cbRoute.Items.Add(new RouteItem(kv.Key, kv.Value));

                var cabinBus = new CabinClassBUS();
                cabins = cabinBus.GetAllCabinClasses();
                cbCabin.Items.Clear();
                foreach (var c in cabins)
                    cbCabin.Items.Add(c.ClassName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void LoadFareRuleTable()
        {
            try
            {
                var bus = new FareRuleBUS();
                var list = bus.GetAll();
                table.Rows.Clear();

                foreach (var item in list)
                {
                    table.Rows.Add(item.RuleId, item.RouteName, item.CabinClass, item.FareType, item.Season,
                                   item.EffectiveDate.ToString("dd/MM/yyyy"),
                                   item.ExpiryDate.ToString("dd/MM/yyyy"),
                                   $"{item.Price:N0}");
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải bảng: " + ex.Message); }
        }

        private void Table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < table.Rows.Count)
            {
                var row = table.Rows[e.RowIndex];
                selectedRuleId = Convert.ToInt32(row.Cells["ruleId"].Value);

                string routeName = row.Cells["route"].Value?.ToString();
                string cabinName = row.Cells["cabin"].Value?.ToString();
                string fareType = row.Cells["fareType"].Value?.ToString();
                string season = row.Cells["season"].Value?.ToString();

                // Set Combo Route
                for (int i = 0; i < cbRoute.Items.Count; i++)
                {
                    if (cbRoute.Items[i].ToString().Equals(routeName, StringComparison.OrdinalIgnoreCase))
                    {
                        cbRoute.SelectedIndex = i; break;
                    }
                }
                // Set Combo Cabin
                for (int i = 0; i < cbCabin.Items.Count; i++)
                {
                    if (cbCabin.Items[i].ToString().Equals(cabinName, StringComparison.OrdinalIgnoreCase))
                    {
                        cbCabin.SelectedIndex = i; break;
                    }
                }

                cbFareType.SelectedItem = fareType;
                cbSeason.SelectedItem = season;

                if (DateTime.TryParseExact(row.Cells["effective"].Value?.ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime eff))
                    dtEffective.Value = eff;
                if (DateTime.TryParseExact(row.Cells["expiry"].Value?.ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime exp))
                    dtExpiry.Value = exp;

                var priceStr = row.Cells["price"].Value?.ToString().Replace("₫", "").Replace(",", "").Trim();
                if (decimal.TryParse(priceStr, out decimal price))
                    txtPrice.Text = price.ToString();

                var bus = new FareRuleBUS();
                var dto = bus.GetById(selectedRuleId.Value);
                txtDescription.Text = dto?.Description ?? "";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            try
            {
                var dto = BuildDTOFromForm();
                var bus = new FareRuleBUS();
                if (bus.Insert(dto))
                {
                    MessageBox.Show("Thêm thành công!");
                    ReloadData();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi thêm: " + ex.Message); }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (selectedRuleId == null) { MessageBox.Show("Chọn dòng cần sửa!"); return; }
            if (!ValidateInput()) return;
            try
            {
                var dto = BuildDTOFromForm();
                dto.RuleId = selectedRuleId.Value;
                var bus = new FareRuleBUS();
                if (bus.Update(dto))
                {
                    MessageBox.Show("Cập nhật thành công!");
                    ReloadData();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi sửa: " + ex.Message); }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedRuleId == null) { MessageBox.Show("Chọn dòng cần xóa!"); return; }
            if (MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    var bus = new FareRuleBUS();
                    if (bus.Delete(selectedRuleId.Value))
                    {
                        MessageBox.Show("Xóa thành công!");
                        ReloadData();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Lỗi xóa: " + ex.Message); }
            }
        }

        private void RefreshFields()
        {
            selectedRuleId = null;
            cbRoute.SelectedIndex = -1;
            cbCabin.SelectedIndex = -1;
            cbFareType.SelectedIndex = -1;
            cbSeason.SelectedIndex = -1;
            dtEffective.Value = DateTime.Today;
            dtExpiry.Value = DateTime.Today;
            txtPrice.Text = "";
            txtDescription.Text = "";
            table.ClearSelection();
        }

        private void ReloadData()
        {
            LoadFareRuleTable();
            OnFareRuleAdded?.Invoke();
            RefreshFields();
        }

        private bool ValidateInput()
        {
            if (cbRoute.SelectedItem == null) { MessageBox.Show("Chọn tuyến bay!"); return false; }
            if (cbCabin.SelectedIndex < 0) { MessageBox.Show("Chọn hạng ghế!"); return false; }
            if (string.IsNullOrWhiteSpace(txtPrice.Text)) { MessageBox.Show("Nhập giá vé!"); return false; }
            return true;
        }

        private FareRuleDTO BuildDTOFromForm()
        {
            var selRoute = cbRoute.SelectedItem as RouteItem;
            int routeId = selRoute.Id;
            int classId = cabins[cbCabin.SelectedIndex].ClassId;

            return new FareRuleDTO
            {
                RouteId = routeId,
                ClassId = classId,
                FareType = cbFareType.SelectedItem?.ToString() ?? "",
                Season = cbSeason.SelectedItem?.ToString() ?? "",
                EffectiveDate = dtEffective.Value,
                ExpiryDate = dtExpiry.Value,
                Price = decimal.TryParse(txtPrice.Text, out var p) ? p : 0,
                Description = txtDescription.Text
            };
        }
    }
}