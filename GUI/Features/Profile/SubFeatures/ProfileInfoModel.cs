using BUS.Profile;
using DTO.Profile;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using BUS.Auth;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Profile.SubFeatures {
    public class ProfileInfoControl : UserControl {
        public event Action<ProfileInfoDto>? OnSaveRequested;

        private Label lblTitle;
        private UnderlinedTextField txtEmail, txtFullName, txtPhone, txtPassport, txtNationality;
        private DateTimePickerCustom dtpDob;

        private readonly int _accountId;
        private readonly ProfileService _profileService = new ProfileService();

        public ProfileInfoControl(int accountId) {
            _accountId = accountId;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            lblTitle = new Label {
                Text = "🙍 Thông tin cá nhân",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            var grid = new TableLayoutPanel {
                Dock = DockStyle.Top,
                Padding = new Padding(24, 12, 24, 0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 4
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            txtEmail = new UnderlinedTextField("Email", "") { 
                MinimumSize = new Size(0, 56), 
                Width = 320,
                ReadOnly = true
            };
            txtFullName = new UnderlinedTextField("Họ và tên", "") { 
                MinimumSize = new Size(0, 56), 
                Width = 320 
            };
            dtpDob = new DateTimePickerCustom("Ngày sinh", "") { 
                Width = 320, 
                Height = 72 
            };
            txtPhone = new UnderlinedTextField("Số điện thoại", "") { 
                MinimumSize = new Size(0, 56), 
                Width = 320 
            };
            txtPassport = new UnderlinedTextField("Hộ chiếu", "") { 
                MinimumSize = new Size(0, 56), 
                Width = 320 
            };
            txtNationality = new UnderlinedTextField("Quốc tịch", "") { 
                MinimumSize = new Size(0, 56), 
                Width = 320 
            };

            // Hàng 1
            grid.Controls.Add(txtEmail, 0, 0);
            grid.Controls.Add(txtFullName, 1, 0);
            // Hàng 2
            grid.Controls.Add(dtpDob, 0, 1);
            grid.Controls.Add(txtPhone, 1, 1);
            // Hàng 3
            grid.Controls.Add(txtPassport, 0, 2);
            grid.Controls.Add(txtNationality, 1, 2);

            var btnSave = new PrimaryButton("💾 Lưu thay đổi") { Width = 160, Height = 36, Margin = new Padding(0, 0, 0, 0) };
            btnSave.Click += (s, e) => {
                var model = new ProfileInfoDto {
                    Email = txtEmail.Text,
                    FullName = txtFullName.Text,
                    DateOfBirth = dtpDob.Value,
                    PhoneNumber = txtPhone.Text,
                    PassportNumber = txtPassport.Text,
                    Nationality = txtNationality.Text
                };
                OnSaveRequested?.Invoke(model);
            };

            var btnRow = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(24, 8, 24, 0)
            };
            btnRow.Controls.Add(btnSave);

            var main = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(grid, 0, 1);
            main.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 8 }, 0, 3);
            Controls.Add(main);
            Controls.Add(btnRow);
        }

        private void LoadData() {
            try {
                var profile = _profileService.GetProfile(_accountId);
                if (profile == null) {
                    return;
                }

                txtEmail.Text = profile.Email ?? string.Empty;
                txtFullName.Text = profile.FullName ?? string.Empty;
                txtPhone.Text = profile.PhoneNumber ?? string.Empty;
                txtPassport.Text = profile.PassportNumber ?? string.Empty;
                txtNationality.Text = profile.Nationality ?? string.Empty;

                if (profile.DateOfBirth.HasValue) {
                    dtpDob.Value = profile.DateOfBirth.Value;
                } else {
                }
            } catch (Exception ex) {
                MessageBox.Show(
                    "Không thể tải thông tin hồ sơ: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
