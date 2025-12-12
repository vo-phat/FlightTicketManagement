using BUS.Seat;
using DTO.FlightSeat;
using DTO.Seat;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace GUI.Features.Seat.SubFeatures
{
    public class OpenSeatSelectorControl : UserControl
    {
        private FlowLayoutPanel legendPanel;

        private readonly OpenSeatSelectorBUS _bus = new();
        private Panel seatPanel;
        private Label lblPrice;
        private Button btnConfirm;

        private SeatSelectDTO selectedSeat;
        private int SelectedClassId;
        public List<int> TakenSeats { get; set; } = new();

        // ======================
        // MÀU THEO HẠNG VÉ (KHỚP UI)
        // ======================
        private readonly Color COLOR_FIRST = Color.Gold;                 // Hạng Nhất
        private readonly Color COLOR_BUSINESS = Color.DodgerBlue;        // Thương Gia
        private readonly Color COLOR_PREMIUM = Color.MediumSeaGreen;    // Phổ Thông Đặc Biệt
        private readonly Color COLOR_ECONOMY = Color.LightGray;          // Phổ Thông

        // ======================
        // MÀU TRẠNG THÁI
        // ======================
        private readonly Color COLOR_BOOKED = Color.DarkGray;
        private readonly Color COLOR_BLOCKED = Color.IndianRed;
        private readonly Color COLOR_SELECTED = Color.Black;             // GHẾ ĐANG CHỌN


        public OpenSeatSelectorControl()
        {
            
            InitializeUI();
        }

        private void InitializeUI()
        {
            lblPrice = new Label
            {
                Dock = DockStyle.Top,
                Height = 35,
                Text = "Chưa chọn ghế",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            seatPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                AutoScroll = true,
                Padding = new Padding(40)
            };

            legendPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                Padding = new Padding(10),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.White
            };

            // ===== Legend items =====
            legendPanel.Controls.Add(CreateLegendItem(COLOR_FIRST, "Hạng Nhất"));
            legendPanel.Controls.Add(CreateLegendItem(COLOR_BUSINESS, "Hạng Thương Gia"));
            legendPanel.Controls.Add(CreateLegendItem(COLOR_PREMIUM, "Phổ Thông Đặc Biệt"));
            legendPanel.Controls.Add(CreateLegendItem(COLOR_ECONOMY, "Phổ Thông"));

            legendPanel.Controls.Add(CreateLegendItem(COLOR_SELECTED, "Ghế đang chọn", true));
            legendPanel.Controls.Add(CreateLegendItem(COLOR_BOOKED, "Đã đặt"));
            legendPanel.Controls.Add(CreateLegendItem(COLOR_BLOCKED, "Không khả dụng"));
            legendPanel.Controls.Add(CreateLegendItem(Color.DarkOrange, "Đang giữ"));

            btnConfirm = new Button
            {
                Text = "Xác nhận",
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnConfirm.Click += BtnConfirm_Click;

            // ===== THỨ TỰ ADD CONTROLS (RẤT QUAN TRỌNG) =====
            Controls.Add(btnConfirm);   // Bottom (dưới cùng)
            Controls.Add(legendPanel);  // Trên nút xác nhận
            Controls.Add(seatPanel);    // Chiếm phần giữa
            Controls.Add(lblPrice);     // Trên cùng
        }

        public void LoadSeats(int flightId, int classId)
        {
            selectedSeat = null;
            SelectedClassId = classId;

            var seats = _bus.GetOpenSeats(flightId, 1); // Lấy FULL ghế, không lọc theo class

            seatPanel.Controls.Clear();

            if (seats.Count == 0)
            {
                lblPrice.Text = "Không tìm thấy ghế!";
                return;
            }

            var rows = seats
                .Select(s => int.Parse(new string(s.SeatNumber.Where(char.IsDigit).ToArray())))
                .Distinct()
                .OrderBy(r => r)
                .ToList();

            var cols = seats
                .Select(s => new string(s.SeatNumber.Where(char.IsLetter).ToArray()))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            int w = 55, h = 45;
            int spacing = 10;
            int gap = 40;

            // Header column letters
            for (int i = 0; i < cols.Count; i++)
            {
                int blockOffset = i >= 3 ? gap : 0;

                seatPanel.Controls.Add(new Label
                {
                    Text = cols[i],
                    Width = w,
                    Height = 25,
                    Location = new Point(i * (w + spacing) + blockOffset, 0),
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }

            // Create seat map
            foreach (var row in rows)
            {
                int rowIndex = row - 1;

                seatPanel.Controls.Add(new Label
                {
                    Text = row.ToString(),
                    Width = 30,
                    Height = h,
                    Location = new Point(-35, 33 + rowIndex * (h + spacing)),
                    TextAlign = ContentAlignment.MiddleCenter
                });

                foreach (var col in cols)
                {
                    string code = $"{row}{col}";
                    var seat = seats.FirstOrDefault(s => s.SeatNumber == code);
                    if (seat == null) continue;

                    int colIndex = col[0] - 'A';
                    int blockOffset = colIndex >= 3 ? gap : 0;

                    var btn = new Button
                    {
                        Text = code,
                        Tag = seat,
                        Width = w,
                        Height = h,
                        Location = new Point(colIndex * (w + spacing) + blockOffset,
                        30 + rowIndex * (h + spacing)),
                        BackColor = GetSeatColor(seat)
                    };

                    btn.Click += Seat_Click;

                    seatPanel.Controls.Add(btn);
                }
            }
        }

        private void Seat_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var seat = (SeatSelectDTO)btn.Tag;
            if (TakenSeats.Contains(seat.FlightSeatId))
            {
                MessageBox.Show($"Ghế {seat.SeatNumber} đã được hành khách khác chọn!", "Cảnh báo");
                return;
            }
            // Kiểm tra class mismatch
            if (seat.ClassId != SelectedClassId)
            {
                MessageBox.Show(
                    $"Ghế {seat.SeatNumber} thuộc {GetClassName(seat.ClassId)}\n" +
                    $"Bạn đang chọn hạng {GetClassName(SelectedClassId)}.",
                    "Sai hạng vé!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Check trạng thái
            if (seat.SeatStatus != "AVAILABLE")
            {
                MessageBox.Show("Ghế này không thể chọn!", "Thông báo");
                return;
            }

            selectedSeat = seat;
            lblPrice.Text = $"Ghế {seat.SeatNumber} – Giá {seat.Price:#,0}₫";

            HighlightSelection(btn);
        }

        private Color GetSeatColor(SeatSelectDTO seat)
        {
            // Trạng thái ưu tiên cao nhất
            if (seat.SeatStatus == "BOOKED") return COLOR_BOOKED;
            if (seat.SeatStatus == "BLOCKED") return COLOR_BLOCKED;
            if (TakenSeats.Contains(seat.FlightSeatId))
                return Color.DarkOrange; // ghế đang bị giữ trong booking khác

            // Màu theo hạng vé
            return seat.ClassId switch
            {
                1 => COLOR_FIRST,
                2 => COLOR_BUSINESS,
                3 => COLOR_PREMIUM,
                4 => COLOR_ECONOMY,
                _ => Color.White
            };
        }
        // tạo legend cho chọn ghế
        private Control CreateLegendItem(Color color, string text, bool isSelected = false)
        {
            var box = new Panel
            {
                Width = 22,
                Height = 22,
                BackColor = color,
                Margin = new Padding(8, 10, 4, 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            var label = new Label
            {
                Text = text,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 12, 16, 0),
                Font = isSelected
                    ? new Font("Segoe UI", 9, FontStyle.Bold)
                    : new Font("Segoe UI", 9)
            };

            var wrapper = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            wrapper.Controls.Add(box);
            wrapper.Controls.Add(label);

            return wrapper;
        }


        private void HighlightSelection(Button selectedBtn)
        {
            foreach (Control ctrl in seatPanel.Controls)
            {
                if (ctrl is Button b && b.Tag is SeatSelectDTO dto)
                {
                    if (dto == selectedSeat)
                    {
                        b.BackColor = COLOR_SELECTED;
                        b.ForeColor = Color.White;
                        b.Font = new Font(b.Font, FontStyle.Bold);
                    }
                    else
                    {
                        b.BackColor = GetSeatColor(dto);
                        b.ForeColor = Color.Black;
                        b.Font = new Font(b.Font, FontStyle.Regular);
                    }
                }
            }
        }


        private string GetClassName(int id)
        {
            return id switch
            {
                1 => "First",
                2 => "Business",
                3 => "Premium Economy",
                4 => "Economy",
                _ => "Unknown"
            };
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (selectedSeat == null)
            {
                MessageBox.Show("Bạn chưa chọn ghế!", "Thông báo");
                return;
            }

            var f = this.FindForm();
            if (f != null)
            {
                f.DialogResult = DialogResult.OK;
                f.Close();
            }
        }

        public SeatSelectDTO GetSelectedSeat() => selectedSeat;
    }
}
