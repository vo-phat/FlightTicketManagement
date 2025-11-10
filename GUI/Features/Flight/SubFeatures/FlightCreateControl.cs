using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS.Flight; // Thêm BUS
using DAO.Aircraft; // Thêm DAO mới
using DAO.Route;   // Thêm DAO mới
using DTO.Flight;  // Thêm DTO

namespace GUI.Features.Flight.SubFeatures
{
    public partial class FlightCreateControl : UserControl
    {
        private int _currentRouteDurationMinutes = 0;
        private int? _editingFlightId = null;
        public event Action OnSaveSuccess;
        public FlightCreateControl()
        {
            InitializeComponent();
            LoadInitialData();

            dtpDepartureTime.DateTimePicker.ValueChanged += dtpDepartureTime_ValueChanged;
        }

        private void FlightCreateControl_Load(object sender, EventArgs e)
        {
            dtpDepartureTime.DateTimePicker.Format = DateTimePickerFormat.Custom;
            dtpDepartureTime.DateTimePicker.CustomFormat = "dd/MM/yyyy HH:mm";

            dtpArrivalTime.DateTimePicker.Format = DateTimePickerFormat.Custom;
            dtpArrivalTime.DateTimePicker.CustomFormat = "dd/MM/yyyy HH:mm";
        }
        public void LoadFlightForEdit(int flightId)
        {
            _editingFlightId = flightId;
            lblTitle.Text = "✏️ Chỉnh sửa chuyến bay";
            btnSave.Text = "💾 Cập nhật chuyến bay";

            dtpDepartureTime.DateTimePicker.MinDate = new DateTime(2004, 1, 1);
            dtpArrivalTime.DateTimePicker.MinDate = new DateTime(2004, 1, 1);

            var result = FlightBUS.Instance.GetFlightById(flightId);
            if (!result.Success)
            {
                MessageBox.Show(result.GetFullErrorMessage(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearForm();
                return;
            }

            var flight = result.GetData<FlightDTO>();

            try
            {
                txtFlightNumber.Text = flight.FlightNumber;

                cbAircraft.SelectedValue = flight.AircraftId;
                cbRoute.SelectedValue = flight.RouteId;
                cbStatus.SelectedValue = flight.Status;

                if (flight.DepartureTime.HasValue)
                    dtpDepartureTime.Value = flight.DepartureTime.Value;

                if (flight.ArrivalTime.HasValue)
                    dtpArrivalTime.Value = flight.ArrivalTime.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi điền thông tin: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadInitialData()
        {
            try
            {
                cbAircraft.DataSource = AircraftDAO.Instance.GetAllAircraftForComboBox();
                cbAircraft.DisplayMember = "DisplayName";
                cbAircraft.ValueMember = "aircraft_id";
                cbAircraft.SelectedIndex = -1;
                cbAircraft.SelectedIndexChanged += cbAircraft_SelectedIndexChanged;

                cbRoute.DataSource = RouteDAO.Instance.GetAllRoutesForComboBox();
                cbRoute.DisplayMember = "DisplayName";
                cbRoute.ValueMember = "route_id";
                cbRoute.SelectedIndex = -1;
                cbRoute.SelectedIndexChanged += cbRoute_SelectedIndexChanged;

                var statusValues = Enum.GetValues(typeof(FlightStatus))
                    .Cast<FlightStatus>()
                    .Select(s => new { Value = s, Description = s.GetDescription() })
                    .ToList();

                cbStatus.DataSource = statusValues;
                cbStatus.DisplayMember = "Description";
                cbStatus.ValueMember = "Value";
                cbStatus.SelectedValue = FlightStatus.SCHEDULED;

                DateTime minDate = DateTime.Now.AddHours(1);
                DateTime defaultDate = DateTime.Now.AddDays(1);

                dtpDepartureTime.DateTimePicker.MinDate = minDate;
                dtpDepartureTime.Value = (defaultDate > minDate) ? defaultDate : minDate;

                dtpArrivalTime.DateTimePicker.MinDate = dtpDepartureTime.Value;
                dtpArrivalTime.Value = dtpDepartureTime.Value.AddHours(2);

                txtFlightNumber.LabelText = "Số hiệu chuyến bay";
                txtFlightNumber.PlaceholderText = "Chọn máy bay để gợi ý...";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cbRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentRouteDurationMinutes = 0;
            if (cbRoute.SelectedItem is DataRowView selectedRoute)
            {
                object durationObj = selectedRoute["duration_minutes"];
                if (durationObj != DBNull.Value && durationObj != null)
                {
                    _currentRouteDurationMinutes = Convert.ToInt32(durationObj);
                }
            }
            SuggestArrivalTime();
        }
        private void dtpDepartureTime_ValueChanged(object sender, EventArgs e)
        {
            dtpArrivalTime.DateTimePicker.MinDate = dtpDepartureTime.Value;

            SuggestArrivalTime();
        }
        private void SuggestArrivalTime()
        {
            if (_currentRouteDurationMinutes <= 0)
            {
                if (dtpArrivalTime.Value < dtpDepartureTime.Value)
                {
                    dtpArrivalTime.Value = dtpDepartureTime.Value.AddHours(1);
                }
                return;
            }

            try
            {
                DateTime departureTime = dtpDepartureTime.Value;

                DateTime suggestedArrivalTime = departureTime.AddMinutes(_currentRouteDurationMinutes);

                dtpArrivalTime.Value = suggestedArrivalTime;
            }
            catch (Exception)
            {
                dtpArrivalTime.Value = dtpDepartureTime.Value.AddHours(1);
            }
        }
        private void cbAircraft_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAircraft.SelectedItem is DataRowView selectedAircraft)
            {
                try
                {
                    string airlineCode = selectedAircraft["airline_code"]?.ToString();

                    if (!string.IsNullOrEmpty(airlineCode))
                    {
                        LoadSuggestedFlightNumber(airlineCode);
                    }
                }
                catch (ArgumentException)
                {
                    txtFlightNumber.PlaceholderText = "Lỗi (không tìm thấy code)";
                }
            }
            else
            {
                txtFlightNumber.PlaceholderText = "Chọn máy bay để gợi ý...";
            }
        }
        private void LoadSuggestedFlightNumber(string prefix)
        {
            var result = FlightBUS.Instance.SuggestNextFlightNumber(prefix);

            if (result.Success && result.Data != null)
            {
                txtFlightNumber.PlaceholderText = $"VD: {result.Data.ToString()}";
            }
            else
            {
                txtFlightNumber.PlaceholderText = $"VD: {prefix.ToUpper()}1";
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Thu thập dữ liệu
                var flight = new FlightDTO
                {
                    FlightNumber = txtFlightNumber.Text,
                    AircraftId = Convert.ToInt32(cbAircraft.SelectedValue),
                    RouteId = Convert.ToInt32(cbRoute.SelectedValue),
                    DepartureTime = dtpDepartureTime.Value,
                    ArrivalTime = dtpArrivalTime.Value,
                    Status = (FlightStatus)cbStatus.SelectedValue
                };

                // 2. Gọi BUS
                BUS.Common.BusinessResult result;
                if (_editingFlightId.HasValue)
                {
                    flight.FlightId = _editingFlightId.Value;
                    result = FlightBUS.Instance.UpdateFlight(flight);
                }
                else
                {
                    result = FlightBUS.Instance.CreateFlight(flight);
                }

                // 3. Hiển thị kết quả
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearForm();

                    OnSaveSuccess?.Invoke();
                }
                else
                {
                    MessageBox.Show(result.GetFullErrorMessage(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ Máy bay và Tuyến bay.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi không mong muốn: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ClearForm()
        {
            _editingFlightId = null;
            lblTitle.Text = "➕ Tạo chuyến bay";
            btnSave.Text = "💾 Lưu chuyến bay";

            txtFlightNumber.Text = "";
            cbAircraft.SelectedIndex = -1;
            cbRoute.SelectedIndex = -1;
            cbStatus.SelectedValue = FlightStatus.SCHEDULED;

            DateTime minDate = DateTime.Now.AddHours(1);
            DateTime defaultDate = DateTime.Now.AddDays(1);

            dtpDepartureTime.DateTimePicker.MinDate = minDate;
            dtpDepartureTime.Value = (defaultDate > minDate) ? defaultDate : minDate;

            dtpArrivalTime.DateTimePicker.MinDate = dtpDepartureTime.Value;
            dtpArrivalTime.Value = dtpDepartureTime.Value.AddHours(2);

            _currentRouteDurationMinutes = 0;
            txtFlightNumber.PlaceholderText = "Chọn máy bay để gợi ý...";
        }
    }
}