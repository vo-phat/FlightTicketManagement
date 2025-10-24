using System.Drawing;
using System.Windows.Forms;

namespace GUI.Components.Link {
    public class LinkMenuRenderer : ToolStripProfessionalRenderer {
        private readonly Color _link = Color.FromArgb(0, 92, 175);
        private readonly Color _hoverBg = Color.FromArgb(245, 247, 250);
        private readonly Color _border = Color.FromArgb(210, 220, 230);

        public LinkMenuRenderer() : base(new LinkMenuColorTable()) { }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
            var r = e.ToolStrip.ClientRectangle; r.Width -= 1; r.Height -= 1;
            using var pen = new Pen(_border);
            e.Graphics.DrawRectangle(pen, r);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {
            using var bg = new SolidBrush(e.Item.Selected ? _hoverBg : Color.White);
            e.Graphics.FillRectangle(bg, new Rectangle(Point.Empty, e.Item.Bounds.Size));
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {
            bool hover = e.Item.Selected && e.Item.Enabled;
            using var font = new Font(e.TextFont, hover ? FontStyle.Underline : FontStyle.Regular);

            TextRenderer.DrawText(
                e.Graphics,
                e.Text,
                font,
                e.TextRectangle,
                e.Item.Enabled ? _link : Color.Gray,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left
            );
        }
    }
}
