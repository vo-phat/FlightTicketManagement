using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using GUI.Components.Buttons;
using GUI.Features.Flight;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using BUS.Flight;
using DTO.Flight;


namespace GUI.Features.Flight.SubFeatures {
    public class FlightListControl : UserControl {
        // Action column constants
        private const string ACTION_COL_NAME = "actions";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";   // Admin/Staff
        private const string TXT_DELETE = "Xóa";  // Admin/Staff
        private const string TXT_BOOK = "Đặt vé";  // Customer
        private const string SEP = " | ";
        
        // Permission check delegate
        private Func<string, bool>? _hasPermission;
        
        // Event để notify khi user chọn xong hạng vé
        public event Action<int, string>? BookingRequested; // (flightId, cabinClass)
        
        private TableCustom table;
        private Panel headerPanel;
        private Label lblTitle;
        
        // Search filters
        private Panel searchCard;
        private UnderlinedTextField txtFlightNumber;
        private UnderlinedComboBox cbDepartureAirport;
        private UnderlinedComboBox cbArrivalAirport;
        private DateTimePickerCustom dtpDepartureDate;
        private DateTimePickerCustom dtpReturnDate;
        private UnderlinedTextField txtAdults;
        private UnderlinedTextField txtChildren;
        private CheckBox chkRoundTrip;
        private PrimaryButton btnSearch;
        private SecondaryButton btnClear;
        private SecondaryButton btnRefresh;

        public FlightListControl() {
            InitializeComponent();
            LoadAirports();
            LoadFlights();
        }

        public FlightListControl(Func<string, bool> hasPermission) : this() {
            _hasPermission = hasPermission;
            
            // Debug: Log permissions
            Console.WriteLine($"[FlightListControl] Permission Check:");
            Console.WriteLine($"  - accounts.manage: {_hasPermission?.Invoke("accounts.manage")}");
            Console.WriteLine($"  - system.roles: {_hasPermission?.Invoke("system.roles")}");
            Console.WriteLine($"  - flights.read: {_hasPermission?.Invoke("flights.read")}");
            Console.WriteLine($"  - flights.create: {_hasPermission?.Invoke("flights.create")}");
            Console.WriteLine($"  => IsAdmin: {IsAdmin()}");
            Console.WriteLine($"  => IsStaff: {IsStaff()}");
            Console.WriteLine($"  => IsCustomer: {IsCustomer()}");
        }

        // Admin: Có quyền flights.create VÀ các quyền quản trị khác (accounts.manage, system.roles)
        private bool IsAdmin() {
            return _hasPermission?.Invoke("accounts.manage") == true ||
                   _hasPermission?.Invoke("system.roles") == true;
        }

        // Staff: Có quyền flights.read NHƯ NG KHÔNG phải Admin
        private bool IsStaff() {
            return _hasPermission?.Invoke("flights.read") == true && 
                   !IsAdmin();
        }

        // Customer: Không có quyền flights.read
        private bool IsCustomer() {
            return !_hasPermission?.Invoke("flights.read") == true;
        }

        private void InitializeComponent() {
            SuspendLayout();
            
            // Main container
            BackColor = Color.FromArgb(240, 244, 248);
            Dock = DockStyle.Fill;
            Padding = new Padding(20);
            AutoScroll = true;

            // Header
            headerPanel = CreateHeader();
            Controls.Add(headerPanel);

            // Search card
            searchCard = CreateSearchCard();
            Controls.Add(searchCard);

            // Results table
            table = CreateTable();
            Controls.Add(table);
            
            // Debug: Log column headers visibility
            Console.WriteLine($"Table ColumnHeadersVisible: {table.ColumnHeadersVisible}");
            Console.WriteLine($"Table ColumnHeadersHeight: {table.ColumnHeadersHeight}");
            Console.WriteLine($"Table Columns Count: {table.Columns.Count}");

            ResumeLayout(false);
        }

        private Panel CreateHeader() {
            var panel = new Panel {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Transparent
            };

            lblTitle = new Label {
                Text = "🔍 TÌM KIẾM CHUYẾN BAY",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(28, 48, 74),
                AutoSize = true,
                Location = new Point(0, 15)
            };

            panel.Controls.Add(lblTitle);
            return panel;
        }

