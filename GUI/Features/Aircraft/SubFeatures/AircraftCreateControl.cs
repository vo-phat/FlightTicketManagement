using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using DTO.Aircraft;
using BUS.Aircraft;

namespace GUI.Features.Aircraft.SubFeatures
{
    public class AircraftCreateControl : UserControl
    {
        private const int VIETNAM_AIRLINES_ID = 1;
        
        private Label lblTitle = null!;
        private UnderlinedTextField txtRegNum = null!;
        private UnderlinedComboBox cbModel = null!;
        private UnderlinedComboBox cbManu = null!;
        private NumericUpDown nudCapacity = null!;
        private PrimaryButton btnSave = null!;
        private SecondaryButton btnCancel = null!;

        private readonly AircraftBUS _bus = new AircraftBUS();
        private int _editingId = 0;

        public event EventHandler? DataSaved;
        public event EventHandler? DataUpdated;

        public AircraftCreateControl()
        {
            InitializeComponent();
            LoadExistingData();
        }

        private Label CreateKeyLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 55, 77),
                Margin = new Padding(0, 10, 0, 0),
                Padding = new Padding(0, 10, 0, 0)
            };
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);
            AutoScroll = true;

            // Title
            lblTitle = new Label
            {
                Text = "🛩️ Tạo máy bay mới",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 55, 77),
                AutoSize = true,
                Padding = new Padding(24, 20, 24, 12),
                Dock = DockStyle.Top
            };

            // Main card panel
            var containerPanel = new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(24),
                Margin = new Padding(24, 8, 24, 24),
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Info grid using TableLayoutPanel
            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                Padding = new Padding(0, 0, 0, 16)
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int row = 0;

            // ===== SECTION: THÔNG TIN MÁY BAY =====
            var lblSection = new Label
            {
                Text = "📋 THÔNG TIN MÁY BAY",
                AutoSize = true,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                Margin = new Padding(0, 0, 0, 12),
                Dock = DockStyle.Top
            };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(lblSection, 0, row);
            grid.SetColumnSpan(lblSection, 2);
            row++;

            // Registration Number (Auto-generated, read-only)
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Số hiệu đăng ký:"), 0, row);
            txtRegNum = new UnderlinedTextField("", "VN-A###") 
            { 
                Dock = DockStyle.Fill, 
                Margin = new Padding(0, 8, 0, 8),
                Enabled = false  // Read-only
            };
            grid.Controls.Add(txtRegNum, 1, row++);

            // Model (ComboBox)
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Model: *"), 0, row);
            cbModel = new UnderlinedComboBox("", Array.Empty<object>()) 
            { 
                Dock = DockStyle.Fill, 
                Margin = new Padding(0, 8, 0, 8) 
            };
            if (cbModel.InnerCombo is ComboBox rawModel)
            {
                rawModel.DropDownStyle = ComboBoxStyle.DropDown;
                rawModel.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                rawModel.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            grid.Controls.Add(cbModel, 1, row++);

            // Manufacturer (ComboBox)
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Hãng sản xuất: *"), 0, row);
            cbManu = new UnderlinedComboBox("", Array.Empty<object>()) 
            { 
                Dock = DockStyle.Fill, 
                Margin = new Padding(0, 8, 0, 8) 
            };
            if (cbManu.InnerCombo is ComboBox rawManu)
            {
                rawManu.DropDownStyle = ComboBoxStyle.DropDown;
                rawManu.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                rawManu.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            grid.Controls.Add(cbManu, 1, row++);

            // Capacity (NumericUpDown)
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Sức chứa (ghế): *"), 0, row);
            nudCapacity = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 8, 0, 8),
                Font = new Font("Segoe UI", 11f),
                Minimum = 1,
                Maximum = 850,  // Max for largest commercial aircraft
                Value = 180,
                DecimalPlaces = 0,
                ThousandsSeparator = false,
                Increment = 10
            };
            grid.Controls.Add(nudCapacity, 1, row++);

            // Separator
            var separator = new Panel 
            { 
                Height = 2, 
                BackColor = Color.FromArgb(220, 220, 220), 
                Margin = new Padding(0, 16, 0, 16), 
                Dock = DockStyle.Top 
            };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(separator, 0, row);
            grid.SetColumnSpan(separator, 2);
            row++;

            // Action buttons
            var actionPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 16, 0, 0)
            };

            btnSave = new PrimaryButton
            {
                Text = "💾 Lưu máy bay",
                Width = 160,
                Height = 45
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new SecondaryButton
            {
                Text = "❌ Hủy bỏ",
                Width = 130,
                Height = 45,
                Margin = new Padding(10, 0, 0, 0)
            };
            btnCancel.Click += BtnCancel_Click;

            actionPanel.Controls.Add(btnSave);
            actionPanel.Controls.Add(btnCancel);

            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(actionPanel, 0, row);
            grid.SetColumnSpan(actionPanel, 2);

            containerPanel.Controls.Add(grid);
            Controls.Add(containerPanel);
            Controls.Add(lblTitle);

            ResumeLayout(false);
            PerformLayout();
        }

        private void LoadExistingData()
        {
            try
            {
                var allAircrafts = _bus.GetAllAircrafts();
                
                // Load unique Models
                var models = allAircrafts
                    .Where(a => !string.IsNullOrWhiteSpace(a.Model))
                    .Select(a => a.Model!)
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList();
                
                cbModel.InnerCombo.Items.Clear();
                cbModel.InnerCombo.Items.AddRange(models.Cast<object>().ToArray());
                
                // Load unique Manufacturers
                var manufacturers = allAircrafts
                    .Where(a => !string.IsNullOrWhiteSpace(a.Manufacturer))
                    .Select(a => a.Manufacturer!)
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList();
                
                cbManu.InnerCombo.Items.Clear();
                cbManu.InnerCombo.Items.AddRange(manufacturers.Cast<object>().ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu máy bay hiện có: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                // Get values from ComboBox
                var model = cbModel.InnerCombo.Text?.Trim();
                var manufacturer = cbManu.InnerCombo.Text?.Trim();
                
                if (string.IsNullOrWhiteSpace(model))
                {
                    MessageBox.Show("Vui lòng nhập Model máy bay.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(manufacturer))
                {
                    MessageBox.Show("Vui lòng nhập Hãng sản xuất.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int capacity = (int)nudCapacity.Value;
                if (capacity < 1)
                {
                    MessageBox.Show("Sức chứa phải lớn hơn 0.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                AircraftDTO dto;
                string message;
                bool ok;

                // HARDCODE: airline_id = 1 (Vietnam Airlines)
                if (_editingId == 0)
                {
                    // Create new
                    dto = new AircraftDTO(VIETNAM_AIRLINES_ID, model, manufacturer, capacity);
                    ok = _bus.AddAircraft(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataSaved?.Invoke(this, EventArgs.Empty);
                        ClearAndReset();
                    }
                    else
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Update
                    dto = new AircraftDTO(_editingId, VIETNAM_AIRLINES_ID, model, manufacturer, capacity);
                    ok = _bus.UpdateAircraft(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataUpdated?.Invoke(this, EventArgs.Empty);
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

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            ClearAndReset();
        }

        private void ClearAndReset()
        {
            txtRegNum.Text = "VN-A###";
            cbModel.InnerCombo.SelectedIndex = -1;
            cbModel.InnerCombo.Text = "";
            cbManu.InnerCombo.SelectedIndex = -1;
            cbManu.InnerCombo.Text = "";
            nudCapacity.Value = 180;
            _editingId = 0;
            lblTitle.Text = "🛩️ Tạo máy bay mới";
            btnSave.Text = "💾 Lưu máy bay";
        }

        public void LoadForEdit(AircraftDTO dto)
        {
            if (dto == null || dto.AircraftId <= 0)
            {
                ClearAndReset();
                return;
            }

            _editingId = dto.AircraftId;
            
            // Auto-generate registration number
            txtRegNum.Text = $"VN-A{dto.AircraftId:000}";
            
            cbModel.InnerCombo.Text = dto.Model ?? "";
            cbManu.InnerCombo.Text = dto.Manufacturer ?? "";
            nudCapacity.Value = dto.Capacity ?? 180;
            
            lblTitle.Text = $"✏️ Chỉnh sửa máy bay #{dto.AircraftId}";
            btnSave.Text = "💾 Cập nhật";
        }
    }
}
