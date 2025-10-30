using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using DTO.Aircraft; // DTO của bạn
using BUS.Aircraft; // BUS của bạn

namespace GUI.Features.Aircraft.SubFeatures
{
    public class AircraftCreateControl : UserControl
    {
        private UnderlinedTextField _txtAirline, _txtModel, _txtManu, _txtCap;
        private PrimaryButton _btnSave;
        private SecondaryButton _btnCancel;
        private TableCustom _table;

        private readonly AircraftBUS _bus = new AircraftBUS();
        private int _editingId = 0; // 0 = tạo mới, >0 = edit

        // Sự kiện thông báo cho control cha
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
            var lblTitle = new Label { Text = "➕ Tạo máy bay mới", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
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

            _txtAirline = new UnderlinedTextField("Mã hãng (Airline ID)", "");
            _txtModel = new UnderlinedTextField("Model", "");
            _txtManu = new UnderlinedTextField("Hãng sản xuất", "");
            _txtCap = new UnderlinedTextField("Sức chứa (ghế)", "");

            _txtAirline.Width = 200; _txtModel.Width = 300; _txtManu.Width = 200; _txtCap.Width = 200;

            inputs.Controls.Add(_txtAirline, 0, 0);
            inputs.Controls.Add(_txtModel, 1, 0);
            inputs.Controls.Add(_txtManu, 0, 1);
            inputs.Controls.Add(_txtCap, 1, 1);

            // --- Buttons ---
            _btnSave = new PrimaryButton("💾 Lưu máy bay") { Width = 160, Height = 40, Margin = new Padding(4) };
            _btnCancel = new SecondaryButton("Hủy") { Width = 90, Height = 40, Margin = new Padding(4) };
            _btnSave.Click += BtnSave_Click;
            _btnCancel.Click += (_, __) => ClearAndReset(); // Sẽ reset về trạng thái Add

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(24, 0, 24, 0)
            };
            btnPanel.Controls.AddRange(new Control[] { _btnSave, _btnCancel });

            // --- Table (danh sách máy bay) ---
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
                if (!int.TryParse(_txtAirline.Text, out int airlineId))
                {
                    MessageBox.Show("Mã hãng (Airline ID) phải là số nguyên dương.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ... (Logic Capacity, Model, Manufacturer giữ nguyên) ...
                int? capacity = null;
                if (!string.IsNullOrWhiteSpace(_txtCap.Text))
                {
                    if (!int.TryParse(_txtCap.Text, out int capValue) || capValue < 1)
                    {
                        MessageBox.Show("Sức chứa phải là số nguyên dương hoặc để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Phương thức này CỰC KỲ QUAN TRỌNG để reset về trạng thái "Thêm mới"
        private void ClearAndReset()
        {
            _editingId = 0;
            _txtAirline.Text = "";
            _txtModel.Text = "";
            _txtManu.Text = "";
            _txtCap.Text = "";

            // 1. ĐẢM BẢO RẰNG AIRLINE ID CÓ THỂ CHỈNH SỬA KHI TẠO MỚI
            _txtAirline.ReadOnly = false;

            _btnSave.Text = "💾 Lưu máy bay"; // Reset nút về trạng thái Lưu
        }

        // Phương thức được gọi từ AircraftControl để nạp dữ liệu chỉnh sửa
        public void LoadForEdit(AircraftDTO dto)
        {
            if (dto == null)
            {
                // Nếu được gọi với DTO rỗng/null, coi như reset về Add
                ClearAndReset();
                return;
            }

            // Nếu AircraftId không hợp lệ, cũng coi như reset về Add
            if (dto.AircraftId <= 0)
            {
                ClearAndReset();
                return;
            }

            // BẮT ĐẦU TRẠNG THÁI EDIT
            _editingId = dto.AircraftId;
            _txtAirline.Text = dto.AirlineId.ToString();
            _txtModel.Text = dto.Model ?? "";
            _txtManu.Text = dto.Manufacturer ?? "";
            _txtCap.Text = dto.Capacity.HasValue ? dto.Capacity.Value.ToString() : "";

            // 2. KHÓA ID HÃNG KHI CHỈNH SỬA
            _txtAirline.ReadOnly = true;

            _btnSave.Text = $"✍️ Cập nhật #{dto.AircraftId}";
        }
    }
}