using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using BUS.Airport;
using DTO.Airport;

namespace GUI.Features.Airport.SubFeatures
{
    public class AirportCreateControl : UserControl
    {
        private UnderlinedTextField _txtCode, _txtName, _txtCity;
        private UnderlinedComboBox _cbCountry;
        private PrimaryButton _btnSave;
        private SecondaryButton _btnCancel;
        private TableCustom _table;
        private readonly AirportBUS _bus = new AirportBUS();
        private int _editingId = 0; // 0 = tạo mới, >0 = edit

        public event EventHandler? AirportSaved;
        public event EventHandler? AirportSavedUpdated;

        public AirportCreateControl()
        {
            InitializeComponent();
            LoadAirportList();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // --- Title ---
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label
            {
                Text = "➕ Tạo sân bay",
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

            _txtCode = new UnderlinedTextField("Mã IATA", "");
            _txtName = new UnderlinedTextField("Tên sân bay", "");
            _txtCity = new UnderlinedTextField("Thành phố", "");
            _cbCountry = new UnderlinedComboBox("Quốc gia", new object[]
            {
                "Việt Nam", "Nhật Bản", "Hàn Quốc", "Singapore",
                "Thái Lan", "Hoa Kỳ", "Anh", "Pháp", "Úc", "Canada"
            });

            _txtCode.Width = 200; _txtName.Width = 300; _txtCity.Width = 200; _cbCountry.Width = 200;

            inputs.Controls.Add(_txtCode, 0, 0);
            inputs.Controls.Add(_txtName, 1, 0);
            inputs.Controls.Add(_txtCity, 0, 1);
            inputs.Controls.Add(_cbCountry, 1, 1);

            // --- Buttons ---
            _btnSave = new PrimaryButton("💾 Lưu sân bay") { Width = 160, Height = 40, Margin = new Padding(4) };
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

            // --- Table (danh sách sân bay) ---
            _table = new TableCustom
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            _table.Columns.Add("code", "IATA");
            _table.Columns.Add("name", "Tên sân bay");
            _table.Columns.Add("city", "Thành phố");
            _table.Columns.Add("country", "Quốc gia");

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

            // Chỉ cho phép chữ hoa 3 ký tự ở IATA
            _txtCode.TextChanged += (_, __) =>
            {
                var t = _txtCode.Text ?? string.Empty;
                var filtered = new string(t.Where(char.IsLetter).ToArray()).ToUpperInvariant();
                if (filtered.Length > 3) filtered = filtered.Substring(0, 3);
                if (filtered != _txtCode.Text) _txtCode.Text = filtered;
            };
        }

        private void LoadAirportList()
        {
            try
            {   
                var list = _bus.GetAllAirports();
                _table.Rows.Clear();
                foreach (var a in list)
                {
                    _table.Rows.Add(a.AirportCode, a.AirportName, a.City, a.Country);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sân bay: " + ex.Message);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var code = _txtCode.Text?.Trim() ?? "";
                var name = _txtName.Text?.Trim() ?? "";
                var city = _txtCity.Text?.Trim() ?? "";
                var country = _cbCountry.SelectedItem?.ToString() ?? "";

                var dto = new AirportDTO(code, name, city, country);
                string message;
                bool ok;

                if (_editingId == 0)
                {
                    ok = _bus.AddAirport(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AirportSaved?.Invoke(this, EventArgs.Empty);
                        LoadAirportList(); // cập nhật danh sách
                        ClearAndReset();
                    }
                    else
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    dto = new AirportDTO(_editingId, dto.AirportCode, dto.AirportName, dto.City, dto.Country);
                    ok = _bus.UpdateAirport(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AirportSavedUpdated?.Invoke(this, EventArgs.Empty);
                        LoadAirportList();
                        ClearAndReset();
                        _editingId = 0;
                    }
                    else
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu sân bay: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAndReset()
        {
            _editingId = 0;
            _txtCode.Text = _txtName.Text = _txtCity.Text = "";
            _cbCountry.SelectedIndex = -1;
        }

        public void LoadForEdit(AirportDTO dto)
        {
            if (dto == null) return;
            _editingId = dto.AirportId;
            _txtCode.Text = dto.AirportCode ?? "";
            _txtName.Text = dto.AirportName ?? "";
            _txtCity.Text = dto.City ?? "";
            _cbCountry.SelectedItem = dto.Country ?? _cbCountry.Items.Cast<object>().FirstOrDefault();
        }

    }
}
