namespace GUI.Components.Inputs
{
    // Lớp này CŨNG PHẢI LÀ 'partial'
    partial class PassengerSelectorControl
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
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblAdults = new System.Windows.Forms.Label();
            this.lblAdultsSub = new System.Windows.Forms.Label();
            this.btnAdultsMinus = new System.Windows.Forms.Button();
            this.lblAdultsCount = new System.Windows.Forms.Label();
            this.btnAdultsPlus = new System.Windows.Forms.Button();

            this.lblChildren = new System.Windows.Forms.Label();
            this.lblChildrenSub = new System.Windows.Forms.Label();
            this.btnChildrenMinus = new System.Windows.Forms.Button();
            this.lblChildrenCount = new System.Windows.Forms.Label();
            this.btnChildrenPlus = new System.Windows.Forms.Button();

            this.lblInfants = new System.Windows.Forms.Label();
            this.lblInfantsSub = new System.Windows.Forms.Label();
            this.btnInfantsMinus = new System.Windows.Forms.Button();
            this.lblInfantsCount = new System.Windows.Forms.Label();
            this.btnInfantsPlus = new System.Windows.Forms.Button();

            this.mainLayout.SuspendLayout();
            this.SuspendLayout();

            // 
            // mainLayout
            // 
            this.mainLayout.BackColor = System.Drawing.Color.White;
            this.mainLayout.ColumnCount = 4;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new System.Windows.Forms.Padding(10);
            this.mainLayout.RowCount = 3;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.mainLayout.Size = new System.Drawing.Size(300, 155); // Kích thước (10+45+45+45+10)
            this.mainLayout.TabIndex = 0;

            // --- Hàng 1: Người lớn ---
            // lblAdults
            this.lblAdults.Text = "Người lớn";
            this.lblAdults.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblAdults.ForeColor = System.Drawing.Color.Black;
            this.lblAdults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAdults.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // lblAdultsSub
            this.lblAdultsSub.Text = "Người lớn (từ 12 tuổi)";
            this.lblAdultsSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAdultsSub.ForeColor = System.Drawing.Color.Gray;
            this.lblAdultsSub.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblAdultsSub.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblAdults.Controls.Add(this.lblAdultsSub); // Đặt sub-label BÊN TRONG label chính
            // btnAdultsMinus (SỬA LỖI: Đây là Button)
            this.btnAdultsMinus.Text = "-";
            this.btnAdultsMinus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnAdultsMinus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnAdultsMinus.BackColor = System.Drawing.Color.White;
            this.btnAdultsMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdultsMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdultsMinus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnAdultsMinus.Name = "btnAdultsMinus";
            // lblAdultsCount
            this.lblAdultsCount.Text = "1";
            this.lblAdultsCount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAdultsCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAdultsCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // btnAdultsPlus
            this.btnAdultsPlus.Text = "+";
            this.btnAdultsPlus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnAdultsPlus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnAdultsPlus.BackColor = System.Drawing.Color.White;
            this.btnAdultsPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdultsPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdultsPlus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnAdultsPlus.Name = "btnAdultsPlus";

            // --- Hàng 2: Trẻ em ---
            // lblChildren
            this.lblChildren.Text = "Trẻ em";
            this.lblChildren.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblChildren.ForeColor = System.Drawing.Color.Black;
            this.lblChildren.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChildren.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // lblChildrenSub
            this.lblChildrenSub.Text = "Trẻ em (2-12 tuổi)";
            this.lblChildrenSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblChildrenSub.ForeColor = System.Drawing.Color.Gray;
            this.lblChildrenSub.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblChildrenSub.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblChildren.Controls.Add(this.lblChildrenSub);
            // btnChildrenMinus
            this.btnChildrenMinus.Text = "-";
            this.btnChildrenMinus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnChildrenMinus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnChildrenMinus.BackColor = System.Drawing.Color.White;
            this.btnChildrenMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChildrenMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChildrenMinus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnChildrenMinus.Name = "btnChildrenMinus";
            // lblChildrenCount
            this.lblChildrenCount.Text = "0";
            this.lblChildrenCount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblChildrenCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChildrenCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // btnChildrenPlus
            this.btnChildrenPlus.Text = "+";
            this.btnChildrenPlus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnChildrenPlus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnChildrenPlus.BackColor = System.Drawing.Color.White;
            this.btnChildrenPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChildrenPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChildrenPlus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnChildrenPlus.Name = "btnChildrenPlus";

