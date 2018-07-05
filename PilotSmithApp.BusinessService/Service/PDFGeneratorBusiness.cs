using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf.draw;

namespace PilotSmithApp.BusinessService.Service
{
    public class PDFGeneratorBusiness : IPDFGeneratorBusiness
    {
        ICommonBusiness _commonBusiness;
        public PDFGeneratorBusiness(ICommonBusiness commonBusiness)
        {
            _commonBusiness = commonBusiness;
        }
        public byte[] GetPdfAttachment(PDFTools pDFTools)
        {
            try
            {
                string htmlBody = pDFTools.Content == null ? "" : pDFTools.Content.Replace("<br>", "<br/>").ToString().Replace("workAround:image\">", "workAround:image\"/>");
                StringReader reader = new StringReader(htmlBody.ToString());
                Document pdfDoc = new Document(PageSize.A4, 42.5197f, 28.3465f, 155.732f, 141.732f);
                byte[] bytes = null;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    Footer footobj = new Footer();
                    footobj.imageURL = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/LetterHead.jpg");
                    writer.PageEvent = footobj;
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    //return bytes;
                }
                string contentFileName = pDFTools.ContentFileName.ToString() == null ? "Report.pdf" : (pDFTools.ContentFileName.ToString()+".pdf");
                //string filename = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/Uploads/"), contentFileName);
                if (pDFTools.ContentFileName.ToString() == "Quotation" || pDFTools.ContentFileName.ToString() == "ProformaInvoice" || pDFTools.ContentFileName.ToString() == "SaleInvoice")
                {
                    string demo = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/Uploads/"), "Demo" + contentFileName);
                    System.IO.File.WriteAllBytes(demo, bytes);
                    return MergePDFs(demo, System.Web.HttpContext.Current.Server.MapPath("~/Content/images/Terms&Cond.pdf"));
                }
                else
                {
                    return bytes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private byte[] MergePDFs(params string[] filesPath)
        {
            List<PdfReader> readerList = new List<PdfReader>();
            foreach (string filePath in filesPath)
            {
                PdfReader pdfReader = new PdfReader(filePath);
                readerList.Add(pdfReader);
            }

            //Define a new output document and its size, type
            Document document = new Document(PageSize.A4, 0, 0, 0, 0);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //Create blank output pdf file and get the stream to write on it.
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                foreach (PdfReader reader in readerList)
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        document.Add(iTextSharp.text.Image.GetInstance(page));
                    }
                }
                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                return bytes;
            }

        }
        public partial class Footer : PdfPageEventHelper

        {
            public string imageURL { get; set; }
            public string Tableheader { get; set; }
            public ElementList Header;
            public override void OnEndPage(PdfWriter writer, Document doc)

