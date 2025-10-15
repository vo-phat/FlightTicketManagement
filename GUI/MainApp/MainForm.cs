using FlightTicketManagement.GUI.Features.Flight;
using FlightTicketManagement.GUI.Features.Ticket;
using FlightTicketManagement.GUI.Features.Location;
using FlightTicketManagement.GUI.Features.Stats;
using FlightTicketManagement.GUI.Features.Account;
using FlightTicketManagement.GUI.Features.Settings;

namespace FlightTicketManagement.GUI.Features.MainApp {
    public class MainForm : Form {
        private Panel navbarPanel;
        private Panel mainContentPanel;
        private PictureBox defaultPicture;

        // Dictionary để giữ instance UserControl nếu cần giữ trạng thái
        private readonly System.Collections.Generic.Dictionary<string, UserControl> controls = new();

        public MainForm() {
            InitializeComponent();
            BuildNavbar();
            BuildMainContent();
        }

        private void InitializeComponent() {
            this.Text = "Airline Management System";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private Label selectedLabel = null;

        private void BuildNavbar() {
            navbarPanel = new Panel {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(20, 0, 20, 0)
            };
            this.Controls.Add(navbarPanel);

            // Logo
            PictureBox logo = new PictureBox {
                Image = Properties.Resources.logo,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(100, 40),
                Location = new Point(20, 10),
                Cursor = Cursors.Hand
            };

            logo.Click += Logo_Click;

            navbarPanel.Controls.Add(logo);

            string[] menuItems = { "Chuyến bay", "Vé máy bay", "Vị trí", "Thống kê", "Tài khoản và Quyền", "Cài đặt" };

            FlowLayoutPanel menuFlow = new FlowLayoutPanel {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Padding = new Padding(0, 20, 0, 0)
            };
            navbarPanel.Controls.Add(menuFlow);

            int spacing = 50;

            foreach (var item in menuItems) {
                var lbl = new Label {
                    Text = item,
                    AutoSize = true,
                    ForeColor = Color.FromArgb(0, 92, 175),
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(spacing, 0, 0, 0)
                };

                // Hover: underline tạm thời
                lbl.MouseEnter += (s, e) =>
                {
                    if (selectedLabel != lbl)
                        lbl.Font = new Font(lbl.Font, FontStyle.Underline);
                };
                lbl.MouseLeave += (s, e) =>
                {
                    if (selectedLabel != lbl)
                        lbl.Font = new Font(lbl.Font, FontStyle.Regular);
                };

                // Click: giữ underline
                lbl.Click += (s, e) =>
                {
                    if (defaultPicture != null)
                        defaultPicture.Visible = false;

                    if (selectedLabel != null && selectedLabel != lbl)
                        selectedLabel.Font = new Font(selectedLabel.Font, FontStyle.Regular);

                    lbl.Font = new Font(lbl.Font, FontStyle.Underline);
                    selectedLabel = lbl;

                    // Load content
                    switch (item) {
                        case "Chuyến bay":
                            ShowControl("Flight", () => new FlightControl());
                            break;
                        case "Vé máy bay":
                            ShowControl("Ticket", () => new TicketControl());
                            break;
                        case "Vị trí":
                            ShowControl("Location", () => new LocationControl());
                            break;
                        case "Thống kê":
                            ShowControl("Stats", () => new StatsControl());
                            break;
                        case "Tài khoản & Quyền":
                            ShowControl("Account", () => new AccountControl());
                            break;
                        case "Cài đặt":
                            ShowControl("Settings", () => new SettingsControl());
                            break;
                    }
                };

                menuFlow.Controls.Add(lbl);
            }
        }

        private void BuildMainContent() {
            mainContentPanel = new Panel {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, navbarPanel.Height, 0, 0),
                //BackColor = Color.FromArgb(245, 245, 245)
                BackColor = Color.DarkBlue
            };
            this.Controls.Add(mainContentPanel);

            defaultPicture = new PictureBox {
                Image = Properties.Resources.home,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            mainContentPanel.Controls.Add(defaultPicture);
        }

        // Load UserControl vào mainContentPanel
        private void ShowControl(string key, Func<UserControl> creator) {
            if (!controls.ContainsKey(key))
                controls[key] = creator();

            mainContentPanel.Controls.Clear();
            var control = controls[key];
            control.Dock = DockStyle.Fill;
            mainContentPanel.Controls.Add(control);
            control.BringToFront();
        }

        private void Logo_Click(object sender, EventArgs e) {
            // 1. Xóa hoặc ẩn UserControl đang hiển thị
            var userControls = mainContentPanel.Controls.OfType<UserControl>().ToList();
            foreach (var uc in userControls) {
                mainContentPanel.Controls.Remove(uc);
            }

            // 2. Hiển thị lại ảnh mặc định
            if (!mainContentPanel.Controls.Contains(defaultPicture)) {
                mainContentPanel.Controls.Add(defaultPicture);
            }
            defaultPicture.Visible = true;
            defaultPicture.Dock = DockStyle.Fill;
            defaultPicture.BringToFront();

            // 3. Reset link đang chọn
            if (selectedLabel != null) {
                selectedLabel.Font = new Font(selectedLabel.Font, FontStyle.Regular);
                selectedLabel = null;
            }
        }

    }
}
