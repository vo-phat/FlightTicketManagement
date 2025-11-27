using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;
using GUI.Features.Airline.SubFeatures;
using DTO.Airline;

namespace GUI.Features.Airline
{
    public class AirlineControl : UserControl
    {
        private Button btnList, btnCreate;
        private AirlineListControl list;
        private AirlineCreateControl create;
        private AirlineDetailControl detail;

        private FlowLayoutPanel topPanel;

        public AirlineControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            // 1. Khởi tạo Controls con
            list = new AirlineListControl { Dock = DockStyle.Fill };
            create = new AirlineCreateControl { Dock = DockStyle.Fill };
            detail = new AirlineDetailControl { Dock = DockStyle.Fill };

            // 2. Khởi tạo nút (ban đầu)
            btnList = new PrimaryButton("Danh sách hãng");
            btnCreate = new SecondaryButton("Tạo hãng mới");
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

            // 4. Events (Giống hệt luồng Aircraft/Airport)
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

        private void OnListViewRequested(AirlineDTO dto)
        {
            SwitchTabDetail(dto);
        }

        private void OnListEditRequested(AirlineDTO dto)
        {
            // Nếu là DTO rỗng (Thêm mới)
            if (dto.AirlineId > 0)
                create.LoadForEdit(dto);
            else
                create.LoadForEdit(new AirlineDTO()); // Reset form cho Add

            SwitchTab(1);
        }

        private void SwitchTabDetail(AirlineDTO dto)
        {
            list.Visible = false;
            create.Visible = false;
            detail.Visible = true;
            detail.LoadAirline(dto);

            detail.BringToFront();
            topPanel.BringToFront();
        }

        private void SwitchTab(int idx)
        {
            // Reset trạng thái Create/Edit khi chuyển đi
            if (idx != 1)
                create.LoadForEdit(new AirlineDTO());

            // Hi/ẩn controls
            list.Visible = (idx == 0);
            create.Visible = (idx == 1);
            detail.Visible = (idx == 2);

            // Cập nhật trạng thái nút (Giữ nguyên logic Aircraft/Airport)
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