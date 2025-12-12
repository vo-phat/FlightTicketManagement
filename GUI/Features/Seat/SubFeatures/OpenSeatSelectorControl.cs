using BUS.Seat;
using DTO.FlightSeat;
using DTO.Seat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Features.Seat.SubFeatures
{
    public class OpenSeatSelectorControl : UserControl
    {
        private readonly OpenSeatSelectorBUS _bus = new();

        // Controls
        private Panel pnlBottomWrapper; // Panel chứa Legend + Button
        private FlowLayoutPanel legendPanel;
        private Button btnConfirm;
        private Panel seatPanel;
        private Label lblSelectedInfo;

        // Data
        private SeatSelectDTO selectedSeat;
        private int SelectedClassId;
        public List<int> TakenSeats { get; set; } = new();

        // ======================
        // MÀU THEO HẠNG VÉ
        // ======================
        private readonly Color COLOR_FIRST = Color.Gold;
        private readonly Color COLOR_BUSINESS = Color.SkyBlue;
        private readonly Color COLOR_PREMIUM = Color.MediumSeaGreen;
        private readonly Color COLOR_ECONOMY = Color.LightGray;

        // ======================
        // MÀU TRẠNG THÁI
        // ======================
        private readonly Color COLOR_BOOKED = Color.FromArgb(220, 53, 69); // Đỏ
        private readonly Color COLOR_BLOCKED = Color.DimGray;
        private readonly Color COLOR_SELECTED = Color.FromArgb(0, 92, 175); // Xanh đậm
        private readonly Color COLOR_TAKEN_SESSION = Color.Orange;

        public OpenSeatSelectorControl()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.BackColor = Color.White;

            // 1. Label thông tin (Top)
            lblSelectedInfo = new Label
            {
                Dock = DockStyle.Top,
                Height = 50,
                Text = "Vui lòng chọn ghế trên sơ đồ",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 92, 175),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White
            };

            // 2. Wrapper chứa Legend và Button (Bottom)
            pnlBottomWrapper = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.FromArgb(248, 250, 252), // Nền xám nhạt phân cách
                Padding = new Padding(10)
            };

            // --- Nút Xác nhận (Nằm bên phải của Bottom Wrapper) ---
            btnConfirm = new Button
            {
                Text = "Xác nhận ghế",
                Dock = DockStyle.Right,
                Width = 160,
                BackColor = Color.FromArgb(0, 92, 175),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click; // Sự kiện click

            // --- Legend Panel (Chiếm phần còn lại bên trái) ---
            legendPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 15, 0, 0) // Căn giữa theo chiều dọc
            };

            // Thêm các mục chú thích vào Legend
            legendPanel.Controls.Add(CreateLegendItem(COLOR_FIRST, "Hạng Nhất"));
            legendPanel.Controls.Add(CreateLegendItem(COLOR_BUSINESS, "Thương Gia"));
            legendPanel.Controls.Add(CreateLegendItem(COLOR_PREMIUM, "PT Đặc Biệt"));
            legendPanel.Controls.Add(CreateLegendItem(COLOR_ECONOMY, "Phổ Thông"));
            legendPanel.Controls.Add(new Panel { Width = 15, Height = 1 }); // Khoảng cách
            legendPanel.Controls.Add(CreateLegendItem(COLOR_SELECTED, "Đang chọn", true));
            legendPanel.Controls.Add(CreateLegendItem(COLOR_BOOKED, "Đã đặt"));

            // Add vào Wrapper (Button trước vì nó Dock Right, Legend sau vì nó Dock Fill)
            pnlBottomWrapper.Controls.Add(legendPanel);
            pnlBottomWrapper.Controls.Add(btnConfirm);

            // 3. Panel chứa ghế (Fill)
            seatPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            // Lắp ráp vào UserControl
            this.Controls.Add(seatPanel);       // Giữa
            this.Controls.Add(pnlBottomWrapper);// Dưới
            this.Controls.Add(lblSelectedInfo); // Trên
        }

        private Control CreateLegendItem(Color color, string text, bool isBold = false)
        {
            var container = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 0, 15, 0),
                VerticalScroll = { Visible = false }
            };

            var box = new Panel
            {
                Width = 16,
                Height = 16,
                BackColor = color,
                Margin = new Padding(0, 3, 5, 0)
            };
            if (color == Color.LightGray || color == Color.White)
                box.BorderStyle = BorderStyle.FixedSingle;

            var lbl = new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Segoe UI", 9, isBold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 0, 0, 0)
            };

            container.Controls.Add(box);
            container.Controls.Add(lbl);
            return container;
        }

        public void LoadSeats(int flightId, int classId)
        {
            selectedSeat = null;
            SelectedClassId = classId;
            lblSelectedInfo.Text = "Vui lòng chọn ghế trên sơ đồ";
            lblSelectedInfo.ForeColor = Color.Gray;

            seatPanel.Controls.Clear();

            var seats = _bus.GetOpenSeats(flightId, 1);
            if (seats.Count == 0)
            {
                lblSelectedInfo.Text = "Chuyến bay này chưa có sơ đồ ghế!";
                return;
            }

            var rows = seats.Select(s => int.Parse(new string(s.SeatNumber.Where(char.IsDigit).ToArray()))).Distinct().OrderBy(r => r).ToList();
            var cols = seats.Select(s => new string(s.SeatNumber.Where(char.IsLetter).ToArray())).Distinct().OrderBy(c => c).ToList();

            int seatW = 50, seatH = 40;
            int spacingX = 10, spacingY = 15;
            int aisleGap = 30;

            int totalW = (cols.Count * (seatW + spacingX)) + aisleGap;
            int startX = Math.Max(20, (this.Width - totalW) / 2);
            int startY = 40;

            // Header Cột
            for (int i = 0; i < cols.Count; i++)
            {
                int blockOffset = (i >= cols.Count / 2) ? aisleGap : 0;
                var lblCol = new Label
                {
                    Text = cols[i],
                    Width = seatW,
                    Height = 20,
                    Location = new Point(startX + 30 + i * (seatW + spacingX) + blockOffset, 10),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.Gray
                };
                seatPanel.Controls.Add(lblCol);
            }

            // Vẽ Ghế
            foreach (var row in rows)
            {
                int rowIndex = row - 1;
                int yPos = startY + rowIndex * (seatH + spacingY);

                // Số hàng
                seatPanel.Controls.Add(new Label
                {
                    Text = row.ToString(),
                    Width = 30,
                    Height = seatH,
                    Location = new Point(startX, yPos),
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.Gray
                });

                for (int i = 0; i < cols.Count; i++)
                {
                    string colChar = cols[i];
                    string seatCode = $"{row}{colChar}";
                    var seatData = seats.FirstOrDefault(s => s.SeatNumber == seatCode);
                    if (seatData == null) continue;

                    int blockOffset = (i >= cols.Count / 2) ? aisleGap : 0;
                    int xPos = startX + 30 + i * (seatW + spacingX) + blockOffset;

                    var btnSeat = new Button
                    {
                        Text = seatCode,
                        Tag = seatData,
                        Width = seatW,
                        Height = seatH,
                        Location = new Point(xPos, yPos),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Font = new Font("Segoe UI", 8),
                        BackColor = GetSeatColor(seatData)
                    };
                    btnSeat.FlatAppearance.BorderSize = 1;
                    btnSeat.FlatAppearance.BorderColor = Color.Silver;
                    btnSeat.Click += Seat_Click;
                    seatPanel.Controls.Add(btnSeat);
                }
            }
        }

        private void Seat_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var seat = (SeatSelectDTO)btn.Tag;

            if (TakenSeats.Contains(seat.FlightSeatId))
            {
                MessageBox.Show($"Ghế {seat.SeatNumber} đang được giữ.", "Thông báo");
                return;
            }

            if (seat.ClassId != SelectedClassId)
            {
                MessageBox.Show($"Vui lòng chọn ghế hạng {GetClassName(SelectedClassId)}.", "Sai hạng vé");
                return;
            }

            if (seat.SeatStatus != "AVAILABLE")
            {
                MessageBox.Show("Ghế không khả dụng.");
                return;
            }

            selectedSeat = seat;
            lblSelectedInfo.Text = $"Đang chọn: {seat.SeatNumber}  •  Giá: {seat.Price:N0} VNĐ";
            lblSelectedInfo.ForeColor = COLOR_SELECTED;
            HighlightSelection(btn);
        }

        // --- SỰ KIỆN NÚT XÁC NHẬN ---
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (selectedSeat == null)
            {
                MessageBox.Show("Bạn chưa chọn ghế!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tìm Form cha và đóng nó lại với kết quả OK
            var parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.OK;
                parentForm.Close();
            }
        }

        private void HighlightSelection(Button selectedBtn)
        {
            foreach (Control ctrl in seatPanel.Controls)
            {
                if (ctrl is Button b && b.Tag is SeatSelectDTO dto)
                {
                    if (b == selectedBtn)
                    {
                        b.BackColor = COLOR_SELECTED;
                        b.ForeColor = Color.White;
                        b.FlatAppearance.BorderColor = COLOR_SELECTED;
                    }
                    else
                    {
                        b.BackColor = GetSeatColor(dto);
                        b.ForeColor = (b.BackColor == COLOR_BOOKED || b.BackColor == COLOR_BLOCKED) ? Color.White : Color.Black;
                        b.FlatAppearance.BorderColor = Color.Silver;
                    }
                }
            }
        }

        private Color GetSeatColor(SeatSelectDTO seat)
        {
            if (seat.SeatStatus == "BOOKED") return COLOR_BOOKED;
            if (seat.SeatStatus == "BLOCKED") return COLOR_BLOCKED;
            if (TakenSeats.Contains(seat.FlightSeatId)) return COLOR_TAKEN_SESSION;

            return seat.ClassId switch
            {
                1 => COLOR_FIRST,
                2 => COLOR_BUSINESS,
                3 => COLOR_PREMIUM,
                4 => COLOR_ECONOMY,
                _ => Color.White
            };
        }

        private string GetClassName(int id) => id switch { 1 => "Hạng Nhất", 2 => "Thương Gia", 3 => "Phổ Thông ĐB", 4 => "Phổ Thông", _ => "Khác" };

        public SeatSelectDTO GetSelectedSeat() => selectedSeat;
    }
}