using System.Drawing;
using System.Windows.Forms;
using GUI.Properties;
using GUI.Components.Buttons;
using GUI.Components.Inputs;

namespace GUI.Features.Auth
{

    // 1. Tạo class Panel hỗ trợ chống giật (Double Buffer)
    // Bạn có thể để class này bên trong file này hoặc tách ra file riêng
    public class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }
    }

    public class AuthBaseForm : Form
    {
        // 2. Đổi kiểu dữ liệu từ Panel thường sang BufferedPanel
        protected BufferedPanel content;
        protected Label? title;

        public AuthBaseForm(string titleText)
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            StartPosition = FormStartPosition.CenterScreen;
            BackgroundImage = Resources.login;
            BackgroundImageLayout = ImageLayout.Stretch;
            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Maximized;

            var overlay = new BufferedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(10, 0, 0, 0) 
            };
            Controls.Add(overlay);

            content = new BufferedPanel
            {
                BackColor = Color.Transparent,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            overlay.Controls.Add(content);

            content.SizeChanged += (_, __) => {
                RecenterChildren();
                CenterContentHorizontally();
            };

            title = new Label
            {
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

            Resize += (_, __) => CenterContentHorizontally();
            Shown += (_, __) => CenterContentHorizontally();
        }


        protected void CenterContentHorizontally()
        {
            int x = (ClientSize.Width - content.Width) / 2;
            int y = (ClientSize.Height - content.Height) / 2 - 20;
            content.Left = x < 0 ? 0 : x;
            content.Top = y < 0 ? 0 : y;
        }

        protected virtual void RecenterChildren()
        {
            foreach (Control c in content.Controls)
            {
                if (ShouldCenter(c)) CenterX(c);
            }
        }

        protected bool ShouldCenter(Control c) =>
            c is UnderlinedTextField ||
            c is PrimaryButton;

        protected Panel CreateRightAlignedLinkRow(Control alignTo, string linkText, EventHandler onClick)
        {
            var row = new Panel
            {
                Width = alignTo.Width,
                Height = 24,
                Left = alignTo.Left,
                Top = alignTo.Bottom + 8,
                BackColor = Color.Transparent
            };

            var link = new LinkLabel
            {
                Text = linkText,
                AutoSize = true,
                LinkColor = Color.FromArgb(0, 92, 175),
                ActiveLinkColor = Color.FromArgb(0, 92, 175),
                VisitedLinkColor = Color.FromArgb(0, 92, 175),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            row.Controls.Add(link);

            void RightAlignLink()
            {
                int x = row.Width - link.PreferredWidth;
                if (x < 0) x = 0;
                link.Location = new Point(x, 0);
            }
            RightAlignLink();

            link.Click += onClick;
            row.SizeChanged += (_, __) => RightAlignLink();

            void FollowAlignTo(object? s, EventArgs e)
            {
                row.Left = alignTo.Left;
                row.Width = alignTo.Width;
                row.Top = alignTo.Bottom + 8;
                RightAlignLink();
            }
            alignTo.LocationChanged += FollowAlignTo;
            alignTo.SizeChanged += FollowAlignTo;

            return row;
        }

        protected void CenterX(Control c) => c.Left = (content.Width - c.Width) / 2;

        protected void Navigate(Form next)
        {
            next.StartPosition = FormStartPosition.CenterScreen;
            next.Show();
            Hide();
            next.FormClosed += (_, __) => Close();
        }
    }
}