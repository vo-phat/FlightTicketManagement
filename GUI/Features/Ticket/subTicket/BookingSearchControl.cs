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
    public partial class BookingSearchControl : UserControl
    {
        private TicketBUS _ticketBUS;

        public BookingSearchControl()
        {
            InitializeComponent();
            
            _ticketBUS = new TicketBUS();
            LoadDataToGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dgvBookingsTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadDataToGrid()
        {
            // Giả sử anh có đối tượng _ticketBUS đã được khởi tạo
            List<TicketDTO> tickets = _ticketBUS.GetAllTickets(); // Lấy dữ liệu từ BUS
            MessageBox.Show("Số vé lấy được: " + tickets.Count); // Hiển thị số lượng vé lấy được
            // Gán dữ liệu vào DataSource
            dgvBookingsTicket.DataSource = tickets;
        }
    }
}
