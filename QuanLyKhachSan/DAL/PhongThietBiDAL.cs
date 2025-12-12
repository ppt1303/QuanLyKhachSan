using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class PhongThietBiDAL
    {
        public DataTable TimKiemThietBiPhong(int maPhong, int maTB)
        {
            string query = @"
                SELECT 
                    PT.MaPhong,
                    P.TenPhong AS [Tên Phòng],
                    PT.MaTB,
                    TB.TenTB AS [Tên Thiết Bị],
                    PT.SoLuong AS [Số Lượng],
                    TB.MoTa AS [Mô Tả]
                FROM PHONG_THIETBI PT
                JOIN THIETBI TB ON PT.MaTB = TB.MaTB
                JOIN PHONG P ON PT.MaPhong = P.MaPhong
                WHERE (@MaPhong = -1 OR PT.MaPhong = @MaPhong)
                  AND (@MaTB = -1 OR PT.MaTB = @MaTB)
                ORDER BY P.TenPhong, TB.TenTB";

            SqlParameter[] parameters = {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@MaTB", maTB)
            };

            return DatabaseHelper.GetData(query, parameters, CommandType.Text);
        }
        // 1. Lấy danh sách thiết bị theo Mã phòng (Dùng khi lọc)
        public DataTable LayDSThietBiTheoPhong(int maPhong)
        {
            string query = @"
                SELECT 
                    PT.MaPhong,
                    P.TenPhong AS [Tên Phòng],
                    PT.MaTB,
                    TB.TenTB AS [Tên Thiết Bị],
                    PT.SoLuong AS [Số Lượng],
                    TB.MoTa AS [Mô Tả]
                FROM PHONG_THIETBI PT
                JOIN THIETBI TB ON PT.MaTB = TB.MaTB
                JOIN PHONG P ON PT.MaPhong = P.MaPhong
                WHERE PT.MaPhong = @MaPhong";

            SqlParameter[] param = { new SqlParameter("@MaPhong", maPhong) };
            return DatabaseHelper.GetData(query, param, CommandType.Text);
        }

        // 2. Lấy TẤT CẢ danh sách thiết bị của TẤT CẢ phòng (Dùng khi mới vào Tab)
        public DataTable LayTatCaThietBiPhong()
        {
            string query = @"
                SELECT 
                    PT.MaPhong,
                    P.TenPhong AS [Tên Phòng],
                    PT.MaTB,
                    TB.TenTB AS [Tên Thiết Bị],
                    PT.SoLuong AS [Số Lượng],
                    TB.MoTa AS [Mô Tả]
                FROM PHONG_THIETBI PT
                JOIN THIETBI TB ON PT.MaTB = TB.MaTB
                JOIN PHONG P ON PT.MaPhong = P.MaPhong
                ORDER BY P.TenPhong, TB.TenTB";

            return DatabaseHelper.GetData(query, null, CommandType.Text);
        }

        // 3. Thêm hoặc Cập nhật (Giữ nguyên)
        public bool ThemHoacCapNhatThietBi(int maPhong, int maTB, int soLuong)
        {
            string spName = "sp_ThemHoacCapNhatThietBiPhong";
            SqlParameter[] parameters = {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@MaTB", maTB),
                new SqlParameter("@SoLuong", soLuong)
            };
            return DatabaseHelper.ExecuteNonQuery(spName, parameters, CommandType.StoredProcedure);
        }

        // 4. Xóa (Giữ nguyên)
        public bool XoaThietBiKhoiPhong(int maPhong, int maTB)
        {
            string query = "DELETE FROM PHONG_THIETBI WHERE MaPhong = @MaPhong AND MaTB = @MaTB";
            SqlParameter[] parameters = {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@MaTB", maTB)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters, CommandType.Text);
        }
    }
}