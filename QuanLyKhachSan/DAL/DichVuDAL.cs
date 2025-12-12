using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class DichVuDAL
    {
        // Hàm chung lấy danh sách dịch vụ
        public DataTable LayDSDichVu()
        {
            return DatabaseHelper.GetData("SELECT MaDV, TenDV, Gia FROM dbo.DICHVU ORDER BY MaDV", null);
        }

        // Hàm thêm dịch vụ (KHÔNG CẦN TRUYỀN MaDV)
        public bool ThemDichVu(string tenDV, decimal gia)
        {
            string spName = "sp_ThemDichVu";
            SqlParameter[] parameters = {
                new SqlParameter("@TenDV", tenDV),
                new SqlParameter("@Gia", gia)
            };
            return DatabaseHelper.ExecuteNonQuery(spName, parameters, CommandType.StoredProcedure);
        }

        // Hàm sửa dịch vụ
        public bool SuaDichVu(int maDV, string tenDV, decimal gia)
        {
            string spName = "sp_SuaDichVu";
            SqlParameter[] parameters = {
                new SqlParameter("@MaDV", maDV),
                new SqlParameter("@TenDV", tenDV),
                new SqlParameter("@Gia", gia)
            };
            return DatabaseHelper.ExecuteNonQuery(spName, parameters, CommandType.StoredProcedure);
        }

        // Hàm xóa dịch vụ (Xóa hàng loạt)
        public bool XoaDichVu(string maDVList)
        {
            string spName = "sp_XoaNhieuDichVu";
            SqlParameter[] parameters = {
                new SqlParameter("@MaDVCs", maDVList)
            };
            return DatabaseHelper.ExecuteNonQuery(spName, parameters, CommandType.StoredProcedure);
        }
    }
}