        private Panel CreateSearchCard() {
            var card = new Panel {
                Dock = DockStyle.Top,
                Height = 280,
                BackColor = Color.White,
                Padding = new Padding(25)
            };

            // Add shadow effect
            card.Paint += (s, e) => {
                var rect = card.ClientRectangle;
                rect.Inflate(-1, -1);
                using (var pen = new Pen(Color.FromArgb(220, 225, 230), 2)) {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            var layout = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));

            // Row 1: Flight Number and Airports
            txtFlightNumber = new UnderlinedTextField {
                LabelText = "🔢 Mã chuyến bay",
                PlaceholderText = "VD: VN201",
                Margin = new Padding(5),
                Dock = DockStyle.Fill
            };

            cbDepartureAirport = new UnderlinedComboBox {
                LabelText = "✈️ Nơi đi",
                Margin = new Padding(5),
                Dock = DockStyle.Fill
            };

            cbArrivalAirport = new UnderlinedComboBox {
                LabelText = "🛬 Nơi đến",
                Margin = new Padding(5),
                Dock = DockStyle.Fill
            };

            dtpDepartureDate = new DateTimePickerCustom {
                LabelText = "📅 Ngày đi",
                Margin = new Padding(5),
                Dock = DockStyle.Fill
            };

            var pnlRoundTrip = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
            chkRoundTrip = new CheckBox {
                Text = "Khứ hồi",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(10, 30),
                Checked = false
            };
            chkRoundTrip.CheckedChanged += (s, e) => {
                dtpReturnDate.Enabled = chkRoundTrip.Checked;
                if (!chkRoundTrip.Checked) {
                    dtpReturnDate.Value = DateTime.Now;
                }
            };
            pnlRoundTrip.Controls.Add(chkRoundTrip);

            // Row 2: Dates and Passengers
            dtpReturnDate = new DateTimePickerCustom {
                LabelText = "📅 Ngày về",
                Margin = new Padding(5),
                Dock = DockStyle.Fill,
                Enabled = false
            };

            txtAdults = new UnderlinedTextField {
                LabelText = "👤 Người lớn",
                PlaceholderText = "Số lượng",
                Margin = new Padding(5),
                Dock = DockStyle.Fill
            };
            txtAdults.TextChanged += (s, e) => {
                if (!string.IsNullOrEmpty(txtAdults.Text) && !int.TryParse(txtAdults.Text, out _)) {
                    txtAdults.Text = string.Empty;
                }
            };

            txtChildren = new UnderlinedTextField {
                LabelText = "👶 Trẻ em",
                PlaceholderText = "Số lượng",
                Margin = new Padding(5),
                Dock = DockStyle.Fill
            };
            txtChildren.TextChanged += (s, e) => {
                if (!string.IsNullOrEmpty(txtChildren.Text) && !int.TryParse(txtChildren.Text, out _)) {
                    txtChildren.Text = string.Empty;
                }
            };

            // Row 3: Buttons
            var btnPanel = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5, 10, 5, 5)
            };

            btnSearch = new PrimaryButton {
                Text = "🔍 TÌM CHUYẾN BAY",
                Width = 180,
                Height = 42,
                Margin = new Padding(5, 0, 10, 0)
            };
            btnSearch.Click += BtnSearch_Click;

            btnClear = new SecondaryButton {
                Text = "🔄 Xóa bộ lọc",
                Width = 150,
                Height = 42,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnClear.Click += BtnClear_Click;

            btnRefresh = new SecondaryButton {
                Text = "↻ Làm mới",
                Width = 130,
                Height = 42
            };
            btnRefresh.Click += (s, e) => LoadFlights();

            btnPanel.Controls.Add(btnSearch);
            btnPanel.Controls.Add(btnClear);
            btnPanel.Controls.Add(btnRefresh);

            // Add to layout
            layout.Controls.Add(txtFlightNumber, 0, 0);
            layout.Controls.Add(cbDepartureAirport, 1, 0);
            layout.Controls.Add(cbArrivalAirport, 2, 0);
            layout.Controls.Add(pnlRoundTrip, 3, 0);
            
            layout.Controls.Add(dtpDepartureDate, 0, 1);
            layout.Controls.Add(dtpReturnDate, 1, 1);
            layout.Controls.Add(txtAdults, 2, 1);
            layout.Controls.Add(txtChildren, 3, 1);
            
            layout.SetColumnSpan(btnPanel, 4);
            layout.Controls.Add(btnPanel, 0, 2);

            card.Controls.Add(layout);
            return card;
        }

