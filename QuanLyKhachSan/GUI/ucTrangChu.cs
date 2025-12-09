using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyKhachSan.BLL; // Đảm bảo bạn đã có namespace này
using QuanLyKhachSan.DTO; // Đảm bảo bạn đã có namespace này

namespace QuanLyKhachSan.GUI
{
    public partial class ucTrangChu : XtraUserControl
    {
        // Khai báo các biến toàn cục
        private PhongBLL _bll;
        private DataTable _dtPhong;
        private int _filterTang = 0; // 0 là hiển thị Tất cả

        public ucTrangChu()
        {
            InitializeComponent();

            // Khởi tạo BLL
            _bll = new PhongBLL();

            // Cấu hình giao diện TileControl (Sơ đồ phòng)
            CaiDatGiaoDienTile();

            // QUAN TRỌNG: Đăng ký sự kiện Load để tải dữ liệu khi mở form
            this.Load += UcTrangChu_Load;
        }

        private void CaiDatGiaoDienTile()
        {
            tileControlPhong.AllowDrag = false;
            tileControlPhong.Orientation = Orientation.Vertical; // Cuộn dọc
            tileControlPhong.ScrollMode = TileControlScrollMode.ScrollBar;
            tileControlPhong.VerticalContentAlignment = DevExpress.Utils.VertAlignment.Top;
        }

        // Sự kiện Load của UserControl
        private void UcTrangChu_Load(object sender, EventArgs e)
        {
            // Kiểm tra DesignMode: Không tải dữ liệu khi đang thiết kế trong Visual Studio
            if (!this.DesignMode)
            {
                LoadData();
            }
        }

