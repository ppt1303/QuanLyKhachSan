using System;
using System.Data;
using System.Linq;
using QuanLyKhachSan.DAL; // Đảm bảo bạn đã có class DatabaseHelper trong namespace này
using QuanLyKhachSan.DTO;

namespace QuanLyKhachSan.BLL
{
    public class PhongBLL
    {
        // 1. Lấy toàn bộ dữ liệu phòng từ CSDL
        public DataTable LayTatCaPhong()
        {
            // Giả sử bạn có Function hoặc View trong SQL trả về các cột này
            // TrangThaiPhong: 0=Bảo trì, 1=Sẵn sàng, 2=Bẩn
            // TrangThaiO: 'Trống', 'Đang ở', 'Đặt trước'
            string query = @"
                SELECT 
                    P.MaPhong, P.TenPhong, P.Tang, LP.TenLP, P.TrangThaiPhong, 
                    dbo.fn_KiemTraTrangThaiPhong(P.MaPhong) as TrangThaiO 
                FROM PHONG P 
                JOIN LOAIPHONG LP ON P.MaLP = LP.MaLP 
                ORDER BY P.Tang ASC, P.TenPhong ASC";

            return DatabaseHelper.GetData(query);
        }

        // 2. Tính toán các con số cho thanh thống kê
        public RoomStatistics TinhThongKe(DataTable dt)
        {
            if (dt == null) return new RoomStatistics();

            return new RoomStatistics
            {
                TongSo = dt.Rows.Count,
                Trong = dt.Select("TrangThaiO = 'Trống' AND TrangThaiPhong = 1").Length,
                DangO = dt.Select("TrangThaiO = 'Đang ở'").Length,
                DatTruoc = dt.Select("TrangThaiO = 'Đặt trước'").Length, // Cần logic SQL trả về 'Đặt trước'
                Ban = dt.Select("TrangThaiPhong = 2").Length, // 2 = Bẩn
                BaoTri = dt.Select("TrangThaiPhong = 0").Length // 0 = Bảo trì
            };
        }

        // 3. Lấy danh sách các tầng duy nhất (để tạo nút lọc)
        public DataTable LayDanhSachTang(DataTable dtPhong)
        {
            if (dtPhong == null) return null;
            DataView view = new DataView(dtPhong);
            return view.ToTable(true, "Tang");
        }

        // 4. Lọc dữ liệu theo tầng
        public DataRow[] LocPhongTheoTang(DataTable dt, int tang)
        {
            if (dt == null) return new DataRow[0];
            // Nếu tang = 0 nghĩa là chọn "Tất cả"
            if (tang == 0) return dt.Select("", "Tang ASC, TenPhong ASC");

            return dt.Select("Tang = " + tang, "TenPhong ASC");
        }
    }
}