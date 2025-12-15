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
        private PhongBLL _bll;
        private BookingBLL _bookingBLL;
        private DataTable _dtPhong;
        private int _filterTang = 0;
        private int _filterTrangThai = -1;
        public ucTrangChu()
        {
            InitializeComponent();
            _bll = new PhongBLL();
            _bookingBLL = new BookingBLL();
            KhoiTaoGiaoDien();
            this.Load += UcTrangChu_Load;
        }

        private void UcTrangChu_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode) LoadData();
        }
        private void KhoiTaoGiaoDien()
        {
            pnlHeader = new PanelControl();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 60;
            pnlHeader.Padding = new Padding(10);
            pnlHeader.BorderStyle = BorderStyles.NoBorder;
            dtpNgay = new DateEdit();
            dtpNgay.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Fluent;
            dtpNgay.DateTime = DateTime.Now;
            dtpNgay.Parent = pnlHeader;
            dtpNgay.Location = new Point(10, 15);
            dtpNgay.Width = 120;
            dtpNgay.Properties.NullText = "Chọn ngày";
            dtpNgay.EditValueChanged += (s, e) => LoadData(); // Load lại khi đổi ngày
            dtpGio = new TimeEdit();
            dtpGio.Time = DateTime.Now;
            dtpGio.Parent = pnlHeader;
            dtpGio.Location = new Point(140, 15);
            dtpGio.Width = 100;
            dtpGio.EditValueChanged += (s, e) => LoadData(); // Load lại khi đổi giờ
            txtTimKiem = new SearchControl();
            txtTimKiem.Parent = pnlHeader;
            txtTimKiem.Location = new Point(260, 15);
            txtTimKiem.Width = 300;
            txtTimKiem.Properties.NullValuePrompt = "Tìm tên phòng, loại phòng...";
            txtTimKiem.TextChanged += (s, e) => VeSoDoPhong(); // Tìm kiếm Realtime

    
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
            flowStats.Controls.Clear();

            // Tính toán số lượng từ dữ liệu hiện tại
            RoomStatistics stats = _bll.TinhThongKe(_dtPhong);

            // QUY ƯỚC MÃ LỌC (_filterTrangThai) ĐỂ KHỚP VỚI HÀM VeSoDoPhong:
            // -1: Tất cả
            //  1: Trống
            //  2: Đang ở
            //  3: Đặt trước
            //  4: Chưa dọn
            //  0: Bảo trì

            AddStatButton(flowStats, $"TẤT CẢ ({stats.TongSo})", Color.Black, -1);
            AddStatButton(flowStats, $"TRỐNG ({stats.Trong})", Color.SeaGreen, 1);
            AddStatButton(flowStats, $"ĐANG Ở ({stats.DangO})", Color.Firebrick, 2);
            AddStatButton(flowStats, $"ĐẶT TRƯỚC ({stats.DatTruoc})", Color.Goldenrod, 3);
            AddStatButton(flowStats, $"CHƯA DỌN ({stats.Ban})", Color.Gray, 4);
            AddStatButton(flowStats, $"BẢO TRÌ ({stats.BaoTri})", Color.Purple, 0);

            flowFloors.Controls.Clear();

            // 1. Tạo nút "Mọi tầng"
            SimpleButton btnAll = new SimpleButton { Text = "Mọi tầng", Size = new Size(100, 35) };
            HighlightButton(btnAll, _filterTang == 0); // Highlight nếu đang chọn (0 là mặc định)

            btnAll.Click += (s, e) =>
            {
                _filterTang = 0;           // 0 nghĩa là lấy tất cả tầng
                VeSoDoPhong();             // Vẽ lại sơ đồ
                TaoThanhBoLocVaThongKe();  // Vẽ lại nút để cập nhật highlight
            };
            flowFloors.Controls.Add(btnAll);

            // 2. Tạo các nút tầng động từ CSDL
            DataTable dtTang = _bll.LayDanhSachTang(_dtPhong);
            if (dtTang != null)
            {
                foreach (DataRow row in dtTang.Rows)
                {
                    if (row["Tang"] != DBNull.Value)
                    {
                        int tang = Convert.ToInt32(row["Tang"]);
                        SimpleButton btn = new SimpleButton { Text = "Tầng " + tang, Tag = tang, Size = new Size(80, 35) };

                        HighlightButton(btn, _filterTang == tang); // Highlight nếu tầng này đang được chọn

                        btn.Click += (s, e) =>
                        {
                            _filterTang = (int)((SimpleButton)s).Tag; // Lưu tầng vừa chọn
                            VeSoDoPhong();
                            TaoThanhBoLocVaThongKe();
                        };
                        flowFloors.Controls.Add(btn);
                    }
                }
            }
        }

        // --- HÀM HỖ TRỢ 1: TẠO NÚT THỐNG KÊ ---
        private void AddStatButton(FlowLayoutPanel panel, string text, Color color, int statusFilter)
        {
            SimpleButton btn = new SimpleButton { Text = text, Tag = statusFilter, AutoSize = true };
            btn.Padding = new Padding(5);
            btn.Margin = new Padding(3, 3, 10, 3);

            // Highlight nút thống kê nếu nó đang được chọn
            if (_filterTrangThai == statusFilter)
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = Color.Blue; // Màu xanh dương để biết đang chọn                                                                                               
            }
            else
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = color; // Màu đặc trưng của trạng thái (Đỏ, Xanh, Vàng...)
            }

            btn.Click += (s, e) =>
            {
                _filterTrangThai = (int)((SimpleButton)s).Tag;
                VeSoDoPhong();            // Vẽ lại sơ đồ theo bộ lọc mới
                TaoThanhBoLocVaThongKe(); // Vẽ lại các nút để cập nhật hiệu ứng highlight
            };
            panel.Controls.Add(btn);
        }

        // --- HÀM HỖ TRỢ 2: HIGHLIGHT NÚT TẦNG ---
        private void HighlightButton(SimpleButton btn, bool isActive)
        {
            if (isActive)
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = Color.Blue; // Đang chọn
            }
            else
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                btn.Appearance.ForeColor = Color.Black; // Bình thường
            }
        }

        // --- 6. VẼ SƠ ĐỒ PHÒNG (LOGIC LỌC KÉP) ---
        private void VeSoDoPhong()
        {
            tileControlPhong.Groups.Clear();

            // Bước 1: Lọc SQL cơ bản (Giữ nguyên)
            string filterExpr = "1=1";
            if (_filterTang > 0) filterExpr += $" AND Tang = {_filterTang}";

            string searchText = txtTimKiem.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                filterExpr += $" AND (TenPhong LIKE '%{searchText}%' OR TenLP LIKE '%{searchText}%')";
            }

            DataRow[] rows = _dtPhong.Select(filterExpr, "Tang ASC, TenPhong ASC");

            // Bước 2: Lọc nâng cao (Trạng thái)
            if (_filterTrangThai != -1)
            {
                rows = rows.Where(r =>
                {
                    int code = Convert.ToInt32(r["TrangThaiPhong"]); // 0:Bảo trì, 1:Sẵn sàng, 2:Dơ
                    string statusO = r["TrangThaiO"].ToString();     // "Trống", "Đang ở", "Đặt trước"

                    // TH 0: Bảo trì (Chỉ lấy code = 0)
                    if (_filterTrangThai == 0) return code == 0;

                    // TH 4: Chưa dọn/Dơ (Chỉ lấy code = 2)
                    if (_filterTrangThai == 4) return code == 2;

                    // TH 1: Trống (Phải Sạch & Trống)
                    if (_filterTrangThai == 1) return code == 1 && statusO == "Trống";

                    // TH 2: Đang ở (Phải Sạch & Đang ở)
                    if (_filterTrangThai == 2) return code == 1 && statusO == "Đang ở";

                    // TH 3: Đặt trước (Phải Sạch & Đặt trước)
                    if (_filterTrangThai == 3) return code == 1 && statusO == "Đặt trước";

                    return true;
                }).ToArray();
            }

            if (rows.Length == 0) return;

            // Bước 3: Vẽ Tile (Giữ nguyên)
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
                    // item.ItemClick += Item_ItemClick; 
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
            if (ttPhong == 0) 
            {
                backColor = Color.Purple;
                statusText = "Đang bảo trì";
                iconText = "🛠";
            }
            else if (ttPhong == 2)
            {
                backColor = Color.Gray;
                statusText = "Chưa dọn dẹp";
                iconText = "🧹";
            }
            else 
            {
                if (ttO == "Đang ở")
                {
                    backColor = Color.Firebrick;
                    statusText = "Đang thuê";
                    iconText = "👤";
                }
                else if (ttO == "Đặt trước")
                {
                    backColor = Color.Goldenrod;
                    statusText = "Đã đặt trước";
                    iconText = "📅";
                }
                else 
                {
                    backColor = Color.Green;
                    statusText = "Phòng trống";
                    iconText = "✔";
                }
            }

            item.AppearanceItem.Normal.BackColor = backColor;
            item.AppearanceItem.Normal.BorderColor = Color.Transparent;

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
        /////////////////////////TƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯ
        // --- 8. SỰ KIỆN CLICK PHÒNG ---
        //private void Item_ItemClick(object sender, TileItemEventArgs e)
        //{
        //    if (e.Item.Tag != null)
        //    {
        //        int maPhong = Convert.ToInt32(e.Item.Tag);
        //        // 1. Lấy DataRow tương ứng với phòng vừa click để xác định trạng thái
        //        DataRow[] rows = _dtPhong.Select($"MaPhong = {maPhong}");

        //        if (rows.Length > 0)
        //        {
        //            DataRow roomRow = rows[0];
        //            string tenPhong = roomRow["TenPhong"].ToString();
        //            string statusO = roomRow["TrangThaiO"].ToString(); // "Đang ở", "Đặt trước", "Trống"
        //            int ttPhong = Convert.ToInt32(roomRow["TrangThaiPhong"]); // 0:Bảo trì, 1:Sẵn sàng, 2:Dơ

        //            // Biến để lưu Mã Nhận Phòng (MaNP), mặc định là 0 nếu phòng không có khách đang ở.
        //            int maNP = 0;

        //            // 2. Xác định MaNP chỉ khi phòng đang có khách ở
        //            if (statusO == "Đang ở")
        //            {
        //                // Phòng đang có khách -> Lấy Mã Nhận Phòng (MaNP) từ BLL
        //                maNP = _bookingBLL.GetCurrentStayID(maPhong);

        //                if (maNP <= 0)
        //                {
        //                    // Trường hợp lỗi dữ liệu: trạng thái là "Đang ở" nhưng không tìm thấy MaNP. 
        //                    // Vẫn nên thông báo lỗi và ngăn form mở với dữ liệu lỗi.
        //                    XtraMessageBox.Show("Lỗi hệ thống: Không tìm thấy Mã Nhận Phòng (MaNP) đang hoạt động cho phòng đang có khách.", "Lỗi dữ liệu");
        //                    return;
        //                }
        //            }
        //            // KHÔNG CẦN ELSE/ELSE IF cho các trạng thái khác ("Đặt trước", "Trống", Bảo trì/Dơ) nữa.
        //            // Nếu không phải "Đang ở", maNP sẽ giữ giá trị mặc định là 0.

        //            // 3. Mở Form Chi tiết phòng với maNP (0 hoặc ID hợp lệ) và maPhong
        //            // Form frmChiTietPhong cần được chỉnh sửa để xử lý maNP = 0 như một chỉ báo 
        //            // rằng form được mở để thực hiện Check-in/Booking mới hoặc thay đổi trạng thái kỹ thuật.
        //            frmChiTietPhong frm = new frmChiTietPhong(maNP, maPhong);

        //            // Đặt tiêu đề form rõ ràng hơn dựa trên trạng thái
        //            if (maNP > 0)
        //            {
        //                frm.Text = $"Chi Tiết & Thanh Toán Phòng {tenPhong}";
        //            }
        //            else
        //            {
        //                string trangThaiKyThuat = (ttPhong == 0) ? "Bảo Trì" : (ttPhong == 2) ? "Chờ Dọn Dẹp" : "Sẵn Sàng";
        //                frm.Text = $"Quản Lý Phòng {tenPhong} - TT: {statusO} ({trangThaiKyThuat})";
        //            }
        //            frm.ShowDialog();
        //            // 4. Cập nhật lại sơ đồ phòng sau khi đóng form (để làm mới trạng thái)
        //            LoadData();
        //        }
        //    }
        //}
    }
}