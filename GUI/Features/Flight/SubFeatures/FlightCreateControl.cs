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
        public FlightCreateControl()
        {
            InitializeComponent();
        }

        private void FlightCreateControl_Load(object sender, EventArgs e)
        {
            // Thiết lập định dạng cho DateTimePicker
            // (Chúng ta truy cập control gốc bên trong UserControl)
            dtpDepartureTime.DateTimePicker.Format = DateTimePickerFormat.Custom;
            dtpDepartureTime.DateTimePicker.CustomFormat = "dd/MM/yyyy HH:mm";

            dtpArrivalTime.DateTimePicker.Format = DateTimePickerFormat.Custom;
            dtpArrivalTime.DateTimePicker.CustomFormat = "dd/MM/yyyy HH:mm";

            // Tải dữ liệu cho các ComboBox
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            try
            {
                // Tải danh sách Máy bay
                cbAircraft.DataSource = AircraftDAO.Instance.GetAllAircraftForComboBox();
                cbAircraft.DisplayMember = "DisplayName";
                cbAircraft.ValueMember = "aircraft_id";
                cbAircraft.SelectedIndex = -1;

                // Tải danh sách Tuyến bay
                cbRoute.DataSource = RouteDAO.Instance.GetAllRoutesForComboBox();
                cbRoute.DisplayMember = "DisplayName";
                cbRoute.ValueMember = "route_id";
                cbRoute.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Thu thập dữ liệu từ Form
                var flight = new FlightDTO
                {
                    FlightNumber = txtFlightNumber.Text,
                    AircraftId = Convert.ToInt32(cbAircraft.SelectedValue),
                    RouteId = Convert.ToInt32(cbRoute.SelectedValue),
                    DepartureTime = dtpDepartureTime.Value,
                    ArrivalTime = dtpArrivalTime.Value,
                    Status = FlightStatus.SCHEDULED // Mặc định khi tạo mới
                };

                // 2. Gọi BUS để thực thi
                var result = FlightBUS.Instance.CreateFlight(flight);

                // 3. Hiển thị kết quả
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Tùy chọn: Xóa form để nhập mới
                    ClearForm();
                    // Tùy chọn: Cập nhật bảng preview (nếu có)
                    // LoadPreviewTable(); 
                }
                else
                {
                    // Hiển thị lỗi (bao gồm cả lỗi validation và lỗi nghiệp vụ)
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

        private void ClearForm()
        {
            txtFlightNumber.Text = "";
            cbAircraft.SelectedIndex = -1;
            cbRoute.SelectedIndex = -1;
            dtpDepartureTime.Value = DateTime.Now.AddDays(1);
            dtpArrivalTime.Value = DateTime.Now.AddDays(1).AddHours(2);
        }
    }
}