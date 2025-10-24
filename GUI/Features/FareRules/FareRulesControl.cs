using System.Drawing;
using System.Windows.Forms;
using FlightTicketManagement.GUI.Components.Buttons;

namespace FlightTicketManagement.GUI.Features.FareRules {
    public class FareRulesControl : UserControl {
        private Button btnList, btnCreate;
        private SubFeatures.FareRuleListControl list;
        private SubFeatures.FareRuleCreateControl create;
        private SubFeatures.FareRuleDetailControl detail;

        public FareRulesControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill; BackColor = Color.WhiteSmoke;

            btnList = new PrimaryButton("Danh sách Quy tắc");
            btnCreate = new SecondaryButton("Tạo Quy tắc");
            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);

            var top = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            top.Controls.Add(btnList); top.Controls.Add(btnCreate);

            list = new SubFeatures.FareRuleListControl { Dock = DockStyle.Fill };
            create = new SubFeatures.FareRuleCreateControl { Dock = DockStyle.Fill };
            detail = new SubFeatures.FareRuleDetailControl { Dock = DockStyle.Fill };

            Controls.Add(list); Controls.Add(create); Controls.Add(detail); Controls.Add(top);
            SwitchTab(0);
        }

        private void SwitchTab(int idx) {
            list.Visible = (idx == 0);
            create.Visible = (idx == 1);
            detail.Visible = false;

            var top = btnList.Parent as FlowLayoutPanel; top!.Controls.Clear();
            if (idx == 0) { btnList = new PrimaryButton("Danh sách Quy tắc"); btnCreate = new SecondaryButton("Tạo Quy tắc"); } else { btnList = new SecondaryButton("Danh sách Quy tắc"); btnCreate = new PrimaryButton("Tạo Quy tắc"); }
            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);
            top.Controls.Add(btnList); top.Controls.Add(btnCreate);
        }
    }
}
