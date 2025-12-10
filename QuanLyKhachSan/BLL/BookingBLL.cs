using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.BLL
{
    public class BookingBLL
    {
        public DataTable FindRooms(DateTime ngayDen, DateTime ngayDi)
        {
            SqlParameter[] para = {
                new SqlParameter("@NgayDen", ngayDen),
                new SqlParameter("@NgayDi", ngayDi)
            };
            return DatabaseHelper.GetData("sp_TimPhongTrong", para, CommandType.StoredProcedure);
        }

        public DataRow GetKhachHangInfo(string keyword)
        {
            string query = "SELECT * FROM KHACHHANG WHERE CCCD = @key OR SDT = @key";
            SqlParameter[] para = { new SqlParameter("@key", keyword) };

            DataTable dt = DatabaseHelper.GetData(query, para, CommandType.Text);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            return null;
        }
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
            return success ? "Đặt phòng thành công!" : "Đặt thất bại (Lỗi hệ thống).";
        }
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
        AND DP.TrangThai = N'Đã đặt'"; // Quan trọng: Phải đúng chữ có dấu

            SqlParameter[] para = { new SqlParameter("@MaKH", maKH) };
            return DatabaseHelper.GetData(query, para, CommandType.Text);
        }
        // 2. Chức năng Check-in (Nhận phòng)
        public string CheckIn(int maDP)
        {
            // Gọi Procedure sp_CheckIn
            SqlParameter[] para = { new SqlParameter("@MaDP", maDP) };
            bool result = DatabaseHelper.ExecuteNonQuery("sp_CheckIn", para, CommandType.StoredProcedure);
            return result ? "Check-in thành công! Phòng đã chuyển sang màu Đỏ." : "Lỗi Check-in.";
        }

        // 3. Chức năng Đổi phòng (Khi khách muốn đổi ý sang phòng khác)
        public string DoiPhongCheckIn(int maDP, int maPhongCu, int maPhongMoi)
        {
            // Gọi Procedure sp_DoiPhongKhiCheckIn
            SqlParameter[] para = {
        new SqlParameter("@MaDP", maDP),
        new SqlParameter("@MaPhongCu", maPhongCu),
        new SqlParameter("@MaPhongMoi", maPhongMoi)
    };
            bool result = DatabaseHelper.ExecuteNonQuery("sp_DoiPhongKhiCheckIn", para, CommandType.StoredProcedure);
            return result ? "Đổi phòng thành công!" : "Lỗi: Phòng mới có thể đã bị trùng lịch.";
       
        
        }

        // --- Dán vào BookingBLL.cs ---

        public DataTable GetSoDoPhong(DateTime tuNgay, DateTime denNgay)
        {
            string query = @"
        SELECT P.MaPhong, P.TenPhong, LP.TenLP, LP.GiaMacDinh,
               CASE 
                   WHEN EXISTS (
                       SELECT 1 FROM CHITIET_DATPHONG CD
                       JOIN DATPHONG DP ON CD.MaDP = DP.MaDP
                       WHERE CD.MaPhong = P.MaPhong
                       AND DP.TrangThai != N'Đã hủy'
                       AND (CD.NgayNhanPhong < @DenNgay AND CD.NgayTraPhong > @TuNgay)
                   ) THEN 1 
                   ELSE 0 
               END AS TrangThaiBan
        FROM PHONG P
        JOIN LOAIPHONG LP ON P.MaLP = LP.MaLP";

            SqlParameter[] para = {
        new SqlParameter("@TuNgay", tuNgay),
        new SqlParameter("@DenNgay", denNgay)
    };

            return DatabaseHelper.GetData(query, para, CommandType.Text);
        }
    }
}