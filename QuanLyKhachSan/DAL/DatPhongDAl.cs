using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class DatPhongDAL
    {
        // --- BỔ SUNG HÀM NÀY ĐỂ FIX LỖI FORM DANH SÁCH ---
        public DataTable GetDanhSachDonDat()
        {
            return DatabaseHelper.GetData("sp_LayDanhSachDonDat_ChiTiet", null);
        }
        // -------------------------------------------------

        public bool TaoDatPhong(int maKH, int maPhong, DateTime ngayDen, DateTime ngayDi, decimal tienCoc)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@MaKH", maKH),
                new SqlParameter("@MaPhong", maPhong),
                new SqlParameter("@NgayDen", ngayDen),
                new SqlParameter("@NgayDi", ngayDi),
                new SqlParameter("@TienCoc", tienCoc)
                // SQL có tham số @SoNguoi mặc định là 1, nên ở đây không truyền cũng được
            };
            return DatabaseHelper.ExecuteNonQuery("sp_DatPhong", parameters, CommandType.StoredProcedure);
        }

        public int LayMaNPDangO(int maPhong)
        {
            string query = @"
                SELECT TOP 1 NP.MaNP 
                FROM NHANPHONG NP
                WHERE NP.MaPhong = @MaPhong 
                AND NP.MaNP NOT IN (SELECT MaNP FROM TRAPHONG)
                ORDER BY NP.ThoiGianNhan DESC";

            SqlParameter[] param = { new SqlParameter("@MaPhong", maPhong) };

            DataTable dt = DatabaseHelper.GetData(query, param);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }

            return 0;
        }

        public DataTable LayThongTinCheckOut(int maNP)
        {
            string query = @"
                SELECT 
                    P.TenPhong, LP.TenLP, 
                    KH.HoTen, 
                    NP.ThoiGianNhan,
                    GP.GiaTheoNgay,
                    ISNULL(DP.TienCoc, 0) AS TienCoc,
                    (SELECT COUNT(*) FROM NHANPHONG WHERE MaDP = DP.MaDP) AS SoPhongDoan
                FROM NHANPHONG NP
                JOIN DATPHONG DP ON NP.MaDP = DP.MaDP
                JOIN KHACHHANG KH ON DP.MaKH = KH.MaKH
                JOIN PHONG P ON NP.MaPhong = P.MaPhong
                JOIN LOAIPHONG LP ON P.MaLP = LP.MaLP
                JOIN GIA_PHONG GP ON LP.MaLP = GP.MaLP
                WHERE NP.MaNP = @MaNP";

            SqlParameter[] param = { new SqlParameter("@MaNP", maNP) };
            return DatabaseHelper.GetData(query, param);
        }

        public DataTable LayDichVuDaDung(int maNP)
        {
            string query = @"
                SELECT 
                    DV.TenDV AS [Tên Dịch Vụ], 
                    SD.SoLuong AS [Số Lượng], 
                    DV.Gia AS [Đơn Giá], 
                    (SD.SoLuong * DV.Gia) AS [Thành Tiền]
                FROM SUDUNG_DICHVU SD 
                JOIN DICHVU DV ON SD.MaDV = DV.MaDV 
                WHERE SD.MaNP = @MaNP";

            SqlParameter[] param = { new SqlParameter("@MaNP", maNP) };
            return DatabaseHelper.GetData(query, param);
        }

        public DataTable LayPhuThuDaDung(int maNP)
        {
            string query = @"
                SELECT 
                    PT.Ten AS [Tên Phụ Thu], 
                    SP.SoLuong AS [Số Lượng], 
                    SP.GiaHienTai AS [Đơn Giá], 
                    (SP.SoLuong * SP.GiaHienTai) AS [Thành Tiền]
                FROM SUDUNG_PHUTHU SP
                JOIN PHUTHU PT ON SP.MaPhuThu = PT.MaPhuThu
                WHERE SP.MaNP = @MaNP";

            SqlParameter[] param = { new SqlParameter("@MaNP", maNP) };
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