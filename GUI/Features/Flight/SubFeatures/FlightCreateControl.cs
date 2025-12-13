using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using DTO.Flight;
using DTO.Aircraft;
using DTO.Route;
using DTO.Airport;
using BUS.Flight;
using BUS.Aircraft;
using BUS.Route;
using BUS.Airport;
using GUI.Components.Inputs;
using GUI.Components.Buttons;

namespace GUI.Features.Flight.SubFeatures
{
    public class FlightCreateControl : UserControl
    {
        public event EventHandler? FlightSaved;
        public event EventHandler? FlightSavedUpdated;

        private readonly FlightBUS _flightBus;
        private readonly AircraftBUS _aircraftBus;
        private readonly RouteBUS _routeBus;
        private readonly AirportBUS _airportBus;

        private FlightWithDetailsDTO? _editingFlight;
        private FlightWithDetailsDTO? _originalFlight;
        private bool _isEditMode;
        
        private Dictionary<int, string> _airportCodes = new Dictionary<int, string>();

        // UI Components - Section A: Thông tin chung
        private Label lblTitle = null!;
        private UnderlinedTextField txtFlightNumber = null!;
        private UnderlinedComboBox cbAircraft = null!;
        private UnderlinedComboBox cbRoute = null!;
        private DateTimePicker dtpDepartureTime = null!;
        private DateTimePicker dtpArrivalTime = null!;
        private Label lblDuration = null!;
        private Label lblDurationValue = null!;
        private NumericUpDown nudBasePrice = null!;
        private RichTextBox rtbNote = null!;

        // Section B: Trạng thái
        private Label lblCurrentStatus = null!;
        private Label lblCurrentStatusValue = null!;
        private UnderlinedComboBox cbNewStatus = null!;
        private Label lblStatusWarning = null!;

        // Section C: Hành động
        private PrimaryButton btnSave = null!;
        private SecondaryButton btnCancel = null!;
        private Label lblPriceWarning = null!;

        private List<AircraftDTO> _aircraftList;
        private List<RouteDTO> _routeList;

        public FlightCreateControl()
        {
            _flightBus = FlightBUS.Instance;
            _aircraftBus = new BUS.Aircraft.AircraftBUS();
            _routeBus = new BUS.Route.RouteBUS();
            _airportBus = new BUS.Airport.AirportBUS();
            _aircraftList = new List<AircraftDTO>();
            _routeList = new List<RouteDTO>();
            _isEditMode = false;

            LoadAirportCodes();
            InitializeComponent();
            LoadComboBoxData();
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

        private Label CreateValueLabel(string name)
        {
            return new Label
            {
                Name = name,
                AutoSize = true,
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.FromArgb(60, 60, 60),
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
                Text = "✈️ Tạo chuyến bay mới",
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

            // ===== SECTION A: THÔNG TIN CHUYẾN BAY =====
            var lblSectionA = new Label
            {
                Text = "📋 THÔNG TIN CHUYẾN BAY",
                AutoSize = true,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                Margin = new Padding(0, 0, 0, 12),
                Dock = DockStyle.Top
            };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(lblSectionA, 0, row);
            grid.SetColumnSpan(lblSectionA, 2);
            row++;

            // Flight Number
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var lblFlightNum = CreateKeyLabel("Số hiệu chuyến bay: *");
            grid.Controls.Add(lblFlightNum, 0, row);
            txtFlightNumber = new UnderlinedTextField("", "VD: VN123") { Dock = DockStyle.Fill, Margin = new Padding(0, 8, 0, 8) };
            grid.Controls.Add(txtFlightNumber, 1, row++);

            // Aircraft
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Máy bay: *"), 0, row);
            cbAircraft = new UnderlinedComboBox("", Array.Empty<object>()) { Dock = DockStyle.Fill, Margin = new Padding(0, 8, 0, 8) };
            cbAircraft.Items.Add("-- Chọn máy bay --");
            cbAircraft.SelectedIndex = 0;
            grid.Controls.Add(cbAircraft, 1, row++);

            // Route
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Tuyến bay: *"), 0, row);
            cbRoute = new UnderlinedComboBox("", Array.Empty<object>()) { Dock = DockStyle.Fill, Margin = new Padding(0, 8, 0, 8) };
            cbRoute.Items.Add("-- Chọn tuyến bay --");
            cbRoute.SelectedIndex = 0;
            cbRoute.SelectedIndexChanged += CbRoute_SelectedIndexChanged;
            grid.Controls.Add(cbRoute, 1, row++);

            // Departure Time
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Thời gian khởi hành: *"), 0, row);
            dtpDepartureTime = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 8, 0, 8),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm",
                Font = new Font("Segoe UI", 11f),
                Value = DateTime.Now.AddDays(1)
            };
            dtpDepartureTime.ValueChanged += DtpTime_ValueChanged;
            grid.Controls.Add(dtpDepartureTime, 1, row++);

            // Arrival Time
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Thời gian đến: *"), 0, row);
            dtpArrivalTime = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 8, 0, 8),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm",
                Font = new Font("Segoe UI", 11f),
                Value = DateTime.Now.AddDays(1).AddHours(2)
            };
            dtpArrivalTime.ValueChanged += DtpTime_ValueChanged;
            grid.Controls.Add(dtpArrivalTime, 1, row++);

