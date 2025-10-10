using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Inputs;

namespace FlightTicketManagement.GUI.Features.Auth {
    public class LoginForm : AuthBaseForm {
        public LoginForm() : base("Đăng nhập") {
            BuildUI();
        }

        private void BuildUI() {
            // User
            var tfUser = new UnderlinedTextField(labelText: "Tài khoản", placeholder: "") {
                Width = 360,
                Location = new Point(0, 0)
            };
            CenterX(tfUser);
            tfUser.Top = (title?.Bottom ?? 0) + 24; // 🔧 khoảng cách từ title xuống field đầu tiên
            content.Controls.Add(tfUser);

            // Password
            var tfPass = new UnderlinedTextField(labelText: "Mật khẩu", placeholder: "") {
                Width = 360,
                Location = new Point(tfUser.Left, tfUser.Bottom + 0) // 🔧 khoảng cách giữa 2 textfield
            };
            tfPass.UseSystemPasswordChar = true;
            tfPass.PasswordChar = '•';
            content.Controls.Add(tfPass);

            // Hàng link (căn phải so với tfPass)
            var rowLinks = CreateRightAlignedLinkRow(
                tfPass,
                "Quên mật khẩu",
                (_, __) => Navigate(new ForgotPasswordForm())
            );
            rowLinks.Top = tfPass.Bottom + 16; // 🔧 khoảng cách từ textfield xuống link
            content.Controls.Add(rowLinks);

            // Button
            var btnLogin = new PrimaryButton("Đăng nhập", Properties.Resources.login) {
                Width = 210,
                Height = 42,
                Location = new Point(tfPass.Left, rowLinks.Bottom + 14) // 🔧 khoảng cách từ link tới button
            };
            CenterX(btnLogin);
            content.Controls.Add(btnLogin);

            // Link “Đăng ký” (căn phải theo nút cho gọn – hoặc theo tfPass nếu bạn muốn)
            var rowToRegister = CreateRightAlignedLinkRow(
                btnLogin,
                "Bạn chưa có tài khoản? Đăng ký ngay",
                (_, __) => Navigate(new RegisterForm())
            );
            rowToRegister.Top = btnLogin.Bottom + 16; // 🔧 khoảng cách giữa button và link cuối
            content.Controls.Add(rowToRegister);
        }

    }
}
