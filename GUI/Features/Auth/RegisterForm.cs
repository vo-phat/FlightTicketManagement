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
            var tfPass = new UnderlinedTextField(labelText: "Mật khẩu", placeholder: "") {
                Width = 360,
                Location = new Point(tfUser.Left, tfUser.Bottom + 18)
            };
            tfPass.UseSystemPasswordChar = true;
            tfPass.PasswordChar = '•';

            content.Controls.Add(tfPass);

            // Link căn phải: “Đăng nhập”
            var rowLinks = CreateRightAlignedLinkRow(
                tfPass,
                "Quay lại Đăng nhập",
                (_, __) => Navigate(new LoginForm())
            );
            content.Controls.Add(rowLinks);

            // Button đăng ký
            var btnRegister = new PrimaryButton("Đăng ký", Properties.Resources.login) {
                Width = 210,
                Height = 42,
                Location = new Point(tfPass.Left, rowLinks.Bottom + 18)
            };
            CenterX(btnRegister);
            content.Controls.Add(btnRegister);
        }
    }
}
