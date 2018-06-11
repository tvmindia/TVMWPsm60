using System.IO;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Net.Mail;
using System.Net;
using System;
using iTextSharp.text.pdf.draw;
using Newtonsoft.Json;
using iTextSharp.tool.xml.pipeline;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class PDFGeneratorController : Controller
    {
        // GET: PDFGenerator
       
        public ActionResult Index()
        {
           //string result= SendPDFDoc("");
            return View();
        }
        [HttpPost]
        public byte[] GetPdfAttachment(PDFToolsViewModel pDFTools)
        { 
            try
            {
                string htmlBody = pDFTools.Content == null ? "" : pDFTools.Content.Replace("<br>", "<br/>").ToString().Replace("workAround:image\">", "workAround:image\"/>");
                StringReader reader = new StringReader(htmlBody.ToString());
                Document pdfDoc = new Document(PageSize.A4, -13f, -4f, 30f, 100f);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);                    
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);                    
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    return bytes;
                }
                }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public string PrintPDF(PDFTools pDFToolsObj)
        {
            //string imageURL = Server.MapPath("~/Content/images/logo.png");
            //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            ////Resize image depend upon your need
            //jpg.ScaleToFit(70f, 60f);
            //jpg.SpacingBefore = 10f;
            //jpg.SpacingAfter = 1f;

            //jpg.Alignment = Element.ALIGN_LEFT;
            string sw = pDFToolsObj.Content.Replace("<br>", "<br/>").ToString();
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 85f, 30f);
            byte[] bytes = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                Footer footobj = new Footer();
                footobj.imageURL = Server.MapPath("~/Content/images/logo.png");
                footobj.Header = XMLWorkerHelper.ParseToElementList(pDFToolsObj.Headcontent, null);
                writer.PageEvent = footobj;
                // Our custom Header and Footer is done using Event Handler
                //TwoColumnHeaderFooter PageEventHandler = new TwoColumnHeaderFooter();
                //writer.PageEvent = PageEventHandler;
                //// Define the page header
                //PageEventHandler.Title = "Column Header";
                //PageEventHandler.HeaderFont = FontFactory.GetFont(BaseFont.COURIER_BOLD, 10, Font.BOLD);
                //PageEventHandler.HeaderLeft = "Group";
                //PageEventHandler.HeaderRight = "1";
                pdfDoc.Open();
                //jpg.SetAbsolutePosition(pdfDoc.Left, pdfDoc.Top - 60);
                //pdfDoc.Add(jpg);
                //PdfContentByte cb = writer.DirectContent;
                //cb.MoveTo(pdfDoc.Left, pdfDoc.Top-60 );
                //cb.LineTo(pdfDoc.Right, pdfDoc.Top-60);
                //cb.SetLineWidth(1);
                //cb.SetColorStroke(new CMYKColor(0f, 12f, 0f, 7f));
                //cb.Stroke();
                //cb.MoveTo(pdfDoc.Left, pdfDoc.Top+5);
                //cb.LineTo(pdfDoc.Right, pdfDoc.Top+5);
                //cb.SetLineWidth(1);
                //cb.SetColorStroke(new CMYKColor(0f, 12f, 0f, 7f));
                //cb.Stroke();

                //Paragraph welcomeParagraph = new Paragraph("Hello, World!");
                // Our custom Header and Footer is done using Event Handler

                //pdfDoc.Add(welcomeParagraph);

                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                //for (int i = 0; i <= 2; i++)
                //{
                //    // Define the page header
                //    PageEventHandler.HeaderRight = i.ToString();
                //    if (i != 1)
                //    {
                //        pdfDoc.NewPage();
                //    }
                //}
                pdfDoc.Close();
                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }
            string fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), "Report.pdf");
            System.IO.File.WriteAllBytes(fname, bytes);
            //File(bytes, "application/pdf", "Report.pdf").sa
            //bytes.SaveAs(fname);
            return JsonConvert.SerializeObject(new { Result = "OK", URL = "../Content/Uploads/Report.pdf" });

        }
        public partial class Footer : PdfPageEventHelper

        {
            public string imageURL { get; set; }
            public string Tableheader { get; set; }
            public ElementList Header;
            public override void OnEndPage(PdfWriter writer, Document doc)

            {
                var FontColour = new BaseColor(0, 100, 0);
                var FontColour1 = new BaseColor(0, 100, 3);
                Font myFont1 = FontFactory.GetFont("OpenSans", 7, iTextSharp.text.Font.BOLD, FontColour1);
                PSASysCommon pSASSysCommon = new PSASysCommon();
                //Paragraph footer = new Paragraph("Generated on: "+ pSASSysCommon.GetCurrentDateTime().ToString("dd-MMM-yyyy h:mm tt"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.ITALIC));
                //footer.Alignment = Element.ALIGN_RIGHT;
                PdfPTable footerTbl = new PdfPTable(3);
                footerTbl.DefaultCell.Border = Rectangle.NO_BORDER;
                float[] ColumnWidths = new float[] { doc.PageSize.Width / 3, doc.PageSize.Width / 3, doc.PageSize.Width / 3 };
                footerTbl.SetWidths(ColumnWidths);
                footerTbl.TotalWidth = doc.PageSize.Width;
                Paragraph p = new Paragraph(new Chunk(new LineSeparator(34.01F, 102.0F, FontColour, Element.ALIGN_LEFT, 0)));
                PdfPCell cellLine = new PdfPCell(p);
                cellLine.Colspan = 3;
                cellLine.MinimumHeight = 34.01F;
                cellLine.Border = Rectangle.NO_BORDER;
                cellLine.PaddingLeft = -10;
                Phrase ph1 = new Phrase("H.O.,Demonstration Lab and Works\nKallettumkara.P.O,Thrissur-680683\nKerala(India)\nTelefax :+91 480 2881157,2881908\nEmail :mech@pilotsmithindia.com\nRly. Station : Irinjalakuda (0.5Km)\nAirport(25 Km),Seaport(55 Km) : Kochi\nGSTIN : 32AADCP8777H1Z3\nCIN No. U28939TN2005PTC057413", myFont1);
                PdfPCell cellLine1 = new PdfPCell(ph1);
                cellLine1.MinimumHeight = 90.7087F;
                cellLine1.PaddingLeft = 50.51394F;
                cellLine1.Border = Rectangle.NO_BORDER;
                Phrase ph2 = new Phrase("Registered Office and Showroom\nNew No. I,Gajapathy Street,Shenoy nagar,Chennai-600030\nTamil Nadu(India)\nTelefax :+91 44 26640966,09381078554\nEmail :madras@pilotsmithindia.com\nRly. Station : Chennai Central (6 Km)\nAirport(15 Km),Seaport(10 Km) :Chennai \nGSTIN : 33AADCP8777H1Z1", myFont1);
                PdfPCell cellLine2 = new PdfPCell(ph2);
                cellLine2.MinimumHeight = 90.7087F;
                cellLine2.PaddingLeft = 8.50394F;
                cellLine2.BorderColor = FontColour;
                cellLine2.Border = Rectangle.LEFT_BORDER;
                Phrase ph3 = new Phrase("Branch Office and Showroom\nFirst Floor,No. 691,10th Main Road\nVinayaka Layout, Nagar Bhavi\nBengaluru - 560072\nKarnataka(India)\nTelefax: +91 9108001798, 9108001799 \nEmail: bangalore@pilotsmithindia.com\nRly.Station : Bangalore City Jn(11 Km)\nAirport(40 Km) :Bengaluru \nGSTIN: 29AADCP8777H1ZQ", myFont1);
                PdfPCell cellLine3 = new PdfPCell(ph3);//8.50394
                cellLine3.MinimumHeight = 90.7087F;
                cellLine3.PaddingLeft = 8.50394F;
                cellLine3.BorderColor = FontColour;
                cellLine3.Border = Rectangle.LEFT_BORDER;
                footerTbl.AddCell(cellLine1);
                footerTbl.AddCell(cellLine2);
                footerTbl.AddCell(cellLine3);
                footerTbl.AddCell(cellLine);
                footerTbl.WriteSelectedRows(0, -1, 0, 121.732f, writer.DirectContent);

            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {

                BaseFont customfont = BaseFont.CreateFont(System.Web.HttpContext.Current.Server.MapPath("~/fonts/OpenSans-Regular.ttf"), BaseFont.CP1252, BaseFont.EMBEDDED);
                Font font = new Font(customfont);
                var FontColour = new BaseColor(0, 100, 0);
                var FontColour1 = new BaseColor(0, 100, 3);
                PdfPTable headerSectionTbl = new PdfPTable(2);
                headerSectionTbl.DefaultCell.Border = Rectangle.NO_BORDER;
                float[] ColumnWidths = new float[] { 119.055F, document.PageSize.Width - 119.055F };
                headerSectionTbl.SetWidths(ColumnWidths);
                headerSectionTbl.TotalWidth = document.PageSize.Width;
                Paragraph p = new Paragraph(new Chunk(new LineSeparator(34.01F, 102.0F, FontColour, Element.ALIGN_LEFT, 0)));
                PdfPCell cellLine = new PdfPCell(p);
                cellLine.Colspan = 2;
                cellLine.MinimumHeight = 34.01F;
                cellLine.Border = Rectangle.NO_BORDER;
                cellLine.PaddingLeft = -10;
                headerSectionTbl.AddCell(cellLine);
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                //Resize image depend upon your need
                jpg.ScaleToFit(76.5354f, 93.5433f);
                jpg.SpacingBefore = 14.1732F;
                jpg.Alignment = Element.ALIGN_RIGHT;
                Font myFont = FontFactory.GetFont("OpenSans", 12, iTextSharp.text.Font.BOLD, FontColour);
                Font myFont1 = FontFactory.GetFont("OpenSans", 9, iTextSharp.text.Font.BOLD, FontColour1);
                Font myFont2 = FontFactory.GetFont("OpenSans", 10, iTextSharp.text.Font.BOLDITALIC, FontColour1);
                string line1 = "\nPilotsmith (India) Pvt.Ltd." + "\n";
                string line2 = "www.pilotsmithindia.com" + "\n";
                string line3 = "\n" + "Manufacturers and Consultants for Food and Ayurvedic Processing Equipments." + "\n";
                Paragraph header = new Paragraph();
                Phrase ph1 = new Phrase(line1, myFont);
                Phrase ph2 = new Phrase(line2, myFont1);
                Phrase ph3 = new Phrase(line3, myFont2);
                header.Add(ph1);
                header.Add(ph2);
                header.Add(ph3);
                header.SpacingBefore = 14.1732F;
                header.Alignment = Element.ALIGN_LEFT;
                PdfPCell cellImage = new PdfPCell(jpg);
                cellImage.Border = Rectangle.NO_BORDER;
                cellImage.PaddingLeft = 42.5197F;
                headerSectionTbl.AddCell(cellImage);
                PdfPCell cell1 = new PdfPCell(header);
                cell1.Border = 0;
                cell1.PaddingLeft = 19.8425F;
                headerSectionTbl.AddCell(cell1);
                headerSectionTbl.WriteSelectedRows(0, -1, 0, document.PageSize.Height, writer.DirectContent);
            }
        }
        [HttpPost]
        public FileResult Download(PDFTools PDFTools)
        {
            // Footer footobj = new Footer();

            //jpg.Alignment = Element.ALIGN_LEFT;
            string htmlBody = PDFTools.Content == null ? "" : PDFTools.Content.Replace("<br>", "<br/>").ToString().Replace("workAround:image\">", "workAround:image\"/>");
            StringReader reader = new StringReader(htmlBody.ToString());
            Document pdfDoc = new Document(PageSize.A4, 28.3465f, 28.3465f, 141.732f, 141.732f);
            byte[] bytes = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                Footer footobj = new Footer();
                footobj.imageURL = Server.MapPath("~/Content/images/PilotLogo.png");
                writer.PageEvent = footobj;
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                pdfDoc.Close();
                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }
            string contentFileName = PDFTools.ContentFileName.ToString() == null ? "Report.pdf" : (PDFTools.ContentFileName.ToString() + " - " + PDFTools.CustomerName.ToString() + ".pdf");
            string fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), contentFileName);
            System.IO.File.WriteAllBytes(fname, bytes);
            string contentType = "application/pdf";
            return File(fname, contentType, contentFileName);
        }



    }

}

