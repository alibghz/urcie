using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Reports
{
	public class ImageToPdf
	{
		public iTextSharp.text.Rectangle PdfPageSize { get; set; }
		public bool FitImagesToPage { get; set; }
		public int ImagesInWidth { get; set; }
		public int ImagesInHeight { get; set; }
		public float WidthSpace { get; set; }
		public float HeightSpace { get; set; }
		public PointF StartPoint { get; set; }
		public PointF EndPoint { get; set; }
		public string OutPdfPath { get; set; }
		public IList<object> Images { get; set; }

		public bool ExportToPdf()
		{
			if (PdfPageSize == null) PdfPageSize = PageSize.A4;
			if (ImagesInHeight < 1) ImagesInHeight = 1;
			if (ImagesInWidth < 1) ImagesInWidth = 1;
			if (WidthSpace < 0) WidthSpace = 0;
			if (HeightSpace < 0) HeightSpace = 0;
			if (StartPoint == null) StartPoint = new PointF(0, 0);
			if (EndPoint == null || (EndPoint.X == 0 && EndPoint.Y == 0))
				EndPoint = new PointF(PdfPageSize.Width, PdfPageSize.Height);
			if (Images == null || Images.Count < 1) return false;
			if (OutPdfPath == null || OutPdfPath.Trim() == "") return false;

			var pdfDoc = new Document(PdfPageSize);
			var pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(OutPdfPath, FileMode.Create));
			pdfWriter.SetPdfVersion(new PdfName("1.5"));
			pdfWriter.CompressionLevel = PdfStream.BEST_COMPRESSION;
			pdfDoc.Open();

			int pageCount = Images.Count / (ImagesInHeight * ImagesInWidth) + 1, imgCount = 0;

			for (int i = 0; i < pageCount; i++)
			{
				if (imgCount >= Images.Count) break;
				for (int j = 0; j < ImagesInHeight; j++)
				{
					if (imgCount >= Images.Count) break;
					for (int k = 0; k < ImagesInWidth; k++)
					{
						if (imgCount >= Images.Count) break;
						iTextSharp.text.Image img = null;
						var image = Images[imgCount];

						if (image is string)
							img = iTextSharp.text.Image.GetInstance((string)image);
						else if (image is byte[])
							img = iTextSharp.text.Image.GetInstance((byte[])image);
						else if (image is Stream)
							img = iTextSharp.text.Image.GetInstance((Stream)image);
						else if (image is Uri)
							img = iTextSharp.text.Image.GetInstance((Uri)image);
						else if (image is System.Drawing.Image)
							img = iTextSharp.text.Image.GetInstance((System.Drawing.Image)image, ((System.Drawing.Image)image).RawFormat);

						if (img == null) return false;

						float w = EndPoint.X - StartPoint.X, h = EndPoint.Y - StartPoint.Y;
						if (ImagesInWidth > 1)
							w = w / ImagesInWidth - WidthSpace / (ImagesInWidth - 1);
						if (ImagesInHeight > 1)
							h = h / ImagesInHeight - HeightSpace / (ImagesInHeight - 1);
						if (FitImagesToPage)
							img.ScaleToFit(w, h);

						float x = StartPoint.X + k * (w + WidthSpace);
						float y = StartPoint.Y + j * (h + HeightSpace);
						img.SetAbsolutePosition(x, y);
						pdfDoc.Add(img);
						imgCount++;
					}
				}
				pdfDoc.NewPage();
			}
			pdfDoc.Close();
			return true;
		}
	}
}