            // --- Hàng 3: Trẻ sơ sinh ---
            // lblInfants
            this.lblInfants.Text = "Trẻ em";
            this.lblInfants.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblInfants.ForeColor = System.Drawing.Color.Black;
            this.lblInfants.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfants.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // lblInfantsSub
            this.lblInfantsSub.Text = "Trẻ em (dưới 2 tuổi)";
            this.lblInfantsSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInfantsSub.ForeColor = System.Drawing.Color.Gray;
            this.lblInfantsSub.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblInfantsSub.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblInfants.Controls.Add(this.lblInfantsSub);
            // btnInfantsMinus
            this.btnInfantsMinus.Text = "-";
            this.btnInfantsMinus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnInfantsMinus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnInfantsMinus.BackColor = System.Drawing.Color.White;
            this.btnInfantsMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfantsMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInfantsMinus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnInfantsMinus.Name = "btnInfantsMinus";
            // lblInfantsCount
            this.lblInfantsCount.Text = "0";
            this.lblInfantsCount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblInfantsCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfantsCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // btnInfantsPlus
            this.btnInfantsPlus.Text = "+";
            this.btnInfantsPlus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnInfantsPlus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnInfantsPlus.BackColor = System.Drawing.Color.White;
            this.btnInfantsPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfantsPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInfantsPlus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnInfantsPlus.Name = "btnInfantsPlus";

            // Thêm controls vào layout
            this.mainLayout.Controls.Add(this.lblAdults, 0, 0);
            this.mainLayout.Controls.Add(this.btnAdultsMinus, 1, 0);
            this.mainLayout.Controls.Add(this.lblAdultsCount, 2, 0);
            this.mainLayout.Controls.Add(this.btnAdultsPlus, 3, 0);

            this.mainLayout.Controls.Add(this.lblChildren, 0, 1);
            this.mainLayout.Controls.Add(this.btnChildrenMinus, 1, 1);
            this.mainLayout.Controls.Add(this.lblChildrenCount, 2, 1);
            this.mainLayout.Controls.Add(this.btnChildrenPlus, 3, 1);

            this.mainLayout.Controls.Add(this.lblInfants, 0, 2);
            this.mainLayout.Controls.Add(this.btnInfantsMinus, 1, 2);
            this.mainLayout.Controls.Add(this.lblInfantsCount, 2, 2);
            this.mainLayout.Controls.Add(this.btnInfantsPlus, 3, 2);

            // Gán sự kiện Click (trong file .cs)
            this.btnAdultsPlus.Click += new System.EventHandler(this.btnAdultsPlus_Click);
            this.btnAdultsMinus.Click += new System.EventHandler(this.btnAdultsMinus_Click);
            this.btnChildrenPlus.Click += new System.EventHandler(this.btnChildrenPlus_Click);
            this.btnChildrenMinus.Click += new System.EventHandler(this.btnChildrenMinus_Click);
            this.btnInfantsPlus.Click += new System.EventHandler(this.btnInfantsPlus_Click);
            this.btnInfantsMinus.Click += new System.EventHandler(this.btnInfantsMinus_Click);

            // 
            // PassengerSelectorControl
            // 
            this.Controls.Add(this.mainLayout);
            this.Name = "PassengerSelectorControl";
            this.Size = new System.Drawing.Size(300, 155);
            this.mainLayout.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        // Khai báo các biến control
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Label lblAdults;
        private System.Windows.Forms.Label lblAdultsSub;
        private System.Windows.Forms.Button btnAdultsMinus;
        private System.Windows.Forms.Label lblAdultsCount;
        private System.Windows.Forms.Button btnAdultsPlus;

        private System.Windows.Forms.Label lblChildren;
        private System.Windows.Forms.Label lblChildrenSub;
        private System.Windows.Forms.Button btnChildrenMinus;
        private System.Windows.Forms.Label lblChildrenCount;
        private System.Windows.Forms.Button btnChildrenPlus;

        private System.Windows.Forms.Label lblInfants;
        private System.Windows.Forms.Label lblInfantsSub;
        private System.Windows.Forms.Button btnInfantsMinus;
        private System.Windows.Forms.Label lblInfantsCount;
        private System.Windows.Forms.Button btnInfantsPlus;
    }
}