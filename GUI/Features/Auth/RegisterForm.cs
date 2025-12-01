using BUS.Auth;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Features.Auth;
using GUI.Properties;

namespace GUI.Features.Auth {
    public class RegisterForm : AuthBaseForm {
        private readonly AuthService _authService = new AuthService();
        public RegisterForm() : base("Đăng ký") {
            BuildUI();
        }

        private void BuildUI() {
            var tfUser = new UnderlinedTextField(labelText: "Tài khoản", placeholder: "") {
                Width = 360,
                Location = new Point(0, 0)
            };
            CenterX(tfUser);
            tfUser.Top = title.Bottom + 12;
            content.Controls.Add(tfUser);

            var tfPassword = new UnderlinedTextField(labelText: "Mật khẩu", placeholder: "") {
                Width = 360,
                Location = new Point(tfUser.Left, tfUser.Bottom + 18)
            };
            tfPassword.UseSystemPasswordChar = true;
            tfPassword.PasswordChar = '•';

            content.Controls.Add(tfPassword);

            var rowLinks = CreateRightAlignedLinkRow(
                tfPassword,
                "Quay lại Đăng nhập",
                (_, __) => Navigate(new LoginForm())
            );
            content.Controls.Add(rowLinks);

            var btnRegister = new PrimaryButton("Đăng ký", Resources.login) {
                Width = 210,
                Height = 42,
                Location = new Point(tfPassword.Left, rowLinks.Bottom + 18)
            };
            CenterX(btnRegister);

            btnRegister.Click += (s, e) => {
                try {
                    var email = tfUser.Text.Trim();
                    var password = tfPassword.Text.Trim();

                    var account = _authService.Register(email, password);

                    MessageBox.Show("Đăng ký thành công! Hãy đăng nhập.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Navigate(new LoginForm());
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Đăng ký thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            content.Controls.Add(btnRegister);
            this.AcceptButton = btnRegister;
        }
    }
}
