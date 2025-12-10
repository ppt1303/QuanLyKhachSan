using System;
using System.Data;
using System.Drawing; 
using System.Windows.Forms;
using Guna.UI2.WinForms; 
using QuanLyKhachSan.BLL; 

namespace QuanLyKhachSan.GUI
{
    public partial class frmDatPhong : Form
    {
        BookingBLL bll = new BookingBLL();
        int _maPhongDangChon = 0;
        decimal _giaPhongHienTai = 0;

        public frmDatPhong()
        {
            InitializeComponent();
        }
        private void frmDatPhong_Load(object sender, EventArgs e)
        {
            dtpNgayDen.Value = DateTime.Now;
            dtpNgayDi.Value = DateTime.Now.AddDays(1);
            dgvPhongDaDat.Visible = false;
            btnNhanPhong.Visible = false;
            btnDoiPhong.Visible = false;

            LoadSoDoPhong(); 
        }
        private void LoadSoDoPhong()
        {
            flowLayoutPanel1.Controls.Clear(); 

            DataTable dt = bll.GetSoDoPhong(dtpNgayDen.Value, dtpNgayDi.Value);

            if (dt == null || dt.Rows.Count == 0) return;

            foreach (DataRow row in dt.Rows)
            {
                Guna2TileButton btn = new Guna2TileButton();
                btn.Width = 100;
                btn.Height = 100;
                btn.BorderRadius = 10;
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.ForeColor = Color.White;
                string tenPhong = row["TenPhong"].ToString();
                string loaiPhong = row["TenLP"].ToString();
                decimal gia = Convert.ToDecimal(row["GiaMacDinh"]);
                int isBooked = Convert.ToInt32(row["TrangThaiBan"]); 

                if (isBooked == 1)
                {
                    btn.FillColor = Color.Red;
                    btn.Text = $"{tenPhong}\n{loaiPhong}\n(Đã đặt)";
                }
                else
                {
                    btn.FillColor = Color.FromArgb(46, 204, 113);
                    btn.Text = $"{tenPhong}\n{loaiPhong}\n{gia:N0}";
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

            int maPhong = Convert.ToInt32(data["MaPhong"]);

            if (btn.FillColor == Color.Red)
            {
                DataRow info = bll.GetThongTinDatPhongByPhong(maPhong, dtpNgayDen.Value);

                if (info != null)
                {
                    txtHoTen.Text = info["HoTen"].ToString();
                    txtCCCD.Text = info["CCCD"].ToString();
                    txtSDT.Text = info["SDT"].ToString();
                    cboGioiTinh.Text = info["GioiTinh"].ToString();
                    txtQuocTich.Text = info["QuocTich"].ToString();

                    DataTable dtShow = new DataTable();
                    dtShow.Columns.Add("TenPhong");
                    dtShow.Columns.Add("TenLP");
                    dtShow.Columns.Add("NgayNhanPhong", typeof(DateTime));
                    dtShow.Columns.Add("NgayTraPhong", typeof(DateTime));
                    dtShow.Columns.Add("MaDP"); 

                    dtShow.Rows.Add(
                        info["TenPhong"],
                        info["TenLP"],
                        info["NgayNhanPhong"],
                        info["NgayTraPhong"],
                        info["MaDP"]
                    );

        
                    dgvPhongDaDat.Visible = true;
                    dgvPhongDaDat.DataSource = dtShow;

                    dgvPhongDaDat.Columns["TenPhong"].HeaderText = "Phòng";
                    dgvPhongDaDat.Columns["TenLP"].HeaderText = "Loại";
                    dgvPhongDaDat.Columns["NgayNhanPhong"].HeaderText = "Ngày Đến";
                    dgvPhongDaDat.Columns["NgayTraPhong"].HeaderText = "Ngày Đi";
                    dgvPhongDaDat.Columns["NgayNhanPhong"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvPhongDaDat.Columns["NgayTraPhong"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvPhongDaDat.Columns["MaDP"].Visible = false; 

                    btnNhanPhong.Visible = true;
                    btnDoiPhong.Visible = true;
                    lblPhongDangChon.Text = $"PHÒNG: {info["TenPhong"]} (ĐÃ ĐẶT)";
                    lblPhongDangChon.ForeColor = Color.Red;
                    txtTienCoc.Clear();
                    MessageBox.Show($"Phòng này do khách {info["HoTen"]} đặt.\nThông tin đã được hiển thị.");
                }
                return; 
            }

            _maPhongDangChon = maPhong;
            _giaPhongHienTai = Convert.ToDecimal(data["GiaMacDinh"]);

            lblPhongDangChon.Text = $"PHÒNG: {data["TenPhong"]}";
            lblPhongDangChon.ForeColor = Color.Green;

            txtTienCoc.Text = (_giaPhongHienTai * 0.3m).ToString("N0");
        }

        private void btnLocPhong_Click(object sender, EventArgs e)
        {
            LoadSoDoPhong();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim())) return;

            DataRow kh = bll.GetKhachHangInfo(txtSearch.Text.Trim());

            if (kh != null)
            {
                txtHoTen.Text = kh["HoTen"].ToString();
                txtCCCD.Text = kh["CCCD"].ToString();
                txtSDT.Text = kh["SDT"].ToString();
                cboGioiTinh.Text = kh["GioiTinh"].ToString();

                if (kh["NgaySinh"] != DBNull.Value)
                    dtpNgaySinh.Value = Convert.ToDateTime(kh["NgaySinh"]);

                if (kh.Table.Columns.Contains("QuocTich"))
                    txtQuocTich.Text = kh["QuocTich"].ToString();

                int maKH = Convert.ToInt32(kh["MaKH"]);
                DataTable dtBooking = bll.GetPhongDaDat(maKH);

                if (dtBooking != null && dtBooking.Rows.Count > 0)
                {
                    dgvPhongDaDat.Visible = true;
                    btnNhanPhong.Visible = true;
                    btnDoiPhong.Visible = true;

                    dgvPhongDaDat.DataSource = dtBooking;
                    if (dgvPhongDaDat.Columns.Contains("TenPhong")) dgvPhongDaDat.Columns["TenPhong"].HeaderText = "Phòng";
                    if (dgvPhongDaDat.Columns.Contains("TenLP")) dgvPhongDaDat.Columns["TenLP"].HeaderText = "Loại"; 
                    if (dgvPhongDaDat.Columns.Contains("NgayNhanPhong")) dgvPhongDaDat.Columns["NgayNhanPhong"].HeaderText = "Ngày Đến";
                    if (dgvPhongDaDat.Columns.Contains("NgayTraPhong")) dgvPhongDaDat.Columns["NgayTraPhong"].HeaderText = "Ngày Đi";
                    dgvPhongDaDat.Columns["NgayNhanPhong"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvPhongDaDat.Columns["NgayTraPhong"].DefaultCellStyle.Format = "dd/MM/yyyy";

                    if (dgvPhongDaDat.Columns.Contains("MaDP")) dgvPhongDaDat.Columns["MaDP"].Visible = false;
                    if (dgvPhongDaDat.Columns.Contains("MaPhong")) dgvPhongDaDat.Columns["MaPhong"].Visible = false;

                    dgvPhongDaDat.ColumnHeadersHeight = 40;
                    dgvPhongDaDat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

                    dgvPhongDaDat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    if (dgvPhongDaDat.Columns.Contains("TenLP"))
                        dgvPhongDaDat.Columns["TenLP"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

     
                    dgvPhongDaDat.ClearSelection();

                    MessageBox.Show($"Tìm thấy {dtBooking.Rows.Count} phòng đã đặt!");
                }
       
                else
                {
         
                    dgvPhongDaDat.Visible = false;
                    btnNhanPhong.Visible = false;
                    btnDoiPhong.Visible = false;
                    MessageBox.Show("Đã tìm thấy hồ sơ khách hàng cũ.", "Thông báo");
                }
            }
            else
            {
         
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
                LoadSoDoPhong(); 
                btnTim_Click(null, null); 
            }
        }

      
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

        
            _maPhongDangChon = 0;
            lblPhongDangChon.Text = "PHÒNG: CHƯA CHỌN";
            lblPhongDangChon.ForeColor = Color.Black;

            dgvPhongDaDat.Visible = false;
            btnNhanPhong.Visible = false;
            btnDoiPhong.Visible = false;

            txtSearch.Focus();
        }

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
          
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }
    }
}