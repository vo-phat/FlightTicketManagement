using BUS.Ticket;
using DTO.Ticket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Features.Ticket.subTicket
{
    public partial class TicketOpsControl : UserControl
    {
        TicketListBUS ticketListBUS;


        public TicketOpsControl()
        {
            InitializeComponent();

            // Setup ComboBox search
            cboSearchTicket.Items.AddRange(new string[] { "ALL", "BOOKED", "CANCELLED", "REFUNDED" });
            cboSearchTicket.SelectedIndex = 0;
            ticketListBUS = new TicketListBUS();

            // Load bảng lần đầu
            BuildTicketTable(ticketListBUS);
        }
        public void BuildTicketTable(TicketListBUS ticketListBUS)
        {
            // Lấy dữ liệu từ BUS
            List<TicketListDTO> data = ticketListBUS.GetAllTickets();

            // ==========================
            // 1. CẤU HÌNH GRID
            // ==========================
            dgvTicketOpsControl.Columns.Clear();
            dgvTicketOpsControl.AutoGenerateColumns = false;
            dgvTicketOpsControl.AllowUserToAddRows = false;
            dgvTicketOpsControl.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTicketOpsControl.RowHeadersVisible = false;
            dgvTicketOpsControl.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ==========================
            // 2. CỘT DỮ LIỆU
            // ==========================
            dgvTicketOpsControl.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TicketNumber",
                HeaderText = "Số vé",
                DataPropertyName = "TicketNumber",
                FillWeight = 12
            });

            dgvTicketOpsControl.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PassengerName",
                HeaderText = "Hành khách",
                DataPropertyName = "PassengerName",
                FillWeight = 16
            });

            dgvTicketOpsControl.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FlightNumber",
                HeaderText = "Chuyến bay",
                DataPropertyName = "FlightNumber",
                FillWeight = 10
            });

            dgvTicketOpsControl.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Route",
                HeaderText = "Hành trình",
                DataPropertyName = "Route",
                FillWeight = 18
            });

            dgvTicketOpsControl.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepartureTime",
                HeaderText = "Giờ bay",
                DataPropertyName = "DepartureTime",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM HH:mm" },
                FillWeight = 10
            });

            dgvTicketOpsControl.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SeatCode",
                HeaderText = "Ghế",
                DataPropertyName = "SeatCode",
                FillWeight = 7
            });

            dgvTicketOpsControl.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Price",
                HeaderText = "Giá",
                DataPropertyName = "Price",
                FillWeight = 10
            });

            dgvTicketOpsControl.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Trạng thái",
                DataPropertyName = "Status",
                FillWeight = 10
            });


            // ==========================
            // 3. CỘT HÀNH ĐỘNG
            // ==========================
            dgvTicketOpsControl.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "btnView",
                HeaderText = "",
                Text = "Xem",
                UseColumnTextForButtonValue = true,
                FillWeight = 6
            });

            dgvTicketOpsControl.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "btnCancel",
                HeaderText = "",
                Text = "Hủy",
                UseColumnTextForButtonValue = true,
                FillWeight = 6
            });

            dgvTicketOpsControl.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "btnRefund",
                HeaderText = "",
                Text = "Hoàn",
                UseColumnTextForButtonValue = true,
                FillWeight = 6
            });

            // ==========================
            // 4. GẮN DỮ LIỆU
            // ==========================
            dgvTicketOpsControl.DataSource = data;

            // ==========================
            // 5. EVENT CLICK
            // ==========================
            dgvTicketOpsControl.CellClick -= dgvListFilerTickets_CellClick;
            dgvTicketOpsControl.CellClick += dgvListFilerTickets_CellClick;
        }


        // ==================================================================
        // EVENT CLICK – Anh xử lý logic sau
        // ==================================================================
        private void dgvListFilerTickets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var dto = dgvTicketOpsControl.Rows[e.RowIndex].DataBoundItem as TicketListDTO;
            if (dto == null) return;

            string col = dgvTicketOpsControl.Columns[e.ColumnIndex].Name;

            if (col == "btnView")
            {
                MessageBox.Show($"Xem vé: {dto.TicketNumber}");
            }
            else if (col == "btnCancel")
            {
                MessageBox.Show("Nhấn Hủy vé — Anh xử lý logic tại đây.");
            }
            else if (col == "btnRefund")
            {
                MessageBox.Show("Nhấn Hoàn vé — Anh xử lý logic tại đây.");
            }
        }
        private void SearchTickets()
        {
            string keyword = txtSearchTicket.Text.Trim();
            string status = cboSearchTicket.Text;
            ticketListBUS = new TicketListBUS();
            List<TicketListDTO> list = ticketListBUS.SearchTickets(keyword, status);

            dgvTicketOpsControl.DataSource = list;
        }

        private void dgvTicketNumberHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void underlinedComboBox1_Load(object sender, EventArgs e)
        {
            //SearchTickets();
        }

        private void primaryButton1_Click(object sender, EventArgs e)
        {
            //SearchTickets();
        }

        private void txtSearchTicket_Load(object sender, EventArgs e)
        {
            //SearchTickets();
        }

        private void cbo_changedIndex(object sender, EventArgs e)
        {
            SearchTickets();
        }

        private void KeyUp_ticket(object sender, KeyEventArgs e)
        {
            SearchTickets();
        }
    }
}
