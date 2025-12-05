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
            this.mainLayout.Controls.Add(this.lblAdults, 0, 0);
            this.mainLayout.Controls.Add(this.lblAdultsSub, 0, 1);
            this.mainLayout.Controls.Add(this.btnAdultsMinus, 1, 0);
            this.mainLayout.Controls.Add(this.lblAdultsCount, 2, 0);
            this.mainLayout.Controls.Add(this.btnAdultsPlus, 3, 0);
            this.mainLayout.Controls.Add(this.lblChildren, 0, 2);
            this.mainLayout.Controls.Add(this.lblChildrenSub, 0, 3);
            this.mainLayout.Controls.Add(this.btnChildrenMinus, 1, 2);
            this.mainLayout.Controls.Add(this.lblChildrenCount, 2, 2);
            this.mainLayout.Controls.Add(this.btnChildrenPlus, 3, 2);
            this.mainLayout.Controls.Add(this.lblInfants, 0, 4);
            this.mainLayout.Controls.Add(this.lblInfantsSub, 0, 5);
            this.mainLayout.Controls.Add(this.btnInfantsMinus, 1, 4);
            this.mainLayout.Controls.Add(this.lblInfantsCount, 2, 4);
            this.mainLayout.Controls.Add(this.btnInfantsPlus, 3, 4);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new System.Windows.Forms.Padding(10);
            this.mainLayout.RowCount = 6;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.mainLayout.Size = new System.Drawing.Size(300, 155);
            this.mainLayout.TabIndex = 0;
            // 
            // lblAdults
            // 
            this.lblAdults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAdults.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblAdults.ForeColor = System.Drawing.Color.Black;
            this.lblAdults.Location = new System.Drawing.Point(13, 10);
            this.lblAdults.Name = "lblAdults";
            this.lblAdults.Size = new System.Drawing.Size(174, 22);
            this.lblAdults.TabIndex = 0;
            this.lblAdults.Text = "Người lớn";
            this.lblAdults.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblAdultsSub
            // 
            this.lblAdultsSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAdultsSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAdultsSub.ForeColor = System.Drawing.Color.Gray;
            this.lblAdultsSub.Location = new System.Drawing.Point(13, 32);
            this.lblAdultsSub.Name = "lblAdultsSub";
            this.lblAdultsSub.Size = new System.Drawing.Size(174, 23);
            this.lblAdultsSub.TabIndex = 1;
            this.lblAdultsSub.Text = "(từ 12 tuổi)";
            this.lblAdultsSub.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // btnAdultsMinus
            // 
            this.btnAdultsMinus.BackColor = System.Drawing.Color.White;
            this.btnAdultsMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdultsMinus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnAdultsMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdultsMinus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnAdultsMinus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnAdultsMinus.Location = new System.Drawing.Point(193, 13);
            this.btnAdultsMinus.Name = "btnAdultsMinus";
            this.mainLayout.SetRowSpan(this.btnAdultsMinus, 2);
            this.btnAdultsMinus.Size = new System.Drawing.Size(34, 39);
            this.btnAdultsMinus.TabIndex = 2;
            this.btnAdultsMinus.Text = "-";
            this.btnAdultsMinus.UseVisualStyleBackColor = false;
            this.btnAdultsMinus.Click += new System.EventHandler(this.btnAdultsMinus_Click);
            // 
            // lblAdultsCount
            // 
            this.lblAdultsCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAdultsCount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAdultsCount.Location = new System.Drawing.Point(233, 10);
            this.lblAdultsCount.Name = "lblAdultsCount";
            this.mainLayout.SetRowSpan(this.lblAdultsCount, 2);
            this.lblAdultsCount.Size = new System.Drawing.Size(24, 45);
            this.lblAdultsCount.TabIndex = 3;
            this.lblAdultsCount.Text = "1";
            this.lblAdultsCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAdultsPlus
            // 
            this.btnAdultsPlus.BackColor = System.Drawing.Color.White;
            this.btnAdultsPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdultsPlus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnAdultsPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdultsPlus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnAdultsPlus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnAdultsPlus.Location = new System.Drawing.Point(263, 13);
            this.btnAdultsPlus.Name = "btnAdultsPlus";
            this.mainLayout.SetRowSpan(this.btnAdultsPlus, 2);
            this.btnAdultsPlus.Size = new System.Drawing.Size(34, 39);
            this.btnAdultsPlus.TabIndex = 4;
            this.btnAdultsPlus.Text = "+";
            this.btnAdultsPlus.UseVisualStyleBackColor = false;
            this.btnAdultsPlus.Click += new System.EventHandler(this.btnAdultsPlus_Click);
            // 
            // lblChildren
            // 
            this.lblChildren.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChildren.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblChildren.ForeColor = System.Drawing.Color.Black;
            this.lblChildren.Location = new System.Drawing.Point(13, 55);
            this.lblChildren.Name = "lblChildren";
            this.lblChildren.Size = new System.Drawing.Size(174, 22);
            this.lblChildren.TabIndex = 5;
            this.lblChildren.Text = "Trẻ em";
            this.lblChildren.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblChildrenSub
            // 
            this.lblChildrenSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChildrenSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblChildrenSub.ForeColor = System.Drawing.Color.Gray;
            this.lblChildrenSub.Location = new System.Drawing.Point(13, 77);
            this.lblChildrenSub.Name = "lblChildrenSub";
            this.lblChildrenSub.Size = new System.Drawing.Size(174, 23);
            this.lblChildrenSub.TabIndex = 6;
            this.lblChildrenSub.Text = "(2-12 tuổi)";
            this.lblChildrenSub.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // btnChildrenMinus
            // 
            this.btnChildrenMinus.BackColor = System.Drawing.Color.White;
            this.btnChildrenMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChildrenMinus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnChildrenMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChildrenMinus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnChildrenMinus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnChildrenMinus.Location = new System.Drawing.Point(193, 58);
            this.btnChildrenMinus.Name = "btnChildrenMinus";
            this.mainLayout.SetRowSpan(this.btnChildrenMinus, 2);
            this.btnChildrenMinus.Size = new System.Drawing.Size(34, 39);
            this.btnChildrenMinus.TabIndex = 7;
            this.btnChildrenMinus.Text = "-";
            this.btnChildrenMinus.UseVisualStyleBackColor = false;
            this.btnChildrenMinus.Click += new System.EventHandler(this.btnChildrenMinus_Click);
            // 
            // lblChildrenCount
            // 
            this.lblChildrenCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChildrenCount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblChildrenCount.Location = new System.Drawing.Point(233, 55);
            this.lblChildrenCount.Name = "lblChildrenCount";
            this.mainLayout.SetRowSpan(this.lblChildrenCount, 2);
            this.lblChildrenCount.Size = new System.Drawing.Size(24, 45);
            this.lblChildrenCount.TabIndex = 8;
            this.lblChildrenCount.Text = "0";
            this.lblChildrenCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnChildrenPlus
            // 
            this.btnChildrenPlus.BackColor = System.Drawing.Color.White;
            this.btnChildrenPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChildrenPlus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnChildrenPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChildrenPlus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnChildrenPlus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnChildrenPlus.Location = new System.Drawing.Point(263, 58);
            this.btnChildrenPlus.Name = "btnChildrenPlus";
            this.mainLayout.SetRowSpan(this.btnChildrenPlus, 2);
            this.btnChildrenPlus.Size = new System.Drawing.Size(34, 39);
            this.btnChildrenPlus.TabIndex = 9;
            this.btnChildrenPlus.Text = "+";
            this.btnChildrenPlus.UseVisualStyleBackColor = false;
            this.btnChildrenPlus.Click += new System.EventHandler(this.btnChildrenPlus_Click);
            // 
            // lblInfants
            // 
            this.lblInfants.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfants.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblInfants.ForeColor = System.Drawing.Color.Black;
            this.lblInfants.Location = new System.Drawing.Point(13, 100);
            this.lblInfants.Name = "lblInfants";
            this.lblInfants.Size = new System.Drawing.Size(174, 22);
            this.lblInfants.TabIndex = 10;
            this.lblInfants.Text = "Trẻ em";
            this.lblInfants.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblInfantsSub
            // 
            this.lblInfantsSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfantsSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInfantsSub.ForeColor = System.Drawing.Color.Gray;
            this.lblInfantsSub.Location = new System.Drawing.Point(13, 122);
            this.lblInfantsSub.Name = "lblInfantsSub";
            this.lblInfantsSub.Size = new System.Drawing.Size(174, 23);
            this.lblInfantsSub.TabIndex = 11;
            this.lblInfantsSub.Text = "(dưới 2 tuổi)";
            this.lblInfantsSub.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // btnInfantsMinus
            // 
            this.btnInfantsMinus.BackColor = System.Drawing.Color.White;
            this.btnInfantsMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInfantsMinus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnInfantsMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfantsMinus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnInfantsMinus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnInfantsMinus.Location = new System.Drawing.Point(193, 103);
            this.btnInfantsMinus.Name = "btnInfantsMinus";
            this.mainLayout.SetRowSpan(this.btnInfantsMinus, 2);
            this.btnInfantsMinus.Size = new System.Drawing.Size(34, 39);
            this.btnInfantsMinus.TabIndex = 12;
            this.btnInfantsMinus.Text = "-";
            this.btnInfantsMinus.UseVisualStyleBackColor = false;
            this.btnInfantsMinus.Click += new System.EventHandler(this.btnInfantsMinus_Click);
            // 
            // lblInfantsCount
            // 
            this.lblInfantsCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfantsCount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblInfantsCount.Location = new System.Drawing.Point(233, 100);
            this.lblInfantsCount.Name = "lblInfantsCount";
            this.mainLayout.SetRowSpan(this.lblInfantsCount, 2);
            this.lblInfantsCount.Size = new System.Drawing.Size(24, 45);
            this.lblInfantsCount.TabIndex = 13;
            this.lblInfantsCount.Text = "0";
            this.lblInfantsCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnInfantsPlus
            // 
            this.btnInfantsPlus.BackColor = System.Drawing.Color.White;
            this.btnInfantsPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInfantsPlus.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnInfantsPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfantsPlus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnInfantsPlus.ForeColor = System.Drawing.Color.DarkOrange;
            this.btnInfantsPlus.Location = new System.Drawing.Point(263, 103);
            this.btnInfantsPlus.Name = "btnInfantsPlus";
            this.mainLayout.SetRowSpan(this.btnInfantsPlus, 2);
            this.btnInfantsPlus.Size = new System.Drawing.Size(34, 39);
            this.btnInfantsPlus.TabIndex = 14;
            this.btnInfantsPlus.Text = "+";
            this.btnInfantsPlus.UseVisualStyleBackColor = false;
            this.btnInfantsPlus.Click += new System.EventHandler(this.btnInfantsPlus_Click);
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