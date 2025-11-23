using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;

namespace GUI.Features.CabinClass {
    public class CabinClassControl : UserControl {
        private Button btnList;
        private SubFeatures.CabinClassListControl list;
        private SubFeatures.CabinClassDetailControl detail;

        public CabinClassControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill; BackColor = Color.WhiteSmoke;

            btnList = new PrimaryButton("Danh sách Hạng ghế");

            var top = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            top.Controls.AddRange(new Control[] { btnList });

            list = new SubFeatures.CabinClassListControl { Dock = DockStyle.Fill };
            detail = new SubFeatures.CabinClassDetailControl { Dock = DockStyle.Fill };

            Controls.Add(list);
            Controls.Add(detail); Controls.Add(top);
        }
    }
}
