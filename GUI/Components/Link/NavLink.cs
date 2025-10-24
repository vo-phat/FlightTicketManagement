using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Components.Link {
    // LinkLabel cho navbar: có trạng thái Active + dropdown menu
    public class NavLink : LinkLabel {
        public bool IsActive {
            get => _isActive;
            set { _isActive = value; ApplyStyle(); }
        }
        private bool _isActive;

        public ContextMenuStrip? DropMenu { get; set; }

        public NavLink(string text) {
            Text = text;
            AutoSize = true;
            LinkBehavior = LinkBehavior.HoverUnderline;
            Cursor = Cursors.Hand;

            // style mặc định (match màu brand đã dùng)
            LinkColor = Color.FromArgb(0, 92, 175);        // xanh brand
            ActiveLinkColor = Color.FromArgb(0, 92, 175);
            VisitedLinkColor = LinkColor;
            DisabledLinkColor = Color.Gray;

            Font = new Font("Segoe UI", 11f, FontStyle.Regular);
            Margin = new Padding(8, 8, 8, 8);
            Padding = new Padding(2, 2, 2, 2);

            // mở menu (nếu có)
            Click += (_, __) => {
                if (DropMenu != null && DropMenu.Items.Count > 0) {
                    var p = PointToScreen(new Point(0, Height));
                    DropMenu.Show(p);
                }
            };

            MouseEnter += (_, __) => { if (!IsActive) ForeColor = ActiveLinkColor; };
            MouseLeave += (_, __) => { if (!IsActive) ForeColor = LinkColor; };
        }

        private void ApplyStyle() {
            if (IsActive) {
                // Active: đậm + gạch chân nhẹ + màu nổi + “thanh chỉ báo” dưới
                Font = new Font(Font, FontStyle.Bold);
                ForeColor = ActiveLinkColor;
            } else {
                Font = new Font("Segoe UI", 11f, FontStyle.Regular);
                ForeColor = LinkColor;
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            // vẽ underline dày cho trạng thái Active (tab đang chọn)
            if (IsActive) {
                using var pen = new Pen(ActiveLinkColor, 2);
                var y = Bottom - 1;
                e.Graphics.DrawLine(pen, Left, y, Right, y);
            }
        }
    }
}
