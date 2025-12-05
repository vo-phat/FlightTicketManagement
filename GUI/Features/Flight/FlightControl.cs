using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;

namespace GUI.Features.Flight
{
    public class FlightControl : UserControl
    {
        private Button btnList, btnCreate;
        private SubFeatures.FlightListControl list;
        private SubFeatures.FlightCreateControl create;
        private SubFeatures.FlightDetailControl detail;

        public FlightControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            btnList = new PrimaryButton("Danh sách chuyến bay");
            btnCreate = new SecondaryButton("Tạo chuyến bay");
            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);

            var top = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            top.Controls.AddRange(new Control[] { btnList, btnCreate });

            list = new SubFeatures.FlightListControl { Dock = DockStyle.Fill };
            create = new SubFeatures.FlightCreateControl { Dock = DockStyle.Fill };
            detail = new SubFeatures.FlightDetailControl { Dock = DockStyle.Fill };

            // Event handlers
            detail.CloseRequested += (_, __) => SwitchTab(0);
            list.ViewRequested += OnListViewRequested;
            list.RequestEdit += OnListEditRequested;
            list.DataChanged += () => list.RefreshList();
            create.FlightSaved += (_, __) => { list.RefreshList(); SwitchTab(0); };
            create.FlightSavedUpdated += (_, __) => { list.RefreshList(); SwitchTab(0); };

            Controls.Add(list);
            Controls.Add(create);
            Controls.Add(detail);
            Controls.Add(top);

            SwitchTab(0);
        }

        private void OnListViewRequested(DTO.Flight.FlightWithDetailsDTO dto)
        {
            // Show detail tab and load data
            SwitchTabDetail(dto);
        }

        private void OnListEditRequested(DTO.Flight.FlightWithDetailsDTO dto)
        {
            // Switch to create tab and load for edit
            create.LoadForEdit(dto);
            SwitchTab(1);
        }

        private void SwitchTabDetail(DTO.Flight.FlightWithDetailsDTO dto)
        {
            list.Visible = false;
            create.Visible = false;
            detail.Visible = true;

            // Load DTO into detail control
            detail.LoadFlight(dto);
        }

        private void SwitchTab(int idx)
        {
            // Show/hide controls
            list.Visible = (idx == 0);
            create.Visible = (idx == 1);
            detail.Visible = (idx == 2);

            // Ensure top bar stays on top
            var top = btnList.Parent as FlowLayoutPanel;
            if (top != null) top.BringToFront();

            // Update button states
            if (idx == 0)
            {
                btnList.Enabled = false;
                btnCreate.Enabled = true;
            }
            else if (idx == 1)
            {
                btnList.Enabled = true;
                btnCreate.Enabled = false;
            }
            else
            {
                btnList.Enabled = true;
                btnCreate.Enabled = true;
            }
        }
    }
}
