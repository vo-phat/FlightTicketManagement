using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Components.Inputs {
    // ComboBox có label + underline, nền đồng bộ với parent, dùng mũi tên native
    public class UnderlinedComboBox : UserControl {
        private readonly Label _lblCaption;
        private readonly ComboBox _combo;

        // style
        private Color _lineColor = Color.FromArgb(0, 92, 175);
        private Color _hoverLine = Color.FromArgb(0, 120, 215);
        private bool _hover;
        private const int CONTROL_HEIGHT = 56;
        private const int LINE_THICK = 2;

        public UnderlinedComboBox() : this("Label", Array.Empty<string>()) { }

        private static Color GetEffectiveOpaqueBackColor(Control? c) {
            // đi lên đến khi gặp màu không Transparent
            while (c != null && (c.BackColor.A < 255 || c.BackColor == Color.Transparent)) {
                c = c.Parent;
            }
            // Nếu vẫn không tìm thấy, fallback trắng
            var col = c?.BackColor ?? Color.White;
            // đảm bảo alpha 255
            return Color.FromArgb(255, col.R, col.G, col.B);
        }

        public UnderlinedComboBox(string label, object[] items) {
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Height = CONTROL_HEIGHT;
            MinimumSize = new Size(140, CONTROL_HEIGHT);
            Padding = new Padding(0);

            _lblCaption = new Label {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 22,
                Text = label,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.FromArgb(70, 70, 70),
                Margin = new Padding(0)
            };

            // host để đồng bộ nền với parent (giống UnderlinedTextField)
            var inputHost = new Panel {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 2, 0, 0),
                BackColor = Color.Transparent
            };

            _combo = new ComboBox {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDown,         // cho phép gõ
                FlatStyle = FlatStyle.Popup,                    // tránh mảng xám đậm
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                BackColor = Color.White,                        // giống textfield
                Margin = new Padding(0),
                IntegralHeight = false,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };

            _combo.MouseEnter += (_, __) => { _hover = true; Invalidate(); };
            _combo.MouseLeave += (_, __) => { _hover = false; Invalidate(); };
            _combo.SelectedIndexChanged += (_, __) => OnSelectedIndexChanged(EventArgs.Empty);

            // khi parent đổi nền (như #E8F0FC), đồng bộ nền khu vực input
            ParentChanged += (_, __) => SyncBackColorWithParent();
            VisibleChanged += (_, __) => SyncBackColorWithParent();
            BackColorChanged += (_, __) => SyncBackColorWithParent();

            inputHost.Controls.Add(_combo);
            Controls.Add(inputHost);
            Controls.Add(_lblCaption);

            if (items != null && items.Length > 0) _combo.Items.AddRange(items);

            Paint += (_, e) => {
                using var pen = new Pen(_hover ? _hoverLine : _lineColor, LINE_THICK);
                var y = Height - 1;
                e.Graphics.DrawLine(pen, 0, y, Width, y);
            };

            Resize += (_, __) => Invalidate();
        }

        private void SyncBackColorWithParent() {
            // Màu “vùng nhập” muốn khớp nền container? lấy màu opaque hữu hiệu
            // Nếu bạn thích giữ nền trắng cho input (giống UnderlinedTextField), comment dòng dưới.
            var col = GetEffectiveOpaqueBackColor(Parent);
            _combo.BackColor = col;

            // Nếu bạn có một inputHost (Panel chứa ComboBox), gán luôn cho đồng bộ viền:
            if (Parent is not null && _combo.Parent is Control host) {
                host.BackColor = col;
            }
        }

        // ===== API public =====
        [Category("Appearance")]
        public string LabelText {
            get => _lblCaption.Text;
            set => _lblCaption.Text = value;
        }

        [Browsable(false)]
        public object? SelectedItem {
            get => _combo.SelectedItem;
            set => _combo.SelectedItem = value;
        }

        [Browsable(false)]
        public int SelectedIndex {
            get => _combo.SelectedIndex;
            set => _combo.SelectedIndex = value;
        }

        [Browsable(false)]
        public string SelectedText {
            get {
                if (_combo.DropDownStyle == ComboBoxStyle.DropDown)
                    return _combo.Text; // khi gõ tay
                return _combo.SelectedItem?.ToString() ?? "";
            }
            set {
                _combo.Text = value;
            }
        }
        [Browsable(false)]
        [Description("Gets or sets the value of the member property specified by the ValueMember property.")]
        public object SelectedValue
        {
            get => _combo.SelectedValue;
            set => _combo.SelectedValue = value;
        }
        [Category("Data")]
        [Description("Gets or sets the data source for this ComboBox.")]
        public object DataSource
        {
            get => _combo.DataSource;
            set => _combo.DataSource = value;
        }

        [Category("Data")]
        [Description("Gets or sets the property to display for this ComboBox.")]
        public string DisplayMember
        {
            get => _combo.DisplayMember;
            set => _combo.DisplayMember = value;
        }

        [Category("Data")]
        [Description("Gets or sets the property to use as the actual value for the items.")]
        public string ValueMember
        {
            get => _combo.ValueMember;
            set => _combo.ValueMember = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ComboBox.ObjectCollection Items => _combo.Items;

        public event EventHandler? SelectedIndexChanged;
        protected virtual void OnSelectedIndexChanged(EventArgs e) => SelectedIndexChanged?.Invoke(this, e);

        public override Size GetPreferredSize(Size proposedSize)
            => new Size(base.GetPreferredSize(proposedSize).Width, CONTROL_HEIGHT);
    }
}
