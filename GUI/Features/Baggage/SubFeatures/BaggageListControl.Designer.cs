namespace GUI.Features.Baggage.SubFeatures
{
    partial class BaggageListControl
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
            txtNumTicketBaggageList = new TextBox();
            txtCodeBaggageList = new TextBox();
            txtStatetusBaggageList = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            dtpFromDateList = new DateTimePicker();
            dtpToDateList = new DateTimePicker();
            btnSearchBaggageList = new Button();
            dgvBaggageList = new DataGridView();
            label5 = new Label();
            label6 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvBaggageList).BeginInit();
            SuspendLayout();
            // 
            // txtNumTicketBaggageList
            // 
            txtNumTicketBaggageList.Location = new Point(53, 137);
            txtNumTicketBaggageList.Name = "txtNumTicketBaggageList";
            txtNumTicketBaggageList.Size = new Size(125, 27);
            txtNumTicketBaggageList.TabIndex = 0;
            txtNumTicketBaggageList.TextChanged += textBox1_TextChanged;
            // 
            // txtCodeBaggageList
            // 
            txtCodeBaggageList.Location = new Point(224, 137);
            txtCodeBaggageList.Name = "txtCodeBaggageList";
            txtCodeBaggageList.Size = new Size(125, 27);
            txtCodeBaggageList.TabIndex = 1;
            // 
            // txtStatetusBaggageList
            // 
            txtStatetusBaggageList.Location = new Point(383, 137);
            txtStatetusBaggageList.Name = "txtStatetusBaggageList";
            txtStatetusBaggageList.Size = new Size(125, 27);
            txtStatetusBaggageList.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(53, 47);
            label1.Name = "label1";
            label1.Size = new Size(128, 20);
            label1.TabIndex = 3;
            label1.Text = "Danh sách hành lý";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(53, 87);
            label2.Name = "label2";
            label2.Size = new Size(45, 20);
            label2.TabIndex = 4;
            label2.Text = "Số vé";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(224, 87);
            label3.Name = "label3";
            label3.Size = new Size(66, 20);
            label3.TabIndex = 5;
            label3.Text = "Mã nhãn";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(383, 87);
            label4.Name = "label4";
            label4.Size = new Size(75, 20);
            label4.TabIndex = 6;
            label4.Text = "Trạng thái";
            label4.Click += label4_Click;
            // 
            // dtpFromDateList
            // 
            dtpFromDateList.Location = new Point(537, 137);
            dtpFromDateList.Name = "dtpFromDateList";
            dtpFromDateList.Size = new Size(245, 27);
            dtpFromDateList.TabIndex = 7;
            // 
            // dtpToDateList
            // 
            dtpToDateList.Location = new Point(820, 137);
            dtpToDateList.Name = "dtpToDateList";
            dtpToDateList.Size = new Size(245, 27);
            dtpToDateList.TabIndex = 8;
            // 
            // btnSearchBaggageList
            // 
            btnSearchBaggageList.Location = new Point(1071, 136);
            btnSearchBaggageList.Name = "btnSearchBaggageList";
            btnSearchBaggageList.Size = new Size(94, 29);
            btnSearchBaggageList.TabIndex = 9;
            btnSearchBaggageList.Text = "Tìm kiếm";
            btnSearchBaggageList.UseVisualStyleBackColor = true;
            // 
            // dgvBaggageList
            // 
            dgvBaggageList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBaggageList.Location = new Point(53, 310);
            dgvBaggageList.Name = "dgvBaggageList";
            dgvBaggageList.RowHeadersWidth = 51;
            dgvBaggageList.Size = new Size(1068, 236);
            dgvBaggageList.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(820, 87);
            label5.Name = "label5";
            label5.Size = new Size(72, 20);
            label5.TabIndex = 11;
            label5.Text = "Đến ngày";
            label5.Click += label5_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(537, 87);
            label6.Name = "label6";
            label6.Size = new Size(62, 20);
            label6.TabIndex = 12;
            label6.Text = "Từ ngày";
            label6.Click += label6_Click;
            // 
            // BaggageListControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(dgvBaggageList);
            Controls.Add(btnSearchBaggageList);
            Controls.Add(dtpToDateList);
            Controls.Add(dtpFromDateList);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtStatetusBaggageList);
            Controls.Add(txtCodeBaggageList);
            Controls.Add(txtNumTicketBaggageList);
            Name = "BaggageListControl";
            Size = new Size(1154, 586);
            ((System.ComponentModel.ISupportInitialize)dgvBaggageList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtNumTicketBaggageList;
        private TextBox txtCodeBaggageList;
        private TextBox txtStatetusBaggageList;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private DateTimePicker dtpFromDateList;
        private DateTimePicker dtpToDateList;
        private Button btnSearchBaggageList;
        private DataGridView dgvBaggageList;
        private Label label5;
        private Label label6;
    }
}
