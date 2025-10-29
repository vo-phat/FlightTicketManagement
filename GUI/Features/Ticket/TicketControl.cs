using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI.Features.Ticket.subTicket;
namespace GUI.Features.Ticket
{
    public partial class TicketControl : UserControl
    {
        private BookingSearchControl bookingSearchControl;
        private HistoryTicketControl historyTicketControl;
        private TicketOpsControl ticketOpsControl;
        public TicketControl()
        {
            InitializeComponent();
            /// content
            bookingSearchControl = new BookingSearchControl();
            historyTicketControl = new HistoryTicketControl();
            ticketOpsControl = new TicketOpsControl();

            pnlBookingSearch.Dock = DockStyle.Fill;
            pnlHistoryTicket.Dock = DockStyle.Fill;
            pnlTicketOps.Dock = DockStyle.Fill;

            bookingSearchControl.Dock = DockStyle.Fill;
            historyTicketControl.Dock = DockStyle.Fill;
            ticketOpsControl.Dock = DockStyle.Fill;

            //pnl content con
            pnlBookingSearch.Controls.Add(bookingSearchControl);
            pnlHistoryTicket.Controls.Add(historyTicketControl);
            pnlTicketOps.Controls.Add(ticketOpsControl);


            // pnl content cha
            pnlContentTicket.Controls.Add(pnlBookingSearch);
            pnlContentTicket.Controls.Add(pnlHistoryTicket);
            pnlContentTicket.Controls.Add(pnlTicketOps);
            pnlContentTicket.BringToFront();

            ///// add butoon
            //pnlHeaderTicket.Controls.Add(btnOpsTicket);
            //pnlHeaderTicket.Controls.Add(btnCreateFindTicket);
            //pnlHeaderTicket.Controls.Add(btnHistoryTicket);
            // chỉnh header
            pnlHeaderTicket.Dock = DockStyle.Top;
            pnlHeaderTicket.Height = 60;
            pnlHeaderTicket.BringToFront();
        }

        private void btnBookingAndSearchTicket_Click(object sender, EventArgs e)
        {
            switchTab(0);
        }

        private void btnOpsTicket_Click(object sender, EventArgs e)
        {
            switchTab(1);
        }

        private void btnHistoryTicket_Click(object sender, EventArgs e)
        {
            switchTab(2);
        }
        public void switchTab(int i)
        {
            pnlBookingSearch.Visible = false;
            pnlHistoryTicket.Visible = false;
            pnlTicketOps.Visible = false;
            switch (i)
            {
                case 0:
                    pnlBookingSearch.Visible = true;
                    break;
                case 1:
                    pnlHistoryTicket.Visible = true;
                    break;
                case 2:
                    pnlTicketOps.Visible = true;
                    break;
                default:
                    break;
            }
        }
    }
}
