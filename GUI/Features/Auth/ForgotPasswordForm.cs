using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Inputs;

namespace FlightTicketManagement.GUI.Features.Auth {
    public class ForgotPasswordForm : AuthBaseForm {
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

            var btnSend = new PrimaryButton("Gửi mã xác thực", Properties.Resources.login) {
                Width = 240,
                Height = 42,
                Location = new Point(tfEmail.Left, rowLink.Bottom + 6)
            };
            CenterX(btnSend);
            content.Controls.Add(btnSend);
        }
    }
}
