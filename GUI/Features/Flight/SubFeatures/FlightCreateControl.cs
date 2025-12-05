using GUI.Components.Buttons;
using GUI.Components.Inputs;
using BUS.Flight;
using DTO.Flight;
using System.Data;

namespace GUI.Features.Flight.SubFeatures {
    public class FlightCreateControl : UserControl {
        private readonly FlightBUS _flightBUS;
        
        // Form controls
        private UnderlinedTextField txtFlightNumber = null!;
        private UnderlinedComboBox cbRoute = null!;
        private UnderlinedComboBox cbAircraft = null!;
        private DateTimePickerCustom dtpDeparture = null!;
        private DateTimePickerCustom dtpArrival = null!;
        private UnderlinedComboBox cbStatus = null!;
        private Label lblDuration = null!;
        private Label lblAircraftInfo = null!;
        private Label lblRouteInfo = null!;
        private PrimaryButton btnCreate = null!;
        private SecondaryButton btnCancel = null!;

        public event EventHandler? FlightCreated;
        private int? _editingFlightId;

        public FlightCreateControl() {
            _flightBUS = FlightBUS.Instance;
            InitializeComponent();
            LoadData();
            AttachEvents();
        }

        private void NavigateBackToList() {
            // Find parent FlightControl and switch to list tab
            var parent = this.Parent;
            while (parent != null && !(parent is FlightControl)) {
                parent = parent.Parent;
            }
            
            if (parent is FlightControl flightControl) {
                // Reload list and switch to it
                var listControl = FindControl<FlightListControl>(flightControl);
                if (listControl != null) {
                    listControl.LoadFlights();
                }
                
                // Use reflection to call SwitchTab(0)
                var switchTabMethod = flightControl.GetType().GetMethod("SwitchTab", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                switchTabMethod?.Invoke(flightControl, new object[] { 0 });
            }
        }

        private T? FindControl<T>(Control parent) where T : Control {
            foreach (Control control in parent.Controls) {
                if (control is T target) {
                    return target;
                }
                var result = FindControl<T>(control);
                if (result != null) {
                    return result;
                }
            }
            return null;
        }

        private void InitializeComponent() {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(240, 244, 248);
            this.AutoScroll = true;

            // ===== Layout =====
            var layoutPanel = new TableLayoutPanel {
                Dock = DockStyle.Top,
                ColumnCount = 1,
                RowCount = 2,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(30)
            };
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            
            // ===== Header =====
            var headerPanel = CreateHeaderPanel();
            layoutPanel.Controls.Add(headerPanel, 0, 0);

            // ===== Form Card =====
            var formCard = CreateFormCard();
            layoutPanel.Controls.Add(formCard, 0, 1);

            this.Controls.Add(layoutPanel);
        }

        private Panel CreateHeaderPanel() {
            var panel = new Panel {
                Height = 110,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent
            };

            var lblTitle = new Label {
                Text = "✈️ Tạo chuyến bay mới",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                AutoSize = true,
                Location = new Point(0, 10)
            };

            var lblSubtitle = new Label {
                Text = "Nhập thông tin chi tiết để thêm chuyến bay mới vào hệ thống",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(108, 117, 125),
                AutoSize = true,
                Location = new Point(0, 65)
            };

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblSubtitle);

            return panel;
        }

        private Panel CreateFormCard() {
            var card = new Panel {
                BackColor = Color.White,
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(30)
            };

            // Shadow effect
            card.Paint += (s, e) => {
                var rect = card.ClientRectangle;
                using (var path = new System.Drawing.Drawing2D.GraphicsPath()) {
                    int radius = 8;
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                    path.CloseFigure();
                    card.Region = new Region(path);
                }
            };

            var formLayout = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Basic Info
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Route & Aircraft
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Time
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Status
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Buttons

            // 1. Basic Information Section
            var basicInfoSection = CreateBasicInfoSection();
            formLayout.Controls.Add(basicInfoSection, 0, 0);

            // 2. Route & Aircraft Section
            var routeAircraftSection = CreateRouteAircraftSection();
            formLayout.Controls.Add(routeAircraftSection, 0, 1);

            // 3. Time Section
            var timeSection = CreateTimeSection();
            formLayout.Controls.Add(timeSection, 0, 2);

            // 4. Status Section
            var statusSection = CreateStatusSection();
            formLayout.Controls.Add(statusSection, 0, 3);

            // 5. Action Buttons
            var buttonPanel = CreateButtonPanel();
            formLayout.Controls.Add(buttonPanel, 0, 4);

            card.Controls.Add(formLayout);
            return card;
        }

