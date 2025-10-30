using System;
using System.Drawing;
using System.Windows.Forms;
using BUS.Flight;
using DTO.Flight;
using GUI.Components.Buttons;

namespace GUI.Features.Flight.SubFeatures
{
    public class FlightDetailControl : UserControl
    {
        #region Fields

        private TableLayoutPanel main;
        private Label lblTitle;
        private Panel card;
        private TableLayoutPanel grid;
        private FlowLayoutPanel bottomPanel;

        // Value labels
        private Label valueFlightId;
        private Label valueFlightNumber;
        private Label valueAircraftId;
        private Label valueRouteId;
        private Label valueDepartureTime;
        private Label valueArrivalTime;
        private Label valueStatus;
        private Label valueDuration;

        // Buttons
        private PrimaryButton btnEdit;
        private SecondaryButton btnDelete;
        private SecondaryButton btnClose;

        // Current flight
        private FlightDTO currentFlight;

        #endregion

        #region Events

        public event Action<int> OnEditRequested;
        public event Action OnClosed;

        #endregion

        #region Constructor

        public FlightDetailControl()
        {
            InitializeComponent();
            BuildLayout();
        }

        #endregion

        #region Initialization

        private void InitializeComponent()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(232, 240, 252);
            Name = "FlightDetailControl";
            Size = new Size(1460, 430);
            ResumeLayout(false);
        }

        private static Label Key(string text) => new Label
        {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            Margin = new Padding(0, 8, 12, 8),
            ForeColor = Color.FromArgb(70, 70, 70)
        };

        private static Label Val(string name) => new Label
        {
            Name = name,
            Text = "-",
            AutoSize = true,
            Font = new Font("Segoe UI", 11f, FontStyle.Regular),
            Margin = new Padding(0, 8, 0, 8),
            ForeColor = Color.FromArgb(33, 37, 41)
        };

        private void BuildLayout()
        {
            // ===== Title =====
            lblTitle = new Label
            {
                Text = "🧾 Chi tiết chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Card =====
            card = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20),
                Margin = new Padding(24, 8, 24, 24),
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            var secTitle = new Label
            {
                Text = "Thông tin chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 20),
                Dock = DockStyle.Top
            };

            // ===== Grid =====
            grid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 4, // 2 cặp key-value
                Padding = new Padding(0, 0, 0, 20)
            };

