using System;
using System.Data;
using System.Drawing; // Để dùng Color, Font
using System.Windows.Forms;
using Guna.UI2.WinForms; // Thư viện Guna
using QuanLyKhachSan.BLL; // Gọi logic

namespace QuanLyKhachSan.GUI
{
    public partial class frmDatPhong : Form
    {
        // Khai báo Logic & Biến tạm
        BookingBLL bll = new BookingBLL();
        int _maPhongDangChon = 0;
        decimal _giaPhongHienTai = 0;

        public frmDatPhong()
        {
            InitializeComponent();
        }

        // 1. Khi Form vừa mở lên
        private void frmDatPhong_Load(object sender, EventArgs e)
        {
            // Mặc định ngày trên Header
            dtpNgayDen.Value = DateTime.Now;
            dtpNgayDi.Value = DateTime.Now.AddDays(1);

            // Ẩn khu vực danh sách đặt trước đi cho gọn
            dgvPhongDaDat.Visible = false;
            btnNhanPhong.Visible = false;
            btnDoiPhong.Visible = false;

            LoadSoDoPhong(); // Vẽ sơ đồ
        }

        // 2. Hàm vẽ sơ đồ phòng (Core Function)
        // --- Thay thế toàn bộ hàm LoadSoDoPhong cũ ---

        private void LoadSoDoPhong()
        {
            flowLayoutPanel1.Controls.Clear(); // Xóa nút cũ

            // GỌI HÀM MỚI trong BLL (Lấy tất cả phòng + Trạng thái)
            DataTable dt = bll.GetSoDoPhong(dtpNgayDen.Value, dtpNgayDi.Value);

            if (dt == null || dt.Rows.Count == 0) return;

            foreach (DataRow row in dt.Rows)
            {
                // 1. Tạo nút
                Guna2TileButton btn = new Guna2TileButton();
                btn.Width = 100;
                btn.Height = 100;
                btn.BorderRadius = 10;
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.ForeColor = Color.White;

                // 2. Lấy thông tin
                string tenPhong = row["TenPhong"].ToString();
                string loaiPhong = row["TenLP"].ToString();
                decimal gia = Convert.ToDecimal(row["GiaMacDinh"]);
                int isBooked = Convert.ToInt32(row["TrangThaiBan"]); // 1: Bận, 0: Trống

                // 3. XỬ LÝ MÀU SẮC (Quan trọng nhất)
                if (isBooked == 1)
                {
                    // Phòng ĐÃ CÓ NGƯỜI -> Màu Đỏ
                    btn.FillColor = Color.Red;
                    btn.Text = $"{tenPhong}\n{loaiPhong}\n(Đã đặt)";
                    btn.Enabled = false; // (Tùy chọn) Khóa không cho bấm hoặc vẫn cho bấm để xem
                }
                else
                {
                    // Phòng TRỐNG -> Màu Xanh
                    btn.FillColor = Color.FromArgb(46, 204, 113);
                    btn.Text = $"{tenPhong}\n{loaiPhong}\n{gia:N0}";
                }

                // 4. Lưu dữ liệu vào Tag
                btn.Tag = row;

                // 5. Sự kiện Click
                // Nếu bạn muốn phòng đỏ vẫn bấm được (để đổi phòng) thì đừng set Enabled = false ở trên
                btn.Click += BtnPhong_Click;

                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        // 3. Sự kiện khi Click vào 1 ô phòng
        private void BtnPhong_Click(object sender, EventArgs e)
        {
            Guna2TileButton btn = (Guna2TileButton)sender;
            DataRow data = (DataRow)btn.Tag;

            // Kiểm tra nếu phòng đang Đỏ (Bận)
            if (btn.FillColor == Color.Red)
            {
                MessageBox.Show("Phòng này đã có người đặt trong khoảng thời gian bạn chọn!");
                // Reset lựa chọn
                _maPhongDangChon = 0;
                lblPhongDangChon.Text = "PHÒNG: ĐÃ BẬN";
                lblPhongDangChon.ForeColor = Color.Red;
                return;
            }

            // Nếu phòng Xanh (Trống) -> Xử lý bình thường
            _maPhongDangChon = Convert.ToInt32(data["MaPhong"]);
            _giaPhongHienTai = Convert.ToDecimal(data["GiaMacDinh"]);

            lblPhongDangChon.Text = $"PHÒNG: {data["TenPhong"]}";
            lblPhongDangChon.ForeColor = Color.Green;

            txtTienCoc.Text = (_giaPhongHienTai * 0.3m).ToString("N0");
        }

        // 4. Nút "Tìm phòng" (Lọc ngày)
        private void btnLocPhong_Click(object sender, EventArgs e)
        {
            LoadSoDoPhong();
        }

        // 5. Nút "Tìm khách"
        // 5. Nút "Tìm khách" - Đã nâng cấp Full thông tin
        private void btnTim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text)) return;