        private Panel CreateBasicInfoSection() {
            var section = new Panel {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0, 0, 0, 20)
            };

            var lblSection = new Label {
                Text = "📋 Thông tin cơ bản",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            txtFlightNumber = new UnderlinedTextField("Số hiệu chuyến bay *", "") {
                Width = 400,
                Location = new Point(0, 40)
            };

            section.Controls.Add(lblSection);
            section.Controls.Add(txtFlightNumber);
            section.Height = 120;

            return section;
        }

        private Panel CreateRouteAircraftSection() {
            var section = new Panel {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0, 0, 0, 20)
            };

            var lblSection = new Label {
                Text = "🛫 Tuyến bay & Máy bay",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            var fieldsLayout = new TableLayoutPanel {
                ColumnCount = 2,
                RowCount = 3,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Location = new Point(0, 40),
                Width = 900
            };
            fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            cbRoute = new UnderlinedComboBox("Tuyến bay *", new object[] { }) {
                Width = 400,
                Margin = new Padding(0, 0, 20, 10)
            };

            cbAircraft = new UnderlinedComboBox("Máy bay *", new object[] { }) {
                Width = 400,
                Margin = new Padding(20, 0, 0, 10)
            };

            lblRouteInfo = new Label {
                Text = "Chọn tuyến bay để xem thông tin chi tiết",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                AutoSize = true,
                Margin = new Padding(0, 5, 20, 0)
            };

            lblAircraftInfo = new Label {
                Text = "Chọn máy bay để xem thông tin chi tiết",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                AutoSize = true,
                Margin = new Padding(20, 5, 0, 0)
            };

            fieldsLayout.Controls.Add(cbRoute, 0, 0);
            fieldsLayout.Controls.Add(cbAircraft, 1, 0);
            fieldsLayout.Controls.Add(lblRouteInfo, 0, 1);
            fieldsLayout.Controls.Add(lblAircraftInfo, 1, 1);

            section.Controls.Add(lblSection);
            section.Controls.Add(fieldsLayout);
            section.Height = 180;

            return section;
        }

        private Panel CreateTimeSection() {
            var section = new Panel {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0, 0, 0, 20)
            };

            var lblSection = new Label {
                Text = "🕐 Thời gian chuyến bay",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            var fieldsLayout = new TableLayoutPanel {
                ColumnCount = 2,
                RowCount = 2,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Location = new Point(0, 40),
                Width = 900
            };
            fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            dtpDeparture = new DateTimePickerCustom("Thời gian khởi hành *", "") {
                Width = 400,
                EnableTime = true,
                TimeFormat = "dd/MM/yyyy HH:mm",
                ShowUpDownWhenTime = true,
                Margin = new Padding(0, 0, 20, 10)
            };

            dtpArrival = new DateTimePickerCustom("Thời gian hạ cánh *", "") {
                Width = 400,
                EnableTime = true,
                TimeFormat = "dd/MM/yyyy HH:mm",
                ShowUpDownWhenTime = true,
                Margin = new Padding(20, 0, 0, 10)
            };

            lblDuration = new Label {
                Text = "⏱️ Thời gian bay: --",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(13, 110, 253),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0)
            };

            fieldsLayout.Controls.Add(dtpDeparture, 0, 0);
            fieldsLayout.Controls.Add(dtpArrival, 1, 0);
            fieldsLayout.SetColumnSpan(lblDuration, 2);
            fieldsLayout.Controls.Add(lblDuration, 0, 1);

            section.Controls.Add(lblSection);
            section.Controls.Add(fieldsLayout);
            section.Height = 180;

            return section;
        }

        private Panel CreateStatusSection() {
            var section = new Panel {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0, 0, 0, 20)
            };

            var lblSection = new Label {
                Text = "📊 Trạng thái",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 58, 64),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            cbStatus = new UnderlinedComboBox("Trạng thái chuyến bay *", 
                Enum.GetValues(typeof(FlightStatus)).Cast<object>().ToArray()) {
                Width = 400,
                Location = new Point(0, 40)
            };
            cbStatus.SelectedIndex = 0; // Default: SCHEDULED

            section.Controls.Add(lblSection);
            section.Controls.Add(cbStatus);
            section.Height = 140;

