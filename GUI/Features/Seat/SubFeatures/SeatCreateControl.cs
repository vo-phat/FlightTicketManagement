//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Windows.Forms;
//using GUI.Components.Buttons;
//using GUI.Components.Inputs;
//using BUS.Seat;
//using DTO.Seat;

//namespace GUI.Features.Seat.SubFeatures {
//    // DTO tạm thời cho ComboBox (chứa cả tên và ID)
//    public class ComboboxItem {
//        public string Name { get; set; }
//        public int Id { get; set; }
//        public override string ToString() => Name;
//    }

//    public class SeatCreateControl : UserControl {
//        private readonly SeatBUS _seatBUS;
//        private SeatDTO? _seatToEdit;

//        private TableLayoutPanel root, form;
//        private Label lblTitle;
//        private UnderlinedComboBox cbAircraft, cbClass;
//        private UnderlinedTextField txtSeat;
//        private PrimaryButton btnSave;
//        private SecondaryButton btnReset;

//        private List<ComboboxItem> _aircraftItems = new List<ComboboxItem>();
//        private List<ComboboxItem> _classItems = new List<ComboboxItem>();

//        public event Action SeatCreated;
//        public event Action EditCancelled;

//        public SeatCreateControl() {
//            _seatBUS = new SeatBUS();
//            InitializeComponent();
//            LoadComboboxData();
//            SetCreateMode();
//        }

//        private void InitializeComponent() {
//            SuspendLayout();
//            Dock = DockStyle.Fill;
//            BackColor = Color.FromArgb(232, 240, 252);

//            lblTitle = new Label {
//                Text = "➕ Tạo ghế",
//                AutoSize = true,
//                Font = new Font("Segoe UI", 20, FontStyle.Bold),
//                Padding = new Padding(24, 20, 24, 0),
//                Dock = DockStyle.Top
//            };

//            form = new TableLayoutPanel {
//                Dock = DockStyle.None,
//                Anchor = AnchorStyles.Top | AnchorStyles.None,
//                AutoSize = true,
//                Padding = new Padding(24),
//                ColumnCount = 2
//            };
//            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
//            form.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

//            cbAircraft = new UnderlinedComboBox("Máy bay", Array.Empty<object>()) { Width = 260 };
//            cbClass = new UnderlinedComboBox("Hạng ghế", Array.Empty<object>()) { Width = 260 };
//            txtSeat = new UnderlinedTextField("Số ghế (VD: 12A)", "") { Width = 260 };

//            form.Controls.Add(new Label { Text = "Máy bay", AutoSize = true, Margin = new Padding(0, 8, 8, 8) }, 0, 0);
//            form.Controls.Add(cbAircraft, 1, 0);
//            form.Controls.Add(new Label { Text = "Hạng ghế", AutoSize = true, Margin = new Padding(0, 8, 8, 8) }, 0, 1);
//            form.Controls.Add(cbClass, 1, 1);
//            form.Controls.Add(new Label { Text = "Số ghế", AutoSize = true, Margin = new Padding(0, 8, 8, 8) }, 0, 2);
//            form.Controls.Add(txtSeat, 1, 2);

//            var actions = new FlowLayoutPanel {
//                Dock = DockStyle.None,
//                Anchor = AnchorStyles.Top | AnchorStyles.None,
//                AutoSize = true,
//                Height = 48,
//                Padding = new Padding(24, 6, 24, 6),
//                WrapContents = false
//            };
//            btnSave = new PrimaryButton("💾 Lưu") { Width = 100, Height = 36 };
//            btnReset = new SecondaryButton("✖ Hủy") { Width = 110, Height = 36, Margin = new Padding(12, 0, 0, 0) };

//            btnSave.Click += Save_Click;
//            btnReset.Click += Reset_Click;
//            actions.Controls.AddRange(new Control[] { btnSave, btnReset });

//            root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 3 };
//            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
//            root.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
//            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

//            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

//            root.Controls.Add(lblTitle, 0, 0);
//            root.SetColumnSpan(lblTitle, 3);
//            root.Controls.Add(form, 1, 1);
//            root.Controls.Add(actions, 1, 2);

//            // thiết lập DisplayMember / ValueMember
//            if (cbAircraft.InnerCombo is ComboBox rawCbAircraft) {
//                rawCbAircraft.DisplayMember = "Name";
//                rawCbAircraft.ValueMember = "Id";
//            }
//            if (cbClass.InnerCombo is ComboBox rawCbClass) {
//                rawCbClass.DisplayMember = "Name";
//                rawCbClass.ValueMember = "Id";
//            }

//            Controls.Add(root);
//            ResumeLayout(false);
//        }

//        private void Reset_Click(object? sender, EventArgs e) {
//            EditCancelled?.Invoke();
//            SetCreateMode();
//        }

//        // 🔹 Đổi thành public để SeatControl có thể gọi được
//        public void LoadComboboxData() {
//            try {
//                var allSeats = _seatBUS.GetAllSeatsWithDetails();

