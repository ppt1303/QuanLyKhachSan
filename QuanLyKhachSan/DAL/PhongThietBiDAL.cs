using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class PhongThietBiDAL
    {
        // 1. Tìm kiếm (Đã chuyển sang Proc)
        public DataTable TimKiemThietBiPhong(int maPhong, int maTB)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@MaTB", maTB)
            };
            return DatabaseHelper.GetData("sp_TimKiemThietBiPhong", parameters, CommandType.StoredProcedure);
        }

        // 2. Lấy danh sách theo phòng (Đã chuyển sang Proc)
        public DataTable LayDSThietBiTheoPhong(int maPhong)
        {
            SqlParameter[] param = { new SqlParameter("@MaPhong", maPhong) };
            return DatabaseHelper.GetData("sp_LayDSThietBiTheoPhong", param, CommandType.StoredProcedure);
        }

        // 3. Lấy tất cả (Đã chuyển sang Proc)
        public DataTable LayTatCaThietBiPhong()
        {
            return DatabaseHelper.GetData("sp_LayTatCaThietBiPhong", null, CommandType.StoredProcedure);
        }

        // 4. Thêm hoặc Cập nhật (Giữ nguyên vì đã dùng Proc)
        public bool ThemHoacCapNhatThietBi(int maPhong, int maTB, int soLuong)
        {
            string spName = "sp_ThemHoacCapNhatThietBiPhong"; // Đảm bảo Proc này đã có trong DB
            SqlParameter[] parameters = {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@MaTB", maTB),
                new SqlParameter("@SoLuong", soLuong)
            };
            return DatabaseHelper.ExecuteNonQuery(spName, parameters, CommandType.StoredProcedure);
        }

        // 5. Xóa (Đã chuyển sang Proc)
        public bool XoaThietBiKhoiPhong(int maPhong, int maTB)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@MaTB", maTB)
            };
            return DatabaseHelper.ExecuteNonQuery("sp_XoaThietBiKhoiPhong", parameters, CommandType.StoredProcedure);
        }
    }
}