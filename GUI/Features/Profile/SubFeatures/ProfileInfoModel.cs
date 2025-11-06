using BUS.Profile;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Features.Auth;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Profile.SubFeatures {
    public class ProfileInfoModel {
        public string Email { get; set; } = "";
        public string FullName { get; set; } = "";
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string PassportNumber { get; set; } = "";
        public string Nationality { get; set; } = "";
        public string RoleDisplay { get; set; } = ""; // tùy chọn, đọc từ User_Role
    }

    public class ProfileInfoControl : UserControl {
        public event Action<ProfileInfoModel>? OnSaveRequested;

        private Label lblTitle;
        private UnderlinedTextField txtEmail, txtFullName, txtPhone, txtPassport, txtNationality;
        private DateTimePickerCustom dtpDob;
        private Label lblRole;


        public ProfileInfoControl() {
            InitializeComponent();
            LoadData(); // nạp dữ liệu ban đầu
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

            txtEmail = new UnderlinedTextField("Email", "") { MinimumSize = new Size(0, 56), Width = 320 };
            txtFullName = new UnderlinedTextField("Họ và tên", "") { MinimumSize = new Size(0, 56), Width = 320 };
            dtpDob = new DateTimePickerCustom("Ngày sinh", "") { Width = 320, Height = 72 };
            txtPhone = new UnderlinedTextField("Số điện thoại", "") { MinimumSize = new Size(0, 56), Width = 320 };
            txtPassport = new UnderlinedTextField("Hộ chiếu", "") { MinimumSize = new Size(0, 56), Width = 320 };
            txtNationality = new UnderlinedTextField("Quốc tịch", "") { MinimumSize = new Size(0, 56), Width = 320 };

            // Hàng 1
            grid.Controls.Add(txtEmail, 0, 0);
            grid.Controls.Add(txtFullName, 1, 0);
            // Hàng 2
            grid.Controls.Add(dtpDob, 0, 1);
            grid.Controls.Add(txtPhone, 1, 1);
            // Hàng 3
            grid.Controls.Add(txtPassport, 0, 2);
            grid.Controls.Add(txtNationality, 1, 2);

            // Vai trò hiển thị
            lblRole = new Label {
                Text = "Vai trò: ",
                AutoSize = true,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Margin = new Padding(24, 8, 24, 0)
            };

            var btnSave = new PrimaryButton("💾 Lưu thay đổi") { Width = 160, Height = 36, Margin = new Padding(0, 0, 0, 0) };
            btnSave.Click += (s, e) =>
            {
                try
                {
                    int accId = SessionManager.AccountId;
                    var model = new ProfileInfoModel
                    {
                        Email = txtEmail.Text,
                        FullName = txtFullName.Text,
                        DateOfBirth = dtpDob.Value,
                        PhoneNumber = txtPhone.Text,
                        PassportNumber = txtPassport.Text,
                        Nationality = txtNationality.Text,
                        RoleDisplay = lblRole.Text
                    };

                    var dto = new DTO.Profile.ProfileDTO
                    {
                        AccountId = accId,
                        FullName = model.FullName,
                        DateOfBirth = model.DateOfBirth,
                        PhoneNumber = model.PhoneNumber,
                        PassportNumber = model.PassportNumber,
                        Nationality = model.Nationality
                    };

                    var bus = new ProfileBUS();
                    bool ok = bus.UpdateProfile(dto);

                    if (ok)
                        MessageBox.Show("✅ Đã cập nhật thông tin cá nhân thành công!",
                                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Không có thay đổi nào được lưu.",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu thông tin: " + ex.Message,
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            main.Controls.Add(lblRole, 0, 2);
            main.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 8 }, 0, 3);
            Controls.Add(main);
            Controls.Add(btnRow);
        }

        private void LoadData()
        {
            try
            {
                int accId = SessionManager.AccountId;
                var bus = new ProfileBUS();
                var profile = bus.GetProfileByAccountId(accId);

                if (profile != null)
                {
                    txtEmail.Text = profile.Email;
                    txtFullName.Text = profile.FullName;
                    dtpDob.Value = profile.DateOfBirth ?? DateTime.Today;
                    txtPhone.Text = profile.PhoneNumber;
                    txtPassport.Text = profile.PassportNumber;
                    txtNationality.Text = profile.Nationality;
                    lblRole.Text = $"Vai trò: {profile.RoleName ?? "Không xác định"}";
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin hồ sơ: " + ex.Message,
                    "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
