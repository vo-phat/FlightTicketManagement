using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Tables;
using FlightTicketManagement.GUI.Components.Inputs;

namespace FlightTicketManagement.GUI.Features.Test {
    public class TestControl : Form {
        public TestControl() {

            // --- Cấu hình form ---
            Text = "Test Custom Primary Button";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1920, 1080);
            BackColor = Color.WhiteSmoke;

            // primary button
            var primaryButton = new PrimaryButton("Primary Button", Properties.Resources.login);
            primaryButton.Location = new Point(200, 150);
            primaryButton.Click += (s, e) => MessageBox.Show("Bạn đã nhấn nút Primary Button!", "Thông báo");

            // secondary button
            var secondaryButton = new SecondaryButton("Secondary Button", Properties.Resources.login);
            secondaryButton.Location = new Point(400, 300);
            secondaryButton.Click += (s, e) => MessageBox.Show("Bạn đã nhấn nút Secondary Button!", "Thông báo");

            // table
            var table = new TableCustom {
                Location = new Point(600,450),
                Size = new Size(760, 360)
            };

            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã chuyến bay", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nơi cất cánh", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nơi hạ cánh", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Giờ bay", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số ghế trống", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            table.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            table.Rows.Add("FT12345", "Hà Nội", "Thành phố Hồ Chí Minh", "00:00", 100, "Thêm, Sửa, Xóa");
            table.Rows.Add("FT12345", "Hà Nội", "Thành phố Hồ Chí Minh", "00:00", 100, "Thêm, Sửa, Xóa");
            table.Rows.Add("FT12345", "Hà Nội", "Thành phố Hồ Chí Minh", "00:00", 100, "Thêm, Sửa, Xóa");
            table.Rows.Add("FT12345", "Hà Nội", "Thành phố Hồ Chí Minh", "00:00", 100, "Thêm, Sửa, Xóa");
            table.Rows.Add("FT12345", "Hà Nội", "Thành phố Hồ Chí Minh", "00:00", 100, "Thêm, Sửa, Xóa");
            table.Rows.Add("FT12345", "Hà Nội", "Thành phố Hồ Chí Minh", "00:00", 100, "Thêm, Sửa, Xóa");

            // textfield
            var textField = new UnderlinedTextField(labelText: "Nơi cất cánh", placeholder: "Thành phố Hà Nội") {
                Location = new Point(0, 0),
                Width = 360
            };

            // --- Thêm vào form ---
            Controls.Add(primaryButton);
            Controls.Add(secondaryButton);
            Controls.Add(table);
            Controls.Add(textField);
        }

        private void InitializeComponent() {
            SuspendLayout();
            // 
            // TestButton
            // 
            ClientSize = new Size(821, 605);
            Name = "TestButton";
            Load += TestButton_Load;
            ResumeLayout(false);

        }

        private void TestButton_Load(object? sender, EventArgs e) {

        }

        private void primaryButton1_Click(object? sender, EventArgs e) {

        }
    }
}
