using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyKhachSan.GUI
{
    public partial class frmQLDichVu : Form
    {
        // Vui lòng kiểm tra lại: Nếu tên Database là QuanLiDeTai, hãy sửa lại Initial Catalog.
        private const string CONNECTION_STRING = "Data Source=KARMA\\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True";

        private int selectedMaDV_ForEdit = -1;
        private int lastClickedRowIndex = -1; // Cho chức năng Toggle

        public frmQLDichVu()
        {
            InitializeComponent();

            // Cấu hình DataGridView
            dgvDIchVu.MultiSelect = true;
            dgvDIchVu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDIchVu.ReadOnly = true;

            // ************ GÁN SỰ KIỆN CHỈ MỘT LẦN ************
            dgvDIchVu.SelectionChanged += dgvDIchVu_SelectionChanged;
            dgvDIchVu.CellClick += dgvDIchVu_CellClick;

            this.Load += frmQLDichVu_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            // **************************************************

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
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    string query = "SELECT MaDV, TenDV, Gia FROM dbo.DICHVU ORDER BY MaDV";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvDIchVu.DataSource = dt;
                    dgvDIchVu.ClearSelection(); // Ngăn chọn hàng đầu tiên

                    // Cấu hình hiển thị cột
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

            if (isToggleOffCandidate && (Control.ModifierKeys & Keys.Control) != Keys.Control) // Chỉ toggle khi không nhấn Ctrl
            {
                dgv.Rows[e.RowIndex].Selected = false;
                dgv.ClearSelection();
                lastClickedRowIndex = -1;
            }
            else
            {
                // Nếu không nhấn Ctrl, chỉ chọn duy nhất hàng này
                if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                {
                    dgv.ClearSelection();
                    dgv.Rows[e.RowIndex].Selected = true;
                }
                lastClickedRowIndex = e.RowIndex;
            }
        }

        // =================================================================
        // 3. LOGIC NÚT THAO TÁC
        // =================================================================

        private bool KiemTraDuLieuDichVu()
        {
            if (string.IsNullOrWhiteSpace(txtTenDV.Text) || string.IsNullOrWhiteSpace(txtGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên Dịch Vụ và Giá.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!decimal.TryParse(txtGia.Text, out _))
            {
                MessageBox.Show("Giá phải là một số hợp lệ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieuDichVu()) return;
            decimal gia = decimal.Parse(txtGia.Text.Replace(",", ""));

            try
            {
                ExecuteProcedure("sp_ThemDichVu",
                    new SqlParameter("@TenDV", txtTenDV.Text.Trim()),
                    new SqlParameter("@Gia", gia)
                );

                MessageBox.Show("Thêm dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDichVu();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi thêm dịch vụ: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaDV_ForEdit == -1 || !KiemTraDuLieuDichVu()) return;
            decimal gia = decimal.Parse(txtGia.Text.Replace(",", ""));

            try
            {
                ExecuteProcedure("sp_SuaDichVu",
                    new SqlParameter("@MaDV", selectedMaDV_ForEdit),
                    new SqlParameter("@TenDV", txtTenDV.Text.Trim()),
                    new SqlParameter("@Gia", gia)
                );

                MessageBox.Show("Sửa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDichVu();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi sửa dịch vụ: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDIchVu.SelectedRows.Count == 0) return;

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa {dgvDIchVu.SelectedRows.Count} dịch vụ đã chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string maDVList = string.Join(",", dgvDIchVu.SelectedRows.Cast<DataGridViewRow>()
                    .Where(row => row.Cells["MaDV"].Value != DBNull.Value && row.Cells["MaDV"].Value != null)
                    .Select(row => row.Cells["MaDV"].Value.ToString()));

                if (string.IsNullOrWhiteSpace(maDVList)) return;

                try
                {
                    ExecuteProcedure("sp_XoaNhieuDichVu",
                        new SqlParameter("@MaDVCs", maDVList)
                    );

                    MessageBox.Show("Xóa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDichVu();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi xóa dịch vụ: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // =================================================================
        // 4. HÀM HỖ TRỢ CSDL
        // =================================================================

        private void ExecuteProcedure(string procName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand command = new SqlCommand(procName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        // Giữ lại các hàm Designer không sử dụng để tránh lỗi CS0102 nếu chúng không được xóa trong Designer.cs
        private void txtGia_TextChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Click(object sender, EventArgs e) { }
    }
}