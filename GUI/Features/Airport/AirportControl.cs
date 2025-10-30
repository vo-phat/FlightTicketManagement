using System.Drawing;
using System.Windows.Forms;
using GUI.Components.Buttons;

namespace GUI.Features.Airport
{
    public class AirportControl : UserControl
    {
        private Button btnList, btnCreate;
        private SubFeatures.AirportListControl list;
        private SubFeatures.AirportCreateControl create;
        private SubFeatures.AirportDetailControl detail;

        public AirportControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.WhiteSmoke;

            btnList = new PrimaryButton("Danh sách sân bay");
            btnCreate = new SecondaryButton("Tạo sân bay");
            btnList.Click += (_, __) => SwitchTab(0);
            btnCreate.Click += (_, __) => SwitchTab(1);

            var top = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 0, 0),
                AutoSize = true
            };
            top.Controls.AddRange(new Control[] { btnList, btnCreate });

            list = new SubFeatures.AirportListControl { Dock = DockStyle.Fill };
            create = new SubFeatures.AirportCreateControl { Dock = DockStyle.Fill };
            detail = new SubFeatures.AirportDetailControl { Dock = DockStyle.Fill };

            // 👉 Thêm dòng này
            detail.CloseRequested += (_, __) => SwitchTab(0);

            // Events
            list.ViewRequested += OnListViewRequested;
            list.RequestEdit += OnListEditRequested;
            list.DataChanged += () => list.RefreshList();
            create.AirportSaved += (_, __) => { list.RefreshList(); SwitchTab(0); };
            create.AirportSavedUpdated += (_, __) => { list.RefreshList(); SwitchTab(0); };

            Controls.Add(list);
            Controls.Add(create);
            Controls.Add(detail);
            Controls.Add(top);

            SwitchTab(0);
        }

        private void OnListViewRequested(DTO.Airport.AirportDTO dto)
        {
            // show detail tab and load data
            SwitchTabDetail(dto);
        }

        private void OnListEditRequested(DTO.Airport.AirportDTO dto)
        {
            // Switch to create tab and load for edit
            create.LoadForEdit(dto);
            SwitchTab(1);
        }

        private void SwitchTabDetail(DTO.Airport.AirportDTO dto)
        {
            list.Visible = false;
            create.Visible = false;
            detail.Visible = true;

            // Load DTO into detail control (we added a method that accepts DTO)
            detail.LoadAirport(dto);
        }

        // Thay body của AirportControl.SwitchTab bằng đoạn sau
        private void SwitchTab(int idx)
        {
            // hi/ẩn controls
            list.Visible = (idx == 0);
            create.Visible = (idx == 1);
            detail.Visible = (idx == 2);

            // đảm bảo top bar nằm trên cùng
            var top = btnList.Parent as FlowLayoutPanel;
            if (top != null) top.BringToFront();

            // Chỉ thay đổi visual của nút, không tạo mới
            if (idx == 0)
            {
                btnList.Enabled = false;
                btnCreate.Enabled = true;
            }
            else if (idx == 1)
            {
                btnList.Enabled = true;
                btnCreate.Enabled = false;
            }
            else
            {
                btnList.Enabled = true;
                btnCreate.Enabled = true;
            }
        }

    }
}
