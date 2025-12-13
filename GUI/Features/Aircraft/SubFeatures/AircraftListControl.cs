using BUS.Aircraft;
using DTO.Aircraft;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Aircraft.SubFeatures
{
    public partial class AircraftListControl : UserControl
    {
        private readonly AircraftBUS _bus = new AircraftBUS();
        
        // Controls
        private Panel headerPanel;
        private Panel searchPanel;
        private TextBox txtSearch;
        private ComboBox cboStatus;
        private Button btnSearch;
        private Button btnClear;
        private Button btnAddNew;
        private FlowLayoutPanel aircraftContainer;
        private Label lblNoData;
        
        // Events
        public event Action<AircraftDTO>? ViewRequested;
        public event Action<AircraftDTO>? RequestEdit;
        public event Action? DataChanged;

        private List<AircraftDTO> _allAircrafts = new List<AircraftDTO>();
        private const int CARD_WIDTH = 380;
        private const int CARD_HEIGHT = 280;

        public AircraftListControl()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            
            BackColor = Color.FromArgb(245, 247, 250);
            Dock = DockStyle.Fill;
            Padding = new Padding(30, 20, 30, 20);
            AutoScroll = true;

            // === HEADER PANEL ===
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.Transparent
            };

            // === SEARCH PANEL ===
            searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.White,
                Padding = new Padding(20, 15, 20, 15)
            };

            // Search textbox
            txtSearch = new TextBox
            {
                Location = new Point(20, 20),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(70, 70, 70)
            };
            txtSearch.Text = "🔍 Tìm kiếm theo tên, model, hãng...";
            txtSearch.ForeColor = Color.Gray;
            txtSearch.GotFocus += (s, e) => {
                if (txtSearch.Text == "🔍 Tìm kiếm theo tên, model, hãng...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.FromArgb(70, 70, 70);
                }
            };
            txtSearch.LostFocus += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "🔍 Tìm kiếm theo tên, model, hãng...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };
            txtSearch.TextChanged += (s, e) => FilterAircrafts();

            // Status filter
            cboStatus = new ComboBox
            {
                Location = new Point(340, 20),
                Size = new Size(180, 35),
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatus.Items.AddRange(new object[] { "Tất cả trạng thái", "ACTIVE", "MAINTENANCE", "RETIRED" });
            cboStatus.SelectedIndex = 0;
            cboStatus.SelectedIndexChanged += (s, e) => FilterAircrafts();

            // Search button
            btnSearch = new Button
            {
                Location = new Point(540, 18),
                Size = new Size(120, 38),
                Text = "🔍 Tìm kiếm",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += (s, e) => FilterAircrafts();

            // Clear button
            btnClear = new Button
            {
                Location = new Point(670, 18),
                Size = new Size(100, 38),
                Text = "↻ Làm mới",
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += BtnClear_Click;

            // Add new button
            btnAddNew = new Button
            {
                Location = new Point(790, 18),
                Size = new Size(150, 38),
                Text = "➕ Thêm máy bay",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddNew.FlatAppearance.BorderSize = 0;
            btnAddNew.Click += (s, e) => RequestEdit?.Invoke(new AircraftDTO());

            // === AIRCRAFT CONTAINER ===
            aircraftContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(10),
                WrapContents = true
            };

            lblNoData = new Label
            {
                Text = "📭 Không tìm thấy máy bay nào",
                Font = new Font("Segoe UI", 14F, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = true,
                Visible = false
            };

            // Add controls
            searchPanel.Controls.AddRange(new Control[] { 
                txtSearch, btnSearch, btnClear, btnAddNew 
            });
            
            Controls.Add(aircraftContainer);
            Controls.Add(searchPanel);
            Controls.Add(headerPanel);
            aircraftContainer.Controls.Add(lblNoData);

            ResumeLayout(false);
            PerformLayout();
        }

        private void LoadData()
        {
            try
            {
                _allAircrafts = _bus.GetAllAircrafts();
                DisplayAircrafts(_allAircrafts);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterAircrafts()
        {
            string searchText = txtSearch.Text.ToLower();
            if (searchText == "🔍 tìm kiếm theo tên, model, hãng...")
                searchText = "";

            string statusFilter = cboStatus.SelectedIndex == 0 ? "" : cboStatus.SelectedItem.ToString();

            var filtered = _allAircrafts.Where(a =>
            {
                bool matchSearch = string.IsNullOrWhiteSpace(searchText) ||
                    (a.Model?.ToLower().Contains(searchText) ?? false) ||
                    (a.Manufacturer?.ToLower().Contains(searchText) ?? false);

                return matchSearch; // Không cần filter Status nữa
            }).ToList();

            DisplayAircrafts(filtered);
        }

        private void DisplayAircrafts(List<AircraftDTO> aircrafts)
        {
            aircraftContainer.Controls.Clear();

            if (aircrafts == null || aircrafts.Count == 0)
            {
                lblNoData.Visible = true;
                lblNoData.Location = new Point(
                    (aircraftContainer.Width - lblNoData.Width) / 2,
                    (aircraftContainer.Height - lblNoData.Height) / 2
                );
                aircraftContainer.Controls.Add(lblNoData);
                return;
            }

            lblNoData.Visible = false;

            foreach (var aircraft in aircrafts)
            {
                aircraftContainer.Controls.Add(CreateAircraftCard(aircraft));
            }
        }

        private Panel CreateAircraftCard(AircraftDTO aircraft)
        {
            var card = new Panel
            {
                Size = new Size(CARD_WIDTH, CARD_HEIGHT),
                BackColor = Color.White,
                Margin = new Padding(10),
                Cursor = Cursors.Hand
            };
            card.Paint += (s, e) => DrawCardBorder(e.Graphics, card);

            // Aircraft icon/image placeholder
            var iconPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(80, 80),
                BackColor = Color.FromArgb(0, 123, 255) // Màu mặc định
            };
            iconPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = GetRoundedRectPath(iconPanel.ClientRectangle, 12))
                {
                    e.Graphics.FillPath(new SolidBrush(iconPanel.BackColor), path);
                    
                    // Draw airplane icon
                    var iconFont = new Font("Segoe UI Emoji", 32F);
                    var iconText = "✈️";
                    var iconSize = e.Graphics.MeasureString(iconText, iconFont);
                    var iconX = (iconPanel.Width - iconSize.Width) / 2;
                    var iconY = (iconPanel.Height - iconSize.Height) / 2;
                    e.Graphics.DrawString(iconText, iconFont, Brushes.White, iconX, iconY);
                }
            };

            // Airline ID (main title)
            var lblRegNum = new Label
            {
                Text = aircraft.AirlineId.HasValue ? $"Airline ID: {aircraft.AirlineId.Value}" : "N/A",
                Location = new Point(115, 25),
                Size = new Size(240, 30),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 31, 31)
            };

            // Model
            var lblModel = new Label
            {
                Text = $"📋 Model: {aircraft.Model ?? "N/A"}",
                Location = new Point(115, 60),
                Size = new Size(240, 22),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            // Model badge (thay thế Status badge)
            var statusBadge = new Label
            {
                Text = aircraft.Model ?? "N/A",
                Location = new Point(115, 85),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 123, 255),
                Padding = new Padding(8, 4, 8, 4)
            };
            statusBadge.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = GetRoundedRectPath(statusBadge.ClientRectangle, 6))
                {
                    e.Graphics.FillPath(new SolidBrush(statusBadge.BackColor), path);
                }
                TextRenderer.DrawText(e.Graphics, statusBadge.Text, statusBadge.Font,
                    statusBadge.ClientRectangle, statusBadge.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            // Divider line
            var divider = new Panel
            {
                Location = new Point(20, 120),
                Size = new Size(CARD_WIDTH - 40, 1),
                BackColor = Color.FromArgb(230, 230, 230)
            };

            // Manufacturer info
            var lblManufacturer = new Label
            {
                Text = $"🏭 Hãng: {aircraft.Manufacturer ?? "N/A"}",
                Location = new Point(20, 135),
                Size = new Size(CARD_WIDTH - 40, 25),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(70, 70, 70)
            };

            // Capacity info
            var lblCapacity = new Label
            {
                Text = $"💺 Sức chứa: {aircraft.Capacity?.ToString() ?? "N/A"} ghế",
                Location = new Point(20, 165),
                Size = new Size(CARD_WIDTH - 40, 25),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(70, 70, 70)
            };

            // Capacity info
            var lblYear = new Label
            {
                Text = aircraft.Capacity.HasValue 
                    ? $"💺 Sức chứa: {aircraft.Capacity.Value} ghế"
                    : "💺 Sức chứa: N/A",
                Location = new Point(20, 195),
                Size = new Size(CARD_WIDTH - 40, 25),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(70, 70, 70)
            };

            // Action buttons
            var btnView = CreateActionButton("👁️ Xem", 20, CARD_HEIGHT - 50, 100, 
                Color.FromArgb(0, 123, 255));
            btnView.Click += (s, e) => ViewRequested?.Invoke(aircraft);

            var btnEdit = CreateActionButton("✏️ Sửa", 130, CARD_HEIGHT - 50, 100, 
                Color.FromArgb(255, 193, 7));
            btnEdit.Click += (s, e) => RequestEdit?.Invoke(aircraft);

            var btnDelete = CreateActionButton("🗑️ Xóa", 240, CARD_HEIGHT - 50, 100, 
                Color.FromArgb(220, 53, 69));
            btnDelete.Click += (s, e) => DeleteAircraft(aircraft);

            // Add all controls to card
            card.Controls.AddRange(new Control[]
            {
                iconPanel, lblRegNum, lblModel, statusBadge, divider,
                lblManufacturer, lblCapacity, lblYear,
                btnView, btnEdit, btnDelete
            });

            return card;
        }

        private Button CreateActionButton(string text, int x, int y, int width, Color color)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 35),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void DrawCardBorder(Graphics g, Panel card)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = GetRoundedRectPath(card.ClientRectangle, 12))
            {
                g.FillPath(Brushes.White, path);
                g.DrawPath(new Pen(Color.FromArgb(220, 220, 220), 2), path);
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private Color GetStatusColor(string status)
        {
            return status?.ToUpper() switch
            {
                "ACTIVE" => Color.FromArgb(40, 167, 69),
                "MAINTENANCE" => Color.FromArgb(255, 193, 7),
                "RETIRED" => Color.FromArgb(108, 117, 125),
                _ => Color.Gray
            };
        }

        private string GetStatusText(string status)
        {
            return status?.ToUpper() switch
            {
                "ACTIVE" => "🟢 Hoạt động",
                "MAINTENANCE" => "🟡 Bảo trì",
                "RETIRED" => "⚫ Ngừng hoạt động",
                _ => "❓ Không xác định"
            };
        }

        private void DeleteAircraft(AircraftDTO aircraft)
        {
            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa máy bay '{aircraft.Model}'?\n\n" +
                $"Hãng: {aircraft.Manufacturer}\n" +
                $"Sức chứa: {aircraft.Capacity ?? 0} ghế",
                "⚠️ Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    string message;
                    bool success = _bus.DeleteAircraft(aircraft.AircraftId, out message);
                    
                    MessageBox.Show(message, 
                        success ? "✅ Thành công" : "❌ Lỗi",
                        MessageBoxButtons.OK,
                        success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                    if (success)
                    {
                        LoadData();
                        DataChanged?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "❌ Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "🔍 Tìm kiếm theo tên, model, hãng...";
            txtSearch.ForeColor = Color.Gray;
            cboStatus.SelectedIndex = 0;
            LoadData();
        }

        public void RefreshList()
        {
            LoadData();
        }
    }
}