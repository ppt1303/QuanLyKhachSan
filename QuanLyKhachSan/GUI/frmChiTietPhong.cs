using QuanLyKhachSan.BLL;
using QuanLyKhachSan.DAL; // Cần dùng để truy cập DatabaseHelper
using System;
using System.Data;
using System.Data.SqlClient; // Cần dùng cho SqlParameter
using System.Windows.Forms;
using System.Drawing;

namespace QuanLyKhachSan.GUI
{
    public partial class frmChiTietPhong : Form
    {
        public frmChiTietPhong()
        {
            InitializeComponent();
        }

        private int maNP;
        private int maPhong;

        // 2. Khai báo BLL để sử dụng (Giữ lại để thêm dịch vụ)
        private PhongBLL phongBLL = new PhongBLL();
        private BookingBLL bookingBLL = new BookingBLL();

        // Constructor mới
        public frmChiTietPhong(int maNP, int maPhong)
        {
            InitializeComponent();
            this.maNP = maNP;
            this.maPhong = maPhong;

            // Đảm bảo nút được liên kết trong Designer, sau đó code sẽ chạy các phương thức dưới đây
            LoadDanhSachDichVu();
            LoadLichSuDichVu();
            LoadDanhSachTrangThaiPhong();
            LoadThongTinChung();
        }

        // --- HÀM HỖ TRỢ TRẠNG THÁI ---

        // Map ID (0, 1, 2) sang chuỗi trạng thái hiển thị
        private string GetTrangThaiString(int maTrangThai)
        {
            switch (maTrangThai)
            {
                case 1: return "Sẵn sàng (Trống)";
                case 2: return "Cần dọn dẹp (Dơ)";
                case 0: return "Đang bảo trì/Sửa chữa";
                default: return "Không xác định";
            }
        }

        // Hàm lấy trạng thái hiện tại của phòng từ DB (Sử dụng DatabaseHelper trực tiếp)
        private int GetTrangThaiPhongHienTai()
        {
            // Truy vấn trực tiếp vào bảng PHONG
            string query = "SELECT TrangThaiPhong FROM PHONG WHERE MaPhong = @MaPhong";
            SqlParameter[] para = { new SqlParameter("@MaPhong", this.maPhong) };

            DataTable dt = DatabaseHelper.GetData(query, para, CommandType.Text);

            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["TrangThaiPhong"] != DBNull.Value)
            {
                return Convert.ToInt32(dt.Rows[0]["TrangThaiPhong"]);
            }
            return 1; // Mặc định là Sẵn sàng (1)
        }

        // Hàm tải danh sách trạng thái phòng cho ComboBox
        private void LoadDanhSachTrangThaiPhong()
        {
            // Tạo dữ liệu mô phỏng dựa trên mã trạng thái trong DB: 0, 1, 2
            DataTable dtTrangThai = new DataTable();
            dtTrangThai.Columns.Add("MaTrangThai", typeof(int));
            dtTrangThai.Columns.Add("TenTrangThai", typeof(string));

            dtTrangThai.Rows.Add(1, "Sẵn sàng (Trống)");
            dtTrangThai.Rows.Add(2, "Cần dọn dẹp (Dơ)");
            dtTrangThai.Rows.Add(0, "Đang bảo trì/Sửa chữa");

            cboTrangThaiPhong.DataSource = dtTrangThai;
            cboTrangThaiPhong.DisplayMember = "TenTrangThai";
            cboTrangThaiPhong.ValueMember = "MaTrangThai";
        }

        // --- HÀM XỬ LÝ SỰ KIỆN NÚT ---

        // Xử lý nút LƯU (Cập nhật trạng thái phòng)
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboTrangThaiPhong.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn trạng thái phòng mới.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maTrangThaiMoi = Convert.ToInt32(cboTrangThaiPhong.SelectedValue);
                string ghiChu = "Cập nhật thủ công từ Form Chi Tiết";

                // GỌI STORED PROCEDURE: sp_CapNhatTrangThaiDonDep (Có sẵn trong nhat.sql)
                string procName = "sp_CapNhatTrangThaiDonDep";
                SqlParameter[] para = {
                    new SqlParameter("@MaPhong", this.maPhong),
                    new SqlParameter("@TrangThai", maTrangThaiMoi),
                    new SqlParameter("@GhiChu", ghiChu)
                };

