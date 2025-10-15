using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace FlightTicketManagement.GUI.Components.Inputs {
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
                MinDate = DateTime.Today,
                Width = 140,
                BackColor = Color.White
            };
            // Loại bỏ viền (border) nếu có
            _dtp.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, _dtp.ClientRectangle, Color.White, ButtonBorderStyle.None);
            };

            Padding = new Padding(0, 4, 0, 8);
            Controls.Add(_dtp);
            Controls.Add(_lbl);

            LabelText = labelText;
            PlaceholderText = placeholder;
        }

        [Category("Appearance")]
        public string LabelText { get => _lbl.Text; set { _lbl.Text = value; Invalidate(); } }

        [Category("Appearance")]
        public string PlaceholderText { get => _placeholder; set { _placeholder = value ?? string.Empty; Invalidate(); } }
        private string _placeholder = string.Empty;

        [Category("Behavior")]
        public DateTime Value { get => _dtp.Value; set { _dtp.Value = value; Invalidate(); } }

        [Category("Behavior")]
        public DateTimePicker DateTimePicker => _dtp;
    }
}
