using DTO.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
namespace GUI.Features.Ticket.subTicket
{

    public partial class frmPassengerInfoControl : UserControl
    {
        private List<ProfileDTO> ListBooking;
        public frmPassengerInfoControl()
        {
            InitializeComponent();
            // chức năng này luôn đưuojc thêm tạo mới khi dặt vé
            // laays dduowjc cais list tuwf mays bay thif voo day lamf tieem
            // max ves may bay, 
            //take_data_from_Fright();
            ListBooking = new List<ProfileDTO>();
        }
        private void btnSelectSeatTicket_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chọn ghế sẽ xuất hiện sớm .");
        }

        private void btnAddPassengerTicket_Click(object sender, EventArgs e)
        {
            for (int index = 1; index < 3; index++)
            {
                //MessageBox.Show("Thêm hành khách thành công.", $"Thông báo cho hành khách {index}");
                //MessageBox.Show($"Thông báo cho hành khách {dtpDateOfBirthTicket.Value}");

                ProfileDTO profile = new ProfileDTO
                {
                    FullName = txtFullNameTicket.Text.Trim(),
                    NumberPhone = txtPhoneNumberTicket.Text.Trim(),
                    Email = txtEmailTicket.Text.Trim(),
                    //Nation = cboNationalityTicket.Text,
                    Nationality = "VN",
                    Passport = txtPassportNumberTicket.Text.Trim(),
                    DateOfBirth = dtpDateOfBirthTicket.Value,
                    FlightDate = dtpFlightDateTicket.Value,
                    NumberSeat = "1"
                };
                ListBooking.Add(profile);
            }
            MessageBox.Show($"{ListBooking.Count}");
            SetupPassengerGrid(ListBooking);
        }
        //private List<ProfileDTO> _passengerList = new List<ProfileDTO>();

