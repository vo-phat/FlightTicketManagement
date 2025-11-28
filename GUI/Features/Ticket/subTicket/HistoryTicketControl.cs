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
        public List<TicketHistoryDTO> listhistory;
        public HistoryTicketControl()
        {
            InitializeComponent();
            TicketsHistoryBUS ticketsHistoryBUS = new TicketsHistoryBUS();
            listhistory = ticketsHistoryBUS.GetAllTicketHistories();

            LoadFormTable(listhistory);
        }

        private void TicketNumberHistoryTicket_Load(object sender, EventArgs e)
        {

        }

        private void btnSearchHistoryTicket(object sender, EventArgs e)
        {
            var data = listhistory.Where(x => x.TicketNumber == txtNumberHistoryTicket.Text).ToList();
            LoadFormTable(data);

        }

        private void tableCustom1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void LoadFormTable(List<TicketHistoryDTO>data)
        {
            dgvTicketNumberHistory.AutoGenerateColumns = false;

            dgvTicketNumberHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TicketNumber",
                HeaderText = "Ticket Number"
            });
            dgvTicketNumberHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Issue Date",
                DataPropertyName = "issue_date"

            });
            dgvTicketNumberHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Status",
                DataPropertyName = "status"
            });
            dgvTicketNumberHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Old Status",
                DataPropertyName = "old_status"
            });
            dgvTicketNumberHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "New Status",
                DataPropertyName = "new_status"
            });
            dgvTicketNumberHistory.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Changed At",
                DataPropertyName = "changed_at"
            });
            dgvTicketNumberHistory.DataSource = data;
        }
    }
}
