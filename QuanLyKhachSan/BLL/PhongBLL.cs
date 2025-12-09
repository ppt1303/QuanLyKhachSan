using System;
using System.Data;
using System.Linq;
using QuanLyKhachSan.DAL; 
using QuanLyKhachSan.DTO;


namespace QuanLyKhachSan.BLL
{
    public class PhongBLL
    {
        private PhongDAL _dal;

        public PhongBLL()
        {
            _dal = new PhongDAL(); 
        }
        public DataTable LayTatCaPhong()
        {
            return _dal.LayDanhSachPhong();
        }
        public RoomStatistics TinhThongKe(DataTable dt)
        {
            if (dt == null) return new RoomStatistics();

            return new RoomStatistics
            {
                TongSo = dt.Rows.Count,
                Trong = dt.Select("TrangThaiO = 'Trống' AND TrangThaiPhong = 1").Length,
                DangO = dt.Select("TrangThaiO = 'Đang ở'").Length,
                DatTruoc = 0, // Logic mở rộng sau này
                Ban = dt.Select("TrangThaiPhong = 2").Length,
                BaoTri = dt.Select("TrangThaiPhong = 0").Length
            };
        }
        public DataTable LayDanhSachTang(DataTable dtPhong)
        {
            if (dtPhong == null) return null;
            DataView view = new DataView(dtPhong);
            return view.ToTable(true, "Tang");
        }
        public DataRow[] LocPhongTheoTang(DataTable dt, int tang)
        {
            if (dt == null) return new DataRow[0];
            if (tang == 0) return dt.Select("", "Tang ASC, TenPhong ASC");
            return dt.Select("Tang = " + tang, "TenPhong ASC");
        }
    }
}