        // Đổi tên phương thức để rõ nghĩa hơn
        private void SetupPassengerGrid(List<ProfileDTO> _passengerList)
        {

            dgvPassengerListTicket.AutoGenerateColumns = false;
            dgvPassengerListTicket.AllowUserToAddRows = false;
            dgvPassengerListTicket.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPassengerListTicket.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPassengerListTicket.Columns.Clear(); // Xóa các cột cũ nếu có


            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFullName",
                HeaderText = "Họ và tên",
                DataPropertyName = "FullName", // Phải khớp với tên thuộc tính trong PassengerProfileDTO
                FillWeight = 25 // Chiếm 25% độ rộng
            });

            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDateOfBirth",
                HeaderText = "Ngày sinh",
                DataPropertyName = "DateOfBirth",
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } // Định dạng ngày tháng
            });
            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFlightDate",
                HeaderText = "Ngày bay",
                DataPropertyName = "FlightDate",
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } // Định dạng ngày tháng
            });

            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPassportNumber",
                HeaderText = "Số hộ chiếu",
                DataPropertyName = "PassportNumber",
                FillWeight = 20
            });

            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNumberSeat",
                HeaderText = "Số ghế",
                DataPropertyName = "NumberSeat", // Thuộc tính này có trong DTO
                FillWeight = 10
            });
            //dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    Name = "colPassport",
            //    HeaderText = "Hộ chiếu",
            //    DataPropertyName = "Passport",
            //    FillWeight = 15
            //});
            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNationality",
                HeaderText = "Quốc tịch",
                DataPropertyName = "Nationality",
                FillWeight = 15
            });

            dgvPassengerListTicket.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colAction",
                HeaderText = "Hành động",
                Text = "Sửa", // Chữ hiển thị trên nút
                UseColumnTextForButtonValue = true, // Luôn hiển thị chữ "Xóa"
                FillWeight = 10,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells // Co cột vừa với chữ "Xóa"
            });

            //var bindingList = new System.ComponentModel.BindingList<ProfileDTO>(_passengerList);
            dgvPassengerListTicket.DataSource = _passengerList;


        }
        private void take_data_from_Fright()
        {
            for (int index = 1; index < 3; index++)
            {
                //MessageBox.Show("Thêm hành khách thành công.", $"Thông báo cho hành khách {index}");
                //MessageBox.Show($"Thông báo cho hành khách {dtpDateOfBirthTicket.Value}");

                ProfileDTO profile = new ProfileDTO
                {
                    FullName = txtFullNameTicket.Text.Trim(),
                    NumberPhone = txtPhoneNumberTicket.Text.Trim(),
                    Email = txtEmailTicket.Text.Trim(),
                    //Nation = cboNationalityTicket.Text,
                    Nationality = "VN",
                    Passport = txtPassportNumberTicket.Text.Trim(),
                    DateOfBirth = dtpDateOfBirthTicket.Value,
                    FlightDate = dtpFlightDateTicket.Value,
                    NumberSeat = "1"
                };
                ListBooking.Add(profile);
            }
        }

        private void dgvPassengerListTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // --- KIỂM TRA ĐIỀU KIỆN ---

            // 1. Đảm bảo người dùng không nhấn vào hàng tiêu đề (header)
            if (e.RowIndex < 0)
            {
                return;
            }

            // 2. Đảm bảo người dùng đã nhấn đúng vào cột "Hành động" (cột chứa nút "Sửa")
            //    Chúng ta kiểm tra bằng tên của cột là "colAction".
            if (dgvPassengerListTicket.Columns[e.ColumnIndex].Name == "colAction")
            {
                // --- XỬ LÝ LOGIC ---

                // Lấy ra dòng hiện tại mà người dùng đã nhấn vào
                DataGridViewRow selectedRow = dgvPassengerListTicket.Rows[e.RowIndex];

                // Lấy đối tượng ProfileDTO tương ứng với dòng đó từ DataSource
                // Đây là cách an toàn và chuyên nghiệp để lấy dữ liệu.
                ProfileDTO selectedProfile = selectedRow.DataBoundItem as ProfileDTO;

                // Kiểm tra xem có lấy được đối tượng không (để phòng lỗi)
                if (selectedProfile != null)
                {
                    // --- ĐIỀN DỮ LIỆU NGƯỢC LẠI CÁC Ô NHẬP LIỆU ---

                    txtFullNameTicket.Text = selectedProfile.FullName;
                    txtPhoneNumberTicket.Text = selectedProfile.NumberPhone;
                    txtEmailTicket.Text = selectedProfile.Email;
                    cboNationalityTicket.Text = selectedProfile.Nationality; // Hoặc dùng .SelectedValue nếu có
                    txtPassportNumberTicket.Text = selectedProfile.Passport;
                    txtSeatTicket.Text = selectedProfile.NumberSeat;

                    // Đối với DateTimePicker, cần kiểm tra để không bị lỗi nếu ngày tháng null
                    // (Mặc dù trong trường hợp này có vẻ không null)
                    if (selectedProfile.DateOfBirth.HasValue) // Giả sử DateOfBirth là DateTime?
                    {
                        dtpDateOfBirthTicket.Value = selectedProfile.DateOfBirth.Value;
                    }
                    // Tương tự cho ngày bay
                    if (selectedProfile.FlightDate.HasValue)
                    {
                        dtpFlightDateTicket.Value = selectedProfile.FlightDate.Value;
                    }


                    // --- XỬ LÝ SAU KHI ĐIỀN ---

                    // Sau khi điền thông tin lên, chúng ta nên xóa hành khách này khỏi danh sách
                    // để người dùng sau khi sửa xong sẽ nhấn nút "Thêm" (lúc này đóng vai trò là nút "Cập nhật")
                    // để thêm lại vào danh sách với thông tin mới.
                    ListBooking.Remove(selectedProfile);

                    // Cập nhật lại DataGridView (nó sẽ thấy dòng đó bị xóa đi)
                    //RefreshDataGridView();

                    MessageBox.Show("Vui lòng chỉnh sửa thông tin và nhấn nút 'Thêm' để cập nhật.",
                                    "Sẵn sàng để sửa",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
        }
    }
}
