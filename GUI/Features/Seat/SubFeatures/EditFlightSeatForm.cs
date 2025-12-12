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
using DTO.Seat;

public class EditFlightSeatForm : Form
{
    public int FlightSeatId { get; }
    public int SelectedFlightId { get; private set; }
    public int SelectedSeatId { get; private set; }
    public int SelectedClassId { get; private set; }
    public decimal NewPrice { get; private set; }

    private readonly SeatBUS _seatBUS = new SeatBUS();

    // Controls
    private UnderlinedComboBox cbAircraft;
    private UnderlinedComboBox cbSeat;   // <--- QUAY LẠI COMBOBOX
    private UnderlinedComboBox cbClass;
    private UnderlinedTextField txtPrice;
    private PrimaryButton btnSave;
    private SecondaryButton btnCancel;

    // Data lists
    private List<ComboboxItem> _aircraftItems = new();
    private List<ComboboxItem> _seatItems = new();
    private List<ComboboxItem> _classItems = new();

    // Current values
    private int _currentAircraftId;
    private int _currentSeatId;
    private int _currentClassId;
    private decimal _currentPrice;

    public EditFlightSeatForm(int flightSeatId, int aircraftId, int seatId, int classId, decimal price)
    {
        FlightSeatId = flightSeatId;
        _currentAircraftId = aircraftId;
        _currentSeatId = seatId;
        _currentClassId = classId;
        _currentPrice = price;

        InitializeComponent();
        LoadComboboxData();
        SelectCurrentValues();
    }

    private void InitializeComponent()
    {
        // 1. Cấu hình Form
        Text = $"✏️ Sửa thông tin ghế";
        Size = new Size(480, 560); // Tăng chiều cao một chút cho thoải mái
        BackColor = Color.FromArgb(250, 253, 255);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        // 2. Panel chứa nút (Ghim đáy - Dock Bottom) -> Đảm bảo luôn hiển thị
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
        var pnlInputs = new Panel // Dùng Panel thường để chứa FlowLayout
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

        cbAircraft = new UnderlinedComboBox("Máy bay", Array.Empty<object>()) { Width = itemWidth, Height = 60, Margin = new Padding(0, 0, 0, 20) };
        cbAircraft.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

        cbClass = new UnderlinedComboBox("Hạng ghế", Array.Empty<object>()) { Width = itemWidth, Height = 60, Margin = new Padding(0, 0, 0, 20) };
        cbClass.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

        // <--- COMBOBOX GHẾ ---
        cbSeat = new UnderlinedComboBox("Số ghế", Array.Empty<object>()) { Width = itemWidth, Height = 60, Margin = new Padding(0, 0, 0, 20) };
        cbSeat.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

        txtPrice = new UnderlinedTextField("💰 Giá cơ bản (₫)", "Ví dụ: 1.000.000") { Width = itemWidth, Height = 60, Margin = new Padding(0, 0, 0, 20) };
        txtPrice.InnerTextBox.TextAlign = HorizontalAlignment.Right;
        txtPrice.InnerTextBox.Text = _currentPrice.ToString("N0", new CultureInfo("vi-VN"));

        // Thêm vào FlowLayout
        //flowInputs.Controls.Add(cbAircraft);
        flowInputs.Controls.Add(CreateSpacer(10));
        flowInputs.Controls.Add(cbClass);
        flowInputs.Controls.Add(CreateSpacer(10));
        flowInputs.Controls.Add(cbSeat); // Thêm Combobox ghế
        flowInputs.Controls.Add(CreateSpacer(10));
        flowInputs.Controls.Add(txtPrice);

        pnlInputs.Controls.Add(flowInputs);

        // 4. Add vào Form
        Controls.Add(pnlInputs);
        Controls.Add(pnlBottom);

        // Events
        btnSave.Click += BtnSave_Click;
        btnCancel.Click += (_, __) => Close();

        // Sự kiện lọc ghế khi đổi máy bay
        cbAircraft.InnerCombo.SelectedIndexChanged += (_, __) => FilterSeatsByAircraft();
    }

    private Control CreateSpacer(int height) => new Panel { Height = height, Width = 1, BackColor = Color.Transparent };

