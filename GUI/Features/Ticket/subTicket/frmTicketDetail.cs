using DTO.Ticket;
using DTO.Ticket.DTO.Ticket;
using GUI.Export;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Ticket.subTicket
{
    public class frmTicketDetail : Form
    {
        // ===== LABEL VALUE =====
        private Label lblTicketNumber, lblStatus, lblPrice, lblCabin;
        private Label lblPassengerName, lblPassport, lblNationality, lblDob;
        private Label lblFlightNumber, lblRoute, lblDeparture, lblArrival, lblSeat;
        private TicketDetailDTO _dto;

        public frmTicketDetail()
        {
            InitializeUI();
        }

        // ===============================
        // UI INIT
        // ===============================
        private void InitializeUI()
        {
            this.Text = "Chi tiết vé";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ClientSize = new Size(700, 520);
            this.MaximizeBox = false;

            int y = 20;

            // ===== SECTION: TICKET =====
            AddSectionTitle("Thông tin vé", ref y);
            lblTicketNumber = AddRow("Số vé:", ref y);
            lblStatus = AddRow("Trạng thái:", ref y);
            lblPrice = AddRow("Giá vé:", ref y);
            lblCabin = AddRow("Hạng ghế:", ref y);

            y += 10;

            // ===== SECTION: PASSENGER =====
            AddSectionTitle("Hành khách", ref y);
            lblPassengerName = AddRow("Tên:", ref y);
            lblPassport = AddRow("Hộ chiếu:", ref y);
            lblNationality = AddRow("Quốc tịch:", ref y);
            lblDob = AddRow("Ngày sinh:", ref y);

            y += 10;

            // ===== SECTION: FLIGHT =====
            AddSectionTitle("Chuyến bay", ref y);
            lblFlightNumber = AddRow("Chuyến:", ref y);
            lblRoute = AddRow("Hành trình:", ref y);
            lblDeparture = AddRow("Giờ đi:", ref y);
            lblArrival = AddRow("Giờ đến:", ref y);
            lblSeat = AddRow("Ghế:", ref y);

            // ===== CLOSE BUTTON =====
            var btnClose = new Button
            {
                Text = "Đóng",
                Width = 100,
                Height = 32,
                Location = new Point(this.ClientSize.Width - 120, this.ClientSize.Height - 50)
            };
            btnClose.Click += (s, e) => this.Close();


            var btnExport = new Button
            {
                Text = "Xuất vé (PDF)",
                Width = 140,
                Height = 32,
                Location = new Point(20, this.ClientSize.Height - 50)
            };
            btnExport.Click += BtnExport_Click;

            this.Controls.Add(btnExport);


            this.Controls.Add(btnClose);
        }

        // ===============================
        // HELPER UI METHODS
        // ===============================
        private void AddSectionTitle(string text, ref int y)
        {
            var lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, y),
                AutoSize = true
            };
            this.Controls.Add(lbl);
            y += 28;
        }

        private Label AddRow(string title, ref int y)
        {
            var lblTitle = new Label
            {
                Text = title,
                Location = new Point(40, y),
                Width = 120
            };

            var lblValue = new Label
            {
                Location = new Point(170, y),
                Width = 480,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblValue);

            y += 24;
            return lblValue;
        }

        // ===============================
        // LOAD DATA
        // ===============================
        public void LoadData(TicketDetailDTO dto)
        {
            _dto = dto;
            lblTicketNumber.Text = dto.TicketNumber;
            lblStatus.Text = dto.Status;
            lblPrice.Text = dto.TotalPrice.ToString("N0");
            lblCabin.Text = dto.CabinClass;

            lblPassengerName.Text = dto.PassengerName;
            lblPassport.Text = dto.PassportNumber;
            lblNationality.Text = dto.Nationality;
            lblDob.Text = dto.DateOfBirth?.ToString("dd/MM/yyyy") ?? "—";

            lblFlightNumber.Text = dto.FlightNumber;
            lblRoute.Text = dto.Route;
            lblDeparture.Text = dto.DepartureTime.ToString("dd/MM/yyyy HH:mm");
            lblArrival.Text = dto.ArrivalTime.ToString("dd/MM/yyyy HH:mm");
            lblSeat.Text = dto.SeatNumber;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (_dto == null)
            {
                MessageBox.Show("Chưa có dữ liệu vé để xuất");
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PDF file (*.pdf)|*.pdf",
                FileName = $"Ticket_{_dto.TicketNumber}.pdf"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            TicketPdfExporter.Export(_dto, sfd.FileName);
            MessageBox.Show("Xuất vé thành công");
        }

    }
}
