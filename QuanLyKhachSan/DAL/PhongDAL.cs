using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class PhongDAL
    {
        
        public DataTable GetDanhSachPhongTheoNgay(DateTime viewTime)
        {
            string query = "sp_LayDanhSachPhong_TrangChu";

            
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ThoiDiemXem", viewTime)
            };

            return DatabaseHelper.GetData(query, param, CommandType.StoredProcedure);
        }

      
        public DataTable LayDanhSachTang(DataTable dt)
        {
            string query = "SELECT DISTINCT Tang FROM PHONG ORDER BY Tang ASC";
            return DatabaseHelper.GetData(query);
        }
      
        public bool UpdateTrangThaiPhong(int maPhong, int trangThai)
        {
       
            string query = "sp_CapNhatTrangThaiDonDep";
            SqlParameter[] param = new SqlParameter[]
            {
        new SqlParameter("@MaPhong", maPhong),
        new SqlParameter("@TrangThai", trangThai),
        new SqlParameter("@GhiChu", DBNull.Value) 
            };

            return DatabaseHelper.ExecuteNonQuery(query, param, CommandType.StoredProcedure);
        }
    }
}