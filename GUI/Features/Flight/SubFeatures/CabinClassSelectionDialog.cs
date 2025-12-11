using BUS.Auth;
using BUS.Flight;
using DTO.Booking;
using DTO.Flight;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Flight.SubFeatures
{
    public class CabinClassSelectionDialog : Form
    {
        // Dialog modes for round-trip flow
        private enum DialogMode { Outbound, ReturnFlightSelection, ReturnCabinSelection }
        
        private FlightWithDetailsDTO _flight;
        private string _selectedCabinClass;
        private int _selectedCabinClassId;
        private CheckBox chkRoundTrip;
        private NumericUpDown numPassengers;
        
        // Seat availability tracking
        private Dictionary<int, int> _availableSeats; // class_id -> available count
        private BUS.FlightSeat.FlightSeatBUS _seatBUS = new BUS.FlightSeat.FlightSeatBUS();
        
        // Round-trip state
        private DialogMode _currentMode = DialogMode.Outbound;
        private FlightWithDetailsDTO _selectedReturnFlight;
        private BookingRequestDTO _outboundBooking;
        private Panel _mainContentPanel;
        private Panel _returnFlightPanel;
        private List<FlightWithDetailsDTO> _allFlights; // For return flight selection

        // Property để lấy thông tin đặt vé sau khi dialog đóng
        public BookingRequestDTO BookingRequest { get; private set; }
        public BookingRequestDTO ReturnBooking { get; set; } // For round-trip
        public bool IsRoundTrip { get; private set; }

        public CabinClassSelectionDialog(FlightWithDetailsDTO flight, List<FlightWithDetailsDTO> allFlights = null)
        {
            _flight = flight;
            _allFlights = allFlights ?? new List<FlightWithDetailsDTO>();
            
            // Load available seats for this flight
            try
            {
                _availableSeats = _seatBUS.GetAvailableSeatsByClass(flight.FlightId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin ghế trống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _availableSeats = new Dictionary<int, int>();
            }
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Chọn hạng vé";
            Size = new Size(600, 580); // Increased to 580 for buttons
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;

            // Title
            var lblTitle = new Label
            {
                Text = "🎫 CHỌN HẠNG VÉ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            Controls.Add(lblTitle);

            // Flight info panel
            var flightInfoPanel = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(540, 80),
                BackColor = Color.FromArgb(232, 240, 252),
                Padding = new Padding(12)
            };

            var lblFlightNumber = new Label
            {
                Text = $"Chuyến bay: {_flight.FlightNumber}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = true,
                Location = new Point(12, 12)
            };
            flightInfoPanel.Controls.Add(lblFlightNumber);

            var lblRoute = new Label
            {
                Text = $"Từ: {_flight.DepartureAirportCode} - {_flight.DepartureCity} → Đến: {_flight.ArrivalAirportCode} - {_flight.ArrivalCity}",
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(12, 38)
            };
            flightInfoPanel.Controls.Add(lblRoute);

            var lblTime = new Label
            {
                Text = $"Khởi hành: {_flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm")}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(100, 100, 100),
                AutoSize = true,
                Location = new Point(12, 60)
            };
            flightInfoPanel.Controls.Add(lblTime);

            Controls.Add(flightInfoPanel);

            // Passenger count selector
            var lblPassengerCount = new Label
            {
                Text = "Số lượng hành khách:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 155)
            };
            Controls.Add(lblPassengerCount);

            var numPassengers = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 9,
                Value = 1,
                Width = 80,
                Location = new Point(220, 153),
                Font = new Font("Segoe UI", 11)
            };
            Controls.Add(numPassengers);

            var lblNote = new Label
            {
                Text = "(1-9 người)",
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9),
                AutoSize = true,
                Location = new Point(310, 157)
            };
            Controls.Add(lblNote);

            // Round-trip checkbox (NOW ENABLED!)
            chkRoundTrip = new CheckBox
            {
                Text = "Khứ hồi (2 chiều)",
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(400, 157),
                Enabled = true, // ENABLED for Phase 2
                ForeColor = Color.FromArgb(0, 92, 175)
            };
            Controls.Add(chkRoundTrip);

            // Cabin classes
            var lblSelectClass = new Label
            {
                Text = "Chọn hạng vé:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 195)
            };
            Controls.Add(lblSelectClass);

            // Cabin class options with IDs
            CreateCabinClassOption("First Class", "Hạng Nhất", "✈️", Color.FromArgb(255, 215, 0), 20, 225, 1);
            CreateCabinClassOption("Business", "Hạng Thương Gia", "💼", Color.FromArgb(100, 149, 237), 20, 285, 2);
            CreateCabinClassOption("Premium Economy", "Hạng Phổ Thông Đặc Biệt", "🎫", Color.FromArgb(60, 179, 113), 20, 345, 3);
            CreateCabinClassOption("Economy", "Hạng Phổ Thông", "🪑", Color.FromArgb(169, 169, 169), 20, 405, 4);

            // Buttons
            var btnConfirm = new Button
            {
                Text = "Xác nhận",
                Size = new Size(120, 40),
                Location = new Point(340, 480), // Shifted down
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += (s, e) =>
            {
                if (_currentMode == DialogMode.Outbound)
                {
                    // Outbound mode validation
                    if (string.IsNullOrEmpty(_selectedCabinClass))
                    {
                        MessageBox.Show("Vui lòng chọn hạng vé.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Validate seat availability
                    int requestedSeats = (int)numPassengers.Value;
                    int availableSeats = _availableSeats.ContainsKey(_selectedCabinClassId) ? _availableSeats[_selectedCabinClassId] : 0;
                    
                    if (requestedSeats > availableSeats)
                    {
                        MessageBox.Show(
                            $"Không đủ chỗ trống!\n\n" +
                            $"Hạng vé đã chọn chỉ còn {availableSeats} chỗ trống.\n" +
                            $"Vui lòng chọn ít hành khách hơn hoặc chọn hạng vé khác.",
                            "Không đủ chỗ",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    // Create outbound booking
                    var groupId = chkRoundTrip.Checked ? Guid.NewGuid() : (Guid?)null;
                    
                    BookingRequest = new BookingRequestDTO
                    {
                        AccountId = UserSession.CurrentAccountId,
                        FlightId = _flight.FlightId,
                        CabinClassId = _selectedCabinClassId,
                        CabinClassName = _selectedCabinClass,
                        BookingDate = DateTime.Now,
                        TicketCount = (int)numPassengers.Value,
                        IsRoundTrip = chkRoundTrip.Checked,
                        GroupBookingId = groupId,
                        FlightNumber = _flight.FlightNumber,
                        DepartureAirportCode = _flight.DepartureAirportCode,
                        ArrivalAirportCode = _flight.ArrivalAirportCode,
                        DepartureTime = _flight.DepartureTime
                    };

                    IsRoundTrip = chkRoundTrip.Checked;

                    if (chkRoundTrip.Checked)
                    {
                        // Store outbound and switch to return flight selection
                        _outboundBooking = BookingRequest;
                        _currentMode = DialogMode.ReturnFlightSelection;
                        ShowReturnFlightSelection();
                        return; // Don't close dialog
                    }
                    else
                    {
                        // One-way complete
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                else if (_currentMode == DialogMode.ReturnCabinSelection)
                {
                    // Return cabin selection validation
                    if (string.IsNullOrEmpty(_selectedCabinClass))
                    {
                        MessageBox.Show("Vui lòng chọn hạng vé cho chuyến về.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Validate seat availability for return flight
                    int requestedSeats = (int)numPassengers.Value;
                    int availableSeats = _availableSeats.ContainsKey(_selectedCabinClassId) ? _availableSeats[_selectedCabinClassId] : 0;
                    
                    if (requestedSeats > availableSeats)
                    {
                        MessageBox.Show(
                            $"Không đủ chỗ trống cho chuyến về!\n\n" +
                            $"Hạng vé đã chọn chỉ còn {availableSeats} chỗ trống.\n" +
                            $"Vui lòng chọn ít hành khách hơn hoặc chọn hạng vé khác.",
                            "Không đủ chỗ",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    // Create return booking with same GroupBookingId
                    ReturnBooking = new BookingRequestDTO
                    {
                        AccountId = UserSession.CurrentAccountId,
                        FlightId = _selectedReturnFlight.FlightId,
                        CabinClassId = _selectedCabinClassId,
                        CabinClassName = _selectedCabinClass,
                        BookingDate = DateTime.Now,
                        TicketCount = (int)numPassengers.Value,
                        IsRoundTrip = true,
                        GroupBookingId = _outboundBooking.GroupBookingId,
                        FlightNumber = _selectedReturnFlight.FlightNumber,
                        DepartureAirportCode = _selectedReturnFlight.DepartureAirportCode,
                        ArrivalAirportCode = _selectedReturnFlight.ArrivalAirportCode,
                        DepartureTime = _selectedReturnFlight.DepartureTime
                    };

                    // Round-trip complete
                    DialogResult = DialogResult.OK;
                    Close();
                }
            };
            Controls.Add(btnConfirm);

            var btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(100, 40),
                Location = new Point(470, 480), // Shifted down
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            Controls.Add(btnCancel);
        }

        private void CreateCabinClassOption(string className, string displayName, string icon, Color color, int x, int y, int cabinClassId)
        {
            // Get available seats for this class
            int availableCount = _availableSeats.ContainsKey(cabinClassId) ? _availableSeats[cabinClassId] : 0;
            string availabilityText = availableCount > 0 ? $"(còn {availableCount} chỗ)" : "(hết chỗ)";
            
            var panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(540, 50),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = availableCount > 0 ? Color.White : Color.FromArgb(240, 240, 240),
                Cursor = availableCount > 0 ? Cursors.Hand : Cursors.No,
                Tag = (className, cabinClassId),
                Enabled = availableCount > 0
            };

            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 18),
                AutoSize = true,
                Location = new Point(12, 10)
            };
            panel.Controls.Add(lblIcon);

            var lblName = new Label
            {
                Text = $"{displayName} {availabilityText}",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = availableCount > 0 ? color : Color.Gray,
                AutoSize = true,
                Location = new Point(60, 15)
            };
            panel.Controls.Add(lblName);

            // Click event for the entire panel
            EventHandler clickHandler = (s, e) =>
            {
                // Only allow selection if seats are available
                if (availableCount == 0)
                {
                    MessageBox.Show(
                        $"Hạng vé {displayName} đã hết chỗ.\nVui lòng chọn hạng vé khác.",
                        "Hết chỗ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                
                _selectedCabinClass = className;
                _selectedCabinClassId = cabinClassId;
                
                // Highlight selected panel
                foreach (Control ctrl in Controls)
                {
                    if (ctrl is Panel p && p.Tag is ValueTuple<string, int>)
                    {
                        p.BackColor = Color.White;
                        p.BorderStyle = BorderStyle.FixedSingle;
                    }
                }
                
                panel.BackColor = Color.FromArgb(232, 240, 252);
                panel.BorderStyle = BorderStyle.FixedSingle;
            };

            panel.Click += clickHandler;
            lblIcon.Click += clickHandler;
            lblName.Click += clickHandler;

            Controls.Add(panel);
        }

        private void ShowReturnFlightSelection()
        {
            // Hide all existing controls
            foreach (Control ctrl in Controls.OfType<Control>().ToList())
            {
                ctrl.Visible = false;
            }

            // Update title
            Text = "Chọn chuyến bay chiều về";

            // Title label
            var lblTitle = new Label
            {
                Text = $"✈️ CHỌN CHUYẾN BAY CHIỀU VỀ\n{_flight.ArrivalAirportCode} → {_flight.DepartureAirportCode}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = false,
                Size = new Size(560, 60),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(lblTitle);

            // Get return flights from passed list
            var returnFlights = _allFlights
                .Where(f => f.DepartureAirportCode == _flight.ArrivalAirportCode &&
                           f.ArrivalAirportCode == _flight.DepartureAirportCode &&
                           f.Status == DTO.Flight.FlightStatus.SCHEDULED)
                .ToList();

            if (!returnFlights.Any())
            {
                MessageBox.Show(
                    "Không tìm thấy chuyến bay chiều về.\nVui lòng thử lại sau.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            // Flight list
            var listBox = new ListBox
            {
                Location = new Point(20, 90),
                Size = new Size(560, 350),
                Font = new Font("Segoe UI", 10),
                DisplayMember = "FlightNumber"
            };

            foreach (var flight in returnFlights)
            {
                var displayText = $"{flight.FlightNumber} | {flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm")} | " +
                                $"{flight.DepartureCity} → {flight.ArrivalCity}";
                listBox.Items.Add(new { Flight = flight, Display = displayText });
            }
            listBox.DisplayMember = "Display";
            listBox.SelectedIndexChanged += (s, e) =>
            {
                if (listBox.SelectedItem != null)
                {
                    dynamic selected = listBox.SelectedItem;
                    _selectedReturnFlight = selected.Flight;
                }
            };
            Controls.Add(listBox);

            // Continue button
            var btnContinue = new Button
            {
                Text = "Tiếp tục →",
                Size = new Size(150, 45),
                Location = new Point(340, 460),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnContinue.FlatAppearance.BorderSize = 0;
            btnContinue.Click += (s, e) =>
            {
                if (_selectedReturnFlight == null)
                {
                    MessageBox.Show("Vui lòng chọn chuyến bay.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Load available seats for return flight
                try
                {
                    _availableSeats = _seatBUS.GetAvailableSeatsByClass(_selectedReturnFlight.FlightId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải thông tin ghế trống cho chuyến về: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _availableSeats = new Dictionary<int, int>();
                }
                
                ShowReturnCabinSelection();
            };
            Controls.Add(btnContinue);

            // Cancel button
            var btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(120, 45),
                Location = new Point(500, 460),
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            Controls.Add(btnCancel);
        }

        private void ShowReturnCabinSelection()
        {
            // Reset for return cabin selection
            _selectedCabinClass = null;
            _selectedCabinClassId = 0;
            _currentMode = DialogMode.ReturnCabinSelection;

            // Hide return flight list
            foreach (Control ctrl in Controls.OfType<Control>().ToList())
            {
                ctrl.Visible = false;
            }

            // Show cabin selection UI (reuse InitializeComponent structure)
            Text = "Chọn hạng vé - Chiều về";
            InitializeReturnCabinUI();
        }

        private void InitializeReturnCabinUI()
        {
            // Similar to InitializeComponent but for return
            var lblTitle = new Label
            {
                Text = $"🎫 CHỌN HẠNG VÉ - CHIỀU VỀ\n{_selectedReturnFlight.FlightNumber}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                AutoSize = false,
                Size = new Size(560, 60),
                Location = new Point(20, 20)
            };
            Controls.Add(lblTitle);

            var lblSelectClass = new Label
            {
                Text = "Chọn hạng vé:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 90)
            };
            Controls.Add(lblSelectClass);

            // Cabin class options
            CreateCabinClassOption("First Class", "Hạng Nhất", "✈️", Color.FromArgb(255, 215, 0), 20, 120, 1);
            CreateCabinClassOption("Business", "Hạng Thương Gia", "💼", Color.FromArgb(100, 149, 237), 20, 180, 2);
            CreateCabinClassOption("Premium Economy", "Hạng Phổ Thông Đặc Biệt", "🎫", Color.FromArgb(60, 179, 113), 20, 240, 3);
            CreateCabinClassOption("Economy", "Hạng Phổ Thông", "🪑", Color.FromArgb(169, 169, 169), 20, 300, 4);

            // Confirm button (uses existing btnConfirm logic which handles ReturnCabinSelection mode)
            var btnConfirm = new Button
            {
                Text = "Xác nhận",
                Size = new Size(150, 45),
                Location = new Point(340, 380),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(_selectedCabinClass))
                {
                    MessageBox.Show("Vui lòng chọn hạng vé cho chuyến về.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate seat availability for return flight  
                int requestedSeats = _outboundBooking.TicketCount;
                int availableSeats = _availableSeats.ContainsKey(_selectedCabinClassId) ? _availableSeats[_selectedCabinClassId] : 0;
                
                if (requestedSeats > availableSeats)
                {
                    MessageBox.Show(
                        $"Không đủ chỗ trống cho chuyến về!\n\n" +
                        $"Hạng vé đã chọn chỉ còn {availableSeats} chỗ trống.\n" +
                        $"Vui lòng chọn ít hành khách hơn hoặc chọn hạng vé khác.",
                        "Không đủ chỗ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Create return booking
                ReturnBooking = new BookingRequestDTO
                {
                    AccountId = UserSession.CurrentAccountId,
                    FlightId = _selectedReturnFlight.FlightId,
                    CabinClassId = _selectedCabinClassId,
                    CabinClassName = _selectedCabinClass,
                    BookingDate = DateTime.Now,
                    TicketCount = _outboundBooking.TicketCount,
                    IsRoundTrip = true,
                    GroupBookingId = _outboundBooking.GroupBookingId,
                    FlightNumber = _selectedReturnFlight.FlightNumber,
                    DepartureAirportCode = _selectedReturnFlight.DepartureAirportCode,
                    ArrivalAirportCode = _selectedReturnFlight.ArrivalAirportCode,
                    DepartureTime = _selectedReturnFlight.DepartureTime
                };

                DialogResult = DialogResult.OK;
                Close();
            };
            Controls.Add(btnConfirm);
        }
    }
}
