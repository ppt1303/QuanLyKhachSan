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

        public DataTable GetDanhSachPhongTheoNgay(DateTime thoiDiem)
        {
            return _dal.GetDanhSachPhongTheoNgay(thoiDiem);
        }

        public DataTable LayTatCaPhong(DateTime thoiDiem)
        {
            return _dal.GetDanhSachPhongTheoNgay(thoiDiem);
        }

        public DataTable LayDanhSachPhongTrangChu()
        {
            return _dal.GetDanhSachPhongTheoNgay(DateTime.Now);
        }

        public RoomStatistics TinhThongKe(DataTable dt)
        {
            RoomStatistics stats = new RoomStatistics();
            if (dt == null || dt.Rows.Count == 0) return stats;

            stats.TongSo = dt.Rows.Count;

            foreach (DataRow row in dt.Rows)
            {

                int ttHienThi = 0;

                if (row.Table.Columns.Contains("TrangThaiHienThi"))
                {
                    ttHienThi = Convert.ToInt32(row["TrangThaiHienThi"]);
                }
                else if (row.Table.Columns.Contains("TrangThaiPhong"))
                {
                    ttHienThi = Convert.ToInt32(row["TrangThaiPhong"]);
                }

                switch (ttHienThi)
                {
                    case 0: stats.BaoTri++; break;
                    case 4: stats.Ban++; break;     
                    case 2: stats.DangO++; break;
                    case 3: stats.DatTruoc++; break;
                    default: stats.Trong++; break; 
                }
            }
            return stats;
        }

        public DataTable LayDanhSachTang(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return null;
            try
            {
                DataView view = new DataView(dt);
                DataTable distinctTang = view.ToTable(true, "Tang");
                distinctTang.DefaultView.Sort = "Tang ASC";
                return distinctTang.DefaultView.ToTable();
            }
            catch { return null; }
        }

        public DataRow[] LocPhongTheoTang(DataTable dt, int tang)
        {
            if (dt == null) return new DataRow[0];
            string expr = "1=1";
            if (tang > 0) expr += $" AND Tang = {tang}";
            return dt.Select(expr, "TenPhong ASC");
        }

        public bool CapNhatTrangThai(int maPhong, int trangThai)
        {
            return _dal.UpdateTrangThaiPhong(maPhong, trangThai);
        }
    }
}