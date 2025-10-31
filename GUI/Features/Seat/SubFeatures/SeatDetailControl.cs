using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Seat.SubFeatures
{
    public class SeatDetailControl : UserControl
    {
        private Label lblTitle;
        private TableLayoutPanel root, leftForm;
        private Panel rightPanel;

        private UnderlinedTextField txtSeat, txtClass, txtAircraft;
        private SecondaryButton btnEdit, btnCancel;
        private PrimaryButton btnSave;

        private TableCustom tableHistory;
        private bool editing = false;

        public SeatDetailControl() { InitializeComponent(); LoadDemo(); }

        private void InitializeComponent()
        {
            SuspendLayout();
            Dock = DockStyle.Fill; BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label
            {
                Text = "ℹ️ Chi tiết ghế",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, Padding = new Padding(0, 0, 0, 12) };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));

            // Left form
            leftForm = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(24), ColumnCount = 2 };
            leftForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            leftForm.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            txtSeat = new UnderlinedTextField("", "") { Width = 240 };
            txtClass = new UnderlinedTextField("", "") { Width = 240 };
            txtAircraft = new UnderlinedTextField("", "") { Width = 240 };

            leftForm.Controls.Add(new Label { Text = "Số ghế", AutoSize = true, Margin = new Padding(0, 8, 8, 8) }, 0, 0);
            leftForm.Controls.Add(txtSeat, 1, 0);
            leftForm.Controls.Add(new Label { Text = "Hạng", AutoSize = true, Margin = new Padding(0, 8, 8, 8) }, 0, 1);
            leftForm.Controls.Add(txtClass, 1, 1);
            leftForm.Controls.Add(new Label { Text = "Máy bay", AutoSize = true, Margin = new Padding(0, 8, 8, 8) }, 0, 2);
            leftForm.Controls.Add(txtAircraft, 1, 2);

            var actions = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(24, 6, 24, 6), WrapContents = false };
            btnEdit = new SecondaryButton("✏️ Sửa") { Width = 90, Height = 36 };
            btnSave = new PrimaryButton("💾 Lưu") { Width = 90, Height = 36, Enabled = false, Margin = new Padding(12, 0, 0, 0) };
            btnCancel = new SecondaryButton("✖ Hủy") { Width = 90, Height = 36, Enabled = false, Margin = new Padding(12, 0, 0, 0) };
            btnEdit.Click += (_, __) => SetEditing(true);
            btnCancel.Click += (_, __) => { SetEditing(false); LoadDemo(); };
            btnSave.Click += (_, __) => { SetEditing(false); MessageBox.Show("Đã lưu (demo)"); };
            actions.Controls.AddRange(new Control[] { btnEdit, btnSave, btnCancel });

            var leftPanel = new Panel { Dock = DockStyle.Fill };
            leftPanel.Controls.Add(actions);
            leftPanel.Controls.Add(leftForm);

            // Right history
            rightPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 16, 24, 24) };
            tableHistory = new TableCustom
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            tableHistory.Columns.Add("flight", "Chuyến bay");
            tableHistory.Columns.Add("status", "Trạng thái");
            tableHistory.Columns.Add("price", "Giá (₫)");
            tableHistory.Columns.Add("date", "Ngày");
            rightPanel.Controls.Add(tableHistory);

            root.Controls.Add(lblTitle, 0, 0);
            root.SetColumnSpan(lblTitle, 2);
            root.Controls.Add(leftPanel, 0, 1);
            root.Controls.Add(rightPanel, 1, 1);

            Controls.Add(root);
            ResumeLayout(false);
        }

        private void SetEditing(bool on)
        {
            editing = on;
            //txtClass.ReadOnly = !on;
            //txtAircraft.ReadOnly = !on;
            btnSave.Enabled = on;
            btnCancel.Enabled = on;
            btnEdit.Enabled = !on;
        }

        private void LoadDemo()
        {
            txtSeat.Text = "12A";
            txtClass.Text = "Economy";
            txtAircraft.Text = "A320";
            tableHistory.Rows.Clear();
            tableHistory.Rows.Add("VN001", "BOOKED", 900000.ToString("#,0"), DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            tableHistory.Rows.Add("VN002", "AVAILABLE", 900000.ToString("#,0"), DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd"));
        }
    }
}