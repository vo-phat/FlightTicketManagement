using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Features.Auth;
using GUI.MainApp;
using GUI.Properties;

namespace GUI.Features.Auth {
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
            tfUser.Top = (title?.Bottom ?? 0) + 24;
            content.Controls.Add(tfUser);

            // Password
            var tfPassword = new UnderlinedTextField(labelText: "Mật khẩu", placeholder: "") {
                Width = 360,
                Location = new Point(tfUser.Left, tfUser.Bottom + 0)
            };
            tfPassword.UseSystemPasswordChar = true;
            tfPassword.PasswordChar = '•';
            content.Controls.Add(tfPassword);

            // Hàng link (căn phải so với tfPass)
            var rowLinks = CreateRightAlignedLinkRow(
                tfPassword,
                "Quên mật khẩu",
                (_, __) => Navigate(new ForgotPasswordForm())
            );
            rowLinks.Top = tfPassword.Bottom + 16;
            content.Controls.Add(rowLinks);

            // Button
            var btnLogin = new PrimaryButton("Đăng nhập", Resources.login) {
                Width = 210,
                Height = 42,
                Location = new Point(tfPassword.Left, rowLinks.Bottom + 14)
            };
            CenterX(btnLogin);
            btnLogin.Click += (s, e) => {
                string email = tfUser.Text.Trim();
                string password = tfPassword.Text.Trim();

                if (email == "admin" && password == "12345") {
                    var mainForm = new MainForm();
                    mainForm.StartPosition = FormStartPosition.CenterScreen;
                    mainForm.Show();

                    // Ẩn hoặc đóng form đăng nhập
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                } else {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", "Đăng nhập thất bại",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            content.Controls.Add(btnLogin);
            this.AcceptButton = btnLogin;

            // Link “Đăng ký” (căn phải theo nút cho gọn – hoặc theo tfPass nếu bạn muốn)
            var rowToRegister = CreateRightAlignedLinkRow(
                btnLogin,
                "Bạn chưa có tài khoản? Đăng ký ngay",
                (_, __) => Navigate(new RegisterForm())
            );
            rowToRegister.Top = btnLogin.Bottom + 16; // khoảng cách giữa button và link cuối
            content.Controls.Add(rowToRegister);
        }

    }
}
