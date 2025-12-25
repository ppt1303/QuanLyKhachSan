namespace QuanLyKhachSan.GUI
{
    partial class ucQuanLyDatPhong
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
            this.btnDatPhongMoi = new System.Windows.Forms.Button();
            this.dgvDanhSachDon = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachDon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDatPhongMoi
            // 
            this.btnDatPhongMoi.BackColor = System.Drawing.Color.Cyan;
            this.btnDatPhongMoi.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDatPhongMoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDatPhongMoi.Location = new System.Drawing.Point(0, 0);
            this.btnDatPhongMoi.Name = "btnDatPhongMoi";
            this.btnDatPhongMoi.Size = new System.Drawing.Size(609, 58);
            this.btnDatPhongMoi.TabIndex = 0;
            this.btnDatPhongMoi.Text = "Đặt Phòng";
            this.btnDatPhongMoi.UseVisualStyleBackColor = false;
            this.btnDatPhongMoi.Click += new System.EventHandler(this.btnDatPhongMoi_Click_1);
            // 
            // dgvDanhSachDon
            // 
            this.dgvDanhSachDon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDanhSachDon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDanhSachDon.Location = new System.Drawing.Point(0, 58);
            this.dgvDanhSachDon.Name = "dgvDanhSachDon";
            this.dgvDanhSachDon.RowHeadersWidth = 51;
            this.dgvDanhSachDon.RowTemplate.Height = 24;
            this.dgvDanhSachDon.Size = new System.Drawing.Size(609, 434);
            this.dgvDanhSachDon.TabIndex = 1;
            this.dgvDanhSachDon.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDanhSachDon_CellContentClick);
            // 
            // ucQuanLyDatPhong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvDanhSachDon);
            this.Controls.Add(this.btnDatPhongMoi);
            this.Name = "ucQuanLyDatPhong";
            this.Size = new System.Drawing.Size(609, 492);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachDon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDatPhongMoi;
        private System.Windows.Forms.DataGridView dgvDanhSachDon;
    }
}
