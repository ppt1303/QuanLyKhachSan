using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyKhachSan.BLL; // Nhớ đảm bảo namespace này đúng với project của cậu

namespace QuanLyKhachSan.GUI


{

    
    public partial class CheckOut : Form
    {
        private QuanLyKhachSan.BLL.PhuThuBLL _bllPhuThu = new QuanLyKhachSan.BLL.PhuThuBLL();
        // 1. KHAI BÁO BIẾN
        private int _maNP; // Mã nhận phòng được truyền từ form bên kia sang
        private BookingBLL _bll = new BookingBLL(); // Gọi lớp logic BLL

        // Các biến lưu tiền để cộng trừ
        private decimal _tienPhong = 0;
        private decimal _tienDichVu = 0;
        private decimal _tienPhuThu = 0;
        private decimal _tienCoc = 0;
        private decimal _giaPhongNgay = 0; // Giá gốc 1 ngày của phòng đó

        // 2. CONSTRUCTOR (Quan trọng: Phải nhận tham số maNP)
        public CheckOut(int maNP)
        {
            InitializeComponent();
            _maNP = maNP; // Lưu lại mã này để dùng xuyên suốt Form
            LoadComboPhuThu();
            KiemTraTraMuon();
            HienThiDanhSachPhuThu();
        }

        // 3. KHI FORM VỪA MỞ LÊN
        private void CheckOut_Load(object sender, EventArgs e)
        {
            try
            {
                // Cấu hình DateTimePicker hiển thị ngày giờ đẹp
                dtpNgayRa.Format = DateTimePickerFormat.Custom;
                dtpNgayRa.CustomFormat = "dd/MM/yyyy HH:mm";

                // Load dữ liệu từ SQL lên
                LoadThongTinPhong();
                LoadDichVu();
                LoadPhuThu();
                HienThiDanhSachPhuThu();

                // Tính toán tổng tiền lần đầu tiên
                TinhTienPhongDisplay();
                TinhTongCongFinal();

                // Cấu hình ô giảm giá chỉ cho nhập từ 0 đến 100
                numGiamGia.Minimum = 0;
                numGiamGia.Maximum = 100;

                // Nếu muốn hiển thị đẹp hơn thì thêm hậu tố % (tùy chỉnh)
                numGiamGia.DecimalPlaces = 0; // Chỉ nhập số nguyên
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // --- PHẦN 4: HÀM LOAD DỮ LIỆU TỪ BLL ---

        private void LoadThongTinPhong()
        {
            // Gọi hàm BLL lấy thông tin Header
            DataTable dt = _bll.GetCheckOutInfo(_maNP);

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];

                // Gán dữ liệu vào các Label bên trái
                lblPhong.Text = r["TenPhong"].ToString() + " - " + r["TenLP"].ToString();
                lblKhachHang.Text = r["HoTen"].ToString();

                // Xử lý ngày giờ
                if (r["ThoiGianNhan"] != DBNull.Value)
                {
                    DateTime ngayVao = Convert.ToDateTime(r["ThoiGianNhan"]);
                    lblNgayVao.Text = ngayVao.ToString("dd/MM/yyyy HH:mm");
                }

                // Mặc định giờ ra là hiện tại (Lúc mở form)
                dtpNgayRa.Value = DateTime.Now;

                // Lưu giá phòng vào biến để tí nữa tính toán
                _giaPhongNgay = Convert.ToDecimal(r["GiaTheoNgay"]);
                lblDonGia.Text = _giaPhongNgay.ToString("N0") + " VNĐ/ngày";

                // Xử lý Tiền Cọc (Logic chia đều nếu đi theo đoàn)
                decimal tongCoc = Convert.ToDecimal(r["TienCoc"]);
                int soPhong = Convert.ToInt32(r["SoPhongDoan"]);

                if (soPhong > 0)
                    _tienCoc = tongCoc / soPhong;
                else
                    _tienCoc = 0;

                // Hiển thị tiền cọc sang bên phải (Thêm dấu trừ màu xanh)
                lblTienCoc.Text = "-" + _tienCoc.ToString("N0");
            }
        }

