using System;
using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Tables;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Inputs;

namespace FlightTicketManagement.GUI.Features.Flight.SubFeatures {
    public class FlightCreateControl : UserControl {
        public FlightCreateControl() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(232, 240, 252);

            // ===== Tiêu đề =====
            var titlePanel = new Panel {
                Dock = DockStyle.Top,
                Padding = new Padding(24, 20, 24, 0),
                Height = 60
            };
            var lblTitle = new Label {
                Text = "✈️ Tạo chuyến bay mới",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black
            };
            titlePanel.Controls.Add(lblTitle);

            // ===== Khối input (2 cột × 5 hàng) =====
            var inputPanel = new TableLayoutPanel {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 12, 24, 0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 5
            };
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            for (int i = 0; i < 5; i++)
                inputPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            // === Cột 1 ===
            inputPanel.Controls.Add(new UnderlinedTextField("Mã chuyến bay", "") { MinimumSize = new Size(0, 56), Width = 250 }, 0, 0);
            inputPanel.Controls.Add(new UnderlinedTextField("Nơi cất cánh", "") { MinimumSize = new Size(0, 56), Width = 250 }, 0, 1);
            inputPanel.Controls.Add(new UnderlinedTextField("Mã máy bay", "") { MinimumSize = new Size(0, 56), Width = 250 }, 0, 2);
            inputPanel.Controls.Add(new UnderlinedTextField("Giá vé cơ bản", "") { MinimumSize = new Size(0, 56), Width = 250 }, 0, 3);

            // === Cột 2 ===
            inputPanel.Controls.Add(new UnderlinedTextField("Nơi hạ cánh", "") { MinimumSize = new Size(0, 56), Width = 250 }, 1, 1);
            inputPanel.Controls.Add(new UnderlinedTextField("Mã tuyến bay", "") { MinimumSize = new Size(0, 56), Width = 250 }, 1, 2);
            inputPanel.Controls.Add(new DateTimePickerCustom("Thời gian khởi hành", "") { Width = 250, Height = 56 }, 1, 3);
            inputPanel.Controls.Add(new DateTimePickerCustom("Thời gian hạ cánh", "") { Width = 250, Height = 56 }, 1, 4);

            // === Hàng thời lượng bay (có thể tự tính hoặc nhập tay) ===
            inputPanel.Controls.Add(new UnderlinedTextField("Thời gian bay (phút)", "") { MinimumSize = new Size(0, 56), Width = 250 }, 0, 4);

            // Sau khi Add các control vào inputPanel:
            for (int r = 0; r < inputPanel.RowCount; r++) {
                int h = 0;
                for (int c = 0; c < inputPanel.ColumnCount; c++) {
                    var ctl = inputPanel.GetControlFromPosition(c, r);
                    if (ctl != null) {
                        h = Math.Max(h, ctl.GetPreferredSize(Size.Empty).Height + ctl.Margin.Vertical);
                    }
                }
                inputPanel.RowStyles[r] = new RowStyle(SizeType.Absolute, Math.Max(72, h));
            }


            // ===== Nút tạo mới =====
            var btnCreate = new PrimaryButton("✈️ Tạo chuyến bay") {
                Height = 40,
                Width = 180,
                Margin = new Padding(0, 12, 0, 12),
                Anchor = AnchorStyles.Right
            };
            var buttonRow = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(24, 0, 24, 0),
                WrapContents = false
            };
            buttonRow.Controls.Add(btnCreate);

            // ===== Bảng danh sách chuyến bay =====
            var table = new TableCustom {
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
            table.Columns.Add("flightId", "Mã chuyến bay");
            table.Columns.Add("aircraftId", "Mã máy bay");
            table.Columns.Add("routeId", "Mã tuyến bay");
            table.Columns.Add("departureAirport", "Nơi cất cánh");
            table.Columns.Add("arrivalAirport", "Nơi hạ cánh");
            table.Columns.Add("departureTime", "Khởi hành");
            table.Columns.Add("arrivalTime", "Hạ cánh");
            table.Columns.Add("actions", "Thao tác");

            // mẫu hàng trống
            for (int i = 0; i < 4; i++)
                table.Rows.Add("", "", "", "", "", "", "", "");

            // ===== Layout tổng =====
            var main = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 4
            };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            main.Controls.Add(titlePanel, 0, 0);
            main.Controls.Add(inputPanel, 0, 1);
            main.Controls.Add(buttonRow, 0, 2);
            main.Controls.Add(table, 0, 3);

            this.Controls.Clear();
            this.Controls.Add(main);
        }
    }
}
