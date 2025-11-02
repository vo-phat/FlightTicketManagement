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
        private TableCustom table;
        private UnderlinedComboBox cbRoute, cbCabin, cbFareType, cbSeason;
        private UnderlinedTextField txtPrice, txtDescription;
        private UnderlinedDatePicker dtEffective, dtExpiry;

        private List<RouteDTO> routes;
        private List<CabinClassDTO> cabins;

        private int? selectedRuleId = null; // lưu rule_id đang chọn
        public static event Action OnFareRuleAdded;

        public FareRuleCreateControl()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadFareRuleTable();
        }

        private class RouteItem
        {
            public int Id { get; }
            public string Text { get; }
            public RouteItem(int id, string text) { Id = id; Text = text; }
            public override string ToString() => Text;
        }

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

            public UnderlinedDatePicker(string label, DateTime? defaultDate = null)
            {
                Height = 64;
                Width = 300;
                Margin = new Padding(0, 6, 24, 6);
                BackColor = Color.Transparent;

                lbl = new Label
                {
                    Text = label,
                    Dock = DockStyle.Top,
                    Height = 20,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 40, 40)
                };

                picker = new DateTimePicker
                {
                    Dock = DockStyle.Top,
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy",
                    Font = new Font("Segoe UI", 10),
                    Value = defaultDate ?? DateTime.Today
                };

                underline = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 2,
                    BackColor = Color.FromArgb(180, 180, 180)
                };

                Controls.Add(underline);
                Controls.Add(picker);
                Controls.Add(lbl);

                picker.Enter += (s, e) => underline.BackColor = Color.FromArgb(0, 120, 215);
                picker.Leave += (s, e) => underline.BackColor = Color.FromArgb(180, 180, 180);
            }
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // ===== Title =====
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label { Text = "➕ Tạo quy tắc vé", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
            titlePanel.Controls.Add(lblTitle);

            // ===== Inputs =====
            var inputs = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Padding = new Padding(24, 12, 24, 0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 4
            };
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            cbRoute = new UnderlinedComboBox("Tuyến bay", Array.Empty<object>()) { Width = 300, Margin = new Padding(0, 6, 24, 6) };
            cbCabin = new UnderlinedComboBox("Hạng vé (Cabin Class)", Array.Empty<object>()) { Width = 300, Margin = new Padding(0, 6, 24, 6) };
            cbFareType = new UnderlinedComboBox("Loại vé", new object[] { "Standard", "Flex", "Saver", "Promo" }) { Width = 300, Margin = new Padding(0, 6, 24, 6) };
            cbSeason = new UnderlinedComboBox("Mùa", new object[] { "PEAK", "NORMAL", "OFFPEAK" }) { Width = 300, Margin = new Padding(0, 6, 24, 6) };

            dtEffective = new UnderlinedDatePicker("Ngày hiệu lực");
            dtExpiry = new UnderlinedDatePicker("Ngày hết hạn");

            txtPrice = new UnderlinedTextField("Giá", "") { Width = 300, Margin = new Padding(0, 6, 24, 6) };
            txtPrice.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };

            txtDescription = new UnderlinedTextField("Mô tả", "") { Width = 300, Margin = new Padding(0, 6, 24, 6) };

            inputs.Controls.Add(cbRoute, 0, 0);
            inputs.Controls.Add(cbCabin, 1, 0);
            inputs.Controls.Add(cbFareType, 0, 1);
            inputs.Controls.Add(cbSeason, 1, 1);
            inputs.Controls.Add(dtEffective, 0, 2);
            inputs.Controls.Add(dtExpiry, 1, 2);
            inputs.Controls.Add(txtPrice, 0, 3);
            inputs.Controls.Add(txtDescription, 1, 3);

            // ===== Buttons =====
            var btnAdd = new PrimaryButton("➕ Thêm mới") { Width = 140, Height = 40, Margin = new Padding(0, 12, 12, 12) };
            var btnEdit = new SecondaryButton("✏️ Sửa") { Width = 100, Height = 40, Margin = new Padding(0, 12, 12, 12) };
            var btnDelete = new SecondaryButton("❌ Xóa") { Width = 100, Height = 40, Margin = new Padding(0, 12, 12, 12), BackColor = Color.Red, ForeColor = Color.White }; // Nút Xóa

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click; // Gắn sự kiện Click cho nút Xóa

            var buttonRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(24, 0, 24, 0)
            };
            buttonRow.Controls.Add(btnAdd);
            buttonRow.Controls.Add(btnEdit);
            buttonRow.Controls.Add(btnDelete); // Thêm nút Xóa vào panel

            // ===== Table =====
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 4),
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            table.Columns.Add("ruleId", "Mã Quy tắc");
            table.Columns.Add("route", "Tuyến");
            table.Columns.Add("cabin", "Hạng vé");
            table.Columns.Add("fareType", "Loại vé");
            table.Columns.Add("season", "Mùa");
            table.Columns.Add("effective", "Hiệu lực");
            table.Columns.Add("expiry", "Hết hạn");
            table.Columns.Add("price", "Giá (₫)");

            table.CellClick += Table_CellClick;

            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4 };
            main.Controls.Add(titlePanel, 0, 0);
            main.Controls.Add(inputs, 0, 1);
            main.Controls.Add(buttonRow, 0, 2);
            main.Controls.Add(table, 0, 3);
            Controls.Add(main);
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

                // --- Route ---
                for (int i = 0; i < cbRoute.Items.Count; i++)
                {
                    if (cbRoute.Items[i].ToString().Equals(routeName, StringComparison.OrdinalIgnoreCase))
                    {
                        cbRoute.SelectedIndex = i;
                        break;
                    }
                }

                // --- Cabin ---
                for (int i = 0; i < cbCabin.Items.Count; i++)
                {
                    if (cbCabin.Items[i].ToString().Equals(cabinName, StringComparison.OrdinalIgnoreCase))
                    {
                        cbCabin.SelectedIndex = i;
                        break;
                    }
                }

                // --- FareType và Season ---
                cbFareType.SelectedItem = fareType;
                cbSeason.SelectedItem = season;

                // --- Date ---
                if (DateTime.TryParseExact(row.Cells["effective"].Value?.ToString(), "dd/MM/yyyy",
                    null, System.Globalization.DateTimeStyles.None, out DateTime eff))
                    dtEffective.Value = eff;

                if (DateTime.TryParseExact(row.Cells["expiry"].Value?.ToString(), "dd/MM/yyyy",
                    null, System.Globalization.DateTimeStyles.None, out DateTime exp))
                    dtExpiry.Value = exp;

                // --- Price ---
                var priceStr = row.Cells["price"].Value?.ToString().Replace("₫", "").Replace(",", "").Trim();
                if (decimal.TryParse(priceStr, out decimal price))
                    txtPrice.Text = price.ToString();

                // --- Description ---
                var bus = new FareRuleBUS();
                var dto = bus.GetById(selectedRuleId.Value);
                txtDescription.Text = dto?.Description ?? "";
            }
        }



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
                MessageBox.Show("Lỗi khi tải combobox:\n" + ex.Message);
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu quy tắc vé:\n" + ex.Message);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(cbRoute.SelectedItem is RouteItem selRoute))
                {
                    MessageBox.Show("Vui lòng chọn tuyến bay!");
                    return;
                }
                if (cbCabin.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn hạng vé!");
                    return;
                }

                int routeId = selRoute.Id;
                int classId = cabins[cbCabin.SelectedIndex].ClassId;

                var dto = new FareRuleDTO
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

                var bus = new FareRuleBUS();
                bool ok = bus.Insert(dto);

                if (ok)
                {
                    MessageBox.Show("Thêm quy tắc vé thành công!");
                    LoadFareRuleTable();
                    OnFareRuleAdded?.Invoke();
                    RefreshFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm quy tắc vé:\n" + ex.Message);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedRuleId == null)
                {
                    MessageBox.Show("Vui lòng chọn dòng cần sửa!");
                    return;
                }

                if (!(cbRoute.SelectedItem is RouteItem selRoute))
                {
                    MessageBox.Show("Vui lòng chọn tuyến bay!");
                    return;
                }

                int routeId = selRoute.Id;
                int classId = cabins[cbCabin.SelectedIndex].ClassId;

                var dto = new FareRuleDTO
                {
                    RuleId = selectedRuleId.Value,
                    RouteId = routeId,
                    ClassId = classId,
                    FareType = cbFareType.SelectedItem?.ToString() ?? "",
                    Season = cbSeason.SelectedItem?.ToString() ?? "",
                    EffectiveDate = dtEffective.Value,
                    ExpiryDate = dtExpiry.Value,
                    Price = decimal.TryParse(txtPrice.Text, out var p) ? p : 0,
                    Description = txtDescription.Text
                };

                var bus = new FareRuleBUS();
                bool ok = bus.Update(dto);

                if (ok)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    LoadFareRuleTable();
                    selectedRuleId = null;
                    OnFareRuleAdded?.Invoke();
                    RefreshFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật:\n" + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedRuleId == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!");
                return;
            }

            // Hiển thị hộp thoại xác nhận trước khi xóa
            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa Quy tắc vé #{selectedRuleId} này không?",
                "Xác nhận Xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var bus = new FareRuleBUS();
                    bool ok = bus.Delete(selectedRuleId.Value);

                    if (ok)
                    {
                        MessageBox.Show("Xóa quy tắc vé thành công!");
                        LoadFareRuleTable();
                        selectedRuleId = null; // Xóa ID đang chọn
                        OnFareRuleAdded?.Invoke();
                        RefreshFields(); // Làm mới các trường nhập liệu
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa quy tắc vé. Có thể đã bị xóa trước đó.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa quy tắc vé:\n" + ex.Message);
                }
            }
        }
        private void RefreshFields()
        {
            selectedRuleId = null; // xóa lựa chọn dòng hiện tại

            cbRoute.SelectedIndex = -1;
            cbCabin.SelectedIndex = -1;
            cbFareType.SelectedIndex = -1;
            cbSeason.SelectedIndex = -1;

            dtEffective.Value = DateTime.Today;
            dtExpiry.Value = DateTime.Today;

            txtPrice.Text = "";
            txtDescription.Text = "";
        }

    }
}
