using BUS.Baggage;
using BUS.Profile;
using BUS.Ticket;
using DAO.EF;
using DAO.Models;
using DTO.Baggage;
using DTO.Booking;
using DTO.Profile;
using DTO.Ticket;
using GUI.Features.Seat.SubFeatures;
using GUI.Features.Validator;
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
        private readonly ProfileBUS _profileBus = new ProfileBUS();
        private readonly CarryOnBaggageBUS _carryOnBaggageBUS = new CarryOnBaggageBUS();
        private List<CarryOnBaggageDTO> carryOnList = new();
        private readonly BindingSource _bs = new();
        private int _editingIndex = -1; // -1 = thêm mới; >=0 = đang sửa dòng này
        private int _passengerCount = 0;

        private int _ticketCount ;
        private int _accountId ;

        private string _typeTrip;
        private int _selectedSeatId;
        private int _selectedFlightSeatId;
        private decimal _selectedSeatPrice;
        private int _classId ;
        private int _flightId;
        private int _seatId;
        private BookingRequestDTO bookingRequest;

        //private int _ticketCount = 3;
        //private int _accountId = 1;
        
        // Round-trip booking support
        private DTO.Booking.BookingRequestDTO _outboundBooking;
        private DTO.Booking.BookingRequestDTO _returnBooking;
        private bool _isRoundTrip = false;
        private bool _isEnteringReturn = false;
        private TicketBookingRequestDTO _tempOutbound = null;

        private readonly BindingList<TicketBookingRequestDTO> _outboundPassengers = new();
        private readonly BindingList<TicketBookingRequestDTO> _inboundPassengers = new();
        public frmPassengerInfoControl()
        {
            
            InitializeComponent();
            
            InitGrid();
            LoadCheckBaggage();
            LoadNationality();
            btnAddPassengerTicket.Text = "Nhập";
            carryOnList = _carryOnBaggageBUS.GetAll();
            // Chặn popup DataError mặc định của DGV
            dgvPassengerListTicket.DataError += (s, e) => { e.ThrowException = false; };
        }
       
        private void LoadCheckBaggage()
        {
            CheckedBaggageBUS checkedBaggageBUS = new CheckedBaggageBUS();
            var baggageList = checkedBaggageBUS.GetAllCheckedBaggage();
            cboBaggageTicket.DataSource = baggageList;
            cboBaggageTicket.DisplayMember = "Description";
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
            //_bs.DataSource = _passengers; // List<TicketBookingRequestDTO>
            //dgvPassengerListTicket.DataSource = _bs;

            _bs.DataSource = _outboundPassengers;
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
        public class SeatSelectorForm : Form
        {
            public OpenSeatSelectorControl Selector { get; private set; }

            public SeatSelectorForm(int flightId, int classId)
            {
                Text = "Chọn ghế";
                Width = 500;
                Height = 400;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;

                Selector = new OpenSeatSelectorControl();
                Selector.Dock = DockStyle.Fill;
                Controls.Add(Selector);

                // Load đúng thời điểm
                Load += (s, e) =>
                {
                    Selector.LoadSeats(flightId, classId);
                };
            }
        }
        private void btnSelectSeatTicket_Click(object sender, EventArgs e)
        {
            var form = new SeatSelectorForm(1, 1);

            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                var seat = form.Selector.GetSelectedSeat();
                if (seat != null)
                {
                    // Gán vào UI
                    txtSeatTicket.Text = seat.SeatNumber;
                    _selectedFlightSeatId = seat.FlightSeatId;
                    _selectedSeatPrice = seat.Price;
                    _classId = seat.ClassId;
                }
            }
        }
        // Nhập / Cập nhật
        private void btnAddPassengerTicket_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            if (!ValidatorForFrm.Check(txtFullNameTicket.Text, "name")) return;
            if (!ValidatorForFrm.Check(txtPhoneNumberTicket.Text, "phone")) return;
            if (!ValidatorForFrm.Check(txtEmailTicket.Text, "email")) return;
            if (!ValidatorForFrm.Check(txtPassportNumberTicket.Text, "passport")) return;

            // ========================================
            // CASE SỬA THÔNG TIN CHIỀU ĐI
            // ========================================
            if (_editingIndex >= 0 && !_isEnteringReturn)
            {
                var dto = _outboundPassengers[_editingIndex];
                MapFormToDto(dto);

                // Nếu vé khứ hồi → yêu cầu sửa tiếp chiều về
                if (_isRoundTrip)
                {
                    MessageBox.Show("✔ Đã cập nhật chiều đi. Tiếp tục sửa chiều về!");

                    // Load form lại để nhập ghế chiều về
                    PrepareReturnForm(dto);
                    _isEnteringReturn = true;

                    btnAddPassengerTicket.Text = "Cập nhật chiều về";
                    return;
                }

                // Một chiều → kết thúc sửa
                _outboundPassengers.ResetItem(_editingIndex);
                _editingIndex = -1;
                btnAddPassengerTicket.Text = "Nhập";
                ClearForm();
                return;
            }

            // ========================================
            // CASE SỬA THÔNG TIN CHIỀU VỀ
            // ========================================
            if (_editingIndex >= 0 && _isEnteringReturn)
            {
                var dtoReturn = _inboundPassengers[_editingIndex];
                MapFormToDto(dtoReturn);

                dtoReturn.FlightId = _returnBooking.FlightId;
                dtoReturn.FlightDate = _returnBooking.DepartureTime;

                _inboundPassengers.ResetItem(_editingIndex);

                MessageBox.Show("✔ Đã cập nhật hoàn chỉnh chiều đi & chiều về!");

                _editingIndex = -1;
                _isEnteringReturn = false;
                _tempOutbound = null;

                btnAddPassengerTicket.Text = "Nhập";
                ClearForm();
                return;
            }

            // ========================================
            // CASE THÊM MỚI (Anh giữ nguyên logic cũ)
            // ========================================
            if (_isRoundTrip && _isEnteringReturn)
            {
                var returnDto = new TicketBookingRequestDTO();
                MapFormToDto(returnDto);
                returnDto.FlightId = _returnBooking.FlightId;
                returnDto.FlightDate = _returnBooking.DepartureTime;

                _inboundPassengers.Add(returnDto);

                MessageBox.Show("✔ Đã nhập xong chiều đi và chiều về!");

                _isEnteringReturn = false;
                _tempOutbound = null;

                _passengerCount++;
                ClearForm();
                return;
            }

            if (_passengerCount < _ticketCount)
            {
                var outboundDto = new TicketBookingRequestDTO();
                MapFormToDto(outboundDto);

                outboundDto.FlightId = _outboundBooking.FlightId;
                outboundDto.FlightDate = _outboundBooking.DepartureTime;

                _outboundPassengers.Add(outboundDto);

                if (_isRoundTrip)
                {
                    _tempOutbound = outboundDto;
                    MessageBox.Show("✔ Đã nhập chiều đi. Vui lòng nhập chiều về!");
                    PrepareReturnForm(outboundDto);
                    _isEnteringReturn = true;
                    return;
                }

                _passengerCount++;
                ClearForm();
                return;
            }

            MessageBox.Show($"Đã đủ {_ticketCount} hành khách.");
        }

        private void PrepareReturnForm(TicketBookingRequestDTO dto)
        {
            txtFullNameTicket.Text = dto.FullName;
            txtPhoneNumberTicket.Text = dto.PhoneNumber;
            txtEmailTicket.Text = dto.Email;
            txtPassportNumberTicket.Text = dto.PassportNumber;
            cboNationalityTicket.Text = dto.Nationality;

            dtpDateOfBirthTicket.Value = dto.DateOfBirth ?? DateTime.Now;

            // Gán thông tin flight chiều về
            _flightId = _returnBooking.FlightId;
            dtpFlightDateTicket.Value = _returnBooking.DepartureTime ?? DateTime.Now;

            // RESET GHẾ vì cần chọn lại ghế cho chiều về
            txtSeatTicket.Text = "";
            _selectedSeatPrice = 0;
            _selectedSeatId = 0;
            _selectedFlightSeatId = 0;
        }

        // Bấm nút "Sửa" trong lưới → đổ ngược lên form
        private void dgvPassengerListTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvPassengerListTicket.Columns[e.ColumnIndex].Name != "colAction") return;

            // Chỉ sửa chiều đi (outbound)
            var dto = _outboundPassengers[e.RowIndex];

            _editingIndex = e.RowIndex;
            _isEnteringReturn = false;
            _tempOutbound = dto;

            // Đổ form từ DTO chiều đi
            LoadFormFromDto(dto);

            btnAddPassengerTicket.Text = "Cập nhật chiều đi";
        }
        private void LoadFormFromDto(TicketBookingRequestDTO item)
        {
            txtFullNameTicket.Text = item.FullName ?? "";
            txtPhoneNumberTicket.Text = item.PhoneNumber ?? "";
            txtEmailTicket.Text = item.Email ?? "";
            cboNationalityTicket.Text = item.Nationality ?? "VN";
            txtPassportNumberTicket.Text = item.PassportNumber ?? "";

            txtSeatTicket.Text = item.SeatNumber ?? "";

            dtpDateOfBirthTicket.Value = item.DateOfBirth ?? DateTime.Now;
            dtpFlightDateTicket.Value = item.FlightDate ?? DateTime.Now;

            cboBaggageTicket.Text = item.BaggageDisplayText ?? "";
            txtNoteBaggage.Text = item.BaggageNote ?? "";
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
            dto.FlightId = _flightId; //=> Anh map ở ngoài khi chọn chuyến bay

            // ========= Seat info =========
            dto.SeatNumber = txtSeatTicket.Text;
            dto.SeatId = 1;        // tùy theo UI của Anh, nếu chưa có thì để null.
            dto.FlightSeatId = _selectedFlightSeatId;  // tùy theo UI của Anh, nếu chưa có thì để null.
            dto.ClassId = _classId;      // tùy theo UI của Anh, nếu chưa có thì để null.

            // ========= Baggage info =========
            if (cboBaggageTicket.SelectedItem != null)
            {
                dto.CheckedId = (cboBaggageTicket.SelectedItem as DTO.Baggage.CheckedBaggageDTO)?.CheckedId;
                dto.BaggageDisplayText = cboBaggageTicket.Text;
            }
            dto.Quantity = 1; // mặc định 1 kiện
            dto.CarryOnId = CarryBaggageId(_classId); // hang ve
            // carry on id nếu có UI chọn hành lý xách tay thêm thì map tương tự CheckedId
            dto.BaggageNote = txtNoteBaggage.Text;
            dto.TicketPrice = (cboBaggageTicket.SelectedItem as DTO.Baggage.CheckedBaggageDTO)?.Price + _selectedSeatPrice;
            // ========= Ticket info =========
            // dto.TicketNumber => BUS sẽ tự generate
        }
        
        private int CarryBaggageId(int ClassId)
        {
            CarryOnBaggageDTO result = carryOnList.FirstOrDefault(c => c.ClassId == ClassId);
            return result.CarryOnId;   // nếu null thì return 0

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
            var bus = new SaveTicketRequestBUS();

            if (_isRoundTrip)
            {
                bus.SaveRoundTrip(
                    _outboundPassengers.ToList(),
                    _inboundPassengers.ToList(),
                    _accountId
                );
            }
            else
            {
                bus.SaveOneWay(
                    _outboundPassengers.ToList(),
                    _accountId
                );
            }

            MessageBox.Show("Đặt vé thành công!");
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
        public void LoadBookingRequest(BookingRequestDTO outbound, BookingRequestDTO inbound = null)
        {
            if (outbound == null) return;
            // chưa biết cách lấy id.
            _accountId = 2;
            LoadInfomationAccount(_accountId);

            _outboundBooking = outbound;
            _returnBooking = inbound;
            _isRoundTrip = inbound != null;

            var (flightId, cabinClass, ticketCount, _) = outbound.GetBookingInfo();
            _flightId = flightId;
            _classId = cabinClass;
            _ticketCount = ticketCount;

            _typeTrip = _isRoundTrip ? "ROUND_TRIP" : "ONE_WAY";

            dtpFlightDateTicket.Value = outbound.DepartureTime ?? DateTime.Now;
        }

        //public void LoadBookingRequest1(DTO.Booking.BookingRequestDTO outboundBooking, DTO.Booking.BookingRequestDTO returnBooking = null)
        //{
        //    if (bookingRequest == null) return;
        //    _accountId = 2;

        //    bookingRequest = bookingRequest;
        //    LoadInfomationAccount(_accountId);

           
        //    if (outboundBooking == null) return;

        //    // Store booking information
        //    _outboundBooking = outboundBooking;
        //    _returnBooking = returnBooking;
        //    _isRoundTrip = outboundBooking.IsRoundTrip && returnBooking != null;

        //    // Lấy thông tin đặt vé
        //    var (flightId, cabinClassId, ticketCount, isRoundTrip) = outboundBooking.GetBookingInfo();
        //    _ticketCount = ticketCount;
        //    _classId = cabinClassId;
        //    _flightId = flightId;
        //    _typeTrip = "ONE_WAY";
        //    // Build message with flight information
        //    var message = new System.Text.StringBuilder();
        //    message.AppendLine("═══════════════════════════════════════");

        //    if (_isRoundTrip)
        //    {
        //        message.AppendLine("✈️ VÉ KHỨ HỒI (2 CHIỀU)");
        //        message.AppendLine("═══════════════════════════════════════\n");

        //        // Outbound flight
        //        message.AppendLine("🛫 CHUYẾN ĐI:");
        //        message.AppendLine($"   Chuyến bay: {outboundBooking.FlightNumber}");
        //        message.AppendLine($"   Tuyến: {outboundBooking.DepartureAirportCode} → {outboundBooking.ArrivalAirportCode}");
        //        message.AppendLine($"   Hạng vé: {outboundBooking.CabinClassName}");
        //        message.AppendLine($"   Giờ khởi hành: {outboundBooking.DepartureTime?.ToString("dd/MM/yyyy HH:mm")}");
        //        message.AppendLine();

        //        // Return flight
        //        message.AppendLine("🛬 CHUYẾN VỀ:");
        //        message.AppendLine($"   Chuyến bay: {returnBooking.FlightNumber}");
        //        message.AppendLine($"   Tuyến: {returnBooking.DepartureAirportCode} → {returnBooking.ArrivalAirportCode}");
        //        message.AppendLine($"   Hạng vé: {returnBooking.CabinClassName}");
        //        message.AppendLine($"   Giờ khởi hành: {returnBooking.DepartureTime?.ToString("dd/MM/yyyy HH:mm")}");
        //        message.AppendLine();

        //        message.AppendLine($"👥 Số lượng hành khách: {ticketCount} người");
        //        message.AppendLine($"🔗 Mã nhóm: {outboundBooking.GroupBookingId}");
        //    }
        //    else
        //    {
        //        message.AppendLine("✈️ VÉ MỘT CHIỀU");
        //        message.AppendLine("═══════════════════════════════════════\n");
        //        message.AppendLine($"Chuyến bay: {outboundBooking.FlightNumber}");
        //        message.AppendLine($"Tuyến: {outboundBooking.DepartureAirportCode} → {outboundBooking.ArrivalAirportCode}");
        //        message.AppendLine($"Hạng vé: {outboundBooking.CabinClassName}");
        //        message.AppendLine($"Giờ khởi hành: {outboundBooking.DepartureTime?.ToString("dd/MM/yyyy HH:mm")}");
        //        message.AppendLine($"Số lượng hành khách: {ticketCount} người");
        //    }

        //    message.AppendLine();
        //    message.AppendLine($"Vui lòng điền thông tin cho {ticketCount} hành khách.");

        //    MessageBox.Show(
        //        message.ToString(),
        //        "Thông tin đặt vé",
        //        MessageBoxButtons.OK,
        //        MessageBoxIcon.Information);

        //    // Pre-fill flight date from outbound booking
        //    if (outboundBooking.DepartureTime.HasValue)
        //    {
        //        dtpFlightDateTicket.Value = outboundBooking.DepartureTime.Value;
        //    }
        //}
    }
}
