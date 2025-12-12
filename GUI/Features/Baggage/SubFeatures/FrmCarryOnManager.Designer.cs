namespace GUI.Features.Baggage.SubFeatures
{
    partial class FrmCarryOnManager
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 1. Khởi tạo các biến Style cho Grid (Giữ nguyên style cũ của bạn)
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();

            // 2. Khởi tạo các Control (Instance)
            this.dgvCarryOn = new GUI.Components.Tables.TableCustom();
            this.txtCarryOnId = new GUI.Components.Inputs.UnderlinedTextField();
            this.btnUpdate = new GUI.Components.Buttons.PrimaryButton();
            this.cbClass = new GUI.Components.Inputs.UnderlinedComboBox();
            this.txtWeightKg = new GUI.Components.Inputs.UnderlinedTextField();
            this.txtSizeLimit = new GUI.Components.Inputs.UnderlinedTextField();
            this.txtDescription = new GUI.Components.Inputs.UnderlinedTextField();
            this.underlinedTextField5 = new GUI.Components.Inputs.UnderlinedTextField();
            this.btnAdd = new GUI.Components.Buttons.PrimaryButton();
            this.btnDelete = new GUI.Components.Buttons.PrimaryButton();
            this.chkIsDefault = new CheckBox();
            this.btnClear = new GUI.Components.Buttons.PrimaryButton();

            // --- CÁC CONTAINER BỐ CỤC MỚI ---
            // Panel chứa vùng nhập liệu và nút bấm (Dock Top)
            System.Windows.Forms.Panel pnlTopContainer = new System.Windows.Forms.Panel();
            // TableLayout chia cột cho các ô nhập liệu
            System.Windows.Forms.TableLayoutPanel tlpInputs = new System.Windows.Forms.TableLayoutPanel();
            // FlowLayout để gom nhóm các nút bấm
            System.Windows.Forms.FlowLayoutPanel flpActions = new System.Windows.Forms.FlowLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvCarryOn)).BeginInit();
            this.SuspendLayout();

            // 
            // === CẤU HÌNH CONTAINER ===
            // 

            // 1. pnlTopContainer: Chứa toàn bộ phần trên
            pnlTopContainer.Dock = DockStyle.Top;
            pnlTopContainer.Padding = new Padding(10);
            pnlTopContainer.Height = 220; // Chiều cao ước lượng cho vùng nhập liệu
            pnlTopContainer.BackColor = Color.FromArgb(240, 242, 245);

            // 2. tlpInputs: Chia lưới nhập liệu (2 hàng, 4 cột)
            tlpInputs.Dock = DockStyle.Top;
            tlpInputs.Height = 150;
            tlpInputs.ColumnCount = 4;
            // Chia 4 cột, mỗi cột 25% chiều rộng -> Đảm bảo giãn đều
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            // Chia 2 hàng
            tlpInputs.RowCount = 2;
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            // 3. flpActions: Chứa các nút bấm, canh phải
            flpActions.Dock = DockStyle.Bottom; // Nằm dưới tlpInputs
            flpActions.Height = 60;
            flpActions.FlowDirection = FlowDirection.RightToLeft; // Nút mới nhất nằm bên phải
            flpActions.Padding = new Padding(0, 10, 10, 0);

            // 
            // === CẤU HÌNH CONTROLS CHI TIẾT ===
            // 

            // txtCarryOnId
            txtCarryOnId.LabelText = "CarryOn ID";
            txtCarryOnId.Dock = DockStyle.Fill; // Tự động giãn đầy ô lưới
            txtCarryOnId.Margin = new Padding(5); // Tạo khoảng cách giữa các ô
                                                  // ... (Giữ các thuộc tính giao diện cũ của bạn) ...
            txtCarryOnId.LineColor = Color.FromArgb(40, 40, 40);
            txtCarryOnId.LineColorFocused = Color.FromArgb(0, 92, 175);

            // txtWeightKg
            txtWeightKg.LabelText = "Weight kg";
            txtWeightKg.Dock = DockStyle.Fill;
            txtWeightKg.Margin = new Padding(5);

            // txtSizeLimit
            txtSizeLimit.LabelText = "Size limit";
            txtSizeLimit.Dock = DockStyle.Fill;
            txtSizeLimit.Margin = new Padding(5);

            // cbClass
            cbClass.LabelText = "Class";
            cbClass.Dock = DockStyle.Fill;
            cbClass.Margin = new Padding(5);

            // txtDescription
            txtDescription.LabelText = "Description";
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Margin = new Padding(5);

            // underlinedTextField5 (Nhãn)
            underlinedTextField5.LabelText = "Nhãn";
            underlinedTextField5.Dock = DockStyle.Fill;
            underlinedTextField5.Margin = new Padding(5);

            // chkIsDefault
            chkIsDefault.Text = "Is Default";
            chkIsDefault.Dock = DockStyle.Fill;
            chkIsDefault.Margin = new Padding(15, 5, 5, 5); // Căn lề trái một chút cho đẹp
            chkIsDefault.Font = new Font("Segoe UI", 11F);

            // === CÁC NÚT BẤM (BUTTONS) ===
            // Đặt kích thước cố định cho nút, nhưng vị trí sẽ tự động
            Size btnSize = new Size(120, 45);

            // btnClear
            btnClear.Text = "Làm mới";
            btnClear.Size = btnSize;
            btnClear.Margin = new Padding(5);
            // ... style giữ nguyên ...

            // btnDelete
            btnDelete.Text = "Xóa";
            btnDelete.Size = btnSize;
            btnDelete.Margin = new Padding(5);

            // btnUpdate
            btnUpdate.Text = "Sửa";
            btnUpdate.Size = btnSize;
            btnUpdate.Margin = new Padding(5);

            // btnAdd
            btnAdd.Text = "Thêm";
            btnAdd.Size = btnSize;
            btnAdd.Margin = new Padding(5);

            // 
            // === CẤU HÌNH DATA GRID VIEW ===
            // 
            dgvCarryOn.Dock = DockStyle.Fill; // Lấp đầy phần còn lại của màn hình
            dgvCarryOn.BringToFront(); // Đảm bảo hiển thị đúng lớp
                                       // ... (Các style grid cũ giữ nguyên) ...
            dgvCarryOn.BackgroundColor = Color.White;
            dgvCarryOn.BorderColor = Color.FromArgb(40, 40, 40);
            dgvCarryOn.BorderThickness = 2;
            dgvCarryOn.AllowUserToResizeRows = false;
            dgvCarryOn.ColumnHeadersHeight = 40; // Set chiều cao header hợp lý
            dgvCarryOn.RowTemplate.Height = 40;

            // Style màu sắc (Copy lại từ code cũ của bạn)
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvCarryOn.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dgvCarryOn.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dgvCarryOn.DefaultCellStyle = dataGridViewCellStyle3;


            // 
            // === ADD CONTROLS VÀO CONTAINER ===
            // 

            // 1. Thêm Inputs vào TableLayout (Grid 4x2)
            // Hàng 1
            tlpInputs.Controls.Add(txtCarryOnId, 0, 0); // Cột 0, Hàng 0
            tlpInputs.Controls.Add(txtWeightKg, 1, 0);  // Cột 1, Hàng 0
            tlpInputs.Controls.Add(txtSizeLimit, 2, 0); // Cột 2, Hàng 0
            tlpInputs.Controls.Add(cbClass, 3, 0);      // Cột 3, Hàng 0
                                                        // Hàng 2
            tlpInputs.Controls.Add(txtDescription, 0, 1);
            tlpInputs.Controls.Add(underlinedTextField5, 1, 1);
            tlpInputs.Controls.Add(chkIsDefault, 2, 1);
            // Cột 3 hàng 2 để trống

            // 2. Thêm Buttons vào FlowLayout (Thứ tự thêm sẽ hiển thị từ Phải sang Trái do FlowDirection)
            flpActions.Controls.Add(btnClear);
            flpActions.Controls.Add(btnDelete);
            flpActions.Controls.Add(btnUpdate);
            flpActions.Controls.Add(btnAdd);

            // 3. Thêm TableLayout và FlowLayout vào Panel Top
            pnlTopContainer.Controls.Add(flpActions); // Add FlowLayout (Bottom của Panel)
            pnlTopContainer.Controls.Add(tlpInputs);  // Add TableLayout (Top của Panel)

            // 4. Thêm Panel Top và Grid vào Form chính
            this.Controls.Add(dgvCarryOn);      // Grid (Fill)
            this.Controls.Add(pnlTopContainer); // Top Panel

            // Các sự kiện Click (giữ nguyên)
            btnUpdate.Click += btnUpdate_Click;
            btnAdd.Click += btnAdd_Click_1;
            btnDelete.Click += btnDelete_Click_1;
            btnClear.Click += btnClear_Click_1;

            // Finalize
            this.Name = "FrmCarryOnManager";
            this.Size = new Size(1200, 700);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarryOn)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Components.Tables.TableCustom dgvCarryOn;
        private Components.Inputs.UnderlinedTextField txtCarryOnId;
        private Components.Buttons.PrimaryButton btnUpdate;
        private Components.Inputs.UnderlinedComboBox cbClass;
        private Components.Inputs.UnderlinedTextField txtWeightKg;
        private Components.Inputs.UnderlinedTextField txtSizeLimit;
        private Components.Inputs.UnderlinedTextField txtDescription;
        private Components.Inputs.UnderlinedTextField underlinedTextField5;
        private Components.Buttons.PrimaryButton btnAdd;
        private Components.Buttons.PrimaryButton btnDelete;
        private CheckBox chkIsDefault;
        private Components.Buttons.PrimaryButton btnClear;
    }
}
