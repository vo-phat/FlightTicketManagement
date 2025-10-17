using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Components.Link;
using FlightTicketManagement.GUI.Features.Account;
using FlightTicketManagement.GUI.Features.Aircraft;
using FlightTicketManagement.GUI.Features.Airline;
using FlightTicketManagement.GUI.Features.Airport;
using FlightTicketManagement.GUI.Features.CabinClass;
using FlightTicketManagement.GUI.Features.FareRules;
using FlightTicketManagement.GUI.Features.Flight;
using FlightTicketManagement.GUI.Features.Route;
using FlightTicketManagement.GUI.Features.Seat;
using FlightTicketManagement.GUI.Features.Settings;
using FlightTicketManagement.GUI.Features.Stats;
using FlightTicketManagement.GUI.Features.Ticket;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FlightTicketManagement.GUI.Features.MainApp {
    public enum AppRole { User, Staff, Admin }
    public enum NavKey {
        Home, Flights, BookingsTickets, Baggage, Catalogs,
        Payments, Customers, Notifications, Reports, System, MyProfile
    }

    public class NavItem {
        public NavKey Key { get; init; }
        public string Text { get; init; } = "";
        public Func<AppRole, bool> IsVisible { get; init; } = _ => true;
        public Action? OnClick { get; init; } // dùng cho tab không có submenu
        public List<(string text, Func<AppRole, bool> canShow, Action onClick)> SubItems { get; init; } = new();
    }

    public class MainForm : Form {
        private Panel navbarPanel;
        private FlowLayoutPanel navFlow;
        private Panel mainContentPanel;
        private PictureBox defaultPicture;

        // lưu UC theo key để giữ trạng thái (nếu cần)
        private readonly Dictionary<string, UserControl> controls = new();

        private AppRole _role;
        private NavKey _active = NavKey.Home;

        public MainForm() : this(AppRole.Admin) { } // mặc định user
        public MainForm(AppRole role) {
            _role = role;
            InitializeComponent();
            BuildNavbarShell();
            RenderNavbar();
            BuildMainContent();
            ActivateTab(NavKey.Home);
        }

        private void InitializeComponent() {
            Text = "Flight Ticket Management";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
        }

        // ===== Navbar (khung) ===================================================
        private void BuildNavbarShell() {
            navbarPanel = new Panel {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(16, 8, 16, 8)
            };
            Controls.Add(navbarPanel);

            // Logo (click -> về Trang chủ)
            var logo = new PictureBox {
                Image = Properties.Resources.logo,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(120, 44),
                Dock = DockStyle.Left,
                Cursor = Cursors.Hand
            };
            logo.Click += Logo_Click;
            navbarPanel.Controls.Add(logo);

            // Flow menu ngang (phải -> trái)
            navFlow = new FlowLayoutPanel {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Padding = new Padding(0, 4, 0, 0)
            };
            navbarPanel.Controls.Add(navFlow);
        }

        // ===== Đặc tả menu (ẩn/hiện theo quyền) =================================
        private List<NavItem> BuildSpec() {
            return new List<NavItem> {
                new() {
                    Key = NavKey.Home, Text = "🏠 Trang chủ",
                    IsVisible = r => true,
                    OnClick = () => LoadControl(new Label {
                        Text = "Bảng điều khiển",
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Segoe UI", 18, FontStyle.Bold)
                    })
                },
                new() {
                    Key = NavKey.Flights, Text = "✈️ Chuyến bay",
                    IsVisible = r => true,
                    SubItems = {
                        ("Quản lý chuyến bay", r => true,
                            () => LoadControl(new FlightControl())),
                        ("Quy tắc giá vé", r => r == AppRole.Admin,
                            () => OpenFareRules())
                    }
                },
                new() {
                    Key = NavKey.BookingsTickets, Text = "🎟 Đặt chỗ & Vé",
                    IsVisible = r => true,
                    SubItems = {
                        ("Tạo/Tìm đặt chỗ", r => true, () => OpenBookingSearch()),
                        ("Đặt chỗ của tôi", r => r == AppRole.User, () => OpenMyBookings()),
                        ("Quản lý vé (check-in/đổi trạng thái)",
                            r => r is AppRole.Staff or AppRole.Admin, () => OpenTicketOps()),
                        ("Lịch sử vé", r => r == AppRole.Admin, () => OpenTicketHistory())
                    }
                },
                new() {
                    Key = NavKey.Baggage, Text = "🧳 Hành lý",
                    IsVisible = r => r is AppRole.Staff or AppRole.Admin,
                    SubItems = {
                        ("Check-in hành lý / gán tag",
                            r => r is AppRole.Staff or AppRole.Admin, () => OpenBaggageCheckin()),
                        ("Theo dõi trạng thái",
                            r => r is AppRole.Staff or AppRole.Admin, () => OpenBaggageTracking()),
                        ("Báo cáo thất lạc", r => r == AppRole.Admin, () => OpenBaggageReports())
                    }
                },
                new() {
                    Key = NavKey.Catalogs, Text = "📚 Danh mục",
                    IsVisible = r => r == AppRole.Admin,
                    SubItems = {
                        ("Hãng hàng không", r => r == AppRole.Admin, () => OpenAirlines()),
                        ("Máy bay", r => r == AppRole.Admin, () => OpenAircrafts()),
                        ("Sân bay", r => r == AppRole.Admin,
                            () => LoadControl(new AirportControl())),
                        ("Tuyến bay", r => r == AppRole.Admin, () => OpenRoutes()),
                        ("Hạng vé", r => r == AppRole.Admin, () => OpenCabinClasses()),
                        ("Ghế máy bay", r => r == AppRole.Admin, () => OpenSeats())
                    }
                },
                new() {
                    Key = NavKey.Payments, Text = "💳 Thanh toán",
                    IsVisible = r => r is AppRole.Staff or AppRole.Admin,
                    SubItems = {
                        ("POS / Giao dịch", r => r is AppRole.Staff or AppRole.Admin, () => OpenPayments())
                    }
                },
                new() {
                    Key = NavKey.Customers, Text = "👤 Khách hàng",
                    IsVisible = r => true,
                    SubItems = {
                        ("Hồ sơ hành khách", r => true, () => OpenPassengerProfiles()),
                        ("Tài khoản & Quyền", r => r == AppRole.Admin, () => LoadControl(new AccountControl()))
                    }
                },
                new() {
                    Key = NavKey.Notifications, Text = "🔔 Thông báo",
                    IsVisible = r => true,
                    OnClick = () => OpenNotifications()
                },
                new() {
                    Key = NavKey.Reports, Text = "📈 Báo cáo",
                    IsVisible = r => r is AppRole.Staff or AppRole.Admin,
                    OnClick = () => LoadControl(new StatsControl())
                },
                new() {
                    Key = NavKey.System, Text = "⚙️ Hệ thống",
                    IsVisible = r => r == AppRole.Admin,
                    SubItems = {
                        ("Vai trò & phân quyền", r => r == AppRole.Admin, () => OpenRoles()),
                        ("Cấu hình ứng dụng", r => r == AppRole.Admin, () => LoadControl(new SettingsControl()))
                    }
                },
                new() {
                    Key = NavKey.MyProfile, Text = "🙍 Hồ sơ của tôi",
                    IsVisible = r => true,
                    SubItems = {
                        ("Thông tin cá nhân", r => true, () => OpenMyProfile()),
                        ("Đổi mật khẩu", r => true, () => OpenChangePassword()),
                        ("Đăng xuất", r => true, () => DoLogout())
                    }
                }
            };
        }

        // ===== Render navbar theo quyền + trạng thái active ======================
        private void RenderNavbar() {
            navFlow.SuspendLayout();
            navFlow.Controls.Clear();

        var spec = BuildSpec().Where(x => x.IsVisible(_role)).ToList();

        for (int i = 0; i < spec.Count; i++) {
            var item = spec[i];

            // Tạo Link
            var link = new NavLink(item.Text) {
                IsActive = (item.Key == _active),
                Margin = new Padding(6, 4, 6, 0)
            };

                if (item.SubItems.Any()) {
                    var menu = new ContextMenuStrip {
                        Renderer = new LinkMenuRenderer(),
                        ShowImageMargin = false,
                        ShowCheckMargin = false,
                        BackColor = Color.White,
                        Padding = new Padding(4, 4, 4, 4)
                    };

                    foreach (var (text, canShow, onClick) in item.SubItems) {
                        if (!canShow(_role)) continue;

                        var mi = new ToolStripMenuItem(text) {
                            Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                            ForeColor = Color.FromArgb(0, 92, 175),
                            Padding = new Padding(8, 4, 8, 4),
                            Margin = new Padding(2, 2, 2, 2)
                        };
                        mi.Click += (_, __) => { ActivateTab(item.Key); onClick(); };
                        menu.Items.Add(mi);
                    }

                    if (menu.Items.Count > 0) {
                        link.DropMenu = menu; // cơ chế dropdown giữ nguyên
                    } else if (item.OnClick != null) {
                        link.Click += (_, __) => { ActivateTab(item.Key); item.OnClick(); };
                    }
                }


                navFlow.Controls.Add(link);

            if (i < spec.Count - 1) {
                navFlow.Controls.Add(new Label {
                    AutoSize = true,
                    ForeColor = Color.FromArgb(140, 140, 140),
                    Font = new Font("Segoe UI", 11f, FontStyle.Regular),
                    Margin = new Padding(0, 4, 0, 0)
                });
            }
        }

        navFlow.ResumeLayout();
    }

        private void ActivateTab(NavKey key) {
            _active = key;
            RenderNavbar();
        }

        // ===== Main content ======================================================
        private void BuildMainContent() {
            mainContentPanel = new Panel {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };
            Controls.Add(mainContentPanel);
            mainContentPanel.BringToFront();

            defaultPicture = new PictureBox {
                Image = Properties.Resources.home,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            mainContentPanel.Controls.Add(defaultPicture);
        }

        // Load UC vào mainContentPanel (ghi nhớ theo key)
        private void ShowControl(string key, Func<UserControl> creator) {
            if (!controls.ContainsKey(key))
                controls[key] = creator();

            mainContentPanel.Controls.Clear();
            var control = controls[key];
            control.Dock = DockStyle.Fill;
            mainContentPanel.Controls.Add(control);
            control.BringToFront();
        }

        private void LoadControl(Control c) {
            mainContentPanel.Controls.Clear();
            c.Dock = DockStyle.Fill;
            mainContentPanel.Controls.Add(c);
            c.BringToFront();
        }

        private void Logo_Click(object? sender, EventArgs e) {
            // về Trang chủ
            mainContentPanel.Controls.Clear();
            if (!mainContentPanel.Controls.Contains(defaultPicture))
                mainContentPanel.Controls.Add(defaultPicture);
            defaultPicture.Visible = true;
            defaultPicture.BringToFront();
            ActivateTab(NavKey.Home);
        }

        // ===== Các hành động mở màn hình thực tế / stub tạm ======================
        private void OpenCreateFlight() {
            // Có thể mở form tạo hoặc load view tạo ở FlightControl
            ShowControl("Flight", () => new FlightControl());
            // TODO: chuyển tab nội bộ sang "Tạo chuyến bay" nếu cần
        }

        private void OpenFareRules() {
            ShowControl("FareRules", () => new FareRulesControl());
        }

        private void OpenBookingSearch() {
            ShowControl("Ticket", () => new TicketControl());
        }

        private void OpenMyBookings() {
            MessageBox.Show("Đặt chỗ của tôi (User). TODO gắn UserControl lọc theo account_id.", "My Bookings");
        }

        private void OpenTicketOps() {
            MessageBox.Show("Quản lý vé (Staff/Admin) – check-in/đổi trạng thái.", "Ticket Ops");
        }

        private void OpenTicketHistory() {
            MessageBox.Show("Lịch sử vé (Admin).", "Ticket History");
        }

        private void OpenBaggageCheckin() {
            MessageBox.Show("Check-in hành lý / gán tag.", "Baggage");
        }

        private void OpenBaggageTracking() {
            MessageBox.Show("Theo dõi trạng thái hành lý.", "Baggage Tracking");
        }

        private void OpenBaggageReports() {
            MessageBox.Show("Báo cáo thất lạc hành lý (Admin).", "Baggage Reports");
        }

        private void OpenAirlines() {
            ShowControl("Airlines", () => new AirlineControl());
        }

        private void OpenAircrafts() {
            ShowControl("Aircrafts", () => new AircraftControl());
        }

        private void OpenRoutes() {
            ShowControl("Routes", () => new RouteControl());
        }

        private void OpenCabinClasses() {
            ShowControl("CabinClasses", () => new CabinClassControl());
        }

        private void OpenSeats() {
            ShowControl("Seats", () => new SeatControl());
        }

        private void OpenPayments() {
            MessageBox.Show("POS / Giao dịch (Staff/Admin).", "Payments");
        }

        private void OpenPassengerProfiles() {
            MessageBox.Show("Hồ sơ hành khách.", "Passenger Profiles");
        }

        private void OpenNotifications() {
            MessageBox.Show("Thông báo (lọc theo account_id với User).", "Notifications");
        }

        private void OpenRoles() {
            LoadControl(new AccountControl());
        }

        private void OpenMyProfile() {
            MessageBox.Show("Thông tin cá nhân của tôi.", "My Profile");
        }

        private void OpenChangePassword() {
            MessageBox.Show("Đổi mật khẩu.", "Change Password");
        }

        private void DoLogout() {
            // TODO: clear session/token, điều hướng về màn hình đăng nhập
            Close();
        }

        // ===== Public: đổi quyền runtime (nếu cần) ===============================
        public void SetRole(AppRole role) {
            _role = role;
            ActivateTab(NavKey.Home);
        }
    }
}
