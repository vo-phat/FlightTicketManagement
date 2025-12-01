namespace GUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.btnTestDTO = new System.Windows.Forms.Button();
            this.btnTestBaseDAO = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnTestFlightDAO = new System.Windows.Forms.Button();
            this.btnTestFlightBUS = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnTestConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestConnection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTestConnection.ForeColor = System.Drawing.Color.White;
            this.btnTestConnection.Location = new System.Drawing.Point(12, 12);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(200, 40);
            this.btnTestConnection.TabIndex = 0;
            this.btnTestConnection.Text = "1. Test Database Connection";
            this.btnTestConnection.UseVisualStyleBackColor = false;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // btnTestDTO
            // 
            this.btnTestDTO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnTestDTO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestDTO.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTestDTO.ForeColor = System.Drawing.Color.White;
            this.btnTestDTO.Location = new System.Drawing.Point(12, 58);
            this.btnTestDTO.Name = "btnTestDTO";
            this.btnTestDTO.Size = new System.Drawing.Size(200, 40);
            this.btnTestDTO.TabIndex = 1;
            this.btnTestDTO.Text = "2. Test Flight DTO";
            this.btnTestDTO.UseVisualStyleBackColor = false;
            this.btnTestDTO.Click += new System.EventHandler(this.btnTestDTO_Click);
            // 
            // btnTestBaseDAO
            // 
            this.btnTestBaseDAO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnTestBaseDAO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestBaseDAO.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTestBaseDAO.ForeColor = System.Drawing.Color.White;
            this.btnTestBaseDAO.Location = new System.Drawing.Point(12, 104);
            this.btnTestBaseDAO.Name = "btnTestBaseDAO";
            this.btnTestBaseDAO.Size = new System.Drawing.Size(200, 40);
            this.btnTestBaseDAO.TabIndex = 2;
            this.btnTestBaseDAO.Text = "3. Test Base DAO";
            this.btnTestBaseDAO.UseVisualStyleBackColor = false;
            this.btnTestBaseDAO.Click += new System.EventHandler(this.btnTestBaseDAO_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(12, 150);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(200, 40);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear Result";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtResult.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.txtResult.Location = new System.Drawing.Point(230, 12);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(742, 507);
            this.txtResult.TabIndex = 4;
            this.txtResult.WordWrap = false;
            // 
            // Config button
            // 
            this.btnTestFlightDAO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnTestFlightDAO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestFlightDAO.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTestFlightDAO.ForeColor = System.Drawing.Color.White;
            this.btnTestFlightDAO.Location = new System.Drawing.Point(12, 196);
            this.btnTestFlightDAO.Name = "btnTestFlightDAO";
            this.btnTestFlightDAO.Size = new System.Drawing.Size(200, 40);
            this.btnTestFlightDAO.TabIndex = 5;
            this.btnTestFlightDAO.Text = "4. Test Flight DAO";
            this.btnTestFlightDAO.UseVisualStyleBackColor = false;
            this.btnTestFlightDAO.Click += new System.EventHandler(this.btnTestFlightDAO_Click);
            ///
            /// Test FlightBUS
            ///
            this.btnTestFlightBUS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.btnTestFlightBUS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestFlightBUS.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTestFlightBUS.ForeColor = System.Drawing.Color.White;
            this.btnTestFlightBUS.Location = new System.Drawing.Point(12, 242);
            this.btnTestFlightBUS.Name = "btnTestFlightBUS";
            this.btnTestFlightBUS.Size = new System.Drawing.Size(200, 40);
            this.btnTestFlightBUS.TabIndex = 6;
            this.btnTestFlightBUS.Text = "5. Test Flight BUS";
            this.btnTestFlightBUS.UseVisualStyleBackColor = false;
            this.btnTestFlightBUS.Click += new System.EventHandler(this.btnTestFlightBUS_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(984, 531);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnTestBaseDAO);
            this.Controls.Add(this.btnTestDTO);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.btnTestFlightDAO);
            this.Controls.Add(this.btnTestFlightBUS);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Flight Ticket Management - Test Console";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button btnTestDTO;
        private System.Windows.Forms.Button btnTestBaseDAO;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnTestFlightDAO;
        private System.Windows.Forms.Button btnTestFlightBUS;


    }
}