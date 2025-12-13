using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using BUS.Seat;
using BUS.CabinClass;
using DTO.Seat;

public class EditFlightSeatForm : Form
{
    public int FlightSeatId { get; }
    public int SelectedSeatId { get; private set; }
    public int SelectedClassId { get; private set; }
    public decimal NewPrice { get; private set; }

    private readonly SeatBUS _seatBUS = new SeatBUS();
    private readonly CabinClassBUS _cabinClassBUS = new CabinClassBUS();

    // Controls
    private UnderlinedComboBox cbClass;
    private UnderlinedComboBox cbSeat;
    private UnderlinedTextField txtPrice;
    private PrimaryButton btnSave;
    private SecondaryButton btnCancel;

    // Data lists
    private List<ComboboxItem> _classItems = new();
    private List<SeatDTO> _allSeatsInAircraft = new(); // ✅ Lưu TẤT CẢ ghế của máy bay
    private int _currentAircraftId;

    // Current values
    private int _currentSeatId;
    private int _currentClassId;
    private decimal _currentPrice;

    public EditFlightSeatForm(int flightSeatId, int seatId, decimal price)
    {
        FlightSeatId = flightSeatId;
        _currentSeatId = seatId;
        _currentPrice = price;

        InitializeComponent();
        LoadComboboxData();
        SelectCurrentValues();
    }

