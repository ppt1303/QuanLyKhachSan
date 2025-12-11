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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTB = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnXoaTB = new Guna.UI2.WinForms.Guna2Button();
            this.btnSuaTB = new Guna.UI2.WinForms.Guna2Button();
            this.btnThemTB = new Guna.UI2.WinForms.Guna2Button();
            this.lblTenTB = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtMoTaTB = new System.Windows.Forms.TextBox();
            this.txtTenTB = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvThietBi = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThietBi)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.lblTB);
            this.panel1.Controls.Add(this.btnXoaTB);
            this.panel1.Controls.Add(this.btnSuaTB);
            this.panel1.Controls.Add(this.btnThemTB);
            this.panel1.Controls.Add(this.lblTenTB);
            this.panel1.Controls.Add(this.txtMoTaTB);
            this.panel1.Controls.Add(this.txtTenTB);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(497, 442);
            this.panel1.TabIndex = 1;
            // 
            // lblTB
            // 
            this.lblTB.AutoSize = false;
            this.lblTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lblTB.Location = new System.Drawing.Point(42, 190);
            this.lblTB.Name = "lblTB";
            this.lblTB.Size = new System.Drawing.Size(103, 32);
            this.lblTB.TabIndex = 7;
            this.lblTB.Text = "Mô tả";
            this.lblTB.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.btnXoaTB.Location = new System.Drawing.Point(336, 290);
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
            this.btnSuaTB.Location = new System.Drawing.Point(189, 290);
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
            this.btnThemTB.Location = new System.Drawing.Point(38, 290);
            this.btnThemTB.Name = "btnThemTB";
            this.btnThemTB.Size = new System.Drawing.Size(107, 50);
            this.btnThemTB.TabIndex = 4;
            this.btnThemTB.Text = "Thêm";
            // 
            // lblTenTB
            // 
            this.lblTenTB.AutoSize = false;
            this.lblTenTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lblTenTB.Location = new System.Drawing.Point(42, 77);
            this.lblTenTB.Name = "lblTenTB";
            this.lblTenTB.Size = new System.Drawing.Size(103, 32);
            this.lblTenTB.TabIndex = 3;
            this.lblTenTB.Text = "Tên thiết bị";
            this.lblTenTB.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMoTaTB
            // 
            this.txtMoTaTB.Location = new System.Drawing.Point(189, 200);
            this.txtMoTaTB.Name = "txtMoTaTB";
            this.txtMoTaTB.Size = new System.Drawing.Size(195, 22);
            this.txtMoTaTB.TabIndex = 1;
            //this.txtMoTaTB.TextChanged += new System.EventHandler(this.txtGia_TextChanged);
            // 
            // txtTenTB
            // 
            this.txtTenTB.Location = new System.Drawing.Point(189, 87);
            this.txtTenTB.Name = "txtTenTB";
            this.txtTenTB.Size = new System.Drawing.Size(195, 22);
            this.txtTenTB.TabIndex = 0;
            //this.txtTenTB.TextChanged += new System.EventHandler(this.txtTenDV_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvThietBi);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(497, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(631, 442);
            this.panel2.TabIndex = 2;
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
            this.dgvThietBi.Size = new System.Drawing.Size(631, 442);
            this.dgvThietBi.TabIndex = 0;
            // 
            // QLThietBi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 442);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "QLThietBi";
            this.Text = "Quản lý thiết bị";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThietBi)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTB;
        private Guna.UI2.WinForms.Guna2Button btnXoaTB;
        private Guna.UI2.WinForms.Guna2Button btnSuaTB;
        private Guna.UI2.WinForms.Guna2Button btnThemTB;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTenTB;
        private System.Windows.Forms.TextBox txtMoTaTB;
        private System.Windows.Forms.TextBox txtTenTB;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvThietBi;
    }
}