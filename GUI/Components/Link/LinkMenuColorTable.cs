using System.Drawing;
using System.Windows.Forms;

namespace GUI.Components.Link {
    public class LinkMenuColorTable : ProfessionalColorTable {
        // Nền dropdown trắng
        public override Color ToolStripDropDownBackground => Color.White;

        // Ẩn viền/nền chọn mặc định của item
        public override Color MenuItemSelected => Color.Transparent;
        public override Color MenuItemBorder => Color.Transparent;

        // Ẩn dải màu bên trái (image margin)
        public override Color ImageMarginGradientBegin => Color.White;
        public override Color ImageMarginGradientMiddle => Color.White;
        public override Color ImageMarginGradientEnd => Color.White;

        // (Tùy chọn) phẳng hoàn toàn menu strip/greedy gradients khác
        public override Color ToolStripGradientBegin => Color.White;
        public override Color ToolStripGradientMiddle => Color.White;
        public override Color ToolStripGradientEnd => Color.White;
    }
}
