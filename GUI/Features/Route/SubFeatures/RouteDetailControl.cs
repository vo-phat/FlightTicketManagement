﻿using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Route.SubFeatures {
    public class RouteDetailControl : UserControl {
        private Label vFrom, vTo, vDist, vDur;

        public RouteDetailControl() { InitializeComponent(); BuildLayout(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);
        }

        private static Label Key(string t) => new Label { Text = t, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Margin = new Padding(0, 6, 12, 6) };
        private static Label Val(string n) => new Label { Name = n, AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Regular), Margin = new Padding(0, 6, 0, 6) };

        private void BuildLayout() {
            var title = new Label { Text = "🧭 Chi tiết tuyến bay", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold), Padding = new Padding(24, 20, 24, 0), Dock = DockStyle.Top };
            var card = new Panel { BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(16), Margin = new Padding(24, 8, 24, 24), Dock = DockStyle.Fill };

            var sec = new Label { Text = "Thông tin tuyến", AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Top, Margin = new Padding(0, 0, 0, 16) };
            var grid = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int r = 0;
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Sân bay đi:"), 0, r); vFrom = Val("vFrom"); grid.Controls.Add(vFrom, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Sân bay đến:"), 0, r); vTo = Val("vTo"); grid.Controls.Add(vTo, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Khoảng cách (km):"), 0, r); vDist = Val("vDist"); grid.Controls.Add(vDist, 1, r++);
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize)); grid.Controls.Add(Key("Thời lượng (phút):"), 0, r); vDur = Val("vDur"); grid.Controls.Add(vDur, 1, r++);

            card.Controls.Add(grid);
            card.Controls.Add(sec);

            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Padding = new Padding(0, 12, 12, 12) };
            var btnClose = new Button { Text = "Đóng", AutoSize = true }; btnClose.Click += (_, __) => FindForm()?.Close(); bottom.Controls.Add(btnClose);
            card.Controls.Add(bottom);

            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.Controls.Add(title, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Add(main);
        }

        public void LoadRouteInfo(string from, string to, string distanceKm, string durationMin) {
            vFrom.Text = from ?? ""; vTo.Text = to ?? "";
            vDist.Text = distanceKm ?? "0"; vDur.Text = durationMin ?? "0";
        }
    }
}
