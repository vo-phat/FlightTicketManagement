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

    // Khai báo các Controls Tùy chỉnh
    private UnderlinedComboBox cbAircraft;
    private UnderlinedComboBox cbSeat;
    private UnderlinedComboBox cbClass;
    private UnderlinedTextField txtPrice;
    private PrimaryButton btnSave;
    private SecondaryButton btnCancel; // Khai báo tại đây để tránh tạo lại đối tượng

    private List<ComboboxItem> _aircraftItems = new();
    private List<ComboboxItem> _seatItems = new();
    private List<ComboboxItem> _classItems = new();

    private int _currentFlightId;
    private int _currentSeatId;
    private int _currentClassId;
    private decimal _currentPrice;

    public EditFlightSeatForm(int flightSeatId, int flightId, int seatId, int classId, decimal price)
    {
        FlightSeatId = flightSeatId;
        _currentFlightId = flightId;
        _currentSeatId = seatId;
        _currentClassId = classId;
        _currentPrice = price;

        Text = $"✏️ Sửa thông tin ghế #{seatId}";
        Size = new Size(380, 460); // Đã giữ nguyên kích thước Form
        BackColor = Color.FromArgb(250, 253, 255);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        DialogResult = DialogResult.Cancel;

        InitializeComponent();
        LoadComboboxData();
        SelectCurrentValues();
    }

    private void InitializeComponent()
    {
        // 🎨 Cấu hình Form tổng thể
        BackColor = Color.FromArgb(235, 243, 254);
        Size = new Size(600, 580); // 👉 Tăng kích thước form
        StartPosition = FormStartPosition.CenterParent;
        Text = "✏️ Sửa thông tin ghế";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        // === Layout chính ===
        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(60, 50, 60, 40), // 👉 thêm padding dưới để nút cách đáy form
            ColumnCount = 1,
            RowCount = 6
        };

        // Row styles
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Máy bay
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Hạng
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Ghế
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Giá
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Đệm trống
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100f)); // ✅ Nút bấm nằm thấp hơn

        // ======= Inputs =======
        cbAircraft = new UnderlinedComboBox("Máy bay", Array.Empty<object>())
        {
            Dock = DockStyle.Top,
            Width = 380,
            Margin = new Padding(0, 0, 0, 25)
        };
        cbAircraft.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

        cbClass = new UnderlinedComboBox("Hạng ghế", Array.Empty<object>())
        {
            Dock = DockStyle.Top,
            Width = 380,
            Margin = new Padding(0, 0, 0, 25)
        };
        cbClass.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

        cbSeat = new UnderlinedComboBox("Số ghế (VD: 12A)", Array.Empty<object>())
        {
            Dock = DockStyle.Top,
            Width = 380,
            Margin = new Padding(0, 0, 0, 25)
        };
        cbSeat.InnerCombo.DropDownStyle = ComboBoxStyle.DropDownList;

        txtPrice = new UnderlinedTextField("💰 Giá cơ bản (₫)", "Ví dụ: 1.000.000")
        {
            Dock = DockStyle.Top,
            Width = 380,
            Margin = new Padding(0, 0, 0, 15)
        };
        txtPrice.InnerTextBox.TextAlign = HorizontalAlignment.Right;
        txtPrice.InnerTextBox.Text = _currentPrice.ToString("N0", new CultureInfo("vi-VN"));

        // ====== Panel chứa nút (căn giữa & xuống dưới) ======
        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = false,
            Height = 80,
            Padding = new Padding(0, 20, 0, 0), // 👉 tăng top padding để nút xuống thấp
            Margin = new Padding(0, 20, 0, 0),  // 👉 tạo khoảng cách phía trên nút
            WrapContents = false,
            Anchor = AnchorStyles.None,
        };

        btnSave = new PrimaryButton("💾 Lưu")
        {
            Width = 140,
            Height = 45,
            Margin = new Padding(30, 0, 20, 0)
        };

        btnCancel = new SecondaryButton("✖ Hủy")
        {
            Width = 140,
            Height = 45,
            Margin = new Padding(0)
        };

        buttonPanel.Controls.Add(btnSave);
        buttonPanel.Controls.Add(btnCancel);

        // ===== Thêm tất cả vào layout chính =====
        mainLayout.Controls.Add(cbAircraft, 0, 0);
        mainLayout.Controls.Add(cbClass, 0, 1);
        mainLayout.Controls.Add(cbSeat, 0, 2);
        mainLayout.Controls.Add(txtPrice, 0, 3);
        mainLayout.Controls.Add(new Panel(), 0, 4); // khoảng trống
        mainLayout.Controls.Add(buttonPanel, 0, 5);

        mainLayout.SetCellPosition(buttonPanel, new TableLayoutPanelCellPosition(0, 5));
        mainLayout.SetColumnSpan(buttonPanel, 1);
        mainLayout.SetRow(buttonPanel, 5);
        buttonPanel.Anchor = AnchorStyles.None; // ✅ Căn giữa hoàn toàn

        Controls.Add(mainLayout);

        // ==== Sự kiện ====
        btnSave.Click += BtnSave_Click;
        btnCancel.Click += (_, __) => Close();
        cbAircraft.InnerCombo.SelectedIndexChanged += (_, __) => FilterSeatsByAircraft();
    }

    private void LoadComboboxData()
    {
        // Giữ nguyên logic LoadComboboxData, chỉ thay đổi cách truy cập InnerCombo
        try
        {
            var allSeats = _seatBUS.GetAllSeatsWithDetails();

            // 🛫 Máy bay
            _aircraftItems = allSeats
                .Select(s => new { s.AircraftId, Name = $"{s.AircraftManufacturer} {s.AircraftModel}" })
                .Distinct()
                .Select(a => new ComboboxItem { Id = a.AircraftId, Name = a.Name })
                .OrderBy(a => a.Name)
                .ToList();

            // 💺 Tất cả ghế (lọc sau)
            _seatItems = allSeats
                .Select(s => new ComboboxItem { Id = s.SeatId, Name = s.SeatNumber, ExtraId = s.AircraftId })
                .OrderBy(c => c.Name)
                .ToList();

            // 🏷 Hạng ghế
            _classItems = allSeats
                .Select(s => new ComboboxItem { Id = s.ClassId, Name = s.ClassName })
                .DistinctBy(c => c.Id)
                .OrderBy(c => c.Name)
                .ToList();

            // Bind data
            if (cbAircraft.InnerCombo is ComboBox rawAircraft)
            {
                rawAircraft.DisplayMember = "Name";
                rawAircraft.ValueMember = "Id";
                rawAircraft.DataSource = _aircraftItems;
            }

            if (cbClass.InnerCombo is ComboBox rawClass)
            {
                rawClass.DisplayMember = "Name";
                rawClass.ValueMember = "Id";
                rawClass.DataSource = _classItems;
            }

            FilterSeatsByAircraft();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể tải dữ liệu ComboBox: " + ex.Message,
                "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void FilterSeatsByAircraft()
    {
        // Giữ nguyên logic FilterSeatsByAircraft, chỉ thay đổi cách truy cập InnerCombo
        if (cbAircraft.InnerCombo is not ComboBox rawAircraft || cbSeat.InnerCombo is not ComboBox rawSeat)
            return;

        var selectedAircraft = rawAircraft.SelectedItem as ComboboxItem;
        if (selectedAircraft == null) return;

        var filteredSeats = _seatItems
            .Where(x => x.ExtraId == selectedAircraft.Id)
            .ToList();

        rawSeat.DisplayMember = "Name";
        rawSeat.ValueMember = "Id";
        rawSeat.DataSource = filteredSeats;
    }

    private void SelectCurrentValues()
    {
        // Giữ nguyên logic SelectCurrentValues, chỉ thay đổi cách truy cập InnerCombo và TextField
        if (cbAircraft.InnerCombo is ComboBox rawAircraft)
            rawAircraft.SelectedIndex = _aircraftItems.FindIndex(a => a.Id == _currentFlightId);

        if (cbSeat.InnerCombo is ComboBox rawSeat)
            // LƯU Ý: Phải filterSeats trước khi select. FilterSeatsByAircraft() đã được gọi trong LoadComboboxData()
            // Đảm bảo chỉ mục tìm kiếm đúng trên danh sách đã lọc (DataSource của rawSeat)
            rawSeat.SelectedIndex = rawSeat.Items.OfType<ComboboxItem>().ToList().FindIndex(c => c.Id == _currentSeatId);

        if (cbClass.InnerCombo is ComboBox rawClass)
            rawClass.SelectedIndex = _classItems.FindIndex(c => c.Id == _currentClassId);

        // Gán giá trị tiền tệ
        txtPrice.InnerTextBox.Text = _currentPrice.ToString("N0", new CultureInfo("vi-VN"));
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        // Giữ nguyên logic BtnSave_Click, chỉ thay đổi cách truy cập InnerCombo và TextField
        var aircraft = cbAircraft.InnerCombo.SelectedItem as ComboboxItem;
        var seat = cbSeat.InnerCombo.SelectedItem as ComboboxItem;
        var cls = cbClass.InnerCombo.SelectedItem as ComboboxItem;

        if (aircraft == null || seat == null || cls == null)
        {
            MessageBox.Show("Vui lòng chọn đầy đủ máy bay, số ghế và hạng ghế.",
                "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var rawPriceText = txtPrice.InnerTextBox.Text ?? "";
        var cleaned = Regex.Replace(rawPriceText, @"[^\d]", "");

        if (!decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out var price) || price <= 0)
        {
            MessageBox.Show("Giá cơ bản không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SelectedFlightId = aircraft.Id;
        SelectedSeatId = seat.Id;
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