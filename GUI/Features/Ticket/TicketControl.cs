namespace FlightTicketManagement.GUI.Features.Ticket {
    public class TicketControl   : UserControl {
        public TicketControl() {
            InitializeControl();
        }

        private void InitializeControl() {
            Label lbl = new Label {
                Text = "Bạn đã nhấp vào Vé máy bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 20)
            };
            this.Controls.Add(lbl);
        }
    }
}
