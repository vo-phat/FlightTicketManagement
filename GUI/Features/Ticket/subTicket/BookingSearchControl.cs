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
            dgvBookingsTicket.AutoGenerateColumns = false;
            // Giả sử anh có đối tượng _ticketBUS đã được khởi tạo
            List<TicketDTO> tickets = _ticketBUS.GetAllTickets(); // Lấy dữ liệu từ BUS
            MessageBox.Show("Số vé lấy được: " + tickets.Count); // Hiển thị số lượng vé lấy được

            // Xây dựng các cột cho DataGridView nếu chưa có
            // === BƯỚC 1: CẤU HÌNH CƠ BẢN CHO DATAGRIDVIEW ===

            // Tắt chế độ tự động tạo cột để chúng ta có thể kiểm soát hoàn toàn
            dgvBookingsTicket.AutoGenerateColumns = false;

            // Các thiết lập giao diện khác cho đẹp hơn (tùy chọn)
            dgvBookingsTicket.AllowUserToAddRows = false;
            dgvBookingsTicket.ReadOnly = true;
            dgvBookingsTicket.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBookingsTicket.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động co giãn cột

            // === BƯỚC 2: ĐỊNH NGHĨA VÀ THÊM TỪNG CỘT DỮ LIỆU ===
            // Mấu chốt là gán "DataPropertyName" khớp với tên thuộc tính trong TicketDTO

            // --- Cột Mã Vé (TicketId) ---
            dgvBookingsTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTicketId",
                HeaderText = "Mã vé",
                DataPropertyName = "TicketId", // Nối với TicketDTO.TicketId
                FillWeight = 10 // Tỷ lệ độ rộng
            });

            // --- Cột Số hiệu vé (TicketNumber) ---
            dgvBookingsTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTicketNumber",
                HeaderText = "Số hiệu vé",
                DataPropertyName = "TicketNumber", // Nối với TicketDTO.TicketNumber
                FillWeight = 20
            });

            // --- Cột Ngày xuất vé (IssueDate) ---
            dgvBookingsTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIssueDate",
                HeaderText = "Ngày xuất vé",
                DataPropertyName = "IssueDate", // Nối với TicketDTO.IssueDate
                FillWeight = 25,
                // Định dạng ngày tháng cho dễ đọc
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });

            // --- Cột Trạng thái (Status) ---
            dgvBookingsTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Trạng thái",
                DataPropertyName = "Status", // Nối với TicketDTO.Status
                FillWeight = 15
            });

            // --- Cột Mã Hành khách (PassengerId) và Mã Ghế (FlightSeatId) ---
            // Thường các cột ID này không cần hiển thị cho người dùng.
            // Chúng ta vẫn tạo ra nhưng cho nó ẩn đi để có thể lấy giá trị khi cần.
            dgvBookingsTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPassengerId",
                HeaderText = "Passenger ID",
                DataPropertyName = "PassengerId", // Nối với TicketDTO.PassengerId
                Visible = false // Cho ẩn cột này đi
            });

            dgvBookingsTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFlightSeatId",
                HeaderText = "FlightSeat ID",
                DataPropertyName = "FlightSeatId", // Nối với TicketDTO.FlightSeatId
                Visible = false // Cho ẩn cột này đi
            });


            // === BƯỚC 3: ĐỊNH NGHĨA VÀ THÊM CÁC CỘT THAO TÁC (KHÔNG CÓ TRONG DTO) ===
            // Các cột này không có DataPropertyName

            // --- Cột link "Xem" ---
            var colView = new DataGridViewLinkColumn
            {
                Name = "colView",
                HeaderText = "",
                Text = "Xem",
                UseColumnTextForLinkValue = true,
                TrackVisitedState = false,
                FillWeight = 8,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            };
            dgvBookingsTicket.Columns.Add(colView);

            // --- Cột link "Xuất vé" ---
            var colIssue = new DataGridViewLinkColumn
            {
                Name = "colIssue",
                HeaderText = "",
                Text = "Xuất vé",
                UseColumnTextForLinkValue = true,
                TrackVisitedState = false,
                FillWeight = 12,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };
            dgvBookingsTicket.Columns.Add(colIssue);

            // --- Cột link "Hủy" ---
            var colCancel = new DataGridViewLinkColumn
            {
                Name = "colCancel",
                HeaderText = "",
                Text = "Hủy",
                UseColumnTextForLinkValue = true, // sử dụng cùng một text cho tất cả các ô trong cột
                TrackVisitedState = false,
                LinkColor = Color.IndianRed,
                ActiveLinkColor = Color.Red,
                FillWeight = 8,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };
            dgvBookingsTicket.Columns.Add(colCancel);

            // === BƯỚC 4: ĐĂNG KÝ CÁC SỰ KIỆN CẦN THIẾT ===
            dgvBookingsTicket.CellContentClick += DgvBookingsTicket_CellContentClick;
            dgvBookingsTicket.CellFormatting += DgvBookingsTicket_CellFormatting;


            // Gán dữ liệu vào DataSource
            dgvBookingsTicket.DataSource = tickets;
        }
        private void DgvBookingsTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua nếu bấm vào header
            if (e.RowIndex < 0) return;
            // Lấy ra đối tượng TicketDTO của dòng được bấm
            var selectedTicket = dgvBookingsTicket.Rows[e.RowIndex].DataBoundItem as TicketDTO;
            if (selectedTicket == null) return;

            // Lấy ra tên của cột được bấm để biết hành động là gì
            string columnName = dgvBookingsTicket.Columns[e.ColumnIndex].Name;

            // Kiểm tra xem ô link có bị vô hiệu hóa không trước khi thực hiện
            if (dgvBookingsTicket[e.ColumnIndex, e.RowIndex].ReadOnly)
            {
                return;
            }

            // Thực hiện hành động tương ứng với cột được bấm
            switch (columnName)
            {
                case "colView":
                    MessageBox.Show($"Xem chi tiết vé: {selectedTicket.TicketNumber}");
                    // TODO: Mở form chi tiết vé
                    
                    break;

                case "colIssue":
                    MessageBox.Show($"Thực hiện xuất vé cho: {selectedTicket.TicketNumber}");
                    // TODO: Gọi BUS để thực hiện nghiệp vụ xuất vé
                    // Ví dụ cập nhật trạng thái và vẽ lại giao diện:
                    // selectedTicket.Status = TicketStatus.CONFIRMED;
                    // dgvBookingsTicket.InvalidateRow(e.RowIndex);
                    break;

                case "colCancel":
                    var confirm = MessageBox.Show($"Bạn có chắc muốn hủy vé {selectedTicket.TicketNumber}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        MessageBox.Show($"Đã hủy vé: {selectedTicket.TicketNumber}");
                        // TODO: Gọi BUS để thực hiện nghiệp vụ hủy vé
                        // Ví dụ cập nhật trạng thái và vẽ lại giao diện:
                        // selectedTicket.Status = TicketStatus.CANCELED;
                        // dgvBookingsTicket.InvalidateRow(e.RowIndex);
                    }
                    break;
                default:
                    //MessageBox.Show($"Xem chi tiết vé:");

                    break;
            }
        }
        private void DgvBookingsTicket_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Bỏ qua header
            if (e.RowIndex < 0) return;

            // Lấy ra đối tượng TicketDTO của dòng hiện tại
            var ticket = dgvBookingsTicket.Rows[e.RowIndex].DataBoundItem as TicketDTO;
            if (ticket == null) return;

            // Lấy ra ô (cell) hiện tại đang được định dạng
            var cell = dgvBookingsTicket[e.ColumnIndex, e.RowIndex] as DataGridViewLinkCell;
            if (cell == null) return; // Chỉ xử lý các ô dạng link

            // Lấy tên cột để biết đang định dạng cho link nào
            string columnName = dgvBookingsTicket.Columns[e.ColumnIndex].Name;

            if (columnName == "colIssue")
            {
                // Điều kiện: Chỉ cho phép "Xuất vé" khi trạng thái là BOOKED
                bool canIssue = (ticket.Status == TicketStatus.BOOKED);
                SetLinkState(cell, canIssue);
            }
            else if (columnName == "colCancel")
            {
                // Điều kiện: Chỉ cho phép "Hủy" khi vé chưa lên máy bay, chưa bị hủy hoặc hoàn tiền
                bool canCancel = (ticket.Status != TicketStatus.BOARDED &&
                                  ticket.Status != TicketStatus.CANCELED &&
                                  ticket.Status != TicketStatus.REFUNDED);
                // Dùng màu đỏ cho link Hủy
                SetLinkState(cell, canCancel, Color.IndianRed, Color.Red);
            }
        }

        // Hàm tiện ích để thiết lập trạng thái cho một ô link
        private void SetLinkState(DataGridViewLinkCell cell, bool enabled, Color? enabledColor = null, Color? activeColor = null)
        {
            cell.LinkColor = enabled ? (enabledColor ?? Color.DodgerBlue) : Color.Gray;
            cell.ActiveLinkColor = enabled ? (activeColor ?? Color.Blue) : Color.Gray;
            cell.LinkBehavior = enabled ? LinkBehavior.SystemDefault : LinkBehavior.NeverUnderline;
            cell.ReadOnly = !enabled;
        }


    }
}
