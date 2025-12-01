using BUS.Flight;
using DTO.Flight;
using GUI.Components.Buttons;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Flight.SubFeatures
{
    public partial class FlightDetailControl : UserControl
    {
        public event Action? OnBackToListRequested;
        public event Action<int>? OnEditRequested;

        private int _currentFlightId;

        // Designer controls
        private Label lblFlightNumber = null!;
        private Label lblAircraftId = null!;
        private Label lblRouteId = null!;
        private Label lblDepartureTime = null!;
        private Label lblArrivalTime = null!;
        private Label lblStatus = null!;
        private Label lblDuration = null!;
        private Button btnBack = null!;
        private Button btnEdit = null!;

        public FlightDetailControl()
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            this.SuspendLayout();
            
            lblFlightNumber = new Label { Location = new Point(20, 20), AutoSize = true };
            lblAircraftId = new Label { Location = new Point(20, 50), AutoSize = true };
            lblRouteId = new Label { Location = new Point(20, 80), AutoSize = true };
            lblDepartureTime = new Label { Location = new Point(20, 110), AutoSize = true };
            lblArrivalTime = new Label { Location = new Point(20, 140), AutoSize = true };
            lblStatus = new Label { Location = new Point(20, 170), AutoSize = true };
            lblDuration = new Label { Location = new Point(20, 200), AutoSize = true };
            
            btnBack = new Button { Text = "Back", Location = new Point(20, 250), Size = new Size(100, 30) };
            btnBack.Click += btnBack_Click;
            
            btnEdit = new Button { Text = "Edit", Location = new Point(130, 250), Size = new Size(100, 30) };
            btnEdit.Click += btnEdit_Click;
            
            this.Controls.AddRange(new Control[] { 
                lblFlightNumber, lblAircraftId, lblRouteId, lblDepartureTime, 
                lblArrivalTime, lblStatus, lblDuration, btnBack, btnEdit 
            });
            
            this.Load += FlightDetailControl_Load;
            this.ResumeLayout(false);
        }

        private void FlightDetailControl_Load(object? sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

        public void LoadFlightDetails(int flightId)
        {
            _currentFlightId = flightId;

            var result = FlightBUS.Instance.GetFlightById(flightId);

            if (!result.Success)
            {
                MessageBox.Show(result.GetFullErrorMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var flight = result.GetData<FlightDTO>();

            if (flight == null)
            {
                MessageBox.Show("Flight not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblFlightNumber.Text = flight.FlightNumber;
            lblAircraftId.Text = flight.AircraftId.ToString();
            lblRouteId.Text = flight.RouteId.ToString();
            lblDepartureTime.Text = flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
            lblArrivalTime.Text = flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
            lblStatus.Text = flight.Status.ToString();

            if (flight.DepartureTime.HasValue && flight.ArrivalTime.HasValue)
            {
                var duration = flight.ArrivalTime.Value - flight.DepartureTime.Value;
                lblDuration.Text = $"{duration.Hours}h {duration.Minutes}m";
            }
            else
            {
                lblDuration.Text = "N/A";
            }
        }

        private void btnBack_Click(object? sender, EventArgs e)
        {
            OnBackToListRequested?.Invoke();
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            OnEditRequested?.Invoke(_currentFlightId);
        }
    }
}
