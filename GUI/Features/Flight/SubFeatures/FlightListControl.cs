using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BUS.Flight;
using DTO.Flight;
using GUI.Components.Buttons;
using GUI.Components.Inputs;
using GUI.Components.Tables;

namespace GUI.Features.Flight.SubFeatures
{
    public class FlightListControl : UserControl
    {
        #region Fields

        private TableCustom table;
        private TableLayoutPanel mainPanel;
        private TableLayoutPanel filterWrapPanel;
        private FlowLayoutPanel filterPanel;
        private FlowLayoutPanel btnPanel;
        private Label lblTitle;
        private DateTimePickerCustom dtpDepartureDate;
        private DateTimePickerCustom dtpArrivalDate;
        private UnderlinedTextField txtDeparturePlace;
        private UnderlinedTextField txtArrivalPlace;
        private PrimaryButton btnSearchFlight;

        private const string ACTION_COL_NAME = "Action";
        private const string TXT_VIEW = "Xem";
        private const string TXT_EDIT = "Sửa";
        private const string TXT_DELETE = "Xóa";
        private const string SEP = " / ";

        #endregion

        #region Events

        /// <summary>
        /// Event khi user click "Xem" - trả về flight_id
        /// </summary>
        public event Action<int> OnViewDetail;

        /// <summary>
        /// Event khi user click "Sửa" - trả về flight_id
        /// </summary>
        public event Action<int> OnEditFlight;

        #endregion

        #region Constructor

        public FlightListControl()
        {
            InitializeComponent();
            LoadFlights(); // Load dữ liệu ban đầu
        }

        #endregion

        #region UI Initialization

        private void InitializeComponent()
        {
            SuspendLayout();

            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(232, 240, 252);

            // ===== Title =====
            lblTitle = new Label
            {
                Text = "✈️ Danh sách chuyến bay",
                AutoSize = true,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Black,
                Padding = new Padding(24, 20, 24, 0),
                Dock = DockStyle.Top
            };

            // ===== Filter Panel =====
            BuildFilterPanel();

            // ===== Table =====
            BuildTable();

            // ===== Main Layout =====
            mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 3
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // Title
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // Filter
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Table

            mainPanel.Controls.Add(lblTitle, 0, 0);
            mainPanel.Controls.Add(filterWrapPanel, 0, 1);
            mainPanel.Controls.Add(table, 0, 2);

            Controls.Clear();
            Controls.Add(mainPanel);

            ResumeLayout(false);
        }

        private void BuildFilterPanel()
        {
            filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            dtpDepartureDate = new DateTimePickerCustom("Ngày đi", "")
            {
                Width = 280,
                Margin = new Padding(0, 0, 16, 0)
            };

            dtpArrivalDate = new DateTimePickerCustom("Ngày về", "")
            {
                Width = 280,
                Margin = new Padding(0, 0, 16, 0)
            };

            txtDeparturePlace = new UnderlinedTextField("Nơi cất cánh", "")
            {
                Width = 280,
                Margin = new Padding(0, 0, 16, 0)
            };

            txtArrivalPlace = new UnderlinedTextField("Nơi hạ cánh", "")
            {
                Width = 280,
                Margin = new Padding(0, 0, 16, 0)
            };

            filterPanel.Controls.AddRange(new Control[] {
                dtpDepartureDate,
                dtpArrivalDate,
                txtDeparturePlace,
                txtArrivalPlace
            });

            // Button panel
            btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };

            btnSearchFlight = new PrimaryButton("🔍 Tìm chuyến bay")
            {
                Width = 180,
                Height = 40,
                Margin = new Padding(0)
            };
            btnSearchFlight.Click += BtnSearchFlight_Click;

            btnPanel.Controls.Add(btnSearchFlight);

