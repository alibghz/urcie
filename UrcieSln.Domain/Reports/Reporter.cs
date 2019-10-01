using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;
using UrcieSln.Domain.Models;

namespace UrcieSln.Domain.Reports
{
    public static class Reporter
    {
        public static iTextSharp.text.Font GetFont(string FontName, string FontPath, bool IsFullPath = false)
        {
            if (!FontFactory.IsRegistered(FontName))
            {
                var fontPath = IsFullPath ? FontPath : Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\" + FontPath;
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(FontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }

        public static iTextSharp.text.Font GetBZar()
        {
            return GetFont("b zar", "bzar.ttf");
        }

        public static iTextSharp.text.Font GetTahoma()
        {
            return GetFont("Tahoma", "tahoma.ttf");
        }

        public static iTextSharp.text.Font GetCalibri()
        {
            return GetFont("calibri", "calibri.ttf");
        }

        public static bool CreateTemplateReport(string pdfReportTemplateAddress, string finalPdfReportAddress, IDictionary<string, string> report, iTextSharp.text.Font font = null)
        {
            bool done = false;
            if (font == null) font = GetTahoma();
            using (var existingFileStream = new FileStream(pdfReportTemplateAddress, FileMode.Open))
            using (var newFileStream = new FileStream(finalPdfReportAddress, FileMode.Create))
            {
                var pdfReader = new PdfReader(existingFileStream);
                var stamper = new PdfStamper(pdfReader, newFileStream);
                stamper.AcroFields.AddSubstitutionFont(font.BaseFont);
                var form = stamper.AcroFields;

                foreach (var item in report)
                {
                    form.SetField(item.Key, item.Value);
                }

                stamper.FormFlattening = false;

                stamper.Close();
                pdfReader.Close();
                done = true;
            }

            return done;
        }

        public static bool ItemsReport(string destination, IList<PdfPTable> tables, string projectName, string logoPath)
        {

            var done = false;
            var pdfDoc = new Document(PageSize.A4, 40, 40, 115, 55);
            var pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(destination, FileMode.Create));
            var textEvents = new ITextEvents();
            textEvents.ProjectName = projectName;
            textEvents.LogoFileName = logoPath;

            pdfWriter.PageEvent = textEvents;
            pdfWriter.SetPdfVersion(new PdfName("1.5"));
            pdfWriter.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfDoc.Open();

            if (tables != null)
                foreach (var table in tables)
                    if (table != null)
                    {
                        table.SpacingAfter = 10;
                        pdfDoc.Add(table);
                    }
            pdfDoc.Close();
            done = true;
            return done;
        }

        public static bool ProfilesReport(string destination, IList<Profile> items, bool includesPrice = false, int unitQty = 1)
        {
            var done = false;
            var pdfDoc = new Document(PageSize.A4);
            var pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(destination, FileMode.Create));
            pdfWriter.SetPdfVersion(new PdfName("1.5"));
            pdfWriter.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfDoc.Open();

            var tables = ProfilesTable(items, includesPrice: includesPrice, unitQty: unitQty);

            if (tables != null)
                foreach (var table in tables)
                    if (table != null)
                    {
                        table.SpacingAfter = 10;
                        pdfDoc.Add(table);
                    }
            pdfDoc.Close();
            done = true;

            return done;
        }

        public static bool FillingsReport(string destination, IList<Filling> items, bool includeTolerance = true, bool includesPrice = false, int unitQty = 1)
        {
            var done = false;
            var pdfDoc = new Document(PageSize.A4);
            var pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream(destination, FileMode.Create));
            pdfWriter.SetPdfVersion(new PdfName("1.5"));
            pdfWriter.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfDoc.Open();

            var tables = FillingsTable(items, includeTolerance, includesPrice);

            if (tables != null)
                foreach (var table in tables)
                    if (table != null)
                    {
                        table.SpacingAfter = 10;
                        pdfDoc.Add(table);
                    }
            pdfDoc.Close();
            done = true;

            return done;
        }

        public static PdfPTable[] ImageTable(System.Drawing.Image image)
        {
            if (image == null) return null;

            var tables = new PdfPTable[1];
            tables[0] = new PdfPTable(1);
            tables[0].DefaultCell.Border = Rectangle.NO_BORDER;
            tables[0].WidthPercentage = 100;
            var img = iTextSharp.text.Image.GetInstance((System.Drawing.Image)image, ImageFormat.Png);
            tables[0].AddCell(img);

            return tables;
        }

