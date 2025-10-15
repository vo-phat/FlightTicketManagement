namespace FlightTicketManagement.GUI.Features.Stats {
    public class StatsControl : UserControl {
        public StatsControl() {
            InitializeControl();
        }

        private void InitializeControl() {
            Label lbl = new Label {
                Text = "Bạn đã nhấp vào Thống kê",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 20)
            };
            this.Controls.Add(lbl);
        }
    }
}
