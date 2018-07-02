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
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Service
{
    public class ProformaInvoiceBusiness:IProformaInvoiceBusiness
    {
        IProformaInvoiceRepository _proforaInvoiceRepository;
        ICommonBusiness _commonBusiness;
        IMailBusiness _mailBusiness;
        IPDFGeneratorBusiness _pdfGeneratorBusiness;

        public ProformaInvoiceBusiness(IProformaInvoiceRepository proformaInvoiceRepository,ICommonBusiness commonBusiness,IMailBusiness mailBusiness,IPDFGeneratorBusiness pdfGenerator)
        {
            _proforaInvoiceRepository = proformaInvoiceRepository;
            _commonBusiness = commonBusiness;
            _mailBusiness = mailBusiness;
            _pdfGeneratorBusiness = pdfGenerator;
        }
        public List<ProformaInvoice> GetAllProformaInvoice(ProformaInvoiceAdvanceSearch proformaInvoiceAdvanceSearch)
        {
            return _proforaInvoiceRepository.GetAllProformaInvoice(proformaInvoiceAdvanceSearch);
        }
        public List<ProformaInvoiceDetail> GetProformaInvoiceDetailListByProformaInvoiceID(Guid proformaInvoiceID)
        {
            return _proforaInvoiceRepository.GetProformaInvoiceDetailListByProformaInvoiceID(proformaInvoiceID);
        }
        public object InsertUpdateProformaInvoice(ProformaInvoice proformaInvoice)
        {
            if (proformaInvoice.ProformaInvoiceDetailList.Count > 0)
            {
                proformaInvoice.DetailXML = _commonBusiness.GetXMLfromProformaInvoiceObject(proformaInvoice.ProformaInvoiceDetailList, "ProductID,ProductModelID,UnitCode,Qty,Rate");
            }
            if (proformaInvoice.ProformaInvoiceOtherChargeDetailList.Count > 0)
            {
                proformaInvoice.OtherChargeDetailXML = _commonBusiness.GetXMLfromProformaInvoiceOtherChargeObject(proformaInvoice.ProformaInvoiceOtherChargeDetailList, "OtherChargeCode,ChargeAmount");
            }
            return _proforaInvoiceRepository.InsertUpdateProformaInvoice(proformaInvoice);
        }
        public ProformaInvoice GetProformaInvoice(Guid id)
        {
            return _proforaInvoiceRepository.GetProformaInvoice(id);
        }
        public object DeleteProformaInvoice(Guid id)
        {
            return _proforaInvoiceRepository.DeleteProformaInvoice(id);
        }
        public object DeleteProformaInvoiceDetail(Guid id)
        {
            return _proforaInvoiceRepository.DeleteProformaInvoiceDetail(id);
        }
        public object DeleteProformaInvoiceOtherChargeDetail(Guid id)
        {
            return _proforaInvoiceRepository.DeleteProformaInvoiceOtherChargeDetail(id);
        }
        public List<ProformaInvoiceOtherCharge> GetProformaInvoiceOtherChargesDetailListByProformaInvoiceID(Guid proformaInvoiceID)
        {
            return _proforaInvoiceRepository.GetProformaInvoiceOtherChargesDetailListByProformaInvoiceID(proformaInvoiceID);
        }
        public async Task<bool> ProformaInvoiceEmailPush(ProformaInvoice proformaInvoice)
        {
            bool sendsuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(proformaInvoice.EmailSentTo))
                {
                    //------------------------
                    string[] EmailList = proformaInvoice.EmailSentTo.Split(',');
                    string mailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/DocumentEmailBody.html"));
                    MailMessage _mail = new MailMessage();
                    PDFTools pDFTools = new PDFTools();
                    string link = WebConfigurationManager.AppSettings["AppURL"] + "/Content/images/Pilot1.png";
                    _mail.Body = mailBody.Replace("$Customer$", proformaInvoice.Customer.ContactPerson).Replace("$Document$", "Proforma Invoice").Replace("$DocumentNo$", proformaInvoice.ProfInvNo).Replace("$DocumentDate$", proformaInvoice.ProfInvDateFormatted).Replace("$Logo$", link);
                    pDFTools.Content = proformaInvoice.MailContant;
                    _mail.Attachments.Add(new Attachment(new MemoryStream(_pdfGeneratorBusiness.GetPdfAttachment(pDFTools)), proformaInvoice.ProfInvNo + ".pdf"));
                    _mail.Subject = "Proforma Invoice";
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
        public object UpdateProformaInvoiceEmailInfo(ProformaInvoice proformaInvoice)
        {
            return _proforaInvoiceRepository.UpdateProformaInvoiceEmailInfo(proformaInvoice);
        }
        public List<ProformaInvoice> GetProformaInvoiceForSelectListOnDemand(string searchTerm)
        {
            return _proforaInvoiceRepository.GetProformaInvoiceForSelectListOnDemand(searchTerm);
        }
        public List<SelectListItem> GetProformaInvoiceForSelectList(Guid? proformaInvoiceID)
        {
            List<SelectListItem> selectListItem = null;
            List<ProformaInvoice> proformaInvoiceList = _proforaInvoiceRepository.GetProformaInvoiceForSelectList(proformaInvoiceID);
            return selectListItem = proformaInvoiceList != null ? (from proformaInvoice in proformaInvoiceList
                                                                   select new SelectListItem
                                                                   {
                                                                       Text = proformaInvoice.ProfInvNo,
                                                                       Value = proformaInvoice.ID.ToString(),
                                                                       Selected = false
                                                                   }).ToList() : new List<SelectListItem>();


        }
    }
}
