using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.Aircraft.SubFeatures;
using DTO.Aircraft; // Thêm DTO

namespace GUI.Features.Aircraft
{
    public class AircraftControl : UserControl
    {
        // Sử dụng Primary/SecondaryButton để giữ logic visual
        private Button btnList, btnCreate;
        private AircraftListControl list;
        private AircraftCreateControl create;
        private AircraftDetailControl detail;

        private FlowLayoutPanel topPanel; // Lưu panel chứa nút

        public AircraftControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            // 1. Khởi tạo Controls con
            list = new AircraftListControl { Dock = DockStyle.Fill };
            create = new AircraftCreateControl { Dock = DockStyle.Fill };
            detail = new AircraftDetailControl { Dock = DockStyle.Fill };

            // 2. Khởi tạo nút (ban đầu)
            btnList = new PrimaryButton("Danh sách máy bay");
            btnCreate = new SecondaryButton("Tạo máy bay mới");
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
            // List -> Detail (ViewRequested)
            list.ViewRequested += OnListViewRequested;
            // List -> Create (RequestEdit/Add)
            list.RequestEdit += OnListEditRequested;
            // Detail -> List (CloseRequested)
            detail.CloseRequested += (_, __) => SwitchTab(0);
            // Create -> List (DataSaved/Updated)
            create.DataSaved += (_, __) => { list.RefreshList(); SwitchTab(0); };
            create.DataUpdated += (_, __) => { list.RefreshList(); SwitchTab(0); };

            // 5. Thêm Controls vào cha
            Controls.Add(list);
            Controls.Add(create);
            Controls.Add(detail);
            Controls.Add(topPanel);

            SwitchTab(0);
        }

        private void OnListViewRequested(AircraftDTO dto)
        {
            SwitchTabDetail(dto);
        }

        private void OnListEditRequested(AircraftDTO dto)
        {
            // Nếu là DTO rỗng (Thêm mới) thì không cần LoadForEdit
            if (dto.AircraftId > 0)
                create.LoadForEdit(dto);
            else
                create.LoadForEdit(new AircraftDTO()); // Reset form cho Add

            SwitchTab(1);
        }

        private void SwitchTabDetail(AircraftDTO dto)
        {
            list.Visible = false;
            create.Visible = false;
            detail.Visible = true;
            detail.LoadAircraft(dto);

            detail.BringToFront();
            topPanel.BringToFront(); // Giữ thanh top luôn nằm trên cùng
        }

        private void SwitchTab(int idx)
        {
            // Reset trạng thái Create/Edit khi chuyển đi
            if (idx != 1)
                create.LoadForEdit(new AircraftDTO());

            // Hi/ẩn controls
            list.Visible = (idx == 0);
            create.Visible = (idx == 1);
            detail.Visible = (idx == 2);

            // Cập nhật trạng thái nút (dựa trên class nút tự thay đổi visual)
            if (idx == 0) // Danh sách
            {
                list.RefreshList(); // Đảm bảo làm mới danh sách
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