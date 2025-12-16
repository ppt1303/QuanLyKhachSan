using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

namespace QuanLyKhachSan
{
    public partial class rptHoaDon : DevExpress.XtraReports.UI.XtraReport
    {
        public rptHoaDon()
        {
            // 1. Cấu hình trang giấy
            this.Name = "rptHoaDon";
            this.Margins = new System.Drawing.Printing.Margins(50, 50, 50, 50); // Lề chuẩn

            // 2. Vẽ giao diện
            CreateReportLayout();
        }

        private void CreateReportLayout()
        {
            // --- TẠO CÁC BĂNG (BANDS) ---
            DetailBand detail = new DetailBand();
            detail.HeightF = 35; // Chiều cao dòng

            ReportHeaderBand reportHeader = new ReportHeaderBand();
            reportHeader.HeightF = 150;

            ReportFooterBand reportFooter = new ReportFooterBand();
            reportFooter.HeightF = 100;

            this.Bands.AddRange(new Band[] { reportHeader, detail, reportFooter });

            // =================================================================
            // 1. PHẦN ĐẦU: TIÊU ĐỀ
            // =================================================================
            XRLabel lblTitle = new XRLabel();
            lblTitle.Text = "HÓA ĐƠN THANH TOÁN";
            lblTitle.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitle.TextAlignment = TextAlignment.MiddleCenter;
            lblTitle.BoundsF = new RectangleF(0, 50, 650, 40); // Width 650 (An toàn)
            reportHeader.Controls.Add(lblTitle);

            // HEADER BẢNG (Tiêu đề cột)
            XRTable tableHeader = new XRTable();
            tableHeader.BoundsF = new RectangleF(0, 110, 650, 30);
            tableHeader.Borders = BorderSide.All;
            tableHeader.BackColor = Color.LightGray;
            tableHeader.Font = new Font("Arial", 10, FontStyle.Bold);
            tableHeader.BeginInit(); // Bắt đầu vẽ

            XRTableRow headerRow = new XRTableRow();
            headerRow.Cells.Add(new XRTableCell { Text = "Nội Dung", WidthF = 450 });
            headerRow.Cells.Add(new XRTableCell { Text = "Thành Tiền", WidthF = 200, TextAlignment = TextAlignment.MiddleRight });

            tableHeader.Rows.Add(headerRow);
            tableHeader.EndInit(); // Kết thúc vẽ
            reportHeader.Controls.Add(tableHeader);

            // =================================================================
            // 2. PHẦN THÂN: DỮ LIỆU (CHI TIẾT)
            // =================================================================
            XRTable tableDetail = new XRTable();
            tableDetail.BoundsF = new RectangleF(0, 0, 650, 30);
            tableDetail.Borders = BorderSide.Bottom | BorderSide.Left | BorderSide.Right; // Viền rõ ràng
            tableDetail.BorderWidth = 1;
            tableDetail.Font = new Font("Arial", 11);
            tableDetail.BeginInit(); // Bắt đầu vẽ

            XRTableRow detailRow = new XRTableRow();

            // Ô 1: Bind vào [NoiDung]
            XRTableCell cellTen = new XRTableCell();
            cellTen.WidthF = 450;
            cellTen.Padding = new PaddingInfo(5, 0, 0, 0);
            // Binding dữ liệu
            cellTen.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[NoiDung]"));

            // Ô 2: Bind vào [ThanhTien]
            XRTableCell cellTien = new XRTableCell();
            cellTien.WidthF = 200;
            cellTien.Padding = new PaddingInfo(0, 5, 0, 0);
            cellTien.TextAlignment = TextAlignment.MiddleRight;
            // Binding dữ liệu
            cellTien.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ThanhTien]"));
            cellTien.TextFormatString = "{0:N0}";

            detailRow.Cells.Add(cellTen);
            detailRow.Cells.Add(cellTien);
            tableDetail.Rows.Add(detailRow);

            tableDetail.EndInit(); // Kết thúc vẽ

            // QUAN TRỌNG: Phải Add vào Detail Band
            detail.Controls.Add(tableDetail);

            // =================================================================
            // 3. PHẦN ĐUÔI: TỔNG CỘNG
            // =================================================================
            XRLabel lblTong = new XRLabel();
            lblTong.BoundsF = new RectangleF(350, 10, 300, 30);
            lblTong.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTong.TextAlignment = TextAlignment.MiddleRight;

            // Bind vào cột [TongCong] từ SQL
            lblTong.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TongCong]"));
            lblTong.TextFormatString = "TỔNG CỘNG: {0:N0} VNĐ";

            reportFooter.Controls.Add(lblTong);
        }
    }
}