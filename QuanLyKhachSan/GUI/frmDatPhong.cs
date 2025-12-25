using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms; // Thư viện Guna
using QuanLyKhachSan.BLL;

namespace QuanLyKhachSan.GUI
{
    public partial class frmDatPhong : Form
    {
        // Khởi tạo BLL
        BookingBLL bll = new BookingBLL();

        // Biến tạm để lưu phòng đang chọn (cho hành động đặt mới hoặc đổi phòng)
        int _maPhongDangChon = 0;
        decimal _giaPhongHienTai = 0;

        public frmDatPhong()
        {
            InitializeComponent();
        }

        // 1. Load Form
        private void frmDatPhong_Load(object sender, EventArgs e)
        {
            // Set ngày mặc định
            dtpNgayDen.Value = DateTime.Now;
            dtpNgayDi.Value = DateTime.Now.AddDays(1);

            // Ẩn các nút chức năng check-in khi mới vào
            dgvPhongDaDat.Visible = false;
            btnNhanPhong.Visible = false;
            btnDoiPhong.Visible = false;

            LoadSoDoPhong(); // Vẽ sơ đồ
        }

        // 2. Hàm vẽ sơ đồ phòng
        // Trong file QuanLyKhachSan.GUI.frmDatPhong.cs

        private void LoadSoDoPhong()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dt = bll.GetSoDoPhong(dtpNgayDen.Value, dtpNgayDi.Value);

            if (dt == null || dt.Rows.Count == 0) return;

