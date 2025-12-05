using GUI.Components.Link;
using GUI.Features.Aircraft;
// ĐÃ XÓA: using GUI.Features.Airline; - Không còn quản lý Airlines
using GUI.Features.Airport;
using GUI.Features.Auth;
using GUI.Features.Baggage;
using GUI.Features.CabinClass;
using GUI.Features.FareRules;
using GUI.Features.Flight;
using GUI.Features.Payments;
using GUI.Features.Profile;
using GUI.Features.Route;
using GUI.Features.Seat;
using GUI.Features.Setting;
using GUI.Features.Stats;
using GUI.Features.Ticket;
using GUI.Properties;
using DTO.Auth;
using DTO.Booking;
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
        private Button btnFindFlights; // Lưu reference để tái sử dụng

        // lưu UC theo key để giữ trạng thái (nếu cần)
        private readonly Dictionary<string, UserControl> controls = new();

        private AppRole _role;
        private NavKey _active = NavKey.Home;

        // ===== Permission =======================================================
        private readonly RolePermissionService _permService = new();
        private HashSet<string> _perms = new(StringComparer.OrdinalIgnoreCase);

        public MainForm() : this(AppRole.User) { } // mặc định User (khách hàng)

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
            } catch (Exception ex) {
                // Fallback: Nếu không connect được database, cấp quyền theo role
                Console.WriteLine($"[MainForm] Không thể load permissions: {ex.Message}");
                
                if (_role == AppRole.User) {
                    // QUYỀN CHO KHÁCH HÀNG (USER)
                    Console.WriteLine("[MainForm] Chế độ Demo - Quyền Khách hàng: Xem và đặt vé");
                    _perms = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                        // Quyền chuyến bay
                        "flights.read",
                        
                        // Quyền đặt vé
                        "tickets.read",
                        "tickets.create",
                        "tickets.create_search",  // Tạo/Tìm đặt chỗ
                        "tickets.mine",           // Xem vé của mình
                        "tickets.history",        // Lịch sử vé
                        
                        // Quyền xem danh mục (để hiển thị thông tin)
                        "airports.read",
                        "airlines.read",
                        "cabins.read",
                        
                        // Quyền hành lý
                        "baggage.checkin",
                        "baggage.track",
                        "baggage.report",
                        
                        // Quyền thanh toán (cho khách đặt vé)
                        "payments.pos",
                        
                        // Thông báo và profile
                        "notifications.read",
                        "customers.profiles"
                    };
                } else if (_role == AppRole.Staff) {
                    // QUYỀN CHO NHÂN VIÊN
                    Console.WriteLine("[MainForm] Chế độ Demo - Quyền Nhân viên");
                    _perms = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                        "flights.read", "flights.create", "flights.update",
                        "tickets.read", "tickets.create", "tickets.update",
                        "tickets.create_search", "tickets.mine", "tickets.operate", "tickets.history",
                        "airports.read", "airlines.read", "aircraft.read",
                        "routes.read", "seats.read", "cabins.read",
                        "payments.pos",
                        "baggage.checkin", "baggage.track", "baggage.report",
                        "notifications.read", "customers.profiles", "reports.view"
                    };
                } else {
                    // QUYỀN CHO ADMIN (đầy đủ)
                    Console.WriteLine("[MainForm] Chế độ Demo - Quyền Admin: Toàn quyền");
                    _perms = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                        "flights.read", "flights.create", "flights.update", "flights.delete",
                        "tickets.read", "tickets.create", "tickets.update",
                        "tickets.create_search", "tickets.mine", "tickets.operate", "tickets.history",
                        "airports.read", "airlines.read", "aircraft.read",
                        "routes.read", "seats.read", "cabins.read",
                        "catalogs.airports", "catalogs.airlines", "catalogs.aircrafts",
                        "catalogs.routes", "catalogs.cabin_classes", "catalogs.seats",
                        "payments.pos",
                        "baggage.checkin", "baggage.track", "baggage.report",
                        "notifications.read", "customers.profiles",
                        "reports.view", "fare_rules.manage",
                        "accounts.manage", "system.roles"
                    };
                }
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

                        // Thêm lại hình nền
                        if (!mainContentPanel.Controls.Contains(defaultPicture))
                            mainContentPanel.Controls.Add(defaultPicture);
                        defaultPicture.Visible = true;
                        defaultPicture.BringToFront();

                        // Thêm lại nút "Tìm chuyến bay"
                        if (!mainContentPanel.Controls.Contains(btnFindFlights))
                            mainContentPanel.Controls.Add(btnFindFlights);
                        btnFindFlights.BringToFront();

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
                        // Ẩn Catalogs_Airlines vì chỉ quản lý Vietnam Airlines
                        HasPerm(Perm.Catalogs_Aircrafts) ||
                        HasPerm(Perm.Catalogs_Airports) ||
                        HasPerm(Perm.Catalogs_Routes) ||
                        HasPerm(Perm.Catalogs_CabinClasses) ||
                        HasPerm(Perm.Catalogs_Seats),
                    SubItems = {
                        // ĐÃ ẨN: Hãng hàng không (chỉ quản lý Vietnam Airlines)
                        // ("Hãng hàng không",
                        //     r => HasPerm(Perm.Catalogs_Airlines),
                        //     () => OpenAirlines()),
                        ("Máy bay Vietnam Airlines",
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

            // Tạo nút "Tìm chuyến bay" - lưu vào field để tái sử dụng
            btnFindFlights = new Button {
                Text = "🔍 TÌM CHUYẾN BAY",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Size = new Size(400, 80),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFindFlights.FlatAppearance.BorderSize = 0;
            btnFindFlights.Location = new Point(
                (mainContentPanel.Width - btnFindFlights.Width) / 2,
                mainContentPanel.Height - 150
            );
            btnFindFlights.Anchor = AnchorStyles.Bottom;
            btnFindFlights.Click += (s, e) => {
                OpenFlightManagement();
            };
            mainContentPanel.Controls.Add(btnFindFlights);
            btnFindFlights.BringToFront();
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
            
            // Thêm lại hình nền
            if (!mainContentPanel.Controls.Contains(defaultPicture))
                mainContentPanel.Controls.Add(defaultPicture);
            defaultPicture.Visible = true;
            defaultPicture.BringToFront();
            
            // Thêm lại nút "Tìm chuyến bay"
            if (!mainContentPanel.Controls.Contains(btnFindFlights))
                mainContentPanel.Controls.Add(btnFindFlights);
            btnFindFlights.BringToFront();
            
            ActivateTab(NavKey.Home);
        }

        // ===== Các hành động mở màn hình thực tế ================================
        private void OpenFlightManagement() {
            // Load FlightControl và đăng ký event
            ShowControl("Flight", () => {
                var control = new GUI.Features.Flight.FlightControl();
                control.NavigateToBookingRequested += OnNavigateToBookingRequested;
                return control;
            });
            ActivateTab(NavKey.Flights);
        }

        private void OnNavigateToBookingRequested(DTO.Flight.FlightWithDetailsDTO flight)
        {
            // Chuyển sang trang Tạo/Tìm đặt chỗ
            MessageBox.Show(
                $"Đang chuyển sang trang đặt vé cho chuyến bay {flight.FlightNumber}\n" +
                $"{flight.DepartureAirportCode} → {flight.ArrivalAirportCode}\n" +
                $"Khởi hành: {flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm")}", 
                "Đặt vé", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
            
            OpenBookingSearch();
        }

        private void OpenFareRules() {
            ShowControl("FareRules", () => new FareRulesControl());
        }
        /// <summary>
        /// /Chua xet den viec co tai khoan do la admin hay user, chua quan tam
        /// 
        /// 
        /// </summary>
        private void OpenBookingSearch() {
            var control = new TicketControl();
            control.switchTab(0);
            LoadControl(control);
            //ShowControl("Ticket", () => new TicketControl());
        }

        private void OpenMyBookings() {
            var control = new TicketControl();
            control.switchTab(0);
            LoadControl(control);
            //MessageBox.Show("Đặt chỗ của tôi (User). TODO gắn UserControl lọc theo account_id.", "My Bookings");
        }

        private void OpenTicketOps() {
            var control = new TicketControl();
            control.switchTab(2);
            LoadControl(control);
            //MessageBox.Show("Quản lý vé (Staff/Admin) – check-in/đổi trạng thái.", "Ticket Ops");
        }

        private void OpenTicketHistory() {
            var control = new TicketControl();
            control.switchTab(1);
            LoadControl(control);
            //MessageBox.Show("Lịch sử vé (Admin).", "Ticket History");
        }
         //Baggage
        private void OpenBaggageCheckin() {
            var control = new BaggageControl();
            control.SwitchTab(0);
            LoadControl(control);
        }

        private void OpenBaggageTracking() {
            var control = new BaggageControl();
            control.SwitchTab(1);
            LoadControl(control);
        }

        private void OpenBaggageReports() {
            var control = new BaggageControl();
            control.SwitchTab(2);
            LoadControl(control);
        }

        // ĐÃ XÓA: OpenAirlines() - Không còn cần quản lý Airlines

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
