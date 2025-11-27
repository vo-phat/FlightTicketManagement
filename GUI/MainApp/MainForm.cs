using GUI.Components.Link;
using GUI.Features.Account;
using GUI.Features.Aircraft;
using GUI.Features.Airline;
using GUI.Features.Airport;
using GUI.Features.Auth;
using GUI.Features.Baggage;
using GUI.Features.CabinClass;
using GUI.Features.FareRules;
using GUI.Features.Flight;
using GUI.Features.Profile;
using GUI.Features.Route;
using GUI.Features.Seat;
using GUI.Features.Stats;
using GUI.Features.Ticket;
using GUI.Features.Payments;
using GUI.Features.Setting;
using GUI.Properties;
using DTO.Auth;
using BUS.Auth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.MainApp {
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

        // ===== Permission =======================================================
        private readonly RolePermissionService _permService = new();
        private HashSet<string> _perms = new(StringComparer.OrdinalIgnoreCase);

        public MainForm() : this(AppRole.Admin) { } // mặc định admin

        public MainForm(AppRole role) {
            _role = role;
            InitializeComponent();
            BuildNavbarShell();

            // 🔥 Nạp quyền của account hiện tại
            ReloadPermissions();

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

        // ===== Permission helper ================================================
        private void ReloadPermissions() {
            try {
                var codes = _permService.GetEffectivePermissionCodesOfAccount(UserSession.CurrentAccountId);
                _perms = codes ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            } catch {
                _perms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private bool HasPerm(string code) => _perms.Contains(code);

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
                Image = Resources.logo,
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
                    Key = NavKey.Home,
                    Text = "🏠 Trang chủ",
                    IsVisible = r => true,
                    OnClick = () => {
                        mainContentPanel.Controls.Clear();

                        if (!mainContentPanel.Controls.Contains(defaultPicture))
                            mainContentPanel.Controls.Add(defaultPicture);

                        defaultPicture.Visible = true;
                        defaultPicture.BringToFront();

                        ActivateTab(NavKey.Home);
                    }
                },

                new() {
                    Key = NavKey.Flights, Text = "✈️ Chuyến bay",
                    // Chỉ hiển thị menu nếu có ít nhất 1 trong 2 quyền
                    IsVisible = r => HasPerm(Perm.Flights_Read) || HasPerm(Perm.Flights_Create),
                    SubItems = {
                        ("Quản lý chuyến bay",
                            r => HasPerm(Perm.Flights_Read) || HasPerm(Perm.Flights_Create),
                            () => OpenFlightManagement()),
                        ("Quy tắc giá vé",
                            r => HasPerm(Perm.FareRules_Manage),
                            () => OpenFareRules())
                    }
                },

                new() {
                    Key = NavKey.BookingsTickets, Text = "🎟 Đặt chỗ & Vé",
                    IsVisible = r =>
                        HasPerm(Perm.Tickets_CreateSearch) ||
                        HasPerm(Perm.Tickets_Mine) ||
                        HasPerm(Perm.Tickets_Operate) ||
                        HasPerm(Perm.Tickets_History),
                    SubItems = {
                        ("Tạo/Tìm đặt chỗ",
                            r => HasPerm(Perm.Tickets_CreateSearch),
                            () => OpenBookingSearch()),
                        ("Đặt chỗ của tôi",
                            r => HasPerm(Perm.Tickets_Mine),
                            () => OpenMyBookings()),
                        ("Quản lý vé (check-in/đổi trạng thái)",
                            r => HasPerm(Perm.Tickets_Operate),
                            () => OpenTicketOps()),
                        ("Lịch sử vé",
                            r => HasPerm(Perm.Tickets_History),
                            () => OpenTicketHistory())
                    }
                },

                new() {
                    Key = NavKey.Baggage, Text = "🧳 Hành lý",
                    IsVisible = r =>
                        HasPerm(Perm.Baggage_Checkin) ||
                        HasPerm(Perm.Baggage_Track) ||
                        HasPerm(Perm.Baggage_Report),
                    SubItems = {
                        ("Check-in hành lý / gán tag",
                            r => HasPerm(Perm.Baggage_Checkin),
                            () => OpenBaggageCheckin()),
                        ("Theo dõi trạng thái",
                            r => HasPerm(Perm.Baggage_Track),
                            () => OpenBaggageTracking()),
                        ("Báo cáo thất lạc",
                            r => HasPerm(Perm.Baggage_Report),
                            () => OpenBaggageReports())
                    }
                },

                new() {
                    Key = NavKey.Catalogs, Text = "📚 Danh mục",
                    IsVisible = r =>
                        HasPerm(Perm.Catalogs_Airlines) ||
                        HasPerm(Perm.Catalogs_Aircrafts) ||
                        HasPerm(Perm.Catalogs_Airports) ||
                        HasPerm(Perm.Catalogs_Routes) ||
                        HasPerm(Perm.Catalogs_CabinClasses) ||
                        HasPerm(Perm.Catalogs_Seats),
                    SubItems = {
                        ("Hãng hàng không",
                            r => HasPerm(Perm.Catalogs_Airlines),
                            () => OpenAirlines()),
                        ("Máy bay",
                            r => HasPerm(Perm.Catalogs_Aircrafts),
                            () => OpenAircrafts()),
                        ("Sân bay",
                            r => HasPerm(Perm.Catalogs_Airports),
                            () => LoadControl(new AirportControl())),
                        ("Tuyến bay",
                            r => HasPerm(Perm.Catalogs_Routes),
                            () => OpenRoutes()),
                        ("Hạng vé",
                            r => HasPerm(Perm.Catalogs_CabinClasses),
                            () => OpenCabinClasses()),
                        ("Ghế máy bay",
                            r => HasPerm(Perm.Catalogs_Seats),
                            () => OpenSeats())
                    }
                },

                new() {
                    Key = NavKey.Payments, Text = "💳 Thanh toán",
                    IsVisible = r => HasPerm(Perm.Payments_Pos),
                    SubItems = {
                        ("POS / Giao dịch",
                            r => HasPerm(Perm.Payments_Pos),
                            () => OpenPayments())
                    }
                },

                new() {
                    Key = NavKey.Reports, Text = "📈 Báo cáo",
                    IsVisible = r => HasPerm(Perm.Reports_View),
                    OnClick = () => LoadControl(new StatsControl())
                },

                new() {
                    Key = NavKey.MyProfile, Text = "🙍 Hồ sơ của tôi",
                    IsVisible = r => true,
                    OnClick = () => ShowControl("MyProfile",
                        () => new MyProfileControl(UserSession.CurrentAccountId))
                },

                new() {
                    Key = NavKey.System, Text = "⚙️ Hệ thống",
                    IsVisible = r => HasPerm(Perm.Accounts_Manage) || HasPerm(Perm.System_Roles),
                    SubItems = {
                        ("Quản lý quyền và Tài khoản",
                            r => HasPerm(Perm.Accounts_Manage) || HasPerm(Perm.System_Roles),
                            () => OpenRoles())
                    }
                },
            };
        }

        // ===== Render navbar theo quyền + trạng thái active ======================
        private void RenderNavbar() {
            navFlow.SuspendLayout();
            navFlow.Controls.Clear();

            var spec = BuildSpec().Where(x => x.IsVisible(_role)).ToList();

            for (int i = 0; i < spec.Count; i++) {
                var item = spec[i];

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
                        link.DropMenu = menu;
                    } else if (item.OnClick != null) {
                        link.Click += (_, __) => { ActivateTab(item.Key); item.OnClick(); };
                    }
                } else if (item.OnClick != null) {
                    link.Click += (_, __) => { ActivateTab(item.Key); item.OnClick(); };
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
                Image = Resources.home,
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
            mainContentPanel.Controls.Clear();
            if (!mainContentPanel.Controls.Contains(defaultPicture))
                mainContentPanel.Controls.Add(defaultPicture);
            defaultPicture.Visible = true;
            defaultPicture.BringToFront();
            ActivateTab(NavKey.Home);
        }

        // ===== Các hành động mở màn hình thực tế ================================
        private void OpenFlightManagement() {
            // Truyền delegate HasPerm xuống FlightControl
            ShowControl("Flight", () => new FlightControl(code => HasPerm(code)));
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
            var control = new BaggageControl();
            control.SwitchTab(1);
            LoadControl(control);
        }

        private void OpenBaggageTracking() {
            var control = new BaggageControl();
            control.SwitchTab(2);
            LoadControl(control);
        }

        private void OpenBaggageReports() {
            var control = new BaggageControl();
            control.SwitchTab(0);
            LoadControl(control);
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
            ShowControl("Payments", () => new PaymentsControl());
        }

        private void OpenRoles() {
            LoadControl(new RolePermissionControl());
        }

        // ===== Public: đổi quyền runtime (nếu cần) ===============================
        public void SetRole(AppRole role) {
            _role = role;
            ReloadPermissions();     // nếu đổi role -> load lại perm cho account hiện tại (hoặc account khác)
            ActivateTab(NavKey.Home);
        }
    }
}
