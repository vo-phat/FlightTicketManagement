using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace FlightTicketManagement.GUI.Components.Inputs {
    [DesignerCategory("Code")]
    [ToolboxItem(true)]
    public class UnderlinedTextField : UserControl {
        // Win32 cue banner (placeholder)
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        private readonly Label _lbl;
        private readonly TextBox _tb;

        public UnderlinedTextField() : this("Nhãn", "Placeholder") { }

        public UnderlinedTextField(string labelText, string placeholder) {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            DoubleBuffered = true;
            BackColor = Color.Transparent; // cho phép trong suốt bề mặt control

            _lbl = new Label {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.FromArgb(70, 70, 70),
                BackColor = Color.Transparent
            };

            _tb = new TextBox {
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Top,
                Margin = new Padding(0),
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                Height = TextRenderer.MeasureText("Ag", new Font("Segoe UI", 14f, FontStyle.Bold)).Height + 4
            };

            Padding = new Padding(0, 4, 0, 8);
            Controls.Add(_tb);
            Controls.Add(_lbl);

            LabelText = labelText;
            PlaceholderText = placeholder;

            // events
            _tb.GotFocus += (_, __) => { _focused = true; Invalidate(); };
            _tb.LostFocus += (_, __) => { _focused = false; Invalidate(); };
            _tb.TextChanged += (_, __) => Invalidate();
            Resize += (_, __) => Invalidate();

            ParentChanged += (_, __) => SyncTextBoxBackColor();
            _tb.HandleCreated += (_, __) => SetPlaceholder(_placeholder);
        }

        // ===== Public API =====
        [Category("Appearance")]
        public string LabelText { get => _lbl.Text; set { _lbl.Text = value; Invalidate(); } }

        [Category("Appearance")]
        public string PlaceholderText { get => _placeholder; set { _placeholder = value ?? string.Empty; SetPlaceholder(_placeholder); Invalidate(); } }
        private string _placeholder = string.Empty;

        [Category("Behavior")]
        public override string Text { get => _tb.Text; set { _tb.Text = value ?? string.Empty; Invalidate(); } }

        [Category("Behavior"), Description("Ẩn ký tự nhập bằng ký tự hệ thống (●).")]
        public bool UseSystemPasswordChar { get => _tb.UseSystemPasswordChar; set { _tb.UseSystemPasswordChar = value; SetPlaceholder(_placeholder); Invalidate(); } }

        [Category("Behavior"), Description("Ký tự ẩn mật khẩu khi UseSystemPasswordChar = false.")]
        public char PasswordChar { get => _tb.PasswordChar; set { _tb.PasswordChar = value; SetPlaceholder(_placeholder); Invalidate(); } }

        // ===== ReadOnly mới thêm =====
        [Category("Behavior"), Description("Chỉ đọc (không cho sửa) nhưng vẫn hiển thị bình thường.")]
        public bool ReadOnly {
            get => _tb.ReadOnly;
            set {
                if (_tb.ReadOnly == value) return;
                _tb.ReadOnly = value;

                // UX: khi readonly -> không cho tab-focus & đổi cursor
                _tb.TabStop = !value;
                _tb.Cursor = value ? Cursors.Arrow : Cursors.IBeam;

                UpdateReadOnlyStyle();
                ReadOnlyChanged?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        [Category("Property Changed")]
        public event EventHandler? ReadOnlyChanged;

        // Màu khi ReadOnly
        [Category("Appearance")] public Color ReadOnlyLineColor { get; set; } = Color.FromArgb(200, 200, 200);
        [Category("Appearance")] public Color ReadOnlyTextColor { get; set; } = Color.FromArgb(90, 90, 90);

        [Category("Appearance")] public Color LineColor { get; set; } = Color.FromArgb(40, 40, 40);
        [Category("Appearance")] public Color LineColorFocused { get; set; } = Color.FromArgb(0, 92, 175);
        [Category("Appearance")] public int LineThickness { get; set; } = 2;
        [Category("Appearance")] public int FocusedLineThickness { get; set; } = 3;

        [Category("Layout"), Description("Khoảng cách từ đáy TextBox đến gạch chân (px).")]
        public int UnderlineSpacing { get; set; } = 2;

        [Category("Behavior"), Description("Đặt nền TextBox theo nền tổ tiên gần nhất (opaque).")]
        public bool InheritParentBackColor { get; set; } = true;

        [Browsable(false)] public TextBox InnerTextBox => _tb;

        [Category("Appearance")] public Color LabelForeColor { get => _lbl.ForeColor; set { _lbl.ForeColor = value; Invalidate(); } }
        [Category("Appearance")] public Color TextForeColor { get => _tb.ForeColor; set { _tb.ForeColor = value; Invalidate(); } }

        public void UseLightOnDarkTheme() {
            LabelForeColor = Color.White;
            TextForeColor = Color.White;
            LineColor = Color.FromArgb(210, 210, 210);
            LineColorFocused = Color.FromArgb(0, 120, 215);
            _tb.Font = new Font("Segoe UI", 12f, FontStyle.Regular);
        }

        private bool _focused;

        // ---- TRANSPARENCY: vẽ nền của Parent để giả lập trong suốt
        protected override void OnPaintBackground(PaintEventArgs e) {
            if (Parent == null || BackColor.A == 255) {
                base.OnPaintBackground(e); // màu đặc
                return;
            }

            // Vẽ nền của parent (kể cả ảnh nền) “nhìn xuyên” qua control
            var g = e.Graphics;
            var translate = g.Transform;
            try {
                Rectangle rect = new Rectangle(this.Left, this.Top, this.Width, this.Height);
                g.TranslateTransform(-Left, -Top);
                InvokePaintBackground(Parent, new PaintEventArgs(g, rect));
                InvokePaint(Parent, new PaintEventArgs(g, rect));
            } finally {
                g.Transform = translate;
            }
        }

        // ---- đồng bộ màu nền OPAQUE cho TextBox để tránh ArgumentException
        protected override void OnParentBackColorChanged(EventArgs e) {
            base.OnParentBackColorChanged(e);
            SyncTextBoxBackColor();
        }

        protected override void OnCreateControl() {
            base.OnCreateControl();
            _lbl.BringToFront();
            _tb.BringToFront();
            SyncTextBoxBackColor();
            SetPlaceholder(_placeholder);
            UpdateReadOnlyStyle(); // áp style ban đầu theo trạng thái mặc định
        }

        private void SyncTextBoxBackColor() {
            if (!InheritParentBackColor) return;

            _tb.BackColor = GetNearestOpaqueBackColor() ?? SystemColors.Window; // luôn là màu đặc
            Invalidate();
        }

        private Color? GetNearestOpaqueBackColor() {
            Control? p = this;
            while (p != null) {
                var c = p.BackColor;
                if (c != Color.Transparent && c.A == 255) return c;
                p = p.Parent;
            }
            return null;
        }

        private void SetPlaceholder(string text) {
            if (_tb.IsHandleCreated)
                SendMessage(_tb.Handle, EM_SETCUEBANNER, 0, text ?? string.Empty);
            else
                _tb.HandleCreated += (_, __) => SendMessage(_tb.Handle, EM_SETCUEBANNER, 0, text ?? string.Empty);
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int underlineY = Math.Min(_tb.Bottom + UnderlineSpacing, Height - 1);

            // Nếu ReadOnly thì không dùng màu Focused
            var color = ReadOnly ? ReadOnlyLineColor : (_focused ? LineColorFocused : LineColor);
            var thick = ReadOnly ? LineThickness : (_focused ? FocusedLineThickness : LineThickness);

            using var pen = new Pen(color, thick) { Alignment = PenAlignment.Inset };
            g.DrawLine(pen, 0, underlineY, Width, underlineY);
        }

        public override Size GetPreferredSize(Size proposedSize) {
            int w = Math.Max(220, proposedSize.Width);
            int h = Padding.Top + _lbl.Height + _tb.Height + UnderlineSpacing + FocusedLineThickness + Padding.Bottom + 2;
            return new Size(w, h);
        }

        private void UpdateReadOnlyStyle() {
            if (ReadOnly) {
                // Giữ nền theo Parent, chỉ đổi màu chữ nhạt hơn
                _tb.ForeColor = ReadOnlyTextColor;
            } else {
                _tb.ForeColor = TextForeColor; // quay về màu cấu hình
            }
            Invalidate();
        }
    }
}
