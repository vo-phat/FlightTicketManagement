using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Buttons;
using FlightTicketManagement.GUI.Features.Seat.SubFeatures;

namespace FlightTicketManagement.GUI.Features.Seat {
    public class SeatControl : UserControl {
        private Button _btnTemplate, _btnInventory, _btnSelection;
        private SeatTemplateControl _template;
        private FlightSeatInventoryControl _inventory;
        private SeatSelectionControl _selection;

        public SeatControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            _btnTemplate = new PrimaryButton("Template (Máy bay)");
            _btnInventory = new SecondaryButton("Inventory (Chuyến bay)");
            _btnSelection = new SecondaryButton("Chọn ghế (Đặt vé)");

            _btnTemplate.Click += (_, __) => SwitchTab(0);
            _btnInventory.Click += (_, __) => SwitchTab(1);
            _btnSelection.Click += (_, __) => SwitchTab(2);

            var top = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            top.Controls.AddRange(new Control[] { _btnTemplate, _btnInventory, _btnSelection });

            _template = new SeatTemplateControl { Dock = DockStyle.Fill };
            _inventory = new FlightSeatInventoryControl { Dock = DockStyle.Fill };
            _selection = new SeatSelectionControl { Dock = DockStyle.Fill };

            Controls.Add(_template);
            Controls.Add(_inventory);
            Controls.Add(_selection);
            Controls.Add(top);

            SwitchTab(0);
        }

        private void SwitchTab(int idx) {
            _template.Visible = (idx == 0);
            _inventory.Visible = (idx == 1);
            _selection.Visible = (idx == 2);

            var top = _btnTemplate.Parent as FlowLayoutPanel; top!.Controls.Clear();
            if (idx == 0) {
                _btnTemplate = new PrimaryButton("Template (Máy bay)");
                _btnInventory = new SecondaryButton("Inventory (Chuyến bay)");
                _btnSelection = new SecondaryButton("Chọn ghế (Đặt vé)");
            } else if (idx == 1) {
                _btnTemplate = new SecondaryButton("Template (Máy bay)");
                _btnInventory = new PrimaryButton("Inventory (Chuyến bay)");
                _btnSelection = new SecondaryButton("Chọn ghế (Đặt vé)");
            } else {
                _btnTemplate = new SecondaryButton("Template (Máy bay)");
                _btnInventory = new SecondaryButton("Inventory (Chuyến bay)");
                _btnSelection = new PrimaryButton("Chọn ghế (Đặt vé)");
            }

            _btnTemplate.Click += (_, __) => SwitchTab(0);
            _btnInventory.Click += (_, __) => SwitchTab(1);
            _btnSelection.Click += (_, __) => SwitchTab(2);
            top.Controls.AddRange(new Control[] { _btnTemplate, _btnInventory, _btnSelection });
        }
    }
}
