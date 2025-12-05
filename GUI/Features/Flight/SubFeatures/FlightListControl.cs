using BUS.Auth;
using BUS.Flight;
using DTO.Auth;
using DTO.Flight;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Flight.SubFeatures
{
    public class FlightListControl : UserControl
    {
        private readonly FlightBUS _bus = FlightBUS.Instance;
        private DataGridView table = null!;
        private UnderlinedTextField txtFlightNumber = null!;
        private UnderlinedComboBox cbDepartureAirport = null!;
        private UnderlinedComboBox cbArrivalAirport = null!;
        private UnderlinedComboBox cbStatus = null!;
        private DateTimePickerCustom dtpDeparture = null!;
        private Button btnSearch = null!;
        private Button btnClear = null!;
        private const string ACTION_COL = "Actions";
        private const string TXT_VIEW = "Xem";
        private const string TXT_BOOK = "Đặt vé";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " | ";
        private int _hoveredRow = -1;
        private int _hoveredAction = -1; // 0=View, 1=Book, 2=Edit, 3=Delete

        public event Action<FlightWithDetailsDTO>? ViewRequested;
        public event Action<DTO.Booking.BookingRequestDTO>? NavigateToBookingRequested;
        public event Action<FlightWithDetailsDTO>? RequestEdit;
        public event Action? DataChanged;

        private List<FlightWithDetailsDTO> _allFlights = new List<FlightWithDetailsDTO>();
        private List<DTO.Booking.BookingRequestDTO> _confirmedBookings = new List<DTO.Booking.BookingRequestDTO>();

        public FlightListControl()
        {
            InitializeComponent();
            LoadComboBoxData();
            RefreshList();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(232, 240, 252);
            Dock = DockStyle.Fill;
            AutoScroll = true;

            // === TIÊU ĐỀ ===
            var lblTitle = new Label
            {
                Text = "✈️ Danh sách chuyến bay",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(40, 55, 77),
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(24, 20, 0, 12)
            };

            // === PANEL BỘ LỌC ===
            var filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 8, 24, 8),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.FromArgb(250, 253, 255)
            };

            // --- INPUTS TÙY CHỈNH ---
            txtFlightNumber = new UnderlinedTextField("Số hiệu chuyến bay", "VN123")
            {
                Width = 180,
                Margin = new Padding(6, 4, 6, 4),
                InheritParentBackColor = true,
                LineThickness = 1
            };
            
            // Realtime search when user types
            txtFlightNumber.TextChanged += TxtFlightNumber_TextChanged;

            cbDepartureAirport = new UnderlinedComboBox("Sân bay đi", Array.Empty<string>())
            {
                Width = 200,
                Margin = new Padding(6, 4, 6, 4),
            };

            cbArrivalAirport = new UnderlinedComboBox("Sân bay đến", Array.Empty<string>())
            {
                Width = 200,
                Margin = new Padding(6, 4, 6, 4),
            };

            cbStatus = new UnderlinedComboBox("Trạng thái", new string[] 
            { 
                "Tất cả", 
                "Đã lên lịch", 
                "Đang bay", 
                "Đã hạ cánh", 
                "Đã hủy", 
                "Trì hoãn" 
            })
            {
                Width = 160,
                Margin = new Padding(6, 4, 6, 4),
            };
            cbStatus.SelectedIndex = 0;

            dtpDeparture = new DateTimePickerCustom("Ngày khởi hành", "")
            {
                Width = 180,
                Margin = new Padding(6, 4, 6, 4)
            };

            // --- BUTTONS TÙY CHỈNH ---
            btnSearch = new PrimaryButton("🔍 Tìm kiếm")
            {
                Width = 100,
                Height = 40,
                Margin = new Padding(10, 6, 6, 6),
            };

            btnClear = new SecondaryButton("🔄 Làm mới")
            {
                Width = 100,
                Height = 40,
                Margin = new Padding(6),
            };

            btnSearch.Click += (s, e) => RefreshList();
            btnClear.Click += (s, e) => ClearFilters();

            filterPanel.Controls.AddRange(new Control[] 
            { 
                txtFlightNumber, 
                cbDepartureAirport, 
                cbArrivalAirport, 
                cbStatus, 
                dtpDeparture,
                btnSearch, 
                btnClear 
            });

            // === BẢNG DANH SÁCH TÙY CHỈNH ===
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                CornerRadius = 16,
                BorderThickness = 2,
                BorderColor = Color.FromArgb(200, 200, 200),
            };

            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "FlightId", Name = "FlightId", Visible = false });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số hiệu", DataPropertyName = "FlightNumber", Name = "FlightNumber", Width = 100 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Sân bay đi", DataPropertyName = "DepartureAirportDisplay", Name = "DepartureAirport", Width = 180 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Sân bay đến", DataPropertyName = "ArrivalAirportDisplay", Name = "ArrivalAirport", Width = 180 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Giờ khởi hành", DataPropertyName = "DepartureTime", Name = "DepartureTime", Width = 140 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Giờ đến", DataPropertyName = "ArrivalTime", Name = "ArrivalTime", Width = 140 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ghi chú", DataPropertyName = "Note", Name = "Note", Width = 200 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng thái", DataPropertyName = "Status", Name = "Status", Width = 120 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ghế trống", DataPropertyName = "AvailableSeats", Name = "AvailableSeats", Width = 90 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = ACTION_COL, Name = ACTION_COL, Width = 150 });

            table.CellFormatting += Table_CellFormatting;
            table.CellClick += Table_CellClick;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseLeave += Table_CellMouseLeave;

            // Wrap table in panel for margin
            var tablePanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24, 12, 24, 24),
                BackColor = Color.Transparent
            };
            tablePanel.Controls.Add(table);

            Controls.Add(tablePanel);
            Controls.Add(filterPanel);
            Controls.Add(lblTitle);

            ResumeLayout(false);
            PerformLayout();
        }

        private void LoadComboBoxData()
        {
            try
            {
                // Load airports - using Airport BUS
                var airportBus = new BUS.Airport.AirportBUS();
                var airports = airportBus.GetAllAirports();
                
                var airportDisplayList = new List<string> { "Tất cả" };
                airportDisplayList.AddRange(airports.Select(a => $"{a.AirportCode} - {a.AirportName}"));

                cbDepartureAirport.Items.Clear();
                cbDepartureAirport.Items.AddRange(airportDisplayList.ToArray());
                cbDepartureAirport.SelectedIndex = 0;

                cbArrivalAirport.Items.Clear();
                cbArrivalAirport.Items.AddRange(airportDisplayList.ToArray());
                cbArrivalAirport.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu combobox: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        
        private void TxtFlightNumber_TextChanged(object? sender, EventArgs e)
        {
            // Auto-refresh when user types (debounce could be added for performance)
            if (txtFlightNumber.Text.Length >= 2 || string.IsNullOrWhiteSpace(txtFlightNumber.Text))
            {
                RefreshList();
            }
        }

        public void RefreshList()
        {
            try
            {
                _allFlights = _bus.GetAllFlightsWithDetails();
                
                // Apply filters
                var filtered = _allFlights.AsEnumerable();

                // Filter by flight number - optimized search
                if (!string.IsNullOrWhiteSpace(txtFlightNumber.Text))
                {
                    var searchText = txtFlightNumber.Text?.Trim().ToUpperInvariant().Replace(" ", "").Replace("-", "") ?? "";
                    filtered = filtered.Where(f => 
                    {
                        var flightNum = (f.FlightNumber ?? "").ToUpperInvariant().Replace(" ", "").Replace("-", "");
                        return flightNum.Contains(searchText) || flightNum.StartsWith(searchText);
                    });
                }

                // Filter by departure airport
                if (cbDepartureAirport.SelectedIndex > 0)
                {
                    var selectedAirport = cbDepartureAirport.SelectedItem?.ToString()?.Split('-')[0].Trim() ?? "";
                    filtered = filtered.Where(f => f.DepartureAirportCode == selectedAirport);
                }

                // Filter by arrival airport
                if (cbArrivalAirport.SelectedIndex > 0)
                {
                    var selectedAirport = cbArrivalAirport.SelectedItem?.ToString()?.Split('-')[0].Trim() ?? "";
                    filtered = filtered.Where(f => f.ArrivalAirportCode == selectedAirport);
                }

                // Filter by status
                if (cbStatus.SelectedIndex > 0)
                {
                    var statusText = cbStatus.SelectedItem.ToString();
                    FlightStatus status;
                    switch (statusText)
                    {
                        case "Đã lên lịch":
                            status = FlightStatus.SCHEDULED;
                            break;
                        case "Đang bay":
                            status = FlightStatus.SCHEDULED;
                            break;
                        case "Đã hạ cánh":
                            status = FlightStatus.COMPLETED;
                            break;
                        case "Đã hủy":
                            status = FlightStatus.CANCELLED;
                            break;
                        case "Trì hoãn":
                            status = FlightStatus.DELAYED;
                            break;
                        default:
                            status = FlightStatus.SCHEDULED;
                            break;
                    }
                    filtered = filtered.Where(f => f.Status == status);
                }

                // Filter by departure date - skip for now as DateTimePickerCustom doesn't have Checked
                if (false)
                {
                    var selectedDate = dtpDeparture.Value.Date;
                    filtered = filtered.Where(f => f.DepartureTime.HasValue && 
                                                   f.DepartureTime.Value.Date == selectedDate);
                }

                // Update table
                table.Rows.Clear();
                foreach (var flight in filtered.OrderByDescending(f => f.DepartureTime))
                {
                    int rowIdx = table.Rows.Add();
                    var row = table.Rows[rowIdx];
                    
                    row.Cells["FlightId"].Value = flight.FlightId;
                    row.Cells["FlightNumber"].Value = flight.FlightNumber;
                    row.Cells["DepartureAirport"].Value = flight.DepartureAirportDisplay;
                    row.Cells["ArrivalAirport"].Value = flight.ArrivalAirportDisplay;
                    row.Cells["DepartureTime"].Value = flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm");
                    row.Cells["ArrivalTime"].Value = flight.ArrivalTime?.ToString("HH:mm");
                    row.Cells["Status"].Value = GetStatusText(flight.Status);
                    row.Cells["AvailableSeats"].Value = flight.AvailableSeats;
                    
                    // Phân quyền actions theo role
                    string actions = "";
                    if (UserSession.CurrentAppRole == AppRole.User)
                    {
                        // User: Xem và Đặt vé (nếu chuyến bay đã lên lịch)
                        actions = flight.Status == FlightStatus.SCHEDULED 
                            ? $"{TXT_VIEW}{SEP}{TXT_BOOK}"
                            : TXT_VIEW;
                    }
                    else if (UserSession.CurrentAppRole == AppRole.Staff)
                    {
                        // Staff: Xem, Đặt vé (nếu lên lịch), Sửa, Xóa
                        actions = flight.Status == FlightStatus.SCHEDULED
                            ? $"{TXT_VIEW}{SEP}{TXT_BOOK}{SEP}{TXT_EDIT}{SEP}{TXT_DEL}"
                            : $"{TXT_VIEW}{SEP}{TXT_EDIT}{SEP}{TXT_DEL}";
                    }
                    else // Admin
                    {
                        // Admin: Xem, Sửa, Xóa (không đặt vé)
                        actions = $"{TXT_VIEW}{SEP}{TXT_EDIT}{SEP}{TXT_DEL}";
                    }
                    
                    row.Cells[ACTION_COL].Value = actions;
                    row.Tag = flight;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chuyến bay: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFilters()
        {
            txtFlightNumber.Text = "";
            cbDepartureAirport.SelectedIndex = 0;
            cbArrivalAirport.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
            RefreshList();
        }

        private string GetStatusText(FlightStatus status)
        {
            return status switch
            {
                FlightStatus.SCHEDULED => "Đã lên lịch",
                FlightStatus.COMPLETED => "Hoàn thành",
                FlightStatus.CANCELLED => "Đã hủy",
                FlightStatus.DELAYED => "Trì hoãn",
                _ => "Không xác định"
            };
        }

        private void Table_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (table.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                var statusText = e.Value.ToString();
                var cell = table.Rows[e.RowIndex].Cells[e.ColumnIndex];
                
                switch (statusText)
                {
                    case "Đã lên lịch":
                        cell.Style.BackColor = Color.LightBlue;
                        cell.Style.ForeColor = Color.DarkBlue;
                        break;
                    case "Đang bay":
                        cell.Style.BackColor = Color.LightGreen;
                        cell.Style.ForeColor = Color.DarkGreen;
                        break;
                    case "Đã hạ cánh":
                        cell.Style.BackColor = Color.LightGray;
                        cell.Style.ForeColor = Color.Black;
                        break;
                    case "Đã hủy":
                        cell.Style.BackColor = Color.LightCoral;
                        cell.Style.ForeColor = Color.DarkRed;
                        break;
                    case "Trì hoãn":
                        cell.Style.BackColor = Color.LightYellow;
                        cell.Style.ForeColor = Color.DarkOrange;
                        break;
                }
            }

            // Format Note column - truncate if too long
            if (table.Columns[e.ColumnIndex].Name == "Note" && e.Value != null)
            {
                var note = e.Value.ToString();
                if (!string.IsNullOrEmpty(note) && note.Length > 50)
                {
                    e.Value = note.Substring(0, 47) + "...";
                    e.FormattingApplied = true;
                }
            }

            if (table.Columns[e.ColumnIndex].Name == ACTION_COL)
            {
                e.CellStyle.ForeColor = Color.Blue;
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Underline);
            }
        }

        private void Table_CellMouseMove(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            
            var colName = table.Columns[e.ColumnIndex].Name;
            if (colName != ACTION_COL)
            {
                table.Cursor = Cursors.Default;
                return;
            }

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            var mouseX = e.X;
            
            // Calculate button positions with padding
            const int padding = 8;
            var startX = padding;
            
            using (var g = table.CreateGraphics())
            {
                var viewSize = g.MeasureString(TXT_VIEW, table.Font);
                var bookSize = g.MeasureString(TXT_BOOK, table.Font);
                var editSize = g.MeasureString(TXT_EDIT, table.Font);
                var delSize = g.MeasureString(TXT_DEL, table.Font);
                var sepSize = g.MeasureString(SEP, table.Font);
                
                var viewEnd = startX + viewSize.Width;
                
                int newHoveredAction = -1;
                
                if (UserSession.CurrentAppRole == AppRole.User)
                {
                    // User: Xem | Đặt vé (nếu có)
                    var bookStart = viewEnd + sepSize.Width;
                    var bookEnd = bookStart + bookSize.Width;
                    
                    if (mouseX >= startX && mouseX <= viewEnd)
                    {
                        newHoveredAction = 0; // View
                    }
                    else if (mouseX >= bookStart && mouseX <= bookEnd)
                    {
                        newHoveredAction = 1; // Book
                    }
                }
                else if (UserSession.CurrentAppRole == AppRole.Staff)
                {
                    // Staff: Xem | Đặt vé | Sửa | Xóa
                    var bookStart = viewEnd + sepSize.Width;
                    var bookEnd = bookStart + bookSize.Width;
                    var editStart = bookEnd + sepSize.Width;
                    var editEnd = editStart + editSize.Width;
                    var delStart = editEnd + sepSize.Width;
                    var delEnd = delStart + delSize.Width;
                    
                    if (mouseX >= startX && mouseX <= viewEnd)
                    {
                        newHoveredAction = 0; // View
                    }
                    else if (mouseX >= bookStart && mouseX <= bookEnd)
                    {
                        newHoveredAction = 1; // Book
                    }
                    else if (mouseX >= editStart && mouseX <= editEnd)
                    {
                        newHoveredAction = 2; // Edit
                    }
                    else if (mouseX >= delStart && mouseX <= delEnd)
                    {
                        newHoveredAction = 3; // Delete
                    }
                }
                else // Admin
                {
                    // Admin: Xem | Sửa | Xóa
                    var editStart = viewEnd + sepSize.Width;
                    var editEnd = editStart + editSize.Width;
                    var delStart = editEnd + sepSize.Width;
                    var delEnd = delStart + delSize.Width;
                    
                    if (mouseX >= startX && mouseX <= viewEnd)
                    {
                        newHoveredAction = 0; // View
                    }
                    else if (mouseX >= editStart && mouseX <= editEnd)
                    {
                        newHoveredAction = 2; // Edit
                    }
                    else if (mouseX >= delStart && mouseX <= delEnd)
                    {
                        newHoveredAction = 3; // Delete
                    }
                }
                
                if (newHoveredAction != -1)
                {
                    table.Cursor = Cursors.Hand;
                    if (_hoveredRow != e.RowIndex || _hoveredAction != newHoveredAction)
                    {
                        _hoveredRow = e.RowIndex;
                        _hoveredAction = newHoveredAction;
                        table.InvalidateRow(e.RowIndex);
                    }
                }
                else
                {
                    table.Cursor = Cursors.Default;
                    if (_hoveredRow == e.RowIndex)
                    {
                        _hoveredRow = -1;
                        _hoveredAction = -1;
                        table.InvalidateRow(e.RowIndex);
                    }
                }
            }
        }

        private void Table_CellMouseLeave(object? sender, DataGridViewCellEventArgs e)
        {
            if (_hoveredRow != -1)
            {
                var oldRow = _hoveredRow;
                _hoveredRow = -1;
                _hoveredAction = -1;
                table.Cursor = Cursors.Default;
                if (oldRow >= 0 && oldRow < table.Rows.Count)
                {
                    table.InvalidateRow(oldRow);
                }
            }
        }

        private void Table_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var colName = table.Columns[e.ColumnIndex].Name;
            if (colName != ACTION_COL) return;

            var row = table.Rows[e.RowIndex];
            var flight = row.Tag as FlightWithDetailsDTO;
            if (flight == null) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            var clickPoint = table.PointToClient(Cursor.Position);
            var clickX = clickPoint.X - rect.X;
            
            // Calculate button positions with padding
            const int padding = 8;
            var startX = padding;
            
            using (var g = table.CreateGraphics())
            {
                var viewSize = g.MeasureString(TXT_VIEW, table.Font);
                var bookSize = g.MeasureString(TXT_BOOK, table.Font);
                var editSize = g.MeasureString(TXT_EDIT, table.Font);
                var delSize = g.MeasureString(TXT_DEL, table.Font);
                var sepSize = g.MeasureString(SEP, table.Font);
                
                var viewEnd = startX + viewSize.Width;
                
                if (UserSession.CurrentAppRole == AppRole.User)
                {
                    // User: Xem | Đặt vé
                    var bookStart = viewEnd + sepSize.Width;
                    var bookEnd = bookStart + bookSize.Width;
                    
                    if (clickX >= startX && clickX <= viewEnd)
                    {
                        ViewRequested?.Invoke(flight);
                    }
                    else if (clickX >= bookStart && clickX <= bookEnd)
                    {
                        HandleBookFlight(flight);
                    }
                }
                else if (UserSession.CurrentAppRole == AppRole.Staff)
                {
                    // Staff: Xem | Đặt vé | Sửa | Xóa
                    var bookStart = viewEnd + sepSize.Width;
                    var bookEnd = bookStart + bookSize.Width;
                    var editStart = bookEnd + sepSize.Width;
                    var editEnd = editStart + editSize.Width;
                    var delStart = editEnd + sepSize.Width;
                    var delEnd = delStart + delSize.Width;
                    
                    if (clickX >= startX && clickX <= viewEnd)
                    {
                        ViewRequested?.Invoke(flight);
                    }
                    else if (clickX >= bookStart && clickX <= bookEnd)
                    {
                        HandleBookFlight(flight);
                    }
                    else if (clickX >= editStart && clickX <= editEnd)
                    {
                        RequestEdit?.Invoke(flight);
                    }
                    else if (clickX >= delStart && clickX <= delEnd)
                    {
                        HandleDelete(flight);
                    }
                }
                else // Admin
                {
                    // Admin: Xem | Sửa | Xóa
                    var editStart = viewEnd + sepSize.Width;
                    var editEnd = editStart + editSize.Width;
                    var delStart = editEnd + sepSize.Width;
                    var delEnd = delStart + delSize.Width;
                    
                    if (clickX >= startX && clickX <= viewEnd)
                    {
                        ViewRequested?.Invoke(flight);
                    }
                    else if (clickX >= editStart && clickX <= editEnd)
                    {
                        RequestEdit?.Invoke(flight);
                    }
                    else if (clickX >= delStart && clickX <= delEnd)
                    {
                        HandleDelete(flight);
                    }
                }
            }
        }

        private void HandleBookFlight(FlightWithDetailsDTO flight)
        {
            // Kiểm tra chuyến bay có thể đặt không
            if (flight.Status != FlightStatus.SCHEDULED)
            {
                MessageBox.Show("Chỉ có thể đặt vé cho chuyến bay đã lên lịch.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (flight.AvailableSeats <= 0)
            {
                MessageBox.Show("Chuyến bay này đã hết chỗ.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị dialog chọn hạng vé
            using (var dialog = new CabinClassSelectionDialog(flight))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Chuyển sang trang Thông tin khách hàng với thông tin đặt vé
                    if (dialog.BookingRequest != null)
                    {
                        // Lưu thông tin booking đã xác nhận
                        _confirmedBookings.Add(dialog.BookingRequest);
                        
                        NavigateToBookingRequested?.Invoke(dialog.BookingRequest);
                    }
                }
            }
        }

        private void HandleDelete(FlightWithDetailsDTO flight)
        {
            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn ẩn chuyến bay {flight.FlightNumber}?\n" +
                $"Chuyến bay sẽ không còn hiển thị trong danh sách.",
                "Xác nhận ẩn chuyến bay",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (_bus.DeleteFlight(flight.FlightId, out string message))
                {
                    MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshList();
                    DataChanged?.Invoke();
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Lấy danh sách các booking đã xác nhận
        /// </summary>
        public List<DTO.Booking.BookingRequestDTO> ConfirmedBooking()
        {
            return new List<DTO.Booking.BookingRequestDTO>(_confirmedBookings);
        }

        /// <summary>
        /// Xóa danh sách booking đã xác nhận
        /// </summary>
        public void ClearConfirmedBookings()
        {
            _confirmedBookings.Clear();
        }
    }
}
