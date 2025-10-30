namespace GUI.Features.Flight
{
    partial class FlightControl
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
            panel1 = new Panel();
            buttonTaoMoiChuyenBay = new Button();
            buttonDanhSachChuyenBay = new Button();
            panelContent = new Panel();
            panelFlightList = new Panel();
            panelFlightDetail = new Panel();
            panelFlightCreate = new Panel();
            panel1.SuspendLayout();
            panelContent.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(buttonTaoMoiChuyenBay);
            panel1.Controls.Add(buttonDanhSachChuyenBay);
            panel1.Location = new Point(12, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1283, 66);
            panel1.TabIndex = 0;
            // 
            // buttonTaoMoiChuyenBay
            // 
            buttonTaoMoiChuyenBay.Location = new Point(171, 22);
            buttonTaoMoiChuyenBay.Name = "buttonTaoMoiChuyenBay";
            buttonTaoMoiChuyenBay.Size = new Size(75, 23);
            buttonTaoMoiChuyenBay.TabIndex = 1;
            buttonTaoMoiChuyenBay.Text = "Tạo mới chuyến bay";
            buttonTaoMoiChuyenBay.UseVisualStyleBackColor = true;
            buttonTaoMoiChuyenBay.Click += buttonTaoMoiChuyenBay_Click;
            // 
            // buttonDanhSachChuyenBay
            // 
            buttonDanhSachChuyenBay.Location = new Point(33, 22);
            buttonDanhSachChuyenBay.Name = "buttonDanhSachChuyenBay";
            buttonDanhSachChuyenBay.Size = new Size(75, 23);
            buttonDanhSachChuyenBay.TabIndex = 0;
            buttonDanhSachChuyenBay.Text = "Danh sách chuyến bay";
            buttonDanhSachChuyenBay.UseVisualStyleBackColor = true;
            buttonDanhSachChuyenBay.Click += buttonDanhSachChuyenBay_Click;
            // 
            // panelContent
            // 
            panelContent.Controls.Add(panelFlightList);
            panelContent.Controls.Add(panelFlightDetail);
            panelContent.Controls.Add(panelFlightCreate);
            panelContent.Location = new Point(12, 94);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(1283, 598);
            panelContent.TabIndex = 1;
            // 
            // panelFlightList
            // 
            panelFlightList.Location = new Point(578, 164);
            panelFlightList.Name = "panelFlightList";
            panelFlightList.Size = new Size(200, 100);
            panelFlightList.TabIndex = 1;
            // 
            // panelFlightDetail
            // 
            panelFlightDetail.Location = new Point(904, 380);
            panelFlightDetail.Name = "panelFlightDetail";
            panelFlightDetail.Size = new Size(200, 100);
            panelFlightDetail.TabIndex = 2;
            // 
            // panelFlightCreate
            // 
            panelFlightCreate.Location = new Point(299, 233);
            panelFlightCreate.Name = "panelFlightCreate";
            panelFlightCreate.Size = new Size(200, 100);
            panelFlightCreate.TabIndex = 0;
            // 
            // FlightControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelContent);
            Controls.Add(panel1);
            Name = "FlightControl";
            Size = new Size(1229, 605);
            Load += FlightControl_Load;
            panel1.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button buttonDanhSachChuyenBay;
        private Panel panelContent;
        private Panel panelFlightDetail;
        private Panel panelFlightCreate;
        private Button buttonTaoMoiChuyenBay;
        private Panel panelFlightList;
    }
}
