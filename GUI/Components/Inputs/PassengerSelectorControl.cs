using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Components.Inputs
{
    // Lớp này PHẢI LÀ 'partial' để khớp với file Designer
    public partial class PassengerSelectorControl : UserControl
    {
        // Thuộc tính public để lưu trữ giá trị
        private int _adults = 1;
        private int _children = 0;
        private int _infants = 0;

        // Định nghĩa sự kiện để thông báo cho cha
        public event EventHandler ValueChanged;

        // Các thuộc tính public để truy cập từ bên ngoài
        public int Adults
        {
            get => _adults;
            set { _adults = Math.Max(1, value); UpdateUI(); } // Phải có ít nhất 1 người lớn
        }
        public int Children
        {
            get => _children;
            set { _children = Math.Max(0, value); UpdateUI(); }
        }
        public int Infants
        {
            get => _infants;
            // Trẻ sơ sinh không được nhiều hơn người lớn
            set { _infants = Math.Max(0, Math.Min(value, _adults)); UpdateUI(); }
        }
        public int TotalPassengers => _adults + _children + _infants;

        public PassengerSelectorControl()
        {
            InitializeComponent(); // Hàm này được định nghĩa trong file .Designer.cs
            UpdateUI(); // Cập nhật giao diện ban đầu
        }

        // Cập nhật giao diện và kiểm tra logic
        private void UpdateUI()
        {
            // Logic nghiệp vụ:
            if (_adults < 1) _adults = 1;
            if (_infants > _adults) _infants = _adults;
            if (_children < 0) _children = 0;
            if (_infants < 0) _infants = 0;

            // Cập nhật Text cho Labels
            lblAdultsCount.Text = _adults.ToString();
            lblChildrenCount.Text = _children.ToString();
            lblInfantsCount.Text = _infants.ToString();

            // Bật/Tắt nút Minus
            btnAdultsMinus.Enabled = (_adults > 1);
            btnChildrenMinus.Enabled = (_children > 0);
            btnInfantsMinus.Enabled = (_infants > 0);

            // Bật/Tắt nút Plus (ví dụ: giới hạn 9 hành khách mỗi loại)
            btnAdultsPlus.Enabled = (_adults < 9);
            btnChildrenPlus.Enabled = (_children < 9);
            btnInfantsPlus.Enabled = (_infants < 9 && _infants < _adults);

            // Phát sự kiện
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        // === CÁC HÀM XỬ LÝ SỰ KIỆN ===
        // (Chúng sẽ được gán tự động trong file .Designer.cs)

        private void btnAdultsPlus_Click(object sender, EventArgs e)
        {
            Adults++;
        }

        private void btnAdultsMinus_Click(object sender, EventArgs e)
        {
            Adults--;
        }

        private void btnChildrenPlus_Click(object sender, EventArgs e)
        {
            Children++;
        }

        private void btnChildrenMinus_Click(object sender, EventArgs e)
        {
            Children--;
        }

        private void btnInfantsPlus_Click(object sender, EventArgs e)
        {
            Infants++;
        }

        private void btnInfantsMinus_Click(object sender, EventArgs e)
        {
            Infants--;
        }
    }
}