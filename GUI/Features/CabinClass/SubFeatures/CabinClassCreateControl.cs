using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using DTO.CabinClass;
using BUS.CabinClass;

namespace GUI.Features.CabinClass.SubFeatures
{
    public class CabinClassCreateControl : UserControl
    {
        private UnderlinedTextField _txtName, _txtDescription;
        private PrimaryButton _btnSave;
        private SecondaryButton _btnCancel;
        private TableCustom _table;

        private readonly CabinClassBUS _bus = new CabinClassBUS();
        private int _editingId = 0;

        public event EventHandler? DataSaved;
        public event EventHandler? DataUpdated;

        public CabinClassCreateControl()
        {
            InitializeComponent();
            LoadCabinClassList();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // --- Title ---
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label { Text = "➕ Tạo hạng ghế mới", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
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

            _txtName = new UnderlinedTextField("Tên hạng ghế", "");
            _txtDescription = new UnderlinedTextField("Mô tả", "");

            _txtName.Width = 300; _txtDescription.Width = 400;

            inputs.Controls.Add(_txtName, 0, 0);
            inputs.Controls.Add(_txtDescription, 1, 0);

            // --- Buttons ---
            _btnSave = new PrimaryButton("💾 Lưu hạng ghế") { Width = 160, Height = 40, Margin = new Padding(4) };
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

            // --- Table (danh sách hạng ghế) ---
            _table = new TableCustom
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            _table.Columns.Add("name", "Tên hạng ghế");
            _table.Columns.Add("description", "Mô tả");

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

        public void LoadCabinClassList()
        {
            try
            {
                var list = _bus.GetAllCabinClasses();
                _table.Rows.Clear();
                foreach (var c in list)
                {
                    _table.Rows.Add(
                        c.ClassName,
                        c.Description ?? ""
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hạng ghế: " + ex.Message);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var name = _txtName.Text?.Trim();
                var description = _txtDescription.Text?.Trim();

                CabinClassDTO dto;
                string message;
                bool ok;

                if (_editingId == 0)
                {
                    // THÊM MỚI
                    dto = new CabinClassDTO(name, description);
                    ok = _bus.AddCabinClass(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataSaved?.Invoke(this, EventArgs.Empty);
                        LoadCabinClassList();
                        ClearAndReset();
                    }
                    else
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // CẬP NHẬT
                    dto = new CabinClassDTO(_editingId, name, description);
                    ok = _bus.UpdateCabinClass(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataUpdated?.Invoke(this, EventArgs.Empty);
                        LoadCabinClassList();
                        ClearAndReset();
                    }
                    else
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hạng ghế: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAndReset()
        {
            _editingId = 0;
            _txtName.Text = _txtDescription.Text = "";

            _txtName.ReadOnly = false;
            _btnSave.Text = "💾 Lưu hạng ghế";
        }

        public void LoadForEdit(CabinClassDTO dto)
        {
            if (dto == null || dto.ClassId <= 0)
            {
                ClearAndReset();
                return;
            }

            _editingId = dto.ClassId;
            _txtName.Text = dto.ClassName ?? "";
            _txtDescription.Text = dto.Description ?? "";

            // Giữ nguyên logic cho phép chỉnh sửa Tên hạng ghế
            _txtName.ReadOnly = false;

            _btnSave.Text = $"✍️ Cập nhật #{dto.ClassId}";
        }
    }
}