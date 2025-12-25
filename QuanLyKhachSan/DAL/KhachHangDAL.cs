using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class KhachHangDAL
    {
        public DataTable LayDanhSachKhachHang()
        {
            string query = "SELECT MaKH, HoTen, SDT, CCCD, GioiTinh, NgaySinh, QuocTich FROM KHACHHANG";
            return DatabaseHelper.GetData(query, null);
        }

    
        public DataTable TimKiemKhachHang(string keyword)
        {
            string query = @"
                SELECT MaKH, HoTen, SDT, CCCD, GioiTinh, NgaySinh, QuocTich FROM KHACHHANG 
                WHERE 
                    CCCD LIKE '%' + @keyword + '%' OR 
                    SDT LIKE '%' + @keyword + '%' OR 
                    HoTen LIKE '%' + @keyword + '%'";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@keyword", keyword)
            };
            return DatabaseHelper.GetData(query, parameters);
        }

        public bool ThemKhachHang(string hoTen, string cccd, string sdt, string gioiTinh, DateTime ngaySinh, string quocTich)
        {
            string query = "sp_ThemKhachHang";
            SqlParameter[] parameters = {
                new SqlParameter("@HoTen", hoTen),
                new SqlParameter("@CCCD", cccd),
                new SqlParameter("@SDT", sdt),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@NgaySinh", ngaySinh),
                new SqlParameter("@QuocTich", quocTich) 
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters, CommandType.StoredProcedure);
        }

     
        public bool CapNhatKhachHang(int maKH, string hoTen, string sdt, string cccd, string gioiTinh, DateTime ngaySinh, string quocTich)
        {
            string query = "sp_CapNhatKhachHang";
            SqlParameter[] parameters = {
                new SqlParameter("@MaKH", maKH),
                new SqlParameter("@HoTen", hoTen),
                new SqlParameter("@SDT", sdt),
                new SqlParameter("@CCCD", cccd),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@NgaySinh", ngaySinh),
                new SqlParameter("@QuocTich", quocTich)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters, CommandType.StoredProcedure);
        }

    

        public bool XoaKhachHang(int maKH)
        {
            string query = "sp_XoaKhachHangHoanToan"; 
            SqlParameter[] parameters = {
         new SqlParameter("@MaKH", maKH)
     };
         
            return DatabaseHelper.ExecuteNonQuery(query, parameters, CommandType.StoredProcedure);
        }
    }
}