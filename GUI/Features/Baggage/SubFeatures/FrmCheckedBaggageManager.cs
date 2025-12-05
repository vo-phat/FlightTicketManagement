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
    public partial class FrmCheckedBaggageManager : UserControl
    {
        private readonly CheckedBaggageBUS bus = new CheckedBaggageBUS();

        public FrmCheckedBaggageManager()
        {
            InitializeComponent();
        }

        private void FrmCheckedBaggageManager_Load(object sender, EventArgs e)
        {
            LoadCheckedList();
        }

        // ============================
        // LOAD LIST
        // ============================
        private void LoadCheckedList()
        {
            dgvChecked.DataSource = bus.GetAll();

            dgvChecked.Columns["CheckedId"].HeaderText = "ID";
            dgvChecked.Columns["WeightKg"].HeaderText = "Weight (kg)";
            dgvChecked.Columns["Price"].HeaderText = "Price";
            dgvChecked.Columns["Description"].HeaderText = "Description";
        }

        // ============================
        // CLICK BẢNG -> ĐỔ LÊN FORM
        // ============================
        private void dgvChecked_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow r = dgvChecked.Rows[e.RowIndex];

            txtCheckedId.Text = r.Cells["CheckedId"].Value.ToString();
            txtWeightKg.Text = r.Cells["WeightKg"].Value.ToString();
            txtPrice.Text = r.Cells["Price"].Value.ToString();
            txtDescription.Text = r.Cells["Description"].Value.ToString();
        }

        // ============================
        // CLEAR FORM
        // ============================
        private void ClearForm()
        {
            txtCheckedId.Text = "";
            txtWeightKg.Text = "";
            txtPrice.Text = "";
            txtDescription.Text = "";
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
            if (string.IsNullOrWhiteSpace(txtWeightKg.Text))
            {
                MessageBox.Show("Vui lòng nhập trọng lượng.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập giá.");
                return;
            }

            CheckedBaggageDTO dto = new CheckedBaggageDTO
            {
                WeightKg = int.Parse(txtWeightKg.Text),
                Price = decimal.Parse(txtPrice.Text),
                Description = txtDescription.Text
            };

            if (bus.Add(dto))
            {
                MessageBox.Show("Thêm hành lý ký gửi thành công!");
                LoadCheckedList();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!");
            }
        }

        // ============================
        // UPDATE
        // ============================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCheckedId.Text))
            {
                MessageBox.Show("Hãy chọn mục cần sửa.");
                return;
            }

            CheckedBaggageDTO dto = new CheckedBaggageDTO
            {
                CheckedId = int.Parse(txtCheckedId.Text),
                WeightKg = int.Parse(txtWeightKg.Text),
                Price = decimal.Parse(txtPrice.Text),
                Description = txtDescription.Text
            };

            if (bus.Update(dto))
            {
                MessageBox.Show("Cập nhật thành công!");
                LoadCheckedList();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!");
            }
        }

        // ============================
        // DELETE
        // ============================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCheckedId.Text))
            {
                MessageBox.Show("Hãy chọn mục cần xoá.");
                return;
            }

            int id = int.Parse(txtCheckedId.Text);

            if (bus.Delete(id))
            {
                MessageBox.Show("Xóa thành công!");
                LoadCheckedList();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Xoá thất bại!");
            }
        }
    }
}
