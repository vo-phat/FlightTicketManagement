using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using DTO.Aircraft;
using BUS.Aircraft;
// ✅ Added for seat generation
using BUS.Seat;
using BUS.CabinClass;
using DTO.Seat;

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
            txtRegNum = new UnderlinedTextField("", "Sinh tự động") 
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
                Minimum = 6,  // ✅ Min 6
                Maximum = 850,  // Max for largest commercial aircraft
                Value = 18,  // ✅ Default 18
                DecimalPlaces = 0,
                ThousandsSeparator = false,
                Increment = 6  // ✅ Step by 6
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

                // ✅ Validate: Capacity must be multiple of 6
                if (capacity % 6 != 0)
                {
                    MessageBox.Show("Sức chứa phải là bội số của 6 (ví dụ: 6, 12, 18, 24, 30...).\n\nĐiều này đảm bảo số ghế phù hợp với cấu hình hàng ghế.", 
                                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    int newAircraftId = _bus.AddAircraft(dto, out message);
                    if (newAircraftId > 0)
                    {
                        // ✅ Auto-generate seats for new aircraft
                        GenerateSeatsForNewAircraft(newAircraftId, capacity);
                        
                        MessageBox.Show(message + "\n\nGhế đã được tạo tự động!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtRegNum.Text = "Sinh tự động";
            cbModel.InnerCombo.SelectedIndex = -1;
            cbModel.InnerCombo.Text = "";
            cbManu.InnerCombo.SelectedIndex = -1;
            cbManu.InnerCombo.Text = "";
            nudCapacity.Value = 18;  // ✅ Default 18
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
            nudCapacity.Value = dto.Capacity ?? 18;  // ✅ Default 18
            
            lblTitle.Text = $"✏️ Chỉnh sửa máy bay #{dto.AircraftId}";
            btnSave.Text = "💾 Cập nhật";
        }

        // ✅ Auto-generate seats for newly created aircraft
        private void GenerateSeatsForNewAircraft(int aircraftId, int capacity)
        {
            try
            {
                var seatBUS = new SeatBUS();
                var cabinClassBUS = new CabinClassBUS();
                
                // Get cabin class IDs
                var allClasses = cabinClassBUS.GetAllCabinClasses();
                var firstClass = allClasses.FirstOrDefault(c => c.ClassName == "First");
                var businessClass = allClasses.FirstOrDefault(c => c.ClassName == "Business");
                var economyClass = allClasses.FirstOrDefault(c => c.ClassName == "Economy");

                if (economyClass == null)
                {
                    MessageBox.Show("Không tìm thấy hạng ghế Economy trong database!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int firstRows = 0, businessRows = 0, economyRows = 0;
                int firstCols = 4;  // A B C D
                int businessCols = 4; // A B C D
                int economyCols = 6; // A B C D E F

                // Determine seat configuration based on capacity
                if (capacity < 100)
                {
                    // Small aircraft: Economy only
                    economyRows = (int)Math.Ceiling(capacity / (double)economyCols);
                }
                else if (capacity <= 200)
                {
                    // Medium aircraft: 15% Business, 85% Economy
                    int businessSeats = (int)(capacity * 0.15);
                    businessRows = (int)Math.Ceiling(businessSeats / (double)businessCols);
                    int remainingSeats = capacity - (businessRows * businessCols);
                    economyRows = (int)Math.Ceiling(remainingSeats / (double)economyCols);
                }
                else
                {
                    // Large aircraft: 5% First, 15% Business, 80% Economy
                    int firstSeats = (int)(capacity * 0.05);
                    firstRows = Math.Max(1, (int)Math.Ceiling(firstSeats / (double)firstCols));
                    
                    int businessSeats = (int)(capacity * 0.15);
                    businessRows = (int)Math.Ceiling(businessSeats / (double)businessCols);
                    
                    int remainingSeats = capacity - (firstRows * firstCols) - (businessRows * businessCols);
                    economyRows = (int)Math.Ceiling(remainingSeats / (double)economyCols);
                }

                int currentRow = 1;

                // Generate First class seats
                if (firstRows > 0 && firstClass != null)
                {
                    for (int row = 0; row < firstRows; row++)
                    {
                        for (char col = 'A'; col <= 'D'; col++)
                        {
                            string seatNumber = $"{currentRow}{col}";
                            var seat = new SeatDTO(0, aircraftId, seatNumber, firstClass.ClassId);
                            seatBUS.AddSeat(seat, out _);
                        }
                        currentRow++;
                    }
                }

                // Generate Business class seats
                if (businessRows > 0 && businessClass != null)
                {
                    for (int row = 0; row < businessRows; row++)
                    {
                        for (char col = 'A'; col <= 'D'; col++)
                        {
                            string seatNumber = $"{currentRow}{col}";
                            var seat = new SeatDTO(0, aircraftId, seatNumber, businessClass.ClassId);
                            seatBUS.AddSeat(seat, out _);
                        }
                        currentRow++;
                    }
                }

                // Generate Economy class seats
                for (int row = 0; row < economyRows; row++)
                {
                    for (char col = 'A'; col <= 'F'; col++)
                    {
                        string seatNumber = $"{currentRow}{col}";
                        var seat = new SeatDTO(0, aircraftId, seatNumber, economyClass.ClassId);
                        seatBUS.AddSeat(seat, out _);
                    }
                    currentRow++;
                }

                System.Diagnostics.Debug.WriteLine($"[Auto Seat Generation] Created seats for aircraft {aircraftId}: {firstRows}F + {businessRows}B + {economyRows}E");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo ghế tự động: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
