namespace FlightTicketManagement.GUI.Features.Settings {
    public class SettingsControl : UserControl {
        public SettingsControl() {
            InitializeControl();
        }

        private void InitializeControl() {
            Label lbl = new Label {
                Text = "Bạn đã nhấp vào Cài đặt",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 20)
            };
            this.Controls.Add(lbl);
        }
    }
}
