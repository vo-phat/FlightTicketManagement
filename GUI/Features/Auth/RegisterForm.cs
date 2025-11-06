using BUS.Account;
using DTO.Account;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Auth
{
    public class RegisterForm : AuthBaseForm
    {
        private UnderlinedTextField tfUser;
        private UnderlinedTextField tfPassword;
        private PrimaryButton btnRegister;
        private readonly AccountBUS bus = new AccountBUS();

        public RegisterForm() : base("Đăng ký")
        {
            BuildUI();
        }

        private void BuildUI()
        {
            // ===== Ô nhập Email =====
            tfUser = new UnderlinedTextField(labelText: "Email", placeholder: "")
            {
                Width = 360,
                Location = new Point(0, 0)
            };
            CenterX(tfUser);
            tfUser.Top = title.Bottom + 12;
            content.Controls.Add(tfUser);

            // ===== Ô nhập Password =====
            tfPassword = new UnderlinedTextField(labelText: "Mật khẩu", placeholder: "")
            {
                Width = 360,
                Location = new Point(tfUser.Left, tfUser.Bottom + 18)
            };
            tfPassword.UseSystemPasswordChar = true;
            tfPassword.PasswordChar = '•';
            content.Controls.Add(tfPassword);

            // ===== Link “Quay lại đăng nhập” =====
            var rowLinks = CreateRightAlignedLinkRow(
                tfPassword,
                "Quay lại Đăng nhập",
                (_, __) => Navigate(new LoginForm())
            );
            content.Controls.Add(rowLinks);

            // ===== Nút đăng ký =====
            btnRegister = new PrimaryButton("Đăng ký", Resources.login)
            {
                Width = 210,
                Height = 42,
                Location = new Point(tfPassword.Left, rowLinks.Bottom + 18)
            };
            CenterX(btnRegister);
            content.Controls.Add(btnRegister);

            btnRegister.Click += BtnRegister_Click;
            this.AcceptButton = btnRegister;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string email = tfUser.Text.Trim();
                string password = tfPassword.Text.Trim();

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ email và mật khẩu.", "Thiếu thông tin",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dto = new AccountDTO
                {
                    Email = email,
                    Password = password
                };

                bool success = bus.Insert(dto);

                if (success)
                {
                    MessageBox.Show("Đăng ký tài khoản thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Quay lại trang đăng nhập
                    Navigate(new LoginForm());
                }
                else
                {
                    MessageBox.Show("Không thể đăng ký. Vui lòng thử lại sau.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
