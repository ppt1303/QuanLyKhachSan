namespace QuanLyKhachSan.GUI
{
    partial class frmChiTietPhong
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTenPhong = new System.Windows.Forms.Label();
            this.lblTenKhach = new System.Windows.Forms.Label();
            this.lblGioNhan = new System.Windows.Forms.Label();
            this.cboDichVu = new System.Windows.Forms.ComboBox();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.btnThemDV = new System.Windows.Forms.Button();
            this.dgvLichSuDV = new System.Windows.Forms.DataGridView();
            this.lblTieuDeTrangThai = new System.Windows.Forms.Label();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnCloseFrm = new System.Windows.Forms.Button();
            this.cboTrangThaiPhong = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichSuDV)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTenPhong
            // 
            this.lblTenPhong.AutoSize = true;
            this.lblTenPhong.BackColor = System.Drawing.Color.Orange;
            this.lblTenPhong.Location = new System.Drawing.Point(82, 25);
            this.lblTenPhong.Name = "lblTenPhong";
            this.lblTenPhong.Size = new System.Drawing.Size(44, 16);
            this.lblTenPhong.TabIndex = 0;
            this.lblTenPhong.Text = "label1";
            this.lblTenPhong.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblTenPhong.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblTenKhach
            // 
            this.lblTenKhach.AutoSize = true;
            this.lblTenKhach.BackColor = System.Drawing.Color.Orange;
            this.lblTenKhach.Location = new System.Drawing.Point(483, 25);
            this.lblTenKhach.Name = "lblTenKhach";
            this.lblTenKhach.Size = new System.Drawing.Size(44, 16);
            this.lblTenKhach.TabIndex = 1;
            this.lblTenKhach.Text = "label2";
            this.lblTenKhach.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblGioNhan
            // 
            this.lblGioNhan.AutoSize = true;
            this.lblGioNhan.BackColor = System.Drawing.Color.Orange;
            this.lblGioNhan.Location = new System.Drawing.Point(775, 25);
            this.lblGioNhan.Name = "lblGioNhan";
            this.lblGioNhan.Size = new System.Drawing.Size(44, 16);
            this.lblGioNhan.TabIndex = 2;
            this.lblGioNhan.Text = "label3";
            this.lblGioNhan.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblGioNhan.Click += new System.EventHandler(this.label3_Click);
            // 
            // cboDichVu
            // 
            this.cboDichVu.BackColor = System.Drawing.Color.Orange;
            this.cboDichVu.FormattingEnabled = true;
            this.cboDichVu.Location = new System.Drawing.Point(84, 13);
            this.cboDichVu.Name = "cboDichVu";
            this.cboDichVu.Size = new System.Drawing.Size(224, 24);
            this.cboDichVu.TabIndex = 3;
            this.cboDichVu.SelectedIndexChanged += new System.EventHandler(this.cboDichVu_SelectedIndexChanged);
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.BackColor = System.Drawing.Color.Orange;
            this.txtSoLuong.Location = new System.Drawing.Point(505, 15);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(74, 22);
            this.txtSoLuong.TabIndex = 4;
            // 
            // btnThemDV
            // 
            this.btnThemDV.BackColor = System.Drawing.Color.Orange;
            this.btnThemDV.Location = new System.Drawing.Point(682, 14);
            this.btnThemDV.Name = "btnThemDV";
            this.btnThemDV.Size = new System.Drawing.Size(128, 23);
            this.btnThemDV.TabIndex = 5;
            this.btnThemDV.Text = "Thêm Dịch Vụ";
            this.btnThemDV.UseVisualStyleBackColor = false;
            this.btnThemDV.Click += new System.EventHandler(this.btnThemDV_Click_1);
            // 
            // dgvLichSuDV
            // 
            this.dgvLichSuDV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLichSuDV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLichSuDV.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLichSuDV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvLichSuDV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLichSuDV.Location = new System.Drawing.Point(13, 12);
            this.dgvLichSuDV.Name = "dgvLichSuDV";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLichSuDV.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvLichSuDV.RowHeadersWidth = 51;
            this.dgvLichSuDV.RowTemplate.Height = 24;
            this.dgvLichSuDV.Size = new System.Drawing.Size(702, 285);
            this.dgvLichSuDV.TabIndex = 6;
            // 
            // lblTieuDeTrangThai
            // 
            this.lblTieuDeTrangThai.AutoSize = true;
            this.lblTieuDeTrangThai.BackColor = System.Drawing.Color.Orange;
            this.lblTieuDeTrangThai.Location = new System.Drawing.Point(145, 30);
            this.lblTieuDeTrangThai.Name = "lblTieuDeTrangThai";
            this.lblTieuDeTrangThai.Size = new System.Drawing.Size(44, 16);
            this.lblTieuDeTrangThai.TabIndex = 7;
            this.lblTieuDeTrangThai.Text = "label1";
            this.lblTieuDeTrangThai.Click += new System.EventHandler(this.lblTieuDeTrangThai_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.Orange;
            this.btnLuu.Location = new System.Drawing.Point(27, 244);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(74, 23);
            this.btnLuu.TabIndex = 8;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnCloseFrm
            // 
            this.btnCloseFrm.BackColor = System.Drawing.Color.Orange;
            this.btnCloseFrm.Location = new System.Drawing.Point(191, 244);
            this.btnCloseFrm.Name = "btnCloseFrm";
            this.btnCloseFrm.Size = new System.Drawing.Size(75, 23);
            this.btnCloseFrm.TabIndex = 9;
            this.btnCloseFrm.Text = "Đóng";
            this.btnCloseFrm.UseVisualStyleBackColor = false;
            this.btnCloseFrm.Click += new System.EventHandler(this.btnCloseFrm_Click_1);
            // 
            // cboTrangThaiPhong
            // 
            this.cboTrangThaiPhong.BackColor = System.Drawing.Color.Orange;
            this.cboTrangThaiPhong.FormattingEnabled = true;
            this.cboTrangThaiPhong.Location = new System.Drawing.Point(93, 126);
            this.cboTrangThaiPhong.Name = "cboTrangThaiPhong";
            this.cboTrangThaiPhong.Size = new System.Drawing.Size(173, 24);
            this.cboTrangThaiPhong.TabIndex = 10;
            this.cboTrangThaiPhong.SelectedIndexChanged += new System.EventHandler(this.cboTrangThaiPhong_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(27, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Phòng:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Info;
            this.label2.Location = new System.Drawing.Point(404, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Tên khách:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Info;
            this.label3.Location = new System.Drawing.Point(684, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Ngày nhận:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Info;
            this.label4.Location = new System.Drawing.Point(21, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Dịch vụ:";
            this.label4.Click += new System.EventHandler(this.label4_Click_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Info;
            this.label5.Location = new System.Drawing.Point(402, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "Nhập số lượng:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Info;
            this.label6.Location = new System.Drawing.Point(24, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "Trạng thái hiện tại:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Info;
            this.label7.Location = new System.Drawing.Point(24, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 16);
            this.label7.TabIndex = 17;
            this.label7.Text = "Cập nhật:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblTenKhach);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblGioNhan);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblTenPhong);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1039, 72);
            this.panel1.TabIndex = 18;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.Info;
            this.panel2.Controls.Add(this.cboDichVu);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtSoLuong);
            this.panel2.Controls.Add(this.btnThemDV);
            this.panel2.Location = new System.Drawing.Point(12, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1039, 59);
            this.panel2.TabIndex = 19;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.SystemColors.Info;
            this.panel3.Controls.Add(this.cboTrangThaiPhong);
            this.panel3.Controls.Add(this.lblTieuDeTrangThai);
            this.panel3.Controls.Add(this.btnLuu);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.btnCloseFrm);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Location = new System.Drawing.Point(749, 155);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(302, 309);
            this.panel3.TabIndex = 20;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel4.BackColor = System.Drawing.SystemColors.Info;
            this.panel4.Controls.Add(this.dgvLichSuDV);
            this.panel4.Location = new System.Drawing.Point(12, 155);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(731, 309);
            this.panel4.TabIndex = 16;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // frmChiTietPhong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1063, 480);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmChiTietPhong";
            this.Text = "frmChiTietPhong";
            this.Load += new System.EventHandler(this.frmChiTietPhong_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichSuDV)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTenPhong;
        private System.Windows.Forms.Label lblTenKhach;
        private System.Windows.Forms.Label lblGioNhan;
        private System.Windows.Forms.ComboBox cboDichVu;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Button btnThemDV;
        private System.Windows.Forms.DataGridView dgvLichSuDV;
        private System.Windows.Forms.Label lblTieuDeTrangThai;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnCloseFrm;
        private System.Windows.Forms.ComboBox cboTrangThaiPhong;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
    }
}