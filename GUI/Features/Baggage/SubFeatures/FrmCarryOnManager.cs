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
            LoadClassCombo();
            LoadCarryOnList();
            txtCarryOnId.PlaceholderText = "__";
            txtWeightKg.PlaceholderText = "Nhập số KG";
            txtSizeLimit.PlaceholderText = "VD: 56 x 36 x 23 cm";
            underlinedTextField5.PlaceholderText = "VD: Hành lý Tiêu chuẩn";
            txtDescription.PlaceholderText = "Nhập ghi chú chi tiết về quy định...";
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
            cbClass.DisplayMember = "ClassName";
            cbClass.ValueMember = "ClassId";
        }

        // ===============================
        // 2. LOAD DANH SÁCH CARRY-ON 
        // ===============================
        private void LoadCarryOnList()
        {
            // 1. Gán dữ liệu
            dgvCarryOn.DataSource = bus.GetAll();

            // 2. Gọi hàm định dạng giao diện (Tách riêng để code gọn)
            StyleGrid();
        }

        private void StyleGrid()
        {
            // --- A. CẤU HÌNH CHUNG ---
            dgvCarryOn.RowHeadersVisible = false; 
            dgvCarryOn.EnableHeadersVisualStyles = false; 
            dgvCarryOn.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; 
            dgvCarryOn.RowTemplate.Height = 45; 

            // --- B. ẨN CÁC CỘT KHÔNG CẦN THIẾT ---
            if (dgvCarryOn.Columns["ClassId"] != null) 
                dgvCarryOn.Columns["ClassId"].Visible = false; 

            // --- C. ĐẶT TÊN VÀ CĂN LỀ CỘT ---
            
            // 1. Cột ID
            ConfigureColumn("CarryOnId", "Mã", 60, DataGridViewContentAlignment.MiddleCenter);

            // 2. Cột Cân nặng
            ConfigureColumn("WeightKg", "Cân nặng (Kg)", 120, DataGridViewContentAlignment.MiddleCenter);
            
            // 3. Cột Tên hạng vé
            ConfigureColumn("ClassName", "Hạng vé", 150, DataGridViewContentAlignment.MiddleLeft);

            // 4. Cột Kích thước
            ConfigureColumn("SizeLimit", "Kích thước", 120, DataGridViewContentAlignment.MiddleCenter);

            // 5. Cột Mặc định (Checkbox)
            ConfigureColumn("IsDefault", "Mặc định", 80, DataGridViewContentAlignment.MiddleCenter);

            // 6. Cột Mô tả (Quan trọng: Cho giãn hết phần còn lại)
            if (dgvCarryOn.Columns["Description"] != null)
            {
                dgvCarryOn.Columns["Description"].HeaderText = "Mô tả chi tiết";
                dgvCarryOn.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; 
                dgvCarryOn.Columns["Description"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }

        // Hàm phụ trợ để set thuộc tính cột nhanh gọn
        private void ConfigureColumn(string colName, string headerText, int width, DataGridViewContentAlignment align)
        {
            if (dgvCarryOn.Columns[colName] != null)
            {
                dgvCarryOn.Columns[colName].HeaderText = headerText;
                dgvCarryOn.Columns[colName].Width = width;
                dgvCarryOn.Columns[colName].DefaultCellStyle.Alignment = align;
            }
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

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWeightKg.Text))
            {
                MessageBox.Show("Vui lòng nhập trọng lượng");
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


        private void btnClear_Click_1(object sender, EventArgs e)
        {
            ClearForm();
        }



        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCarryOnId.Text))
            {
                MessageBox.Show("Vui lòng chọn mục muốn xoá.");
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
