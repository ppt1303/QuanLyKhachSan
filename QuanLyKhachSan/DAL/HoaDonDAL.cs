using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class HoaDonDAL
    {
        // 1. Lấy danh sách hóa đơn (Để hiện lên lưới tìm kiếm)
        public DataTable LayDanhSachHoaDon(DateTime tuNgay, DateTime denNgay)
        {
            string query = @"
                SELECT 
                    HD.MaHD AS [Mã HĐ],
                    P.TenPhong AS [Phòng],
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
                LEFT JOIN CHITIET_HOADON CT ON HD.MaHD = CT.MaHD
                WHERE HD.NgayLap BETWEEN @TuNgay AND @DenNgay
                GROUP BY HD.MaHD, P.TenPhong, KH.HoTen, HD.NgayLap, HD.MaPTTT
                ORDER BY HD.NgayLap DESC";

            SqlParameter[] param = {
                new SqlParameter("@TuNgay", tuNgay.Date),
                new SqlParameter("@DenNgay", denNgay.Date.AddDays(1).AddSeconds(-1))
            };

            return DatabaseHelper.GetData(query, param);
        }

        // 2. Lấy chi tiết hóa đơn (Để IN ra giấy)
        public DataTable LayChiTietInHoaDon(int maHD)
        {
            // Gọi Stored Procedure bạn đã sửa trong SQL
            string spName = "sp_LayChiTietHoaDon";
            SqlParameter[] param = { new SqlParameter("@MaHD", maHD) };

            return DatabaseHelper.GetData(spName, param, CommandType.StoredProcedure);
        }

        public int LayMaHDMoiNhat(int maNP)
        {
            // Tìm hóa đơn có ngày lập mới nhất của Mã Nhận Phòng này
            string query = @"
        SELECT TOP 1 MaHD 
        FROM HOADON HD 
        JOIN TRAPHONG TP ON HD.MaTraPhong = TP.MaTraPhong 
        WHERE TP.MaNP = @MaNP 
        ORDER BY HD.MaHD DESC"; // Lấy cái mới nhất

            SqlParameter[] param = { new SqlParameter("@MaNP", maNP) };

            DataTable dt = DatabaseHelper.GetData(query, param);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["MaHD"]);
            return 0;
        }
    }
}