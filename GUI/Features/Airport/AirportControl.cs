using System.Windows.Forms;

namespace GUI.Features.Airport
{
    public class AirportControl : UserControl
    {
        private SubFeatures.AirportListControl list;
        private SubFeatures.AirportCreateControl create;

        public AirportControl()
        {
            Dock = DockStyle.Fill;
            list = new SubFeatures.AirportListControl { Dock = DockStyle.Fill };
            create = new SubFeatures.AirportCreateControl { Dock = DockStyle.Fill };

            create.AirportAdded += (_, __) => list.Refresh(); // khi thêm mới, reload danh sách
            Controls.Add(list);
            Controls.Add(create);
            create.Visible = false;
        }
    }
}