    private void LoadComboboxData()
    {
        try
        {
            var allSeats = _seatBUS.GetAllSeatsWithDetails();

            // 1. Máy bay
            _aircraftItems = allSeats
                .Select(s => new { s.AircraftId, Name = $"{s.AircraftManufacturer} {s.AircraftModel}" })
                .Distinct()
                .Select(a => new ComboboxItem { Id = a.AircraftId, Name = a.Name })
                .OrderBy(a => a.Name)
                .ToList();

            // 2. Tất cả ghế (để lọc sau)
            _seatItems = allSeats
                .Select(s => new ComboboxItem { Id = s.SeatId, Name = s.SeatNumber, ExtraId = s.AircraftId })
                .ToList();

            // 3. Hạng ghế
            _classItems = allSeats
                .Select(s => new ComboboxItem { Id = s.ClassId, Name = s.ClassName })
                .DistinctBy(c => c.Id)
                .OrderBy(c => c.Name)
                .ToList();

            // Bind data ban đầu
            if (cbAircraft.InnerCombo is ComboBox rawAircraft)
            {
                rawAircraft.DisplayMember = "Name"; rawAircraft.ValueMember = "Id"; rawAircraft.DataSource = _aircraftItems;
            }

            if (cbClass.InnerCombo is ComboBox rawClass)
            {
                rawClass.DisplayMember = "Name"; rawClass.ValueMember = "Id"; rawClass.DataSource = _classItems;
            }

            // Gọi lọc lần đầu để điền dữ liệu cho cbSeat
            FilterSeatsByAircraft();
        }
        catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
    }

    private void FilterSeatsByAircraft()
    {
        if (cbAircraft.InnerCombo is not ComboBox rawAircraft || cbSeat.InnerCombo is not ComboBox rawSeat) return;

        var selectedAircraft = rawAircraft.SelectedItem as ComboboxItem;
        if (selectedAircraft == null) return;

        // Lọc danh sách ghế thuộc máy bay đã chọn
        var filteredSeats = _seatItems
            .Where(x => x.ExtraId == selectedAircraft.Id)
            .OrderBy(x => x.Name) // Sắp xếp theo số ghế (A-Z)
            .ToList();

        rawSeat.DisplayMember = "Name";
        rawSeat.ValueMember = "Id";
        rawSeat.DataSource = filteredSeats;
    }

    private void SelectCurrentValues()
    {
        // 1. Chọn máy bay
        if (cbAircraft.InnerCombo is ComboBox rawAircraft)
            rawAircraft.SelectedIndex = _aircraftItems.FindIndex(a => a.Id == _currentAircraftId);

        // 2. Chọn ghế (Logic lọc đã chạy ở FilterSeatsByAircraft nhờ sự kiện hoặc gọi trực tiếp)
        // Cần tìm lại index trong danh sách ĐÃ LỌC
        if (cbSeat.InnerCombo is ComboBox rawSeat)
        {
            var currentList = rawSeat.DataSource as List<ComboboxItem>;
            if (currentList != null)
            {
                rawSeat.SelectedIndex = currentList.FindIndex(s => s.Id == _currentSeatId);
            }
        }

        // 3. Chọn hạng
        if (cbClass.InnerCombo is ComboBox rawClass)
            rawClass.SelectedIndex = _classItems.FindIndex(c => c.Id == _currentClassId);
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var aircraft = cbAircraft.InnerCombo.SelectedItem as ComboboxItem;
        var seat = cbSeat.InnerCombo.SelectedItem as ComboboxItem;
        var cls = cbClass.InnerCombo.SelectedItem as ComboboxItem;

        if (aircraft == null || seat == null || cls == null)
        {
            MessageBox.Show("Vui lòng chọn đầy đủ thông tin.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
        }

        var rawPrice = Regex.Replace(txtPrice.InnerTextBox.Text ?? "", @"[^\d]", "");
        if (!decimal.TryParse(rawPrice, out var price) || price <= 0)
        {
            MessageBox.Show("Giá không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
        }

        SelectedFlightId = aircraft.Id;
        SelectedSeatId = seat.Id; // Lấy trực tiếp ID từ Combobox
        SelectedClassId = cls.Id;
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