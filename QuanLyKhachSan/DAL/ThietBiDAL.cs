using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class ThietBiDAL
    {
        public DataTable LayDSThietBi()
        {
            // Lấy thêm cột SoLuong
            return DatabaseHelper.GetData("SELECT MaTB, TenTB, SoLuong, MoTa FROM dbo.THIETBI ORDER BY MaTB", null);
        }

        public bool ThemThietBi(string tenTB, string moTa, int soLuong)
        {
            string spName = "sp_ThemThietBi";
            SqlParameter[] parameters = {
                new SqlParameter("@TenTB", tenTB),
                new SqlParameter("@MoTa", moTa),
                new SqlParameter("@SoLuong", soLuong)
            };
            return DatabaseHelper.ExecuteNonQuery(spName, parameters, CommandType.StoredProcedure);
        }

        public bool SuaThietBi(int maTB, string tenTB, string moTa, int soLuong)
        {
            string spName = "sp_SuaThietBi";
            SqlParameter[] parameters = {
                new SqlParameter("@MaTB", maTB),
                new SqlParameter("@TenTB", tenTB),
                new SqlParameter("@MoTa", moTa),
                new SqlParameter("@SoLuong", soLuong)
            };
            return DatabaseHelper.ExecuteNonQuery(spName, parameters, CommandType.StoredProcedure);
        }

        public bool XoaThietBi(string maTBList)
        {
            string spName = "sp_XoaNhieuThietBi";
            SqlParameter[] parameters = { new SqlParameter("@MaTBList", maTBList) };
            return DatabaseHelper.ExecuteNonQuery(spName, parameters, CommandType.StoredProcedure);
        }
    }
}