using System; 
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class KhachHangDAL
    {
        public DataTable TimKiemKhachHang(string keyword)
        {
            string query = "SELECT * FROM KHACHHANG WHERE CCCD = @keyword OR SDT = @keyword";

            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@keyword", keyword)
            };

            return DatabaseHelper.GetData(query, parameters);
        }

       
        public bool ThemKhachHang(string hoTen, string cccd, string sdt, string gioiTinh, DateTime ngaySinh)
        {
            string query = "sp_ThemKhachHang";
            
            SqlParameter[] parameters = {
                new SqlParameter("@HoTen", hoTen),
                new SqlParameter("@CCCD", cccd),
                new SqlParameter("@SDT", sdt),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@NgaySinh", ngaySinh)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters, CommandType.StoredProcedure);
        }
    }
}