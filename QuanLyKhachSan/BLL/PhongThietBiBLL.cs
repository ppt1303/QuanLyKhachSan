using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.BLL
{
    public class PhongThietBiBLL
    {
        private PhongThietBiDAL ptbDAL = new PhongThietBiDAL();
        private ThietBiDAL tbDAL = new ThietBiDAL();

        public DataTable LayDSThietBiTheoPhong(int maPhong) => ptbDAL.LayDSThietBiTheoPhong(maPhong);
        public DataTable LayTatCaThietBi() => tbDAL.LayDSThietBi();
        public DataTable TimKiem(int maPhong, int maTB) => ptbDAL.TimKiemThietBiPhong(maPhong, maTB);
        public bool XoaThietBiKhoiPhong(int maPhong, int maTB) => ptbDAL.XoaThietBiKhoiPhong(maPhong, maTB);

        public bool CapNhatThietBiPhong(int maPhong, int maTB, int soLuong)
        {
            return ptbDAL.ThemHoacCapNhatThietBi(maPhong, maTB, soLuong);
        }

        // --- HÀM KIỂM TRA TỒN KHO ---
        public string KiemTraKhaDung(int maPhong, int maTB, int soLuongMuonSet)
        {
            // 1. Lấy Tổng kho của thiết bị này
            string queryKho = "SELECT SoLuong FROM THIETBI WHERE MaTB = @MaTB";
            DataTable dtKho = DatabaseHelper.GetData(queryKho, new SqlParameter[] { new SqlParameter("@MaTB", maTB) });
            int tongKho = (dtKho.Rows.Count > 0 && dtKho.Rows[0][0] != DBNull.Value) ? Convert.ToInt32(dtKho.Rows[0][0]) : 0;

            // 2. Lấy Tổng đang dùng ở CÁC PHÒNG KHÁC (trừ phòng hiện tại ra)
            string queryDaDung = "SELECT SUM(SoLuong) FROM PHONG_THIETBI WHERE MaTB = @MaTB AND MaPhong != @MaPhong";
            SqlParameter[] param = {
                new SqlParameter("@MaTB", maTB),
                new SqlParameter("@MaPhong", maPhong)
            };
            DataTable dtDaDung = DatabaseHelper.GetData(queryDaDung, param);

            int daDungChoPhongKhac = 0;
            if (dtDaDung.Rows.Count > 0 && dtDaDung.Rows[0][0] != DBNull.Value)
                daDungChoPhongKhac = Convert.ToInt32(dtDaDung.Rows[0][0]);

            // 3. Tính toán
            int conLaiChoPhongNay = tongKho - daDungChoPhongKhac;

            if (soLuongMuonSet > conLaiChoPhongNay)
            {
                return $"Không đủ hàng! Tổng kho: {tongKho}. Đang dùng nơi khác: {daDungChoPhongKhac}. Bạn chỉ có thể thêm tối đa: {conLaiChoPhongNay}.";
            }

            return "OK"; // Đủ hàng
        }
    }
}