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

        public DataTable LayTatCaPhong(DateTime thoiDiem)
        {
            return _dal.GetTatCaPhong(thoiDiem);
        }

        public RoomStatistics TinhThongKe(DataTable dt)
        {
            RoomStatistics stats = new RoomStatistics();
            if (dt == null || dt.Rows.Count == 0) return stats;

            stats.TongSo = dt.Rows.Count;

            foreach (DataRow row in dt.Rows)
            {
                // DB Script quy định:
                // TrangThaiPhong (tinyint): 0: Đang sửa (Bảo trì), 1: Sẵn sàng, 2: Dơ
                int ttPhong = Convert.ToInt32(row["TrangThaiPhong"]);

                // TrangThaiO (string): "Đang ở", "Trống", "Đặt trước" (Lấy từ SQL Query)
                string ttO = row["TrangThaiO"].ToString();

                if (ttPhong == 0) // Bảo trì
                {
                    stats.BaoTri++;
                }
                else if (ttPhong == 2) // Dơ / Chưa dọn
                {
                    stats.Ban++;
                }
                else // ttPhong == 1 (Sẵn sàng) -> Xét tiếp có người ở không
                {
                    if (ttO == "Đang ở") stats.DangO++;
                    else if (ttO == "Đặt trước") stats.DatTruoc++;
                    else stats.Trong++;
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
        public DataTable LayDanhSachPhongTrangChu()
        {
            return _dal.LayDanhSachPhongTrangChu();
        }
    }
}