        private TableCustom CreateTable() {
            var tbl = new TableCustom {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderColor = Color.FromArgb(220, 225, 230),
                BorderThickness = 1,
                CornerRadius = 12,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                ColumnHeadersVisible = true,
                Margin = new Padding(0, 10, 0, 0),
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 45,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };

            // Style for column headers
            tbl.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);
            tbl.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
            tbl.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            tbl.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            tbl.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            // Style for data rows - increase height
            tbl.RowTemplate.Height = 50;
            tbl.DefaultCellStyle.Padding = new Padding(5);
            tbl.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            tbl.DefaultCellStyle.SelectionForeColor = Color.FromArgb(28, 48, 74);

            // Columns
            tbl.Columns.Add(new DataGridViewTextBoxColumn {
                Name = "flightNumber",
                HeaderText = "Mã chuyến bay",
                FillWeight = 100
            });
            tbl.Columns.Add(new DataGridViewTextBoxColumn {
                Name = "departureAirport",
                HeaderText = "Nơi đi",
                FillWeight = 120
            });
            tbl.Columns.Add(new DataGridViewTextBoxColumn {
                Name = "arrivalAirport",
                HeaderText = "Nơi đến",
                FillWeight = 120
            });
            tbl.Columns.Add(new DataGridViewTextBoxColumn {
                Name = "departureTime",
                HeaderText = "Giờ khởi hành",
                FillWeight = 130
            });
            tbl.Columns.Add(new DataGridViewTextBoxColumn {
                Name = "arrivalTime",
                HeaderText = "Giờ đến",
                FillWeight = 130
            });
            tbl.Columns.Add(new DataGridViewTextBoxColumn {
                Name = "availableSeats",
                HeaderText = "Ghế trống",
                FillWeight = 80
            });
            tbl.Columns.Add(new DataGridViewTextBoxColumn {
                Name = "status",
                HeaderText = "Trạng thái",
                FillWeight = 110
            });
            tbl.Columns.Add(new DataGridViewTextBoxColumn {
                Name = "actions",
                HeaderText = "Thao tác",
                FillWeight = 150
            });
            tbl.Columns.Add(new DataGridViewTextBoxColumn {
                Name = "flightId",
                HeaderText = "ID",
                Visible = false
            });

            tbl.CellPainting += Table_CellPainting;
            tbl.CellMouseMove += Table_CellMouseMove;
            tbl.CellMouseClick += Table_CellMouseClick;

