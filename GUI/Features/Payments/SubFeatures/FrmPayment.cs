using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO.Ticket; // Nếu Anh dùng DTO để nhận list thì thêm namespace này

namespace GUI.Features.Payments.SubFeatures
{
    public partial class FrmPayment : UserControl
    {
        private decimal _amount;
        private List<TicketBookingRequestDTO> _tickets;

        public FrmPayment(decimal amount, List<TicketBookingRequestDTO> tickets)
        {
            InitializeComponent();

            _amount = amount;
            _tickets = tickets;
        }

        private void FrmPayment_Load(object sender, EventArgs e)
        {
            // Load ảnh QR giả lập (Anh tự thay link hoặc resource)
            try
            {
                picQR.Image = Image.FromFile("Assets/fake_qr.png");
            }
            catch
            {
                picQR.BackColor = Color.LightGray;
            }
        }

        private async void btnPay_Click(object sender, EventArgs e)
        {
            btnPay.Enabled = false;
            btnPay.Text = "Đang xử lý...";

            await Task.Delay(1500); // Fake payment

            MessageBox.Show("Thanh toán thành công!", "SUCCESS",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Nếu cần callback → raise event tại đây
            // PaymentCompleted?.Invoke(this, EventArgs.Empty);

            btnPay.Text = "Thanh Toán";
            btnPay.Enabled = true;
        }
    }
}
