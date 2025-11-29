using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace GUI.Components.Inputs {
    [DesignerCategory("Code")]
    [ToolboxItem(true)]
    public class DateTimePickerCustom : UserControl {
        private readonly Label _lbl;
        private readonly DateTimePicker _dtp;

        public DateTimePickerCustom() : this("Nhãn", "") { }

        public DateTimePickerCustom(string labelText, string placeholder) {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            DoubleBuffered = true;
            BackColor = Color.Transparent;

            _lbl = new Label {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.FromArgb(70, 70, 70),
                BackColor = Color.Transparent
            };

            _dtp = new DateTimePicker {
                Format = DateTimePickerFormat.Short,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                CalendarForeColor = Color.FromArgb(0, 92, 175),
                CalendarMonthBackground = Color.White,
                Width = 140,
                BackColor = Color.White,
                MinDate = DateTime.MinValue,
                MaxDate = DateTime.MaxValue
            };

            // Bỏ viền mặc định
            _dtp.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, _dtp.ClientRectangle, Color.White, ButtonBorderStyle.None);
            };

            // NEW: chuyển tiếp sự kiện ValueChanged ra ngoài control bọc
            _dtp.ValueChanged += (s, e) => OnValueChanged(e); // NEW

            Padding = new Padding(0, 4, 0, 8);
            Controls.Add(_dtp);
            Controls.Add(_lbl);

            LabelText = labelText;
            PlaceholderText = placeholder;
        }

        // ====== PROPERTIES ======

        [Category("Appearance")]
        public string LabelText {
            get => _lbl.Text;
            set { _lbl.Text = value; Invalidate(); }
        }

        [Category("Appearance")]
        public string PlaceholderText {
            get => _placeholder;
            set { _placeholder = value ?? string.Empty; Invalidate(); }
        }
        private string _placeholder = string.Empty;

        [Category("Behavior")]
        public DateTime Value {
            get => _dtp.Value;
            set { _dtp.Value = value; Invalidate(); }
        }

        [Category("Behavior")]
        public DateTimePicker DateTimePicker => _dtp;

        // ✅ Thuộc tính định dạng ngày tháng
        [Category("Behavior"), Description("Định dạng hiển thị ngày tháng (ví dụ: dd/MM/yyyy).")]
        public string CustomFormat {
            get => _dtp.CustomFormat;
            set {
                if (!string.IsNullOrWhiteSpace(value)) {
                    _dtp.Format = DateTimePickerFormat.Custom;
                    _dtp.CustomFormat = value;
                }
            }
        }

        // ✅ Giới hạn ngày
        [Category("Behavior"), Description("Giới hạn ngày tối đa có thể chọn.")]
        public DateTime MaxDate {
            get => _dtp.MaxDate;
            set { _dtp.MaxDate = value; }
        }

        [Category("Behavior"), Description("Giới hạn ngày tối thiểu có thể chọn.")]
        public DateTime MinDate {
            get => _dtp.MinDate;
            set { _dtp.MinDate = value; }
        }

        // =========================
        // 🔥 NEW: Bật/tắt chọn Giờ:Phút
        // =========================

        private bool _enableTime; // NEW

        /// <summary>
        /// Bật chọn giờ:phút (dùng spinner). Khi bật sẽ dùng TimeFormat (mặc định "dd/MM/yyyy HH:mm").
        /// </summary>
        [Category("Behavior"), Description("Bật chọn giờ:phút bằng spinner.")]
        public bool EnableTime { // NEW
            get => _enableTime;
            set {
                _enableTime = value;
                ApplyFormat(); // cập nhật Format/CustomFormat/ShowUpDown theo trạng thái mới
            }
        }

        private string _timeFormat = "dd/MM/yyyy HH:mm"; // NEW

        /// <summary>
        /// Định dạng khi EnableTime=true. Ví dụ: \"HH:mm dd/MM/yyyy\" hoặc \"dd/MM/yyyy HH:mm\".
        /// </summary>
        [Category("Behavior"), Description("Định dạng khi EnableTime=true (ví dụ: dd/MM/yyyy HH:mm).")]
        public string TimeFormat { // NEW
            get => _timeFormat;
            set {
                _timeFormat = string.IsNullOrWhiteSpace(value) ? "dd/MM/yyyy HH:mm" : value;
                if (_enableTime) ApplyFormat();
            }
        }

        private bool _showUpDownWhenTime = true; // NEW

        /// <summary>
        /// Khi EnableTime=true, có hiển thị spinner UpDown không (khuyến nghị: true).
        /// </summary>
        [Category("Behavior"), Description("Khi EnableTime=true, sử dụng spinner UpDown thay vì popup calendar.")]
        public bool ShowUpDownWhenTime { // NEW
            get => _showUpDownWhenTime;
            set {
                _showUpDownWhenTime = value;
                if (_enableTime) ApplyFormat();
            }
        }

        // NEW: Cho phép chuyển tiếp trực tiếp thuộc tính ShowUpDown nếu muốn dùng cả khi chỉ chọn ngày
        [Category("Behavior"), Description("Bật spinner UpDown trực tiếp cho DateTimePicker bên trong.")]
        public bool ShowUpDown {
            get => _dtp.ShowUpDown;
            set => _dtp.ShowUpDown = value;
        }

        // NEW: Phương thức áp định dạng phù hợp
        private void ApplyFormat() {
            if (_enableTime) {
                _dtp.Format = DateTimePickerFormat.Custom;
                _dtp.CustomFormat = _timeFormat;
                _dtp.ShowUpDown = _showUpDownWhenTime;
            } else {
                // Quay về chọn ngày bình thường
                if (string.IsNullOrWhiteSpace(_dtp.CustomFormat)) {
                    _dtp.Format = DateTimePickerFormat.Short;
                } else {
                    // Nếu dev đã set CustomFormat bằng property CustomFormat, tôn trọng nó
                    _dtp.Format = DateTimePickerFormat.Custom;
                }
                _dtp.ShowUpDown = false;
            }
            Invalidate();
        }

        // NEW: Phát sự kiện ValueChanged ra ngoài
        public event EventHandler? ValueChanged;
        protected virtual void OnValueChanged(EventArgs e) => ValueChanged?.Invoke(this, e);
    }
}
