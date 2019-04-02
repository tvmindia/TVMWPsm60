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
    public class QuotationBusiness : IQuotationBusiness
    {
        IQuotationRepository _quotationRepository;
        ICommonBusiness _commonBusiness;
        ITaxTypeBusiness _taxTypeBusiness;
        IMailBusiness _mailBusiness;
        IPDFGeneratorBusiness _pDFGeneratorBusiness;
        public QuotationBusiness(IQuotationRepository quotationRepository, ICommonBusiness commonBusiness, ITaxTypeBusiness taxTypeBusiness, IMailBusiness mailBusiness, IPDFGeneratorBusiness pDFGeneratorBusiness)
        {
            _quotationRepository = quotationRepository;
            _commonBusiness = commonBusiness;
            _taxTypeBusiness = taxTypeBusiness;
            _mailBusiness = mailBusiness;
            _pDFGeneratorBusiness = pDFGeneratorBusiness;
        }
        public List<Quotation> GetAllQuotation(QuotationAdvanceSearch quotationAdvanceSearch)
        {
            return _quotationRepository.GetAllQuotation(quotationAdvanceSearch);
        }
        public List<QuotationDetail> GetQuotationDetailListByQuotationID(Guid quoteID, bool isCopy)
        {
            return _quotationRepository.GetQuotationDetailListByQuotationID(quoteID,isCopy);
        }
        public List<QuotationOtherCharge> GetQuotationOtherChargesDetailListByQuotationID(Guid quotationID, bool isCopy)
        {
            return _quotationRepository.GetQuotationOtherChargesDetailListByQuotationID(quotationID,isCopy);
        }
        public object InsertUpdateQuotation(Quotation quotation)
        {
            if (quotation.QuotationDetailList.Count > 0)
            {
                quotation.DetailXML = _commonBusiness.GetXMLfromQuotationObject(quotation.QuotationDetailList, "ProductID,ProductModelID,Qty,Rate,UnitCode");
            }
            if (quotation.QuotationOtherChargeList.Count > 0)
            {
                quotation.OtherChargeDetailXML = _commonBusiness.GetXMLfromQuotationOtherChargeObject(quotation.QuotationOtherChargeList, "OtherChargeCode,ChargeAmount");
            }
            return _quotationRepository.InsertUpdateQuotation(quotation);
        }
        public Quotation GetQuotation(Guid id)
        {
            return _quotationRepository.GetQuotation(id);
        }
        public object DeleteQuotation(Guid id)
        {
            return _quotationRepository.DeleteQuotation(id);
        }
        public object DeleteQuotationDetail(Guid id, string CreatedBy, DateTime CreatedDate)
        {
            return _quotationRepository.DeleteQuotationDetail(id,CreatedBy,CreatedDate);
        }
        public object UpdateQuotationEmailInfo(Quotation quotation)
        {
            return _quotationRepository.UpdateQuotationEmailInfo(quotation);
        }
        public async Task<bool> QuoteEmailPush(Quotation quotation)
        {

            bool sendsuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(quotation.EmailSentTo))
                {
                    string[] BccList=null;
                    string[] CcList=null;
                    string[] EmailList = quotation.EmailSentTo.Split(',');
                    if (quotation.Cc != null)
                        CcList = quotation.Cc.Split(',');
                    if(quotation.Bcc != null)
                        BccList = quotation.Bcc.Split(',');
                    string mailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/DocumentEmailBody.html"));
                    MailMessage _mail = new MailMessage();
                    PDFTools pDFTools = new PDFTools();
                    string link = WebConfigurationManager.AppSettings["AppURL"] + "/Content/images/Pilot1.png";
                    _mail.Body = mailBody.Replace("$Customer$", quotation.Customer.ContactPerson).Replace("$Document$", "Quotation").Replace("$DocumentNo$", quotation.QuoteNo).Replace("$DocumentDate$", quotation.QuoteDateFormatted).Replace("$Logo$", link);
                    pDFTools.Content = quotation.MailContant;
                    pDFTools.ContentFileName = "Quotation";
                    _mail.Attachments.Add(new Attachment(new MemoryStream(_pDFGeneratorBusiness.GetPdfAttachment(pDFTools)), quotation.QuoteNo + ".pdf"));

                    _mail.Subject = quotation.Subject;
                    _mail.IsBodyHtml = true;
                    foreach (string email in EmailList)
                    {
                        _mail.To.Add(email);
                    }
                    if (quotation.Cc != null)
                        foreach (string email in CcList)
                        {
                        _mail.CC.Add(email);
                        }
                    if (quotation.Bcc != null)
                        foreach (string email in BccList)
                        {
                        _mail.Bcc.Add(email);
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

        public List<SelectListItem> GetQuotationForSelectList(Guid? quoteID)
        {
            List<SelectListItem> selectListItem = null;
            List<Quotation> quotationList = _quotationRepository.GetQuotationForSelectList(quoteID);
            return selectListItem = quotationList != null ? (from quotation in quotationList
                                                             select new SelectListItem
                                                             {
                                                                 Text = quotation.QuoteNo,
                                                                 Value = quotation.ID.ToString(),
                                                                 Selected = false
                                                             }).ToList() : new List<SelectListItem>();
        }
        public List<Quotation> GetQuotationForSelectListOnDemand(string searchTerm)
        {
            return _quotationRepository.GetQuotationForSelectListOnDemand(searchTerm);
        }
        public object DeleteQuotationOtherChargeDetail(Guid id, string CreatedBy, DateTime CreatedDate)
        {
            return _quotationRepository.DeleteQuotationOtherChargeDetail(id,CreatedBy, CreatedDate);
        }
        public QuotationSummary GetQuotationSummaryCount()
        {
            return _quotationRepository.GetQuotationSummaryCount();
        }
    }
}
