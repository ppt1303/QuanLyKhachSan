using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BLL;

namespace QuanLyKhachSan.GUI
{
    // Kế thừa từ UserControl thay vì Form
    public partial class ucQLPhong : UserControl
    {
        // Gọi lớp Nghiệp vụ (BLL)
        private QLPhongBLL bll = new QLPhongBLL();

        // Biến cờ để chặn sự kiện khi đang tải dữ liệu
        private bool isBindingData = false;
        private int selectedMaPhong = -1;

        public ucQLPhong()
        {
            InitializeComponent();
            SetupEvents();
        }

        // Sự kiện Load UserControl (Đổi tên hàm cho phù hợp)
        private void ucQLPhong_Load(object sender, EventArgs e)
        {
            LoadCombobox();
            LoadGrid();
        }

        // Thiết lập các sự kiện thủ công
        private void SetupEvents()
        {
            // Cấu hình GridView
            dgvPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhong.MultiSelect = false;
            dgvPhong.ReadOnly = true;

            // Gán sự kiện GridView
            dgvPhong.SelectionChanged += dgvPhong_SelectionChanged;

            // Gán sự kiện các nút bấm
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;

            // Gán sự kiện Load cho UserControl
            this.Load += ucQLPhong_Load;
        }

        // --- HÀM TẢI DỮ LIỆU ---
        public void LoadCombobox() // Có thể để public nếu muốn gọi từ Form cha
        {
            try
            {
                DataTable dtLP = bll.LayDSLoaiPhong();
                cboLoaiPhong.DataSource = dtLP;
                cboLoaiPhong.DisplayMember = "TenLP";
                cboLoaiPhong.ValueMember = "MaLP";
                cboLoaiPhong.SelectedIndex = -1;
            }
            catch (Exception )
            {
                // Log lỗi nếu cần
            }
        }

        public void LoadGrid() // Để public để Form cha có thể refresh dữ liệu khi cần
        {
            try
            {
                isBindingData = true; // Chặn sự kiện SelectionChanged

                dgvPhong.DataSource = bll.LayTatCaPhong();

                // Ẩn các cột ID
                if (dgvPhong.Columns.Contains("MaPhong")) dgvPhong.Columns["MaPhong"].Visible = false;
                if (dgvPhong.Columns.Contains("MaLP")) dgvPhong.Columns["MaLP"].Visible = false;

                // Định dạng tiền tệ
                if (dgvPhong.Columns.Contains("Giá Giờ")) dgvPhong.Columns["Giá Giờ"].DefaultCellStyle.Format = "N0";
                if (dgvPhong.Columns.Contains("Giá Ngày")) dgvPhong.Columns["Giá Ngày"].DefaultCellStyle.Format = "N0";
                if (dgvPhong.Columns.Contains("Giá Đêm")) dgvPhong.Columns["Giá Đêm"].DefaultCellStyle.Format = "N0";

                dgvPhong.ClearSelection();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                isBindingData = false; // Mở lại sự kiện
            }
        }

        // --- SỰ KIỆN CHỌN DÒNG TRÊN GRID ---
        private void dgvPhong_SelectionChanged(object sender, EventArgs e)
        {
            if (isBindingData) return;

            if (dgvPhong.SelectedRows.Count == 1)
            {
                DataGridViewRow row = dgvPhong.SelectedRows[0];

                if (row.Cells["MaPhong"].Value != null)
                {
                    selectedMaPhong = Convert.ToInt32(row.Cells["MaPhong"].Value);

                    // Điền dữ liệu vào TextBox
                    txtTenPhong.Text = row.Cells["Tên Phòng"].Value.ToString();
                    txtTang.Text = row.Cells["Tầng"].Value.ToString();
                    txtHuong.Text = row.Cells["Hướng"].Value.ToString();

                    // Điền ComboBox
                    if (row.Cells["MaLP"].Value != null)
                        cboLoaiPhong.SelectedValue = Convert.ToInt32(row.Cells["MaLP"].Value);

                    // Hiển thị Giá (Chỉ xem)
                    if (row.Cells["Giá Giờ"].Value != DBNull.Value)
                        txtGiaGio.Text = string.Format("{0:N0}", row.Cells["Giá Giờ"].Value);
                    else txtGiaGio.Text = "0";

                    if (row.Cells["Giá Ngày"].Value != DBNull.Value)
                        txtGiaNgay.Text = string.Format("{0:N0}", row.Cells["Giá Ngày"].Value);
                    else txtGiaNgay.Text = "0";

                    if (row.Cells["Giá Đêm"].Value != DBNull.Value)
                        txtGiaDem.Text = string.Format("{0:N0}", row.Cells["Giá Đêm"].Value);
                    else txtGiaDem.Text = "0";

                    // Điều khiển nút bấm
                    btnThem.Enabled = false;
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                }
            }
            else
            {
                ResetForm();
            }
        }

        private void ResetForm()
        {
            selectedMaPhong = -1;
            txtTenPhong.Clear();
            txtTang.Clear();
            txtHuong.Clear();

            txtGiaGio.Clear();
            txtGiaNgay.Clear();
            txtGiaDem.Clear();

            cboLoaiPhong.SelectedIndex = -1;

            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        // --- CÁC NÚT CHỨC NĂNG ---
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!CheckInput()) return;

            // Lấy dữ liệu
            int maLP = Convert.ToInt32(cboLoaiPhong.SelectedValue);
            int tang = 0;
            int.TryParse(txtTang.Text, out tang);

            // Gọi BLL thêm phòng
            if (bll.ThemPhong(txtTenPhong.Text, maLP, tang, txtHuong.Text))
            {
                MessageBox.Show("Thêm phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid();
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Vui lòng kiểm tra lại thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaPhong == -1)
            {
                MessageBox.Show("Vui lòng chọn phòng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!CheckInput()) return;

            int maLP = Convert.ToInt32(cboLoaiPhong.SelectedValue);
            int tang = 0;
            int.TryParse(txtTang.Text, out tang);

            if (bll.SuaPhong(selectedMaPhong, txtTenPhong.Text, maLP, tang, txtHuong.Text))
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadGrid();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMaPhong == -1) return;

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa phòng này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (bll.XoaPhong(selectedMaPhong))
                {
                    MessageBox.Show("Đã xóa phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadGrid();
                }
                else
                {
                    MessageBox.Show("Không thể xóa (Phòng đang được sử dụng hoặc có dữ liệu liên quan).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Hàm kiểm tra nhập liệu
        private bool CheckInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenPhong.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhong.Focus();
                return false;
            }
            if (cboLoaiPhong.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}