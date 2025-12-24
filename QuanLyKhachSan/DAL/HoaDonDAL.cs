using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class HoaDonDAL
    {
        // 1. Lấy danh sách hóa đơn (Để hiện lên lưới tìm kiếm)
        public DataTable LayDanhSachHoaDon(DateTime tuNgay, DateTime denNgay)
        {
            string query = "EXEC sp_LayDanhSachHoaDon @TuNgay, @DenNgay";

            SqlParameter[] param = {
                new SqlParameter("@TuNgay", tuNgay.Date),
                new SqlParameter("@DenNgay", denNgay.Date.AddDays(1).AddSeconds(-1))
            };

            return DatabaseHelper.GetData(query, param);
        }

        // 2. Lấy chi tiết hóa đơn (Để IN ra giấy)
        public DataTable LayChiTietInHoaDon(int maHD)
        {
            string spName = "sp_LayChiTietHoaDon";
            SqlParameter[] param = { new SqlParameter("@MaHD", maHD) };

            return DatabaseHelper.GetData(spName, param, CommandType.StoredProcedure);
        }

        public int LayMaHDMoiNhat(int maNP)
        {
            string query = "EXEC sp_LayMaHDMoiNhat @MaNP";

            SqlParameter[] param = {
                new SqlParameter("@MaNP", maNP)
            };

            DataTable dt = DatabaseHelper.GetData(query, param);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["MaHD"]);
            }

            return 0;
        }
    }
}