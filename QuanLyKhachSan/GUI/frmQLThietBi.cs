using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyKhachSan.GUI
{
    public partial class frmQLThietBi : Form
    {
        private const string CONNECTION_STRING = "Data Source=KARMA\\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True";

        private int selectedMaTB_ForEdit = -1;
        private int lastClickedRowIndex = -1; // Cho chức năng Toggle

        public frmQLThietBi()
        {
            InitializeComponent();

            // Cấu hình DataGridView
            dgvThietBi.MultiSelect = true;
            dgvThietBi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThietBi.ReadOnly = true;

            // Gán sự kiện
            dgvThietBi.SelectionChanged += dgvThietBi_SelectionChanged;
            dgvThietBi.CellClick += dgvThietBi_CellClick;

            this.Load += frmQLThietBi_Load;
            btnThemTB.Click += btnThemTB_Click;
            btnSuaTB.Click += btnSuaTB_Click;
            btnXoaTB.Click += btnXoaTB_Click;

            ResetForm();
        }

        // =================================================================
        // 1. TẢI DỮ LIỆU & RESET
        // =================================================================

        private void frmQLThietBi_Load(object sender, EventArgs e)
        {
            LoadThietBi();
        }

        private void LoadThietBi()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    string query = "SELECT MaTB, TenTB, MoTa FROM dbo.THIETBI ORDER BY MaTB";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvThietBi.DataSource = dt;
                    dgvThietBi.ClearSelection();

                    // Cấu hình hiển thị cột
                    dgvThietBi.Columns["MaTB"].HeaderText = "Mã TB";
                    dgvThietBi.Columns["TenTB"].HeaderText = "Tên Thiết Bị";
                    dgvThietBi.Columns["MoTa"].HeaderText = "Mô Tả";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            selectedMaTB_ForEdit = -1;
            txtTenTB.Clear();
            txtMoTaTB.Clear();
            btnThemTB.Enabled = true;
            btnSuaTB.Enabled = false;
            btnXoaTB.Enabled = false;
            dgvThietBi.ClearSelection();
            txtTenTB.Focus();
        }

        // =================================================================
        // 2. LOGIC CHỌN HÀNG (SelectionChanged & Toggle)
        // =================================================================

        private void dgvThietBi_SelectionChanged(object sender, EventArgs e)
        {
            int count = dgvThietBi.SelectedRows.Count;

            if (count == 0)
            {
                ResetForm();
                return;
            }

            // Chọn 1 hàng: Bật Sửa
            if (count == 1)
            {
                DataGridViewRow row = dgvThietBi.SelectedRows[0];

                if (row.Cells["MaTB"].Value != DBNull.Value && row.Cells["MaTB"].Value != null)
                {
                    selectedMaTB_ForEdit = Convert.ToInt32(row.Cells["MaTB"].Value);
                    txtTenTB.Text = row.Cells["TenTB"].Value.ToString();
                    txtMoTaTB.Text = row.Cells["MoTa"].Value.ToString();

                    btnThemTB.Enabled = false;
                    btnSuaTB.Enabled = true;
                    btnXoaTB.Enabled = true;
                }
                else
                {
                    ResetForm();
                }
            }
            // Chọn nhiều hàng: Ẩn Sửa
            else
            {
                selectedMaTB_ForEdit = -1;
                txtTenTB.Clear();
                txtMoTaTB.Clear();

                btnThemTB.Enabled = false;
                btnSuaTB.Enabled = false;
                btnXoaTB.Enabled = true;
            }
        }

        // Sự kiện CellClick dùng để Toggle (Nhấn lần 2 hủy chọn)
        private void dgvThietBi_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private bool KiemTraDuLieuThietBi()
        {
            if (string.IsNullOrWhiteSpace(txtTenTB.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên Thiết Bị.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnThemTB_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieuThietBi()) return;

            try
            {
                ExecuteProcedure("sp_ThemThietBi",
                    new SqlParameter("@TenTB", txtTenTB.Text.Trim()),
                    new SqlParameter("@MoTa", txtMoTaTB.Text.Trim())
                );

                MessageBox.Show("Thêm thiết bị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadThietBi();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi thêm thiết bị: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaTB_Click(object sender, EventArgs e)
        {
            if (selectedMaTB_ForEdit == -1 || !KiemTraDuLieuThietBi()) return;

            try
            {
                ExecuteProcedure("sp_SuaThietBi",
                    new SqlParameter("@MaTB", selectedMaTB_ForEdit),
                    new SqlParameter("@TenTB", txtTenTB.Text.Trim()),
                    new SqlParameter("@MoTa", txtMoTaTB.Text.Trim())
                );

                MessageBox.Show("Sửa thiết bị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadThietBi();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi sửa thiết bị: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaTB_Click(object sender, EventArgs e)
        {
            if (dgvThietBi.SelectedRows.Count == 0) return;

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa {dgvThietBi.SelectedRows.Count} thiết bị đã chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string maTBList = string.Join(",", dgvThietBi.SelectedRows.Cast<DataGridViewRow>()
                    .Where(row => row.Cells["MaTB"].Value != DBNull.Value && row.Cells["MaTB"].Value != null)
                    .Select(row => row.Cells["MaTB"].Value.ToString()));

                if (string.IsNullOrWhiteSpace(maTBList)) return;

                try
                {
                    ExecuteProcedure("sp_XoaNhieuThietBi",
                        new SqlParameter("@MaTBList", maTBList)
                    );

                    MessageBox.Show("Xóa thiết bị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadThietBi();
                    ResetForm();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi xóa thiết bị: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void panel1_Paint(object sender, PaintEventArgs e) { }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}