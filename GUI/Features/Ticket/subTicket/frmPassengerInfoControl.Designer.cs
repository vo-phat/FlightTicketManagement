namespace GUI.Features.Ticket.subTicket
{
    partial class frmPassengerInfoControl
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
            // 1. Khởi tạo Style cho Grid
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();

            // 2. Khởi tạo Controls (Giữ nguyên instance)
            this.cboNationalityTicket = new GUI.Components.Inputs.UnderlinedComboBox();
            this.txtPassportNumberTicket = new GUI.Components.Inputs.UnderlinedTextField();
            this.txtFullNameTicket = new GUI.Components.Inputs.UnderlinedTextField();
            this.txtEmailTicket = new GUI.Components.Inputs.UnderlinedTextField();
            this.txtPhoneNumberTicket = new GUI.Components.Inputs.UnderlinedTextField();
            this.dtpFlightDateTicket = new GUI.Components.Inputs.DateTimePickerCustom();
            this.txtSeatTicket = new GUI.Components.Inputs.UnderlinedTextField();
            this.btnSelectSeatTicket = new GUI.Components.Buttons.PrimaryButton();
            this.dgvPassengerListTicket = new GUI.Components.Tables.TableCustom();
            this.dtpDateOfBirthTicket = new GUI.Components.Inputs.DateTimePickerCustom();
            this.cboBaggageTicket = new GUI.Components.Inputs.UnderlinedComboBox();
            this.txtNoteBaggage = new GUI.Components.Inputs.UnderlinedTextField();
            this.btnNextToPayment = new GUI.Components.Buttons.PrimaryButton();
            this.btnAddOutbound = new GUI.Components.Buttons.PrimaryButton();
            this.btnAddInbound = new GUI.Components.Buttons.PrimaryButton();

            // --- 3. CONTAINER BỐ CỤC MỚI ---
            System.Windows.Forms.Panel pnlTopContainer = new System.Windows.Forms.Panel();
            System.Windows.Forms.TableLayoutPanel tlpInputs = new System.Windows.Forms.TableLayoutPanel();
            System.Windows.Forms.FlowLayoutPanel flpActions = new System.Windows.Forms.FlowLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvPassengerListTicket)).BeginInit();
            this.SuspendLayout();

            // 
            // === 1. PANEL CONTAINER (XÁM - TOP) ===
            // 
            pnlTopContainer.Dock = DockStyle.Top;
            pnlTopContainer.Height = 340; // Tăng chiều cao để chứa 3 hàng input + nút
            pnlTopContainer.BackColor = Color.FromArgb(240, 242, 245); // Màu xám nền
            pnlTopContainer.Padding = new Padding(10);

            // 
            // === 2. TABLE LAYOUT (INPUTS - LƯỚI 4 CỘT) ===
            // 
            tlpInputs.Dock = DockStyle.Top;
            tlpInputs.Height = 240; // 3 hàng x 80px
            tlpInputs.ColumnCount = 4; // Chia 4 cột cho RỘNG
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            tlpInputs.RowCount = 3; // 3 Hàng
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            // 
            // === 3. FLOW LAYOUT (BUTTONS - BOTTOM) ===
            // 
            flpActions.Dock = DockStyle.Bottom;
            flpActions.Height = 60;
            flpActions.FlowDirection = FlowDirection.RightToLeft; // Nút quan trọng bên phải
            flpActions.Padding = new Padding(0, 10, 5, 0);

            // 
            // === 4. CẤU HÌNH INPUTS (MÀU THEO NỀN) ===
            // 
            Color grayBg = Color.FromArgb(240, 242, 245);

            // Row 1
            txtFullNameTicket.LabelText = "Họ và tên hành khách";
            txtFullNameTicket.Dock = DockStyle.Fill;
            txtFullNameTicket.Margin = new Padding(10);
            txtFullNameTicket.BackColor = grayBg;

            dtpDateOfBirthTicket.LabelText = "Ngày sinh";
            dtpDateOfBirthTicket.Dock = DockStyle.Fill;
            dtpDateOfBirthTicket.Margin = new Padding(10, 15, 10, 10); // Margin top chút để căn dòng
            dtpDateOfBirthTicket.BackColor = grayBg;

            cboNationalityTicket.LabelText = "Quốc tịch";
            cboNationalityTicket.Dock = DockStyle.Fill;
            cboNationalityTicket.Margin = new Padding(10);
            cboNationalityTicket.BackColor = grayBg;

            txtPassportNumberTicket.LabelText = "Số Hộ chiếu / CCCD";
            txtPassportNumberTicket.Dock = DockStyle.Fill;
            txtPassportNumberTicket.Margin = new Padding(10);
            txtPassportNumberTicket.BackColor = grayBg;

            // Row 2
            txtPhoneNumberTicket.LabelText = "Số điện thoại";
            txtPhoneNumberTicket.Dock = DockStyle.Fill;
            txtPhoneNumberTicket.Margin = new Padding(10);
            txtPhoneNumberTicket.BackColor = grayBg;

            txtEmailTicket.LabelText = "Email liên hệ";
            txtEmailTicket.Dock = DockStyle.Fill;
            txtEmailTicket.Margin = new Padding(10);
            txtEmailTicket.BackColor = grayBg;
            txtEmailTicket.Visible = true; // Hiện lại để bố cục đẹp hơn

            cboBaggageTicket.LabelText = "Hành lý ký gửi";
            cboBaggageTicket.Dock = DockStyle.Fill;
            cboBaggageTicket.Margin = new Padding(10);
            cboBaggageTicket.BackColor = grayBg;

            txtNoteBaggage.LabelText = "Ghi chú hành lý";
            txtNoteBaggage.Dock = DockStyle.Fill;
            txtNoteBaggage.Margin = new Padding(10);
            txtNoteBaggage.BackColor = grayBg;

            // Row 3
            txtSeatTicket.LabelText = "Ghế đã chọn";
            txtSeatTicket.Dock = DockStyle.Fill;
            txtSeatTicket.Margin = new Padding(10);
            txtSeatTicket.BackColor = grayBg;
            txtSeatTicket.PlaceholderText = "Vui lòng chọn ghế ->";

            // 
            // === 5. CẤU HÌNH BUTTONS ===
            // 
            Size btnSmall = new Size(160, 45);
            Size btnLarge = new Size(200, 45);

            // Nút Chọn ghế (Nằm trong lưới input)
            btnSelectSeatTicket.Text = "Chọn ghế ngồi";
            btnSelectSeatTicket.Size = btnSmall;
            btnSelectSeatTicket.Dock = DockStyle.Left; // Để nút không giãn hết ô
            btnSelectSeatTicket.Margin = new Padding(10, 20, 10, 10); // Căn lề cho thẳng hàng text
            btnSelectSeatTicket.Click += btnSelectSeatTicket_Click;
            // Style
            btnSelectSeatTicket.BackColor = Color.FromArgb(155, 209, 243);
            btnSelectSeatTicket.ForeColor = Color.White;
            btnSelectSeatTicket.CornerRadius = 20;

            // Nút Actions (Flow Layout)
            btnNextToPayment.Text = "Thanh toán >>";
            btnNextToPayment.Size = btnLarge;
            btnNextToPayment.Margin = new Padding(10, 0, 0, 0);
            btnNextToPayment.BackColor = Color.FromArgb(0, 92, 175); // Xanh đậm nổi bật
            btnNextToPayment.Click += btnNextToPayment_Click;

            btnAddInbound.Text = "+ Thêm chiều về";
            btnAddInbound.Size = btnLarge;
            btnAddInbound.Margin = new Padding(10, 0, 0, 0);
            btnAddInbound.Click += btnAddInbound_Click;

            btnAddOutbound.Text = "+ Thêm chiều đi";
            btnAddOutbound.Size = btnLarge;
            btnAddOutbound.Margin = new Padding(10, 0, 0, 0);
            btnAddOutbound.Click += btnAddOutbound_Click;

            // 
            // === 6. CẤU HÌNH GRID ===
            // 
            dgvPassengerListTicket.Dock = DockStyle.Fill;
            dgvPassengerListTicket.BackgroundColor = Color.White;
            dgvPassengerListTicket.BorderColor = Color.FromArgb(40, 40, 40);
            dgvPassengerListTicket.BorderThickness = 1;
            dgvPassengerListTicket.RowHeadersVisible = false;
            dgvPassengerListTicket.RowTemplate.Height = 40;
            dgvPassengerListTicket.ColumnHeadersHeight = 45;

            // Style Grid
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvPassengerListTicket.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;

            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dgvPassengerListTicket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;

            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dgvPassengerListTicket.DefaultCellStyle = dataGridViewCellStyle3;

            // Hidden Control (Giữ lại để ko lỗi code logic)
            dtpFlightDateTicket.Visible = false;

            // 
            // === 7. ADD CONTROLS ===
            // 

            // Hàng 1
            tlpInputs.Controls.Add(txtFullNameTicket, 0, 0);
            tlpInputs.Controls.Add(dtpDateOfBirthTicket, 1, 0);
            tlpInputs.Controls.Add(cboNationalityTicket, 2, 0);
            tlpInputs.Controls.Add(txtPassportNumberTicket, 3, 0);

            // Hàng 2
            tlpInputs.Controls.Add(txtPhoneNumberTicket, 0, 1);
            tlpInputs.Controls.Add(txtEmailTicket, 1, 1);
            tlpInputs.Controls.Add(cboBaggageTicket, 2, 1);
            tlpInputs.Controls.Add(txtNoteBaggage, 3, 1);

            // Hàng 3 (Ghế + Nút chọn ghế)
            tlpInputs.Controls.Add(txtSeatTicket, 0, 2);
            tlpInputs.Controls.Add(btnSelectSeatTicket, 1, 2);
            // Ô (2,2) và (3,2) để trống

            // Flow Buttons (Phải -> Trái)
            flpActions.Controls.Add(btnNextToPayment);
            flpActions.Controls.Add(btnAddInbound);
            flpActions.Controls.Add(btnAddOutbound);

            // Add to Container
            pnlTopContainer.Controls.Add(flpActions);
            pnlTopContainer.Controls.Add(tlpInputs);

            // Add to Form
            this.Controls.Add(dgvPassengerListTicket); // Fill
            this.Controls.Add(pnlTopContainer);        // Top
            this.Controls.Add(dtpFlightDateTicket);    // Hidden

            this.Name = "frmPassengerInfoControl";
            this.Size = new Size(1300, 750);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPassengerListTicket)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Components.Inputs.UnderlinedComboBox cboNationalityTicket;
        private Components.Inputs.UnderlinedTextField txtPassportNumberTicket;
        private Components.Inputs.UnderlinedTextField txtFullNameTicket;
        private Components.Inputs.UnderlinedTextField txtEmailTicket;
        private Components.Inputs.UnderlinedTextField txtPhoneNumberTicket;
        private Components.Inputs.DateTimePickerCustom dtpFlightDateTicket;
        private Components.Inputs.UnderlinedTextField txtSeatTicket;
        private Components.Buttons.PrimaryButton btnSelectSeatTicket;
        private Components.Tables.TableCustom dgvPassengerListTicket;
        private Components.Inputs.DateTimePickerCustom dtpDateOfBirthTicket;
        private Components.Inputs.UnderlinedComboBox cboBaggageTicket;
        private Components.Inputs.UnderlinedTextField txtNoteBaggage;
        private Components.Buttons.PrimaryButton btnNextToPayment;
        private Components.Buttons.PrimaryButton btnAddOutbound;
        private Components.Buttons.PrimaryButton btnAddInbound;
    }
}
