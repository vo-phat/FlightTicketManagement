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
        private ComboBox _cbAirline; // ✅ ComboBox hiển thị "ID - Tên hãng"
        private UnderlinedTextField _txtModel, _txtManu, _txtCap;
        private PrimaryButton _btnSave;
        private SecondaryButton _btnCancel;
        private TableCustom _table;

        private readonly AircraftBUS _bus = new AircraftBUS();
        private readonly AirlineBUS _airlineBus = new AirlineBUS();
        private int _editingId = 0; // 0 = tạo mới, >0 = edit

        // Sự kiện thông báo cho control cha
        public event EventHandler? DataSaved;
        public event EventHandler? DataUpdated;

        public AircraftCreateControl()
        {
            InitializeComponent();
            LoadAirlines();     // ✅ load danh sách hãng trước
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

            // ✅ Combobox Airline ID + Name
            _cbAirline = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 250,
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(0, 10, 0, 10)
            };

            _txtModel = new UnderlinedTextField("Model", "");
            _txtManu = new UnderlinedTextField("Hãng sản xuất", "");
            _txtCap = new UnderlinedTextField("Sức chứa (ghế)", "");

            // Label cho combobox
            var lblAirline = new Label
            {
                Text = "Mã hãng (ID - Tên hãng)",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0)
            };

            inputs.Controls.Add(lblAirline, 0, 0);
            inputs.Controls.Add(_cbAirline, 1, 0);
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
            _table.Columns.Add("airline", "Mã hãng");
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

        // ✅ Nạp danh sách hãng hiển thị "ID - Tên hãng"
        private void LoadAirlines()
        {
            try
            {
                var list = _airlineBus.GetAllAirlines(); // List<AirlineDTO>

                // Hiển thị "1 - Vietnam Airlines"
                _cbAirline.DataSource = list;
                _cbAirline.DisplayMember = "DisplayText"; // property ghép ID + Name
                _cbAirline.ValueMember = "AirlineId";     // giá trị thật là ID
                _cbAirline.SelectedIndex = -1;
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
                _table.Rows.Clear();
                foreach (var a in list)
                {
                    _table.Rows.Add(
                        a.AirlineId,
                        a.Model ?? "N/A",
                        a.Manufacturer ?? "N/A",
                        a.Capacity.HasValue ? a.Capacity.Value.ToString() : "N/A"
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
                if (_cbAirline.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn hãng hàng không.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int airlineId = (int)_cbAirline.SelectedValue;

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
                    // THÊM MỚI
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
                    // CẬP NHẬT
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
            _cbAirline.Enabled = true;
            _cbAirline.SelectedIndex = -1;
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
            _cbAirline.SelectedValue = dto.AirlineId;
            _cbAirline.Enabled = false;
            _txtModel.Text = dto.Model ?? "";
            _txtManu.Text = dto.Manufacturer ?? "";
            _txtCap.Text = dto.Capacity.HasValue ? dto.Capacity.Value.ToString() : "";
            _btnSave.Text = $"✍️ Cập nhật #{dto.AircraftId}";
        }
    }
}
