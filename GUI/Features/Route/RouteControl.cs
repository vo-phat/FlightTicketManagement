using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.Route.SubFeatures;
using DTO.Route;

namespace GUI.Features.Route
{
    public class RouteControl : UserControl
    {
        private Button btnList, btnCreate;
        private RouteListControl list;
        private RouteCreateControl create;
        private RouteDetailControl detail;

        private FlowLayoutPanel topPanel;

        public RouteControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            // 1. Khởi tạo Controls con
            list = new RouteListControl { Dock = DockStyle.Fill };
            create = new RouteCreateControl { Dock = DockStyle.Fill };
            detail = new RouteDetailControl { Dock = DockStyle.Fill };

            // 2. Khởi tạo nút (ban đầu)
            btnList = new PrimaryButton("Danh sách tuyến bay");
            btnCreate = new SecondaryButton("Tạo tuyến bay mới");
            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);

            // 3. Thanh Top
            topPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            topPanel.Controls.AddRange(new Control[] { btnList, btnCreate });

            // 4. Events 
            list.ViewRequested += OnListViewRequested;
            list.RequestEdit += OnListEditRequested;
            detail.CloseRequested += (_, __) => SwitchTab(0);
            create.DataSaved += (_, __) => { list.RefreshList(); SwitchTab(0); };
            create.DataUpdated += (_, __) => { list.RefreshList(); SwitchTab(0); };

            // 5. Thêm Controls vào cha
            Controls.Add(list);
            Controls.Add(create);
            Controls.Add(detail);
            Controls.Add(topPanel);

            SwitchTab(0);
        }

        private void OnListViewRequested(RouteDTO dto)
        {
            SwitchTabDetail(dto);
        }

        private void OnListEditRequested(RouteDTO dto)
        {
            // Nếu là DTO rỗng (Thêm mới)
            if (dto.RouteId > 0)
                create.LoadForEdit(dto);
            else
                create.LoadForEdit(new RouteDTO());

            SwitchTab(1);
        }

        private void SwitchTabDetail(RouteDTO dto)
        {
            list.Visible = false;
            create.Visible = false;
            detail.Visible = true;
            detail.LoadRoute(dto);

            detail.BringToFront();
            topPanel.BringToFront();
        }

        private void SwitchTab(int idx)
        {
            // Reset trạng thái Create/Edit khi chuyển đi
            if (idx != 1)
                create.LoadForEdit(new RouteDTO());

            // Hi/ẩn controls
            list.Visible = (idx == 0);
            create.Visible = (idx == 1);
            detail.Visible = (idx == 2);

            // Cập nhật trạng thái nút
            if (idx == 0) // Danh sách
            {
                list.RefreshList();
                btnList.Enabled = false;
                btnCreate.Enabled = true;
                list.BringToFront();
            }
            else if (idx == 1) // Tạo/Sửa
            {
                btnList.Enabled = true;
                btnCreate.Enabled = false;
                create.BringToFront();
            }
            else // Chi tiết (2)
            {
                btnList.Enabled = true;
                btnCreate.Enabled = true;
                detail.BringToFront();
            }

            topPanel.BringToFront();
        }
    }
}