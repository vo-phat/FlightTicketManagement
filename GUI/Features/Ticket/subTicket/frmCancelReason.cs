using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Features.Ticket.subTicket
{
    public class frmCancelReason : Form
    {
        private ComboBox cboReason;
        private TextBox txtNote;
        private Button btnConfirm;
        private Button btnCancel;

        // 👉 Chuỗi trả về cho bên gọi
        public string SelectedReason { get; private set; }

        public frmCancelReason()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // ===== FORM =====
            this.Text = "Lý do hủy vé";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(400, 220);

            // ===== LABEL =====
            var lblReason = new Label
            {
                Text = "Chọn lý do hủy:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            // ===== COMBOBOX =====
            cboReason = new ComboBox
            {
                Location = new Point(20, 45),
                Width = 350,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cboReason.Items.AddRange(new string[]
            {
                "Khách yêu cầu hủy",
                "Khách không đến (No-show)",
                "Sai thông tin vé",
                "Lỗi hệ thống",
                "Khác"
            });

            cboReason.SelectedIndexChanged += CboReason_SelectedIndexChanged;

            // ===== TEXTBOX NOTE =====
            txtNote = new TextBox
            {
                Location = new Point(20, 80),
                Width = 350,
                Height = 50,
                Multiline = true,
                Enabled = false
            };

            // ===== BUTTON CONFIRM =====
            btnConfirm = new Button
            {
                Text = "Xác nhận",
                Location = new Point(190, 150),
                Width = 80
            };
            btnConfirm.Click += BtnConfirm_Click;

            // ===== BUTTON CANCEL =====
            btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(290, 150),
                Width = 80
            };
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // ===== ADD CONTROL =====
            this.Controls.Add(lblReason);
            this.Controls.Add(cboReason);
            this.Controls.Add(txtNote);
            this.Controls.Add(btnConfirm);
            this.Controls.Add(btnCancel);
        }

        private void CboReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNote.Enabled = cboReason.SelectedItem?.ToString() == "Khác";
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (cboReason.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn lý do hủy");
                return;
            }

            var reason = cboReason.SelectedItem.ToString();

            if (reason == "Khác")
            {
                if (string.IsNullOrWhiteSpace(txtNote.Text))
                {
                    MessageBox.Show("Vui lòng nhập lý do cụ thể");
                    return;
                }

                reason += " - " + txtNote.Text.Trim();
            }

            SelectedReason = reason;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
