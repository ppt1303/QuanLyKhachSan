using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan.GUI
{
    public partial class frmQLThietBi : Form
    {
        private const string CONNECTION_STRING = "Data Source=KARMA\\SQLEXPRESS;Initial Catalog=QuanLyKhachSan;Integrated Security=True";

        private int selectedMaTB_ForEdit = -1;

        // Bổ sung thêm logic khởi tạo này vào constructor CỦA BẠN:
        
        public frmQLThietBi()
        {
            InitializeComponent(); 

            dgvThietBi.MultiSelect = true;
            dgvThietBi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThietBi.ReadOnly = true; 

            dgvThietBi.SelectionChanged += dgvThietBi_SelectionChanged;

            this.Load += frmQLThietBi_Load;
            btnThemTB.Click += btnThemTB_Click;
            btnSuaTB.Click += btnSuaTB_Click;
            btnXoaTB.Click += btnXoaTB_Click;

            ResetForm();
        }
      

        // =================================================================
        // 2. TẢI DỮ LIỆU VÀ XỬ LÝ SỰ KIỆN LOAD
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
        // 3. LOGIC CHỌN HÀNG (SelectionChanged)
        // =================================================================

        private void dgvThietBi_SelectionChanged(object sender, EventArgs e)
        {
            int count = dgvThietBi.SelectedRows.Count;

            // Trường hợp 1: 0 HÀNG ĐƯỢC CHỌN
            if (count == 0)
            {
                ResetForm();
                return;
            }

            // Trường hợp 2: DUY NHẤT 1 HÀNG ĐƯỢC CHỌN (Cho phép Sửa)
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

            // Trường hợp 3: NHIỀU HÀNG ĐƯỢC CHỌN (> 1)
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

        // =================================================================
        // 4. LOGIC NÚT THAO TÁC
        // =================================================================

        private bool KiemTraDuLieu()
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
            if (!KiemTraDuLieu()) return;

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
            if (selectedMaTB_ForEdit == -1)
            {
                MessageBox.Show("Vui lòng chọn một thiết bị để sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!KiemTraDuLieu()) return;

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
            if (dgvThietBi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một thiết bị để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa {dgvThietBi.SelectedRows.Count} thiết bị đã chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Lấy danh sách MaTB của các hàng được chọn
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
        // 5. HÀM HỖ TRỢ TRUY VẤN CSDL
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
    }
}
