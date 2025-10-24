namespace GUI.Features.Account {
    public class AccountControl : UserControl {
        public AccountControl() {
            InitializeControl();
        }

        private void InitializeControl() {
            Label lbl = new Label {
                Text = "Bạn đã nhấp vào Tài khoản",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 20)
            };
            this.Controls.Add(lbl);
        }
    }
}
