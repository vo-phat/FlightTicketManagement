using System;
using System.Drawing;
using System.Windows.Forms;
using DTO.Aircraft; // DTO của bạn

namespace GUI.Features.Aircraft.SubFeatures
{
    public class AircraftDetailControl : UserControl
    {
        private Label vRegNum, vModel, vManu, vCap, vYear, vStatus;

        // Sự kiện để báo cho control cha biết khi bấm nút Đóng
        public event EventHandler CloseRequested;

        public AircraftDetailControl()
        {
            InitializeComponent();
            BuildLayout();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // AircraftDetailControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Name = "AircraftDetailControl";
            Size = new Size(1074, 527);
            Load += AircraftDetailControl_Load;
            ResumeLayout(false);
        }

        private static Label Key(string t) => new Label
        {
            Text = t,
            AutoSize = true,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            Margin = new Padding(0, 6, 12, 6)
        };
        private static Label Val(string n) => new Label
        {
            Name = n,
            AutoSize = true,
            Font = new Font("Segoe UI", 10f),
            Margin = new Padding(0, 6, 0, 6)
        };

        private void BuildLayout()
        {
            var title = new Label { Text = "🛩 Chi tiết máy bay", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold), Padding = new Padding(24, 20, 24, 0), Dock = DockStyle.Top };
            var card = new Panel { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(16), Margin = new Padding(24, 8, 24, 24), Dock = DockStyle.Fill };

            var grid = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int r = 0;
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Đã XOÁ: Số hiệu đăng ký:"), 0, r); vRegNum = Val("vRegNum"); grid.Controls.Add(vRegNum, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Model:"), 0, r); vModel = Val("vModel"); grid.Controls.Add(vModel, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Hãng sản xuất:"), 0, r); vManu = Val("vManu"); grid.Controls.Add(vManu, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Sức chứa (ghế):"), 0, r); vCap = Val("vCap"); grid.Controls.Add(vCap, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Năm sản xuất:"), 0, r); vYear = Val("vYear"); grid.Controls.Add(vYear, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Trạng thái:"), 0, r); vStatus = Val("vStatus"); grid.Controls.Add(vStatus, 1, r++);

            card.Controls.Add(grid);

            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 12, 12, 12) };
            var btnClose = new Button { Text = "Đóng", AutoSize = true };
            btnClose.Click += (_, __) => CloseRequested?.Invoke(this, EventArgs.Empty);
            bottom.Controls.Add(btnClose);
            card.Controls.Add(bottom);

            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(title, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Add(main);
        }

        // Nạp dữ liệu chi tiết từ DTO
        public void LoadAircraft(AircraftDTO dto)
        {
            if (dto == null) return;
            vRegNum.Text = dto.RegistrationNumber ?? "N/A";
            vModel.Text = dto.Model ?? "N/A";
            vManu.Text = dto.Manufacturer ?? "N/A";
            vCap.Text = dto.Capacity.HasValue ? dto.Capacity.Value.ToString() : "N/A";
            vYear.Text = dto.ManufactureYear.HasValue ? dto.ManufactureYear.Value.ToString() : "N/A";
            vStatus.Text = dto.Status ?? "N/A";
        }

        private void AircraftDetailControl_Load(object sender, EventArgs e)
        {

        }
    }
}