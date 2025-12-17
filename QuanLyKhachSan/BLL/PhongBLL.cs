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

        // 1. Hàm chính: Lấy danh sách phòng theo thời điểm (Gọi DAL mới)
        public DataTable GetDanhSachPhongTheoNgay(DateTime thoiDiem)
        {
            return _dal.GetDanhSachPhongTheoNgay(thoiDiem);
        }

        // =================================================================
        // 2. CÁC HÀM CŨ (GIỮ LẠI ĐỂ TRÁNH LỖI CODE CŨ)
        // =================================================================

        // Sửa lỗi: Thêm lại hàm này để code cũ gọi được
        public DataTable LayTatCaPhong(DateTime thoiDiem)
        {
            return _dal.GetDanhSachPhongTheoNgay(thoiDiem);
        }

        // Sửa lỗi: Hàm này cũng được giữ lại và trỏ về hàm mới
        public DataTable LayDanhSachPhongTrangChu()
        {
            return _dal.GetDanhSachPhongTheoNgay(DateTime.Now);
        }
        // =================================================================

        // 3. Tính toán thống kê
        public RoomStatistics TinhThongKe(DataTable dt)
        {
            RoomStatistics stats = new RoomStatistics();
            if (dt == null || dt.Rows.Count == 0) return stats;

            stats.TongSo = dt.Rows.Count;

            foreach (DataRow row in dt.Rows)
            {
                // Lưu ý: Đảm bảo tên cột khớp với SQL (TrangThaiHienThi hoặc TrangThaiPhong)
                // Ở query mới nhất chúng ta dùng 'TrangThaiHienThi'
                int ttHienThi = 0;

                if (row.Table.Columns.Contains("TrangThaiHienThi"))
                {
                    ttHienThi = Convert.ToInt32(row["TrangThaiHienThi"]);
                }
                else if (row.Table.Columns.Contains("TrangThaiPhong"))
                {
                    // Fallback nếu dùng query cũ
                    ttHienThi = Convert.ToInt32(row["TrangThaiPhong"]);
                }

                switch (ttHienThi)
                {
                    case 0: stats.BaoTri++; break;
                    case 4: stats.Ban++; break;     // 4 là Dơ/Chưa dọn
                    case 2: stats.DangO++; break;
                    case 3: stats.DatTruoc++; break;
                    default: stats.Trong++; break; // 1 là Trống
                }
            }
            return stats;
        }

        // 4. Lấy danh sách tầng
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

        // 5. Lọc phòng theo tầng
        public DataRow[] LocPhongTheoTang(DataTable dt, int tang)
        {
            if (dt == null) return new DataRow[0];
            string expr = "1=1";
            if (tang > 0) expr += $" AND Tang = {tang}";
            return dt.Select(expr, "TenPhong ASC");
        }
        // Trong class PhongBLL
        public bool CapNhatTrangThai(int maPhong, int trangThai)
        {
            return _dal.UpdateTrangThaiPhong(maPhong, trangThai);
        }
    }
}