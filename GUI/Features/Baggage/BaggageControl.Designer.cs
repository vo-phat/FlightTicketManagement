namespace GUI.Features.Baggage
{
    partial class BaggageControl
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
            btnListBaggage = new Button();
            btnCheckinBaggage = new Button();
            btnDetailBaggage = new Button();
            pnlList = new Panel();
            pnlDetail = new Panel();
            pnlCheckin = new Panel();
            pnlHeaderBaggage = new Panel();
            btnLostBaggage = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            pnlContentBaggage = new Panel();
            pnlLost = new Panel();
            pnlHeaderBaggage.SuspendLayout();
            pnlContentBaggage.SuspendLayout();
            SuspendLayout();
            // 
            // btnListBaggage
            // 
            btnListBaggage.Location = new Point(14, 34);
            btnListBaggage.Name = "btnListBaggage";
            btnListBaggage.Size = new Size(194, 29);
            btnListBaggage.TabIndex = 0;
            btnListBaggage.Text = "Danh sách hành lý";
            btnListBaggage.UseVisualStyleBackColor = true;
            btnListBaggage.Click += btnListBaggage_Click;
            // 
            // btnCheckinBaggage
            // 
            btnCheckinBaggage.Location = new Point(244, 34);
            btnCheckinBaggage.Name = "btnCheckinBaggage";
            btnCheckinBaggage.Size = new Size(169, 29);
            btnCheckinBaggage.TabIndex = 1;
            btnCheckinBaggage.Text = "Gắn tag/ Check_in";
            btnCheckinBaggage.UseVisualStyleBackColor = true;
            btnCheckinBaggage.Click += btnCheckinBaggage_Click;
            // 
            // btnDetailBaggage
            // 
            btnDetailBaggage.Location = new Point(437, 34);
            btnDetailBaggage.Name = "btnDetailBaggage";
            btnDetailBaggage.Size = new Size(192, 29);
            btnDetailBaggage.TabIndex = 2;
            btnDetailBaggage.Text = "Theo dõi/ Chi tiết";
            btnDetailBaggage.UseVisualStyleBackColor = true;
            btnDetailBaggage.Click += btnDetailBaggage_Click;
            // 
            // pnlList
            // 
            pnlList.Location = new Point(360, 359);
            pnlList.Name = "pnlList";
            pnlList.Size = new Size(123, 94);
            pnlList.TabIndex = 3;
            // 
            // pnlDetail
            // 
            pnlDetail.Location = new Point(559, 270);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Size = new Size(156, 91);
            pnlDetail.TabIndex = 4;
            // 
            // pnlCheckin
            // 
            pnlCheckin.Location = new Point(351, 214);
            pnlCheckin.Name = "pnlCheckin";
            pnlCheckin.Size = new Size(120, 100);
            pnlCheckin.TabIndex = 0;
            // 
            // pnlHeaderBaggage
            // 
            pnlHeaderBaggage.Controls.Add(btnLostBaggage);
            pnlHeaderBaggage.Controls.Add(flowLayoutPanel1);
            pnlHeaderBaggage.Controls.Add(btnListBaggage);
            pnlHeaderBaggage.Controls.Add(btnDetailBaggage);
            pnlHeaderBaggage.Controls.Add(btnCheckinBaggage);
            pnlHeaderBaggage.Location = new Point(0, 0);
            pnlHeaderBaggage.Name = "pnlHeaderBaggage";
            pnlHeaderBaggage.Size = new Size(1163, 93);
            pnlHeaderBaggage.TabIndex = 5;
            // 
            // btnLostBaggage
            // 
            btnLostBaggage.Location = new Point(685, 34);
            btnLostBaggage.Name = "btnLostBaggage";
            btnLostBaggage.Size = new Size(207, 29);
            btnLostBaggage.TabIndex = 3;
            btnLostBaggage.Text = "Báo cáo thất lạc";
            btnLostBaggage.UseVisualStyleBackColor = true;
            btnLostBaggage.Click += btnLostBaggage_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Location = new Point(1113, 98);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(8, 9);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // pnlContentBaggage
            // 
            pnlContentBaggage.Controls.Add(pnlLost);
            pnlContentBaggage.Location = new Point(0, 98);
            pnlContentBaggage.Name = "pnlContentBaggage";
            pnlContentBaggage.Size = new Size(1163, 489);
            pnlContentBaggage.TabIndex = 6;
            // 
            // pnlLost
            // 
            pnlLost.Location = new Point(817, 212);
            pnlLost.Name = "pnlLost";
            pnlLost.Size = new Size(250, 125);
            pnlLost.TabIndex = 0;
            // 
            // BaggageControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlList);
            Controls.Add(pnlDetail);
            Controls.Add(pnlCheckin);
            Controls.Add(pnlHeaderBaggage);
            Controls.Add(pnlContentBaggage);
            Name = "BaggageControl";
            Size = new Size(1163, 587);
            pnlHeaderBaggage.ResumeLayout(false);
            pnlContentBaggage.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnListBaggage;
        private Button btnCheckinBaggage;
        private Button btnDetailBaggage;
        private Panel pnlList;
        private Panel pnlCheckin;
        private Panel pnlDetail;
        private Panel pnlHeaderBaggage;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel pnlContentBaggage;
        private Button btnLostBaggage;
        private Panel pnlLost;
    }
}
