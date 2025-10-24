using System;
using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Tables;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Inputs;

namespace FlightTicketManagement.GUI.Features.Flight {
    public class FlightListControl : UserControl {
        private TableCustom table;

        private const string ACTION_COL_NAME = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DELETE = "Xóa";
        private const string SEP = " / ";

        private TableLayoutPanel mainPanel;
        private TableLayoutPanel filterWrapPanel;
        private FlowLayoutPanel filterPanel;
        private FlowLayoutPanel btnPanel;
        private Label lblTitle;
        private DateTimePickerCustom dtpDepartureDate;
        private DateTimePickerCustom dtpArrivalDate;
        private UnderlinedTextField txtDeparturePlace;
        private UnderlinedTextField txtArrivalPlace;

        public FlightListControl() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            SuspendLayout();

            // ===== Root =====
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // ===== Title =====
            lblTitle = new Label {
                Text = "✈️ Danh sách chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Filter row =====
            filterPanel = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            dtpDepartureDate = new DateTimePickerCustom("Ngày đi", "") {
                Width = 300,
                Margin = new Padding(0, 0, 32, 0)
            };

            dtpArrivalDate = new DateTimePickerCustom("Ngày về", "") {
                Width = 300,
                Margin = new Padding(0, 0, 32, 0)
            };

            txtDeparturePlace = new UnderlinedTextField("Nơi cất cánh", "") {
                Width = 300,
                Margin = new Padding(0, 0, 32, 0)
            };
            txtArrivalPlace = new UnderlinedTextField("Nơi hạ cánh", "") {
                Width = 300,
                Margin = new Padding(0, 0, 32, 0)
            };
            filterPanel.Controls.AddRange(new Control[] { dtpDepartureDate, dtpArrivalDate, txtDeparturePlace, txtArrivalPlace });

            btnPanel = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
            var btnSearchFlight = new PrimaryButton("🔍 Tìm chuyến bay") {
                Width = 160,
                Height = 36,
                Margin = new Padding(0, 0, 0, 0)
            };
            btnPanel.Controls.Add(btnSearchFlight);

            filterWrapPanel = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 16, 24, 0),
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1
            };
            filterWrapPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            filterWrapPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filterWrapPanel.Controls.Add(filterPanel, 0, 0);
            filterWrapPanel.Controls.Add(btnPanel, 1, 0);

            // ===== Table =====
            table = new TableCustom {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 24),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Cột hiển thị (khớp DB + nghiệp vụ)
            table.Columns.Add("flightNumber", "Mã chuyến bay");  // Flights.flight_number
            table.Columns.Add("fromAirport", "Nơi cất cánh");    // Airports.code từ Routes.from_airport_id
            table.Columns.Add("toAirport", "Nơi hạ cánh");     // Airports.code từ Routes.to_airport_id
            table.Columns.Add("departureTime", "Giờ cất cánh");    // Flights.departure_time
            table.Columns.Add("arrivalTime", "Giờ hạ cánh");     // Flights.arrival_time
            table.Columns.Add("status", "Trạng thái");      // Flights.status
            table.Columns.Add("seatAvailable", "Số ghế trống");    // COUNT(Flight_Seats WHERE AVAILABLE)

            // Cột Thao tác (vẽ custom link)
            var colAction = new DataGridViewTextBoxColumn {
                Name = ACTION_COL_NAME,
                HeaderText = "Thao tác",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            table.Columns.Add(colAction);

            // Khóa kỹ thuật ẩn: flight_id
            var colIdHidden = new DataGridViewTextBoxColumn {
                Name = "flightIdHidden",
                HeaderText = "",
                Visible = false
            };
            table.Columns.Add(colIdHidden);

            // Demo data (có thể bỏ)
            table.Rows.Add("VN123", "SGN (HCM)", "HAN (HN)",
                DateTime.Now.AddHours(2).ToString("HH:mm dd/MM/yyyy"),
                DateTime.Now.AddHours(3.5).ToString("HH:mm dd/MM/yyyy"),
                "SCHEDULED", 20, null, 1001);
            table.Rows.Add("VN456", "DAD (ĐN)", "HAN (HN)",
                DateTime.Now.AddHours(4).ToString("HH:mm dd/MM/yyyy"),
                DateTime.Now.AddHours(5.5).ToString("HH:mm dd/MM/yyyy"),
                "DELAYED", 15, null, 1002);
            table.Rows.Add("VN789", "SGN (HCM)", "DAD (ĐN)",
                DateTime.Now.AddHours(6).ToString("HH:mm dd/MM/yyyy"),
                DateTime.Now.AddHours(7.2).ToString("HH:mm dd/MM/yyyy"),
                "SCHEDULED", 5, null, 1003);

            // Vẽ/hover/click cho cột thao tác
            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // ===== Main panel =====
            mainPanel = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 3
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // Title
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // Filter
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Table

            mainPanel.Controls.Add(lblTitle, 0, 0);
            mainPanel.Controls.Add(filterWrapPanel, 0, 1);
            mainPanel.Controls.Add(table, 0, 2);

            Controls.Clear();
            Controls.Add(mainPanel);

            ResumeLayout(false);
        }

