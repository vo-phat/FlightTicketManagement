using System;
using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Inputs;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Tables;

namespace FlightTicketManagement.GUI.Features.FareRules.SubFeatures {
    public class FareRuleListControl : UserControl {
        private TableCustom table;

        private const string ACTION_COL = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DEL = "Xóa";
        private const string SEP = " / ";

        private TableLayoutPanel root, filterWrap;
        private FlowLayoutPanel filterLeft, filterRight;
        private Label lblTitle;
        private UnderlinedTextField txtCode, txtName;
        private UnderlinedComboBox cbCabin, cbRefundable;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn colAction;
        private DataGridViewTextBoxColumn colHiddenId;

        public FareRuleListControl() { InitializeComponent(); }

        private void InitializeComponent() {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            lblTitle = new Label();
            filterLeft = new FlowLayoutPanel();
            txtCode = new UnderlinedTextField();
            txtName = new UnderlinedTextField();
            cbCabin = new UnderlinedComboBox();
            cbRefundable = new UnderlinedComboBox();
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
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
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
            filterLeft.Controls.Add(txtCode);
            filterLeft.Controls.Add(txtName);
            filterLeft.Controls.Add(cbCabin);
            filterLeft.Controls.Add(cbRefundable);
            filterLeft.Location = new Point(3, 3);
            filterLeft.Name = "filterLeft";
            filterLeft.Size = new Size(1, 94);
            filterLeft.TabIndex = 0;
            // 
            // txtCode
            // 
            txtCode.BackColor = Color.Transparent;
            txtCode.FocusedLineThickness = 3;
            txtCode.InheritParentBackColor = true;
            txtCode.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtCode.LabelText = "Mã rule";
            txtCode.LineColor = Color.FromArgb(40, 40, 40);
            txtCode.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtCode.LineThickness = 2;
            txtCode.Location = new Point(3, 3);
            txtCode.Name = "txtCode";
            txtCode.Padding = new Padding(0, 4, 0, 8);
            txtCode.PasswordChar = '\0';
            txtCode.PlaceholderText = "";
            txtCode.Size = new Size(150, 150);
            txtCode.TabIndex = 0;
            txtCode.TextForeColor = Color.FromArgb(30, 30, 30);
            txtCode.UnderlineSpacing = 2;
            txtCode.UseSystemPasswordChar = false;
            // 
            // txtName
            // 
            txtName.BackColor = Color.Transparent;
            txtName.FocusedLineThickness = 3;
            txtName.InheritParentBackColor = true;
            txtName.LabelForeColor = Color.FromArgb(70, 70, 70);
            txtName.LabelText = "Tên rule";
            txtName.LineColor = Color.FromArgb(40, 40, 40);
            txtName.LineColorFocused = Color.FromArgb(0, 92, 175);
            txtName.LineThickness = 2;
            txtName.Location = new Point(3, 159);
            txtName.Name = "txtName";
            txtName.Padding = new Padding(0, 4, 0, 8);
            txtName.PasswordChar = '\0';
            txtName.PlaceholderText = "";
            txtName.Size = new Size(150, 150);
            txtName.TabIndex = 1;
            txtName.TextForeColor = Color.FromArgb(30, 30, 30);
            txtName.UnderlineSpacing = 2;
            txtName.UseSystemPasswordChar = false;
            // 
            // cbCabin
            // 
            cbCabin.BackColor = Color.Transparent;
            cbCabin.Items.AddRange(new object[] { "Economy", "Premium Economy", "Business", "First" });
            cbCabin.LabelText = "Hạng vé";
            cbCabin.Location = new Point(3, 315);
            cbCabin.MinimumSize = new Size(140, 56);
            cbCabin.Name = "cbCabin";
            cbCabin.SelectedIndex = -1;
            cbCabin.SelectedItem = null;
            cbCabin.SelectedText = "";
            cbCabin.Size = new Size(150, 56);
            cbCabin.TabIndex = 2;
            // 
            // cbRefundable
            // 
            cbRefundable.BackColor = Color.Transparent;
            cbRefundable.Items.AddRange(new object[] { "Tất cả", "Có thể hoàn", "Không hoàn" });
            cbRefundable.LabelText = "Hoàn vé";
            cbRefundable.Location = new Point(3, 377);
            cbRefundable.MinimumSize = new Size(140, 56);
            cbRefundable.Name = "cbRefundable";
            cbRefundable.SelectedIndex = -1;
            cbRefundable.SelectedItem = null;
            cbRefundable.SelectedText = "";
            cbRefundable.Size = new Size(150, 56);
            cbRefundable.TabIndex = 3;
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
            table.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, colAction, colHiddenId });
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
            dataGridViewTextBoxColumn1.HeaderText = "Mã";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Tên rule";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Hạng vé";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Hoàn vé";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Width = 125;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Phí đổi (₫)";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.Width = 125;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Hành lý (kg)";
            dataGridViewTextBoxColumn6.MinimumWidth = 6;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Width = 125;
            // 
            // FareRuleListControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Controls.Add(root);
            Name = "FareRuleListControl";
            Size = new Size(1460, 577);
            filterLeft.ResumeLayout(false);
            filterWrap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)table).EndInit();
            root.ResumeLayout(false);
            ResumeLayout(false);
        }

        // ===== Action links =====
        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDel) GetRects(Rectangle cellBounds, Font font) {
            int pad = 6; int x = cellBounds.Left + pad; int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;
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
            e.Handled = true; e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
            var font = e.CellStyle.Font ?? table.Font; var r = GetRects(e.CellBounds, font);
            Color link = Color.FromArgb(0, 92, 175), sep = Color.FromArgb(120, 120, 120), del = Color.FromArgb(220, 53, 69);
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
            string id = row.Cells["fareRuleIdHidden"].Value?.ToString() ?? "";
            string code = row.Cells["ruleCode"].Value?.ToString() ?? "(n/a)";
            string name = row.Cells["ruleName"].Value?.ToString() ?? "(n/a)";
            string cabin = row.Cells["cabinClass"].Value?.ToString() ?? "(n/a)";
            string refd = row.Cells["refundable"].Value?.ToString() ?? "Không";
            string fee = row.Cells["changeFee"].Value?.ToString() ?? "0";
            string bag = row.Cells["baggage"].Value?.ToString() ?? "0";

            if (r.rcView.Contains(p)) {
                using (var frm = new FareRuleDetailForm(code, name, cabin, refd, fee, bag)) {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(FindForm());
                }
            } else if (r.rcEdit.Contains(p)) {
                MessageBox.Show($"Sửa rule #{id} ({code})", "Sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcDel.Contains(p)) {
                MessageBox.Show($"Xóa rule #{id} ({code})", "Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    internal class FareRuleDetailForm : Form {
        public FareRuleDetailForm(string code, string name, string cabin, string refundable, string changeFee, string baggage) {
            Text = $"Chi tiết Fare Rule {code}";
            Size = new Size(840, 540); BackColor = Color.White;
            var detail = new FareRuleDetailControl { Dock = DockStyle.Fill };
            detail.LoadRule(code, name, cabin, refundable, changeFee, baggage);
            Controls.Add(detail);
        }
    }
}
