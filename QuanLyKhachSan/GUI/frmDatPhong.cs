using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Data.SqlClient; // Thư viện quan trọng để kết nối SQL
using System.Drawing; // Để đổi màu nút bấm
using System.Windows.Forms;

namespace QuanLyKhachSan.GUI
{
    public partial class frmDatPhong : DevExpress.XtraEditors.XtraForm
    {
        // CÁC BIẾN TOÀN CỤC
        private int _maPhongSelected = 0;      // Mã phòng đang thao tác
        private int _maDP_HienTai = 0;         // Mã đơn đặt phòng (nếu đang sửa/check-in đơn cũ)
        private int _trangThaiPhongHienTai = 1;// 1: Trống, 2: Đã đặt, 3: Đang ở

        public frmDatPhong(int maPhong = 0)
        {
            InitializeComponent();
            _maPhongSelected = maPhong;
        }

        // =================================================================================
        // PHẦN 1: KHỞI TẠO VÀ LOAD DỮ LIỆU
        // =================================================================================

        private void frmDatPhong_Load(object sender, EventArgs e)
        {
            // 1. Kiểm tra trạng thái phòng hiện tại từ Database
            CheckTrangThaiPhong();

            // 2. Cấu hình giao diện dựa trên trạng thái
            if (_trangThaiPhongHienTai == 2) // --- TRƯỜNG HỢP: PHÒNG ĐÃ ĐẶT (MÀU VÀNG) ---
            {
                this.Text = $"CHECK-IN CHO PHÒNG ĐÃ ĐẶT - MÃ SỐ: {_maPhongSelected}";

                // Đổi tên và màu sắc nút bấm
                btnDatTruoc.Text = "Check-In";     // Nút trái thành Check-In

                btnCheckIn.Text = "Hủy Đặt Phòng";      // Nút giữa thành Hủy
                btnCheckIn.Appearance.ForeColor = Color.Red;

                // Load dữ liệu đơn đặt trước đó lên form
                LoadThongTinDaDat();
            }
            else // --- TRƯỜNG HỢP: PHÒNG TRỐNG (MÀU XANH) ---
            {
                this.Text = $"ĐẶT PHÒNG MỚI - MÃ SỐ: {_maPhongSelected}";
                btnDatTruoc.Text = "Đặt trước";
                btnCheckIn.Text = "Check-In";

                // Thiết lập giá trị mặc định
                dtpNgayDen.DateTime = DateTime.Now;
                dtpNgayDi.DateTime = DateTime.Now.AddDays(1);
                dtpNgaySinh.DateTime = new DateTime(1990, 1, 1);
                if (cboGioiTinh.Properties.Items.Count > 0) cboGioiTinh.SelectedIndex = 0; // Mặc định Nam
            }
        }

        // Hàm kiểm tra phòng này đang Trống hay Đã đặt
        void CheckTrangThaiPhong()
        {
            if (_maPhongSelected <= 0) return;
            string sql = $"SELECT TrangThaiPhong FROM PHONG WHERE MaPhong = {_maPhongSelected}";
            DataTable dt = DatabaseHelper.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                _trangThaiPhongHienTai = Convert.ToInt32(dt.Rows[0][0]);
            }
        }