                if (DatabaseHelper.ExecuteNonQuery(procName, para, CommandType.StoredProcedure))
                {
                    MessageBox.Show("Cập nhật trạng thái phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Đóng Form sau khi lưu
                }
                else
                {
                    MessageBox.Show("Cập nhật trạng thái phòng thất bại. Lỗi hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý nút ĐÓNG Form
        private void btnCloseFrm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- CÁC HÀM TẢI DỮ LIỆU ---

        private (string TenPhong, string TenLP) GetRoomDetailsById()
        {
            string query = @"
                SELECT P.TenPhong, LP.TenLP 
                FROM PHONG P 
                JOIN LOAIPHONG LP ON P.MaLP = LP.MaLP 
                WHERE P.MaPhong = @MaPhong";

            SqlParameter[] para = { new SqlParameter("@MaPhong", this.maPhong) };

            DataTable dt = DatabaseHelper.GetData(query, para, CommandType.Text);

            if (dt != null && dt.Rows.Count > 0)
            {
                return (dt.Rows[0]["TenPhong"].ToString(), dt.Rows[0]["TenLP"].ToString());
            }
            return ("Lỗi tải phòng", "");
        }

        // --- HÀM TẢI DỮ LIỆU CŨ (SỬA ĐỂ TẢI TÊN PHÒNG LUÔN) ---
        private void LoadThongTinChung()
        {
            // BƯỚC 1: LUÔN HIỂN THỊ TÊN PHÒNG VÀ LOẠI PHÒNG
            var roomDetails = GetRoomDetailsById();
            lblTenPhong.Text = roomDetails.TenPhong + " (" + roomDetails.TenLP + ")";

            // Tải thông tin khách đang ở phòng 
            DataTable dtGuest = bookingBLL.GetRoomAndGuestDetails(this.maNP);

            if (dtGuest != null && dtGuest.Rows.Count > 0 && this.maNP > 0)
            {
                DataRow row = dtGuest.Rows[0];
                // KHÔNG CẦN SET LẠI lblTenPhong
                lblTenKhach.Text = row["HoTen"].ToString();
                lblGioNhan.Text = Convert.ToDateTime(row["ThoiGianNhan"]).ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                // Phòng Trống/Đang Sửa/Bảo trì (MaNP=0)
                lblTenKhach.Text = "Không có khách đang ở";
                lblGioNhan.Text = "---";
            }

            // BƯỚC 2: HIỂN THỊ TRẠNG THÁI HIỆN TẠI
            int currentStatusId = GetTrangThaiPhongHienTai();
            string currentStatusName = GetTrangThaiString(currentStatusId);

            lblTieuDeTrangThai.Text = currentStatusName;

            if (cboTrangThaiPhong.Items.Count > 0)
            {
                cboTrangThaiPhong.SelectedValue = currentStatusId;
            }
        }

        private void LoadDanhSachDichVu()
        {
            // Tải danh sách dịch vụ vào ComboBox
            DataTable dtDichVu = bookingBLL.LoadAllDichVu();
            cboDichVu.DataSource = dtDichVu;
            cboDichVu.DisplayMember = "TenDV";
            cboDichVu.ValueMember = "MaDV";
        }

        public void LoadLichSuDichVu()
        {
            // Tải lịch sử chi tiêu (Dịch vụ và Phụ thu) vào DataGridView
            DataTable dtChiTieu = bookingBLL.LoadLichSuChiTieu(this.maNP);
            dgvLichSuDV.DataSource = dtChiTieu;
        }

        // --- CÁC HANDLER KHÁC ---

        private void btnThemDV_Click_1(object sender, EventArgs e)
        {
            // Logic thêm dịch vụ (Giữ nguyên)
            try
            {
                if (cboDichVu.SelectedValue == null || string.IsNullOrEmpty(txtSoLuong.Text))
                {
                    MessageBox.Show("Vui lòng chọn dịch vụ và nhập số lượng.", "Cảnh báo");
                    return;
                }

                short maDV = Convert.ToInt16(cboDichVu.SelectedValue);
                short soLuong = Convert.ToInt16(txtSoLuong.Text);

                if (soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0.", "Lỗi nhập liệu");
                    return;
                }

                if (bookingBLL.ThemDichVuSuDung(this.maNP, maDV, soLuong))
                {
                    MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo");
                    LoadLichSuDichVu();
                    txtSoLuong.Text = "1";
                }
                else
                {
                    MessageBox.Show("Thêm dịch vụ thất bại.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dữ liệu nhập không hợp lệ hoặc lỗi hệ thống: " + ex.Message, "Lỗi");
            }
        }

        // Giữ lại các handler trống
        private void frmChiTietPhong_Load(object sender, EventArgs e) { }
        private void btnCheckOut_Click(object sender, EventArgs e) { }
        private void cboDichVu_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void lblTieuDeTrangThai_Click(object sender, EventArgs e) { }
        private void cboTrangThaiPhong_SelectedIndexChanged(object sender, EventArgs e) { }
        private void btnThemDV_Click(object sender, EventArgs e)
        {
            btnThemDV_Click_1(sender, e);
        }

        private void btnLuu_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (cboTrangThaiPhong.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn trạng thái phòng mới.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maTrangThaiMoi = Convert.ToInt32(cboTrangThaiPhong.SelectedValue);
                string ghiChu = "Cập nhật thủ công từ Form Chi Tiết";

                // GỌI STORED PROCEDURE: sp_CapNhatTrangThaiDonDep
                string procName = "sp_CapNhatTrangThaiDonDep";
                SqlParameter[] para = {
                    new SqlParameter("@MaPhong", this.maPhong),
                    new SqlParameter("@TrangThai", maTrangThaiMoi),
                    new SqlParameter("@GhiChu", ghiChu)
                };

                if (DatabaseHelper.ExecuteNonQuery(procName, para, CommandType.StoredProcedure))
                {
                    MessageBox.Show("Cập nhật trạng thái phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Đóng Form sau khi lưu
                }
                else
                {
                    MessageBox.Show("Cập nhật trạng thái phòng thất bại. Lỗi hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCloseFrm_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}