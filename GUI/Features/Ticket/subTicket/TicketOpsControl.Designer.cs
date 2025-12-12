namespace GUI.Features.Ticket.subTicket
{
    partial class TicketOpsControl
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
            this.dgvTicketOpsControl = new GUI.Components.Tables.TableCustom();
            this.txtSearchTicket = new GUI.Components.Inputs.UnderlinedTextField();
            this.cboSearchTicket = new GUI.Components.Inputs.UnderlinedComboBox();
            this.btnSearchTicket = new GUI.Components.Buttons.PrimaryButton();

            // --- 3. CONTAINER BỐ CỤC ---
            System.Windows.Forms.Panel pnlTopContainer = new System.Windows.Forms.Panel();
            System.Windows.Forms.TableLayoutPanel tlpSearch = new System.Windows.Forms.TableLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvTicketOpsControl)).BeginInit();
            this.SuspendLayout();

            // 
            // === 1. PANEL CONTAINER (TOP) ===
            // 
            pnlTopContainer.Dock = DockStyle.Top;
            pnlTopContainer.Height = 100; // Chiều cao gọn gàng cho 1 hàng
            pnlTopContainer.BackColor = Color.FromArgb(240, 242, 245); // Xám nhạt
            pnlTopContainer.Padding = new Padding(20, 15, 20, 15);

            // 
            // === 2. TABLE LAYOUT (3 CỘT: TỪ KHÓA - LOẠI - NÚT) ===
            // 
            tlpSearch.Dock = DockStyle.Fill;
            tlpSearch.ColumnCount = 3;
            // Cột 1 (Input): 45%
            tlpSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            // Cột 2 (Combo): 35%
            tlpSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            // Cột 3 (Button): 20%
            tlpSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpSearch.RowCount = 1;
            tlpSearch.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // 
            // === 3. CẤU HÌNH INPUTS ===
            // 
            Color grayBg = Color.FromArgb(240, 242, 245);
            Padding inputMargin = new Padding(0, 0, 15, 0); // Khoảng cách phải

            // txtSearchTicket
            txtSearchTicket.LabelText = "Từ khóa tìm kiếm";
            txtSearchTicket.PlaceholderText = "Nhập mã vé, tên khách hàng...";
            txtSearchTicket.Dock = DockStyle.Fill;
            txtSearchTicket.Margin = inputMargin;
            txtSearchTicket.BackColor = grayBg;
            txtSearchTicket.LineColor = Color.FromArgb(40, 40, 40);
            txtSearchTicket.LineColorFocused = Color.FromArgb(0, 92, 175);
            // Events
            txtSearchTicket.Load += txtSearchTicket_Load;
            txtSearchTicket.KeyUp += KeyUp_ticket;

            // cboSearchTicket
            cboSearchTicket.LabelText = "Tiêu chí lọc / Trạng thái";
            cboSearchTicket.Dock = DockStyle.Fill;
            cboSearchTicket.Margin = inputMargin;
            cboSearchTicket.BackColor = grayBg;
            // Events
            cboSearchTicket.SelectedIndexChanged += cbo_changedIndex;
            cboSearchTicket.Load += underlinedComboBox1_Load;

            // 
            // === 4. CẤU HÌNH BUTTON ===
            // 
            btnSearchTicket.Text = "🔍 Tra cứu";
            btnSearchTicket.Size = new Size(140, 45);
            // Căn nút xuống dưới một chút để thẳng hàng với Textbox (do Textbox có label)
            btnSearchTicket.Margin = new Padding(0, 12, 0, 0);
            btnSearchTicket.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            btnSearchTicket.BackColor = Color.FromArgb(0, 92, 175);
            btnSearchTicket.ForeColor = Color.White;
            btnSearchTicket.BorderColor = Color.FromArgb(40, 40, 40);
            btnSearchTicket.BorderThickness = 2;
            btnSearchTicket.CornerRadius = 22;
            btnSearchTicket.Click += primaryButton1_Click;

            // 
            // === 5. CẤU HÌNH GRID ===
            // 
            dgvTicketOpsControl.Dock = DockStyle.Fill;
            dgvTicketOpsControl.BackgroundColor = Color.White;
            dgvTicketOpsControl.BorderStyle = BorderStyle.None;
            dgvTicketOpsControl.BorderColor = Color.FromArgb(240, 240, 240);
            dgvTicketOpsControl.BorderThickness = 1;
            dgvTicketOpsControl.RowHeadersVisible = false;
            dgvTicketOpsControl.RowTemplate.Height = 45;
            dgvTicketOpsControl.ColumnHeadersHeight = 50;

            // Style
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(100, 100, 100);
            dataGridViewCellStyle1.Padding = new Padding(10, 0, 0, 0);
            dgvTicketOpsControl.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;

            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle3.Padding = new Padding(10, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(235, 245, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(0, 92, 175);
            dgvTicketOpsControl.DefaultCellStyle = dataGridViewCellStyle3;

            dgvTicketOpsControl.CellContentClick += dgvTicketNumberHistory_CellContentClick;

            // 
            // === 6. LẮP RÁP FORM ===
            // 

            // Add Controls vào Table
            tlpSearch.Controls.Add(txtSearchTicket, 0, 0);
            tlpSearch.Controls.Add(cboSearchTicket, 1, 0);
            tlpSearch.Controls.Add(btnSearchTicket, 2, 0);

            // Add Table vào Panel
            pnlTopContainer.Controls.Add(tlpSearch);

            // Add vào Form
            this.Controls.Add(dgvTicketOpsControl); // Fill
            this.Controls.Add(pnlTopContainer);     // Top

            this.Name = "TicketOpsControl";
            this.Size = new Size(1200, 700);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTicketOpsControl)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Components.Tables.TableCustom dgvTicketOpsControl;
        private Components.Inputs.UnderlinedTextField txtSearchTicket;
        private Components.Inputs.UnderlinedComboBox cboSearchTicket;
        private Components.Buttons.PrimaryButton btnSearchTicket;
    }
}
