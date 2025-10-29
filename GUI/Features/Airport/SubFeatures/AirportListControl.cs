using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Inputs;
using GUI.Components.Buttons;
using GUI.Components.Tables;
using BUS.Airport;
using DTO.Airport;

namespace GUI.Features.Airport.SubFeatures
{
    public class AirportListControl : UserControl
    {
        private readonly AirportBUS _bus = new AirportBUS();
        private TableCustom table;
        private UnderlinedTextField txtCode, txtName, txtCity;
        private UnderlinedComboBox cbCountry;
        private PrimaryButton btnSearch;

        public event Action<AirportDTO>? ViewRequested;
        public event Action? DataChanged;

        public AirportListControl() { InitializeComponent(); LoadData(); }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);
            txtCode = new UnderlinedTextField("Mã IATA", "");
            txtName = new UnderlinedTextField("Tên sân bay", "");
            txtCity = new UnderlinedTextField("Thành phố", "");
            cbCountry = new UnderlinedComboBox("Quốc gia", new object[] { "Tất cả", "Việt Nam", "Nhật Bản", "Hàn Quốc", "Singapore", "Thái Lan", "Hoa Kỳ", "Anh", "Pháp", "Úc", "Canada" });

            btnSearch = new PrimaryButton("Tìm");
            btnSearch.Click += (_, __) => LoadData();

            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                RowHeadersVisible = false
            };
            table.Columns.Add("airportId", "ID");
            table.Columns.Add("airportCode", "IATA");
            table.Columns.Add("airportName", "Tên sân bay");
            table.Columns.Add("city", "Thành phố");
            table.Columns.Add("country", "Quốc gia");

            var panel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true };
            panel.Controls.AddRange(new Control[] { txtCode, txtName, txtCity, cbCountry, btnSearch });

            Controls.Add(table);
            Controls.Add(panel);
        }

        private void LoadData()
        {
            try
            {
                string keyword = $"{txtCode.Text} {txtName.Text} {txtCity.Text}".Trim();
                List<AirportDTO> list = _bus.SearchAirports(keyword);
                if (cbCountry.SelectedItem != null && cbCountry.SelectedItem.ToString() != "Tất cả")
                    list = list.FindAll(a => a.Country == cbCountry.SelectedItem.ToString());

                table.Rows.Clear();
                foreach (var a in list)
                    table.Rows.Add(a.AirportId, a.AirportCode, a.AirportName, a.City, a.Country);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }
    }
}
