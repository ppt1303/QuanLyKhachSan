using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class DatPhongDAL
    {
        public bool TaoDatPhong(int maKH, int maPhong, DateTime ngayDen, DateTime ngayDi, decimal tienCoc)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@MaKH", maKH),
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@NgayDen", ngayDen),
                new SqlParameter("@NgayDi", ngayDi),
                new SqlParameter("@TienCoc", tienCoc)
            };
            return DatabaseHelper.ExecuteNonQuery("sp_DatPhong", parameters, CommandType.StoredProcedure);
        }
    }
}