using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Guna.UI2.WinForms;
using QuanLyKhachSan.BLL;

namespace QuanLyKhachSan.GUI
{
    public partial class ucDatPhong : DevExpress.XtraEditors.XtraUserControl
    {
        private PhongBLL _phongBLL = new PhongBLL();
        private BookingBLL _bookingBLL = new BookingBLL();

        private int _maPhongDangChon = 0;
        private decimal _giaPhongHienTai = 0;

        public ucDatPhong()
        {
            InitializeComponent();
        }

        private void ucDatPhong_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;

            // SỬA LỖI: Dùng .Value thay vì .DateTime
            dtpNgayDen.Value = DateTime.Now;
            dtpNgayDi.Value = DateTime.Now.AddDays(1);

            // Set mặc định ngày sinh (để tránh lỗi null)
            dtpNgaySinh.Value = DateTime.Now.AddYears(-18);

            if (dgvPhongDaDat != null) dgvPhongDaDat.Visible = false;
            if (btnNhanPhong != null) btnNhanPhong.Visible = false;
            if (btnDoiPhong != null) btnDoiPhong.Visible = false;

            LoadSoDoPhong();
        }

        public void LoadSoDoPhong()
        {
            flowLayoutPanel1.Controls.Clear();

            DataTable dt = _phongBLL.LayDanhSachPhongTrangChu();

            if (dt == null || dt.Rows.Count == 0) return;

            foreach (DataRow row in dt.Rows)
            {
                Guna2TileButton btn = new Guna2TileButton();
                btn.Width = 110;
                btn.Height = 110;
                btn.BorderRadius = 12;
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.ForeColor = Color.White;

                string tenPhong = row["TenPhong"].ToString();
                string loaiPhong = row["TenLP"].ToString();
                decimal gia = Convert.ToDecimal(row["GiaMacDinh"]);
                int trangThai = Convert.ToInt32(row["TrangThaiHienThi"]);

                switch (trangThai)
                {
                    case 1: // TRỐNG
                        btn.FillColor = Color.FromArgb(46, 204, 113);
                        btn.Text = $"{tenPhong}\n{loaiPhong}\n{gia:N0}";
                        break;
                    case 3: // ĐẶT TRƯỚC
                        btn.FillColor = Color.Gold;
                        btn.ForeColor = Color.Black;
                        btn.Text = $"{tenPhong}\n{loaiPhong}\n(Đã đặt)";
                        break;
                    case 2: // ĐANG Ở
                        btn.FillColor = Color.Firebrick;
                        btn.Text = $"{tenPhong}\n{loaiPhong}\n(Đang ở)";
                        break;
                    default: // BẢO TRÌ/DƠ
                        btn.FillColor = Color.Gray;
                        btn.Text = $"{tenPhong}\n{loaiPhong}\n(Bảo trì)";
                        break;
                }

                btn.Tag = row;
                btn.Click += BtnPhong_Click;
                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        private void BtnPhong_Click(object sender, EventArgs e)
        {
            Guna2TileButton btn = (Guna2TileButton)sender;
            DataRow data = (DataRow)btn.Tag;

            int trangThai = Convert.ToInt32(data["TrangThaiHienThi"]);
            int maPhong = Convert.ToInt32(data["MaPhong"]);
            string tenPhong = data["TenPhong"].ToString();

            lblPhongDangChon.Text = $"PHÒNG: {tenPhong}";

            if (trangThai == 1) // TRỐNG
            {
                _maPhongDangChon = maPhong;
                _giaPhongHienTai = Convert.ToDecimal(data["GiaMacDinh"]);

                lblPhongDangChon.ForeColor = Color.Green;

                txtTienCoc.Text = (_giaPhongHienTai * 0.5m).ToString("N0");
                txtTienCoc.Enabled = true;
                btnDatPhong.Enabled = true;

                ClearKhachHangInfo();
            }
            else if (trangThai == 3) // ĐÃ ĐẶT
            {
                _maPhongDangChon = 0;
                lblPhongDangChon.ForeColor = Color.Gold;
                txtTienCoc.Enabled = false;
                btnDatPhong.Enabled = false;

                // SỬA LỖI: Dùng .Value
                DataRow kh = _bookingBLL.GetKhachHangByRoom(maPhong, dtpNgayDen.Value, dtpNgayDi.Value);
                if (kh != null) FillKhachHangInfo(kh);
                else XtraMessageBox.Show("Không tìm thấy thông tin khách đặt.", "Thông báo");
            }
            else // ĐANG Ở HOẶC BẢO TRÌ
            {
                _maPhongDangChon = 0;
                lblPhongDangChon.ForeColor = Color.Red;
                XtraMessageBox.Show("Phòng này đang có người ở hoặc bảo trì.", "Thông báo");
            }
        }

        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            if (_maPhongDangChon == 0)
            {
                XtraMessageBox.Show("Vui lòng chọn phòng TRỐNG (Màu xanh)!", "Cảnh báo");
                return;
            }
            if (string.IsNullOrEmpty(txtCCCD.Text))
            {
                XtraMessageBox.Show("Chưa có thông tin khách hàng!", "Cảnh báo");
                return;
            }

            DataRow kh = _bookingBLL.GetKhachHangInfo(txtCCCD.Text);
            if (kh == null)
            {
                XtraMessageBox.Show("Vui lòng nhấn 'Lưu Khách Mới' trước khi đặt!", "Thông báo");
                return;
            }

            decimal tienCoc = 0;
            decimal.TryParse(txtTienCoc.Text.Replace(",", "").Replace(".", ""), out tienCoc);

            // SỬA LỖI: Dùng .Value
            string kq = _bookingBLL.BookRoom(
                Convert.ToInt32(kh["MaKH"]),
                _maPhongDangChon,
                dtpNgayDen.Value,
                dtpNgayDi.Value,
                tienCoc
            );

            XtraMessageBox.Show(kq);

            if (kq.Contains("thành công"))
            {
                LoadSoDoPhong();
                _maPhongDangChon = 0;
                lblPhongDangChon.Text = "PHÒNG: CHƯA CHỌN";
                lblPhongDangChon.ForeColor = Color.Black;
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim())) return;

            DataRow kh = _bookingBLL.GetKhachHangInfo(txtSearch.Text.Trim());

            if (kh != null)
            {
                FillKhachHangInfo(kh);
            }
            else
            {
                XtraMessageBox.Show("Khách hàng mới. Vui lòng nhập thông tin và Lưu.", "Thông báo");
                ClearKhachHangInfo();
            }
        }

