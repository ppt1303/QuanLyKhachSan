using QuanLyKhachSan.BLL;
using QuanLyKhachSan.DAL; // Sử dụng DatabaseHelper
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace QuanLyKhachSan.GUI
{
    public partial class frmChiTietPhong : Form 
    {
        private int maNP;    
        private int maPhong; 
        private BookingBLL bookingBLL = new BookingBLL();

        public frmChiTietPhong(int maNP, int maPhong)
        {
            InitializeComponent();
            this.maNP = maNP;
            this.maPhong = maPhong;
        }

        private void frmChiTietPhong_Load(object sender, EventArgs e)
        {
            
            LoadDanhSachTrangThaiPhong(); 
            LoadDanhSachDichVu();       

            LoadThongTinChung();          
            LoadLichSuDichVu();          
        }
        private void LoadThongTinChung()
        {
            try
            {
                // A. Lấy tên phòng và loại phòng
                string queryPhong = @"
                    SELECT P.TenPhong, LP.TenLP, P.TrangThaiPhong
                    FROM PHONG P 
                    JOIN LOAIPHONG LP ON P.MaLP = LP.MaLP 
                    WHERE P.MaPhong = @MaPhong";

                DataTable dtPhong = DatabaseHelper.GetData(queryPhong, new SqlParameter[] {
                    new SqlParameter("@MaPhong", this.maPhong)
                });

                if (dtPhong.Rows.Count > 0)
                {
                    string tenPhong = dtPhong.Rows[0]["TenPhong"].ToString();
                    string tenLP = dtPhong.Rows[0]["TenLP"].ToString();
                    int trangThaiHienTai = Convert.ToInt32(dtPhong.Rows[0]["TrangThaiPhong"]);

                    lblTenPhong.Text = $"{tenPhong} - {tenLP}";
                    lblTieuDeTrangThai.Text = GetTrangThaiString(trangThaiHienTai);

                    // Set giá trị cho ComboBox trạng thái
                    if (cboTrangThaiPhong.Items.Count > 0)
                        cboTrangThaiPhong.SelectedValue = trangThaiHienTai;
                }

                // B. Lấy thông tin Khách hàng (Nếu đang có người ở - maNP > 0)
                if (this.maNP > 0)
                {
                    // JOIN 3 Bảng: NHANPHONG -> DATPHONG -> KHACHHANG
                    string queryKhach = @"
                        SELECT KH.HoTen, NP.ThoiGianNhan
                        FROM NHANPHONG NP
                        JOIN DATPHONG DP ON NP.MaDP = DP.MaDP
                        JOIN KHACHHANG KH ON DP.MaKH = KH.MaKH
                        WHERE NP.MaNP = @MaNP";

                    DataTable dtKhach = DatabaseHelper.GetData(queryKhach, new SqlParameter[] {
                        new SqlParameter("@MaNP", this.maNP)
                    });

                    if (dtKhach.Rows.Count > 0)
                    {
                        lblTenKhach.Text = dtKhach.Rows[0]["HoTen"].ToString();
                        DateTime timeNhan = Convert.ToDateTime(dtKhach.Rows[0]["ThoiGianNhan"]);
                        lblGioNhan.Text = timeNhan.ToString("dd/MM/yyyy HH:mm");
                    }
                }
                else
                {
                    lblTenKhach.Text = "---";
                    lblGioNhan.Text = "---";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin chung: " + ex.Message);
            }
        }

        // 1.2 Tải lịch sử sử dụng Dịch vụ & Phụ thu (UNION 2 bảng)
        public void LoadLichSuDichVu()
        {
            if (this.maNP <= 0) return; // Không có khách thì không có lịch sử

            try
            {
                // Query kết hợp Dịch vụ và Phụ thu
                string query = @"
                    SELECT 
                        DV.TenDV AS [Tên Dịch Vụ/Phụ Thu], 
                        SD.SoLuong AS [SL], 
                        FORMAT(DV.Gia, 'N0') AS [Đơn Giá], 
                        FORMAT(SD.SoLuong * DV.Gia, 'N0') AS [Thành Tiền],
                        SD.NgaySuDung AS [Thời Gian]
                    FROM SUDUNG_DICHVU SD 
                    JOIN DICHVU DV ON SD.MaDV = DV.MaDV 
                    WHERE SD.MaNP = @MaNP

                    UNION ALL

                    SELECT 
                        PT.Ten AS [Tên Dịch Vụ/Phụ Thu], 
                        SP.SoLuong AS [SL], 
                        FORMAT(SP.GiaHienTai, 'N0') AS [Đơn Giá], 
                        FORMAT(SP.SoLuong * SP.GiaHienTai, 'N0') AS [Thành Tiền],
                        SP.ThoiGianGhiNhan AS [Thời Gian]
                    FROM SUDUNG_PHUTHU SP 
                    JOIN PHUTHU PT ON SP.MaPhuThu = PT.MaPhuThu 
                    WHERE SP.MaNP = @MaNP";

                DataTable dt = DatabaseHelper.GetData(query, new SqlParameter[] {
                    new SqlParameter("@MaNP", this.maNP)
                });

                dgvLichSuDV.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử dịch vụ: " + ex.Message);
            }
        }
        private void LoadDanhSachTrangThaiPhong()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Val", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            dt.Rows.Add(1, "Sẵn sàng");
            dt.Rows.Add(2, "Chưa dọn dẹp");
            dt.Rows.Add(0, "Bảo trì");

            cboTrangThaiPhong.DataSource = dt;
            cboTrangThaiPhong.DisplayMember = "Name";
            cboTrangThaiPhong.ValueMember = "Val";
        }

        // 1.4 Load ComboBox Dịch Vụ
        private void LoadDanhSachDichVu()
        {
            try
            {
                string query = "SELECT MaDV, TenDV, Gia FROM DICHVU";
                DataTable dt = DatabaseHelper.GetData(query);

                cboDichVu.DataSource = dt;
                cboDichVu.DisplayMember = "TenDV"; // Hiển thị tên
                cboDichVu.ValueMember = "MaDV";    // Giá trị là mã
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách dịch vụ: " + ex.Message);
            }
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                int trangThaiMoi = Convert.ToInt32(cboTrangThaiPhong.SelectedValue);

                // Gọi SP: sp_CapNhatTrangThaiDonDep
                // Tham số: @MaPhong, @TrangThai, @GhiChu
                string spName = "sp_CapNhatTrangThaiDonDep";
                SqlParameter[] p = {
                    new SqlParameter("@MaPhong", this.maPhong),
                    new SqlParameter("@TrangThai", trangThaiMoi),
                    new SqlParameter("@GhiChu", "Cập nhật từ chi tiết phòng")
                };

                if (DatabaseHelper.ExecuteNonQuery(spName, p, CommandType.StoredProcedure))
                {
                    MessageBox.Show("Cập nhật trạng thái thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Nếu cập nhật thành công, load lại màu chữ
                    lblTieuDeTrangThai.Text = GetTrangThaiString(trangThaiMoi);

                    // Có thể đóng form luôn nếu muốn
                    // this.Close(); 
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        // 2.2 Thêm dịch vụ cho phòng
        private void btnThemDV_Click(object sender, EventArgs e)
        {
            // Kiểm tra: Phòng phải có khách (MaNP > 0) mới thêm dịch vụ được
            if (this.maNP <= 0)
            {
                MessageBox.Show("Phòng chưa có khách check-in, không thể thêm dịch vụ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy dữ liệu từ giao diện
                short maDV = Convert.ToInt16(cboDichVu.SelectedValue); // DB dùng smallint -> C# Int16
                short soLuong = 0;

                if (!short.TryParse(txtSoLuong.Text, out soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ (>0).", "Thông báo");
                    return;
                }

                // Gọi SP: sp_ThemDichVuSuDung
                // Tham số: @MaNP, @MaDV, @SoLuong, @GhiChu
                string spName = "sp_ThemDichVuSuDung";
                SqlParameter[] p = {
                    new SqlParameter("@MaNP", this.maNP),
                    new SqlParameter("@MaDV", maDV),
                    new SqlParameter("@SoLuong", soLuong),
                    new SqlParameter("@GhiChu", DBNull.Value)
                };

                if (DatabaseHelper.ExecuteNonQuery(spName, p, CommandType.StoredProcedure))
                {
                    MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo");
                    LoadLichSuDichVu(); // Refresh lại lưới
                    txtSoLuong.Text = "1"; // Reset số lượng về 1
                }
                else
                {
                    MessageBox.Show("Thêm dịch vụ thất bại.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm dịch vụ: " + ex.Message);
            }
        }

        // --- HÀM HỖ TRỢ ---
        private string GetTrangThaiString(int statusId)
        {
            switch (statusId)
            {
                case 1: return "Sẵn sàng đón khách";
                case 2: return "Phòng chưa dọn dẹp";
                case 0: return "Đang bảo trì";
                default: return "Không xác định";
            }
        }

        private void btnCloseFrm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThemDV_Click_1(object sender, EventArgs e)
        {
            // Kiểm tra: Phòng phải có khách (MaNP > 0) mới thêm dịch vụ được
            if (this.maNP <= 0)
            {
                MessageBox.Show("Phòng chưa có khách check-in, không thể thêm dịch vụ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy dữ liệu từ giao diện
                // Lưu ý: Kiểm tra lại cboDichVu có dữ liệu chưa
                if (cboDichVu.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn dịch vụ!", "Thông báo");
                    return;
                }

                short maDV = Convert.ToInt16(cboDichVu.SelectedValue);
                short soLuong = 0;

                if (!short.TryParse(txtSoLuong.Text, out soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ (>0).", "Thông báo");
                    return;
                }

                // Gọi SP: sp_ThemDichVuSuDung
                string spName = "sp_ThemDichVuSuDung";
                SqlParameter[] p = {
                    new SqlParameter("@MaNP", this.maNP),
                    new SqlParameter("@MaDV", maDV),
                    new SqlParameter("@SoLuong", soLuong),
                    new SqlParameter("@GhiChu", DBNull.Value)
                };

                if (DatabaseHelper.ExecuteNonQuery(spName, p, CommandType.StoredProcedure))
                {
                    MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo");
                    LoadLichSuDichVu(); // Refresh lại lưới
                    txtSoLuong.Text = "1"; // Reset số lượng về 1
                }
                else
                {
                    MessageBox.Show("Thêm dịch vụ thất bại.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm dịch vụ: " + ex.Message);
            }
        }
    }
}