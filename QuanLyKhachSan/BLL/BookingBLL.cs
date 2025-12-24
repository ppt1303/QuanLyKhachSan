using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.BLL
{
    public class BookingBLL
    {
        public static event Action OnDataChanged;
        private DatPhongDAL dal = new DatPhongDAL();

        // Hàm dùng để kích hoạt sự kiện khi có thay đổi dữ liệu
        public static void NotifyDataChanged()
        {
            OnDataChanged?.Invoke();
        }

        // --- 1. LẤY SƠ ĐỒ PHÒNG (Sử dụng sp_GetSoDoPhong) ---
        public DataTable GetSoDoPhong(DateTime tuNgay, DateTime denNgay)
        {
            SqlParameter[] para = {
                new SqlParameter("@TuNgay", tuNgay),
                new SqlParameter("@DenNgay", denNgay)
            };

            // Chuyển sang dùng Stored Procedure
            return DatabaseHelper.GetData("sp_GetSoDoPhong", para, CommandType.StoredProcedure);
        }

        // --- 2. TÌM THÔNG TIN KHÁCH HÀNG (Sử dụng sp_GetKhachHangInfo) ---
        public DataRow GetKhachHangInfo(string keyword)
        {
            // Lưu ý: Tên tham số @Key phải khớp chính xác với trong Stored Procedure
            SqlParameter[] para = { new SqlParameter("@Key", keyword) };

            DataTable dt = DatabaseHelper.GetData("sp_GetKhachHangInfo", para, CommandType.StoredProcedure);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            return null;
        }

        // --- 3. LẤY KHÁCH ĐANG GIỮ PHÒNG (Sử dụng sp_GetKhachHangByRoom) ---
        public DataRow GetKhachHangByRoom(int maPhong, DateTime tuNgay, DateTime denNgay)
        {
            SqlParameter[] para = {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@TuNgay", tuNgay),
                new SqlParameter("@DenNgay", denNgay)
            };

            DataTable dt = DatabaseHelper.GetData("sp_GetKhachHangByRoom", para, CommandType.StoredProcedure);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            return null;
        }

        // --- 4. LẤY DANH SÁCH PHÒNG ĐÃ ĐẶT (Sử dụng sp_GetPhongDaDat) ---
        public DataTable GetPhongDaDat(int maKH)
        {
            SqlParameter[] para = { new SqlParameter("@MaKH", maKH) };

            return DatabaseHelper.GetData("sp_GetPhongDaDat", para, CommandType.StoredProcedure);
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

        // 6. Đặt phòng (Tạo đơn 'Đã đặt')
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

        // 7. Check-in
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

        public int GetCurrentStayID(int maPhong)
        {
            return dal.LayMaNPDangO(maPhong);
        }

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

        public bool ThanhToanFull(int maNP, string tinhTrang, int kieuTT)
        {
            try
            {
                bool ketQuaTra = dal.ThucHienTraPhong(maNP, tinhTrang, "Thanh toán tại quầy");
                if (ketQuaTra)
                {
                    int maTraPhong = dal.LayMaTraPhongMoiNhat(maNP);
                    if (maTraPhong > 0)
                    {
                        dal.TaoHoaDon(maTraPhong, kieuTT);
                        return true;
                    }
                }
                return false;
            }
            catch { return false; }
        }

        public DataTable GetRoomAndGuestDetails(int maNP)
        {
            string query = "EXEC sp_LayThongTinPhongVaKhach @MaNP";

            SqlParameter[] para = {
                new SqlParameter("@MaNP", maNP)
            };

            return DatabaseHelper.GetData(query, para);
        }

        public DataTable LoadAllDichVu()
        {
            return DatabaseHelper.GetData("sp_loadDV", null, CommandType.StoredProcedure);
        }

        public bool ThemDichVuSuDung(int maNP, short maDV, short soLuong)
        {
            SqlParameter[] para = {
                new SqlParameter("@MaNP", maNP),
                new SqlParameter("@MaDV", maDV),
                new SqlParameter("@SoLuong", soLuong)
            };
            return DatabaseHelper.ExecuteNonQuery("sp_ThemDichVuSuDung", para, CommandType.StoredProcedure);
        }

        public DataTable LoadLichSuChiTieu(int maNP)
        {
            string query = "EXEC sp_LoadLichSuChiTieu @MaNP";

            SqlParameter[] para = {
                new SqlParameter("@MaNP", maNP)
            };

            return DatabaseHelper.GetData(query, para);
        }

        public decimal TinhTienTam(int maNP)
        {
            SqlParameter[] para = { new SqlParameter("@MaNP", maNP) };
            DataTable dt = DatabaseHelper.GetData("sp_TinhTienTam", para, CommandType.StoredProcedure);

            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
            {
                return Convert.ToDecimal(dt.Rows[0][0]);
            }
            return 0;
        }

        public DataTable GetDanhSachDonDat()
        {
            return DatabaseHelper.GetData("sp_LayDanhSachDonDat_ChiTiet", null, CommandType.StoredProcedure);
        }
    }
}