using GUI.Components.Buttons;
using GUI.Features.Flight.SubFeatures;

namespace GUI.Features.Flight
{
    public class FlightControl : UserControl
    {
        private PrimaryButton btnList;
        private SecondaryButton btnCreate;
        private FlowLayoutPanel buttonPanel;

        private FlightListControl listControl;
        private FlightDetailControl detailControl;
        private FlightCreateControl createControl;

        private int currentTab = 0;

        public FlightControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.WhiteSmoke;

            // ===== Button Panel =====
            buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true,
                WrapContents = false
            };

            btnList = new PrimaryButton("📋 Danh sách chuyến bay");
            btnCreate = new SecondaryButton("➕ Tạo chuyến bay mới");

            btnList.Click += (s, e) => SwitchTab(0);
            btnCreate.Click += (s, e) => SwitchTab(2);

            buttonPanel.Controls.Add(btnList);
            buttonPanel.Controls.Add(btnCreate);

            // ===== Sub Controls =====
            listControl = new FlightListControl { Dock = DockStyle.Fill, Visible = false };
            detailControl = new FlightDetailControl { Dock = DockStyle.Fill, Visible = false };
            createControl = new FlightCreateControl { Dock = DockStyle.Fill, Visible = false };

            listControl.OnViewDetail += (flightId) => ShowFlightDetail(flightId);
            listControl.OnEditFlight += (flightId) => ShowFlightEdit(flightId);

            detailControl.OnEditRequested += (flightId) => ShowFlightEdit(flightId);
            detailControl.OnClosed += () => { SwitchTab(0); listControl.RefreshData(); };

            createControl.OnFlightCreated += () => { SwitchTab(0); listControl.RefreshData(); };
            // ===== Add to container =====
            this.Controls.Add(listControl);
            this.Controls.Add(detailControl);
            this.Controls.Add(createControl);
            this.Controls.Add(buttonPanel);

            // Show list by default
            SwitchTab(0);
        }
        private void ShowFlightEdit(int flightId)
        {
            // TODO: Implement edit mode
            MessageBox.Show($"Edit flight {flightId} - Coming soon!");
        }
        private void SwitchTab(int tabIndex)
        {
            if (currentTab == tabIndex) return;
            currentTab = tabIndex;

            // Hide all
            listControl.Visible = false;
            detailControl.Visible = false;
            createControl.Visible = false;

            // Show selected
            switch (tabIndex)
            {
                case 0: // List
                    listControl.Visible = true;
                    UpdateButtonStyles(isListActive: true);
                    break;

                case 1: // Detail
                    detailControl.Visible = true;
                    UpdateButtonStyles(isListActive: false);
                    break;

                case 2: // Create
                    createControl.Visible = true;
                    UpdateButtonStyles(isListActive: false);
                    break;
            }
        }

        private void UpdateButtonStyles(bool isListActive)
        {
            if (isListActive)
            {
                // List active
                btnList.NormalBackColor = Color.FromArgb(155, 209, 243);
                btnList.NormalForeColor = Color.White;
                btnList.Font = new Font("Segoe UI", 12f, FontStyle.Bold);

                btnCreate.NormalBackColor = Color.White;
                btnCreate.NormalForeColor = Color.FromArgb(155, 209, 243);
                btnCreate.Font = new Font("Segoe UI", 12f, FontStyle.Regular);
            }
            else
            {
                // Create active
                btnList.NormalBackColor = Color.White;
                btnList.NormalForeColor = Color.FromArgb(155, 209, 243);
                btnList.Font = new Font("Segoe UI", 12f, FontStyle.Regular);

                btnCreate.NormalBackColor = Color.FromArgb(155, 209, 243);
                btnCreate.NormalForeColor = Color.White;
                btnCreate.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            }

            btnList.Invalidate();
            btnCreate.Invalidate();
        }

        private void ShowFlightDetail(int flightId)
        {
            detailControl.LoadFlight(flightId);
            SwitchTab(1);
        }

        public void RefreshFlightList()
        {
            listControl.RefreshData();
        }
    }
}