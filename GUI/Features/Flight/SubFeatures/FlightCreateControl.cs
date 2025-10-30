using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BUS.Flight;
using DTO.Flight;
using GUI.Components.Buttons;
using GUI.Components.Inputs;

namespace GUI.Features.Flight.SubFeatures
{
    public class FlightCreateControl : UserControl
    {
        #region Fields

        private TableLayoutPanel mainPanel;
        private Label lblTitle;
        private TableLayoutPanel inputPanel;
        private FlowLayoutPanel buttonRow;

        // Input controls
        private UnderlinedTextField txtFlightNumber;
        private UnderlinedTextField txtAircraftId;
        private UnderlinedTextField txtRouteId;
        private DateTimePickerCustom dtpDepartureTime;
        private DateTimePickerCustom dtpArrivalTime;

        // Buttons
        private PrimaryButton btnCreate;
        private Panel titlePanel;
        private SecondaryButton btnCancel;

        #endregion

        #region Events

        /// <summary>
        /// Event khi tạo flight thành công
        /// </summary>
        public event Action OnFlightCreated;

        #endregion

        #region Constructor

        public FlightCreateControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Initialization

        private void InitializeComponent()
        {
            titlePanel = new Panel();
            lblTitle = new Label();
            mainPanel = new TableLayoutPanel();
            titlePanel.SuspendLayout();
            mainPanel.SuspendLayout();
            SuspendLayout();
            // 
            // titlePanel
            // 
            titlePanel.Controls.Add(lblTitle);
            titlePanel.Location = new Point(3, 3);
            titlePanel.Name = "titlePanel";
            titlePanel.Size = new Size(194, 100);
            titlePanel.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(100, 23);
            lblTitle.TabIndex = 0;
            // 
            // mainPanel
            // 
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            mainPanel.Controls.Add(titlePanel, 0, 0);
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.RowStyles.Add(new RowStyle());
            mainPanel.RowStyles.Add(new RowStyle());
            mainPanel.RowStyles.Add(new RowStyle());
            mainPanel.Size = new Size(200, 100);
            mainPanel.TabIndex = 0;
            // 
            // FlightCreateControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Controls.Add(mainPanel);
            Name = "FlightCreateControl";
            Size = new Size(1538, 736);
            titlePanel.ResumeLayout(false);
            mainPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void BuildInputPanel()
        {
            inputPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Padding = new Padding(32, 24, 32, 24),
                Margin = new Padding(24, 12, 24, 0),
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 3,
                BorderStyle = BorderStyle.FixedSingle
            };

            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            // Row heights
            for (int i = 0; i < 3; i++)
                inputPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // ===== Row 0: Flight Number | Aircraft ID =====
            txtFlightNumber = new UnderlinedTextField("Số hiệu chuyến bay *", "VD: VN123")
            {
                Width = 350,
                Margin = new Padding(0, 0, 16, 16)
            };

            txtAircraftId = new UnderlinedTextField("ID Máy bay *", "VD: 1")
            {
                Width = 350,
                Margin = new Padding(16, 0, 0, 16)
            };

            inputPanel.Controls.Add(txtFlightNumber, 0, 0);
            inputPanel.Controls.Add(txtAircraftId, 1, 0);

            // ===== Row 1: Route ID | (Empty or future field) =====
            txtRouteId = new UnderlinedTextField("ID Tuyến bay *", "VD: 1")
            {
                Width = 350,
                Margin = new Padding(0, 0, 16, 16)
            };

            inputPanel.Controls.Add(txtRouteId, 0, 1);
            // Column 1,1 empty for now

            // ===== Row 2: Departure Time | Arrival Time =====
            dtpDepartureTime = new DateTimePickerCustom("Thời gian khởi hành *", "")
            {
                Width = 350,
                Height = 72,
                Margin = new Padding(0, 0, 16, 0)
            };
            dtpDepartureTime.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpDepartureTime.MinDate = DateTime.Now;

            dtpArrivalTime = new DateTimePickerCustom("Thời gian hạ cánh *", "")
            {
                Width = 350,
                Height = 72,
                Margin = new Padding(16, 0, 0, 0)
            };
            dtpArrivalTime.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpArrivalTime.MinDate = DateTime.Now;

            inputPanel.Controls.Add(dtpDepartureTime, 0, 2);
            inputPanel.Controls.Add(dtpArrivalTime, 1, 2);

            // ===== Realtime validation =====
            txtFlightNumber.InnerTextBox.TextChanged += (s, e) => ValidateFlightNumber();
            txtAircraftId.InnerTextBox.TextChanged += (s, e) => ValidateNumeric(txtAircraftId, "ID Máy bay");
            txtRouteId.InnerTextBox.TextChanged += (s, e) => ValidateNumeric(txtRouteId, "ID Tuyến bay");
            dtpDepartureTime.DateTimePicker.ValueChanged += (s, e) => ValidateDateTimes();
            dtpArrivalTime.DateTimePicker.ValueChanged += (s, e) => ValidateDateTimes();
        }

