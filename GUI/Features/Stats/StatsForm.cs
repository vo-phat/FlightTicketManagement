using System.Windows.Forms;

namespace GUI.Features.Stats
{
    public partial class StatsForm : Form
    {
        public StatsForm()
        {
            InitializeComponent();
            var statsControl = new StatsControl();
            statsControl.Dock = DockStyle.Fill;
            this.Controls.Add(statsControl);
        }
    }
}
