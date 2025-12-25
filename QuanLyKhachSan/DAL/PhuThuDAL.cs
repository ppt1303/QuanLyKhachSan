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
            // Gọi hàm chạy Stored Procedure (ExecuteNonQuery)
            return DatabaseHelper.ExecuteNonQuery(query, param);
        }

        public DateTime LayNgayTraDuKien(int maNP)
        {
            string query = "SELECT CD.NgayTraPhong FROM NHANPHONG NP JOIN CHITIET_DATPHONG CD ON NP.MaDP = CD.MaDP AND NP.MaPhong = CD.MaPhong WHERE NP.MaNP = " + maNP;
            DataTable dt = DatabaseHelper.GetData(query); // Hoặc DataProvider.Instance...
            if (dt.Rows.Count > 0)
                return Convert.ToDateTime(dt.Rows[0]["NgayTraPhong"]);
            return DateTime.Now; // Mặc định nếu lỗi
        }

        // 1. Hàm lấy danh sách về
        public DataTable LayPhuThuTheoPhong(int maNP)
        {
            // Viết thẳng lệnh EXEC để tránh lỗi tham số
            string query = "EXEC sp_LayDanhSachPhuThuTheoPhong @MaNP = " + maNP;
            return DatabaseHelper.GetData(query);
            // Lưu ý: Nếu cậu dùng DataProvider.Instance.ExecuteQuery thì thay vào nhé
        }

        // 2. Hàm xóa
        public bool XoaPhuThu(int maSDPT)
        {
            // 1. Tên thủ tục (Ngắn gọn, đúng chuẩn)
            string spName = "sp_XoaPhuThuDaThem";

            // 2. Gói tham số (Để truyền số 11 vào)
            SqlParameter[] p = new SqlParameter[]
            {
        new SqlParameter("@MaSDPT", maSDPT)
            };

            // 3. Gọi hàm thực thi
            // (DatabaseHelper sẽ hiểu đây là SP và chạy ngon lành)
            return DatabaseHelper.ExecuteNonQuery(spName, p);
        }
    }
}