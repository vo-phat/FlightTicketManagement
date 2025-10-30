namespace GUI.Features.Flight.SubFeatures
{
    partial class FlightCreateControl
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            timChuyenBay = new GUI.Components.Buttons.PrimaryButton();
            danhSachChuyenBay = new GUI.Components.Tables.TableCustom();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewComboBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            dateTimeNgayDi = new GUI.Components.Inputs.DateTimePickerCustom();
            dateTimeNgayVe = new GUI.Components.Inputs.DateTimePickerCustom();
            noiCatCanh = new GUI.Components.Inputs.UnderlinedComboBox();
            noiHaCanh = new GUI.Components.Inputs.UnderlinedComboBox();
            soLuongHanhKhach = new GUI.Components.Inputs.UnderlinedComboBox();
            khuHoi_MotChieu = new GUI.Components.Inputs.UnderlinedComboBox();
            passengerSelectorControl1 = new GUI.Components.Inputs.PassengerSelectorControl();
            ((System.ComponentModel.ISupportInitialize)danhSachChuyenBay).BeginInit();
            SuspendLayout();
            // 
            // timChuyenBay
            // 
            timChuyenBay.AutoSize = true;
            timChuyenBay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            timChuyenBay.BackColor = Color.FromArgb(155, 209, 243);
            timChuyenBay.BorderColor = Color.FromArgb(40, 40, 40);
            timChuyenBay.BorderThickness = 2;
            timChuyenBay.CornerRadius = 22;
            timChuyenBay.EnableHoverEffects = true;
            timChuyenBay.FlatAppearance.BorderSize = 0;
            timChuyenBay.FlatStyle = FlatStyle.Flat;
            timChuyenBay.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            timChuyenBay.ForeColor = Color.White;
            timChuyenBay.HoverBackColor = Color.White;
            timChuyenBay.HoverBorderColor = Color.FromArgb(0, 92, 175);
            timChuyenBay.HoverForeColor = Color.FromArgb(0, 92, 175);
            timChuyenBay.Icon = null;
            timChuyenBay.IconSize = new Size(22, 22);
            timChuyenBay.IconSpacing = 10;
            timChuyenBay.Location = new Point(857, 111);
            timChuyenBay.Name = "timChuyenBay";
            timChuyenBay.NormalBackColor = Color.FromArgb(155, 209, 243);
            timChuyenBay.NormalBorderColor = Color.FromArgb(40, 40, 40);
            timChuyenBay.NormalForeColor = Color.White;
            timChuyenBay.Padding = new Padding(24, 10, 24, 10);
            timChuyenBay.PreferredMaxWidth = 0;
            timChuyenBay.PressedBackColor = Color.FromArgb(225, 240, 255);
            timChuyenBay.PressedBorderColor = Color.FromArgb(0, 92, 175);
            timChuyenBay.PressedForeColor = Color.FromArgb(0, 92, 175);
            timChuyenBay.Size = new Size(218, 46);
            timChuyenBay.TabIndex = 0;
            timChuyenBay.Text = "🔍Tìm chuyến bay";
            timChuyenBay.TextAlign = ContentAlignment.MiddleLeft;
            timChuyenBay.UseVisualStyleBackColor = false;
            timChuyenBay.WordWrap = false;
            timChuyenBay.Click += timChuyenBay_Click;
            // 
            // danhSachChuyenBay
            // 
            danhSachChuyenBay.AllowUserToAddRows = false;
            danhSachChuyenBay.AllowUserToDeleteRows = false;
            danhSachChuyenBay.AllowUserToOrderColumns = true;
            danhSachChuyenBay.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            danhSachChuyenBay.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            danhSachChuyenBay.BackgroundColor = Color.White;
            danhSachChuyenBay.BorderColor = Color.FromArgb(40, 40, 40);
            danhSachChuyenBay.BorderStyle = BorderStyle.None;
            danhSachChuyenBay.BorderThickness = 2;
            danhSachChuyenBay.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            danhSachChuyenBay.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.Padding = new Padding(12, 10, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(126, 185, 232);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            danhSachChuyenBay.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            danhSachChuyenBay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            danhSachChuyenBay.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8 });
            danhSachChuyenBay.CornerRadius = 16;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.Padding = new Padding(12, 6, 12, 6);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(155, 209, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            danhSachChuyenBay.DefaultCellStyle = dataGridViewCellStyle3;
            danhSachChuyenBay.EnableHeadersVisualStyles = false;
            danhSachChuyenBay.GridColor = Color.FromArgb(230, 235, 240);
            danhSachChuyenBay.HeaderBackColor = Color.White;
            danhSachChuyenBay.HeaderForeColor = Color.FromArgb(126, 185, 232);
            danhSachChuyenBay.HoverBackColor = Color.FromArgb(232, 245, 255);
            danhSachChuyenBay.Location = new Point(12, 173);
            danhSachChuyenBay.MultiSelect = false;
            danhSachChuyenBay.Name = "danhSachChuyenBay";
            danhSachChuyenBay.ReadOnly = true;
            danhSachChuyenBay.RowAltBackColor = Color.FromArgb(248, 250, 252);
            danhSachChuyenBay.RowBackColor = Color.White;
            danhSachChuyenBay.RowForeColor = Color.FromArgb(33, 37, 41);
            danhSachChuyenBay.RowHeadersVisible = false;
            danhSachChuyenBay.RowTemplate.Height = 40;
            danhSachChuyenBay.SelectionBackColor = Color.FromArgb(155, 209, 243);
            danhSachChuyenBay.SelectionForeColor = Color.White;
            danhSachChuyenBay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            danhSachChuyenBay.Size = new Size(1072, 491);
            danhSachChuyenBay.TabIndex = 1;
            danhSachChuyenBay.CellContentClick += danhSachChuyenBay_CellContentClick;
            // 
            // Column1
            // 
            Column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column1.DataPropertyName = "FlightNumber";
            Column1.HeaderText = "Mã chuyến bay";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            // 
            // Column2
            // 
            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column2.DataPropertyName = "DepartureAirportName";
            Column2.HeaderText = "Nơi cất cánh";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column3.DataPropertyName = "ArrivalAirportName";
            Column3.HeaderText = "Nơi hạ cánh";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            // 
            // Column4
            // 
            Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column4.DataPropertyName = "DepartureTime";
            Column4.HeaderText = "Giờ cất cánh";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            // 
            // Column5
            // 
            Column5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column5.DataPropertyName = "ArrivalTime";
            Column5.HeaderText = "Giờ hạ cánh";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            // 
            // Column6
            // 
            Column6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column6.DataPropertyName = "Status";
            Column6.HeaderText = "Trạng thái";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            // 
            // Column7
            // 
            Column7.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column7.FlatStyle = FlatStyle.Flat;
            Column7.HeaderText = "Chọn hạng vé";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            // 
            // Column8
            // 
            Column8.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column8.HeaderText = "Thao tác";
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            // 
            // dateTimeNgayDi
            // 
            dateTimeNgayDi.BackColor = Color.Transparent;
            dateTimeNgayDi.CustomFormat = null;
            dateTimeNgayDi.LabelText = "Ngày đi";
            dateTimeNgayDi.Location = new Point(270, 18);
            dateTimeNgayDi.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dateTimeNgayDi.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dateTimeNgayDi.Name = "dateTimeNgayDi";
            dateTimeNgayDi.Padding = new Padding(0, 4, 0, 8);
            dateTimeNgayDi.PlaceholderText = "";
            dateTimeNgayDi.Size = new Size(151, 57);
            dateTimeNgayDi.TabIndex = 2;
            dateTimeNgayDi.Value = new DateTime(2025, 10, 30, 10, 5, 19, 31);
            dateTimeNgayDi.Load += dateTimeNgayDi_Load;
            // 
            // dateTimeNgayVe
            // 
            dateTimeNgayVe.BackColor = Color.Transparent;
            dateTimeNgayVe.CustomFormat = null;
            dateTimeNgayVe.LabelText = "Ngày về";
            dateTimeNgayVe.Location = new Point(453, 17);
            dateTimeNgayVe.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dateTimeNgayVe.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dateTimeNgayVe.Name = "dateTimeNgayVe";
            dateTimeNgayVe.Padding = new Padding(0, 4, 0, 8);
            dateTimeNgayVe.PlaceholderText = "";
            dateTimeNgayVe.Size = new Size(151, 57);
            dateTimeNgayVe.TabIndex = 3;
            dateTimeNgayVe.Value = new DateTime(2025, 10, 30, 10, 5, 19, 31);
            dateTimeNgayVe.Load += dateTimeNgayVe_Load;
            // 
            // noiCatCanh
            // 
            noiCatCanh.BackColor = Color.Transparent;
            noiCatCanh.DataSource = null;
            noiCatCanh.DisplayMember = "";
            noiCatCanh.LabelText = "Từ";
            noiCatCanh.Location = new Point(636, 17);
            noiCatCanh.MinimumSize = new Size(140, 56);
            noiCatCanh.Name = "noiCatCanh";
            noiCatCanh.SelectedIndex = -1;
            noiCatCanh.SelectedItem = null;
            noiCatCanh.SelectedText = "";
            noiCatCanh.Size = new Size(227, 56);
            noiCatCanh.TabIndex = 4;
            noiCatCanh.ValueMember = "";
            noiCatCanh.Load += noiCatCanh_Load;
            // 
            // noiHaCanh
            // 
            noiHaCanh.BackColor = Color.Transparent;
            noiHaCanh.DataSource = null;
            noiHaCanh.DisplayMember = "";
            noiHaCanh.LabelText = "Đến";
            noiHaCanh.Location = new Point(898, 18);
            noiHaCanh.MinimumSize = new Size(140, 56);
            noiHaCanh.Name = "noiHaCanh";
            noiHaCanh.SelectedIndex = -1;
            noiHaCanh.SelectedItem = null;
            noiHaCanh.SelectedText = "";
            noiHaCanh.Size = new Size(238, 56);
            noiHaCanh.TabIndex = 5;
            noiHaCanh.ValueMember = "";
            noiHaCanh.Load += noiHaCanh_Load;
            // 
            // soLuongHanhKhach
            // 
            soLuongHanhKhach.BackColor = Color.Transparent;
            soLuongHanhKhach.DataSource = null;
            soLuongHanhKhach.DisplayMember = "";
            soLuongHanhKhach.LabelText = "Hành khách";
            soLuongHanhKhach.Location = new Point(41, 101);
            soLuongHanhKhach.MinimumSize = new Size(140, 56);
            soLuongHanhKhach.Name = "soLuongHanhKhach";
            soLuongHanhKhach.SelectedIndex = -1;
            soLuongHanhKhach.SelectedItem = null;
            soLuongHanhKhach.SelectedText = "";
            soLuongHanhKhach.Size = new Size(197, 56);
            soLuongHanhKhach.TabIndex = 6;
            soLuongHanhKhach.ValueMember = "";
            // 
            // khuHoi_MotChieu
            // 
            khuHoi_MotChieu.BackColor = Color.Transparent;
            khuHoi_MotChieu.DataSource = null;
            khuHoi_MotChieu.DisplayMember = "";
            khuHoi_MotChieu.LabelText = "Hành trình bay";
            khuHoi_MotChieu.Location = new Point(41, 18);
            khuHoi_MotChieu.MinimumSize = new Size(140, 56);
            khuHoi_MotChieu.Name = "khuHoi_MotChieu";
            khuHoi_MotChieu.SelectedIndex = -1;
            khuHoi_MotChieu.SelectedItem = null;
            khuHoi_MotChieu.SelectedText = "";
            khuHoi_MotChieu.Size = new Size(197, 56);
            khuHoi_MotChieu.TabIndex = 7;
            khuHoi_MotChieu.ValueMember = "";
            // 
            // passengerSelectorControl1
            // 
            passengerSelectorControl1.Adults = 1;
            passengerSelectorControl1.Children = 0;
            passengerSelectorControl1.Infants = 0;
            passengerSelectorControl1.Location = new Point(27, 230);
            passengerSelectorControl1.Name = "passengerSelectorControl1";
            passengerSelectorControl1.Size = new Size(300, 155);
            passengerSelectorControl1.TabIndex = 8;
            // 
            // FlightCreateControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(232, 240, 252);
            Controls.Add(passengerSelectorControl1);
            Controls.Add(khuHoi_MotChieu);
            Controls.Add(soLuongHanhKhach);
            Controls.Add(noiHaCanh);
            Controls.Add(noiCatCanh);
            Controls.Add(dateTimeNgayVe);
            Controls.Add(dateTimeNgayDi);
            Controls.Add(danhSachChuyenBay);
            Controls.Add(timChuyenBay);
            Name = "FlightCreateControl";
            Size = new Size(1139, 679);
            Load += FlightCreateControl_Load;
            ((System.ComponentModel.ISupportInitialize)danhSachChuyenBay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Components.Buttons.PrimaryButton timChuyenBay;
        private Components.Tables.TableCustom danhSachChuyenBay;
        private Components.Inputs.DateTimePickerCustom dateTimeNgayDi;
        private Components.Inputs.DateTimePickerCustom dateTimeNgayVe;
        private Components.Inputs.UnderlinedComboBox noiCatCanh;
        private Components.Inputs.UnderlinedComboBox noiHaCanh;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewComboBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private Components.Inputs.UnderlinedComboBox soLuongHanhKhach;
        private Components.Inputs.UnderlinedComboBox khuHoi_MotChieu;
        private Components.Inputs.PassengerSelectorControl passengerSelectorControl1;
    }
}