            // Column styles: Key1 | Value1 | Key2 | Value2
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            // Row 0: Flight ID | Flight Number
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("ID:"), 0, 0);
            valueFlightId = Val("valueFlightId");
            grid.Controls.Add(valueFlightId, 1, 0);
            grid.Controls.Add(Key("Số hiệu:"), 2, 0);
            valueFlightNumber = Val("valueFlightNumber");
            grid.Controls.Add(valueFlightNumber, 3, 0);

            // Row 1: Aircraft ID | Route ID
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Máy bay:"), 0, 1);
            valueAircraftId = Val("valueAircraftId");
            grid.Controls.Add(valueAircraftId, 1, 1);
            grid.Controls.Add(Key("Tuyến bay:"), 2, 1);
            valueRouteId = Val("valueRouteId");
            grid.Controls.Add(valueRouteId, 3, 1);

            // Row 2: Departure Time | Arrival Time
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Giờ khởi hành:"), 0, 2);
            valueDepartureTime = Val("valueDepartureTime");
            grid.Controls.Add(valueDepartureTime, 1, 2);
            grid.Controls.Add(Key("Giờ hạ cánh:"), 2, 2);
            valueArrivalTime = Val("valueArrivalTime");
            grid.Controls.Add(valueArrivalTime, 3, 2);

            // Row 3: Status | Duration
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(Key("Trạng thái:"), 0, 3);
            valueStatus = Val("valueStatus");
            grid.Controls.Add(valueStatus, 1, 3);
            grid.Controls.Add(Key("Thời gian bay:"), 2, 3);
            valueDuration = Val("valueDuration");
            grid.Controls.Add(valueDuration, 3, 3);

            // ===== Bottom actions =====
            bottomPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(0, 20, 0, 0),
                WrapContents = false
            };

            btnClose = new SecondaryButton("✖ Đóng") { Width = 120, Height = 40 };
            btnClose.Click += (s, e) => OnClosed?.Invoke();

            btnDelete = new SecondaryButton("🗑 Xóa") { Width = 120, Height = 40, Margin = new Padding(0, 0, 12, 0) };
            btnDelete.Click += BtnDelete_Click;

            btnEdit = new PrimaryButton("✏ Sửa") { Width = 120, Height = 40, Margin = new Padding(0, 0, 12, 0) };
            btnEdit.Click += BtnEdit_Click;

            bottomPanel.Controls.Add(btnClose);
            bottomPanel.Controls.Add(btnDelete);
            bottomPanel.Controls.Add(btnEdit);

            // ===== Assemble card =====
            card.Controls.Add(grid);
            card.Controls.Add(bottomPanel);
            card.Controls.Add(secTitle);

            // ===== Main =====
            main = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 2
            };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            main.Controls.Add(lblTitle, 0, 0);
            main.Controls.Add(card, 0, 1);

            Controls.Clear();
            Controls.Add(main);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load flight từ database theo ID
        /// </summary>
        public void LoadFlight(int flightId)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var result = FlightBUS.Instance.GetFlightById(flightId);

                if (!result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        "Lỗi tải dữ liệu",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                currentFlight = result.GetData<FlightDTO>();
                DisplayFlightInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi không xác định:\n{ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Hiển thị thông tin flight lên UI
        /// </summary>
        private void DisplayFlightInfo()
        {
            if (currentFlight == null) return;

            lblTitle.Text = $"🧾 Chi tiết chuyến bay - {currentFlight.FlightNumber}";

            valueFlightId.Text = currentFlight.FlightId.ToString();
            valueFlightNumber.Text = currentFlight.FlightNumber;
            valueAircraftId.Text = $"#{currentFlight.AircraftId}";
            valueRouteId.Text = $"#{currentFlight.RouteId}";
            valueDepartureTime.Text = currentFlight.DepartureTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
            valueArrivalTime.Text = currentFlight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
            valueStatus.Text = currentFlight.Status.GetDescription();

            var duration = currentFlight.GetFlightDuration();
            valueDuration.Text = duration.HasValue
                ? $"{duration.Value.Hours}h {duration.Value.Minutes}m"
                : "N/A";

            // Style cho status
            valueStatus.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            valueStatus.ForeColor = currentFlight.Status switch
            {
                FlightStatus.SCHEDULED => Color.FromArgb(0, 123, 255),
                FlightStatus.DELAYED => Color.FromArgb(255, 193, 7),
                FlightStatus.CANCELLED => Color.FromArgb(220, 53, 69),
                FlightStatus.COMPLETED => Color.FromArgb(40, 167, 69),
                _ => Color.Black
            };

            // Disable buttons nếu không thể sửa/xóa
            bool canModify = currentFlight.Status == FlightStatus.SCHEDULED ||
                            currentFlight.Status == FlightStatus.DELAYED;
            btnEdit.Enabled = canModify;

            bool canDelete = currentFlight.Status == FlightStatus.SCHEDULED;
            btnDelete.Enabled = canDelete;
        }

        #endregion

        #region Event Handlers

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (currentFlight == null) return;
            OnEditRequested?.Invoke(currentFlight.FlightId);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (currentFlight == null) return;

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa chuyến bay '{currentFlight.FlightNumber}'?\n\n" +
                $"Hành động này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;

                var deleteResult = FlightBUS.Instance.DeleteFlight(currentFlight.FlightId);

                if (deleteResult.Success)
                {
                    MessageBox.Show(
                        deleteResult.Message,
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    OnClosed?.Invoke(); // Đóng detail và quay về list
                }
                else
                {
                    MessageBox.Show(
                        deleteResult.Message,
                        "Lỗi xóa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi không xác định:\n{ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}