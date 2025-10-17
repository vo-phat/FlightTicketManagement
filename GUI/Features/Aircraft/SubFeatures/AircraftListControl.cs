using System;
using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Tables;

namespace FlightTicketManagement.GUI.Features.Aircraft.SubFeatures {
    public class AircraftListControl : UserControl {
        private TableCustom table;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight;
        private Label lblTitle;
        private UnderlinedTextField txtAirlineCode, txtModel, txtManufacturer, txtMinCap, txtMaxCap;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn colAction;
        private DataGridViewTextBoxColumn colHiddenId;

        public AircraftListControl() { InitializeComponent(); }

        private void InitializeComponent() {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            lblTitle = new Label();
            filterLeft = new FlowLayoutPanel();
            txtAirlineCode = new UnderlinedTextField();
            txtModel = new UnderlinedTextField();
            txtManufacturer = new UnderlinedTextField();
            txtMinCap = new UnderlinedTextField();
            txtMaxCap = new UnderlinedTextField();
            filterRight = new FlowLayoutPanel();
            filterWrap = new TableLayoutPanel();
            table = new TableCustom();
            colAction = new DataGridViewTextBoxColumn();
            colHiddenId = new DataGridViewTextBoxColumn();
            root = new TableLayoutPanel();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            filterLeft.SuspendLayout();
            filterWrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)table).BeginInit();
            root.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(3, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(100, 23);
            lblTitle.TabIndex = 0;
            // 
            // filterLeft
            // 
            filterLeft.Controls.Add(txtAirlineCode);
            filterLeft.Controls.Add(txtModel);
            filterLeft.Controls.Add(txtManufacturer);
            filterLeft.Controls.Add(txtMinCap);
            filterLeft.Controls.Add(txtMaxCap);
            filterLeft.Location = new Point(3, 3);
            filterLeft.Name = "filterLeft";
            filterLeft.Size = new Size(1, 94);
            filterLeft.TabIndex = 0;
            // 
            // txtAirlineCode
            // 
            txtAirlineCode.BackColor = Color.Transparent;
            txtAirlineCode.FocusedLineThickness = 3;
            txtAirlineCode.InheritParentBackColor = true;
            txtAirlineCode.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtAirlineCode.LabelText = "Mã hãng (Airline)";
            txtAirlineCode.LineColor = Color.FromArgb(40, 40, 40);
            txtAirlineCode.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtAirlineCode.LineThickness = 2;
            txtAirlineCode.Location = new Point(3, 3);
            txtAirlineCode.Name = "txtAirlineCode";
            txtAirlineCode.Padding = new Padding(0, 4, 0, 8);
            txtAirlineCode.PasswordChar = '\0';
            txtAirlineCode.PlaceholderText = "";
            txtAirlineCode.Size = new Size(150, 150);
            txtAirlineCode.TabIndex = 0;
            txtAirlineCode.TextForeColor = Color.FromArgb(30, 30, 30);
            txtAirlineCode.UnderlineSpacing = 2;
            txtAirlineCode.UseSystemPasswordChar = false;
            // 
            // txtModel
            // 
            txtModel.BackColor = Color.Transparent;
            txtModel.FocusedLineThickness = 3;
            txtModel.InheritParentBackColor = true;
            txtModel.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtModel.LabelText = "Model";
            txtModel.LineColor = Color.FromArgb(40, 40, 40);
            txtModel.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtModel.LineThickness = 2;
            txtModel.Location = new Point(3, 159);
            txtModel.Name = "txtModel";
            txtModel.Padding = new Padding(0, 4, 0, 8);
            txtModel.PasswordChar = '\0';
            txtModel.PlaceholderText = "";
            txtModel.Size = new Size(150, 150);
            txtModel.TabIndex = 1;
            txtModel.TextForeColor = Color.FromArgb(30, 30, 30);
            txtModel.UnderlineSpacing = 2;
            txtModel.UseSystemPasswordChar = false;
            // 
            // txtManufacturer
            // 
            txtManufacturer.BackColor = Color.Transparent;
            txtManufacturer.FocusedLineThickness = 3;
            txtManufacturer.InheritParentBackColor = true;
            txtManufacturer.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtManufacturer.LabelText = "Hãng sản xuất";
            txtManufacturer.LineColor = Color.FromArgb(40, 40, 40);
            txtManufacturer.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtManufacturer.LineThickness = 2;
            txtManufacturer.Location = new Point(3, 315);
            txtManufacturer.Name = "txtManufacturer";
            txtManufacturer.Padding = new Padding(0, 4, 0, 8);
            txtManufacturer.PasswordChar = '\0';
            txtManufacturer.PlaceholderText = "";
            txtManufacturer.Size = new Size(150, 150);
            txtManufacturer.TabIndex = 2;
            txtManufacturer.TextForeColor = Color.FromArgb(30, 30, 30);
            txtManufacturer.UnderlineSpacing = 2;
            txtManufacturer.UseSystemPasswordChar = false;
            // 
            // txtMinCap
            // 
            txtMinCap.BackColor = Color.Transparent;
            txtMinCap.FocusedLineThickness = 3;
            txtMinCap.InheritParentBackColor = true;
            txtMinCap.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtMinCap.LabelText = "Sức chứa từ";
            txtMinCap.LineColor = Color.FromArgb(40, 40, 40);
            txtMinCap.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtMinCap.LineThickness = 2;
            txtMinCap.Location = new Point(3, 471);
            txtMinCap.Name = "txtMinCap";
            txtMinCap.Padding = new Padding(0, 4, 0, 8);
            txtMinCap.PasswordChar = '\0';
            txtMinCap.PlaceholderText = "";
            txtMinCap.Size = new Size(150, 150);
            txtMinCap.TabIndex = 3;
            txtMinCap.TextForeColor = Color.FromArgb(30, 30, 30);
            txtMinCap.UnderlineSpacing = 2;
            txtMinCap.UseSystemPasswordChar = false;
            // 
            // txtMaxCap
            // 
            txtMaxCap.BackColor = Color.Transparent;
            txtMaxCap.FocusedLineThickness = 3;
            txtMaxCap.InheritParentBackColor = true;
            txtMaxCap.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtMaxCap.LabelText = "Sức chứa đến";
            txtMaxCap.LineColor = Color.FromArgb(40, 40, 40);
            txtMaxCap.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtMaxCap.LineThickness = 2;
            txtMaxCap.Location = new Point(3, 627);
            txtMaxCap.Name = "txtMaxCap";
            txtMaxCap.Padding = new Padding(0, 4, 0, 8);
            txtMaxCap.PasswordChar = '\0';
            txtMaxCap.PlaceholderText = "";
            txtMaxCap.Size = new Size(150, 150);
            txtMaxCap.TabIndex = 4;
            txtMaxCap.TextForeColor = Color.FromArgb(30, 30, 30);
            txtMaxCap.UnderlineSpacing = 2;
            txtMaxCap.UseSystemPasswordChar = false;
            // 
            // filterRight
            // 
            filterRight.Location = new Point(-9, 3);
            filterRight.Name = "filterRight";
            filterRight.Size = new Size(200, 94);
            filterRight.TabIndex = 1;
            // 
            // filterWrap
            // 
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            filterWrap.ColumnStyles.Add(new ColumnStyle());
            filterWrap.Controls.Add(filterLeft, 0, 0);
            filterWrap.Controls.Add(filterRight, 1, 0);
            filterWrap.Location = new Point(3, 26);
            filterWrap.Name = "filterWrap";
            filterWrap.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            filterWrap.Size = new Size(194, 100);
            filterWrap.TabIndex = 1;
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
            table.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, colAction, colHiddenId });
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
            // colHiddenId
            // 
            colHiddenId.MinimumWidth = 6;
            colHiddenId.Name = "colHiddenId";
            colHiddenId.Width = 125;
            // 
            // root
            // 
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(filterWrap, 0, 1);
            root.Controls.Add(table, 0, 2);
            root.Location = new Point(0, 0);
            root.Name = "root";
            root.RowStyles.Add(new RowStyle());
            root.RowStyles.Add(new RowStyle());
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.Size = new Size(200, 100);
            root.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Hãng";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Model";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Hãng sản xuất";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Sức chứa";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Width = 125;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Số ghế cấu hình";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.Width = 125;
            // 
            // AircraftListControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Controls.Add(root);
            Name = "AircraftListControl";
            Size = new Size(1460, 577);
            filterLeft.ResumeLayout(false);
            filterWrap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)table).EndInit();
            root.ResumeLayout(false);
            ResumeLayout(false);
        }

        // Action column drawing
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDel) GetRects(Rectangle cellBounds, Font font) {
            int pad = 6;
            int x = cellBounds.Left + pad;
            int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;
            var flags = TextFormatFlags.NoPadding;
            var szV = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szS = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szE = TextRenderer.MeasureText(TXT_EDIT, font, Size.Empty, flags);
            var szD = TextRenderer.MeasureText(TXT_DEL, font, Size.Empty, flags);
            var rcV = new Rectangle(new Point(x, y), szV); x += szV.Width + szS.Width;
            var rcE = new Rectangle(new Point(x, y), szE); x += szE.Width + szS.Width;
            var rcD = new Rectangle(new Point(x, y), szD);
            return (rcV, rcE, rcD);
        }

        private void Table_CellPainting(object? s, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var font = e.CellStyle.Font ?? table.Font;
            var r = GetRects(e.CellBounds, font);

            Color link = Color.FromArgb(0, 92, 175);
            Color sep = Color.FromArgb(120, 120, 120);
            Color del = Color.FromArgb(220, 53, 69);

            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, r.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcView.Right, r.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, r.rcEdit.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(r.rcEdit.Right, r.rcEdit.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_DEL, font, r.rcDel.Location, del, TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) { table.Cursor = Cursors.Default; return; }

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);
            table.Cursor = (r.rcView.Contains(p) || r.rcEdit.Contains(p) || r.rcDel.Contains(p)) ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var rect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(rect, font);
            var p = new Point(e.Location.X + rect.Left, e.Location.Y + rect.Top);

            var row = table.Rows[e.RowIndex];
            string aircraftId = row.Cells["aircraftIdHidden"].Value?.ToString() ?? "";
            string airline = row.Cells["airline"].Value?.ToString() ?? "(n/a)";
            string model = row.Cells["model"].Value?.ToString() ?? "(n/a)";
            string manufacturer = row.Cells["manufacturer"].Value?.ToString() ?? "(n/a)";
            string capacity = row.Cells["capacity"].Value?.ToString() ?? "0";
            string seats = row.Cells["seats"].Value?.ToString() ?? "0";

            if (r.rcView.Contains(p)) {
                using (var frm = new AircraftDetailForm(airline, model, manufacturer, capacity, seats)) {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(FindForm());
                }
            } else if (r.rcEdit.Contains(p)) {
                MessageBox.Show($"Sửa máy bay #{aircraftId} ({model})", "Sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcDel.Contains(p)) {
                MessageBox.Show($"Xóa máy bay #{aircraftId} ({model})", "Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    internal class AircraftDetailForm : Form {
        public AircraftDetailForm(string airline, string model, string manufacturer, string capacity, string seats) {
            Text = $"Chi tiết máy bay {model}";
            Size = new Size(800, 520);
            BackColor = Color.White;

            var detail = new AircraftDetailControl { Dock = DockStyle.Fill };
            detail.LoadAircraftInfo(airline, model, manufacturer, capacity, seats);
            Controls.Add(detail);
        }
    }
}
