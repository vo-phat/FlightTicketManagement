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
        private int _returnFlightId;
        private int _returnClassId;

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
            // Nếu đang nhập chiều về thì dùng flight về
            int targetFlightId = _isEnteringReturn ? _returnFlightId : _flightId;
            int targetClassId = _isEnteringReturn ? _returnClassId : _classId;

            var form = new SeatSelectorForm(targetFlightId, targetClassId);

            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                var seat = form.Selector.GetSelectedSeat();
                if (seat != null)
                {
                    txtSeatTicket.Text = seat.SeatNumber;
                    _selectedFlightSeatId = seat.FlightSeatId;
                    _selectedSeatPrice = seat.Price;
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
            // Nếu đang yêu cầu nhập chiều về → không cho nhập chiều đi tiếp
            if (_isEnteringReturn && !_isRoundTrip)
            {
                MessageBox.Show("Bạn đang trong bước nhập chiều về. Vui lòng hoàn tất trước.");
                return;
            }

            // ============================================================
            // CASE 1) SỬA CHIỀU ĐI
            // ============================================================
            if (_editingIndex >= 0 && !_isEnteringReturn)
            {
                var dto = _outboundPassengers[_editingIndex];

                // Chỉ update thông tin passenger – KHÔNG được sửa FlightId/ClassId
                MapFormToDto(dto);

                if (_isRoundTrip)
                {
                    MessageBox.Show("✔ Đã cập nhật chiều đi. Tiếp tục sửa chiều về!");
                    PrepareReturnForm(dto);
                    _isEnteringReturn = true;
                    btnAddPassengerTicket.Text = "Cập nhật chiều về";
                    return;
                }

                // Một chiều → kết thúc
                _outboundPassengers.ResetItem(_editingIndex);
                _editingIndex = -1;
                btnAddPassengerTicket.Text = "Nhập";
                ClearForm();
                return;
            }

            // ============================================================
            // CASE 2) SỬA CHIỀU VỀ
            // ============================================================
            if (_editingIndex >= 0 && _isEnteringReturn)
            {
                var dtoReturn = _inboundPassengers[_editingIndex];

                // Map passenger info
                MapFormToDto(dtoReturn);

                // FIX: Gán đúng flight/class chiều về
                dtoReturn.FlightId = _returnFlightId;
                dtoReturn.ClassId = _returnClassId;
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

            // ============================================================
            // CASE 3) THÊM MỚI – NHẬP CHIỀU VỀ (ROUND-TRIP)
            // ============================================================
            if (_isRoundTrip && _isEnteringReturn)
            {
                var returnDto = new TicketBookingRequestDTO();

                MapFormToDto(returnDto);

                // FIX: Gán flight/class chiều về 100% đúng
                returnDto.FlightId = _returnFlightId;
                returnDto.ClassId = _returnClassId;
                returnDto.FlightDate = _returnBooking.DepartureTime;

                _inboundPassengers.Add(returnDto);

                MessageBox.Show("✔ Đã nhập xong chiều đi và chiều về!");

                _isEnteringReturn = false;
                _tempOutbound = null;

                _passengerCount++;
                ClearForm();
                return;
            }

            // ============================================================
            // CASE 4) THÊM MỚI – NHẬP CHIỀU ĐI
            // ============================================================
            if (_passengerCount < _ticketCount)
            {
                var outboundDto = new TicketBookingRequestDTO();

                MapFormToDto(outboundDto);

                // FlightId/ClassId CHỈ GÁN Ở ĐÂY (chiều đi)
                outboundDto.FlightId = _outboundBooking.FlightId;
                outboundDto.ClassId = _classId;
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

                // Một chiều
                _passengerCount++;
                ClearForm();
                return;
            }

            // ============================================================
            // CASE 5) ĐÃ ĐỦ VÉ
            // ============================================================
            MessageBox.Show($"Đã đủ {_ticketCount} hành khách.");
        }


        private void PrepareReturnForm(TicketBookingRequestDTO dto)
        {
            // Copy passenger info
            txtFullNameTicket.Text = dto.FullName;
            txtPhoneNumberTicket.Text = dto.PhoneNumber;
            txtEmailTicket.Text = dto.Email;
            txtPassportNumberTicket.Text = dto.PassportNumber;
            cboNationalityTicket.Text = dto.Nationality;
            dtpDateOfBirthTicket.Value = dto.DateOfBirth ?? DateTime.Now;

            // Set flight chiều về – nhưng chỉ dùng để HIỂN THỊ
            dtpFlightDateTicket.Value = _returnBooking.DepartureTime ?? DateTime.Now;

            // reset seat
            txtSeatTicket.Text = "";
            _selectedSeatPrice = 0;
            _selectedSeatId = 0;
            _selectedFlightSeatId = 0;
        }


        // Bấm nút "Sửa" trong lưới → đổ ngược lên form
        private void dgvPassengerListTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_isEnteringReturn)
            {
                MessageBox.Show("Bạn phải hoàn tất nhập/sửa chiều về trước khi sửa hành khách khác.");
                return;
            }

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

            // ⭐ RẤT QUAN TRỌNG – GIỮ LẠI GHẾ CŨ KHI EDIT
            _selectedFlightSeatId = item.FlightSeatId;
            //_classId = item.ClassId ?? _classId;
            //_flightId = item.FlightId ?? _flightId;


            // Nếu có giá seat → giữ
            if (item.TicketPrice.HasValue)
            {
                _selectedSeatPrice = item.TicketPrice.Value;
            }
        }


        // Helpers
        private void MapFormToDto(TicketBookingRequestDTO dto)
        {
            // ===== Passenger Info =====
            dto.AccountId = _accountId;
            dto.FullName = txtFullNameTicket.Text;
            dto.DateOfBirth = dtpDateOfBirthTicket.Value;
            dto.PhoneNumber = txtPhoneNumberTicket.Text;
            dto.PassportNumber = txtPassportNumberTicket.Text;
            dto.Nationality = cboNationalityTicket.SelectedValue?.ToString();
            dto.Email = txtEmailTicket.Text;

            // ===== Seat Info =====
            if (!string.IsNullOrEmpty(txtSeatTicket.Text) && _selectedFlightSeatId != 0)
            {
                // chọn ghế mới
                dto.SeatNumber = txtSeatTicket.Text;
                dto.FlightSeatId = _selectedFlightSeatId;
            }

            // ===== Baggage =====
            if (cboBaggageTicket.SelectedItem is DTO.Baggage.CheckedBaggageDTO cb)
            {
                dto.CheckedId = cb.CheckedId;
                dto.BaggageDisplayText = cboBaggageTicket.Text;
            }

            dto.Quantity = 1;
            dto.BaggageNote = txtNoteBaggage.Text;

            // ===== Price =====
            if (_selectedSeatPrice > 0)
            {
                var selectedBaggage = cboBaggageTicket.SelectedItem as DTO.Baggage.CheckedBaggageDTO;
                var baggagePrice = selectedBaggage?.Price ?? 0;

                dto.TicketPrice = baggagePrice + _selectedSeatPrice;
            }
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
            if (_isRoundTrip && _inboundPassengers.Count != _outboundPassengers.Count)
            {
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin chiều về trước khi thanh toán.");
                return;
            }
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
            _outboundBooking = outbound;
            _returnBooking = inbound;
            _accountId = 2;

            LoadInfomationAccount(_accountId);

            if (outbound == null)
                return;

            // =====================================================
            // CHIỀU ĐI
            // =====================================================
            var (flightId, cabinClass, ticketCount, _) = outbound.GetBookingInfo();

            _flightId = flightId;          // Flight chiều đi
            _classId = cabinClass;         // Class chiều đi
            _ticketCount = ticketCount;    // Số lượng vé

            // =====================================================
            // CHIỀU VỀ (NẾU LÀ VÉ KHỨ HỒI)
            // =====================================================
            _isRoundTrip = inbound != null;

            if (_isRoundTrip)
            {
                var (reFlightId, reClassId, _, _) = inbound.GetBookingInfo();

                _returnFlightId = reFlightId;    // Flight chiều về
                _returnClassId = reClassId;      // Class chiều về
            }

            // Loại trip
            _typeTrip = _isRoundTrip ? "ROUND_TRIP" : "ONE_WAY";

            // Hiển thị ngày bay chiều đi
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
