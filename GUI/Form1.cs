using DAO.Database;
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
                    result.AppendLine("✅ Kết nối database THÀNH CÔNG!");
                    result.AppendLine();
                    result.AppendLine($"Server: localhost:3306");
                    result.AppendLine($"Database: flightticketmanagement");
                    result.AppendLine($"MySQL Version: {DatabaseConnection.GetServerVersion()}");
                    result.AppendLine($"Connection String: {DatabaseConnection.ConnectionString}");
                }
                else
                {
                    result.AppendLine("❌ KHÔNG THỂ KẾT NỐI DATABASE!");
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
                txtResult.Text = $"❌ LỖI:\r\n\r\n{ex.Message}\r\n\r\nStack Trace:\r\n{ex.StackTrace}";
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
                    result.AppendLine($"✅ DTO hợp lệ!");
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
                    result.AppendLine($"❌ DTO không hợp lệ: {error}");
                }

                result.AppendLine();
                result.AppendLine();

                // Test 2.2: Test validation lỗi
                result.AppendLine("Test 2.2: Test Validation (DTO không hợp lệ)");
                result.AppendLine("---");
                var invalidFlight = new FlightDTO();
                if (!invalidFlight.IsValid(out string validationError))
                {
                    result.AppendLine($"✅ Validation hoạt động đúng!");
                    result.AppendLine($"Lỗi bắt được: {validationError}");
                }
                else
                {
                    result.AppendLine($"❌ Validation không hoạt động!");
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
                        DateTime.Now.AddDays(1).AddHours(2), // Khởi hành SAU
                        DateTime.Now.AddDays(1)              // Đến TRƯỚC ❌
                    );
                    result.AppendLine($"❌ Exception KHÔNG được throw!");
                }
                catch (ArgumentException ex)
                {
                    result.AppendLine($"✅ Exception được throw đúng!");
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
                txtResult.Text = $"❌ LỖI:\r\n\r\n{ex.Message}\r\n\r\nStack Trace:\r\n{ex.StackTrace}";
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
                txtResult.Text = $"❌ LỖI:\r\n\r\n{ex.Message}\r\n\r\nStack Trace:\r\n{ex.StackTrace}";
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