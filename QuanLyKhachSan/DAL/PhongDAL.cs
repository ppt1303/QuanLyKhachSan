using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class PhongDAL
    {
        // Hàm này trước đây bị thiếu tham số, giờ sửa lại để truyền @ThoiDiemXem
        public DataTable GetDanhSachPhongTheoNgay(DateTime viewTime)
        {
            string query = "sp_LayDanhSachPhong_TrangChu";

            // Truyền tham số vào SQL để tránh lỗi "parameter was not supplied"
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ThoiDiemXem", viewTime)
            };

            return DatabaseHelper.GetData(query, param, CommandType.StoredProcedure);
        }

        // Các hàm phụ trợ khác giữ nguyên
        public DataTable LayDanhSachTang(DataTable dt)
        {
            string query = "SELECT DISTINCT Tang FROM PHONG ORDER BY Tang ASC";
            return DatabaseHelper.GetData(query);
        }
        // Trong class PhongDAL
        public bool UpdateTrangThaiPhong(int maPhong, int trangThai)
        {
            // Gọi Stored Procedure đã có sẵn trong SQL: sp_CapNhatTrangThaiDonDep
            string query = "sp_CapNhatTrangThaiDonDep";
            SqlParameter[] param = new SqlParameter[]
            {
        new SqlParameter("@MaPhong", maPhong),
        new SqlParameter("@TrangThai", trangThai),
        new SqlParameter("@GhiChu", DBNull.Value) // Gửi null nếu không cần ghi chú
            };

            return DatabaseHelper.ExecuteNonQuery(query, param, CommandType.StoredProcedure);
        }
        // Nếu còn hàm cũ LayDanhSachPhongTrangChu không tham số, hãy XÓA nó đi để tránh nhầm lẫn
    }
}