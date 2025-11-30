using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.Flight;
using GUI.Components.Inputs;
using GUI.Components.Tables;


namespace GUI.Features.Flight.SubFeatures {
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
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn colAction;
        private DataGridViewTextBoxColumn colIdHidden;
        private UnderlinedTextField txtArrivalPlace;

        public FlightListControl() {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            lblTitle = new Label();
            filterPanel = new FlowLayoutPanel();
            dtpDepartureDate = new DateTimePickerCustom();
            dtpArrivalDate = new DateTimePickerCustom();
            txtDeparturePlace = new UnderlinedTextField();
            txtArrivalPlace = new UnderlinedTextField();
            btnPanel = new FlowLayoutPanel();
            filterWrapPanel = new TableLayoutPanel();
            table = new TableCustom();
            colAction = new DataGridViewTextBoxColumn();
            colIdHidden = new DataGridViewTextBoxColumn();
            mainPanel = new TableLayoutPanel();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            filterPanel.SuspendLayout();
            filterWrapPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)table).BeginInit();
            mainPanel.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(3, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(100, 23);
            lblTitle.TabIndex = 0;
            // 
            // filterPanel
            // 
            filterPanel.Controls.Add(dtpDepartureDate);
            filterPanel.Controls.Add(dtpArrivalDate);
            filterPanel.Controls.Add(txtDeparturePlace);
            filterPanel.Controls.Add(txtArrivalPlace);
            filterPanel.Location = new Point(3, 3);
            filterPanel.Name = "filterPanel";
            filterPanel.Size = new Size(1, 94);
            filterPanel.TabIndex = 0;
            // 
            // dtpDepartureDate
            // 
            dtpDepartureDate.BackColor = Color.Transparent;
            dtpDepartureDate.CustomFormat = null;
            dtpDepartureDate.LabelText = "Ngày đi";
            dtpDepartureDate.Location = new Point(3, 3);
            dtpDepartureDate.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpDepartureDate.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpDepartureDate.Name = "dtpDepartureDate";
            dtpDepartureDate.Padding = new Padding(0, 4, 0, 8);
            dtpDepartureDate.PlaceholderText = "";
            dtpDepartureDate.Size = new Size(150, 150);
            dtpDepartureDate.TabIndex = 0;
            dtpDepartureDate.Value = new DateTime(2025, 10, 30, 8, 11, 13, 662);
            // 
            // dtpArrivalDate
            // 
            dtpArrivalDate.BackColor = Color.Transparent;
            dtpArrivalDate.CustomFormat = null;
            dtpArrivalDate.LabelText = "Ngày về";
            dtpArrivalDate.Location = new Point(3, 159);
            dtpArrivalDate.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpArrivalDate.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpArrivalDate.Name = "dtpArrivalDate";
            dtpArrivalDate.Padding = new Padding(0, 4, 0, 8);
            dtpArrivalDate.PlaceholderText = "";
            dtpArrivalDate.Size = new Size(150, 150);
            dtpArrivalDate.TabIndex = 1;
            dtpArrivalDate.Value = new DateTime(2025, 10, 30, 8, 11, 13, 668);
            // 
            // txtDeparturePlace
            // 
            txtDeparturePlace.BackColor = Color.Transparent;
            txtDeparturePlace.FocusedLineThickness = 3;
            txtDeparturePlace.InheritParentBackColor = true;
            txtDeparturePlace.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtDeparturePlace.LabelText = "Nơi cất cánh";
            txtDeparturePlace.LineColor = Color.FromArgb(40, 40, 40);
            txtDeparturePlace.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtDeparturePlace.LineThickness = 2;
            txtDeparturePlace.Location = new Point(3, 315);
            txtDeparturePlace.Name = "txtDeparturePlace";
            txtDeparturePlace.Padding = new Padding(0, 4, 0, 8);
            txtDeparturePlace.PasswordChar = '\0';
            txtDeparturePlace.PlaceholderText = "";
            txtDeparturePlace.ReadOnly = false;
            txtDeparturePlace.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtDeparturePlace.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtDeparturePlace.Size = new Size(150, 150);
            txtDeparturePlace.TabIndex = 2;
            txtDeparturePlace.TextForeColor = Color.FromArgb(30, 30, 30);
            txtDeparturePlace.UnderlineSpacing = 2;
            txtDeparturePlace.UseSystemPasswordChar = false;
            // 
            // txtArrivalPlace
            // 
            txtArrivalPlace.BackColor = Color.Transparent;
            txtArrivalPlace.FocusedLineThickness = 3;
            txtArrivalPlace.InheritParentBackColor = true;
            txtArrivalPlace.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtArrivalPlace.LabelText = "Nơi hạ cánh";
            txtArrivalPlace.LineColor = Color.FromArgb(40, 40, 40);
            txtArrivalPlace.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtArrivalPlace.LineThickness = 2;
            txtArrivalPlace.Location = new Point(3, 471);
            txtArrivalPlace.Name = "txtArrivalPlace";
            txtArrivalPlace.Padding = new Padding(0, 4, 0, 8);
            txtArrivalPlace.PasswordChar = '\0';
            txtArrivalPlace.PlaceholderText = "";
            txtArrivalPlace.ReadOnly = false;
            txtArrivalPlace.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            txtArrivalPlace.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            txtArrivalPlace.Size = new Size(150, 150);
            txtArrivalPlace.TabIndex = 3;
            txtArrivalPlace.TextForeColor = Color.FromArgb(30, 30, 30);
            txtArrivalPlace.UnderlineSpacing = 2;
            txtArrivalPlace.UseSystemPasswordChar = false;
            // 
            // btnPanel
            // 
            btnPanel.Location = new Point(-9, 3);
            btnPanel.Name = "btnPanel";
            btnPanel.Size = new Size(200, 94);
            btnPanel.TabIndex = 1;
            // 
            // filterWrapPanel
            // 
            filterWrapPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            filterWrapPanel.ColumnStyles.Add(new ColumnStyle());
            filterWrapPanel.Controls.Add(filterPanel, 0, 0);
            filterWrapPanel.Controls.Add(btnPanel, 1, 0);
            filterWrapPanel.Location = new Point(3, 26);
            filterWrapPanel.Name = "filterWrapPanel";
            filterWrapPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            filterWrapPanel.Size = new Size(194, 100);
            filterWrapPanel.TabIndex = 1;
            // 
            // table
            // 
            table.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            table.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            table.BackgroundColor = Color.White;
            table.BorderColor = Color.FromArgb(40, 40, 40);
            table.BorderStyle = BorderStyle.None;
            table.BorderThickness = 2;
            table.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            table.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            table.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            table.ColumnHeadersHeight = 44;
            table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            table.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, colAction, colIdHidden });
            table.CornerRadius = 16;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            table.DefaultCellStyle = dataGridViewCellStyle3;
            table.EnableHeadersVisualStyles = false;
            table.GridColor = Color.FromArgb(230, 235, 240);
            table.HeaderBackColor = Color.White;
            table.HeaderForeColor = Color.FromArgb(126, 185, 232);
            table.HoverBackColor = Color.FromArgb(232, 245, 255);
            table.Location = new Point(3, 132);
            table.MultiSelect = false;
            table.Name = "table";
            table.RowAltBackColor = Color.FromArgb(248, 250, 252);
            table.RowBackColor = Color.White;
            table.RowForeColor = Color.FromArgb(33, 37, 41);
            table.RowHeadersVisible = false;
            table.RowHeadersWidth = 51;
            table.SelectionBackColor = Color.FromArgb(155, 209, 243);
            table.SelectionForeColor = Color.White;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.Size = new Size(194, 1);
            table.TabIndex = 2;
            table.CellMouseClick += Table_CellMouseClick;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellPainting += Table_CellPainting;
            // 
            // colAction
            // 
            colAction.MinimumWidth = 6;
            colAction.Name = "colAction";
            colAction.Width = 125;
            // 
            // colIdHidden
            // 
            colIdHidden.MinimumWidth = 6;
            colIdHidden.Name = "colIdHidden";
            colIdHidden.Width = 125;
            // 
            // mainPanel
            // 
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            mainPanel.Controls.Add(lblTitle, 0, 0);
            mainPanel.Controls.Add(filterWrapPanel, 0, 1);
            mainPanel.Controls.Add(table, 0, 2);
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.RowStyles.Add(new RowStyle());
            mainPanel.RowStyles.Add(new RowStyle());
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainPanel.Size = new Size(200, 100);
            mainPanel.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Mã chuyến bay";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Nơi cất cánh";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Nơi hạ cánh";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Giờ cất cánh";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Width = 125;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Giờ hạ cánh";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.Width = 125;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Trạng thái";
            dataGridViewTextBoxColumn6.MinimumWidth = 6;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Width = 125;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Số ghế trống";
            dataGridViewTextBoxColumn7.MinimumWidth = 6;
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.Width = 125;
            // 
            // FlightListControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Controls.Add(mainPanel);
            Name = "FlightListControl";
            Size = new Size(1257, 810);
            filterPanel.ResumeLayout(false);
            filterWrapPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)table).EndInit();
            mainPanel.ResumeLayout(false);
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

            string flightId = row.Cells["flightIdHidden"].Value?.ToString() ?? string.Empty;
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