        private void LoadDichVu()
        {
            // Lấy danh sách dịch vụ đổ vào Grid
            DataTable dt = _bll.GetServices(_maNP);
            dgvDichVu.DataSource = dt;
            FormatGrid(dgvDichVu);

            // Cộng tổng tiền cột "Thành Tiền"
            _tienDichVu = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row["Thành Tiền"] != DBNull.Value)
                    _tienDichVu += Convert.ToDecimal(row["Thành Tiền"]);
            }
            // Hiển thị sang bảng bên phải
            lblTongDichVu_Right.Text = _tienDichVu.ToString("N0");
            lblThanhTienDichVu.Text = _tienDichVu.ToString("N0") + " VNĐ";
        }

        private void LoadPhuThu()
        {
            // Lấy danh sách phụ thu đổ vào Grid
            DataTable dt = _bll.GetSurcharges(_maNP);
            dgvPhuThu.DataSource = dt;
            FormatGrid(dgvPhuThu);

            // Cộng tổng tiền
            _tienPhuThu = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row["Thành Tiền"] != DBNull.Value)
                    _tienPhuThu += Convert.ToDecimal(row["Thành Tiền"]);
            }
            // Hiển thị sang bảng bên phải
            lblTongPhuThu_Right.Text = _tienPhuThu.ToString("N0");
            lblThanhTienPhuThu.Text = _tienPhuThu.ToString("N0") + " VNĐ";
            HienThiDanhSachPhuThu();
        }

        // --- PHẦN 5: LOGIC TÍNH TIỀN (Update liên tục) ---

        private void TinhTienPhongDisplay()
        {
            // Lấy giờ vào từ Label (cần parse lại)
            DateTime vao;
            if (!DateTime.TryParseExact(lblNgayVao.Text, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out vao))
            {
                return; // Nếu lỗi format ngày thì bỏ qua
            }

            DateTime ra = dtpNgayRa.Value;

            // Tính khoảng cách
            TimeSpan duration = ra - vao;

            // Logic tính tiền: Ở đây mình làm tròn lên theo Ngày (Bạn có thể sửa thành theo Giờ nếu muốn)
            // Ví dụ: 1 ngày 1 phút -> Tính 2 ngày
            int soNgay = (int)Math.Ceiling(duration.TotalDays);

            if (soNgay < 1) soNgay = 1; // Ở ít nhất cũng tính 1 ngày

            // Hiển thị Text "2 ngày 4 giờ" cho đẹp
            lblThoiGianO.Text = string.Format("{0} ngày ({1} giờ)", soNgay, Math.Floor(duration.TotalHours));

            // Tính ra tiền
            _tienPhong = soNgay * _giaPhongNgay;

            // Cập nhật lên giao diện (Cả bên trái và bên phải)
            lblThanhTienPhong.Text = _tienPhong.ToString("N0") + " VNĐ";
            lblTongTienPhong_Right.Text = _tienPhong.ToString("N0");
        }

        private void TinhTongCongFinal()
        {
            // 1. Tính TỔNG CHI PHÍ phát sinh (Phòng + Dịch vụ + Phụ thu)
            decimal tongChiPhi = _tienPhong + _tienDichVu + _tienPhuThu;

            // 2. Lấy phần trăm giảm giá từ ô nhập
            decimal phanTramGiam = numGiamGia.Value;

            // 3. Tính ra số tiền giảm cụ thể
            // Công thức: Tổng chi phí * (Số % / 100)
            decimal tienGiamGia = tongChiPhi * (phanTramGiam / 100);

            // 4. Công thức chốt sổ: (Tổng chi phí - Tiền giảm giá) - Tiền cọc đã đóng
            decimal tongThanhToan = (tongChiPhi - tienGiamGia) - _tienCoc;

            // 5. Chặn số âm (đề phòng giảm giá 100% mà khách cọc nhiều hơn tiền phòng)
            if (tongThanhToan < 0) tongThanhToan = 0;

            // 6. Hiển thị kết quả
            lblTongCongFinal.Text = tongThanhToan.ToString("N0") + " VNĐ";

            // (Mẹo) Bạn có thể đổi cái Label tiêu đề "Giảm giá" thành hiển thị số tiền cho dễ nhìn
            // Ví dụ label bên trái dòng giảm giá tên là label20
            // label20.Text = $"Giảm giá ({phanTramGiam}%): -{tienGiamGia.ToString("N0")}";
        }

        // --- PHẦN 6: CÁC SỰ KIỆN (Events) ---

        // Khi chỉnh giờ ra -> Tính lại tiền phòng -> Tính lại tổng
        private void dtpNgayRa_ValueChanged(object sender, EventArgs e)
        {
            TinhTienPhongDisplay();
            TinhTongCongFinal();
        }

        // Khi nhập giảm giá -> Tính lại tổng
        private void numGiamGia_ValueChanged(object sender, EventArgs e)
        {
            TinhTongCongFinal();
        }

 

        // Helper làm đẹp GridView (Tùy chọn)
        private void FormatGrid(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;
            // Format cột tiền
            if (dgv.Columns["Thành Tiền"] != null)
                dgv.Columns["Thành Tiền"].DefaultCellStyle.Format = "N0";
            if (dgv.Columns["Đơn Giá"] != null)
                dgv.Columns["Đơn Giá"].DefaultCellStyle.Format = "N0";
        }

        //Nút thanh toán
        private void btnThanhToan_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận thanh toán và trả phòng?", "Xác nhận",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            // Lấy kiểu thanh toán (1: Tiền mặt, 2: CK)
            int kieuTT = rdoTienMat.Checked ? 1 : 2;

            // Gọi BLL để lưu xuống DB (Hàm này bạn đã viết ở bước trước)
            bool ketQua = _bll.ThanhToanFull(_maNP, "Bình thường", kieuTT);

            if (ketQua)
            {
                MessageBox.Show("Thanh toán thành công! Phòng đã được trả.", "Thông báo");
                this.Close(); // Đóng form quay về sơ đồ
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi lưu Database.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboLoaiPhuThu_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ten = cboLoaiPhuThu.Text.ToLower();

            // Nếu là Check-in/out -> Khóa cứng số lượng là 1
            if (ten.Contains("check") || ten.Contains("sớm") || ten.Contains("muộn"))
            {
                numSoLuong.Value = 1;
                numSoLuong.Enabled = false; // Khóa lại
            }
            else
            {
                numSoLuong.Enabled = true; // Mở ra cho nhập (Vỡ ly, thêm người...)
            }
        }

        // Gọi hàm này khi Form vừa mở lên
        private void LoadComboPhuThu()
        {
            // Lấy danh sách từ BLL (SELECT * FROM PHUTHU)
            cboLoaiPhuThu.DataSource = _bllPhuThu.LayDanhSachPhuThu();
            cboLoaiPhuThu.DisplayMember = "Ten";
            cboLoaiPhuThu.ValueMember = "MaPhuThu";
        }

        private void btnThemPhuThu_Click(object sender, EventArgs e)
        {
            try
            {
                int maPT = Convert.ToInt32(cboLoaiPhuThu.SelectedValue);
                int sl = (int)numSoLuong.Value;
                string ghiChu = txtGhiChu.Text;

                // Gọi xuống SQL lưu lại
                if (_bllPhuThu.ThemPhuThu(_maNP, maPT, sl, ghiChu))
                {
                    MessageBox.Show("Thêm thành công!");

                    // QUAN TRỌNG: Load lại thông tin để Tổng tiền tự nhảy lên
                    LoadPhuThu();
                    HienThiDanhSachPhuThu();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        
    }

        private void KiemTraTraMuon()
        {
            // 1. Lấy ngày trả dự kiến từ Booking
            DateTime ngayDuKien = _bllPhuThu.GetExpectedCheckout(_maNP);

            // 2. Thiết lập giờ Checkout chuẩn là 12:00 trưa của ngày đó
            DateTime gioCheckOutChuan = new DateTime(ngayDuKien.Year, ngayDuKien.Month, ngayDuKien.Day, 12, 0, 0);

            // 3. So sánh với giờ hiện tại
            DateTime bayGio = DateTime.Now;

            if (bayGio > gioCheckOutChuan)
            {
                // Tính độ lệch (TimeSpan)
                TimeSpan tre = bayGio - gioCheckOutChuan;

                // Làm tròn số giờ (Ví dụ: 1 tiếng 15 phút -> tính là 2 tiếng cho máu, hoặc 1 tùy cậu)
                int soGioTre = (int)Math.Ceiling(tre.TotalHours);

                // Nếu trễ > 0 thì hiện thông báo gợi ý
                if (soGioTre > 0)
                {
                    string thongBao = string.Format("Khách trả muộn {0} giờ so với quy định (12:00 - {1:dd/MM}).\nBạn có muốn tính phí trả muộn không?",
                                                    soGioTre, ngayDuKien);

                    if (MessageBox.Show(thongBao, "Phát hiện trả muộn", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // --- TỰ ĐỘNG CHỌN COMBOBOX VÀ ĐIỀN SỐ LƯỢNG ---

                        // 1. Chọn loại phụ thu là "Check-out muộn" (Tìm theo tên)
                        cboLoaiPhuThu.SelectedIndex = cboLoaiPhuThu.FindString("Check-out muộn");

                        // 2. Mở khóa ô số lượng (để điền số giờ) - Quan trọng!
                        numSoLuong.Enabled = true;

                        // 3. Điền số giờ trễ vào
                        numSoLuong.Value = soGioTre;

                        // 4. Ghi chú tự động
                        txtGhiChu.Text = "Tự động tính: Trễ " + soGioTre + " tiếng";
                    }
                }
            }
        }

        private void panel2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblThanhTienPhuThu_Click(object sender, EventArgs e)
        {

        }

        // 1. Hàm hiển thị (Đơn giản hóa tối đa)
        private void HienThiDanhSachPhuThu()
        {
            // 1. Lấy dữ liệu mới nhất từ SQL (Lúc này SQL đã có MaSDPT rồi)
            DataTable dt = _bllPhuThu.LayPhuThuTheoPhong(_maNP);

            // 2. ĐẬP ĐI XÂY LẠI GIAO DIỆN (Code cục súc trị lỗi lì lợm)
            dgvPhuThu.DataSource = null;
            dgvPhuThu.Columns.Clear(); // Xóa sạch cột cũ
            dgvPhuThu.AutoGenerateColumns = true; // <--- BẮT BUỘC: Tự động hiện tất cả cột

            // 3. Đổ dữ liệu vào
            dgvPhuThu.DataSource = dt;

            // (Tùy chọn) Đổi tên cột đầu tiên cho dễ nhìn
            if (dgvPhuThu.Columns.Count > 0)
                dgvPhuThu.Columns[0].HeaderText = "Mã ID (Cấm xóa)";
        }

        // 2. Nút Xóa (Lấy theo chỉ số cột - Index)
        private void btnXoaPhuThu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem có chọn dòng nào chưa
            if (dgvPhuThu.SelectedRows.Count == 0) return;

            if (MessageBox.Show("Xóa dòng này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    // --- ĐOẠN QUAN TRỌNG NHẤT ---
                    // Ép kiểu dòng đang chọn về DataRowView (Dữ liệu gốc từ SQL)
                    System.Data.DataRowView row = (System.Data.DataRowView)dgvPhuThu.SelectedRows[0].DataBoundItem;

                    // Lấy cột "MaSDPT" từ trong DỮ LIỆU GỐC (SQL)
                    // Lúc này đéo cần quan tâm trên giao diện tên cột là gì nữa
                    int maSDPT = Convert.ToInt32(row["MaSDPT"]);
                    // -----------------------------

                    // Gọi hàm xóa
                    if (_bllPhuThu.XoaPhuThu(maSDPT))
                    {
                        MessageBox.Show("Đã xóa thành công!");
                        HienThiDanhSachPhuThu(); // Load lại bảng
                                                 // Gọi hàm cập nhật lại tổng tiền ở đây...
                    }
                }
                catch (Exception ex)
                {
                    // Nếu nó báo lỗi ở đây nghĩa là SQL chưa trả về cột MaSDPT -> Quay lại chửi SQL
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
    }
}
    //QuanLyKhachSan.GUI.CheckOut frm = new QuanLyKhachSan.GUI.CheckOut(maNP);

    //// ShowDialog: Mở form và CHẶN không cho thao tác form cũ cho đến khi tắt form này
    //frm.ShowDialog();

