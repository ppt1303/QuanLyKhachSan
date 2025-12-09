using System; // Thêm nếu chưa có
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class KhachHangDAL
    {
        public DataTable TimKiemKhachHang(string keyword)
        {
            string query = "SELECT * FROM KHACHHANG WHERE CCCD = @keyword OR SDT = @keyword";

            // SỬA Ở ĐÂY: Đóng gói SqlParameter vào trong mảng []
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@keyword", keyword)
            };

            return DatabaseHelper.GetData(query, parameters);
        }

        // ... giữ nguyên phần ThemKhachHang
        public bool ThemKhachHang(string hoTen, string cccd, string sdt, string gioiTinh, DateTime ngaySinh)
        {
            string query = "sp_ThemKhachHang";
            // Đoạn này bạn đã làm đúng (khai báo mảng parameters = { ... }) nên không cần sửa
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