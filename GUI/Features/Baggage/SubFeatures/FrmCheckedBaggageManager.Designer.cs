namespace GUI.Features.Baggage.SubFeatures
{
    partial class FrmCheckedBaggageManager
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
            // 1. Setup biến style Grid
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();

            // 2. Khởi tạo Controls
            this.txtCheckedId = new GUI.Components.Inputs.UnderlinedTextField();
            this.txtWeightKg = new GUI.Components.Inputs.UnderlinedTextField();
            this.txtDescription = new GUI.Components.Inputs.UnderlinedTextField();
            this.txtPrice = new GUI.Components.Inputs.UnderlinedTextField();
            this.btnAdd = new GUI.Components.Buttons.PrimaryButton();
            this.btnUpdate = new GUI.Components.Buttons.PrimaryButton();
            this.btnClear = new GUI.Components.Buttons.PrimaryButton();
            this.btnDelete = new GUI.Components.Buttons.PrimaryButton();
            this.dgvChecked = new GUI.Components.Tables.TableCustom();

            // --- CONTAINER MỚI ---
            System.Windows.Forms.Panel pnlTopContainer = new System.Windows.Forms.Panel();
            System.Windows.Forms.TableLayoutPanel tlpInputs = new System.Windows.Forms.TableLayoutPanel();
            System.Windows.Forms.FlowLayoutPanel flpActions = new System.Windows.Forms.FlowLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvChecked)).BeginInit();
            this.SuspendLayout();

            // 
            // === 1. PANEL CONTAINER (XÁM) ===
            // 
            pnlTopContainer.Dock = DockStyle.Top;
            pnlTopContainer.Height = 240;
            // MÀU NỀN CHÍNH: Xám nhạt hiện đại
            pnlTopContainer.BackColor = Color.FromArgb(240, 242, 245);
            pnlTopContainer.Padding = new Padding(10);

            // 
            // === 2. TABLE LAYOUT (INPUTS) ===
            // 
            tlpInputs.Dock = DockStyle.Top;
            tlpInputs.Height = 160;
            tlpInputs.ColumnCount = 3;
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
            flpActions.FlowDirection = FlowDirection.RightToLeft;
            flpActions.Padding = new Padding(0, 10, 5, 0);

            // 
            // === CẤU HÌNH INPUTS (MÀU XÁM THEO NỀN) ===
            // 
            Color grayBackground = Color.FromArgb(240, 242, 245); // Biến màu cho dễ quản lý

            // txtCheckedId
            txtCheckedId.LabelText = "Checked ID";
            txtCheckedId.Dock = DockStyle.Fill;
            txtCheckedId.Margin = new Padding(5);
            txtCheckedId.BackColor = grayBackground; // <--- Cùng màu nền Panel
            txtCheckedId.LineColor = Color.FromArgb(40, 40, 40);
            txtCheckedId.LineColorFocused = Color.FromArgb(0, 92, 175);

            // txtWeightKg
            txtWeightKg.LabelText = "Weight (Kg)";
            txtWeightKg.Dock = DockStyle.Fill;
            txtWeightKg.Margin = new Padding(5);
            txtWeightKg.BackColor = grayBackground; // <--- Cùng màu nền Panel

            // txtPrice
            txtPrice.LabelText = "Price";
            txtPrice.Dock = DockStyle.Fill;
            txtPrice.Margin = new Padding(5);
            txtPrice.BackColor = grayBackground; // <--- Cùng màu nền Panel

            // txtDescription
            txtDescription.LabelText = "Description";
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Margin = new Padding(5);
            txtDescription.BackColor = grayBackground; // <--- Cùng màu nền Panel

            // 
            // === CẤU HÌNH BUTTONS ===
            // 
            Size btnSize = new Size(130, 45);

            btnClear.Text = "Làm mới";
            btnClear.Size = btnSize;
            btnClear.Margin = new Padding(5);

            btnDelete.Text = "Xóa";
            btnDelete.Size = btnSize;
            btnDelete.Margin = new Padding(5);

            btnUpdate.Text = "Sửa";
            btnUpdate.Size = btnSize;
            btnUpdate.Margin = new Padding(5);

            btnAdd.Text = "Thêm";
            btnAdd.Size = btnSize;
            btnAdd.Margin = new Padding(5);

            // 
            // === CẤU HÌNH GRID ===
            // 
            dgvChecked.Dock = DockStyle.Fill;
            dgvChecked.BackgroundColor = Color.White;
            dgvChecked.BorderColor = Color.FromArgb(40, 40, 40);
            dgvChecked.BorderThickness = 2;
            dgvChecked.RowHeadersVisible = false;
            dgvChecked.RowTemplate.Height = 40;
            dgvChecked.ColumnHeadersHeight = 45;

            // Style màu sắc Grid
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvChecked.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dgvChecked.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dgvChecked.DefaultCellStyle = dataGridViewCellStyle3;

            // 
            // === ADD CONTROLS ===
            // 

            // Hàng 1
            tlpInputs.Controls.Add(txtCheckedId, 0, 0);
            tlpInputs.Controls.Add(txtWeightKg, 1, 0);
            tlpInputs.Controls.Add(txtPrice, 2, 0);

            // Hàng 2 (Description Span 2 cột)
            tlpInputs.Controls.Add(txtDescription, 0, 1);
            tlpInputs.SetColumnSpan(txtDescription, 2);

            // Buttons
            flpActions.Controls.Add(btnClear);
            flpActions.Controls.Add(btnDelete);
            flpActions.Controls.Add(btnUpdate);
            flpActions.Controls.Add(btnAdd);

            // Container
            pnlTopContainer.Controls.Add(flpActions);
            pnlTopContainer.Controls.Add(tlpInputs);

            // Form
            this.Controls.Add(dgvChecked);
            this.Controls.Add(pnlTopContainer);

            // Events
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;

            // Final Setup
            this.Name = "FrmCheckedBaggageManager";
            this.Size = new Size(1200, 700);
            this.Load += FrmCheckedBaggageManager_Load;
            ((System.ComponentModel.ISupportInitialize)(this.dgvChecked)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Components.Inputs.UnderlinedTextField txtCheckedId;
        private Components.Inputs.UnderlinedTextField txtWeightKg;
        private Components.Inputs.UnderlinedTextField txtDescription;
        private Components.Inputs.UnderlinedTextField txtPrice;
        private Components.Buttons.PrimaryButton btnAdd;
        private Components.Buttons.PrimaryButton btnUpdate;
        private Components.Buttons.PrimaryButton btnClear;
        private Components.Buttons.PrimaryButton btnDelete;
        private Components.Tables.TableCustom dgvChecked;
    }
}
