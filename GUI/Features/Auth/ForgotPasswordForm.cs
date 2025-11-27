using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Properties;
using GUI.Features.Auth;
using BUS.Auth;

namespace GUI.Features.Auth {
    public class ForgotPasswordForm : AuthBaseForm {
        private readonly AuthService _authService = new AuthService();
        public ForgotPasswordForm() : base("Quên mật khẩu") {
            BuildUI();
        }

        private void BuildUI() {
            var tfEmail = new UnderlinedTextField(labelText: "Tài khoản", placeholder: "") {
                Width = 360,
                Location = new Point(0, 0)
            };
            CenterX(tfEmail);
            tfEmail.Top = title.Bottom + 12;
            content.Controls.Add(tfEmail);

            // Link căn phải: “Đăng nhập”
            var rowLink = CreateRightAlignedLinkRow(
                tfEmail,
                "Quay lại Đăng nhập",
                (_, __) => Navigate(new LoginForm())
            );
            content.Controls.Add(rowLink);

            var btnSendCode = new PrimaryButton("Gửi mã xác thực", Resources.login) {
                Width = 240,
                Height = 42,
                Location = new Point(tfEmail.Left, rowLink.Bottom + 6)
            };
            CenterX(btnSendCode);
            content.Controls.Add(btnSendCode);

            var tfOtp = new UnderlinedTextField(labelText: "Mã xác thực", placeholder: "Nhập mã 6 số") {
                Width = 360,
                Location = new Point(tfEmail.Left, btnSendCode.Bottom + 18)
            };
            content.Controls.Add(tfOtp);

            // Mật khẩu mới
            var tfNewPassword = new UnderlinedTextField(labelText: "Mật khẩu mới", placeholder: "") {
                Width = 360,
                Location = new Point(tfOtp.Left, tfOtp.Bottom + 18)
            };
            tfNewPassword.UseSystemPasswordChar = true;
            tfNewPassword.PasswordChar = '•';
            content.Controls.Add(tfNewPassword);

            // Nhập lại mật khẩu mới
            var tfConfirmNewPassword = new UnderlinedTextField(labelText: "Nhập lại mật khẩu mới", placeholder: "") {
                Width = 360,
                Location = new Point(tfNewPassword.Left, tfNewPassword.Bottom + 18)
            };
            tfConfirmNewPassword.UseSystemPasswordChar = true;
            tfConfirmNewPassword.PasswordChar = '•';
            content.Controls.Add(tfConfirmNewPassword);

            // Nút xác nhận đổi mật khẩu
            var btnConfirm = new PrimaryButton("Xác nhận đổi mật khẩu", Resources.login) {
                Width = 260,
                Height = 42,
                Location = new Point(tfConfirmNewPassword.Left, tfConfirmNewPassword.Bottom + 18)
            };
            CenterX(btnConfirm);
            content.Controls.Add(btnConfirm);

            // Sự kiện
            btnSendCode.Click += (s, e) => {
                try {
                    var email = tfEmail.Text.Trim();
                    _authService.SendResetPasswordCode(email);
                    MessageBox.Show("Mã xác thực đã được gửi tới email của bạn.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Lỗi gửi mã xác thực",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnConfirm.Click += (s, e) => {
                try {
                    var email = tfEmail.Text.Trim();
                    var otp = tfOtp.Text.Trim();
                    var newPass = tfNewPassword.Text.Trim();
                    var confirmNew = tfConfirmNewPassword.Text.Trim();

                    _authService.ResetPassword(email, otp, newPass, confirmNew);

                    MessageBox.Show("Đổi mật khẩu thành công! Hãy đăng nhập với mật khẩu mới.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Navigate(new LoginForm());
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Đổi mật khẩu thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };


            this.AcceptButton = btnSendCode;
        }
    }
}
