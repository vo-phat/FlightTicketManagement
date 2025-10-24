using System;
using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Features.Baggage.SubFeatures;

namespace FlightTicketManagement.GUI.Features.Baggage {
    public class BaggageControl : UserControl {
        private Button btnList;
        private Button btnCheckin;
        private Button btnDetail;

        private BaggageListControl listControl;
        private BaggageCheckinControl checkinControl;
        private BaggageDetailControl detailControl;

        public BaggageControl() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            btnList = new PrimaryButton("Danh sách hành lý");
            btnCheckin = new SecondaryButton("Gán tag / Check-in");
            btnDetail = new SecondaryButton("Theo dõi / Chi tiết");

            btnList.Click += (s, e) => SwitchTab(0);
            btnCheckin.Click += (s, e) => SwitchTab(1);
            btnDetail.Click += (s, e) => SwitchTab(2);

            var buttonPanel = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            buttonPanel.Controls.AddRange(new Control[] { btnList, btnCheckin, btnDetail });

            listControl = new BaggageListControl { Dock = DockStyle.Fill };
            checkinControl = new BaggageCheckinControl { Dock = DockStyle.Fill };
            detailControl = new BaggageDetailControl { Dock = DockStyle.Fill };

            Controls.Add(listControl);
            Controls.Add(checkinControl);
            Controls.Add(detailControl);
            Controls.Add(buttonPanel);

            // Điều hướng giữa các tab khi user thao tác
            listControl.OnViewRequested += data => { detailControl.LoadBaggageInfo(data); SwitchTab(2); };
            checkinControl.OnCreated += data => {
                var row = new BaggageListControl.BaggageRow {
                    BaggageId = data.BaggageId,
                    BaggageTag = data.BaggageTag,
                    Type = data.Type,
                    WeightKg = data.WeightKg,
                    AllowedWeightKg = data.AllowedWeightKg,
                    Fee = data.Fee,
                    Status = data.Status,
                    FlightId = data.FlightId,
                    TicketId = data.TicketId
                };
                detailControl.LoadBaggageInfo(row);
                SwitchTab(2);
            };

            SwitchTab(0);
        }

        public void SwitchTab(int idx) {
            listControl.Visible = (idx == 0);
            checkinControl.Visible = (idx == 1);
            detailControl.Visible = (idx == 2);

            var panel = btnList.Parent as FlowLayoutPanel;
            if (panel != null) {
                panel.Controls.Clear();
                btnList = (idx == 0) ? new PrimaryButton("Danh sách hành lý") : new SecondaryButton("Danh sách hành lý");
                btnCheckin = (idx == 1) ? new PrimaryButton("Gán tag / Check-in") : new SecondaryButton("Gán tag / Check-in");
                btnDetail = (idx == 2) ? new PrimaryButton("Theo dõi / Chi tiết") : new SecondaryButton("Theo dõi / Chi tiết");

                btnList.Click += (s, e) => SwitchTab(0);
                btnCheckin.Click += (s, e) => SwitchTab(1);
                btnDetail.Click += (s, e) => SwitchTab(2);

                panel.Controls.AddRange(new Control[] { btnList, btnCheckin, btnDetail });
            }
        }
    }
}
