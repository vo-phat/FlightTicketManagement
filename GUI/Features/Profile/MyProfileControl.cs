using GUI.Components.Buttons;
using GUI.Features.Auth;
using GUI.Features.Profile.SubFeatures;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Profile {
    public class MyProfileControl : UserControl {
        private Button btnInfo;
        private Button btnChangePassword;
        private Button btnLogout;

        private ProfileInfoControl infoControl;
        private ChangePasswordControl changePwdControl;

        private readonly int _accountId;

        public MyProfileControl(int accountId) {
            _accountId = accountId;
            InitializeComponent();
        }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            // Thanh nút trên
            btnInfo = new PrimaryButton("Thông tin cá nhân");
            btnChangePassword = new SecondaryButton("Đổi mật khẩu");
            btnLogout = new SecondaryButton("Đăng xuất");

            btnInfo.Click += (s, e) => SwitchTab(0);
            btnChangePassword.Click += (s, e) => SwitchTab(1);
            btnLogout.Click += (s, e) => SwitchTab(2);

            var buttonPanel = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            buttonPanel.Controls.Add(btnInfo);
            buttonPanel.Controls.Add(btnChangePassword);
            buttonPanel.Controls.Add(btnLogout);

            // Ba control con (giống FlightControl có list/detail/create)
            infoControl = new ProfileInfoControl(_accountId) { Dock = DockStyle.Fill };
            changePwdControl = new ChangePasswordControl(_accountId) { Dock = DockStyle.Fill };

            // Sự kiện hành động
            infoControl.OnSaveRequested += HandleSaveProfileAsync;
            changePwdControl.OnChangePasswordRequested += HandleChangePasswordAsync;

            Controls.Add(infoControl);
            Controls.Add(changePwdControl);
            Controls.Add(buttonPanel);

            SwitchTab(0);
        }

        private void SwitchTab(int idx) {
            infoControl.Visible = (idx == 0);
            changePwdControl.Visible = (idx == 1);

            // Đổi kiểu nút theo tab đã chọn (pattern của FlightControl)
            var wrap = btnInfo.Parent as FlowLayoutPanel;
            if (wrap != null) {
                wrap.Controls.Clear();
                btnInfo = idx == 0 ? new PrimaryButton("Thông tin cá nhân") : new SecondaryButton("Thông tin cá nhân");
                btnChangePassword = idx == 1 ? new PrimaryButton("Đổi mật khẩu") : new SecondaryButton("Đổi mật khẩu");
                btnLogout = idx == 2 ? new PrimaryButton("Đăng xuất") : new SecondaryButton("Đăng xuất");

                btnInfo.Click += (s, e) => SwitchTab(0);
                btnChangePassword.Click += (s, e) => SwitchTab(1);
                btnLogout.Click += (s, e) => DoLogout();

                wrap.Controls.AddRange(new Control[] { btnInfo, btnChangePassword, btnLogout });
            }
        }

        // TODO: thay bằng gọi Service/Repository thật (EF/Dapper) của bạn
        private async void HandleSaveProfileAsync(ProfileInfoModel model) {
            try {
                // Gợi ý: cập nhật Accounts.email và Passenger_Profiles.* theo account_id
                // Accounts(email), Passenger_Profiles(full_name, date_of_birth, phone_number, passport_number, nationality)
                // … gọi DB và thông báo
                await System.Threading.Tasks.Task.Delay(200);
                MessageBox.Show("Đã lưu thông tin cá nhân.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void HandleChangePasswordAsync(ChangePasswordModel model) {
            try {
                // Kiểm tra mật khẩu hiện tại + cập nhật Accounts.password (nhớ hash)
                await System.Threading.Tasks.Task.Delay(200);
                MessageBox.Show("Đã đổi mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show("Đổi mật khẩu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DoLogout() {
            // Hiển thị hộp thoại xác nhận
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống không?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes) {
                var current = FindForm();
                try {
                    var login = new LoginForm();
                    login.Show();
                } catch {
                }
                current?.Hide();
            }
        }
    }
}
