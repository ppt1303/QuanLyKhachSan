using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class PhuThuDAL
    {
        

        public DataTable GetListPhuThu()
        {
            return DatabaseHelper.GetData("SELECT * FROM PHUTHU");
        }

        public bool InsertPhuThu(int maNP, int maPhuThu, int soLuong, string ghiChu)
        {
            string query = "sp_ThemPhuThuTaiQuay";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@MaNP", maNP),
                new SqlParameter("@MaPhuThu", maPhuThu),
                new SqlParameter("@SoLuong", soLuong),
                new SqlParameter("@GhiChu", string.IsNullOrEmpty(ghiChu) ? (object)DBNull.Value : ghiChu)
            };
       
            return DatabaseHelper.ExecuteNonQuery(query, param);
        }

        public DateTime LayNgayTraDuKien(int maNP)
        {
            string query = "SELECT CD.NgayTraPhong FROM NHANPHONG NP JOIN CHITIET_DATPHONG CD ON NP.MaDP = CD.MaDP AND NP.MaPhong = CD.MaPhong WHERE NP.MaNP = " + maNP;
            DataTable dt = DatabaseHelper.GetData(query); 
            if (dt.Rows.Count > 0)
                return Convert.ToDateTime(dt.Rows[0]["NgayTraPhong"]);
            return DateTime.Now; 
        }

        // 1. Hàm lấy danh sách về
        public DataTable LayPhuThuTheoPhong(int maNP)
        {
           
            string query = "EXEC sp_LayDanhSachPhuThuTheoPhong @MaNP = " + maNP;
            return DatabaseHelper.GetData(query);
          
        }

        // 2. Hàm xóa
        public bool XoaPhuThu(int maSDPT)
        {

            string spName = "sp_XoaPhuThuDaThem";


            SqlParameter[] p = new SqlParameter[]
            {
        new SqlParameter("@MaSDPT", maSDPT)
            };
            return DatabaseHelper.ExecuteNonQuery(spName, p);
        }
    }
}