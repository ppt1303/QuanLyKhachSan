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
    }
}
    //QuanLyKhachSan.GUI.CheckOut frm = new QuanLyKhachSan.GUI.CheckOut(maNP);

    //// ShowDialog: Mở form và CHẶN không cho thao tác form cũ cho đến khi tắt form này
    //frm.ShowDialog();

