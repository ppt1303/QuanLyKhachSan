using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class QLPhongDAL
    {
        // 1. Lấy tất cả phòng (Bỏ Trạng Thái, Bỏ Ghi Chú, Thêm Giá để hiển thị)
        public DataTable LayTatCaPhong()
        {
            // KẾT NỐI 3 BẢNG: PHONG - LOAIPHONG - GIA_PHONG
            string query = @"
                SELECT 
                    P.MaPhong,
                    P.TenPhong AS [Tên Phòng],
                    L.TenLP AS [Loại Phòng],
                    P.Tang AS [Tầng],
                    P.Huong AS [Hướng],
                    -- Lấy giá để tham khảo (Chỉ xem)
                    GP.GiaTheoGio AS [Giá Giờ],
                    GP.GiaTheoNgay AS [Giá Ngày],
                    GP.GiaTheoDem AS [Giá Đêm],
                    P.MaLP
                FROM PHONG P
                LEFT JOIN LOAIPHONG L ON P.MaLP = L.MaLP
                LEFT JOIN GIA_PHONG GP ON L.MaLP = GP.MaLP
                ORDER BY P.TenPhong ASC";

            return DatabaseHelper.GetData(query, null, CommandType.Text);
        }

        // 2. Lấy danh sách Loại Phòng (để nạp vào ComboBox)
        public DataTable LayDSLoaiPhong()
        {
            return DatabaseHelper.GetData("SELECT MaLP, TenLP FROM LOAIPHONG", null, CommandType.Text);
        }

        // 3. Thêm Phòng Mới (Chỉ cần Tên, Loại, Tầng, Hướng)
        public bool ThemPhong(string tenPhong, int maLP, int tang, string huong)
        {
            string query = @"INSERT INTO PHONG (TenPhong, MaLP, Tang, Huong) 
                             VALUES (@TenPhong, @MaLP, @Tang, @Huong)";

            SqlParameter[] param = {
                new SqlParameter("@TenPhong", tenPhong),
                new SqlParameter("@MaLP", maLP),
                new SqlParameter("@Tang", tang),
                new SqlParameter("@Huong", huong)
            };
            return DatabaseHelper.ExecuteNonQuery(query, param, CommandType.Text);
        }

        // 4. Sửa Thông Tin Phòng
        public bool SuaPhong(int maPhong, string tenPhong, int maLP, int tang, string huong)
        {
            string query = @"UPDATE PHONG 
                             SET TenPhong = @TenPhong, MaLP = @MaLP, Tang = @Tang, Huong = @Huong
                             WHERE MaPhong = @MaPhong";

            SqlParameter[] param = {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@TenPhong", tenPhong),
                new SqlParameter("@MaLP", maLP),
                new SqlParameter("@Tang", tang),
                new SqlParameter("@Huong", huong)
            };
            return DatabaseHelper.ExecuteNonQuery(query, param, CommandType.Text);
        }

        // 5. Xóa Phòng
        public bool XoaPhong(int maPhong)
        {
            string query = "DELETE FROM PHONG WHERE MaPhong = @MaPhong";
            SqlParameter[] param = { new SqlParameter("@MaPhong", maPhong) };
            return DatabaseHelper.ExecuteNonQuery(query, param, CommandType.Text);
        }
    }
}