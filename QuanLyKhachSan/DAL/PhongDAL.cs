using System.Data;

namespace QuanLyKhachSan.DAL
{
    public class PhongDAL
    {
        public DataTable LayDanhSachPhong()
        {
            string query = @"
                SELECT 
                    P.MaPhong, 
                    P.TenPhong, 
                    P.Tang, 
                    LP.TenLP, 
                    P.TrangThaiPhong, 
                    dbo.fn_KiemTraTrangThaiPhong(P.MaPhong) as TrangThaiO 
                FROM PHONG P
                JOIN LOAIPHONG LP ON P.MaLP = LP.MaLP
                ORDER BY P.Tang ASC, P.TenPhong ASC";

            return DatabaseHelper.GetDataTable(query);
        }
    }
}