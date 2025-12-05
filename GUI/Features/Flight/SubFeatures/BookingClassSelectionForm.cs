using System;
using System.Drawing;
using System.Windows.Forms;
using DTO.Flight;
using DAO.Flight;

namespace GUI.Features.Flight.SubFeatures
{
    public class BookingClassSelectionForm : Form
    {
        private FlightDTO _flight;
        private FlightWithDetailsDTO _flightDetails;
        private string _selectedCabinClass = string.Empty;
        
        private Label lblTitle;
        private Panel flightInfoPanel;
        private Panel cabinSelectionPanel;
        private Button btnConfirm;
        private Button btnCancel;
        
        // Flight info labels
        private Label lblFlightNumber, lblRoute, lblDepartureTime, lblArrivalTime, lblAvailableSeats;
        
        // Cabin class buttons
        private Button btnEconomy, btnBusiness;
        
        // Price labels
        private Label lblEconomyPrice, lblBusinessPrice;

        /// <summary>
        /// NOTE: Sau khi user xác nhận, sử dụng properties này để lấy dữ liệu:
        /// - SelectedFlightId: ID chuyến bay đã chọn
        /// - SelectedCabinClass: Hạng vé đã chọn ("Economy" hoặc "Business")
        /// </summary>
        public int SelectedFlightId => _flight?.FlightId ?? 0;
        public string SelectedCabinClass => _selectedCabinClass;
        public FlightDTO SelectedFlight => _flight;
        public FlightWithDetailsDTO SelectedFlightDetails => _flightDetails;

        public BookingClassSelectionForm(FlightDTO flight)
        {
            _flight = flight ?? throw new ArgumentNullException(nameof(flight));
            InitializeComponent();
            LoadFlightInfo();
            LoadCabinPrices();
        }

        public BookingClassSelectionForm(FlightWithDetailsDTO flightDetails)
        {
            _flightDetails = flightDetails ?? throw new ArgumentNullException(nameof(flightDetails));
            _flight = new FlightDTO(
                flightDetails.FlightId,
                flightDetails.FlightNumber,
                flightDetails.AircraftId,
                flightDetails.RouteId,
                flightDetails.DepartureTime,
                flightDetails.ArrivalTime,
                flightDetails.Status
            );
            InitializeComponent();
            LoadFlightInfo();
            LoadCabinPrices();
        }

        private void InitializeComponent()
        {
            Text = "Chọn hạng vé";
            Size = new Size(700, 600);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(240, 244, 248);
            Padding = new Padding(20);

            // Title
            lblTitle = new Label
            {
                Text = "✈️ CHỌN HẠNG VÉ MÁY BAY",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(28, 48, 74),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            Controls.Add(lblTitle);

            // Flight Info Panel
            flightInfoPanel = CreateFlightInfoPanel();
            flightInfoPanel.Location = new Point(20, 70);
            Controls.Add(flightInfoPanel);

            // Cabin Selection Panel
            cabinSelectionPanel = CreateCabinSelectionPanel();
            cabinSelectionPanel.Location = new Point(20, 280);
            Controls.Add(cabinSelectionPanel);

            // Buttons
            var buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0)
            };

