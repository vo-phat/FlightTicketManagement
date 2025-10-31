using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using DTO.Route;
using BUS.Route;

namespace GUI.Features.Route.SubFeatures
{
    public class RouteCreateControl : UserControl
    {
        // Giả định dùng TextField để nhập ID hoặc chọn từ ComboBox (nếu có)
        private UnderlinedTextField _txtDepId, _txtArrId, _txtDistance, _txtDuration;
        private PrimaryButton _btnSave;
        private SecondaryButton _btnCancel;
        private TableCustom _table;

        private readonly RouteBUS _bus = new RouteBUS();
        private int _editingId = 0;

        public event EventHandler? DataSaved;
        public event EventHandler? DataUpdated;

        public RouteCreateControl()
        {
            InitializeComponent();
            LoadRouteList();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // --- Title ---
            var titlePanel = new Panel { Dock = DockStyle.Top, Padding = new Padding(24, 20, 24, 0), Height = 60 };
            var lblTitle = new Label { Text = "➕ Tạo tuyến bay mới", AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold) };
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

            _txtDepId = new UnderlinedTextField("ID Khởi hành", "");
            _txtArrId = new UnderlinedTextField("ID Đến", "");
            _txtDistance = new UnderlinedTextField("Khoảng cách (km)", "");
            _txtDuration = new UnderlinedTextField("Thời gian bay (phút)", "");

            _txtDepId.Width = 200; _txtArrId.Width = 200; _txtDistance.Width = 200; _txtDuration.Width = 200;

            inputs.Controls.Add(_txtDepId, 0, 0);
            inputs.Controls.Add(_txtArrId, 1, 0);
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

            // --- Table (danh sách tuyến bay) ---
            _table = new TableCustom
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            _table.Columns.Add("depId", "ID Khởi hành");
            _table.Columns.Add("arrId", "ID Đến");
            _table.Columns.Add("distance", "Khoảng cách (km)");
            _table.Columns.Add("duration", "Thời gian (phút)");

            // --- Main layout ---
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

        public void LoadRouteList()
        {
            try
            {
                var list = _bus.GetAllRoutes();
                _table.Rows.Clear();
                foreach (var r in list)
                {
                    _table.Rows.Add(
                        r.DeparturePlaceId,
                        r.ArrivalPlaceId,
                        r.DistanceKm.HasValue ? r.DistanceKm.Value.ToString() : "N/A",
                        r.DurationMinutes.HasValue ? r.DurationMinutes.Value.ToString() : "N/A"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách tuyến bay: " + ex.Message);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra và chuyển đổi ID (phải là số nguyên dương)
                if (!int.TryParse(_txtDepId.Text, out int depId) || depId <= 0)
                {
                    MessageBox.Show("ID Khởi hành không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!int.TryParse(_txtArrId.Text, out int arrId) || arrId <= 0)
                {
                    MessageBox.Show("ID Đến không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Kiểm tra khoảng cách/thời gian
                int? distance = null;
                if (!string.IsNullOrWhiteSpace(_txtDistance.Text) && int.TryParse(_txtDistance.Text, out int distVal) && distVal >= 0) distance = distVal;

                int? duration = null;
                if (!string.IsNullOrWhiteSpace(_txtDuration.Text) && int.TryParse(_txtDuration.Text, out int durVal) && durVal >= 0) duration = durVal;


                RouteDTO dto;
                string message;
                bool ok;

                // 3. Xử lý Thêm mới / Cập nhật
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
                MessageBox.Show("Lỗi khi lưu tuyến bay: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAndReset()
        {
            _editingId = 0;
            _txtDepId.Text = _txtArrId.Text = _txtDistance.Text = _txtDuration.Text = "";

            // ID tuyến đường không bị khóa
            _txtDepId.ReadOnly = false;
            _txtArrId.ReadOnly = false;

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
            _txtDepId.Text = dto.DeparturePlaceId.ToString();
            _txtArrId.Text = dto.ArrivalPlaceId.ToString();
            _txtDistance.Text = dto.DistanceKm.HasValue ? dto.DistanceKm.Value.ToString() : "";
            _txtDuration.Text = dto.DurationMinutes.HasValue ? dto.DurationMinutes.Value.ToString() : "";

            // ID khởi hành/đến có thể chỉnh sửa
            _txtDepId.ReadOnly = false;
            _txtArrId.ReadOnly = false;

            _btnSave.Text = $"✍️ Cập nhật #{dto.RouteId}";
        }
    }
}