        public static PdfPTable[] AccessoriesTable(IList<Accessory> items, bool includesPrice = false, bool onlyTotal = false, int unitQty = 1)
        {
            if (items == null || items.Count < 1) return new PdfPTable[1];

            var itemTypes = items.GroupBy(x => x.AccessoryType.Name);
            var tables = new PdfPTable[1];

            var clmns = 3;
            if (includesPrice)
                clmns = 4;

            tables[0] = new PdfPTable(clmns);
            tables[0].WidthPercentage = 100;
            tables[0].HeaderRows = 2;
            tables[0].AddCell(new PdfPCell { Phrase = new Phrase("Accessories"), HorizontalAlignment = Element.ALIGN_CENTER, Colspan = clmns });
            tables[0].AddCell("#");
            tables[0].AddCell("Name");
            tables[0].AddCell("QTY");
            if (includesPrice) tables[0].AddCell("Estimated Price");
            int rowNo = 1;
            double sum = 0;
            foreach (var i in itemTypes)
            {
                tables[0].AddCell(rowNo.ToString());
                tables[0].AddCell(i.FirstOrDefault().AccessoryType.Name);
                var qty = i.Count() * i.FirstOrDefault().Quantity;
                var estimatedPrice = qty * i.FirstOrDefault().AccessoryType.Price.Sale;
                tables[0].AddCell(qty.ToString());
                if (includesPrice) tables[0].AddCell(estimatedPrice.ToString());
                sum += estimatedPrice;
            }
            if (includesPrice)
            {
                tables[0].AddCell("");
                tables[0].AddCell("");
                tables[0].AddCell("Total Estimated Price");
                tables[0].AddCell(sum.ToString());
            }
            return tables;
        }

        public static PdfPTable[] ProfilesTable(IList<Profile> items, bool includesPrice = false, bool onlyTotal = false, int unitQty = 1)
        {
            if (items == null || items.Count < 1) return new PdfPTable[1];

            var itemTypes = items.GroupBy(x => x.ProfileType.Name);
            var tables = new PdfPTable[itemTypes.Count()];
            int count = 0;

            if (!onlyTotal)
                foreach (var i in itemTypes)
                {
                    var clmns = 4;
                    if (includesPrice) clmns = 5;
                    tables[count] = new PdfPTable(clmns);
                    tables[count].WidthPercentage = 100;
                    tables[count].HeaderRows = 2;
                    tables[count].AddCell(new PdfPCell { Phrase = new Phrase(i.FirstOrDefault().ProfileType.ToString()), HorizontalAlignment = Element.ALIGN_CENTER, Colspan = clmns });
                    tables[count].AddCell("#");
                    tables[count].AddCell("Codes");
                    tables[count].AddCell("Length (mm)");
                    tables[count].AddCell("QTY");
                    if (includesPrice)
                    {
                        tables[count].AddCell("Price");
                    }
                    int rowNo = 1;
                    var unitPrice = i.FirstOrDefault().ProfileType.PricePerLength;
                    foreach (var j in i.GroupBy(y => y.Length))
                    {
                        tables[count].AddCell(rowNo.ToString());
                        var codes = "";
                        foreach (var item in j)
                        {
                            if (!codes.Contains(item.Code))
                                codes += item.Code + ", ";
                        }
                        tables[count].AddCell(codes);
                        var length = j.FirstOrDefault().Length;
                        var pCount = j.Count() * unitQty;
                        tables[count].AddCell(length.ToString());
                        tables[count].AddCell(pCount.ToString());
                        if (includesPrice)
                        {
                            tables[count].AddCell(Math.Round(unitPrice.Sale * length * pCount, 2).ToString() + " " + unitPrice.Currency.ToString());
                        }
                        rowNo++;
                    }
                    tables[count].AddCell("Total");
                    tables[count].AddCell("");
                    tables[count].AddCell(i.Sum(x => x.Length).ToString() + " mm");
                    tables[count].AddCell((i.Count() * unitQty).ToString());
                    tables[count].AddCell(Math.Round(unitPrice.Sale * i.Sum(x => x.Length) /* * i.Count()*/, 2).ToString() + " " + unitPrice.Currency.ToString());

                    count++;
                }
            else
            {
                var clmns = 3;
                if (includesPrice) clmns = 4;
                tables[count] = new PdfPTable(clmns);
                tables[count].WidthPercentage = 100;
                tables[count].HeaderRows = 2;
                tables[count].AddCell(new PdfPCell { Phrase = new Phrase("Profiles"), HorizontalAlignment = Element.ALIGN_CENTER, Colspan = clmns });
                tables[count].AddCell("#");
                tables[count].AddCell("Type");
                tables[count].AddCell("Total Length (mm)");
                if (includesPrice)
                    tables[count].AddCell("Estimated Cost");

                int rowNo = 1;
                double totalCost = 0;
                foreach (var i in itemTypes)
                {
                    tables[count].AddCell(rowNo.ToString());
                    tables[count].AddCell(i.FirstOrDefault().ProfileType.ToString());
                    tables[count].AddCell(i.Sum(x => x.Length).ToString());
                    if (includesPrice)
                    {
                        double estimatedCost = Math.Round(i.FirstOrDefault().ProfileType.PricePerLength.Sale * i.Sum(x => x.Length) * unitQty/** i.Count()*/, 2);
                        totalCost += estimatedCost;

                        tables[count].AddCell(estimatedCost.ToString() + " " + i.FirstOrDefault().ProfileType.PricePerLength.Currency.ToString());
                    }
                    rowNo++;
                }

                if (includesPrice)
                {
                    tables[count].AddCell(new PdfPCell { Phrase = new Phrase("Total Estimated Cost"), HorizontalAlignment = Element.ALIGN_RIGHT, Colspan = 3 });
                    tables[count].AddCell(Math.Round(totalCost, 2).ToString() + " " + itemTypes.FirstOrDefault().FirstOrDefault().ProfileType.PricePerLength.Currency.ToString());
                }
            }

            return tables;
        }

