using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Aircraft.SubFeatures {
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
        private UnderlinedTextField txtModel, txtManufacturer, txtCapacity, txtMaxCap;
        private UnderlinedComboBox cbAirlineCode;

        public AircraftListControl() { InitializeComponent(); }

        private void InitializeComponent() {
            SuspendLayout();

            // ===== Root =====
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // ===== Title =====
            lblTitle = new Label {
                Text = "🛩️ Danh sách máy bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Filters =====
            filterLeft = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight
            };

            cbAirlineCode = new UnderlinedComboBox("Mã hãng", new object[] { "AL001", "AL002", "AL003" }) { 
                MinimumSize = new Size(0, 64),
                Width = 250, 
                Margin = new Padding(0, 0, 24, 0) 
            };
            txtModel = new UnderlinedTextField("Model", "") { 
                Width = 250, 
                Margin = new Padding(0, 0, 24, 0) 
            };
            txtManufacturer = new UnderlinedTextField("Hãng sản xuất", "") { 
                Width = 250, 
                Margin = new Padding(0, 0, 24, 0) 
            };
            txtCapacity = new UnderlinedTextField("Sức chứa", "") { 
                Width = 250, 
                Margin = new Padding(0, 0, 24, 0) 
            };

            filterLeft.Controls.AddRange(new Control[] { cbAirlineCode, txtModel, txtManufacturer, txtCapacity });

            filterRight = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
            var btnSearch = new PrimaryButton("🔍 Tìm máy bay") { Width = 140, Height = 36 };
            filterRight.Controls.Add(btnSearch);

            filterWrap = new TableLayoutPanel {
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 10, 24, 0),
                ColumnCount = 2
            };
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            filterWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            filterWrap.Controls.Add(filterLeft, 0, 0);
            filterWrap.Controls.Add(filterRight, 1, 0);

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

            // cột hiển thị — dùng "Name" đúng với truy cập ở handler
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "airline", HeaderText = "Hãng" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "model", HeaderText = "Model" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "manufacturer", HeaderText = "Hãng sản xuất" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "capacity", HeaderText = "Sức chứa" });
            table.Columns.Add(new DataGridViewTextBoxColumn { Name = "seats", HeaderText = "Số ghế cấu hình" });

            var colAction = new DataGridViewTextBoxColumn {
                Name = ACTION_COL,
                HeaderText = "Thao tác",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            table.Columns.Add(colAction);

            var colHiddenId = new DataGridViewTextBoxColumn {
                Name = "aircraftIdHidden",
                HeaderText = "",
                Visible = false
            };
            table.Columns.Add(colHiddenId);

            // demo rows (thay bằng dữ liệu thật)
            table.Rows.Add("VNA", "A321neo", "Airbus", 220, 220, null, 101);
            table.Rows.Add("VNA", "B787-10", "Boeing", 330, 330, null, 102);
            table.Rows.Add("BBA", "E190", "Embraer", 100, 100, null, 103);

            // vẽ/hover/click cột thao tác
            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;

            // ===== Root layout =====
            root = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 3
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            root.Controls.Add(lblTitle, 0, 0);
            root.Controls.Add(filterWrap, 0, 1);
            root.Controls.Add(table, 0, 2);

            Controls.Clear();
            Controls.Add(root);

            ResumeLayout(false);
        }

        // === Helpers vẽ link trong cột thao tác ===
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

            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var r = GetRects(cellRect, font);

            // location trong toạ độ cell
            var local = new Point(e.Location.X + cellRect.Left, e.Location.Y + cellRect.Top);
            bool over = r.rcView.Contains(local) || r.rcEdit.Contains(local) || r.rcDel.Contains(local);
            table.Cursor = over ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object? s, DataGridViewCellMouseEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL) return;

            var row = table.Rows[e.RowIndex];

            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = row.InheritedStyle?.Font ?? table.Font;
            var r = GetRects(cellRect, font);

            // toạ độ tương đối cell
            var pScreen = table.PointToClient(Cursor.Position);
            var local = new Point(pScreen.X, pScreen.Y);

            if (r.rcView.Contains(local)) {
                string airline = row.Cells["airline"].Value?.ToString() ?? "(n/a)";
                string model = row.Cells["model"].Value?.ToString() ?? "(n/a)";
                string manufacturer = row.Cells["manufacturer"].Value?.ToString() ?? "(n/a)";
                string capacity = row.Cells["capacity"].Value?.ToString() ?? "0";
                string seats = row.Cells["seats"].Value?.ToString() ?? "0";
                using (var frm = new AircraftDetailForm(airline, model, manufacturer, capacity, seats)) {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(FindForm());
                }
            } else if (r.rcEdit.Contains(local)) {
                string id = row.Cells["aircraftIdHidden"].Value?.ToString() ?? "";
                MessageBox.Show($"Sửa máy bay #{id}", "Sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (r.rcDel.Contains(local)) {
                string id = row.Cells["aircraftIdHidden"].Value?.ToString() ?? "";
                MessageBox.Show($"Xoá máy bay #{id}", "Xoá", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    internal class AircraftDetailForm : Form {
        public AircraftDetailForm(string airline, string model, string manufacturer, string capacity, string seats) {
            Text = $"Chi tiết máy bay {model}";
            Size = new Size(860, 540);
            BackColor = Color.White;

            var detail = new AircraftDetailControl { Dock = DockStyle.Fill };
            detail.LoadAircraftInfo(airline, model, manufacturer, capacity, seats);
            Controls.Add(detail);
        }
    }
}
    