namespace GUI.Features.Ticket.subTicket
{
    partial class HistoryTicketControl
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

            // 2. Init Controls
            this.dgvTickets = new GUI.Components.Tables.TableCustom();
            this.txtTicketNumber = new GUI.Components.Inputs.UnderlinedTextField();
            this.btnFilter = new GUI.Components.Buttons.PrimaryButton();
            this.cbStatus = new GUI.Components.Inputs.UnderlinedComboBox();
            this.dtTo = new GUI.Components.Inputs.DateTimePickerCustom();
            this.dtFrom = new GUI.Components.Inputs.DateTimePickerCustom();

            // --- 3. CONTAINER BỐ CỤC ---
            System.Windows.Forms.Panel pnlTopContainer = new System.Windows.Forms.Panel();
            System.Windows.Forms.TableLayoutPanel tlpInputs = new System.Windows.Forms.TableLayoutPanel();
            System.Windows.Forms.FlowLayoutPanel flpActions = new System.Windows.Forms.FlowLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvTickets)).BeginInit();
            this.SuspendLayout();

            // 
            // === 1. PANEL CONTAINER (VÙNG TÌM KIẾM) ===
            // 
            pnlTopContainer.Dock = DockStyle.Top;
            pnlTopContainer.Height = 160; // Chiều cao = Input (80) + Button (60) + Padding
            pnlTopContainer.BackColor = Color.FromArgb(240, 242, 245); // Xám nhạt
            pnlTopContainer.Padding = new Padding(15);

            // 
            // === 2. TABLE LAYOUT (CHỈ CHỨA 4 Ô INPUT) ===
            // 
            tlpInputs.Dock = DockStyle.Top;
            tlpInputs.Height = 85; // Chiều cao cố định cho hàng input
            tlpInputs.ColumnCount = 4;
            // CHIA ĐỀU 4 CỘT (25% TUYỆT ĐỐI)
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpInputs.RowCount = 1;
            tlpInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // 
            // === 3. FLOW LAYOUT (CHỨA NÚT BẤM) ===
            // 
            flpActions.Dock = DockStyle.Bottom;
            flpActions.Height = 50;
            flpActions.FlowDirection = FlowDirection.RightToLeft; // Canh phải
            flpActions.Padding = new Padding(0, 0, 10, 0); // Thụt lề phải một chút cho cân

            // 
            // === 4. CẤU HÌNH INPUTS (MÀU THEO NỀN) ===
            // 
            Color grayBg = Color.FromArgb(240, 242, 245);
            // Margin đồng bộ để tạo khe hở đều nhau
            Padding inputMargin = new Padding(10, 5, 10, 5);

            // 1. Mã vé
            txtTicketNumber.LabelText = "Mã số vé";
            txtTicketNumber.Dock = DockStyle.Fill;
            txtTicketNumber.Margin = inputMargin;
            txtTicketNumber.BackColor = grayBg;
            txtTicketNumber.LineColor = Color.FromArgb(40, 40, 40);
            txtTicketNumber.LineColorFocused = Color.FromArgb(0, 92, 175);

            // 2. Trạng thái
            cbStatus.LabelText = "Trạng thái vé";
            cbStatus.Dock = DockStyle.Fill;
            cbStatus.Margin = inputMargin;
            cbStatus.BackColor = grayBg;

            // 3. Từ ngày
            dtFrom.LabelText = "Từ ngày";
            dtFrom.Dock = DockStyle.Fill;
            // Chỉnh Margin Top lớn hơn chút để Label của DatePicker ngang hàng với TextBox
            dtFrom.Margin = new Padding(10, 20, 10, 5);
            dtFrom.BackColor = grayBg;
            dtFrom.ShowUpDown = false;
            dtFrom.TimeFormat = "dd/MM/yyyy";

            // 4. Đến ngày
            dtTo.LabelText = "Đến ngày";
            dtTo.Dock = DockStyle.Fill;
            dtTo.Margin = new Padding(10, 20, 10, 5);
            dtTo.BackColor = grayBg;
            dtTo.ShowUpDown = false;
            dtTo.TimeFormat = "dd/MM/yyyy";

            // 
            // === 5. CẤU HÌNH BUTTON ===
            // 
            btnFilter.Text = "🔍 Tìm kiếm";
            btnFilter.Size = new Size(160, 45);
            btnFilter.BackColor = Color.FromArgb(0, 92, 175);
            btnFilter.ForeColor = Color.White;
            btnFilter.BorderColor = Color.FromArgb(40, 40, 40);
            btnFilter.BorderThickness = 2;
            btnFilter.CornerRadius = 22;
            btnFilter.Click += btnFilter_click;

            // 
            // === 6. CẤU HÌNH GRID ===
            // 
            dgvTickets.Dock = DockStyle.Fill;
            dgvTickets.BackgroundColor = Color.White;
            dgvTickets.BorderStyle = BorderStyle.None;
            dgvTickets.BorderColor = Color.FromArgb(240, 240, 240);
            dgvTickets.BorderThickness = 1;
            dgvTickets.RowHeadersVisible = false;
            dgvTickets.RowTemplate.Height = 45;
            dgvTickets.ColumnHeadersHeight = 50;

            // Style
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(100, 100, 100);
            dataGridViewCellStyle1.Padding = new Padding(10, 0, 0, 0);
            dgvTickets.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;

            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle3.Padding = new Padding(10, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(235, 245, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(0, 92, 175);
            dgvTickets.DefaultCellStyle = dataGridViewCellStyle3;

            dgvTickets.CellContentClick += tableCustom1_CellContentClick;

            // 
            // === 7. LẮP RÁP FORM ===
            // 

            // Thêm 4 ô Input vào Grid 1 hàng
            tlpInputs.Controls.Add(txtTicketNumber, 0, 0);
            tlpInputs.Controls.Add(cbStatus, 1, 0);
            tlpInputs.Controls.Add(dtFrom, 2, 0);
            tlpInputs.Controls.Add(dtTo, 3, 0);

            // Thêm Button vào Flow (Dưới cùng)
            flpActions.Controls.Add(btnFilter);

            // Thêm vào Panel Top
            pnlTopContainer.Controls.Add(flpActions); // Add Flow trước (Dock Bottom)
            pnlTopContainer.Controls.Add(tlpInputs);  // Add Grid sau (Dock Top)

            // Thêm vào Form
            this.Controls.Add(dgvTickets);      // Fill
            this.Controls.Add(pnlTopContainer); // Top

            this.Name = "HistoryTicketControl";
            this.Size = new Size(1200, 700);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTickets)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
        private Components.Tables.TableCustom dgvTickets;
        private Components.Inputs.UnderlinedTextField txtTicketNumber;
        private Components.Inputs.UnderlinedComboBox cbStatus;
        private Components.Inputs.DateTimePickerCustom dtTo;
        private Components.Inputs.DateTimePickerCustom dtFrom;
        private Components.Buttons.PrimaryButton btnFilter;
    }
}
