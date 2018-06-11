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
        public string PrintPDF(PDFToolsViewModel pDFToolsObj)
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
                footobj.imageURL=Server.MapPath("~/Content/images/logo.png");
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
                PSASysCommon pSASSysCommon = new PSASysCommon();
                Paragraph footer = new Paragraph("This is a Computer Generated Document on: " + pSASSysCommon.GetCurrentDateTime().ToString("dd-MMM-yyyy h:mm tt"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.ITALIC));
                footer.Alignment = Element.ALIGN_RIGHT;
                PdfPTable footerTbl = new PdfPTable(1);
                footerTbl.TotalWidth = 400;
                footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell cell = new PdfPCell(footer);
                cell.Border = 0;
                cell.PaddingLeft = 10;
                footerTbl.AddCell(cell);
                footerTbl.WriteSelectedRows(0, -1,250, 30, writer.DirectContent);

            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {

                //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                ////Resize image depend upon your need
                //jpg.ScaleToFit(90.70f, 70.86f);
                ////jpg.SpacingBefore = 39.68f;
                ////jpg.SpacingAfter = 22.67f;
                //jpg.Alignment = Element.ALIGN_LEFT;
                ////jpg.SetAbsolutePosition(document.Left, document.Top - 60);
                //Font myFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 14, iTextSharp.text.Font.BOLD);
                //Font myFont1 = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10, iTextSharp.text.Font.NORMAL);
                //Font myFont2 = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, iTextSharp.text.Font.BOLDITALIC);
                //string line1 = "Pilotsmith (India) Pvt.Ltd." + "\n";
                //string line2 = "www.pilotsmithindia.com" + "\n";
                //string line3 = "\n"+"Manufacturers and Consultants for Food and Ayurvedic Processing Equipments." + "\n";
                //Paragraph header = new Paragraph();
                //Phrase ph1 = new Phrase(line1, myFont);
                //Phrase ph2 = new Phrase(line2, myFont1);
                //Phrase ph3 = new Phrase(line3, myFont2);
                //header.Add(ph1);
                //header.Add(ph2);
                //header.Add(ph3);
                //header.Alignment = Element.ALIGN_LEFT;
                //PdfPTable headerTbl = new PdfPTable(2);
                //headerTbl.TotalWidth = document.PageSize.Width;
                ////headerTbl.HeaderHeight = 60;
                //headerTbl.HorizontalAlignment = Element.ALIGN_LEFT;
                //float[] widths = new float[] { 90.70f, document.PageSize.Width - 90.70f };
                //headerTbl.SetWidths(widths);
                //PdfPCell cell = new PdfPCell(jpg);
                //cell.Border = 0;
                //cell.PaddingLeft = 10;
                //headerTbl.AddCell(cell);
                //PdfPCell cell1 = new PdfPCell(header);
                //cell1.Border = 0;
                //cell1.PaddingLeft = 50;
                ////cell1.Width = document.PageSize.Width - 90;
                //headerTbl.AddCell(cell1);
                ////ColumnText ct = new ColumnText(writer.DirectContent);
                ////ct.SetSimpleColumn(new Rectangle(10, 790, 559, 600));
                ////foreach (IElement e in Header)
                ////{
                ////    ct.AddElement(e);
                ////}
                ////ct.Go();

                //headerTbl.WriteSelectedRows(0, -1, 0, 832, writer.DirectContent);
            }
            }
        [HttpPost]
        public FileResult Download(PDFToolsViewModel PDFTools)
        {
           // Footer footobj = new Footer();
            
            //jpg.Alignment = Element.ALIGN_LEFT;
            string htmlBody = PDFTools.Content == null ? "" : PDFTools.Content.Replace("<br>", "<br/>").ToString().Replace("workAround:image\">", "workAround:image\"/>");
            StringReader reader = new StringReader(htmlBody.ToString());
            Document pdfDoc = new Document(PageSize.A4, -13f, -4f, 30f, 100f);
            byte[] bytes = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                Footer footobj = new Footer();
                footobj.imageURL = Server.MapPath("~/Content/images/logo2.png");
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

