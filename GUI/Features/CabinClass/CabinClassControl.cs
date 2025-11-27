using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.CabinClass.SubFeatures;
using DTO.CabinClass;

namespace GUI.Features.CabinClass
{
    public class CabinClassControl : UserControl
    {
        private Button btnList, btnCreate;
        private CabinClassListControl list;
        private CabinClassCreateControl create;
        private CabinClassDetailControl detail;

        private FlowLayoutPanel topPanel;

        public CabinClassControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            // 1. Khởi tạo Controls con
            list = new CabinClassListControl { Dock = DockStyle.Fill };
            create = new CabinClassCreateControl { Dock = DockStyle.Fill };
            detail = new CabinClassDetailControl { Dock = DockStyle.Fill };

            // 2. Khởi tạo nút (ban đầu)
            btnList = new PrimaryButton("Danh sách hạng ghế");
            btnCreate = new SecondaryButton("Tạo hạng ghế mới");
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

        private void OnListViewRequested(CabinClassDTO dto)
        {
            SwitchTabDetail(dto);
        }

        private void OnListEditRequested(CabinClassDTO dto)
        {
            if (dto.ClassId > 0)
                create.LoadForEdit(dto);
            else
                create.LoadForEdit(new CabinClassDTO());

            SwitchTab(1);
        }

        private void SwitchTabDetail(CabinClassDTO dto)
        {
            list.Visible = false;
            create.Visible = false;
            detail.Visible = true;
            detail.LoadCabinClass(dto);

            detail.BringToFront();
            topPanel.BringToFront();
        }

        private void SwitchTab(int idx)
        {
            // Reset trạng thái Create/Edit khi chuyển đi
            if (idx != 1)
                create.LoadForEdit(new CabinClassDTO());

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