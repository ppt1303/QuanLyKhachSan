using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class PhongThietBiDAL
    {
   
        public DataTable TimKiemThietBiPhong(int maPhong, int maTB)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@MaTB", maTB)
            };
            return DatabaseHelper.GetData("sp_TimKiemThietBiPhong", parameters, CommandType.StoredProcedure);
        }

    
        public DataTable LayDSThietBiTheoPhong(int maPhong)
        {
            SqlParameter[] param = { new SqlParameter("@MaPhong", maPhong) };
            return DatabaseHelper.GetData("sp_LayDSThietBiTheoPhong", param, CommandType.StoredProcedure);
        }

     
        public DataTable LayTatCaThietBiPhong()
        {
            return DatabaseHelper.GetData("sp_LayTatCaThietBiPhong", null, CommandType.StoredProcedure);
        }

       
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