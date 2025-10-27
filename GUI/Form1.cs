using DAO.Database;
using DAO.Flight;
using DTO.Flight;
using System;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Hiển thị welcome message
            txtResult.Text = "=== FLIGHT TICKET MANAGEMENT - TEST CONSOLE ===\r\n\r\n" +
                           "Nhấn các nút bên trái để chạy test:\r\n" +
                           "1. Test Database Connection\r\n" +
                           "2. Test Flight DTO\r\n" +
                           "3. Test Base DAO\r\n\r\n" +
                           "Kết quả sẽ hiển thị ở đây...";
        }

        #region Test 1: Database Connection

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                txtResult.Clear();
                txtResult.Text = "Đang test kết nối database...\r\n\r\n";
                this.Refresh();

                StringBuilder result = new StringBuilder();
                result.AppendLine("=== TEST 1: DATABASE CONNECTION ===");
                result.AppendLine();

                // Test connection
                if (DatabaseConnection.TestConnection())
                {
                    result.AppendLine("Kết nối database THÀNH CÔNG!");
                    result.AppendLine();
                    result.AppendLine($"Server: localhost:3306");
                    result.AppendLine($"Database: flightticketmanagement");
                    result.AppendLine($"MySQL Version: {DatabaseConnection.GetServerVersion()}");
                    result.AppendLine($"Connection String: {DatabaseConnection.ConnectionString}");
                }
                else
                {
                    result.AppendLine("KHÔNG THỂ KẾT NỐI DATABASE!");
                    result.AppendLine();
                    result.AppendLine("Kiểm tra:");
                    result.AppendLine("- XAMPP MySQL đã chạy chưa?");
                    result.AppendLine("- Database 'flightticketmanagement' đã tạo chưa?");
                    result.AppendLine("- Port 3306 có bị chiếm không?");
                }

                txtResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = $"LỖI:\r\n\r\n{ex.Message}\r\n\r\nStack Trace:\r\n{ex.StackTrace}";
            }
        }

        #endregion

        #region Test 2: Flight DTO

        private void btnTestDTO_Click(object sender, EventArgs e)
        {
            try
            {
                txtResult.Clear();
                txtResult.Text = "Đang test Flight DTO...\r\n\r\n";
                this.Refresh();

                StringBuilder result = new StringBuilder();
                result.AppendLine("=== TEST 2: FLIGHT DTO ===");
                result.AppendLine();

                // Test 2.1: Tạo DTO hợp lệ
                result.AppendLine("Test 2.1: Tạo Flight DTO hợp lệ");
                result.AppendLine("---");
                var flight = new FlightDTO(
                    "VN123",
                    1, // aircraft_id
                    1, // route_id
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(1).AddHours(2)
                );

                if (flight.IsValid(out string error))
                {
                    result.AppendLine($"DTO hợp lệ!");
                    result.AppendLine($"Flight Number: {flight.FlightNumber}");
                    result.AppendLine($"Aircraft ID: {flight.AircraftId}");
                    result.AppendLine($"Route ID: {flight.RouteId}");
                    result.AppendLine($"Departure: {flight.DepartureTime:dd/MM/yyyy HH:mm}");
                    result.AppendLine($"Arrival: {flight.ArrivalTime:dd/MM/yyyy HH:mm}");
                    result.AppendLine($"Duration: {flight.GetFlightDuration()?.TotalHours:F2} hours");
                    result.AppendLine($"Status: {flight.Status.GetDescription()}");
                    result.AppendLine($"ToString: {flight.ToString()}");
                }
                else
                {
                    result.AppendLine($"DTO không hợp lệ: {error}");
                }

                result.AppendLine();
                result.AppendLine();

                // Test 2.2: Test validation lỗi
                result.AppendLine("Test 2.2: Test Validation (DTO không hợp lệ)");
                result.AppendLine("---");
                var invalidFlight = new FlightDTO();
                if (!invalidFlight.IsValid(out string validationError))
                {
                    result.AppendLine($"Validation hoạt động đúng!");
                    result.AppendLine($"Lỗi bắt được: {validationError}");
                }
                else
                {
                    result.AppendLine($"Validation không hoạt động!");
                }

                result.AppendLine();
                result.AppendLine();

                // Test 2.3: Test exception khi thời gian không hợp lệ
                result.AppendLine("Test 2.3: Test Exception (Thời gian đến trước khởi hành)");
                result.AppendLine("---");
                try
                {
                    var badFlight = new FlightDTO(
                        "VN456",
                        1,
                        1,
                        DateTime.Now.AddDays(1).AddHours(2),
                        DateTime.Now.AddDays(1)
                    );
                    result.AppendLine($"Exception KHÔNG được throw!");
                }
                catch (ArgumentException ex)
                {
                    result.AppendLine($"Exception được throw đúng!");
                    result.AppendLine($"Message: {ex.Message}");
                }

                result.AppendLine();
                result.AppendLine();

                // Test 2.4: Test FlightStatus enum
                result.AppendLine("Test 2.4: Test FlightStatus Enum");
                result.AppendLine("---");
                result.AppendLine($"SCHEDULED: {FlightStatus.SCHEDULED.GetDescription()}");
                result.AppendLine($"DELAYED: {FlightStatus.DELAYED.GetDescription()}");
                result.AppendLine($"CANCELLED: {FlightStatus.CANCELLED.GetDescription()}");
                result.AppendLine($"COMPLETED: {FlightStatus.COMPLETED.GetDescription()}");
                result.AppendLine();
                result.AppendLine("Test chuyển đổi trạng thái:");
                result.AppendLine($"SCHEDULED → DELAYED: {FlightStatus.SCHEDULED.CanTransitionTo(FlightStatus.DELAYED)} (Expected: True)");
                result.AppendLine($"CANCELLED → SCHEDULED: {FlightStatus.CANCELLED.CanTransitionTo(FlightStatus.SCHEDULED)} (Expected: False)");

                txtResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = $" LỖI:\r\n\r\n{ex.Message}\r\n\r\nStack Trace:\r\n{ex.StackTrace}";
            }
        }

        #endregion

        #region Test 3: Base DAO

        private void btnTestBaseDAO_Click(object sender, EventArgs e)
        {
            try
            {
                txtResult.Clear();
                txtResult.Text = "Đang test Base DAO...\r\n\r\n";
                this.Refresh();

                var test = new BaseDAOTest();
                string result = test.TestAll();

                txtResult.Text = result;
            }
            catch (Exception ex)
            {
                txtResult.Text = $" LỖI:\r\n\r\n{ex.Message}\r\n\r\nStack Trace:\r\n{ex.StackTrace}";
            }
        }

        #endregion

        #region Test 4: Flight DAO
        private void btnTestFlightDAO_Click(object sender, EventArgs e)
        {
            try
            {
                txtResult.Clear();
                txtResult.Text = "Đang test Flight DAO...\r\n\r\n";
                this.Refresh();

                StringBuilder result = new StringBuilder();
                result.AppendLine("=== TEST 4: FLIGHT DAO ===");
                result.AppendLine();

                // Test 1: GetAll
                result.AppendLine("Test 4.1: GetAll() - Lấy tất cả chuyến bay");
                result.AppendLine("---");
                var allFlights = FlightDAO.Instance.GetAll();
                result.AppendLine($"Tìm thấy {allFlights.Count} chuyến bay");
                if (allFlights.Count > 0)
                {
                    result.AppendLine("\nCác chuyến bay đầu tiên:");
                    for (int i = 0; i < Math.Min(5, allFlights.Count); i++)
                    {
                        var f = allFlights[i];
                        result.AppendLine($"  {i + 1}. {f.FlightNumber} - {f.Status.GetDescription()}");
                    }
                }
                result.AppendLine();
                result.AppendLine();

                // Test 2: Count
                result.AppendLine("Test 4.2: CountAll() & CountByStatus()");
                result.AppendLine("---");
                int totalFlights = FlightDAO.Instance.CountAll();
                result.AppendLine($"Tổng số chuyến bay: {totalFlights}");
                result.AppendLine($"   - SCHEDULED: {FlightDAO.Instance.CountByStatus(FlightStatus.SCHEDULED)}");
                result.AppendLine($"   - DELAYED: {FlightDAO.Instance.CountByStatus(FlightStatus.DELAYED)}");
                result.AppendLine($"   - CANCELLED: {FlightDAO.Instance.CountByStatus(FlightStatus.CANCELLED)}");
                result.AppendLine($"   - COMPLETED: {FlightDAO.Instance.CountByStatus(FlightStatus.COMPLETED)}");
                result.AppendLine();
                result.AppendLine();

                // Test 3: Insert (nếu có dữ liệu aircraft và route)
                result.AppendLine("Test 4.3: Insert() - Thêm chuyến bay mới");
                result.AppendLine("---");
                try
                {
                    var newFlight = new FlightDTO(
                        "TEST" + DateTime.Now.ToString("HHmmss"),
                        1, // aircraft_id (giả sử có)
                        1, // route_id (giả sử có)
                        DateTime.Now.AddDays(7),
                        DateTime.Now.AddDays(7).AddHours(2)
                    );

                    long newId = FlightDAO.Instance.Insert(newFlight);
                    result.AppendLine($"Thêm thành công! New ID: {newId}");
                    result.AppendLine($"   Flight Number: {newFlight.FlightNumber}");

                    // Test 4: GetById
                    result.AppendLine();
                    result.AppendLine("Test 4.4: GetById() - Lấy chuyến bay vừa thêm");
                    result.AppendLine("---");
                    var retrievedFlight = FlightDAO.Instance.GetById((int)newId);
                    if (retrievedFlight != null)
                    {
                        result.AppendLine($"Tìm thấy: {retrievedFlight.ToString()}");
                    }

                    // Test 5: Update
                    result.AppendLine();
                    result.AppendLine("Test 4.5: Update() - Cập nhật chuyến bay");
                    result.AppendLine("---");
                    retrievedFlight.Status = FlightStatus.DELAYED;
                    bool updated = FlightDAO.Instance.Update(retrievedFlight);
                    result.AppendLine($"Cập nhật: {(updated ? "Thành công" : "Thất bại")}");

                    // Test 6: UpdateStatus
                    result.AppendLine();
                    result.AppendLine("Test 4.6: UpdateStatus() - Đổi trạng thái");
                    result.AppendLine("---");
                    bool statusUpdated = FlightDAO.Instance.UpdateStatus((int)newId, FlightStatus.COMPLETED);
                    result.AppendLine($"Đổi trạng thái: {(statusUpdated ? "Thành công" : "Thất bại")}");

                    // Test 7: Delete
                    result.AppendLine();
                    result.AppendLine("Test 4.7: Delete() - Xóa chuyến bay test");
                    result.AppendLine("---");
                    bool deleted = FlightDAO.Instance.Delete((int)newId);
                    result.AppendLine($"Xóa: {(deleted ? "Thành công" : "Thất bại")}");
                }
                catch (Exception ex)
                {
                    result.AppendLine($"Test Insert/Update/Delete bỏ qua do: {ex.Message}");
                    result.AppendLine("   (Có thể chưa có dữ liệu Aircraft hoặc Route trong DB)");
                }

                result.AppendLine();
                result.AppendLine();

                // Test 8: Search
                result.AppendLine("Test 4.8: Search Methods");
                result.AppendLine("---");

                // Search by flight number
                if (allFlights.Count > 0)
                {
                    string searchTerm = allFlights[0].FlightNumber.Substring(0, 2);
                    var searchResults = FlightDAO.Instance.SearchByFlightNumber(searchTerm);
                    result.AppendLine($"Tìm kiếm '{searchTerm}': {searchResults.Count} kết quả");
                }

                // Get by date range
                var tomorrow = DateTime.Now.Date.AddDays(1);
                var nextWeek = DateTime.Now.Date.AddDays(7);
                var flightsByDate = FlightDAO.Instance.GetByDateRange(tomorrow, nextWeek);
                result.AppendLine($"Chuyến bay từ {tomorrow:dd/MM/yyyy} đến {nextWeek:dd/MM/yyyy}: {flightsByDate.Count}");

                // Get by status
                var scheduledFlights = FlightDAO.Instance.GetByStatus(FlightStatus.SCHEDULED);
                result.AppendLine($"Chuyến bay SCHEDULED: {scheduledFlights.Count}");

                result.AppendLine();
                result.AppendLine();
                result.AppendLine("=== KẾT THÚC TEST FLIGHT DAO ===");

                txtResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = $"LỖI:\r\n\r\n{ex.Message}\r\n\r\nStack Trace:\r\n{ex.StackTrace}";
            }
        }
        #endregion

        #region Clear Button

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtResult.Clear();
            txtResult.Text = "=== FLIGHT TICKET MANAGEMENT - TEST CONSOLE ===\r\n\r\n" +
                           "Nhấn các nút bên trái để chạy test:\r\n" +
                           "1. Test Database Connection\r\n" +
                           "2. Test Flight DTO\r\n" +
                           "3. Test Base DAO\r\n\r\n" +
                           "Kết quả sẽ hiển thị ở đây...";
        }

        #endregion

    }
}