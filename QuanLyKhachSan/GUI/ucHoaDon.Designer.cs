namespace QuanLyKhachSan.GUI
{
    partial class ucHoaDon
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dtpTuNgay = new System.Windows.Forms.DateTimePicker();
            this.dgvHoaDon = new System.Windows.Forms.DataGridView();
            this.dtpDenNgay = new System.Windows.Forms.DateTimePicker();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.btnXemChiTiet = new System.Windows.Forms.Button();
            this.btnInHoaDon = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpTuNgay
            // 
            this.dtpTuNgay.Location = new System.Drawing.Point(149, 67);
            this.dtpTuNgay.Name = "dtpTuNgay";
            this.dtpTuNgay.Size = new System.Drawing.Size(400, 31);
            this.dtpTuNgay.TabIndex = 5;
            // 
            // dgvHoaDon
            // 
            this.dgvHoaDon.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHoaDon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHoaDon.Location = new System.Drawing.Point(73, 240);
            this.dgvHoaDon.Name = "dgvHoaDon";
            this.dgvHoaDon.RowHeadersWidth = 82;
            this.dgvHoaDon.RowTemplate.Height = 33;
            this.dgvHoaDon.Size = new System.Drawing.Size(1576, 510);
            this.dgvHoaDon.TabIndex = 6;
            // 
            // dtpDenNgay
            // 
            this.dtpDenNgay.Location = new System.Drawing.Point(785, 67);
            this.dtpDenNgay.Name = "dtpDenNgay";
            this.dtpDenNgay.Size = new System.Drawing.Size(400, 31);
            this.dtpDenNgay.TabIndex = 7;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnTimKiem.Location = new System.Drawing.Point(1396, 57);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(171, 51);
            this.btnTimKiem.TabIndex = 8;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click_1);
            // 
            // btnXemChiTiet
            // 
            this.btnXemChiTiet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXemChiTiet.Location = new System.Drawing.Point(240, 830);
            this.btnXemChiTiet.Name = "btnXemChiTiet";
            this.btnXemChiTiet.Size = new System.Drawing.Size(171, 51);
            this.btnXemChiTiet.TabIndex = 9;
            this.btnXemChiTiet.Text = "Xem chi tiết";
            this.btnXemChiTiet.UseVisualStyleBackColor = true;
            this.btnXemChiTiet.Click += new System.EventHandler(this.btnXemChiTiet_Click);
            // 
            // btnInHoaDon
            // 
            this.btnInHoaDon.Font = new System.Drawing.Font("Segoe UI Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInHoaDon.Location = new System.Drawing.Point(796, 830);
            this.btnInHoaDon.Name = "btnInHoaDon";
            this.btnInHoaDon.Size = new System.Drawing.Size(171, 51);
            this.btnInHoaDon.TabIndex = 10;
            this.btnInHoaDon.Text = "In hóa đơn ";
            this.btnInHoaDon.UseVisualStyleBackColor = true;
            this.btnInHoaDon.Click += new System.EventHandler(this.btnInHoaDon_Click_1);
            // 
            // ucHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnInHoaDon);
            this.Controls.Add(this.btnXemChiTiet);
            this.Controls.Add(this.btnTimKiem);
            this.Controls.Add(this.dtpDenNgay);
            this.Controls.Add(this.dgvHoaDon);
            this.Controls.Add(this.dtpTuNgay);
            this.Name = "ucHoaDon";
            this.Size = new System.Drawing.Size(1722, 990);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dtpTuNgay;
        private System.Windows.Forms.DataGridView dgvHoaDon;
        private System.Windows.Forms.DateTimePicker dtpDenNgay;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnXemChiTiet;
        private System.Windows.Forms.Button btnInHoaDon;
    }
}
