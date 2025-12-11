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

        // trạng thái sửa
        private int _editingIndex = -1;      // -1 = thêm mới; >= 0 = đang sửa
        private bool _isEditingInbound = false;

        // dữ liệu ticket
        private int _ticketCount;
        private int _accountId;

        // danh sách passengers
        private readonly BindingList<TicketBookingRequestDTO> _outboundPassengers = new();
        private readonly BindingList<TicketBookingRequestDTO> _inboundPassengers = new();

        // ghế đang chọn
        private int _selectedSeatId;
        private int _selectedFlightSeatId;
        private decimal _selectedSeatPrice;

        // chuyến bay chiều đi
        private int _classId;
        private int _flightId;
        private BookingRequestDTO bookingRequest;

        // chuyến bay round-trip
        private BookingRequestDTO _outboundBooking;
        private BookingRequestDTO _returnBooking;
        private bool _isRoundTrip = false;
        private int _returnFlightId;
        private int _returnClassId;

        public frmPassengerInfoControl()
        {
            InitializeComponent();
            InitGrid();
            LoadCheckBaggage();
            LoadNationality();
            carryOnList = _carryOnBaggageBUS.GetAll();
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

            dgvPassengerListTicket.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colEditOutbound",
                HeaderText = "Sửa đi",
                Text = "Sửa đi",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            // Các cột ẩn
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

            _bs.DataSource = _outboundPassengers;
            dgvPassengerListTicket.DataSource = _bs;
            dgvPassengerListTicket.CellContentClick += dgvPassengerListTicket_CellContentClick;
        }

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

                Load += (s, e) =>
                {
                    Selector.LoadSeats(flightId, classId);
                };
            }
        }

        private void dgvPassengerListTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string col = dgvPassengerListTicket.Columns[e.ColumnIndex].Name;

            if (col == "colEditOutbound")
            {
                var dto = _outboundPassengers[e.RowIndex];
                LoadOutboundToForm(dto);
                return;
            }

            if (col == "colEditInbound")
            {
                if (!_isRoundTrip)
                {
                    MessageBox.Show("Không phải vé khứ hồi!");
                    return;
                }

                if (e.RowIndex >= _inboundPassengers.Count)
                {
                    MessageBox.Show("Hành khách này chưa nhập chiều về!");
                    return;
                }

                var dto = _inboundPassengers[e.RowIndex];
                LoadInboundToForm(dto);
                return;
            }
        }

        private void btnSelectSeatTicket_Click(object sender, EventArgs e)
        {
            int targetFlightId = _isEditingInbound ? _returnFlightId : _flightId;
            int targetClassId = _isEditingInbound ? _returnClassId : _classId;

            var form = new SeatSelectorForm(targetFlightId, targetClassId);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var seat = form.Selector.GetSelectedSeat();
                if (seat != null)
                {
                    txtSeatTicket.Text = seat.SeatNumber;
                    _selectedFlightSeatId = seat.FlightSeatId;
                    _selectedSeatId = seat.SeatId;
                    _selectedSeatPrice = seat.Price; // ✅ Đã có giá từ selector

                    // ⭐ Hiển thị giá ghế ngay sau khi chọn
                    UpdatePriceDisplay();
                }
            }
        }

        // ===== HELPERS CẢI TIẾN =====

        /// <summary>
        /// Load RIÊNG thông tin cá nhân (6 trường cơ bản)
        /// </summary>
        private void LoadPersonalInfoToForm(TicketBookingRequestDTO dto)
        {
            txtFullNameTicket.Text = dto.FullName ?? "";
            dtpDateOfBirthTicket.Value = dto.DateOfBirth ?? DateTime.Now;
            txtPhoneNumberTicket.Text = dto.PhoneNumber ?? "";
            txtPassportNumberTicket.Text = dto.PassportNumber ?? "";
            cboNationalityTicket.Text = dto.Nationality ?? "VN";
            txtEmailTicket.Text = dto.Email ?? "";
        }

        /// <summary>
        /// Reset CHỈ ghế và hành lý (không động đến thông tin cá nhân)
        /// </summary>
        private void ResetSeatAndBaggage()
        {
            txtSeatTicket.Text = "";
            _selectedSeatId = 0;
            _selectedFlightSeatId = 0;
            _selectedSeatPrice = 0;
            cboBaggageTicket.SelectedIndex = -1;
            txtNoteBaggage.Text = "";
        }

        private void MapFormToDto(TicketBookingRequestDTO dto, bool isInbound)
        {
            // ===== 1. THÔNG TIN CƠ BẢN =====
            dto.FullName = txtFullNameTicket.Text;
            dto.DateOfBirth = dtpDateOfBirthTicket.Value;
            dto.PhoneNumber = txtPhoneNumberTicket.Text;
            dto.PassportNumber = txtPassportNumberTicket.Text;
            dto.Nationality = cboNationalityTicket.SelectedValue?.ToString();
            dto.Email = txtEmailTicket.Text;

            // ===== 2. GHẾ =====
            if (_selectedFlightSeatId != 0)
            {
                dto.SeatId = _selectedSeatId;
                dto.FlightSeatId = _selectedFlightSeatId;
                dto.SeatNumber = txtSeatTicket.Text;
            }

            // ===== 3. CARRY-ON =====
            int classId = isInbound ? _returnClassId : _classId;
            dto.CarryOnId = CarryBaggageId(classId);

            // ===== 4. CHECKED BAGGAGE =====
            if (cboBaggageTicket.SelectedItem is CheckedBaggageDTO cb)
            {
                dto.CheckedId = cb.CheckedId;
                dto.BaggageDisplayText = cb.Description;
            }
            dto.BaggageNote = txtNoteBaggage.Text;

            // ===== 5. PRICE =====
            decimal baggagePrice = (cboBaggageTicket.SelectedItem as CheckedBaggageDTO)?.Price ?? 0;
            dto.TicketPrice = _selectedSeatPrice + baggagePrice;
        }

        /// <summary>
        /// Load form để nhập chiều về - GIỮ NGUYÊN thông tin cá nhân
        /// </summary>
        private void LoadInboundToForm(TicketBookingRequestDTO dto)
        {
            _isEditingInbound = true;
            _editingIndex = _inboundPassengers.IndexOf(dto);

            // ===== 1. LOAD THÔNG TIN CÁ NHÂN (KHÔNG BỊ MẤT) =====
            LoadPersonalInfoToForm(dto);

            // ===== 2. GHẾ VÀ HÀNH LÝ =====
            // Nếu DTO đã có ghế/hành lý → LOAD LẠI (đang SỬA)
            // Nếu chưa có → RESET (lần đầu nhập)
            if (!string.IsNullOrEmpty(dto.SeatNumber) && dto.FlightSeatId > 0)
            {
                // ⭐ LOAD LẠI ghế đã chọn - GIÁ ĐÃ CÓ TRONG DTO
                txtSeatTicket.Text = dto.SeatNumber;
                _selectedSeatId = dto.SeatId ?? 0;
                _selectedFlightSeatId = dto.FlightSeatId;

                // ⭐ Tính lại giá ghế từ TicketPrice - baggagePrice
                decimal baggagePrice = 0;
                if (dto.CheckedId.HasValue && dto.CheckedId > 0)
                {
                    var baggage = (cboBaggageTicket.DataSource as System.Collections.IList)
                        ?.Cast<CheckedBaggageDTO>()
                        .FirstOrDefault(b => b.CheckedId == dto.CheckedId.Value);

                    if (baggage != null)
                    {
                        baggagePrice = baggage.Price;
                    }
                }

                _selectedSeatPrice = (dto.TicketPrice ?? 0) - baggagePrice;
            }
            else
            {
                // Reset ghế (lần đầu nhập)
                txtSeatTicket.Text = "";
                _selectedSeatId = 0;
                _selectedFlightSeatId = 0;
                _selectedSeatPrice = 0;
            }

            // Load lại hành lý nếu đã chọn
            if (dto.CheckedId.HasValue && dto.CheckedId > 0)
            {
                cboBaggageTicket.SelectedValue = dto.CheckedId;
            }
            else
            {
                cboBaggageTicket.SelectedIndex = -1;
            }

            txtNoteBaggage.Text = dto.BaggageNote ?? "";

            // ===== 3. NGÀY BAY CHIỀU VỀ =====
            if (_returnBooking != null)
            {
                dtpFlightDateTicket.Value = _returnBooking.DepartureTime ?? DateTime.Now;
            }

            // ===== 4. ẨN NÚT CHIỀU ĐI, HIỆN NÚT CHIỀU VỀ =====
            btnAddOutbound.Visible = false;
            btnAddInbound.Visible = true;
            btnAddInbound.Text = "💾 Lưu chiều về";
        }

        /// <summary>
        /// Load form để sửa chiều đi - Load ĐẦY ĐỦ thông tin
        /// </summary>
        private void LoadOutboundToForm(TicketBookingRequestDTO dto)
        {
            _isEditingInbound = false;
            _editingIndex = _outboundPassengers.IndexOf(dto);

            // ===== LOAD ĐẦY ĐỦ (bao gồm cả ghế/hành lý) =====
            LoadPersonalInfoToForm(dto);

            txtSeatTicket.Text = dto.SeatNumber ?? "";
            _selectedSeatId = dto.SeatId ?? 0;
            _selectedFlightSeatId = dto.FlightSeatId;

            // ⭐ Tính lại giá ghế từ TicketPrice - baggagePrice
            decimal baggagePrice = 0;
            if (dto.CheckedId.HasValue && dto.CheckedId > 0)
            {
                var baggage = (cboBaggageTicket.DataSource as System.Collections.IList)
                    ?.Cast<CheckedBaggageDTO>()
                    .FirstOrDefault(b => b.CheckedId == dto.CheckedId.Value);

                if (baggage != null)
                {
                    baggagePrice = baggage.Price;
                }

                cboBaggageTicket.SelectedValue = dto.CheckedId;
            }

            _selectedSeatPrice = (dto.TicketPrice ?? 0) - baggagePrice;
            txtNoteBaggage.Text = dto.BaggageNote ?? "";

            if (_outboundBooking != null)
            {
                dtpFlightDateTicket.Value = _outboundBooking.DepartureTime ?? DateTime.Now;
            }

            // ===== HIỆN NÚT CHIỀU ĐI, ẨN NÚT CHIỀU VỀ =====
            btnAddOutbound.Visible = true;
            btnAddInbound.Visible = false;
            btnAddOutbound.Text = "💾 Cập nhật chiều đi";
        }

        private int CarryBaggageId(int classId)
        {
            var result = carryOnList.FirstOrDefault(c => c.ClassId == classId);
            return result?.CarryOnId ?? 0;
        }

        /// <summary>
        /// Hiển thị giá vé tạm tính (real-time khi chọn ghế/hành lý)
        /// </summary>
        private void UpdatePriceDisplay()
        {
            decimal seatPrice = _selectedSeatPrice;
            decimal baggagePrice = 0;

            if (cboBaggageTicket.SelectedItem is CheckedBaggageDTO baggage)
            {
                baggagePrice = baggage.Price;
            }

            decimal totalPrice = seatPrice + baggagePrice;

            // Cập nhật label hoặc textbox hiển thị giá (nếu có)
            // Ví dụ: lblTotalPrice.Text = $"Tạm tính: {totalPrice:N0} VNĐ";
        }

        /// <summary>
        /// Tính tổng giá vé hiện tại (ghế + hành lý)
        /// </summary>
        private decimal CalculateTicketPrice()
        {
            decimal seatPrice = _selectedSeatPrice;
            decimal baggagePrice = 0;

            if (cboBaggageTicket.SelectedItem is CheckedBaggageDTO baggage)
            {
                baggagePrice = baggage.Price;
            }

            return seatPrice + baggagePrice;
        }

        /// <summary>
        /// Hiển thị chi tiết giá vé trước khi lưu
        /// </summary>
        private bool ShowPriceConfirmation(bool isInbound)
        {
            decimal seatPrice = _selectedSeatPrice;
            decimal baggagePrice = 0;
            string baggageDesc = "Không chọn";

            if (cboBaggageTicket.SelectedItem is CheckedBaggageDTO baggage)
            {
                baggagePrice = baggage.Price;
                baggageDesc = baggage.Description;
            }

            decimal totalPrice = seatPrice + baggagePrice;
            string tripType = isInbound ? "CHIỀU VỀ" : "CHIỀU ĐI";

            string message = $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                           $"📋 XÁC NHẬN GIÁ VÉ - {tripType}\n" +
                           $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                           $"👤 Hành khách: {txtFullNameTicket.Text}\n" +
                           $"💺 Ghế: {txtSeatTicket.Text}\n\n" +
                           $"💰 CHI TIẾT GIÁ:\n" +
                           $"   • Giá ghế:        {seatPrice:N0} VNĐ\n" +
                           $"   • Hành lý:        {baggageDesc}\n" +
                           $"   • Phí hành lý:    {baggagePrice:N0} VNĐ\n" +
                           $"   ─────────────────────────────\n" +
                           $"   • TỔNG CỘNG:      {totalPrice:N0} VNĐ\n\n" +
                           $"Xác nhận lưu thông tin?";

            DialogResult result = MessageBox.Show(
                message,
                "💳 Xác nhận giá vé",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information
            );

            return result == DialogResult.Yes;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtFullNameTicket.Text))
            {
                MessageBox.Show("Vui lòng nhập họ và tên.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSeatTicket.Text))
            {
                MessageBox.Show("Vui lòng chọn ghế.");
                return false;
            }

            if (_selectedFlightSeatId == 0)
            {
                MessageBox.Show("Ghế chưa hợp lệ. Vui lòng chọn lại ghế.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clear form hoàn toàn (dùng khi chuyển hành khách mới)
        /// </summary>
        private void ClearForm()
        {
            txtFullNameTicket.Text = "";
            txtPhoneNumberTicket.Text = "";
            txtEmailTicket.Text = "";
            txtPassportNumberTicket.Text = "";
            cboNationalityTicket.Text = "VN";
            dtpDateOfBirthTicket.Value = DateTime.Today;

            ResetSeatAndBaggage();

            // ===== RESET TRẠNG THÁI NÚT =====
            btnAddOutbound.Text = "➕ Thêm chiều đi";
            btnAddInbound.Text = "➕ Thêm chiều về";

            // ===== HIỂN THỊ LẠI CẢ 2 NÚT (nếu round-trip) =====
            btnAddOutbound.Visible = true;
            if (_isRoundTrip)
            {
                btnAddInbound.Visible = true;
            }

            _isEditingInbound = false;
            _editingIndex = -1;
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
            _flightId = flightId;
            _classId = cabinClass;
            _ticketCount = ticketCount;

            // =====================================================
            // CHIỀU VỀ (NẾU LÀ VÉ KHỨ HỒI)
            // =====================================================
            _isRoundTrip = inbound != null;
            if (_isRoundTrip)
            {
                var (reFlightId, reClassId, _, _) = inbound.GetBookingInfo();
                _returnFlightId = reFlightId;
                _returnClassId = reClassId;
            }

            dtpFlightDateTicket.Value = outbound.DepartureTime ?? DateTime.Now;

            // ===== HIỂN THỊ NÚT PHÙ HỢP =====
            btnAddOutbound.Visible = true;
            btnAddInbound.Visible = _isRoundTrip;

            // Thêm cột sửa chiều về nếu là round-trip
            if (_isRoundTrip)
            {
                if (!dgvPassengerListTicket.Columns.Contains("colEditInbound"))
                {
                    dgvPassengerListTicket.Columns.Add(new DataGridViewButtonColumn
                    {
                        Name = "colEditInbound",
                        HeaderText = "Sửa về",
                        Text = "Sửa về",
                        UseColumnTextForButtonValue = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    });
                }
            }
        }

        private void btnAddOutbound_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            // ⭐ HIỂN THỊ XÁC NHẬN GIÁ VÉ
            if (!ShowPriceConfirmation(false))
            {
                return; // Người dùng chọn No → không lưu
            }

            // ============ EDIT OUTBOUND ===============
            if (_editingIndex >= 0 && !_isEditingInbound)
            {
                var outbound = _outboundPassengers[_editingIndex];
                MapFormToDto(outbound, false);
                outbound.FlightId = _flightId;
                outbound.ClassId = _classId;
                outbound.FlightDate = _outboundBooking.DepartureTime;

                // Nếu là round-trip → CHỈ sync thông tin cơ bản sang inbound
                if (_isRoundTrip)
                {
                    var inbound = _inboundPassengers[_editingIndex];
                    inbound.FullName = outbound.FullName;
                    inbound.DateOfBirth = outbound.DateOfBirth;
                    inbound.PhoneNumber = outbound.PhoneNumber;
                    inbound.PassportNumber = outbound.PassportNumber;
                    inbound.Nationality = outbound.Nationality;
                    inbound.Email = outbound.Email;
                    _inboundPassengers.ResetItem(_editingIndex);
                }

                _outboundPassengers.ResetItem(_editingIndex);
                _editingIndex = -1;
                btnAddOutbound.Text = "➕ Thêm chiều đi";
                btnAddOutbound.Visible = true;
                btnAddInbound.Visible = _isRoundTrip;
                ClearForm();
                return;
            }

            // ============ ADD OUTBOUND ===============
            if (_outboundPassengers.Count >= _ticketCount)
            {
                MessageBox.Show("Đã đủ số lượng hành khách chiều đi.");
                return;
            }

            var newOutbound = new TicketBookingRequestDTO();
            MapFormToDto(newOutbound, false);
            newOutbound.FlightId = _flightId;
            newOutbound.ClassId = _classId;
            newOutbound.FlightDate = _outboundBooking.DepartureTime;
            _outboundPassengers.Add(newOutbound);

            // ===== Khi là round-trip — tạo inbound CLONE =====
            if (_isRoundTrip)
            {
                var clone = new TicketBookingRequestDTO
                {
                    FullName = newOutbound.FullName,
                    DateOfBirth = newOutbound.DateOfBirth,
                    PhoneNumber = newOutbound.PhoneNumber,
                    PassportNumber = newOutbound.PassportNumber,
                    Nationality = newOutbound.Nationality,
                    Email = newOutbound.Email,
                    Quantity = 1
                };
                _inboundPassengers.Add(clone);

                // ⭐ NGAY LẬP TỨC đưa form vào chế độ nhập chiều về
                LoadInboundToForm(clone);
            }
            else
            {
                ClearForm();
            }
        }

        private void btnAddInbound_Click(object sender, EventArgs e)
        {
            if (!_isRoundTrip)
            {
                MessageBox.Show("Đây không phải vé khứ hồi.");
                return;
            }
            if (!ValidateForm()) return;

            // ⭐ HIỂN THỊ XÁC NHẬN GIÁ VÉ
            if (!ShowPriceConfirmation(true))
            {
                return; // Người dùng chọn No → không lưu
            }

            // ============ EDIT INBOUND ===============
            if (_editingIndex >= 0 && _isEditingInbound)
            {
                var inbound = _inboundPassengers[_editingIndex];
                MapFormToDto(inbound, true);
                inbound.FlightId = _returnFlightId;
                inbound.ClassId = _returnClassId;
                inbound.FlightDate = _returnBooking.DepartureTime;
                _inboundPassengers.ResetItem(_editingIndex);

                // ⭐ TỰ ĐỘNG chuyển sang hành khách tiếp theo
                int nextIndex = _editingIndex + 1;
                if (nextIndex < _inboundPassengers.Count)
                {
                    LoadInboundToForm(_inboundPassengers[nextIndex]);
                }
                else
                {
                    // ===== HẾT HÀNH KHÁCH → RESET =====
                    MessageBox.Show("✅ Hoàn tất nhập thông tin tất cả hành khách!");
                    _editingIndex = -1;
                    _isEditingInbound = false;
                    btnAddOutbound.Visible = true;
                    btnAddInbound.Visible = true;
                    ClearForm();
                }
                return;
            }

            // ============ ADD INBOUND (không bao giờ xảy ra với logic mới) ===============
            if (_inboundPassengers.Count >= _outboundPassengers.Count)
            {
                MessageBox.Show("Hãy nhập chiều đi trước.");
                return;
            }

            var newInbound = new TicketBookingRequestDTO();
            MapFormToDto(newInbound, true);
            newInbound.FlightId = _returnFlightId;
            newInbound.ClassId = _returnClassId;
            newInbound.FlightDate = _returnBooking.DepartureTime;
            _inboundPassengers.Add(newInbound);
            ClearForm();
        }
    }
}