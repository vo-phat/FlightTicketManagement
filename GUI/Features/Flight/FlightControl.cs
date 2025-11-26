using GUI.Components.Buttons;
using GUI.Features.Flight.SubFeatures;
using GUI.MainApp;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Flight
{
    public partial class FlightControl : UserControl
    {
        private FlightCreateControl flightCreateControl;
        private FlightDetailControl flightDetailControl;
        private FlightListControl flightListControl;

        private int _currentIndex = 0;
        private readonly AppRole _role;
        public event Action<int> OnBookFlightRequested;
        public FlightControl() : this(AppRole.Admin)
        {
        }
        public FlightControl(AppRole role)
        {
            _role = role;
            InitializeComponent();
                
            flightCreateControl = new FlightCreateControl { Dock = DockStyle.Fill };
            flightDetailControl = new FlightDetailControl { Dock = DockStyle.Fill };
            flightListControl = new FlightListControl(_role) { Dock = DockStyle.Fill };

            flightListControl.OnBookFlightRequested += (flightId) => {
                OnBookFlightRequested?.Invoke(flightId);
            };
            flightListControl.OnViewFlightDetailRequested += HandleViewFlightDetail;
            flightDetailControl.OnBackToListRequested += () => SwitchTab(0);
            flightDetailControl.OnEditRequested += HandleEditFlight;

            flightListControl.OnEditFlightRequested += HandleEditFlight;

            flightCreateControl.OnSaveSuccess += HandleSaveSuccess;

            panelContent.Controls.Add(panelFlightList);
            panelContent.Controls.Add(panelFlightCreate);
            panelContent.Controls.Add(panelFlightDetail);

            panelFlightCreate.Controls.Add(flightCreateControl);
            panelFlightDetail.Controls.Add(flightDetailControl);
            panelFlightList.Controls.Add(flightListControl);

            panelFlightCreate.Visible = false;
            panelFlightDetail.Visible = false;
        }
        private void HandleEditFlight(int flightId)
        {
            if (flightCreateControl != null)
            {
                flightCreateControl.LoadFlightForEdit(flightId);
                SwitchTab(1);
            }
        }
        private void HandleViewFlightDetail(int flightId)
        {
            if (flightDetailControl != null)
            {
                flightDetailControl.LoadFlightDetails(flightId);
                SwitchTab(2);
            }
        }
        private void FlightControl_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;

            taoMoiChuyenBay.Visible = (_role == AppRole.Admin);

            SwitchTab(0);
        }

        private void buttonDanhSachChuyenBay_Click(object sender, EventArgs e)
        {
            SwitchTab(0);
        }

        private void buttonTaoMoiChuyenBay_Click(object sender, EventArgs e)
        {
            SwitchTab(1);
        }
        void SwitchTab(int index)
        {
            if (_currentIndex == index && panelTabs.Controls.Count > 0)
                return;

            _currentIndex = index;

            panelFlightCreate.Visible = false;
            panelFlightList.Visible = false;
            panelFlightDetail.Visible = false;
            switch (index)
            {
                case 0:
                    panelFlightList.Visible = true;
                    break;
                case 1:
                    panelFlightCreate.Visible = true;
                    break;
                case 2:
                    panelFlightDetail.Visible = true;
                    break;
            }
            panelTabs.BringToFront();
        }

        private void danhSachChuyenBay_Click(object sender, EventArgs e)
        {
            if (flightListControl != null)
            {
                flightListControl.LoadFlightData();
            }

            SwitchTab(0);
        }
        private void taoMoiChuyenBay_Click(object sender, EventArgs e)
        {
            if (flightCreateControl != null)
            {
                flightCreateControl.ClearForm();
            }

            SwitchTab(1);
        }
        private void HandleSaveSuccess()
        {
            if (flightListControl != null)
            {
                flightListControl.LoadFlightData();
            }

            SwitchTab(0);
        }
    }
}