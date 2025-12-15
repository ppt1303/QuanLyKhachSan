using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.DAL
{
    public class PhongDAL
    {
        // Thêm tham số viewTime (Thời điểm xem)
        public DataTable GetTatCaPhong(DateTime viewTime)
        {
            string query = @"
                SELECT 
                    p.MaPhong,
                    p.TenPhong,
                    p.Tang,
                    lp.TenLP,
                    p.TrangThaiPhong, -- 0: Bảo trì, 1: Sẵn sàng, 2: Dơ (Lấy trạng thái vật lý hiện tại)
                    
                    CASE 
                        -- 1. Kiểm tra trạng thái ĐANG Ở tại thời điểm viewTime
                        -- Logic: Có phiếu nhận phòng trước viewTime VÀ (Chưa trả hoặc Trả sau viewTime)
                        WHEN EXISTS (
                            SELECT 1 FROM NHANPHONG np
                            LEFT JOIN TRAPHONG tp ON np.MaNP = tp.MaNP
                            WHERE np.MaPhong = p.MaPhong
                              AND np.ThoiGianNhan <= @ViewTime
                              AND (tp.ThoiGianTra IS NULL OR tp.ThoiGianTra > @ViewTime)
                        ) THEN N'Đang ở'
                        
                        -- 2. Kiểm tra trạng thái ĐẶT TRƯỚC tại thời điểm viewTime
                        -- Logic: Có đơn đặt phòng 'Đã đặt' và viewTime nằm trong khoảng ngày ở
                        WHEN EXISTS (
                            SELECT 1 FROM CHITIET_DATPHONG cdp
                            JOIN DATPHONG dp ON cdp.MaDP = dp.MaDP
                            WHERE cdp.MaPhong = p.MaPhong 
                            AND dp.TrangThai = N'Đã đặt'
                            AND CAST(@ViewTime AS DATE) BETWEEN cdp.NgayNhanPhong AND cdp.NgayTraPhong
                        ) THEN N'Đặt trước'
                        
                        ELSE N'Trống'
                    END AS TrangThaiO

                FROM PHONG p
                JOIN LOAIPHONG lp ON p.MaLP = lp.MaLP
                ORDER BY p.Tang ASC, p.TenPhong ASC";

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ViewTime", viewTime)
            };

            return DatabaseHelper.GetData(query, param, CommandType.Text);
        }
        public DataTable LayDanhSachTang(DataTable dt)
        {
            string query = "SELECT DISTINCT Tang FROM PHONG ORDER BY Tang ASC";
            return DatabaseHelper.GetData(query);
        }
        public DataTable LayDanhSachPhongTrangChu()
        {
            string spName = "sp_LayDanhSachPhong_TrangChu";

            // Gọi hàm GetData từ Helper của bạn
            // Tham số 1: Tên SP
            // Tham số 2: null (vì SP này không cần tham số đầu vào)
            // Tham số 3: CommandType.StoredProcedure
            return DatabaseHelper.GetData(spName, null, CommandType.StoredProcedure);
        }
    }
}