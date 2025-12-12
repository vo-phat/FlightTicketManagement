using BUS.Airport;
using BUS.Route;
using DAO.Models;
using DTO.Airport;
using DTO.Route;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Route.SubFeatures
{
    public class RouteCreateControl : UserControl
    {
        // ✅ Dùng UnderlinedComboBox thay vì ComboBox thuần
        private UnderlinedComboBox _cbDepAirport, _cbArrAirport;
        private UnderlinedTextField _txtDistance, _txtDuration;
        private PrimaryButton _btnSave;
        private SecondaryButton _btnCancel;
        private TableCustom _table;

        private readonly RouteBUS _bus = new RouteBUS();
        private readonly AirportBUS _airportBus = new AirportBUS();
        private int _editingId = 0;

        public event EventHandler? DataSaved;
        public event EventHandler? DataUpdated;

        public RouteCreateControl()
        {
            InitializeComponent();
            LoadAirports();  
            LoadRouteList();
        }

        private void FrmRouteManager_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                LoadAirports();
            }
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // --- Title ---
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label
            {
                Text = "➕ Tạo tuyến bay mới",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold)
            };
            titlePanel.Controls.Add(lblTitle);

            // --- Inputs ---
            var inputs = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Padding = new Padding(24, 12, 24, 0),
                AutoSize = true,
                ColumnCount = 2
            };
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            _cbDepAirport = new UnderlinedComboBox("Sân bay khởi hành", Array.Empty<string>())
            {
                Width = 350
            };
            _cbArrAirport = new UnderlinedComboBox("Sân bay đến", Array.Empty<string>())
            {
                Width = 350
            };
            _txtDistance = new UnderlinedTextField("Khoảng cách (km)", "");
            _txtDuration = new UnderlinedTextField("Thời gian bay (phút)", "");

            inputs.Controls.Add(_cbDepAirport, 0, 0);
            inputs.Controls.Add(_cbArrAirport, 1, 0);
            inputs.Controls.Add(_txtDistance, 0, 1);
            inputs.Controls.Add(_txtDuration, 1, 1);

            // --- Buttons ---
            _btnSave = new PrimaryButton("💾 Lưu tuyến bay") { Width = 160, Height = 40, Margin = new Padding(4) };
            _btnCancel = new SecondaryButton("Hủy") { Width = 90, Height = 40, Margin = new Padding(4) };
            _btnSave.Click += BtnSave_Click;
            _btnCancel.Click += (_, __) => ClearAndReset();

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                Padding = new Padding(24, 0, 24, 0)
            };
            btnPanel.Controls.AddRange(new Control[] { _btnSave, _btnCancel });

            // --- Table ---
            _table = new TableCustom
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            _table.Columns.Add("dep", "Sân bay khởi hành");
            _table.Columns.Add("arr", "Sân bay đến");
            _table.Columns.Add("distance", "Khoảng cách (km)");
            _table.Columns.Add("duration", "Thời gian (phút)");

            // --- Main Layout ---
            var main = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            main.Controls.Add(titlePanel, 0, 0);
            main.Controls.Add(inputs, 0, 1);
            main.Controls.Add(btnPanel, 0, 2);
            main.Controls.Add(_table, 0, 3);

            Controls.Add(main);
        }

        // ✅ Nạp danh sách sân bay hiển thị “ID - Tên”
        private void LoadAirports()
        {
            try
            {
                var airportList = new AirportBUS();

                var list = airportList.GetAllAirports();

                var displayList = list.Select(a => new
                {
                    a.AirportId,
                    Display = $"{a.AirportId} - {a.AirportName}"
                }).ToList();

                _cbDepAirport.InnerComboBox.DataSource = displayList.ToList();
                _cbDepAirport.InnerComboBox.DisplayMember = "Display";
                _cbDepAirport.InnerComboBox.ValueMember = "AirportId";

                _cbArrAirport.InnerComboBox.DataSource = displayList.ToList();
                _cbArrAirport.InnerComboBox.DisplayMember = "Display";
                _cbArrAirport.InnerComboBox.ValueMember = "AirportId";

                _cbDepAirport.InnerComboBox.SelectedIndex = -1;
                _cbArrAirport.InnerComboBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sân bay: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadRouteList()
        {
            try
            {
                var list = _bus.GetAllRoutes();
                var airports = _airportBus.GetAllAirports().ToDictionary(a => a.AirportId, a => a.AirportName);

                _table.Rows.Clear();
                foreach (var r in list)
                {
                    string depName = airports.ContainsKey(r.DeparturePlaceId) ? airports[r.DeparturePlaceId] : $"#{r.DeparturePlaceId}";
                    string arrName = airports.ContainsKey(r.ArrivalPlaceId) ? airports[r.ArrivalPlaceId] : $"#{r.ArrivalPlaceId}";

                    _table.Rows.Add(
                        depName,
                        arrName,
                        r.DistanceKm?.ToString() ?? "N/A",
                        r.DurationMinutes?.ToString() ?? "N/A"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách tuyến bay: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_cbDepAirport.InnerComboBox.SelectedValue == null ||
                    _cbArrAirport.InnerComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ sân bay khởi hành và đến.",
                        "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int depId = (int)_cbDepAirport.InnerComboBox.SelectedValue;
                int arrId = (int)_cbArrAirport.InnerComboBox.SelectedValue;

                if (depId == arrId)
                {
                    MessageBox.Show("Sân bay khởi hành và đến không được trùng nhau.",
                        "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int? distance = null;
                if (int.TryParse(_txtDistance.Text, out int distVal) && distVal >= 0)
                    distance = distVal;

                int? duration = null;
                if (int.TryParse(_txtDuration.Text, out int durVal) && durVal >= 0)
                    duration = durVal;

                RouteDTO dto;
                string message;
                bool ok;

                if (_editingId == 0)
                {
                    dto = new RouteDTO(depId, arrId, distance, duration);
                    ok = _bus.AddRoute(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataSaved?.Invoke(this, EventArgs.Empty);
                        LoadRouteList();
                        ClearAndReset();
                    }
                    else
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    dto = new RouteDTO(_editingId, depId, arrId, distance, duration);
                    ok = _bus.UpdateRoute(dto, out message);
                    if (ok)
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataUpdated?.Invoke(this, EventArgs.Empty);
                        LoadRouteList();
                        ClearAndReset();
                    }
                    else
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu tuyến bay: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAndReset()
        {
            _editingId = 0;
            _cbDepAirport.InnerComboBox.SelectedIndex = -1;
            _cbArrAirport.InnerComboBox.SelectedIndex = -1;
            _txtDistance.Text = _txtDuration.Text = "";
            _btnSave.Text = "💾 Lưu tuyến bay";
        }

        public void LoadForEdit(RouteDTO dto)
        {
            if (dto == null || dto.RouteId <= 0)
            {
                ClearAndReset();
                return;
            }

            _editingId = dto.RouteId;
            _cbDepAirport.InnerComboBox.SelectedValue = dto.DeparturePlaceId;
            _cbArrAirport.InnerComboBox.SelectedValue = dto.ArrivalPlaceId;
            _txtDistance.Text = dto.DistanceKm?.ToString() ?? "";
            _txtDuration.Text = dto.DurationMinutes?.ToString() ?? "";
            _btnSave.Text = $"✍️ Cập nhật #{dto.RouteId}";
        }
    }
}
