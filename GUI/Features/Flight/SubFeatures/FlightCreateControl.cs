using DAO.CabinClass;
using DAO.Flight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAO.Airport;

namespace GUI.Features.Flight.SubFeatures
{
    public partial class FlightCreateControl : UserControl
    {
        private DataTable _airportData;
        public FlightCreateControl()
        {
            InitializeComponent();
        }
        #region Data Loading
        private void LoadFlightData()
        {
            try
            {
                // Tải danh sách chuyến bay (đã JOIN)
                DataTable dtFlights = FlightDAO.Instance.GetFlightDetailsForDisplay();
                danhSachChuyenBay.DataSource = dtFlights;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chuyến bay: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FlightCreateControl_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;

            danhSachChuyenBay.AutoGenerateColumns = false;

            LoadInitialData();

            LoadFlightData();
        }
        private void timChuyenBay_Click(object sender, EventArgs e) 
        {
            // TODO: Bổ sung logic lọc (filter) vào câu SQL
            // Hiện tại, nó chỉ tải lại toàn bộ danh sách
            LoadFlightData();
        }
        private void LoadInitialData()
        {
            try
            {
                // 1. Tải và gán danh sách Hạng vé (như cũ)
                DataTable dtCabinClasses = CabinClassDAO.Instance.GetAllCabinClasses();
                if (danhSachChuyenBay.Columns["Column7"] is DataGridViewComboBoxColumn cbColumn)
                {
                    cbColumn.DataSource = dtCabinClasses;
                    cbColumn.DisplayMember = "class_name";
                    cbColumn.ValueMember = "class_id";
                }

                // 2. Tải danh sách Sân bay
                _airportData = AirportDAO.Instance.GetAllAirportsForComboBox();

                // 3. Gán dữ liệu cho ComboBox "Nơi cất cánh"
                // QUAN TRỌNG: Dùng .Copy() để hai ComboBox hoạt động độc lập
                noiCatCanh.DataSource = _airportData.Copy();
                noiCatCanh.DisplayMember = "DisplayName"; // Cột hiển thị
                noiCatCanh.ValueMember = "airport_id";   // Giá trị thực (ẩn)

                // 4. Gán dữ liệu cho ComboBox "Nơi hạ cánh"
                noiHaCanh.DataSource = _airportData.Copy();
                noiHaCanh.DisplayMember = "DisplayName";
                noiHaCanh.ValueMember = "airport_id";

                // 5. Xóa lựa chọn mặc định
                noiCatCanh.SelectedIndex = -1;
                noiHaCanh.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu ban đầu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
        private void dateTimeNgayDi_Load(object sender, EventArgs e)
        {

        }

        private void dateTimeNgayVe_Load(object sender, EventArgs e)
        {

        }

        private void noiCatCanh_Load(object sender, EventArgs e)
        {

        }

        private void noiHaCanh_Load(object sender, EventArgs e)
        {

        }

        //private void timChuyenBay_Click(object sender, EventArgs e)
        //{

        //}

        private void danhSachChuyenBay_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //private void FlightCreateControl_Load(object sender, EventArgs e)
        //{
        //    this.Dock = DockStyle.Fill;
        //}
    }
}
