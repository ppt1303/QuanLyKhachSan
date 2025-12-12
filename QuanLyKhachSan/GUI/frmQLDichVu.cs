using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyKhachSan.BLL; // Đã thêm using BLL

namespace QuanLyKhachSan.GUI
{
    public partial class frmQLDichVu : Form
    {
        // =================================================================
        // KHAI BÁO BIẾN VÀ BLL
        // =================================================================
        private DichVuBLL dichVuBLL = new DichVuBLL();
        private int selectedMaDV_ForEdit = -1;
        private int lastClickedRowIndex = -1; // Cho chức năng Toggle

        // Constructor
        public frmQLDichVu()
        {
            InitializeComponent();

            // Cấu hình DataGridView
            // Giả định tên controls: dgvDIchVu, txtTenDV, txtGia, btnThem, btnSua, btnXoa
            dgvDIchVu.MultiSelect = true;
            dgvDIchVu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDIchVu.ReadOnly = true;

            // Gán sự kiện
            dgvDIchVu.SelectionChanged += dgvDIchVu_SelectionChanged;
            dgvDIchVu.CellClick += dgvDIchVu_CellClick;

            this.Load += frmQLDichVu_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;

            ResetForm();
        }

        // =================================================================
        // 1. TẢI DỮ LIỆU & RESET
        // =================================================================

        private void frmQLDichVu_Load(object sender, EventArgs e)
        {
            LoadDichVu();
        }

        private void LoadDichVu()
        {
            try
            {
                // GỌI BLL:
                dgvDIchVu.DataSource = dichVuBLL.LayDSDichVu();
                dgvDIchVu.ClearSelection();

                // Cấu hình hiển thị cột (Đảm bảo cột tồn tại)
                if (dgvDIchVu.Columns.Contains("MaDV"))
                {
                    dgvDIchVu.Columns["MaDV"].HeaderText = "Mã DV";
                    dgvDIchVu.Columns["TenDV"].HeaderText = "Tên Dịch Vụ";
                    dgvDIchVu.Columns["Gia"].HeaderText = "Giá";
                    dgvDIchVu.Columns["Gia"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            selectedMaDV_ForEdit = -1;
            txtTenDV.Clear();
            txtGia.Clear();
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            dgvDIchVu.ClearSelection();
            txtTenDV.Focus();
        }

        // =================================================================
        // 2. LOGIC CHỌN HÀNG (SelectionChanged & Toggle)
        // =================================================================

        private void dgvDIchVu_SelectionChanged(object sender, EventArgs e)
        {
            int count = dgvDIchVu.SelectedRows.Count;

            if (count == 0)
            {
                ResetForm();
                return;
            }

            // Chọn 1 hàng: Bật Sửa
            if (count == 1)
            {
                DataGridViewRow row = dgvDIchVu.SelectedRows[0];

                if (row.Cells["MaDV"].Value != DBNull.Value && row.Cells["MaDV"].Value != null)
                {
                    selectedMaDV_ForEdit = Convert.ToInt32(row.Cells["MaDV"].Value);
                    txtTenDV.Text = row.Cells["TenDV"].Value.ToString();
                    txtGia.Text = row.Cells["Gia"].Value.ToString();

                    btnThem.Enabled = false;
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                }
            }
            // Chọn nhiều hàng: Ẩn Sửa
            else
            {
                selectedMaDV_ForEdit = -1;
                txtTenDV.Clear();
                txtGia.Clear();

                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = true;
            }
        }

        // Sự kiện CellClick dùng để Toggle (Nhấn lần 2 hủy chọn)
        private void dgvDIchVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;
            bool isToggleOffCandidate = dgv.Rows[e.RowIndex].Selected && (e.RowIndex == lastClickedRowIndex);

            if (isToggleOffCandidate && (Control.ModifierKeys & Keys.Control) != Keys.Control)
            {
                dgv.Rows[e.RowIndex].Selected = false;
                dgv.ClearSelection();
                lastClickedRowIndex = -1;
            }
            else
            {
                if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                {
                    dgv.ClearSelection();
                    dgv.Rows[e.RowIndex].Selected = true;
                }
                lastClickedRowIndex = e.RowIndex;
            }
        }

        // =================================================================
        // 3. LOGIC NÚT THAO TÁC VÀ HÀM HỖ TRỢ BÊN TRONG
        // =================================================================

        // HÀM HỖ TRỢ: Đã bị thiếu trong code của bạn
        private bool KiemTraDuLieuDichVu()
        {
            if (string.IsNullOrWhiteSpace(txtTenDV.Text) || string.IsNullOrWhiteSpace(txtGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên Dịch Vụ và Giá.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Sử dụng TryParse an toàn hơn
            if (!decimal.TryParse(txtGia.Text.Replace(",", ""), out _))
            {
                MessageBox.Show("Giá phải là một số hợp lệ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieuDichVu()) return;
            // Xử lý giá để loại bỏ ký tự định dạng nếu có
            decimal gia = decimal.Parse(txtGia.Text.Replace(",", ""));

            if (dichVuBLL.ThemDichVu(txtTenDV.Text.Trim(), gia)) // GỌI BLL
            {
                MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDichVu();
            }
            else
            {
                MessageBox.Show("Thêm dịch vụ thất bại (Lỗi CSDL).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaDV_ForEdit == -1 || !KiemTraDuLieuDichVu()) return;
            decimal gia = decimal.Parse(txtGia.Text.Replace(",", ""));

            if (dichVuBLL.SuaDichVu(selectedMaDV_ForEdit, txtTenDV.Text.Trim(), gia)) // GỌI BLL
            {
                MessageBox.Show("Sửa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDichVu();
            }
            else
            {
                MessageBox.Show("Sửa dịch vụ thất bại (Lỗi CSDL).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDIchVu.SelectedRows.Count == 0) return;

            // Xây dựng biến maDVList: Đã bị thiếu trong code bạn gửi
            string maDVList = string.Join(",", dgvDIchVu.SelectedRows.Cast<DataGridViewRow>()
                .Where(row => row.Cells["MaDV"].Value != DBNull.Value && row.Cells["MaDV"].Value != null)
                .Select(row => row.Cells["MaDV"].Value.ToString()));

            if (string.IsNullOrWhiteSpace(maDVList)) return;


            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa {dgvDIchVu.SelectedRows.Count} dịch vụ đã chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dichVuBLL.XoaDichVu(maDVList)) // GỌI BLL
                {
                    MessageBox.Show("Xóa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDichVu();
                }
                else
                {
                    MessageBox.Show("Xóa dịch vụ thất bại (Có thể do ràng buộc dữ liệu).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Giữ lại các hàm Designer không sử dụng
        private void txtGia_TextChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Click(object sender, EventArgs e) { }
    }
}