    private void InitializeComponent()
    {
        // 1. Cấu hình Form
        Text = $"✏️ Sửa thông tin ghế";
        Size = new Size(480, 520);
        BackColor = Color.FromArgb(250, 253, 255);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        // 2. Panel chứa nút (Ghim đáy - Dock Bottom)
        var pnlBottom = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 80,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 10, 20, 20)
        };

        var flowButtons = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        btnSave = new PrimaryButton("💾 Lưu") { Width = 120, Height = 40, Margin = new Padding(10, 0, 0, 0) };
        btnCancel = new SecondaryButton("✖ Hủy") { Width = 120, Height = 40, Margin = new Padding(10, 0, 0, 0) };

        flowButtons.Controls.Add(btnCancel);
        flowButtons.Controls.Add(btnSave);
        pnlBottom.Controls.Add(flowButtons);

        // 3. Panel chứa Inputs (Dock Fill)
        var pnlInputs = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(40, 30, 40, 0)
        };

        var flowInputs = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };

        // Khởi tạo Inputs
        int itemWidth = 380;

        // ComboBox hạng ghế (✅ CHO PHÉP CHỌN)
        cbClass = new UnderlinedComboBox("Hạng ghế", Array.Empty<object>())
        {
            Width = itemWidth,
            Height = 60,
            Margin = new Padding(0, 0, 0, 20)
        };
        cbClass.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        cbClass.InnerCombo.Enabled = true; // ✅ Cho phép chọn hạng ghế
        cbClass.InnerCombo.BackColor = Color.White;
        cbClass.InnerCombo.ForeColor = Color.Black;

        // ComboBox ghế
        cbSeat = new UnderlinedComboBox("Số ghế", Array.Empty<object>())
        {
            Width = itemWidth,
            Height = 60,
            Margin = new Padding(0, 0, 0, 20)
        };
        cbSeat.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        cbSeat.InnerCombo.Enabled = false;
        cbSeat.InnerCombo.BackColor = Color.White;
        cbSeat.InnerCombo.ForeColor = Color.Black;

        // TextField giá
        txtPrice = new UnderlinedTextField("💰 Giá cơ bản (₫)", "Ví dụ: 1.000.000")
        {
            Width = itemWidth,
            Height = 60,
            Margin = new Padding(0, 0, 0, 20)
        };
        txtPrice.InnerTextBox.TextAlign = HorizontalAlignment.Right;
        txtPrice.InnerTextBox.Text = _currentPrice.ToString("N0", new CultureInfo("vi-VN"));

        flowInputs.Controls.Add(CreateSpacer(10));
        flowInputs.Controls.Add(cbClass);
        flowInputs.Controls.Add(CreateSpacer(10));
        flowInputs.Controls.Add(cbSeat);
        flowInputs.Controls.Add(CreateSpacer(10));
        flowInputs.Controls.Add(txtPrice);

        pnlInputs.Controls.Add(flowInputs);

        // 4. Add vào Form
        Controls.Add(pnlInputs);
        Controls.Add(pnlBottom);

        // Events
        btnSave.Click += BtnSave_Click;
        btnCancel.Click += (_, __) => Close();

        // ✅ THÊM: Event khi thay đổi hạng ghế
        cbClass.InnerCombo.SelectedIndexChanged += CbClass_SelectedIndexChanged;
    }

    private Control CreateSpacer(int height) => new Panel { Height = height, Width = 1, BackColor = Color.Transparent };

    private void LoadComboboxData()
    {
        try
        {
            // Lấy thông tin của ghế hiện tại
            var allSeats = _seatBUS.GetAllSeatsWithDetails();
            var currentSeat = allSeats.FirstOrDefault(s => s.SeatId == _currentSeatId);

            if (currentSeat == null)
            {
                MessageBox.Show("Không tìm thấy thông tin ghế.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            _currentAircraftId = currentSeat.AircraftId;
            _currentClassId = currentSeat.ClassId;

            // ✅ Lưu TẤT CẢ ghế của máy bay này (không lọc theo hạng)
            _allSeatsInAircraft = allSeats
                .Where(s => s.AircraftId == _currentAircraftId)
                .OrderBy(s => s.SeatNumber)
                .ToList();

            // 1. Load TẤT CẢ hạng ghế từ database
            var allCabinClasses = _cabinClassBUS.GetAllCabinClasses();

            _classItems = allCabinClasses
                .Select(c => new ComboboxItem { Id = c.ClassId, Name = c.ClassName })
                .OrderBy(c =>
                {
                    var order = new Dictionary<string, int>
                    {
                        { "First", 1 },
                        { "Business", 2 },
                        { "Premium Economy", 3 },
                        { "Economy", 4 }
                    };
                    return order.ContainsKey(c.Name) ? order[c.Name] : 99;
                })
                .ThenBy(c => c.Name)
                .ToList();

            // Bind ComboBox hạng ghế
            if (cbClass.InnerCombo is ComboBox rawClass)
            {
                rawClass.DisplayMember = "Name";
                rawClass.ValueMember = "Id";
                rawClass.DataSource = _classItems;
            }

            // ✅ Không bind cbSeat ở đây - sẽ bind trong CbClass_SelectedIndexChanged
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SelectCurrentValues()
    {
        // 1. Chọn hạng ghế hiện tại (sẽ trigger event và load ghế)
        if (cbClass.InnerCombo is ComboBox rawClass)
        {
            var currentClassIndex = _classItems.FindIndex(c => c.Id == _currentClassId);
            if (currentClassIndex >= 0)
            {
                rawClass.SelectedIndex = currentClassIndex;
            }
        }

        // 2. Chọn ghế hiện tại (sẽ được thực hiện trong CbClass_SelectedIndexChanged)
    }

    // ✅ THÊM: Event handler khi thay đổi hạng ghế
    private void CbClass_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cbClass.InnerCombo.SelectedItem is not ComboboxItem selectedClass)
            return;

        // Lọc ghế theo hạng đã chọn
        var seatsForClass = _allSeatsInAircraft
            .Where(s => s.ClassId == selectedClass.Id)
            .Select(s => new ComboboxItem
            {
                Id = s.SeatId,
                Name = s.SeatNumber,
                ExtraId = s.ClassId
            })
            .OrderBy(s => s.Name)
            .ToList();

        // Bind lại ComboBox ghế
        if (cbSeat.InnerCombo is ComboBox rawSeat)
        {
            rawSeat.DataSource = null; // Clear trước
            rawSeat.DisplayMember = "Name";
            rawSeat.ValueMember = "Id";
            rawSeat.DataSource = seatsForClass;

            // Nếu đang chọn hạng hiện tại, select ghế hiện tại
            if (selectedClass.Id == _currentClassId)
            {
                var currentIndex = seatsForClass.FindIndex(s => s.Id == _currentSeatId);
                if (currentIndex >= 0)
                {
                    rawSeat.SelectedIndex = currentIndex;
                }
            }
            else
            {
                // Chọn ghế đầu tiên nếu chuyển sang hạng khác
                if (seatsForClass.Count > 0)
                {
                    rawSeat.SelectedIndex = 0;
                }
            }

            // Enable ComboBox ghế khi có dữ liệu
            rawSeat.Enabled = seatsForClass.Count > 0;
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var cls = cbClass.InnerCombo.SelectedItem as ComboboxItem;
        var seat = cbSeat.InnerCombo.SelectedItem as ComboboxItem;

        if (cls == null || seat == null)
        {
            MessageBox.Show("Vui lòng chọn đầy đủ thông tin.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var rawPrice = Regex.Replace(txtPrice.InnerTextBox.Text ?? "", @"[^\d]", "");
        if (!decimal.TryParse(rawPrice, out var price) || price <= 0)
        {
            MessageBox.Show("Giá không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SelectedClassId = cls.Id;
        SelectedSeatId = seat.Id;
        NewPrice = price;

        DialogResult = DialogResult.OK;
        Close();
    }

    public class ComboboxItem
    {
        public string Name { get; set; } = "";
        public int Id { get; set; }
        public int ExtraId { get; set; }
        public override string ToString() => Name;
    }
}