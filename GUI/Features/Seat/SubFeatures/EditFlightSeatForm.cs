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
    private UnderlinedTextField txtSeatNumber;  // ✅ Đổi sang TextField (read-only)
    private UnderlinedTextField txtPrice;
    private PrimaryButton btnSave;
    private SecondaryButton btnCancel;

    // Data lists
    private List<ComboboxItem> _classItems = new();

    // Current values
    private int _currentSeatId;
    private string _currentSeatNumber;
    private int _currentClassId;
    private decimal _currentPrice;

    public EditFlightSeatForm(int flightSeatId, int seatId, string seatNumber, int classId, decimal price)
    {
        FlightSeatId = flightSeatId;
        _currentSeatId = seatId;
        _currentSeatNumber = seatNumber;
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

        // TextField số ghế (READ-ONLY)
        txtSeatNumber = new UnderlinedTextField("Số ghế", "")
        {
            Width = itemWidth,
            Height = 60,
            Margin = new Padding(0, 0, 0, 20)
        };
        txtSeatNumber.InnerTextBox.ReadOnly = true;
        txtSeatNumber.InnerTextBox.BackColor = Color.FromArgb(245, 245, 245);
        txtSeatNumber.InnerTextBox.ForeColor = Color.Black;

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
        flowInputs.Controls.Add(txtSeatNumber);
        flowInputs.Controls.Add(CreateSpacer(10));
        flowInputs.Controls.Add(cbClass);
        flowInputs.Controls.Add(CreateSpacer(10));
        flowInputs.Controls.Add(txtPrice);

        pnlInputs.Controls.Add(flowInputs);

        // 4. Add vào Form
        Controls.Add(pnlInputs);
        Controls.Add(pnlBottom);

        // Events
        btnSave.Click += BtnSave_Click;
        btnCancel.Click += (_, __) => Close();
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

            _currentClassId = currentSeat.ClassId;
            _currentSeatNumber = currentSeat.SeatNumber;

            // Hiển thị số ghế (read-only)
            txtSeatNumber.Text = _currentSeatNumber;

            // Load TẤT CẢ hạng ghế từ database
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
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SelectCurrentValues()
    {
        // Chọn hạng ghế hiện tại
        if (cbClass.InnerCombo is ComboBox rawClass)
        {
            var currentClassIndex = _classItems.FindIndex(c => c.Id == _currentClassId);
            if (currentClassIndex >= 0)
            {
                rawClass.SelectedIndex = currentClassIndex;
            }
        }
    }



    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var cls = cbClass.InnerCombo.SelectedItem as ComboboxItem;

        if (cls == null)
        {
            MessageBox.Show("Vui lòng chọn hạng ghế.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var rawPrice = Regex.Replace(txtPrice.InnerTextBox.Text ?? "", @"[^\d]", "");
        if (!decimal.TryParse(rawPrice, out var price) || price <= 0)
        {
            MessageBox.Show("Giá không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // ✅ GIỮ NGUYÊN seat_id, chỉ đổi class_id và price
        SelectedClassId = cls.Id;
        SelectedSeatId = _currentSeatId;  // Giữ nguyên seat_id
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