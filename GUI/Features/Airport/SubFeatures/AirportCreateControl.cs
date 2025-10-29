using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;
using BUS.Airport;
using DTO.Airport;

namespace GUI.Features.Airport.SubFeatures
{
    public class AirportCreateControl : UserControl
    {
        private UnderlinedTextField _txtCode, _txtName, _txtCity;
        private UnderlinedComboBox _cbCountry, _cbTimezone;
        private PrimaryButton _btnSave;
        private Panel titlePanel;
        private Label lblTitle;
        private TableLayoutPanel inputs;
        private FlowLayoutPanel buttonRow;
        private TableLayoutPanel main;
        private readonly AirportBUS _bus = new AirportBUS();

        public event EventHandler? AirportAdded; // để thông báo cho danh sách reload

        public AirportCreateControl() { InitializeComponent(); }

        private void InitializeComponent()
        {
            titlePanel = new Panel();
            lblTitle = new Label();
            inputs = new TableLayoutPanel();
            _txtCode = new UnderlinedTextField();
            _txtName = new UnderlinedTextField();
            _txtCity = new UnderlinedTextField();
            _cbCountry = new UnderlinedComboBox();
            _cbTimezone = new UnderlinedComboBox();
            buttonRow = new FlowLayoutPanel();
            main = new TableLayoutPanel();
            titlePanel.SuspendLayout();
            inputs.SuspendLayout();
            main.SuspendLayout();
            SuspendLayout();
            // 
            // titlePanel
            // 
            titlePanel.Controls.Add(lblTitle);
            titlePanel.Location = new Point(3, 3);
            titlePanel.Name = "titlePanel";
            titlePanel.Size = new Size(194, 100);
            titlePanel.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(100, 23);
            lblTitle.TabIndex = 0;
            // 
            // inputs
            // 
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            inputs.Controls.Add(_txtCode, 0, 0);
            inputs.Controls.Add(_txtName, 1, 0);
            inputs.Controls.Add(_txtCity, 0, 1);
            inputs.Controls.Add(_cbCountry, 1, 1);
            inputs.Controls.Add(_cbTimezone, 0, 2);
            inputs.Location = new Point(3, 109);
            inputs.Name = "inputs";
            inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            inputs.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            inputs.Size = new Size(194, 100);
            inputs.TabIndex = 1;
            // 
            // _txtCode
            // 
            _txtCode.BackColor = Color.Transparent;
            _txtCode.FocusedLineThickness = 3;
            _txtCode.InheritParentBackColor = true;
            _txtCode.LabelForeColor = Color.FromArgb(70, 70, 70);
            _txtCode.LabelText = "Mã IATA";
            _txtCode.LineColor = Color.FromArgb(40, 40, 40);
            _txtCode.LineColorFocused = Color.FromArgb(0, 92, 175);
            _txtCode.LineThickness = 2;
            _txtCode.Location = new Point(3, 3);
            _txtCode.Name = "_txtCode";
            _txtCode.Padding = new Padding(0, 4, 0, 8);
            _txtCode.PasswordChar = '\0';
            _txtCode.PlaceholderText = "";
            _txtCode.ReadOnly = false;
            _txtCode.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            _txtCode.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            _txtCode.Size = new Size(91, 14);
            _txtCode.TabIndex = 0;
            _txtCode.TextForeColor = Color.FromArgb(30, 30, 30);
            _txtCode.UnderlineSpacing = 2;
            _txtCode.UseSystemPasswordChar = false;
            // 
            // _txtName
            // 
            _txtName.BackColor = Color.Transparent;
            _txtName.FocusedLineThickness = 3;
            _txtName.InheritParentBackColor = true;
            _txtName.LabelForeColor = Color.FromArgb(70, 70, 70);
            _txtName.LabelText = "Tên sân bay";
            _txtName.LineColor = Color.FromArgb(40, 40, 40);
            _txtName.LineColorFocused = Color.FromArgb(0, 92, 175);
            _txtName.LineThickness = 2;
            _txtName.Location = new Point(100, 3);
            _txtName.Name = "_txtName";
            _txtName.Padding = new Padding(0, 4, 0, 8);
            _txtName.PasswordChar = '\0';
            _txtName.PlaceholderText = "";
            _txtName.ReadOnly = false;
            _txtName.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            _txtName.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            _txtName.Size = new Size(91, 14);
            _txtName.TabIndex = 1;
            _txtName.TextForeColor = Color.FromArgb(30, 30, 30);
            _txtName.UnderlineSpacing = 2;
            _txtName.UseSystemPasswordChar = false;
            // 
            // _txtCity
            // 
            _txtCity.BackColor = Color.Transparent;
            _txtCity.FocusedLineThickness = 3;
            _txtCity.InheritParentBackColor = true;
            _txtCity.LabelForeColor = Color.FromArgb(70, 70, 70);
            _txtCity.LabelText = "Thành phố";
            _txtCity.LineColor = Color.FromArgb(40, 40, 40);
            _txtCity.LineColorFocused = Color.FromArgb(0, 92, 175);
            _txtCity.LineThickness = 2;
            _txtCity.Location = new Point(3, 23);
            _txtCity.Name = "_txtCity";
            _txtCity.Padding = new Padding(0, 4, 0, 8);
            _txtCity.PasswordChar = '\0';
            _txtCity.PlaceholderText = "";
            _txtCity.ReadOnly = false;
            _txtCity.ReadOnlyLineColor = Color.FromArgb(200, 200, 200);
            _txtCity.ReadOnlyTextColor = Color.FromArgb(90, 90, 90);
            _txtCity.Size = new Size(91, 14);
            _txtCity.TabIndex = 2;
            _txtCity.TextForeColor = Color.FromArgb(30, 30, 30);
            _txtCity.UnderlineSpacing = 2;
            _txtCity.UseSystemPasswordChar = false;
            // 
            // _cbCountry
            // 
            _cbCountry.BackColor = Color.Transparent;
            _cbCountry.Items.AddRange(new object[] { "Việt Nam", "Nhật Bản", "Hàn Quốc", "Singapore", "Thái Lan", "Hoa Kỳ", "Anh", "Pháp", "Úc", "Canada" });
            _cbCountry.LabelText = "Quốc gia";
            _cbCountry.Location = new Point(100, 23);
            _cbCountry.MinimumSize = new Size(140, 56);
            _cbCountry.Name = "_cbCountry";
            _cbCountry.SelectedIndex = -1;
            _cbCountry.SelectedItem = null;
            _cbCountry.SelectedText = "";
            _cbCountry.Size = new Size(140, 56);
            _cbCountry.TabIndex = 3;
            // 
            // _cbTimezone
            // 
            //_cbTimezone.BackColor = Color.Transparent;
            //_cbTimezone.Items.AddRange(new object[] { "UTC−5", "UTC−4", "UTC", "UTC+1", "UTC+7", "UTC+8", "UTC+9" });
            //_cbTimezone.LabelText = "Múi giờ";
            //_cbTimezone.Location = new Point(3, 43);
            //_cbTimezone.MinimumSize = new Size(140, 56);
            //_cbTimezone.Name = "_cbTimezone";
            //_cbTimezone.SelectedIndex = -1;
            //_cbTimezone.SelectedItem = null;
            //_cbTimezone.SelectedText = "";
            //_cbTimezone.Size = new Size(140, 56);
            //_cbTimezone.TabIndex = 4;
            // 
            // buttonRow
            // 
            buttonRow.Location = new Point(3, 215);
            buttonRow.Name = "buttonRow";
            buttonRow.Size = new Size(194, 1);
            buttonRow.TabIndex = 2;
            // 
            // main
            // 
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            main.Controls.Add(titlePanel, 0, 0);
            main.Controls.Add(inputs, 0, 1);
            main.Controls.Add(buttonRow, 0, 2);
            main.Location = new Point(0, 0);
            main.Name = "main";
            main.RowStyles.Add(new RowStyle());
            main.RowStyles.Add(new RowStyle());
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            main.Size = new Size(200, 100);
            main.TabIndex = 0;
            // 
            // AirportCreateControl
            // 
            BackColor = Color.FromArgb(232, 240, 252);
            Controls.Add(main);
            Name = "AirportCreateControl";
            Size = new Size(991, 582);
            Load += AirportCreateControl_Load;
            titlePanel.ResumeLayout(false);
            inputs.ResumeLayout(false);
            main.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            var airport = new AirportDTO(
                _txtCode.Text.Trim(),
                _txtName.Text.Trim(),
                _txtCity.Text.Trim(),
                _cbCountry.SelectedItem?.ToString() ?? ""
            );

            if (_bus.AddAirport(airport, out string message))
            {
                MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AirportAdded?.Invoke(this, EventArgs.Empty); // Thông báo để reload danh sách
                ClearInputs();
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputs()
        {
            _txtCode.Text = _txtName.Text = _txtCity.Text = "";
            _cbCountry.SelectedIndex = -1;
            _cbTimezone.SelectedIndex = -1;
        }

        private void AirportCreateControl_Load(object sender, EventArgs e)
        {
            
        }
    }
}
