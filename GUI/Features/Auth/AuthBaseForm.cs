using System.Drawing;
using System.Windows.Forms;
using GUI.Properties;
using GUI.Components.Buttons;
using GUI.Components.Inputs;

namespace GUI.Features.Auth {
    public class AuthBaseForm : Form {
        protected Panel content;       // nơi đặt controls chính
        protected Label? title;

        public AuthBaseForm(string titleText) {
            // --- Khung form & nền ---
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            BackgroundImage = Resources.login;    // ảnh nền máy bay
            BackgroundImageLayout = ImageLayout.Stretch;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(860, 720);

            // Lớp phủ mờ để tăng độ tương phản
            var overlay = new Panel {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(10, 0, 0, 0)
            };
            Controls.Add(overlay);

            // --- Vùng nội dung tự co giãn ---
            content = new Panel {
                BackColor = Color.Transparent,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            overlay.Controls.Add(content);

            // Căn giữa khi content thay đổi kích thước
            content.SizeChanged += (_, __) => {
                RecenterChildren();            // canh giữa lại con
                CenterContentHorizontally();   // canh giữa content
            };

            // --- Tiêu đề ---
            title = new Label {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Text = titleText,
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.Transparent
            };
            content.Controls.Add(title);

            // Khi form đổi size / hiển thị lần đầu, căn giữa lại
            Resize += (_, __) => CenterContentHorizontally();
            Shown += (_, __) => CenterContentHorizontally();
        }

        // Căn giữa panel content trong form
        protected void CenterContentHorizontally() {
            // Căn giữa theo trục X và Y, đẩy lên một chút
            int x = (ClientSize.Width - content.Width) / 2;
            int y = (ClientSize.Height - content.Height) / 2 - 20;

            // tránh giá trị âm nếu form quá nhỏ
            content.Left = x < 0 ? 0 : x;
            content.Top = y < 0 ? 0 : y;
        }

        // Canh giữa những control nên đặt giữa (textfield, primary button)
        protected virtual void RecenterChildren() {
            foreach (Control c in content.Controls) {
                if (ShouldCenter(c)) CenterX(c);
            }
        }

        // Quy tắc chọn control cần canh giữa
        protected bool ShouldCenter(Control c) =>
            c is UnderlinedTextField ||
            c is PrimaryButton;

        /// <summary>
        /// Tạo một panel “hàng link” rộng bằng control tham chiếu (alignTo),
        /// luôn căn-phải LinkLabel và tự động "bám" khi alignTo di chuyển/đổi kích thước.
        /// </summary>
        protected Panel CreateRightAlignedLinkRow(Control alignTo, string linkText, EventHandler onClick) {
            var row = new Panel {
                Width = alignTo.Width,
                Height = 24,
                Left = alignTo.Left,
                Top = alignTo.Bottom + 8,
                BackColor = Color.Transparent
            };

            var link = new LinkLabel {
                Text = linkText,
                AutoSize = true,
                LinkColor = Color.FromArgb(0, 92, 175),       // màu dễ nhìn trên nền sáng
                ActiveLinkColor = Color.FromArgb(0, 92, 175),
                VisitedLinkColor = Color.FromArgb(0, 92, 175),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            row.Controls.Add(link);

            // Căn phải link trong row
            void RightAlignLink() {
                int x = row.Width - link.PreferredWidth;
                if (x < 0) x = 0;
                link.Location = new Point(x, 0);
            }
            RightAlignLink();

            link.Click += onClick;

            // Khi row đổi size (do content AutoSize), cập nhật vị trí link
            row.SizeChanged += (_, __) => RightAlignLink();

            // 🔗 BÁM THEO control tham chiếu
            void FollowAlignTo(object? s, EventArgs e) {
                row.Left = alignTo.Left;
                row.Width = alignTo.Width;
                row.Top = alignTo.Bottom + 8;
                RightAlignLink();
            }
            alignTo.LocationChanged += FollowAlignTo;
            alignTo.SizeChanged += FollowAlignTo;

            return row;
        }

        /// Căn control theo giữa nội dung theo trục X
        protected void CenterX(Control c) => c.Left = (content.Width - c.Width) / 2;

        /// Điều hướng: ẩn form hiện tại và mở form đích.
        protected void Navigate(Form next, bool reopenCurrent = true)
        {
            Hide(); // Ẩn form hiện tại
            next.StartPosition = FormStartPosition.CenterScreen;

            // 🔹 Mở form kế tiếp dạng hộp thoại (modal)
            next.ShowDialog();

            // 🔹 Chỉ hiện lại form hiện tại nếu được yêu cầu
            if (reopenCurrent)
                Show();
            else
                Close(); // Đăng nhập xong thì đóng luôn
        }



    }
}
