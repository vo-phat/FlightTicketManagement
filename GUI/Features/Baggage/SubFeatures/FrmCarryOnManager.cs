using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS.Baggage;
using DTO.Baggage;
using DAO.BaggageDAO;
using System;
using System.Windows.Forms;
namespace GUI.Features.Baggage.SubFeatures
{


    public partial class FrmCarryOnManager : UserControl
    {
        private readonly CarryOnBaggageBUS bus = new CarryOnBaggageBUS();

        public FrmCarryOnManager()
        {
            InitializeComponent();
        }

        private void FrmCarryOnManager_Load(object sender, EventArgs e)
        {
            LoadClassCombo();
            LoadCarryOnList();
        }

        // ===============================
        // 1. LOAD COMBOBOX CLASS
        // ===============================
        private void LoadClassCombo()
        {
            CabinClassDAO dao = new CabinClassDAO();
            var list = dao.GetAll();

            cbClass.DataSource = list;
            cbClass.DisplayMember = "class_name";
            cbClass.ValueMember = "class_id";
        }

        // ===============================
        // 2. LOAD DANH SÁCH CARRY-ON
        // ===============================
        private void LoadCarryOnList()
        {
            dgvCarryOn.DataSource = bus.GetAll();

            // Đảm bảo mapping đúng tên cột
            dgvCarryOn.Columns["CarryOnId"].HeaderText = "ID";
            dgvCarryOn.Columns["WeightKg"].HeaderText = "Weight (kg)";
            dgvCarryOn.Columns["ClassId"].HeaderText = "Class ID";
            dgvCarryOn.Columns["ClassName"].HeaderText = "Class Name";
            dgvCarryOn.Columns["SizeLimit"].HeaderText = "Size Limit";
            dgvCarryOn.Columns["Description"].HeaderText = "Description";
            dgvCarryOn.Columns["IsDefault"].HeaderText = "Default";
        }

        // ===============================
        // 3. CLICK BẢNG → ĐỔ DỮ LIỆU
        // ===============================
        private void dgvCarryOn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow r = dgvCarryOn.Rows[e.RowIndex];

            txtCarryOnId.Text = r.Cells["CarryOnId"].Value.ToString();
            txtWeightKg.Text = r.Cells["WeightKg"].Value.ToString();
            cbClass.SelectedValue = r.Cells["ClassId"].Value;
            txtSizeLimit.Text = r.Cells["SizeLimit"].Value.ToString();
            txtDescription.Text = r.Cells["Description"].Value.ToString();
            chkIsDefault.Checked = (bool)r.Cells["IsDefault"].Value;
        }

        // ===============================
        // 4. CLEAR FORM
        // ===============================
        private void ClearForm()
        {
            txtCarryOnId.Text = "";
            txtWeightKg.Text = "";
            txtSizeLimit.Text = "";
            txtDescription.Text = "";
            chkIsDefault.Checked = false;

            if (cbClass.Items.Count > 0)
                cbClass.SelectedIndex = 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        // ===============================
        // 5. THÊM
        // ===============================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWeightKg.Text))
            {
                MessageBox.Show("Vui lòng nhập trọng lượng.");
                return;
            }

            CarryOnBaggageDTO dto = new CarryOnBaggageDTO
            {
                WeightKg = int.Parse(txtWeightKg.Text),
                ClassId = (int)cbClass.SelectedValue,
                SizeLimit = txtSizeLimit.Text,
                Description = txtDescription.Text,
                IsDefault = chkIsDefault.Checked
            };

            if (bus.Add(dto))
            {
                MessageBox.Show("Thêm carry-on thành công!");
                LoadCarryOnList();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!");
            }
        }

        // ===============================
        // 6. CẬP NHẬT
        // ===============================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCarryOnId.Text))
            {
                MessageBox.Show("Chọn một dòng để sửa.");
                return;
            }

            CarryOnBaggageDTO dto = new CarryOnBaggageDTO
            {
                CarryOnId = int.Parse(txtCarryOnId.Text),
                WeightKg = int.Parse(txtWeightKg.Text),
                ClassId = (int)cbClass.SelectedValue,
                SizeLimit = txtSizeLimit.Text,
                Description = txtDescription.Text,
                IsDefault = chkIsDefault.Checked
            };

            if (bus.Update(dto))
            {
                MessageBox.Show("Cập nhật thành công!");
                LoadCarryOnList();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!");
            }
        }

        // ===============================
        // 7. XÓA
        // ===============================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCarryOnId.Text))
            {
                MessageBox.Show("Chọn một dòng để xoá.");
                return;
            }

            int id = int.Parse(txtCarryOnId.Text);

            if (bus.Delete(id))
            {
                MessageBox.Show("Xoá thành công!");
                LoadCarryOnList();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Xoá thất bại!");
            }
        }
    }

}
