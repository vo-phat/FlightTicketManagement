namespace FlightTicketManagement.GUI.Features.Location {
    public class LocationControl : UserControl {
        public LocationControl() {
            InitializeControl();
        }

        private void InitializeControl() {
            Label lbl = new Label {
                Text = "Bạn đã nhấp vào Vị trí",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 20)
            };
            this.Controls.Add(lbl);
        }
    }
}
