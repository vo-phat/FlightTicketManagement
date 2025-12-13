using BUS.Baggage;
using DTO.Baggage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Features.Baggage.SubFeatures
{
    public partial class FrmTicketBaggageManager : UserControl
    {
        private readonly TicketBaggageBUS tbBus = new TicketBaggageBUS();
        private readonly CarryOnBaggageBUS carryBus = new CarryOnBaggageBUS();
        private readonly CheckedBaggageBUS checkedBus = new CheckedBaggageBUS();

        private int _ticketId;

        public FrmTicketBaggageManager(int ticketId)
        {
            InitializeComponent();
            _ticketId = ticketId;
            LoadBaggageType();
            LoadBaggageList();
            LoadTicketBaggage();
        }

        private void FrmTicketBaggageManager_Load(object sender, EventArgs e)
        {
            LoadBaggageType();
            LoadBaggageList();
            LoadTicketBaggage();
        }

        // ============================
        // LOAD TYPE: carry_on / checked
        // ============================
        private void LoadBaggageType()
        {
            cbBaggageType.Items.Clear();
            cbBaggageType.Items.Add("carry_on");
            cbBaggageType.Items.Add("checked");
            cbBaggageType.SelectedIndex = 0;
        }

        // ============================
        // LOAD GÓI BẢNG PHỤ
        // ============================
        private void LoadBaggageList()
        {
            string type = cbBaggageType.SelectedItem.ToString();

            if (type == "carry_on")
            {
                cbBaggageList.DataSource = carryBus.GetAll();
                cbBaggageList.DisplayMember = "Description";
                cbBaggageList.ValueMember = "CarryOnId";
            }
            else
            {
                cbBaggageList.DataSource = checkedBus.GetAll();
                cbBaggageList.DisplayMember = "Description";
                cbBaggageList.ValueMember = "CheckedId";
            }
        }

        private void cbBaggageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBaggageList();
        }
        // ============================
        // LOAD DANH SÁCH HÀNH LÝ THEO VÉ (ĐÃ CHỈNH GIÃN BẢNG)
        // ============================
        private void LoadTicketBaggage()
        {
            // 1. Gán dữ liệu
            dgvTicketBaggage.DataSource = tbBus.GetByTicketId(_ticketId);

            // 2. CẤU HÌNH QUAN TRỌNG: Giãn bảng ra toàn màn hình
            dgvTicketBaggage.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTicketBaggage.RowHeadersVisible = false; // Ẩn cột thừa bên trái cho đẹp

            // 3. Cấu hình cột (Dùng FillWeight để chia tỷ lệ phần trăm)
            // Tổng FillWeight không nhất thiết phải là 100, nó là tỷ lệ tương đối.

            // ID: Nhỏ (10%)
            if (dgvTicketBaggage.Columns["Id"] != null)
            {
                dgvTicketBaggage.Columns["Id"].HeaderText = "Mã";
                dgvTicketBaggage.Columns["Id"].FillWeight = 10;
            }

            // Type: Vừa (15%)
            if (dgvTicketBaggage.Columns["BaggageType"] != null)
            {
                dgvTicketBaggage.Columns["BaggageType"].HeaderText = "Loại";
                dgvTicketBaggage.Columns["BaggageType"].FillWeight = 15;
            }

            // Kg: Nhỏ (10%)
            if (dgvTicketBaggage.Columns["Kg"] != null)
            {
                dgvTicketBaggage.Columns["Kg"].HeaderText = "Kg";
                dgvTicketBaggage.Columns["Kg"].FillWeight = 10;
            }

            // Price: Vừa (15%)
            if (dgvTicketBaggage.Columns["Price"] != null)
            {
                dgvTicketBaggage.Columns["Price"].HeaderText = "Đơn giá";
                dgvTicketBaggage.Columns["Price"].DefaultCellStyle.Format = "N0"; // 100,000
                dgvTicketBaggage.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvTicketBaggage.Columns["Price"].FillWeight = 15;
            }

            // Quantity: Nhỏ (10%)
            if (dgvTicketBaggage.Columns["Quantity"] != null)
            {
                dgvTicketBaggage.Columns["Quantity"].HeaderText = "SL";
                dgvTicketBaggage.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTicketBaggage.Columns["Quantity"].FillWeight = 10;
            }

            // Description: Rất Lớn (30%) - Để hiển thị tên gói dài
            if (dgvTicketBaggage.Columns["Description"] != null)
            {
                dgvTicketBaggage.Columns["Description"].HeaderText = "Tên gói hành lý";
                dgvTicketBaggage.Columns["Description"].FillWeight = 30;
                dgvTicketBaggage.Columns["Description"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            // Note: Lớn (25%)
            if (dgvTicketBaggage.Columns["Note"] != null)
            {
                dgvTicketBaggage.Columns["Note"].HeaderText = "Ghi chú";
                dgvTicketBaggage.Columns["Note"].FillWeight = 25;
            }

            // Ẩn các cột ID khóa ngoại
            if (dgvTicketBaggage.Columns["TicketId"] != null) dgvTicketBaggage.Columns["TicketId"].Visible = false;
            if (dgvTicketBaggage.Columns["CarryOnId"] != null) dgvTicketBaggage.Columns["CarryOnId"].Visible = false;
            if (dgvTicketBaggage.Columns["CheckedId"] != null) dgvTicketBaggage.Columns["CheckedId"].Visible = false;
        }

        // ============================
        // CELL CLICK → LOAD FORM
        // ============================
        private void dgvTicketBaggage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow r = dgvTicketBaggage.Rows[e.RowIndex];

            txtTicketBaggageId.Text = r.Cells["Id"].Value.ToString();
            txtQuantity.Text = r.Cells["Quantity"].Value.ToString();
            txtNote.Text = r.Cells["Note"].Value.ToString();
        }

        // ============================
        // CLEAR
        // ============================
        private void ClearForm()
        {
            txtTicketBaggageId.Text = "";
            txtQuantity.Text = "1";
            txtNote.Text = "";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }


        // ============================
        // ADD
        // ============================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtQuantity.Text.Trim() == "")
            {
                MessageBox.Show("Nhập số lượng!");
                return;
            }

            string type = cbBaggageType.SelectedItem.ToString();

            TicketBaggageDTO dto = new TicketBaggageDTO
            {
                TicketId = _ticketId,
                BaggageType = type,
                Quantity = int.Parse(txtQuantity.Text),
                Note = txtNote.Text
            };

            if (type == "carry_on")
            {
                dto.CarryOnId = (int)cbBaggageList.SelectedValue;
                dto.CheckedId = null;
            }
            else
            {
                dto.CheckedId = (int)cbBaggageList.SelectedValue;
                dto.CarryOnId = null;
            }

            if (tbBus.Add(dto))
            {
                MessageBox.Show("Thêm hành lý thành công!");
                LoadTicketBaggage();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!");
            }
        }

        // ============================
        // DELETE
        // ============================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtTicketBaggageId.Text == "")
            {
                MessageBox.Show("Chọn mục cần xóa!");
                return;
            }

            int id = int.Parse(txtTicketBaggageId.Text);

            if (tbBus.Delete(id))
            {
                MessageBox.Show("Xóa hành lý thành công!");
                LoadTicketBaggage();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Xóa thất bại!");
            }
        }
    }
}