            {
                //var FontColour = new BaseColor(0, 100, 0);
                //var FontColour1 = new BaseColor(0, 100, 3);
                //Font myFont1 = FontFactory.GetFont("OpenSans", 7, iTextSharp.text.Font.BOLD, FontColour1);
                //PSASysCommon pSASSysCommon = new PSASysCommon();
                ////Paragraph footer = new Paragraph("Generated on: "+ pSASSysCommon.GetCurrentDateTime().ToString("dd-MMM-yyyy h:mm tt"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.ITALIC));
                ////footer.Alignment = Element.ALIGN_RIGHT;
                //PdfPTable footerTbl = new PdfPTable(3);
                //footerTbl.DefaultCell.Border = Rectangle.NO_BORDER;
                //float[] ColumnWidths = new float[] { doc.PageSize.Width / 3, doc.PageSize.Width / 3, doc.PageSize.Width / 3 };
                //footerTbl.SetWidths(ColumnWidths);
                //footerTbl.TotalWidth = doc.PageSize.Width;
                //Paragraph p = new Paragraph(new Chunk(new LineSeparator(34.01F, 102.0F, FontColour, Element.ALIGN_LEFT, 0)));
                //PdfPCell cellLine = new PdfPCell(p);
                //cellLine.Colspan = 3;
                //cellLine.MinimumHeight = 34.01F;
                //cellLine.Border = Rectangle.NO_BORDER;
                //cellLine.PaddingLeft = -10;
                //Phrase ph1 = new Phrase("H.O.,Demonstration Lab and Works\nKallettumkara.P.O,Thrissur-680683\nKerala(India)\nTelefax :+91 480 2881157,2881908\nEmail :mech@pilotsmithindia.com\nRly. Station : Irinjalakuda (0.5Km)\nAirport(25 Km),Seaport(55 Km) : Kochi\nGSTIN : 32AADCP8777H1Z3\nCIN No. U28939TN2005PTC057413", myFont1);
                //PdfPCell cellLine1 = new PdfPCell(ph1);
                //cellLine1.MinimumHeight = 90.7087F;
                //cellLine1.PaddingLeft = 50.51394F;
                //cellLine1.Border = Rectangle.NO_BORDER;
                //Phrase ph2 = new Phrase("Registered Office and Showroom\nNew No. I,Gajapathy Street,Shenoy nagar,Chennai-600030\nTamil Nadu(India)\nTelefax :+91 44 26640966,09381078554\nEmail :madras@pilotsmithindia.com\nRly. Station : Chennai Central (6 Km)\nAirport(15 Km),Seaport(10 Km) :Chennai \nGSTIN : 33AADCP8777H1Z1", myFont1);
                //PdfPCell cellLine2 = new PdfPCell(ph2);
                //cellLine2.MinimumHeight = 90.7087F;
                //cellLine2.PaddingLeft = 8.50394F;
                //cellLine2.BorderColor = FontColour;
                //cellLine2.Border = Rectangle.LEFT_BORDER;
                //Phrase ph3 = new Phrase("Branch Office and Showroom\nFirst Floor,No. 691,10th Main Road\nVinayaka Layout, Nagar Bhavi\nBengaluru - 560072\nKarnataka(India)\nTelefax: +91 9108001798, 9108001799 \nEmail: bangalore@pilotsmithindia.com\nRly.Station : Bangalore City Jn(11 Km)\nAirport(40 Km) :Bengaluru \nGSTIN: 29AADCP8777H1ZQ", myFont1);
                //PdfPCell cellLine3 = new PdfPCell(ph3);//8.50394
                //cellLine3.MinimumHeight = 90.7087F;
                //cellLine3.PaddingLeft = 8.50394F;
                //cellLine3.BorderColor = FontColour;
                //cellLine3.Border = Rectangle.LEFT_BORDER;
                //footerTbl.AddCell(cellLine1);
                //footerTbl.AddCell(cellLine2);
                //footerTbl.AddCell(cellLine3);
                //footerTbl.AddCell(cellLine);
                //footerTbl.WriteSelectedRows(0, -1, 0, 121.732f, writer.DirectContent);

            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {

                //BaseFont customfont = BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/fonts/OpenSans-Regular.ttf"), BaseFont.CP1252, BaseFont.EMBEDDED);
                //Font font = new Font(customfont);
                //var FontColour = new BaseColor(0, 100, 0);
                //var FontColour1 = new BaseColor(0, 100, 3);
                //PdfPTable headerSectionTbl = new PdfPTable(2);
                //headerSectionTbl.DefaultCell.Border = Rectangle.NO_BORDER;
                //float[] ColumnWidths = new float[] { 119.055F, document.PageSize.Width - 119.055F };
                //headerSectionTbl.SetWidths(ColumnWidths);
                //headerSectionTbl.TotalWidth = document.PageSize.Width;
                //Paragraph p = new Paragraph(new Chunk(new LineSeparator(34.01F, 102.0F, FontColour, Element.ALIGN_LEFT, 0)));
                //PdfPCell cellLine = new PdfPCell(p);
                //cellLine.Colspan = 2;
                //cellLine.MinimumHeight = 34.01F;
                //cellLine.Border = Rectangle.NO_BORDER;
                //cellLine.PaddingLeft = -10;
                //headerSectionTbl.AddCell(cellLine);
                //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                //jpg.SetAbsolutePosition(0, 0);
                //Resize image depend upon your need
                //jpg.ScaleToFit(76.5354f, 93.5433f);
                //jpg.SpacingBefore = 14.1732F;
                //jpg.Alignment = Element.ALIGN_RIGHT;
                //Font myFont = FontFactory.GetFont("OpenSans", 12, iTextSharp.text.Font.BOLD, FontColour);
                //Font myFont1 = FontFactory.GetFont("OpenSans", 9, iTextSharp.text.Font.BOLD, FontColour1);
                //Font myFont2 = FontFactory.GetFont("OpenSans", 10, iTextSharp.text.Font.BOLDITALIC, FontColour1);
                //string line1 = "\nPilotsmith (India) Pvt.Ltd." + "\n";
                //string line2 = "www.pilotsmithindia.com" + "\n";
                //string line3 = "\n" + "Manufacturers and Consultants for Food and Ayurvedic Processing Equipments." + "\n";
                //Paragraph header = new Paragraph();
                //Phrase ph1 = new Phrase(line1, myFont);
                //Phrase ph2 = new Phrase(line2, myFont1);
                //Phrase ph3 = new Phrase(line3, myFont2);
                //header.Add(ph1);
                //header.Add(ph2);
                //header.Add(ph3);
                //header.SpacingBefore = 14.1732F;
                //header.Alignment = Element.ALIGN_LEFT;
                //PdfPCell cellImage = new PdfPCell(jpg);
                //cellImage.Border = Rectangle.NO_BORDER;
                //cellImage.PaddingLeft = 42.5197F;
                //headerSectionTbl.AddCell(cellImage);
                //PdfPCell cell1 = new PdfPCell(header);
                //cell1.Border = 0;
                //cell1.PaddingLeft = 19.8425F;
                //headerSectionTbl.AddCell(cell1);
                //headerSectionTbl.WriteSelectedRows(0, -1, 0, document.PageSize.Height, writer.DirectContent);
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                jpg.SetAbsolutePosition(0, 0);
                document.Add(jpg);
            }
        }
    }
}
