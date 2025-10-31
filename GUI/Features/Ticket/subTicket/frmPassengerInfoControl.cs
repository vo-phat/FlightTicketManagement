using DTO.Profile;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Ticket.subTicket
{
    public partial class frmPassengerInfoControl : UserControl
    {
        // Nguồn dữ liệu & trạng thái
        private readonly BindingList<ProfileDTO> _passengers = new(); // CÁCH 1
        private readonly BindingSource _bs = new();
        private int _editingIndex = -1; // -1 = thêm mới; >=0 = đang sửa dòng này

        public frmPassengerInfoControl()
        {
            InitializeComponent();
            InitGrid();                        // cấu hình lưới + binding 1 lần
            btnAddPassengerTicket.Text = "Nhập";

            // Chặn popup DataError mặc định của DGV
            dgvPassengerListTicket.DataError += (s, e) => { e.ThrowException = false; };
        }

        private void InitGrid()
        {
            dgvPassengerListTicket.AutoGenerateColumns = false;
            dgvPassengerListTicket.AllowUserToAddRows = false;
            dgvPassengerListTicket.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPassengerListTicket.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPassengerListTicket.Columns.Clear();

            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFullName",
                HeaderText = "Họ và tên",
                DataPropertyName = "FullName",
                FillWeight = 25
            });
            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDateOfBirth",
                HeaderText = "Ngày sinh",
                DataPropertyName = "DateOfBirth",
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFlightDate",
                HeaderText = "Ngày bay",
                DataPropertyName = "FlightDate",
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPassport",
                HeaderText = "Số hộ chiếu",
                DataPropertyName = "Passport",
                FillWeight = 18
            });
            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEmail",
                HeaderText = "Email",
                DataPropertyName = "Email",
                FillWeight = 20
            });
            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNumberSeat",
                HeaderText = "Số ghế",
                DataPropertyName = "NumberSeat",
                FillWeight = 10
            });
            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNationality",
                HeaderText = "Quốc tịch",
                DataPropertyName = "Nationality",
                FillWeight = 12
            });
            dgvPassengerListTicket.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colAction",
                HeaderText = "Hành động",
                Text = "Sửa",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            // Binding
            _bs.DataSource = _passengers;
            dgvPassengerListTicket.DataSource = _bs;

            // Sự kiện click nút "Sửa"
            dgvPassengerListTicket.CellContentClick += dgvPassengerListTicket_CellContentClick;
        }

        private void btnSelectSeatTicket_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chọn ghế sẽ xuất hiện sớm.");
        }

        // Nhập / Cập nhật
        private void btnAddPassengerTicket_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            if (_editingIndex >= 0)
            {
                // CẬP NHẬT IN-PLACE
                var p = _passengers[_editingIndex];
                MapFormToDto(p);
                _passengers.ResetItem(_editingIndex); // refresh đúng 1 dòng
                _editingIndex = -1;
                btnAddPassengerTicket.Text = "Nhập";
                ClearForm();
                return;
            }

            // THÊM MỚI (mỗi lần 1 người – bỏ for)
            var profile = new ProfileDTO();
            MapFormToDto(profile);
            _passengers.Add(profile); // BindingList tự thông báo Add, không cần reset
            ClearForm();
        }

        // Bấm nút "Sửa" trong lưới → đổ ngược lên form
        private void dgvPassengerListTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvPassengerListTicket.Columns[e.ColumnIndex].Name != "colAction") return;

            var item = _passengers[e.RowIndex];
            _editingIndex = e.RowIndex;

            txtFullNameTicket.Text = item.FullName ?? "";
            txtPhoneNumberTicket.Text = item.NumberPhone ?? "";
            txtEmailTicket.Text = item.Email ?? "";
            cboNationalityTicket.Text = item.Nationality ?? "VN";
            txtPassportNumberTicket.Text = item.Passport ?? "";
            txtSeatTicket.Text = item.NumberSeat ?? "";
            // Nếu DTO là DateTime (không nullable) thì set thẳng:
            dtpDateOfBirthTicket.Value = item.DateOfBirth;
            dtpFlightDateTicket.Value = item.FlightDate;

            btnAddPassengerTicket.Text = "Cập nhật";
        }

        // Helpers
        private void MapFormToDto(ProfileDTO p)
        {
            p.FullName = txtFullNameTicket.Text.Trim();
            p.NumberPhone = txtPhoneNumberTicket.Text.Trim();
            p.Email = txtEmailTicket.Text.Trim();
            p.Nationality = string.IsNullOrWhiteSpace(cboNationalityTicket.Text) ? "VN" : cboNationalityTicket.Text.Trim();
            p.Passport = txtPassportNumberTicket.Text.Trim();
            p.DateOfBirth = dtpDateOfBirthTicket.Value.Date;
            p.FlightDate = dtpFlightDateTicket.Value.Date;
            p.NumberSeat = txtSeatTicket.Text.Trim();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtFullNameTicket.Text))
            {
                MessageBox.Show("Vui lòng nhập họ và tên.");
                return false;
            }
            return true;
        }

        private void ClearForm()
        {
            txtFullNameTicket.Text = "";
            txtPhoneNumberTicket.Text = "";
            txtEmailTicket.Text = "";
            txtPassportNumberTicket.Text = "";
            txtSeatTicket.Text = "";
            cboNationalityTicket.Text = "VN";
            dtpDateOfBirthTicket.Value = DateTime.Today;
            dtpFlightDateTicket.Value = DateTime.Today;
        }
    }
}