        // Hàm tải dữ liệu chính (Public để FormMain có thể gọi khi cần làm mới)
        public void LoadData()
        {
            try
            {
                // 1. Lấy dữ liệu từ Database
                _dtPhong = _bll.LayTatCaPhong();

                // Kiểm tra dữ liệu rỗng
                if (_dtPhong == null || _dtPhong.Rows.Count == 0)
                {
                    // Nếu muốn thông báo thì mở comment dòng dưới
                    // XtraMessageBox.Show("Chưa có dữ liệu phòng nào trong hệ thống!");
                    return;
                }

                // 2. Gọi các hàm hiển thị
                HienThiThongKe();   // Vẽ thanh thống kê màu sắc
                TaoBoLocTang();     // Vẽ các nút lọc tầng
                VeSoDoPhong();      // Vẽ các ô phòng
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi tải dữ liệu trang chủ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- PHẦN 1: HEADER THỐNG KÊ (Bảo trì, Đang ở, Trống...) ---
        private void HienThiThongKe()
        {
            pnlThongKe.Controls.Clear();

            // Tính toán số lượng từ dữ liệu đã lấy
            RoomStatistics stats = _bll.TinhThongKe(_dtPhong);

            // Lưu ý: Dock=Left nên cái nào Add sau sẽ nằm bên phải cái Add trước
            // Hoặc Add ngược thứ tự nếu muốn kiểm soát vị trí chính xác

            AddStatLabel("TẤT CẢ: " + stats.TongSo, Color.Black);
            AddStatLabel("TRỐNG: " + stats.Trong, Color.SeaGreen);
            AddStatLabel("ĐANG Ở: " + stats.DangO, Color.Firebrick);
            AddStatLabel("ĐẶT TRƯỚC: " + stats.DatTruoc, Color.Goldenrod);
            AddStatLabel("BẨN: " + stats.Ban, Color.Gray);
            AddStatLabel("BẢO TRÌ: " + stats.BaoTri, Color.DimGray);
        }

        private void AddStatLabel(string text, Color color)
        {
            LabelControl lbl = new LabelControl();
            lbl.Text = text;
            lbl.Appearance.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lbl.Appearance.ForeColor = color;

            // Padding(Left, Top, Right, Bottom) để căn chỉnh khoảng cách
            lbl.Padding = new Padding(0, 12, 20, 10);
            lbl.Dock = DockStyle.Left;

            pnlThongKe.Controls.Add(lbl);
            lbl.BringToFront(); // Đẩy sang trái cùng trong Dock stack
        }

        // --- PHẦN 2: BỘ LỌC TẦNG (Tất cả, Tầng 1, Tầng 2...) ---
        private void TaoBoLocTang()
        {
            pnlBoLoc.Controls.Clear();

            // Nút "Tất cả" mặc định
            AddFilterButton("Tất cả", 0);

            // Lấy danh sách các tầng duy nhất
            DataTable dtTang = _bll.LayDanhSachTang(_dtPhong);
            if (dtTang != null)
            {
                foreach (DataRow row in dtTang.Rows)
                {
                    if (row["Tang"] != DBNull.Value)
                    {
                        int tang = Convert.ToInt32(row["Tang"]);
                        AddFilterButton("Tầng " + tang, tang);
                    }
                }
            }
        }

        private void AddFilterButton(string text, int tagValue)
        {
            SimpleButton btn = new SimpleButton();
            btn.Text = text;
            btn.Tag = tagValue;

            // Xử lý sự kiện click lọc
            btn.Click += (s, e) =>
            {
                _filterTang = Convert.ToInt32((s as SimpleButton).Tag);
                VeSoDoPhong(); // Vẽ lại sơ đồ theo bộ lọc mới
            };

            btn.Dock = DockStyle.Left;
            btn.Width = 80;
            btn.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light; // Style phẳng
            btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Regular);

            // Highlight nút đang chọn (cơ bản)
            if (_filterTang == tagValue)
            {
                btn.Appearance.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Appearance.ForeColor = Color.Blue;
            }

            pnlBoLoc.Controls.Add(btn);
            btn.BringToFront();
        }

        // --- PHẦN 3: VẼ SƠ ĐỒ PHÒNG (TileControl) ---
        private void VeSoDoPhong()
        {
            // Xóa các nhóm cũ
            tileControlPhong.Groups.Clear();

            // Lọc dữ liệu theo tầng đang chọn
            DataRow[] rows = _bll.LocPhongTheoTang(_dtPhong, _filterTang);
            if (rows.Length == 0) return;

            // Lấy danh sách tầng có trong dữ liệu lọc để tạo Group
            var distinctTang = rows.Select(r => r["Tang"]).Distinct().OrderBy(t => t);

            foreach (var tang in distinctTang)
            {
                TileGroup group = new TileGroup();
                group.Text = "TẦNG " + tang; // Tiêu đề nhóm (VD: TẦNG 1)
                tileControlPhong.Groups.Add(group);

                // Lấy các phòng thuộc tầng này
                var phongInGroup = rows.Where(r => r["Tang"].ToString() == tang.ToString());

                foreach (DataRow row in phongInGroup)
                {
                    TileItem item = new TileItem();

                    // Định dạng hiển thị (Màu sắc, Text)
                    FormatTileItem(
                        item,
                        row["TenPhong"].ToString(),
                        row["TenLP"].ToString(), // Tên loại phòng
                        row["TrangThaiO"].ToString(), // "Trống", "Đang ở"...
                        Convert.ToInt32(row["TrangThaiPhong"]) // Mã trạng thái 0,1,2...
                    );

                    // Lưu MaPhong vào Tag để xử lý khi click
                    item.Tag = row["MaPhong"];
                    item.ItemClick += Item_ItemClick;

                    group.Items.Add(item);
                }
            }
        }

        private void FormatTileItem(TileItem item, string tenPhong, string loaiPhong, string trangThaiO, int trangThaiCode)
        {
            item.ItemSize = TileItemSize.Wide; // Kích thước chữ nhật
            item.Elements.Clear();

            // 1. Element Tên phòng (Góc trái trên)
            TileItemElement tenEl = new TileItemElement();
            tenEl.Text = tenPhong;
            tenEl.TextAlignment = TileItemContentAlignment.TopLeft;
            tenEl.Appearance.Normal.Font = new Font("Segoe UI", 14, FontStyle.Bold);

            // 2. Element Loại phòng (Góc phải trên)
            TileItemElement loaiEl = new TileItemElement();
            loaiEl.Text = loaiPhong;
            loaiEl.TextAlignment = TileItemContentAlignment.TopRight;
            loaiEl.Appearance.Normal.FontSizeDelta = -1;

            // 3. Element Trạng thái (Góc trái dưới)
            TileItemElement statusEl = new TileItemElement();
            statusEl.TextAlignment = TileItemContentAlignment.BottomLeft;

            Color backColor;
            string statusText;

            // Logic màu sắc
            // Giả định: 0=Bảo trì, 1=Sẵn sàng, 2=Bẩn
            // Kết hợp với cột TrangThaiO (Đang ở, Đặt trước...)

            if (trangThaiCode == 0) // Bảo trì
            {
                backColor = Color.DimGray;
                statusText = "BẢO TRÌ";
            }
            else if (trangThaiCode == 2) // Bẩn
            {
                backColor = Color.Gray; // Hoặc màu nâu nhạt
                statusText = "CHƯA DỌN";
            }
            else // Sẵn sàng đón khách hoặc đang có khách
            {
                if (trangThaiO == "Đang ở")
                {
                    backColor = Color.Firebrick; // Đỏ
                    statusText = "ĐANG Ở";
                }
                else if (trangThaiO == "Đặt trước")
                {
                    backColor = Color.Goldenrod; // Vàng cam
                    statusText = "ĐẶT TRƯỚC";
                }
                else
                {
                    backColor = Color.SeaGreen; // Xanh lá
                    statusText = "TRỐNG";
                }
            }

            item.AppearanceItem.Normal.BackColor = backColor;
            item.AppearanceItem.Normal.BorderColor = Color.White; // Viền trắng cho nổi
            statusEl.Text = statusText;

            item.Elements.Add(tenEl);
            item.Elements.Add(loaiEl);
            item.Elements.Add(statusEl);
        }

        // Sự kiện khi click vào một phòng
        private void Item_ItemClick(object sender, TileItemEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                int maPhong = Convert.ToInt32(e.Item.Tag);

                //Demo xử lý
                //XtraMessageBox.Show("Bạn đang chọn phòng có ID: " + maPhong);

                // TODO: Mở form chi tiết phòng / Check-in / Check-out
                //frmDatPhong frm = new frmDatPhong();
                //frm.ShowDialog();
                //LoadData(); // Load lại dữ liệu sau khi đóng form chi tiết
            }
        }
    }
}