using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Inputs;

namespace FlightTicketManagement.GUI.Features.Auth {
    public class RegisterForm : AuthBaseForm {
        public RegisterForm() : base("Đăng ký") {
            BuildUI();
        }

        private void BuildUI() {
            // Email / username
            var tfUser = new UnderlinedTextField(labelText: "Tài khoản", placeholder: "") {
                Width = 360,
                Location = new Point(0, 0)
            };
            CenterX(tfUser);
            tfUser.Top = title.Bottom + 12;
            content.Controls.Add(tfUser);

            // Password
            var tfPassword = new UnderlinedTextField(labelText: "Mật khẩu", placeholder: "") {
                Width = 360,
                Location = new Point(tfUser.Left, tfUser.Bottom + 18)
            };
            tfPassword.UseSystemPasswordChar = true;
            tfPassword.PasswordChar = '•';

            content.Controls.Add(tfPassword);

            // Link căn phải: “Đăng nhập”
            var rowLinks = CreateRightAlignedLinkRow(
                tfPassword,
                "Quay lại Đăng nhập",
                (_, __) => Navigate(new LoginForm())
            );
            content.Controls.Add(rowLinks);

            // Button đăng ký
            var btnRegister = new PrimaryButton("Đăng ký", Properties.Resources.login) {
                Width = 210,
                Height = 42,
                Location = new Point(tfPassword.Left, rowLinks.Bottom + 18)
            };
            CenterX(btnRegister);
            content.Controls.Add(btnRegister);
            this.AcceptButton = btnRegister;
        }
    }
}
