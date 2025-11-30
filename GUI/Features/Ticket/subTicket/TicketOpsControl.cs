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
        private TicketBUS _ticketBUS;
        private TicketFilterBUS _ticketFilterBUS;

        // Hằng số hiển thị
        private const string ACTION_COL_NAME = "colAction";
        private const string TXT_VIEW = "Xem";
        private const string TXT_ISSUE = "Xuất vé";
        private const string TXT_CANCEL = "Hủy";
        private const string SEP = " / ";
        public TicketOpsControl()
        {
            InitializeComponent();
            _ticketBUS = new TicketBUS();
            // Vừa khởi tạo chỉ load khoảng 10 vé mới nhất
            _ticketFilterBUS = new TicketFilterBUS();
            List<TicketFilterDTO>  tickets = _ticketFilterBUS.ListTenTicketsNews();
            SetupAndLoadGrid(tickets);
            // Xóa form lọc
            loadForm();
        }
        private void SetupAndLoadGrid(List<TicketFilterDTO> tickets)
        {
            // ⚙️ Cấu hình chung
            dgvListFilerTickets.AutoGenerateColumns = false;
            dgvListFilerTickets.AllowUserToAddRows = false;
            dgvListFilerTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvListFilerTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvListFilerTickets.Columns.Clear();

            // 🧾 Cột dữ liệu chính (hiển thị trên bảng)
            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTicketNumber",
                HeaderText = "Số vé",
                DataPropertyName = "TicketNumber",  // ✅ map với DTO
                FillWeight = 12
            });

            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPassengerName",
                HeaderText = "Hành khách",
                DataPropertyName = "PassengerName",  // ✅ map với DTO
                FillWeight = 18
            });

            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSeatNumber",
                HeaderText = "Ghế",
                DataPropertyName = "SeatNumber",  // ✅ map với DTO
                FillWeight = 8
            });

            

            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDepartureTime",
                HeaderText = "Giờ đi",
                DataPropertyName = "DepartureTime",  // ✅ map với DTO
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "HH:mm" }
            });

            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colArrivalTime",
                HeaderText = "Giờ đến",
                DataPropertyName = "ArrivalTime",  // ✅ map với DTO
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "HH:mm" }
            });

            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Trạng thái",
                DataPropertyName = "Status",  // ✅ map với DTO
                FillWeight = 10,
                Visible = false
            });

            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colBasePrice",
                HeaderText = "Giá ghế",
                DataPropertyName = "BasePrice",  // ✅ map với DTO
                FillWeight = 12,
                Visible = false
            });

            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAirlineName",
                HeaderText = "Hãng bay",
                DataPropertyName = "AirlineName",  // ✅ map với DTO
                FillWeight = 15,
                Visible = false
            });

            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAirportName",
                HeaderText = "Sân bay đi",
                DataPropertyName = "AirportName",  // ✅ map với DTO
                FillWeight = 15,
                Visible = false
            });

            // ✈️ Cột ẩn (nếu cần giữ thông tin mà không hiển thị)
            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPassengerPhone",
                DataPropertyName = "PassengerPhone",
                Visible = false
            });

            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPassportNumber",
                DataPropertyName = "PassportNumber",
                Visible = false
            });




            // Cột Hành động (custom draw)
            dgvListFilerTickets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = ACTION_COL_NAME,
                HeaderText = "Hành động",
                ReadOnly = true,
                FillWeight = 25,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Co cột theo nội dung + đặt min width để không tràn
            var actionCol = dgvListFilerTickets.Columns[ACTION_COL_NAME];
            actionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            actionCol.MinimumWidth = 160;

            // Sự kiện
            dgvListFilerTickets.CellPainting += DgvBookingsTicket_CellPainting;
            dgvListFilerTickets.CellMouseClick += DgvBookingsTicket_CellMouseClick;
            dgvListFilerTickets.CellMouseMove += DgvBookingsTicket_CellMouseMove;

            // Bật double-buffer giảm flicker
            EnableDoubleBuffering(dgvListFilerTickets);

            //MessageBox.Show("Đã tải " + tickets.Count + " vé đã lọc.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvListFilerTickets.DataSource = tickets;
        }
        private void DgvBookingsTicket_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvListFilerTickets.Columns[ACTION_COL_NAME].Index) return;

            e.Paint(e.CellBounds, DataGridViewPaintParts.All);

            var ticket = dgvListFilerTickets.Rows[e.RowIndex].DataBoundItem as TicketDTO;
            if (ticket == null) return;

            var font = e.CellStyle.Font ?? dgvListFilerTickets.DefaultCellStyle.Font;

            // Tính rect TƯƠNG ĐỐI trong ô
            var (rcViewRel, rcIssueRel, rcCancelRel) = GetActionRects(e.CellBounds.Size, font);

            // Offset sang toạ độ tuyệt đối để vẽ
            var rcView = rcViewRel; rcView.Offset(e.CellBounds.Location);
            var rcIssue = rcIssueRel; rcIssue.Offset(e.CellBounds.Location);
            var rcCancel = rcCancelRel; rcCancel.Offset(e.CellBounds.Location);

            // Vẽ text
            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, rcView, Color.DodgerBlue, TextFormatFlags.VerticalCenter);

            // Dấu phân cách sau "Xem"
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rcView.Right, rcView.Top), Color.Gray, TextFormatFlags.VerticalCenter);

            bool canIssue = (ticket.Status == TicketStatus.BOOKED);
            TextRenderer.DrawText(e.Graphics, TXT_ISSUE, font, rcIssue, canIssue ? Color.Green : Color.Gray, TextFormatFlags.VerticalCenter);

            // Dấu phân cách sau "Xuất vé"
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rcIssue.Right, rcIssue.Top), Color.Gray, TextFormatFlags.VerticalCenter);

            bool canCancel = (ticket.Status != TicketStatus.BOARDED && ticket.Status != TicketStatus.CANCELED && ticket.Status != TicketStatus.REFUNDED);
            TextRenderer.DrawText(e.Graphics, TXT_CANCEL, font, rcCancel, canCancel ? Color.Red : Color.Gray, TextFormatFlags.VerticalCenter);

            e.Handled = true;
        }

        private void DgvBookingsTicket_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvListFilerTickets.Columns[ACTION_COL_NAME].Index) return;

            var ticket = dgvListFilerTickets.Rows[e.RowIndex].DataBoundItem as TicketDTO;
            if (ticket == null) return;

            var cell = dgvListFilerTickets.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var font = cell.InheritedStyle.Font ?? dgvListFilerTickets.DefaultCellStyle.Font;

            // Rect tương đối trong ô; e.Location cũng tương đối
            var (rcView, rcIssue, rcCancel) = GetActionRects(cell.Size, font);
            Point p = e.Location;

            if (rcView.Contains(p))
            {
                ShowRowInformation(ticket, "Xem Chi Tiết Vé");
            }
            else if (rcIssue.Contains(p) && ticket.Status == TicketStatus.BOOKED)
            {
                ShowRowInformation(ticket, "Xác Nhận Xuất Vé", true);
            }
            else if (rcCancel.Contains(p))
            {
                bool canCancel = (ticket.Status != TicketStatus.BOARDED && ticket.Status != TicketStatus.CANCELED && ticket.Status != TicketStatus.REFUNDED);
                if (canCancel)
                    ShowRowInformation(ticket, "Xác Nhận Hủy Vé", true);
            }
        }

        private void DgvBookingsTicket_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvListFilerTickets.Columns[ACTION_COL_NAME].Index)
            {
                dgvListFilerTickets.Cursor = Cursors.Default;
                return;
            }

            var ticket = dgvListFilerTickets.Rows[e.RowIndex].DataBoundItem as TicketDTO;
            if (ticket == null) { dgvListFilerTickets.Cursor = Cursors.Default; return; }

            var cell = dgvListFilerTickets.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var font = cell.InheritedStyle.Font ?? dgvListFilerTickets.DefaultCellStyle.Font;

            var (rcView, rcIssue, rcCancel) = GetActionRects(cell.Size, font);
            Point p = e.Location;

            bool onView = rcView.Contains(p);
            bool onIssue = rcIssue.Contains(p) && ticket.Status == TicketStatus.BOOKED;
            bool onCancel = rcCancel.Contains(p) &&
                            (ticket.Status != TicketStatus.BOARDED &&
                             ticket.Status != TicketStatus.CANCELED &&
                             ticket.Status != TicketStatus.REFUNDED);

            dgvListFilerTickets.Cursor = (onView || onIssue || onCancel) ? Cursors.Hand : Cursors.Default;
        }



        /// <summary>
        /// Hiển thị thông tin vé, kèm xác nhận nếu cần.
        /// </summary>
        private void ShowRowInformation(TicketDTO ticket, string title, bool isConfirmation = false)
        {
            var infoBuilder = new StringBuilder();
            infoBuilder.AppendLine("THÔNG TIN VÉ:");
            infoBuilder.AppendLine("------------------------------");
            infoBuilder.AppendLine($"Mã Vé:\t\t{ticket.TicketId}");
            infoBuilder.AppendLine($"Số Hiệu Vé:\t{ticket.TicketNumber}");
            infoBuilder.AppendLine($"Ngày Xuất:\t{ticket.IssueDate:dd/MM/yyyy HH:mm}");
            infoBuilder.AppendLine($"Trạng Thái:\t{ticket.Status}");
            infoBuilder.AppendLine($"Mã Hành Khách:\t{ticket.PassengerId}");
            infoBuilder.AppendLine($"Mã Ghế:\t\t{ticket.FlightSeatId}");
            infoBuilder.AppendLine("------------------------------");

            if (isConfirmation)
            {
                infoBuilder.AppendLine("\nBạn có chắc chắn muốn thực hiện hành động này không?");
                var result = MessageBox.Show(infoBuilder.ToString(), title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // TODO: Gọi nghiệp vụ thực tế ở đây, ví dụ:
                    // _ticketBUS.IssueTicket(ticket.TicketId);
                    // _ticketBUS.CancelTicket(ticket.TicketId);
                    MessageBox.Show("Hành động đã được thực hiện!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(infoBuilder.ToString(), title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Tính vị trí 3 nút (Xem / Xuất vé / Hủy) TƯƠNG ĐỐI trong ô để vẽ & hit-test.
        /// </summary>
        private (Rectangle view, Rectangle issue, Rectangle cancel) GetActionRects(Size cellSize, Font font)
        {
            Size szView = TextRenderer.MeasureText(TXT_VIEW, font);
            Size szIssue = TextRenderer.MeasureText(TXT_ISSUE, font);
            Size szCancel = TextRenderer.MeasureText(TXT_CANCEL, font);
            Size szSep = TextRenderer.MeasureText(SEP, font);

            int totalWidth = szView.Width + szSep.Width + szIssue.Width + szSep.Width + szCancel.Width;

            int x = Math.Max(0, (cellSize.Width - totalWidth) / 2);
            int y = Math.Max(0, (cellSize.Height - szView.Height) / 2);

            Rectangle rcView = new Rectangle(x, y, szView.Width, szView.Height);
            x += szView.Width + szSep.Width;

            Rectangle rcIssue = new Rectangle(x, y, szIssue.Width, szIssue.Height);
            x += szIssue.Width + szSep.Width;

            Rectangle rcCancel = new Rectangle(x, y, szCancel.Width, szCancel.Height);

            return (rcView, rcIssue, rcCancel);
        }

        /// <summary>
        /// Bật double-buffer cho DataGridView để giảm flicker khi vẽ custom.
        /// </summary>
        private void EnableDoubleBuffering(DataGridView dgv)
        {
            try
            {
                typeof(DataGridView).InvokeMember(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.SetProperty,
                    null, dgv, new object[] { true });
            }
            catch
            {
                // Bỏ qua nếu không set được
            }
        }

        private void btnSearchOpsTicket_Click(object sender, EventArgs e)
        {
            string BookingCode  = txtFilterBookingCode.Text.Trim();
            string FlightCode   = txtFilterFlightCode.Text.Trim();
            string TicketStatus = cbFilterTicketStatus.Text.Trim();
            //MessageBox.Show(TicketStatus);
            DateTime FlightDate = dtpFilterFlightDate.Value;
            //MessageBox.Show(FlightDate.ToString());
            string PhoneNumber  = txtFilterPhoneNumber.Text.Trim();

            List<TicketFilterDTO> tickets = _ticketFilterBUS.ListFilterTickets(BookingCode, FlightCode,  FlightDate, TicketStatus, PhoneNumber);
            SetupAndLoadGrid(tickets);
            // Xóa form lọc
            loadForm();
        }
        private void loadForm()
        {
            txtFilterBookingCode.Text = " ";
            txtFilterFlightCode.Text = " ";
            cbFilterTicketStatus.SelectedIndex = -1;
            //dtpFilterFlightDate.Value = DateTime.Now;
            txtFilterPhoneNumber.Text = " ";
        }
        private void underlinedTextField1_Load(object sender, EventArgs e)
        {

        }
    }
}

