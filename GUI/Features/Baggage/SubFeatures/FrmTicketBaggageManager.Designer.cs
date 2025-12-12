namespace GUI.Features.Baggage.SubFeatures
{
    partial class FrmTicketBaggageManager
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
            // 1. Style Grid
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();

            // 2. Init Controls (Giữ nguyên tên biến cũ)
            this.txtTicketBaggageId = new GUI.Components.Inputs.UnderlinedTextField();
            this.txtQuantity = new GUI.Components.Inputs.UnderlinedTextField();
            this.txtNote = new GUI.Components.Inputs.UnderlinedTextField();
            this.cbBaggageType = new GUI.Components.Inputs.UnderlinedComboBox();
            this.cbBaggageList = new GUI.Components.Inputs.UnderlinedComboBox();
            this.dgvTicketBaggage = new GUI.Components.Tables.TableCustom();
            this.btnAdd = new GUI.Components.Buttons.PrimaryButton();
            this.btnDelete = new GUI.Components.Buttons.PrimaryButton();
            this.btnClear = new GUI.Components.Buttons.PrimaryButton();

            // --- 3. CONTAINER BỐ CỤC ---
            System.Windows.Forms.Panel pnlTopContainer = new System.Windows.Forms.Panel();
            System.Windows.Forms.TableLayoutPanel tlpInputs = new System.Windows.Forms.TableLayoutPanel();
            System.Windows.Forms.FlowLayoutPanel flpActions = new System.Windows.Forms.FlowLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvTicketBaggage)).BeginInit();
            this.SuspendLayout();

            // 
            // === 1. PANEL CONTAINER (VÙNG NHẬP LIỆU) ===
            // 
            pnlTopContainer.Dock = DockStyle.Top;
            pnlTopContainer.Height = 240; // Đủ cao cho 2 hàng input + nút bấm
            pnlTopContainer.BackColor = Color.FromArgb(240, 242, 245); // Xám nhạt
            pnlTopContainer.Padding = new Padding(15);

            // 
            // === 2. TABLE LAYOUT (INPUTS - 3 CỘT) ===
            // 
            tlpInputs.Dock = DockStyle.Top;
            tlpInputs.Height = 160; // 2 hàng x 80px
            tlpInputs.ColumnCount = 3;
            // Chia đều 3 cột (33.33%)
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpInputs.RowCount = 2;
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            // 
            // === 3. FLOW LAYOUT (BUTTONS) ===
            // 
            flpActions.Dock = DockStyle.Bottom;
            flpActions.Height = 60;
            flpActions.FlowDirection = FlowDirection.RightToLeft; // Nút nằm bên phải
            flpActions.Padding = new Padding(0, 10, 0, 0);

            // 
            // === 4. CẤU HÌNH INPUTS (MÀU THEO NỀN) ===
            // 
            Color grayBg = Color.FromArgb(240, 242, 245);
            Padding inputMargin = new Padding(10, 5, 10, 5);

            // -- Row 1 --
            // ID (Thường là ReadOnly hoặc ẩn, nhưng vẫn hiển thị cho đều)
            txtTicketBaggageId.LabelText = "Mã ID";
            txtTicketBaggageId.PlaceholderText = "___";
            txtTicketBaggageId.Dock = DockStyle.Fill;
            txtTicketBaggageId.Margin = inputMargin;
            txtTicketBaggageId.BackColor = grayBg;
            txtTicketBaggageId.LineColor = Color.FromArgb(40, 40, 40);
            txtTicketBaggageId.LineColorFocused = Color.FromArgb(0, 92, 175);

            // Loại hành lý
            cbBaggageType.LabelText = "Loại hành lý";
            cbBaggageType.Dock = DockStyle.Fill;
            cbBaggageType.Margin = inputMargin;
            cbBaggageType.BackColor = grayBg;

            // Danh sách gói
            cbBaggageList.LabelText = "Gói hành lý";
            cbBaggageList.Dock = DockStyle.Fill;
            cbBaggageList.Margin = inputMargin;
            cbBaggageList.BackColor = grayBg;

            // -- Row 2 --
            // Số lượng
            txtQuantity.LabelText = "Số lượng / Kg";
            txtQuantity.PlaceholderText = "VD: 20";
            txtQuantity.Dock = DockStyle.Fill;
            txtQuantity.Margin = inputMargin;
            txtQuantity.BackColor = grayBg;

            // Ghi chú
            txtNote.LabelText = "Ghi chú";
            txtNote.Dock = DockStyle.Fill;
            txtNote.Margin = inputMargin;
            txtNote.BackColor = grayBg;

            // 
            // === 5. CẤU HÌNH BUTTONS ===
            // 
            Size btnSize = new Size(130, 45);

            // Nút Clear (Làm mới)
            btnClear.Text = "Làm mới";
            btnClear.Size = btnSize;
            btnClear.Margin = new Padding(10, 0, 0, 0);
            btnClear.BackColor = Color.White;
            btnClear.ForeColor = Color.Black;
            btnClear.BorderColor = Color.Silver;
            btnClear.BorderThickness = 1;
            btnClear.Click += btnClear_Click;

            // Nút Xóa
            btnDelete.Text = "Xóa";
            btnDelete.Size = btnSize;
            btnDelete.Margin = new Padding(10, 0, 0, 0);
            btnDelete.BackColor = Color.FromArgb(255, 235, 238); // Đỏ nhạt
            btnDelete.ForeColor = Color.FromArgb(211, 47, 47); // Đỏ đậm
            btnDelete.BorderColor = Color.FromArgb(211, 47, 47);
            btnDelete.Click += btnDelete_Click;

            // Nút Thêm
            btnAdd.Text = "Thêm / Lưu";
            btnAdd.Size = btnSize;
            btnAdd.Margin = new Padding(10, 0, 0, 0);
            btnAdd.BackColor = Color.FromArgb(0, 92, 175); // Xanh chủ đạo
            btnAdd.ForeColor = Color.White;
            btnAdd.Click += btnAdd_Click;

            // 
            // === 6. CẤU HÌNH GRID ===
            // 
            dgvTicketBaggage.Dock = DockStyle.Fill;
            dgvTicketBaggage.BackgroundColor = Color.White;
            dgvTicketBaggage.BorderStyle = BorderStyle.None;
            dgvTicketBaggage.BorderColor = Color.FromArgb(240, 240, 240);
            dgvTicketBaggage.BorderThickness = 1;
            dgvTicketBaggage.RowHeadersVisible = false;
            dgvTicketBaggage.RowTemplate.Height = 45;
            dgvTicketBaggage.ColumnHeadersHeight = 50;

            // Style
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(100, 100, 100);
            dataGridViewCellStyle1.Padding = new Padding(10, 0, 0, 0);
            dgvTicketBaggage.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;

            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle3.Padding = new Padding(10, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(235, 245, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(0, 92, 175);
            dgvTicketBaggage.DefaultCellStyle = dataGridViewCellStyle3;

            // 
            // === 7. LẮP RÁP FORM ===
            // 

            // Add Input vào Grid (Hàng 1)
            tlpInputs.Controls.Add(txtTicketBaggageId, 0, 0);
            tlpInputs.Controls.Add(cbBaggageType, 1, 0);
            tlpInputs.Controls.Add(cbBaggageList, 2, 0);

            // Add Input vào Grid (Hàng 2)
            tlpInputs.Controls.Add(txtQuantity, 0, 1);
            tlpInputs.Controls.Add(txtNote, 1, 1);
            tlpInputs.SetColumnSpan(txtNote, 2); // Note chiếm 2 cột cho rộng

            // Add Button vào Flow (Thứ tự thêm = Thứ tự hiển thị từ Phải sang Trái)
            flpActions.Controls.Add(btnClear);
            flpActions.Controls.Add(btnDelete);
            flpActions.Controls.Add(btnAdd);

            // Add vào Panel Top
            pnlTopContainer.Controls.Add(flpActions); // Dưới
            pnlTopContainer.Controls.Add(tlpInputs);  // Trên

            // Add vào Form chính
            this.Controls.Add(dgvTicketBaggage); // Fill
            this.Controls.Add(pnlTopContainer);  // Top

            this.Name = "FrmTicketBaggageManager";
            this.Size = new Size(1200, 700);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTicketBaggage)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Components.Inputs.UnderlinedTextField txtTicketBaggageId;
        private Components.Inputs.UnderlinedTextField txtQuantity;
        private Components.Inputs.UnderlinedTextField txtNote;
        private Components.Inputs.UnderlinedComboBox cbBaggageType;
        private Components.Inputs.UnderlinedComboBox cbBaggageList;
        private Components.Tables.TableCustom dgvTicketBaggage;
        private Components.Buttons.PrimaryButton btnAdd;
        private Components.Buttons.PrimaryButton btnDelete;
        private Components.Buttons.PrimaryButton btnClear;
    }
}
