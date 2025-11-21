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
        public HistoryTicketControl()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void primaryButton1_Click(object sender, EventArgs e)
        {
            TicketsHistoryBUS ticketsHistoryBUS = new TicketsHistoryBUS();
            List<TicketHistory> listhistory = ticketsHistoryBUS.GetAllTicketHistories();

            MessageBox.Show($"List có {listhistory.Count} phần tử");
        }
    }
}
