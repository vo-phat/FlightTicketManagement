using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using DTO.Aircraft;
using BUS.Aircraft;
// ĐÃ XÓA: using BUS.Airline; - Không còn quản lý Airlines
// ĐÃ XÓA: using DTO.Airline; - Không còn quản lý Airlines

namespace GUI.Features.Aircraft.SubFeatures
{
    public class AircraftCreateControl : UserControl
    {
        // ĐÃ XÓA: _cbAirline, _airlineBus - Không còn quản lý Airlines
        private UnderlinedTextField _txtRegNum, _txtModel, _txtManu, _txtCap, _txtYear;
        private UnderlinedComboBox _cbStatus;
        private PrimaryButton _btnSave;
        private SecondaryButton _btnCancel;
        private TableCustom _table;

        private readonly AircraftBUS _bus = new AircraftBUS();
        private int _editingId = 0;

        public event EventHandler? DataSaved;
        public event EventHandler? DataUpdated;

        public AircraftCreateControl()
        {
            InitializeComponent();
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

            // ĐÃ XÓA: _cbAirline - Chỉ quản lý Vietnam Airlines
            _txtRegNum = new UnderlinedTextField("Số hiệu đăng ký (VN-A###)", "");
            _txtModel = new UnderlinedTextField("Model", "");
            _txtManu = new UnderlinedTextField("Hãng sản xuất", "");
            _txtCap = new UnderlinedTextField("Sức chứa (ghế)", "");
            _txtYear = new UnderlinedTextField("Năm sản xuất", "");
            
            _cbStatus = new UnderlinedComboBox("Trạng thái", new[] { "Active", "Maintenance", "Retired" });
            _cbStatus.InnerComboBox.SelectedIndex = 0;

            inputs.Controls.Add(_txtRegNum, 0, 0);
            inputs.Controls.Add(_txtModel, 1, 0);
            inputs.Controls.Add(_txtManu, 0, 1);
            inputs.Controls.Add(_txtCap, 1, 1);
            inputs.Controls.Add(_txtYear, 0, 2);
            inputs.Controls.Add(_cbStatus, 1, 2);

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
            _table.Columns.Add("registration", "Số hiệu đăng ký");
            _table.Columns.Add("model", "Model");
            _table.Columns.Add("manufacturer", "Hãng sản xuất");
            _table.Columns.Add("capacity", "Sức chứa");
            _table.Columns.Add("year", "Năm SX");
            _table.Columns.Add("status", "Trạng thái");

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

        // ĐÃ XÓA: LoadAirlines() - Không còn quản lý Airlines

        public void LoadAircraftList()
        {
            try
            {
                var list = _bus.GetAllAircrafts();

                _table.Rows.Clear();
                foreach (var a in list)
                {
                    _table.Rows.Add(
                        a.RegistrationNumber ?? "N/A",
                        a.Model ?? "N/A",
                        a.Manufacturer ?? "N/A",
                        a.Capacity?.ToString() ?? "N/A",
                        a.ManufactureYear?.ToString() ?? "N/A",
                        a.Status ?? "N/A"
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
                // ĐÃ XÓA: Airline validation - Chỉ quản lý Vietnam Airlines
                
                var regNum = _txtRegNum.Text?.Trim();
                if (string.IsNullOrWhiteSpace(regNum))
                {
                    MessageBox.Show("Vui lòng nhập số hiệu đăng ký (VN-A###).", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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

                int? year = null;
                if (!string.IsNullOrWhiteSpace(_txtYear.Text))
                {
                    if (!int.TryParse(_txtYear.Text, out int yearValue))
                    {
                        MessageBox.Show("Năm sản xuất phải là số nguyên.",
                            "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    year = yearValue;
                }

                var model = _txtModel.Text?.Trim();
                var manufacturer = _txtManu.Text?.Trim();
                var status = _cbStatus.InnerComboBox.SelectedItem?.ToString();

                AircraftDTO dto;
                string message;
                bool ok;

                if (_editingId == 0)
                {
                    dto = new AircraftDTO(regNum, model, manufacturer, capacity, year, status);
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
                    dto = new AircraftDTO(_editingId, regNum, model, manufacturer, capacity, year, status);
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
            _txtRegNum.Text = "";
            _txtModel.Text = "";
            _txtManu.Text = "";
            _txtCap.Text = "";
            _txtYear.Text = "";
            _cbStatus.InnerComboBox.SelectedIndex = 0;
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
            _txtRegNum.Text = dto.RegistrationNumber ?? "";
            _txtModel.Text = dto.Model ?? "";
            _txtManu.Text = dto.Manufacturer ?? "";
            _txtCap.Text = dto.Capacity?.ToString() ?? "";
            _txtYear.Text = dto.ManufactureYear?.ToString() ?? "";
            _cbStatus.InnerComboBox.SelectedItem = dto.Status ?? "Active";
            _btnSave.Text = $"✍️ Cập nhật #{dto.AircraftId}";
        }
    }
}