            // Duration Display
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            lblDuration = CreateKeyLabel("Thời lượng bay:");
            grid.Controls.Add(lblDuration, 0, row);
            lblDurationValue = CreateValueLabel("lblDurationValue");
            lblDurationValue.Text = "2 giờ 0 phút";
            grid.Controls.Add(lblDurationValue, 1, row++);

            // Base Price
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Giá vé cơ bản (VNĐ): *"), 0, row);
            nudBasePrice = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 8, 0, 8),
                Font = new Font("Segoe UI", 11f),
                Minimum = 0,
                Maximum = 999999999,
                Value = 1000000,
                DecimalPlaces = 0,
                ThousandsSeparator = true,
                Increment = 50000
            };
            nudBasePrice.ValueChanged += NudBasePrice_ValueChanged;
            grid.Controls.Add(nudBasePrice, 1, row++);

            // Price Warning Label
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            lblPriceWarning = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(255, 140, 0),
                AutoSize = false,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 8),
                Visible = false
            };
            grid.Controls.Add(lblPriceWarning, 1, row++);

            // Note
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Ghi chú:"), 0, row);
            rtbNote = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 8, 0, 8),
                Font = new Font("Segoe UI", 10f),
                Height = 80,
                MaxLength = 500,
                BorderStyle = BorderStyle.FixedSingle
            };
            grid.Controls.Add(rtbNote, 1, row++);

            // Separator
            var separator1 = new Panel { Height = 2, BackColor = Color.FromArgb(220, 220, 220), Margin = new Padding(0, 16, 0, 16), Dock = DockStyle.Top };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(separator1, 0, row);
            grid.SetColumnSpan(separator1, 2);
            row++;

            // ===== SECTION B: TRẠNG THÁI =====
            var lblSectionB = new Label
            {
                Text = "📊 QUẢN LÝ TRẠNG THÁI",
                AutoSize = true,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                Margin = new Padding(0, 0, 0, 12),
                Dock = DockStyle.Top
            };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(lblSectionB, 0, row);
            grid.SetColumnSpan(lblSectionB, 2);
            row++;

            // Current Status
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            lblCurrentStatus = CreateKeyLabel("Trạng thái hiện tại:");
            grid.Controls.Add(lblCurrentStatus, 0, row);
            lblCurrentStatusValue = CreateValueLabel("lblCurrentStatusValue");
            lblCurrentStatusValue.Text = "Đã lên lịch";
            grid.Controls.Add(lblCurrentStatusValue, 1, row++);

            // New Status
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(CreateKeyLabel("Chọn trạng thái mới:"), 0, row);
            cbNewStatus = new UnderlinedComboBox("", Array.Empty<object>()) { Dock = DockStyle.Fill, Margin = new Padding(0, 8, 0, 8) };
            cbNewStatus.Items.AddRange(new string[] { "-- Giữ nguyên --", "Đã lên lịch", "Bị hoãn", "Đã hủy", "Hoàn thành" });
            cbNewStatus.SelectedIndex = 0;
            cbNewStatus.SelectedIndexChanged += CbNewStatus_SelectedIndexChanged;
            grid.Controls.Add(cbNewStatus, 1, row++);

            // Status Warning
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            lblStatusWarning = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(200, 50, 50),
                AutoSize = false,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 8, 0, 8),
                Visible = false
            };
            grid.Controls.Add(lblStatusWarning, 0, row);
            grid.SetColumnSpan(lblStatusWarning, 2);
            row++;

            // Separator 2
            var separator2 = new Panel { Height = 2, BackColor = Color.FromArgb(220, 220, 220), Margin = new Padding(0, 16, 0, 16), Dock = DockStyle.Top };
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(separator2, 0, row);
            grid.SetColumnSpan(separator2, 2);
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
                Text = "💾 Lưu chuyến bay",
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

        private void LoadAirportCodes()
        {
            try
            {
                var airports = _airportBus.GetAllAirports();
                _airportCodes = airports.ToDictionary(a => a.AirportId, a => a.AirportCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải danh sách sân bay: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxData()
        {
            try
            {
                // Load Aircraft
                _aircraftList = _aircraftBus.GetAllAircrafts();
                cbAircraft.Items.Clear();
                cbAircraft.Items.Add("-- Chọn máy bay --");
                foreach (var aircraft in _aircraftList)
                {
                    string display = aircraft.Model ?? "Unknown Model";
                    if (aircraft.Capacity.HasValue)
                        display += $" ({aircraft.Capacity} ghế)";
                    cbAircraft.Items.Add(display);
                }
                cbAircraft.SelectedIndex = 0;

                // Load Route
                _routeList = _routeBus.GetAllRoutes();
                cbRoute.Items.Clear();
                cbRoute.Items.Add("-- Chọn tuyến bay --");
                foreach (var route in _routeList)
                {
                    string depCode = _airportCodes.ContainsKey(route.DeparturePlaceId) 
                        ? _airportCodes[route.DeparturePlaceId] 
                        : $"ID{route.DeparturePlaceId}";
                    string arrCode = _airportCodes.ContainsKey(route.ArrivalPlaceId) 
                        ? _airportCodes[route.ArrivalPlaceId] 
                        : $"ID{route.ArrivalPlaceId}";
                    string distance = route.DistanceKm.HasValue ? $"{route.DistanceKm}km" : "N/A";
                    cbRoute.Items.Add($"{depCode} → {arrCode} ({distance})");
                }
                cbRoute.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadForEdit(FlightWithDetailsDTO dto)
        {
            _isEditMode = true;
            _editingFlight = dto;
            _originalFlight = new FlightWithDetailsDTO
            {
                FlightId = dto.FlightId,
                FlightNumber = dto.FlightNumber,
                AircraftId = dto.AircraftId,
                RouteId = dto.RouteId,
                DepartureTime = dto.DepartureTime,
                ArrivalTime = dto.ArrivalTime,
                BasePrice = dto.BasePrice,
                Note = dto.Note,
                Status = dto.Status
            };

            lblTitle.Text = $"✏️ Chỉnh sửa chuyến bay {dto.FlightNumber}";
            btnSave.Text = "💾 Cập nhật";

            // Load data
            txtFlightNumber.Text = dto.FlightNumber;
            nudBasePrice.Value = dto.BasePrice;
            rtbNote.Text = dto.Note ?? "";

            // Select Aircraft
            var aircraftIndex = _aircraftList.FindIndex(a => a.AircraftId == dto.AircraftId);
            if (aircraftIndex >= 0)
                cbAircraft.SelectedIndex = aircraftIndex + 1;

            // Select Route
            var routeIndex = _routeList.FindIndex(r => r.RouteId == dto.RouteId);
            if (routeIndex >= 0)
                cbRoute.SelectedIndex = routeIndex + 1;

            if (dto.DepartureTime.HasValue)
                dtpDepartureTime.Value = dto.DepartureTime.Value;

            if (dto.ArrivalTime.HasValue)
                dtpArrivalTime.Value = dto.ArrivalTime.Value;

            // Status
            lblCurrentStatusValue.Text = dto.Status.GetDescription();
            UpdateStatusColor(dto.Status);

            // Check if can edit
            if (dto.Status == FlightStatus.COMPLETED)
            {
                DisableAllInputs();
                MessageBox.Show("Chuyến bay đã hoàn thành, không thể chỉnh sửa!", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (dto.DepartureTime.HasValue && (dto.DepartureTime.Value - DateTime.Now).TotalHours < 2)
            {
                MessageBox.Show("Chuyến bay sắp cất cánh (< 2 giờ), một số thông tin có thể không được thay đổi!", 
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            UpdateDuration();
        }

        private void DisableAllInputs()
        {
            txtFlightNumber.Enabled = false;
            cbAircraft.Enabled = false;
            cbRoute.Enabled = false;
            dtpDepartureTime.Enabled = false;
            dtpArrivalTime.Enabled = false;
            cbNewStatus.Enabled = false;
            btnSave.Enabled = false;
        }

        private void CbRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRoute.SelectedIndex > 0)
            {
                var route = _routeList[cbRoute.SelectedIndex - 1];
                // Auto update arrival time based on route duration
                if (route.DurationMinutes.HasValue)
                {
                    dtpArrivalTime.Value = dtpDepartureTime.Value.AddMinutes(route.DurationMinutes.Value);
                }
            }
        }

        private void DtpTime_ValueChanged(object? sender, EventArgs e)
        {
            UpdateDuration();
        }

        private void UpdateDuration()
        {
            var duration = dtpArrivalTime.Value - dtpDepartureTime.Value;
            if (duration.TotalMinutes > 0)
            {
                int hours = (int)duration.TotalHours;
                int minutes = duration.Minutes;
                lblDurationValue.Text = $"{hours} giờ {minutes} phút";
                lblDurationValue.ForeColor = Color.FromArgb(0, 120, 212);
            }
            else
            {
                lblDurationValue.Text = "Không hợp lệ!";
                lblDurationValue.ForeColor = Color.Red;
            }
        }

        private void NudBasePrice_ValueChanged(object sender, EventArgs e)
        {
            decimal price = nudBasePrice.Value;
            const decimal MIN_RECOMMENDED = 500000m;

            if (price < MIN_RECOMMENDED && price > 0)
            {
                lblPriceWarning.Text = $"⚠️ Giá quá thấp so với thị trường (< {MIN_RECOMMENDED:N0} VNĐ)";
                lblPriceWarning.Visible = true;
            }
            else
            {
                lblPriceWarning.Visible = false;
            }
        }

        private void CbNewStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStatusWarning.Visible = false;

            if (!_isEditMode || cbNewStatus.SelectedIndex == 0)
                return;

            var currentStatus = _editingFlight!.Status;
            var targetStatus = GetStatusFromComboBox(cbNewStatus.SelectedIndex);

            if (!currentStatus.CanTransitionTo(targetStatus))
            {
                lblStatusWarning.Text = $"⚠️ Không thể chuyển từ '{currentStatus.GetDescription()}' sang '{targetStatus.GetDescription()}'!";
                lblStatusWarning.Visible = true;
            }
        }

        private FlightStatus GetStatusFromComboBox(int index)
        {
            return index switch
            {
                1 => FlightStatus.SCHEDULED,
                2 => FlightStatus.DELAYED,
                3 => FlightStatus.CANCELLED,
                4 => FlightStatus.COMPLETED,
                _ => FlightStatus.SCHEDULED
            };
        }

        private void UpdateStatusColor(FlightStatus status)
        {
            lblCurrentStatusValue.ForeColor = status switch
            {
                FlightStatus.SCHEDULED => Color.FromArgb(0, 120, 212),
                FlightStatus.DELAYED => Color.FromArgb(255, 140, 0),
                FlightStatus.CANCELLED => Color.FromArgb(200, 50, 50),
                FlightStatus.COMPLETED => Color.FromArgb(0, 150, 0),
                _ => Color.Black
            };
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (!ValidateInputs(out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get data
                var flightNumber = txtFlightNumber.Text.Trim().ToUpper();
                
                // Safe get aircraft ID
                if (cbAircraft.SelectedIndex <= 0 || cbAircraft.SelectedIndex > _aircraftList.Count)
                {
                    MessageBox.Show("Vui lòng chọn máy bay hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var aircraftId = _aircraftList[cbAircraft.SelectedIndex - 1].AircraftId;
                
                // Safe get route ID
                if (cbRoute.SelectedIndex <= 0 || cbRoute.SelectedIndex > _routeList.Count)
                {
                    MessageBox.Show("Vui lòng chọn tuyến bay hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var routeId = _routeList[cbRoute.SelectedIndex - 1].RouteId;
                
                var departureTime = dtpDepartureTime.Value;
                var arrivalTime = dtpArrivalTime.Value;

                // Status validation for edit mode
                FlightStatus newStatus = _isEditMode ? _editingFlight!.Status : FlightStatus.SCHEDULED;
                if (_isEditMode && cbNewStatus.SelectedIndex > 0)
                {
                    newStatus = GetStatusFromComboBox(cbNewStatus.SelectedIndex);
                    if (!_editingFlight!.Status.CanTransitionTo(newStatus))
                    {
                        MessageBox.Show($"Không thể chuyển trạng thái từ '{_editingFlight.Status.GetDescription()}' sang '{newStatus.GetDescription()}'!", 
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Check time constraint for edit
                if (_isEditMode && _editingFlight!.DepartureTime.HasValue)
                {
                    var timeToDeparture = _editingFlight.DepartureTime.Value - DateTime.Now;
                    if (timeToDeparture.TotalHours < 2 && 
                        (departureTime != _originalFlight!.DepartureTime || arrivalTime != _originalFlight.ArrivalTime))
                    {
                        MessageBox.Show("Không thể thay đổi giờ bay khi chuyến bay cất cánh trong vòng 2 giờ!", 
                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Check for sold tickets (simplified - you may need to implement this in BUS)
                if (_isEditMode && HasChangedTime())
                {
                    var result = MessageBox.Show(
                        "Thay đổi giờ bay có thể ảnh hưởng đến hành khách đã đặt vé.\n\nBạn có chắc chắn muốn tiếp tục?",
                        "Xác nhận thay đổi",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                        return;
                }

                // Show confirmation dialog
                if (!ShowConfirmationDialog(flightNumber, departureTime, arrivalTime, newStatus))
                    return;

                // Save
                bool success;
                string message;
                List<string> warnings;

                // Get BasePrice and Note from UI
                decimal basePrice = nudBasePrice.Value;
                string? note = string.IsNullOrWhiteSpace(rtbNote.Text) ? null : rtbNote.Text.Trim();

                if (_isEditMode)
                {
                    var flight = new FlightDTO(
                        _editingFlight!.FlightId,
                        flightNumber,
                        aircraftId,
                        routeId,
                        departureTime,
                        arrivalTime,
                        basePrice,
                        note,
                        newStatus
                    );
                    success = _flightBus.UpdateFlight(flight, out message, out warnings);
                }
                else
                {
                    var flight = new FlightDTO(
                        flightNumber,
                        aircraftId,
                        routeId,
                        departureTime,
                        arrivalTime,
                        basePrice,
                        note
                    );
                    success = _flightBus.CreateFlight(flight, out message, out warnings);
                }

                if (success)
                {
                    // Show warnings if any
                    string successMsg = message;
                    if (warnings != null && warnings.Count > 0)
                    {
                        successMsg += "\n\n⚠️ Cảnh báo:\n" + string.Join("\n", warnings);
                    }

                    MessageBox.Show(successMsg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    if (_isEditMode)
                        FlightSavedUpdated?.Invoke(this, EventArgs.Empty);
                    else
                        FlightSaved?.Invoke(this, EventArgs.Empty);

                    ClearForm();
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu chuyến bay: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs(out string errorMessage)
        {
            errorMessage = string.Empty;

            // Basic null checks
            if (string.IsNullOrWhiteSpace(txtFlightNumber.Text))
            {
                errorMessage = "Vui lòng nhập số hiệu chuyến bay!";
                return false;
            }

            if (cbAircraft.SelectedIndex <= 0)
            {
                errorMessage = "Vui lòng chọn máy bay!";
                return false;
            }

            if (cbRoute.SelectedIndex <= 0)
            {
                errorMessage = "Vui lòng chọn tuyến bay!";
                return false;
            }

            // Validate using FlightValidationHelper
            string flightCode = txtFlightNumber.Text.Trim().ToUpper();

            // 1. Flight code format
            if (!BUS.Flight.FlightValidationHelper.IsValidFlightCode(flightCode, out string codeError))
            {
                errorMessage = codeError;
                return false;
            }

            // 2. Departure time
            if (!BUS.Flight.FlightValidationHelper.IsValidDepartureTime(dtpDepartureTime.Value, out string depError))
            {
                errorMessage = depError;
                return false;
            }

            // 3. Arrival time
            if (!BUS.Flight.FlightValidationHelper.IsValidArrivalTime(dtpDepartureTime.Value, dtpArrivalTime.Value, out string arrError))
            {
                errorMessage = arrError;
                return false;
            }

            // 4. Base price
            if (!BUS.Flight.FlightValidationHelper.IsValidBasePrice(nudBasePrice.Value, out string priceError, out _))
            {
                errorMessage = priceError;
                return false;
            }

            var route = _routeList[cbRoute.SelectedIndex - 1];
            if (route.DeparturePlaceId == route.ArrivalPlaceId)
            {
                errorMessage = "Sân bay đi và sân bay đến không được trùng nhau!";
                return false;
            }

            return true;
        }

        private bool HasChangedTime()
        {
            if (!_isEditMode || _originalFlight == null)
                return false;

            return dtpDepartureTime.Value != _originalFlight.DepartureTime ||
                   dtpArrivalTime.Value != _originalFlight.ArrivalTime;
        }

        private bool ShowConfirmationDialog(string flightNumber, DateTime departure, DateTime arrival, FlightStatus status)
        {
            var duration = arrival - departure;
            var message = _isEditMode ? "XÁC NHẬN CẬP NHẬT CHUYẾN BAY\n\n" : "XÁC NHẬN TẠO CHUYẾN BAY MỚI\n\n";

            message += $"Số hiệu: {flightNumber}\n";
            message += $"Máy bay: {cbAircraft.SelectedItem}\n";
            message += $"Tuyến: {cbRoute.SelectedItem}\n";
            message += $"Khởi hành: {departure:dd/MM/yyyy HH:mm}\n";
            message += $"Đến: {arrival:dd/MM/yyyy HH:mm}\n";
            message += $"Thời lượng: {(int)duration.TotalHours}h {duration.Minutes}m\n";
            message += $"Giá vé cơ bản: {nudBasePrice.Value:N0} VNĐ\n";
            if (!string.IsNullOrWhiteSpace(rtbNote.Text))
                message += $"Ghi chú: {rtbNote.Text.Substring(0, Math.Min(50, rtbNote.Text.Length))}...\n";
            message += $"Trạng thái: {status.GetDescription()}\n\n";

            if (_isEditMode && _originalFlight != null)
            {
                message += "THAY ĐỔI:\n";
                if (txtFlightNumber.Text != _originalFlight.FlightNumber)
                    message += $"• Số hiệu: {_originalFlight.FlightNumber} → {flightNumber}\n";
                if (departure != _originalFlight.DepartureTime)
                    message += $"• Giờ đi: {_originalFlight.DepartureTime:HH:mm} → {departure:HH:mm}\n";
                if (arrival != _originalFlight.ArrivalTime)
                    message += $"• Giờ đến: {_originalFlight.ArrivalTime:HH:mm} → {arrival:HH:mm}\n";
                if (status != _originalFlight.Status)
                    message += $"• Trạng thái: {_originalFlight.Status.GetDescription()} → {status.GetDescription()}\n";
            }

            message += "\nBạn có chắc chắn muốn tiếp tục?";

            var result = MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn hủy bỏ?", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClearForm();
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (_isEditMode && _originalFlight != null)
            {
                LoadForEdit(_originalFlight);
            }
            else
            {
                ClearForm();
            }
        }

        private void ClearForm()
        {
            _isEditMode = false;
            _editingFlight = null;
            _originalFlight = null;

            lblTitle.Text = "✈️ Tạo chuyến bay mới";
            btnSave.Text = "💾 Lưu chuyến bay";

            txtFlightNumber.Text = "";
            cbAircraft.SelectedIndex = 0;
            cbRoute.SelectedIndex = 0;
            dtpDepartureTime.Value = DateTime.Now.AddDays(1);
            dtpArrivalTime.Value = DateTime.Now.AddDays(1).AddHours(2);
            nudBasePrice.Value = 1000000m;
            rtbNote.Text = "";
            cbNewStatus.SelectedIndex = 0;

            lblCurrentStatusValue.Text = "Đã lên lịch";
            lblCurrentStatusValue.ForeColor = Color.FromArgb(0, 120, 212);
            lblStatusWarning.Visible = false;
            lblPriceWarning.Visible = false;

            txtFlightNumber.Enabled = true;
            cbAircraft.Enabled = true;
            cbRoute.Enabled = true;
            dtpDepartureTime.Enabled = true;
            dtpArrivalTime.Enabled = true;
            cbNewStatus.Enabled = true;
            btnSave.Enabled = true;

            UpdateDuration();
        }
    }
}
