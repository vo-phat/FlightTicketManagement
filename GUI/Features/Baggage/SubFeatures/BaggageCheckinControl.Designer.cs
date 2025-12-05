using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;

namespace GUI.Features.Baggage.SubFeatures {
    partial class BaggageCheckinControl {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent() {
            components = new System.ComponentModel.Container();

            label1 = new Label();

            txtNumTicketCheckin = new UnderlinedTextField("Số vé", "");
            txtCodeCheckin = new UnderlinedTextField("Mã nhãn", "");
            txtSpecialHandlingCheckin = new UnderlinedTextField("Xử lý đặc biệt", "");
            txtActualWeightCheckin = new UnderlinedTextField("Cân nặng thực tế (Kg)", "");
            txtExtraFeeCheckin = new UnderlinedTextField("Phí phát sinh", "");
            owedWeightCheckin = new UnderlinedTextField("Định mức miễn cước (Kg)", "");
            txtBaggageTypeCheckin = new UnderlinedTextField("Loại", "");
            txtFlightCodeCheckin = new UnderlinedTextField("Mã chuyến bay", "");

            btnSaveAndPrintCheckin = new PrimaryButton();

            // layout local
            var mainLayout = new TableLayoutPanel();
            var titlePanel = new Panel();
            var inputsLayout = new TableLayoutPanel();
            var buttonRow = new FlowLayoutPanel();

            SuspendLayout();

            // ===== Root control =====
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(232, 240, 252);
            Name = "BaggageCheckinControl";
            Size = new Size(1063, 565);

            // ===== Main layout (Title + Inputs + Buttons) =====
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.ColumnCount = 1;
            mainLayout.RowCount = 3;
            mainLayout.Padding = new Padding(24, 8, 24, 24);
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // title
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // inputs
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // buttons

            Controls.Add(mainLayout);

            // ===== Title panel =====
            titlePanel.Dock = DockStyle.Top;
            titlePanel.Height = 64;
            titlePanel.Padding = new Padding(0, 8, 0, 8);
            titlePanel.BackColor = Color.Transparent;

            label1.AutoSize = true;
            label1.Text = "Gắn tag / Check-in hành lý";
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(40, 40, 40);
            label1.Dock = DockStyle.Left;
            label1.Click += label1_Click; // nếu bạn có handler

            titlePanel.Controls.Add(label1);
            mainLayout.Controls.Add(titlePanel, 0, 0);

            // ===== Inputs layout (2 cột x 4 hàng) =====
            inputsLayout.Dock = DockStyle.Top;
            inputsLayout.ColumnCount = 2;
            inputsLayout.RowCount = 4;
            inputsLayout.AutoSize = true;
            inputsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            inputsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            inputsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            inputsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            inputsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            inputsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            inputsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            inputsLayout.Margin = new Padding(0, 8, 0, 0);

            // common config cho UnderlinedTextField
            void ConfigField(UnderlinedTextField field) {
                field.Width = 300;
                field.Margin = new Padding(0, 6, 24, 6);
            }

            ConfigField(txtNumTicketCheckin);
            ConfigField(txtCodeCheckin);
            ConfigField(txtSpecialHandlingCheckin);
            ConfigField(txtActualWeightCheckin);
            ConfigField(txtExtraFeeCheckin);
            ConfigField(owedWeightCheckin);
            ConfigField(txtBaggageTypeCheckin);
            ConfigField(txtFlightCodeCheckin);

