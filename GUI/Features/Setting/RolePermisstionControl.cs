using System;
using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.Setting.SubFeatures;

namespace GUI.Features.Setting {
    public class RolePermissionControl : UserControl {
        private Button btnByRoleTab, btnByAccountTab;
        private Panel contentPanel;

        private AssignByRoleControl byRole;
        private ManageAccountRolesControl byAccount;

        public RolePermissionControl() { InitializeComponent(); }

        private void InitializeComponent() {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            btnByRoleTab = new PrimaryButton("Phân quyền theo Vai trò");
            btnByAccountTab = new SecondaryButton("Quản lý quyền cho Tài khoản");

            btnByRoleTab.Click += (_, __) => SwitchTab(0);
            btnByAccountTab.Click += (_, __) => SwitchTab(1);

            var top = new FlowLayoutPanel {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            top.Controls.AddRange(new Control[] { btnByRoleTab, btnByAccountTab });

            contentPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.WhiteSmoke };

            byRole = new AssignByRoleControl { Dock = DockStyle.Fill };
            byAccount = new ManageAccountRolesControl { Dock = DockStyle.Fill };

            contentPanel.Controls.Add(byRole);
            contentPanel.Controls.Add(byAccount);

            Controls.Add(contentPanel);
            Controls.Add(top);

            SwitchTab(0);
        }

        private void SwitchTab(int idx) {
            byRole.Visible = (idx == 0);
            byAccount.Visible = (idx == 1);

            var top = btnByRoleTab.Parent as FlowLayoutPanel;
            top!.Controls.Clear();

            if (idx == 0) {
                btnByRoleTab = new PrimaryButton("Phân quyền theo Vai trò");
                btnByAccountTab = new SecondaryButton("Quản lý quyền cho Tài khoản");
            } else {
                btnByRoleTab = new SecondaryButton("Phân quyền theo Vai trò");
                btnByAccountTab = new PrimaryButton("Quản lý quyền cho Tài khoản");
            }

            btnByRoleTab.Click += (_, __) => SwitchTab(0);
            btnByAccountTab.Click += (_, __) => SwitchTab(1);

            top.Controls.AddRange(new Control[] { btnByRoleTab, btnByAccountTab });

            if (idx == 0) byRole.BringToFront(); else byAccount.BringToFront();
        }
    }
}
