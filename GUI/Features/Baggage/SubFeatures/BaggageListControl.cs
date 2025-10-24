using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Baggage.SubFeatures {
    public class BaggageListControl : UserControl {
        public class BaggageRow {
            public int BaggageId { get; set; }
            public string BaggageTag { get; set; } = "";
            public string Type { get; set; } = "";
            public decimal WeightKg { get; set; }
            public decimal AllowedWeightKg { get; set; }
            public decimal Fee { get; set; }
            public string Status { get; set; } = "";
            public int FlightId { get; set; }
            public int TicketId { get; set; }
        }

        public event Action<BaggageRow>? OnViewRequested;

        private TableCustom table;
        private TableLayoutPanel mainPanel;
        private FlowLayoutPanel filterPanel, btnPanel;
        private Label lblTitle;
        private UnderlinedTextField txtTicketNumber, txtBaggageTag, txtStatus;
        private DateTimePickerCustom dtFrom, dtTo;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DELETE = "Xóa";
        private const string SEP = " / ";

        public BaggageListControl() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            SuspendLayout();

            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label {
                Text = "🧳 Danh sách hành lý",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            filterPanel = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            txtTicketNumber = new UnderlinedTextField("Số vé (Tickets.ticket_number)", "") { Width = 220, Margin = new Padding(0, 0, 24, 0) };
            txtBaggageTag = new UnderlinedTextField("Mã nhãn (Baggage.baggage_tag)", "") { Width = 220, Margin = new Padding(0, 0, 24, 0) };
            txtStatus = new UnderlinedTextField("Trạng thái (CREATED/...)", "") { Width = 180, Margin = new Padding(0, 0, 24, 0) };
            dtFrom = new DateTimePickerCustom("Từ ngày", "") { Width = 200, Margin = new Padding(0, 0, 24, 0) };
            dtTo = new DateTimePickerCustom("Đến ngày", "") { Width = 200, Margin = new Padding(0, 0, 24, 0) };

            filterPanel.Controls.AddRange(new Control[] { txtTicketNumber, txtBaggageTag, txtStatus, dtFrom, dtTo });

            btnPanel = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
            var btnSearch = new PrimaryButton("🔍 Tìm hành lý") { Width = 150, Height = 36 };
            btnPanel.Controls.Add(btnSearch);

            var filterWrap = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                ColumnCount = 2,
                AutoSize = true,
                Padding = new Padding(24, 8, 24, 0)
            };
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            filterWrap.Controls.Add(filterPanel, 0, 0);
            filterWrap.Controls.Add(btnPanel, 1, 0);

            table = new TableCustom {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 4),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            table.Columns.Add("baggageTag", "Mã nhãn");
            table.Columns.Add("type", "Loại");
            table.Columns.Add("weight", "Cân nặng (kg)");
            table.Columns.Add("allowed", "Định mức (kg)");
            table.Columns.Add("fee", "Phí");
            table.Columns.Add("status", "Trạng thái");
            table.Columns.Add("flightId", "Mã chuyến bay");
            table.Columns.Add("ticketId", "Mã vé");

            var colAction = new DataGridViewTextBoxColumn { Name = ACTION_COL, HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells };
            table.Columns.Add(colAction);

            var colIdHidden = new DataGridViewTextBoxColumn { Name = "baggageIdHidden", HeaderText = "", Visible = false };
            table.Columns.Add(colIdHidden);

            // DEMO rows (thay bằng dữ liệu thật)
            table.Rows.Add("BG123456", "CHECKED", "18.5", "20.0", "0.00", "CHECKED_IN", 1001, 501, null, 1);
            table.Rows.Add("BG654321", "CHECKED", "26.0", "23.0", "300000", "LOADED", 1002, 502, null, 2);

            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            mainPanel = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 3
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            mainPanel.Controls.Add(lblTitle, 0, 0);
            mainPanel.Controls.Add(filterWrap, 0, 1);
            mainPanel.Controls.Add(table, 0, 2);

            Controls.Clear();
            Controls.Add(mainPanel);

            ResumeLayout(false);
        }

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
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

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
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }

            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(cellRect, font);

            var p = new Point(e.Location.X + cellRect.Left, e.Location.Y + cellRect.Top);
            bool over = rects.rcView.Contains(p) || rects.rcEdit.Contains(p) || rects.rcDelete.Contains(p);
            table.Cursor = over ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? sender, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var row = table.Rows[e.RowIndex];
            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = row.InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(cellRect, font);
            var p = table.PointToClient(Cursor.Position);

            var rcCell = new Rectangle(cellRect.Location, cellRect.Size);
            var localPoint = new Point(p.X - rcCell.Left, p.Y - rcCell.Top);

            if (rects.rcView.Contains(localPoint)) {
                var data = new BaggageRow {
                    BaggageId = Convert.ToInt32(row.Cells["baggageIdHidden"].Value ?? 0),
                    BaggageTag = Convert.ToString(row.Cells["baggageTag"].Value) ?? "",
                    Type = Convert.ToString(row.Cells["type"].Value) ?? "",
                    WeightKg = Convert.ToDecimal(row.Cells["weight"].Value ?? 0),
                    AllowedWeightKg = Convert.ToDecimal(row.Cells["allowed"].Value ?? 0),
                    Fee = Convert.ToDecimal(row.Cells["fee"].Value ?? 0),
                    Status = Convert.ToString(row.Cells["status"].Value) ?? "",
                    FlightId = Convert.ToInt32(row.Cells["flightId"].Value ?? 0),
                    TicketId = Convert.ToInt32(row.Cells["ticketId"].Value ?? 0),
                };
                OnViewRequested?.Invoke(data);
            } else if (rects.rcEdit.Contains(localPoint)) {
                MessageBox.Show("Sửa (TODO)", "Baggage");
            } else if (rects.rcDelete.Contains(localPoint)) {
                MessageBox.Show("Xóa (TODO)", "Baggage");
            }
        }
    }
}
