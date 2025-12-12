namespace QuanLyKhachSan.GUI
{
    partial class frmQLThietBi
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvThietBi = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnXoaTB = new Guna.UI2.WinForms.Guna2Button();
            this.btnSuaTB = new Guna.UI2.WinForms.Guna2Button();
            this.btnThemTB = new Guna.UI2.WinForms.Guna2Button();
            this.txtMoTaTB = new System.Windows.Forms.TextBox();
            this.txtTenTB = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvThietBiPhong = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.cboTenTBP = new System.Windows.Forms.ComboBox();
            this.cboTenPhong = new System.Windows.Forms.ComboBox();
            this.btnXoaTBP = new Guna.UI2.WinForms.Guna2Button();
            this.btnThemTBP = new Guna.UI2.WinForms.Guna2Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSoLuongKho = new System.Windows.Forms.TextBox();
            this.btnReset = new Guna.UI2.WinForms.Guna2Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThietBi)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThietBiPhong)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1133, 518);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1125, 489);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Quản lí kho thiết bị";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvThietBi);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(500, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(622, 483);
            this.panel2.TabIndex = 3;
            // 
            // dgvThietBi
            // 
            this.dgvThietBi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvThietBi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThietBi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThietBi.Location = new System.Drawing.Point(0, 0);
            this.dgvThietBi.Name = "dgvThietBi";
            this.dgvThietBi.RowHeadersWidth = 51;
            this.dgvThietBi.RowTemplate.Height = 24;
            this.dgvThietBi.Size = new System.Drawing.Size(622, 483);
            this.dgvThietBi.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.txtSoLuongKho);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnXoaTB);
            this.panel1.Controls.Add(this.btnSuaTB);
            this.panel1.Controls.Add(this.btnThemTB);
            this.panel1.Controls.Add(this.txtMoTaTB);
            this.panel1.Controls.Add(this.txtTenTB);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(497, 483);
            this.panel1.TabIndex = 2;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(54, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 35);
            this.label2.TabIndex = 8;
            this.label2.Text = "Mô tả";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(54, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 32);
            this.label1.TabIndex = 7;
            this.label1.Text = "Tên thiết bị";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnXoaTB
            // 
            this.btnXoaTB.Animated = true;
            this.btnXoaTB.BorderRadius = 15;
            this.btnXoaTB.BorderThickness = 1;
            this.btnXoaTB.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnXoaTB.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnXoaTB.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnXoaTB.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnXoaTB.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnXoaTB.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnXoaTB.ForeColor = System.Drawing.Color.White;
            this.btnXoaTB.Location = new System.Drawing.Point(341, 300);
            this.btnXoaTB.Name = "btnXoaTB";
            this.btnXoaTB.Size = new System.Drawing.Size(107, 50);
            this.btnXoaTB.TabIndex = 6;
            this.btnXoaTB.Text = "Xoá";
            // 
            // btnSuaTB
            // 
            this.btnSuaTB.Animated = true;
            this.btnSuaTB.BackColor = System.Drawing.Color.Transparent;
            this.btnSuaTB.BorderRadius = 15;
            this.btnSuaTB.BorderThickness = 1;
            this.btnSuaTB.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSuaTB.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSuaTB.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSuaTB.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSuaTB.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnSuaTB.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSuaTB.ForeColor = System.Drawing.Color.White;
            this.btnSuaTB.Location = new System.Drawing.Point(189, 300);
            this.btnSuaTB.Name = "btnSuaTB";
            this.btnSuaTB.Size = new System.Drawing.Size(107, 50);
            this.btnSuaTB.TabIndex = 5;
            this.btnSuaTB.Text = "Sửa";
            this.btnSuaTB.UseTransparentBackground = true;
            // 
            // btnThemTB
            // 
            this.btnThemTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnThemTB.BorderRadius = 15;
            this.btnThemTB.BorderThickness = 1;
            this.btnThemTB.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnThemTB.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnThemTB.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnThemTB.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnThemTB.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnThemTB.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThemTB.ForeColor = System.Drawing.Color.White;
            this.btnThemTB.Location = new System.Drawing.Point(37, 300);
            this.btnThemTB.Name = "btnThemTB";
            this.btnThemTB.Size = new System.Drawing.Size(107, 50);
            this.btnThemTB.TabIndex = 4;
            this.btnThemTB.Text = "Thêm";
            // 
            // txtMoTaTB
            // 
            this.txtMoTaTB.Location = new System.Drawing.Point(189, 159);
            this.txtMoTaTB.Name = "txtMoTaTB";
            this.txtMoTaTB.Size = new System.Drawing.Size(195, 22);
            this.txtMoTaTB.TabIndex = 1;
            // 
            // txtTenTB
            // 
            this.txtTenTB.Location = new System.Drawing.Point(189, 92);
            this.txtTenTB.Name = "txtTenTB";
            this.txtTenTB.Size = new System.Drawing.Size(195, 22);
            this.txtTenTB.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel4);
            this.tabPage2.Controls.Add(this.panel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1125, 489);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Quản lí thiết bị phòng";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvThietBiPhong);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(491, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(631, 483);
            this.panel4.TabIndex = 1;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // dgvThietBiPhong
            // 
            this.dgvThietBiPhong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThietBiPhong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThietBiPhong.Location = new System.Drawing.Point(0, 0);
            this.dgvThietBiPhong.Name = "dgvThietBiPhong";
            this.dgvThietBiPhong.RowHeadersWidth = 51;
            this.dgvThietBiPhong.RowTemplate.Height = 24;
            this.dgvThietBiPhong.Size = new System.Drawing.Size(631, 483);
            this.dgvThietBiPhong.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panel3.Controls.Add(this.btnReset);
            this.panel3.Controls.Add(this.txtSoLuong);
            this.panel3.Controls.Add(this.cboTenTBP);
            this.panel3.Controls.Add(this.cboTenPhong);
            this.panel3.Controls.Add(this.btnXoaTBP);
            this.panel3.Controls.Add(this.btnThemTBP);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(488, 483);
            this.panel3.TabIndex = 0;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(229, 219);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(171, 22);
            this.txtSoLuong.TabIndex = 10;
            // 
            // cboTenTBP
            // 
            this.cboTenTBP.FormattingEnabled = true;
            this.cboTenTBP.Location = new System.Drawing.Point(229, 150);
            this.cboTenTBP.Name = "cboTenTBP";
            this.cboTenTBP.Size = new System.Drawing.Size(171, 24);
            this.cboTenTBP.TabIndex = 9;
            // 
            // cboTenPhong
            // 
            this.cboTenPhong.FormattingEnabled = true;
            this.cboTenPhong.Location = new System.Drawing.Point(229, 89);
            this.cboTenPhong.Name = "cboTenPhong";
            this.cboTenPhong.Size = new System.Drawing.Size(171, 24);
            this.cboTenPhong.TabIndex = 8;
            // 
            // btnXoaTBP
            // 
            this.btnXoaTBP.Animated = true;
            this.btnXoaTBP.BorderRadius = 15;
            this.btnXoaTBP.BorderThickness = 1;
            this.btnXoaTBP.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnXoaTBP.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnXoaTBP.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnXoaTBP.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnXoaTBP.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnXoaTBP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnXoaTBP.ForeColor = System.Drawing.Color.White;
            this.btnXoaTBP.Location = new System.Drawing.Point(201, 347);
            this.btnXoaTBP.Name = "btnXoaTBP";
            this.btnXoaTBP.Size = new System.Drawing.Size(107, 50);
            this.btnXoaTBP.TabIndex = 7;
            this.btnXoaTBP.Text = "Xoá";
            // 
            // btnThemTBP
            // 
            this.btnThemTBP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnThemTBP.BorderRadius = 15;
            this.btnThemTBP.BorderThickness = 1;
            this.btnThemTBP.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnThemTBP.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnThemTBP.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnThemTBP.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnThemTBP.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnThemTBP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThemTBP.ForeColor = System.Drawing.Color.White;
            this.btnThemTBP.Location = new System.Drawing.Point(36, 347);
            this.btnThemTBP.Name = "btnThemTBP";
            this.btnThemTBP.Size = new System.Drawing.Size(107, 50);
            this.btnThemTBP.TabIndex = 5;
            this.btnThemTBP.Text = "Thêm";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label5.Location = new System.Drawing.Point(53, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 38);
            this.label5.TabIndex = 2;
            this.label5.Text = "Số lượng";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label4.Location = new System.Drawing.Point(53, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 38);
            this.label4.TabIndex = 1;
            this.label4.Text = "Tên thiết bị";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label3.Location = new System.Drawing.Point(53, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 38);
            this.label3.TabIndex = 0;
            this.label3.Text = "Tên Phòng";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.label6.Location = new System.Drawing.Point(54, 223);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 32);
            this.label6.TabIndex = 9;
            this.label6.Text = "Số lượng";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSoLuongKho
            // 
            this.txtSoLuongKho.Location = new System.Drawing.Point(189, 228);
            this.txtSoLuongKho.Name = "txtSoLuongKho";
            this.txtSoLuongKho.Size = new System.Drawing.Size(195, 22);
            this.txtSoLuongKho.TabIndex = 10;
            // 
            // btnReset
            // 
            this.btnReset.Animated = true;
            this.btnReset.BackColor = System.Drawing.Color.Transparent;
            this.btnReset.BorderRadius = 15;
            this.btnReset.BorderThickness = 1;
            this.btnReset.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReset.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReset.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReset.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReset.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(343, 347);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(107, 50);
            this.btnReset.TabIndex = 11;
            this.btnReset.Text = "Làm mới";
            this.btnReset.UseTransparentBackground = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // frmQLThietBi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 518);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmQLThietBi";
            this.Text = "Quản lý thiết bị";
            this.Load += new System.EventHandler(this.frmQLThietBi_Load_1);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThietBi)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThietBiPhong)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button btnXoaTB;
        private Guna.UI2.WinForms.Guna2Button btnSuaTB;
        private Guna.UI2.WinForms.Guna2Button btnThemTB;
        private System.Windows.Forms.TextBox txtMoTaTB;
        private System.Windows.Forms.TextBox txtTenTB;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvThietBi;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgvThietBiPhong;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.ComboBox cboTenTBP;
        private System.Windows.Forms.ComboBox cboTenPhong;
        private Guna.UI2.WinForms.Guna2Button btnXoaTBP;
        private Guna.UI2.WinForms.Guna2Button btnThemTBP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSoLuongKho;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2Button btnReset;
    }
}