        // Hàm lấy thông tin đơn đặt phòng cũ (để Check-in hoặc Hủy)
        void LoadThongTinDaDat()
        {
            string sql = @"
                SELECT TOP 1 
                    DP.MaDP, DP.TienCoc, 
                    KH.MaKH, KH.HoTen, KH.SDT, KH.CCCD, KH.QuocTich, KH.GioiTinh, KH.NgaySinh,
                    CD.NgayNhanPhong, CD.NgayTraPhong, CD.SoNguoi
                FROM DATPHONG DP
                JOIN CHITIET_DATPHONG CD ON DP.MaDP = CD.MaDP
                JOIN KHACHHANG KH ON DP.MaKH = KH.MaKH
                WHERE CD.MaPhong = " + _maPhongSelected + " AND DP.TrangThai = N'Đã đặt'";

            DataTable dt = DatabaseHelper.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                _maDP_HienTai = Convert.ToInt32(r["MaDP"]); // Lưu MaDP quan trọng

                // Đổ dữ liệu lên giao diện
                txtHoTen.Text = r["HoTen"].ToString();
                txtSDT.Text = r["SDT"].ToString();
                txtCCCD.Text = r["CCCD"].ToString();
                txtQuocTich.Text = r["QuocTich"].ToString();
                cboGioiTinh.Text = r["GioiTinh"].ToString();
                txtTienCoc.Text = r["TienCoc"].ToString();
                txtSoNguoi.Text = r["SoNguoi"].ToString();

                if (r["NgaySinh"] != DBNull.Value) dtpNgaySinh.DateTime = Convert.ToDateTime(r["NgaySinh"]);
                dtpNgayDen.DateTime = Convert.ToDateTime(r["NgayNhanPhong"]);
                dtpNgayDi.DateTime = Convert.ToDateTime(r["NgayTraPhong"]);

                // Lưu ID khách để không tạo mới nếu không cần thiết
                txtHoTen.Tag = r["MaKH"];
            }
        }

        // =================================================================================
        // PHẦN 2: CÁC HÀM XỬ LÝ NGHIỆP VỤ (LOGIC)
        // =================================================================================

        // Hàm kiểm tra nhập liệu chung
        private bool KiemTraNhapLieu()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                XtraMessageBox.Show("Vui lòng nhập Họ tên khách hàng!", "Cảnh báo"); txtHoTen.Focus(); return false;
            }
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                XtraMessageBox.Show("Vui lòng nhập SĐT!", "Cảnh báo"); txtSDT.Focus(); return false;
            }
            if (string.IsNullOrWhiteSpace(txtCCCD.Text))
            {
                XtraMessageBox.Show("Vui lòng nhập CCCD!", "Cảnh báo"); txtCCCD.Focus(); return false;
            }
            if (string.IsNullOrEmpty(cboGioiTinh.Text))
            {
                XtraMessageBox.Show("Vui lòng chọn Giới tính!", "Cảnh báo"); cboGioiTinh.ShowPopup(); return false;
            }
            if (dtpNgayDen.DateTime >= dtpNgayDi.DateTime)
            {
                XtraMessageBox.Show("Ngày đi phải sau ngày đến!", "Lỗi ngày tháng"); return false;
            }
            return true;
        }

        // --- LOGIC 1: ĐẶT MỚI (Cho phòng Trống) ---
        private void XuLyDatPhongMoi(bool isCheckIn)
        {
            if (!KiemTraNhapLieu()) return;

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = trans;

                    // 1. Xử lý Khách hàng (Thêm mới hoặc Lấy cũ)
                    object maKH = txtHoTen.Tag;
                    if (maKH == null)
                    {
                        cmd.CommandText = @"INSERT INTO KHACHHANG(HoTen, SDT, CCCD, QuocTich, NgaySinh, GioiTinh) 
                                            VALUES (@HoTen, @SDT, @CCCD, @QuocTich, @NgaySinh, @GioiTinh); SELECT SCOPE_IDENTITY();";
                        cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                        cmd.Parameters.AddWithValue("@SDT", txtSDT.Text);
                        cmd.Parameters.AddWithValue("@CCCD", txtCCCD.Text);
                        cmd.Parameters.AddWithValue("@QuocTich", txtQuocTich.Text);
                        cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.DateTime);
                        cmd.Parameters.AddWithValue("@GioiTinh", cboGioiTinh.Text);
                        maKH = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                    }

                    // 2. Insert DATPHONG
                    string trangThaiDon = isCheckIn ? "Đang ở" : "Đã đặt";
                    decimal tienCoc = 0; decimal.TryParse(txtTienCoc.Text, out tienCoc);

                    cmd.CommandText = @"INSERT INTO DATPHONG(MaKH, NgayDat, TrangThai, TienCoc) 
                                        VALUES (@MaKH, GETDATE(), @TrangThai, @TienCoc); SELECT SCOPE_IDENTITY();";
                    cmd.Parameters.AddWithValue("@MaKH", maKH);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThaiDon);
                    cmd.Parameters.AddWithValue("@TienCoc", tienCoc);
                    int maDP = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Parameters.Clear();

                    // 3. Insert CHITIET
                    int soNguoi = 1; int.TryParse(txtSoNguoi.Text, out soNguoi);
                    cmd.CommandText = @"INSERT INTO CHITIET_DATPHONG(MaDP, MaPhong, SoNguoi, NgayNhanPhong, NgayTraPhong) 
                                        VALUES (@MaDP, @MaPhong, @SoNguoi, @NgayDen, @NgayDi)";
                    cmd.Parameters.AddWithValue("@MaDP", maDP);
                    cmd.Parameters.AddWithValue("@MaPhong", _maPhongSelected);
                    cmd.Parameters.AddWithValue("@SoNguoi", soNguoi);
                    cmd.Parameters.AddWithValue("@NgayDen", dtpNgayDen.DateTime);
                    cmd.Parameters.AddWithValue("@NgayDi", dtpNgayDi.DateTime);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    // --- D. INSERT BẢNG NHẬN PHÒNG (Chỉ khi Check-in) ---
                    if (isCheckIn)
                    {
                        // CÂU LỆNH CŨ (BỊ LỖI):
                        // string sqlNP = "INSERT INTO NHANPHONG(MaDP, ThoiGianNhan, SoKhach) VALUES (@MaDP, GETDATE(), @SoKhach)";

                        // CÂU LỆNH MỚI (ĐÃ SỬA): Thêm cột ThoiGianTra và tham số @NgayDi
                        string sqlNP = @"INSERT INTO NHANPHONG(MaDP, ThoiGianNhan, ThoiGianTra, SoKhach) 
                     VALUES (@MaDP, GETDATE(), @NgayDi, @SoKhach)";

                        cmd.CommandText = sqlNP;
                        cmd.Parameters.AddWithValue("@MaDP", maDP);
                        cmd.Parameters.AddWithValue("@SoKhach", soNguoi);

                        // Thêm dòng này để truyền ngày trả dự kiến vào
                        cmd.Parameters.AddWithValue("@NgayDi", dtpNgayDi.DateTime);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    // 5. Update PHONG (Check-in -> 3, Đặt trước -> 2)
                    int trangThaiMoi = isCheckIn ? 3 : 2;
                    cmd.CommandText = $"UPDATE PHONG SET TrangThaiPhong = {trangThaiMoi} WHERE MaPhong = {_maPhongSelected}";
                    cmd.ExecuteNonQuery();

                    trans.Commit();
                    XtraMessageBox.Show(isCheckIn ? "Check-In thành công!" : "Đặt trước thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    XtraMessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- LOGIC 2: CHECK-IN TỪ ĐƠN ĐẶT TRƯỚC (Phòng Vàng -> Đỏ) ---
        private void XuLyCheckInTuDatTruoc()
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = trans;

                    // Update DATPHONG: 'Đã đặt' -> 'Đang ở'
                    cmd.CommandText = $"UPDATE DATPHONG SET TrangThai = N'Đang ở' WHERE MaDP = {_maDP_HienTai}";
                    cmd.ExecuteNonQuery();

                    // Insert NHANPHONG
                    // === ĐOẠN ĐÃ SỬA: Thêm ThoiGianTra và @NgayDi ===
                    int soNguoi = 1; int.TryParse(txtSoNguoi.Text, out soNguoi);
                    cmd.CommandText = "INSERT INTO NHANPHONG(MaDP, ThoiGianNhan, ThoiGianTra, SoKhach) VALUES (@MaDP, GETDATE(), @NgayDi, @SoKhach)";
                    cmd.Parameters.AddWithValue("@MaDP", _maDP_HienTai);
                    cmd.Parameters.AddWithValue("@SoKhach", soNguoi);
                    cmd.Parameters.AddWithValue("@NgayDi", dtpNgayDi.DateTime); // Lấy ngày trả dự kiến
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear(); // Xóa tham số để an toàn cho lệnh sau
                    // ===============================================

                    // Update PHONG: 2 -> 3
                    cmd.CommandText = $"UPDATE PHONG SET TrangThaiPhong = 3 WHERE MaPhong = {_maPhongSelected}";
                    cmd.ExecuteNonQuery();

                    trans.Commit();
                    XtraMessageBox.Show("Check-In thành công! Phòng đã chuyển trạng thái.", "Thông báo");
                    this.Close();
                }
                catch (Exception ex) { trans.Rollback(); XtraMessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        // --- LOGIC 3: HỦY ĐẶT PHÒNG (Phòng Vàng -> Xanh) ---
        private void XuLyHuyDatPhong()
        {
            if (XtraMessageBox.Show("Bạn chắc chắn muốn HỦY đơn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = trans;

                    // Update DATPHONG: 'Đã hủy'
                    cmd.CommandText = $"UPDATE DATPHONG SET TrangThai = N'Đã hủy' WHERE MaDP = {_maDP_HienTai}";
                    cmd.ExecuteNonQuery();

                    // Update PHONG: 2 -> 1
                    cmd.CommandText = $"UPDATE PHONG SET TrangThaiPhong = 1 WHERE MaPhong = {_maPhongSelected}";
                    cmd.ExecuteNonQuery();

                    trans.Commit();
                    XtraMessageBox.Show("Đã hủy đơn! Phòng đã trống.", "Thông báo");
                    this.Close();
                }
                catch (Exception ex) { trans.Rollback(); XtraMessageBox.Show("Lỗi: " + ex.Message); }
            }
        }


        // =================================================================================
        // PHẦN 3: XỬ LÝ SỰ KIỆN NÚT BẤM (BUTTON CLICK)
        // =================================================================================

        private void btnDatTruoc_Click(object sender, EventArgs e)
        {
            if (_trangThaiPhongHienTai == 2)
            {
                // Phòng Đã đặt (Vàng) -> Nút này là "Check-In"
                XuLyCheckInTuDatTruoc();
            }
            else
            {
                // Phòng Trống (Xanh) -> Nút này là "Đặt trước"
                XuLyDatPhongMoi(false);
            }
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            if (_trangThaiPhongHienTai == 2)
            {
                // Phòng Đã đặt (Vàng) -> Nút này là "Hủy"
                XuLyHuyDatPhong();
            }
            else
            {
                // Phòng Trống (Xanh) -> Nút này là "Check-In Ngay"
                XuLyDatPhongMoi(true);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // =================================================================================
        // PHẦN 4: TÍNH NĂNG PHỤ - TỰ ĐIỀN THÔNG TIN KHÁCH
        // =================================================================================

        private void txtSDT_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSDT.Text)) return;

            string sql = $"SELECT * FROM KHACHHANG WHERE SDT = '{txtSDT.Text}'";
            DataTable dt = DatabaseHelper.GetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                txtHoTen.Text = r["HoTen"].ToString();
                txtCCCD.Text = r["CCCD"].ToString();
                txtQuocTich.Text = r["QuocTich"].ToString();
                cboGioiTinh.Text = r["GioiTinh"].ToString();

                if (r["NgaySinh"] != DBNull.Value) dtpNgaySinh.DateTime = Convert.ToDateTime(r["NgaySinh"]);

                // Lưu ID khách cũ để dùng lại
                txtHoTen.Tag = r["MaKH"];
            }
            else
            {
                // Khách mới -> Xóa Tag
                txtHoTen.Tag = null;
            }
        }
    }
}