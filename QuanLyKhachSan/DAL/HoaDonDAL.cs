using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class HoaDonDAL
    {
        public DataTable LayDanhSachHoaDon(DateTime tuNgay, DateTime denNgay)
        {
            string query = @"
                SELECT 
                    HD.MaHD AS [Mã HĐ],
                    P.TenPhong [Tên phòng],
                    KH.HoTen AS [Khách Hàng],
                    HD.NgayLap AS [Ngày Lập],
                    SUM(CT.SoTien) AS [Tổng Tiền],
                    CASE WHEN HD.MaPTTT = 1 THEN N'Tiền mặt' ELSE N'Chuyển khoản' END AS [Hình Thức]
                FROM HOADON HD
                JOIN TRAPHONG TP ON HD.MaTraPhong = TP.MaTraPhong
                JOIN NHANPHONG NP ON TP.MaNP = NP.MaNP
                JOIN PHONG P ON NP.MaPhong = P.MaPhong
                JOIN DATPHONG DP ON NP.MaDP = DP.MaDP
                JOIN KHACHHANG KH ON DP.MaKH = KH.MaKH
                JOIN CHITIET_HOADON CT ON HD.MaHD = CT.MaHD
                WHERE HD.NgayLap BETWEEN @TuNgay AND @DenNgay
                GROUP BY HD.MaHD, P.TenPhong, KH.HoTen, HD.NgayLap, HD.MaPTTT
                ORDER BY HD.NgayLap DESC";

            SqlParameter[] param = {
                new SqlParameter("@TuNgay", tuNgay.Date), // Lấy đầu ngày
                new SqlParameter("@DenNgay", denNgay.Date.AddDays(1).AddSeconds(-1)) // Lấy cuối ngày
            };

            return DatabaseHelper.GetData(query, param);
        }

        // Lấy chi tiết các mục trong hóa đơn (Phòng, DV, Phụ thu...)
        public DataTable LayChiTietHoaDon(int maHD)
        {
            // Gọi Stored Procedure có sẵn trong SQL
            string spName = "sp_LayChiTietHoaDon";

            SqlParameter[] param = {
        new SqlParameter("@MaHD", maHD)
    };

            return DatabaseHelper.GetData(spName, param, CommandType.StoredProcedure);
        }
    }
}