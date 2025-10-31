﻿namespace GUI.Features.Baggage
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
            pnlList = new Panel();
            pnlDetail = new Panel();
            pnlCheckin = new Panel();
            pnlHeaderBaggage = new Panel();
            btnLostBaggage = new GUI.Components.Buttons.PrimaryButton();
            btnDetailBaggage = new GUI.Components.Buttons.PrimaryButton();
            btnListBaggage = new GUI.Components.Buttons.PrimaryButton();
            btnCheckinBaggage = new GUI.Components.Buttons.PrimaryButton();
            flowLayoutPanel1 = new FlowLayoutPanel();
            pnlContentBaggage = new Panel();
            pnlLost = new Panel();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker4 = new System.ComponentModel.BackgroundWorker();
            pnlHeaderBaggage.SuspendLayout();
            pnlContentBaggage.SuspendLayout();
            SuspendLayout();
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
            pnlDetail.Paint += pnlDetail_Paint;
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
            pnlHeaderBaggage.Controls.Add(btnDetailBaggage);
            pnlHeaderBaggage.Controls.Add(btnListBaggage);
            pnlHeaderBaggage.Controls.Add(btnCheckinBaggage);
            pnlHeaderBaggage.Controls.Add(flowLayoutPanel1);
            pnlHeaderBaggage.Location = new Point(0, 0);
            pnlHeaderBaggage.Name = "pnlHeaderBaggage";
            pnlHeaderBaggage.Size = new Size(1163, 93);
            pnlHeaderBaggage.TabIndex = 5;
            // 
            // btnLostBaggage
            // 
            btnLostBaggage.AutoSize = true;
            btnLostBaggage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnLostBaggage.BackColor = Color.FromArgb(155, 209, 243);
            btnLostBaggage.BorderColor = Color.FromArgb(40, 40, 40);
            btnLostBaggage.BorderThickness = 2;
            btnLostBaggage.CornerRadius = 22;
            btnLostBaggage.EnableHoverEffects = true;
            btnLostBaggage.FlatAppearance.BorderSize = 0;
            btnLostBaggage.FlatStyle = FlatStyle.Flat;
            btnLostBaggage.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLostBaggage.ForeColor = Color.White;
            btnLostBaggage.HoverBackColor = Color.White;
            btnLostBaggage.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnLostBaggage.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnLostBaggage.Icon = null;
            btnLostBaggage.IconSize = new Size(22, 22);
            btnLostBaggage.IconSpacing = 10;
            btnLostBaggage.Location = new Point(789, 19);
            btnLostBaggage.Name = "btnLostBaggage";
            btnLostBaggage.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnLostBaggage.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnLostBaggage.NormalForeColor = Color.White;
            btnLostBaggage.Padding = new Padding(24, 10, 24, 10);
            btnLostBaggage.PreferredMaxWidth = 0;
            btnLostBaggage.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnLostBaggage.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnLostBaggage.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnLostBaggage.Size = new Size(233, 52);
            btnLostBaggage.TabIndex = 7;
            btnLostBaggage.Text = "Báo cáo thất lạc";
            btnLostBaggage.TextAlign = ContentAlignment.MiddleLeft;
            btnLostBaggage.UseVisualStyleBackColor = false;
            btnLostBaggage.WordWrap = false;
            btnLostBaggage.Click += btnLostBaggage_Click;
            // 
            // btnDetailBaggage
            // 
            btnDetailBaggage.AutoSize = true;
            btnDetailBaggage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnDetailBaggage.BackColor = Color.FromArgb(155, 209, 243);
            btnDetailBaggage.BorderColor = Color.FromArgb(40, 40, 40);
            btnDetailBaggage.BorderThickness = 2;
            btnDetailBaggage.CornerRadius = 22;
            btnDetailBaggage.EnableHoverEffects = true;
            btnDetailBaggage.FlatAppearance.BorderSize = 0;
            btnDetailBaggage.FlatStyle = FlatStyle.Flat;
            btnDetailBaggage.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnDetailBaggage.ForeColor = Color.White;
            btnDetailBaggage.HoverBackColor = Color.White;
            btnDetailBaggage.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnDetailBaggage.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnDetailBaggage.Icon = null;
            btnDetailBaggage.IconSize = new Size(22, 22);
            btnDetailBaggage.IconSpacing = 10;
            btnDetailBaggage.Location = new Point(506, 19);
            btnDetailBaggage.Name = "btnDetailBaggage";
            btnDetailBaggage.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnDetailBaggage.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnDetailBaggage.NormalForeColor = Color.White;
            btnDetailBaggage.Padding = new Padding(24, 10, 24, 10);
            btnDetailBaggage.PreferredMaxWidth = 0;
            btnDetailBaggage.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnDetailBaggage.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnDetailBaggage.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnDetailBaggage.Size = new Size(247, 52);
            btnDetailBaggage.TabIndex = 6;
            btnDetailBaggage.Text = "Theo dõi/ Chi tiết";
            btnDetailBaggage.TextAlign = ContentAlignment.MiddleLeft;
            btnDetailBaggage.UseVisualStyleBackColor = false;
            btnDetailBaggage.WordWrap = false;
            btnDetailBaggage.Click += btnDetailBaggage_Click;
            // 
            // btnListBaggage
            // 
            btnListBaggage.AutoSize = true;
            btnListBaggage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnListBaggage.BackColor = Color.FromArgb(155, 209, 243);
            btnListBaggage.BorderColor = Color.FromArgb(40, 40, 40);
            btnListBaggage.BorderThickness = 2;
            btnListBaggage.CornerRadius = 22;
            btnListBaggage.EnableHoverEffects = true;
            btnListBaggage.FlatAppearance.BorderSize = 0;
            btnListBaggage.FlatStyle = FlatStyle.Flat;
            btnListBaggage.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnListBaggage.ForeColor = Color.White;
            btnListBaggage.HoverBackColor = Color.White;
            btnListBaggage.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnListBaggage.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnListBaggage.Icon = null;
            btnListBaggage.IconSize = new Size(22, 22);
            btnListBaggage.IconSpacing = 10;
            btnListBaggage.Location = new Point(3, 19);
            btnListBaggage.Name = "btnListBaggage";
            btnListBaggage.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnListBaggage.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnListBaggage.NormalForeColor = Color.White;
            btnListBaggage.Padding = new Padding(24, 10, 24, 10);
            btnListBaggage.PreferredMaxWidth = 0;
            btnListBaggage.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnListBaggage.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnListBaggage.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnListBaggage.Size = new Size(254, 52);
            btnListBaggage.TabIndex = 5;
            btnListBaggage.Text = "Danh sách hành lý";
            btnListBaggage.TextAlign = ContentAlignment.MiddleLeft;
            btnListBaggage.UseVisualStyleBackColor = false;
            btnListBaggage.WordWrap = false;
            btnListBaggage.Click += btnListBaggage_Click;
            // 
            // btnCheckinBaggage
            // 
            btnCheckinBaggage.AutoSize = true;
            btnCheckinBaggage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnCheckinBaggage.BackColor = Color.FromArgb(155, 209, 243);
            btnCheckinBaggage.BorderColor = Color.FromArgb(40, 40, 40);
            btnCheckinBaggage.BorderThickness = 2;
            btnCheckinBaggage.CornerRadius = 22;
            btnCheckinBaggage.EnableHoverEffects = true;
            btnCheckinBaggage.FlatAppearance.BorderSize = 0;
            btnCheckinBaggage.FlatStyle = FlatStyle.Flat;
            btnCheckinBaggage.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCheckinBaggage.ForeColor = Color.White;
            btnCheckinBaggage.HoverBackColor = Color.White;
            btnCheckinBaggage.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnCheckinBaggage.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnCheckinBaggage.Icon = null;
            btnCheckinBaggage.IconSize = new Size(22, 22);
            btnCheckinBaggage.IconSpacing = 10;
            btnCheckinBaggage.Location = new Point(257, 19);
            btnCheckinBaggage.Name = "btnCheckinBaggage";
            btnCheckinBaggage.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnCheckinBaggage.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnCheckinBaggage.NormalForeColor = Color.White;
            btnCheckinBaggage.Padding = new Padding(24, 10, 24, 10);
            btnCheckinBaggage.PreferredMaxWidth = 0;
            btnCheckinBaggage.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnCheckinBaggage.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnCheckinBaggage.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnCheckinBaggage.Size = new Size(243, 52);
            btnCheckinBaggage.TabIndex = 4;
            btnCheckinBaggage.Text = "Gắn tag/ Checkin";
            btnCheckinBaggage.TextAlign = ContentAlignment.MiddleLeft;
            btnCheckinBaggage.UseVisualStyleBackColor = false;
            btnCheckinBaggage.WordWrap = false;
            btnCheckinBaggage.Click += btnCheckinBaggage_Click;
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
            pnlHeaderBaggage.PerformLayout();
            pnlContentBaggage.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlList;
        private Panel pnlCheckin;
        private Panel pnlDetail;
        private Panel pnlHeaderBaggage;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel pnlContentBaggage;
        private Panel pnlLost;
        private Components.Buttons.PrimaryButton btnCheckinBaggage;
        private Components.Buttons.PrimaryButton btnListBaggage;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.ComponentModel.BackgroundWorker backgroundWorker4;
        private Components.Buttons.PrimaryButton btnDetailBaggage;
        private Components.Buttons.PrimaryButton btnLostBaggage;
    }
}