        private void btnThemKhach_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtHoTen.Text) || string.IsNullOrEmpty(txtCCCD.Text))
            {
                XtraMessageBox.Show("Vui lòng nhập Họ tên và CCCD.", "Cảnh báo");
                return;
            }

            // SỬA LỖI: Dùng .Value cho dtpNgaySinh
            string kq = _bookingBLL.AddKhachHang(
                txtHoTen.Text,
                txtCCCD.Text,
                txtSDT.Text,
                cboGioiTinh.Text,
                dtpNgaySinh.Value,
                txtQuocTich.Text
            );
            XtraMessageBox.Show(kq);
        }

        private void btnLocPhong_Click(object sender, EventArgs e)
        {
            LoadSoDoPhong();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            ClearKhachHangInfo();
            txtTienCoc.Clear();
            _maPhongDangChon = 0;
            lblPhongDangChon.Text = "PHÒNG: CHƯA CHỌN";
            LoadSoDoPhong();
        }

        // --- CÁC HÀM HỖ TRỢ ---
        private void FillKhachHangInfo(DataRow kh)
        {
            txtHoTen.Text = kh["HoTen"].ToString();
            txtCCCD.Text = kh["CCCD"].ToString();
            txtSDT.Text = kh["SDT"].ToString();
            cboGioiTinh.Text = kh["GioiTinh"].ToString();
            if (kh.Table.Columns.Contains("QuocTich")) txtQuocTich.Text = kh["QuocTich"].ToString();

            // SỬA LỖI: Dùng .Value
            if (kh["NgaySinh"] != DBNull.Value) dtpNgaySinh.Value = Convert.ToDateTime(kh["NgaySinh"]);
        }

        private void ClearKhachHangInfo()
        {
            txtHoTen.Clear();
            txtCCCD.Clear();
            txtSDT.Clear();
            txtQuocTich.Text = "Việt Nam";
            // SỬA LỖI: Dùng .Value
            dtpNgaySinh.Value = DateTime.Now.AddYears(-18);
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}