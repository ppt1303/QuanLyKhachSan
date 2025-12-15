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
            tileControlPhong.ItemClick += TileControlPhong_ItemClick;
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
                _dtPhong = _bll.LayDanhSachPhongTrangChu();
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
            if (_dtPhong == null) return;

            // Lấy tên cột đúng theo Stored Procedure vừa tạo
            string colName = "TrangThaiHienThi";

            // Đếm số lượng bằng LINQ
            int countAll = _dtPhong.Rows.Count;
            int countTrong = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 1);
            int countDangO = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 2);
            int countDatTruoc = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 3);
            int countChuaDon = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 4); // DB là 2, nhưng SP đã đổi thành 4
            int countBaoTri = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 0);

            // Tạo nút (Logic giữ nguyên)
            AddStatButton(flowStats, $"TẤT CẢ ({countAll})", Color.Black, -1);
            AddStatButton(flowStats, $"TRỐNG ({countTrong})", Color.SeaGreen, 1);
            AddStatButton(flowStats, $"ĐANG Ở ({countDangO})", Color.Firebrick, 2);
            AddStatButton(flowStats, $"ĐẶT TRƯỚC ({countDatTruoc})", Color.Goldenrod, 3);
            AddStatButton(flowStats, $"CHƯA DỌN ({countChuaDon})", Color.Gray, 4);
            AddStatButton(flowStats, $"BẢO TRÌ ({countBaoTri})", Color.Purple, 0);

            flowFloors.Controls.Clear();
            // Tạo nút All
            SimpleButton btnAll = new SimpleButton { Text = "Mọi tầng", Size = new Size(100, 35), Tag = 0 };
            HighlightButton(btnAll, _filterTang == 0);
            btnAll.Click += FloorButton_Click;
            flowFloors.Controls.Add(btnAll);

            // Tạo nút từng tầng (Lấy cột Tang từ SP)
            var listTang = _dtPhong.AsEnumerable()
                                   .Select(r => r.Field<byte>("Tang")) // Trong DB Tang là tinyint -> C# là byte
                                   .Distinct()
                                   .OrderBy(t => t);

            foreach (byte tang in listTang)
            {
                SimpleButton btn = new SimpleButton { Text = "Tầng " + tang, Size = new Size(80, 35), Tag = (int)tang };
                HighlightButton(btn, _filterTang == (int)tang);
                btn.Click += FloorButton_Click;
                flowFloors.Controls.Add(btn);
            }
        }

        // --- SỰ KIỆN CLICK NÚT TẦNG (Viết gọn lại) ---
        private void FloorButton_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            _filterTang = Convert.ToInt32(btn.Tag); // Lấy tầng từ Tag

            VeSoDoPhong();            // Vẽ lại sơ đồ
            TaoThanhBoLocVaThongKe(); // Vẽ lại thanh công cụ để cập nhật Highlight màu xanh
        }

        // --- HÀM HỖ TRỢ 1: TẠO NÚT THỐNG KÊ ---
        private void AddStatButton(FlowLayoutPanel panel, string text, Color color, int statusFilter)
        {
            SimpleButton btn = new SimpleButton { Text = text, Tag = statusFilter, AutoSize = true };
            btn.Padding = new Padding(5);
            btn.Margin = new Padding(3, 3, 10, 3);
            btn.Cursor = Cursors.Hand;

            // Logic Highlight: Nếu nút này khớp với bộ lọc đang chọn (_filterTrangThai)
            if (_filterTrangThai == statusFilter)
            {
                // Đang chọn: Chữ đậm, màu Xanh Dương (Blue) để nổi bật
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = Color.Blue;
            }
            else
            {
                // Bình thường: Chữ đậm, màu theo quy định (Đỏ, Vàng, Tím...)
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = color;
            }

            btn.Click += (s, e) =>
            {
                _filterTrangThai = (int)((SimpleButton)s).Tag; 
                VeSoDoPhong();            
                TaoThanhBoLocVaThongKe(); 
            };

            panel.Controls.Add(btn);
        }

        // --- HÀM HỖ TRỢ 2: HIGHLIGHT NÚT TẦNG ---
        private void HighlightButton(SimpleButton btn, bool isActive)
        {
            if (isActive)
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = Color.Blue; // Màu xanh dương khi đang chọn
            }
            else
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                btn.Appearance.ForeColor = Color.Black; // Màu đen bình thường
            }
        }

        // --- 6. VẼ SƠ ĐỒ PHÒNG (LOGIC LỌC KÉP) ---
        private void VeSoDoPhong()
        {
            tileControlPhong.Groups.Clear();

            string filterExpr = "1=1";
            if (_filterTang > 0) filterExpr += $" AND Tang = {_filterTang}";

            string searchText = txtTimKiem.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                filterExpr += $" AND (TenPhong LIKE '%{searchText}%' OR TenLP LIKE '%{searchText}%')";
            }

            DataRow[] rows = _dtPhong.Select(filterExpr, "Tang ASC, TenPhong ASC");

            // Lọc theo trạng thái (0-4)
            if (_filterTrangThai != -1)
            {
                rows = rows.Where(r =>
                {
                    // Quan trọng: Lấy cột TrangThaiHienThi từ SP
                    int code = Convert.ToInt32(r["TrangThaiHienThi"]);
                    return code == _filterTrangThai;
                }).ToArray();
            }

            if (rows.Length == 0) return;

            // Vẽ Tile
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

            // Lấy trạng thái đã được tính toán từ SQL
            int ttHienThi = Convert.ToInt32(row["TrangThaiHienThi"]);

            Color backColor;
            string statusText;
            string iconText;

            switch (ttHienThi)
            {
                case 0: // Bảo trì
                    backColor = Color.Purple;
                    statusText = "Đang bảo trì";
                    iconText = "🛠";
                    break;
                case 4: // Chưa dọn (DB là 2 -> SP đổi thành 4)
                    backColor = Color.Gray;
                    statusText = "Chưa dọn dẹp";
                    iconText = "🧹";
                    break;
                case 2: // Đang ở
                    backColor = Color.Firebrick;
                    statusText = "Đang thuê";
                    iconText = "👤";
                    break;
                case 3: // Đặt trước
                    backColor = Color.Goldenrod;
                    statusText = "Đã đặt trước";
                    iconText = "📅";
                    break;
                case 1: // Trống
                default:
                    backColor = Color.SeaGreen;
                    statusText = "Phòng trống";
                    iconText = "✔";
                    break;
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
        private void TileControlPhong_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                // 1. Kiểm tra Tag (nơi chứa MaPhong)
                if (e.Item.Tag == null) return;

                int maPhong = Convert.ToInt32(e.Item.Tag);

                // 2. Tìm thông tin phòng trong DataTable (_dtPhong) để lấy trạng thái hiện tại
                DataRow[] rows = _dtPhong.Select($"MaPhong = {maPhong}");
                if (rows.Length == 0) return;

                DataRow row = rows[0];
                string tenPhong = row["TenPhong"].ToString();
                int trangThai = Convert.ToInt32(row["TrangThaiHienThi"]); // 0:Bảo trì, 1:Trống, 2:Đang ở, 3:Đặt trước, 4:Dơ

                // 3. Xử lý mở Form dựa trên trạng thái
                // Giả sử bạn có một Form tên là frmChiTietPhong để xử lý chung
                // Hoặc frmDatPhong (cho đặt mới), frmCheckOut (cho thanh toán)

                switch (trangThai)
                {
                    case 1: // --- PHÒNG TRỐNG ---
                    case 4: // --- PHÒNG DƠ (Cho phép mở lên để dọn dẹp -> set về trống) ---
                            // Mở form đặt phòng hoặc dọn dẹp
                            // Truyền: MaNP = 0 (chưa có), MaPhong = maPhong
                        frmChiTietPhong frmTrong = new frmChiTietPhong(0, maPhong);
                        frmTrong.Text = $"Quản lý phòng {tenPhong} - TRỐNG";
                        frmTrong.ShowDialog();
                        break;

                    case 2: // --- ĐANG Ở ---
                            // Cần lấy Mã Nhận Phòng (MaNP) đang hoạt động của phòng này
                        int maNP = _bookingBLL.GetCurrentStayID(maPhong);

                        if (maNP > 0)
                        {
                            // Mở form chi tiết cho khách đang ở (Thêm dịch vụ / Thanh toán)
                            frmChiTietPhong frmDangO = new frmChiTietPhong(maNP, maPhong);
                            frmDangO.Text = $"Chi tiết phòng {tenPhong} - ĐANG CÓ KHÁCH";
                            frmDangO.ShowDialog();
                        }
                        else
                        {
                            XtraMessageBox.Show($"Lỗi dữ liệu: Phòng {tenPhong} báo đang ở nhưng không tìm thấy thông tin nhận phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;

                    case 3: // --- ĐẶT TRƯỚC ---
                        XtraMessageBox.Show($"Phòng {tenPhong} đã được đặt trước. Vui lòng vào mục 'Quản lý đặt phòng' để Check-in.", "Thông báo");
                        break;

                    case 0: // --- BẢO TRÌ ---
                        if (XtraMessageBox.Show($"Phòng {tenPhong} đang bảo trì. Bạn có muốn mở khóa (Set về Trống) không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // Gọi hàm cập nhật trạng thái về 1 (Sẵn sàng)
                            // _bll.CapNhatTrangThaiPhong(maPhong, 1);
                            // LoadData();
                        }
                        break;
                }

                // 4. Load lại dữ liệu sau khi đóng Form (để cập nhật màu sắc nếu trạng thái thay đổi)
                LoadData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}