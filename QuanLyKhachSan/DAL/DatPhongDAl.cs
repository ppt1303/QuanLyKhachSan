using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class DatPhongDAL
    {
        public DataTable GetDanhSachDonDat()
        {
            return DatabaseHelper.GetData("sp_LayDanhSachDonDat_ChiTiet", null);
        }
     

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

        public int LayMaNPDangO(int maPhong)
        {
            string query = "EXEC sp_LayMaNPDangO @MaPhong";

            SqlParameter[] param = {
                new SqlParameter("@MaPhong", maPhong)
            };

            DataTable dt = DatabaseHelper.GetData(query, param);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]); 
            }

            return 0;
        }

        public DataTable LayThongTinCheckOut(int maNP)
        {
            string query = "EXEC sp_LayThongTinCheckOut @MaNP";

            SqlParameter[] param = {
              new SqlParameter("@MaNP", maNP)
            };

            return DatabaseHelper.GetData(query, param);
        }

        public DataTable LayDichVuDaDung(int maNP)
        {
            string query = "EXEC sp_LayDichVuDaDung @MaNP";

            SqlParameter[] param = {
        new SqlParameter("@MaNP", maNP)
    };

            return DatabaseHelper.GetData(query, param);
        }

        public DataTable LayPhuThuDaDung(int maNP)
        {
            string query = "EXEC sp_LayPhuThuDaDung @MaNP";

            SqlParameter[] param = {
        new SqlParameter("@MaNP", maNP)
    };

            return DatabaseHelper.GetData(query, param);
        }

        public bool ThucHienTraPhong(int maNP, string tinhTrang, string ghiChu)
        {
            string spName = "sp_ThucHienTraPhong";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNP", maNP),
                new SqlParameter("@TinhTrangPhong", tinhTrang),
                new SqlParameter("@GhiChu", ghiChu)
            };
            return DatabaseHelper.ExecuteNonQuery(spName, parameters, CommandType.StoredProcedure);
        }

        public void TaoHoaDon(int maTraPhong, int maPTTT)
        {
            string spName = "sp_TaoHoaDonThanhToan";
            SqlParameter[] parameters = {
                new SqlParameter("@MaTraPhong", maTraPhong),
                new SqlParameter("@MaPTTT", maPTTT)
            };
            DatabaseHelper.ExecuteNonQuery(spName, parameters, CommandType.StoredProcedure);
        }

        public int LayMaTraPhongMoiNhat(int maNP)
        {
            string query = "SELECT MAX(MaTraPhong) FROM TRAPHONG WHERE MaNP = @MaNP";
            SqlParameter[] param = { new SqlParameter("@MaNP", maNP) };

            DataTable dt = DatabaseHelper.GetData(query, param);

            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                return Convert.ToInt32(dt.Rows[0][0]);

            return 0;
        }
    }
}