            return section;
        }

        private Panel CreateButtonPanel() {
            var panel = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(0, 20, 0, 0),
                WrapContents = false
            };

            btnCreate = new PrimaryButton("✓ Tạo chuyến bay") {
                Width = 160,
                Height = 45,
                Margin = new Padding(0, 0, 15, 0)
            };

            btnCancel = new SecondaryButton("✕ Hủy") {
                Width = 120,
                Height = 45
            };

            panel.Controls.Add(btnCreate);
            panel.Controls.Add(btnCancel);

            return panel;
        }

        private void LoadData() {
            try {
                // Load Routes
                var routeDAO = new DAO.Route.RouteDAO();
                var routes = routeDAO.GetAllRoutes();
                cbRoute.Items.Clear();
                foreach (var route in routes) {
                    cbRoute.Items.Add(new ComboBoxItem {
                        Text = $"Route {route.RouteId}: {route.DeparturePlaceId} → {route.ArrivalPlaceId}",
                        Value = route.RouteId
                    });
                }

                // Load Aircraft
                var aircraftDAO = new DAO.Aircraft.AircraftDAO();
                var aircrafts = aircraftDAO.GetAllAircrafts();
                cbAircraft.Items.Clear();
                foreach (var aircraft in aircrafts) {
                    cbAircraft.Items.Add(new ComboBoxItem {
                        Text = $"{aircraft.Model} ({aircraft.Manufacturer}) - Sức chứa: {aircraft.Capacity}",
                        Value = aircraft.AircraftId
                    });
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AttachEvents() {
            dtpDeparture.ValueChanged += (s, e) => CalculateDuration();
            dtpArrival.ValueChanged += (s, e) => CalculateDuration();
            
            cbRoute.SelectedIndexChanged += (s, e) => UpdateRouteInfo();
            cbAircraft.SelectedIndexChanged += (s, e) => UpdateAircraftInfo();

            btnCreate.Click += BtnCreate_Click;
            btnCancel.Click += (s, e) => ClearForm();
        }

        private void CalculateDuration() {
            try {
                var departure = dtpDeparture.Value;
                var arrival = dtpArrival.Value;

                if (arrival > departure) {
                    var duration = arrival - departure;
                    lblDuration.Text = $"⏱️ Thời gian bay: {duration.Hours}h {duration.Minutes}m";
                    lblDuration.ForeColor = Color.FromArgb(25, 135, 84);
                } else {
                    lblDuration.Text = "⚠️ Thời gian hạ cánh phải sau thời gian khởi hành!";
                    lblDuration.ForeColor = Color.FromArgb(220, 53, 69);
                }
            }
            catch {
                lblDuration.Text = "⏱️ Thời gian bay: --";
                lblDuration.ForeColor = Color.FromArgb(108, 117, 125);
            }
        }

        private void UpdateRouteInfo() {
            if (cbRoute.SelectedItem is ComboBoxItem item && item.Value != null) {
                try {
                    var routeDAO = new DAO.Route.RouteDAO();
                    var allRoutes = routeDAO.GetAllRoutes();
                    var route = allRoutes.FirstOrDefault(r => r.RouteId == (int)item.Value);
                    if (route != null) {
                        lblRouteInfo.Text = $"📍 Khoảng cách: {route.DistanceKm} km | " +
                                          $"⏱️ Thời gian dự kiến: {route.DurationMinutes} phút";
                        lblRouteInfo.ForeColor = Color.FromArgb(25, 135, 84);
                    }
                }
                catch (Exception ex) {
                    lblRouteInfo.Text = $"⚠️ Lỗi: {ex.Message}";
                    lblRouteInfo.ForeColor = Color.FromArgb(220, 53, 69);
                }
            }
        }

        private void UpdateAircraftInfo() {
            if (cbAircraft.SelectedItem is ComboBoxItem item && item.Value != null) {
                try {
                    var aircraftDAO = new DAO.Aircraft.AircraftDAO();
                    var allAircrafts = aircraftDAO.GetAllAircrafts();
                    var aircraft = allAircrafts.FirstOrDefault(a => a.AircraftId == (int)item.Value);
                    if (aircraft != null) {
                        lblAircraftInfo.Text = $"✈️ Máy bay: {aircraft.Model} | " +
                                             $"👥 Sức chứa: {aircraft.Capacity} hành khách";
                        lblAircraftInfo.ForeColor = Color.FromArgb(25, 135, 84);
                    }
                }
                catch (Exception ex) {
                    lblAircraftInfo.Text = $"⚠️ Lỗi: {ex.Message}";
                    lblAircraftInfo.ForeColor = Color.FromArgb(220, 53, 69);
                }
            }
        }

        private void BtnCreate_Click(object? sender, EventArgs e) {
            try {
                // Validation
                if (string.IsNullOrWhiteSpace(txtFlightNumber.Text)) {
                    MessageBox.Show("Vui lòng nhập số hiệu chuyến bay!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFlightNumber.Focus();
                    return;
                }

                if (cbRoute.SelectedItem == null) {
                    MessageBox.Show("Vui lòng chọn tuyến bay!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbAircraft.SelectedItem == null) {
                    MessageBox.Show("Vui lòng chọn máy bay!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dtpArrival.Value <= dtpDeparture.Value) {
                    MessageBox.Show("Thời gian hạ cánh phải sau thời gian khởi hành!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create/Update FlightDTO
                var routeItem = (ComboBoxItem)cbRoute.SelectedItem;
                var aircraftItem = (ComboBoxItem)cbAircraft.SelectedItem;
                var status = (FlightStatus)cbStatus.SelectedItem!;

                FlightDTO flight;
                if (_editingFlightId.HasValue) {
                    // Update mode
                    flight = new FlightDTO(
                        _editingFlightId.Value,
                        txtFlightNumber.Text.Trim(),
                        (int)aircraftItem.Value!,
                        (int)routeItem.Value!,
                        dtpDeparture.Value,
                        dtpArrival.Value,
                        status
                    );
                } else {
                    // Create mode
                    flight = new FlightDTO(
                        txtFlightNumber.Text.Trim(),
                        (int)aircraftItem.Value!,
                        (int)routeItem.Value!,
                        dtpDeparture.Value,
                        dtpArrival.Value
                    );
                }
                flight.Status = status;

                // Validate business rules
                string validationError;
                if (!flight.IsValid(out validationError)) {
                    MessageBox.Show(validationError, "Dữ liệu không hợp lệ",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Save to database
                string busMessage;
                bool success;
                
                if (_editingFlightId.HasValue) {
                    success = _flightBUS.UpdateFlight(flight, out busMessage);
                    if (success) {
                        MessageBox.Show("✓ Cập nhật chuyến bay thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Navigate back to list after successful update
                        ClearForm();
                        NavigateBackToList();
                    }
                } else {
                    success = _flightBUS.CreateFlight(flight, out busMessage);
                    if (success) {
                        MessageBox.Show("✓ Tạo chuyến bay thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        FlightCreated?.Invoke(this, EventArgs.Empty);
                    }
                }

                if (!success) {
                    MessageBox.Show($"Không thể lưu chuyến bay:\n{busMessage}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadFlight(FlightDTO? flight) {
            if (flight == null) {
                _editingFlightId = null;
                ClearForm();
                btnCreate.Text = "Tạo chuyến bay";
                return;
            }

            _editingFlightId = flight.FlightId;
            txtFlightNumber.Text = flight.FlightNumber;
            
            // Set route
            for (int i = 0; i < cbRoute.Items.Count; i++) {
                if (cbRoute.Items[i] is ComboBoxItem item && (int)item.Value! == flight.RouteId) {
                    cbRoute.SelectedIndex = i;
                    break;
                }
            }
            
            // Set aircraft
            for (int i = 0; i < cbAircraft.Items.Count; i++) {
                if (cbAircraft.Items[i] is ComboBoxItem item && (int)item.Value! == flight.AircraftId) {
                    cbAircraft.SelectedIndex = i;
                    break;
                }
            }
            
            dtpDeparture.Value = flight.DepartureTime ?? DateTime.Now;
            dtpArrival.Value = flight.ArrivalTime ?? DateTime.Now.AddHours(2);
            
            // Set status
            for (int i = 0; i < cbStatus.Items.Count; i++) {
                if (cbStatus.Items[i] is FlightStatus statusItem && statusItem == flight.Status) {
                    cbStatus.SelectedIndex = i;
                    break;
                }
            }
            
            btnCreate.Text = "Cập nhật chuyến bay";
        }

        private void ClearForm() {
            _editingFlightId = null;
            txtFlightNumber.Text = "";
            cbRoute.SelectedIndex = -1;
            cbAircraft.SelectedIndex = -1;
            dtpDeparture.Value = DateTime.Now;
            dtpArrival.Value = DateTime.Now.AddHours(2);
            cbStatus.SelectedIndex = 0;
            lblDuration.Text = "⏱️ Thời gian bay: --";
            lblRouteInfo.Text = "Chọn tuyến bay để xem thông tin chi tiết";
            lblAircraftInfo.Text = "Chọn máy bay để xem thông tin chi tiết";
            btnCreate.Text = "Tạo chuyến bay";
        }

        // Helper class for ComboBox items
        private class ComboBoxItem {
            public string Text { get; set; } = "";
            public object? Value { get; set; }
            public override string ToString() => Text;
        }
    }
}