        // === Helpers for Action column ===
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDelete) GetActionRects(Rectangle cellBounds, Font font) {
            int padding = 6;
            int x = cellBounds.Left + padding;
            int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;

            var flags = TextFormatFlags.NoPadding;
            var szView = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szSep = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szEdit = TextRenderer.MeasureText(TXT_EDIT, font, Size.Empty, flags);
            var szDel = TextRenderer.MeasureText(TXT_DELETE, font, Size.Empty, flags);

            var rcView = new Rectangle(new Point(x, y), szView); x += szView.Width + szSep.Width;
            var rcEdit = new Rectangle(new Point(x, y), szEdit); x += szEdit.Width + szSep.Width;
            var rcDel = new Rectangle(new Point(x, y), szDel);

            return (rcView, rcEdit, rcDel);
        }

        private void Table_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) return;

            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var font = e.CellStyle.Font ?? table.Font;
            var rects = GetActionRects(e.CellBounds, font);

            Color link = Color.FromArgb(0, 92, 175);
            Color sep = Color.FromArgb(120, 120, 120);
            Color del = Color.FromArgb(220, 53, 69);

            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, rects.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rects.rcView.Right, rects.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, rects.rcEdit.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rects.rcEdit.Right, rects.rcEdit.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_DELETE, font, rects.rcDelete.Location, del, TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object? sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) { table.Cursor = Cursors.Default; return; }

            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(cellRect, font);

            var p = new Point(e.Location.X + cellRect.Left, e.Location.Y + cellRect.Top);
            bool over = rects.rcView.Contains(p) || rects.rcEdit.Contains(p) || rects.rcDelete.Contains(p);
            table.Cursor = over ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) return;

            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(cellRect, font);
            var p = new Point(e.Location.X + cellRect.Left, e.Location.Y + cellRect.Top);

            var row = table.Rows[e.RowIndex];

            string flightId = row.Cells["flightIdHidden"].Value?.ToString() ?? string.Empty; // khóa kỹ thuật
            string flightNumber = row.Cells["flightNumber"].Value?.ToString() ?? "(n/a)";
            string fromAirport = row.Cells["fromAirport"].Value?.ToString() ?? "(n/a)";
            string toAirport = row.Cells["toAirport"].Value?.ToString() ?? "(n/a)";
            string departureTime = row.Cells["departureTime"].Value?.ToString() ?? "(n/a)";
            string arrivalTime = row.Cells["arrivalTime"].Value?.ToString() ?? "(n/a)";
            string seatAvailable = row.Cells["seatAvailable"].Value?.ToString() ?? "(n/a)";

            if (rects.rcView.Contains(p)) {
                using (var frm = new FlightDetailForm(flightNumber, fromAirport, toAirport, departureTime, arrivalTime, seatAvailable)) {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(FindForm());
                }
            } else if (rects.rcEdit.Contains(p)) {
                MessageBox.Show($"Bạn đã chọn SỬA - Flight #{flightId} ({flightNumber})", "Sửa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (rects.rcDelete.Contains(p)) {
                MessageBox.Show($"Bạn đã chọn XÓA - Flight #{flightId} ({flightNumber})", "Xóa",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    // Popup form bọc FlightDetailControl + nạp dữ liệu
    internal class FlightDetailForm : Form {
        public FlightDetailForm(string flightNumber, string fromAirport, string toAirport, string departureTime, string arrivalTime, string seatAvailable) {
            Text = $"Chi tiết chuyến bay {flightNumber}";
            Size = new Size(900, 600);
            BackColor = Color.White;

            var detail = new FlightDetailControl { Dock = DockStyle.Fill };
            detail.LoadFlightInfo(flightNumber, fromAirport, toAirport, departureTime, arrivalTime, seatAvailable);

            Controls.Add(detail);
        }
    }
}
