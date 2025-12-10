using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class LoaiPhongDAL
    {
        public DataTable LayDSLoaiPhong()
        {
            return DatabaseHelper.GetData("SELECT * FROM LOAIPHONG", null);
        }

        public DataTable TimPhongTrong(DateTime ngayDen, DateTime ngayDi)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@NgayDen", ngayDen),
                new SqlParameter("@NgayDi", ngayDi)
            };
            return DatabaseHelper.GetData("sp_TimPhongTrong", parameters, CommandType.StoredProcedure);
        }
    }
}