            foreach (DataRow row in dt.Rows)
            {
                Guna2TileButton btn = new Guna2TileButton();
                // ... (Code setup style cũ giữ nguyên) ...
                btn.Width = 100; btn.Height = 100; btn.BorderRadius = 10;
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.ForeColor = Color.White;

                string tenPhong = row["TenPhong"].ToString();
                string loaiPhong = row["TenLP"].ToString();
                decimal gia = Convert.ToDecimal(row["GiaMacDinh"]);
                int trangThai = Convert.ToInt32(row["TrangThaiSo"]);

                // Xử lý màu sắc thống nhất
                if (trangThai == 2) // MÀU ĐỎ: ĐANG Ở
                {
                    btn.FillColor = Color.Red;
                    btn.Text = $"{tenPhong}\n{loaiPhong}\n(Đang ở)";
                }
                else if (trangThai == 3) // MÀU TÍM: BẢO TRÌ (Mới thêm)
                {
                    btn.FillColor = Color.Purple;
                    btn.Text = $"{tenPhong}\n{loaiPhong}\n(Bảo trì)";
                    btn.Enabled = false; // Không cho đặt phòng bảo trì
                }
                else if (trangThai == 1) // MÀU VÀNG: ĐÃ ĐẶT
                {
                    btn.FillColor = Color.Gold;
                    btn.ForeColor = Color.Black;
                    btn.Text = $"{tenPhong}\n{loaiPhong}\n(Đã đặt)";
                }
                else // MÀU XANH: TRỐNG
                {
                    btn.FillColor = Color.FromArgb(46, 204, 113);
                    btn.Text = $"{tenPhong}\n{loaiPhong}\n{gia:N0}";
                }

                btn.Tag = row;
                btn.Click += BtnPhong_Click;
                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        // 3. Sự kiện Click vào phòng
        private void BtnPhong_Click(object sender, EventArgs e)
        {
            Guna2TileButton btn = (Guna2TileButton)sender;
            DataRow data = (DataRow)btn.Tag;
            int trangThai = Convert.ToInt32(data["TrangThaiSo"]);
            int maPhongClick = Convert.ToInt32(data["MaPhong"]);

            // --- TRƯỜNG HỢP A: Click vào phòng VÀNG hoặc ĐỎ (Xem thông tin) ---
            if (trangThai == 1)
            {
                // Reset biến chọn phòng (để ko bị đặt nhầm)
                _maPhongDangChon = 0;

                // Hiện tên phòng thay vì trạng thái
                lblPhongDangChon.Text = $"PHÒNG: {data["TenPhong"]}";
                lblPhongDangChon.ForeColor = (trangThai == 1) ? Color.Gold : Color.Red;
                txtTienCoc.Clear();

                // Lấy thông tin khách đang giữ phòng này
                DataRow kh = bll.GetKhachHangByRoom(maPhongClick, dtpNgayDen.Value, dtpNgayDi.Value);

                if (kh != null)
                {
                    // Điền thông tin lên TextBox
                    txtHoTen.Text = kh["HoTen"].ToString();
                    txtCCCD.Text = kh["CCCD"].ToString();
                    txtSDT.Text = kh["SDT"].ToString();
                    cboGioiTinh.Text = kh["GioiTinh"].ToString();

                    if (kh.Table.Columns.Contains("QuocTich"))
                        txtQuocTich.Text = kh["QuocTich"].ToString();

                    if (kh["NgaySinh"] != DBNull.Value)
                        dtpNgaySinh.Value = Convert.ToDateTime(kh["NgaySinh"]);
                }
                return; // Kết thúc, không làm gì thêm
            }
            else if (trangThai == 2) // TRƯỜNG HỢP C: Phòng đang ở (Màu ĐỎ) -> Muốn thanh toán
            {
                // Bước 1: Gọi BLL để tìm xem "Mã Nhận Phòng" nào đang ở trong "Mã Phòng" này
                int maNP = bll.GetCurrentStayID(maPhongClick);

                if (maNP > 0)
                {
                    // Bước 2: Khởi tạo Form CheckOut và TRUYỀN maNP VÀO
                    QuanLyKhachSan.GUI.CheckOut frm = new QuanLyKhachSan.GUI.CheckOut(maNP);

                    // Bước 3: Hiện Form lên
                    frm.ShowDialog();

                    // Bước 4: Sau khi Form CheckOut đóng lại (đã thanh toán xong)
               
                    BookingBLL.NotifyDataChanged(); 

                    // Load lại danh sách phòng tại form này
                    LoadSoDoPhong();
                }
                else
                {
                    MessageBox.Show("Lỗi dữ liệu: Phòng báo đang ở nhưng không tìm thấy thông tin Nhận phòng (MaNP)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
            {
                _maPhongDangChon = maPhongClick;
                _giaPhongHienTai = Convert.ToDecimal(data["GiaMacDinh"]);

                lblPhongDangChon.Text = $"PHÒNG: {data["TenPhong"]}";
                lblPhongDangChon.ForeColor = Color.Green;

                // Gợi ý tiền cọc
                txtTienCoc.Text = (_giaPhongHienTai * 0.3m).ToString("N0");
            }
        }

        // 4. Nút Tìm Kiếm 
        private void btnTim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim())) return;

            dgvPhongDaDat.Visible = false;
            btnNhanPhong.Visible = false;
            btnDoiPhong.Visible = false;

            DataRow kh = bll.GetKhachHangInfo(txtSearch.Text.Trim());

            if (kh != null)
            {
                txtHoTen.Text = kh["HoTen"].ToString();
                txtCCCD.Text = kh["CCCD"].ToString();
                txtSDT.Text = kh["SDT"].ToString();
                cboGioiTinh.Text = kh["GioiTinh"].ToString();
                if (kh.Table.Columns.Contains("QuocTich")) txtQuocTich.Text = kh["QuocTich"].ToString();
                if (kh["NgaySinh"] != DBNull.Value) dtpNgaySinh.Value = Convert.ToDateTime(kh["NgaySinh"]);

                DataTable dtBooking = bll.GetPhongDaDat(Convert.ToInt32(kh["MaKH"]));

                if (dtBooking != null && dtBooking.Rows.Count > 0)
                {
                    dgvPhongDaDat.Visible = true;
                    btnNhanPhong.Visible = true;
                    btnDoiPhong.Visible = true;
                    dgvPhongDaDat.DataSource = dtBooking;

                    if (dgvPhongDaDat.Columns.Contains("TenPhong")) dgvPhongDaDat.Columns["TenPhong"].HeaderText = "Phòng";
                    if (dgvPhongDaDat.Columns.Contains("NgayNhanPhong")) dgvPhongDaDat.Columns["NgayNhanPhong"].HeaderText = "Ngày Đến";
                    if (dgvPhongDaDat.Columns.Contains("NgayTraPhong")) dgvPhongDaDat.Columns["NgayTraPhong"].HeaderText = "Ngày Đi";

                    if (dgvPhongDaDat.Columns.Contains("MaDP")) dgvPhongDaDat.Columns["MaDP"].Visible = false;
                    if (dgvPhongDaDat.Columns.Contains("MaPhong")) dgvPhongDaDat.Columns["MaPhong"].Visible = false;
                }
                else
                {
                    MessageBox.Show("Khách hàng này không có phòng nào đang chờ Check-in.", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Khách hàng mới (Chưa có trong hệ thống).", "Thông báo");
                txtHoTen.Clear(); txtCCCD.Clear(); txtSDT.Clear();
            }
        }

        // 5. Nút Xác Nhận Đặt
        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            if (_maPhongDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn một phòng TRỐNG (Màu xanh)!", "Cảnh báo");
                return;
            }
            if (string.IsNullOrEmpty(txtCCCD.Text))
            {
                MessageBox.Show("Vui lòng nhập và lưu thông tin khách hàng trước!", "Cảnh báo");
                return;
            }

            DataRow kh = bll.GetKhachHangInfo(txtCCCD.Text);
            if (kh == null)
            {
                MessageBox.Show("Vui lòng nhấn nút 'Lưu Khách Mới' trước khi đặt!", "Thông báo");
                return;
            }

            decimal tienCoc = 0;
            decimal.TryParse(txtTienCoc.Text.Replace(",", "").Replace(".", ""), out tienCoc);

            string kq = bll.BookRoom(Convert.ToInt32(kh["MaKH"]), _maPhongDangChon, dtpNgayDen.Value, dtpNgayDi.Value, tienCoc);

            MessageBox.Show(kq);

            if (kq.Contains("thành công"))
            {
                BookingBLL.NotifyDataChanged(); 

                LoadSoDoPhong();
                _maPhongDangChon = 0;
                lblPhongDangChon.Text = "PHÒNG: CHƯA CHỌN";
                lblPhongDangChon.ForeColor = Color.Black;
            }
        }

        // 6. Nút Nhận Phòng
        private void btnNhanPhong_Click(object sender, EventArgs e)
        {
            if (dgvPhongDaDat.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn phòng trong bảng danh sách để nhận!", "Thông báo");
                return;
            }

            int maDP = Convert.ToInt32(dgvPhongDaDat.CurrentRow.Cells["MaDP"].Value);
            string tenPhong = dgvPhongDaDat.CurrentRow.Cells["TenPhong"].Value.ToString();

            if (MessageBox.Show($"Xác nhận Check-in cho phòng {tenPhong}?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string kq = bll.CheckIn(maDP);
                MessageBox.Show(kq);

                // ===> CẬP NHẬT TRANG CHỦ <===
                BookingBLL.NotifyDataChanged(); // <--- THÊM MỚI

                LoadSoDoPhong();
                btnTim_Click(null, null); // Refresh lại danh sách đặt
            }
        }

        // 7. Nút Lưu Khách Mới
        private void btnThemKhach_Click(object sender, EventArgs e)
        {
            string kq = bll.AddKhachHang(txtHoTen.Text, txtCCCD.Text, txtSDT.Text, cboGioiTinh.Text, dtpNgaySinh.Value, txtQuocTich.Text);
            MessageBox.Show(kq);
        }

        // 8. Nút Làm Mới
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtSearch.Clear(); txtHoTen.Clear(); txtCCCD.Clear(); txtSDT.Clear(); txtTienCoc.Clear();
            dgvPhongDaDat.Visible = false;
            btnNhanPhong.Visible = false;
            btnDoiPhong.Visible = false;

            _maPhongDangChon = 0;
            lblPhongDangChon.Text = "PHÒNG: CHƯA CHỌN";
            LoadSoDoPhong();
        }

        // 9. Nút Lọc Phòng
        private void btnLocPhong_Click(object sender, EventArgs e)
        {
            LoadSoDoPhong();
        }

        // 10. Nút Đổi Phòng
        private void btnDoiPhong_Click(object sender, EventArgs e)
        {
            if (dgvPhongDaDat.CurrentRow == null)
            {
                MessageBox.Show("Chọn phòng cần đổi trong danh sách!", "Hướng dẫn");
                return;
            }
            if (_maPhongDangChon == 0)
            {
                MessageBox.Show("Chọn phòng TRỐNG (Xanh) trên sơ đồ muốn chuyển tới!", "Hướng dẫn");
                return;
            }

            int maDP = Convert.ToInt32(dgvPhongDaDat.CurrentRow.Cells["MaDP"].Value);
            int maPhongCu = Convert.ToInt32(dgvPhongDaDat.CurrentRow.Cells["MaPhong"].Value);

            string kq = bll.DoiPhongCheckIn(maDP, maPhongCu, _maPhongDangChon);
            MessageBox.Show(kq);

            if (kq.Contains("thành công"))
            {
                // ===> CẬP NHẬT TRANG CHỦ <===
                BookingBLL.NotifyDataChanged(); // <--- THÊM MỚI

                LoadSoDoPhong();
                btnTim_Click(null, null);
                _maPhongDangChon = 0;
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {
        }

        private void txtHoTen_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvPhongDaDat_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}