using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using DTO.Airline;
using BUS.Airline;

namespace GUI.Features.Airline.SubFeatures
{
    public class AirlineCreateControl : UserControl
    {
        private UnderlinedTextField _txtCode, _txtName;
        private UnderlinedTextField _txtCountry; // Dùng TextField thay vì ComboBox để đơn giản
        private PrimaryButton _btnSave;
        private SecondaryButton _btnCancel;
        private TableCustom _table;

        private readonly AirlineBUS _bus = new AirlineBUS();
        private int _editingId = 0; // 0 = tạo mới, >0 = edit

        public event EventHandler? DataSaved;
        public event EventHandler? DataUpdated;

        public AirlineCreateControl()
        {
            InitializeComponent();
            LoadAirlineList();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // --- Title ---
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label { Text = "➕ Tạo hãng hàng không mới", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
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

            _txtCode = new UnderlinedTextField("Mã hãng (VD: VN, JL)", "");
            _txtName = new UnderlinedTextField("Tên hãng", "");
            _txtCountry = new UnderlinedTextField("Quốc gia", "");

            _txtCode.Width = 200; _txtName.Width = 300; _txtCountry.Width = 200;

            inputs.Controls.Add(_txtCode, 0, 0);
            inputs.Controls.Add(_txtName, 1, 0);
            inputs.Controls.Add(_txtCountry, 0, 1);
            // Bỏ ô (1, 1) để giữ layout 2x1 cho các trường chính
            inputs.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // --- Buttons ---
            _btnSave = new PrimaryButton("💾 Lưu hãng") { Width = 160, Height = 40, Margin = new Padding(4) };
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

            // --- Table (danh sách hãng hàng không) ---
            _table = new TableCustom
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            _table.Columns.Add("code", "Mã hãng");
            _table.Columns.Add("name", "Tên hãng");
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
        }

        public void LoadAirlineList()
        {
            try
            {
                var list = _bus.GetAllAirlines();
                _table.Rows.Clear();
                foreach (var a in list)
                {
                    _table.Rows.Add(
                        a.AirlineCode,
                        a.AirlineName,
                        a.Country ?? "N/A"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hãng: " + ex.Message);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var code = _txtCode.Text?.Trim();
                var name = _txtName.Text?.Trim();
                var country = _txtCountry.Text?.Trim();

                AirlineDTO dto;
                string message;
                bool ok;

                if (_editingId == 0)
                {
                    // THÊM MỚI (AirlineId = 0)
                    dto = new AirlineDTO(code, name, country);
                    ok = _bus.AddAirline(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataSaved?.Invoke(this, EventArgs.Empty);
                        LoadAirlineList();
                        ClearAndReset();
                    }
                    else
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // CẬP NHẬT
                    dto = new AirlineDTO(_editingId, code, name, country);
                    ok = _bus.UpdateAirline(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataUpdated?.Invoke(this, EventArgs.Empty);
                        LoadAirlineList();
                        ClearAndReset();
                    }
                    else
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hãng hàng không: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAndReset()
        {
            _editingId = 0;
            _txtCode.Text = _txtName.Text = _txtCountry.Text = "";

            _txtCode.ReadOnly = false;
            _btnSave.Text = "💾 Lưu hãng";
        }

        public void LoadForEdit(AirlineDTO dto)
        {
            if (dto == null || dto.AirlineId <= 0)
            {
                ClearAndReset();
                return;
            }

            _editingId = dto.AirlineId;
            _txtCode.Text = dto.AirlineCode ?? "";
            _txtName.Text = dto.AirlineName ?? "";
            _txtCountry.Text = dto.Country ?? "";

            _txtCode.ReadOnly = false; // Khóa Mã hãng khi chỉnh sửa
            _btnSave.Text = $"✍️ Cập nhật #{dto.AirlineId}";
        }
    }
}