            btnConfirm = new Button
            {
                Text = "✓ Xác nhận đặt vé",
                Width = 150,
                Height = 40,
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click;

            btnCancel = new Button
            {
                Text = "✗ Hủy",
                Width = 100,
                Height = 40,
                BackColor = Color.FromArgb(100, 116, 139),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            buttonPanel.Controls.Add(btnConfirm);
            buttonPanel.Controls.Add(btnCancel);
            Controls.Add(buttonPanel);
        }

        private Panel CreateFlightInfoPanel()
        {
            var panel = new Panel
            {
                Width = 640,
                Height = 190,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            var titleLabel = new Label
            {
                Text = "📋 THÔNG TIN CHUYẾN BAY",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(28, 48, 74),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            panel.Controls.Add(titleLabel);

            var layout = new TableLayoutPanel
            {
                Location = new Point(20, 50),
                Size = new Size(600, 130),
                ColumnCount = 2,
                RowCount = 5,
                BackColor = Color.Transparent
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            lblFlightNumber = AddInfoRow(layout, 0, "Mã chuyến bay:", "");
            lblRoute = AddInfoRow(layout, 1, "Tuyến bay:", "");
            lblDepartureTime = AddInfoRow(layout, 2, "Giờ khởi hành:", "");
            lblArrivalTime = AddInfoRow(layout, 3, "Giờ đến:", "");
            lblAvailableSeats = AddInfoRow(layout, 4, "Ghế trống:", "");

            panel.Controls.Add(layout);
            return panel;
        }

        private Label AddInfoRow(TableLayoutPanel layout, int row, string labelText, string value)
        {
            var keyLabel = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            layout.Controls.Add(keyLabel, 0, row);

            var valueLabel = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(28, 48, 74),
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            layout.Controls.Add(valueLabel, 1, row);

            return valueLabel;
        }

        private Panel CreateCabinSelectionPanel()
        {
            var panel = new Panel
            {
                Width = 640,
                Height = 200,
                BackColor = Color.Transparent
            };

            var titleLabel = new Label
            {
                Text = "🎫 CHỌN HẠNG VÉ",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(28, 48, 74),
                AutoSize = true,
                Location = new Point(0, 0)
            };
            panel.Controls.Add(titleLabel);

            // Economy class card
            var economyCard = CreateCabinCard(
                "PHỔ THÔNG",
                "Economy Class",
                Color.FromArgb(59, 130, 246),
                new Point(0, 40),
                () => SelectCabinClass("Economy")
            );
            btnEconomy = economyCard.Item1;
            lblEconomyPrice = economyCard.Item2;
            panel.Controls.Add(economyCard.Item1);

            // Business class card
            var businessCard = CreateCabinCard(
                "THƯƠNG GIA",
                "Business Class",
                Color.FromArgb(168, 85, 247),
                new Point(330, 40),
                () => SelectCabinClass("Business")
            );
            btnBusiness = businessCard.Item1;
            lblBusinessPrice = businessCard.Item2;
            panel.Controls.Add(businessCard.Item1);

            return panel;
        }

        private (Button, Label) CreateCabinCard(string title, string subtitle, Color color, Point location, Action onClick)
        {
            var card = new Button
            {
                Width = 300,
                Height = 140,
                Location = location,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = subtitle
            };
            card.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            card.FlatAppearance.BorderSize = 2;
            card.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 250, 252);
            card.Click += (s, e) => onClick();

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = color,
                AutoSize = true,
                Location = new Point(20, 20),
                BackColor = Color.Transparent
            };
            titleLabel.Click += (s, e) => onClick();
            card.Controls.Add(titleLabel);

            var subtitleLabel = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true,
                Location = new Point(20, 50),
                BackColor = Color.Transparent
            };
            subtitleLabel.Click += (s, e) => onClick();
            card.Controls.Add(subtitleLabel);

            var priceLabel = new Label
            {
                Text = "Đang tải...",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(28, 48, 74),
                AutoSize = true,
                Location = new Point(20, 90),
                BackColor = Color.Transparent
            };
            priceLabel.Click += (s, e) => onClick();
            card.Controls.Add(priceLabel);

            return (card, priceLabel);
        }

        private void SelectCabinClass(string cabinClass)
        {
            _selectedCabinClass = cabinClass;

            // Reset all cards
            btnEconomy.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnEconomy.BackColor = Color.White;
            btnBusiness.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnBusiness.BackColor = Color.White;

            // Highlight selected card
            if (cabinClass == "Economy")
            {
                btnEconomy.FlatAppearance.BorderColor = Color.FromArgb(59, 130, 246);
                btnEconomy.BackColor = Color.FromArgb(239, 246, 255);
            }
            else if (cabinClass == "Business")
            {
                btnBusiness.FlatAppearance.BorderColor = Color.FromArgb(168, 85, 247);
                btnBusiness.BackColor = Color.FromArgb(250, 245, 255);
            }

            btnConfirm.Enabled = true;
        }

        private void LoadFlightInfo()
        {
            if (_flight == null) return;

            lblFlightNumber.Text = _flight.FlightNumber ?? "N/A";
            
            // Hiển thị thông tin sân bay nếu có chi tiết
            if (_flightDetails != null)
            {
                lblRoute.Text = $"{_flightDetails.DepartureAirportDisplay} → {_flightDetails.ArrivalAirportDisplay}";
                lblAvailableSeats.Text = $"{_flightDetails.AvailableSeats} ghế";
            }
            else
            {
                lblRoute.Text = $"Route ID: {_flight.RouteId}";
                lblAvailableSeats.Text = "Đang kiểm tra...";
            }
            
            lblDepartureTime.Text = _flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
            lblArrivalTime.Text = _flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
        }

        private void LoadCabinPrices()
        {
            try
            {
                // Lấy giá vé từ database theo FlightId và CabinClass
                var economyPrice = GetCabinPrice(_flight.FlightId, "Economy");
                var businessPrice = GetCabinPrice(_flight.FlightId, "Business");

                lblEconomyPrice.Text = economyPrice > 0 
                    ? $"{economyPrice:N0} VNĐ" 
                    : "Liên hệ";
                    
                lblBusinessPrice.Text = businessPrice > 0 
                    ? $"{businessPrice:N0} VNĐ" 
                    : "Liên hệ";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải giá vé: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private long GetCabinPrice(int flightId, string cabinClass)
        {
            try
            {
                // Lấy giá vé trung bình từ Flight_Seats theo cabin class
                // Query: SELECT AVG(base_price) FROM Flight_Seats fs
                //        JOIN Seats s ON fs.seat_id = s.seat_id
                //        JOIN Cabin_Classes cc ON s.class_id = cc.class_id
                //        WHERE fs.flight_id = @flightId AND cc.class_name = @cabinClass AND fs.seat_status = 'AVAILABLE'

                // Giá mẫu (có thể cập nhật khi hoàn thành database query)
                if (cabinClass == "Economy")
                    return 1500000; // 1.5 triệu VNĐ
                else if (cabinClass == "Business")
                    return 3500000; // 3.5 triệu VNĐ

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy giá vé: {ex.Message}");
                return 0;
            }
            {
                return 0;
            }
        }

        private void BtnConfirm_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedCabinClass))
            {
                MessageBox.Show("Vui lòng chọn hạng vé!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Xác nhận đặt vé hạng {(_selectedCabinClass == "Economy" ? "Phổ thông" : "Thương gia")}?\n\n" +
                $"Chuyến bay: {_flight.FlightNumber}\n" +
                $"Tuyến: {lblRoute.Text}\n" +
                $"Giờ khởi hành: {lblDepartureTime.Text}",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
