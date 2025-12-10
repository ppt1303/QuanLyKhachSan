using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyKhachSan.BLL;
using QuanLyKhachSan.DTO;

namespace QuanLyKhachSan.GUI
{
    public partial class ucTrangChu : XtraUserControl
    {
        // --- 1. KHAI BÁO CONTROL ---
        private PanelControl pnlHeader;       // Hàng 1: Ngày, Giờ, Tìm kiếm
        private FlowLayoutPanel flowStats;    // Hàng 2: Thống kê trạng thái
        private FlowLayoutPanel flowFloors;   // Hàng 3: Bộ lọc Tầng
        private TileControl tileControlPhong; // Hàng 4: Sơ đồ phòng

        private DateEdit dtpNgay;
        private TimeEdit dtpGio;
        private SearchControl txtTimKiem;

        // --- 2. BIẾN DỮ LIỆU ---
        private PhongBLL _bll;
        private DataTable _dtPhong;

        // Biến lưu trạng thái lọc (Mặc định: Tất cả)
        private int _filterTang = 0;
        private int _filterTrangThai = -1;

        public ucTrangChu()
        {
            InitializeComponent();
            _bll = new PhongBLL();

            // Dựng giao diện bằng code (Tránh lỗi Designer)
            KhoiTaoGiaoDien();

            // Đăng ký sự kiện Load
            this.Load += UcTrangChu_Load;
        }

