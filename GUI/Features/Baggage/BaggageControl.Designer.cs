using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Baggage {
    partial class BaggageControl {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent() {
            pnlHeaderBaggage = new FlowLayoutPanel();
            btnListBaggage = new GUI.Components.Buttons.PrimaryButton();
            btnCheckinBaggage = new GUI.Components.Buttons.PrimaryButton();
            btnDetailBaggage = new GUI.Components.Buttons.PrimaryButton();
            btnLostBaggage = new GUI.Components.Buttons.PrimaryButton();

            pnlContentBaggage = new Panel();
            pnlList = new Panel();
            pnlCheckin = new Panel();
            pnlDetail = new Panel();
            pnlLost = new Panel();

            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker4 = new System.ComponentModel.BackgroundWorker();

            pnlHeaderBaggage.SuspendLayout();
            pnlContentBaggage.SuspendLayout();
            SuspendLayout();

            // ============================================================
            // HEADER – FlowLayoutPanel chứa 4 tab nút
            // ============================================================
            pnlHeaderBaggage.Controls.Add(btnListBaggage);
            pnlHeaderBaggage.Controls.Add(btnCheckinBaggage);
            pnlHeaderBaggage.Controls.Add(btnDetailBaggage);
            pnlHeaderBaggage.Controls.Add(btnLostBaggage);

            pnlHeaderBaggage.Dock = DockStyle.Top;
            pnlHeaderBaggage.Height = 60;
            pnlHeaderBaggage.Name = "pnlHeaderBaggage";
            pnlHeaderBaggage.TabIndex = 0;
            pnlHeaderBaggage.BackColor = Color.White;
            pnlHeaderBaggage.Padding = new Padding(24, 12, 0, 0);
            pnlHeaderBaggage.AutoSize = true;
            pnlHeaderBaggage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlHeaderBaggage.WrapContents = false;

            // ============================================================
            // BUTTON – DANH SÁCH HÀNH LÝ
            // ============================================================
            btnListBaggage.AutoSize = true;
            btnListBaggage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnListBaggage.Text = "Danh sách hành lý";
            btnListBaggage.Padding = new Padding(24, 10, 24, 10);
            btnListBaggage.Click += btnListBaggage_Click;

            // ============================================================
            // BUTTON – GẮN TAG / CHECKIN
            // ============================================================
            btnCheckinBaggage.AutoSize = true;
            btnCheckinBaggage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnCheckinBaggage.Text = "Gắn tag/ Checkin";
            btnCheckinBaggage.Padding = new Padding(24, 10, 24, 10);
            btnCheckinBaggage.Click += btnCheckinBaggage_Click;

            // ============================================================
            // BUTTON – THEO DÕI / CHI TIẾT
            // ============================================================
            btnDetailBaggage.AutoSize = true;
            btnDetailBaggage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnDetailBaggage.Text = "Theo dõi/ Chi tiết";
            btnDetailBaggage.Padding = new Padding(24, 10, 24, 10);
            btnDetailBaggage.Click += btnDetailBaggage_Click;

            // ============================================================
            // BUTTON – BÁO CÁO THẤT LẠC
            // ============================================================
            btnLostBaggage.AutoSize = true;
            btnLostBaggage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnLostBaggage.Text = "Báo cáo thất lạc";
            btnLostBaggage.Padding = new Padding(24, 10, 24, 10);
            btnLostBaggage.Click += btnLostBaggage_Click;

            // ============================================================
            // CONTENT – Panel cha chứa 4 panel con (DockFill)
            // ============================================================
            pnlContentBaggage.Controls.Add(pnlLost);
            pnlContentBaggage.Controls.Add(pnlDetail);
            pnlContentBaggage.Controls.Add(pnlCheckin);
            pnlContentBaggage.Controls.Add(pnlList);
            pnlContentBaggage.Dock = DockStyle.Fill;
            pnlContentBaggage.Name = "pnlContentBaggage";
            pnlContentBaggage.TabIndex = 1;

            // 4 panel con: Dock Fill, để logic switchTab bật/tắt Visible
            pnlList.Dock = DockStyle.Fill;
            pnlList.Name = "pnlList";
            pnlList.TabIndex = 0;

            pnlCheckin.Dock = DockStyle.Fill;
            pnlCheckin.Name = "pnlCheckin";
            pnlCheckin.TabIndex = 1;

            pnlDetail.Dock = DockStyle.Fill;
            pnlDetail.Name = "pnlDetail";
            pnlDetail.TabIndex = 2;
            pnlDetail.Paint += pnlDetail_Paint;

            pnlLost.Dock = DockStyle.Fill;
            pnlLost.Name = "pnlLost";
            pnlLost.TabIndex = 3;

            // ============================================================
            // ROOT CONTROL
            // ============================================================
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlContentBaggage);
            Controls.Add(pnlHeaderBaggage);
            Name = "BaggageControl";
            Size = new Size(1163, 587);

            pnlHeaderBaggage.ResumeLayout(false);
            pnlHeaderBaggage.PerformLayout();
            pnlContentBaggage.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel pnlHeaderBaggage;
        private Panel pnlContentBaggage;

        private Panel pnlList;
        private Panel pnlCheckin;
        private Panel pnlDetail;
        private Panel pnlLost;

        private Button btnListBaggage;
        private Button btnCheckinBaggage;
        private Button btnDetailBaggage;
        private Button btnLostBaggage;

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.ComponentModel.BackgroundWorker backgroundWorker4;
    }
}
