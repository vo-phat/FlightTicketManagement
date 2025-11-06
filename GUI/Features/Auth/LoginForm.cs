using BUS.Account;
using DTO.Account;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.MainApp;
using GUI.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Auth
{
    public class LoginForm : AuthBaseForm
    {
        private UnderlinedTextField tfUser;
        private UnderlinedTextField tfPassword;
        private PrimaryButton btnLogin;
        private readonly AccountBUS bus = new AccountBUS();

        public LoginForm() : base("Đăng nhập")
        {
            BuildUI();
        }

        private void BuildUI()
        {
            // ===== Email / Username =====
            tfUser = new UnderlinedTextField(labelText: "Tài khoản (Email)", placeholder: "")
            {
                Width = 360,
                Location = new Point(0, 0)
            };
            CenterX(tfUser);
            tfUser.Top = (title?.Bottom ?? 0) + 24;
            content.Controls.Add(tfUser);

            // ===== Password =====
            tfPassword = new UnderlinedTextField(labelText: "Mật khẩu", placeholder: "")
            {
                Width = 360,
                Location = new Point(tfUser.Left, tfUser.Bottom + 0)
            };
            tfPassword.UseSystemPasswordChar = true;
            tfPassword.PasswordChar = '•';
            content.Controls.Add(tfPassword);

            // ===== Link “Quên mật khẩu” =====
            var rowLinks = CreateRightAlignedLinkRow(
                tfPassword,
                "Quên mật khẩu",
                (_, __) => Navigate(new ForgotPasswordForm())
            );
            rowLinks.Top = tfPassword.Bottom + 16;
            content.Controls.Add(rowLinks);

            // ===== Button “Đăng nhập” =====
            btnLogin = new PrimaryButton("Đăng nhập", Resources.login)
            {
                Width = 210,
                Height = 42,
                Location = new Point(tfPassword.Left, rowLinks.Bottom + 14)
            };
            CenterX(btnLogin);
            content.Controls.Add(btnLogin);

            btnLogin.Click += BtnLogin_Click;
            this.AcceptButton = btnLogin;

            // ===== Link “Đăng ký” =====
            var rowToRegister = CreateRightAlignedLinkRow(
                btnLogin,
                "Bạn chưa có tài khoản? Đăng ký ngay",
                (_, __) => Navigate(new RegisterForm())
            );
            rowToRegister.Top = btnLogin.Bottom + 16;
            content.Controls.Add(rowToRegister);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = tfUser.Text.Trim();
                string password = tfPassword.Text.Trim();

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Vui lòng nhập email và mật khẩu.",
                        "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var acc = bus.Authenticate(email, password);

                if (acc != null)
                {
                    // ✅ Lưu thông tin người dùng đang đăng nhập
                    // ✅ Lưu cả role
                    SessionManager.SetCurrentUser(acc.AccountId, acc.Email, acc.RoleName);

                    MessageBox.Show("Đăng nhập thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var mainForm = new MainForm();
                    mainForm.StartPosition = FormStartPosition.CenterScreen;
                    Hide(); // Ẩn form đăng nhập
                    mainForm.ShowDialog(); // Mở mainForm dạng modal
                    Show(); // Quay lại login sau khi mainForm đóng (nếu muốn)
                    tfUser.Text = "";
                    tfPassword.Text = "";
                }
                else
                {
                    MessageBox.Show("Sai email hoặc mật khẩu!",
                        "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Email hoặc mật khẩu không chính xác.",
                    "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message,
                    "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