        private void UcTrangChu_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode) LoadData();
        }

        // --- 3. DỰNG GIAO DIỆN (LAYOUT CHUẨN KHÔNG BỊ CHE) ---
        private void KhoiTaoGiaoDien()
        {
            // A. Khởi tạo các thành phần (Chưa Add vào Form)

            // 1. Header (Panel trên cùng)
            pnlHeader = new PanelControl();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 60;
            pnlHeader.Padding = new Padding(10);
            pnlHeader.BorderStyle = BorderStyles.NoBorder;

            // --- Các control con trong Header ---
            // Date Picker
            dtpNgay = new DateEdit();
            dtpNgay.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Fluent;
            dtpNgay.DateTime = DateTime.Now;
            dtpNgay.Parent = pnlHeader;
            dtpNgay.Location = new Point(10, 15);
            dtpNgay.Width = 120;
            dtpNgay.Properties.NullText = "Chọn ngày";
            dtpNgay.EditValueChanged += (s, e) => LoadData(); // Load lại khi đổi ngày

            // Time Picker
            dtpGio = new TimeEdit();
            dtpGio.Time = DateTime.Now;
            dtpGio.Parent = pnlHeader;
            dtpGio.Location = new Point(140, 15);
            dtpGio.Width = 100;
            dtpGio.EditValueChanged += (s, e) => LoadData(); // Load lại khi đổi giờ

            // Search Box
            txtTimKiem = new SearchControl();
            txtTimKiem.Parent = pnlHeader;
            txtTimKiem.Location = new Point(260, 15);
            txtTimKiem.Width = 300;
            txtTimKiem.Properties.NullValuePrompt = "Tìm tên phòng, loại phòng...";
            txtTimKiem.TextChanged += (s, e) => VeSoDoPhong(); // Tìm kiếm Realtime

            // 2. Hàng Thống kê (Statistics)
            flowStats = new FlowLayoutPanel();
            flowStats.Dock = DockStyle.Top;
            flowStats.Height = 45;
            flowStats.Padding = new Padding(5);
            flowStats.BackColor = Color.WhiteSmoke;

            // 3. Hàng Tầng (Floors)
            flowFloors = new FlowLayoutPanel();
            flowFloors.Dock = DockStyle.Top;
            flowFloors.Height = 50;
            flowFloors.Padding = new Padding(5);
            flowFloors.AutoScroll = true;    // Cho phép cuộn ngang
            flowFloors.WrapContents = false; // Không xuống dòng

            // 4. Sơ đồ phòng (TileControl)
            tileControlPhong = new TileControl();
            tileControlPhong.Dock = DockStyle.Fill; // Lấp đầy phần còn lại
            tileControlPhong.AllowDrag = false;
            tileControlPhong.Orientation = Orientation.Vertical;
            tileControlPhong.VerticalContentAlignment = DevExpress.Utils.VertAlignment.Top;
            tileControlPhong.ScrollMode = TileControlScrollMode.ScrollBar;
            tileControlPhong.Padding = new Padding(10);

            // B. THÊM VÀO FORM (ADD NGƯỢC ĐỂ XẾP ĐÚNG Z-ORDER)
            // Thứ tự muốn hiển thị từ trên xuống: Header -> Stats -> Floors -> Tile
            // Thứ tự Add code: Tile -> Floors -> Stats -> Header

            this.Controls.Add(tileControlPhong); // Dưới cùng
            this.Controls.Add(flowFloors);       // Đè lên Tile
            this.Controls.Add(flowStats);        // Đè lên Floors
            this.Controls.Add(pnlHeader);        // Trên cùng
        }

        // --- 4. TẢI DỮ LIỆU TỪ CSDL ---
        public void LoadData()
        {
            try
            {
                // Lấy thời điểm xem từ giao diện
                DateTime ngay = dtpNgay.DateTime.Date;
                TimeSpan gio = dtpGio.Time.TimeOfDay;
                DateTime thoiDiemXem = ngay + gio;

                // Gọi BLL lấy dữ liệu tại thời điểm đó
                _dtPhong = _bll.LayTatCaPhong(thoiDiemXem);

                if (_dtPhong == null || _dtPhong.Rows.Count == 0) return;

                TaoThanhBoLocVaThongKe(); // Vẽ lại nút và số lượng
                VeSoDoPhong();            // Vẽ lại sơ đồ
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // --- 5. TẠO THANH CÔNG CỤ (NÚT BẤM & HIGHLIGHT) ---
        private void TaoThanhBoLocVaThongKe()
        {
            // A. Cập nhật Thống kê
            flowStats.Controls.Clear();
            RoomStatistics stats = _bll.TinhThongKe(_dtPhong);

            // Các mã trạng thái lọc: -1(All), 1(Trống), 2(Đang ở), 3(Đặt trước), 0(Bảo trì/Dơ)
            AddStatButton(flowStats, $"TẤT CẢ ({stats.TongSo})", Color.Black, -1);
            AddStatButton(flowStats, $"TRỐNG ({stats.Trong})", Color.SeaGreen, 1);
            AddStatButton(flowStats, $"ĐANG Ở ({stats.DangO})", Color.Firebrick, 2);
            AddStatButton(flowStats, $"ĐẶT TRƯỚC ({stats.DatTruoc})", Color.Goldenrod, 3);
            AddStatButton(flowStats, $"BẢO TRÌ/DƠ ({stats.BaoTri + stats.Ban})", Color.DimGray, 0);

            // B. Cập nhật Tầng
            flowFloors.Controls.Clear();

            // Nút Mọi tầng
            SimpleButton btnAll = new SimpleButton { Text = "Mọi tầng", Size = new Size(100, 35) };
            HighlightButton(btnAll, _filterTang == 0); // Highlight nếu đang chọn
            btnAll.Click += (s, e) =>
            {
                _filterTang = 0;
                VeSoDoPhong();
                TaoThanhBoLocVaThongKe(); // Redraw buttons
            };
            flowFloors.Controls.Add(btnAll);

            // Các nút tầng động từ DB
            DataTable dtTang = _bll.LayDanhSachTang(_dtPhong);
            if (dtTang != null)
            {
                foreach (DataRow row in dtTang.Rows)
                {
                    if (row["Tang"] != DBNull.Value)
                    {
                        int tang = Convert.ToInt32(row["Tang"]);
                        SimpleButton btn = new SimpleButton { Text = "Tầng " + tang, Tag = tang, Size = new Size(80, 35) };

                        HighlightButton(btn, _filterTang == tang); // Highlight

                        btn.Click += (s, e) =>
                        {
                            _filterTang = (int)((SimpleButton)s).Tag;
                            VeSoDoPhong();
                            TaoThanhBoLocVaThongKe();
                        };
                        flowFloors.Controls.Add(btn);
                    }
                }
            }
        }

        // Hàm tạo nút thống kê
        private void AddStatButton(FlowLayoutPanel panel, string text, Color color, int statusFilter)
        {
            SimpleButton btn = new SimpleButton { Text = text, Tag = statusFilter, AutoSize = true };
            btn.Padding = new Padding(5);
            btn.Margin = new Padding(3, 3, 10, 3);

            // Highlight nếu đang chọn trạng thái này
            if (_filterTrangThai == statusFilter)
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = Color.Blue;
            }
            else
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = color;
            }

            btn.Click += (s, e) =>
            {
                _filterTrangThai = (int)((SimpleButton)s).Tag;
                VeSoDoPhong();
                TaoThanhBoLocVaThongKe(); // Redraw
            };
            panel.Controls.Add(btn);
        }

        // Hàm highlight nút Tầng
        private void HighlightButton(SimpleButton btn, bool isActive)
        {
            if (isActive)
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = Color.Blue;
            }
            else
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                btn.Appearance.ForeColor = Color.Black;
            }
        }

        // --- 6. VẼ SƠ ĐỒ PHÒNG (LOGIC LỌC KÉP) ---
        private void VeSoDoPhong()
        {
            tileControlPhong.Groups.Clear();

            // Bước 1: Lọc cơ bản (Tầng + Search) bằng SQL Select string
            string filterExpr = "1=1";
            if (_filterTang > 0) filterExpr += $" AND Tang = {_filterTang}";

            string searchText = txtTimKiem.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                filterExpr += $" AND (TenPhong LIKE '%{searchText}%' OR TenLP LIKE '%{searchText}%')";
            }

            DataRow[] rows = _dtPhong.Select(filterExpr, "Tang ASC, TenPhong ASC");

            // Bước 2: Lọc nâng cao (Trạng thái) bằng LINQ
            if (_filterTrangThai != -1)
            {
                rows = rows.Where(r =>
                {
                    int code = Convert.ToInt32(r["TrangThaiPhong"]); // 0,1,2
                    string statusO = r["TrangThaiO"].ToString();     // "Trống", "Đang ở"...

                    if (_filterTrangThai == 0) return code == 0 || code == 2; // Bảo trì hoặc Dơ
                    if (_filterTrangThai == 1) return code == 1 && statusO == "Trống";
                    if (_filterTrangThai == 2) return statusO == "Đang ở";
                    if (_filterTrangThai == 3) return statusO == "Đặt trước";
                    return true;
                }).ToArray();
            }

            if (rows.Length == 0) return;

            // Bước 3: Vẽ Tile theo nhóm
            var distinctTang = rows.Select(r => r["Tang"]).Distinct().OrderBy(t => t);
            foreach (var tang in distinctTang)
            {
                TileGroup group = new TileGroup { Text = "TẦNG " + tang };
                tileControlPhong.Groups.Add(group);

                var phongInGroup = rows.Where(r => r["Tang"].ToString() == tang.ToString());
                foreach (DataRow row in phongInGroup)
                {
                    TileItem item = new TileItem();
                    FormatTileItem_Final(item, row);
                    item.Tag = row["MaPhong"];
                    item.ItemClick += Item_ItemClick; // Xử lý click
                    group.Items.Add(item);
                }
            }
        }

        // --- 7. ĐỊNH DẠNG CARD PHÒNG ---
        private void FormatTileItem_Final(TileItem item, DataRow row)
        {
            item.ItemSize = TileItemSize.Wide;
            item.Elements.Clear();

            string tenPhong = row["TenPhong"].ToString();
            string tenLP = row["TenLP"].ToString();
            int ttPhong = Convert.ToInt32(row["TrangThaiPhong"]);
            string ttO = row["TrangThaiO"].ToString();

            Color backColor;
            string statusText;
            string iconText;

            // Logic màu sắc chuẩn
            if (ttPhong == 0) // Bảo trì
            {
                backColor = Color.FromArgb(255, 140, 0);
                statusText = "Đang bảo trì";
                iconText = "🛠";
            }
            else if (ttPhong == 2) // Dơ
            {
                backColor = Color.Gray;
                statusText = "Chưa dọn dẹp";
                iconText = "🧹";
            }
            else // Sẵn sàng
            {
                if (ttO == "Đang ở")
                {
                    backColor = Color.FromArgb(105, 105, 105);
                    statusText = "Đang thuê";
                    iconText = "👤";
                }
                else if (ttO == "Đặt trước")
                {
                    backColor = Color.Goldenrod;
                    statusText = "Đã đặt trước";
                    iconText = "📅";
                }
                else // Trống
                {
                    backColor = Color.FromArgb(46, 204, 113);
                    statusText = "Phòng trống";
                    iconText = "✔";
                }
            }

            item.AppearanceItem.Normal.BackColor = backColor;
            item.AppearanceItem.Normal.BorderColor = Color.Transparent;

            // Vẽ thông tin lên Card
            TileItemElement eTen = new TileItemElement { Text = tenPhong, TextAlignment = TileItemContentAlignment.TopLeft };
            eTen.Appearance.Normal.Font = new Font("Segoe UI", 16, FontStyle.Bold);

            TileItemElement eStatus = new TileItemElement { Text = statusText, TextAlignment = TileItemContentAlignment.TopRight };
            eStatus.Appearance.Normal.FontSizeDelta = -2;

            TileItemElement eIcon = new TileItemElement { Text = iconText, TextAlignment = TileItemContentAlignment.MiddleLeft };
            eIcon.Appearance.Normal.Font = new Font("Segoe UI", 24, FontStyle.Bold);

            TileItemElement eFooter = new TileItemElement { Text = tenLP, TextAlignment = TileItemContentAlignment.BottomLeft };
            eFooter.Appearance.Normal.FontSizeDelta = -2;

            item.Elements.Add(eTen);
            item.Elements.Add(eStatus);
            item.Elements.Add(eIcon);
            item.Elements.Add(eFooter);
        }

        // --- 8. SỰ KIỆN CLICK PHÒNG ---
        private void Item_ItemClick(object sender, TileItemEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                int maPhong = Convert.ToInt32(e.Item.Tag);
                XtraMessageBox.Show("Mã phòng: " + maPhong);
                // TODO: Mở form Check-in/Check-out tại đây
            }
        }
    }
}