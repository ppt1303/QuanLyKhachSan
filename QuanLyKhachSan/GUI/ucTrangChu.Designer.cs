namespace QuanLyKhachSan.GUI
{
    partial class ucTrangChu
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
            this.pnlThongKe = new DevExpress.XtraEditors.PanelControl();
            this.pnlBoLoc = new DevExpress.XtraEditors.PanelControl();
            this.tileControlPhong = new DevExpress.XtraEditors.TileControl();
            ((System.ComponentModel.ISupportInitialize)(this.pnlThongKe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBoLoc)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlThongKe
            // 
            this.pnlThongKe.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlThongKe.Location = new System.Drawing.Point(0, 0);
            this.pnlThongKe.Name = "pnlThongKe";
            this.pnlThongKe.Size = new System.Drawing.Size(1590, 80);
            this.pnlThongKe.TabIndex = 0;
            // 
            // pnlBoLoc
            // 
            this.pnlBoLoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBoLoc.Location = new System.Drawing.Point(0, 80);
            this.pnlBoLoc.Name = "pnlBoLoc";
            this.pnlBoLoc.Size = new System.Drawing.Size(1590, 76);
            this.pnlBoLoc.TabIndex = 1;
            // 
            // tileControlPhong
            // 
            this.tileControlPhong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileControlPhong.Location = new System.Drawing.Point(0, 156);
            this.tileControlPhong.Name = "tileControlPhong";
            this.tileControlPhong.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tileControlPhong.Size = new System.Drawing.Size(1590, 723);
            this.tileControlPhong.TabIndex = 2;
            this.tileControlPhong.Text = "tileControl1";
            // 
            // ucTrangChu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tileControlPhong);
            this.Controls.Add(this.pnlBoLoc);
            this.Controls.Add(this.pnlThongKe);
            this.Name = "ucTrangChu";
            this.Size = new System.Drawing.Size(1590, 879);
            ((System.ComponentModel.ISupportInitialize)(this.pnlThongKe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBoLoc)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlThongKe;
        private DevExpress.XtraEditors.PanelControl pnlBoLoc;
        private DevExpress.XtraEditors.TileControl tileControlPhong;
    }
}
