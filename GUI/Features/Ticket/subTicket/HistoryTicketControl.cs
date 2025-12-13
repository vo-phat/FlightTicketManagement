using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BUS.Auth;
using BUS.Ticket;
using DTO.Ticket;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Ticket.subTicket
{
    public partial class HistoryTicketControl : UserControl
    {
        private readonly TicketsHistoryBUS _ticketBus;
        private List<TicketHistoryDTO> _allTickets;
        private int _accountId;
        
        // UI Components
        private DataGridView table = null!;
        private DateTimePickerCustom dtpFromDate = null!;
        private DateTimePickerCustom dtpToDate = null!;
        private CheckBox chkEnableDateFilter = null!;
        private Button btnClear = null!;
        
        private const string ACTION_COL = "Actions";
        private const string TXT_VIEW = "Xem chi tiết";
        private int _hoveredRow = -1;

        public HistoryTicketControl()
        {
            _ticketBus = new TicketsHistoryBUS();
            _accountId = UserSession.CurrentAccount?.AccountId ?? 0;
            _allTickets = new List<TicketHistoryDTO>();
            
            InitializeComponent();
            RefreshData();
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
                Text = "🎫 Lịch sử vé của tôi",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(40, 55, 77),
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(24, 20, 0, 12)
            };

            // === PANEL BỘ LỌC THEO NGÀY ===
            var filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 8, 24, 16),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.FromArgb(250, 253, 255)
            };

            // Date filter with checkbox
            var dateFilterPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = new Padding(6, 4, 6, 4)
            };

            chkEnableDateFilter = new CheckBox
            {
                Text = "Lọc theo khoảng ngày",
                AutoSize = true,
                Checked = false,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(70, 70, 70),
                Margin = new Padding(0, 0, 0, 4)
            };
            
            var datePickersPanel = new FlowLayoutPanel 
            { 
                FlowDirection = FlowDirection.LeftToRight, 
                AutoSize = true, 
                Margin = new Padding(0), 
                Padding = new Padding(0) 
            };

            dtpFromDate = new DateTimePickerCustom("Từ ngày", "")
            {
                Width = 170,
                Enabled = false
            };
            dtpFromDate.Value = DateTime.Today.AddMonths(-1);

            dtpToDate = new DateTimePickerCustom("Đến ngày", "")
            {
                Width = 170,
                Margin = new Padding(8, 0, 0, 0),
                Enabled = false
            };
            dtpToDate.Value = DateTime.Today;

            chkEnableDateFilter.CheckedChanged += (s, e) =>
            {
                dtpFromDate.Enabled = chkEnableDateFilter.Checked;
                dtpToDate.Enabled = chkEnableDateFilter.Checked;
            };

            datePickersPanel.Controls.Add(dtpFromDate);
            datePickersPanel.Controls.Add(dtpToDate);

            dateFilterPanel.Controls.Add(chkEnableDateFilter);
            dateFilterPanel.Controls.Add(datePickersPanel);

            // --- BUTTON LÀM MỚI ===
            btnClear = new SecondaryButton("🔄 Làm mới")
            {
                Width = 100,
                Height = 40,
                Margin = new Padding(10, 6, 6, 6),
            };

            btnClear.Click += (s, e) => ClearFilters();

            filterPanel.Controls.AddRange(new Control[] 
            { 
                dateFilterPanel,
                btnClear 
            });

            // === BẢNG DANH SÁCH ===
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

            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã vé", Name = "TicketNumber", Width = 120 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Hành khách", Name = "PassengerName", Width = 180 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Chuyến bay", Name = "FlightCode", Width = 100 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đi", Name = "DepartureAirport", Width = 100 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đến", Name = "ArrivalAirport", Width = 100 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Khởi hành", Name = "DepartureTime", Width = 140 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ghế", Name = "SeatCode", Width = 80 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng thái", Name = "Status", Width = 120 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Hành lý", Name = "BaggageSummary", Width = 150 });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = ACTION_COL, Name = ACTION_COL, Width = 120 });

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

        public void RefreshData()
        {
            try
            {
                if (_accountId == 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin tài khoản. Vui lòng đăng nhập lại.", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _allTickets = _ticketBus.GetAll(_accountId);
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch sử vé: {ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter()
        {
            try
            {
                var filtered = _allTickets.AsEnumerable();

                // Apply date filter if enabled
                if (chkEnableDateFilter.Checked)
                {
                    if (dtpFromDate.Value.Date > dtpToDate.Value.Date)
                    {
                        MessageBox.Show("Ngày bắt đầu không thể sau ngày kết thúc.", 
                            "Lỗi lọc ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    DateTime fromDate = dtpFromDate.Value.Date;
                    DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);
                    
                    filtered = filtered.Where(t => t.DepartureTime >= fromDate && t.DepartureTime <= toDate);
                }

                // Update table
                table.Rows.Clear();
                
                var resultList = filtered.ToList();
                
                foreach (var ticket in resultList)
                {
                    int rowIdx = table.Rows.Add();
                    var row = table.Rows[rowIdx];
                    
                    row.Cells["TicketNumber"].Value = ticket.TicketNumber ?? "-";
                    row.Cells["PassengerName"].Value = ticket.PassengerName ?? "-";
                    row.Cells["FlightCode"].Value = ticket.FlightCode ?? "-";
                    row.Cells["DepartureAirport"].Value = ticket.DepartureAirport ?? "-";
                    row.Cells["ArrivalAirport"].Value = ticket.ArrivalAirport ?? "-";
                    row.Cells["DepartureTime"].Value = ticket.DepartureTime.ToString("dd/MM/yyyy HH:mm");
                    row.Cells["SeatCode"].Value = ticket.SeatCode ?? "-";
                    row.Cells["Status"].Value = ticket.Status ?? "-";
                    row.Cells["BaggageSummary"].Value = ticket.BaggageSummary ?? "Không có";
                    row.Cells[ACTION_COL].Value = TXT_VIEW;
                    row.Tag = ticket;
                }
                
                if (resultList.Count == 0 && _allTickets.Count > 0)
                {
                    MessageBox.Show("Không tìm thấy vé phù hợp với điều kiện lọc.", 
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc dữ liệu: {ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFilters()
        {
            chkEnableDateFilter.Checked = false;
            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
            dtpToDate.Value = DateTime.Today;
            RefreshData();
        }

        private void Table_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            // Format Status column with colors
            if (table.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                var statusText = e.Value.ToString();
                var cell = table.Rows[e.RowIndex].Cells[e.ColumnIndex];
                
                switch (statusText)
                {
                    case "BOOKED":
                        cell.Style.BackColor = Color.LightBlue;
                        cell.Style.ForeColor = Color.DarkBlue;
                        break;
                    case "CONFIRMED":
                        cell.Style.BackColor = Color.LightGreen;
                        cell.Style.ForeColor = Color.DarkGreen;
                        break;
                    case "CHECKED_IN":
                        cell.Style.BackColor = Color.LightCyan;
                        cell.Style.ForeColor = Color.DarkCyan;
                        break;
                    case "BOARDED":
                        cell.Style.BackColor = Color.LightGray;
                        cell.Style.ForeColor = Color.Black;
                        break;
                    case "CANCELLED":
                        cell.Style.BackColor = Color.LightCoral;
                        cell.Style.ForeColor = Color.DarkRed;
                        break;
                    case "REFUNDED":
                        cell.Style.BackColor = Color.LightYellow;
                        cell.Style.ForeColor = Color.DarkOrange;
                        break;
                }
            }

            // Format Seat column with color
            if (table.Columns[e.ColumnIndex].Name == "SeatCode" && e.Value != null)
            {
                var cell = table.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.ForeColor = Color.SeaGreen;
                cell.Style.Font = new Font(table.Font, FontStyle.Bold);
            }

            // Format Action column
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
            if (colName == ACTION_COL)
            {
                table.Cursor = Cursors.Hand;
                if (_hoveredRow != e.RowIndex)
                {
                    _hoveredRow = e.RowIndex;
                    table.InvalidateRow(e.RowIndex);
                }
            }
            else
            {
                table.Cursor = Cursors.Default;
            }
        }

        private void Table_CellMouseLeave(object? sender, DataGridViewCellEventArgs e)
        {
            if (_hoveredRow != -1)
            {
                var oldRow = _hoveredRow;
                _hoveredRow = -1;
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
            var ticket = row.Tag as TicketHistoryDTO;
            if (ticket == null) return;

            // Show ticket details
            ShowTicketDetails(ticket);
        }

        private void ShowTicketDetails(TicketHistoryDTO ticket)
        {
            var details = $"═══════════════════════════════════════\n" +
                         $"📋 CHI TIẾT VÉ\n" +
                         $"═══════════════════════════════════════\n\n" +
                         $"🎫 Mã vé: {ticket.TicketNumber}\n" +
                         $"👤 Hành khách: {ticket.PassengerName}\n" +
                         $"✈️ Chuyến bay: {ticket.FlightCode}\n" +
                         $"📍 Lộ trình: {ticket.DepartureAirport} → {ticket.ArrivalAirport}\n" +
                         $"🕐 Khởi hành: {ticket.DepartureTime:dd/MM/yyyy HH:mm}\n" +
                         $"💺 Ghế: {ticket.SeatCode}\n" +
                         $"📊 Trạng thái: {ticket.Status}\n" +
                         $"🧳 Hành lý: {ticket.BaggageSummary}\n";

            MessageBox.Show(details, "Chi tiết vé", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}