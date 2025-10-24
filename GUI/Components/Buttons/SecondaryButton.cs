using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace GUI.Components.Buttons {
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [ToolboxItem(true)]
    public class SecondaryButton : Button {
        public SecondaryButton() {
            InitializeDefaults();
        }

        public SecondaryButton(string label, Image? icon = null) {
            InitializeDefaults();
            Text = label ?? string.Empty;
            Icon = icon;
        }

        private void InitializeDefaults() {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;

            // —— màu mặc định
            Font = new Font("Segoe UI", 12f, FontStyle.Bold);

            // xanh thương hiệu
            NormalBackColor = Color.White;
            NormalForeColor = Color.FromArgb(155, 209, 243);
            NormalBorderColor = Color.FromArgb(40, 40, 40);

            HoverBackColor = Color.FromArgb(155, 209, 243);
            HoverForeColor = Color.White;
            HoverBorderColor = Color.FromArgb(0, 92, 175);  // viền xanh

            PressedBackColor = Color.FromArgb(120, 191, 239);
            PressedForeColor = Color.White;
            PressedBorderColor = Color.FromArgb(31, 111, 178);

            BackColor = NormalBackColor;
            ForeColor = NormalForeColor;
            BorderColor = NormalBorderColor;

            BorderThickness = 2;
            CornerRadius = 22;

            Padding = new Padding(24, 10, 24, 10);
            IconSize = new Size(22, 22);
            IconSpacing = 10;

            // —— không giới hạn chiều ngang, không xuống dòng
            WordWrap = false;
            PreferredMaxWidth = 0;

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TextAlign = ContentAlignment.MiddleLeft;
        }

        // ====== Properties ======
        [Category("Appearance")]
        public Image? Icon { get; set; }

        [Category("Appearance")]
        public int CornerRadius {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(0, value); UpdateRegion(); Invalidate(); }
        }
        private int _cornerRadius;

        [Category("Appearance")]
        public Color BorderColor { get; set; }

        [Category("Appearance")]
        public int BorderThickness { get; set; } = 2;

        [Category("Layout")]
        public Size IconSize { get; set; }

        [Category("Layout")]
        public int IconSpacing { get; set; }

        [Category("Behavior")]
        public bool EnableHoverEffects { get; set; } = true;

        [Category("Appearance")]
        public Color NormalBackColor { get; set; }
        [Category("Appearance")]
        public Color NormalForeColor { get; set; }
        [Category("Appearance")]
        public Color NormalBorderColor { get; set; }

        [Category("Appearance")]
        public Color HoverBackColor { get; set; }
        [Category("Appearance")]
        public Color HoverForeColor { get; set; }
        [Category("Appearance")]
        public Color HoverBorderColor { get; set; }

        [Category("Appearance")]
        public Color PressedBackColor { get; set; }
        [Category("Appearance")]
        public Color PressedForeColor { get; set; }
        [Category("Appearance")]
        public Color PressedBorderColor { get; set; }

        [Category("Layout")]
        [Description("Cho phép xuống dòng. Mặc định false.")]
        public bool WordWrap { get; set; }

        [Category("Layout")]
        [Description("Giới hạn bề rộng tối đa khi đo autosize (0 = không giới hạn).")]
        public int PreferredMaxWidth { get; set; }

        private bool _hover, _pressed;

        protected override void OnCreateControl() {
            base.OnCreateControl();
            UpdateRegion();
        }
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            UpdateRegion(); // bo góc thật sự, loại bỏ viền đen
        }

        private void UpdateRegion() {
            if (Width <= 0 || Height <= 0) return;
            var r = ClientRectangle;
            r.Inflate(-1, -1); // tránh aliasing viền
            using var path = GetRoundRect(r, CornerRadius);
            Region?.Dispose();
            Region = new Region(path);
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { if (e.Button == MouseButtons.Left) _pressed = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }

        protected override void OnPaint(PaintEventArgs e) {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            // —— chọn màu theo state
            Color bg = NormalBackColor;
            Color fg = NormalForeColor;
            Color bd = NormalBorderColor;

            if (EnableHoverEffects) {
                if (_pressed) {
                    bg = PressedBackColor;
                    fg = PressedForeColor;
                    bd = PressedBorderColor;
                } else if (_hover) {
                    bg = HoverBackColor;
                    fg = HoverForeColor;
                    bd = HoverBorderColor;
                }
            }

            // —— nền + viền
            var inner = Rectangle.Inflate(rect, -BorderThickness, -BorderThickness);
            using (var path = GetRoundRect(inner, CornerRadius))
            using (var bgBrush = new SolidBrush(bg)) {
                g.FillPath(bgBrush, path);
                if (BorderThickness > 0) {
                    using var pen = new Pen(bd, BorderThickness);
                    g.DrawPath(pen, path);
                }
            }

            // —— vùng content: trừ padding
            var content = new Rectangle(
                inner.Left + Padding.Left,
                inner.Top + Padding.Top,
                inner.Width - Padding.Horizontal,
                inner.Height - Padding.Vertical
            );

            // —— cờ text
            var flags = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter;
            if (WordWrap)
                flags |= TextFormatFlags.WordBreak;
            else
                flags |= TextFormatFlags.SingleLine | TextFormatFlags.LeftAndRightPadding;

            // —— tính vùng icon + text
            Rectangle textRect = content;
            if (Icon != null) {
                int iconW = IconSize.Width;
                int iconH = IconSize.Height;
                int iconX = content.X;
                int iconY = content.Y + (content.Height - iconH) / 2;
                var iconRect = new Rectangle(iconX, iconY, iconW, iconH);

                g.DrawImage(Icon, iconRect);

                int textX = iconRect.Right + IconSpacing;
                textRect = new Rectangle(textX, content.Y, content.Right - textX, content.Height);
            } else if (!WordWrap) {
                flags |= TextFormatFlags.HorizontalCenter;
            }

            // ✅ FIX: Nới thêm vùng text để tránh cắt chân ký tự
            textRect.Y -= 2;
            textRect.Height += 6;

            TextRenderer.DrawText(g, Text ?? string.Empty, Font, textRect, fg, flags);
        }

        public override Size GetPreferredSize(Size proposedSize) {
            // bề rộng tối đa cho text
            int maxWidth;
            if (PreferredMaxWidth > 0) {
                maxWidth = PreferredMaxWidth
                           - Padding.Horizontal
                           - BorderThickness * 2
                           - (Icon != null ? (IconSize.Width + IconSpacing) : 0);
                maxWidth = Math.Max(10, maxWidth);
            } else {
                maxWidth = WordWrap ? 10000 : int.MaxValue;
            }

            // dùng cờ giống OnPaint, thêm padding hợp lý để tránh hụt bên phải
            var flags = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter;
            if (WordWrap) flags |= TextFormatFlags.WordBreak;
            else flags |= TextFormatFlags.SingleLine | TextFormatFlags.LeftAndRightPadding;

            // đo text
            Size textSize = TextRenderer.MeasureText(
                Text ?? string.Empty,
                Font,
                new Size(maxWidth, int.MaxValue),
                flags
            );

            // buffer chống hụt (glyph overhang / DPI)
            const int TextExtraBuffer = 6; // có thể tăng lên 8 nếu DPI cao
            int w = Padding.Left + textSize.Width + Padding.Right + TextExtraBuffer;
            int h = Padding.Top + textSize.Height + Padding.Bottom;

            if (Icon != null) {
                w += IconSize.Width + IconSpacing;
                h = Math.Max(h, Padding.Top + IconSize.Height + Padding.Bottom);
            }

            w += BorderThickness * 2;
            h += BorderThickness * 2;

            h = Math.Max(h, Font.Height + Padding.Vertical + BorderThickness * 2);

            return new Size(w, h);
        }

        protected override void OnTextChanged(EventArgs e) { base.OnTextChanged(e); if (AutoSize) { Invalidate(); PerformLayout(); } }
        protected override void OnFontChanged(EventArgs e) { base.OnFontChanged(e); if (AutoSize) { Invalidate(); PerformLayout(); } }
        protected override void OnPaddingChanged(EventArgs e) { base.OnPaddingChanged(e); if (AutoSize) { Invalidate(); PerformLayout(); } }

        private static GraphicsPath GetRoundRect(Rectangle r, int radius) {
            int d = Math.Max(0, radius) * 2;
            var path = new GraphicsPath();
            if (d <= 0) { path.AddRectangle(r); path.CloseFigure(); return path; }
            path.AddArc(r.Left, r.Top, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