            return tbl;
        }

        private void LoadAirports() {
            try {
                var airportDAO = new DAO.Airport.AirportDAO();
                var airports = airportDAO.GetAllAirports();

                cbDepartureAirport.Items.Clear();
                cbArrivalAirport.Items.Clear();

                cbDepartureAirport.Items.Add(new ComboBoxItem { Text = "-- Chọn sân bay --", Value = 0 });
                cbArrivalAirport.Items.Add(new ComboBoxItem { Text = "-- Chọn sân bay --", Value = 0 });

                foreach (var airport in airports) {
                    var item = new ComboBoxItem {
                        Text = $"{airport.AirportName} ({airport.AirportCode}) - {airport.City}",
                        Value = airport.AirportId
                    };
                    cbDepartureAirport.Items.Add(item);
                    cbArrivalAirport.Items.Add(item);
                }

                cbDepartureAirport.SelectedIndex = 0;
                cbArrivalAirport.SelectedIndex = 0;
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi load sân bay: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadFlights() {
            try {
                var flightBUS = FlightBUS.Instance;
                var flights = flightBUS.GetAllFlightsWithDetails();

                table.Rows.Clear();

                foreach (var flight in flights) {
                    var row = new DataGridViewRow();
                    row.CreateCells(table);
                    
                    row.Cells[0].Value = flight.FlightNumber;
                    row.Cells[1].Value = flight.DepartureAirportDisplay; // SGN (TP. Hồ Chí Minh)
                    row.Cells[2].Value = flight.ArrivalAirportDisplay;   // HAN (Hà Nội)
                    row.Cells[3].Value = flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
                    row.Cells[4].Value = flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
                    row.Cells[5].Value = flight.AvailableSeats.ToString();
                    row.Cells[6].Value = flight.Status.GetDescription();
                    row.Cells[7].Value = ""; // Actions column (custom painted)
                    row.Cells[8].Value = flight.FlightId;

                    table.Rows.Add(row);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi load chuyến bay: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e) {
            try {
                // Search flights
                var flightBUS = FlightBUS.Instance;
                var allFlights = flightBUS.GetAllFlightsWithDetails();

                // Filter by flight number if provided (prioritize this)
                if (!string.IsNullOrWhiteSpace(txtFlightNumber.Text)) {
                    allFlights = allFlights.Where(f => 
                        f.FlightNumber?.Contains(txtFlightNumber.Text.Trim(), StringComparison.OrdinalIgnoreCase) == true
                    ).ToList();
                    
                    // Display results immediately if searching by flight number only
                    if (string.IsNullOrEmpty(cbDepartureAirport.Text) && string.IsNullOrEmpty(cbArrivalAirport.Text)) {
                        DisplayFlightsWithDetails(allFlights);
                        return;
                    }
                }

                // Validate other fields only if provided
                if (cbDepartureAirport.SelectedItem is ComboBoxItem depItem && (int)depItem.Value != 0) {
                    if (cbArrivalAirport.SelectedItem is ComboBoxItem arrItem && (int)arrItem.Value != 0) {
                        if ((int)depItem.Value == (int)arrItem.Value) {
                            MessageBox.Show("Nơi đi và nơi đến không được trùng nhau!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                // Get passengers count
                int adults = string.IsNullOrEmpty(txtAdults.Text) ? 1 : int.Parse(txtAdults.Text);
                int children = string.IsNullOrEmpty(txtChildren.Text) ? 0 : int.Parse(txtChildren.Text);
                int totalPassengers = adults + children;

                if (totalPassengers <= 0 && !string.IsNullOrWhiteSpace(txtAdults.Text)) {
                    MessageBox.Show("Số lượng hành khách phải lớn hơn 0!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Filter by flight number if provided
                if (!string.IsNullOrWhiteSpace(txtFlightNumber.Text)) {
                    allFlights = allFlights.Where(f => 
                        f.FlightNumber?.Contains(txtFlightNumber.Text.Trim(), StringComparison.OrdinalIgnoreCase) == true
                    ).ToList();
                }

                // Filter by date range
                var departureDate = dtpDepartureDate.Value.Date;
                DateTime? returnDate = chkRoundTrip.Checked ? dtpReturnDate.Value.Date : (DateTime?)null;

                // Filter by route and date
                var filteredFlights = allFlights.Where(f => 
                    f.DepartureTime.HasValue && 
                    f.DepartureTime.Value.Date >= departureDate &&
                    f.Status == FlightStatus.SCHEDULED
                ).ToList();

                // Display results
                DisplayFlightsWithDetails(filteredFlights);

                if (table.Rows.Count == 0) {
                    MessageBox.Show("Không tìm thấy chuyến bay phù hợp!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e) {
            txtFlightNumber.Text = string.Empty;
            cbDepartureAirport.SelectedIndex = 0;
            cbArrivalAirport.SelectedIndex = 0;
            dtpDepartureDate.Value = DateTime.Now;
            dtpReturnDate.Value = DateTime.Now;
            txtAdults.Text = string.Empty;
            txtChildren.Text = string.Empty;
            chkRoundTrip.Checked = false;
            LoadFlights();
        }

        private void DisplayFlights(List<FlightDTO> flights) {
            table.Rows.Clear();
            foreach (var flight in flights) {
                var row = new DataGridViewRow();
                row.CreateCells(table);
                
                row.Cells[0].Value = flight.FlightNumber;
                row.Cells[1].Value = $"Airport {flight.RouteId}";
                row.Cells[2].Value = $"Airport {flight.AircraftId}";
                row.Cells[3].Value = flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
                row.Cells[4].Value = flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
                row.Cells[5].Value = "Available";
                row.Cells[6].Value = flight.Status.GetDescription();
                row.Cells[7].Value = "";
                row.Cells[8].Value = flight.FlightId;

                table.Rows.Add(row);
            }
        }

        private void DisplayFlightsWithDetails(List<FlightWithDetailsDTO> flights) {
            table.Rows.Clear();
            foreach (var flight in flights) {
                var row = new DataGridViewRow();
                row.CreateCells(table);
                
                row.Cells[0].Value = flight.FlightNumber;
                row.Cells[1].Value = flight.DepartureAirportDisplay;
                row.Cells[2].Value = flight.ArrivalAirportDisplay;
                row.Cells[3].Value = flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
                row.Cells[4].Value = flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
                row.Cells[5].Value = flight.AvailableSeats.ToString();
                row.Cells[6].Value = flight.Status.GetDescription();
                row.Cells[7].Value = "";
                row.Cells[8].Value = flight.FlightId;

                table.Rows.Add(row);
            }
        }

        // === Helpers for Action column ===
        private (Rectangle rcView, Rectangle rcAction1, Rectangle rcAction2) GetActionRects(Rectangle cellBounds, Font font) {
            int padding = 6;
            int x = cellBounds.Left + padding;
            int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;

            var flags = TextFormatFlags.NoPadding;
            var szView = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szSep = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);

            var rcView = new Rectangle(new Point(x, y), szView); 
            x += szView.Width + szSep.Width;

            Rectangle rcAction1, rcAction2;
            
            if (IsAdmin()) {
                // Admin: Xem | Sửa | Xóa
                var szEdit = TextRenderer.MeasureText(TXT_EDIT, font, Size.Empty, flags);
                var szDelete = TextRenderer.MeasureText(TXT_DELETE, font, Size.Empty, flags);
                
                rcAction1 = new Rectangle(new Point(x, y), szEdit); 
                x += szEdit.Width + szSep.Width;
                rcAction2 = new Rectangle(new Point(x, y), szDelete);
            } else if (IsStaff()) {
                // Staff: Xem | Sửa | Đặt vé
                var szEdit = TextRenderer.MeasureText(TXT_EDIT, font, Size.Empty, flags);
                var szBook = TextRenderer.MeasureText(TXT_BOOK, font, Size.Empty, flags);
                
                rcAction1 = new Rectangle(new Point(x, y), szEdit); 
                x += szEdit.Width + szSep.Width;
                rcAction2 = new Rectangle(new Point(x, y), szBook);
            } else {
                // Customer: Xem | Đặt vé
                var szBook = TextRenderer.MeasureText(TXT_BOOK, font, Size.Empty, flags);
                rcAction1 = new Rectangle(new Point(x, y), szBook);
                rcAction2 = Rectangle.Empty;
            }

            return (rcView, rcAction1, rcAction2);
        }

        private void Table_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) return;

            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var font = e.CellStyle.Font ?? table.Font;
            var rects = GetActionRects(e.CellBounds, font);

            Color link = Color.FromArgb(0, 92, 175);
            Color sep = Color.FromArgb(120, 120, 120);
            Color deleteColor = Color.FromArgb(220, 53, 69); // Red for delete
            Color bookColor = Color.FromArgb(34, 197, 94); // Green for booking

            // Xem (all roles)
            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, rects.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rects.rcView.Right, rects.rcView.Top), sep, TextFormatFlags.NoPadding);

            if (IsAdmin()) {
                // Admin: Sửa | Xóa
                TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, rects.rcAction1.Location, link, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rects.rcAction1.Right, rects.rcAction1.Top), sep, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, TXT_DELETE, font, rects.rcAction2.Location, deleteColor, TextFormatFlags.NoPadding);
            } else if (IsStaff()) {
                // Staff: Sửa | Đặt vé
                TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, rects.rcAction1.Location, link, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rects.rcAction1.Right, rects.rcAction1.Top), sep, TextFormatFlags.NoPadding);
                TextRenderer.DrawText(e.Graphics, TXT_BOOK, font, rects.rcAction2.Location, bookColor, TextFormatFlags.NoPadding);
            } else {
                // Customer: Đặt vé
                TextRenderer.DrawText(e.Graphics, TXT_BOOK, font, rects.rcAction1.Location, bookColor, TextFormatFlags.NoPadding);
            }
        }

        private void Table_CellMouseMove(object? sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) { table.Cursor = Cursors.Default; return; }

            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(cellRect, font);

            var p = new Point(e.Location.X + cellRect.Left, e.Location.Y + cellRect.Top);
            bool over = rects.rcView.Contains(p) || 
                       rects.rcAction1.Contains(p) || 
                       (!rects.rcAction2.IsEmpty && rects.rcAction2.Contains(p));
            table.Cursor = over ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) return;

            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(cellRect, font);
            var p = new Point(e.Location.X + cellRect.Left, e.Location.Y + cellRect.Top);

            var row = table.Rows[e.RowIndex];

            string flightId = row.Cells[8].Value?.ToString() ?? string.Empty;
            string flightNumber = row.Cells[0].Value?.ToString() ?? "(n/a)";
            string fromAirport = row.Cells[1].Value?.ToString() ?? "(n/a)";
            string toAirport = row.Cells[2].Value?.ToString() ?? "(n/a)";
            string departureTime = row.Cells[3].Value?.ToString() ?? "(n/a)";
            string arrivalTime = row.Cells[4].Value?.ToString() ?? "(n/a)";
            string seatAvailable = row.Cells[5].Value?.ToString() ?? "(n/a)";

            if (rects.rcView.Contains(p)) {
                // Xem chi tiết (all roles)
                using (var frm = new FlightDetailForm(flightNumber, fromAirport, toAirport, departureTime, arrivalTime, seatAvailable)) {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(FindForm());
                }
            } else if (rects.rcAction1.Contains(p)) {
                if (IsAdmin() || IsStaff()) {
                    // Admin/Staff: Sửa
                    EditFlight(flightId);
                } else {
                    // Customer: Đặt vé
                    BookFlight(flightId);
                }
            } else if (!rects.rcAction2.IsEmpty && rects.rcAction2.Contains(p)) {
                if (IsAdmin()) {
                    // Admin: Xóa
                    DeleteFlight(flightId, flightNumber);
                } else if (IsStaff()) {
                    // Staff: Đặt vé
                    BookFlight(flightId);
                }
            }
        }

        /// <summary>
        /// Sửa chuyến bay (Admin/Staff only)
        /// </summary>
        private void EditFlight(string flightId) {
            if (string.IsNullOrEmpty(flightId)) return;
            
            try {
                var flight = FlightBUS.Instance.GetFlightById(int.Parse(flightId));
                if (flight == null) {
                    MessageBox.Show("Không tìm thấy chuyến bay!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Open edit form or navigate to edit control
                var parentForm = FindForm();
                if (parentForm != null) {
                    // Find FlightControl parent
                    var flightControl = FindParentControl<FlightControl>(this);
                    if (flightControl != null) {
                        flightControl.ShowCreateForm(flight);
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Lỗi khi mở form sửa: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xóa chuyến bay (Admin/Staff only)
        /// </summary>
        private void DeleteFlight(string flightId, string flightNumber) {
            if (string.IsNullOrEmpty(flightId)) return;

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa chuyến bay {flightNumber}?\n\nThao tác này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try {
                var success = FlightBUS.Instance.DeleteFlight(int.Parse(flightId), out string message);
                if (success) {
                    MessageBox.Show("Xóa chuyến bay thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadFlights(); // Reload table
                } else {
                    MessageBox.Show($"Không thể xóa chuyến bay:\n{message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } catch (Exception ex) {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Mở form chọn hạng vé cho chuyến bay (Customer only)
        /// NOTE: Sau khi user xác nhận, sử dụng event BookingRequested để nhận:
        /// - int flightId: ID chuyến bay
        /// - string cabinClass: Hạng vé ("Economy" hoặc "Business")
        /// Sau đó chuyển user sang trang Đặt vé với dữ liệu này.
        /// </summary>
        private void BookFlight(string flightId) {
            if (string.IsNullOrEmpty(flightId)) return;
            
            try {
                var flight = FlightBUS.Instance.GetFlightById(int.Parse(flightId));
                if (flight == null) {
                    MessageBox.Show("Không tìm thấy chuyến bay!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Mở form chọn hạng vé
                using (var bookingForm = new BookingClassSelectionForm(flight)) {
                    bookingForm.StartPosition = FormStartPosition.CenterParent;
                    var result = bookingForm.ShowDialog(FindForm());
                    
                    if (result == DialogResult.OK) {
                        // User đã xác nhận đặt vé
                        int selectedFlightId = bookingForm.SelectedFlightId;
                        string selectedCabinClass = bookingForm.SelectedCabinClass;
                        
                        // Raise event để MainForm hoặc parent control xử lý
                        BookingRequested?.Invoke(selectedFlightId, selectedCabinClass);
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Lỗi khi đặt vé: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private T? FindParentControl<T>(Control child) where T : Control {
            var parent = child.Parent;
            while (parent != null) {
                if (parent is T result) return result;
                parent = parent.Parent;
            }
            return null;
        }

        private class ComboBoxItem {
            public string Text { get; set; } = string.Empty;
            public object Value { get; set; } = 0;
            public override string ToString() => Text;
        }
    }

    // Popup form bọc FlightDetailControl + nạp dữ liệu
    internal class FlightDetailForm : Form {
        public FlightDetailForm(string flightNumber, string fromAirport, string toAirport, string departureTime, string arrivalTime, string seatAvailable) {
            Text = $"Chi tiết chuyến bay {flightNumber}";
            Size = new Size(900, 600);
            BackColor = Color.White;

            var detail = new FlightDetailControl { Dock = DockStyle.Fill };
            detail.LoadFlightInfo(flightNumber, fromAirport, toAirport, departureTime, arrivalTime, seatAvailable);

            Controls.Add(detail);
        }
    }
}
