using BUS.Baggage;
using BUS.Profile;
using BUS.Ticket;
using DAO.EF;
using DTO.BaggageDTO;
using DTO.Profile;
using DTO.Ticket;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace GUI.Features.Ticket.subTicket
{
    public partial class frmPassengerInfoControl : UserControl
    {
        // Nguồn dữ liệu & trạng thái
        private readonly BindingList<TicketBookingRequestDTO> _passengers = new(); // CÁCH 1
        private readonly ProfileBUS _profileBus = new ProfileBUS();
        private readonly BindingSource _bs = new();
        private int _editingIndex = -1; // -1 = thêm mới; >=0 = đang sửa dòng này
        private int _passengerCount = 0;
        private int _ticketCount = 3;
        private int _accountId = 1;
        public frmPassengerInfoControl()
        {
            
            InitializeComponent();
            
            InitGrid();
            LoadInfomationAccount(_accountId);
            LoadCheckBaggage();
            LoadNationality();
            // cấu hình lưới + binding 1 lần
            btnAddPassengerTicket.Text = "Nhập";

            // Chặn popup DataError mặc định của DGV
            dgvPassengerListTicket.DataError += (s, e) => { e.ThrowException = false; };
        }
        private void LoadCheckBaggage()
        {
            CheckedBaggageBUS checkedBaggageBUS = new CheckedBaggageBUS();
            var baggageList = checkedBaggageBUS.GetAllCheckedBaggage();
            cboBaggageTicket.DataSource = baggageList;
            cboBaggageTicket.DisplayMember = "DisplayText";
            cboBaggageTicket.ValueMember = "CheckedId";
        }
        private void LoadNationality()
        {
            var bus = new NationalBUS();
            var list = bus.GetAllNationals();

            cboNationalityTicket.DataSource = list;
            cboNationalityTicket.DisplayMember = "DisplayName";
            cboNationalityTicket.ValueMember = "CountryName";
        }
        private void LoadInfomationAccount(int _accountID)
        {
            //MessageBox.Show(_accountID.ToString());
            var profile = _profileBus.GetProfileByAccountId(_accountID);

            if (profile == null)
            {
                MessageBox.Show("Không tìm thấy thông tin tài khoản!");
                return;
            }

            txtFullNameTicket.Text = profile.FullName ?? "";
            dtpDateOfBirthTicket.Value = profile.DateOfBirth ?? DateTime.Now;
            txtPhoneNumberTicket.Text = profile.PhoneNumber ?? "";
            txtPassportNumberTicket.Text = profile.PassportNumber ?? "";
            txtEmailTicket.Text = profile.Email ?? "";

            cboNationalityTicket.Text = profile.Nationality ?? "VN";
        }
        private void InitGrid()
        {
            dgvPassengerListTicket.AutoGenerateColumns = false;
            dgvPassengerListTicket.AllowUserToAddRows = false;
            dgvPassengerListTicket.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPassengerListTicket.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPassengerListTicket.Columns.Clear();

            // ========================================
            // 1) CỘT HIỂN THỊ CHÍNH (4–5 trường quan trọng)
            // ========================================

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
                Name = "colNationality",
                HeaderText = "Quốc tịch",
                DataPropertyName = "Nationality",
                FillWeight = 15
            });

            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPassportNumber",
                HeaderText = "Hộ chiếu",
                DataPropertyName = "PassportNumber",
                FillWeight = 20
            });

            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSeatNumber",
                HeaderText = "Số ghế",
                DataPropertyName = "SeatNumber",
                FillWeight = 15
            });

            // ============================
            // 2) NÚT SỬA
            // ============================

            dgvPassengerListTicket.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colAction",
                HeaderText = "Hành động",
                Text = "Sửa",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            // ========================================
            // 3) HIDDEN COLUMNS (vẫn map DTO nhưng ẩn)
            // ========================================

            AddHiddenColumn("AccountId");
            AddHiddenColumn("FlightId");
            AddHiddenColumn("FlightDate");
            AddHiddenColumn("PhoneNumber");
            AddHiddenColumn("Email");
            AddHiddenColumn("SeatId");
            AddHiddenColumn("FlightSeatId");
            AddHiddenColumn("ClassId");
            AddHiddenColumn("CarryOnId");
            AddHiddenColumn("CheckedId");
            AddHiddenColumn("Quantity");
            AddHiddenColumn("BaggageNote");
            AddHiddenColumn("BaggageDisplayText");
            AddHiddenColumn("TicketNumber");
            AddHiddenColumn("Note");

            // Binding
            _bs.DataSource = _passengers; // List<TicketBookingRequestDTO>
            dgvPassengerListTicket.DataSource = _bs;

            dgvPassengerListTicket.CellContentClick += dgvPassengerListTicket_CellContentClick;
        }

        /// <summary>
        /// Thêm cột ẩn nhưng vẫn bind property để giữ dữ liệu đầy đủ.
        /// </summary>
        private void AddHiddenColumn(string propertyName)
        {
            dgvPassengerListTicket.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "col" + propertyName,
                HeaderText = propertyName,
                DataPropertyName = propertyName,
                Visible = false
            });
        }
       

        private void btnSelectSeatTicket_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chọn ghế sẽ xuất hiện sớm.");
        }

        // Nhập / Cập nhật
        private void btnAddPassengerTicket_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            // ======== TRƯỜNG HỢP SỬA ========
            if (_editingIndex >= 0)
            {
                var dto = _passengers[_editingIndex];       // DTO cũ
                MapFormToDto(dto);
                ShowTicketDtoInfo(dto);// Update DTO cũ
                _passengers.ResetItem(_editingIndex);       // Refresh 1 dòng
                _editingIndex = -1;

                btnAddPassengerTicket.Text = "Nhập";
                ClearForm();
                return;
            }

            // ======== TRƯỜNG HỢP THÊM MỚI ========
            if (_passengerCount < _ticketCount)
            {
               

                var dto = new TicketBookingRequestDTO();
                MapFormToDto(dto);
                ShowTicketDtoInfo(dto);
                _passengers.Add(dto);                       // BindingList tự refresh
                ClearForm();
                _passengerCount++;
            }
            else
            {
                MessageBox.Show($"Đã đủ {_ticketCount} hành khách cho vé này.");
            }
        }


        // Bấm nút "Sửa" trong lưới → đổ ngược lên form
        private void dgvPassengerListTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvPassengerListTicket.Columns[e.ColumnIndex].Name != "colAction") return;

            var item = _passengers[e.RowIndex];          // TicketBookingRequestDTO
            _editingIndex = e.RowIndex;

            // ========= Passenger info =========
            txtFullNameTicket.Text = item.FullName ?? "";
            txtPhoneNumberTicket.Text = item.PhoneNumber ?? "";
            txtEmailTicket.Text = item.Email ?? "";
            cboNationalityTicket.Text = item.Nationality ?? "VN";
            txtPassportNumberTicket.Text = item.PassportNumber ?? "";

            // ========= Seat info =========
            txtSeatTicket.Text = item.SeatNumber ?? "";

            // ========= DateTime? phải check null =========
            dtpDateOfBirthTicket.Value = item.DateOfBirth ?? DateTime.Now;
            dtpFlightDateTicket.Value = item.FlightDate ?? DateTime.Now;

            // ========= Baggage info =========
            cboBaggageTicket.Text = item.BaggageDisplayText ?? "";
            txtNoteBaggage.Text = item.BaggageNote ?? "";

            btnAddPassengerTicket.Text = "Cập nhật";
        }


        // Helpers
        private void MapFormToDto(TicketBookingRequestDTO dto)
        {
            // ========= Passenger info =========
            if(_editingIndex == 0 || _passengerCount ==0)
            {
                dto.AccountId = _accountId; // nếu user login
                MessageBox.Show(dto.AccountId.ToString());
            }
            dto.FullName = txtFullNameTicket.Text;
            dto.DateOfBirth = dtpDateOfBirthTicket.Value;
            dto.PhoneNumber = txtPhoneNumberTicket.Text;
            dto.PassportNumber = txtPassportNumberTicket.Text;
            dto.Nationality = cboNationalityTicket.SelectedValue?.ToString();
            dto.Email = txtEmailTicket.Text;
            

            // ========= Flight info =========
            dto.FlightDate = dtpFlightDateTicket.Value;
            dto.FlightId = 1; //=> Anh map ở ngoài khi chọn chuyến bay

            // ========= Seat info =========
            dto.SeatNumber = txtSeatTicket.Text;
            dto.SeatId = 1;        // tùy theo UI của Anh, nếu chưa có thì để null.
            dto.FlightSeatId = 1;  // tùy theo UI của Anh, nếu chưa có thì để null.
            dto.ClassId = 1;      // tùy theo UI của Anh, nếu chưa có thì để null.

            // ========= Baggage info =========
            if (cboBaggageTicket.SelectedItem != null)
            {
                dto.CheckedId = (cboBaggageTicket.SelectedItem as DTO.Baggage.CheckedBaggageDTO)?.CheckedId;
                dto.BaggageDisplayText = cboBaggageTicket.Text;
            }
            dto.Quantity = 1; // mặc định 1 kiện
            dto.CarryOnId = 1;
            // carry on id nếu có UI chọn hành lý xách tay thêm thì map tương tự CheckedId
            dto.BaggageNote = txtNoteBaggage.Text;

            // ========= Ticket info =========
            // dto.TicketNumber => BUS sẽ tự generate
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

        private void underlinedTextField1_Load(object sender, EventArgs e)
        {

        }

        private void cboBaggageTicket_Load(object sender, EventArgs e)
        {

        }

        private void btnNextToPayment_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_passengers.Count.ToString());

        }
        public void ShowTicketDtoInfo(TicketBookingRequestDTO dto)
        {
            if (dto == null)
            {
                MessageBox.Show("DTO null rồi Anh!");
                return;
            }

            var sb = new StringBuilder();

            foreach (var prop in typeof(TicketBookingRequestDTO).GetProperties())
            {
                var name = prop.Name;
                var value = prop.GetValue(dto) ?? "(null)";

                // format DateTime?
                if (value is DateTime dt)
                    value = dt.ToString("dd/MM/yyyy");

                sb.AppendLine($"{name}: {value}");
            }

            MessageBox.Show(sb.ToString(), "DEBUG – Dữ liệu TicketBookingRequestDTO");
        }

        /// <summary>
        /// Load thông tin booking từ dialog chọn hạng vé
        /// </summary>
        public void LoadBookingRequest(DTO.Booking.BookingRequestDTO bookingRequest)
        {
            if (bookingRequest == null) return;

            // Hiển thị thông tin chuyến bay đã chọn
            MessageBox.Show(
                $"Thông tin đặt vé:\n" +
                $"Chuyến bay: {bookingRequest.FlightNumber}\n" +
                $"Tuyến: {bookingRequest.DepartureAirportCode} → {bookingRequest.ArrivalAirportCode}\n" +
                $"Hạng vé: {bookingRequest.CabinClassName}\n" +
                $"Giờ khởi hành: {bookingRequest.DepartureTime?.ToString("dd/MM/yyyy HH:mm")}\n\n" +
                $"Vui lòng điền thông tin hành khách.",
                "Thông tin đặt vé",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // TODO: Pre-fill flight và cabin class information vào form
            // Có thể lưu bookingRequest vào field để dùng khi submit
        }
    }
}
