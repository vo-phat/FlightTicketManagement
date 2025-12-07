namespace GUI.Features.Payments.SubFeatures
{
    partial class FrmPayment
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
            picQR = new PictureBox();
            btnPay = new GUI.Components.Buttons.PrimaryButton();
            ((System.ComponentModel.ISupportInitialize)picQR).BeginInit();
            SuspendLayout();
            // 
            // picQR
            // 
            picQR.Location = new Point(143, 49);
            picQR.Name = "picQR";
            picQR.Size = new Size(128, 123);
            picQR.TabIndex = 0;
            picQR.TabStop = false;
            // 
            // btnPay
            // 
            btnPay.AutoSize = true;
            btnPay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnPay.BackColor = Color.FromArgb(155, 209, 243);
            btnPay.BorderColor = Color.FromArgb(40, 40, 40);
            btnPay.BorderThickness = 2;
            btnPay.CornerRadius = 22;
            btnPay.EnableHoverEffects = true;
            btnPay.FlatAppearance.BorderSize = 0;
            btnPay.FlatStyle = FlatStyle.Flat;
            btnPay.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnPay.ForeColor = Color.White;
            btnPay.HoverBackColor = Color.White;
            btnPay.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnPay.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnPay.Icon = null;
            btnPay.IconSize = new Size(22, 22);
            btnPay.IconSpacing = 10;
            btnPay.Location = new Point(113, 220);
            btnPay.Name = "btnPay";
            btnPay.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnPay.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnPay.NormalForeColor = Color.White;
            btnPay.Padding = new Padding(24, 10, 24, 10);
            btnPay.PreferredMaxWidth = 0;
            btnPay.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnPay.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnPay.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnPay.Size = new Size(188, 52);
            btnPay.TabIndex = 1;
            btnPay.Text = "Thanh toán";
            btnPay.TextAlign = ContentAlignment.MiddleLeft;
            btnPay.UseVisualStyleBackColor = false;
            btnPay.WordWrap = false;
            btnPay.Click += btnPay_Click;
            // 
            // FrmPayment
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnPay);
            Controls.Add(picQR);
            Name = "FrmPayment";
            Size = new Size(394, 386);
            ((System.ComponentModel.ISupportInitialize)picQR).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picQR;
        private Components.Buttons.PrimaryButton btnPay;
    }
}
