using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Reports
{
    public class ITextEvents : PdfPageEventHelper
    {

        PdfContentByte cb;
        PdfTemplate headerTemplate, footerTemplate;
        BaseFont bf = null;
        DateTime PrintTime = DateTime.Now;

        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public string ProjectName { get; set; }
        public string LogoFileName { get; set; }

        private string _header;

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {

            }
            catch (System.IO.IOException ioe)
            {

            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);

            iTextSharp.text.Font baseFontNormal = Reporter.GetCalibri();

            iTextSharp.text.Font baseFontBig = Reporter.GetTahoma();

            Phrase p1Header = new Phrase("Gholghola Group", baseFontNormal);

            PdfPTable pdfTab = new PdfPTable(3);

            iTextSharp.text.Image logo = null;
            if (!String.IsNullOrEmpty(LogoFileName))
            {
                try
                {
                    logo = iTextSharp.text.Image.GetInstance(LogoFileName);
                    logo.ScaleToFit(95, 95);
                }
                catch { }
            }

            PdfPCell pdfCell1 = new PdfPCell(new Phrase("Project: " + ProjectName, baseFontBig));
            PdfPCell pdfCell2 = new PdfPCell();
            if (logo != null)
                pdfCell2 = new PdfPCell(logo);
            PdfPCell pdfCell3 = new PdfPCell(new Phrase("Date: _______-___-___", baseFontBig));
            String text = "Page " + writer.PageNumber + " of ";

            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(100), document.PageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(100) + len, document.PageSize.GetBottom(30));
            }

            pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell3.HorizontalAlignment = Element.ALIGN_RIGHT;

            pdfCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfCell2.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell3.VerticalAlignment = Element.ALIGN_BOTTOM;

            pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            pdfCell3.Border = 0;

            pdfTab.AddCell(pdfCell1);
            pdfTab.AddCell(pdfCell2);
            pdfTab.AddCell(pdfCell3);

            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 70;

            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 10, writer.DirectContent);

            cb.MoveTo(40, document.PageSize.Height - 110);
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 110);
            cb.Stroke();

            cb.MoveTo(40, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 12);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.EndText();
        }
    }
}
