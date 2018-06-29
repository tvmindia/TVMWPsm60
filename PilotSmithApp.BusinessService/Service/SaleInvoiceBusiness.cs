using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace PilotSmithApp.BusinessService.Service
{
    public class SaleInvoiceBusiness: ISaleInvoiceBusiness
    {
        ISaleInvoiceRepository _saleInvoiceRepository;
        ICommonBusiness _commonBusiness;
        IMailBusiness _mailBusiness;
        IPDFGeneratorBusiness _pDFGeneratorBusiness;
        public SaleInvoiceBusiness(ISaleInvoiceRepository saleInvoiceRepository, ICommonBusiness commonBusiness, IMailBusiness mailBusiness, IPDFGeneratorBusiness pDFGeneratorBusiness)
        {
            _saleInvoiceRepository = saleInvoiceRepository;
            _commonBusiness = commonBusiness;
            _mailBusiness = mailBusiness;
            _pDFGeneratorBusiness = pDFGeneratorBusiness;
        }
        public List<SaleInvoice> GetAllSaleInvoice(SaleInvoiceAdvanceSearch saleInvoiceAdvanceSearch)
        {
            return _saleInvoiceRepository.GetAllSaleInvoice(saleInvoiceAdvanceSearch);
        }
        public List<SaleInvoiceDetail> GetSaleInvoiceDetailListBySaleInvoiceID(Guid saleInvoiceID)
        {
            return _saleInvoiceRepository.GetSaleInvoiceDetailListBySaleInvoiceID(saleInvoiceID);
        }
        public object InsertUpdateSaleInvoice(SaleInvoice saleInvoice)
        {
            if (saleInvoice.SaleInvoiceDetailList.Count > 0)
            {
                saleInvoice.DetailXML = _commonBusiness.GetXMLfromSaleInvoiceObject(saleInvoice.SaleInvoiceDetailList, "ProductID,ProductModelID,UnitCode,Qty,Rate");
            }
            if (saleInvoice.SaleInvoiceOtherChargeDetailList.Count > 0)
            {
                saleInvoice.OtherChargeDetailXML = _commonBusiness.GetXMLfromSaleInvoiceOtherChargeObject(saleInvoice.SaleInvoiceOtherChargeDetailList, "OtherChargeCode,ChargeAmount");
            }
            return _saleInvoiceRepository.InsertUpdateSaleInvoice(saleInvoice);
        }
        public SaleInvoice GetSaleInvoice(Guid id)
        {
            return _saleInvoiceRepository.GetSaleInvoice(id);
        }
        public object DeleteSaleInvoice(Guid id)
        {
            return _saleInvoiceRepository.DeleteSaleInvoice(id);
        }
        public object DeleteSaleInvoiceDetail(Guid id)
        {
            return _saleInvoiceRepository.DeleteSaleInvoiceDetail(id);
        }
        public List<SaleInvoiceOtherCharge> GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(Guid saleInvoiceID)
        {
            return _saleInvoiceRepository.GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(saleInvoiceID);
        }

        public async Task<bool> QuoteEmailPush(SaleInvoice saleInvoice)
        {
            bool sendsuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(saleInvoice.EmailSentTo))
                {
                    //------------------------
                    string[] EmailList = saleInvoice.EmailSentTo.Split(',');
                    string mailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/DocumentEmailBody.html"));
                    MailMessage _mail = new MailMessage();
                    PDFTools pDFTools = new PDFTools();
                    string link = WebConfigurationManager.AppSettings["AppURL"] + "/Content/images/Pilot1.png";
                    _mail.Body = mailBody.Replace("$Customer$", saleInvoice.Customer.ContactPerson).Replace("$Document$", "Sale Invoice").Replace("$DocumentNo$", saleInvoice.SaleInvNo).Replace("$DocumentDate$", saleInvoice.SaleInvDateFormatted).Replace("$Logo$", link);
                    pDFTools.Content = saleInvoice.MailContant;
                    _mail.Attachments.Add(new Attachment(new MemoryStream(_pDFGeneratorBusiness.GetPdfAttachment(pDFTools)), saleInvoice.SaleInvNo + ".pdf"));
                    _mail.Subject = "Sale Invoice";
                    _mail.IsBodyHtml = true;
                    foreach (string email in EmailList)
                    {
                        _mail.To.Add(email);
                    }
                    sendsuccess = await _mailBusiness.MailMessageSendAsync(_mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sendsuccess;
        }

        public object UpdateSaleInvoiceEmailInfo(SaleInvoice saleInvoice)
        {
            return _saleInvoiceRepository.UpdateSaleInvoiceEmailInfo(saleInvoice);
        }

        public object DeleteSaleInvoiceOtherChargeDetail(Guid id)
        {
            return _saleInvoiceRepository.DeleteSaleInvoiceOtherChargeDetail(id);
        }
    }
}
