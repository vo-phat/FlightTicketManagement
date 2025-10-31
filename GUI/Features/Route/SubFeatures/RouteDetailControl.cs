using System;
using System.Drawing;
using System.Windows.Forms;
using DTO.Route;

namespace GUI.Features.Route.SubFeatures
{
    public class RouteDetailControl : UserControl
    {
        private Label vDep, vArr, vDist, vDur;
        public event EventHandler CloseRequested;

        public RouteDetailControl()
        {
            InitializeComponent();
            BuildLayout();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // RouteDetailControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Name = "RouteDetailControl";
            Size = new Size(1074, 527);
            Load += RouteDetailControl_Load;
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
            var title = new Label { Text = "🧭 Chi tiết tuyến bay", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold), Padding = new Padding(24, 20, 24, 0), Dock = DockStyle.Top };
            var card = new Panel { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(16), Margin = new Padding(24, 8, 24, 24), Dock = DockStyle.Fill };

            var grid = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int r = 0;
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("ID khởi hành (Place ID):"), 0, r); vDep = Val("vDep"); grid.Controls.Add(vDep, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("ID đến (Place ID):"), 0, r); vArr = Val("vArr"); grid.Controls.Add(vArr, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Khoảng cách (km):"), 0, r); vDist = Val("vDist"); grid.Controls.Add(vDist, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Thời gian bay (phút):"), 0, r); vDur = Val("vDur"); grid.Controls.Add(vDur, 1, r++);

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

        public void LoadRoute(RouteDTO dto)
        {
            if (dto == null) return;
            vDep.Text = dto.DeparturePlaceId.ToString();
            vArr.Text = dto.ArrivalPlaceId.ToString();
            vDist.Text = dto.DistanceKm.HasValue ? $"{dto.DistanceKm.Value} km" : "N/A";
            vDur.Text = dto.DurationMinutes.HasValue ? $"{dto.DurationMinutes.Value} phút" : "N/A";
        }

        private void RouteDetailControl_Load(object sender, EventArgs e)
        {

        }
    }
}