            // Wrapper
            filterWrapPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(24, 16, 24, 0),
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 1
            };
            filterWrapPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            filterWrapPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filterWrapPanel.Controls.Add(filterPanel, 0, 0);
            filterWrapPanel.Controls.Add(btnPanel, 1, 0);
        }

        private void BuildTable()
        {
            table = new TableCustom
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(24, 12, 24, 24),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Columns
            table.Columns.Add("flightNumber", "Mã chuyến bay");
            table.Columns.Add("fromAirport", "Nơi cất cánh");
            table.Columns.Add("toAirport", "Nơi hạ cánh");
            table.Columns.Add("departureTime", "Giờ cất cánh");
            table.Columns.Add("arrivalTime", "Giờ hạ cánh");
            table.Columns.Add("status", "Trạng thái");

            var colAction = new DataGridViewTextBoxColumn
            {
                Name = ACTION_COL_NAME,
                HeaderText = "Thao tác",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            table.Columns.Add(colAction);

            // Hidden ID column
            var colIdHidden = new DataGridViewTextBoxColumn
            {
                Name = "flightIdHidden",
                HeaderText = "",
                Visible = false
            };
            table.Columns.Add(colIdHidden);

            // Events
            table.CellPainting += Table_CellPainting;
            table.CellMouseMove += Table_CellMouseMove;
            table.CellMouseClick += Table_CellMouseClick;
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Load tất cả chuyến bay từ BUS với thông tin đầy đủ
        /// </summary>
        public void LoadFlights()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                table.Rows.Clear();

                // ✅ Gọi method mới
                var result = FlightBUS.Instance.GetFlightViewModels();

                if (!result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        "Lỗi tải dữ liệu",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // ✅ Cast về FlightViewModel
                var flights = result.GetData<List<FlightViewModel>>();

                if (flights == null || flights.Count == 0)
                {
                    MessageBox.Show(
                        "Không có chuyến bay nào trong hệ thống",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                // ✅ Populate table với data thật
                foreach (var flight in flights)
                {
                    table.Rows.Add(
                        flight.FlightNumber,
                        $"{flight.DepartureAirportCode} - {flight.DepartureAirportName}",  // ✅ Có tên rồi
                        $"{flight.ArrivalAirportCode} - {flight.ArrivalAirportName}",      // ✅ Có tên rồi
                        flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A",
                        flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A",
                        flight.Status.GetDescription(),
                        "", // Action column (custom painted)
                        flight.FlightId // Hidden ID
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi không xác định:\n{ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Public method để refresh data (gọi từ FlightControl)
        /// </summary>
        public void RefreshData()
        {
            LoadFlights();
        }

        #endregion

        #region Search/Filter

        private void BtnSearchFlight_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                table.Rows.Clear();

                // Lấy filter values
                DateTime fromDate = dtpDepartureDate.Value.Date;
                DateTime toDate = dtpArrivalDate.Value.Date.AddDays(1).AddSeconds(-1); // End of day

                // Call BUS
                var result = FlightBUS.Instance.GetFlightsByDateRange(fromDate, toDate);

                if (!result.Success)
                {
                    MessageBox.Show(result.Message, "Lỗi tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var flights = result.GetData<List<FlightDTO>>();

                if (flights == null || flights.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy chuyến bay nào", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Filter thêm theo departure/arrival place (nếu có nhập)
                var departureFilter = txtDeparturePlace.Text.Trim().ToUpper();
                var arrivalFilter = txtArrivalPlace.Text.Trim().ToUpper();

                // TODO: Filter theo airport code (cần data từ Route/Airport)

                // Populate table
                foreach (var flight in flights)
                {
                    table.Rows.Add(
                        flight.FlightNumber,
                        "N/A",
                        "N/A",
                        flight.DepartureTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A",
                        flight.ArrivalTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A",
                        flight.Status.GetDescription(),
                        "",
                        flight.FlightId
                    );
                }

                MessageBox.Show($"Tìm thấy {flights.Count} chuyến bay", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Action Column Rendering & Handling

        private (Rectangle rcView, Rectangle rcEdit, Rectangle rcDelete) GetActionRects(Rectangle cellBounds, Font font)
        {
            int padding = 6;
            int x = cellBounds.Left + padding;
            int y = cellBounds.Top + (cellBounds.Height - font.Height) / 2;

            var flags = TextFormatFlags.NoPadding;
            var szView = TextRenderer.MeasureText(TXT_VIEW, font, Size.Empty, flags);
            var szSep = TextRenderer.MeasureText(SEP, font, Size.Empty, flags);
            var szEdit = TextRenderer.MeasureText(TXT_EDIT, font, Size.Empty, flags);
            var szDel = TextRenderer.MeasureText(TXT_DELETE, font, Size.Empty, flags);

            var rcView = new Rectangle(new Point(x, y), szView); x += szView.Width + szSep.Width;
            var rcEdit = new Rectangle(new Point(x, y), szEdit); x += szEdit.Width + szSep.Width;
            var rcDel = new Rectangle(new Point(x, y), szDel);

            return (rcView, rcEdit, rcDel);
        }

        private void Table_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) return;

            e.Handled = true;
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            var font = e.CellStyle.Font ?? table.Font;
            var rects = GetActionRects(e.CellBounds, font);

            Color link = Color.FromArgb(0, 92, 175);
            Color sep = Color.FromArgb(120, 120, 120);
            Color del = Color.FromArgb(220, 53, 69);

            TextRenderer.DrawText(e.Graphics, TXT_VIEW, font, rects.rcView.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rects.rcView.Right, rects.rcView.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_EDIT, font, rects.rcEdit.Location, link, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, SEP, font, new Point(rects.rcEdit.Right, rects.rcEdit.Top), sep, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(e.Graphics, TXT_DELETE, font, rects.rcDelete.Location, del, TextFormatFlags.NoPadding);
        }

        private void Table_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) { table.Cursor = Cursors.Default; return; }
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) { table.Cursor = Cursors.Default; return; }

            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(cellRect, font);

            var p = new Point(e.Location.X + cellRect.Left, e.Location.Y + cellRect.Top);
            bool over = rects.rcView.Contains(p) || rects.rcEdit.Contains(p) || rects.rcDelete.Contains(p);
            table.Cursor = over ? Cursors.Hand : Cursors.Default;
        }

        private void Table_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (table.Columns[e.ColumnIndex].Name != ACTION_COL_NAME) return;

            var cellRect = table.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var font = table[e.ColumnIndex, e.RowIndex].InheritedStyle?.Font ?? table.Font;
            var rects = GetActionRects(cellRect, font);
            var p = new Point(e.Location.X + cellRect.Left, e.Location.Y + cellRect.Top);

            var row = table.Rows[e.RowIndex];

            // Lấy flight_id từ hidden column
            if (!int.TryParse(row.Cells["flightIdHidden"].Value?.ToString(), out int flightId))
            {
                MessageBox.Show("Không tìm thấy ID chuyến bay", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string flightNumber = row.Cells["flightNumber"].Value?.ToString() ?? "(n/a)";

            if (rects.rcView.Contains(p))
            {
                // ✅ Raise event để FlightControl xử lý
                OnViewDetail?.Invoke(flightId);
            }
            else if (rects.rcEdit.Contains(p))
            {
                // ✅ Raise event để FlightControl xử lý
                OnEditFlight?.Invoke(flightId);
            }
            else if (rects.rcDelete.Contains(p))
            {
                HandleDelete(flightId, flightNumber);
            }
        }

        #endregion

        #region Delete Handling

        private void HandleDelete(int flightId, string flightNumber)
        {
            var confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa chuyến bay '{flightNumber}'?\n\nHành động này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmResult != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;

                var result = FlightBUS.Instance.DeleteFlight(flightId);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshData(); // Reload table
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}