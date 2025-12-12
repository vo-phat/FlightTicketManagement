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
    private List<ComboboxItem> _allSeatItems = new();
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

        // ComboBox hạng ghế
        cbClass = new UnderlinedComboBox("Hạng ghế", Array.Empty<object>())
        {
            Width = itemWidth,
            Height = 60,
            Margin = new Padding(0, 0, 0, 20)
        };
        cbClass.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

        // ComboBox ghế
        cbSeat = new UnderlinedComboBox("Số ghế", Array.Empty<object>())
        {
            Width = itemWidth,
            Height = 60,
            Margin = new Padding(0, 0, 0, 20)
        };
        cbSeat.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

        // TextField giá
        txtPrice = new UnderlinedTextField("💰 Giá cơ bản (₫)", "Ví dụ: 1.000.000")
        {
            Width = itemWidth,
            Height = 60,
            Margin = new Padding(0, 0, 0, 20)
        };
        txtPrice.InnerTextBox.TextAlign = HorizontalAlignment.Right;
        txtPrice.InnerTextBox.Text = _currentPrice.ToString("N0", new CultureInfo("vi-VN"));

        // Thêm vào FlowLayout
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

        // Sự kiện khi thay đổi hạng ghế -> lọc lại danh sách ghế
        cbClass.InnerCombo.SelectedIndexChanged += (_, __) => FilterSeatsByClass();
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

            // 1. Load TẤT CẢ hạng ghế từ bảng cabin_classes trong database
            var allCabinClasses = _cabinClassBUS.GetAllCabinClasses();

            _classItems = allCabinClasses
                .Select(c => new ComboboxItem { Id = c.ClassId, Name = c.ClassName })
                .OrderBy(c =>
                {
                    // Sắp xếp theo thứ tự ưu tiên
                    var order = new Dictionary<string, int>
                    {
                        { "First", 1 },
                        { "Business", 2 },
                        { "Premium Economy", 3 },
                        { "Economy", 4 }
                    };
                    return order.ContainsKey(c.Name) ? order[c.Name] : 99;
                })
                .ThenBy(c => c.Name) // Sắp xếp theo tên nếu không có trong dictionary
                .ToList();

            // 2. Load tất cả ghế của máy bay hiện tại (để lọc sau theo hạng)
            _allSeatItems = allSeats
                .Where(s => s.AircraftId == _currentAircraftId)
                .Select(s => new ComboboxItem
                {
                    Id = s.SeatId,
                    Name = s.SeatNumber,
                    ExtraId = s.ClassId // Lưu ClassId để lọc
                })
                .OrderBy(s => s.Name)
                .ToList();

            // Bind ComboBox hạng ghế
            if (cbClass.InnerCombo is ComboBox rawClass)
            {
                rawClass.DisplayMember = "Name";
                rawClass.ValueMember = "Id";
                rawClass.DataSource = _classItems;
            }

            // Gọi lọc ghế lần đầu
            FilterSeatsByClass();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void FilterSeatsByClass()
    {
        if (cbClass.InnerCombo is not ComboBox rawClass || cbSeat.InnerCombo is not ComboBox rawSeat)
            return;

        var selectedClass = rawClass.SelectedItem as ComboboxItem;
        if (selectedClass == null) return;

        // Lọc ghế theo hạng đã chọn
        var filteredSeats = _allSeatItems
            .Where(s => s.ExtraId == selectedClass.Id)
            .OrderBy(s => s.Name)
            .ToList();

        rawSeat.DisplayMember = "Name";
        rawSeat.ValueMember = "Id";
        rawSeat.DataSource = filteredSeats;

        // Nếu đang chọn hạng hiện tại, tự động chọn ghế hiện tại
        if (selectedClass.Id == _currentClassId)
        {
            var currentIndex = filteredSeats.FindIndex(s => s.Id == _currentSeatId);
            if (currentIndex >= 0)
            {
                rawSeat.SelectedIndex = currentIndex;
            }
        }
    }

    private void SelectCurrentValues()
    {
        // 1. Chọn hạng ghế hiện tại
        if (cbClass.InnerCombo is ComboBox rawClass)
        {
            rawClass.SelectedIndex = _classItems.FindIndex(c => c.Id == _currentClassId);
        }

        // 2. Ghế sẽ tự động được chọn trong FilterSeatsByClass()
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
        public int ExtraId { get; set; } // Dùng để lưu ClassId cho seat items
        public override string ToString() => Name;
    }
}