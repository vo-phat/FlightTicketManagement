using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI.Features.Flight.SubFeatures;


namespace GUI.Features.Flight
{
    public partial class FlightControl : UserControl
    {
        private FlightCreateControl flightCreateControl = new FlightCreateControl();
        private FlightDetailControl flightDetailControl = new FlightDetailControl();
        private FlightListControl flightListControl = new FlightListControl();
        public FlightControl()
        {
            InitializeComponent();


            panel1.Dock = DockStyle.Top;
            panel1.Height = 60;
            panel1.BringToFront();

            panelContent.Controls.Add(panelFlightCreate);
            panelContent.Controls.Add(panelFlightDetail);
            panelContent.Controls.Add(panelFlightList);

            panelFlightCreate.Controls.Add(flightCreateControl);
            panelFlightDetail.Controls.Add(flightDetailControl);
            panelFlightList.Controls.Add(flightListControl);
            panelFlightCreate.Dock = DockStyle.Fill;
            panelFlightDetail.Dock = DockStyle.Fill;
            panelFlightList.Dock = DockStyle.Fill;

        }
        private void FlightControl_Load(object sender, EventArgs e)
        {
            if (flightDetailControl != null) flightDetailControl.Dock = DockStyle.Fill;
            if (flightListControl != null) flightListControl.Dock = DockStyle.Fill;

            switchTab(0);
        }

        private void buttonDanhSachChuyenBay_Click(object sender, EventArgs e)
        {
            switchTab(0);
        }
        private void buttonTaoMoiChuyenBay_Click(object sender, EventArgs e)
        {
            switchTab(1);
        }   
        void switchTab(int i)
        {
            if (flightListControl != null) flightListControl.Visible = false;
            if (flightDetailControl != null) flightDetailControl.Visible = false;
            if (flightCreateControl != null) flightCreateControl.Visible = false;
            switch (i)
            {
                case 0: // Tab Tạo mới (Create)
                    if (flightCreateControl != null)
                    {
                        flightCreateControl.Visible = true;
                        flightCreateControl.BringToFront();
                    }
                    break;
                case 1: // Tab Danh sách (List)
                    if (flightListControl != null)
                    {
                        flightListControl.Visible = true;
                        flightListControl.BringToFront();
                    }
                    break;

                case 2: // Tab Chi tiết (Detail)
                if (flightDetailControl != null)
                {
                    flightDetailControl.Visible = true;
                    flightDetailControl.BringToFront();
                }
                    break;

            
            }

        }

       
    }
}
