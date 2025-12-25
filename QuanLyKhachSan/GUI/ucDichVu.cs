using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyKhachSan.BLL;

namespace QuanLyKhachSan.GUI
{

    public partial class ucDichVu : UserControl
    {
        
        private DichVuBLL dichVuBLL = new DichVuBLL();
        private int selectedMaDV_ForEdit = -1;
        private int lastClickedRowIndex = -1;

        public ucDichVu()
        {
            InitializeComponent();

          
            dgvDIchVu.MultiSelect = true;
            dgvDIchVu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDIchVu.ReadOnly = true;

            // Gán sự kiện
            dgvDIchVu.SelectionChanged += dgvDIchVu_SelectionChanged;
            dgvDIchVu.CellClick += dgvDIchVu_CellClick;

            // Sự kiện Load của UserControl
            this.Load += ucDichVu_Load;

            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;

            ResetForm();
        }

       
        private void ucDichVu_Load(object sender, EventArgs e)
        {
            LoadDichVu();
        }

        private void LoadDichVu()
        {
            try
            {
                dgvDIchVu.DataSource = dichVuBLL.LayDSDichVu();
                dgvDIchVu.ClearSelection();

              
                if (dgvDIchVu.Columns.Contains("MaDV"))
                {
                    dgvDIchVu.Columns["MaDV"].HeaderText = "Mã DV";
                    dgvDIchVu.Columns["TenDV"].HeaderText = "Tên Dịch Vụ";
                    dgvDIchVu.Columns["Gia"].HeaderText = "Giá";
                    dgvDIchVu.Columns["Gia"].DefaultCellStyle.Format = "N0";

                   
                }

               
                dgvDIchVu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

   

        private void dgvDIchVu_SelectionChanged(object sender, EventArgs e)
        {
            int count = dgvDIchVu.SelectedRows.Count;

            if (count == 0)
            {
                ResetForm();
                return;
            }

            
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
            // Chọn nhiều hàng: Ẩn Sửa, chỉ cho Xóa
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
                ResetForm(); 
            }
            else
            {
                if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                {
                }
                lastClickedRowIndex = e.RowIndex;
            }
        }

       
        private bool KiemTraDuLieuDichVu()
        {
            if (string.IsNullOrWhiteSpace(txtTenDV.Text) || string.IsNullOrWhiteSpace(txtGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên Dịch Vụ và Giá.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

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

            try
            {
                decimal gia = decimal.Parse(txtGia.Text.Replace(",", ""));

                if (dichVuBLL.ThemDichVu(txtTenDV.Text.Trim(), gia))
                {
                    MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDichVu();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Thêm dịch vụ thất bại (Lỗi CSDL).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi định dạng giá: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaDV_ForEdit == -1)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần sửa.", "Thông báo");
                return;
            }
            if (!KiemTraDuLieuDichVu()) return;

            try
            {
                decimal gia = decimal.Parse(txtGia.Text.Replace(",", ""));

                if (dichVuBLL.SuaDichVu(selectedMaDV_ForEdit, txtTenDV.Text.Trim(), gia))
                {
                    MessageBox.Show("Sửa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDichVu();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Sửa dịch vụ thất bại (Lỗi CSDL).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDIchVu.SelectedRows.Count == 0) return;

            // Lấy danh sách ID các dòng đang chọn
            string maDVList = string.Join(",", dgvDIchVu.SelectedRows.Cast<DataGridViewRow>()
                .Where(row => row.Cells["MaDV"].Value != DBNull.Value && row.Cells["MaDV"].Value != null)
                .Select(row => row.Cells["MaDV"].Value.ToString()));

            if (string.IsNullOrWhiteSpace(maDVList)) return;

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa {dgvDIchVu.SelectedRows.Count} dịch vụ đã chọn?",
                                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dichVuBLL.XoaDichVu(maDVList))
                {
                    MessageBox.Show("Xóa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDichVu();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Xóa dịch vụ thất bại (Có thể do ràng buộc dữ liệu hóa đơn).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}