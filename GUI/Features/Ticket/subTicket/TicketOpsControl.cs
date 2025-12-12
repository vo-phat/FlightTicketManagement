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
        private int currentAdminId = 1; // Giả sử admin hiện tại có ID là 1

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
            dgvTicketOpsControl.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "btnBaggage",
                HeaderText = "",
                Text = "Xem hành lý",
                UseColumnTextForButtonValue = true,
                FillWeight = 10
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
            // 1️⃣ BẢO VỆ CƠ BẢN
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex < 0) return;

            // 2️⃣ LẤY DTO TỪ DÒNG
            var dto = dgvTicketOpsControl
                .Rows[e.RowIndex]
                .DataBoundItem as TicketListDTO;

            if (dto == null) return;

            // 3️⃣ XÁC ĐỊNH CỘT ĐƯỢC BẤM
            string colName = dgvTicketOpsControl.Columns[e.ColumnIndex].Name;

            // =========================
            // 4️⃣ GỌI BUS ĐÚNG CHỖ
            // =========================

            if (colName == "btnCancel")
            {
                try
                {
                    // ✅ Check nghiệp vụ nhanh
                    if (dto.Status != "BOOKED")
                    {
                        MessageBox.Show("Chỉ được hủy vé đang BOOKED");
                        return;
                    }

                    // ✅ MỞ FORM CHỌN LÝ DO
                    using (var frm = new frmCancelReason())
                    {
                        if (frm.ShowDialog(this) != DialogResult.OK)
                            return;

                        string reason = frm.SelectedReason;

                        // ✅ GỌI BUS VỚI LÝ DO THẬT
                        new CancelTicketBUS()
                            .CancelTicket(dto, currentAdminId, reason);
                    }

                    MessageBox.Show("Hủy vé thành công");
                    ReloadGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi");
                }
            }

            else if (colName == "btnRefund")
            {
                try
                {
                    // 1️⃣ DTO từ grid (list)
                    var listDto = dto; // dto đã lấy ở trên rồi

                    // 2️⃣ MAP sang DTO refund
                    var refundDto = new RefundTicketDTO
                    {
                        TicketId = listDto.TicketId,
                        Status = listDto.Status,
                        TicketPrice = listDto.Price,          // hoặc TicketPrice
                        IsRefundable = listDto.IsRefundable,
                        RefundFeePercent = listDto.RefundFeePercent,
                        AdminId = currentAdminId
                    };

                    // 3️⃣ Gọi BUS refund
                    new RefundTicketBUS().Refund(refundDto);

                    MessageBox.Show("Đã ghi nhận hoàn tiền");
                    ReloadGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi");
                }
            }

            else if (colName == "btnView")
            {
                MessageBox.Show($"Xem vé: {dto.TicketNumber}");
            }
            else if (colName == "btnBaggage")
            {
                OnOpenBaggageManager?.Invoke(dto.TicketId);
            }
        }
        private void ReloadGrid()
        {
            ticketListBUS = new TicketListBUS();
            List<TicketListDTO> data = ticketListBUS.GetAllTickets();

            dgvTicketOpsControl.DataSource = null;
            dgvTicketOpsControl.DataSource = data;
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


        public event Action<int> OnOpenBaggageManager;

    }
}
