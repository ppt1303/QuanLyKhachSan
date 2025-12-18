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

        // --- KHAI BÁO BIẾN DỮ LIỆU ---
        private PhongBLL _bll;
        private BookingBLL _bookingBLL;
        private DataTable _dtPhong;
        private int _filterTang = 0;
        private int _filterTrangThai = -1;

        public ucTrangChu()
        {
            InitializeComponent();

            // Khởi tạo lớp xử lý nghiệp vụ
            _bll = new PhongBLL();
            _bookingBLL = new BookingBLL();

            // Vẽ giao diện code-behind
            KhoiTaoGiaoDien();

            // Đăng ký sự kiện UI
            tileControlPhong.ItemClick += TileControlPhong_ItemClick;
            this.Load += UcTrangChu_Load;

            // ============================================================
            // --- ĐĂNG KÝ SỰ KIỆN ĐỒNG BỘ DỮ LIỆU (REAL-TIME) ---
            // ============================================================
            BookingBLL.OnDataChanged += () =>
            {
                // Kiểm tra xem Control có còn tồn tại và Handle đã được tạo chưa để tránh lỗi
                if (this.IsHandleCreated && !this.Disposing && !this.IsDisposed)
                {
                    // Dùng Invoke để đảm bảo chạy trên luồng UI (tránh lỗi Cross-thread)
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => LoadData()));
                    }
                    else
                    {
                        LoadData();
                    }
                }
            };
        }

        private void UcTrangChu_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                LoadData();
            }
        }

        // --- 2. KHỞI TẠO GIAO DIỆN (CODE GIAO DIỆN) ---
        private void KhoiTaoGiaoDien()
        {
            // 1. Header
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

            // 2. Hàng Thống kê (Stats)
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

            // Add Control vào Form
            this.Controls.Add(tileControlPhong);
            this.Controls.Add(flowFloors);
            this.Controls.Add(flowStats);
            this.Controls.Add(pnlHeader);
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

                // Gọi hàm BLL có truyền ngày giờ để lấy trạng thái chính xác
                _dtPhong = _bll.GetDanhSachPhongTheoNgay(thoiDiemXem);

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

            string colName = "TrangThaiHienThi";

            // Đếm số lượng bằng LINQ
            int countAll = _dtPhong.Rows.Count;
            int countTrong = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 1);
            int countDangO = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 2);
            int countDatTruoc = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 3);
            int countChuaDon = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 4);
            int countBaoTri = _dtPhong.AsEnumerable().Count(r => r.Field<int>(colName) == 0);

            // Tạo nút
            AddStatButton(flowStats, $"TẤT CẢ ({countAll})", Color.Black, -1);
            AddStatButton(flowStats, $"TRỐNG ({countTrong})", Color.SeaGreen, 1);
            AddStatButton(flowStats, $"ĐANG Ở ({countDangO})", Color.Firebrick, 2);
            AddStatButton(flowStats, $"ĐẶT TRƯỚC ({countDatTruoc})", Color.Goldenrod, 3);
            AddStatButton(flowStats, $"CHƯA DỌN ({countChuaDon})", Color.Gray, 4);
            AddStatButton(flowStats, $"BẢO TRÌ ({countBaoTri})", Color.Purple, 0);

            flowFloors.Controls.Clear();

            // Tạo nút All Tầng
            SimpleButton btnAll = new SimpleButton { Text = "Mọi tầng", Size = new Size(100, 35), Tag = 0 };
            HighlightButton(btnAll, _filterTang == 0);
            btnAll.Click += FloorButton_Click;
            flowFloors.Controls.Add(btnAll);

            // Tạo nút từng tầng
            var listTang = _dtPhong.AsEnumerable()
                                   .Select(r => r.Field<byte>("Tang"))
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

        private void FloorButton_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            _filterTang = Convert.ToInt32(btn.Tag);

            VeSoDoPhong();            // Vẽ lại sơ đồ
            TaoThanhBoLocVaThongKe(); // Vẽ lại thanh công cụ để cập nhật màu nút
        }

        private void AddStatButton(FlowLayoutPanel panel, string text, Color color, int statusFilter)
        {
            SimpleButton btn = new SimpleButton { Text = text, Tag = statusFilter, AutoSize = true };
            btn.Padding = new Padding(5);
            btn.Margin = new Padding(3, 3, 10, 3);
            btn.Cursor = Cursors.Hand;

            // Logic Highlight
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
                TaoThanhBoLocVaThongKe();
            };

            panel.Controls.Add(btn);
        }

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

        // --- 6. VẼ SƠ ĐỒ PHÒNG ---
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

            // Lấy các dòng thỏa điều kiện tìm kiếm và tầng
            DataRow[] rows = _dtPhong.Select(filterExpr, "Tang ASC, TenPhong ASC");

            // Lọc tiếp theo trạng thái (nếu có chọn)
            if (_filterTrangThai != -1)
            {
                rows = rows.Where(r =>
                {
                    int code = Convert.ToInt32(r["TrangThaiHienThi"]);
                    return code == _filterTrangThai;
                }).ToArray();
            }

            if (rows.Length == 0) return;

            // Vẽ Group và Item
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
                    item.Tag = row["MaPhong"]; // Lưu mã phòng vào Tag để dùng khi Click
                    group.Items.Add(item);
                }
            }
        }

        private void FormatTileItem_Final(TileItem item, DataRow row)
        {
            item.ItemSize = TileItemSize.Wide;
            item.Elements.Clear();

            string tenPhong = row["TenPhong"].ToString();
            string tenLP = row["TenLP"].ToString();
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
                case 4: // Chưa dọn
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

            // Thiết kế nội dung thẻ
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

        // --- 7. SỰ KIỆN CLICK VÀO PHÒNG ---
        private void TileControlPhong_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (e.Item.Tag == null) return;
                int maPhong = Convert.ToInt32(e.Item.Tag);

                // Lấy thông tin phòng hiện tại từ DataTable
                DataRow[] rows = _dtPhong.Select($"MaPhong = {maPhong}");
                if (rows.Length == 0) return;

                DataRow row = rows[0];
                string tenPhong = row["TenPhong"].ToString();
                int trangThai = Convert.ToInt32(row["TrangThaiHienThi"]);

                switch (trangThai)
                {
                    case 1: // TRỐNG
                    case 4: // DƠ (Vẫn cho mở để dọn)
                        // Giả định form frmChiTietPhong có constructor (int maNP, int maPhong)
                        frmChiTietPhong frmTrong = new frmChiTietPhong(0, maPhong);
                        frmTrong.Text = $"Quản lý phòng {tenPhong} - TRỐNG";
                        frmTrong.ShowDialog();
                        break;

                    case 2: // ĐANG Ở
                        // Lấy mã nhận phòng (MaNP)
                        int maNP = _bookingBLL.GetCurrentStayID(maPhong);
                        if (maNP > 0)
                        {
                            frmChiTietPhong frmDangO = new frmChiTietPhong(maNP, maPhong);
                            frmDangO.Text = $"Chi tiết phòng {tenPhong} - ĐANG CÓ KHÁCH";
                            frmDangO.ShowDialog();
                        }
                        else
                        {
                            XtraMessageBox.Show($"Lỗi: Không tìm thấy thông tin nhận phòng cho {tenPhong}.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;

                    case 3: // ĐẶT TRƯỚC
                        XtraMessageBox.Show($"Phòng {tenPhong} đã đặt trước. Vui lòng Check-in trong mục Đặt phòng.", "Thông báo");
                        break;

                    case 0: // BẢO TRÌ
                        if (XtraMessageBox.Show($"Phòng {tenPhong} đang bảo trì. Mở khóa về Trống?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            // --- ĐÃ SỬA: Gọi hàm cập nhật trạng thái ---
                            bool kq = _bll.CapNhatTrangThai(maPhong, 1); // 1 = Trống

                            if (kq)
                            {
                                XtraMessageBox.Show("Đã mở khóa phòng thành công!", "Thông báo");
                                // Đồng bộ dữ liệu sang các form khác (ví dụ frmDatPhong)
                                BookingBLL.NotifyDataChanged();
                                // Load lại giao diện hiện tại
                                LoadData();
                            }
                            else
                            {
                                XtraMessageBox.Show("Lỗi: Không thể cập nhật trạng thái.", "Lỗi");
                            }
                        }
                        break;
                }

                // Load lại dữ liệu sau khi đóng form chi tiết để cập nhật màu sắc
                LoadData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ucTrangChu_Load_1(object sender, EventArgs e)
        {

        }
    }
}