        private void BuildButtonRow()
        {
            buttonRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(24, 20, 24, 20),
                WrapContents = false,
                BackColor = Color.Transparent
            };

            btnCreate = new PrimaryButton("➕ Tạo chuyến bay")
            {
                Width = 180,
                Height = 44,
                Margin = new Padding(0)
            };
            btnCreate.Click += BtnCreate_Click;

            btnCancel = new SecondaryButton("✖ Hủy")
            {
                Width = 120,
                Height = 44,
                Margin = new Padding(0, 0, 12, 0)
            };
            btnCancel.Click += BtnCancel_Click;

            buttonRow.Controls.Add(btnCreate);
            buttonRow.Controls.Add(btnCancel);
        }

        #endregion

        #region Validation

        private void ValidateFlightNumber()
        {
            string value = txtFlightNumber.Text.Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                txtFlightNumber.LabelForeColor = Color.Red;
                return;
            }

            if (value.Length > 20)
            {
                txtFlightNumber.LabelForeColor = Color.Red;
                MessageBox.Show("Số hiệu chuyến bay không được quá 20 ký tự", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtFlightNumber.LabelForeColor = Color.FromArgb(70, 70, 70);
        }

        private void ValidateNumeric(UnderlinedTextField textField, string fieldName)
        {
            string value = textField.Text.Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                textField.LabelForeColor = Color.Red;
                return;
            }

            if (!int.TryParse(value, out int number) || number <= 0)
            {
                textField.LabelForeColor = Color.Red;
                return;
            }

            textField.LabelForeColor = Color.FromArgb(70, 70, 70);
        }

        private void ValidateDateTimes()
        {
            DateTime departure = dtpDepartureTime.Value;
            DateTime arrival = dtpArrivalTime.Value;

            if (arrival <= departure)
            {
                dtpArrivalTime.LabelText = "Thời gian hạ cánh * (phải sau giờ khởi hành!)";
                // Có thể đổi màu label
            }
            else
            {
                dtpArrivalTime.LabelText = "Thời gian hạ cánh *";
            }
        }

        private bool ValidateAllFields(out string errorMessage)
        {
            errorMessage = string.Empty;

            // Flight Number
            if (string.IsNullOrWhiteSpace(txtFlightNumber.Text))
            {
                errorMessage = "Vui lòng nhập số hiệu chuyến bay";
                txtFlightNumber.InnerTextBox.Focus();
                return false;
            }

            // Aircraft ID
            if (!int.TryParse(txtAircraftId.Text, out int aircraftId) || aircraftId <= 0)
            {
                errorMessage = "ID Máy bay phải là số nguyên dương";
                txtAircraftId.InnerTextBox.Focus();
                return false;
            }

            // Route ID
            if (!int.TryParse(txtRouteId.Text, out int routeId) || routeId <= 0)
            {
                errorMessage = "ID Tuyến bay phải là số nguyên dương";
                txtRouteId.InnerTextBox.Focus();
                return false;
            }

            // Date/Time
            DateTime departure = dtpDepartureTime.Value;
            DateTime arrival = dtpArrivalTime.Value;

            if (arrival <= departure)
            {
                errorMessage = "Thời gian hạ cánh phải sau thời gian khởi hành";
                return false;
            }

            return true;
        }

        #endregion

        #region Event Handlers

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            // Validate
            if (!ValidateAllFields(out string validationError))
            {
                MessageBox.Show(
                    validationError,
                    "Lỗi nhập liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                btnCreate.Enabled = false;

                // Create DTO
                var newFlight = new FlightDTO(
                    flightNumber: txtFlightNumber.Text.Trim().ToUpper(),
                    aircraftId: int.Parse(txtAircraftId.Text),
                    routeId: int.Parse(txtRouteId.Text),
                    departureTime: dtpDepartureTime.Value,
                    arrivalTime: dtpArrivalTime.Value
                );

                // Call BUS
                var result = FlightBUS.Instance.CreateFlight(newFlight);

                if (result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Clear form
                    ClearForm();

                    // Raise event
                    OnFlightCreated?.Invoke();
                }
                else
                {
                    MessageBox.Show(
                        result.GetFullErrorMessage(),
                        "Lỗi tạo chuyến bay",
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
                btnCreate.Enabled = true;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc muốn hủy? Dữ liệu đã nhập sẽ bị mất.",
                "Xác nhận hủy",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                ClearForm();
            }
        }

        private void ClearForm()
        {
            txtFlightNumber.Text = string.Empty;
            txtAircraftId.Text = string.Empty;
            txtRouteId.Text = string.Empty;
            dtpDepartureTime.Value = DateTime.Now.AddDays(1);
            dtpArrivalTime.Value = DateTime.Now.AddDays(1).AddHours(2);
        }

        #endregion
    }
}