        public static PdfPTable[] FillingsTable(IList<Filling> items, bool includeTolerance = true, bool includesPrice = false, bool onlyTotal = false)
        {
            if (items == null || items.Count < 1) return new PdfPTable[1];

            var itemTypes = items.GroupBy(x => x.FillingType.Name);
            var tables = new PdfPTable[itemTypes.Count()];
            int count = 0;

            if (!onlyTotal)
                foreach (var i in itemTypes)
                {
                    var clmns = 6;
                    if (includesPrice) clmns = 7;
                    tables[count] = new PdfPTable(clmns);
                    tables[count].WidthPercentage = 100;
                    tables[count].HeaderRows = 2;
                    tables[count].AddCell(new PdfPCell { Phrase = new Phrase(i.FirstOrDefault().FillingType.Name.ToString()), HorizontalAlignment = Element.ALIGN_CENTER, Colspan = clmns });
                    tables[count].AddCell("#");
                    tables[count].AddCell("Codes");
                    tables[count].AddCell("Width (mm)");
                    tables[count].AddCell("Height (mm)");
                    tables[count].AddCell("QTY");
                    tables[count].AddCell("Area (m2)");
                    if (includesPrice) tables[count].AddCell("Estimated Cost");
                    float rowNo = 1, sumArea = 0, sumQty = 0;
                    foreach (var j in i.GroupBy(x => x.Code))
                    {
                        float w = j.FirstOrDefault().Parent.Width, h = j.FirstOrDefault().Parent.Height;
                        if (includeTolerance)
                        {
                            w += j.FirstOrDefault().FillingType.Tolerance;
                            h += j.FirstOrDefault().FillingType.Tolerance;
                        }
                        tables[count].AddCell(rowNo.ToString());
                        tables[count].AddCell(j.FirstOrDefault().Code);
                        tables[count].AddCell(w.ToString());
                        tables[count].AddCell(h.ToString());
                        var qty = j.Count();
                        float area = w * h / 1000000 * qty;
                        sumArea += area;
                        sumQty += qty;
                        tables[count].AddCell(qty.ToString());
                        tables[count].AddCell(area.ToString());
                        if (includesPrice)
                            tables[count].AddCell(Math.Round(j.FirstOrDefault().FillingType.PricePerSquare.Sale * area * 1000000, 2).ToString()
                            + " " + j.FirstOrDefault().FillingType.PricePerSquare.Currency.ToString());
                        rowNo++;
                    }
                    tables[count].AddCell("");
                    tables[count].AddCell("");
                    tables[count].AddCell("");
                    tables[count].AddCell("");
                    tables[count].AddCell(sumQty.ToString());
                    tables[count].AddCell(sumArea.ToString() + " m2");
                    if (includesPrice)
                        tables[count].AddCell(Math.Round(i.FirstOrDefault().FillingType.PricePerSquare.Sale * sumArea * 1000000, 2).ToString()
                        + " " + i.FirstOrDefault().FillingType.PricePerSquare.Currency.ToString());
                    count++;
                }
            else
            {
                var clmns = 3;
                if (includesPrice) clmns = 4;
                tables[count] = new PdfPTable(clmns);
                tables[count].WidthPercentage = 100;
                tables[count].HeaderRows = 2;
                tables[count].AddCell(new PdfPCell { Phrase = new Phrase("Glass/Board"), HorizontalAlignment = Element.ALIGN_CENTER, Colspan = clmns });
                tables[count].AddCell("#");
                tables[count].AddCell("Type");
                tables[count].AddCell("Total Area (m2)");
                if (includesPrice)
                    tables[count].AddCell("Estimated Cost");
                int rowNo = 1;
                double totalCost = 0;
                foreach (var i in itemTypes)
                {
                    float area = 0;
                    foreach (var j in i.GroupBy(x => x.Code))
                    {
                        float w = j.FirstOrDefault().Parent.Width, h = j.FirstOrDefault().Parent.Height;
                        if (includeTolerance)
                        {
                            w += j.FirstOrDefault().FillingType.Tolerance;
                            h += j.FirstOrDefault().FillingType.Tolerance;
                        }
                        area = area + (w * h / 1000000) * j.Count();
                    }

                    tables[count].AddCell(rowNo.ToString());
                    tables[count].AddCell(i.FirstOrDefault().FillingType.Name.ToString());
                    tables[count].AddCell(area.ToString());
                    if (includesPrice)
                    {
                        double estimatedCost = Math.Round(i.FirstOrDefault().FillingType.PricePerSquare.Sale * area * 1000000, 2);
                        totalCost += estimatedCost;

                        tables[count].AddCell(estimatedCost.ToString() + " " + i.FirstOrDefault().FillingType.PricePerSquare.Currency.ToString());
                    }
                    rowNo++;
                }
                if (includesPrice)
                {
                    tables[count].AddCell(new PdfPCell { Phrase = new Phrase("Total Estimated Cost"), HorizontalAlignment = Element.ALIGN_RIGHT, Colspan = 3 });
                    tables[count].AddCell(Math.Round(totalCost, 2).ToString() + " " + itemTypes.FirstOrDefault().FirstOrDefault().FillingType.PricePerSquare.Currency.ToString());
                }
            }
            return tables;
        }

