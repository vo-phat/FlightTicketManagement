<<<<<<< Updated upstream
﻿// TRONG FILE: GUI/Features/Flight/SubFeatures/FlightDetailControl.cs
// THAY THẾ TOÀN BỘ NỘI DUNG FILE BẰNG MÃ NÀY:

using BUS.Flight;
using DTO.Flight;
using GUI.Components.Buttons;
=======
>>>>>>> Stashed changes
using System;
using System.Drawing;
using System.Windows.Forms;
using BUS.Flight;
using DTO.Flight;

<<<<<<< Updated upstream
namespace GUI.Features.Flight.SubFeatures
{
    public partial class FlightDetailControl : UserControl
    {
        public event Action OnBackToListRequested;
        public event Action<int> OnEditRequested;

        private int _currentFlightId;
        private Label lblTitle;
        private Label vFlightNumber, vRoute, vAircraft, vDeparture, vArrival, vStatus, vSeats;
        private Button btnBack;

        public FlightDetailControl()
        {
=======
namespace GUI.Features.Flight.SubFeatures {
    public class FlightDetailControl : UserControl {
        private readonly FlightBUS _flightBUS;

        private TableLayoutPanel main;
        private Label lblTitle;
        private Panel card;
        private TableLayoutPanel grid;

        // Labels to display values
        private Label valueFlightId, valueFlightNumber, valueAircraftId, valueRouteId,
                      valueDepartureTime, valueArrivalTime, valueStatus;

        private int _currentFlightId = 0;
        public event Action? DataChanged;

        public FlightDetailControl() {
            _flightBUS = new FlightBUS();
>>>>>>> Stashed changes
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.BackColor = Color.FromArgb(232, 240, 252);
            this.Dock = DockStyle.Fill;

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Title
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Back Button
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Content
            this.Controls.Add(mainLayout);

            lblTitle = new Label
            {
                Text = "✈️ Chi tiết chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };
            mainLayout.Controls.Add(lblTitle, 0, 0);

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 12, 24, 0)
            };
            btnBack = new SecondaryButton("⬅️ Quay lại danh sách");
            btnBack.Click += (s, e) => OnBackToListRequested?.Invoke();
            buttonPanel.Controls.Add(btnBack);
            
            var btnEdit = new PrimaryButton("✏️ Chỉnh sửa") { Margin = new Padding(8, 0, 0, 0) };
            btnEdit.Click += (s, e) => OnEditRequested?.Invoke(_currentFlightId);
            buttonPanel.Controls.Add(btnEdit);
            
            mainLayout.Controls.Add(buttonPanel, 0, 1);

            var card = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(16),
                Margin = new Padding(24, 8, 24, 24),
                Dock = DockStyle.Fill
            };
            mainLayout.Controls.Add(card, 0, 2);

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            card.Controls.Add(grid);

<<<<<<< Updated upstream
            Label Key(string t) => new Label { Text = t, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Margin = new Padding(0, 6, 12, 6) };
            Label Val() => new Label { AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Regular), Margin = new Padding(0, 6, 0, 6) };

            int r = 0;
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Số hiệu:"), 0, r); vFlightNumber = Val(); grid.Controls.Add(vFlightNumber, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Tuyến bay:"), 0, r); vRoute = Val(); grid.Controls.Add(vRoute, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Máy bay:"), 0, r); vAircraft = Val(); grid.Controls.Add(vAircraft, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Khởi hành:"), 0, r); vDeparture = Val(); grid.Controls.Add(vDeparture, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hạ cánh:"), 0, r); vArrival = Val(); grid.Controls.Add(vArrival, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Trạng thái:"), 0, r); vStatus = Val(); grid.Controls.Add(vStatus, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Ghế trống:"), 0, r); vSeats = Val(); grid.Controls.Add(vSeats, 1, r++);
        }

        public void LoadFlightDetails(int flightId)
        {
            _currentFlightId = flightId;
            
            // Reset dữ liệu cũ
            vFlightNumber.Text = vRoute.Text = vAircraft.Text = vDeparture.Text = vArrival.Text = vStatus.Text = vSeats.Text = "(Đang tải...)";

            // Gọi BUS để lấy FlightDTO
            var result = FlightBUS.Instance.GetFlightById(flightId);

            if (result.Success)
            {
                var flight = result.GetData<FlightDTO>();
                vFlightNumber.Text = flight.FlightNumber;
                vRoute.Text = $"(Route ID: {flight.RouteId})";
                vAircraft.Text = $"(Aircraft ID: {flight.AircraftId})";
                vDeparture.Text = flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm");
                vArrival.Text = flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm");
                vStatus.Text = flight.Status.GetDescription();

                vSeats.Text = "(Chưa có dữ liệu)";
            }
            else
            {
                MessageBox.Show(result.GetFullErrorMessage(), "Lỗi tải chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Quay lại danh sách nếu lỗi
                OnBackToListRequested?.Invoke();
=======
            // ===== Các dòng thông tin =====
            int row = 0;

            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("ID Chuyến bay:"), 0, row);
            valueFlightId = Val("valueFlightId");
            grid.Controls.Add(valueFlightId, 1, row++);

            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Số hiệu:"), 0, row);
            valueFlightNumber = Val("valueFlightNumber");
            grid.Controls.Add(valueFlightNumber, 1, row++);

            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("ID Máy bay:"), 0, row);
            valueAircraftId = Val("valueAircraftId");
            grid.Controls.Add(valueAircraftId, 1, row++);

            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("ID Tuyến bay:"), 0, row);
            valueRouteId = Val("valueRouteId");
            grid.Controls.Add(valueRouteId, 1, row++);

            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Giờ khởi hành:"), 0, row);
            valueDepartureTime = Val("valueDepartureTime");
            grid.Controls.Add(valueDepartureTime, 1, row++);

            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Giờ hạ cánh:"), 0, row);
            valueArrivalTime = Val("valueArrivalTime");
            grid.Controls.Add(valueArrivalTime, 1, row++);

            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Trạng thái:"), 0, row);
            valueStatus = Val("valueStatus");
            grid.Controls.Add(valueStatus, 1, row++);

            card.Controls.Add(grid);
            grid.BringToFront();

            // ===== Bottom actions =====
            var bottom = new FlowLayoutPanel {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(0, 12, 12, 12)
            };

            var btnClose = new Button { 
                Text = "Đóng", 
                AutoSize = true,
                Padding = new Padding(12, 6, 12, 6)
            };
            btnClose.Click += (_, __) => FindForm()?.Close();

            var btnDelete = new Button { 
                Text = "🗑️ Xóa", 
                AutoSize = true,
                Padding = new Padding(12, 6, 12, 6),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.Click += BtnDelete_Click;

            var btnEdit = new Button { 
                Text = "✏️ Sửa", 
                AutoSize = true,
                Padding = new Padding(12, 6, 12, 6),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.Click += BtnEdit_Click;

            bottom.Controls.Add(btnClose);
            bottom.Controls.Add(btnDelete);
            bottom.Controls.Add(btnEdit);
            card.Controls.Add(bottom);

            // ===== Main =====
            main = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 2
            };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Clear();
            Controls.Add(main);
        }

        // Signature for compatibility with FlightListControl
        public void LoadFlightInfo(string flightNumber, string departurePlace, string arrivalPlace,
                                   string departureTime, string arrivalTime, string seatAvailable) {
            valueFlightNumber.Text = flightNumber ?? "";
            valueDepartureTime.Text = departureTime ?? "";
            valueArrivalTime.Text = arrivalTime ?? "";
            
            // Note: This method is used by popup form, doesn't have full info
            // For full detail view, use LoadFlightById
        }

        public void LoadFlightById(int flightId)
        {
            try
            {
                _currentFlightId = flightId;
                var flight = _flightBUS.GetFlightById(flightId);
                
                if (flight != null)
                {
                    valueFlightId.Text = flight.FlightId.ToString();
                    valueFlightNumber.Text = flight.FlightNumber;
                    valueAircraftId.Text = flight.AircraftId.ToString();
                    valueRouteId.Text = flight.RouteId.ToString();
                    valueDepartureTime.Text = flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
                    valueArrivalTime.Text = flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
                    valueStatus.Text = flight.Status.GetDescription();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin chuyến bay: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_currentFlightId <= 0)
            {
                MessageBox.Show("Không có chuyến bay được chọn", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var flight = _flightBUS.GetFlightById(_currentFlightId);
                if (flight == null)
                {
                    MessageBox.Show("Không tìm thấy chuyến bay", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create edit form
                var editForm = new Form {
                    Text = "Sửa chuyến bay",
                    Size = new Size(500, 400),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                var panel = new TableLayoutPanel {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(20),
                    ColumnCount = 2,
                    RowCount = 5,
                    AutoSize = true
                };

                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

                // Flight Number (read-only)
                int row = 0;
                panel.Controls.Add(new Label { Text = "Số hiệu:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
                var txtFlightNumber = new TextBox { Text = flight.FlightNumber, Dock = DockStyle.Fill, Enabled = false };
                panel.Controls.Add(txtFlightNumber, 1, row++);

                // Departure Time
                panel.Controls.Add(new Label { Text = "Giờ khởi hành:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
                var dtpDeparture = new DateTimePicker { 
                    Value = flight.DepartureTime ?? DateTime.Now, 
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy HH:mm",
                    Dock = DockStyle.Fill
                };
                panel.Controls.Add(dtpDeparture, 1, row++);

                // Arrival Time
                panel.Controls.Add(new Label { Text = "Giờ hạ cánh:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
                var dtpArrival = new DateTimePicker { 
                    Value = flight.ArrivalTime ?? DateTime.Now, 
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy HH:mm",
                    Dock = DockStyle.Fill
                };
                panel.Controls.Add(dtpArrival, 1, row++);

                // Status
                panel.Controls.Add(new Label { Text = "Trạng thái:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
                var cboStatus = new ComboBox { 
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboStatus.Items.Add(new { Text = "Đã lên lịch", Value = FlightStatus.SCHEDULED });
                cboStatus.Items.Add(new { Text = "Bị trễ", Value = FlightStatus.DELAYED });
                cboStatus.Items.Add(new { Text = "Đã hủy", Value = FlightStatus.CANCELLED });
                cboStatus.Items.Add(new { Text = "Hoàn thành", Value = FlightStatus.COMPLETED });
                cboStatus.DisplayMember = "Text";
                cboStatus.ValueMember = "Value";
                cboStatus.SelectedIndex = (int)flight.Status;
                panel.Controls.Add(cboStatus, 1, row++);

                // Buttons
                var btnPanel = new FlowLayoutPanel {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.RightToLeft
                };

                var btnSave = new Button { 
                    Text = "💾 Lưu", 
                    AutoSize = true,
                    Padding = new Padding(12, 6, 12, 6),
                    BackColor = Color.FromArgb(40, 167, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnSave.Click += (s, ev) => {
                    try {
                        if (dtpDeparture.Value >= dtpArrival.Value) {
                            MessageBox.Show("Giờ hạ cánh phải sau giờ khởi hành!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var updatedFlight = new FlightDTO(
                            flight.FlightId,
                            flight.FlightNumber,
                            flight.AircraftId,
                            flight.RouteId,
                            dtpDeparture.Value,
                            dtpArrival.Value,
                            (FlightStatus)((dynamic)cboStatus.SelectedItem).Value
                        );

                        if (_flightBUS.UpdateFlight(updatedFlight, out string msg)) {
                            MessageBox.Show("Cập nhật thành công!", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadFlightById(_currentFlightId);
                            DataChanged?.Invoke();
                            editForm.Close();
                        } else {
                            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    } catch (Exception ex) {
                        MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                var btnCancel = new Button { 
                    Text = "Hủy", 
                    AutoSize = true,
                    Padding = new Padding(12, 6, 12, 6)
                };
                btnCancel.Click += (s, ev) => editForm.Close();

                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnSave);
                panel.Controls.Add(btnPanel, 1, row++);

                editForm.Controls.Add(panel);
                editForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form sửa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_currentFlightId <= 0)
            {
                MessageBox.Show("Không có chuyến bay được chọn", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa chuyến bay ID {_currentFlightId}?\n\nLưu ý: Chỉ có thể xóa nếu không có dữ liệu liên quan.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    if (_flightBUS.DeleteFlight(_currentFlightId, out string message))
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataChanged?.Invoke();
                        FindForm()?.Close(); // Close detail form after delete
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
>>>>>>> Stashed changes
            }
        }
    }
}