            // 1. Tìm thông tin khách
            DataRow kh = bll.GetKhachHangInfo(txtSearch.Text);

            if (kh != null)
            {
                // Điền thông tin khách
                txtHoTen.Text = kh["HoTen"].ToString();
                txtCCCD.Text = kh["CCCD"].ToString();
                txtSDT.Text = kh["SDT"].ToString();
                cboGioiTinh.Text = kh["GioiTinh"].ToString();

                // --- LOGIC MỚI: HIỆN DANH SÁCH PHÒNG ĐÃ ĐẶT ---
                int maKH = Convert.ToInt32(kh["MaKH"]);
                DataTable dtBooking = bll.GetPhongDaDat(maKH);

                if (dtBooking.Rows.Count > 0)
                {
                    // Có phòng đặt trước -> Hiện bảng lên
                    dgvPhongDaDat.Visible = true;
                    btnNhanPhong.Visible = true;
                    btnDoiPhong.Visible = true;

                    dgvPhongDaDat.DataSource = dtBooking;

                    // Ẩn cột ID đi cho đẹp (chỉ hiện Tên phòng, Loại, Ngày)
                    if (dgvPhongDaDat.Columns.Contains("MaDP")) dgvPhongDaDat.Columns["MaDP"].Visible = false;
                    if (dgvPhongDaDat.Columns.Contains("MaPhong")) dgvPhongDaDat.Columns["MaPhong"].Visible = false;
                }
                else
                {
                    // Không có phòng đặt trước -> Ẩn đi
                    dgvPhongDaDat.Visible = false;
                    btnNhanPhong.Visible = false;
                    btnDoiPhong.Visible = false;
                }

                MessageBox.Show("Đã tìm thấy khách hàng!");
            }
            else
            {
                MessageBox.Show("Khách mới. Hãy nhập thông tin và Lưu khách mới.");
                // Xóa trắng & Ẩn bảng
                txtHoTen.Clear(); txtCCCD.Clear();
                dgvPhongDaDat.Visible = false;
                btnNhanPhong.Visible = false;
                btnDoiPhong.Visible = false;
            }
        }

        // 6. Nút "Lưu khách mới"
        private void btnThemKhach_Click(object sender, EventArgs e)
        {
            // 1. Lấy dữ liệu từ giao diện
            string ten = txtHoTen.Text;
            string cccd = txtCCCD.Text;
            string sdt = txtSDT.Text;
            string gioitinh = cboGioiTinh.Text; // Lấy từ ComboBox
            DateTime ngaysinh = dtpNgaySinh.Value;
            string quoctich = txtQuocTich.Text;

            // 2. Kiểm tra sơ bộ
            if (string.IsNullOrEmpty(gioitinh))
            {
                MessageBox.Show("Vui lòng chọn Giới tính!");
                return;
            }

            // 3. Gọi BLL (Lúc này hàm AddKhachHang đã có đủ tham số)
            string ketQua = bll.AddKhachHang(ten, cccd, sdt, gioitinh, ngaysinh, quoctich);

            MessageBox.Show(ketQua);
        }

        // 7. Nút "Xác nhận đặt" (Final)
        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra chọn phòng trên sơ đồ
            if (_maPhongDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng trống trên sơ đồ để đặt mới!");
                return;
            }

            // 2. Kiểm tra khách
            if (string.IsNullOrEmpty(txtCCCD.Text))
            {
                MessageBox.Show("Chưa có thông tin khách hàng!");
                return;
            }

