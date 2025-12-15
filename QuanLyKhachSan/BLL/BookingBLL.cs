using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.BLL
{
    public class BookingBLL
    {
        // 1. Lấy sơ đồ phòng với 3 trạng thái ưu tiên: 2 (Đỏ - Đang ở) -> 1 (Vàng - Đã đặt) -> 0 (Xanh - Trống)
        // (Giữ lại logic này vì nó xử lý màu sắc hiển thị trên giao diện tốt hơn)
        public DataTable GetSoDoPhong(DateTime tuNgay, DateTime denNgay)
        {
            string query = @"
                SELECT P.MaPhong, P.TenPhong, LP.TenLP, LP.GiaMacDinh,
                       CASE 
                           -- Ưu tiên 1: Đang có người ở (Trạng thái = Đang ở) -> MÀU ĐỎ (2)
                           WHEN EXISTS (
                               SELECT 1 FROM CHITIET_DATPHONG CD
                               JOIN DATPHONG DP ON CD.MaDP = DP.MaDP
                               WHERE CD.MaPhong = P.MaPhong
                               AND DP.TrangThai = N'Đang ở'
                               AND (CD.NgayNhanPhong < @DenNgay AND CD.NgayTraPhong > @TuNgay)
                           ) THEN 2 
                           
                           -- Ưu tiên 2: Đã được đặt trước (Trạng thái = Đã đặt) -> MÀU VÀNG (1)
                           WHEN EXISTS (
                               SELECT 1 FROM CHITIET_DATPHONG CD
                               JOIN DATPHONG DP ON CD.MaDP = DP.MaDP
                               WHERE CD.MaPhong = P.MaPhong
                               AND DP.TrangThai = N'Đã đặt'
                               AND (CD.NgayNhanPhong < @DenNgay AND CD.NgayTraPhong > @TuNgay)
                           ) THEN 1 
                           
                           -- Còn lại: Trống -> MÀU XANH (0)
                           ELSE 0 
                       END AS TrangThaiSo
                FROM PHONG P
                JOIN LOAIPHONG LP ON P.MaLP = LP.MaLP";

            SqlParameter[] para = {
                new SqlParameter("@TuNgay", tuNgay),
                new SqlParameter("@DenNgay", denNgay)
            };

            return DatabaseHelper.GetData(query, para, CommandType.Text);
        }

        // 2. Tìm thông tin khách hàng cơ bản (để điền vào form khi tìm kiếm)
        public DataRow GetKhachHangInfo(string keyword)
        {
            string query = "SELECT * FROM KHACHHANG WHERE CCCD = @key OR SDT = @key";
            SqlParameter[] para = { new SqlParameter("@key", keyword) };

            DataTable dt = DatabaseHelper.GetData(query, para, CommandType.Text);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            return null;
        }

        // 3. Lấy thông tin khách đang giữ phòng (Dùng khi Click vào phòng Vàng hoặc Đỏ)
        public DataRow GetKhachHangByRoom(int maPhong, DateTime tuNgay, DateTime denNgay)
        {
            string query = @"
                SELECT TOP 1 KH.* FROM KHACHHANG KH
                JOIN DATPHONG DP ON KH.MaKH = DP.MaKH
                JOIN CHITIET_DATPHONG CD ON DP.MaDP = CD.MaDP
                WHERE CD.MaPhong = @MaPhong
                AND DP.TrangThai IN (N'Đã đặt', N'Đang ở') -- Chỉ lấy trạng thái hoạt động
                AND (CD.NgayNhanPhong < @DenNgay AND CD.NgayTraPhong > @TuNgay)
                ORDER BY DP.NgayDat DESC"; // Lấy đơn mới nhất

            SqlParameter[] para = {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@TuNgay", tuNgay),
                new SqlParameter("@DenNgay", denNgay)
            };

            DataTable dt = DatabaseHelper.GetData(query, para, CommandType.Text);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            return null;
        }

        // 4. Lấy danh sách các phòng CHỜ CHECK-IN (Trạng thái 'Đã đặt') của khách
        public DataTable GetPhongDaDat(int maKH)
        {
            string query = @"
                SELECT DP.MaDP, CD.MaPhong, P.TenPhong, LP.TenLP, 
                       CD.NgayNhanPhong, CD.NgayTraPhong
                FROM DATPHONG DP
                JOIN CHITIET_DATPHONG CD ON DP.MaDP = CD.MaDP
                JOIN PHONG P ON CD.MaPhong = P.MaPhong
                JOIN LOAIPHONG LP ON P.MaLP = LP.MaLP
                WHERE DP.MaKH = @MaKH 
                AND DP.TrangThai = N'Đã đặt'"; // Chỉ lấy phòng CHƯA nhận (Màu Vàng)

            SqlParameter[] para = { new SqlParameter("@MaKH", maKH) };
            return DatabaseHelper.GetData(query, para, CommandType.Text);
        }

        // 5. Thêm khách hàng mới
        public string AddKhachHang(string ten, string cccd, string sdt, string gioitinh, DateTime ngaysinh, string quoctich)
        {
            if (string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(cccd)) return "Thiếu tên hoặc CCCD!";
            if (string.IsNullOrEmpty(quoctich)) quoctich = "Việt Nam";

            string proc = "sp_ThemKhachHang";
            SqlParameter[] para = {
                new SqlParameter("@HoTen", ten),
                new SqlParameter("@CCCD", cccd),
                new SqlParameter("@SDT", sdt),
                new SqlParameter("@GioiTinh", gioitinh),
                new SqlParameter("@NgaySinh", ngaysinh),
                new SqlParameter("@QuocTich", quoctich)
            };

            bool success = DatabaseHelper.ExecuteNonQuery(proc, para, CommandType.StoredProcedure);
            return success ? "Thêm khách thành công!" : "Lỗi: Có thể trùng CCCD.";
        }

        // 6. Đặt phòng (Tạo đơn 'Đã đặt' -> Màu Vàng)
        public string BookRoom(int maKH, int maPhong, DateTime den, DateTime di, decimal coc)
        {
            string proc = "sp_DatPhong";
            SqlParameter[] para = {
                new SqlParameter("@MaKH", maKH),
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@NgayDen", den),
                new SqlParameter("@NgayDi", di),
                new SqlParameter("@TienCoc", coc)
            };

            bool success = DatabaseHelper.ExecuteNonQuery(proc, para, CommandType.StoredProcedure);
            return success ? "Đặt phòng thành công!" : "Đặt thất bại.";
        }

        // 7. Check-in (Chuyển từ 'Đã đặt' sang 'Đang ở' -> Màu Đỏ)
        public string CheckIn(int maDP)
        {
            SqlParameter[] para = { new SqlParameter("@MaDP", maDP) };
            bool result = DatabaseHelper.ExecuteNonQuery("sp_CheckIn", para, CommandType.StoredProcedure);
            return result ? "Check-in thành công!" : "Lỗi Check-in.";
        }

        // 8. Đổi phòng khi Check-in
        public string DoiPhongCheckIn(int maDP, int maPhongCu, int maPhongMoi)
        {
            SqlParameter[] para = {
                new SqlParameter("@MaDP", maDP),
                new SqlParameter("@MaPhongCu", maPhongCu),
                new SqlParameter("@MaPhongMoi", maPhongMoi)
            };
            bool result = DatabaseHelper.ExecuteNonQuery("sp_DoiPhongKhiCheckIn", para, CommandType.StoredProcedure);
            return result ? "Đổi phòng thành công!" : "Lỗi: Phòng mới không khả dụng.";
        }

        // File: BLL/BookingBLL.cs

        public int GetCurrentStayID(int maPhong)
        {
            DatPhongDAL dal = new DatPhongDAL();
            return dal.LayMaNPDangO(maPhong);
        }

        private DatPhongDAL dal = new DatPhongDAL();

        // --- CÁC HÀM GET DỮ LIỆU ---
        public DataTable GetCheckOutInfo(int maNP)
        {
            return dal.LayThongTinCheckOut(maNP);
        }

        public DataTable GetServices(int maNP)
        {
            return dal.LayDichVuDaDung(maNP);
        }

        public DataTable GetSurcharges(int maNP)
        {
            return dal.LayPhuThuDaDung(maNP);
        }

        // --- LOGIC THANH TOÁN TRỌN GÓI ---
        // Hàm này sẽ gọi liên tiếp 3 hành động dưới Database
        public bool ThanhToanFull(int maNP, string tinhTrang, int kieuTT)
        {
            try
            {
                // Bước 1: Gọi DAL trả phòng (Cập nhật giờ ra, tình trạng phòng)
                bool ketQuaTra = dal.ThucHienTraPhong(maNP, tinhTrang, "Thanh toán tại quầy");

                if (ketQuaTra)
                {
                    // Bước 2: Lấy ID phiếu trả phòng vừa sinh ra
                    int maTraPhong = dal.LayMaTraPhongMoiNhat(maNP);

                    if (maTraPhong > 0)
                    {
                        // Bước 3: Tạo hóa đơn chốt sổ
                        dal.TaoHoaDon(maTraPhong, kieuTT);
                        return true; // Thành công mỹ mãn
                    }
                }
                return false; // Lỗi ở bước trả phòng hoặc không lấy được ID
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /////////////////////////TƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯTƯ
        /// </summary>
        /// <param name="maNP"></param>
        /// <returns></returns>
        // 9. Lấy thông tin khách và phòng đang ở
        public DataTable GetRoomAndGuestDetails(int maNP)
        {
            string query = @"
        SELECT KH.HoTen, P.TenPhong, LP.TenLP, NP.ThoiGianNhan 
        FROM NHANPHONG NP
        JOIN PHONG P ON NP.MaPhong = P.MaPhong
        JOIN LOAIPHONG LP ON P.MaLP = LP.MaLP
        JOIN DATPHONG DP ON NP.MaDP = DP.MaDP
        JOIN KHACHHANG KH ON DP.MaKH = KH.MaKH
        WHERE NP.MaNP = @MaNP";
            SqlParameter[] para = { new SqlParameter("@MaNP", maNP) };
            return DatabaseHelper.GetData(query, para, CommandType.Text);
        }

        // 10. Tải danh sách tất cả dịch vụ (Dùng cho ComboBox)
        public DataTable LoadAllDichVu()
        {
            // Sử dụng sp_loadDV có sẵn trong nhat.sql
            return DatabaseHelper.GetData("sp_loadDV", null, CommandType.StoredProcedure);
        }

        // 11. Ghi nhận thêm dịch vụ cho khách đang ở
        public bool ThemDichVuSuDung(int maNP, short maDV, short soLuong)
        {
            // Sử dụng sp_ThemDichVuSuDung có sẵn trong nhat.sql
            SqlParameter[] para = {
        new SqlParameter("@MaNP", maNP),
        new SqlParameter("@MaDV", maDV),
        new SqlParameter("@SoLuong", soLuong)
    };
            return DatabaseHelper.ExecuteNonQuery("sp_ThemDichVuSuDung", para, CommandType.StoredProcedure);
        }

        // 12. Tải lịch sử chi tiêu (Dịch vụ và Phụ thu)
        public DataTable LoadLichSuChiTieu(int maNP)
        {
            // Lấy chi tiết dịch vụ và phụ thu, hiển thị dạng thống nhất cho DataGridView
            string query = @"
        -- Dịch vụ
        SELECT  DV.TenDV AS DichVu, SD.SoLuong, DV.Gia, (SD.SoLuong * DV.Gia) AS ThanhTien, SD.NgaySuDung
        FROM SUDUNG_DICHVU SD
        JOIN DICHVU DV ON SD.MaDV = DV.MaDV
        WHERE SD.MaNP = @MaNP
        
        UNION ALL
        
        -- Phụ thu
        SELECT  PT.Ten AS DichVu, SP.SoLuong, SP.GiaHienTai AS Gia, (SP.SoLuong * SP.GiaHienTai) AS ThanhTien, CAST(SP.ThoiGianGhiNhan AS DATE)
        FROM SUDUNG_PHUTHU SP
        JOIN PHUTHU PT ON SP.MaPhuThu = PT.MaPhuThu
        WHERE SP.MaNP = @MaNP
        ORDER BY NgaySuDung DESC";
            SqlParameter[] para = { new SqlParameter("@MaNP", maNP) };
            return DatabaseHelper.GetData(query, para, CommandType.Text);
        }

        // 13. Tính tổng tiền tạm thời
        public decimal TinhTienTam(int maNP)
        {
            // Sử dụng sp_TinhTienTam có sẵn trong nhat.sql
            SqlParameter[] para = { new SqlParameter("@MaNP", maNP) };
            DataTable dt = DatabaseHelper.GetData("sp_TinhTienTam", para, CommandType.StoredProcedure);

            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["TongTienTamTinh"] != DBNull.Value)
            {
                return Convert.ToDecimal(dt.Rows[0]["TongTienTamTinh"]);
            }
            return 0;
        }

        // Trong class BookingBLL
        public DataTable GetDanhSachDonDat()
        {
            // Gọi Stored Procedure đã có sẵn trong database: sp_LayDanhSachDonDat_ChiTiet
            return DatabaseHelper.GetData("sp_LayDanhSachDonDat_ChiTiet", null, CommandType.StoredProcedure);
        }
    }
}