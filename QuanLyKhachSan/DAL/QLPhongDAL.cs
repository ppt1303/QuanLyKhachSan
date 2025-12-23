using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class QLPhongDAL
    {
        public DataTable LayTatCaPhong()
        {
            return DatabaseHelper.GetData("sp_LayTatCaPhong", null, CommandType.StoredProcedure);
        }

        public DataTable LayDSLoaiPhong()
        {
            return DatabaseHelper.GetData("sp_LayDSLoaiPhong", null, CommandType.StoredProcedure);
        }

        public bool ThemPhong(string tenPhong, int maLP, int tang, string huong)
        {
            SqlParameter[] param = {
                new SqlParameter("@TenPhong", tenPhong),
                new SqlParameter("@MaLP", maLP),
                new SqlParameter("@Tang", tang),
                new SqlParameter("@Huong", huong)
            };
            return DatabaseHelper.ExecuteNonQuery("sp_ThemPhong", param, CommandType.StoredProcedure);
        }

        public bool SuaPhong(int maPhong, string tenPhong, int maLP, int tang, string huong)
        {
            SqlParameter[] param = {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@TenPhong", tenPhong),
                new SqlParameter("@MaLP", maLP),
                new SqlParameter("@Tang", tang),
                new SqlParameter("@Huong", huong)
            };
            return DatabaseHelper.ExecuteNonQuery("sp_SuaPhong", param, CommandType.StoredProcedure);
        }

        public bool XoaPhong(int maPhong)
        {
            SqlParameter[] param = { new SqlParameter("@MaPhong", maPhong) };
            return DatabaseHelper.ExecuteNonQuery("sp_XoaPhong", param, CommandType.StoredProcedure);
        }
    }
}