            // Lấy ID khách (Từ ô CCCD đang hiện)
            DataRow kh = bll.GetKhachHangInfo(txtCCCD.Text);
            if (kh == null)
            {
                MessageBox.Show("Vui lòng Lưu khách mới trước khi đặt!");
                return;
            }
            int maKH = Convert.ToInt32(kh["MaKH"]);

            // 3. Lấy tiền cọc
            decimal tienCoc = 0;
            decimal.TryParse(txtTienCoc.Text.Replace(",", ""), out tienCoc);

            // 4. Đặt phòng (Lấy ngày từ HEADER)
            string kq = bll.BookRoom(maKH, _maPhongDangChon, dtpNgayDen.Value, dtpNgayDi.Value, tienCoc);

            MessageBox.Show(kq);

            if (kq.Contains("thành công"))
            {
                LoadSoDoPhong();
                // Nếu là khách cũ, load lại bảng danh sách để thấy phòng mới đặt hiện ra
                btnTim_Click(null, null);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // 1. Xóa sạch chữ trong các ô nhập
            txtSearch.Clear();
            txtHoTen.Clear();
            txtCCCD.Clear();
            txtSDT.Clear();
            txtTienCoc.Clear();

            // 2. Reset các ô chọn về mặc định
            txtQuocTich.Text = "Việt Nam";    // Mặc định lại là VN
            cboGioiTinh.StartIndex = 0;      // -1 nghĩa là không chọn gì cả (trắng)
            dtpNgaySinh.Value = DateTime.Now; // Reset ngày sinh về hôm nay

            // 3. (Tùy chọn) Nếu bạn muốn Reset luôn cái phòng đang chọn thì mở comment 3 dòng dưới này ra:
            /*
            _maPhongDangChon = 0;
            lblPhongDangChon.Text = "PHÒNG: CHƯA CHỌN";
            lblPhongDangChon.ForeColor = Color.Black;
            */

            // Đặt con trỏ chuột quay về ô Tìm kiếm để nhập cho nhanh
            txtSearch.Focus();
        }

        private void btnNhanPhong_Click(object sender, EventArgs e)
        {
            // Kiểm tra đã chọn dòng nào trong bảng chưa
            if (dgvPhongDaDat.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn phòng trong bảng danh sách để nhận!");
                return;
            }

            int maDP = Convert.ToInt32(dgvPhongDaDat.CurrentRow.Cells["MaDP"].Value);
            string tenPhong = dgvPhongDaDat.CurrentRow.Cells["TenPhong"].Value.ToString();

            if (MessageBox.Show($"Xác nhận Check-in phòng {tenPhong}?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string kq = bll.CheckIn(maDP);
                MessageBox.Show(kq);

                // Load lại tất cả
                LoadSoDoPhong(); // Để phòng chuyển màu đỏ
                btnTim_Click(null, null); // Để cập nhật lại bảng danh sách
            }
        }

        private void btnDoiPhong_Click(object sender, EventArgs e)
        {
            // 1. Phải chọn booking cũ trong bảng
            if (dgvPhongDaDat.CurrentRow == null)
            {
                MessageBox.Show("Bước 1: Chọn phòng ĐÃ ĐẶT trong bảng danh sách!");
                return;
            }

            // 2. Phải chọn phòng mới trên sơ đồ
            if (_maPhongDangChon == 0)
            {
                MessageBox.Show("Bước 2: Click chọn một phòng TRỐNG (Màu xanh) trên sơ đồ bên trái!");
                return;
            }

            // Lấy dữ liệu
            int maDP = Convert.ToInt32(dgvPhongDaDat.CurrentRow.Cells["MaDP"].Value);
            int maPhongCu = Convert.ToInt32(dgvPhongDaDat.CurrentRow.Cells["MaPhong"].Value);
            int maPhongMoi = _maPhongDangChon;

            // Gọi BLL
            string kq = bll.DoiPhongCheckIn(maDP, maPhongCu, maPhongMoi);
            MessageBox.Show(kq);

            if (kq.Contains("thành công"))
            {
                LoadSoDoPhong();
                btnTim_Click(null, null);

                // Reset lựa chọn
                _maPhongDangChon = 0;
                lblPhongDangChon.Text = "PHÒNG: CHƯA CHỌN";
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}