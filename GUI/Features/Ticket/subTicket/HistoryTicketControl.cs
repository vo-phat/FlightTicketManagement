using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS.Ticket;
using DTO.Ticket;
namespace GUI.Features.Ticket.subTicket
{
    public partial class HistoryTicketControl : UserControl
    {
        private readonly TicketsHistoryBUS _ticketBus;
        private List<TicketHistoryDTO> _allTickets;
        private int _accountId = 2; // TODO: lấy account thực từ login

        public HistoryTicketControl()
        {
            InitializeComponent();

            _ticketBus = new TicketsHistoryBUS();

            Load += HistoryTicketControl_Load;
            btnFilter.Click += btnFilter_click;
        }

        private void HistoryTicketControl_Load(object sender, EventArgs e)
        {
            InitStatusCombo();
            LoadTicketHistory();
            SetupGridColumns();
        }

        private void InitStatusCombo()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Tất cả");
            cbStatus.Items.Add("Upcoming");
            cbStatus.Items.Add("Completed");
            cbStatus.Items.Add("Cancelled");
            cbStatus.SelectedIndex = 0;
        }

        private void LoadTicketHistory()
        {
            _allTickets = _ticketBus.GetAll(_accountId);
            dgvTickets.DataSource = _allTickets;
        }

        private void SetupGridColumns()
        {
            dgvTickets.AutoGenerateColumns = false;
            dgvTickets.Columns.Clear();

            dgvTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TicketNumber",
                HeaderText = "Mã vé",
                Width = 120
            });

            dgvTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PassengerName",
                HeaderText = "Hành khách",
                Width = 150
            });

            dgvTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FlightCode",
                HeaderText = "Mã chuyến bay",
                Width = 110
            });

            dgvTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DepartureAirport",
                HeaderText = "Sân bay đi",
                Width = 100
            });

            dgvTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ArrivalAirport",
                HeaderText = "Sân bay đến",
                Width = 100
            });

            dgvTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DepartureTime",
                HeaderText = "Ngày giờ bay",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });

            dgvTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SeatCode",
                HeaderText = "Ghế",
                Width = 60
            });

            dgvTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Status",
                HeaderText = "Trạng thái",
                Width = 100
            });

            dgvTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BaggageSummary",
                HeaderText = "Hành lý",
                Width = 200
            });
        }

        private void btnFilter_click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var result = _allTickets;

            // --- Lọc theo Mã vé ---
            string ticketNumber = txtTicketNumber.Text.Trim();
            if (!string.IsNullOrWhiteSpace(ticketNumber))
            {
                result = result
                    .Where(c => c.TicketNumber.Contains(ticketNumber, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // --- Lọc theo Trạng thái ---
            string status = cbStatus.SelectedItem.ToString();
            if (status != "Tất cả")
            {
                result = result
                    .Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // --- Lọc theo ngày bay ---
            DateTime from = dtFrom.Value.Date;
            DateTime to = dtTo.Value.Date;

            result = result
                .Where(c => c.DepartureTime.Date >= from && c.DepartureTime.Date <= to)
                .ToList();

            dgvTickets.DataSource = result;
        }

        private void tableCustom1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Không cần xử lý gì ở đây
        }
    }
}