            // Numeric-only cho cân nặng, định mức, phí phát sinh
            txtActualWeightCheckin.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };
            owedWeightCheckin.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };
            txtExtraFeeCheckin.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };

            // ===== HÀNG 0: Số vé – Mã chuyến bay =====
            inputsLayout.Controls.Add(txtNumTicketCheckin, 0, 0);
            inputsLayout.Controls.Add(txtFlightCodeCheckin, 1, 0);

            // ===== HÀNG 1: Mã nhãn – Loại =====
            inputsLayout.Controls.Add(txtCodeCheckin, 0, 1);
            inputsLayout.Controls.Add(txtBaggageTypeCheckin, 1, 1);

            // ===== HÀNG 2: Cân nặng thực tế – Định mức miễn cước =====
            inputsLayout.Controls.Add(txtActualWeightCheckin, 0, 2);
            inputsLayout.Controls.Add(owedWeightCheckin, 1, 2);

            // ===== HÀNG 3: Xử lý đặc biệt – Phí phát sinh =====
            inputsLayout.Controls.Add(txtSpecialHandlingCheckin, 0, 3);
            inputsLayout.Controls.Add(txtExtraFeeCheckin, 1, 3);

            mainLayout.Controls.Add(inputsLayout, 0, 1);

            // ===== Button row =====
            buttonRow.Dock = DockStyle.Top;
            buttonRow.FlowDirection = FlowDirection.RightToLeft;
            buttonRow.AutoSize = true;
            buttonRow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            buttonRow.Padding = new Padding(0, 12, 0, 0);

            // Giữ style PrimaryButton hiện tại cho đẹp
            btnSaveAndPrintCheckin.AutoSize = true;
            btnSaveAndPrintCheckin.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSaveAndPrintCheckin.BackColor = Color.FromArgb(155, 209, 243);
            btnSaveAndPrintCheckin.BorderColor = Color.FromArgb(40, 40, 40);
            btnSaveAndPrintCheckin.BorderThickness = 2;
            btnSaveAndPrintCheckin.CornerRadius = 22;
            btnSaveAndPrintCheckin.EnableHoverEffects = true;
            btnSaveAndPrintCheckin.FlatAppearance.BorderSize = 0;
            btnSaveAndPrintCheckin.FlatStyle = FlatStyle.Flat;
            btnSaveAndPrintCheckin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSaveAndPrintCheckin.ForeColor = Color.White;
            btnSaveAndPrintCheckin.HoverBackColor = Color.White;
            btnSaveAndPrintCheckin.HoverBorderColor = Color.FromArgb(0, 92, 175);
            btnSaveAndPrintCheckin.HoverForeColor = Color.FromArgb(0, 92, 175);
            btnSaveAndPrintCheckin.Icon = null;
            btnSaveAndPrintCheckin.IconSize = new Size(22, 22);
            btnSaveAndPrintCheckin.IconSpacing = 10;
            btnSaveAndPrintCheckin.Text = "Lưu và in tag";
            btnSaveAndPrintCheckin.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndPrintCheckin.NormalBackColor = Color.FromArgb(155, 209, 243);
            btnSaveAndPrintCheckin.NormalBorderColor = Color.FromArgb(40, 40, 40);
            btnSaveAndPrintCheckin.NormalForeColor = Color.White;
            btnSaveAndPrintCheckin.Padding = new Padding(24, 10, 24, 10);
            btnSaveAndPrintCheckin.PreferredMaxWidth = 0;
            btnSaveAndPrintCheckin.PressedBackColor = Color.FromArgb(225, 240, 255);
            btnSaveAndPrintCheckin.PressedBorderColor = Color.FromArgb(0, 92, 175);
            btnSaveAndPrintCheckin.PressedForeColor = Color.FromArgb(0, 92, 175);
            btnSaveAndPrintCheckin.Margin = new Padding(0, 0, 0, 0);
            // event Click gắn ở file .cs chính

            buttonRow.Controls.Add(btnSaveAndPrintCheckin);
            mainLayout.Controls.Add(buttonRow, 0, 2);

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private UnderlinedTextField txtNumTicketCheckin;
        private UnderlinedTextField txtCodeCheckin;
        private UnderlinedTextField txtSpecialHandlingCheckin;
        private UnderlinedTextField txtActualWeightCheckin;
        private UnderlinedTextField txtExtraFeeCheckin;
        private UnderlinedTextField owedWeightCheckin;
        private UnderlinedTextField txtBaggageTypeCheckin;
        private UnderlinedTextField txtFlightCodeCheckin;
        private PrimaryButton btnSaveAndPrintCheckin;
    }
}
