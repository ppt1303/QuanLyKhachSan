using System;
using System.Data;
using System.Data.SqlClient; // Thư viện SQL
using System.Windows.Forms;

namespace QuanLyKhachSan.DAL
{
    public class DatabaseHelper
    {
        // Chuỗi kết nối (Lấy từ ảnh bạn gửi)
        private static string connectionString = @"Data Source=PHAMPUTINL;Initial Catalog=QuanLyKhachSan;Integrated Security=True";

        // 1. Hàm lấy dữ liệu (SELECT)
        public static DataTable GetData(string query, SqlParameter[] parameters = null, CommandType cmdType = CommandType.Text)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = cmdType;
                        if (parameters != null) cmd.Parameters.AddRange(parameters);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Database: " + ex.Message);
            }
            return dt;
        }

        // 2. Hàm thực thi lệnh (INSERT, UPDATE, DELETE)
        public static bool ExecuteNonQuery(string query, SqlParameter[] parameters = null, CommandType cmdType = CommandType.StoredProcedure)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = cmdType;
                        if (parameters != null) cmd.Parameters.AddRange(parameters);
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực thi: " + ex.Message);
                return false;
            }
        }
    }
}