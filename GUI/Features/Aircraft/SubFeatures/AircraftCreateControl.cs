using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using DTO.Aircraft;
using BUS.Aircraft;
using BUS.Airline;
using DTO.Airline;

namespace GUI.Features.Aircraft.SubFeatures
{
    public class AircraftCreateControl : UserControl
    {
        private UnderlinedComboBox _cbAirline; // ✅ Dùng combo tùy chỉnh
        private UnderlinedTextField _txtModel, _txtManu, _txtCap;
        private PrimaryButton _btnSave;
        private SecondaryButton _btnCancel;
        private TableCustom _table;

        private readonly AircraftBUS _bus = new AircraftBUS();
        private readonly AirlineBUS _airlineBus = new AirlineBUS();
        private int _editingId = 0;

        public event EventHandler? DataSaved;
        public event EventHandler? DataUpdated;

        public AircraftCreateControl()
        {
            InitializeComponent();
            LoadAirlines();
            LoadAircraftList();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // --- Title ---
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label
            {
                Text = "➕ Tạo máy bay mới",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold)
            };
            titlePanel.Controls.Add(lblTitle);

            // --- Inputs ---
            var inputs = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Padding = new Padding(24, 12, 24, 0),
                AutoSize = true,
                ColumnCount = 2
            };
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            // ✅ Dùng UnderlinedComboBox thay vì ComboBox thường
            _cbAirline = new UnderlinedComboBox("Hãng hàng không", Array.Empty<string>());
            _cbAirline.Width = 250;

            _txtModel = new UnderlinedTextField("Model", "");
            _txtManu = new UnderlinedTextField("Hãng sản xuất", "");
            _txtCap = new UnderlinedTextField("Sức chứa (ghế)", "");

            inputs.Controls.Add(_cbAirline, 0, 0);
            inputs.SetColumnSpan(_cbAirline, 2); // cho rộng ra 2 cột
            inputs.Controls.Add(_txtModel, 0, 1);
            inputs.Controls.Add(_txtManu, 1, 1);
            inputs.Controls.Add(_txtCap, 0, 2);

            // --- Buttons ---
            _btnSave = new PrimaryButton("💾 Lưu máy bay") { Width = 160, Height = 40, Margin = new Padding(4) };
            _btnCancel = new SecondaryButton("Hủy") { Width = 90, Height = 40, Margin = new Padding(4) };
            _btnSave.Click += BtnSave_Click;
            _btnCancel.Click += (_, __) => ClearAndReset();

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(24, 0, 24, 0)
            };
            btnPanel.Controls.AddRange(new Control[] { _btnSave, _btnCancel });

            // --- Table ---
            _table = new TableCustom
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            _table.Columns.Add("airline", "Hãng hàng không");
            _table.Columns.Add("model", "Model");
            _table.Columns.Add("manufacturer", "Hãng sản xuất");
            _table.Columns.Add("capacity", "Sức chứa");

            // --- Main layout ---
            var main = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            main.Controls.Add(titlePanel, 0, 0);
            main.Controls.Add(inputs, 0, 1);
            main.Controls.Add(btnPanel, 0, 2);
            main.Controls.Add(_table, 0, 3);

            Controls.Add(main);
        }

        // ✅ Load danh sách hãng hiển thị "ID - Tên hãng"
        private void LoadAirlines()
        {
            try
            {
                var list = _airlineBus.GetAllAirlines(); // List<AirlineDTO>

                _cbAirline.InnerComboBox.DataSource = list;
                _cbAirline.InnerComboBox.DisplayMember = "DisplayText";
                _cbAirline.InnerComboBox.ValueMember = "AirlineId";
                _cbAirline.InnerComboBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hãng: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void LoadAircraftList()
        {
            try
            {
                var list = _bus.GetAllAircrafts();
                var airlines = _airlineBus.GetAllAirlines();

                _table.Rows.Clear();
                foreach (var a in list)
                {
                    string airlineName = airlines.FirstOrDefault(al => al.AirlineId == a.AirlineId)?.AirlineName ?? "N/A";
                    _table.Rows.Add(
                        $"{a.AirlineId} - {airlineName}",
                        a.Model ?? "N/A",
                        a.Manufacturer ?? "N/A",
                        a.Capacity?.ToString() ?? "N/A"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách máy bay: " + ex.Message);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var combo = _cbAirline.InnerComboBox;
                if (combo.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn hãng hàng không.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int airlineId = (int)combo.SelectedValue;

                int? capacity = null;
                if (!string.IsNullOrWhiteSpace(_txtCap.Text))
                {
                    if (!int.TryParse(_txtCap.Text, out int capValue) || capValue < 1)
                    {
                        MessageBox.Show("Sức chứa phải là số nguyên dương hoặc để trống.",
                            "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    capacity = capValue;
                }

                var model = _txtModel.Text?.Trim();
                var manufacturer = _txtManu.Text?.Trim();

                AircraftDTO dto;
                string message;
                bool ok;

                if (_editingId == 0)
                {
                    dto = new AircraftDTO(airlineId, model, manufacturer, capacity);
                    ok = _bus.AddAircraft(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataSaved?.Invoke(this, EventArgs.Empty);
                        LoadAircraftList();
                        ClearAndReset();
                    }
                    else
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    dto = new AircraftDTO(_editingId, airlineId, model, manufacturer, capacity);
                    ok = _bus.UpdateAircraft(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataUpdated?.Invoke(this, EventArgs.Empty);
                        LoadAircraftList();
                        ClearAndReset();
                    }
                    else
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu máy bay: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAndReset()
        {
            _editingId = 0;
            _cbAirline.InnerComboBox.Enabled = true;
            _cbAirline.InnerComboBox.SelectedIndex = -1;
            _txtModel.Text = "";
            _txtManu.Text = "";
            _txtCap.Text = "";
            _btnSave.Text = "💾 Lưu máy bay";
        }

        public void LoadForEdit(AircraftDTO dto)
        {
            if (dto == null || dto.AircraftId <= 0)
            {
                ClearAndReset();
                return;
            }

            _editingId = dto.AircraftId;
            _cbAirline.InnerComboBox.SelectedValue = dto.AirlineId;
            _cbAirline.InnerComboBox.Enabled = false;
            _txtModel.Text = dto.Model ?? "";
            _txtManu.Text = dto.Manufacturer ?? "";
            _txtCap.Text = dto.Capacity?.ToString() ?? "";
            _btnSave.Text = $"✍️ Cập nhật #{dto.AircraftId}";
        }
    }
}
