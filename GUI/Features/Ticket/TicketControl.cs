using BUS.Ticket;
using DTO.Ticket;
using GUI.Features.Ticket.subTicket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GUI.Features.Ticket
{
    public partial class TicketControl : UserControl
    {
        private BookingSearchControl bookingSearchControl;
        private HistoryTicketControl historyTicketControl;
        private TicketOpsControl ticketOpsControl;
        private frmPassengerInfoControl frmPassengerInfoControl;
        public TicketControl()
        {
            InitializeComponent();
            /// content
            bookingSearchControl = new BookingSearchControl();
            historyTicketControl = new HistoryTicketControl();
            ticketOpsControl = new TicketOpsControl();
            frmPassengerInfoControl = new frmPassengerInfoControl();

            pnlBookingSearch.Dock = DockStyle.Fill;
            pnlHistoryTicket.Dock = DockStyle.Fill;
            pnlTicketOps.Dock = DockStyle.Fill;
            pnlFrmPassengerInfo.Dock = DockStyle.Fill;

            bookingSearchControl.Dock = DockStyle.Fill;
            historyTicketControl.Dock = DockStyle.Fill;
            ticketOpsControl.Dock = DockStyle.Fill;
            frmPassengerInfoControl.Dock = DockStyle.Fill;

            //pnl content con
            pnlBookingSearch.Controls.Add(bookingSearchControl);
            pnlHistoryTicket.Controls.Add(historyTicketControl);
            pnlTicketOps.Controls.Add(ticketOpsControl);
            pnlFrmPassengerInfo.Controls.Add(frmPassengerInfoControl);


            // pnl content cha
            pnlContentTicket.Controls.Add(pnlBookingSearch);
            pnlContentTicket.Controls.Add(pnlHistoryTicket);
            pnlContentTicket.Controls.Add(pnlTicketOps);
            pnlContentTicket.Controls.Add(pnlFrmPassengerInfo);
            pnlBookingSearch.Dock = DockStyle.Fill;
            pnlContentTicket.BringToFront();

            ///// add butoon
            //pnlHeaderTicket.Controls.Add(btnOpsTicket);
            //pnlHeaderTicket.Controls.Add(btnCreateFindTicket);
            //pnlHeaderTicket.Controls.Add(btnHistoryTicket);
            // chỉnh header
            pnlHeaderTicket.Dock = DockStyle.Top;
            pnlHeaderTicket.Height = 60;
            pnlHeaderTicket.BringToFront();

            //TicketControl.Dock = DockStyle.Fill;
        }

        private void btnHistoryTicketAdmin_Click(object sender, EventArgs e)
        {
            switchTab(1);
        }
        private void btnCreateAndFindSeat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng đang phát triển!");
            //switchTab(0);
        }

        private void btnOpsTicket_Click(object sender, EventArgs e)
        {
            switchTab(2);
        }
        private void btnFrmPassengerInfoTiket_Click(object sender, EventArgs e)
        {
            switchTab(3);
        }

        public void switchTab(int i)
        {
            pnlBookingSearch.Visible = false;
            pnlHistoryTicket.Visible = false;
            pnlTicketOps.Visible = false;
            pnlFrmPassengerInfo.Visible = false;
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
                case 3:
                    pnlFrmPassengerInfo.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void TicketControl_Load(object sender, EventArgs e)
        {

        }

        private void pnlHeaderTicket_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
