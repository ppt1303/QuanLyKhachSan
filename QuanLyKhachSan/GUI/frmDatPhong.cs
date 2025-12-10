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
        private void LoadSoDoPhong()
        {
            flowLayoutPanel1.Controls.Clear(); // Xóa nút cũ

            // GỌI HÀM MỚI trong BLL (Lấy tất cả phòng + Trạng thái)
            DataTable dt = bll.GetSoDoPhong(dtpNgayDen.Value, dtpNgayDi.Value);

            if (dt == null || dt.Rows.Count == 0) return;

            foreach (DataRow row in dt.Rows)
            {
                // Tạo nút phòng
                Guna2TileButton btn = new Guna2TileButton();
                btn.Width = 100;
                btn.Height = 100;
                btn.BorderRadius = 10;
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.ForeColor = Color.White;

                // Lấy thông tin
                string tenPhong = row["TenPhong"].ToString();
                string loaiPhong = row["TenLP"].ToString();
                decimal gia = Convert.ToDecimal(row["GiaMacDinh"]);
                int isBooked = Convert.ToInt32(row["TrangThaiBan"]); // 1: Bận, 0: Trống

                // XỬ LÝ MÀU SẮC (Quan trọng nhất)
                if (isBooked == 1)
                {
                    // Phòng ĐÃ CÓ NGƯỜI -> Màu Đỏ
                    btn.FillColor = Color.Red;
                    btn.Text = $"{tenPhong}\n{loaiPhong}\n(Đã đặt)";
                    // btn.Enabled = false; // Mở nếu muốn khóa click
                }
                else
                {
                    // Phòng TRỐNG -> Màu Xanh
                    btn.FillColor = Color.FromArgb(46, 204, 113);
                    btn.Text = $"{tenPhong}\n{loaiPhong}\n{gia:N0}";
                }

                // Lưu dữ liệu vào Tag và gắn sự kiện
                btn.Tag = row;
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
                MessageBox.Show("Phòng này đã có người đặt trong khoảng thời gian bạn chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Reset lựa chọn để không cho đặt đè
                _maPhongDangChon = 0;
                lblPhongDangChon.Text = "PHÒNG: ĐÃ BẬN";
                lblPhongDangChon.ForeColor = Color.Red;
                txtTienCoc.Clear();
                return;
            }

            // Nếu phòng Xanh (Trống) -> Xử lý bình thường
            _maPhongDangChon = Convert.ToInt32(data["MaPhong"]);
            _giaPhongHienTai = Convert.ToDecimal(data["GiaMacDinh"]);

            lblPhongDangChon.Text = $"PHÒNG: {data["TenPhong"]}";
            lblPhongDangChon.ForeColor = Color.Green;

            // Gợi ý tiền cọc (30%)
            txtTienCoc.Text = (_giaPhongHienTai * 0.3m).ToString("N0");
        }

        // 4. Nút "Tìm phòng" (Lọc ngày)
        private void btnLocPhong_Click(object sender, EventArgs e)
        {
            LoadSoDoPhong();
        }

        // 5. Nút "Tìm khách"
        private void btnTim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim())) return;

            // Tìm thông tin khách
            DataRow kh = bll.GetKhachHangInfo(txtSearch.Text.Trim());

            if (kh != null)
            {
                // --- A. Điền thông tin ---
                txtHoTen.Text = kh["HoTen"].ToString();
                txtCCCD.Text = kh["CCCD"].ToString();
                txtSDT.Text = kh["SDT"].ToString();
                cboGioiTinh.Text = kh["GioiTinh"].ToString();

                if (kh["NgaySinh"] != DBNull.Value)
                    dtpNgaySinh.Value = Convert.ToDateTime(kh["NgaySinh"]);

                if (kh.Table.Columns.Contains("QuocTich"))
                    txtQuocTich.Text = kh["QuocTich"].ToString();

                // --- B. Hiện danh sách phòng đã đặt ---
                int maKH = Convert.ToInt32(kh["MaKH"]);
                DataTable dtBooking = bll.GetPhongDaDat(maKH);
                // ... (Đoạn trên giữ nguyên) ...

                if (dtBooking != null && dtBooking.Rows.Count > 0)
                {
                    // 1. Hiện bảng
                    dgvPhongDaDat.Visible = true;
                    btnNhanPhong.Visible = true;
                    btnDoiPhong.Visible = true;

                    // 2. Gán dữ liệu
                    dgvPhongDaDat.DataSource = dtBooking;

                    // --- CÁC DÒNG QUAN TRỌNG ĐỂ HIỆN ĐẸP LUÔN ---

                    // A. Đặt tên cột Tiếng Việt
                    if (dgvPhongDaDat.Columns.Contains("TenPhong")) dgvPhongDaDat.Columns["TenPhong"].HeaderText = "Phòng";
                    if (dgvPhongDaDat.Columns.Contains("TenLP")) dgvPhongDaDat.Columns["TenLP"].HeaderText = "Loại"; // Viết ngắn cho gọn
                    if (dgvPhongDaDat.Columns.Contains("NgayNhanPhong")) dgvPhongDaDat.Columns["NgayNhanPhong"].HeaderText = "Ngày Đến";
                    if (dgvPhongDaDat.Columns.Contains("NgayTraPhong")) dgvPhongDaDat.Columns["NgayTraPhong"].HeaderText = "Ngày Đi";

                    // B. Định dạng ngày tháng
                    dgvPhongDaDat.Columns["NgayNhanPhong"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvPhongDaDat.Columns["NgayTraPhong"].DefaultCellStyle.Format = "dd/MM/yyyy";

                    // C. Ẩn cột thừa
                    if (dgvPhongDaDat.Columns.Contains("MaDP")) dgvPhongDaDat.Columns["MaDP"].Visible = false;
                    if (dgvPhongDaDat.Columns.Contains("MaPhong")) dgvPhongDaDat.Columns["MaPhong"].Visible = false;

                    // D. KHẮC PHỤC LỖI "PHẢI CLICK MỚI HIỆN":
                    // Chỉnh chiều cao tiêu đề cố định (để không bị mất chữ)
                    dgvPhongDaDat.ColumnHeadersHeight = 40;
                    dgvPhongDaDat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

                    // Tự động giãn cột theo nội dung chữ (AllCells) thay vì Fill (để chữ dài không bị che)
                    dgvPhongDaDat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // Nếu bảng còn dư chỗ trống thì cho cột "Loại Phòng" giãn hết mức
                    if (dgvPhongDaDat.Columns.Contains("TenLP"))
                        dgvPhongDaDat.Columns["TenLP"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    // Bỏ chọn dòng đầu tiên (để nhìn bảng sạch sẽ, không bị bôi xanh dòng đầu)
                    dgvPhongDaDat.ClearSelection();

                    MessageBox.Show($"Tìm thấy {dtBooking.Rows.Count} phòng đã đặt!");
                }
                // ... (Đoạn dưới giữ nguyên) ...
                else
                {
                    // Khách cũ nhưng không có phòng đặt trước
                    dgvPhongDaDat.Visible = false;
                    btnNhanPhong.Visible = false;
                    btnDoiPhong.Visible = false;
                    MessageBox.Show("Đã tìm thấy hồ sơ khách hàng cũ.", "Thông báo");
                }
            }
            else
            {
                // Khách mới
                MessageBox.Show("Khách hàng này chưa có trong hệ thống.\nVui lòng nhập thông tin và bấm 'Lưu Khách Mới'.", "Khách mới", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtHoTen.Clear(); txtCCCD.Clear(); txtSDT.Clear();
                txtQuocTich.Text = "Việt Nam";
                cboGioiTinh.StartIndex = 0;
                dtpNgaySinh.Value = DateTime.Now;

                dgvPhongDaDat.Visible = false;
                btnNhanPhong.Visible = false;
                btnDoiPhong.Visible = false;
                txtHoTen.Focus();
            }
        }

        // 6. Nút "Lưu khách mới"
        private void btnThemKhach_Click(object sender, EventArgs e)
        {
            string ten = txtHoTen.Text;
            string cccd = txtCCCD.Text;
            string sdt = txtSDT.Text;
            string gioitinh = cboGioiTinh.Text;
            DateTime ngaysinh = dtpNgaySinh.Value;
            string quoctich = txtQuocTich.Text;

            if (string.IsNullOrEmpty(gioitinh))
            {
                MessageBox.Show("Vui lòng chọn Giới tính!", "Cảnh báo");
                return;
            }

            string ketQua = bll.AddKhachHang(ten, cccd, sdt, gioitinh, ngaysinh, quoctich);
            MessageBox.Show(ketQua);
        }

        // 7. Nút "Xác nhận đặt" (Final)
        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            if (_maPhongDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng trống (Màu xanh) trên sơ đồ!", "Thông báo");
                return;
            }

            if (string.IsNullOrEmpty(txtCCCD.Text))
            {
                MessageBox.Show("Chưa có thông tin khách hàng!", "Thông báo");
                return;
            }

            DataRow kh = bll.GetKhachHangInfo(txtCCCD.Text);
            if (kh == null)
            {
                MessageBox.Show("Vui lòng bấm 'Lưu Khách Mới' trước khi đặt!", "Thông báo");
                return;
            }

            int maKH = Convert.ToInt32(kh["MaKH"]);
            decimal tienCoc = 0;
            decimal.TryParse(txtTienCoc.Text.Replace(",", "").Replace(".", ""), out tienCoc);

            string kq = bll.BookRoom(maKH, _maPhongDangChon, dtpNgayDen.Value, dtpNgayDi.Value, tienCoc);
            MessageBox.Show(kq);

            if (kq.Contains("thành công"))
            {
                LoadSoDoPhong(); // Vẽ lại sơ đồ
                btnTim_Click(null, null); // Load lại thông tin khách
            }
        }

        // 8. Nút làm mới (Reset form)
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            txtHoTen.Clear();
            txtCCCD.Clear();
            txtSDT.Clear();
            txtTienCoc.Clear();
            txtQuocTich.Text = "Việt Nam";
            cboGioiTinh.StartIndex = 0;
            dtpNgaySinh.Value = DateTime.Now;

            // Reset chọn phòng
            _maPhongDangChon = 0;
            lblPhongDangChon.Text = "PHÒNG: CHƯA CHỌN";
            lblPhongDangChon.ForeColor = Color.Black;

            dgvPhongDaDat.Visible = false;
            btnNhanPhong.Visible = false;
            btnDoiPhong.Visible = false;

            txtSearch.Focus();
        }

        // 9. Nút Nhận Phòng (Check-in từ danh sách)
        private void btnNhanPhong_Click(object sender, EventArgs e)
        {
            if (dgvPhongDaDat.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn phòng trong bảng danh sách để nhận!", "Thông báo");
                return;
            }

            int maDP = Convert.ToInt32(dgvPhongDaDat.CurrentRow.Cells["MaDP"].Value);
            string tenPhong = dgvPhongDaDat.CurrentRow.Cells["TenPhong"].Value.ToString();

            if (MessageBox.Show($"Xác nhận Check-in phòng {tenPhong}?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string kq = bll.CheckIn(maDP);
                MessageBox.Show(kq);

                LoadSoDoPhong();
                btnTim_Click(null, null);
            }
        }

        // 10. Nút Đổi Phòng
        private void btnDoiPhong_Click(object sender, EventArgs e)
        {
            if (dgvPhongDaDat.CurrentRow == null)
            {
                MessageBox.Show("Bước 1: Chọn phòng ĐÃ ĐẶT trong bảng danh sách!", "Hướng dẫn");
                return;
            }

            if (_maPhongDangChon == 0)
            {
                MessageBox.Show("Bước 2: Click chọn một phòng TRỐNG (Màu xanh) trên sơ đồ!", "Hướng dẫn");
                return;
            }

            int maDP = Convert.ToInt32(dgvPhongDaDat.CurrentRow.Cells["MaDP"].Value);
            int maPhongCu = Convert.ToInt32(dgvPhongDaDat.CurrentRow.Cells["MaPhong"].Value);
            int maPhongMoi = _maPhongDangChon;

            string kq = bll.DoiPhongCheckIn(maDP, maPhongCu, maPhongMoi);
            MessageBox.Show(kq);

            if (kq.Contains("thành công"))
            {
                LoadSoDoPhong();
                btnTim_Click(null, null);

                _maPhongDangChon = 0;
                lblPhongDangChon.Text = "PHÒNG: CHƯA CHỌN";
                lblPhongDangChon.ForeColor = Color.Black;
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Không làm gì cả
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }
    }
}