//                _aircraftItems = allSeats
//                    .Select(s => new { s.AircraftId, Name = $"{s.AircraftManufacturer} {s.AircraftModel}" })
//                    .Distinct()
//                    .Select(a => new ComboboxItem { Id = a.AircraftId, Name = a.Name })
//                    .OrderBy(a => a.Name)
//                    .ToList();

//                _classItems = allSeats
//                    .Select(s => new { s.ClassId, s.ClassName })
//                    .Distinct()
//                    .Select(c => new ComboboxItem { Id = c.ClassId, Name = c.ClassName })
//                    .OrderBy(c => c.Name)
//                    .ToList();

//                if (cbAircraft.InnerCombo is ComboBox rawCbAircraft)
//                    rawCbAircraft.DataSource = _aircraftItems;

//                if (cbClass.InnerCombo is ComboBox rawCbClass)
//                    rawCbClass.DataSource = _classItems;
//            } catch (Exception ex) {
//                MessageBox.Show("Không thể tải dữ liệu máy bay và hạng ghế: " + ex.Message,
//                    "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        public void SetCreateMode() {
//            _seatToEdit = null;
//            lblTitle.Text = "➕ Tạo ghế";
//            txtSeat.Text = "";

//            if (cbAircraft.InnerCombo is ComboBox rawCbAircraft) {
//                if (rawCbAircraft.DataSource != null)
//                    rawCbAircraft.SelectedIndex = -1; // an toàn cho data-bound combobox
//                else
//                    rawCbAircraft.SelectedItem = null; // an toàn khi không có DataSource
//            }

//            if (cbClass.InnerCombo is ComboBox rawCbClass) {
//                if (rawCbClass.DataSource != null)
//                    rawCbClass.SelectedIndex = -1;
//                else
//                    rawCbClass.SelectedItem = null;
//            }

//            btnReset.Text = "✖ Hủy";
//        }

//        public void LoadSeatForEdit(int seatId) {
//            if (_aircraftItems == null || _aircraftItems.Count == 0 ||
//                _classItems == null || _classItems.Count == 0) {
//                LoadComboboxData();
//            }

//            var freshSeatData = _seatBUS.GetAllSeatsWithDetails()
//                .FirstOrDefault(s => s.SeatId == seatId);

//            if (freshSeatData == null) {
//                MessageBox.Show("Không tìm thấy ghế để sửa");
//                return;
//            }

//            _seatToEdit = freshSeatData;
//            lblTitle.Text = $"✏️ Sửa ghế #{freshSeatData.SeatId}";
//            txtSeat.Text = freshSeatData.SeatNumber;

//            if (cbAircraft.InnerCombo is ComboBox rawCbAircraft)
//                rawCbAircraft.SelectedIndex = _aircraftItems.FindIndex(a => a.Id == freshSeatData.AircraftId);

//            if (cbClass.InnerCombo is ComboBox rawCbClass)
//                rawCbClass.SelectedIndex = _classItems.FindIndex(c => c.Id == freshSeatData.ClassId);

//            btnReset.Text = "✖ Hủy";
//        }

//        private void Save_Click(object? sender, EventArgs e) {
//            var rawCbAircraft = cbAircraft.InnerCombo as ComboBox;
//            var rawCbClass = cbClass.InnerCombo as ComboBox;

//            bool isEditing = _seatToEdit != null;
//            var seatNumber = (txtSeat.Text ?? "").Trim().ToUpper();

//            int? aircraftId = rawCbAircraft?.SelectedValue as int? ??
//                              (rawCbAircraft?.SelectedItem as ComboboxItem)?.Id;
//            int? classId = rawCbClass?.SelectedValue as int? ??
//                           (rawCbClass?.SelectedItem as ComboboxItem)?.Id;

//            if (aircraftId == null || aircraftId <= 0) {
//                MessageBox.Show("Vui lòng chọn Máy bay hợp lệ.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }
//            if (classId == null || classId <= 0) {
//                MessageBox.Show("Vui lòng chọn Hạng ghế hợp lệ.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            if (!Regex.IsMatch(seatNumber, @"^[1-9]\d*[A-F]$")) {
//                MessageBox.Show("Số ghế không hợp lệ. Ví dụ: 12A.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            var seatToProcess = new SeatDTO(
//                seatId: isEditing ? _seatToEdit!.SeatId : 0,
//                aircraftId: aircraftId.Value,
//                seatNumber: seatNumber,
//                classId: classId.Value
//            );

//            try {
//                bool success;
//                string message;
//                string action = isEditing ? "Cập nhật" : "Thêm mới";

//                if (isEditing)
//                    success = _seatBUS.UpdateSeat(seatToProcess, out message);
//                else
//                    success = _seatBUS.AddSeat(seatToProcess, out message);

//                if (success) {
//                    MessageBox.Show($"{action} ghế thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

//                    if (isEditing)
//                        EditCancelled?.Invoke();
//                    else
//                        SeatCreated?.Invoke();

//                    SetCreateMode();
//                } else {
//                    MessageBox.Show($"Không thể {action} ghế. Chi tiết: " + message, "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            } catch (Exception ex) {
//                MessageBox.Show("Lỗi hệ thống khi lưu ghế: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
//    }
//}