        public static PdfPTable[] MuntinsTable(IList<Muntin> items, bool includesPrice = false, bool onlyTotal = false, int unitQty = 1)
        {
            if (items == null || items.Count < 1) return new PdfPTable[1];

            var itemTypes = items.GroupBy(x => x.MuntinType.Id);
            var tables = new PdfPTable[itemTypes.Count()];
            int count = 0;

            foreach (var i in itemTypes)
            {
                tables[count] = new PdfPTable(5);
                tables[count].WidthPercentage = 100;
                tables[count].HeaderRows = 2;
                tables[count].AddCell(new PdfPCell { Phrase = new Phrase(i.FirstOrDefault().MuntinType.ToString()), HorizontalAlignment = Element.ALIGN_CENTER, Colspan = 5 });
                tables[count].AddCell("#");
                tables[count].AddCell("Codes");
                tables[count].AddCell("Width (mm)");
                tables[count].AddCell("Height (mm)");
                tables[count].AddCell("Area (m)");
                float rowNo = 1, sumArea = 0;
                foreach (var j in i)
                {
                    float w = j.Width, h = j.Height;

                    tables[count].AddCell(rowNo.ToString());
                    tables[count].AddCell(j.Code);
                    tables[count].AddCell(w.ToString());
                    tables[count].AddCell(h.ToString());
                    float area = w * h / 1000000;
                    sumArea += area;
                    tables[count].AddCell(area.ToString());
                    rowNo++;
                }
                tables[count].AddCell("");
                tables[count].AddCell("");
                tables[count].AddCell("");
                tables[count].AddCell("");
                tables[count].AddCell(sumArea.ToString() + " m2");
                count++;
            }
            return tables;
        }
    }
}
