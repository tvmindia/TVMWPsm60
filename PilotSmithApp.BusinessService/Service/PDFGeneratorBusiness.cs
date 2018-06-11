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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
